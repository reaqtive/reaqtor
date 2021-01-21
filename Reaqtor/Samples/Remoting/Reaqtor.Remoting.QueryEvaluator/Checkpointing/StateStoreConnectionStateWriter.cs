// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Reaqtor.QueryEngine;
using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.QueryEvaluator
{
    //
    // friendly type aliases help us easily remember what Tuple<blah...> means
    //
    using StagedChange = Object;
    using ToAdd = Tuple<string, string, byte[]>;
    using ToRemove = Tuple<string, string>;

    /// <summary>
    /// Thread-safe state writer writing item in the remoting state store role.
    /// <remarks>The rule of not being able to modify the state after committing or rolling back is not
    /// completely enforced and could be violated if Commit() or Rollback() are called simultaneously
    /// as AddOrUpdate() or Delete(). It is therefore the responsability of the user to ensure that
    /// all operations are done (including disposing streams) before calling Commit() or Rollback().</remarks>
    /// </summary>
    internal class StateStoreConnectionStateWriter : IStateWriter
    {
        #region Private Members

        private readonly Action _onCommit;
        private readonly Action _onRollback;
        private readonly ConcurrentBag<StagedChange> _stagedChanges;
        private readonly IReactiveStateStoreConnection _connection;
        private long _openedStreamCount;
        private volatile bool _disposed;

        /// <summary>
        /// Set to 1 once Commit or Rollback has been called.
        /// </summary>
        private int _sealed;

        #endregion

        public StateStoreConnectionStateWriter(
            IReactiveStateStoreConnection connection,
            CheckpointKind checkpointKind,
            Action onCommit = null,
            Action onRollback = null)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _onCommit = onCommit;
            _onRollback = onRollback;
            _stagedChanges = new ConcurrentBag<StagedChange>();
            CheckpointKind = checkpointKind;
        }

        #region IStateWriter implementation

        /// <summary>
        /// The checkpoint kind.
        /// For a full checkpoint, the complete state must be stored,
        /// and for a differential update, only the modification since the last
        /// checkpoint should be stored.
        /// </summary>
        public CheckpointKind CheckpointKind
        {
            get;
            private set;
        }

        /// <summary>
        /// Commit all staged changes. Note that we expect the state store we're
        /// writing to to be threadsafe!
        /// <remarks>
        /// Commit() call will fail if there are undisposed streams as returned
        /// by GetItemWriter(). The reason for that is that the storage
        /// infrastructure must make sure that all data has been transferred
        /// before committing the content.
        /// </remarks>
        /// </summary>
        /// <returns></returns>
        public Task CommitAsync(CancellationToken token, IProgress<int> progress)
        {
            CheckDisposed();

            if (Interlocked.CompareExchange(ref _openedStreamCount, 0, 0) > 0)
            {
                throw new InvalidOperationException("There are some streams opened.");
            }
            int wasSealed = Interlocked.Exchange(ref _sealed, 1);
            if (wasSealed > 0)
            {
                throw new InvalidOperationException("Transaction has already been committed or rollback.");
            }

            // commit all changes
            foreach (StagedChange change in _stagedChanges)
            {
                _connection.ApplyStagedChange(change);
            }

            _onCommit?.Invoke();

            return Task.FromResult(true);
        }

        /// <summary>
        /// Discard all staged changes.
        /// </summary>
        public void Rollback()
        {
            CheckDisposed();

            int wasSealed = Interlocked.Exchange(ref _sealed, 1);
            if (wasSealed > 0)
            {
                throw new InvalidOperationException("Transaction has already been committed or rollback.");
            }

            while (_stagedChanges.TryTake(out _))
                ;

            _onRollback?.Invoke();
        }

        /// <summary>
        /// Returns a stream which enable storing either a new item or an update for
        /// an existing item in the store in the provided <paramref name="category"/>
        /// with the provided <paramref name="key"/>.
        /// </summary>
        /// <param name="category">The category of the item.</param>
        /// <param name="key">The key of the item.</param>
        /// <returns>A stream which must be used to store the item content.</returns>
        public Stream GetItemWriter(string category, string key)
        {
            CheckDisposed();

            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (Interlocked.CompareExchange(ref _sealed, 0, 0) > 0)
            {
                throw new InvalidOperationException("Transaction has already been committed or rollback.");
            }
            MemoryStream memoryStream = new MemoryStream();

            Interlocked.Increment(ref _openedStreamCount);

            void OnDispose()
            {
                var bytes = CompressionUtils.Compress(memoryStream.ToArray(), Compression.None);
                _stagedChanges.Add(new ToAdd(category, key, bytes));
                Interlocked.Decrement(ref _openedStreamCount);
                Debug.Assert(Interlocked.CompareExchange(ref _sealed, 0, 0) == 0,
                    "The state was committed or rollback during the write operation.");
            }

            return new StreamWrapper(memoryStream, OnDispose);
        }

        /// <summary>
        /// Stages an item deletion. Deletion takes place upon call to CommitAsync.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="key">The item key.</param>
        public void DeleteItem(string category, string key)
        {
            CheckDisposed();

            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (Interlocked.CompareExchange(ref _sealed, 0, 0) > 0)
            {
                throw new InvalidOperationException("Transaction has already been committed or rollback.");
            }
            _stagedChanges.Add(new ToRemove(category, key));
            Debug.Assert(Interlocked.CompareExchange(ref _sealed, 0, 0) == 0,
                        "The state was committed or rollback during the delete operation.");
        }

        #endregion

        protected void CheckDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("InMemoryStateReader instance already disposed.");
            }
        }

        /// <summary>
        /// Dispose managed resource.
        /// <remarks>The internal store is not cleared upon disposing.</remarks>
        /// </summary>
        public void Dispose()
        {
            _disposed = true;
        }

        #region Utility classes

        /// <summary>
        /// Lightweight wrapper for stream adding abilities to specify action to take
        /// when disposing as well as the option to leave the stream open when disposing.
        /// </summary>
        private sealed class StreamWrapper : Stream
        {
            private readonly Stream _stream;
            private readonly Action _onDispose;
            private readonly bool _leaveStreamOpen;

            public StreamWrapper(Stream stream, Action onDispose, bool leaveStreamOpen = false)
            {
                _stream = stream ?? throw new ArgumentNullException(nameof(stream));
                _onDispose = onDispose;
                _leaveStreamOpen = leaveStreamOpen;
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);

                _onDispose?.Invoke();

                if (!_leaveStreamOpen)
                {
                    _stream.Dispose();
                }
            }

            public override bool CanRead => _stream.CanRead;

            public override bool CanSeek => _stream.CanSeek;

            public override bool CanWrite => _stream.CanWrite;

            public override void Flush()
            {
                _stream.Flush();
            }

            public override long Length => _stream.Length;

            public override long Position
            {
                get => _stream.Position;
                set => _stream.Position = value;
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                return _stream.Read(buffer, offset, count);
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                return _stream.Seek(offset, origin);
            }

            public override void SetLength(long value)
            {
                _stream.SetLength(value);
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                _stream.Write(buffer, offset, count);
            }
        }

        #endregion
    }

    #region Static extension methods

    internal static class StateStoreHelpers
    {
        public static void ApplyStagedChange(this IReactiveStateStoreConnection connection, StagedChange change)
        {
            if (change is ToAdd itemToAdd)
            {
                connection.AddOrUpdateItem(itemToAdd.Item1, itemToAdd.Item2, itemToAdd.Item3);
                return;
            }

            if (change is ToRemove itemToRemove)
            {
                connection.RemoveItem(itemToRemove.Item1, itemToRemove.Item2);
                return;
            }

            throw new ArgumentException("Type of change to apply to is not recognized!");
        }
    }

    #endregion
}
