// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 08/04/2015 - Adding intern caches.
//

namespace System.Memory
{
    public static partial class InternCache
    {
        private sealed class Strong<T> : IInternCache<T>, IServiceProvider
            where T : class
        {
            private readonly IMemoizedDelegate<Func<T, T>> _delegate;

            public Strong(IMemoizedDelegate<Func<T, T>> @delegate) => _delegate = @delegate;

            public string DebugView => _delegate.Cache.DebugView;

            public int Count => _delegate.Cache.Count;

            public void Clear() => _delegate.Cache.Clear();

            public void Dispose() => _delegate.Cache.Dispose();

            public T Intern(T value) => _delegate.Delegate(value);

            public object GetService(Type serviceType) => _delegate.Cache.GetService(serviceType);
        }
    }
}
