// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - July 2013 - Created this file.
//

using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Tests.System.Linq.Expressions.Bonsai;

namespace System.Reflection
{
    [TestClass]
    public class TypeToTypeSlimConverterTests : TestBase
    {
        private new class Foo
        {
#pragma warning disable IDE0060 // Remove unused parameter
            public Foo(ref int bar)
            { }
#pragma warning restore IDE0060 // Remove unused parameter
        }

        [TestMethod]
        public void TypeToTypeSlimConverter_ArgumentChecks()
        {
            var converter = new TypeToTypeSlimConverter();
            var ex = Assert.ThrowsExactly<ArgumentNullException>(() => converter.Visit(type: null));
            Assert.AreEqual(ex.ParamName, "type");
            var ex2 = Assert.ThrowsExactly<ArgumentNullException>(() => converter.MapType(type: null, typeSlim: null));
            Assert.AreEqual(ex2.ParamName, "type");
            var ex3 = Assert.ThrowsExactly<ArgumentNullException>(() => converter.MapType(typeof(int), typeSlim: null));
            Assert.AreEqual(ex3.ParamName, "typeSlim");
        }

        [TestMethod]
        public void TypeToTypeSlimConverter_ThrowsNotSupported()
        {
            var refType = typeof(Foo).GetConstructors().Single().GetParameters()[0].ParameterType;
            Assert.IsTrue(refType.IsByRef);

            var pointerType = typeof(int*);
            Assert.IsTrue(pointerType.IsPointer);

            var converter = new TypeToTypeSlimConverter();
            Assert.ThrowsExactly<NotSupportedException>(() => converter.Visit(refType));
            Assert.ThrowsExactly<NotSupportedException>(() => converter.Visit(pointerType));
        }

        [TestMethod]
        public void TypeToTypeSlimConverter_Resolve_Success()
        {
            var visitor = new TypeToTypeSlimConverter();
            var longTypeSlim = visitor.Visit(typeof(long));
            visitor.MapType(typeof(int), longTypeSlim);
            var intTypeSlim = visitor.Visit(typeof(int));
            Assert.AreEqual(intTypeSlim, longTypeSlim);
            visitor.MapType(typeof(int), longTypeSlim);
        }

        [TestMethod]
        public void TypeToTypeSlimConverter_Resolve_ThrowsInvalidOperation()
        {
            var visitor = new TypeToTypeSlimConverter();
            var intTypeSlim = visitor.Visit(typeof(int));
            var longTypeSlim = visitor.Visit(typeof(long));
            Assert.ThrowsExactly<InvalidOperationException>(() => visitor.MapType(typeof(int), longTypeSlim));
        }

        [TestMethod]
        public void TypeToTypeSlimConverter_GenericParameter()
        {
            var genericParameterType = typeof(List<>).GetGenericArguments()[0];

            var visitor = new TypeToTypeSlimConverter();
            Assert.ThrowsExactly<InvalidOperationException>(() => visitor.Visit(genericParameterType));
        }
    }
}
