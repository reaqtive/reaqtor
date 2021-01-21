// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.IO;
using System.Linq;

using Reaqtor.QueryEngine;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor.QueryEngine
{
    [TestClass]
    public class ReaderBaseTests
    {
        [TestMethod]
        public void ReaderBase_ReadHeader_InvalidDataExceptions()
        {
            var stream = new MemoryStream();
            var writer = new WriterBase(stream, SerializationPolicy.Default);
            writer.WriteHeader();
            var bytes = stream.ToArray();

            // AD 1.0.0.0 ...
            bytes[0] = b('A');
            Assert.ThrowsException<InvalidDataException>(() => new ReaderBase(new MemoryStream(bytes), SerializationPolicy.Default).ReadHeader());
            bytes[0] = b('B');

            // BD 0.9.0.0 ...
            bytes[2] = 0;
            bytes[6] = 9;
            Assert.ThrowsException<InvalidDataException>(() => new ReaderBase(new MemoryStream(bytes), SerializationPolicy.Default).ReadHeader());
            bytes[2] = 1;
            bytes[6] = 0;
        }

        [TestMethod]
        public void ReaderBase_ReadHeader_MissingHeaderExceptions()
        {
            var stream = new MemoryStream();
            var writer = new WriterBase(stream, SerializationPolicy.Default);
            writer.WriteHeader();
            var bytes = stream.ToArray();

            // B 
            Assert.ThrowsException<InvalidDataException>(() => new ReaderBase(new MemoryStream(bytes.Take(1).ToArray()), SerializationPolicy.Default).ReadHeader());

            // BD 1.0 
            Assert.ThrowsException<InvalidDataException>(() => new ReaderBase(new MemoryStream(bytes.Take(10).ToArray()), SerializationPolicy.Default).ReadHeader());

            // BD 1.0.0.0 
            Assert.ThrowsException<InvalidDataException>(() => new ReaderBase(new MemoryStream(bytes.Take(18).ToArray()), SerializationPolicy.Default).ReadHeader());

            // BD 1.0.0.0 0 1.0 
            Assert.ThrowsException<InvalidDataException>(() => new ReaderBase(new MemoryStream(bytes.Take(30).ToArray()), SerializationPolicy.Default).ReadHeader());
        }

        [TestMethod]
        public void ReaderBase_ReadFooter_InvalidDataExceptions()
        {
            var stream = new MemoryStream();
            var writer = new WriterBase(stream, SerializationPolicy.Default);
            writer.Dispose();
            var bytes = stream.ToArray();

            bytes[0] = 0xDA;
            Assert.ThrowsException<InvalidDataException>(() => new ReaderBase(new MemoryStream(bytes), SerializationPolicy.Default).ReadFooter());
            bytes[0] = 0xDE;

            bytes[1] = 0xAE;
            Assert.ThrowsException<InvalidDataException>(() => new ReaderBase(new MemoryStream(bytes), SerializationPolicy.Default).ReadFooter());
            bytes[1] = 0xAD;

            bytes[2] = 0xDA;
            Assert.ThrowsException<InvalidDataException>(() => new ReaderBase(new MemoryStream(bytes), SerializationPolicy.Default).ReadFooter());
            bytes[2] = 0xDE;

            bytes[3] = 0xAE;
            Assert.ThrowsException<InvalidDataException>(() => new ReaderBase(new MemoryStream(bytes), SerializationPolicy.Default).ReadFooter());
            bytes[3] = 0xAD;
        }

        private static byte b<T>(T value)
            where T : IConvertible
        {
            return value.ToByte(CultureInfo.InvariantCulture);
        }
    }
}
