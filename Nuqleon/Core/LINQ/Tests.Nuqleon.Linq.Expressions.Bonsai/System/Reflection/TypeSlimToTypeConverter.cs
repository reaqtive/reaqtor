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
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Tests.System.Linq.Expressions.Bonsai;

namespace Tests.System.Reflection
{
    [TestClass]
    public class TypeSlimToTypeConverterTests : TestBase
    {
        [TestMethod]
        public void TypeSlimToTypeConverter_ArgumentChecks()
        {
            var converter = new TypeSlimToTypeConverter(DefaultReflectionProvider.Instance);
            AssertEx.ThrowsException<ArgumentNullException>(() => converter.Visit(type: null), ex => Assert.AreEqual(ex.ParamName, "type"));
            AssertEx.ThrowsException<ArgumentNullException>(() => converter.MapType(typeSlim: null, type: null), ex => Assert.AreEqual(ex.ParamName, "typeSlim"));
            AssertEx.ThrowsException<ArgumentNullException>(() => converter.MapType(SlimType, type: null), ex => Assert.AreEqual(ex.ParamName, "type"));
        }

        [TestMethod]
        public void TypeSlimToTypeConverter_Resolve_Success()
        {
            var typeToSlim = new TypeToTypeSlimConverter();
            var visitor = new TypeSlimToTypeConverter(DefaultReflectionProvider.Instance);
            var longTypeSlim = typeToSlim.Visit(typeof(long));
            visitor.MapType(longTypeSlim, typeof(int));
            var longType = visitor.Visit(longTypeSlim);
            Assert.AreEqual(typeof(int), longType);
            visitor.MapType(longTypeSlim, typeof(int));
        }

        [TestMethod]
        public void TypeSlimToTypeConverter_Resolve_ThrowsInvalidOperation()
        {
            var visitor = new TypeSlimToTypeConverter(DefaultReflectionProvider.Instance);
            var intTypeSlim = typeof(int).ToTypeSlim();
            visitor.MapType(intTypeSlim, typeof(string));
            Assert.ThrowsException<InvalidOperationException>(() => visitor.MapType(intTypeSlim, typeof(long)));
        }


        [TestMethod]
        public void TypeSlimToTypeConverter_VisitGenericParameter_ThrowsInvalidOperation()
        {
            var visitor = new TypeSlimToTypeConverter(DefaultReflectionProvider.Instance);
            visitor.Push(new Dictionary<TypeSlim, Type>());
            var param = TypeSlim.GenericParameter("T");
            Assert.ThrowsException<InvalidOperationException>(() => visitor.Visit(param));
        }

        [TestMethod]
        public void TypeSlimToTypeConverter_VisitSimpleFake_ThrowsException()
        {
            var visitor = new TypeSlimToTypeConverter(DefaultReflectionProvider.Instance);
            var simple = TypeSlim.Simple(new AssemblySlim("MyFakeAssembly"), "MyFakeType");

            if (Type.GetType("Mono.Runtime") == null)
            {
                Assert.ThrowsException<FileNotFoundException>(() => visitor.Visit(simple));
            }
            else
            {
                Assert.ThrowsException<TypeLoadException>(() => visitor.Visit(simple));
            }
        }
    }
}
