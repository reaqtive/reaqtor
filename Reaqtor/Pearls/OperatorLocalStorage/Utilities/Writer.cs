// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Reaqtor.QueryEngine;

namespace Utilities
{
    /// <summary>
    /// Implementation of <see cref="IStateWriter"/> for the in-memory key/value store implementation in <see cref="Store"/>.
    /// </summary>
    public sealed class Writer : IStateWriter
    {
        private readonly Store _store;
        private readonly List<StateWriterOperation> _log = new();

        public Writer(Store store, CheckpointKind checkpointKind)
        {
            _store = store;
            CheckpointKind = checkpointKind;
        }

        public CheckpointKind CheckpointKind { get; private set; }

        public StateWriterOperation[] GetLog() => _log.ToArray();

        public Task CommitAsync(CancellationToken token, IProgress<int> progress)
        {
            var log = default(StateWriterOperation[]);

            lock (_log)
            {
                log = _log.ToArray();
            }

            foreach (var item in log)
            {
                item.Apply(_store);
            }

            return Task.CompletedTask;
        }

        public void DeleteItem(string category, string key)
        {
            lock (_log)
            {
                _log.Add(new DeleteStateWriterOperation(category, key));
            }
        }

        public void Dispose() { }

        public Stream GetItemWriter(string category, string key)
        {
            var data = new MemoryStream();

            lock (_log)
            {
                _log.Add(new AddOrUpdateStateWriterOperation(category, key, data));
            }

            return data;
        }

        public void Rollback() { }
    }
}
