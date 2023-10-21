// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - July 2013 - Created this file.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Tests.System.Linq.Expressions.Bonsai;

namespace Tests.System.Linq.Expressions
{
    [TestClass]
    public class Tests : TestBase
    {
        [TestMethod]
        public void ExpressionSlim_Update_Tests()
        {
            var boolSlim = new TypeToTypeSlimConverter().Visit(typeof(bool));
            var intExpr1 = ExpressionSlim.Parameter(SlimType);
            var intExpr2 = ExpressionSlim.Parameter(SlimType);
            var intExpr3 = ExpressionSlim.Parameter(SlimType);
            var boolExpr1 = ExpressionSlim.Parameter(boolSlim);
            var boolExpr2 = ExpressionSlim.Parameter(boolSlim);
            var prop = SlimType.GetProperty("Foo", propertyType: null, EmptyReadOnlyCollection<TypeSlim>.Instance, canWrite: true);
            var elementInits = new ElementInitSlim[] { new(addMethod: null, arguments: null) }.ToReadOnly();
            var method = SlimType.GetSimpleMethod("Foo", EmptyReadOnlyCollection<TypeSlim>.Instance, returnType: null);

            var b = ExpressionSlim.Add(intExpr1, intExpr2);
            Assert.AreNotSame(b, b.Update(intExpr1, conversion: null, intExpr3));
            Assert.AreNotSame(b, b.Update(intExpr3, conversion: null, intExpr2));

            var c = ExpressionSlim.Condition(boolExpr1, intExpr1, intExpr2);
            Assert.AreNotSame(c, c.Update(boolExpr2, intExpr1, intExpr2));
            Assert.AreNotSame(c, c.Update(boolExpr1, intExpr2, intExpr3));
            Assert.AreNotSame(c, c.Update(boolExpr1, intExpr1, intExpr3));

            var i = ExpressionSlim.MakeIndex(intExpr1, prop, new ExpressionSlim[] { intExpr2 }.ToReadOnly());
            Assert.AreNotSame(i, i.Update(intExpr2, new ExpressionSlim[] { intExpr2 }.ToReadOnly()));
            Assert.AreNotSame(i, i.Update(intExpr1, new ExpressionSlim[] { intExpr3 }.ToReadOnly()));

            var inv = ExpressionSlim.Invoke(intExpr1, new[] { intExpr2 });
            Assert.AreNotSame(inv, inv.Update(intExpr2, new ExpressionSlim[] { intExpr2 }.ToReadOnly()));
            Assert.AreNotSame(inv, inv.Update(intExpr1, new ExpressionSlim[] { intExpr3 }.ToReadOnly()));

            var l = ExpressionSlim.Lambda(intExpr1);
            Assert.AreNotSame(l, l.Update(intExpr2, EmptyReadOnlyCollection<ParameterExpressionSlim>.Instance));
            Assert.AreNotSame(l, l.Update(intExpr1, new ParameterExpressionSlim[] { ExpressionSlim.Parameter(SlimType) }.ToReadOnly()));

            var li = ExpressionSlim.ListInit(ExpressionSlim.New(SlimType));
            Assert.AreNotSame(li, li.Update(ExpressionSlim.New(boolSlim), elementInits));
            Assert.AreNotSame(li, li.Update(ExpressionSlim.New(SlimType), elementInits));

            var ma = ExpressionSlim.Bind(prop, intExpr1);
            Assert.AreNotSame(ma, ma.Update(intExpr2));

            var macc = ExpressionSlim.MakeMemberAccess(instance: null, prop);
            Assert.AreNotSame(macc, macc.Update(intExpr1));

            var ml = ExpressionSlim.ListBind(prop);
            Assert.AreNotSame(ml, ml.Update(elementInits));

            var mm = ExpressionSlim.MemberBind(prop);
            Assert.AreNotSame(mm, mm.Update(new MemberBindingSlim[] { ExpressionSlim.Bind(prop, intExpr1) }.ToReadOnly()));

            var m = ExpressionSlim.Call(method);
            Assert.AreNotSame(m, m.Update(intExpr1, new ExpressionSlim[] { intExpr2 }.ToReadOnly()));
            Assert.AreNotSame(m, m.Update(@object: null, new ExpressionSlim[] { intExpr2 }.ToReadOnly()));

            var na = ExpressionSlim.NewArrayInit(SlimType);
            Assert.AreNotSame(na, na.Update(new ExpressionSlim[] { intExpr1 }.ToReadOnly()));

            var nb = ExpressionSlim.NewArrayBounds(SlimType, intExpr1);
            Assert.AreNotSame(nb, nb.Update(new ExpressionSlim[] { intExpr2 }.ToReadOnly()));

            var ctor = SlimType.GetConstructor(new TypeSlim[] { SlimType }.ToReadOnly());
            var n = ExpressionSlim.New(ctor, ExpressionSlim.Parameter(SlimType));
            Assert.AreNotSame(n, n.Update(new ExpressionSlim[] { ExpressionSlim.Default(SlimType) }.ToReadOnly()));

            var t = ExpressionSlim.TypeIs(intExpr1, SlimType);
            Assert.AreNotSame(t, t.Update(intExpr2));

            var u = ExpressionSlim.Not(boolExpr1);
            Assert.AreNotSame(u, u.Update(boolExpr2));
        }

