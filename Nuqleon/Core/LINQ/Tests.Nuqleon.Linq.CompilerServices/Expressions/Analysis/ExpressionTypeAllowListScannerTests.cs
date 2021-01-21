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

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class ExpressionTypeAllowListScannerTests
    {
        [TestMethod]
        public void ExpressionTypeAllowListScanner_Errors()
        {
            var ws = new ExpressionTypeAllowListScanner();

            Assert.ThrowsException<NotSupportedException>(() => ((IEnumerable)ws.Types).GetEnumerator());
            Assert.ThrowsException<NotSupportedException>(() => ((IEnumerable<Type>)ws.Types).GetEnumerator());

            AssertEx.ThrowsException<ArgumentNullException>(() => ws.Types.Add(type: null), ex => Assert.AreEqual("type", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => ws.Types.Add(type: null, includeBase: false), ex => Assert.AreEqual("type", ex.ParamName));
        }

        [TestMethod]
        public void ExpressionTypeAllowListScanner_Simple()
        {
            var ws = new ExpressionTypeAllowListScanner
            {
                Types =
                {
                    typeof(string),
                    typeof(Func<string, string>),
                    typeof(int),
                }
            };

            foreach (var e in new Expression[] {
                (Expression<Func<string, string>>)(s => s.Substring(0, 1).ToUpper()),
            })
            {
                Assert.AreSame(e, ws.Visit(e));
            }

            foreach (var e in new Expression[] {
                (Expression<Func<string, char>>)(s => s[0]),
            })
            {
                Assert.ThrowsException<NotSupportedException>(() => ws.Visit(e));
            }
        }

        [TestMethod]
        public void ExpressionTypeAllowListScanner_Generic1()
        {
            var ws = new ExpressionTypeAllowListScanner
            {
                Types =
                {
                    typeof(List<int>),
                    typeof(HashSet<>),
                    typeof(Func<>),
                }
            };

            foreach (var e in new Expression[] {
                (Expression<Func<List<int>>>)(() => new List<int>()),
                (Expression<Func<HashSet<int>>>)(() => new HashSet<int>()),
                (Expression<Func<HashSet<string>>>)(() => new HashSet<string>()),
            })
            {
                Assert.AreSame(e, ws.Visit(e));
            }

            foreach (var e in new Expression[] {
                (Expression<Func<List<string>>>)(() => new List<string>()),
            })
            {
                Assert.ThrowsException<NotSupportedException>(() => ws.Visit(e));
            }
        }

        [TestMethod]
        public void ExpressionTypeAllowListScanner_Generic2()
        {
            var ws = new ExpressionTypeAllowListScanner
            {
                Types =
                {
                    typeof(IEnumerable<>),
                    typeof(int),
                    typeof(bool),
                    typeof(string),
                    typeof(Func<,>),
                }
            };

            foreach (var e in new Expression[] {
                (Expression<Func<IEnumerable<int>, IEnumerable<string>>>)(xs => from x in xs where x > 0 select x.ToString()),
            })
            {
                Assert.AreSame(e, ws.Visit(e));
            }
        }
    }
}
