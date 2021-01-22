// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor.QueryEngine.KeyValueStore.InMemory
{
    /// <summary>
    /// Thread-safe state writer writing item in a in-memory store.
    /// </summary>
    /// <remarks>
    /// The rule of not being able to modify the state after committing or rolling back is not 
    /// completely enforced and could be violated if Commit() or Rollback() are called simultaneously
    /// as AddOrUpdate() or Delete(). It is therefore the responsability of the user to ensure that
    /// all operations are done (including disposing streams) before calling Commit() or Rollback().
    /// </remarks>
    public class InMemoryStateWriter : IStateWriter
    {
        private readonly Action _onCommit;
        private readonly Action _onRollback;
        private readonly InMemoryStateStore _stateStore;

        private long _openedStreamCount;
        private volatile bool _disposed;

        /// <summary>
        /// Set to 1 once Commit or Rollback has been called.
        /// </summary>
        private int _sealed;

        /// <summary>
        /// Create a new instance of <see cref="InMemoryStateWriter"/>.
        /// </summary>
        /// <param name="stateStore">The store where the state is persisted.</param>
        /// <param name="checkpointKind">The checkpoint kind.</param>
        /// <param name="onCommit">The action to be called when committing.</param>
        /// <param name="onRollback">The action to be called when rolling back.</param>
        public InMemoryStateWriter(
            InMemoryStateStore stateStore,
            CheckpointKind checkpointKind,
            Action onCommit = null,
            Action onRollback = null)
        {
            _onRollback = onRollback;
            _onCommit = onCommit;
            _stateStore = stateStore ?? throw new ArgumentNullException(nameof(stateStore));
            CheckpointKind = checkpointKind;
        }

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
        /// Commit the changes.
        /// </summary>
        /// <returns>A task to await the completion of the commit operation.</returns>
        /// <remarks>
        /// Commit() call will fail if there are undisposed streams as returned by GetItemWriter().
        /// The reason for that is that the storage infrastructure must make sure that all data
        /// has been transferred before committing the content.
        /// </remarks>
        public virtual Task CommitAsync(CancellationToken token, IProgress<int> progress)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("InMemoryStateWriter instance already disposed.");
            }

            if (Interlocked.CompareExchange(ref _openedStreamCount, 0, 0) > 0)
            {
                throw new InvalidOperationException("There are some streams opened.");
            }

            int wasSealed = Interlocked.Exchange(ref _sealed, 1);
            if (wasSealed > 0)
            {
                throw new InvalidOperationException("Transaction has already been committed or rollback.");
            }

            _onCommit?.Invoke();

            return Task.FromResult(true);
        }

        /// <summary>
        /// Discard all the changes.
        /// </summary>
        public void Rollback()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("InMemoryStateWriter instance already disposed.");
            }

            int wasSealed = Interlocked.Exchange(ref _sealed, 1);

            if (wasSealed > 0)
            {
                throw new InvalidOperationException("Transaction has already been committed or rollback.");
            }

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
            if (_disposed)
            {
                throw new ObjectDisposedException("InMemoryStateWriter instance already disposed.");
            }

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

            var memoryStream = new MemoryStream();

            Interlocked.Increment(ref _openedStreamCount);

            return new StreamWrapper(memoryStream, onDispose: () =>
                {
                    byte[] bytes = memoryStream.ToArray();
                    _stateStore.AddOrUpdateItem(category, key, bytes);
                    Interlocked.Decrement(ref _openedStreamCount);
                    Debug.Assert(Interlocked.CompareExchange(ref _sealed, 0, 0) == 0,
                        "The state was committed or rollback during the write operation.");
                });
        }

        /// <summary>
        /// Deletes an item from the stored state.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="key">The item key.</param>
        public void DeleteItem(string category, string key)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("InMemoryStateWriter instance already disposed.");
            }

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

            _stateStore.RemoveItem(category, key);

            Debug.Assert(Interlocked.CompareExchange(ref _sealed, 0, 0) == 0,
                        "The state was committed or rollback during the delete operation.");
        }


        /// <summary>
        /// Dispose managed resources.
        /// </summary>
        /// <remarks>The internal store is not cleared upon disposing.</remarks>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose managed resources.
        /// </summary>
        /// <param name="disposing">true if called from <see cref="IDisposable.Dispose"/>; otherwise, false.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _disposed = true;
            }
        }

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

            public override void Flush() => _stream.Flush();

            public override long Length => _stream.Length;

            public override long Position
            {
                get => _stream.Position;
                set => _stream.Position = value;
            }

            public override int Read(byte[] buffer, int offset, int count) => _stream.Read(buffer, offset, count);

            public override long Seek(long offset, SeekOrigin origin) => _stream.Seek(offset, origin);

            public override void SetLength(long value) => _stream.SetLength(value);

            public override void Write(byte[] buffer, int offset, int count) => _stream.Write(buffer, offset, count);
        }
    }
}