        [TestMethod]
        public void ExpressionSlim_ToString_Constant()
        {
            AssertToString(ExpressionSlim.Constant(ObjectSlim.Create(value: null, typeof(string).ToTypeSlim(), typeof(string)), typeof(string).ToTypeSlim()), "Constant(null, System.String)");
            AssertToString(Expression.Constant(value: null, typeof(string)).ToExpressionSlim(), "Constant(null, System.String)");
            AssertToString(Expression.Constant("bar", typeof(string)).ToExpressionSlim(), "Constant(bar, System.String)");
        }

        [TestMethod]
        public void ExpressionSlim_ToString_Condition()
        {
            AssertToString(Expression.Condition(Expression.Constant(true), Expression.Constant(42), Expression.Constant(43)).ToExpressionSlim(), "Conditional(Constant(True, System.Boolean), Constant(42, System.Int32), Constant(43, System.Int32))");
        }

        [TestMethod]
        public void ExpressionSlim_ToString_Call()
        {
            AssertToString(Expression.Call(Expression.Constant("bar"), typeof(string).GetMethod("ToUpper", Type.EmptyTypes)).ToExpressionSlim(), "Call(System.String System.String.ToUpper(), Constant(bar, System.String))");
            AssertToString(Expression.Call(Expression.Constant("bar"), typeof(string).GetMethod("Substring", new[] { typeof(int) }), Expression.Constant(1)).ToExpressionSlim(), "Call(System.String System.String.Substring(System.Int32), Constant(bar, System.String), Constant(1, System.Int32))");

            AssertToString(Expression.Call(instance: null, typeof(Console).GetMethod("WriteLine", Type.EmptyTypes)).ToExpressionSlim(), "Call(System.Void System.Console.WriteLine())");
            AssertToString(Expression.Call(instance: null, typeof(Console).GetMethod("WriteLine", new[] { typeof(int) }), Expression.Constant(1)).ToExpressionSlim(), "Call(System.Void System.Console.WriteLine(System.Int32), Constant(1, System.Int32))");
        }

        [TestMethod]
        public void ExpressionSlim_ToString_Default()
        {
            AssertToString(Expression.Default(typeof(string)).ToExpressionSlim(), "Default(System.String)");
        }

        [TestMethod]
        public void ExpressionSlim_ToString_Binary_Bitwise()
        {
            foreach (var type in new[]
            {
                ExpressionType.And,
                ExpressionType.Or,
                ExpressionType.ExclusiveOr,
            })
            {
                var e = Expression.MakeBinary(type, Expression.Constant(1), Expression.Constant(2));
                AssertToString(e.ToExpressionSlim(), $"{type}(Constant(1, System.Int32), Constant(2, System.Int32))");
            }
        }

