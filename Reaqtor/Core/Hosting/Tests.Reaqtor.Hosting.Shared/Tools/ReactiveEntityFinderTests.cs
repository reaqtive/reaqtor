// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - November 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtor;
using Reaqtor.Hosting.Shared.Tools;

namespace Tests.Reaqtor.Remoting.Tools
{
    using Result = Tuple<ReactiveEntityType, string, HashSet<IEnumerable<Expression>>>;

    [TestClass]
    public class ReactiveEntityFinderTests
    {
        [TestMethod]
        public void ReactiveEntityFinder_Test()
        {
            AssertFound(
                Expression.Invoke(Expression.Parameter(typeof(Func<int, IAsyncReactiveQbservable<int>>), "foo"), Expression.Constant(4)),
                new Result(ReactiveEntityType.Observable, "foo", new HashSet<IEnumerable<Expression>> { new[] { Expression.Constant(4) } })
            );

            AssertFound(
                Expression.Invoke(Expression.Parameter(typeof(Func<int, IAsyncReactiveQbserver<int>>), "foo"), Expression.Constant(4)),
                new Result(ReactiveEntityType.Observer, "foo", new HashSet<IEnumerable<Expression>> { new[] { Expression.Constant(4) } })
            );

            AssertFound(
                Expression.Parameter(typeof(IAsyncReactiveQubject<int>), "foo"),
                new Result(ReactiveEntityType.Stream, "foo", new HashSet<IEnumerable<Expression>> { Array.Empty<Expression>() })
            );

            AssertFound(
                Expression.Parameter(typeof(IAsyncReactiveQubject<int, int>), "foo"),
                new Result(ReactiveEntityType.Stream, "foo", new HashSet<IEnumerable<Expression>> { Array.Empty<Expression>() })
            );

            AssertFound(
                Expression.Parameter(typeof(IAsyncReactiveQubjectFactory<int, int>), "foo"),
                new Result(ReactiveEntityType.StreamFactory, "foo", new HashSet<IEnumerable<Expression>> { Array.Empty<Expression>() })
            );

            AssertFound(
                Expression.Parameter(typeof(IAsyncReactiveQubscriptionFactory), "foo"),
                new Result(ReactiveEntityType.SubscriptionFactory, "foo", new HashSet<IEnumerable<Expression>> { Array.Empty<Expression>() })
            );

            AssertFound(
                Expression.Parameter(typeof(IAsyncReactiveQubscriptionFactory<int>), "foo"),
                new Result(ReactiveEntityType.SubscriptionFactory, "foo", new HashSet<IEnumerable<Expression>> { Array.Empty<Expression>() })
            );

            AssertFound(
                Expression.Invoke(
                    Expression.Parameter(typeof(Func<Tuple<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>>, Task<IAsyncReactiveQubscription>>), "Subscription"),
                    Expression.New(
                        typeof(Tuple<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>>).GetConstructors().Single(),
                        Expression.Parameter(typeof(IAsyncReactiveQbservable<int>), "Observable"),
                        Expression.Parameter(typeof(IAsyncReactiveQbserver<int>), "Observer")
                    )
                ),
                new Result(ReactiveEntityType.Observable, "Observable", new HashSet<IEnumerable<Expression>> { Array.Empty<Expression>() }),
                new Result(ReactiveEntityType.Observer, "Observer", new HashSet<IEnumerable<Expression>> { Array.Empty<Expression>() })
            );

            AssertFound(
                Expression.Invoke(
                    Expression.Parameter(typeof(Func<Tuple<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>>, Task<IAsyncReactiveQubscription>>), "Subscription"),
                    Expression.New(
                        typeof(Tuple<IAsyncReactiveQbservable<int>, IAsyncReactiveQbserver<int>>).GetConstructors().Single(),
                        Expression.Invoke(Expression.Parameter(typeof(Func<int, IAsyncReactiveQbservable<int>>), "Observable"), Expression.Constant(42)),
                        Expression.Invoke(Expression.Parameter(typeof(Func<int, IAsyncReactiveQbserver<int>>), "Observer"), Expression.Constant(42))
                    )
                ),
                new Result(ReactiveEntityType.Observable, "Observable", new HashSet<IEnumerable<Expression>> { new[] { Expression.Constant(42) } }),
                new Result(ReactiveEntityType.Observer, "Observer", new HashSet<IEnumerable<Expression>> { new[] { Expression.Constant(42) } })
            );

            AssertFound(Expression.Parameter(typeof(Func<int, IAsyncReactiveQubject<int>>), "foo"));
        }

        private static void AssertFound(Expression expression, params Result[] results)
        {
            var comparer = new ExpressionEqualityComparer(() => new Comparator());
            var found = ReactiveEntityFinder.Find(expression.ToExpressionSlim());
            var total = found.SelectMany(kv => kv.Value).Count();
            Assert.AreEqual(total, results.Length);
            foreach (var result in results)
            {
                Assert.IsTrue(found.ContainsKey(result.Item1));
                Assert.IsTrue(found[result.Item1].ContainsKey(result.Item2));
                var argLists = result.Item3.Zip(found[result.Item1][result.Item2], (ExpectedList, ActualList) => (ExpectedList, ActualList));
                foreach (var (ExpectedList, ActualList) in argLists)
                {
                    var expectedEnumerator = ExpectedList.GetEnumerator();
                    var actualEnumerator = ActualList.GetEnumerator();

                    while (expectedEnumerator.MoveNext())
                    {
                        if (!actualEnumerator.MoveNext())
                        {
                            Assert.Fail(string.Format(CultureInfo.InvariantCulture, "Expected invocation argument expression '{0}' but did not find any.", expectedEnumerator.Current.ToTraceString()));
                        }
                        Assert.IsTrue(
                            comparer.Equals(expectedEnumerator.Current, actualEnumerator.Current.ToExpression()),
                            string.Format(CultureInfo.InvariantCulture, "Expected invocation argument expression '{0}' but received '{1}'.", expectedEnumerator.Current.ToTraceString(), actualEnumerator.Current)
                        );
                    }

                    if (actualEnumerator.MoveNext())
                    {
                        Assert.Fail(string.Format(CultureInfo.InvariantCulture, "Did not expect more invocation argument expressions but received '{0}'.", actualEnumerator.Current));
                    }
                }
            }
        }

        private sealed class Comparator : ExpressionEqualityComparator
        {
            protected override bool EqualsGlobalParameter(ParameterExpression x, ParameterExpression y)
            {
                return x.Name == y.Name && Equals(x.Type, y.Type);
            }
        }
    }
}
