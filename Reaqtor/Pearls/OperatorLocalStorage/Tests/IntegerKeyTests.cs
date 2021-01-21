// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using System;
using Reaqtive.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class IntegerKeyTests
    {
        [TestMethod]
        public void NullAlphabet()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new IntegerKey(default));
        }

        [TestMethod]
        public void EmptyAlphabet()
        {
            Assert.ThrowsException<ArgumentException>(() => new IntegerKey());
        }

        [TestMethod]
        public void OneLetterAlphabet()
        {
            Assert.ThrowsException<ArgumentException>(() => new IntegerKey('a'));
        }

        [TestMethod]
        public void ReservedAlphabetCharacter()
        {
            Assert.ThrowsException<ArgumentException>(() => new IntegerKey('a', 'b', '-'));
        }

        [TestMethod]
        public void NonDistinctAlphabet()
        {
            Assert.ThrowsException<ArgumentException>(() => new IntegerKey('a', 'b', 'a'));
        }

        [TestMethod]
        public void ParseNull()
        {
            var key = new IntegerKey('a', 'b');
            Assert.ThrowsException<ArgumentNullException>(() => key.Parse(default));
        }

        [TestMethod]
        public void ParseEmpty()
        {
            var key = new IntegerKey('a', 'b');
            Assert.ThrowsException<ArgumentException>(() => key.Parse(""));
        }

        [TestMethod]
        public void ParseIncompleteNegative()
        {
            var key = new IntegerKey('a', 'b');
            Assert.ThrowsException<ArgumentException>(() => key.Parse("-"));
        }

        [TestMethod]
        public void ParseNotInAlphabet()
        {
            var key = new IntegerKey('a', 'b');
            Assert.ThrowsException<ArgumentException>(() => key.Parse("abcab"));
        }

        [TestMethod]
        public void KeyToString_Binary()
        {
            var key = new IntegerKey('0', '1');

            for (var i = 0; i <= 1000; i++) // NB: We don't use 2-complement.
            {
                var s = key.ToString(i);
                Assert.AreEqual(Convert.ToString(i, 2), s);
            }
        }

        [TestMethod]
        public void StringToKey_Binary()
        {
            var key = new IntegerKey('0', '1');

            for (var i = 0; i <= 1000; i++) // NB: We don't use 2-complement.
            {
                var j = key.Parse(Convert.ToString(i, 2));
                Assert.AreEqual(i, j);
            }
        }

        [TestMethod]
        public void KeyToString_Octal()
        {
            var key = new IntegerKey('0', '1', '2', '3', '4', '5', '6', '7');

            for (var i = 0; i <= 1000; i++) // NB: We don't use 8-complement.
            {
                var s = key.ToString(i);
                Assert.AreEqual(Convert.ToString(i, 8), s);
            }
        }

        [TestMethod]
        public void StringToKey_Octal()
        {
            var key = new IntegerKey('0', '1', '2', '3', '4', '5', '6', '7');

            for (var i = 0; i <= 1000; i++) // NB: We don't use 8-complement.
            {
                var j = key.Parse(Convert.ToString(i, 8));
                Assert.AreEqual(i, j);
            }
        }

        [TestMethod]
        public void KeyToString_Decimal()
        {
            var key = new IntegerKey('0', '1', '2', '3', '4', '5', '6', '7', '8', '9');

            for (var i = -1000; i <= 1000; i++)
            {
                var s = key.ToString(i);
                Assert.AreEqual(i.ToString(), s);
            }
        }

        [TestMethod]
        public void StringToKey_Decimal()
        {
            var key = new IntegerKey('0', '1', '2', '3', '4', '5', '6', '7', '8', '9');

            for (var i = -1000; i <= 1000; i++)
            {
                var j = key.Parse(i.ToString());
                Assert.AreEqual(i, j);
            }
        }

        [TestMethod]
        public void KeyToString_Hexadecimal()
        {
            var key = new IntegerKey('0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f');

            for (var i = 0; i <= 1000; i++) // NB: We don't use 16-complement.
            {
                var s = key.ToString(i);
                Assert.AreEqual(Convert.ToString(i, 16), s);
            }
        }

        [TestMethod]
        public void StringToKey_Hexadecimal()
        {
            var key = new IntegerKey('0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f');

            for (var i = 0; i <= 1000; i++) // NB: We don't use 16-complement.
            {
                var j = key.Parse(Convert.ToString(i, 16));
                Assert.AreEqual(i, j);
            }
        }
    }
}