        [TestMethod]
        public void ExpressionSlim_ToString_Binary_Logical()
        {
            foreach (var type in new[]
            {
                ExpressionType.AndAlso,
                ExpressionType.OrElse,
            })
            {
                var e = Expression.MakeBinary(type, Expression.Constant(true), Expression.Constant(false));
                AssertToString(e.ToExpressionSlim(), $"{type}(Constant(True, System.Boolean), Constant(False, System.Boolean))");
            }
        }

        [TestMethod]
        public void ExpressionSlim_ToString_Binary_Arithmetic()
        {
            foreach (var type in new[]
            {
                ExpressionType.Add,
                ExpressionType.AddChecked,
                ExpressionType.Divide,
                ExpressionType.Modulo,
                ExpressionType.Multiply,
                ExpressionType.MultiplyChecked,
                ExpressionType.Subtract,
                ExpressionType.SubtractChecked,

                ExpressionType.Equal,
                ExpressionType.GreaterThan,
                ExpressionType.GreaterThanOrEqual,
                ExpressionType.LessThan,
                ExpressionType.LessThanOrEqual,
                ExpressionType.NotEqual,
            })
            {
                var e = Expression.MakeBinary(type, Expression.Constant(1), Expression.Constant(2));
                AssertToString(e.ToExpressionSlim(), $"{type}(Constant(1, System.Int32), Constant(2, System.Int32))");
            }
        }

        [TestMethod]
        public void ExpressionSlim_ToString_Binary_Shift()
        {
            foreach (var type in new[]
            {
                ExpressionType.LeftShift,
                ExpressionType.RightShift,
            })
            {
                var e = Expression.MakeBinary(type, Expression.Constant(1), Expression.Constant(2));
                AssertToString(e.ToExpressionSlim(), $"{type}(Constant(1, System.Int32), Constant(2, System.Int32))");
            }
        }

        [TestMethod]
        public void ExpressionSlim_ToString_Binary_Comparison()
        {
            foreach (var type in new[]
            {
                ExpressionType.Equal,
                ExpressionType.GreaterThan,
                ExpressionType.GreaterThanOrEqual,
                ExpressionType.LessThan,
                ExpressionType.LessThanOrEqual,
                ExpressionType.NotEqual,
            })
            {
                var e = Expression.MakeBinary(type, Expression.Constant(1), Expression.Constant(2));
                AssertToString(e.ToExpressionSlim(), $"{type}(Constant(1, System.Int32), Constant(2, System.Int32))");
            }
        }

        [TestMethod]
        public void ExpressionSlim_ToString_Binary_ArrayIndex()
        {
            var e = Expression.ArrayIndex(Expression.Default(typeof(int[])), Expression.Constant(1));
            AssertToString(e.ToExpressionSlim(), "ArrayIndex(Default(System.Int32[]), Constant(1, System.Int32))");
        }

        [TestMethod]
        public void ExpressionSlim_ToString_Unary_Arithmetic()
        {
            foreach (var type in new[]
            {
                ExpressionType.Decrement,
                ExpressionType.Increment,
                ExpressionType.Negate,
                ExpressionType.NegateChecked,
                ExpressionType.OnesComplement,
            })
            {
                var e = Expression.MakeUnary(type, Expression.Constant(1), type: null);
                AssertToString(e.ToExpressionSlim(), $"{type}(Constant(1, System.Int32))");
            }
        }

        [TestMethod]
        public void ExpressionSlim_ToString_Unary_Logical()
        {
            foreach (var type in new[]
            {
                ExpressionType.IsFalse,
                ExpressionType.IsTrue,
                ExpressionType.Not,
            })
            {
                var e = Expression.MakeUnary(type, Expression.Constant(true), type: null);
                AssertToString(e.ToExpressionSlim(), $"{type}(Constant(True, System.Boolean))");
            }
        }

