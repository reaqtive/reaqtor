// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

using Nuqleon.DataModel.Serialization.Binary;
using Nuqleon.DataModel;
using Nuqleon.DataModel.TypeSystem;

using IExpressionSerializer = Nuqleon.DataModel.Serialization.Binary.IExpressionSerializer;

namespace Tests.Nuqleon.DataModel.Serialization.Binary
{
    public class DataModelSerializerFactoryTestCase
    {
#pragma warning disable CA1822 // Mark static

        public Tests DataModelSerializerFactory_Primitives_Tests()
        {
            var tests = new Tests
            {
                { "(Unit)default(Unit)", typeof(Unit), default(Unit) },
                { "(Unit?)default(Unit)", typeof(Unit?), default(Unit) },
                { "(Unit?)default(Unit?)", typeof(Unit?), default(Unit?) },
                { "(Unit?)null", typeof(Unit?), null },

                { "(sbyte)sbyte.MinValue", typeof(sbyte), sbyte.MinValue },
                { "(sbyte)default(sbyte)", typeof(sbyte), default(sbyte) },
                { "(sbyte)sbyte.MaxValue", typeof(sbyte), sbyte.MaxValue },
                { "(sbyte?)sbyte.MinValue", typeof(sbyte?), sbyte.MinValue },
                { "(sbyte?)default(sbyte?)", typeof(sbyte?), default(sbyte?) },
                { "(sbyte?)null", typeof(sbyte?), null },
                { "(sbyte?)sbyte.MaxValue", typeof(sbyte?), sbyte.MaxValue },

                { "(byte)byte.MinValue", typeof(byte), byte.MinValue },
                { "(byte)default(byte)", typeof(byte), default(byte) },
                { "(byte)byte.MaxValue", typeof(byte), byte.MaxValue },
                { "(byte?)byte.MinValue", typeof(byte?), byte.MinValue },
                { "(byte?)default(byte?)", typeof(byte?), default(byte?) },
                { "(byte?)null", typeof(byte?), null },
                { "(byte?)byte.MaxValue", typeof(byte?), byte.MaxValue },

                { "(short)short.MinValue", typeof(short), short.MinValue },
                { "(short)default(short)", typeof(short), default(short) },
                { "(short)short.MaxValue", typeof(short), short.MaxValue },
                { "(short?)short.MinValue", typeof(short?), short.MinValue },
                { "(short?)default(short?)", typeof(short?), default(short?) },
                { "(short?)null", typeof(short?), null },
                { "(short?)short.MaxValue", typeof(short?), short.MaxValue },

                { "(ushort)ushort.MinValue", typeof(ushort), ushort.MinValue },
                { "(ushort)default(ushort)", typeof(ushort), default(ushort) },
                { "(ushort)ushort.MaxValue", typeof(ushort), ushort.MaxValue },
                { "(ushort?)ushort.MinValue", typeof(ushort?), ushort.MinValue },
                { "(ushort?)default(ushort?)", typeof(ushort?), default(ushort?) },
                { "(ushort?)null", typeof(ushort?), null },
                { "(ushort?)ushort.MaxValue", typeof(ushort?), ushort.MaxValue },

                { "(int)int.MinValue", typeof(int), int.MinValue },
                { "(int)default(int)", typeof(int), default(int) },
                { "(int)int.MaxValue", typeof(int), int.MaxValue },
                { "(int?)int.MinValue", typeof(int?), int.MinValue },
                { "(int?)default(int?)", typeof(int?), default(int?) },
                { "(int?)null", typeof(int?), null },
                { "(int?)int.MaxValue", typeof(int?), int.MaxValue },

                { "(uint)uint.MinValue", typeof(uint), uint.MinValue },
                { "(uint)default(uint)", typeof(uint), default(uint) },
                { "(uint)uint.MaxValue", typeof(uint), uint.MaxValue },
                { "(uint?)uint.MinValue", typeof(uint?), uint.MinValue },
                { "(uint?)default(uint?)", typeof(uint?), default(uint?) },
                { "(uint?)null", typeof(uint?), null },
                { "(uint?)uint.MaxValue", typeof(uint?), uint.MaxValue },

                { "(long)long.MinValue", typeof(long), long.MinValue },
                { "(long)default(long)", typeof(long), default(long) },
                { "(long)long.MaxValue", typeof(long), long.MaxValue },
                { "(long?)long.MinValue", typeof(long?), long.MinValue },
                { "(long?)default(long?)", typeof(long?), default(long?) },
                { "(long?)null", typeof(long?), null },
                { "(long?)long.MaxValue", typeof(long?), long.MaxValue },

                { "(ulong)ulong.MinValue", typeof(ulong), ulong.MinValue },
                { "(ulong)default(ulong)", typeof(ulong), default(ulong) },
                { "(ulong)ulong.MaxValue", typeof(ulong), ulong.MaxValue },
                { "(ulong?)ulong.MinValue", typeof(ulong?), ulong.MinValue },
                { "(ulong?)default(ulong?)", typeof(ulong?), default(ulong?) },
                { "(ulong?)null", typeof(ulong?), null },
                { "(ulong?)ulong.MaxValue", typeof(ulong?), ulong.MaxValue },

                { "(float)float.MinValue", typeof(float), float.MinValue },
                { "(float)default(float)", typeof(float), default(float) },
                { "(float)float.MaxValue", typeof(float), float.MaxValue },
                { "(float?)float.MinValue", typeof(float?), float.MinValue },
                { "(float?)default(float?)", typeof(float?), default(float?) },
                { "(float?)null", typeof(float?), null },
                { "(float?)float.MaxValue", typeof(float?), float.MaxValue },

                { "(double)double.MinValue", typeof(double), double.MinValue },
                { "(double)default(double)", typeof(double), default(double) },
                { "(double)double.MaxValue", typeof(double), double.MaxValue },
                { "(double?)double.MinValue", typeof(double?), double.MinValue },
                { "(double?)default(double?)", typeof(double?), default(double?) },
                { "(double?)null", typeof(double?), null },
                { "(double?)double.MaxValue", typeof(double?), double.MaxValue },

                { "(decimal)decimal.MinValue", typeof(decimal), decimal.MinValue },
                { "(decimal)default(decimal)", typeof(decimal), default(decimal) },
                { "(decimal)decimal.MaxValue", typeof(decimal), decimal.MaxValue },
                { "(decimal?)decimal.MinValue", typeof(decimal?), decimal.MinValue },
                { "(decimal?)default(decimal?)", typeof(decimal?), default(decimal?) },
                { "(decimal?)null", typeof(decimal?), null },
                { "(decimal?)decimal.MaxValue", typeof(decimal?), decimal.MaxValue },

                { "(bool)true", typeof(bool), true },
                { "(bool)false", typeof(bool), false },
                { "(bool?)true", typeof(bool?), true },
                { "(bool?)false", typeof(bool?), false },
                { "(bool?)null", typeof(bool?), null },

                { "(char)char.MinValue", typeof(char), char.MinValue },
                { "(char)default(char)", typeof(char), default(char) },
                { "(char)char.MaxValue", typeof(char), char.MaxValue },
                { "(char?)char.MinValue", typeof(char?), char.MinValue },
                { "(char?)default(char?)", typeof(char?), default(char?) },
                { "(char?)null", typeof(char?), null },
                { "(char?)char.MaxValue", typeof(char?), char.MaxValue },

                { "(string)null", typeof(string), null },
                { "(string)\"\"", typeof(string), "" },
                { "(string)\"quick brown fox jumped over the lazy dog\"", typeof(string), "quick brown fox jumped over the lazy dog" },

                { "(DateTime)DateTime.MinValue", typeof(DateTime), DateTime.MinValue },
                { "(DateTime)default(DateTime)", typeof(DateTime), default(DateTime) },
                { "(DateTime)new DateTime(DateTime.UtcNow.Ticks, DateTimeKind.Local)", typeof(DateTime), new DateTime(DateTime.UtcNow.Ticks, DateTimeKind.Local) },
                { "(DateTime)new DateTime(DateTime.UtcNow.Ticks, DateTimeKind.Unspecified)", typeof(DateTime), new DateTime(DateTime.UtcNow.Ticks, DateTimeKind.Unspecified) },
                { "(DateTime)new DateTime(DateTime.UtcNow.Ticks, DateTimeKind.Utc)", typeof(DateTime), new DateTime(DateTime.UtcNow.Ticks, DateTimeKind.Utc) },
                { "(DateTime)DateTime.Now", typeof(DateTime), DateTime.Now },
                { "(DateTime)DateTime.UtcNow", typeof(DateTime), DateTime.UtcNow },
                { "(DateTime)DateTime.MaxValue", typeof(DateTime), DateTime.MaxValue },
                { "(DateTime?)DateTime.MinValue", typeof(DateTime?), DateTime.MinValue },
                { "(DateTime?)default(DateTime?)", typeof(DateTime?), default(DateTime?) },
                { "(DateTime?)null", typeof(DateTime?), null },
                { "(DateTime?)DateTime.MaxValue", typeof(DateTime?), DateTime.MaxValue },

                { "(DateTimeOffset)DateTimeOffset.MinValue", typeof(DateTimeOffset), DateTimeOffset.MinValue },
                { "(DateTimeOffset)default(DateTimeOffset)", typeof(DateTimeOffset), default(DateTimeOffset) },
                { "(DateTimeOffset)DateTimeOffset.Now", typeof(DateTimeOffset), DateTimeOffset.Now },
                { "(DateTimeOffset)DateTimeOffset.UtcNow", typeof(DateTimeOffset), DateTimeOffset.UtcNow },
                { "(DateTimeOffset)DateTimeOffset.MaxValue", typeof(DateTimeOffset), DateTimeOffset.MaxValue },
                { "(DateTimeOffset?)DateTimeOffset.MinValue", typeof(DateTimeOffset?), DateTimeOffset.MinValue },
                { "(DateTimeOffset?)default(DateTimeOffset?)", typeof(DateTimeOffset?), default(DateTimeOffset?) },
                { "(DateTimeOffset?)null", typeof(DateTimeOffset?), null },
                { "(DateTimeOffset?)DateTimeOffset.MaxValue", typeof(DateTimeOffset?), DateTimeOffset.MaxValue },

                { "(TimeSpan)TimeSpan.MinValue", typeof(TimeSpan), TimeSpan.MinValue },
                { "(TimeSpan)default(TimeSpan)", typeof(TimeSpan), default(TimeSpan) },
                { "(TimeSpan)TimeSpan.Zero", typeof(TimeSpan), TimeSpan.Zero },
                { "(TimeSpan)TimeSpan.MaxValue", typeof(TimeSpan), TimeSpan.MaxValue },
                { "(TimeSpan?)TimeSpan.MinValue", typeof(TimeSpan?), TimeSpan.MinValue },
                { "(TimeSpan?)default(TimeSpan?)", typeof(TimeSpan?), default(TimeSpan?) },
                { "(TimeSpan?)null", typeof(TimeSpan?), null },
                { "(TimeSpan?)TimeSpan.MaxValue", typeof(TimeSpan?), TimeSpan.MaxValue },

                { "(Guid)Guid.NewGuid()", typeof(Guid), Guid.NewGuid() },
                { "(Guid)default(Guid)", typeof(Guid), default(Guid) },
                { "(Guid?)Guid.NewGuid()", typeof(Guid?), Guid.NewGuid() },
                { "(Guid?)default(Guid?)", typeof(Guid?), default(Guid?) },
                { "(Guid?)null", typeof(Guid?), null },

                { "(Uri)null", typeof(Uri), null },
                { "(Uri)default(Uri)", typeof(Uri), default(Uri) },
                { "(Uri)new Uri(\"http://microsoft.com\")", typeof(Uri), new Uri("http://microsoft.com") },
                { "(Uri)new Uri(\"reactor://test/path\")", typeof(Uri), new Uri("reactor://test/path") },
                { "(Uri)new Uri(\"test/path\", UriKind.Relative)", typeof(Uri), new Uri("test/path", UriKind.Relative) },
                { "(Uri)new Uri(\"test/path\", UriKind.RelativeOrAbsolute)", typeof(Uri), new Uri("test/path", UriKind.RelativeOrAbsolute) },
            };

            return tests;
        }

