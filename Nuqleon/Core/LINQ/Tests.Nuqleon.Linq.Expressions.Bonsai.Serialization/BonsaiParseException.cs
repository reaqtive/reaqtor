// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2016 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Linq.Expressions.Bonsai.Serialization;

using Json = Nuqleon.Json.Expressions;

#if !NET5_0 // https://aka.ms/binaryformatter
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
#endif

namespace Tests
{
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

#if !NET5_0 // https://aka.ms/binaryformatter
            var f = new BinaryFormatter();

            var ms = new MemoryStream();
            f.Serialize(ms, ex);

            ms.Position = 0;
            var res = (BonsaiParseException)f.Deserialize(ms);

            Assert.AreEqual("foo", res.Message);
            Assert.IsTrue(res.Node is Json.ConstantExpression);
            Assert.AreEqual(json.ToString(), res.Node.ToString());
#endif
        }
    }
}