        [TestMethod]
        public void ExpressionSlim_ToString_Unary_Convert()
        {
            AssertToString(Expression.Convert(Expression.Constant(1), typeof(long)).ToExpressionSlim(), "Convert(Constant(1, System.Int32), System.Int64)");
            AssertToString(Expression.ConvertChecked(Expression.Constant(1), typeof(long)).ToExpressionSlim(), "ConvertChecked(Constant(1, System.Int32), System.Int64)");
        }

        [TestMethod]
        public void ExpressionSlim_ToString_Unary_TypeAs()
        {
            AssertToString(Expression.TypeAs(Expression.Default(typeof(object)), typeof(string)).ToExpressionSlim(), "TypeAs(Default(System.Object), System.String)");
        }

        [TestMethod]
        public void ExpressionSlim_ToString_Unary_Unbox()
        {
            AssertToString(Expression.Unbox(Expression.Default(typeof(object)), typeof(int)).ToExpressionSlim(), "Unbox(Default(System.Object), System.Int32)");
        }

        [TestMethod]
        public void ExpressionSlim_ToString_Unary_Throw()
        {
            AssertToString(Expression.Throw(Expression.Default(typeof(Exception)), typeof(int)).ToExpressionSlim(), "Throw(Default(System.Exception), System.Int32)");
            AssertToString(Expression.Rethrow(typeof(int)).ToExpressionSlim(), "Throw(System.Int32)");
        }

        [TestMethod]
        public void ExpressionSlim_ToString_Parameter()
        {
            AssertToString(Expression.Parameter(typeof(string)).ToExpressionSlim(), "Parameter(System.String, Param)");
            AssertToString(Expression.Parameter(typeof(string), "foo").ToExpressionSlim(), "Parameter(System.String, foo)");
        }

        [TestMethod]
        public void ExpressionSlim_ToString_Index()
        {
            AssertToString(Expression.MakeIndex(Expression.New(typeof(List<int>)), typeof(List<int>).GetProperty("Item"), new[] { Expression.Constant(0) }).ToExpressionSlim(), "Index(System.Collections.Generic.List`1<System.Int32>.Item[System.Int32], New(System.Collections.Generic.List`1<System.Int32>..ctor()), Constant(0, System.Int32))");
        }

        [TestMethod]
        public void ExpressionSlim_ToString_ListInit()
        {
            AssertToString(Expression.ListInit(Expression.New(typeof(List<int>)), Expression.ElementInit(typeof(List<int>).GetMethod("Add"), Expression.Constant(42))).ToExpressionSlim(), "ListInit(New(System.Collections.Generic.List`1<System.Int32>..ctor()), ElementInit(System.Void System.Collections.Generic.List`1<System.Int32>.Add(System.Int32), Constant(42, System.Int32)))");
        }

        [TestMethod]
        public void ExpressionSlim_ToString_MemberInit()
        {
            AssertToString(Expression.MemberInit(Expression.New(typeof(Types2.Bar)), Expression.Bind(typeof(Types2.Bar).GetField("Foo"), Expression.Constant(42))).ToExpressionSlim(), "MemberInit(New(Types2.Bar..ctor()), Assignment(Types2.Bar.Foo, Constant(42, System.Int32)))");
            AssertToString(Expression.MemberInit(Expression.New(typeof(Types2.Bar)), Expression.MemberBind(typeof(Types2.Bar).GetField("Baz"), Expression.Bind(typeof(Types2.Baz).GetField("Qux"), Expression.Constant(42)))).ToExpressionSlim(), "MemberInit(New(Types2.Bar..ctor()), MemberBinding(Types2.Bar.Baz, Assignment(Types2.Baz.Qux, Constant(42, System.Int32))))");
            AssertToString(Expression.MemberInit(Expression.New(typeof(Types2.Bar)), Expression.ListBind(typeof(Types2.Bar).GetField("Quxs"), Expression.ElementInit(typeof(List<int>).GetMethod("Add"), Expression.Constant(42)))).ToExpressionSlim(), "MemberInit(New(Types2.Bar..ctor()), ListBinding(Types2.Bar.Quxs, ElementInit(System.Void System.Collections.Generic.List`1<System.Int32>.Add(System.Int32), Constant(42, System.Int32))))");
        }

