// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2014 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Linq.Expressions.Bonsai.Serialization.Binary;
using System.Threading;

#if USE_SLIM
using ExpressionFactory = System.Linq.Expressions.ExpressionSlimFactory;
#else
using Microsoft.CSharp.RuntimeBinder;
#endif

namespace Tests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void UInt32Compact()
        {
            var values = new[]
            {
                0x0U,
                0x0U + 1,
                0x7FU - 1,
                0x7FU,
                0x7FU + 1,
                0x3FFFU - 1,
                0x3FFFU,
                0x3FFFU + 1,
                0x1FFFFFU - 1,
                0x1FFFFFU,
                0x1FFFFFU + 1,
                0xFFFFFFFU - 1,
                0xFFFFFFFU,
                0xFFFFFFFU + 1,
                uint.MaxValue,
            };

            foreach (var i in values)
            {
                var ms = new MemoryStream();
                ms.WriteUInt32Compact(i);

                ms.Position = 0;
                var r = ms.ReadUInt32Compact();

                Assert.AreEqual(r, i);
            }
        }

#if !USE_SLIM
        [TestMethod]
        public void NotSupportedExpressions()
        {
            var binder = Binder.Convert(CSharpBinderFlags.None, typeof(int), typeof(Tests));

            var es = new Expression[]
            {
                Expression.DebugInfo(Expression.SymbolDocument("foo"), 1, 2, 3, 4),
                Expression.Dynamic(binder, typeof(int), Expression.Constant(42)),
                new MyNode(),
            };

            foreach (var e in es)
            {
                Assert.ThrowsException<NotSupportedException>(() =>
                {
                    var ser = new ExpressionSerializer(new BinaryObjectSerializer(), ExpressionFactory.Instance);
                    ser.Serialize(e);
                });
            }
        }

        private sealed class MyNode : Expression
        {
            public override ExpressionType NodeType
            {
                get
                {
                    return ExpressionType.Extension;
                }
            }
        }
#endif

#if !USE_SLIM // REVIEW: Add by-ref type support in Bonsai.
        [TestMethod]
        public void Roundtrip_NonTrivial1()
        {
            var x = Expression.Parameter(typeof(int));
            var i = typeof(Interlocked).GetMethods().Single(m => m.Name == "Exchange" && m.GetParameters().Last().ParameterType == typeof(int));

            var b = Expression.Block(new[] { x }, Expression.Call(instance: null, i, x, Expression.Constant(1)));

            AssertRoundtrip(new Expression[]
            {
                b,
            });
        }
