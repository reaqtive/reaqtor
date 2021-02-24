// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   PS - 10/17/2014 - Wrote these tests.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;

#if !NET5_0
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
#endif

namespace Tests.System.Collections.Specialized
{
    [TestClass]
    public class EnumDictionaryTests
    {
        [TestMethod]
        public void EnumDictionary_ManOrBoy()
        {
            IDictionary<ExpressionType, bool> dictionary;

            var values = Enum.GetValues(typeof(ExpressionType)).Cast<ExpressionType>().ToArray();

            foreach (var setValue in values)
            {
                dictionary = EnumDictionary.Create<ExpressionType, bool>();
                dictionary[setValue] = true;

                foreach (var testValue in values)
                {
                    Assert.AreEqual(testValue == setValue, dictionary.TryGetValue(testValue, out _));
                }
            }
        }

        [TestMethod]
        public void EnumDictionary_Bounds_Checks()
        {
            var enumDictionary = EnumDictionary.Create<ExpressionType, bool>();
            var values = Enum.GetValues(typeof(ExpressionType)).Cast<ExpressionType>().ToArray();

            try
            {
                enumDictionary.Add((ExpressionType)(-1), true);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException e)
            {
                Assert.AreEqual("key", e.ParamName);
            }

            try
            {
                enumDictionary.Add((ExpressionType)(values.Length), true);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException e)
            {
                Assert.AreEqual("key", e.ParamName);
            }

            try
            {
                enumDictionary[(ExpressionType)(-1)] = true;
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException e)
            {
                Assert.AreEqual("key", e.ParamName);
            }

            try
            {
                enumDictionary[(ExpressionType)(values.Length)] = true;
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException e)
            {
                Assert.AreEqual("key", e.ParamName);
            }

            try
            {
                var dummy = enumDictionary[(ExpressionType)(-1)];
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException e)
            {
                Assert.AreEqual("key", e.ParamName);
            }

            try
            {
                var dummy = enumDictionary[(ExpressionType)(values.Length)];
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException e)
            {
                Assert.AreEqual("key", e.ParamName);
            }

            try
            {
                enumDictionary.ContainsKey((ExpressionType)(-1));
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException e)
            {
                Assert.AreEqual("key", e.ParamName);
            }

            try
            {
                enumDictionary.ContainsKey((ExpressionType)(values.Length));
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException e)
            {
                Assert.AreEqual("key", e.ParamName);
            }

            try
            {
                enumDictionary.Contains(new KeyValuePair<ExpressionType, bool>((ExpressionType)(-1), true));
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException e)
            {
                Assert.AreEqual("item", e.ParamName);
            }

            try
            {
                enumDictionary.Contains(new KeyValuePair<ExpressionType, bool>((ExpressionType)(values.Length), true));
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException e)
            {
                Assert.AreEqual("item", e.ParamName);
            }

            try
            {
                enumDictionary.Remove((ExpressionType)(-1));
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException e)
            {
                Assert.AreEqual("key", e.ParamName);
            }

            try
            {
                enumDictionary.Remove((ExpressionType)(values.Length));
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException e)
            {
                Assert.AreEqual("key", e.ParamName);
            }

            try
            {
                enumDictionary.Remove(new KeyValuePair<ExpressionType, bool>((ExpressionType)(-1), true));
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException e)
            {
                Assert.AreEqual("item", e.ParamName);
            }

            try
            {
                enumDictionary.Remove(new KeyValuePair<ExpressionType, bool>((ExpressionType)(values.Length), true));
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException e)
            {
                Assert.AreEqual("item", e.ParamName);
            }

            try
            {
                enumDictionary.CopyTo(array: null, -1);
                Assert.Fail();
            }
            catch (ArgumentNullException e)
            {
                Assert.AreEqual("array", e.ParamName);
            }

            try
            {
                enumDictionary.CopyTo(new KeyValuePair<ExpressionType, bool>[1], -1);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException e)
            {
                Assert.AreEqual("index", e.ParamName);
            }

            enumDictionary = EnumDictionary.Create<ExpressionType, bool>();

            // This is valid because although the index is past the last array slot,
            // there are zero elements. This is the same behavior as the CopyTo in
            // System.Collections.Generic.Dictionary<TKey, TValue>
            enumDictionary.CopyTo(new KeyValuePair<ExpressionType, bool>[1], 1);

            try
            {
                enumDictionary.CopyTo(new KeyValuePair<ExpressionType, bool>[1], 3);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException e)
            {
                Assert.AreEqual("index", e.ParamName);
            }

            ExpressionType val0 = 0;
            ExpressionType val1 = (ExpressionType)1;

            enumDictionary.Add(val0, true);
            enumDictionary.Add(val1, true);
            try
            {
                enumDictionary.CopyTo(new KeyValuePair<ExpressionType, bool>[1], 0);
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual(typeof(ArgumentException), e.GetType());
            }
        }

