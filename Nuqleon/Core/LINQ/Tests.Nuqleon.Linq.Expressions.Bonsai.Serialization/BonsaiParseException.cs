// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2016 - Created this file.
//

using System.Linq.Expressions.Bonsai.Serialization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Json = Nuqleon.Json.Expressions;


namespace Tests;

[TestClass]
public class BonsaiParseExceptionTests
{
    [TestMethod]
    public void X()
    {
        var json = Json.Expression.Boolean(true);

        var ex = new BonsaiParseException("foo", json);

        Assert.AreEqual("foo", ex.Message);
        Assert.AreSame(json, ex.Node);

    }
}