        [TestMethod]
        public void ExpressionSlim_ToString_New()
        {
            AssertToString(Expression.New(typeof(TimeSpan)).ToExpressionSlim(), "New(System.TimeSpan)");
            AssertToString(Expression.New(typeof(TimeSpan).GetConstructor(new[] { typeof(long) }), Expression.Constant(42L)).ToExpressionSlim(), "New(System.TimeSpan..ctor(System.Int64), Constant(42, System.Int64))");
            AssertToString(Expression.New(typeof(ArrayList).GetConstructor(Type.EmptyTypes)).ToExpressionSlim(), "New(System.Collections.ArrayList..ctor())");
        }

        [TestMethod]
        public void ExpressionSlim_ToString_NewArray()
        {
            AssertToString(Expression.NewArrayInit(typeof(int), Expression.Constant(42)).ToExpressionSlim(), "NewArrayInit(System.Int32, Constant(42, System.Int32))");
            AssertToString(Expression.NewArrayBounds(typeof(int), Expression.Constant(42)).ToExpressionSlim(), "NewArrayBounds(System.Int32, Constant(42, System.Int32))");
        }

        [TestMethod]
        public void ExpressionSlim_ToString_TypeBinary()
        {
            AssertToString(Expression.TypeIs(Expression.Constant("bar"), typeof(string)).ToExpressionSlim(), "TypeIs(Constant(bar, System.String), System.String)");
            AssertToString(Expression.TypeEqual(Expression.Constant("bar"), typeof(string)).ToExpressionSlim(), "TypeEqual(Constant(bar, System.String), System.String)");
        }

        [TestMethod]
        public void ExpressionSlim_ToString_Member()
        {
            AssertToString(Expression.Property(expression: null, typeof(DateTime).GetProperty("Now")).ToExpressionSlim(), "MemberAccess(System.DateTime.Now)");
            AssertToString(Expression.Property(Expression.Constant("foo"), typeof(string).GetProperty("Length")).ToExpressionSlim(), "MemberAccess(System.String.Length, Constant(foo, System.String))");
        }

        [TestMethod]
        public void ExpressionSlim_ToString_Lambda()
        {
            AssertToString(Expression.Lambda(Expression.Constant(42)).ToExpressionSlim(), "Lambda(Constant(42, System.Int32))");
            AssertToString(Expression.Lambda(Expression.Constant(42), Expression.Parameter(typeof(string), "foo")).ToExpressionSlim(), "Lambda(Constant(42, System.Int32), Parameter(System.String, foo))");
        }

        [TestMethod]
        public void ExpressionSlim_ToString_Invoke()
        {
            AssertToString(Expression.Invoke(Expression.Parameter(typeof(Func<int>), "f")).ToExpressionSlim(), "Invoke(Parameter(System.Func`1<System.Int32>, f))");
            AssertToString(Expression.Invoke(Expression.Parameter(typeof(Func<int, int>), "f"), Expression.Constant(42)).ToExpressionSlim(), "Invoke(Parameter(System.Func`2<System.Int32, System.Int32>, f), Constant(42, System.Int32))");
        }

        [TestMethod]
        public void ExpressionSlim_ToString_Block()
        {
            AssertToString(Expression.Block(Expression.Constant(42), Expression.Constant(1337)).ToExpressionSlim(), "Block(Constant(42, System.Int32); Constant(1337, System.Int32))");
            AssertToString(Expression.Block(new[] { Expression.Parameter(typeof(string), "x"), Expression.Parameter(typeof(string), "y") }, Expression.Constant(42), Expression.Constant(1337)).ToExpressionSlim(), "Block(Parameter(System.String, x), Parameter(System.String, y); Constant(42, System.Int32); Constant(1337, System.Int32))");
        }