        public Tests DataModelSerializerFactory_Arrays_Tests()
        {
            var tests = new Tests
            {
                { "(int[])null, ArrayComparer.Instance", typeof(int[]), null, ArrayComparer.Instance },
                { "(int[])new int[0], ArrayComparer.Instance", typeof(int[]), Array.Empty<int>(), ArrayComparer.Instance },
                { "(int[])new[] { 42 }, ArrayComparer.Instance", typeof(int[]), new[] { 42 }, ArrayComparer.Instance },
                { "(int[])Enumerable.Range(0, 2).ToArray(), ArrayComparer.Instance", typeof(int[]), Enumerable.Range(0, 2).ToArray(), ArrayComparer.Instance },
                { "(int[])Enumerable.Range(0, 4).ToArray(), ArrayComparer.Instance", typeof(int[]), Enumerable.Range(0, 4).ToArray(), ArrayComparer.Instance },
                { "(int[])Enumerable.Range(0, 16).ToArray(), ArrayComparer.Instance", typeof(int[]), Enumerable.Range(0, 16).ToArray(), ArrayComparer.Instance },
                { "(int[])Enumerable.Range(0, 256).ToArray(), ArrayComparer.Instance", typeof(int[]), Enumerable.Range(0, 256).ToArray(), ArrayComparer.Instance },
                { "(int[])Enumerable.Range(0, 1024).ToArray(), ArrayComparer.Instance", typeof(int[]), Enumerable.Range(0, 1024).ToArray(), ArrayComparer.Instance },
                { "(int[])Enumerable.Repeat(int.MaxValue, 2).ToArray(), ArrayComparer.Instance", typeof(int[]), Enumerable.Repeat(int.MaxValue, 2).ToArray(), ArrayComparer.Instance },
                { "(int[])Enumerable.Repeat(int.MaxValue, 4).ToArray(), ArrayComparer.Instance", typeof(int[]), Enumerable.Repeat(int.MaxValue, 4).ToArray(), ArrayComparer.Instance },
                { "(int[])Enumerable.Repeat(int.MaxValue, 16).ToArray(), ArrayComparer.Instance", typeof(int[]), Enumerable.Repeat(int.MaxValue, 16).ToArray(), ArrayComparer.Instance },
                { "(int[])Enumerable.Repeat(int.MaxValue, 256).ToArray(), ArrayComparer.Instance", typeof(int[]), Enumerable.Repeat(int.MaxValue, 256).ToArray(), ArrayComparer.Instance },
                { "(int[])Enumerable.Repeat(int.MaxValue, 1024).ToArray(), ArrayComparer.Instance", typeof(int[]), Enumerable.Repeat(int.MaxValue, 1024).ToArray(), ArrayComparer.Instance },
            };

            return tests;
        }

