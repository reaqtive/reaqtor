// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtor.Hosting.Shared.Serialization;

namespace Tests.Microsoft.Hosting.Shared.Serialization;

[TestClass]
public class SerializationHelpersTests
{
    [TestMethod]
    public void SerializationHelpers_ArgumentChecks()
    {
        var serializationHelpers = new SerializationHelpers();
        var ex = Assert.ThrowsExactly<ArgumentNullException>(() => serializationHelpers.Serialize(default(object), null));
        Assert.AreEqual("stream", ex.ParamName);
        var ex2 = Assert.ThrowsExactly<ArgumentNullException>(() => serializationHelpers.Deserialize<object>(null));
        Assert.AreEqual("stream", ex2.ParamName);
    }
}
