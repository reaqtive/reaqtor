// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Memory;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class HeapOptimizerTests
    {
        [TestMethod]
        public void HeapOptimizer_Basics()
        {
            var o = new HeapOptimizer();

            var t1 = new Tuple<string, int>(new StringBuilder().Append("Bart").ToString(), 21);
            var t2 = new Tuple<string, int>(new StringBuilder().Append("Bart").ToString(), 21);
            var t3 = new Tuple<string, int>(null, 21);

            var objs = new[] { t1, t2, t3 };

            o.Walk(objs, _ => true);

            Assert.AreSame(t1.Item1, t2.Item1);
        }

        [TestMethod]
        public void HeapOptimizer_Custom()
        {
            var o = new MyHeapOptimizer();

            var t1 = new Tuple<string, int>(new StringBuilder().Append("Bart").ToString(), 21);
            var t2 = new Tuple<string, int>(new StringBuilder().Append("Bart").ToString(), 21);
            var t3 = new Tuple<string, int>(null, 21);

            var objs = new[] { t1, t2, t3 };

            o.Walk(objs, _ => true);

            Assert.AreSame(objs[0], objs[1]);
        }

        private sealed class MyHeapOptimizer : HeapOptimizer
        {
            protected override bool IsImmutable(Type type) => base.IsImmutable(type) || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Tuple<,>));
        }
    }
}
