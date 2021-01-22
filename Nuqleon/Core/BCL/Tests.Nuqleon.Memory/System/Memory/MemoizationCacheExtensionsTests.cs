// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/29/2015 - Initial work on memoization support.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Memory;

namespace Tests
{
    [TestClass]
    public class MemoizationCacheExtensionsTests
    {
#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
        [TestMethod]
        public void MemoizationCacheExtensions_AsTrimmable_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => MemoizationCacheExtensions.AsTrimmableByMetrics(default(IMemoizationCache)));
            Assert.ThrowsException<ArgumentNullException>(() => MemoizationCacheExtensions.AsTrimmableByArgumentAndResult<int, int>(default(IMemoizationCache)));
            Assert.ThrowsException<ArgumentNullException>(() => MemoizationCacheExtensions.AsTrimmableByArgumentAndResult<int, int>(default(IMemoizationCache<int, int>)));
            Assert.ThrowsException<ArgumentNullException>(() => MemoizationCacheExtensions.AsTrimmableByArgumentAndResultOrError<int, int>(default(IMemoizationCache)));
            Assert.ThrowsException<ArgumentNullException>(() => MemoizationCacheExtensions.AsTrimmableByArgumentAndResultOrError<int, int>(default(IMemoizationCache<int, int>)));
        }

        [TestMethod]
        public void MemoizationCacheExtensions_ToTrimmable_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => MemoizationCacheExtensions.ToTrimmableByMetrics(default(IMemoizationCache)));
            Assert.ThrowsException<ArgumentNullException>(() => MemoizationCacheExtensions.ToTrimmableByArgumentAndResult<int, int>(default(IMemoizationCache)));
            Assert.ThrowsException<ArgumentNullException>(() => MemoizationCacheExtensions.ToTrimmableByArgumentAndResult<int, int>(default(IMemoizationCache<int, int>)));
            Assert.ThrowsException<ArgumentNullException>(() => MemoizationCacheExtensions.ToTrimmableByArgumentAndResultOrError<int, int>(default(IMemoizationCache)));
            Assert.ThrowsException<ArgumentNullException>(() => MemoizationCacheExtensions.ToTrimmableByArgumentAndResultOrError<int, int>(default(IMemoizationCache<int, int>)));
        }
#pragma warning restore IDE0034 // Simplify 'default' expression

        [TestMethod]
        public void MemoizationCacheExtensions_AsTrimmable_HasService()
        {
            var c = new MyCache<int, int>(hasServices: true);

            Assert.IsNotNull(MemoizationCacheExtensions.AsTrimmableByMetrics(c));
            Assert.IsNotNull(MemoizationCacheExtensions.AsTrimmableByArgumentAndResult<int, int>(c));
            Assert.IsNotNull(MemoizationCacheExtensions.AsTrimmableByArgumentAndResult<int, int>((IMemoizationCache)c));
            Assert.IsNotNull(MemoizationCacheExtensions.AsTrimmableByArgumentAndResultOrError<int, int>(c));
            Assert.IsNotNull(MemoizationCacheExtensions.AsTrimmableByArgumentAndResultOrError<int, int>((IMemoizationCache)c));
        }

        [TestMethod]
        public void MemoizationCacheExtensions_ToTrimmable_HasService()
        {
            var c = new MyCache<int, int>(hasServices: true);

            Assert.IsNotNull(MemoizationCacheExtensions.ToTrimmableByMetrics(c));
            Assert.IsNotNull(MemoizationCacheExtensions.ToTrimmableByArgumentAndResult<int, int>(c));
            Assert.IsNotNull(MemoizationCacheExtensions.ToTrimmableByArgumentAndResult<int, int>((IMemoizationCache)c));
            Assert.IsNotNull(MemoizationCacheExtensions.ToTrimmableByArgumentAndResultOrError<int, int>(c));
            Assert.IsNotNull(MemoizationCacheExtensions.ToTrimmableByArgumentAndResultOrError<int, int>((IMemoizationCache)c));
        }

        [TestMethod]
        public void MemoizationCacheExtensions_AsTrimmable_HasNoService()
        {
            var c = new MyCache<int, int>(false);

            Assert.IsNull(MemoizationCacheExtensions.AsTrimmableByMetrics(c));
            Assert.IsNull(MemoizationCacheExtensions.AsTrimmableByArgumentAndResult<int, int>(c));
            Assert.IsNull(MemoizationCacheExtensions.AsTrimmableByArgumentAndResult<int, int>((IMemoizationCache)c));
            Assert.IsNull(MemoizationCacheExtensions.AsTrimmableByArgumentAndResultOrError<int, int>(c));
            Assert.IsNull(MemoizationCacheExtensions.AsTrimmableByArgumentAndResultOrError<int, int>((IMemoizationCache)c));
        }

        [TestMethod]
        public void MemoizationCacheExtensions_ToTrimmable_HasNoService()
        {
            var c = new MyCache<int, int>(false);

            Assert.ThrowsException<InvalidOperationException>(() => MemoizationCacheExtensions.ToTrimmableByMetrics(c));
            Assert.ThrowsException<InvalidOperationException>(() => MemoizationCacheExtensions.ToTrimmableByArgumentAndResult<int, int>(c));
            Assert.ThrowsException<InvalidOperationException>(() => MemoizationCacheExtensions.ToTrimmableByArgumentAndResult<int, int>((IMemoizationCache)c));
            Assert.ThrowsException<InvalidOperationException>(() => MemoizationCacheExtensions.ToTrimmableByArgumentAndResultOrError<int, int>(c));
            Assert.ThrowsException<InvalidOperationException>(() => MemoizationCacheExtensions.ToTrimmableByArgumentAndResultOrError<int, int>((IMemoizationCache)c));
        }

        private sealed class MyCache<T, R> : IMemoizationCache<T, R>, IServiceProvider
        {
            private readonly bool _hasServices;

            public MyCache(bool hasServices = false) => _hasServices = hasServices;

            public R GetOrAdd(T argument) => throw new NotImplementedException();

            public string DebugView => throw new NotImplementedException();

            public int Count => throw new NotImplementedException();

            public void Clear() => throw new NotImplementedException();

            public void Dispose() => throw new NotImplementedException();

            public object GetService(Type serviceType)
            {
                if (_hasServices)
                {
                    if (serviceType == typeof(ITrimmable<KeyValuePair<T, R>>))
                    {
                        return Trimmable.Create<KeyValuePair<T, R>>(_ => 0);
                    }
                    else if (serviceType == typeof(ITrimmable<KeyValuePair<T, IValueOrError<R>>>))
                    {
                        return Trimmable.Create<KeyValuePair<T, IValueOrError<R>>>(_ => 0);
                    }
                    else if (serviceType == typeof(ITrimmable<IMemoizationCacheEntryMetrics>))
                    {
                        return Trimmable.Create<IMemoizationCacheEntryMetrics>(_ => 0);
                    }
                }

                return null;
            }
        }
    }
}
