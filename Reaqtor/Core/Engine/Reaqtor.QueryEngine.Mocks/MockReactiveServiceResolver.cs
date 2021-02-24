// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

using Reaqtor.Reliable.Expressions;

namespace Reaqtor.QueryEngine.Mocks
{
    internal class MockReactiveServiceResolver : IReactiveServiceResolver, IDisposable
    {
        private readonly object _lock = new();
        private readonly List<ICheckpointingQueryEngine> _queryEngines = new();

        public List<ICheckpointingQueryEngine> QueryEngines
        {
            get
            {
                lock (_lock)
                {
                    return new List<ICheckpointingQueryEngine>(_queryEngines);
                }
            }
        }

        public void AddQueryEngine(ICheckpointingQueryEngine queryEngine)
        {
            lock (_lock)
            {
                _queryEngines.Add(queryEngine);
            }
        }

        public void RemoveQueryEngine(ICheckpointingQueryEngine queryEngine)
        {
            lock (_lock)
            {
                _queryEngines.Remove(queryEngine);
            }
        }

        private bool TryResolve(Uri uri, out ICheckpointingQueryEngine queryEngine)
        {
            lock (_lock)
            {
                var u = uri.ToCanonicalString();
                foreach (var c in _queryEngines)
                {
                    var prefix = c.Uri.ToCanonicalString() + "/";
                    if (u.StartsWith(prefix, StringComparison.Ordinal))
                    {
                        queryEngine = c;
                        return true;
                    }
                }

                queryEngine = null;
                return false;
            }
        }

        public bool TryResolve(Uri uri, out IReactive service)
        {
            lock (_lock)
            {
                if (TryResolve(uri, out ICheckpointingQueryEngine c))
                {
                    service = c.ReactiveService;
                    return true;
                }

                service = null;
                return false;
            }
        }

        public bool TryResolve(Uri uri, out IReactiveProxy service)
        {
            throw new NotImplementedException();
        }

        public bool TryResolveReliable(Uri uri, out IReliableReactive service)
        {
            if (TryResolve(uri, out ICheckpointingQueryEngine c))
            {
                service = c.ReliableReactiveService;
                return true;
            }

            service = null;
            return false;
        }

        public void Dispose()
        {
            lock (_lock)
            {
                foreach (var c in _queryEngines)
                {
                    ((CheckpointingQueryEngine)c).Scheduler.Dispose();
                }

                _queryEngines.Clear();
            }
        }
    }
}
