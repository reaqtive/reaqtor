// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - February 2018
//

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Reaqtor.QueryEngine;

namespace Utilities
{
    /// <summary>
    /// Implementation of <see cref="IStateWriter"/> with logging of operations through a <see cref="TextWriter"/>.
    /// </summary>
    public sealed class LoggingStateWriter : LoggingStateReaderWriterBase, IStateWriter
    {
        private readonly IStateWriter _writer;

        public LoggingStateWriter(IStateWriter writer, TextWriter log, bool keepOpen = true)
            : base(log, keepOpen)
        {
            _writer = writer;
        }

        public CheckpointKind CheckpointKind => _writer.CheckpointKind;

        public async Task CommitAsync(CancellationToken token, IProgress<int> progress)
        {
            LogStart(nameof(CommitAsync));
            try
            {
                await _writer.CommitAsync(token, progress).ConfigureAwait(false);
            }
            catch (Exception ex) when (LogError(nameof(CommitAsync), ex)) { throw; }
            finally
            {
                LogStop(nameof(CommitAsync));
            }
        }

        public void Rollback()
        {
            LogStart(nameof(Rollback));
            try
            {
                _writer.Rollback();
            }
            catch (Exception ex) when (LogError(nameof(Rollback), ex)) { throw; }
            finally
            {
                LogStop(nameof(Rollback));
            }
        }

        public Stream GetItemWriter(string category, string key)
        {
            LogStart(nameof(GetItemWriter), category, key);
            try
            {
                return _writer.GetItemWriter(category, key);
            }
            catch (Exception ex) when (LogError(nameof(GetItemWriter), ex, category, key)) { throw; }
            finally
            {
                LogStop(nameof(GetItemWriter), category, key);
            }
        }

        public void DeleteItem(string category, string key)
        {
            LogStart(nameof(DeleteItem), category, key);
            try
            {
                _writer.DeleteItem(category, key);
            }
            catch (Exception ex) when (LogError(nameof(DeleteItem), ex, category, key)) { throw; }
            finally
            {
                LogStop(nameof(DeleteItem), category, key);
            }
        }

        protected override void DisposeCore() => _writer.Dispose();
    }
}