        [TestMethod]
        public void EnumDictionary_Values()
        {
            var enumDictionary = EnumDictionary.Create<ExpressionType, int>();
            var values = Enum.GetValues(typeof(ExpressionType)).Cast<ExpressionType>().ToArray();

            foreach (var val in values.Where(e => ((int)e) % 2 == 0))
            {
                enumDictionary.Add(val, (int)val);
            }

            var dictValues = enumDictionary.Values.ToArray();
            foreach (var value in values.Cast<int>())
            {
                Assert.AreEqual(value % 2 == 0, dictValues.Contains(value));
            }
        }

        [TestMethod]
        public void EnumDictionary_Parameter_Validation()
        {
            try
            {
                _ = new EnumDictionary<int, bool, BitArraySlim<ByteArray1>>(default);
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual(e.ParamName, "TKey");
            }

            try
            {
                _ = new EnumDictionary<Foo, bool, BitArraySlim<ByteArray1>>(default);
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual(e.ParamName, "TKey");
            }

            try
            {
                _ = new EnumDictionary<Bar, bool, BitArraySlim<ByteArray1>>(default);
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual(e.ParamName, "TKey");
            }

            try
            {
                _ = new EnumDictionary<Baz, bool, BitArraySlim<ByteArray1>>(default);
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual(e.ParamName, "TKey");
            }

            try
            {
                _ = new EnumDictionary<ExpressionType, bool, BitArraySlim<ByteArray1>>(default);
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual(e.ParamName, "bitArray");
            }
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

        [TestMethod]
        public void EnumDictionary_Clear()
        {
            var enumDictionary = EnumDictionary.Create<ExpressionType, int>();
            var values = Enum.GetValues(typeof(ExpressionType)).Cast<ExpressionType>().ToArray();

            foreach (var val in values.Where(e => ((int)e) % 2 == 0))
            {
                enumDictionary.Add(val, (int)val);
            }

            enumDictionary.Clear();

            Assert.AreEqual(0, enumDictionary.Count);
            Assert.AreEqual(false, enumDictionary.Keys.Any());
            Assert.AreEqual(false, enumDictionary.Values.Any());
            Assert.AreEqual(false, enumDictionary.Any());
            Assert.AreEqual(false, enumDictionary.Keys.Any());
        }

        [TestMethod]
        public void EnumDictionary_Enumeration()
        {
            const int TRIALS = 100000;
            var rand = new Random(0);

            var enumDictionary = EnumDictionary.Create<ExpressionType, bool>();
            var systemDictionary = new Dictionary<ExpressionType, bool>();

            var values = Enum.GetValues(typeof(ExpressionType)).Cast<ExpressionType>().ToArray();

            for (var i = 0; i < TRIALS; i++)
            {
                var key = (ExpressionType)rand.Next(values.Length);

                if (systemDictionary.ContainsKey(key))
                {
                    if (rand.Next(2) == 0)
                    {
                        systemDictionary[key] ^= true;
                        enumDictionary[key] ^= true;
                    }
                    else
                    {
                        systemDictionary.Remove(key);
                        enumDictionary.Remove(key);
                    }
                }
                else
                {
                    systemDictionary.Add(key, true);
                    enumDictionary.Add(key, true);
                }

                var count = 0;
                foreach (var kvp in enumDictionary)
                {
                    count++;
                    Assert.IsTrue(systemDictionary.Contains(kvp));
                }

                Assert.AreEqual(systemDictionary.Count, count);

                count = 0;
                foreach (var kvp in (IEnumerable)enumDictionary)
                {
                    count++;
                    Assert.IsTrue(kvp is KeyValuePair<ExpressionType, bool>);
                    Assert.IsTrue(systemDictionary.Contains((KeyValuePair<ExpressionType, bool>)kvp));
                }

                Assert.AreEqual(systemDictionary.Count, count);
            }
        }

        [TestMethod]
        public void EnumDictionary_DuplicateAdd()
        {
            var enumDictionary = EnumDictionary.Create<ExpressionType, bool>();
            enumDictionary.Add(ExpressionType.Add, true);

            try
            {
                enumDictionary.Add(ExpressionType.Add, true);
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual(typeof(ArgumentException), e.GetType());
            }
        }

        [TestMethod]
        public void EnumDictionary_DuplicateRemove()
        {
            var enumDictionary = EnumDictionary.Create<ExpressionType, bool>();
            enumDictionary.Add(ExpressionType.Add, true);

            Assert.IsFalse(enumDictionary.Remove(new KeyValuePair<ExpressionType, bool>(ExpressionType.Add, false)));

            Assert.IsTrue(enumDictionary.Remove(new KeyValuePair<ExpressionType, bool>(ExpressionType.Add, true)));

            Assert.IsFalse(enumDictionary.Remove(new KeyValuePair<ExpressionType, bool>(ExpressionType.Add, true)));
        }

        [TestMethod]
        public void EnumDictionary_KeyNotFound()
        {
            var enumDictionary = EnumDictionary.Create<ExpressionType, bool>();
            var systemDictionary = new Dictionary<ExpressionType, bool>();

            var res = LambdaAssert(
                () => enumDictionary[ExpressionType.Add],
                () => systemDictionary[ExpressionType.Add]);
            Assert.IsNull(res);
        }

        [TestMethod]
        public void EnumDictionary_Keys_Test()
        {
            const int TRIALS = 100000;
            var rand = new Random(0);

            var enumDictionary = EnumDictionary.Create<ExpressionType, bool>();
            var systemDictionary = new Dictionary<ExpressionType, bool>();

            var values = Enum.GetValues(typeof(ExpressionType)).Cast<ExpressionType>().ToArray();

            for (var i = 0; i < TRIALS; i++)
            {
                var key = (ExpressionType)rand.Next(values.Length);

                if (systemDictionary.ContainsKey(key))
                {
                    if (rand.Next(2) == 0)
                    {
                        systemDictionary[key] ^= true;
                        enumDictionary[key] ^= true;
                    }
                    else
                    {
                        systemDictionary.Remove(key);
                        enumDictionary.Remove(key);
                    }
                }
                else
                {
                    systemDictionary[key] = true;
                    enumDictionary[key] = true;
                }

                var count = 0;
                foreach (var k in enumDictionary.Keys)
                {
                    count++;
                    Assert.IsTrue(systemDictionary.ContainsKey(k));
                }

                Assert.AreEqual(systemDictionary.Count, count);
            }
        }

        [TestMethod]
        public void EnumDictionary_ContainsKey_Test()
        {
            const int TRIALS = 100000;
            var rand = new Random(0);

            var enumDictionary = EnumDictionary.Create<ExpressionType, bool>();
            var systemDictionary = new Dictionary<ExpressionType, bool>();

            var values = Enum.GetValues(typeof(ExpressionType)).Cast<ExpressionType>().ToArray();

            for (var i = 0; i < TRIALS; i++)
            {
                var key = (ExpressionType)rand.Next(values.Length);

                if (systemDictionary.ContainsKey(key))
                {
                    if (rand.Next(2) == 0)
                    {
                        systemDictionary[key] ^= true;
                        enumDictionary[key] ^= true;
                    }
                    else
                    {
                        systemDictionary.Remove(key);
                        enumDictionary.Remove(key);
                    }
                }
                else
                {
                    systemDictionary[key] = true;
                    enumDictionary[key] = true;
                }

                var count = 0;
                foreach (var k in systemDictionary.Keys)
                {
                    count++;
                    Assert.IsTrue(enumDictionary.ContainsKey(k));
                }

                Assert.AreEqual(systemDictionary.Count, count);
            }
        }

        [TestMethod]
        public void EnumDictionary_Contains()
        {
            const int TRIALS = 100000;
            var rand = new Random(0);

            var enumDictionary = EnumDictionary.Create<ExpressionType, bool>();
            var systemDictionary = new Dictionary<ExpressionType, bool>();

            var values = Enum.GetValues(typeof(ExpressionType)).Cast<ExpressionType>().ToArray();

            for (var i = 0; i < TRIALS; i++)
            {
                var key = (ExpressionType)rand.Next(values.Length);

                if (systemDictionary.ContainsKey(key))
                {
                    if (rand.Next(2) == 0)
                    {
                        systemDictionary[key] ^= true;
                        enumDictionary[key] ^= true;
                    }
                    else
                    {
                        systemDictionary.Remove(key);
                        enumDictionary.Remove(key);
                    }
                }
                else
                {
                    systemDictionary[key] = true;
                    enumDictionary[key] = true;
                }

                var count = 0;
                foreach (var kvp in systemDictionary)
                {
                    count++;
                    Assert.IsTrue(enumDictionary.Contains(kvp));
                    Assert.IsFalse(enumDictionary.Contains(new KeyValuePair<ExpressionType, bool>(kvp.Key, !kvp.Value)));
                }

                foreach (var value in values.Where(val => !systemDictionary.ContainsKey(val)))
                {
                    Assert.IsFalse(enumDictionary.Contains(new KeyValuePair<ExpressionType, bool>(value, true)));
                    Assert.IsFalse(enumDictionary.Contains(new KeyValuePair<ExpressionType, bool>(value, false)));
                }

                Assert.AreEqual(systemDictionary.Count, count);
            }
        }

        [TestMethod]
        public void EnumDictionary_Random_Add_Remove()
        {
            const int TRIALS = 100000;
            var rand = new Random(0);

            var enumDictionary = EnumDictionary.Create<ExpressionType, bool>();
            var systemDictionary = new Dictionary<ExpressionType, bool>();
            var addedItems = new List<ExpressionType>();

            var values = Enum.GetValues(typeof(ExpressionType)).Cast<ExpressionType>().ToArray();

            for (var i = 0; i < TRIALS; i++)
            {
                var op = rand.Next(2);
                if (op == 0 || addedItems.Count == 0)
                {
                    var index = (ExpressionType)(rand.Next(values.Length));
                    var value = (rand.Next(2)) == 1;

                    if (!systemDictionary.ContainsKey(index))
                    {
                        addedItems.Add(index);
                        var res = LambdaAssert(() => systemDictionary[index] = value, () => enumDictionary[index] = value);
                        Assert.AreEqual(res.Item1, res.Item2);
                    }
                }
                else
                {
                    var itemToRemoveIndex = rand.Next(addedItems.Count);
                    var index = addedItems[itemToRemoveIndex];
#pragma warning disable IDE0079 // Remove unnecessary suppression (only on .NET 5)
#pragma warning disable IDE0056 // Indexing can be simplified (only works with System.Index)
                    addedItems[itemToRemoveIndex] = addedItems[addedItems.Count - 1];
#pragma warning restore IDE0056
#pragma warning restore IDE0079
                    addedItems.RemoveAt(addedItems.Count - 1);

                    var res = LambdaAssert(() => systemDictionary.Remove(index), () => enumDictionary.Remove(index));
                    Assert.AreEqual(res.Item1, res.Item2);
                }

                DictionaryAssertAreEqual(systemDictionary, enumDictionary);
            }
        }

        [TestMethod]
        public void EnumDictionary_Random_Add_Remove_KeyValuePair()
        {
            const int TRIALS = 100000;
            var rand = new Random(0);

            var enumDictionary = EnumDictionary.Create<ExpressionType, bool>();
            var systemDictionary = new Dictionary<ExpressionType, bool>();
            var addedItems = new List<ExpressionType>();

            var values = Enum.GetValues(typeof(ExpressionType)).Cast<ExpressionType>().ToArray();

            for (var i = 0; i < TRIALS; i++)
            {
                var op = rand.Next(2);
                if (op == 0 || addedItems.Count == 0)
                {
                    var index = (ExpressionType)(rand.Next(values.Length));
                    var value = (rand.Next(2)) == 1;

                    if (!systemDictionary.ContainsKey(index))
                    {
                        addedItems.Add(index);

                        var toAdd = new KeyValuePair<ExpressionType, bool>(index, value);

                        LambdaAssert(
#pragma warning disable IDE0004 // Remove Unnecessary Cast (clarity of interface used)
                            () => ((ICollection<KeyValuePair<ExpressionType, bool>>)enumDictionary).Add(toAdd),
                            () => ((ICollection<KeyValuePair<ExpressionType, bool>>)systemDictionary).Add(toAdd));
#pragma warning restore IDE0004
                    }
                }
                else
                {
                    var itemToRemoveIndex = rand.Next(addedItems.Count);
                    var index = addedItems[itemToRemoveIndex];
#pragma warning disable IDE0079 // Remove unnecessary suppression (only on .NET 5)
#pragma warning disable IDE0056 // Indexing can be simplified (only works with System.Index)
                    addedItems[itemToRemoveIndex] = addedItems[addedItems.Count - 1];
#pragma warning restore IDE0056
#pragma warning restore IDE0079
                    addedItems.RemoveAt(addedItems.Count - 1);

                    var toRemove = new KeyValuePair<ExpressionType, bool>(index, systemDictionary[index]);

                    Tuple<bool, bool> ret;
                    if (null != (ret = LambdaAssert(
#pragma warning disable IDE0004 // Remove Unnecessary Cast (clarity of interface used)
                        () => ((ICollection<KeyValuePair<ExpressionType, bool>>)enumDictionary).Remove(toRemove),
                        () => ((ICollection<KeyValuePair<ExpressionType, bool>>)systemDictionary).Remove(toRemove))))
#pragma warning restore IDE0004
                    {
                        Assert.AreEqual(ret.Item1, ret.Item2);
                    }
                }

                DictionaryAssertAreEqual(systemDictionary, enumDictionary);
            }
        }

        [TestMethod]
        public void EnumResolution_NonIntTypes()
        {
            var EnumByteDictionary = EnumDictionary.Create<EnumByte, bool>();
            EnumByteDictionary.Add(EnumByte.A, true);
            EnumByteDictionary.Add(EnumByte.Z, true);
            EnumByteDictionary[EnumByte.A] |= true;
            EnumByteDictionary[EnumByte.Z] |= true;
            EnumByteDictionary.CopyTo(new KeyValuePair<EnumByte, bool>[2], 0);
            _ = EnumByteDictionary.Last();
            Assert.IsTrue(EnumByteDictionary.Remove(EnumByte.A));
            Assert.IsTrue(EnumByteDictionary.Remove(EnumByte.Z));
            EnumByteDictionary.Add(new KeyValuePair<EnumByte, bool>(EnumByte.A, true));
            EnumByteDictionary.Add(new KeyValuePair<EnumByte, bool>(EnumByte.Z, true));
            Assert.IsTrue(EnumByteDictionary.Contains(new KeyValuePair<EnumByte, bool>(EnumByte.A, true)));
            Assert.IsTrue(EnumByteDictionary.Contains(new KeyValuePair<EnumByte, bool>(EnumByte.Z, true)));
            Assert.IsTrue(EnumByteDictionary.Remove(new KeyValuePair<EnumByte, bool>(EnumByte.A, true)));
            Assert.IsTrue(EnumByteDictionary.Remove(new KeyValuePair<EnumByte, bool>(EnumByte.Z, true)));

            var EnumShortDictionary = EnumDictionary.Create<EnumShort, bool>();
            EnumShortDictionary.Add(EnumShort.A, true);
            EnumShortDictionary.Add(EnumShort.Z, true);
            EnumShortDictionary[EnumShort.A] |= true;
            EnumShortDictionary[EnumShort.Z] |= true;
            EnumShortDictionary.CopyTo(new KeyValuePair<EnumShort, bool>[2], 0);
            _ = EnumShortDictionary.Last();
            Assert.IsTrue(EnumShortDictionary.Remove(EnumShort.A));
            Assert.IsTrue(EnumShortDictionary.Remove(EnumShort.Z));
            EnumShortDictionary.Add(new KeyValuePair<EnumShort, bool>(EnumShort.A, true));
            EnumShortDictionary.Add(new KeyValuePair<EnumShort, bool>(EnumShort.Z, true));
            Assert.IsTrue(EnumShortDictionary.Contains(new KeyValuePair<EnumShort, bool>(EnumShort.A, true)));
            Assert.IsTrue(EnumShortDictionary.Contains(new KeyValuePair<EnumShort, bool>(EnumShort.Z, true)));
            Assert.IsTrue(EnumShortDictionary.Remove(new KeyValuePair<EnumShort, bool>(EnumShort.A, true)));
            Assert.IsTrue(EnumShortDictionary.Remove(new KeyValuePair<EnumShort, bool>(EnumShort.Z, true)));

            var EnumUShortDictionary = EnumDictionary.Create<EnumUShort, bool>();
            EnumUShortDictionary.Add(EnumUShort.A, true);
            EnumUShortDictionary.Add(EnumUShort.Z, true);
            EnumUShortDictionary[EnumUShort.A] |= true;
            EnumUShortDictionary[EnumUShort.Z] |= true;
            EnumUShortDictionary.CopyTo(new KeyValuePair<EnumUShort, bool>[2], 0);
            _ = EnumUShortDictionary.Last();
            Assert.IsTrue(EnumUShortDictionary.Remove(EnumUShort.A));
            Assert.IsTrue(EnumUShortDictionary.Remove(EnumUShort.Z));
            EnumUShortDictionary.Add(new KeyValuePair<EnumUShort, bool>(EnumUShort.A, true));
            EnumUShortDictionary.Add(new KeyValuePair<EnumUShort, bool>(EnumUShort.Z, true));
            Assert.IsTrue(EnumUShortDictionary.Contains(new KeyValuePair<EnumUShort, bool>(EnumUShort.A, true)));
            Assert.IsTrue(EnumUShortDictionary.Contains(new KeyValuePair<EnumUShort, bool>(EnumUShort.Z, true)));
            Assert.IsTrue(EnumUShortDictionary.Remove(new KeyValuePair<EnumUShort, bool>(EnumUShort.A, true)));
            Assert.IsTrue(EnumUShortDictionary.Remove(new KeyValuePair<EnumUShort, bool>(EnumUShort.Z, true)));

            var EnumSByteDictionary = EnumDictionary.Create<EnumSByte, bool>();
            EnumSByteDictionary.Add(EnumSByte.A, true);
            EnumSByteDictionary.Add(EnumSByte.Z, true);
            EnumSByteDictionary[EnumSByte.A] |= true;
            EnumSByteDictionary[EnumSByte.Z] |= true;
            EnumSByteDictionary.CopyTo(new KeyValuePair<EnumSByte, bool>[2], 0);
            _ = EnumSByteDictionary.Last();
            Assert.IsTrue(EnumSByteDictionary.Remove(EnumSByte.A));
            Assert.IsTrue(EnumSByteDictionary.Remove(EnumSByte.Z));
            EnumSByteDictionary.Add(new KeyValuePair<EnumSByte, bool>(EnumSByte.A, true));
            EnumSByteDictionary.Add(new KeyValuePair<EnumSByte, bool>(EnumSByte.Z, true));
            Assert.IsTrue(EnumSByteDictionary.Contains(new KeyValuePair<EnumSByte, bool>(EnumSByte.A, true)));
            Assert.IsTrue(EnumSByteDictionary.Contains(new KeyValuePair<EnumSByte, bool>(EnumSByte.Z, true)));
            Assert.IsTrue(EnumSByteDictionary.Remove(new KeyValuePair<EnumSByte, bool>(EnumSByte.A, true)));
            Assert.IsTrue(EnumSByteDictionary.Remove(new KeyValuePair<EnumSByte, bool>(EnumSByte.Z, true)));
        }

        private enum EnumByte : byte
        {
            A = 0,
            Z = byte.MaxValue,
        }

        private enum EnumShort : short
        {
            A = 0,
            Z = short.MaxValue,
        }

        private enum EnumSByte : sbyte
        {
            A = 0,
            Z = sbyte.MaxValue,
        }

        private enum EnumUShort : ushort
        {
            A = 0,
            Z = ushort.MaxValue,
        }

#if !NET5_0 // https://aka.ms/binaryformatter
        [TestMethod]
        public void EnumSizeResolutionException_Serialization()
        {
            var ex = new EnumSizeResolutionException(EnumSizeResolutionError.UnderlyingTypeIsNotIntOrSmaller);
            var formatter = new BinaryFormatter();
            var stream = new MemoryStream();
            formatter.Serialize(stream, ex);
            stream.Seek(0, SeekOrigin.Begin);
            var exRoundTripped = (EnumSizeResolutionException)formatter.Deserialize(stream);

            Assert.AreEqual(exRoundTripped.ErrorCode, EnumSizeResolutionError.UnderlyingTypeIsNotIntOrSmaller);

            try
            {
                ex.GetObjectData(info: null, context: default);
                Assert.Fail();
            }
            catch (ArgumentNullException e)
            {
                Assert.AreEqual("info", e.ParamName);
            }
        }
#endif

        [TestMethod]
        public void EnumDictionary_ModificationDuringEnumeration()
        {
            var enumDictionary = EnumDictionary.Create<ExpressionType, bool>();
            enumDictionary[ExpressionType.Add] = true;
            enumDictionary[ExpressionType.AddAssign] = true;

            var enumerator = enumDictionary.GetEnumerator();
            enumerator.MoveNext();
            enumDictionary[ExpressionType.AddAssignChecked] = true;

            try
            {
                enumerator.MoveNext();
                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
            }

            enumerator = enumDictionary.GetEnumerator();
            enumerator.MoveNext();
            enumDictionary.Remove(ExpressionType.AddAssignChecked);

            try
            {
                enumerator.MoveNext();
                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
            }

            enumerator = enumDictionary.GetEnumerator();
            enumerator.MoveNext();
            enumDictionary.Add(ExpressionType.AddAssignChecked, true);

            try
            {
                enumerator.MoveNext();
                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
            }

            enumerator = enumDictionary.GetEnumerator();
            enumerator.MoveNext();
            enumDictionary.Remove(new KeyValuePair<ExpressionType, bool>(ExpressionType.AddAssignChecked, true));

            try
            {
                enumerator.MoveNext();
                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
            }

            enumerator = enumDictionary.GetEnumerator();
            enumerator.MoveNext();
            enumDictionary.Add(new KeyValuePair<ExpressionType, bool>(ExpressionType.AddAssignChecked, true));

            try
            {
                enumerator.MoveNext();
                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
            }

            enumerator = enumDictionary.GetEnumerator();
            enumerator.MoveNext();
            enumDictionary.Clear();

            try
            {
                enumerator.MoveNext();
                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
            }
        }

        private static void DictionaryAssertAreEqual<TKey, TValue>(IDictionary<TKey, TValue> expected, IDictionary<TKey, TValue> actual)
        {
            Assert.AreEqual(expected.Count, actual.Count);

            var expectedElements = expected.OrderBy(kvp => kvp.Key);
            var actualElements = actual.OrderBy(kvp => kvp.Key);
            foreach (var pair in expectedElements.Zip(actualElements, (e, a) => (e, a)))
            {
                Assert.AreEqual(pair.e.Key, pair.a.Key);
                Assert.AreEqual(pair.e.Value, pair.a.Value);
            }
        }

        private static void ExceptionAssertAreEqual(Exception e1, Exception e2)
        {
            Assert.AreEqual(e1 == null, e2 == null);
            if (e1 != null)
            {
                Assert.AreEqual(e1.GetType(), e2.GetType());

                var argException1 = e1 as ArgumentException;
                var argException2 = e2 as ArgumentException;
                Assert.AreEqual(argException1 == null, argException1 == null);

                if (argException1 != null)
                {
                    Assert.AreEqual(argException1.ParamName, argException2.ParamName);
                }
            }
        }

        [TestMethod]
        public void EnumDictionary_IsReadOnly()
        {
            Assert.IsFalse(EnumDictionary.Create<ExpressionType, bool>().IsReadOnly);
        }

        private static void LambdaAssert(Action expected, Action actual)
        {
            LambdaAssert(() => { expected(); return true; }, () => { actual(); return true; });
        }

        private static Tuple<T, T> LambdaAssert<T>(Func<T> expected, Func<T> actual)
        {
            Exception e1 = null;
            var expectedOutput = default(T);
            try
            {
                expectedOutput = expected();
            }
            catch (Exception ex)
            {
                e1 = ex;
            }

            Exception e2 = null;
            var actualOutput = default(T);
            try
            {
                actualOutput = actual();
            }
            catch (Exception ex)
            {
                e2 = ex;
            }

            ExceptionAssertAreEqual(e1, e2);
            return e1 == null ? Tuple.Create(expectedOutput, actualOutput) : null;
        }
    }
}
