// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq.CompilerServices;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class TypeVisitorTests
    {
        [TestMethod]
        public void TypeVisitor_ArgumentChecks()
        {
            var tv = new TypeVisitor();

            AssertEx.ThrowsException<ArgumentNullException>(() => tv.Visit(type: null), ex => Assert.AreEqual("type", ex.ParamName));

            var actv = new ArgumentChecksTypeVisitor();

            actv.RunTests();
        }

        private class ArgumentChecksTypeVisitor : TypeVisitor
        {
            public void RunTests()
            {
                AssertEx.ThrowsException<ArgumentNullException>(() => VisitArray(type: null), ex => Assert.AreEqual("type", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => VisitArrayMultidimensional(type: null), ex => Assert.AreEqual("type", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => VisitArrayVector(type: null), ex => Assert.AreEqual("type", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => VisitByRef(type: null), ex => Assert.AreEqual("type", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => VisitGeneric(type: null), ex => Assert.AreEqual("type", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => VisitGenericClosed(type: null), ex => Assert.AreEqual("type", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => VisitGenericParameter(type: null), ex => Assert.AreEqual("type", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => VisitGenericTypeDefinition(type: null), ex => Assert.AreEqual("type", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => VisitPointer(type: null), ex => Assert.AreEqual("type", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => VisitSimple(type: null), ex => Assert.AreEqual("type", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => Visit(default(Type[])), ex => Assert.AreEqual("types", ex.ParamName));
            }
        }

        [TestMethod]
        public void TypeVisitor_RoundtripTypes()
        {
            var tv = new TypeVisitor();

            foreach (var t in new[] {
                typeof(int),
                typeof(int[]), typeof(int[][]), typeof(int[,]), typeof(int).MakeArrayType(1),
                typeof(Func<int>), typeof(Func<Func<int>>), typeof(Func<>), typeof(Func<>).GetGenericArguments()[0],
                typeof(int*), typeof(int**),
                typeof(int).MakeByRefType(),
                new { a = 42 }.GetType(),
            })
            {
                Assert.AreSame(tv.Visit(t), t);
            }
        }

        [TestMethod]
        public void TypeVisitor_Custom_SubstituteSimple()
        {
            var subst = new SubstituteSimpleTypeVisitor(new Dictionary<Type, Type>
            {
                { typeof(int), typeof(long) }
            });

            Assert.AreEqual(typeof(string), subst.Visit(typeof(string)));

            Assert.AreEqual(typeof(long), subst.Visit(typeof(int)));
            Assert.AreEqual(typeof(long[]), subst.Visit(typeof(int[])));
            Assert.AreEqual(typeof(long[,]), subst.Visit(typeof(int[,])));
            Assert.AreEqual(typeof(long), subst.Visit(typeof(int)));
            Assert.AreEqual(typeof(Func<long>), subst.Visit(typeof(Func<int>)));
            Assert.AreEqual(typeof(Func<bool, Func<object, long[]>, double>), subst.Visit(typeof(Func<bool, Func<object, int[]>, double>)));
            Assert.AreEqual(typeof(long*), subst.Visit(typeof(int*)));
            Assert.AreEqual(typeof(long).MakeByRefType(), subst.Visit(typeof(int).MakeByRefType()));
        }

        private class SubstituteSimpleTypeVisitor : TypeVisitor
        {
            private readonly Dictionary<Type, Type> _subst;

            public SubstituteSimpleTypeVisitor(Dictionary<Type, Type> subst)
            {
                _subst = subst;
            }

            protected override Type VisitSimple(Type type)
            {
                if (_subst.TryGetValue(type, out var res))
                {
                    return res;
                }

                return base.VisitSimple(type);
            }
        }

        [TestMethod]
        public void TypeVisitor_Custom_SubstituteGeneric()
        {
            var subst = new SubstituteGenericTypeVisitor(new Dictionary<Type, Type>
            {
                { typeof(List<>), typeof(IEnumerable<>) }
            });

            Assert.AreEqual(typeof(IEnumerable<int>), subst.Visit(typeof(List<int>)));
            Assert.AreEqual(typeof(Func<string, IEnumerable<int>>), subst.Visit(typeof(Func<string, List<int>>)));
        }

        private class SubstituteGenericTypeVisitor : TypeVisitor
        {
            private readonly Dictionary<Type, Type> _subst;

            public SubstituteGenericTypeVisitor(Dictionary<Type, Type> subst)
            {
                _subst = subst;
            }

            protected override Type VisitGenericTypeDefinition(Type type)
            {
                if (_subst.TryGetValue(type, out var res))
                {
                    return res;
                }

                return base.VisitGenericTypeDefinition(type);
            }
        }
    }
}
