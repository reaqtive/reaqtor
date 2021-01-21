// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Linq.Expressions;

using Reaqtor.Metadata;
using Reaqtor.QueryEngine.Metrics;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Base class for runtime (hot) entities in QueryEngineRegistry.
    /// The runtime entities are streams and subscriptions.
    ///
    /// Owns the lifetime of the runtime instance once it is set - i.e.
    /// disposing the entity will dispose the runtime instance.
    /// </summary>
    internal abstract class RuntimeEntity<TEntity> : ReactiveEntity, IDisposable, IReactiveProcessResource
        where TEntity : IDisposable
    {
        private readonly object _lock = new();

        private bool _started;
        private bool _disposed;
        private TEntity _instance;

        protected RuntimeEntity(Uri uri, Expression expression, object state)
            : base(uri, expression, state)
        {
        }

        /// <summary>
        /// The instance of the runtime entity. The instance can be set after
        /// the entity is disposed, which will result in the entity being disposed
        /// immediately.
        /// </summary>
        public TEntity Instance
        {
            get
            {
                lock (_lock)
                {
                    return _instance;
                }
            }

            set
            {
                lock (_lock)
                {
                    if (_instance != null)
                    {
                        throw new InvalidOperationException("The instance of this runtime entity is already set.");
                    }

                    _instance = value;

                    if (_disposed)
                    {
                        _instance.Dispose();
                    }
                }
            }
        }

        public DateTimeOffset CreationTime => throw new NotImplementedException();

        public void Start(params object[] args)
        {
            if (Instance == null)
            {
                throw new InvalidOperationException("Entity not initialized.");
            }

            StartImpl(Instance, args);
        }

        public void Start(TEntity instance, params object[] args)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            if (StartImpl(instance, args))
            {
                // If StartImpl fails, we won't set the artifact, so it can't be observed by checkpointing etc.
                Instance = instance;
            }
        }

        private bool StartImpl(TEntity instance, params object[] args)
        {
            var shouldStart = false;

            lock (_lock)
            {
                if (!_disposed && !_started)
                {
                    shouldStart = true;
                    _started = true;
                }
            }

            if (shouldStart)
            {
                using (this.Measure(EntityMetric.Start))
                {
                    StartCore(instance, args);
                }
            }

            return shouldStart;
        }

        protected abstract void StartCore(TEntity instance, params object[] args);

        public void Dispose()
        {
            lock (_lock)
            {
                if (!_disposed)
                {
                    if (_instance != null)
                    {
                        using (this.Measure(EntityMetric.Dispose))
                        {
                            _instance.Dispose();
                        }
                    }

                    _disposed = true;
                }
            }
        }

        /// <summary>
        /// Serializes the object.
        /// </summary>
        /// <param name="serializer">The serializer.</param>
        /// <param name="stream">The stream.</param>
        public override void Serialize(ISerializer serializer, Stream stream)
        {
            base.Serialize(serializer, stream);
            serializer.Serialize(_disposed, stream);
        }

        /// <summary>
        /// Deserializes the object.
        /// </summary>
        /// <param name="serializer">The serializer.</param>
        /// <param name="stream">The stream.</param>
        public override void Deserialize(ISerializer serializer, Stream stream)
        {
            base.Deserialize(serializer, stream);
            _disposed = serializer.Deserialize<bool>(stream);
        }
    }
}
