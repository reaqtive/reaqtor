// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2014 - Created this file.
//

using System;

#if !NET8_0 // https://aka.ms/binaryformatter
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
#endif

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Nuqleon.DataModel.Serialization.Json;

namespace Tests.Nuqleon.DataModel.Serialization.Json
{
    [TestClass]
    public class DataSerializerExceptionTests
    {
        [TestMethod]
        public void DataSerializerException_Default()
        {
            var ex = new DataSerializerException();
            Assert.IsFalse(string.IsNullOrEmpty(ex.Message));
        }

        [TestMethod]
        public void DataSerializerException_Message()
        {
            var ex = new DataSerializerException("foo");
            Assert.AreEqual("foo", ex.Message);
        }

        [TestMethod]
        public void DataSerializerException_MessageInner()
        {
            var err = new Exception();

            var ex = new DataSerializerException("foo", err);
            Assert.AreEqual("foo", ex.Message);
            Assert.AreSame(err, ex.InnerException);
        }

#if !NET8_0 // https://aka.ms/binaryformatter
        [TestMethod]
        public void DataSerializerException_Serialize()
        {
            var ex = new DataSerializerException("foo");

            var bf = new BinaryFormatter();
            var ms = new MemoryStream();
            bf.Serialize(ms, ex);
            ms.Position = 0;
            var res = (DataSerializerException)bf.Deserialize(ms);

            Assert.AreEqual(ex.Message, res.Message);
        }
#endif
    }
}
