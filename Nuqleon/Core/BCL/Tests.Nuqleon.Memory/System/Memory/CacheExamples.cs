// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   ER - 11/12/2014 - Created this type.
//

using System;
using System.Linq.Expressions;
using System.Memory;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Memory
{
    [TestClass]
    public class CacheExamples
    {
        /// <summary>
        /// Simple example of caching constants expressions by sharing a
        /// parameter expression maintaining the expression type information
        /// and putting the constant value into a bundle.
        /// </summary>
        [TestMethod]
        public void Cache_ConstantExpressions()
        {
            var const1 = Expression.Constant(42);
            var const2 = Expression.Constant(7);
            var cache = new ConstantExpressionCache();
            var ref1 = cache.Create(const1);
            var ref2 = cache.Create(const2);

            Assert.AreEqual(const1.Value, ref1.Value.Value);
            Assert.AreEqual(const2.Value, ref2.Value.Value);
        }

        private sealed class ConstantExpressionCache : Cache<ConstantExpression, Type, object>
        {
            public ConstantExpressionCache()
            {
            }

            protected override Deconstructed<Type, object> Deconstruct(ConstantExpression item)
            {
                return Deconstructed.Create(item.Type, item.Value);
            }

            protected override ConstantExpression Reconstruct(Deconstructed<Type, object> deconstructed)
            {
                return Expression.Constant(deconstructed.NonCached, deconstructed.Cached);
            }
        }

        [TestMethod]
        public void Cache_LongStringCache()
        {
            var cache = new LongStringCache();

            var cached = "The quick brown fox jumped over the lazy dog.";
            var cachedCopy = string.Format("{0}", cached);
            Assert.AreNotSame(cached, cachedCopy);
            Assert.AreEqual(cached, cachedCopy);

            var cachedRef = cache.Create(cached);
            var cachedCopyRef = cache.Create(cachedCopy);
            Assert.AreSame(cachedRef.Value, cachedCopyRef.Value);
            Assert.AreEqual(cachedRef.Value, cachedCopyRef.Value);

            var notCached = "Foo";
            var notCachedCopy = string.Format("{0}", notCached);
            Assert.AreNotSame(notCached, notCachedCopy);
            Assert.AreEqual(notCached, notCachedCopy);

            var notCachedRef = cache.Create(notCached);
            var notCachedCopyRef = cache.Create(notCachedCopy);
            Assert.AreNotSame(notCachedRef.Value, notCachedCopyRef.Value);
            Assert.AreEqual(notCachedRef.Value, notCachedCopyRef.Value);
        }

        private sealed class LongStringCache : Cache<string>
        {
            public override IDiscardable<string> Create(string item)
            {
                return item.Length > 10
                    ? base.Create(item)
                    : new DummyReference(item);
            }

            private sealed class DummyReference : IDiscardable<string>
            {
                public DummyReference(string value)
                {
                    Value = value;
                }

                public string Value { get; }

                public void Dispose()
                {
                }
            }
        }

        [TestMethod]
        public void Cache_Composability()
        {
            var fooCache = new FooCache();
            var s1 = "hello";
            var s2 = Copy(s1);
            var foo1 = new Foo { Bar = new Bar { Baz = s1, Qux = new object() }, Num = 42 };
            var foo2 = new Foo { Bar = new Bar { Baz = s2, Qux = new object() }, Num = 42 };
            Assert.AreNotSame(s1, s2);

            var ref1 = fooCache.Create(foo1);
            var ref2 = fooCache.Create(foo2);
            Assert.AreSame(ref1.Value.Bar.Baz, ref2.Value.Bar.Baz);
        }

        private static string Copy(string s)
        {
            return string.Format("{0}", s);
        }

        private sealed class Bar
        {
            public string Baz;
            public object Qux;
        }

        private sealed class Foo
        {
            public Bar Bar;
            public int Num;
        }

        private sealed class BarCache : Cache<Bar, string, object>
        {
            protected override Deconstructed<string, object> Deconstruct(Bar item)
            {
                return Deconstructed.Create(item.Baz, item.Qux);
            }

            protected override Bar Reconstruct(Deconstructed<string, object> deconstructed)
            {
                return new Bar { Baz = deconstructed.Cached, Qux = deconstructed.NonCached };
            }
        }

        private sealed class FooCache : Cache<Foo, Bar, int>
        {
            public FooCache()
                : base(new BarCache())
            {
            }

            protected override Deconstructed<Bar, int> Deconstruct(Foo item)
            {
                return Deconstructed.Create(item.Bar, item.Num);
            }

            protected override Foo Reconstruct(Deconstructed<Bar, int> deconstructed)
            {
                return new Foo { Bar = deconstructed.Cached, Num = deconstructed.NonCached };
            }
        }
    }
}