#endif

        [TestMethod]
        public void Roundtrip_NonTrivial2()
        {
            AssertRoundtrip(new Expression[]
            {
                ((Expression<Func<Tests>>)(() => Activator.CreateInstance<Tests>())).Body,
            });
        }

        [TestMethod]
        public void Roundtrip_NonTrivial3()
        {
            AssertRoundtrip(new Expression[]
            {
                ((Expression<Func<IEnumerable<int>>>)(() => Enumerable.Range(0, 10))).Body,
                ((Expression<Func<IEnumerable<int>>>)(() => Enumerable.Range(0, 10).Where(x => x > 0))).Body,
                ((Expression<Func<IEnumerable<int>>>)(() => Enumerable.Range(0, 10).Where((x, i) => x > i))).Body,
            });
        }

        [TestMethod]
        public void Roundtrip_LotsOfTypes()
        {
            var pts = typeof(int).Assembly.GetTypes().Where(t => t.IsPublic && t != typeof(void) && !t.IsGenericTypeDefinition).ToArray();

            var ps = pts.Select(pt => Expression.Parameter(pt)).ToArray();

            var b = Expression.Block(ps, Expression.Empty());

            AssertRoundtrip(new Expression[]
            {
                b,
            });
        }

        [TestMethod]
        public void Roundtrip_Binary1()
        {
            AssertRoundtrip(new Expression[]
            {
                Expression.ArrayIndex(Expression.Default(typeof(int[])), Expression.Default(typeof(int))),
                Expression.Assign(Expression.Parameter(typeof(int)), Expression.Default(typeof(int))),
            });
        }

        [TestMethod]
        public void Roundtrip_Binary2()
        {
            AssertRoundtrip(new Expression[]
            {
                Expression.Add(Expression.Constant(1), Expression.Constant(2)),
                Expression.AddChecked(Expression.Constant(1), Expression.Constant(2)),
                Expression.Subtract(Expression.Constant(1), Expression.Constant(2)),
                Expression.SubtractChecked(Expression.Constant(1), Expression.Constant(2)),
                Expression.Multiply(Expression.Constant(1), Expression.Constant(2)),
                Expression.MultiplyChecked(Expression.Constant(1), Expression.Constant(2)),
                Expression.Divide(Expression.Constant(1), Expression.Constant(2)),
                Expression.Modulo(Expression.Constant(1), Expression.Constant(2)),
                Expression.Power(Expression.Default(typeof(double)), Expression.Default(typeof(double))),
                Expression.And(Expression.Constant(1), Expression.Constant(2)),
                Expression.Or(Expression.Constant(1), Expression.Constant(2)),
                Expression.ExclusiveOr(Expression.Constant(1), Expression.Constant(2)),
                Expression.And(Expression.Constant(false), Expression.Constant(true)),
                Expression.AndAlso(Expression.Constant(false), Expression.Constant(true)),
                Expression.Or(Expression.Constant(false), Expression.Constant(true)),
                Expression.OrElse(Expression.Constant(false), Expression.Constant(true)),
                Expression.ExclusiveOr(Expression.Constant(false), Expression.Constant(true)),
                Expression.LeftShift(Expression.Constant(1), Expression.Constant(2)),
                Expression.RightShift(Expression.Constant(1), Expression.Constant(2)),

                // TODO: with methods
                Expression.Add(Expression.Default(typeof(MyObj)), Expression.Default(typeof(MyObj))),
            });
        }

        [TestMethod]
        public void Roundtrip_Binary3()
        {
            AssertRoundtrip(new Expression[]
            {
                Expression.LessThan(Expression.Constant(1), Expression.Constant(2)),
                Expression.LessThanOrEqual(Expression.Constant(1), Expression.Constant(2)),
                Expression.GreaterThan(Expression.Constant(1), Expression.Constant(2)),
                Expression.GreaterThanOrEqual(Expression.Constant(1), Expression.Constant(2)),
                Expression.Equal(Expression.Constant(1), Expression.Constant(2)),
                Expression.NotEqual(Expression.Constant(1), Expression.Constant(2)),

                Expression.LessThan(Expression.Constant(1, typeof(int?)), Expression.Constant(2, typeof(int?))),
                Expression.LessThanOrEqual(Expression.Constant(1, typeof(int?)), Expression.Constant(2, typeof(int?))),
                Expression.GreaterThan(Expression.Constant(1, typeof(int?)), Expression.Constant(2, typeof(int?))),
                Expression.GreaterThanOrEqual(Expression.Constant(1, typeof(int?)), Expression.Constant(2, typeof(int?))),
                Expression.Equal(Expression.Constant(1, typeof(int?)), Expression.Constant(2, typeof(int?))),
                Expression.NotEqual(Expression.Constant(1, typeof(int?)), Expression.Constant(2, typeof(int?))),

                // TODO: with methods
                Expression.Equal(Expression.Default(typeof(MyObj)), Expression.Default(typeof(MyObj))),
                Expression.NotEqual(Expression.Default(typeof(MyObj)), Expression.Default(typeof(MyObj))),

                Expression.Equal(Expression.Default(typeof(MyVal)), Expression.Default(typeof(MyVal))),
                Expression.NotEqual(Expression.Default(typeof(MyVal)), Expression.Default(typeof(MyVal))),

                // lifting
                Expression.Equal(Expression.Default(typeof(TimeSpan?)), Expression.Default(typeof(TimeSpan?)), liftToNull: true, typeof(TimeSpan).GetMethod("op_Equality"))
            });
        }

        [TestMethod]
        public void Roundtrip_Binary4()
        {
            var x = Expression.Parameter(typeof(string));

            AssertRoundtrip(new Expression[]
            {
                Expression.Coalesce(Expression.Default(typeof(string)), Expression.Default(typeof(string))),
                Expression.Coalesce(Expression.Default(typeof(string)), Expression.Default(typeof(string)), Expression.Lambda(x, x)),
            });
        }

        [TestMethod]
        public void Roundtrip_Binary5()
        {
            var p = Expression.Parameter(typeof(MyObj));

            AssertRoundtrip(new Expression[]
            {
                Expression.AddAssign(Expression.Parameter(typeof(int)), Expression.Default(typeof(int))),
                Expression.AddAssignChecked(Expression.Parameter(typeof(int)), Expression.Default(typeof(int))),
                Expression.SubtractAssign(Expression.Parameter(typeof(int)), Expression.Default(typeof(int))),
                Expression.SubtractAssignChecked(Expression.Parameter(typeof(int)), Expression.Default(typeof(int))),
                Expression.MultiplyAssign(Expression.Parameter(typeof(int)), Expression.Default(typeof(int))),
                Expression.MultiplyAssignChecked(Expression.Parameter(typeof(int)), Expression.Default(typeof(int))),
                Expression.DivideAssign(Expression.Parameter(typeof(int)), Expression.Default(typeof(int))),
                Expression.ModuloAssign(Expression.Parameter(typeof(int)), Expression.Default(typeof(int))),
                Expression.PowerAssign(Expression.Parameter(typeof(double)), Expression.Default(typeof(double))),
                Expression.AndAssign(Expression.Parameter(typeof(int)), Expression.Default(typeof(int))),
                Expression.OrAssign(Expression.Parameter(typeof(int)), Expression.Default(typeof(int))),
                Expression.ExclusiveOrAssign(Expression.Parameter(typeof(int)), Expression.Default(typeof(int))),
                Expression.RightShiftAssign(Expression.Parameter(typeof(int)), Expression.Default(typeof(int))),
                Expression.LeftShiftAssign(Expression.Parameter(typeof(int)), Expression.Default(typeof(int))),

                // TODO: with conversions
                Expression.AddAssign(Expression.Parameter(typeof(MyObj)), Expression.Default(typeof(MyObj)), method: null, Expression.Lambda(p, p)),

                // TODO: with methods
                Expression.AddAssign(Expression.Parameter(typeof(MyObj)), Expression.Default(typeof(MyObj))),
            });
        }

        [TestMethod]
        public void Roundtrip_Block()
        {
            var x = Expression.Parameter(typeof(int));
            var y = Expression.Parameter(typeof(int));
            var z = Expression.Parameter(typeof(int));

            AssertRoundtrip(new Expression[]
            {
                Expression.Block(Expression.Constant(1)),
                Expression.Block(Expression.Constant(1), Expression.Constant(2)),
                Expression.Block(Expression.Constant(1), Expression.Constant(2), Expression.Constant(3)),

                Expression.Block(Array.Empty<ParameterExpression>(), Expression.Constant(1)),
                Expression.Block(Array.Empty<ParameterExpression>(), Expression.Constant(1), Expression.Constant(2)),
                Expression.Block(Array.Empty<ParameterExpression>(), Expression.Constant(1), Expression.Constant(2), Expression.Constant(3)),

                Expression.Block(new ParameterExpression[] { x }, x),
                Expression.Block(new ParameterExpression[] { x, y }, x, y),
                Expression.Block(new ParameterExpression[] { x, y, z }, x, y, z),

                Expression.Block(typeof(int), new ParameterExpression[] { x }, x),
                Expression.Block(typeof(int), new ParameterExpression[] { x, y }, x, y),
                Expression.Block(typeof(int), new ParameterExpression[] { x, y, z }, x, y, z),

                Expression.Block(typeof(void), new ParameterExpression[] { x }, x),
                Expression.Block(typeof(void), new ParameterExpression[] { x, y }, x, y),
                Expression.Block(typeof(void), new ParameterExpression[] { x, y, z }, x, y, z),
            });
        }

        [TestMethod]
        public void Roundtrip_Call()
        {
            AssertRoundtrip(new Expression[]
            {
                ((Expression<Action>)(() => Console.WriteLine())).Body,
                ((Expression<Action>)(() => Console.WriteLine(42))).Body,
                ((Expression<Action>)(() => Console.WriteLine("foo"))).Body,
                ((Expression<Func<string>>)(() => Console.ReadLine())).Body,

                ((Expression<Func<string, string>>)(s => s.ToUpper())).Body,

#pragma warning disable IDE0057 // Substring can be simplified. (https://github.com/dotnet/roslyn/issues/49347)
                ((Expression<Func<string, string>>)(s => s.Substring(1))).Body,
                ((Expression<Func<string, string>>)(s => s.Substring(1, 2))).Body,
#pragma warning restore IDE0057

                // TODO: generic methods
            });
        }

        [TestMethod]
        public void Roundtrip_Conditional()
        {
            AssertRoundtrip(new Expression[]
            {
                Expression.Condition(Expression.Default(typeof(bool)), Expression.Constant(1), Expression.Constant(2)),
                Expression.Condition(Expression.Default(typeof(bool)), Expression.Constant(1), Expression.Constant(2), typeof(int)),
                Expression.IfThen(Expression.Default(typeof(bool)), Expression.Default(typeof(int))),
                Expression.IfThenElse(Expression.Default(typeof(bool)), Expression.Constant(1), Expression.Constant(2)),
            });
        }

        [TestMethod]
        public void Roundtrip_Constant()
        {
            AssertRoundtrip(new Expression[]
            {
                Expression.Constant((byte)42),
                Expression.Constant((sbyte)42),
                Expression.Constant((short)42),
                Expression.Constant((ushort)42),
                Expression.Constant(42),
                Expression.Constant(42U),
                Expression.Constant(42L),
                Expression.Constant(42UL),
                Expression.Constant(42.0),
                Expression.Constant(42.0f),
                Expression.Constant(42m),
                Expression.Constant('a'),
                Expression.Constant("bar"),
                Expression.Constant(DateTime.Now),
                Expression.Constant(value: null, typeof(int?)),
                Expression.Constant(value: null, typeof(string)),
                Expression.Constant(value: null, typeof(object)),
                Expression.Constant(new[] { 1, 2, 3 }),
                Expression.Constant(new int?[] { 1, null, 3 }),
            });
        }

        [TestMethod]
        public void Roundtrip_Default()
        {
            AssertRoundtrip(new Expression[]
            {
                Expression.Default(typeof(int)),
                Expression.Default(typeof(int?)),
                Expression.Default(typeof(int[])),
                Expression.Default(typeof(int?[])),
                Expression.Default(typeof(string)),
                Expression.Default(typeof(object)),

                Expression.Default(typeof(MyObj[])),
                Expression.Default(typeof(MyObj[,])),
                Expression.Default(typeof(MyObj[,,])),

                Expression.Default(typeof(List<int>)),

                Expression.Default(typeof(Guid)),
                Expression.Default(typeof(DateTimeOffset)),
                Expression.Default(typeof(TimeSpan)),

                Expression.Default(typeof(Guid?)),
                Expression.Default(typeof(DateTimeOffset?)),
                Expression.Default(typeof(TimeSpan?)),

                Expression.Default(typeof(Guid[])),
                Expression.Default(typeof(DateTimeOffset[])),
                Expression.Default(typeof(TimeSpan[])),

                Expression.Default(typeof(Guid?[])),
                Expression.Default(typeof(DateTimeOffset?[])),
                Expression.Default(typeof(TimeSpan?[])),

                Expression.Default(typeof(Uri)),
            });
        }

        [TestMethod]
        public void Roundtrip_Goto()
        {
            AssertRoundtrip(new Expression[]
            {
                Expression.Goto(Expression.Label("foo")),
                Expression.Goto(Expression.Label(typeof(int), "foo"), Expression.Constant(42)),
                Expression.Goto(Expression.Label(typeof(int), "foo"), Expression.Constant(42), typeof(int)),

                Expression.Return(Expression.Label("foo")),
                Expression.Return(Expression.Label("foo"), typeof(void)),
                Expression.Return(Expression.Label(typeof(int), "foo"), Expression.Constant(42)),
                Expression.Return(Expression.Label(typeof(int), "foo"), Expression.Constant(42), typeof(int)),
            });
        }

        [TestMethod]
        public void Roundtrip_Index()
        {
            AssertRoundtrip(new Expression[]
            {
                Expression.MakeIndex(Expression.Default(typeof(Indexed)), typeof(Indexed).GetProperty("Item"), new Expression[] { Expression.Constant(1) }),
            });
        }

        private sealed class Indexed
        {
#pragma warning disable CA1822 // Mark members as static (https://github.com/dotnet/roslyn-analyzers/issues/4651)
            public string this[int x] => "";
#pragma warning restore CA1822
        }

        [TestMethod]
        public void Roundtrip_Invocation()
        {
            var x = Expression.Parameter(typeof(int));

            AssertRoundtrip(new Expression[]
            {
                Expression.Invoke(Expression.Lambda(Expression.Constant(1))),
                Expression.Invoke(Expression.Lambda(x, x), Expression.Constant(1)),
            });
        }

        [TestMethod]
        public void Roundtrip_Label()
        {
            AssertRoundtrip(new Expression[]
            {
                Expression.Label(Expression.Label("foo")),
                Expression.Label(Expression.Label(typeof(int)), Expression.Constant(42)),
                Expression.Label(Expression.Label(typeof(int), "foo"), Expression.Constant(42)),
            });
        }

        [TestMethod]
        public void Roundtrip_Lambda()
        {
            var x = Expression.Parameter(typeof(int));
            var y = Expression.Parameter(typeof(int));

            int N = 20;

            if (Type.GetType("Mono.Runtime") != null)
            {
                //
                // NB: On Mono, creating a LambdaExpression of an arity >= 16 causes creation of new delegate types
                //     that are not cached and reused. As such, the resulting delegate type differs upon roundtripping.
                //

                N = 15;
            }

            var ps = Enumerable.Range(0, N).Select(i => Expression.Parameter(typeof(int), "p" + i)).ToArray();

            AssertRoundtrip(new Expression[]
            {
                Expression.Lambda(Expression.Constant(1)),
                Expression.Lambda(x, x),
                Expression.Lambda(Expression.Add(x, y), x, y),
                Expression.Lambda(Expression.Add(ps[0], ps[7]), ps),
                Expression.Lambda(Expression.Empty(), x, y),
                Expression.Lambda(Expression.Empty(), ps),
                Expression.Lambda(Expression.Constant(1), "foo", Array.Empty<ParameterExpression>()),
#if !USE_SLIM
                Expression.Lambda(Expression.Empty(), tailCall: true),
#endif
                Expression.Lambda(Expression.Empty(), tailCall: false),
                Expression.Lambda(typeof(Action), Expression.Empty()),
                Expression.Lambda(typeof(ThreadStart), Expression.Empty()),
            });
        }

        [TestMethod]
        public void Roundtrip_ListInit()
        {
            AssertRoundtrip(new Expression[]
            {
                ((Expression<Func<ArrayList>>)(() => new ArrayList { 1 })).Body,
                ((Expression<Func<ArrayList>>)(() => new ArrayList { 1, 2 })).Body,
                ((Expression<Func<ArrayList>>)(() => new ArrayList { 1, 2, 3 })).Body,

                ((Expression<Func<ArrayList>>)(() => new ArrayList(7) { 1 })).Body,
                ((Expression<Func<ArrayList>>)(() => new ArrayList(7) { 1, 2 })).Body,
                ((Expression<Func<ArrayList>>)(() => new ArrayList(7) { 1, 2, 3 })).Body,

                ((Expression<Func<List<int>>>)(() => new List<int> { 1 })).Body,
                ((Expression<Func<List<int>>>)(() => new List<int> { 1, 2 })).Body,
                ((Expression<Func<List<int>>>)(() => new List<int> { 1, 2, 3 })).Body,

                ((Expression<Func<List<int>>>)(() => new List<int>(7) { 1 })).Body,
                ((Expression<Func<List<int>>>)(() => new List<int>(7) { 1, 2 })).Body,
                ((Expression<Func<List<int>>>)(() => new List<int>(7) { 1, 2, 3 })).Body,

                ((Expression<Func<Hashtable>>)(() => new Hashtable { { "bar", 42 }, { "foo", 43 } })).Body,

                ((Expression<Func<Dictionary<string, int>>>)(() => new Dictionary<string, int> { { "bar", 42 }, { "foo", 43 } })).Body,
            });
        }

        [TestMethod]
        public void Roundtrip_Loop()
        {
            AssertRoundtrip(new Expression[]
            {
                Expression.Loop(Expression.Empty()),
                Expression.Loop(Expression.Empty(), Expression.Label("foo")),
                Expression.Loop(Expression.Empty(), Expression.Label("foo"), Expression.Label("bar")),
            });
        }

        [TestMethod]
        public void Roundtrip_Member()
        {
            AssertRoundtrip(new Expression[]
            {
                ((Expression<Func<DateTime>>)(() => DateTime.Now)).Body,
                ((Expression<Func<string, int>>)(s => s.Length)).Body,
                ((Expression<Func<DateTime>>)(() => DateTime.MinValue)).Body,
                ((Expression<Func<MyObj, int>>)(o => o.X)).Body,
                ((Expression<Func<MyVal, int>>)(o => o.X)).Body,
            });
        }

        [TestMethod]
        public void Roundtrip_MemberInit()
        {
            AssertRoundtrip(new Expression[]
            {
                ((Expression<Func<Bar>>)(() => new Bar('c') { X = { A = "foo", B = 3.14, Cs = { { "bar", 42 } } }, Y = "qux", Zs = { 1, 2, 3 } })).Body,
            });
        }

        [TestMethod]
        public void Roundtrip_NewArray()
        {
            AssertRoundtrip(new Expression[]
            {
                Expression.NewArrayBounds(typeof(int), Expression.Constant(1)),
                Expression.NewArrayBounds(typeof(int), Expression.Constant(1), Expression.Constant(2)),
                Expression.NewArrayInit(typeof(int)),
                Expression.NewArrayInit(typeof(int), Expression.Constant(1)),
                Expression.NewArrayInit(typeof(int), Expression.Constant(1), Expression.Constant(2)),
            });
        }

        [TestMethod]
        public void Roundtrip_New()
        {
            AssertRoundtrip(new Expression[]
            {
                Expression.New(typeof(MyVal)),
                Expression.New(typeof(TimeSpan)),
                Expression.New(typeof(TimeSpan).GetConstructor(new[] { typeof(int), typeof(int), typeof(int) }), Expression.Constant(3), Expression.Constant(14), Expression.Constant(15)),

                Expression.New(typeof(MyObj)),
                Expression.New(typeof(ArrayList)),
                Expression.New(typeof(ArrayList).GetConstructor(new[] { typeof(int) }), Expression.Constant(7)),

                Expression.New(typeof(SomeObj).GetConstructor(new[] { typeof(int), typeof(string) }), new Expression[] { Expression.Constant(42), Expression.Constant("foo") }, typeof(SomeObj).GetProperty("X"), typeof(SomeObj).GetProperty("Y")),

                // TODO: anonymous types
            });
        }

        private sealed class SomeObj
        {
            public SomeObj(int x, string y)
            {
                X = x;
                Y = y;
            }

            public int X { get; set; }
            public string Y { get; set; }
        }

        [TestMethod]
        public void Roundtrip_Parameter()
        {
            AssertRoundtrip(new Expression[]
            {
                Expression.Parameter(typeof(int)),
                Expression.Parameter(typeof(int), ""),
                Expression.Parameter(typeof(int), "foo"),
            });
        }

