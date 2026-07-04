// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Linq.CompilerServices;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Linq.CompilerServices;

[TestClass]
public class TypeVisitorTests
{
    [TestMethod]
    public void TypeVisitor_ArgumentChecks()
    {
        var tv = new TypeVisitor();

        var ex = Assert.ThrowsExactly<ArgumentNullException>(() => tv.Visit(type: null));
        Assert.AreEqual("type", ex.ParamName);

        var actv = new ArgumentChecksTypeVisitor();

        actv.RunTests();
    }

    private class ArgumentChecksTypeVisitor : TypeVisitor
    {
        public void RunTests()
        {
            var ex = Assert.ThrowsExactly<ArgumentNullException>(() => VisitArray(type: null));
            Assert.AreEqual("type", ex.ParamName);
            var ex2 = Assert.ThrowsExactly<ArgumentNullException>(() => VisitArrayMultidimensional(type: null));
            Assert.AreEqual("type", ex2.ParamName);
            var ex3 = Assert.ThrowsExactly<ArgumentNullException>(() => VisitArrayVector(type: null));
            Assert.AreEqual("type", ex3.ParamName);
            var ex4 = Assert.ThrowsExactly<ArgumentNullException>(() => VisitByRef(type: null));
            Assert.AreEqual("type", ex4.ParamName);
            var ex5 = Assert.ThrowsExactly<ArgumentNullException>(() => VisitGeneric(type: null));
            Assert.AreEqual("type", ex5.ParamName);
            var ex6 = Assert.ThrowsExactly<ArgumentNullException>(() => VisitGenericClosed(type: null));
            Assert.AreEqual("type", ex6.ParamName);
            var ex7 = Assert.ThrowsExactly<ArgumentNullException>(() => VisitGenericParameter(type: null));
            Assert.AreEqual("type", ex7.ParamName);
            var ex8 = Assert.ThrowsExactly<ArgumentNullException>(() => VisitGenericTypeDefinition(type: null));
            Assert.AreEqual("type", ex8.ParamName);
            var ex9 = Assert.ThrowsExactly<ArgumentNullException>(() => VisitPointer(type: null));
            Assert.AreEqual("type", ex9.ParamName);
            var ex10 = Assert.ThrowsExactly<ArgumentNullException>(() => VisitSimple(type: null));
            Assert.AreEqual("type", ex10.ParamName);
            var ex11 = Assert.ThrowsExactly<ArgumentNullException>(() => Visit(default(Type[])));
            Assert.AreEqual("types", ex11.ParamName);
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
