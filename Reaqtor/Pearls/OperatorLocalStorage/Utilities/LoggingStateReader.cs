// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - February 2018
//

using System;
using System.Collections.Generic;
using System.IO;

using Reaqtor.QueryEngine;

namespace Utilities
{
    /// <summary>
    /// Implementation of <see cref="IStateReader"/> with logging of operations through a <see cref="TextWriter"/>.
    /// </summary>
    public sealed class LoggingStateReader : IStateReader
    {
        private readonly IStateReader _reader;
        private readonly TextWriter _log;

        public LoggingStateReader(IStateReader reader, TextWriter log)
        {
            _reader = reader;
            _log = log;
        }

        public void Dispose()
        {
            LogStart(nameof(Dispose));
            try
            {
                _reader.Dispose();
            }
            catch (Exception ex) when (LogError(nameof(Dispose), ex)) { throw; }
            finally
            {
                LogStop(nameof(Dispose));
            }
        }

        public IEnumerable<string> GetCategories()
        {
            LogStart(nameof(GetCategories));
            try
            {
                return _reader.GetCategories();
            }
            catch (Exception ex) when (LogError(nameof(GetCategories), ex)) { throw; }
            finally
            {
                LogStop(nameof(GetCategories));
            }
        }

        public bool TryGetItemKeys(string category, out IEnumerable<string> keys)
        {
            LogStart(nameof(TryGetItemKeys), category);
            try
            {
                return _reader.TryGetItemKeys(category, out keys);
            }
            catch (Exception ex) when (LogError(nameof(TryGetItemKeys), ex, category)) { throw; }
            finally
            {
                LogStop(nameof(TryGetItemKeys), category);
            }
        }

        public bool TryGetItemReader(string category, string key, out Stream stream)
        {
            LogStart(nameof(TryGetItemReader), category, key);
            try
            {
                return _reader.TryGetItemReader(category, key, out stream);
            }
            catch (Exception ex) when (LogError(nameof(TryGetItemReader), ex, category, key)) { throw; }
            finally
            {
                LogStop(nameof(TryGetItemReader), category, key);
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
