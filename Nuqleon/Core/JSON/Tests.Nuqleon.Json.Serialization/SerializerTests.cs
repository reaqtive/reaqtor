// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 04/16/2016 - Created fast JSON serializer functionality.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Nuqleon.Json.Serialization;

namespace Tests
{
    [TestClass]
    public partial class SerializerTests
    {
        [TestMethod]
        public void FastSerializer_Unsupported()
        {
            var pbs = new FastJsonSerializerFactory.EmitterStringBuilder(DefaultNameProvider.Instance);
            var bds = typeof(FastJsonSerializerFactory.EmitterStringBuilder).GetMethod("Build", BindingFlags.Public | BindingFlags.Instance);
#if !NO_IO
            var pbr = new FastJsonSerializerFactory.EmitterWriterBuilder(DefaultNameProvider.Instance);
            var bdr = typeof(FastJsonSerializerFactory.EmitterWriterBuilder).GetMethod("Build", BindingFlags.Public | BindingFlags.Instance);
#endif
            var create = typeof(FastJsonSerializerFactory).GetMethod("CreateSerializer", new[] { typeof(INameProvider), typeof(FastJsonConcurrencyMode) });

            foreach (var type in new[] { typeof(E), typeof(E?), typeof(C), typeof(int[,]), typeof(D), typeof(I), typeof(G<int>), typeof(IDictionary<int, int>) })
            {
                AssertNotSupportedException(create, type, null, null, FastJsonConcurrencyMode.SingleThreaded);

                AssertNotSupportedException(bds, type, pbs);
#if !NO_IO
                AssertNotSupportedException(bdr, type, pbr);
#endif
            }
        }

        private static void AssertNotSupportedException(MethodInfo method, Type type, object instance, params object[] args)
        {
            try
            {
                method.MakeGenericMethod(type).Invoke(instance, args);
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException is NotSupportedException)
                {
                    return;
                }
            }

            Assert.Fail("Expected exception for type " + type);
        }