#if !USE_SLIM
        [TestMethod]
        public void Roundtrip_RuntimeVariables()
        {
            var x = Expression.Parameter(typeof(int));
            var y = Expression.Parameter(typeof(int));
            var z = Expression.Parameter(typeof(int));

            AssertRoundtrip(new Expression[]
            {
                Expression.RuntimeVariables(),
                Expression.RuntimeVariables(x),
                Expression.RuntimeVariables(x, y),
                Expression.RuntimeVariables(x, y, z),
            });
        }
#endif

        [TestMethod]
        public void Roundtrip_Switch()
        {
            AssertRoundtrip(new Expression[]
            {
                Expression.Switch(Expression.Constant(42), Expression.SwitchCase(Expression.Empty(), Expression.Constant(1))),
                Expression.Switch(Expression.Constant(42), Expression.Constant("bar"), Expression.SwitchCase(Expression.Constant("foo"), Expression.Constant(1))),
                Expression.Switch(Expression.Constant(42), Expression.Constant("bar"), Expression.SwitchCase(Expression.Constant("foo"), Expression.Constant(1), Expression.Constant(2))),
                Expression.Switch(Expression.Constant(42), Expression.Constant("bar"), Expression.SwitchCase(Expression.Constant("foo"), Expression.Constant(1), Expression.Constant(2)), Expression.SwitchCase(Expression.Constant("qux"), Expression.Constant(3))),
                Expression.Switch(Expression.Constant(42), Expression.Constant("bar"), typeof(Tests).GetMethod("CompareInt32"), Expression.SwitchCase(Expression.Constant("foo"), Expression.Constant(1))),
                Expression.Switch(typeof(void), Expression.Constant(42), Expression.Constant("bar"), comparison: null, Expression.SwitchCase(Expression.Constant("foo"), Expression.Constant(1))),
            });
        }

        public static bool CompareInt32(int x, int y)
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Roundtrip_Try()
        {
            AssertRoundtrip(new Expression[]
            {
                Expression.TryCatch(Expression.Constant(42), Expression.Catch(typeof(Exception), Expression.Constant(43))),
                Expression.TryCatch(Expression.Constant(42), Expression.Catch(Expression.Parameter(typeof(Exception)), Expression.Constant(43))),
                Expression.TryCatch(Expression.Constant(42), Expression.Catch(Expression.Parameter(typeof(Exception)), Expression.Constant(43), Expression.Constant(true))),
                Expression.TryFault(Expression.Constant(42), Expression.Constant(43)),
                Expression.TryFinally(Expression.Constant(42), Expression.Constant(43)),
                Expression.MakeTry(typeof(void), Expression.Constant(42), Expression.Constant(43), fault: null, new CatchBlock[] { Expression.Catch(typeof(Exception), Expression.Constant(45)) }),
            });
        }

        [TestMethod]
        public void Roundtrip_TypeBinary()
        {
            AssertRoundtrip(new Expression[]
            {
                Expression.TypeIs(Expression.Default(typeof(int)), typeof(int)),
                Expression.TypeEqual(Expression.Default(typeof(int)), typeof(int)),
            });
        }

        [TestMethod]
        public void Roundtrip_Unary1()
        {
            AssertRoundtrip(new Expression[]
            {
                Expression.ArrayLength(Expression.Default(typeof(int[]))),
                Expression.Quote(Expression.Lambda(Expression.Empty())),
            });
        }

        [TestMethod]
        public void Roundtrip_Unary2()
        {
            AssertRoundtrip(new Expression[]
            {
                Expression.Throw(Expression.Default(typeof(Exception)), typeof(int)),
                Expression.TypeAs(Expression.Default(typeof(object)), typeof(string)),
                Expression.Unbox(Expression.Default(typeof(object)), typeof(int)),
            });
        }

        [TestMethod]
        public void Roundtrip_Unary3()
        {
            AssertRoundtrip(new Expression[]
            {
                Expression.Increment(Expression.Default(typeof(int))),
                Expression.Decrement(Expression.Default(typeof(int))),
                Expression.PreIncrementAssign(Expression.Parameter(typeof(int))),
                Expression.PreDecrementAssign(Expression.Parameter(typeof(int))),
                Expression.PostIncrementAssign(Expression.Parameter(typeof(int))),
                Expression.PostDecrementAssign(Expression.Parameter(typeof(int))),
                Expression.Negate(Expression.Default(typeof(int))),
                Expression.NegateChecked(Expression.Default(typeof(int))),
                Expression.UnaryPlus(Expression.Default(typeof(int))),
                Expression.OnesComplement(Expression.Default(typeof(int))),
                Expression.Not(Expression.Default(typeof(bool))),
                Expression.IsTrue(Expression.Default(typeof(bool))),
                Expression.IsFalse(Expression.Default(typeof(bool))),

                // TODO: with methods
                Expression.Not(Expression.Default(typeof(MyObj))),
            });
        }

        [TestMethod]
        public void Roundtrip_Unary4()
        {
            AssertRoundtrip(new Expression[]
            {
                Expression.Convert(Expression.Default(typeof(int)), typeof(long)),
                Expression.ConvertChecked(Expression.Default(typeof(int)), typeof(long)),

                // TODO: with methods
                Expression.Convert(Expression.Default(typeof(MyObj)), typeof(int)),
                Expression.Convert(Expression.Default(typeof(MyObj)), typeof(long)),
            });
        }

        [TestMethod]
        public void Null_Expression()
        {
            var ser = new ExpressionSerializer(new BinaryObjectSerializer(), ExpressionFactory.Instance);

            var arr = ser.Serialize(expression: null);
            var otp = ser.Deserialize(arr);

            Assert.IsNull(otp);
        }

        [TestMethod]
        public void ExpressionSerializer_ArgumentChecking()
        {
            var ser = new ExpressionSerializer(new BinaryObjectSerializer());

            Assert.ThrowsException<ArgumentNullException>(() => _ = new ExpressionSerializer(serializer: null));
            Assert.ThrowsException<ArgumentNullException>(() => _ = new ExpressionSerializer(serializer: null, ExpressionFactory.Instance));
            Assert.ThrowsException<ArgumentNullException>(() => _ = new ExpressionSerializer(new BinaryObjectSerializer(), expressionFactory: null));

#if !USE_SLIM
            Assert.ThrowsException<ArgumentNullException>(() => ser.Serialize(default(Stream), Expression.Constant(1)));
#endif
            Assert.ThrowsException<ArgumentNullException>(() => ser.Deserialize(default(Stream)));
            Assert.ThrowsException<ArgumentNullException>(() => ser.Deserialize(default(byte[])));
        }

        private static void AssertRoundtrip(params Expression[] expressions)
        {
            var equ = new ExpressionEqualityComparer(() => new Comparator());

            foreach (var e in expressions)
            {
                var ser = new ExpressionSerializer(new BinaryObjectSerializer(), ExpressionFactory.Instance);

#if USE_SLIM
                var arr = ser.Serialize(e.ToExpressionSlim());
                var otp = ser.Deserialize(arr).ToExpression();
#else
                var arr = ser.Serialize(e);
                var otp = ser.Deserialize(arr);
#endif

                Assert.IsTrue(equ.Equals(e, otp), e.ToString());
            }
        }

        private sealed class Comparator : ExpressionEqualityComparator
        {
            protected override int GetHashCodeNewArray(NewArrayExpression obj)
            {
                var res = base.GetHashCodeNewArray(obj);
                return res;
            }

            protected override bool EqualsNewArray(NewArrayExpression x, NewArrayExpression y)
            {
                var res = base.EqualsNewArray(x, y);
                return res;
            }

            protected override bool EqualsConstant(ConstantExpression x, ConstantExpression y)
            {
                return Equals(x.Type, y.Type) && _valueComparer.Equals(x.Value, y.Value);
            }

            protected override int GetHashCodeConstant(ConstantExpression obj)
            {
                return obj.Type.GetHashCode();
            }

            protected override bool EqualsGlobalParameter(ParameterExpression x, ParameterExpression y)
            {
                return x.Type == y.Type && x.Name == y.Name;
            }

            protected override int GetHashCodeGlobalParameter(ParameterExpression obj)
            {
                return obj.Type.GetHashCode();
            }

            private readonly IEqualityComparer<object> _valueComparer = new ConstantEqualityComparer();

            private sealed class ConstantEqualityComparer : IEqualityComparer<object>
            {
                public new bool Equals(object o1, object o2)
                {
                    if (o1 == null)
                    {
                        return o2 == null;
                    }

                    if (o2 == null)
                    {
                        return false;
                    }

                    if (o1.GetType().IsArray)
                    {
                        _ = o1.GetType().GetElementType();

                        var xs = (IEnumerable)o1;
                        var ys = (IEnumerable)o2;

                        return xs.Cast<object>().SequenceEqual(ys.Cast<object>(), this);
                    }

                    return EqualityComparer<object>.Default.Equals(o1, o2);
                }

                public int GetHashCode(object obj)
                {
                    return 0;
                }
            }
        }
    }

