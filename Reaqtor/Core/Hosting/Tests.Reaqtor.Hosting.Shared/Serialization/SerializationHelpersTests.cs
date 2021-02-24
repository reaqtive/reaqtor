// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtor.Hosting.Shared.Serialization;

namespace Tests.Microsoft.Hosting.Shared.Serialization
{
    [TestClass]
    public class SerializationHelpersTests
    {
        [TestMethod]
        public void SerializationHelpers_ArgumentChecks()
        {
            var serializationHelpers = new SerializationHelpers();
            AssertEx.ThrowsException<ArgumentNullException>(() => serializationHelpers.Serialize(default(object), null), ex => Assert.AreEqual("stream", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => serializationHelpers.Deserialize<object>(null), ex => Assert.AreEqual("stream", ex.ParamName));
        }
    }
}