        public Tests DataModelSerializerFactory_Lists_Tests()
        {
            var tests = new Tests
            {
                { "(List<int>)null, ArrayComparer.Instance", typeof(List<int>), null, ArrayComparer.Instance },
                { "(List<int>)new List<int>(0), ArrayComparer.Instance", typeof(List<int>), new List<int>(0), ArrayComparer.Instance },
                { "(List<int>)new List<int> { 42 }, ArrayComparer.Instance", typeof(List<int>), new List<int> { 42 }, ArrayComparer.Instance },
                { "(List<int>)Enumerable.Range(0, 2).ToList(), ArrayComparer.Instance", typeof(List<int>), Enumerable.Range(0, 2).ToList(), ArrayComparer.Instance },
                { "(List<int>)Enumerable.Range(0, 4).ToList(), ArrayComparer.Instance", typeof(List<int>), Enumerable.Range(0, 4).ToList(), ArrayComparer.Instance },
                { "(List<int>)Enumerable.Range(0, 16).ToList(), ArrayComparer.Instance", typeof(List<int>), Enumerable.Range(0, 16).ToList(), ArrayComparer.Instance },
                { "(List<int>)Enumerable.Range(0, 256).ToList(), ArrayComparer.Instance", typeof(List<int>), Enumerable.Range(0, 256).ToList(), ArrayComparer.Instance },
                { "(List<int>)Enumerable.Range(0, 1024).ToList(), ArrayComparer.Instance", typeof(List<int>), Enumerable.Range(0, 1024).ToList(), ArrayComparer.Instance },
                { "(List<int>)Enumerable.Repeat(int.MaxValue, 2).ToList(), ArrayComparer.Instance", typeof(List<int>), Enumerable.Repeat(int.MaxValue, 2).ToList(), ArrayComparer.Instance },
                { "(List<int>)Enumerable.Repeat(int.MaxValue, 4).ToList(), ArrayComparer.Instance", typeof(List<int>), Enumerable.Repeat(int.MaxValue, 4).ToList(), ArrayComparer.Instance },
                { "(List<int>)Enumerable.Repeat(int.MaxValue, 16).ToList(), ArrayComparer.Instance", typeof(List<int>), Enumerable.Repeat(int.MaxValue, 16).ToList(), ArrayComparer.Instance },
                { "(List<int>)Enumerable.Repeat(int.MaxValue, 256).ToList(), ArrayComparer.Instance", typeof(List<int>), Enumerable.Repeat(int.MaxValue, 256).ToList(), ArrayComparer.Instance },
                { "(List<int>)Enumerable.Repeat(int.MaxValue, 1024).ToList(), ArrayComparer.Instance", typeof(List<int>), Enumerable.Repeat(int.MaxValue, 1024).ToList(), ArrayComparer.Instance },
            };

            return tests;
        }

