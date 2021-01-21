// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// PS - February 2015 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq.CompilerServices.Optimizers;
using System.Linq.Expressions;

namespace Tests.System.Linq.CompilerServices.Optimizers
{
    [TestClass]
    public class OptimizerCombinatorTests
    {
        [TestMethod]
        public void OptimizerCombinators_ArgumentChecks()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => Optimizer.Then(first: null, Optimizer.Nop()), AssertParameterName("first"));
            AssertEx.ThrowsException<ArgumentNullException>(() => Optimizer.Nop().Then(second: null), AssertParameterName("second"));

            AssertEx.ThrowsException<ArgumentNullException>(() => Optimizer.FixedPoint(optimizer: null), AssertParameterName("optimizer"));

            AssertEx.ThrowsException<ArgumentNullException>(() => Optimizer.FixedPoint(optimizer: null, throwOnCycle: true), AssertParameterName("optimizer"));

            AssertEx.ThrowsException<ArgumentNullException>(() => Optimizer.FixedPoint(optimizer: null, throwOnCycle: true, maxIterations: 0), AssertParameterName("optimizer"));
            AssertEx.ThrowsException<ArgumentOutOfRangeException>(() => Optimizer.Nop().FixedPoint(throwOnCycle: true, maxIterations: -1), AssertParameterName("maxIterations"));
        }

        [TestMethod]
        public void OptimizerCombinators_Nop()
        {
            var factory = DefaultQueryExpressionFactory.Instance;
            var qt = factory.LambdaAbstraction(Expression.Lambda(Expression.Empty()));
            Assert.AreSame(qt, Optimizer.Nop().Optimize(qt));
        }

        [TestMethod]
        public void OptimizerCombinators_Then()
        {
            var factory = DefaultQueryExpressionFactory.Instance;
            var qt = factory.LambdaAbstraction(Expression.Lambda(Expression.Empty()));

            {
                var str = string.Empty;
                new MockOptimizer(_ => { str += "first"; return _; })
                    .Then(new MockOptimizer(_ => { str += "second"; return _; }))
                    .Optimize(qt);
                Assert.AreEqual("firstsecond", str);
            }

            {
                var str = string.Empty;
                new MockOptimizer(_ => { str += "first"; return _; })
                    .Then(new MockOptimizer(_ => { str += "second"; return _; }))
                    .Then(new MockOptimizer(_ => { str += "third"; return _; }))
                    .Optimize(qt);
                Assert.AreEqual("firstsecondthird", str);
            }
        }

        [TestMethod]
        public void OptimizerCombinators_FixedPointSimple()
        {
            var factory = DefaultQueryExpressionFactory.Instance;
            var start = factory.LambdaAbstraction(Expression.Lambda(Expression.Empty()));
            var intermediate = factory.LambdaAbstraction(Expression.Lambda(Expression.Default(typeof(int))));
            var end = factory.LambdaAbstraction(Expression.Lambda(Expression.Constant(42)));

            // zero args
            {
                {

                    var counter = 0;
                    var res =
                        new MockOptimizer(_ => ++counter == 1 ? intermediate : end)
                            .FixedPoint()
                            .Optimize(start);
                    Assert.AreSame(end, res);
                    Assert.AreEqual(3, counter); // start -> intermediate -> end -> end
                }

                {

                    var counter = 0;
                    var res =
                        new MockOptimizer(_ => { counter++; return start; })
                            .FixedPoint()
                            .Optimize(start);
                    Assert.AreSame(start, res);
                    Assert.AreEqual(1, counter); // start -> start
                }

                {

                    var counter = 0;
                    Assert.ThrowsException<InvalidOperationException>(() =>
                        new MockOptimizer(_ => ++counter == 1 ? intermediate : (counter == 2 ? end : start))
                            .FixedPoint(throwOnCycle: true)
                            .Optimize(start));
                    Assert.AreEqual(3, counter); // start -> intermediate -> end -> start
                }
            }

            // one arg
            {
                {

                    var counter = 0;
                    var res =
                        new MockOptimizer(_ => ++counter == 1 ? intermediate : end)
                            .FixedPoint(throwOnCycle: false)
                            .Optimize(start);
                    Assert.AreSame(end, res);
                    Assert.AreEqual(3, counter); // start -> intermediate -> end -> end
                }

                {

                    var counter = 0;
                    var res =
                        new MockOptimizer(_ => { counter++; return start; })
                            .FixedPoint(throwOnCycle: false)
                            .Optimize(start);
                    Assert.AreSame(start, res);
                    Assert.AreEqual(1, counter); // start -> start
                }

                {

                    var counter = 0;
                    var res =
                        new MockOptimizer(_ => ++counter == 1 ? intermediate : (counter == 2 ? end : start))
                            .FixedPoint(throwOnCycle: false)
                            .Optimize(start);
                    Assert.AreSame(start, res);
                    Assert.AreEqual(3, counter); // start -> intermediate -> end -> start
                }

                {

                    var counter = 0;
                    var res =
                        new MockOptimizer(_ => ++counter == 1 ? intermediate : end)
                            .FixedPoint(throwOnCycle: true)
                            .Optimize(start);
                    Assert.AreSame(end, res);
                    Assert.AreEqual(3, counter); // start -> intermediate -> end -> end
                }

                {

                    var counter = 0;
                    var res =
                        new MockOptimizer(_ => { counter++; return start; })
                            .FixedPoint(throwOnCycle: true)
                            .Optimize(start);
                    Assert.AreSame(start, res);
                    Assert.AreEqual(1, counter); // start -> start
                }

                {

                    var counter = 0;
                    Assert.ThrowsException<InvalidOperationException>(() =>
                        new MockOptimizer(_ => ++counter == 1 ? intermediate : (counter == 2 ? end : start))
                            .FixedPoint(throwOnCycle: true)
                            .Optimize(start));
                    Assert.AreEqual(3, counter); // start -> intermediate -> end -> start
                }
            }

            // two args
            {
                {
                    var counter = 0;
                    var res =
                        new MockOptimizer(_ => { counter++; return end; })
                            .FixedPoint(throwOnCycle: true, maxIterations: 0)
                            .Optimize(start);
                    Assert.AreSame(start, res);
                    Assert.AreEqual(0, counter); // start
                }

                {
                    var counter = 0;
                    var res =
                        new MockOptimizer(_ => { counter++; return end; })
                            .FixedPoint(throwOnCycle: true, maxIterations: 1)
                            .Optimize(start);
                    Assert.AreSame(end, res);
                    Assert.AreEqual(1, counter); // start -> start
                }

                {
                    var counter = 0;
                    Assert.ThrowsException<InvalidOperationException>(() =>
                        new MockOptimizer(_ => ++counter == 1 ? intermediate : (counter == 2 ? end : start))
                            .FixedPoint(throwOnCycle: true, maxIterations: 3)
                            .Optimize(start));
                    Assert.AreEqual(3, counter); // start -> intermediate -> end -> start
                }

                {
                    var counter = 0;
                    var res =
                        new MockOptimizer(_ => ++counter == 1 ? intermediate : (counter == 2 ? end : start))
                            .FixedPoint(throwOnCycle: true, maxIterations: 2)
                            .Optimize(start);
                    Assert.AreSame(end, res);
                    Assert.AreEqual(2, counter); // start -> intermediate -> end -> start
                }
            }
        }

        private sealed class MockOptimizer : IOptimizer
        {
            private readonly Func<QueryTree, QueryTree> _optimize;

            public MockOptimizer(Func<QueryTree, QueryTree> optimize) => _optimize = optimize;

            public QueryTree Optimize(QueryTree queryTree) => _optimize(queryTree);
        }

        private static Action<ArgumentException> AssertParameterName(string paramName)
        {
            return (ArgumentException ex) => { Assert.AreEqual(paramName, ex.ParamName); };
        }
    }
}
