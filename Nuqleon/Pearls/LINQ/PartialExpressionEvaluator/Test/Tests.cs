// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

using Microsoft.CSharp.RuntimeBinder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void PartialExpressionEvaluator_EvaluateAll()
        {
            var es = new Dictionary<Expression, Expression>
            {
                {
                    Expression.Constant(42),
                    Expression.Constant(42)
                },
                {
                    Expression.Default(typeof(int)),
                    Expression.Constant(0)
                },
                {
                    Expression.TypeIs(Expression.Constant("foo", typeof(object)), typeof(string)),
                    Expression.Constant(true)
                },
                {
                    Expression.Add(Expression.Constant(1), Expression.Constant(2)),
                    Expression.Constant(3)
                },
                {
                    Expression.Coalesce(Expression.Constant(value: null, typeof(string)), Expression.Constant("foo")),
                    Expression.Constant("foo")
                },
                {
                    Expression.MakeIndex(Expression.Constant(new List<int> { 2, 3, 4 }), typeof(List<int>).GetProperty("Item"), new[] { Expression.Constant(1) }),
                    Expression.Constant(3)
                },
                {
                    Expression.Invoke(Expression.Lambda(Expression.Constant(42))),
                    Expression.Constant(42)
                },
                {
                    Expression.Invoke(Expression.Lambda(Expression.Constant(42), Expression.Parameter(typeof(int))), Expression.Constant(0)),
                    Expression.Constant(42)
                },
                {
                    E(() => string.Empty.Length),
                    Expression.Constant(0)
                },
                {
                    E(() => "foo".ToUpper()),
                    Expression.Constant("FOO")
                },
#pragma warning disable IDE0057 // Substring can be simplified. (https://github.com/dotnet/roslyn/issues/49347)
                {
                    E(() => "bar".Substring(1)),
                    Expression.Constant("ar")
                },
                {
                    E(() => "foobar".Substring(2, 3)),
                    Expression.Constant("oba")
                },
#pragma warning restore IDE0057
                {
                    E(() => new TimeSpan(123L).Ticks),
                    Expression.Constant(123L)
                },
                {
                    E(() => new TimeSpan(1, 2, 3, 4, 5).Minutes),
                    Expression.Constant(3)
                },
                {
                    E(() => new TimeSpan().Ticks),
                    Expression.Constant(0L)
                },
                {
                    E(() => (-new TimeSpan(123L)).Ticks),
                    Expression.Constant(-123L)
                },
                {
                    E(() => (TimeSpan.FromSeconds(1) + TimeSpan.FromSeconds(2)).TotalSeconds),
                    Expression.Constant(3.0)
                },
                {
                    E(() => "bar".StartsWith("b") ? 42 : 43),
                    Expression.Constant(42)
                },
                {
                    E(() => new[] { 1, 2, 3 }.Sum()),
                    Expression.Constant(6)
                },
                {
                    E(() => new List<int> { 1, 2, 3 }.Sum()),
                    Expression.Constant(6)
                },
                {
                    E(() => new Bar { X = { Y = 42 } }.X.Y),
                    Expression.Constant(42)
                },
                {
                    E(() => new Bar { Zs = { 1, 2, 3 } }.Zs.Sum()),
                    Expression.Constant(6)
                },
            };

            Test(new Evaluator1(), es);
        }

        [TestMethod]
        public void PartialExpressionEvaluator_Partial()
        {
            var xs = new List<int> { 2, 3, 4 };

            var val = E(() => Value(1));

            var es = new Dictionary<Expression, Expression>
            {
                {
                    E(() => -TimeSpan.FromSeconds(1)),
                    Expression.Negate(Expression.Constant(TimeSpan.FromSeconds(1)))
                },
                {
                    E(() => TimeSpan.FromSeconds(1) + TimeSpan.FromSeconds(2)),
                    Expression.Add(Expression.Constant(TimeSpan.FromSeconds(1)), Expression.Constant(TimeSpan.FromSeconds(2)))
                },
                {
                    E(() => -"bar".Length),
                    E(() => -"bar".Length)
                },
                {
                    E(() => "bar".Length + Value(2) * Value(3)),
                    E(() => "bar".Length + 6)
                },
                {
                    E(() => Value(2) * Value(3) + "bar".Length),
                    E(() => 6 + "bar".Length)
                },
                {
                    E(() => "bar".StartsWith("b") ? Value(2) * Value(3) : 1 + "bar".Length),
                    //
                    // NB: Hand-rolled expression tree below due to breaking change in Roslyn where the conditional reduces to `6` prior to quotation.
                    //
                    // E(() => true ? 6 : 1 + "bar".Length)
                    Expression.Condition(Expression.Constant(true), Expression.Constant(6), Expression.Add(Expression.Constant(1), Expression.Property(Expression.Constant("bar"), typeof(string).GetProperty(nameof(string.Length)))))
                },
                {
                    E(() => "bar".ToUpper() is string),
                    E(() => "bar".ToUpper() is string)
                },
                {
                    E(() => new[] { Value(1) * Value(2), Value(3) + Value(4), "bar".Length }),
                    E(() => new[] { 2, 7, "bar".Length })
                },
                {
                    E(() => new[] { "bar".Length, Value(1) * Value(2), Value(3) + Value(4) }),
                    E(() => new[] { "bar".Length, 2, 7 })
                },
                {
                    E(() => "BAR".ToLower().ToUpper()),
                    E(() => "bar".ToUpper())
                },
                {
                    E(() => "bar".ToUpper().ToLower()),
                    E(() => "bar".ToUpper().ToLower())
                },
#pragma warning disable IDE0057 // Substring can be simplified. (https://github.com/dotnet/roslyn/issues/49347)
                {
                    E(() => "foobar".Substring("foo".Length)),
                    E(() => "foobar".Substring("foo".Length))
                },
                {
                    E(() => "foobar".Substring(Value(1) * Value(2), "foo".Length)),
                    E(() => "foobar".Substring(2, "foo".Length))
                },
                {
                    E(() => "foobar".Substring("foo".Length, Value(1) * Value(2))),
                    E(() => "foobar".Substring("foo".Length, 2))
                },
#pragma warning restore IDE0057
                {
                    E(() => new TimeSpan(Value(1) * Value(2), Value(2) + Value(3), "bar".Length)),
                    E(() => new TimeSpan(2, 5, "bar".Length))
                },
                {
                    E(() => new TimeSpan("bar".Length, Value(1) * Value(2), 2 + 3)),
                    E(() => new TimeSpan("bar".Length, 2, 5))
                },
                {
                    E(() => new List<int> { Value(1) * Value(2), Value(2) + Value(3), "bar".Length }),
                    E(() => new List<int> { 2, 5, "bar".Length })
                },
                {
                    E(() => new List<int> { "bar".Length, Value(1) * Value(2), Value(2) + Value(3) }),
                    E(() => new List<int> { "bar".Length, 2, 5 })
                },
                {
                    E(() => new Dictionary<string, int> { { "BAR".ToLower(), "bar".Length }, { "foo", Value(1) + Value(2) } }),
                    E(() => new Dictionary<string, int> { { "bar", "bar".Length }, { "foo", 3 } })
                },
                {
                    Expression.MakeIndex(Expression.Constant(xs), typeof(List<int>).GetProperty("Item"), new[] { Expression.Property(Expression.Constant("bar"), "Length") }),
                    Expression.MakeIndex(Expression.Constant(xs), typeof(List<int>).GetProperty("Item"), new[] { Expression.Property(Expression.Constant("bar"), "Length") })
                },
                {
                    Expression.MakeIndex(Expression.Constant(xs), typeof(List<int>).GetProperty("Item"), new[] { Expression.Add(Expression.Constant(1), Expression.Constant(2)) }),
                    Expression.MakeIndex(Expression.Constant(xs), typeof(List<int>).GetProperty("Item"), new[] { Expression.Constant(3) })
                },
                {
                    E(() => new Bar { X = { Y = "bar".Length } }),
                    E(() => new Bar { X = { Y = "bar".Length } })
                },
                {
                    E(() => new Bar { X = { Y = Value(1) * Value(2) }, Zs = { Value(2) * Value(3), "foo".Length } }),
                    E(() => new Bar { X = { Y = 2 }, Zs = { 6, "foo".Length } })
                },
                {
                    E(() => new Bar { X = { Y = Value(1) * Value(2) }, Zs = { "foo".Length, Value(2) * Value(3) } }),
                    E(() => new Bar { X = { Y = 2 }, Zs = { "foo".Length, 6 } })
                },
                {
                    E(() => new Baz {}),
                    E(() => new Baz {})
                },
                {
                    E(() => new Baz { X = Value(1) * Value(2) }),
                    E(() => new Baz { X = 2 })
                },
                {
                    E(() => new Baz { X = Value(1) * Value(2), Y = "bar".Length }),
                    E(() => new Baz { X = 2, Y = "bar".Length })
                },
                {
                    E(() => new Baz { X = "bar".Length, Y = Value(1) * Value(2) }),
                    E(() => new Baz { X = "bar".Length, Y = 2 })
                },
                {
                    Expression.Invoke(Expression.Lambda(Expression.Property(Expression.Constant("bar"), "Length"))),
                    Expression.Invoke(Expression.Lambda(Expression.Property(Expression.Constant("bar"), "Length")))
                },
                {
                    Expression.Invoke(Expression.Lambda(Expression.Property(Expression.Constant("bar"), "Length"), Expression.Parameter(typeof(int))), val),
                    Expression.Invoke(Expression.Lambda(Expression.Property(Expression.Constant("bar"), "Length"), Expression.Parameter(typeof(int))), Expression.Constant(1))
                },
                {
                    E(() => TimeSpan.MaxValue),
                    Expression.Constant(TimeSpan.MaxValue)
                },
                {
                    E(() => TimeSpan.FromSeconds(1).Seconds),
                    E(() => 1)
                },
                {
                    E(() => TimeSpan.Zero.Seconds),
                    E(() => TimeSpan.Zero.Seconds)
                }
            };

            Test(new Evaluator2(), es);
        }

        [TestMethod]
        public void PartialExpressionEvaluator_Scoping()
        {
            var eval = new Evaluator2();

            var es = new Dictionary<Expression, Expression>
            {
                {
                    (Expression<Func<string, int>>)(s => s.Length),
                    (Expression<Func<string, int>>)(s => s.Length)
                },
                {
                    E(() => Enumerable.Range(1, 3).Select(x => x).Sum()),
                    Expression.Constant(6)
                },
                {
                    E(() => Enumerable.Range(1, 3).Select(x => x.ToString()).Select(s => int.Parse(s)).Sum()),
                    Expression.Constant(6)
                }
            };

            Test(eval, es);
        }

        [TestMethod]
        public void PartialExpressionEvaluator_NotSupported()
        {
            var eval = new Evaluator1();

            var es = new Expression[]
            {
                Expression.Block(Expression.Constant(1)),
                Expression.DebugInfo(Expression.SymbolDocument("foo.cs"), 1, 2, 3, 4),
                Expression.Dynamic(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(int), typeof(Tests)), typeof(int), Expression.Constant("foo")),
                Expression.Goto(Expression.Label()),
                Expression.Label(Expression.Label()),
                Expression.Loop(Expression.Empty()),
                Expression.RuntimeVariables(Expression.Parameter(typeof(int))),
                Expression.Switch(Expression.Constant(1), Expression.SwitchCase(Expression.Empty(), Expression.Constant(1))),
                Expression.TryFinally(Expression.Empty(), Expression.Empty()),
                new MyNode(),
            };

            foreach (var e in es)
            {
                Assert.ThrowsException<NotSupportedException>(() => eval.Reduce(e));
            }
        }

        private sealed class MyNode : Expression
        {
        }

        private static int Value(int x)
        {
            return x;
        }

        // TODO: checked arithmetic

        private static void Test(PartialExpressionEvaluatorBase eval, Dictionary<Expression, Expression> tests)
        {
            var eq = new ExpressionEqualityComparer();

            foreach (var test in tests)
            {
                var r = eval.Reduce(test.Key);
                Assert.IsTrue(eq.Equals(test.Value, r), test.Value.ToString() + " != " + r.ToString());
            }
        }

        private sealed class Bar
        {
            public Bar()
            {
                X = new Foo();
                Zs = new List<int>();
            }

            public Foo X { get; set; }

            public List<int> Zs { get; set; }
        }

        private sealed class Foo
        {
            public int Y { get; set; }
        }

        private sealed class Baz
        {
            public int X { get; set; }
            public int Y;
        }

        private static Expression E<T>(Expression<Func<T>> f)
        {
            return f.Body;
        }

        private class Evaluator1 : PartialExpressionEvaluatorBase
        {
            protected override bool CanEvaluate(ConstantExpression node)
            {
                return true;
            }

            protected override bool CanEvaluate(Type type)
            {
                return true;
            }

            protected override bool CanEvaluate(MethodInfo method)
            {
                return true;
            }

            protected override bool CanEvaluate(ConstructorInfo constructor)
            {
                return true;
            }

            protected override bool CanEvaluate(PropertyInfo property)
            {
                return true;
            }

            protected override bool CanEvaluate(FieldInfo field)
            {
                return true;
            }

            protected override Expression EvaluateCore(Expression expression)
            {
                return Expression.Constant(expression.Evaluate(), expression.Type);
            }
        }

        private sealed class Evaluator2 : Evaluator1
        {
            protected override bool CanEvaluate(MethodInfo method)
            {
                if (method.DeclaringType == typeof(TimeSpan) && method.Name == "op_UnaryNegation")
                {
                    return false;
                }
                else if (method.DeclaringType == typeof(TimeSpan) && method.Name == "op_Addition")
                {
                    return false;
                }
                else if (method.DeclaringType == typeof(string) && method.Name == "ToUpper")
                {
                    return false;
                }

                return base.CanEvaluate(method);
            }

            protected override bool CanEvaluate(PropertyInfo property)
            {
                return property.Name is not "Length" and not "Item";
            }

            protected override bool CanEvaluate(FieldInfo field)
            {
                return field.Name != "Zero";
            }

            protected override bool CanEvaluate(Type type)
            {
                return type != typeof(Baz);
            }
        }
    }
}
