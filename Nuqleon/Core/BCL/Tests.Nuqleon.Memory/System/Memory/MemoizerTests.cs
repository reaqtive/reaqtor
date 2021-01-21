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
    public class MemoizerTests
    {
        [TestMethod]
        public void Memoizer_Create_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Memoizer.Create(factory: null));
            Assert.ThrowsException<ArgumentNullException>(() => Memoizer.Create(new MyCacheFactory()).Memoize(default(Func<int, int>), MemoizationOptions.None, EqualityComparer<int>.Default));

            Assert.ThrowsException<ArgumentNullException>(() => Memoizer.CreateWeak(factory: null));
            Assert.ThrowsException<ArgumentNullException>(() => Memoizer.CreateWeak(new MyWeakCacheFactory()).MemoizeWeak(default(Func<string, string>), MemoizationOptions.None));
        }

        [TestMethod]
        public void Memoizer_Create()
        {
            var res = Memoizer.Create(new MyCacheFactory()).Memoize<string, int>(s => s.Length, MemoizationOptions.None, EqualityComparer<string>.Default);

            Assert.IsNotNull(res);

            Assert.IsNotNull(res.Delegate);
            Assert.IsNotNull(res.Cache);

            Assert.IsTrue(res.Cache is MyCache<string, int>);
        }

        [TestMethod]
        public void Memoizer_CreateWeak()
        {
            var res = Memoizer.CreateWeak(new MyWeakCacheFactory()).MemoizeWeak<string, int>(s => s.Length, MemoizationOptions.None);

            Assert.IsNotNull(res);

            Assert.IsNotNull(res.Delegate);
            Assert.IsNotNull(res.Cache);

            Assert.IsTrue(res.Cache is MyCache<string, int>);
        }

        private sealed class MyCacheFactory : IMemoizationCacheFactory
        {
            public IMemoizationCache<T, TResult> Create<T, TResult>(Func<T, TResult> function, MemoizationOptions options, IEqualityComparer<T> comparer)
            {
                return new MyCache<T, TResult>();
            }
        }

        private sealed class MyWeakCacheFactory : IWeakMemoizationCacheFactory
        {
            public IMemoizationCache<T, TResult> Create<T, TResult>(Func<T, TResult> function, MemoizationOptions options) where T : class
            {
                return new MyCache<T, TResult>();
            }
        }

        private sealed class MyCache<T, R> : IMemoizationCache<T, R>
        {
            public R GetOrAdd(T argument) => throw new NotImplementedException();

            public string DebugView => throw new NotImplementedException();

            public int Count => throw new NotImplementedException();

            public void Clear() => throw new NotImplementedException();

            public void Dispose() => throw new NotImplementedException();
        }
    }
}
