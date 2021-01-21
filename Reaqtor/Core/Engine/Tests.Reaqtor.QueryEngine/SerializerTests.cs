// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Memory;
using System.Reflection;
using System.Threading.Tasks;

using Reaqtive;
using Reaqtive.Expressions;

using Reaqtor.QueryEngine;
using Reaqtor.Reactive.Expressions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor.QueryEngine
{
    [TestClass]
    public class SerializerTests
    {
        [TestMethod]
        public void Serializer_Arguments()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => _ = new Serializer(null, new Version()), ex => Assert.AreEqual("expressionPolicy", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => _ = new Serializer(DefaultExpressionPolicy.Instance, null), ex => Assert.AreEqual("version", ex.ParamName));
        }

        [TestMethod]
        public void Serializer_UntypedRoundtrips()
        {
            var testCases = new object[]
            {
                null,
                true,
                false,
                byte.MaxValue,
                char.MaxValue,
                double.MinValue,
                short.MinValue,
                int.MinValue,
                long.MinValue,
                sbyte.MinValue,
                float.MinValue,
                ushort.MaxValue,
                uint.MaxValue,
                (ulong)long.MaxValue, // Note, Newtonsoft.Json does not support ulong values greater than this
                "\r\n\t\b\"The quick brown fox...\\\"",
                decimal.MaxValue,
                Guid.NewGuid(),
                TimeSpan.FromTicks(50),
                new DateTime(100),
                new DateTimeOffset(100, TimeSpan.Zero),
            };

            foreach (var testCase in testCases)
            {
                AssertRoundtrip(testCase);
            }
        }

        [TestMethod]
        public void Serializer_TypedRoundtrips()
        {
            AssertRoundtrip(Expression.Constant(42), new ExpressionEqualityComparer());
            AssertRoundtrip(Expression.Parameter(typeof(ISubscribable<int>), "foo"), new ExpressionEqualityComparer(() => new Comparator()));
            AssertRoundtrip<int?>(null);
            AssertRoundtrip<int?>(42);
            AssertRoundtrip(new Uri("http://example.com"));
            AssertRoundtrip<Uri>(null);
        }

        [TestMethod]
        public void Serializer_DictionaryRoundtrips()
        {
            var input = new Dictionary<string, object> { { "foo", 42 } };

            var output = Roundtrip(input, new Serializer(DefaultExpressionPolicy.Instance, SerializerVersioning.v1));
            Assert.AreEqual((long)42, output["foo"]); // Note, Newtonsoft.Json will recover `42` as a long

            output = Roundtrip(input, new Serializer(DefaultExpressionPolicy.Instance, SerializerVersioning.v2));
            Assert.AreEqual(42, output["foo"]);

            output = (Dictionary<string, object>)Roundtrip((object)input, new Serializer(DefaultExpressionPolicy.Instance, SerializerVersioning.v1));
            Assert.AreEqual((long)42, output["foo"]); // Note, Newtonsoft.Json will recover `42` as a long

            output = (Dictionary<string, object>)Roundtrip((object)input, new Serializer(DefaultExpressionPolicy.Instance, SerializerVersioning.v2));
            Assert.AreEqual(42, output["foo"]);
        }

        [TestMethod]
        public void Serializer_Concurrency()
        {
            var testCases = new object[]
            {
                null,
                true,
                false,
                byte.MaxValue,
                char.MaxValue,
                double.MinValue,
                short.MinValue,
                int.MinValue,
                long.MinValue,
                sbyte.MinValue,
                float.MinValue,
                ushort.MaxValue,
                uint.MaxValue,
                (ulong)long.MaxValue, // Note, Newtonsoft.Json does not support ulong values greater than this
                "The quick brown fox...",
                decimal.MaxValue,
                Guid.NewGuid(),
                TimeSpan.FromTicks(50),
                new DateTime(100),
                new DateTimeOffset(100, TimeSpan.Zero),
            };

            var serializer = new Serializer(DefaultExpressionPolicy.Instance, SerializerVersioning.v2);
            Parallel.ForEach(
                Enumerable.Repeat(testCases, 10).SelectMany(x => x),
                testCase =>
                {
                    var result = Roundtrip(testCase, serializer);
                    Assert.IsTrue(
                        EqualityComparer<object>.Default.Equals(testCase, result),
                        string.Format("Expected: {0}\nActual: {1}", testCase, result));
                });
        }

        [TestMethod]
        public void Serializer_QubscribableRoundtrips()
        {
            var input = new QuotedSubscribable<int>(Subscribable.Empty<int>(), ((Expression<Func<ISubscribable<int>>>)(() => Subscribable.Empty<int>())).Body);

            var quotedResult = Roundtrip(input, new Serializer(DefaultExpressionPolicy.Instance, SerializerVersioning.v1));

            Assert.IsNotNull(quotedResult);
            Assert.AreEqual(input.Value.GetType(), quotedResult.Value.GetType());
        }

        [TestMethod]
        public void Serializer_Qubscribable_WithPolicy()
        {
            var input = new QuotedSubscribable<int>(Subscribable.Empty<int>(), ((Expression<Func<ISubscribable<int>>>)(() => Subscribable.Empty<int>())).Body);

            var policy = new TestPolicy();

            Assert.AreEqual(0, policy.ConstantHoisterCalls);
            Assert.AreEqual(0, policy.DelegateCacheCalls);
            Assert.AreEqual(0, policy.InMemoryCacheCalls);
            Assert.AreEqual(0, policy.OutlineCompilationCalls);
            Assert.AreEqual(0, policy.ReflectionProviderCalls);
            Assert.AreEqual(0, policy.ExpressionFactoryCalls);

            var quotedResult = Roundtrip(input, new Serializer(policy, SerializerVersioning.v1));

            Assert.AreEqual(1, policy.ConstantHoisterCalls);
            Assert.AreEqual(1, policy.DelegateCacheCalls);
            Assert.AreEqual(1, policy.InMemoryCacheCalls);
            Assert.AreEqual(1, policy.OutlineCompilationCalls);
            Assert.AreEqual(1, policy.ReflectionProviderCalls);
            Assert.AreEqual(1, policy.ExpressionFactoryCalls);

            Assert.IsNotNull(quotedResult);
            Assert.AreEqual(input.Value.GetType(), quotedResult.Value.GetType());
        }

        [TestMethod]
        public void Serializer_Expression_ManOrBoy()
        {
            var testCases = new Expression[]
            {
                (Expression<Func<int, string>>)(x => !(x > 0) ? ((long)x + 10).ToString() : "\\\t\r\n\f\b\"foo\""),
            };

            AssertRoundtrip(testCases[0], new ExpressionEqualityComparer());
        }

        private static void AssertRoundtrip<T>(T o)
        {
            AssertRoundtrip(o, EqualityComparer<T>.Default);
        }

        private static void AssertRoundtrip<T>(T o, IEqualityComparer<T> comparer)
        {
            AssertRoundtrip(o, comparer, SerializerVersioning.v1);
            AssertRoundtrip(o, comparer, SerializerVersioning.v2);
        }

        private static void AssertRoundtrip<T>(T o, IEqualityComparer<T> comparer, Version version)
        {
            var result = Roundtrip(o, new Serializer(DefaultExpressionPolicy.Instance, version));
            Assert.IsTrue(comparer.Equals(o, result), string.Format("Expected: {0}\nActual {1}", o, result));
        }

        private static T Roundtrip<T>(T o, Serializer serializer)
        {
            var stream = new MemoryStream();
            serializer.Serialize(o, stream);
            stream.Position = 0;
            return serializer.Deserialize<T>(stream);
        }

        private static void AssertRoundtrip(object o)
        {
            AssertRoundtrip(o, EqualityComparer<object>.Default);
        }

        private static void AssertRoundtrip(object o, IEqualityComparer<object> comparer)
        {
            AssertRoundtrip(o, comparer, SerializerVersioning.v1);
            AssertRoundtrip(o, comparer, SerializerVersioning.v2);
        }

        private static void AssertRoundtrip(object o, IEqualityComparer<object> comparer, Version version)
        {
            var result = Roundtrip(o, new Serializer(DefaultExpressionPolicy.Instance, version));
            Assert.IsTrue(comparer.Equals(o, result), string.Format("Expected: {0}\nActual {1}", o, result));
        }

        private static object Roundtrip(object o, Serializer serializer)
        {
            var stream = new MemoryStream();
            var type = o != null ? o.GetType() : typeof(object);
            serializer.Serialize(o, type, stream);
            stream.Position = 0;
            return serializer.Deserialize(type, stream);
        }

        private sealed class Comparator : ExpressionEqualityComparator
        {
            protected override bool EqualsGlobalParameter(ParameterExpression x, ParameterExpression y)
            {
                return x.Name == y.Name && Equals(x.Type, y.Type);
            }
        }

        private sealed class TestPolicy : IExpressionPolicy
        {
            public int OutlineCompilationCalls;
            public int DelegateCacheCalls;
            public int InMemoryCacheCalls;
            public int ConstantHoisterCalls;
            public int ReflectionProviderCalls;
            public int ExpressionFactoryCalls;
            public int LiftMemoizerCalls;
            public int ReduceMemoizerCalls;

            public ICompiledDelegateCache DelegateCache
            {
                get
                {
                    DelegateCacheCalls++;
                    return DefaultExpressionPolicy.Instance.DelegateCache;
                }
            }

            public ICache<Expression> InMemoryCache
            {
                get
                {
                    InMemoryCacheCalls++;
                    return DefaultExpressionPolicy.Instance.InMemoryCache;
                }
            }

            public IConstantHoister ConstantHoister
            {
                get
                {
                    ConstantHoisterCalls++;
                    return DefaultExpressionPolicy.Instance.ConstantHoister;
                }
            }


            public bool OutlineCompilation
            {
                get
                {
                    OutlineCompilationCalls++;
                    return DefaultExpressionPolicy.Instance.OutlineCompilation;
                }
            }

            public IReflectionProvider ReflectionProvider
            {
                get
                {
                    ReflectionProviderCalls++;
                    return DefaultExpressionPolicy.Instance.ReflectionProvider;
                }
            }

            public IExpressionFactory ExpressionFactory
            {
                get
                {
                    ExpressionFactoryCalls++;
                    return DefaultExpressionPolicy.Instance.ExpressionFactory;
                }
            }

            public IMemoizer LiftMemoizer
            {
                get
                {
                    LiftMemoizerCalls++;
                    return DefaultExpressionPolicy.Instance.LiftMemoizer;
                }
            }

            public IMemoizer ReduceMemoizer
            {
                get
                {
                    ReduceMemoizerCalls++;
                    return DefaultExpressionPolicy.Instance.ReduceMemoizer;
                }
            }
        }
    }
}