#pragma warning disable 0649
#pragma warning disable 0660
#pragma warning disable 0661
    internal sealed class MyObj
    {
        public int X;

        public static MyObj operator +(MyObj _1, MyObj _2) => throw new NotImplementedException();
        public static bool operator ==(MyObj _1, MyObj _2) => throw new NotImplementedException();
        public static bool operator !=(MyObj _1, MyObj _2) => throw new NotImplementedException();
        public static MyObj operator !(MyObj _) => throw new NotImplementedException();

        public static explicit operator int(MyObj _) => throw new NotImplementedException();
        public static implicit operator long(MyObj _) => throw new NotImplementedException();
    }

    internal struct MyVal
    {
        public int X;

        public static MyVal operator +(MyVal _1, MyVal _2) => throw new NotImplementedException();
        public static bool operator ==(MyVal _1, MyVal _2) => throw new NotImplementedException();
        public static bool operator !=(MyVal _1, MyVal _2) => throw new NotImplementedException();
    }

    internal sealed class Bar
    {
        public Bar(char c)
        {
            _ = c;
        }

        public Foo X;
        public string Y { get; set; }
        public ArrayList Zs { get; set; }
    }

    internal sealed class Foo
    {
        public string A;
        public double B { get; set; }
        public Hashtable Cs { get; set; }
    }
#pragma warning restore
}
