// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
//

#pragma warning disable IDE0050 // Convert to tuple. (Tests for anonymous types.)

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

using Nuqleon.DataModel;
using Nuqleon.DataModel.TypeSystem;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Nuqleon.DataModel.CompilerServices.TypeSystem
{
    [TestClass]
    public class DataTypeCheckerTests
    {
        [TestMethod]
        public void DataTypeChecker_CheckTypes_Success()
        {
            var anon = new { x = 1, b = new bool[] { true }, c = new List<int[]>() };
            var rec = RuntimeCompiler.CreateRecordType(new[] { new KeyValuePair<string, Type>("bar", typeof(int)) }, valueEquality: true);

            foreach (var t in new[]
            {
                typeof(Unit),
                typeof(sbyte),
                typeof(byte),
                typeof(short),
                typeof(ushort),
                typeof(int),
                typeof(uint),
                typeof(long),
                typeof(ulong),
                typeof(float),
                typeof(double),
                typeof(decimal),
                typeof(bool),
                typeof(char),
                typeof(string),
                typeof(DateTime),
                typeof(DateTimeOffset),
                typeof(TimeSpan),
                typeof(Guid),
                typeof(Uri),

                typeof(sbyte?),
                typeof(byte?),
                typeof(short?),
                typeof(ushort?),
                typeof(int?),
                typeof(uint?),
                typeof(long?),
                typeof(ulong?),
                typeof(float?),
                typeof(double?),
                typeof(decimal?),
                typeof(bool?),
                typeof(char?),
                typeof(DateTime?),
                typeof(DateTimeOffset?),
                typeof(TimeSpan?),
                typeof(Guid?),

                typeof(void),
                typeof(ConsoleColor),
                typeof(ConsoleColor?),

                typeof(int[]),
                typeof(string[][]),
                typeof(bool[][][]),

                typeof(List<int>),
                typeof(List<bool?[]>),
                typeof(IList<int>),
                typeof(IList<bool?[]>),

                typeof(Tuple<short>),
                typeof(Tuple<short, byte>),
                typeof(Tuple<short, byte, double[]>),
                typeof(Tuple<short, byte, double[], decimal>),
                typeof(Tuple<short, byte, double[], decimal, long?>),
                typeof(Tuple<short, byte, double[], decimal, long?, float>),
                typeof(Tuple<short, byte, double[], decimal, long?, float, bool>),
                typeof(Tuple<short, byte, double[], decimal, long?, float, bool, Tuple<char>>),
                typeof(Tuple<short, byte, double[], decimal, long?, float, bool, Tuple<char, string>>),

                typeof(Func<int>),
                typeof(Func<int, int>),
                typeof(Func<int, string, bool>),
                typeof(Action),
                typeof(Action<int>),

                typeof(Expression),
                typeof(Expression<Func<int>>),

                typeof(Func<int?, Func<List<double[]>, Tuple<string, bool[]>>>),

                typeof(Bar),
                typeof(Bar[]),

                anon.GetType(),
                rec,
            })
            {
                {
                    Assert.IsTrue(DataType.TryCheck(t, out var err), "Type check failed for: " + t.Name);
                    Assert.AreEqual(0, err.Count, "Type check failed for: " + t.Name);
                }

                {
                    Assert.IsTrue(DataType.TryCheck(t, allowCycles: false, out var err), "Type check failed for: " + t.Name);
                    Assert.AreEqual(0, err.Count, "Type check failed for: " + t.Name);
                }

                {
                    Assert.IsTrue(DataType.TryCheck(t, allowCycles: true, out var err), "Type check failed for: " + t.Name);
                    Assert.AreEqual(0, err.Count, "Type check failed for: " + t.Name);
                }

                DataType.Check(t);
                Assert.IsNotNull(DataType.FromType(t));
                Assert.IsNotNull(DataType.FromType(t, allowCycles: false));
                Assert.IsNotNull(DataType.FromType(t, allowCycles: true));

                Assert.IsTrue(DataType.TryFromType(t, out _));
                Assert.IsTrue(DataType.TryFromType(t, allowCycles: false, out _));
                Assert.IsTrue(DataType.TryFromType(t, allowCycles: true, out _));
            }
        }

        [TestMethod]
        public void DataTypeChecker_CheckTypes_Failure_UnsupportedTypes()
        {
            var anon = new { x = 1, b = new bool[] { true }, c = new List<AppDomain[]>() };

            foreach (var t in new[]
            {
                typeof(AppDomain),
                typeof(ExpandoObject),

                typeof(RuntimeMethodHandle),
                typeof(RuntimeMethodHandle?),

                typeof(int[,]),
                typeof(int).MakePointerType(),
                typeof(int).MakeByRefType(),
                typeof(IEnumerable<>),
                typeof(IEnumerable<>).GetGenericArguments()[0],
                typeof(List<>),

                typeof(IEnumerable<int>),
                typeof(Func<AppDomain>),
                typeof(Func<AppDomain, int>),
                typeof(Func<int, AppDomain>),

                anon.GetType(),
            })
            {
                {
                    Assert.IsFalse(DataType.TryCheck(t, out var err), "Type check failed for: " + t.Name);
                    Assert.AreNotEqual(0, err.Count, "Type check failed for: " + t.Name);
                }

                {
                    Assert.IsFalse(DataType.TryCheck(t, allowCycles: false, out var err), "Type check failed for: " + t.Name);
                    Assert.AreNotEqual(0, err.Count, "Type check failed for: " + t.Name);
                }

                {
                    Assert.IsFalse(DataType.TryCheck(t, allowCycles: true, out var err), "Type check failed for: " + t.Name);
                    Assert.AreNotEqual(0, err.Count, "Type check failed for: " + t.Name);
                }

                Assert.ThrowsException<AggregateException>(() => DataType.Check(t));
                Assert.ThrowsException<AggregateException>(() => DataType.Check(t, allowCycles: false));
                Assert.ThrowsException<AggregateException>(() => DataType.Check(t, allowCycles: true));

                Assert.ThrowsException<NotSupportedException>(() => DataType.FromType(t));
                Assert.ThrowsException<NotSupportedException>(() => DataType.FromType(t, allowCycles: false));
                Assert.ThrowsException<NotSupportedException>(() => DataType.FromType(t, allowCycles: true));

                Assert.IsFalse(DataType.TryFromType(t, out _));
                Assert.IsFalse(DataType.TryFromType(t, allowCycles: false, out _));
                Assert.IsFalse(DataType.TryFromType(t, allowCycles: true, out _));
            }
        }

        [TestMethod]
        public void DataTypeChecker_CheckTypes_Failure_InvalidTypes()
        {
            foreach (var t in new[]
            {
                typeof(DuplicateAttribute),
                typeof(MissingAttribute),
                typeof(EmptyAttribute1),
                typeof(EmptyAttribute2),
                typeof(EmptyAttribute3),
                typeof(WriteOnlyProperty1),
                typeof(WriteOnlyProperty2),
                typeof(CtorInvalidMapping),
                typeof(CtorMismatchedTypeMapping),
                typeof(CtorMissingMapping),

                typeof(EnumMissingMapping),
                typeof(EnumDuplicateMapping),
            })
            {
                {
                    Assert.IsFalse(DataType.TryCheck(t, out var err), "Type check failed for: " + t.Name);
                    Assert.AreNotEqual(0, err.Count, "Type check failed for: " + t.Name);
                }

                {
                    Assert.IsFalse(DataType.TryCheck(t, allowCycles: false, out var err), "Type check failed for: " + t.Name);
                    Assert.AreNotEqual(0, err.Count, "Type check failed for: " + t.Name);
                }

                {
                    Assert.IsFalse(DataType.TryCheck(t, allowCycles: true, out var err), "Type check failed for: " + t.Name);
                    Assert.AreNotEqual(0, err.Count, "Type check failed for: " + t.Name);
                }

                Assert.ThrowsException<AggregateException>(() => DataType.Check(t));
                Assert.ThrowsException<AggregateException>(() => DataType.Check(t, allowCycles: false));
                Assert.ThrowsException<AggregateException>(() => DataType.Check(t, allowCycles: true));

                Assert.ThrowsException<InvalidOperationException>(() => DataType.FromType(t));
                Assert.ThrowsException<InvalidOperationException>(() => DataType.FromType(t, allowCycles: false));
                Assert.ThrowsException<InvalidOperationException>(() => DataType.FromType(t, allowCycles: true));

                Assert.IsFalse(DataType.TryFromType(t, out _));
                Assert.IsFalse(DataType.TryFromType(t, allowCycles: false, out _));
                Assert.IsFalse(DataType.TryFromType(t, allowCycles: true, out _));
            }
        }

        [TestMethod]
        public void DataTypeChecker_CheckTypes_ErrorReporting()
        {
            Assert.IsFalse(DataType.TryCheck(typeof(Func<int, List<AppDomain[]>>), out var err));

            Assert.AreEqual(1, err.Count);
            Assert.IsTrue(new[] { typeof(Func<int, List<AppDomain[]>>), typeof(List<AppDomain[]>), typeof(AppDomain[]) }.Reverse().SequenceEqual(err[0].Stack));

            Assert.IsTrue(err[0].ToString().Contains("AppDomain"));
        }

        [TestMethod]
        public void DataTypeChecker_CheckTypes_Cycles_Success()
        {
            foreach (var t in new[]
            {
                typeof(Cycle1),
                typeof(Cycle2),
                typeof(Cycle3),
                typeof(Cycle1[]),
                typeof(List<Cycle2>),
                typeof(Tuple<Cycle3>),
            })
            {
                Assert.IsNotNull(DataType.FromType(t, allowCycles: true));

                DataType.Check(t, allowCycles: true);

                Assert.IsTrue(DataType.TryCheck(t, allowCycles: true, out var err));
                Assert.AreEqual(0, err.Count, "Type check failed for: " + t.Name);

                Assert.IsTrue(DataType.TryFromType(t, allowCycles: true, out _));
            }
        }

        [TestMethod]
        public void DataTypeChecker_CheckTypes_Cycles_Failure()
        {
            foreach (var t in new[]
            {
                typeof(Cycle1),
                typeof(Cycle2),
                typeof(Cycle3),
                typeof(Cycle1[]),
                typeof(List<Cycle2>),
                typeof(Tuple<Cycle3>),
            })
            {
                Assert.ThrowsException<InvalidOperationException>(() => DataType.FromType(t));
                Assert.ThrowsException<InvalidOperationException>(() => DataType.FromType(t, allowCycles: false));

                Assert.ThrowsException<AggregateException>(() => DataType.Check(t));
                Assert.ThrowsException<AggregateException>(() => DataType.Check(t, allowCycles: false));

                {
                    Assert.IsFalse(DataType.TryCheck(t, out var err));
                    Assert.AreNotEqual(0, err.Count, "Type check failed for: " + t.Name);
                }

                {
                    Assert.IsFalse(DataType.TryCheck(t, allowCycles: false, out var err));
                    Assert.AreNotEqual(0, err.Count, "Type check failed for: " + t.Name);
                }

                Assert.IsFalse(DataType.TryFromType(t, out _));
                Assert.IsFalse(DataType.TryFromType(t, allowCycles: false, out _));
            }
        }

        [TestMethod]
        public void DataTypeChecker_CheckTypes_Cycles_Simple()
        {
            var simple = DataType.FromType(typeof(SimpleCycle), allowCycles: true);

            Assert.AreEqual(DataTypeKinds.Structural, simple.Kind);

            var structural = (StructuralDataType)simple;

            Assert.AreEqual(1, structural.Properties.Count);

            Assert.AreSame(structural, structural.Properties[0].Type);
        }

        private class SimpleCycle
        {
            [Mapping("self")]
            public SimpleCycle Self { get; set; }
        }

        private class Bar
        {
            [Mapping("foo")]
            public Foo Foo { get; set; }

            [Mapping("qux")]
            public int[] Qux { get; set; }
        }

        private class Foo
        {
            [Mapping("baz")]
            public List<Tuple<bool, int>> Baz { get; set; }
        }

        private class Cycle1
        {
            [Mapping("x")]
            public Cycle2[] X { get; set; }
        }

        private class Cycle2
        {
            [Mapping("y")]
            public Tuple<int, List<Cycle3>> Y { get; set; }
        }

        private class Cycle3
        {
            [Mapping("z")]
            public Cycle1[][] Z { get; set; }
        }

        private class MissingAttribute
        {
            [Mapping("foo")]
            public int Foo { get; set; }

            public int Bar { get; set; }
        }

        private class EmptyAttribute1
        {
            [Mapping(null)]
            public int Foo { get; set; }
        }

        private class EmptyAttribute2
        {
            [Mapping("")]
            public int Foo { get; set; }
        }

        private class EmptyAttribute3
        {
            [Mapping(" \t  ")]
            public int Foo { get; set; }
        }

        private class DuplicateAttribute
        {
            [Mapping("qux")]
            public int Foo { get; set; }

            [Mapping("qux")]
            public int Bar { get; set; }
        }

        private class WriteOnlyProperty1
        {
            [Mapping("bar")]
            public int Bar { private get; set; }
        }

#pragma warning disable CA1822 // Mark static
        private class WriteOnlyProperty2
        {
            [Mapping("bar")]
            public int Bar
            {
                set
                {
                }
            }
        }
#pragma warning restore CA1822

#pragma warning disable IDE0060 // Remove unused parameter (accessed through reflection)
        private class CtorMissingMapping
        {
            public CtorMissingMapping(int x)
            {
            }

            [Mapping("x")]
            public int X { get; set; }
        }

        private class CtorInvalidMapping
        {
            public CtorInvalidMapping([Mapping("x")] int x)
            {
            }

            [Mapping("y")]
            public int X { get; set; }
        }

        private class CtorMismatchedTypeMapping
        {
            public CtorMismatchedTypeMapping([Mapping("x")] int x)
            {
            }

            [Mapping("x")]
            public string X { get; set; }
        }
#pragma warning restore IDE0060 // Remove unused parameter

        private enum EnumMissingMapping
        {
            Bar,

            [Mapping("foo")]
            Foo,
        }

        private enum EnumDuplicateMapping
        {
            [Mapping("foo")]
            Bar,

            [Mapping("foo")]
            Foo,
        }
    }
}