        [TestMethod]
        public void FastSerializer_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => FastJsonSerializerFactory.CreateSerializer<int>(provider: null, settings: null));
        }

        [TestMethod]
        public void FastSerializer_Boolean()
        {
            AssertSerialize(new Asserts<bool>
            {
                { false, "false" },
                { true, "true" },
            });
        }

        [TestMethod]
        public void FastSerializer_NullableBoolean()
        {
            AssertSerialize(new Asserts<bool?>
            {
                { null, "null" },
                { false, "false" },
                { true, "true" },
            });
        }

        [TestMethod]
        public void FastSerializer_Char()
        {
            AssertSerialize(new Asserts<char>
            {
                { 'a', "\"a\"" },
                { '\t', "\"\\t\"" },
            });
        }

        [TestMethod]
        public void FastSerializer_NullableChar()
        {
            AssertSerialize(new Asserts<char?>
            {
                { null, "null" },
                { 'a', "\"a\"" },
                { '\t', "\"\\t\"" },
            });
        }

        [TestMethod]
        public void FastSerializer_Single()
        {
            AssertSerialize(new Asserts<float>
            {
                { 12.34f, "12.34" },
                { -987f, "-987" },
            });
        }

        [TestMethod]
        public void FastSerializer_NullableSingle()
        {
            AssertSerialize(new Asserts<float?>
            {
                { null, "null" },
                { 12.34f, "12.34" },
                { -987f, "-987" },
            });
        }

        [TestMethod]
        public void FastSerializer_Double()
        {
            AssertSerialize(new Asserts<double>
            {
                { 12.34d, "12.34" },
                { -987d, "-987" },
            });
        }

        [TestMethod]
        public void FastSerializer_NullableDouble()
        {
            AssertSerialize(new Asserts<double?>
            {
                { null, "null" },
                { 12.34d, "12.34" },
                { -987d, "-987" },
            });
        }

        [TestMethod]
        public void FastSerializer_Decimal()
        {
            AssertSerialize(new Asserts<decimal>
            {
                { 12.34m, "12.34" },
                { -987m, "-987" },
            });
        }

        [TestMethod]
        public void FastSerializer_NullableDecimal()
        {
            AssertSerialize(new Asserts<decimal?>
            {
                { null, "null" },
                { 12.34m, "12.34" },
                { -987m, "-987" },
            });
        }

        [TestMethod]
        public void FastSerializer_DateTime()
        {
            AssertSerialize(new Asserts<DateTime>
            {
                { new DateTime(2016, 2, 11, 12, 34, 56, 789, DateTimeKind.Utc), "\"2016-02-11T12:34:56.789Z\"" },
            });
        }

        [TestMethod]
        public void FastSerializer_NullableDateTime()
        {
            AssertSerialize(new Asserts<DateTime?>
            {
                { null, "null" },
                { new DateTime(2016, 2, 11, 12, 34, 56, 789, DateTimeKind.Utc), "\"2016-02-11T12:34:56.789Z\"" },
            });
        }

        [TestMethod]
        public void FastSerializer_DateTimeOffset()
        {
            AssertSerialize(new Asserts<DateTimeOffset>
            {
                { new DateTimeOffset(2016, 2, 11, 12, 34, 56, 789, TimeSpan.Zero), "\"2016-02-11T12:34:56.789Z\"" },
                { new DateTimeOffset(2016, 2, 11, 12, 34, 56, 789, new TimeSpan(8, 0, 0)), "\"2016-02-11T12:34:56.789+08:00\"" },
            });
        }

        [TestMethod]
        public void FastSerializer_NullableDateTimeOffset()
        {
            AssertSerialize(new Asserts<DateTimeOffset?>
            {
                { null, "null" },
                { new DateTimeOffset(2016, 2, 11, 12, 34, 56, 789, TimeSpan.Zero), "\"2016-02-11T12:34:56.789Z\"" },
                { new DateTimeOffset(2016, 2, 11, 12, 34, 56, 789, new TimeSpan(8, 0, 0)), "\"2016-02-11T12:34:56.789+08:00\"" },
            });
        }

        [TestMethod]
        public void FastSerializer_String()
        {
            AssertSerialize(new Asserts<string>
            {
                { null, "null" },
                { "", "\"\"" },
                { "bar", "\"bar\"" },
                { "bar\tfoo", "\"bar\\tfoo\"" },
            });
        }

        [TestMethod]
        public void FastSerializer_Int32Array()
        {
            AssertSerialize<int[]>(new Asserts<int[]>
            {
                { null, "null" },
                { Array.Empty<int>(), "[]" },
                { new[] { 2 }, "[2]" },
                { new[] { 2, 3 }, "[2,3]" },
                { new[] { 2, 3, 5 }, "[2,3,5]" },
            });

            AssertSerialize<IList<int>>(new Asserts<IList<int>>
            {
                { null, "null" },
                { Array.Empty<int>(), "[]" },
                { new[] { 2 }, "[2]" },
                { new[] { 2, 3 }, "[2,3]" },
                { new[] { 2, 3, 5 }, "[2,3,5]" },
            });
        }

        [TestMethod]
        public void FastSerializer_Int32List()
        {
            AssertSerialize<List<int>>(new Asserts<List<int>>
            {
                { null, "null" },
                { new List<int>(), "[]" },
                { new List<int> { 2 }, "[2]" },
                { new List<int> { 2, 3 }, "[2,3]" },
                { new List<int> { 2, 3, 5 }, "[2,3,5]" },
            });

            AssertSerialize<IList<int>>(new Asserts<IList<int>>
            {
                { null, "null" },
                { new List<int>(), "[]" },
                { new List<int> { 2 }, "[2]" },
                { new List<int> { 2, 3 }, "[2,3]" },
                { new List<int> { 2, 3, 5 }, "[2,3,5]" },
            });

            AssertSerialize<IReadOnlyList<int>>(new Asserts<IReadOnlyList<int>>
            {
                { null, "null" },
                { new List<int>(), "[]" },
                { new List<int> { 2 }, "[2]" },
                { new List<int> { 2, 3 }, "[2,3]" },
                { new List<int> { 2, 3, 5 }, "[2,3,5]" },
            });
        }

        [TestMethod]
        public void FastSerializer_Empty()
        {
            AssertSerialize<Empty>(new Asserts<Empty>
            {
                { null, "null" },
                { new Empty(), "{}" },
            });
        }

        [TestMethod]
        public void FastSerializer_PersonClass()
        {
            AssertSerialize<PersonClass>(new Asserts<PersonClass>
            {
                { null, "null" },
                { new PersonClass(), """{"Age":0,"Name":null,"Qux":0}""" },
                { new PersonClass { Age = 21, Name = "Bart" }, """{"Age":21,"Name":"Bart","Qux":0}""" },
            });
        }

        [TestMethod]
        public void FastSerializer_PersonStruct()
        {
            AssertSerialize<PersonStruct>(new Asserts<PersonStruct>
            {
                { new PersonStruct(), """{"Age":0,"Name":null}""" },
                { new PersonStruct { Age = 21, Name = "Bart" }, """{"Age":21,"Name":"Bart"}""" },
            });
        }

        [TestMethod]
        public void FastSerializer_NullablePersonStruct()
        {
            AssertSerialize<PersonStruct?>(new Asserts<PersonStruct?>
            {
                { null, "null" },
                { new PersonStruct(), """{"Age":0,"Name":null}""" },
                { new PersonStruct { Age = 21, Name = "Bart" }, """{"Age":21,"Name":"Bart"}""" },
            });
        }

        [TestMethod]
        public void FastSerializer_PersonWithParent()
        {
            var homer = new PersonWithParent { Name = "Homer", Age = 40 };
            var bart = new PersonWithParent { Age = 21, Name = "Bart", Parent = homer };
            var lisa = new PersonWithParent { Age = 16, Name = "Lisa", Parent = homer };

            AssertSerialize<PersonWithParent>(new Asserts<PersonWithParent>
            {
                { new PersonWithParent(), """{"Age":0,"Name":null,"Parent":null}""" },
                { bart, """{"Age":21,"Name":"Bart","Parent":{"Age":40,"Name":"Homer","Parent":null}}""" },
                { lisa, """{"Age":16,"Name":"Lisa","Parent":{"Age":40,"Name":"Homer","Parent":null}}""" },
            });

            AssertSerialize<PersonWithParent[]>(new Asserts<PersonWithParent[]>
            {
                { new[] { bart, lisa }, """[{"Age":21,"Name":"Bart","Parent":{"Age":40,"Name":"Homer","Parent":null}},{"Age":16,"Name":"Lisa","Parent":{"Age":40,"Name":"Homer","Parent":null}}]""" },
            });
        }

        [TestMethod]
        public void FastSerializer_PersonWithParent_CycleDetection()
        {
            var ser = FastJsonSerializerFactory.CreateSerializer<PersonWithParent>(provider: null, FastJsonConcurrencyMode.SingleThreaded);

            var cyclic1 = new PersonWithParent();
            cyclic1.Parent = cyclic1;

            var cyclic2 = new PersonWithParent { Parent = new PersonWithParent() };
            cyclic2.Parent.Parent = cyclic2;

            Assert.ThrowsException<InvalidOperationException>(() => ser.Serialize(cyclic1));
            Assert.ThrowsException<InvalidOperationException>(() => ser.Serialize(cyclic2));
        }

        [TestMethod]
        public void FastSerializer_Dictionary()
        {
            AssertSerialize<Dictionary<string, int>>(new Asserts<Dictionary<string, int>>
            {
                { null, "null" },
                { new Dictionary<string, int> { }, "{}" },
                { new Dictionary<string, int> { { "bar", 42 } }, """{"bar":42}""" },
                { new Dictionary<string, int> { { "bar", 42 }, { "foo", 43 } }, """{"bar":42,"foo":43}""" },
                { new Dictionary<string, int> { { "bar", 42 }, { "foo", 43 }, { "qux", 44 } }, """{"bar":42,"foo":43,"qux":44}""" },
            });

            AssertSerialize<IDictionary<string, int>>(new Asserts<IDictionary<string, int>>
            {
                { null, "null" },
                { new Dictionary<string, int> { }, "{}" },
                { new Dictionary<string, int> { { "bar", 42 } }, """{"bar":42}""" },
                { new Dictionary<string, int> { { "bar", 42 }, { "foo", 43 } }, """{"bar":42,"foo":43}""" },
                { new Dictionary<string, int> { { "bar", 42 }, { "foo", 43 }, { "qux", 44 } }, """{"bar":42,"foo":43,"qux":44}""" },
            });

            AssertSerialize<IReadOnlyDictionary<string, int>>(new Asserts<IReadOnlyDictionary<string, int>>
            {
                { null, "null" },
                { new Dictionary<string, int> { }, "{}" },
                { new Dictionary<string, int> { { "bar", 42 } }, """{"bar":42}""" },
                { new Dictionary<string, int> { { "bar", 42 }, { "foo", 43 } }, """{"bar":42,"foo":43}""" },
                { new Dictionary<string, int> { { "bar", 42 }, { "foo", 43 }, { "qux", 44 } }, """{"bar":42,"foo":43,"qux":44}""" },
            });
        }

        [TestMethod]
        public void FastSerializer_Dictionary_Custom()
        {
            AssertSerialize<IDictionary<string, int>>(new Asserts<IDictionary<string, int>>
            {
                { new NullableDictionary<string, int> { { "bar", 42 }, { "foo", 43 } }, """{"bar":42,"foo":43}""" },
            });
        }

        [TestMethod]
        public void FastSerializer_Dictionary_Custom_ArgumentChecking()
        {
            var ser = FastJsonSerializerFactory.CreateSerializer<IDictionary<string, int>>(provider: null, FastJsonConcurrencyMode.SingleThreaded);

            Assert.ThrowsException<InvalidOperationException>(() => ser.Serialize(new NullableDictionary<string, int> { { null, 42 } }));
        }

        [TestMethod]
        public void FastSerializer_Any()
        {
            AssertSerialize<object>(new Asserts<object>
            {
                { null, "null" },

                { (sbyte)42, "42" },
                { (byte)42, "42" },
                { (short)42, "42" },
                { (ushort)42, "42" },
                { 42, "42" },
                { (uint)42, "42" },
                { (long)42, "42" },
                { (ulong)42, "42" },

                { (float)42, "42" },
                { (double)42, "42" },
                { (decimal)42, "42" },

                { true, "true" },

                { 'a', "\"a\"" },
                { "bar", "\"bar\"" },

                { new DateTime(2016, 2, 11, 12, 34, 56, 789, DateTimeKind.Utc), "\"2016-02-11T12:34:56.789Z\"" },
                { new DateTimeOffset(2016, 2, 11, 12, 34, 56, 789, new TimeSpan(8, 0, 0)), "\"2016-02-11T12:34:56.789+08:00\"" },

                { new int[] { 1, 2, 3 }, "[1,2,3]" },
                { new object[] { 1, true, "bar", null }, """[1,true,"bar",null]""" },

                { new PersonStruct { Age = 21, Name = "Bart" }, """{"Age":21,"Name":"Bart"}""" },
                { new PersonClass { Age = 21, Name = "Bart" }, """{"Age":21,"Name":"Bart","Qux":0}""" },
            });
        }

        [TestMethod]
        public void FastSerializer_Any_ArgumentChecking()
        {
            var ser = FastJsonSerializerFactory.CreateSerializer<object>(provider: null, FastJsonConcurrencyMode.SingleThreaded);

            Assert.ThrowsException<NotSupportedException>(() => ser.Serialize(new int[1, 1]));
#if !NO_IO
            Assert.ThrowsException<NotSupportedException>(() => ser.Serialize(new int[1, 1], new System.IO.StringWriter()));
#endif
        }

        private static void AssertSerialize<T>(Asserts<T> asserts)
        {
            foreach (var ser in new[]
            {
                FastJsonSerializerFactory.CreateSerializer<T>(provider: null, FastJsonConcurrencyMode.SingleThreaded),
                FastJsonSerializerFactory.CreateSerializer<T>(provider: null, FastJsonConcurrencyMode.ThreadSafe),
            })
            {
                foreach (var kv in asserts)
                {
                    Assert.AreEqual(kv.Value, ser.Serialize(kv.Key));
                }

                if (asserts._null != null)
                {
                    Assert.AreEqual(asserts._null, ser.Serialize(default));
                }

#if !NO_IO
                foreach (var kv in asserts)
                {
                    var sw = new System.IO.StringWriter();
                    ser.Serialize(kv.Key, sw);
                    Assert.AreEqual(kv.Value, sw.ToString());
                }

                if (asserts._null != null)
                {
                    var sw = new System.IO.StringWriter();
                    ser.Serialize(default, sw);
                    Assert.AreEqual(asserts._null, sw.ToString());
                }
#endif
            }
        }

        private sealed class Asserts<T> : Dictionary<T, string>
        {
            public string _null;

            public new void Add(T value, string s)
            {
                if (value == null)
                {
                    _null = s;
                }
                else
                {
                    this[value] = s;
                }
            }
        }

        private sealed class Empty
        {
        }

