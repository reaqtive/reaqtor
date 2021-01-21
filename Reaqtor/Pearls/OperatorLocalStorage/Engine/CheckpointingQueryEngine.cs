// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - February 2018
//

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Reaqtive;
using Reaqtive.Scheduler;
using Reaqtive.Storage;

using Reaqtor;
using Reaqtor.QueryEngine;
using Reaqtor.QueryEngine.Events;
using Reaqtor.Reactive;

using Utilities;

namespace Engine
{
    //
    // NB: This type overrides CheckpointAsync and RecoverAsync behavior to split the persisted state into two top-level state "namespaces", using some hacks with scheduler events. The final
    //     integration into the existing engine would be much more direct and alter the behavior of these operations to apply reads/writes at the right time and without using top-level state
    //     "namespaces, likely by making the persisted object space store its state in a sibling category besides the existing categories (e.g. SubscriptionsRuntimeState, etc.).
    //

    /// <summary>
    /// Extension of <see cref="Reaqtive.QueryEngine.CheckpointingQueryEngine"/> to support a persisted object space that can be used by reactive artifacts.
    /// </summary>
    public class CheckpointingQueryEngine : Reaqtor.QueryEngine.CheckpointingQueryEngine
    {
        private readonly PersistedObjectSpace _objectSpace;

        public CheckpointingQueryEngine(Uri uri, IReactiveServiceResolver serviceResolver, IScheduler scheduler, IReactiveMetadata metadataRegistry, IKeyValueStore keyValueStore, IQuotedTypeConversionTargets quotedTypeConversionTargets, TraceSource traceSource, ICompiledDelegateCache delegateCache)
            : base(uri, serviceResolver, scheduler, metadataRegistry, keyValueStore, SerializationPolicy.Default, quotedTypeConversionTargets, traceSource, delegateCache)
        {
            _objectSpace = new PersistedObjectSpace(new SerializationFactory());
        }

        public IDictionary<string, object> OperatorContextElements { get; } = new ConcurrentDictionary<string, object>();

        public new async Task CheckpointAsync(IStateWriter writer, CancellationToken token, IProgress<int> progress)
        {
            var coreState = new PartitionedStateWriter(writer, "Reactive", isOwner: true);
            var objectState = new PartitionedStateWriter(writer, "ObjectSpace", isOwner: false);

            var pausedHandler = new EventHandler<SchedulerPausedEventArgs>(delegate
            {
                _objectSpace.Save(objectState);
            });

            SchedulerPaused += pausedHandler;

            try
            {
                await base.CheckpointAsync(coreState, token, progress).ConfigureAwait(false);
            }
            finally
            {
                SchedulerPaused -= pausedHandler;
            }

            _objectSpace.OnSaved();
        }

        public new async Task RecoverAsync(IStateReader reader, CancellationToken token, IProgress<int> progress)
        {
            var coreState = new PartitionedStateReader(reader, "Reactive", isOwner: true);
            var objectState = new PartitionedStateReader(reader, "ObjectSpace", isOwner: false);

            var pausedHandler = new EventHandler<SchedulerPausedEventArgs>(delegate
            {
                _objectSpace.Load(objectState);
            });

            SchedulerPaused += pausedHandler;

            try
            {
                await base.RecoverAsync(coreState, token, progress).ConfigureAwait(false);
            }
            finally
            {
                SchedulerPaused -= pausedHandler;
            }
        }

        protected override IHostedOperatorContext CreateOperatorContext(Uri instanceId)
        {
            var res = base.CreateOperatorContext(instanceId);

            return new OperatorContext(res, this);
        }

        private sealed class OperatorContext : IHostedOperatorContext
        {
            private readonly IHostedOperatorContext _context;
            private readonly CheckpointingQueryEngine _parent;

            public OperatorContext(IHostedOperatorContext context, CheckpointingQueryEngine parent)
            {
                _context = context;
                _parent = parent;
            }

            public Uri InstanceId => _context.InstanceId;

            public IReactive ReactiveService => _context.ReactiveService;

            public IScheduler Scheduler => _context.Scheduler;

            public TraceSource TraceSource => _context.TraceSource;

            public IExecutionEnvironment ExecutionEnvironment => _context.ExecutionEnvironment;

            public bool TryGetElement<T>(string id, out T value)
            {
                if (!_context.TryGetElement(id, out value))
                {
                    if (id == "PersistedObjectSpace")
                    {
                        value = (T)(object)_parent._objectSpace;
                    }
                    else if (_parent.OperatorContextElements.TryGetValue(id, out var item))
                    {
                        value = (T)item;
                    }
                    else
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        private sealed class PartitionedStateWriter : IStateWriter
        {
            private readonly IStateWriter _writer;
            private readonly string _prefix;
            private readonly bool _isOwner;

            public PartitionedStateWriter(IStateWriter writer, string prefix, bool isOwner)
            {
                _writer = writer;
                _prefix = prefix + "/";
                _isOwner = isOwner;
            }

            public CheckpointKind CheckpointKind => _writer.CheckpointKind;

            public Task CommitAsync(CancellationToken token, IProgress<int> progress) => _isOwner ? _writer.CommitAsync(token, progress) : throw new NotSupportedException();

            public void DeleteItem(string category, string key) => _writer.DeleteItem(_prefix + category, key);

            public void Dispose()
            {
                if (_isOwner)
                {
                    _writer.Dispose();
                }
            }

            public Stream GetItemWriter(string category, string key) => _writer.GetItemWriter(_prefix + category, key);

            public void Rollback()
            {
                if (!_isOwner)
                    throw new NotSupportedException();

                _writer.Rollback();
            }
        }

        private sealed class PartitionedStateReader : IStateReader
        {
            private readonly IStateReader _reader;
            private readonly string _prefix;
            private readonly bool _isOwner;

            public PartitionedStateReader(IStateReader reader, string prefix, bool isOwner)
            {
                _reader = reader;
                _prefix = prefix + "/";
                _isOwner = isOwner;
            }

            public void Dispose()
            {
                if (_isOwner)
                {
                    _reader.Dispose();
                }
            }

#pragma warning disable IDE0057 // Substring can be simplified. (Suppressing here for simplicify with multi-targeting.)
            public IEnumerable<string> GetCategories() => _reader.GetCategories().Where(c => c.StartsWith(_prefix) && c.Length > _prefix.Length).Select(c => c.Substring(_prefix.Length));
#pragma warning restore IDE0057

            public bool TryGetItemKeys(string category, out IEnumerable<string> keys) => _reader.TryGetItemKeys(_prefix + category, out keys);

            public bool TryGetItemReader(string category, string key, out Stream stream) => _reader.TryGetItemReader(_prefix + category, key, out stream);
        }
    }
}
