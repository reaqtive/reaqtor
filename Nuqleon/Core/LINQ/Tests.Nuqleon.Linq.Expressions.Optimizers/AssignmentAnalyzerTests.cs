// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Linq.Expressions.Optimizers
{
    [TestClass]
    public class AssignmentAnalyzerTests
    {
        [TestMethod]
        public void AssignmentAnalyzer_Assign()
        {
            var x = Expression.Parameter(typeof(int));

            var b =
                Expression.Block(
                    new[] { x },
                    Expression.Constant(0),
                    Expression.Assign(x, Expression.Constant(1)),
                    Expression.Constant(2)
                );

            var analyze = new AssignmentAnalyzer(b.Variables);

            var res = analyze.Visit(b.Expressions);

            Assert.AreSame(b.Expressions, res);

            Assert.AreEqual(0, analyze.Unassigned.Count);
        }

        [TestMethod]
        public void AssignmentAnalyzer_ByRef()
        {
            var read = typeof(Volatile).GetMethod(nameof(Volatile.Read), new[] { typeof(int).MakeByRefType() });

            var x = Expression.Parameter(typeof(int));

            var b =
                Expression.Block(
                    new[] { x },
                    Expression.Constant(0),
                    Expression.Call(read, x),
                    Expression.Constant(2)
                );

            var analyze = new AssignmentAnalyzer(b.Variables);

            var res = analyze.Visit(b.Expressions);

            Assert.AreSame(b.Expressions, res);

            Assert.AreEqual(0, analyze.Unassigned.Count);
        }

        [TestMethod]
        public void AssignmentAnalyzer_RuntimeVariables()
        {
            var x = Expression.Parameter(typeof(int));

            var b =
                Expression.Block(
                    new[] { x },
                    Expression.RuntimeVariables(x)
                );

            var analyze = new AssignmentAnalyzer(b.Variables);

            var res = analyze.Visit(b.Expressions);

            Assert.AreSame(b.Expressions, res);

            Assert.AreEqual(0, analyze.Unassigned.Count);
        }

        [TestMethod]
        public void AssignmentAnalyzer_Quote()
        {
            var x = Expression.Parameter(typeof(int));

            var b =
                Expression.Block(
                    new[] { x },
                    Expression.Quote(Expression.Lambda<Func<int>>(x))
                );

            var analyze = new AssignmentAnalyzer(b.Variables);

            var res = analyze.Visit(b.Expressions);

            Assert.AreSame(b.Expressions, res);

            Assert.AreEqual(0, analyze.Unassigned.Count);
        }

        [TestMethod]
        public void AssignmentAnalyzer_Unassigned()
        {
            var x = Expression.Parameter(typeof(int));

            var b =
                Expression.Block(
                    new[] { x },
                    x
                );

            var analyze = new AssignmentAnalyzer(b.Variables);

            var res = analyze.Visit(b.Expressions);

            Assert.AreSame(b.Expressions, res);

            Assert.IsTrue(new[] { x }.SequenceEqual(analyze.Unassigned));
        }
    }
}