        public Tests DataModelSerializerFactory_IntArrayArray_Tests()
        {
            var tests = new Tests
            {
                { "(int [] [])null, ArrayComparer.Instance", typeof(int[][]), null, ArrayComparer.Instance },
                { "(int [] [])new int [][] {null}, ArrayComparer.Instance", typeof(int[][]), new int[][] { null }, ArrayComparer.Instance },
                { "(int [] [])new int [][] {new int [0]}, ArrayComparer.Instance", typeof(int[][]), new int[][] { Array.Empty<int>() }, ArrayComparer.Instance },
                { "(int [] [])Enumerable.Range(0, 10).Select(i => i % 2 == 0 ? new int [0] : null ).ToArray()", typeof(int[][]), Enumerable.Range(0, 10).Select(i => i % 2 == 0 ? Array.Empty<int>() : null ).ToArray(), ArrayComparer.Instance },
            };

            return tests;
        }

        public Tests DataModelSerializerFactory_Enums_Tests()
        {
            var tests = new Tests
            {
                { "(ConsoleColor)default(ConsoleColor)", typeof(ConsoleColor), default(ConsoleColor) },
                { "(ConsoleColor)ConsoleColor.DarkBlue", typeof(ConsoleColor), ConsoleColor.DarkBlue },
                { "(ConsoleColor?)default(ConsoleColor?)", typeof(ConsoleColor?), default(ConsoleColor?) },
                { "(ConsoleColor?)ConsoleColor.DarkBlue", typeof(ConsoleColor?), ConsoleColor.DarkBlue },
                { "(ConsoleColor?)null", typeof(ConsoleColor?), null },
            };

            return tests;
        }

        public Tests DataModelSerializerFactory_Record_Tests()
        {
            var s1 = new StructuralHelper(StructuralDataTypeKinds.Record)
            {
                { "Foo", typeof(int), 42 },
                { "Bar", typeof(string), "qux" },
            };

            var tests = new Tests
            {
                { "new Record()", s1.Type, Activator.CreateInstance(s1.Type) },
                { "new Record { Foo = 42, Bar = \"qux\" }", s1.Type, s1.Instance },
                { "(Record)null", s1.Type, null },
            };

            return tests;
        }

#pragma warning disable IDE0050 // Convert to tuple. (Test for anonymous types.)

        public Tests DataModelSerializerFactory_Anonymous_Tests()
        {
            var instance = new { Foo = 42, Bar = "qux" };
            var type = instance.GetType();

            var tests = new Tests
            {
                { "new { Foo = default(int), Bar = default(string) }", type, new { Foo = default(int), Bar = default(string) } },
                { "new { Foo = 42, Bar = \"qux\" }", type, instance },
                { "(Anonymous)null", type, null },
            };

            return tests;
        }

#pragma warning restore IDE0050