#pragma warning disable IDE0079 // Remove unnecessary suppression (only on .NET 5)
#pragma warning disable CA1822 // Mark members as static
        private sealed class PersonClass : IEquatable<PersonClass>
        {
            // NB: These properties and fields cannot be set.
            public readonly int Bar = 42;
            public const int Foo = 42;
            public int Qux => 0;
            public int this[int x] { get => 0; set { } }
            public int Baz { set { } }

            public string Name { get; set; }
            public int Age;

            public override bool Equals(object obj)
            {
                return Equals(obj as PersonClass);
            }

            public override int GetHashCode()
            {
                return 0;
            }

            public bool Equals(PersonClass other)
            {
                return other != null && other.Name == Name && other.Age == Age;
            }
        }
#pragma warning restore CA1822
#pragma warning restore IDE0079

        private struct PersonStruct : IEquatable<PersonStruct>
        {
            public string Name;
            public int Age { get; set; }

            public override bool Equals(object obj)
            {
                return obj is PersonStruct person && Equals(person);
            }

            public override int GetHashCode()
            {
                return 0;
            }

            public bool Equals(PersonStruct other)
            {
                return other.Name == Name && other.Age == Age;
            }
        }

        private sealed class PersonWithParent : IEquatable<PersonWithParent>
        {
            public string Name { get; set; }
            public int Age;
            public PersonWithParent Parent { get; set; }

            public override bool Equals(object obj)
            {
                return Equals(obj as PersonWithParent);
            }

            public override int GetHashCode()
            {
                return 0;
            }

            public bool Equals(PersonWithParent other)
            {
                return other != null && other.Name == Name && other.Age == Age && EqualityComparer<PersonWithParent>.Default.Equals(other.Parent, Parent);
            }
        }

        private enum E { }
        private abstract class C { }
        private interface I { }
        private delegate void D();
        private sealed class G<T> { }

        private sealed class MyProvider : INameProvider
        {
            public string GetName(FieldInfo field)
            {
                var attr = field.GetCustomAttribute<IdentifierAttribute>();
                if (attr != null)
                {
                    return attr.Id;
                }

                return field.Name;
            }

            public string GetName(PropertyInfo property)
            {
                var attr = property.GetCustomAttribute<IdentifierAttribute>();
                if (attr != null)
                {
                    return attr.Id;
                }

                return property.Name;
            }
        }

        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
        private sealed class IdentifierAttribute : Attribute
        {
            public IdentifierAttribute(string id)
            {
                Id = id;
            }

            public string Id;
        }

        private sealed class NullableDictionary<K, V> : Dictionary<K, V>, IEnumerable<KeyValuePair<K, V>>
            where K : class
        {
            //
            // NB: Only implements what's needed for the test.
            //

            private bool _hasNullValue = false;
            private V _nullValue;

            public new void Add(K key, V value)
            {
                if (key == null)
                {
                    if (_hasNullValue)
                        throw new InvalidOperationException();

                    _nullValue = value;
                    _hasNullValue = true;

                    return;
                }

                base.Add(key, value);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable<KeyValuePair<K, V>>)this).GetEnumerator();
            }

            IEnumerator<KeyValuePair<K, V>> IEnumerable<KeyValuePair<K, V>>.GetEnumerator()
            {
                if (_hasNullValue)
                {
                    yield return new KeyValuePair<K, V>(key: null, _nullValue);
                }

                using var e = GetEnumerator();

                while (e.MoveNext())
                {
                    yield return e.Current;
                }
            }
        }
    }
}
