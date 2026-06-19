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

namespace Utilities
{
    /// <summary>
    /// Base class for logging state readers and writers.
    /// </summary>
    public abstract class LoggingStateReaderWriterBase : IDisposable
    {
        private readonly TextWriter _log;
        private readonly bool _keepOpen;

        protected LoggingStateReaderWriterBase(TextWriter log, bool keepOpen)
        {
            _log = log;
            _keepOpen = keepOpen;
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                LogStart(nameof(Dispose));
                try
                {
                    DisposeCore();
                }
                catch (Exception ex) when (LogError(nameof(Dispose), ex)) { throw; }
                finally
                {
                    LogStop(nameof(Dispose));
                }

                if (!_keepOpen)
                {
                    _log.Dispose();
                }
            }
        }

        protected abstract void DisposeCore();

        protected void LogStart(string method, params object[] args)
        {
            _log.WriteLine(method + "(" + string.Join(", ", args) + ")/Start");
        }

        protected bool LogError(string method, Exception exception, params object[] args)
        {
            _log.WriteLine(method + "(" + string.Join(", ", args) + ")/Error -> " + exception);
            return false;
        }

        protected void LogStop(string method, params object[] args)
        {
            _log.WriteLine(method + "(" + string.Join(", ", args) + ")/Stop");
        }
    }
}
