// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 08/04/2015 - Adding intern caches.
//

using System.Collections.Generic;

namespace System.Memory
{
    public static partial class InternCache
    {
        private sealed class Weak<T> : IWeakInternCache<T>, IServiceProvider
            where T : class
        {
            private readonly Func<T, T> _clone;
            private readonly IMemoizedDelegate<Func<T, WeakReference<T>>> _delegate;

            public Weak(Func<T, T> clone, IMemoizedDelegate<Func<T, WeakReference<T>>> @delegate)
            {
                _clone = clone;
                _delegate = @delegate;
            }

            public string DebugView => _delegate.Cache.DebugView;

            public int Count => _delegate.Cache.Count;

            public void Clear() => _delegate.Cache.Clear();

            public void Dispose() => _delegate.Cache.Dispose();

            public T Intern(T value)
            {
                var weakRef = _delegate.Delegate(value);
                if (!weakRef.TryGetTarget(out T res))
                {
                    res = _clone(value);
                    weakRef.SetTarget(res);
                }

                return res;
            }

            public int Trim()
            {
                var trim = this.GetService<ITrimmable<KeyValuePair<T, WeakReference<T>>>>();
                if (trim == null)
                    throw new NotSupportedException("The underlying cache doesn't support trimming.");

                return trim.Trim(kv => !kv.Value.TryGetTarget(out T ignored));
            }

            public object GetService(Type serviceType) => _delegate.Cache.GetService(serviceType);
        }
    }
}
