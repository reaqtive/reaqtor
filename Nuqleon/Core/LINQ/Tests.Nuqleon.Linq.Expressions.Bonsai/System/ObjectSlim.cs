// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - July 2013 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;
using Tests.System.Linq.Expressions.Bonsai;

namespace Tests.System
{
    [TestClass]
    public class ObjectSlimTests : TestBase
    {
        [TestMethod]
        public void ObjectSlim_NullArguments()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => ObjectSlim.Create(value: null, typeSlim: null, type: null), ex => Assert.AreEqual(ex.ParamName, "typeSlim"));
            AssertEx.ThrowsException<ArgumentNullException>(() => ObjectSlim.Create(value: null, SlimType, type: null), ex => Assert.AreEqual(ex.ParamName, "type"));
            AssertEx.ThrowsException<ArgumentNullException>(() => ObjectSlim.Create(liftedValue: null, typeSlim: null, reduceFactory: (Func<Type, Func<object, object>>)null), ex => Assert.AreEqual(ex.ParamName, "typeSlim"));
            AssertEx.ThrowsException<ArgumentNullException>(() => ObjectSlim.Create(liftedValue: null, SlimType, reduceFactory: (Func<Type, Func<object, object>>)null), ex => Assert.AreEqual(ex.ParamName, "reduceFactory"));
        }

        [TestMethod]
        public void ObjectSlim_Lift_Exceptions()
        {
            var os1 = ObjectSlim.Create(1, SlimType, typeof(int));
            var os2 = ObjectSlim.Create<string>("1", SlimType, t => o => o);
            AssertEx.ThrowsException<ArgumentNullException>(() => os1.Lift<string>(liftFactory: null), ex => Assert.AreEqual(ex.ParamName, "liftFactory"));
            Assert.ThrowsException<InvalidOperationException>(() => { var ot = os2.OriginalType; });
            Assert.ThrowsException<InvalidOperationException>(() => os2.Lift<string>(t => o => o.ToString()));
        }

        [TestMethod]
        public void ObjectSlim_Reduce_Exceptions()
        {
            var os1 = ObjectSlim.Create(1, SlimType, typeof(int));
            var os2 = ObjectSlim.Create("1", SlimType, t => o => o);
            AssertEx.ThrowsException<ArgumentNullException>(() => os2.Reduce(type: null), ex => Assert.AreEqual(ex.ParamName, "type"));
        }

        [TestMethod]
        public void ObjectSlim_CanLift()
        {
            var os1 = ObjectSlim.Create(1, SlimType, typeof(int));
            var os2 = ObjectSlim.Create("1", SlimType, _ => x => int.Parse(x));
            Assert.IsTrue(os1.CanLift);
            Assert.IsFalse(os2.CanLift);
            Assert.AreSame(typeof(int), os1.OriginalType);
            Assert.AreEqual(os1.Lift<string>(t => o => o.ToString()), "1");
        }

        [TestMethod]
        public void ObjectSlim_CanReduce()
        {
            static Func<string, object> rf(Type t) => o => int.Parse(o);
            var os1 = ObjectSlim.Create(1, SlimType, typeof(int));
            var os2 = ObjectSlim.Create("1", SlimType, rf);
            Assert.IsTrue(os2.CanReduce);
            Assert.IsFalse(os1.CanReduce);
            Assert.AreEqual(os2.Reduce(typeof(int)), 1);
        }

        [TestMethod]
        public void ObjectSlim_ToString()
        {
            var o = ObjectSlim.Create("foo", typeof(string).ToTypeSlim(), typeof(string));
            Assert.AreEqual("foo", o.ToString());

            var n = ObjectSlim.Create(value: null, typeof(string).ToTypeSlim(), typeof(string));
            Assert.AreEqual("null", n.ToString());
        }
    }
}