        public Tests DataModelSerializerFactory_Entity_Tests()
        {
            var tests = new Tests
            {
                { "(SimpleEntity)new SimpleEntity()", typeof(SimpleEntity), new SimpleEntity(), DataTypeObjectEqualityComparer.Default },
                { "(SimpleEntity)new SimpleEntity { Foo = 42, Bar = \"qux\" }", typeof(SimpleEntity), new SimpleEntity { Foo = 42, Bar = "qux" }, DataTypeObjectEqualityComparer.Default },
                { "(SimpleEntity)null", typeof(SimpleEntity), null, DataTypeObjectEqualityComparer.Default },
            };

            return tests;
        }

        public Tests DataModelSerializerFactory_Tuples_Tests()
        {
            var tests = new Tests
            {
                { "(Tuple<int, string>)Tuple.Create(default(int), default(string))", typeof(Tuple<int, string>), Tuple.Create(default(int), default(string)) },
                { "(Tuple<int, string>)Tuple.Create(42, \"qux\")", typeof(Tuple<int, string>), Tuple.Create(42, "qux") },
                { "(Tuple<ints,tring>)null", typeof(Tuple<int, string>), null },
            };

            return tests;
        }

        public Tests DataModelSerializerFactory_ArrayConversions_Tests()
        {
            var tests = new Tests
            {
                // list to array
                { "(int[])(List<int>)Enumerable.Range(1, 10).ToList()", typeof(List<int>), typeof(int[]), Enumerable.Range(1, 10).ToList(), DataTypeObjectEqualityComparer.Default },
                { "(int[])(List<int>)null", typeof(List<int>), typeof(int[]), null, DataTypeObjectEqualityComparer.Default },
                { "(int[])(List<int>)new List<int>(0)", typeof(List<int>), typeof(int[]), new List<int>(0), DataTypeObjectEqualityComparer.Default },
                // array to list
                { "(List<int>)(int[])Enumerable.Range(1, 10).ToArray()", typeof(int[]), typeof(List<int>), Enumerable.Range(1, 10).ToArray(), DataTypeObjectEqualityComparer.Default },
                { "(List<int>)(int[])null", typeof(int[]), typeof(List<int>), null, DataTypeObjectEqualityComparer.Default },
                { "(List<int>)(int[])new int[0]", typeof(int[]), typeof(List<int>), Array.Empty<int>(), DataTypeObjectEqualityComparer.Default },
            };

            return tests;
        }

        public Tests DataModelSerializerFactory_EnumConversions_Tests()
        {
            var tests = new Tests
            {
                // enum to primitive
                { "(ConsoleColor)typeof(int), ConsoleColor.Black, DataTypeObjectEqualityComparer.Default", typeof(ConsoleColor), typeof(int), ConsoleColor.Black, DataTypeObjectEqualityComparer.Default },
                // primitive to enum
                { "(int)typeof(ConsoleColor), 0, DataTypeObjectEqualityComparer.Default", typeof(int), typeof(ConsoleColor), 0, DataTypeObjectEqualityComparer.Default },
            };

            return tests;
        }

        public Tests DataModelSerializerFactory_StructuralConversions_Tests()
        {
            var recordHelper = new StructuralHelper(StructuralDataTypeKinds.Record)
            {
                { "Item1", typeof(int), 42 },
                { "Item2", typeof(string), "qux" },
            };

            var record = recordHelper.Instance;
            var recordType = recordHelper.Type;

#pragma warning disable IDE0050 // Convert to tuple. (Test for anonymous types.)

            var anonymous = new { Item1 = 42, Item2 = "qux" };
            var anonymousType = anonymous.GetType();

#pragma warning restore IDE0050

            var tuple = Tuple.Create(42, "qux");
            var tupleType = typeof(Tuple<int, string>);

            var entity = new MyTuple { Item1 = 42, Item2 = "qux" };
            var entityType = typeof(MyTuple);

            var tests = new Tests
            {
                // record to anonymous
                { "(Anonymous)(Record)new { Item1 = 42, Item2 = \"qux\" }", recordType, anonymousType, record, DataTypeObjectEqualityComparer.Default },
                // record to entity
                { "(MyTuple)(Record)new { Item1 = 42, Item2 = \"qux\" }", recordType, entityType, record, DataTypeObjectEqualityComparer.Default },
                // record to tuple
                { "(Tuple<int, string>)(Record)new { Item1 = 42, Item2 = \"qux\" }", recordType, tupleType, record, DataTypeObjectEqualityComparer.Default },

                // anonymous to record
                { "(Record)(Anonymous)new { Item1 = 42, Item2 = \"qux\" }", anonymousType, recordType, anonymous, DataTypeObjectEqualityComparer.Default },
                // anonymous to entity
                { "(MyTuple)(Anonymous)new { Item1 = 42, Item2 = \"qux\" }", anonymousType, entityType, anonymous, DataTypeObjectEqualityComparer.Default },
                // anonymous to tuple
                { "(Tuple<int, string>)(Anonymous)new { Item1 = 42, Item2 = \"qux\" }", anonymousType, tupleType, anonymous, DataTypeObjectEqualityComparer.Default },

                // entity to record
                { "(Record)(MyTuple)new MyTuple { Item1 = 42, Item2 = \"qux\" }", entityType, recordType, entity, DataTypeObjectEqualityComparer.Default },
                // entity to anonymous
                { "(Anonymous)(MyTuple)new MyTuple { Item1 = 42, Item2 = \"qux\" }", entityType, anonymousType, entity, DataTypeObjectEqualityComparer.Default },
                // entity to tuple
                { "(Tuple<int, string>)(MyTuple)new MyTuple { Item1 = 42, Item2 = \"qux\" }", entityType, tupleType, entity, DataTypeObjectEqualityComparer.Default },

                // tuple to record
                { "(Record)(Tuple<int, string>)Tuple.Create(42, \"qux\")", tupleType, recordType, tuple, DataTypeObjectEqualityComparer.Default },
                // tuple to anonymous
                { "(Anonymous)(Tuple<int, string>)Tuple.Create(42, \"qux\")", tupleType, anonymousType, tuple, DataTypeObjectEqualityComparer.Default },
                // tuple to entity
                { "(MyTuple)(Tuple<int, string>)Tuple.Create(42, \"qux\")", tupleType, entityType, tuple, DataTypeObjectEqualityComparer.Default },
            };

            return tests;
        }

