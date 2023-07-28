// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace System.Threading
{
    /// <summary>
    /// Asynchronous reader-writer lock implementation that:
    ///   1) Prioritizes writers - Waiting writers will get the lock before waiting readers.
    ///   2) Doesn't throttle readers - All waiting readers will get the lock when there are no waiting writers
    /// Source: http://blogs.msdn.com/b/pfxteam/archive/2012/02/12/building-async-coordination-primitives-part-7-asyncreaderwriterlock.aspx
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal sealed class AsyncReaderWriterLock
    {
        private readonly Task<Releaser> m_readerReleaser;
        private readonly Task<Releaser> m_writerReleaser;
        private readonly Queue<TaskCompletionSource<Releaser>> m_waitingWriters;

        private TaskCompletionSource<Releaser> m_waitingReader;
        private int m_readersWaiting;
        private int m_status;

        public AsyncReaderWriterLock()
        {
            m_readerReleaser = Task.FromResult(new Releaser(this, writer: false));
            m_writerReleaser = Task.FromResult(new Releaser(this, writer: true));
            m_waitingWriters = new Queue<TaskCompletionSource<Releaser>>();
            m_waitingReader = new TaskCompletionSource<Releaser>();
        }

        public Task<Releaser> EnterReadAsync()
        {
            lock (m_waitingWriters)
            {
                if (m_status >= 0 && m_waitingWriters.Count == 0)
                {
                    ++m_status;
                    return m_readerReleaser;
                }
                else
                {
                    ++m_readersWaiting;
                    return m_waitingReader.Task.ContinueWith(t => t.Result, TaskScheduler.Default);
                }
            }
        }

        public Task<Releaser> EnterWriteAsync()
        {
            lock (m_waitingWriters)
            {
                if (m_status == 0)
                {
                    m_status = -1;
                    return m_writerReleaser;
                }
                else
                {
                    var waiter = new TaskCompletionSource<Releaser>();
                    m_waitingWriters.Enqueue(waiter);
                    return waiter.Task;
                }
            }
        }

        private void ExitRead()
        {
            TaskCompletionSource<Releaser> toWake = null;


            lock (m_waitingWriters)
            {
                --m_status;
                if (m_status == 0 && m_waitingWriters.Count > 0)
                {
                    m_status = -1;
                    toWake = m_waitingWriters.Dequeue();
                }
            }

            toWake?.SetResult(new Releaser(this, true));
        }

        private void ExitWrite()
        {
            TaskCompletionSource<Releaser> toWake = null;
            bool toWakeIsWriter = false;

            lock (m_waitingWriters)
            {
                if (m_waitingWriters.Count > 0)
                {
                    toWake = m_waitingWriters.Dequeue();
                    toWakeIsWriter = true;
                }
                else if (m_readersWaiting > 0)
                {
                    toWake = m_waitingReader;
                    m_status = m_readersWaiting;
                    m_readersWaiting = 0;
                    m_waitingReader = new TaskCompletionSource<Releaser>();
                }
                else m_status = 0;
            }

            toWake?.SetResult(new Releaser(this, toWakeIsWriter));
        }

        public readonly struct Releaser : IDisposable
        {
            private readonly AsyncReaderWriterLock m_toRelease;
            private readonly bool m_writer;

            internal Releaser(AsyncReaderWriterLock toRelease, bool writer)
            {
                m_toRelease = toRelease;
                m_writer = writer;
            }

            public void Dispose()
            {
                if (m_toRelease != null)
                {
                    if (m_writer) m_toRelease.ExitWrite();
                    else m_toRelease.ExitRead();
                }
            }
        }
    }
}
