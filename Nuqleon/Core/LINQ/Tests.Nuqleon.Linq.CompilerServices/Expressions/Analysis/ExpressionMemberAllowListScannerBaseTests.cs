// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class ExpressionMemberAllowListScannerBaseTests
    {
        [TestMethod]
        public void ExpressionMemberAllowListScannerBase_AllPass()
        {
            var aps = new AllPassScanner();

            foreach (var e in new Expression[] {
                (Expression<Func<int, int>>)(x => -x),
                (Expression<Func<int, int, int>>)((a, b) => a + b),
                (Expression<Func<TimeSpan, TimeSpan>>)(t => -t),
                (Expression<Func<DateTime, TimeSpan, DateTime>>)((d, t) => d + t),
                (Expression<Func<DateTime>>)(() => DateTime.Now),
                (Expression<Func<DateTime, int>>)(d => d.Year),
                (Expression<Func<string, string>>)(s => s.ToUpper()),
                (Expression<Func<string, bool>>)(s => string.IsNullOrEmpty(s)),
                (Expression<Func<TimeSpan>>)(() => new TimeSpan(1, 2, 3)),
                Expression.MakeIndex(Expression.Parameter(typeof(Dictionary<int, int>)), typeof(Dictionary<int, int>).GetProperty("Item"), new[] { Expression.Constant(1) }),
                (Expression<Func<List<int>>>)(() => new List<int> { 1 }),
                (Expression<Func<Bar>>)(() => new Bar { Foo = 1 }),
            })
            {
                Assert.AreSame(e, aps.Visit(e));
            }
        }

        [TestMethod]
        public void ExpressionMemberAllowListScannerBase_NoPass()
        {
            var aps = new NoPassScanner();

            foreach (var e in new Expression[] {
                (Expression<Func<int, int>>)(x => -x),
                (Expression<Func<int, int, int>>)((a, b) => a + b),
            })
            {
                Assert.AreSame(e, aps.Visit(e));
            }

            foreach (var e in new Expression[] {
                (Expression<Func<TimeSpan, TimeSpan>>)(t => -t),
                (Expression<Func<DateTime, TimeSpan, DateTime>>)((d, t) => d + t),
                (Expression<Func<DateTime>>)(() => DateTime.Now),
                (Expression<Func<DateTime, int>>)(d => d.Year),
                (Expression<Func<string, string>>)(s => s.ToUpper()),
                (Expression<Func<string, bool>>)(s => string.IsNullOrEmpty(s)),
                (Expression<Func<TimeSpan>>)(() => new TimeSpan(1, 2, 3)),
                Expression.MakeIndex(Expression.Parameter(typeof(Dictionary<int, int>)), typeof(Dictionary<int, int>).GetProperty("Item"), new[] { Expression.Constant(1) }),
                (Expression<Func<List<int>>>)(() => new List<int> { 1 }),
                (Expression<Func<Bar>>)(() => new Bar { Foo = 1 }),
            })
            {
                Assert.ThrowsException<NotSupportedException>(() => aps.Visit(e));
            }
        }

        [TestMethod]
        public void ExpressionMemberAllowListScannerBase_SomePass()
        {
            var aps = new SomePassScanner();

            foreach (var e in new Expression[] {
                (Expression<Func<int, int>>)(x => -x),
                (Expression<Func<int, int, int>>)((a, b) => a + b),
                (Expression<Func<DateTime, TimeSpan, DateTime>>)((d, t) => d + t),
                (Expression<Func<DateTime>>)(() => DateTime.Now),
                (Expression<Func<DateTime, int>>)(d => d.Year),
                (Expression<Func<string, string>>)(s => s.ToUpper()),
                (Expression<Func<string, bool>>)(s => string.IsNullOrEmpty(s)),
                (Expression<Func<Bar>>)(() => new Bar { Foo = 1 }),
            })
            {
                Assert.AreSame(e, aps.Visit(e));
            }

            foreach (var e in new Expression[] {
                (Expression<Func<TimeSpan, TimeSpan>>)(t => -t),
                (Expression<Func<TimeSpan>>)(() => new TimeSpan(1, 2, 3)),
                Expression.MakeIndex(Expression.Parameter(typeof(Dictionary<int, int>)), typeof(Dictionary<int, int>).GetProperty("Item"), new[] { Expression.Constant(1) }),
                (Expression<Func<List<int>>>)(() => new List<int> { 1 }),
            })
            {
                Assert.ThrowsException<NotSupportedException>(() => aps.Visit(e));
            }
        }

        [TestMethod]
        public void ExpressionMemberAllowListScannerBase_Custom()
        {
            var cps = new CustomScanner();

            foreach (var e in new Expression[] {
                (Expression<Func<string, string>>)(s => s.ToLower()),
                (Expression<Func<Bar>>)(() => new Bar { Foo = 1 }),
                (Expression<Func<List<int>>>)(() => new List<int> { 2 }),
            })
            {
                Assert.AreSame(e, cps.Visit(e));
            }

            foreach (var e in new Expression[] {
                (Expression<Func<string, string>>)(s => s.TrimStart()),
                (Expression<Func<Bar>>)(() => new Bar { Qux = 1 }),
                (Expression<Func<Dictionary<string, int>>>)(() => new Dictionary<string, int> { { "bar", 2 } }),
            })
            {
                Assert.ThrowsException<NotSupportedException>(() => cps.Visit(e));
            }
        }

        private sealed class Bar
        {
            public int Foo { get; set; }
            public int Qux { get; set; }
        }

        private sealed class AllPassScanner : ExpressionMemberAllowListScannerBase
        {
            protected override bool Check(MemberInfo member) => true;
        }

        private sealed class NoPassScanner : ExpressionMemberAllowListScannerBase
        {
            protected override bool Check(MemberInfo member) => false;
        }

        private sealed class SomePassScanner : ExpressionMemberAllowListScannerBase
        {
            protected override bool Check(MemberInfo member)
            {
                return member.DeclaringType == typeof(DateTime) || member.DeclaringType == typeof(string) || member.DeclaringType == typeof(Bar);
            }
        }

        private sealed class CustomScanner : ExpressionMemberAllowListScannerBase
        {
            protected override bool Check(MemberInfo member) => false;

            protected override Expression ResolveExpression<T>(T expression, MemberInfo member, Func<T, Expression> visit)
            {
                if (member.MemberType is MemberTypes.Property or MemberTypes.Method)
                {
                    if (member.Name.Contains("e"))
                    {
                        return visit(expression);
                    }
                    else
                    {
                        return base.ResolveExpression<T>(expression, member, visit);
                    }
                }

                return visit(expression);
            }

            protected override ElementInit ResolveElementInit(ElementInit initializer, Func<ElementInit, ElementInit> visit)
            {
                if (initializer.Arguments.Count == 1)
                {
                    return visit(initializer);
                }
                else
                {
                    return base.ResolveElementInit(initializer, visit);
                }
            }

            protected override MemberBinding ResolveMemberBinding<T>(T binding, Func<T, MemberBinding> visit)
            {
                if (binding.Member.Name.Contains("o"))
                {
                    return visit(binding);
                }
                else
                {
                    return base.ResolveMemberBinding<T>(binding, visit);
                }
            }
        }
    }
}
