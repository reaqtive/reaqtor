// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - July 2014 - Created this file.
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Nuqleon.DataModel.TypeSystem;

namespace Nuqleon.DataModel.Serialization.JsonTest
{
    public partial class DataSerializerTests
    {
        private const int Repeat = 1000;
        private static readonly Random r = new(42);

        [TestMethod]
        public void DataSerializer_Concurrency_Basic()
        {
            var baseCases = new object[]
            {
                true,
                false,
                'a',
                byte.MaxValue,
                byte.MinValue,
                sbyte.MaxValue,
                sbyte.MinValue,
                short.MaxValue,
                short.MinValue,
                ushort.MaxValue,
                ushort.MinValue,
                int.MaxValue,
                int.MinValue,
                uint.MaxValue,
                uint.MinValue,
                long.MaxValue,
                ulong.MinValue,
                float.MaxValue,
                float.MinValue,
                float.NegativeInfinity,
                float.PositiveInfinity,
                double.MaxValue,
                double.MinValue,
                double.PositiveInfinity,
                double.NegativeInfinity,
                Guid.NewGuid(),
                EnumByte.EnumValue1,
                EnumInt.EnumValue2,
                EnumLong.EnumValue1,
                EnumSbyte.EnumValue2,
                EnumShort.EnumValue1,
                EnumUint.EnumValue2,
                EnumUlong.EnumValue1,
                EnumUshort.EnumValue2,
                (EnumInt)13,
                Guid.NewGuid(),
                string.Empty,
                new Uri("http://www.microsoft.com"),
                DateTime.Now,
                new TimeSpan(1, 3, 2, 3, 3),
                new byte[] { 1, 2, 3, 4, 5 },
                new ClassWithByteEnumMember(),
                1,
                FlatClass.Create(),
                NestedClass.Create(),
                RecursiveClass.Create(),
#pragma warning disable IDE0079 // Next supression flagged as redundant on .NET SDK 6
#pragma warning disable IDE0050 // Convert to tuple. (Test for anonymous types.)
                new { CurrentTime = DateTime.UtcNow, Value = FlatClass.Create() },
#pragma warning restore IDE0050 // Convert to tuple
#pragma warning restore IDE0079
                InheritedClass.Create(),
                new Person { Name = "Bart", Age = 21 },
            };

            var testCases = Enumerable.Repeat(baseCases, Repeat).SelectMany(x => x).OrderBy(_ => r.Next());

            Parallel.ForEach(testCases, testCase => AssertRoundtrip(testCase));
        }

        [TestMethod]
        public void DataSerializer_Concurrency_Expressions()
        {
            var baseCases = new Expression[]
            {
                Expression.Parameter(typeof(int)),
                Expression.Parameter(typeof(FlatClass)),
                Expression.Constant(FlatClass.Create()),
                Expression.Constant(RecursiveClass.Create()),
                (Expression<Func<string, int, Person>>)((name, age) => new Person { Name = name, Age = age }),
            };

            var testCases = Enumerable.Repeat(baseCases, Repeat).SelectMany(x => x).OrderBy(_ => r.Next());

            Parallel.ForEach(testCases, testCase => AssertRoundtrip(testCase));
        }

        private void AssertRoundtrip(object expected)
        {
            var stream = new MemoryStream();
            var type = expected.GetType();
            var serialization = _genericSerialize.MakeGenericMethod(new[] { type });
            serialization.Invoke(_jsonSerializer, new[] { expected, stream });

            stream.Position = 0;
            var deserialization = _genericDeserialize.MakeGenericMethod(new[] { type });
            var actual = deserialization.Invoke(_jsonSerializer, new object[] { stream });

            if (!expected.Equals(actual))
            {
                Assert.IsTrue(DataTypeObjectEqualityComparer.Default.Equals(expected, actual), "Expected: {0}\nActual: {1}", expected, actual);
            }
        }

        private void AssertRoundtrip(Expression expected)
        {
            var stream = new MemoryStream();
            _jsonSerializer.Serialize(expected, stream);
            stream.Position = 0;
            var actual = _jsonSerializer.Deserialize<Expression>(stream);
            var comparer = new ExpressionEqualityComparer(() => new Comparator());
            Assert.IsTrue(comparer.Equals(expected, actual), "Expected: {0}\nActual: {1}", expected, actual);
        }

        private sealed class ObjectComparer : IEqualityComparer<object>
        {
            private ObjectComparer() { }

            public static ObjectComparer Default { get; } = new();

            public new bool Equals(object x, object y)
            {
                try
                {
                    return DataTypeObjectEqualityComparer.Default.Equals(x, y);
                }
                catch
                {
                    return EqualityComparer<object>.Default.Equals(x, y);
                }
            }

            public int GetHashCode(object obj)
            {
                try
                {
                    return DataTypeObjectEqualityComparer.Default.GetHashCode(obj);
                }
                catch
                {
                    return EqualityComparer<object>.Default.GetHashCode(obj);
                }
            }
        }

        private sealed class Comparator : ExpressionEqualityComparator
        {
            public Comparator()
                : base(EqualityComparer<Type>.Default, EqualityComparer<MemberInfo>.Default, ObjectComparer.Default, EqualityComparer<CallSiteBinder>.Default)
            {
            }

            protected override bool EqualsGlobalParameter(ParameterExpression x, ParameterExpression y)
            {
                return x.Name == y.Name && Equals(x.Type, y.Type);
            }
        }
    }
}
