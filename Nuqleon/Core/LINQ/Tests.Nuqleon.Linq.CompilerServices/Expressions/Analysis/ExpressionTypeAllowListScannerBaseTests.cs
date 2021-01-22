// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class ExpressionTypeAllowListScannerBaseTests
    {
        [TestMethod]
        public void ExpressionTypeAllowListScannerBase_AllPass()
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
            })
            {
                Assert.AreSame(e, aps.Visit(e));
            }
        }

        [TestMethod]
        public void ExpressionTypeAllowListScannerBase_NoPass()
        {
            var aps = new NoPassScanner();

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
            })
            {
                Assert.ThrowsException<NotSupportedException>(() => aps.Visit(e));
            }
        }

        [TestMethod]
        public void ExpressionTypeAllowListScannerBase_SomePass()
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
            })
            {
                Assert.AreSame(e, aps.Visit(e));
            }

            foreach (var e in new Expression[] {
                (Expression<Func<Process>>)(() => Process.Start("notepad.exe")),
                (Expression<Func<FileStream>>)(() => File.OpenRead("foo.txt")),
                Expression.MakeIndex(Expression.Parameter(typeof(Dictionary<int, int>)), typeof(Dictionary<int, int>).GetProperty("Item"), new[] { Expression.Constant(1) }),
                (Expression<Func<List<int>>>)(() => new List<int> { 1 }),
            })
            {
                Assert.ThrowsException<NotSupportedException>(() => aps.Visit(e));
            }
        }

        [TestMethod]
        public void ExpressionTypeAllowListScannerBase_Custom()
        {
            var cps = new CustomScanner();

            foreach (var e in new Expression[]
            {
                (Expression<Func<DateTime, DateTime>>)(x => x),
                (Expression<Func<TimeSpan, TimeSpan>>)(x => x),
            })
            {
                Assert.AreSame(e, cps.Visit(e));
            }

            foreach (var e in new Expression[]
            {
                (Expression<Func<Int32, Int32>>)(x => x),
            })
            {
                Assert.ThrowsException<NotSupportedException>(() => cps.Visit(e));
            }
        }

        private sealed class AllPassScanner : ExpressionTypeAllowListScannerBase
        {
            protected override bool Check(Type type) => true;
        }

        private sealed class NoPassScanner : ExpressionTypeAllowListScannerBase
        {
            protected override bool Check(Type type) => false;
        }

        private sealed class SomePassScanner : ExpressionTypeAllowListScannerBase
        {
            protected override bool Check(Type type) => type.Namespace == "System";
        }

        private sealed class CustomScanner : ExpressionTypeAllowListScannerBase
        {
            protected override bool Check(Type type) => false;

            protected override Expression ResolveExpression<T>(T expression, Type type, Func<T, Expression> visit)
            {
                if (typeof(Delegate).IsAssignableFrom(type) || type.Name.Contains("e"))
                {
                    return visit(expression);
                }
                else
                {
                    return base.ResolveExpression<T>(expression, type, visit);
                }
            }
        }

    }
}
