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
using System.Linq;
using System.Linq.CompilerServices;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class TypeVisitorGenericTests
    {
        [TestMethod]
        public void TypeVisitorGeneric_ArgumentChecks()
        {
            var actv = new ArgumentChecksGenericTypeVisitor();

            actv.RunTests();
        }

        private sealed class ArgumentChecksGenericTypeVisitor : TypeVisitor<bool>
        {
            public void RunTests()
            {
                AssertEx.ThrowsException<ArgumentNullException>(() => Visit(default(Type)), ex => Assert.AreEqual("type", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => VisitArray(type: null), ex => Assert.AreEqual("type", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => VisitArrayMultidimensional(type: null), ex => Assert.AreEqual("type", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => VisitArrayVector(type: null), ex => Assert.AreEqual("type", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => VisitByRef(type: null), ex => Assert.AreEqual("type", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => VisitGeneric(type: null), ex => Assert.AreEqual("type", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => VisitGenericClosed(type: null), ex => Assert.AreEqual("type", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => VisitPointer(type: null), ex => Assert.AreEqual("type", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => Visit(default(IEnumerable<Type>)), ex => Assert.AreEqual("types", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => Visit(default(Type[])), ex => Assert.AreEqual("types", ex.ParamName));
            }

            protected override bool MakeArrayType(Type type, bool elementType) => throw new NotImplementedException();

            protected override bool MakeArrayType(Type type, bool elementType, int rank) => throw new NotImplementedException();

            protected override bool VisitGenericParameter(Type type) => throw new NotImplementedException();

            protected override bool MakeGenericType(Type type, bool genericTypeDefinition, params bool[] genericArguments) => throw new NotImplementedException();

            protected override bool VisitGenericTypeDefinition(Type type) => throw new NotImplementedException();

            protected override bool MakeByRefType(Type type, bool elementType) => throw new NotImplementedException();

            protected override bool MakePointerType(Type type, bool elementType) => throw new NotImplementedException();

            protected override bool VisitSimple(Type type) => throw new NotImplementedException();
        }

        [TestMethod]
        public void TypeVisitorGeneric_FindUsage()
        {
            var futv = new FindUsageTypeVisitor(typeof(int));

            foreach (var t in new[]
            {
                typeof(int),
                typeof(int[]), typeof(int[][]), typeof(int[,]), typeof(int).MakeArrayType(1),
                typeof(int*), typeof(int**),
                typeof(Func<int>), typeof(Func<string, Func<int>>),
                typeof(int).MakeByRefType()
            })
            {
                Assert.IsTrue(futv.Visit(t));
            }

            foreach (var t in new[]
            {
                typeof(double),
                typeof(double[]), typeof(double[][]), typeof(double[,]), typeof(double).MakeArrayType(1),
                typeof(double*), typeof(double**),
                typeof(Func<double>), typeof(Func<string, Func<double>>),
                typeof(double).MakeByRefType()
            })
            {
                Assert.IsFalse(futv.Visit(t));
            }
        }

        private class FindUsageTypeVisitor : TypeVisitor<bool>
        {
            private readonly Type _type;

            public FindUsageTypeVisitor(Type type) => _type = type;

            protected override bool MakeArrayType(Type type, bool elementType) => elementType;

            protected override bool MakeArrayType(Type type, bool elementType, int rank) => elementType;

            protected override bool VisitGenericParameter(Type type) => type == _type;

            protected override bool MakeGenericType(Type type, bool genericTypeDefinition, params bool[] genericArguments) => genericTypeDefinition || genericArguments.Any(b => b);

            protected override bool VisitGenericTypeDefinition(Type type) => type == _type;

            protected override bool MakeByRefType(Type type, bool elementType) => elementType;

            protected override bool MakePointerType(Type type, bool elementType) => elementType;

            protected override bool VisitSimple(Type type) => type == _type;
        }

        [TestMethod]
        public void TypeVisitorGeneric_VisitTypes()
        {
            new TypeNameVisitor().Test();
        }

        private class TypeNameVisitor : TypeVisitor<string>
        {
            public void Test()
            {
                {
                    var res = Visit(new[] { typeof(int), typeof(int[]) });
                    Assert.IsTrue(new[] { "Int32", "Int32[]" }.SequenceEqual(res));
                }

                {
                    var res = Visit(new[] { typeof(int), typeof(int[]) }.AsEnumerable());
                    Assert.IsTrue(new[] { "Int32", "Int32[]" }.SequenceEqual(res));
                }
            }

            protected override string MakeArrayType(Type type, string elementType) => elementType + "[]";

            protected override string MakeArrayType(Type type, string elementType, int rank)
            {
                return elementType + "[" + new string(',', rank - 1) + "]";
            }

            protected override string VisitGenericParameter(Type type) => type.Name;

            protected override string MakeGenericType(Type type, string genericTypeDefinition, params string[] genericArguments)
            {
                return genericTypeDefinition + "<" + string.Join(", ", genericArguments) + ">";
            }

            protected override string VisitGenericTypeDefinition(Type type) => type.Name;

            protected override string MakeByRefType(Type type, string elementType) => elementType + "&";

            protected override string MakePointerType(Type type, string elementType) => elementType + "*";

            protected override string VisitSimple(Type type) => type.Name;
        }
    }
}