        [TestMethod]
        public void ExpressionSlim_ToString_Label()
        {
            AssertToString(Expression.Label(Expression.Label()).ToExpressionSlim(), "Label(LabelTarget(Label, System.Void))");
            AssertToString(Expression.Label(Expression.Label("l")).ToExpressionSlim(), "Label(LabelTarget(l, System.Void))");
            AssertToString(Expression.Label(Expression.Label(typeof(int)), Expression.Constant(42)).ToExpressionSlim(), "Label(LabelTarget(Label, System.Int32), Constant(42, System.Int32))");
            AssertToString(Expression.Label(Expression.Label(typeof(int), "l"), Expression.Constant(42)).ToExpressionSlim(), "Label(LabelTarget(l, System.Int32), Constant(42, System.Int32))");
        }

        [TestMethod]
        public void ExpressionSlim_ToString_Goto()
        {
            AssertToString(Expression.Goto(Expression.Label("l")).ToExpressionSlim(), "Goto(LabelTarget(l, System.Void))");
            AssertToString(Expression.Goto(Expression.Label(typeof(int), "l"), Expression.Constant(42)).ToExpressionSlim(), "Goto(LabelTarget(l, System.Int32), Constant(42, System.Int32))");
            AssertToString(Expression.Break(Expression.Label("l")).ToExpressionSlim(), "Break(LabelTarget(l, System.Void))");
            AssertToString(Expression.Return(Expression.Label("l")).ToExpressionSlim(), "Return(LabelTarget(l, System.Void))");
            AssertToString(Expression.Continue(Expression.Label("l")).ToExpressionSlim(), "Continue(LabelTarget(l, System.Void))");
        }

        [TestMethod]
        public void ExpressionSlim_ToString_Loop()
        {
            AssertToString(Expression.Loop(Expression.Constant(42)).ToExpressionSlim(), "Loop(Constant(42, System.Int32))");
            AssertToString(Expression.Loop(Expression.Constant(42), Expression.Label("Break")).ToExpressionSlim(), "Loop(Constant(42, System.Int32), LabelTarget(Break, System.Void))");
            AssertToString(Expression.Loop(Expression.Constant(42), Expression.Label("Break"), Expression.Label("Continue")).ToExpressionSlim(), "Loop(LabelTarget(Continue, System.Void), Constant(42, System.Int32), LabelTarget(Break, System.Void))");
        }

        [TestMethod]
        public void ExpressionSlim_ToString_Switch()
        {
            var c = (Expression<Func<string, string, bool>>)((x, y) => string.Equals(x, y));

            AssertToString(Expression.Switch(Expression.Constant(42), Expression.Constant(42), Expression.SwitchCase(Expression.Constant(1337), Expression.Constant(42))).ToExpressionSlim(), "Switch(Constant(42, System.Int32), SwitchCase(Constant(1337, System.Int32), Constant(42, System.Int32)), Constant(42, System.Int32))");
            AssertToString(Expression.Switch(Expression.Constant("42"), Expression.Constant("42"), ((MethodCallExpression)(c.Body)).Method, Expression.SwitchCase(Expression.Constant("1337"), Expression.Constant("42"))).ToExpressionSlim(), "Switch(Constant(42, System.String), System.Boolean System.String.Equals(System.String, System.String), SwitchCase(Constant(1337, System.String), Constant(42, System.String)), Constant(42, System.String))");
        }

        [TestMethod]
        public void ExpressionSlim_ToString_Try()
        {
            AssertToString(Expression.TryCatch(Expression.Constant(42), Expression.Catch(typeof(Exception), Expression.Constant(42))).ToExpressionSlim(), "Try(Constant(42, System.Int32), CatchBlock(System.Exception, Constant(42, System.Int32)))");
            AssertToString(Expression.TryFault(Expression.Constant(42), Expression.Constant(42)).ToExpressionSlim(), "Try(Constant(42, System.Int32), Constant(42, System.Int32))");
        }

        private static void AssertToString(ExpressionSlim e, string s)
        {
            Assert.AreEqual(s, e.ToString());
        }
    }
}

#pragma warning disable 0649
namespace Types2
{
    internal class Bar
    {
        public int Foo;
        public List<int> Quxs;
        public Baz Baz;
    }

    internal class Baz
    {
        public int Qux;
    }
}
#pragma warning restore
