// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading;

using Reaqtive;

namespace Reaqtor.QueryEngine.Mocks
{
    /// <summary>
    /// Provides a set of methods to manage <see cref="MockObserver{T}"/> instances.
    /// </summary>
    public static class MockObserver
    {
        private static readonly Dictionary<string, object> _observers = new();

        /// <summary>
        /// Gets or creates an observer with the specified identifier.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="id">Identifier for the observer.</param>
        /// <returns>An existing observer instance if an entry with the specified identifier already exists; otherwise, a new observer instance.</returns>
        public static IObserver<T> CreateObserver<T>(string id)
        {
            lock (_observers)
            {
                if (_observers.TryGetValue(id, out object value))
                {
                    return value as IObserver<T>;
                }

                var o = new MockObserver<T>();
                _observers.Add(id, o);
                return o;
            }
        }

        /// <summary>
        /// Clears all of the observer instances.
        /// </summary>
        public static void Clear()
        {
            lock (_observers)
            {
                _observers.Clear();
            }
        }

        /// <summary>
        /// Gets an observer with the specified identifier, if it exists.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="id">The identifier of the observer to retrieve.</param>
        /// <returns>An observer instance, if found; otherwise, null.</returns>
        public static MockObserver<T> Get<T>(string id)
        {
            lock (_observers)
            {
                if (!_observers.ContainsKey(id))
                {
                    return null;
                }

                return (MockObserver<T>)_observers[id];
            }
        }
    }

    public sealed class MockObserver<T> : Observer<T>
    {
        private readonly List<T> _values = new();
        private long _currentCount;

        private readonly object _lock = new();
#pragma warning disable CA2213 // "never disposed." Analyzer hasn't understood OnDispose
        private readonly ManualResetEvent _completed = new(false);
        private readonly ManualResetEvent _error = new(false);
#pragma warning restore CA2213


        public IList<T> Values => _values;

        public bool Completed
        {
            get;
            private set;
        }

        public bool Error
        {
            get;
            private set;
        }

        public Uri InstanceId
        {
            get;
            private set;
        }

        protected override void OnCompletedCore()
        {
            Completed = true;
            _completed.Set();
        }

        protected override void OnErrorCore(Exception error)
        {
            Error = true;
            _error.Set();
        }

        protected override void OnNextCore(T value)
        {
            lock (_lock)
            {
                _values.Add(value);
                ++_currentCount;
                Monitor.Pulse(_lock);
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                _values.Clear();
                _currentCount = 0;
            }
        }

        public override void SetContext(IOperatorContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            InstanceId = context.InstanceId;

            base.SetContext(context);
        }

        public bool WaitForCount(int count, TimeSpan t)
        {
            DateTime now = DateTime.UtcNow;
            DateTime expirationTime = now + t;

            TimeSpan timeout;
            lock (_lock)
            {
                while (count > _currentCount)
                {
                    timeout = expirationTime - DateTime.UtcNow;
                    if (timeout < TimeSpan.Zero)
                    {
                        return false;
                    }

                    if (!Monitor.Wait(_lock, timeout))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public bool WaitForCompleted(TimeSpan t) => _completed.WaitOne(t);

        public bool WaitForError(TimeSpan t) => _error.WaitOne(t);

        protected override void OnDispose()
        {
            _completed.Dispose();
            _error.Dispose();
        }
    }
}
