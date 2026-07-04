// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using System.Linq.CompilerServices;

namespace Tests.System.Linq.CompilerServices;

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
            var ex = Assert.ThrowsExactly<ArgumentNullException>(() => Visit(default(Type)));
            Assert.AreEqual("type", ex.ParamName);
            var ex2 = Assert.ThrowsExactly<ArgumentNullException>(() => VisitArray(type: null));
            Assert.AreEqual("type", ex2.ParamName);
            var ex3 = Assert.ThrowsExactly<ArgumentNullException>(() => VisitArrayMultidimensional(type: null));
            Assert.AreEqual("type", ex3.ParamName);
            var ex4 = Assert.ThrowsExactly<ArgumentNullException>(() => VisitArrayVector(type: null));
            Assert.AreEqual("type", ex4.ParamName);
            var ex5 = Assert.ThrowsExactly<ArgumentNullException>(() => VisitByRef(type: null));
            Assert.AreEqual("type", ex5.ParamName);
            var ex6 = Assert.ThrowsExactly<ArgumentNullException>(() => VisitGeneric(type: null));
            Assert.AreEqual("type", ex6.ParamName);
            var ex7 = Assert.ThrowsExactly<ArgumentNullException>(() => VisitGenericClosed(type: null));
            Assert.AreEqual("type", ex7.ParamName);
            var ex8 = Assert.ThrowsExactly<ArgumentNullException>(() => VisitPointer(type: null));
            Assert.AreEqual("type", ex8.ParamName);
            var ex9 = Assert.ThrowsExactly<ArgumentNullException>(() => Visit(default(IEnumerable<Type>)));
            Assert.AreEqual("types", ex9.ParamName);
            var ex10 = Assert.ThrowsExactly<ArgumentNullException>(() => Visit(default(Type[])));
            Assert.AreEqual("types", ex10.ParamName);
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
                var res = Visit([typeof(int), typeof(int[])]);
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
