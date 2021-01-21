// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2014 - Created this file.
//

using Nuqleon.Json.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Tests.Nuqleon.Json
{
    [TestClass]
    public class ParseExceptionTests
    {
#if !NET5_0 // https://aka.ms/binaryformatter
        [TestMethod]
        public void ParseException_Serialization()
        {
            var ex = new ParseException("foo", 42, ParseError.ObjectEmptyMember);

            var fmt = new BinaryFormatter();

            var ms = new MemoryStream();
            fmt.Serialize(ms, ex);

            ms.Position = 0;
            var er = (ParseException)fmt.Deserialize(ms);

            Assert.AreEqual(er.Position, ex.Position);
            Assert.AreEqual(er.Error, ex.Error);
            Assert.AreEqual(er.Message, ex.Message);
        }
#endif
    }
}
