// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - July 2013 - Created this file.
//

using System;
using System.IO;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Tests.System.Linq.Expressions.Bonsai;

namespace Tests.System.Reflection;

[TestClass]
public class TypeSlimToTypeConverterTests : TestBase
{
    [TestMethod]
    public void TypeSlimToTypeConverter_ArgumentChecks()
    {
        var converter = new TypeSlimToTypeConverter(DefaultReflectionProvider.Instance);
        var ex = Assert.ThrowsExactly<ArgumentNullException>(() => converter.Visit(type: null));
        Assert.AreEqual("type", ex.ParamName);
        var ex2 = Assert.ThrowsExactly<ArgumentNullException>(() => converter.MapType(typeSlim: null, type: null));
        Assert.AreEqual("typeSlim", ex2.ParamName);
        var ex3 = Assert.ThrowsExactly<ArgumentNullException>(() => converter.MapType(SlimType, type: null));
        Assert.AreEqual("type", ex3.ParamName);
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
        Assert.ThrowsExactly<InvalidOperationException>(() => visitor.MapType(intTypeSlim, typeof(long)));
    }


    [TestMethod]
    public void TypeSlimToTypeConverter_VisitGenericParameter_ThrowsInvalidOperation()
    {
        var visitor = new TypeSlimToTypeConverter(DefaultReflectionProvider.Instance);
        visitor.Push([]);
        var param = TypeSlim.GenericParameter("T");
        Assert.ThrowsExactly<InvalidOperationException>(() => visitor.Visit(param));
    }

    [TestMethod]
    public void TypeSlimToTypeConverter_VisitSimpleFake_ThrowsException()
    {
        var visitor = new TypeSlimToTypeConverter(DefaultReflectionProvider.Instance);
        var simple = TypeSlim.Simple(new AssemblySlim("MyFakeAssembly"), "MyFakeType");

        Assert.ThrowsExactly<FileNotFoundException>(() => visitor.Visit(simple));
    }
}
