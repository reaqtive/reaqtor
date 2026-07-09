// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   PS - 10/17/2014 - Wrote these tests.
//

using System.Collections.Specialized;

namespace Tests.System.Collections.Specialized;

[TestClass]
public partial class EnumDictionaryFactoryTests
{
    [TestMethod]
    public void EnumDictionaryFactory_Parameter_Validation()
    {
        Assert.AreEqual("TKey", Assert.ThrowsExactly<ArgumentException>(() => EnumDictionary.Create<int, bool>()).ParamName);

        Assert.AreEqual("TKey", Assert.ThrowsExactly<ArgumentException>(() => EnumDictionary.Create<Foo, bool>()).ParamName);

        Assert.AreEqual("TKey", Assert.ThrowsExactly<ArgumentException>(() => EnumDictionary.Create<Bar, bool>()).ParamName);

        Assert.AreEqual("TKey", Assert.ThrowsExactly<ArgumentException>(() => EnumDictionary.Create<Baz, bool>()).ParamName);
    }

    private enum Foo : long
    {
    }

    private enum Bar : int
    {
        Qux = -1
    }

    [Flags]
    private enum Baz
    {
    }
}
