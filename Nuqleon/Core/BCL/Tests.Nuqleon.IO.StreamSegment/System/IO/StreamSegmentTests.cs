// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using System;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.IO
{
    [TestClass]
    public class StreamSegmentTests
    {
        [TestMethod]
        public void StreamSegment_Ctor_ArgumentChecking()
        {
            var ms = new MemoryStream(new byte[5]);

            Assert.ThrowsException<ArgumentNullException>(() => _ = new StreamSegment(stream: null, 0, 5));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = new StreamSegment(ms, -1, 5));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = new StreamSegment(ms, 0, -1));
            Assert.ThrowsException<ArgumentException>(() => _ = new StreamSegment(ms, 0, 6));
            Assert.ThrowsException<ArgumentException>(() => _ = new StreamSegment(ms, 1, 5));
            Assert.ThrowsException<ArgumentException>(() => _ = new StreamSegment(ms, 4, 2));
            Assert.ThrowsException<ArgumentException>(() => _ = new StreamSegment(ms, 5, 1));

            for (long s = 0; s <= 4; s++)
            {
                for (long l = 0; l <= 5 - s; l++)
                {
                    Assert.IsNotNull(new StreamSegment(ms, s, l)); // shall not throw
                }
            }
        }

        [TestMethod]
        public void StreamSegment_Assembly()
        {
            Type type = typeof(StreamSegment);
            string assembly = type.Assembly.GetName().Name;
            Assert.AreEqual("Nuqleon.IO.StreamSegment", assembly);
        }

        private static Stream GetPrimeSegment()
        {
            return GetPrimeSegment(out _);
        }

        private static Stream GetPrimeSegment(out byte[] buffer)
        {
            var bs = new byte[] { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29 };

            var ms = new MemoryStream(bs);

            var ss = new StreamSegment(ms, 2, 7);

            buffer = bs;
            return ss;
        }

        [TestMethod]
        public void StreamSegment_Position_ArgumentChecking()
        {
            var ss = GetPrimeSegment();

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => ss.Position = -1);
            Assert.ThrowsException<OverflowException>(() => ss.Position = long.MaxValue);
        }

        [TestMethod]
        public void StreamSegment_PropertyInitialization()
        {
            var ss = GetPrimeSegment();

            Assert.AreEqual(0, ss.Position);
            Assert.AreEqual(7, ss.Length);

            Assert.IsTrue(ss.CanRead);
            Assert.IsTrue(ss.CanWrite);
            Assert.IsTrue(ss.CanSeek);
            Assert.IsFalse(ss.CanTimeout);
        }

        [TestMethod]
        public void StreamSegment_Read_ArgumentChecking()
        {
            var ss = GetPrimeSegment();

            var bf = new byte[3];

            Assert.ThrowsException<ArgumentNullException>(() => ss.Read(buffer: null, 0, 3));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => ss.Read(bf, -1, 3));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => ss.Read(bf, 0, -1));
            Assert.ThrowsException<ArgumentException>(() => ss.Read(bf, 0, 4));
        }

        [TestMethod]
        public void StreamSegment_Read_Sequential()
        {
            var ss = GetPrimeSegment();

            Assert.IsTrue(ss.CanRead);

            var bf = new byte[3];

            Assert.AreEqual(3, ss.Read(bf, 0, bf.Length));
            Assert.IsTrue(new byte[] { 5, 7, 11 }.SequenceEqual(bf));

            Array.Clear(bf, 0, bf.Length);

            Assert.AreEqual(3, ss.Read(bf, 0, bf.Length));
            Assert.IsTrue(new byte[] { 13, 17, 19 }.SequenceEqual(bf));

            Array.Clear(bf, 0, bf.Length);

            Assert.AreEqual(1, ss.Read(bf, 0, bf.Length));
            Assert.IsTrue(new byte[] { 23, 0, 0 }.SequenceEqual(bf));

            Array.Clear(bf, 0, bf.Length);

            Assert.AreEqual(0, ss.Read(bf, 0, bf.Length));
            Assert.IsTrue(new byte[] { 0, 0, 0 }.SequenceEqual(bf));

            var bn = new byte[] { 1, 2, 3 };
            Array.Copy(bn, bf, 3);

            Assert.AreEqual(0, ss.Read(bf, 0, bf.Length));
            Assert.IsTrue(bn.SequenceEqual(bf));
        }

        [TestMethod]
        public void StreamSegment_Read_Position()
        {
            var ss = GetPrimeSegment();

            Assert.IsTrue(ss.CanRead);
            Assert.IsTrue(ss.CanSeek);
            Assert.AreEqual(0, ss.Position);

            var bf = new byte[3];

            Assert.AreEqual(3, ss.Read(bf, 0, bf.Length));
            Assert.AreEqual(3, ss.Position);
            Assert.IsTrue(new byte[] { 5, 7, 11 }.SequenceEqual(bf));

            Array.Clear(bf, 0, bf.Length);

            Assert.AreEqual(3, ss.Read(bf, 0, bf.Length));
            Assert.AreEqual(6, ss.Position);
            Assert.IsTrue(new byte[] { 13, 17, 19 }.SequenceEqual(bf));

            Array.Clear(bf, 0, bf.Length);

            Assert.AreEqual(1, ss.Read(bf, 0, bf.Length));
            Assert.AreEqual(7, ss.Position);
            Assert.IsTrue(new byte[] { 23 }.SequenceEqual(bf.Take(1)));

            for (int i = 0; i < 2; i++)
            {
                ss.Position = 2;

                Array.Clear(bf, 0, bf.Length);

                Assert.AreEqual(3, ss.Read(bf, 0, bf.Length));
                Assert.AreEqual(5, ss.Position);
                Assert.IsTrue(new byte[] { 11, 13, 17 }.SequenceEqual(bf));
            }

            ss.Position = 99; // supported

            Array.Clear(bf, 0, bf.Length);

            Assert.AreEqual(0, ss.Read(bf, 0, bf.Length));
            Assert.AreEqual(99, ss.Position);
            Assert.IsTrue(new byte[] { 0, 0, 0 }.SequenceEqual(bf));
        }

        [TestMethod]
        public void StreamSegment_Write_ArgumentChecking()
        {
            var ss = GetPrimeSegment();

            var bf = new byte[3];

            Assert.ThrowsException<ArgumentNullException>(() => ss.Write(buffer: null, 0, 3));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => ss.Write(bf, -1, 3));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => ss.Write(bf, 0, -1));
            Assert.ThrowsException<ArgumentException>(() => ss.Write(bf, 0, 4));
        }

        [TestMethod]
        public void StreamSegment_Write_Sequential()
        {
            var ss = GetPrimeSegment(out var bs);

            Assert.IsTrue(ss.CanWrite);

            var bf = new byte[] { 1, 2, 3, 4, 5, 6, 7 };

            ss.Write(bf, 0, 3);
            Assert.IsTrue(bs.Skip(2).Take(3).SequenceEqual(bf.Skip(0).Take(3)));

            ss.Write(bf, 3, 3);
            Assert.IsTrue(bs.Skip(5).Take(3).SequenceEqual(bf.Skip(3).Take(3)));

            ss.Write(bf, 6, 1);
            Assert.IsTrue(bs.Skip(8).Take(1).SequenceEqual(bf.Skip(6).Take(1)));
        }

        [TestMethod]
        public void StreamSegment_Write_Position()
        {
            var ss = GetPrimeSegment(out var bs);

            Assert.IsTrue(ss.CanWrite);

            var bf = new byte[] { 1, 2, 3, 4, 5, 6, 7 };

            ss.Position = 2;

            ss.Write(bf, 0, 3);
            Assert.IsTrue(bs.Skip(4).Take(3).SequenceEqual(bf.Skip(0).Take(3)));
            Assert.AreEqual(5, ss.Position);

            ss.Position = 2;

            ss.Write(bf, 1, 4);
            Assert.IsTrue(bs.Skip(4).Take(4).SequenceEqual(bf.Skip(1).Take(4)));
            Assert.AreEqual(6, ss.Position);

            ss.Position = 6;

            ss.WriteByte(42);
            Assert.AreEqual(bs[8], 42);
            Assert.AreEqual(7, ss.Position);
        }

        [TestMethod]
        public void StreamSegment_Write_Position_Checks()
        {
            var ss = GetPrimeSegment(out var bs);

            Assert.IsTrue(ss.CanWrite);

            var ay = new ArraySegment<byte>(bs, 2, 7).ToArray();
            var bf = new byte[] { 1, 2, 3, 4, 5, 6, 7 };

            ss.Position = 6;

            Assert.ThrowsException<ArgumentException>(() => ss.Write(bf, 0, 2));
            Assert.AreEqual(6, ss.Position);
            Assert.IsTrue(ay.SequenceEqual(bs.Skip(2).Take(7)));

            ss.Position = 7;

            Assert.ThrowsException<ArgumentException>(() => ss.Write(bf, 0, 1));
            Assert.AreEqual(7, ss.Position);
            Assert.IsTrue(ay.SequenceEqual(bs.Skip(2).Take(7)));

            ss.Position = 99;

            Assert.ThrowsException<ArgumentException>(() => ss.Write(bf, 0, 3));
            Assert.AreEqual(99, ss.Position);
            Assert.IsTrue(ay.SequenceEqual(bs.Skip(2).Take(7)));
        }

        [TestMethod]
        public void StreamSegment_Seek_ArgumentChecking()
        {
            var ss = GetPrimeSegment();

            Assert.ThrowsException<ArgumentException>(() => ss.Seek(1L, (SeekOrigin)99));
        }

        [TestMethod]
        public void StreamSegment_Seek_Begin()
        {
            var ss = GetPrimeSegment();

            var bf = new byte[3];

            Assert.AreEqual(0, ss.Position);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => ss.Seek(-1, SeekOrigin.Begin));
            Assert.AreEqual(0, ss.Position);

            ss.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(0, ss.Position);

            Assert.AreEqual(3, ss.Read(bf, 0, bf.Length));
            Assert.IsTrue(new byte[] { 5, 7, 11 }.SequenceEqual(bf));
            Assert.AreEqual(3, ss.Position);

            ss.Seek(2, SeekOrigin.Begin);
            Assert.AreEqual(2, ss.Position);

            Assert.AreEqual(3, ss.Read(bf, 0, bf.Length));
            Assert.IsTrue(new byte[] { 11, 13, 17 }.SequenceEqual(bf));
            Assert.AreEqual(5, ss.Position);

            ss.Seek(6, SeekOrigin.Begin);
            Assert.AreEqual(6, ss.Position);

            Assert.AreEqual(1, ss.Read(bf, 0, bf.Length));
            Assert.IsTrue(new byte[] { 23 }.SequenceEqual(bf.Take(1)));
            Assert.AreEqual(7, ss.Position);

            ss.Seek(7, SeekOrigin.Begin);
            Assert.AreEqual(7, ss.Position);

            Assert.AreEqual(0, ss.Read(bf, 0, bf.Length));
            Assert.AreEqual(7, ss.Position);

            ss.Seek(99, SeekOrigin.Begin);
            Assert.AreEqual(99, ss.Position);

            Assert.AreEqual(0, ss.Read(bf, 0, bf.Length));
            Assert.AreEqual(99, ss.Position);
        }

        [TestMethod]
        public void StreamSegment_Seek_Current()
        {
            var ss = GetPrimeSegment();

            var bf = new byte[3];

            Assert.AreEqual(0, ss.Position);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => ss.Seek(-1, SeekOrigin.Current));
            Assert.AreEqual(0, ss.Position);

            ss.Seek(0, SeekOrigin.Current);
            Assert.AreEqual(0, ss.Position);

            Assert.AreEqual(3, ss.Read(bf, 0, bf.Length));
            Assert.IsTrue(new byte[] { 5, 7, 11 }.SequenceEqual(bf));
            Assert.AreEqual(3, ss.Position);

            ss.Seek(2, SeekOrigin.Current);
            Assert.AreEqual(5, ss.Position);

            Assert.AreEqual(2, ss.Read(bf, 0, bf.Length));
            Assert.IsTrue(new byte[] { 19, 23 }.SequenceEqual(bf.Take(2)));
            Assert.AreEqual(7, ss.Position);

            ss.Seek(-5, SeekOrigin.Current);
            Assert.AreEqual(2, ss.Position);

            Assert.AreEqual(3, ss.Read(bf, 0, bf.Length));
            Assert.IsTrue(new byte[] { 11, 13, 17 }.SequenceEqual(bf));
            Assert.AreEqual(5, ss.Position);

            ss.Seek(-5, SeekOrigin.Current);
            Assert.AreEqual(0, ss.Position);
            ss.Seek(+5, SeekOrigin.Current);
            Assert.AreEqual(5, ss.Position);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => ss.Seek(-6, SeekOrigin.Current));
            Assert.AreEqual(5, ss.Position);

            ss.Seek(3, SeekOrigin.Current);
            Assert.AreEqual(8, ss.Position);

            Assert.AreEqual(0, ss.Read(bf, 0, bf.Length));
            Assert.AreEqual(8, ss.Position);

            ss.Seek(91, SeekOrigin.Current);
            Assert.AreEqual(99, ss.Position);

            Assert.AreEqual(0, ss.Read(bf, 0, bf.Length));
            Assert.AreEqual(99, ss.Position);
        }

        [TestMethod]
        public void StreamSegment_Seek_End()
        {
            var ss = GetPrimeSegment();

            var bf = new byte[3];

            Assert.AreEqual(0, ss.Position);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => ss.Seek(-8, SeekOrigin.End));
            Assert.AreEqual(0, ss.Position);

            ss.Seek(0, SeekOrigin.End);
            Assert.AreEqual(7, ss.Position);

            Assert.AreEqual(0, ss.Read(bf, 0, bf.Length));
            Assert.AreEqual(7, ss.Position);

            ss.Seek(-1, SeekOrigin.End);
            Assert.AreEqual(6, ss.Position);

            Assert.AreEqual(1, ss.Read(bf, 0, bf.Length));
            Assert.IsTrue(new byte[] { 23 }.SequenceEqual(bf.Take(1)));
            Assert.AreEqual(7, ss.Position);

            ss.Seek(-7, SeekOrigin.End);
            Assert.AreEqual(0, ss.Position);

            Assert.AreEqual(3, ss.Read(bf, 0, bf.Length));
            Assert.IsTrue(new byte[] { 5, 7, 11 }.SequenceEqual(bf));
            Assert.AreEqual(3, ss.Position);

            ss.Seek(1, SeekOrigin.End);
            Assert.AreEqual(8, ss.Position);

            Assert.AreEqual(0, ss.Read(bf, 0, bf.Length));
            Assert.AreEqual(8, ss.Position);

            ss.Seek(92, SeekOrigin.End);
            Assert.AreEqual(99, ss.Position);

            Assert.AreEqual(0, ss.Read(bf, 0, bf.Length));
            Assert.AreEqual(99, ss.Position);
        }

        [TestMethod]
        public void StreamSegment_SetLength()
        {
            var ss = GetPrimeSegment();

            var bf = new byte[3];

            Assert.AreEqual(0, ss.Position);
            Assert.AreEqual(7, ss.Length);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => ss.SetLength(-1));
            Assert.AreEqual(0, ss.Position);
            Assert.AreEqual(7, ss.Length);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => ss.SetLength(8));
            Assert.AreEqual(0, ss.Position);
            Assert.AreEqual(7, ss.Length);

            ss.SetLength(7);
            Assert.AreEqual(0, ss.Position);
            Assert.AreEqual(7, ss.Length);

            ss.SetLength(5);
            Assert.AreEqual(0, ss.Position);
            Assert.AreEqual(5, ss.Length);

            Assert.AreEqual(3, ss.Read(bf, 0, bf.Length));
            Assert.IsTrue(new byte[] { 5, 7, 11 }.SequenceEqual(bf));
            Assert.AreEqual(3, ss.Position);

            Assert.AreEqual(2, ss.Read(bf, 0, bf.Length));
            Assert.IsTrue(new byte[] { 13, 17 }.SequenceEqual(bf.Take(2)));
            Assert.AreEqual(5, ss.Position);

            ss.SetLength(4);
            Assert.AreEqual(5, ss.Position);
            Assert.AreEqual(4, ss.Length);

            Assert.AreEqual(0, ss.Read(bf, 0, bf.Length));
            Assert.AreEqual(5, ss.Position);

            ss.Position = 2;
            Assert.AreEqual(2, ss.Read(bf, 0, bf.Length));
            Assert.IsTrue(new byte[] { 11, 13 }.SequenceEqual(bf.Take(2)));
            Assert.AreEqual(4, ss.Position);
        }

        [TestMethod]
        public void StreamSegment_Flush()
        {
            var s = new S();
            var ss = new StreamSegment(s, 0, 8);
            ss.Flush();
            Assert.IsTrue(s.HasFlushed);
        }

        private sealed class S : Stream
        {
            public override bool CanRead => true;
            public override bool CanSeek => true;
            public override bool CanWrite => true;

            public bool HasFlushed;

            public override void Flush() => HasFlushed = true;

            public override long Length => 16;

            public override long Position { get; set; }

            public override int Read(byte[] buffer, int offset, int count) => throw new NotImplementedException();
            public override long Seek(long offset, SeekOrigin origin) => throw new NotImplementedException();
            public override void SetLength(long value) => throw new NotImplementedException();
            public override void Write(byte[] buffer, int offset, int count) => throw new NotImplementedException();
        }
    }
}
