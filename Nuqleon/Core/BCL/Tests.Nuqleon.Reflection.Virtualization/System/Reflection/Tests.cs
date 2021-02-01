// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Memory;
using System.Reflection;

namespace Tests.System.Reflection.Virtualization
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void ManOrBoy()
        {
            var memoizer = Memoizer.Create(MemoizationCacheFactory.Unbounded);

            var cacheNone = new CachingDefaultReflectionProvider(memoizer, ReflectionCachingOptions.None);
            var cacheAll = new CachingDefaultReflectionProvider(memoizer);

            foreach (var p in new[]
            {
                DefaultReflectionProvider.Instance,
                cacheNone,
                cacheAll,
            })
            {
                foreach (var t in new[]
                {
                    typeof(object),
                    typeof(int),
                    typeof(string),
                    typeof(int[]),
                    typeof(string[,]),
                    typeof(IFormattable),
                    typeof(IEnumerable<>),
                    typeof(IEnumerable<int>),
                })
                {
                    Assert.AreEqual(t.BaseType, p.GetBaseType(t));

                    Assert.AreEqual(t.IsAssignableFrom(typeof(object)), p.IsAssignableFrom(t, typeof(object)));
                    Assert.AreEqual(t.IsSubclassOf(typeof(object)), p.IsSubclassOf(t, typeof(object)));
                    Assert.AreEqual(t.IsEquivalentTo(typeof(object)), p.IsEquivalentTo(t, typeof(object)));
                    Assert.AreEqual(t.IsInstanceOfType("foo"), p.IsInstanceOfType(t, "foo"));

                    Assert.AreEqual(t.IsAbstract, p.IsAbstract(t));
                    Assert.AreEqual(t.IsAnsiClass, p.IsAnsiClass(t));
                    Assert.AreEqual(t.IsArray, p.IsArray(t));
                    Assert.AreEqual(t.IsAutoClass, p.IsAutoClass(t));
                    Assert.AreEqual(t.IsAutoLayout, p.IsAutoLayout(t));
                    Assert.AreEqual(t.IsByRef, p.IsByRef(t));
                    Assert.AreEqual(t.IsClass, p.IsClass(t));
                    Assert.AreEqual(t.IsPrimitive, p.IsPrimitive(t));
                    Assert.AreEqual(t.IsPointer, p.IsPointer(t));
                    Assert.AreEqual(t.IsInterface, p.IsInterface(t));
                    Assert.AreEqual(t.IsValueType, p.IsValueType(t));
                    Assert.AreEqual(t.IsVisible, p.IsVisible(t));

                    Assert.AreEqual(t.HasElementType, p.HasElementType(t));

                    Assert.AreEqual(t.Attributes, p.GetAttributes(t));
                    Assert.AreEqual(t.FullName, p.GetFullName(t));
                    Assert.AreEqual(t.Name, p.GetName(t));
                }
            }

            cacheNone.Clear();
            cacheAll.Clear();
        }
    }
}
