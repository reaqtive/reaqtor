// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class ExpressionMemberAllowListScannerTests
    {
        [TestMethod]
        public void ExpressionMemberAllowListScanner_Errors()
        {
            var ws = new ExpressionMemberAllowListScanner();

            Assert.ThrowsException<NotSupportedException>(() => ((IEnumerable)ws.DeclaringTypes).GetEnumerator());
            Assert.ThrowsException<NotSupportedException>(() => ((IEnumerable<Type>)ws.DeclaringTypes).GetEnumerator());
            Assert.ThrowsException<NotSupportedException>(() => ((IEnumerable)ws.Members).GetEnumerator());
            Assert.ThrowsException<NotSupportedException>(() => ((IEnumerable<MemberInfo>)ws.Members).GetEnumerator());

            AssertEx.ThrowsException<ArgumentNullException>(() => ws.DeclaringTypes.Add(type: null), ex => Assert.AreEqual("type", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => ws.DeclaringTypes.Add(type: null, includeBase: false), ex => Assert.AreEqual("type", ex.ParamName));

            AssertEx.ThrowsException<ArgumentNullException>(() => ws.Members.Add(member: null), ex => Assert.AreEqual("member", ex.ParamName));

            ws.DeclaringTypes.Add(typeof(string));
            Assert.ThrowsException<InvalidOperationException>(() => ws.Members.Add(typeof(string).GetProperty("Length")));

            ws.Members.Add(typeof(Environment).GetProperty("TickCount"));
            Assert.ThrowsException<InvalidOperationException>(() => ws.DeclaringTypes.Add(typeof(Environment)));
        }

        [TestMethod]
        public void ExpressionMemberAllowListScanner_DeclaringTypes_Simple()
        {
            var ws = new ExpressionMemberAllowListScanner
            {
                DeclaringTypes =
                {
                    typeof(string)
                }
            };

            foreach (var e in new Expression[] {
                (Expression<Func<string, string>>)(s => s.Substring(0, 1).ToUpper()),
                (Expression<Func<string, int>>)(s => s.Length),
                (Expression<Func<string>>)(() => new string(' ', 1)),
            })
            {
                Assert.AreSame(e, ws.Visit(e));
            }

            foreach (var e in new Expression[] {
                (Expression<Func<TimeSpan>>)(() => new TimeSpan(1, 2, 3)),
            })
            {
                Assert.ThrowsException<NotSupportedException>(() => ws.Visit(e));
            }
        }

        [TestMethod]
        public void ExpressionMemberAllowListScanner_DeclaringTypes_Generic()
        {
            var ws = new ExpressionMemberAllowListScanner
            {
                DeclaringTypes =
                {
                    typeof(List<int>),
                    typeof(HashSet<>),
                }
            };

            foreach (var e in new Expression[] {
                (Expression<Func<List<int>, int>>)(xs => xs.Count),
                (Expression<Func<List<int>, int, bool>>)((xs, x) => xs.Contains(x)),
                (Expression<Func<HashSet<int>, int, bool>>)((xs, x) => xs.Contains(x)),
                (Expression<Func<List<int>>>)(() => new List<int> { 2, 3, 5 }),
            })
            {
                Assert.AreSame(e, ws.Visit(e));
            }

            foreach (var e in new Expression[] {
                (Expression<Func<List<string>, int>>)(xs => xs.Count),
            })
            {
                Assert.ThrowsException<NotSupportedException>(() => ws.Visit(e));
            }
        }

        [TestMethod]
        public void ExpressionMemberAllowListScanner_DeclaringTypes_Interface()
        {
            var ws = new ExpressionMemberAllowListScanner
            {
                DeclaringTypes =
                {
                    typeof(ICollection<>)
                }
            };

            foreach (var e in new Expression[] {
                (Expression<Action<List<int>, int>>)((xs, x) => xs.Add(x)),
                (Expression<Func<List<int>, int>>)(xs => xs.Count),
            })
            {
                Assert.AreSame(e, ws.Visit(e));
            }

            foreach (var e in new Expression[] {
                (Expression<Func<string, int>>)(s => s.Length),
            })
            {
                Assert.ThrowsException<NotSupportedException>(() => ws.Visit(e));
            }
        }

        [TestMethod]
        public void ExpressionMemberAllowListScanner_DeclaringTypes_IncludeBase()
        {
            var ws = new ExpressionMemberAllowListScanner
            {
                DeclaringTypes =
                {
                    { typeof(IList<>), true },
                    { typeof(Foo), true },
                    { typeof(Baz), true },
                }
            };

            foreach (var e in new Expression[] {
                (Expression<Action<List<int>, int>>)((xs, x) => xs.Add(x)),
                (Expression<Func<List<int>, int>>)(xs => xs.Count),
                (Expression<Func<List<int>, int>>)(xs => xs.IndexOf(42)),
                (Expression<Action<Foo>>)(f => f.Qux()),
                (Expression<Action<Baz>>)(f => f.Qux()),
            })
            {
                Assert.AreSame(e, ws.Visit(e));
            }

            foreach (var e in new Expression[] {
                (Expression<Func<string, int>>)(s => s.Length),
            })
            {
                Assert.ThrowsException<NotSupportedException>(() => ws.Visit(e));
            }
        }

        [TestMethod]
        public void ExpressionMemberAllowListScanner_Mixed_ElementInitializer()
        {
            var ws = new ExpressionMemberAllowListScanner
            {
                DeclaringTypes =
                {
                    { typeof(IList<>), true },
                },

                Members =
                {
                    { typeof(List<int>).GetConstructor(Type.EmptyTypes) },
                }
            };

            foreach (var e in new Expression[] {
                (Expression<Func<List<int>>>)(() => new List<int> { 2, 3, 5 }),
            })
            {
                Assert.AreSame(e, ws.Visit(e));
            }
        }

        [TestMethod]
        public void ExpressionMemberAllowListScanner_DeclaringTypes_Bindings()
        {
            var ws = new ExpressionMemberAllowListScanner
            {
                DeclaringTypes =
                {
                    { typeof(Bar2) },
                    { typeof(Foo2) },
                    { typeof(List<int>) },
                },

                Members =
                {
                    { typeof(Bar3).GetConstructors().Single() },
                    { typeof(Foo3).GetConstructors().Single() },
                    { typeof(HashSet<int>).GetConstructor(Type.EmptyTypes) }
                },
            };

            foreach (var e in new Expression[] {
                (Expression<Func<Bar2>>)(() => new Bar2(1) { Foo1 = { Xs = { 2, 3, 5 } }, Foo2 = { Baz = "" } }),
            })
            {
                Assert.AreSame(e, ws.Visit(e));
            }

            foreach (var e in new Expression[] {
                (Expression<Func<Bar3>>)(() => new Bar3 { Foo1 = { Baz = "" } }),
                (Expression<Func<Bar3>>)(() => new Bar3 { Foo1 = { Xs = { 2, 3, 5 } } }),
                (Expression<Func<Foo3>>)(() => new Foo3 { Xs = { 2, 3, 5 } }),
                (Expression<Func<HashSet<int>>>)(() => new HashSet<int> { 2, 3, 5 }),
            })
            {
                Assert.ThrowsException<NotSupportedException>(() => ws.Visit(e));
            }
        }

        [TestMethod]
        public void ExpressionMemberAllowListScanner_DeclaringTypes_Interfaces1()
        {
            var ws = new ExpressionMemberAllowListScanner
            {
                DeclaringTypes =
                {
                    { typeof(IEnumerator<int>) },
                },
            };

            foreach (var e in new Expression[] {
                (Expression<Func<IEnumerator<int>, int>>)(r => r.Current),
            })
            {
                Assert.AreSame(e, ws.Visit(e));
            }

            foreach (var e in new Expression[] {
                (Expression<Func<IEnumerator<int>, bool>>)(r => r.MoveNext() /* defined on IEnumerator */),
                (Expression<Func<IEnumerator<int>, int>>)(r => (int)((IEnumerator)r).Current),
#pragma warning disable IDE0004 // Remove Unnecessary Cast. (Keeping inside expression tree.)
                (Expression<Func<IEnumerator<int>, bool>>)(r => ((IEnumerator)r).MoveNext()),
#pragma warning restore IDE0004
                (Expression<Func<IEnumerator<string>, string>>)(r => r.Current),
                (Expression<Func<IEnumerator<string>, bool>>)(r => r.MoveNext()),
            })
            {
                Assert.ThrowsException<NotSupportedException>(() => ws.Visit(e));
            }
        }

        [TestMethod]
        public void ExpressionMemberAllowListScanner_DeclaringTypes_Interfaces2()
        {
            var ws = new ExpressionMemberAllowListScanner
            {
                DeclaringTypes =
                {
                    { typeof(IEnumerator<int>), true },
                },
            };

            foreach (var e in new Expression[] {
                (Expression<Func<IEnumerator<int>, int>>)(r => r.Current),
                (Expression<Func<IEnumerator<int>, bool>>)(r => r.MoveNext()),
                (Expression<Func<IEnumerator<int>, int>>)(r => (int)((IEnumerator)r).Current),
#pragma warning disable IDE0004 // Remove Unnecessary Cast. (Keeping inside expression tree.)
                (Expression<Func<IEnumerator<int>, bool>>)(r => ((IEnumerator)r).MoveNext()),
#pragma warning restore IDE0004
                (Expression<Func<IEnumerator<string>, bool>>)(r => r.MoveNext() /* defined on IEnumerator */),
            })
            {
                Assert.AreSame(e, ws.Visit(e));
            }

            foreach (var e in new Expression[] {
                (Expression<Func<IEnumerator<string>, string>>)(r => r.Current),
            })
            {
                Assert.ThrowsException<NotSupportedException>(() => ws.Visit(e));
            }
        }

        [TestMethod]
        public void ExpressionMemberAllowListScanner_DeclaringTypes_Interfaces3()
        {
            var ws = new ExpressionMemberAllowListScanner
            {
                DeclaringTypes =
                {
                    { typeof(IEnumerator<>) },
                },
            };

            foreach (var e in new Expression[] {
                (Expression<Func<IEnumerator<int>, int>>)(r => r.Current),
                (Expression<Func<IEnumerator<string>, string>>)(r => r.Current),
            })
            {
                Assert.AreSame(e, ws.Visit(e));
            }

            foreach (var e in new Expression[] {
                (Expression<Func<IEnumerator<int>, bool>>)(r => r.MoveNext() /* defined on IEnumerator */),
                (Expression<Func<IEnumerator<string>, bool>>)(r => r.MoveNext() /* defined on IEnumerator */),
                (Expression<Func<IEnumerator<int>, int>>)(r => (int)((IEnumerator)r).Current),
#pragma warning disable IDE0004 // Remove Unnecessary Cast. (Keeping inside expression tree.)
                (Expression<Func<IEnumerator<int>, bool>>)(r => ((IEnumerator)r).MoveNext()),
#pragma warning restore IDE0004
            })
            {
                Assert.ThrowsException<NotSupportedException>(() => ws.Visit(e));
            }
        }

        [TestMethod]
        public void ExpressionMemberAllowListScanner_DeclaringTypes_Interfaces4()
        {
            var ws = new ExpressionMemberAllowListScanner
            {
                DeclaringTypes =
                {
                    { typeof(IEnumerator<>), true },
                },
            };

            foreach (var e in new Expression[] {
                (Expression<Func<IEnumerator<int>, int>>)(r => r.Current),
                (Expression<Func<IEnumerator<string>, string>>)(r => r.Current),
                (Expression<Func<IEnumerator<int>, int>>)(r => (int)((IEnumerator)r).Current),
                (Expression<Func<IEnumerator<int>, bool>>)(r => r.MoveNext()),
                (Expression<Func<IEnumerator<string>, bool>>)(r => r.MoveNext()),
#pragma warning disable IDE0004 // Remove Unnecessary Cast. (Keeping inside expression tree.)
                (Expression<Func<IEnumerator<int>, bool>>)(r => ((IEnumerator)r).MoveNext()),
#pragma warning restore IDE0004
            })
            {
                Assert.AreSame(e, ws.Visit(e));
            }
        }

        [TestMethod]
        public void ExpressionMemberAllowListScanner_DeclaringTypes_Interfaces5()
        {
            var ws = new ExpressionMemberAllowListScanner
            {
                DeclaringTypes =
                {
                    { typeof(IEnumerable<>), true },
                },
            };

            foreach (var e in new Expression[] {
                (Expression<Func<IEnumerable<int>, IEnumerator<int>>>)(r => r.GetEnumerator()),
                (Expression<Func<IEnumerable<int>, IEnumerator>>)(r => ((IEnumerable)r).GetEnumerator()),
            })
            {
                Assert.AreSame(e, ws.Visit(e));
            }
        }

        [TestMethod]
        public void ExpressionMemberAllowListScanner_DeclaringTypes_Interfaces6()
        {
            var ws = new ExpressionMemberAllowListScanner
            {
                DeclaringTypes =
                {
                    { typeof(IEnumerable<>) },
                },
            };

            foreach (var e in new Expression[] {
                (Expression<Func<IEnumerable<int>, IEnumerator<int>>>)(r => r.GetEnumerator()),
            })
            {
                Assert.AreSame(e, ws.Visit(e));
            }

            foreach (var e in new Expression[] {
                (Expression<Func<IEnumerable<int>, IEnumerator>>)(r => ((IEnumerable)r).GetEnumerator()),
            })
            {
                Assert.ThrowsException<NotSupportedException>(() => ws.Visit(e));
            }
        }

        [TestMethod]
        public void ExpressionMemberAllowListScanner_DeclaringTypes_Indexers1()
        {
            var ws = new ExpressionMemberAllowListScanner
            {
                DeclaringTypes =
                {
                    { typeof(IDictionary<,>) },
                },
            };

            foreach (var e in new Expression[] {
                (Expression<Func<IDictionary<int, bool>, bool>>)(d => d[0]),
                (Expression<Func<Dictionary<int, bool>, bool>>)(d => d[0]),
                (Expression<Func<IDictionary<string, object>, object>>)(d => d["foo"]),
                (Expression<Func<Dictionary<string, object>, object>>)(d => d["foo"]),
            })
            {
                Assert.AreSame(e, ws.Visit(e));
            }
        }

        [TestMethod]
        public void ExpressionMemberAllowListScanner_DeclaringTypes_Indexers2()
        {
            var ws = new ExpressionMemberAllowListScanner
            {
                DeclaringTypes =
                {
                    { typeof(IDictionary<string, int>) },
                },
            };

            foreach (var e in new Expression[] {
                (Expression<Func<IDictionary<string, int>, int>>)(d => d["foo"]),
                (Expression<Func<Dictionary<string, int>, int>>)(d => d["foo"]),
            })
            {
                Assert.AreSame(e, ws.Visit(e));
            }

            foreach (var e in new Expression[] {
                (Expression<Func<IDictionary<string, bool>, bool>>)(d => d["foo"]),
                (Expression<Func<IDictionary<bool, int>, int>>)(d => d[true]),
            })
            {
                Assert.ThrowsException<NotSupportedException>(() => ws.Visit(e));
            }
        }

        [TestMethod]
        public void ExpressionMemberAllowListScanner_Member_Simple()
        {
#pragma warning disable IDE0057 // Substring can be simplified. (https://github.com/dotnet/roslyn/issues/49347)
            var ws = new ExpressionMemberAllowListScanner
            {
                Members =
                {
                    ReflectionHelpers.InfoOf(() => Environment.TickCount),
                    ReflectionHelpers.InfoOf(() => Environment.TickCount), // Duplicates are okay
                    ReflectionHelpers.InfoOf(() => Console.ReadLine()),
                    ReflectionHelpers.InfoOf((string s) => s.Length),
                    ReflectionHelpers.InfoOf((string s) => s.Substring(1)),
                }
            };

            foreach (var e in new Expression[] {
                (Expression<Func<int>>)(() => Environment.TickCount),
                (Expression<Func<string>>)(() => Console.ReadLine()),
                (Expression<Func<string, int>>)(s => s.Length),
                (Expression<Func<string, string>>)(s => s.Substring(7)),
            })
            {
                Assert.AreSame(e, ws.Visit(e));
            }

            foreach (var e in new Expression[] {
                (Expression<Func<string>>)(() => Environment.MachineName),
                (Expression<Func<string, string>>)(s => s.Substring(0, 1)),
            })
            {
                Assert.ThrowsException<NotSupportedException>(() => ws.Visit(e));
            }
#pragma warning restore IDE0057
        }

        [TestMethod]
        public void ExpressionMemberAllowListScanner_Member_Operators()
        {
            var ws = new ExpressionMemberAllowListScanner
            {
                Members =
                {
                    ReflectionHelpers.InfoOf(() => DateTime.Now + TimeSpan.Zero),
                    ReflectionHelpers.InfoOf(() => -TimeSpan.Zero),
                }
            };

            foreach (var e in new Expression[] {
                (Expression<Func<DateTime, TimeSpan, DateTime>>)((d, t) => d + t),
                (Expression<Func<TimeSpan, TimeSpan>>)(t => -t),
            })
            {
                Assert.AreSame(e, ws.Visit(e));
            }

            foreach (var e in new Expression[] {
                (Expression<Func<DateTime, TimeSpan, DateTime>>)((d, t) => d - t),
                (Expression<Func<TimeSpan, TimeSpan>>)(t => +t),
            })
            {
                Assert.ThrowsException<NotSupportedException>(() => ws.Visit(e));
            }
        }

        [TestMethod]
        public void ExpressionMemberAllowListScanner_Member_GenericOpen()
        {
            var ws = new ExpressionMemberAllowListScanner
            {
                Members =
                {
                    ((MethodInfo)ReflectionHelpers.InfoOf(() => Activator.CreateInstance<int>())).GetGenericMethodDefinition(),
                    typeof(List<>).GetMethod("Add")
                }
            };

            foreach (var e in new Expression[] {
                (Expression<Func<int>>)(() => Activator.CreateInstance<int>()),
                (Expression<Func<string>>)(() => Activator.CreateInstance<string>()),
                (Expression<Action<List<int>, int>>)((xs, x) => xs.Add(x)),
            })
            {
                Assert.AreSame(e, ws.Visit(e));
            }

            foreach (var e in new Expression[] {
                (Expression<Action<List<int>, int>>)((xs, x) => xs.Remove(x)),
            })
            {
                Assert.ThrowsException<NotSupportedException>(() => ws.Visit(e));
            }
        }

        [TestMethod]
        public void ExpressionMemberAllowListScanner_Member_GenericClosed()
        {
            var ws = new ExpressionMemberAllowListScanner
            {
                Members =
                {
                    ReflectionHelpers.InfoOf(() => Activator.CreateInstance<int>()),
                    ReflectionHelpers.InfoOf((List<int> xs) => xs.Contains(1)),
                }
            };

            foreach (var e in new Expression[] {
                (Expression<Func<int>>)(() => Activator.CreateInstance<int>()),
                (Expression<Func<List<int>, int, bool>>)((xs, x) => xs.Contains(x)),
            })
            {
                Assert.AreSame(e, ws.Visit(e));
            }

            foreach (var e in new Expression[] {
                (Expression<Func<string>>)(() => Activator.CreateInstance<string>()),
                (Expression<Func<List<string>, string, bool>>)((xs, x) => xs.Contains(x)),
                (Expression<Func<List<int>, int>>)(xs => xs.Count),
            })
            {
                Assert.ThrowsException<NotSupportedException>(() => ws.Visit(e));
            }
        }

        [TestMethod]
        public void ExpressionMemberAllowListScanner_ManOrBoyTest1()
        {
            var ws = new ExpressionMemberAllowListScanner
            {
                DeclaringTypes =
                {
                    typeof(Queryable),
                    typeof(string),
                    typeof(int),
                    typeof(Tuple<,>),
                }
            };

            var e = (from x in new[] { 1, 2, 3 }.AsQueryable()
                     where x > 0
                     let y = x * x
                     let z = x.ToString()
                     select z.ToUpper())
                    .Expression;

            e = AnonymousTypeTupletizer.Tupletize(e, Expression.Constant(value: null));

            Assert.AreSame(e, ws.Visit(e));
        }

        [TestMethod]
        public void ExpressionMemberAllowListScanner_ManOrBoyTest2()
        {
            var ws = new AnonymousTypeTolerantExpressionMemberAllowListScanner
            {
                DeclaringTypes =
                {
                    typeof(Queryable),
                    typeof(string),
                    typeof(int),
                    typeof(Tuple<,>),
                }
            };

#pragma warning disable IDE0050 // Convert to tuple. (Test for anonymous types.)

            var e = (from x in new[] { 1, 2, 3 }.AsQueryable()
                     select new { a = x, b = x.ToString() })
                    .Expression;

#pragma warning restore IDE0050

            Assert.AreSame(e, ws.Visit(e));
        }

        private sealed class AnonymousTypeTolerantExpressionMemberAllowListScanner : ExpressionMemberAllowListScanner
        {
            protected override bool Check(MemberInfo member)
            {
                if (member.DeclaringType.IsAnonymousType())
                {
                    return true;
                }

                return base.Check(member);
            }
        }

        [TestMethod]
        public void ExpressionMemberAllowListScanner_Funcletization()
        {
            var ws = new FuncletizingExpressionMemberAllowListScanner
            {
                DeclaringTypes =
                {
                    typeof(string),
                }
            };

            var e = (Expression<Func<string, string>>)(s => s + Environment.MachineName);

            var f = (Expression<Func<string, string>>)ws.Visit(e);

            var t = f.Compile()("Foo");

            Assert.AreEqual("Foo" + Environment.MachineName, t);
        }

        private sealed class FuncletizingExpressionMemberAllowListScanner : ExpressionMemberAllowListScanner
        {
            protected override Expression ResolveExpression<T>(T expression, MemberInfo member, Func<T, Expression> visit)
            {
                return Expression.Constant(expression.Evaluate(), expression.Type); // ignores boxing for testing
            }
        }

#pragma warning disable IDE0060 // Remove unused parameter
#pragma warning disable 0649
        private class Bar
        {
            public virtual void Qux()
            {
            }
        }

        private sealed class Foo : Bar
        {
            public override void Qux() => base.Qux();
        }

        private sealed class Baz : Bar
        {
        }

        private sealed class Bar2
        {
            public Bar2(int x)
            {
            }

            public Foo2 Foo1;
            public Foo2 Foo2 { get; set; }
        }

        private sealed class Foo2
        {
            public List<int> Xs { get; private set; }
            public string Baz { get; set; }
        }

        private sealed class Bar3
        {
            public Foo3 Foo1;
            public Foo3 Foo2 { get; set; }
        }

        private sealed class Foo3
        {
            public HashSet<int> Xs { get; private set; }
            public string Baz { get; set; }
        }
#pragma warning restore 0649
#pragma warning restore IDE0060 // Remove unused parameter
    }
}