        public Tests DataModelSerializerFactory_Expressions_Tests()
        {
            var tests = new Tests
            {
                { "new ExpressionHolder { Expression = new Expression[] { Expression.Constant(42, typeof(int)) } }", typeof(ExpressionHolder), new ExpressionHolder { Expression = new Expression[] { Expression.Constant(42, typeof(int)) } }, new DataTypeObjectEqualityComparer(() => new DataTypeExpressionComparator()) },
                { "new QuotationHolder { Quotation = () => 42 }", typeof(QuotationHolder), new QuotationHolder { Quotation = () => 42 }, new DataTypeObjectEqualityComparer(() => new DataTypeExpressionComparator()) },
            };

            return tests;
        }

        public Tests DataModelSerializerFactory_StructuralTypeCycle_Tests()
        {
            var maxDepth = 5;
            var instances = new OuterCycleType[maxDepth * 2];
            instances[0] = new OuterCycleType();
            for (var i = 1; i < maxDepth * 2; ++i)
            {
                instances[i] = new OuterCycleType
                {
                    Inner = new InnerCycleType
                    {
                        Outer = instances[i - 1],
                        OuterArray = new[] { instances[i - 1] },
                        OuterList = new List<OuterCycleType> { instances[i - 1] },
                    }
                };
            }

            var minDepth = 1;
            var tests = new Tests();
            for (var i = minDepth - 1; i < maxDepth; ++i)
            {
                tests.Add("new OuterCycleType(/* depth = " + i + " */)", typeof(OuterCycleType), instances[i], DataTypeObjectEqualityComparer.Default);
            }

            return tests;
        }

        public Tests DataModelSerializerFactory_StructuralTypeCycle_Anonymous_Tests()
        {
            var rtc = new RuntimeCompiler();
            var atb = rtc.GetNewAnonymousTypeBuilder();
            rtc.DefineAnonymousType(atb, new Dictionary<string, Type>
            {
                { "Self", atb },
            });
            var at = atb.CreateType();
            var inner = Activator.CreateInstance(at, new object[] { null });
            var outer = Activator.CreateInstance(at, new object[] { inner });

            var tests = new Tests
            {
                { "AnonymousTypeWithCycle", at, outer, DataTypeObjectEqualityComparer.Default },
            };

            return tests;
        }

        public Tests DataModelSerializerFactory_ManOrBoy_Tests()
        {
            var instance = new ManOrBoy
            {
                AllPrimitives = new Tuple<Unit, sbyte, byte, short, ushort, int, uint, Tuple<long, ulong, float, double, decimal, bool, char, Tuple<string, DateTime, DateTimeOffset, TimeSpan, Guid, Uri>>>(
                    new Unit(),
                    sbyte.MinValue,
                    byte.MaxValue,
                    short.MinValue,
                    ushort.MaxValue,
                    int.MinValue,
                    uint.MaxValue,
                    new Tuple<long, ulong, float, double, decimal, bool, char, Tuple<string, DateTime, DateTimeOffset, TimeSpan, Guid, Uri>>(
                        long.MinValue,
                        ulong.MaxValue,
                        float.MinValue,
                        double.MaxValue,
                        decimal.MinValue,
                        true,
                        char.MaxValue,
                        Tuple.Create(
                            "Lorem ipsum dolor...",
                            DateTime.Now,
                            DateTimeOffset.MaxValue,
                            TimeSpan.MinValue,
                            Guid.NewGuid(),
                            new Uri("http://microsoft.com")
                        )
                    )
                ),
                ArrayListTupleArray = new[]
                {
                    new List<Tuple<int[], string>>
                    {
                        Tuple.Create(new[] { 1, 2, 3 }, "foo"),
                        Tuple.Create(new[] { 4, 5, 6 }, "bar"),
                    },
                    new List<Tuple<int[], string>>
                    {
                        Tuple.Create(default(int[]), default(string)),
                        Tuple.Create(Array.Empty<int>(), ""),
                        null
                    }
                }
            };

            var tests = new Tests
            {
                { "ManOrBoy", typeof(ManOrBoy), instance, DataTypeObjectEqualityComparer.Default }
            };

            return tests;
        }

#pragma warning restore CA1822

