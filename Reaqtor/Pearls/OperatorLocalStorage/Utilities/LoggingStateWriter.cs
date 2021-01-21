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
    public sealed class LoggingStateWriter : IStateWriter
    {
        private readonly IStateWriter _writer;
        private readonly TextWriter _log;

        public LoggingStateWriter(IStateWriter writer, TextWriter log)
        {
            _writer = writer;
            _log = log;
        }

        public CheckpointKind CheckpointKind => _writer.CheckpointKind;

        public async Task CommitAsync(CancellationToken token, IProgress<int> progress)
        {
            LogStart(nameof(CommitAsync));
            try
            {
                await _writer.CommitAsync(token, progress);
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

        public void Dispose()
        {
            LogStart(nameof(Dispose));
            try
            {
                _writer.Dispose();
            }
            catch (Exception ex) when (LogError(nameof(Dispose), ex)) { throw; }
            finally
            {
                LogStop(nameof(Dispose));
            }
        }

        private void LogStart(string method, params object[] args)
        {
            _log.WriteLine(method + "(" + string.Join(", ", args) + ")/Start");
        }

        private bool LogError(string method, Exception exception, params object[] args)
        {
            _log.WriteLine(method + "(" + string.Join(", ", args) + ")/Error -> " + exception);
            return false;
        }

        private void LogStop(string method, params object[] args)
        {
            _log.WriteLine(method + "(" + string.Join(", ", args) + ")/Stop");
        }
    }
}
