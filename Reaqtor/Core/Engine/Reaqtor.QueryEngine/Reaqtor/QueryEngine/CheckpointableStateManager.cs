// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Manager for checkpointable object state transition tracking.
    /// </summary>
    /// <remarks>
    /// The owner of the instance is responsible to call <see cref="Dispose"/> after all active operations on
    /// the instance have completed. Calling <see cref="Dispose"/> while any operation is in flight will cause
    /// undefined behavior.
    /// </remarks>
    internal sealed class CheckpointableStateManager : ICheckpointable, IDisposable
    {
        private readonly ICheckpointable _engine;
        private readonly Uri _uri;
        private readonly TraceSource _traceSource;

        private readonly object _statusLock = new();
        private volatile QueryEngineStatus _status;
        private volatile TaskCompletionSource<bool> _pendingUnload;
        private volatile IProgress<int> _cancelProgress;
        private volatile IProgress<int> _unloadProgress;
        private volatile CancellationTokenSource _activeOperation;
        private volatile IUnloadable _activeResource;

        /// <summary>
        /// Creates a new checkpointable object state transition manager encapsulating the given engine.
        /// </summary>
        /// <param name="engine">Engine whose state transitions should be managed.</param>
        /// <param name="uri">URI identifying the engine being managed.</param>
        /// <param name="traceSource">(Optional) Trace source to log diagnostic information about state transitions to.</param>
        public CheckpointableStateManager(ICheckpointable engine, Uri uri, TraceSource traceSource)
        {
            Debug.Assert(engine != null);
            Debug.Assert(uri != null);

            _engine = engine;
            _uri = uri;
            _traceSource = traceSource;
            _status = QueryEngineStatus.Running;
        }

        /// <summary>
        /// Gets the current status of the checkpointable object.
        /// </summary>
        public QueryEngineStatus Status
        {
            get
            {
                lock (_statusLock) // prevent dirty reads
                {
                    return _status;
                }
            }
        }

        /// <summary>
        /// Disposes resources.
        /// </summary>
        public void Dispose()
        {
            // NB: This only has an effect if timeouts or blocking waits were used through the exposed
            //     cancellation tokens.

            _activeOperation?.Dispose();
        }

        /// <summary>
        /// Saves the state of the system to a store.
        /// </summary>
        /// <param name="writer">Writer to save state to.</param>
        /// <param name="token">Cancellation token to observe cancellation requests.</param>
        /// <param name="progress">Progress indicator reporting progress percentages.</param>
        /// <returns>Task to observe the eventual completion of the operation.</returns>
        /// <exception cref="EngineUnloadedException">
        /// The engine was unloaded while the operation was in progress, causing the operation to be aborted.
        /// This exception is reported through the returned task.
        /// </exception>
        public async Task CheckpointAsync(IStateWriter writer, CancellationToken token, IProgress<int> progress)
        {
            Debug.Assert(writer != null);

            token.ThrowIfCancellationRequested();

            using var unloadableWriter = new StateWriter(writer);

            CheckpointAsyncEnter(unloadableWriter);

            using (token.Register(_activeOperation.Cancel))
            {
                try
                {
                    await _engine.CheckpointAsync(unloadableWriter, _activeOperation.Token, progress).ConfigureAwait(false);
                }
                catch (OperationCanceledException ex) when (!token.IsCancellationRequested && _pendingUnload != null)
                {
                    // User didn't cancel and unload pending indicates we're responsible for triggering cancellation.
                    throw new EngineUnloadedException(ex);
                }
                finally
                {
                    if (!CheckpointAsyncLeave())
                    {
                        _ = UnloadAsyncCore(grantRequest: true, _unloadProgress); // fire-and-forget; outstanding UnloadAsync call can get result
                    }
                }
            }
        }

        /// <summary>
        /// Tries to enter the Checkpointing state.
        /// </summary>
        /// <param name="resource">Unloadable resource used during checkpointing. This resource may get unloaded while the operation is in progress, when an unload is requested.</param>
        /// <exception cref="InvalidOperationException">The transition is not supported from the current state.</exception>
        private void CheckpointAsyncEnter(IUnloadable resource)
        {
            lock (_statusLock)
            {
                Transition(nameof(CheckpointAsync), isLeaveTransition: false, operation: () =>
                {
                    switch (_status)
                    {
                        case QueryEngineStatus.Running:
                            _status = QueryEngineStatus.Checkpointing;
                            _activeOperation = new CancellationTokenSource();
                            _activeResource = resource;
                            break;
                        case QueryEngineStatus.Recovering:
                        case QueryEngineStatus.Checkpointing:
                        case QueryEngineStatus.Recovering | QueryEngineStatus.UnloadRequested:
                        case QueryEngineStatus.Checkpointing | QueryEngineStatus.UnloadRequested:
                        case QueryEngineStatus.Unloading:
                        case QueryEngineStatus.Unloaded:
                        case QueryEngineStatus.UnloadFailed:
                        case QueryEngineStatus.Faulted:
                            throw InvalidTransition(QueryEngineStatus.Checkpointing);
                        default:
                            throw UnknownState();
                    }
                });
            }
        }

        /// <summary>
        /// Tries to leave the Checkpointing state by transitioning back to the Running state.
        /// If an unload is requested, no transition will take place and the caller should proceed to the unload stage.
        /// </summary>
        /// <returns>true if the transition succeeded; otherwise, false indicating an unload was requested.</returns>
        private bool CheckpointAsyncLeave()
        {
            lock (_statusLock)
            {
                return Transition<bool>(nameof(CheckpointAsync), isLeaveTransition: true, operation: () =>
                {
                    switch (_status)
                    {
                        case QueryEngineStatus.Checkpointing:
                            _status = QueryEngineStatus.Running;
                            _activeOperation = null;
                            _activeResource = null;
                            break;
                        case QueryEngineStatus.Checkpointing | QueryEngineStatus.UnloadRequested:
                            return false;
                        default:
                            throw UnexpectedState();
                    }

                    return true;
                });
            }
        }

        /// <summary>
        /// Recovers the state of the system from a store.
        /// </summary>
        /// <param name="reader">Reader to load state from.</param>
        /// <param name="token">Cancellation token to observe cancellation requests.</param>
        /// <param name="progress">Progress indicator reporting progress percentages.</param>
        /// <returns>Task to observe the eventual completion of the operation.</returns>
        /// <exception cref="EngineUnloadedException">
        /// The engine was unloaded while the operation was in progress, causing the operation to be aborted.
        /// This exception is reported through the returned task.
        /// </exception>
        public async Task RecoverAsync(IStateReader reader, CancellationToken token, IProgress<int> progress)
        {
            Debug.Assert(reader != null);

            token.ThrowIfCancellationRequested();

            using var unloadableReader = new StateReader(reader);

            RecoverAsyncEnter(unloadableReader);

            using (token.Register(_activeOperation.Cancel))
            {
                try
                {
                    await _engine.RecoverAsync(unloadableReader, _activeOperation.Token, progress).ConfigureAwait(false);
                }
                catch (OperationCanceledException ex) when (!token.IsCancellationRequested && _pendingUnload != null)
                {
                    // User didn't cancel and unload pending indicates we're responsible for triggering cancellation.
                    throw new EngineUnloadedException(ex);
                }
                finally
                {
                    if (!RecoverAsyncLeave())
                    {
                        _ = UnloadAsyncCore(grantRequest: true, _unloadProgress); // fire-and-forget; outstanding UnloadAsync call can get result
                    }
                }
            }
        }

        /// <summary>
        /// Tries to enter the Recovering state.
        /// </summary>
        /// <param name="resource">Unloadable resource used during recovery. This resource may get unloaded while the operation is in progress, when an unload is requested.</param>
        /// <exception cref="InvalidOperationException">The transition is not supported from the current state.</exception>
        private void RecoverAsyncEnter(IUnloadable resource)
        {
            lock (_statusLock)
            {
                Transition(nameof(RecoverAsync), isLeaveTransition: false, operation: () =>
                {
                    switch (_status)
                    {
                        case QueryEngineStatus.Running:
                            _status = QueryEngineStatus.Recovering;
                            _activeOperation = new CancellationTokenSource();
                            _activeResource = resource;
                            break;
                        case QueryEngineStatus.Recovering:
                        case QueryEngineStatus.Checkpointing:
                        case QueryEngineStatus.Recovering | QueryEngineStatus.UnloadRequested:
                        case QueryEngineStatus.Checkpointing | QueryEngineStatus.UnloadRequested:
                        case QueryEngineStatus.Unloading:
                        case QueryEngineStatus.Unloaded:
                        case QueryEngineStatus.UnloadFailed:
                        case QueryEngineStatus.Faulted:
                            throw InvalidTransition(QueryEngineStatus.Recovering);
                        default:
                            throw UnknownState();
                    }
                });
            }
        }

        /// <summary>
        /// Tries to leave the Recovering state by transitioning back to the Running state.
        /// If an unload is requested, no transition will take place and the caller should proceed to the unload stage.
        /// </summary>
        /// <returns>true if the transition succeeded; otherwise, false indicating an unload was requested.</returns>
        private bool RecoverAsyncLeave()
        {
            lock (_statusLock)
            {
                return Transition<bool>(nameof(RecoverAsync), isLeaveTransition: true, operation: () =>
                {
                    switch (_status)
                    {
                        case QueryEngineStatus.Recovering:
                            _status = QueryEngineStatus.Running;
                            _activeOperation = null;
                            _activeResource = null;
                            break;
                        case QueryEngineStatus.Recovering | QueryEngineStatus.UnloadRequested:
                            return false;
                        default:
                            throw UnexpectedState();
                    }

                    return true;
                });
            }
        }

        /// <summary>
        /// Unloads the system.
        /// </summary>
        /// <param name="progress">Progress indicator reporting progress percentages.</param>
        /// <returns>Task to observe the eventual completion of the operation.</returns>
        public Task UnloadAsync(IProgress<int> progress)
        {
            return UnloadAsyncCore(grantRequest: false, progress);
        }

        /// <summary>
        /// Unloads the system.
        /// </summary>
        /// <param name="grantRequest">Indicates whether the request to unload can be granted from UnloadRequested states.</param>
        /// <param name="progress">Progress indicator reporting progress percentages.</param>
        /// <returns>Task to observe the eventual completion of the operation.</returns>
        private Task UnloadAsyncCore(bool grantRequest, IProgress<int> progress)
        {
            if (UnloadAsyncEnter(grantRequest, progress))
            {
#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1031 // Do not catch general exception types. (Exception reported asynchronously.)
                try
                {
                    _engine.UnloadAsync(_unloadProgress).ContinueWith(t =>
                    {
                        try
                        {
                            //
                            // NB: Leave first, before signaling the TaskCompletionSource, which can cause
                            //     user code to resume on a different thread, before the state gets set by
                            //     the UnloadAsyncLeave code path.
                            //
                            //     We want to make sure the resuming thread will see the new state, and in
                            //     case of a failure, can immediately retry by starting a new UnloadAsync
                            //     operation (which would not work if the state is still Unloading).
                            //

                            UnloadAsyncLeave(t.Status != TaskStatus.RanToCompletion);
                        }
                        finally
                        {
                            switch (t.Status)
                            {
                                case TaskStatus.Canceled:
                                    _pendingUnload.SetCanceled();
                                    break;
                                case TaskStatus.Faulted:
                                    _pendingUnload.SetException(t.Exception.GetBaseException());
                                    break;
                                case TaskStatus.RanToCompletion:
                                    _pendingUnload.SetResult(true);
                                    break;
                            }
                        }
                    }, TaskScheduler.Default);
                }
                catch (Exception ex)
                {
                    UnloadAsyncLeave(failed: true);

                    // Handles the rare case where the synchronous portion of UnloadAsync throws,
                    // if not implemented as an async method.
                    _pendingUnload.SetException(ex);
                }
#pragma warning restore CA1031
#pragma warning restore IDE0079
            }

            return _pendingUnload.Task;
        }

        /// <summary>
        /// Tries to enter the Unloading state.
        /// If the system has already been unloaded successfully, this operation has no observable side-effects.
        /// If the system is currently checkpointing or recovering, the UnloadRequested flag will be set and outstanding operations will be aborted at the earliest convenience.
        /// </summary>
        /// <param name="grantRequest">Indicates whether a request to unload can be granted from UnloadRequested states.</param>
        /// <param name="progress">Unload progress tracker.</param>
        /// <remarks>true if the unload phase was entered; otherwise, false.</remarks>
        private bool UnloadAsyncEnter(bool grantRequest, IProgress<int> progress)
        {
            var activeOperation = default(CancellationTokenSource);
            var activeResource = default(IUnloadable);

            try
            {
                return Transition<bool>(nameof(UnloadAsync), isLeaveTransition: false, operation: () =>
                {
                    lock (_statusLock)
                    {
                        switch (_status)
                        {
                            case QueryEngineStatus.Running:
                            case QueryEngineStatus.Faulted:
                            case QueryEngineStatus.UnloadFailed:
                                _status = QueryEngineStatus.Unloading;
                                _pendingUnload = new TaskCompletionSource<bool>();
                                _unloadProgress = progress;
                                return true;
                            case QueryEngineStatus.Checkpointing | QueryEngineStatus.UnloadRequested:
                            case QueryEngineStatus.Recovering | QueryEngineStatus.UnloadRequested:
                                if (grantRequest)
                                {
                                    _status = QueryEngineStatus.Unloading;
                                    _cancelProgress.CompleteIfNotNull();
                                }
                                return grantRequest;
                            case QueryEngineStatus.Recovering:
                            case QueryEngineStatus.Checkpointing:
                                _status |= QueryEngineStatus.UnloadRequested;
                                _pendingUnload = new TaskCompletionSource<bool>();
                                SplitUnloadProgress(progress, out var cancelProgress, out var unloadProgress);
                                _cancelProgress = cancelProgress;
                                _unloadProgress = unloadProgress;
                                activeOperation = _activeOperation;
                                activeResource = _activeResource;
                                return false;
                            case QueryEngineStatus.Unloading:
                            case QueryEngineStatus.Unloaded:
                                return false;
                            default:
                                throw UnknownState();
                        }
                    }
                });
            }
            finally
            {
                activeResource?.Unload();
                activeOperation?.Cancel();
            }
        }

        /// <summary>
        /// Tries to leave the Unloading state by transitioning to the Unloaded or UnloadFailed state.
        /// </summary>
        /// <param name="failed">Indicates whether the unload operation failed and can be retried by subsequent unload requests.</param>
        private void UnloadAsyncLeave(bool failed)
        {
            lock (_statusLock)
            {
                Transition(nameof(UnloadAsync), isLeaveTransition: true, operation: () =>
                {
                    _status = _status switch
                    {
                        QueryEngineStatus.Unloading => failed ? QueryEngineStatus.UnloadFailed : QueryEngineStatus.Unloaded,
                        _ => throw UnexpectedState(),
                    };
                });
            }
        }

        /// <summary>
        /// Splits progress of a deferred unload operation in a cancellation phase and an unload phase.
        /// </summary>
        /// <param name="progress">Progress to split.</param>
        /// <param name="cancel">Cancellation portion of the progress.</param>
        /// <param name="unload">Unload portion of the progress.</param>
        private static void SplitUnloadProgress(IProgress<int> progress, out IProgress<int> cancel, out IProgress<int> unload)
        {
            if (progress != null)
            {
                var q = progress.SplitWeight(10, 90);
                cancel = q[0];
                unload = q[1];
                return;
            }

            cancel = null;
            unload = null;
        }

        /// <summary>
        /// Performs a state transition operation that gets logged.
        /// </summary>
        /// <param name="caller">Caller name, used for diagnostic purposes only.</param>
        /// <param name="isLeaveTransition">Indicates whether the transition is leaving a transient state.</param>
        /// <param name="operation">State transition operation to execute.</param>
        private void Transition(string caller, bool isLeaveTransition, Action operation)
        {
            var oldStatus = _status;

            operation();

            var newStatus = _status;

            Log(caller, isLeaveTransition, oldStatus, newStatus);
        }

        /// <summary>
        /// Performs a state transition operation that gets logged.
        /// </summary>
        /// <typeparam name="R">Type of the result.</typeparam>
        /// <param name="caller">Caller name, used for diagnostic purposes only.</param>
        /// <param name="isLeaveTransition">Indicates whether the transition is leaving a transient state.</param>
        /// <param name="operation">State transition operation to execute.</param>
        /// <returns>Result of the transition operation.</returns>
        private R Transition<R>(string caller, bool isLeaveTransition, Func<R> operation)
        {
            var oldStatus = _status;

            var result = operation();

            var newStatus = _status;

            Log(caller, isLeaveTransition, oldStatus, newStatus);

            return result;
        }

        /// <summary>
        /// Logs a state transition.
        /// </summary>
        /// <param name="caller">Caller name, used for diagnostic purposes only.</param>
        /// <param name="isLeaveTransition">Indicates whether the transition is leaving a transient state.</param>
        /// <param name="oldStatus">Old original status.</param>
        /// <param name="newStatus">New target status.</param>
        private void Log(string caller, bool isLeaveTransition, QueryEngineStatus oldStatus, QueryEngineStatus newStatus)
        {
            var trace = _traceSource;
            if (isLeaveTransition)
            {
                trace.CheckpointableStateManager_TransitionCompleted(_uri, caller, oldStatus, newStatus);
            }
            else
            {
                trace.CheckpointableStateManager_TransitionStarted(_uri, caller, oldStatus, newStatus);
            }
        }

        /// <summary>
        /// Returns an exception object reporting an invalid state transition.
        /// </summary>
        /// <param name="targetStatus">Target status that couldn't be transitioned into.</param>
        /// <returns>Exception object reporting an invalid state transition.</returns>
        private Exception InvalidTransition(QueryEngineStatus targetStatus)
        {
            return new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Query engine '{0}' is currently in the '{1}' state and cannot transition to the '{2}' state.", _uri, _status, targetStatus));
        }

        /// <summary>
        /// Returns an exception object reporting an unknown state was encountered. This indicates a bug in the state manager.
        /// </summary>
        /// <returns>Exception object reporting an unknown state was encountered.</returns>
        private Exception UnknownState()
        {
            return new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "State '{0}' is unknown.", _status));
        }

        /// <summary>
        /// Returns an exception object reporting an unexpected state was encountered. This indicates a bug in the state manager.
        /// </summary>
        /// <returns>Exception object reporting an unexpected state was encountered.</returns>
        private Exception UnexpectedState()
        {
            return new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "State '{0}' was unexpected at this time.", _status));
        }

        /// <summary>
        /// State reader wrapper supporting unloading.
        /// </summary>
        private sealed class StateReader : IStateReader, IUnloadable
        {
            /// <summary>
            /// Wrapped state reader.
            /// </summary>
            private readonly IStateReader _reader;

            /// <summary>
            /// Flag indicating a dispose operation was requested.
            /// </summary>
            private int _disposed;

            /// <summary>
            /// Flag indicating an unload operation has taken place.
            /// </summary>
            private int _unloaded;

            /// <summary>
            /// Creates a new state reader wrapper.
            /// </summary>
            /// <param name="reader">State reader to wrap.</param>
            public StateReader(IStateReader reader)
            {
                _reader = reader;
            }

            /// <summary>
            /// Get the list of categories which are part of the state of the engine that was saved in the store.
            /// </summary>
            /// <returns>The list of categories.</returns>
            /// <exception cref="EngineUnloadedException">The request is rejected because the engine has been unloaded.</exception>
            /// <exception cref="ObjectDisposedException">The reader has been disposed.</exception>
            public IEnumerable<string> GetCategories()
            {
                CheckAccess();

                return _reader.GetCategories();
            }

            /// <summary>
            /// Get the list of item keys belonging to the provided category.
            /// </summary>
            /// <param name="category">Category to retrieve keys for.</param>
            /// <param name="keys">Retrieved keys for the specified category.</param>
            /// <returns><b>true</b> if the category exists; otherwise, <b>false</b>.</returns>
            /// <exception cref="EngineUnloadedException">The request is rejected because the engine has been unloaded.</exception>
            /// <exception cref="ObjectDisposedException">The reader has been disposed.</exception>
            public bool TryGetItemKeys(string category, out IEnumerable<string> keys)
            {
                CheckAccess();

                return _reader.TryGetItemKeys(category, out keys);
            }

            /// <summary>
            /// Get a stream to read the state of the item specified by the provided category and key.
            /// </summary>
            /// <param name="category">Category of the item to read state for.</param>
            /// <param name="key">Key of the item to read state for.</param>
            /// <param name="stream">Stream to read that state of the item from.</param>
            /// <returns><b>true</b> if the category and item key exists; otherwise, <b>false</b>.</returns>
            /// <exception cref="EngineUnloadedException">The request is rejected because the engine has been unloaded.</exception>
            /// <exception cref="ObjectDisposedException">The reader has been disposed.</exception>
            public bool TryGetItemReader(string category, string key, out Stream stream)
            {
                CheckAccess();

                return _reader.TryGetItemReader(category, key, out stream);
            }

            /// <summary>
            /// Disposes the state reader.
            /// This operation is idempotent and can take place after an unload operation.
            /// </summary>
            public void Dispose()
            {
                if (Interlocked.Exchange(ref _disposed, 1) == 0)
                {
                    _reader.Dispose();
                }
            }

            /// <summary>
            /// Unloads the state reader.
            /// This operation causes future interactions with the reader to be rejected with an <see cref="EngineUnloadedException"/>.
            /// </summary>
            public void Unload()
            {
                Volatile.Write(ref _unloaded, 1);
            }

            /// <summary>
            /// Checks whether access to the reader is allowed. Access is rejected when the reader is either disposed or unloaded.
            /// </summary>
            /// <exception cref="EngineUnloadedException">The request is rejected because the engine has been unloaded.</exception>
            /// <exception cref="ObjectDisposedException">The reader has been disposed.</exception>
            private void CheckAccess()
            {
                if (Volatile.Read(ref _disposed) == 1)
                    throw new ObjectDisposedException("reader");
                if (Volatile.Read(ref _unloaded) == 1)
                    throw new EngineUnloadedException();
            }
        }

        /// <summary>
        /// State writer wrapper supporting unloading.
        /// </summary>
        private sealed class StateWriter : IStateWriter, IUnloadable
        {
            /// <summary>
            /// Wrapped state writer.
            /// </summary>
            private readonly IStateWriter _writer;

            /// <summary>
            /// Flag indicating a dispose operation was requested.
            /// </summary>
            private int _disposed;

            /// <summary>
            /// Flag indicating an unload operation has taken place.
            /// </summary>
            private int _unloaded;

            /// <summary>
            /// Creates a new state writer wrapper.
            /// </summary>
            /// <param name="writer">State writer to wrap.</param>
            public StateWriter(IStateWriter writer)
            {
                _writer = writer;
            }

            /// <summary>
            /// The checkpoint kind.
            /// - For a full checkpoint, all items will be stored.
            /// - For a differential update, only items that have been modified since the last checkpoint will be stored.
            /// </summary>
            public CheckpointKind CheckpointKind => _writer.CheckpointKind;

            /// <summary>
            /// Commits the transaction.
            /// </summary>
            /// <param name="token">Cancellation token to observe cancellation requests.</param>
            /// <param name="progress">(Optional) Progress indicator reporting progress percentages.</param>
            /// <returns>Task to observe the eventual completion of the commit operation.</returns>
            public Task CommitAsync(CancellationToken token, IProgress<int> progress)
            {
                CheckAccess();

                return _writer.CommitAsync(token, progress);
            }

            /// <summary>
            /// Delete the item with the specified <paramref name="category"/> and <paramref name="key"/> from the store.
            /// </summary>
            /// <param name="category">The category of the item to delete.</param>
            /// <param name="key">The key of the item to delete.</param>
            public void DeleteItem(string category, string key)
            {
                CheckAccess();

                _writer.DeleteItem(category, key);
            }

            /// <summary>
            /// Gets a writable stream to store the state for the item with the specified <paramref name="category"/> and <paramref name="key"/>.
            /// </summary>
            /// <param name="category">The category of the item to obtain a stream for.</param>
            /// <param name="key">The key of the item to obtain a stream for.</param>
            /// <returns>Stream used to store the state for the specified item.</returns>
            public Stream GetItemWriter(string category, string key)
            {
                CheckAccess();

                return _writer.GetItemWriter(category, key);
            }

            /// <summary>
            /// Discards all changes in the transaction.
            /// </summary>
            public void Rollback()
            {
                CheckAccess();

                _writer.Rollback();
            }

            /// <summary>
            /// Disposes the state writer.
            /// This operation is idempotent and can take place after an unload operation.
            /// </summary>
            public void Dispose()
            {
                if (Interlocked.Exchange(ref _disposed, 1) == 0)
                {
                    _writer.Dispose();
                }
            }

            /// <summary>
            /// Unloads the state writer.
            /// This operation causes future interactions with the writer to be rejected with an <see cref="EngineUnloadedException"/>.
            /// </summary>
            public void Unload()
            {
                Volatile.Write(ref _unloaded, 1);
            }

            /// <summary>
            /// Checks whether access to the writer is allowed. Access is rejected when the writer is either disposed or unloaded.
            /// </summary>
            /// <exception cref="EngineUnloadedException">The request is rejected because the engine has been unloaded.</exception>
            /// <exception cref="ObjectDisposedException">The writer has been disposed.</exception>
            private void CheckAccess()
            {
                if (Volatile.Read(ref _disposed) == 1)
                    throw new ObjectDisposedException("writer");
                if (Volatile.Read(ref _unloaded) == 1)
                    throw new EngineUnloadedException();
            }
        }
    }
}