        private enum MyConsoleColor
        {
            MyBlack = 0,
        }

        private class SimpleEntity
        {
            [Mapping("Foo")]
            public int Foo { get; set; }

            [Mapping("Bar")]
            public string Bar { get; set; }
        }

        private class MyTuple
        {
            [Mapping("Item1")]
            public int Item1 { get; set; }

            [Mapping("Item2")]
            public string Item2 { get; set; }
        }

        public class InnerCycleType
        {
            [Mapping("Outer")]
            public OuterCycleType Outer { get; set; }

            [Mapping("OuterArray")]
            public OuterCycleType[] OuterArray { get; set; }

            [Mapping("OuterList")]
            public List<OuterCycleType> OuterList { get; set; }
        }

        public class OuterCycleType
        {
            [Mapping("Inner")]
            public InnerCycleType Inner { get; set; }
        }

        private class ExpressionHolder
        {
            [Mapping("Expressions")]
            public Expression[] Expression { get; set; }
        }

        private class QuotationHolder
        {
            [Mapping("Quotation")]
            public Expression<Func<int>> Quotation { get; set; }
        }

        private class DataTypeExpressionComparator : DataTypeObjectEqualityComparator
        {
            private readonly ExpressionEqualityComparator _expressionComparer = new();

            protected override bool EqualsExpression(object expected, object actual, ExpressionDataType expectedDataType, ExpressionDataType actualDataType)
            {
                return EqualsExpressionCore(expected, actual);
            }

            protected override bool EqualsQuotation(object expected, object actual, QuotationDataType expectedDataType, QuotationDataType actualDataType)
            {
                return EqualsExpressionCore(expected, actual);
            }

            private bool EqualsExpressionCore(object expected, object actual)
            {
                var expectedExpression = (Expression)expected;
                var actualExpression = (Expression)actual;
                return _expressionComparer.Equals(expectedExpression, actualExpression);
            }
        }

        private class ManOrBoy
        {
            [Mapping("AllPrimitives")]
            public Tuple<Unit, sbyte, byte, short, ushort, int, uint, Tuple<long, ulong, float, double, decimal, bool, char, Tuple<string, DateTime, DateTimeOffset, TimeSpan, Guid, Uri>>> AllPrimitives { get; set; }

            [Mapping("ArrayListTupleArray")]
            public List<Tuple<int[], string>>[] ArrayListTupleArray { get; set; }
        }

        public struct TestCase
        {
            public string Name;
            public Type InputType;
            public Type OutputType;
            public object Value;
            public IEqualityComparer<object> Comparer;
        }

        public class IsotopeValue
        {
            public object Value { get; set; }
            public Type Type { get; set; }
        }

        public class IsotopeTestCase : IEnumerable<IsotopeValue>
        {
            private readonly List<IsotopeValue> isotopes = new();

            public void Add(Type type, object value)
            {
                isotopes.Add(new IsotopeValue { Type = type, Value = value });
            }

            public IEnumerator<IsotopeValue> GetEnumerator() => isotopes.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)isotopes).GetEnumerator();
        }

        public class Tests : IEnumerable<TestCase>
        {
            private readonly List<TestCase> _list = new();

            public void Add(string name, Type type, object value)
            {
                _list.Add(new TestCase { Name = name, InputType = type, OutputType = type, Value = value, Comparer = EqualityComparer<object>.Default });
            }

            public void Add(string name, Type type, object value, IEqualityComparer<object> comparer)
            {
                _list.Add(new TestCase { Name = name, InputType = type, OutputType = type, Value = value, Comparer = comparer });
            }

            public void Add(string name, Type inputType, Type outputType, object value, IEqualityComparer<object> comparer)
            {
                _list.Add(new TestCase { Name = name, InputType = inputType, OutputType = outputType, Value = value, Comparer = comparer });
            }

            public IEnumerator<TestCase> GetEnumerator() => _list.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_list).GetEnumerator();
        }

        public class ConstantExpressionSerializer : IExpressionSerializer
        {
            public DataTypeBinarySerializer DataSerializer
            {
                get;
                set;
            }

            public Expression Deserialize(Stream stream)
            {
                var nodeType = (ExpressionType)DataSerializer.Deserialize(typeof(ExpressionType), stream);
                switch (nodeType)
                {
                    case ExpressionType.Constant:
                        {
                            var typeName = (string)DataSerializer.Deserialize(typeof(string), stream);
                            var type = Type.GetType(typeName);
                            var value = DataSerializer.Deserialize(type, stream);
                            return Expression.Constant(value, type);
                        }
                    case ExpressionType.Lambda:
                        {
                            var value = DataSerializer.Deserialize(typeof(int), stream);
                            return Expression.Lambda<Func<int>>(Expression.Constant(value, typeof(int)));
                        }
                    default:
                        throw new NotSupportedException("Expression is not supported.");
                }

            }

            public void Serialize(Stream stream, Expression expression)
            {
                DataSerializer.Serialize(typeof(ExpressionType), stream, expression.NodeType);
                if (expression is ConstantExpression constantExpression)
                {
                    DataSerializer.Serialize(typeof(string), stream, constantExpression.Type.FullName);
                    DataSerializer.Serialize(constantExpression.Type, stream, constantExpression.Value);
                }
                else if (expression is Expression<Func<int>> lambdaExpression && lambdaExpression.Body.NodeType == ExpressionType.Constant)
                {
                    constantExpression = (ConstantExpression)lambdaExpression.Body;
                    DataSerializer.Serialize(typeof(int), stream, constantExpression.Value);
                }
                else
                {
                    throw new NotSupportedException("Expression is not supported.");
                }
            }
        }

        public class ArrayComparer : IEqualityComparer<object>
        {
            private const int Prime = 2017;

            private ArrayComparer() { }

            public static ArrayComparer Instance { get; } = new();

            public new bool Equals(object x, object y)
            {
                if (x is IList xList && y is IList yList)
                {
                    var ex = xList.GetEnumerator();
                    var ey = yList.GetEnumerator();

                    while (ex.MoveNext())
                    {
                        if (!ey.MoveNext() || !Equals(ex.Current, ey.Current))
                        {
                            return false;
                        }
                    }

                    return !ey.MoveNext();
                }

                return DeepComparer.Instance.Equals(x, y);
            }

            public int GetHashCode(object obj)
            {
                if (obj is IList list)
                {
                    var e = list.GetEnumerator();
                    var hash = 0;
                    while (e.MoveNext())
                    {
                        unchecked
                        {
                            hash = hash * Prime + GetHashCode(e.Current);
                        }
                    }
                    return hash;
                }

                return DeepComparer.Instance.GetHashCode(obj);
            }
        }

        public class DeepComparer : IEqualityComparer<object>
        {
            private static readonly ConcurrentDictionary<Type, List<PropertyInfo>> typeProps = new();

            public static readonly DeepComparer Instance = new();

            public new bool Equals(object x, object y)
            {
                if (x == y)
                {
                    return true;
                }

                if (x == null || y == null)
                {
                    return false;
                }

                var xType = x.GetType();
                var yType = y.GetType();

                if (xType != yType)
                {
                    return false;
                }

                if (ShallowComparable(xType))
                {
                    return x.Equals(y);
                }

                if (typeof(IList).IsAssignableFrom(xType))
                {
                    return ArrayComparer.Instance.Equals(x, y);
                }

                foreach (var prop in typeProps.GetOrAdd(xType, GetProperties))
                {
                    if (!Equals(prop.GetValue(x), prop.GetValue(y)))
                    {
                        return false;
                    }
                }

                return true;
            }

            public int GetHashCode(object obj)
            {
                var type = obj.GetType();

                if (ShallowComparable(type))
                {
                    return type.GetHashCode();
                }

                var hashCode = type.GetHashCode();
                foreach (var prop in typeProps.GetOrAdd(type, GetProperties))
                {
                    hashCode ^= GetHashCode(prop.GetValue(obj));
                }

                return hashCode;
            }

            private static bool ShallowComparable(Type type)
            {
                return DataType.FromType(type).Kind == DataTypeKinds.Primitive;
            }

            private static List<PropertyInfo> GetProperties(Type type)
            {
                return type.GetProperties().ToList();
            }
        }

        private class StructuralHelper : IEnumerable<KeyValuePair<string, Type>>
        {
            private readonly object _gate = new();
            private readonly StructuralDataTypeKinds _kind;
            private readonly List<KeyValuePair<string, Type>> _properties;
            private readonly Dictionary<string, object> _values;

            public StructuralHelper(StructuralDataTypeKinds kind)
            {
                _kind = kind;
                _properties = new List<KeyValuePair<string, Type>>();
                _values = new Dictionary<string, object>();
            }

            public void Add(string name, Type type, object value)
            {
                if (_type != null)
                {
                    throw new InvalidOperationException("Type already created.");
                }

                _properties.Add(new KeyValuePair<string, Type>(name, type));
                _values.Add(name, value);
            }

            private Type _type;

            public Type Type
            {
                get
                {
                    if (_type == null)
                    {
                        lock (_gate)
                        {
                            if (_type == null)
                            {
                                _type = _kind switch
                                {
                                    StructuralDataTypeKinds.Anonymous => RuntimeCompiler.CreateAnonymousType(this, _properties.Select(p => p.Key).ToArray()),
                                    StructuralDataTypeKinds.Record => RuntimeCompiler.CreateRecordType(this, valueEquality: true),
                                    _ => throw new NotSupportedException(),
                                };
                            }
                        }
                    }

                    return _type;
                }
            }

            private object _instance;

            public object Instance
            {
                get
                {
                    var type = Type;
                    if (_instance == null)
                    {
                        lock (_gate)
                        {
                            if (_instance == null)
                            {
                                _instance = Activator.CreateInstance(type);
                                foreach (var kv in _values)
                                {
                                    var property = type.GetProperty(kv.Key);
                                    property.SetValue(_instance, kv.Value);
                                }
                            }
                        }
                    }

                    return _instance;
                }
            }

            public IEnumerator<KeyValuePair<string, Type>> GetEnumerator() => _properties.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_properties).GetEnumerator();
        }
    }
}
