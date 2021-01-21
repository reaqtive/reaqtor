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
using System.Collections.Generic;
using System.Linq.CompilerServices;
using System.Linq.CompilerServices.Optimizers;
using System.Linq.Expressions;

namespace Tests.System.Linq.CompilerServices.Optimizers
{
    [TestClass]
    public class QueryExpressionEqualityComparerTests
    {
        [TestMethod]
        public void QueryExpressionEqualityComparer_Simple()
        {
            var type1 = typeof(int);
            var type2 = typeof(bool);
            var monadMember1 = (MonadMember)new MonadAbstraction(type1, inner: null);
            var monadMember2 = (MonadMember)new MonadAbstraction(type2, inner: null);
            var lambdaAbstraction1 = new LambdaAbstraction(Expression.Lambda(Expression.Constant(42)), EmptyReadOnlyCollection<QueryTree>.Instance);
            var lambdaAbstraction2 = new LambdaAbstraction(Expression.Lambda(Expression.Empty()), EmptyReadOnlyCollection<QueryTree>.Instance);

            foreach (dynamic comparer in new object[] {
                new QueryExpressionEqualityComparer(),
                new QueryExpressionEqualityComparator(),
                new QueryTreeDerivedClassEqualityComparer() })
            {
                {
                    var op = new FirstOperator(type1, monadMember1);
                    Assert.IsTrue(comparer.Equals(op, op));
                    Assert.AreEqual(comparer.GetHashCode(op), comparer.GetHashCode(op));

                    var op2 = new FirstOperator(type1, monadMember1);
                    Assert.IsTrue(comparer.Equals(op, op2));
                    Assert.AreEqual(comparer.GetHashCode(op), comparer.GetHashCode(op2));

                    var op3 = new FirstOperator(type2, monadMember1);
                    Assert.IsFalse(comparer.Equals(op, op3));

                    var op4 = new FirstOperator(type1, monadMember2);
                    Assert.IsFalse(comparer.Equals(op, op4));
                }

                {
                    var op = new FirstPredicateOperator(type1, monadMember1, lambdaAbstraction1);
                    Assert.IsTrue(comparer.Equals(op, op));
                    Assert.AreEqual(comparer.GetHashCode(op), comparer.GetHashCode(op));

                    var op2 = new FirstPredicateOperator(type1, monadMember1, lambdaAbstraction1);
                    Assert.IsTrue(comparer.Equals(op, op2));
                    Assert.AreEqual(comparer.GetHashCode(op), comparer.GetHashCode(op2));

                    var op3 = new FirstPredicateOperator(type2, monadMember1, lambdaAbstraction1);
                    Assert.IsFalse(comparer.Equals(op, op3));

                    var op4 = new FirstPredicateOperator(type1, monadMember2, lambdaAbstraction1);
                    Assert.IsFalse(comparer.Equals(op, op4));

                    var op5 = new FirstPredicateOperator(type1, monadMember1, lambdaAbstraction2);
                    Assert.IsFalse(comparer.Equals(op, op5));
                }

                {
                    var type3 = typeof(long);
                    var type4 = typeof(double);

                    var op = new SelectOperator(type1, type3, monadMember1, lambdaAbstraction1);
                    Assert.IsTrue(comparer.Equals(op, op));
                    Assert.AreEqual(comparer.GetHashCode(op), comparer.GetHashCode(op));

                    var op2 = new SelectOperator(type1, type3, monadMember1, lambdaAbstraction1);
                    Assert.IsTrue(comparer.Equals(op, op2));
                    Assert.AreEqual(comparer.GetHashCode(op), comparer.GetHashCode(op2));

                    var op3 = new SelectOperator(type2, type3, monadMember1, lambdaAbstraction1);
                    Assert.IsFalse(comparer.Equals(op, op3));

                    var op4 = new SelectOperator(type2, type4, monadMember1, lambdaAbstraction1);
                    Assert.IsFalse(comparer.Equals(op, op4));

                    var op5 = new SelectOperator(type1, type3, monadMember2, lambdaAbstraction1);
                    Assert.IsFalse(comparer.Equals(op, op5));

                    var op6 = new SelectOperator(type1, type3, monadMember1, lambdaAbstraction2);
                    Assert.IsFalse(comparer.Equals(op, op6));
                }

                {
                    var op = new TakeOperator(type1, monadMember1, lambdaAbstraction1);
                    Assert.IsTrue(comparer.Equals(op, op));
                    Assert.AreEqual(comparer.GetHashCode(op), comparer.GetHashCode(op));

                    var op2 = new TakeOperator(type1, monadMember1, lambdaAbstraction1);
                    Assert.IsTrue(comparer.Equals(op, op2));
                    Assert.AreEqual(comparer.GetHashCode(op), comparer.GetHashCode(op2));

                    var op3 = new TakeOperator(type2, monadMember1, lambdaAbstraction1);
                    Assert.IsFalse(comparer.Equals(op, op3));

                    var op4 = new TakeOperator(type1, monadMember2, lambdaAbstraction1);
                    Assert.IsFalse(comparer.Equals(op, op4));

                    var op5 = new TakeOperator(type1, monadMember1, lambdaAbstraction2);
                    Assert.IsFalse(comparer.Equals(op, op5));
                }

                {
                    var op = new WhereOperator(type1, monadMember1, lambdaAbstraction1);
                    Assert.IsTrue(comparer.Equals(op, op));
                    Assert.AreEqual(comparer.GetHashCode(op), comparer.GetHashCode(op));

                    var op2 = new WhereOperator(type1, monadMember1, lambdaAbstraction1);
                    Assert.IsTrue(comparer.Equals(op, op2));
                    Assert.AreEqual(comparer.GetHashCode(op), comparer.GetHashCode(op2));

                    var op3 = new WhereOperator(type2, monadMember1, lambdaAbstraction1);
                    Assert.IsFalse(comparer.Equals(op, op3));

                    var op4 = new WhereOperator(type1, monadMember2, lambdaAbstraction1);
                    Assert.IsFalse(comparer.Equals(op, op4));

                    var op5 = new WhereOperator(type1, monadMember1, lambdaAbstraction2);
                    Assert.IsFalse(comparer.Equals(op, op5));
                }

                {
                    var op = new MonadAbstraction(type1, lambdaAbstraction1);
                    Assert.IsTrue(comparer.Equals(op, op));
                    Assert.AreEqual(comparer.GetHashCode(op), comparer.GetHashCode(op));

                    var op2 = new MonadAbstraction(type1, lambdaAbstraction1);
                    Assert.IsTrue(comparer.Equals(op, op2));
                    Assert.AreEqual(comparer.GetHashCode(op), comparer.GetHashCode(op2));

                    var op3 = new MonadAbstraction(type2, lambdaAbstraction1);
                    Assert.IsFalse(comparer.Equals(op, op3));

                    var op4 = new MonadAbstraction(type1, lambdaAbstraction2);
                    Assert.IsFalse(comparer.Equals(op, op4));
                }

                {
                    var p1 = Expression.Parameter(type1);
                    var p2 = Expression.Parameter(type2);
                    var lambda1 = Expression.Lambda(p1, p1);
                    var lambda2 = Expression.Lambda(Expression.Constant(41), p2);
                    var lambda3 = Expression.Lambda(p2, p2);
                    var parameters1 = new QueryTree[] { monadMember1 }.ToReadOnly();
                    var parameters2 = new QueryTree[] { monadMember2 }.ToReadOnly();

                    var op = new LambdaAbstraction(lambda1, parameters1);
                    Assert.IsTrue(comparer.Equals(op, op));
                    Assert.AreEqual(comparer.GetHashCode(op), comparer.GetHashCode(op));

                    var op2 = new LambdaAbstraction(lambda1, parameters1);
                    Assert.IsTrue(comparer.Equals(op, op2));
                    Assert.AreEqual(comparer.GetHashCode(op), comparer.GetHashCode(op2));

                    var op3 = new LambdaAbstraction(lambda2, parameters1);
                    Assert.IsFalse(comparer.Equals(op, op3));

                    var op4 = new LambdaAbstraction(lambda1, parameters2);
                    Assert.IsFalse(comparer.Equals(op, op4));

                    var op5 = new LambdaAbstraction(lambda3, parameters1);
                    Assert.IsTrue(comparer.Equals(op, op5));
                }

                {
                    var op = new FirstOperator(type1, monadMember1);
                    Assert.IsFalse(comparer.Equals(op, lambdaAbstraction1));
                }
            }
        }

        [TestMethod]
        public void QueryExpressionEqualityComparer_NullArguments()
        {
            foreach (dynamic comparer in new object[] {
                new QueryExpressionEqualityComparer(),
                new QueryExpressionEqualityComparator(),
                new QueryTreeDerivedClassEqualityComparer()})
            {
                var type1 = typeof(int);
                var type2 = typeof(bool);
                var monadMember1 = (MonadMember)new MonadAbstraction(type1, inner: null);
                var lambdaAbstraction1 = new LambdaAbstraction(Expression.Lambda(Expression.Constant(42)), EmptyReadOnlyCollection<QueryTree>.Instance);

                {
                    var op = new FirstOperator(type1, monadMember1);
                    var nul = default(FirstOperator);
                    Assert.IsFalse(comparer.Equals(op, nul));
                    Assert.IsFalse(comparer.Equals(nul, op));
                    Assert.IsTrue(comparer.Equals(nul, nul));
                    Assert.AreEqual(17, comparer.GetHashCode(nul));
                }

                {
                    var op = new MonadAbstraction(type1, new FirstOperator(type1, monadMember1));
                    var nul = new MonadAbstraction(type1, inner: null);
                    Assert.IsFalse(comparer.Equals(op, nul));
                    Assert.IsFalse(comparer.Equals(nul, op));
                    Assert.IsTrue(comparer.Equals(nul, nul));
                }

                {
                    var op = new FirstPredicateOperator(type1, monadMember1, lambdaAbstraction1);
                    var nul = default(FirstPredicateOperator);
                    Assert.IsFalse(comparer.Equals(op, nul));
                    Assert.IsFalse(comparer.Equals(nul, op));
                    Assert.IsTrue(comparer.Equals(nul, nul));
                    Assert.AreEqual(17, comparer.GetHashCode(nul));
                }

                {
                    var op = new MonadAbstraction(type1, new FirstPredicateOperator(type1, monadMember1, lambdaAbstraction1));
                    var nul = new MonadAbstraction(type1, inner: null);
                    Assert.IsFalse(comparer.Equals(op, nul));
                    Assert.IsFalse(comparer.Equals(nul, op));
                    Assert.IsTrue(comparer.Equals(nul, nul));
                }

                {
                    var op = new SelectOperator(type1, type2, monadMember1, lambdaAbstraction1);
                    var nul = default(SelectOperator);
                    Assert.IsFalse(comparer.Equals(op, nul));
                    Assert.IsFalse(comparer.Equals(nul, op));
                    Assert.IsTrue(comparer.Equals(nul, nul));
                    Assert.AreEqual(17, comparer.GetHashCode(nul));
                }

                {
                    var op = new MonadAbstraction(type1, new SelectOperator(type1, type2, monadMember1, lambdaAbstraction1));
                    var nul = new MonadAbstraction(type1, inner: null);
                    Assert.IsFalse(comparer.Equals(op, nul));
                    Assert.IsFalse(comparer.Equals(nul, op));
                    Assert.IsTrue(comparer.Equals(nul, nul));
                }

                {
                    var op = new TakeOperator(type1, monadMember1, lambdaAbstraction1);
                    var nul = default(TakeOperator);
                    Assert.IsFalse(comparer.Equals(op, nul));
                    Assert.IsFalse(comparer.Equals(nul, op));
                    Assert.IsTrue(comparer.Equals(nul, nul));
                    Assert.AreEqual(17, comparer.GetHashCode(nul));
                }

                {
                    var op = new MonadAbstraction(type1, new TakeOperator(type1, monadMember1, lambdaAbstraction1));
                    var nul = new MonadAbstraction(type1, inner: null);
                    Assert.IsFalse(comparer.Equals(op, nul));
                    Assert.IsFalse(comparer.Equals(nul, op));
                    Assert.IsTrue(comparer.Equals(nul, nul));
                }

                {
                    var op = new WhereOperator(type1, monadMember1, lambdaAbstraction1);
                    var nul = default(WhereOperator);
                    Assert.IsFalse(comparer.Equals(op, nul));
                    Assert.IsFalse(comparer.Equals(nul, op));
                    Assert.IsTrue(comparer.Equals(nul, nul));
                    Assert.AreEqual(17, comparer.GetHashCode(nul));
                }

                {
                    var op = new MonadAbstraction(type1, new WhereOperator(type1, monadMember1, lambdaAbstraction1));
                    var nul = new MonadAbstraction(type1, inner: null);
                    Assert.IsFalse(comparer.Equals(op, nul));
                    Assert.IsFalse(comparer.Equals(nul, op));
                    Assert.IsTrue(comparer.Equals(nul, nul));
                }

                {
                    var op = new MonadAbstraction(type1, lambdaAbstraction1);
                    var nul = default(MonadAbstraction);
                    Assert.IsFalse(comparer.Equals(op, nul));
                    Assert.IsFalse(comparer.Equals(nul, op));
                    Assert.IsTrue(comparer.Equals(nul, nul));
                    Assert.AreEqual(17, comparer.GetHashCode(nul));
                }

                {
                    var op = new MonadAbstraction(type2, new MonadAbstraction(type1, lambdaAbstraction1));
                    var nul = new MonadAbstraction(type2, inner: null);
                    Assert.IsFalse(comparer.Equals(op, nul));
                    Assert.IsFalse(comparer.Equals(nul, op));
                    Assert.IsTrue(comparer.Equals(nul, nul));
                }

                {
                    var op = new LambdaAbstraction(Expression.Lambda(Expression.Constant(41), Expression.Parameter(type1)), new QueryTree[] { lambdaAbstraction1 }.ToReadOnly());
                    var nul = default(LambdaAbstraction);
                    Assert.IsFalse(comparer.Equals(op, nul));
                    Assert.IsFalse(comparer.Equals(nul, op));
                    Assert.IsTrue(comparer.Equals(nul, nul));
                    Assert.AreEqual(17, comparer.GetHashCode(nul));
                }

                {
                    var op = new MonadAbstraction(type2, new LambdaAbstraction(Expression.Lambda(Expression.Constant(41), Expression.Parameter(type1)), new QueryTree[] { lambdaAbstraction1 }.ToReadOnly()));
                    var nul = new MonadAbstraction(type2, inner: null);
                    Assert.IsFalse(comparer.Equals(op, nul));
                    Assert.IsFalse(comparer.Equals(nul, op));
                    Assert.IsTrue(comparer.Equals(nul, nul));
                }
            }
        }

        [TestMethod]
        public void ExpressionEqualityComparer_DerivedClass2()
        {
            var comparer = new QueryTreeDerivedClassEqualityComparer2();
            var type1 = typeof(int);
            var monadMember1 = (MonadMember)new MonadAbstraction(type1, inner: null);

            var op = new FirstOperator(type1, monadMember1);
            var nul = default(FirstOperator);
            Assert.IsFalse(comparer.Equals(op, nul));
            Assert.IsFalse(comparer.Equals(nul, op));
            Assert.IsTrue(comparer.Equals(nul, nul));
            Assert.AreEqual(17, comparer.GetHashCode(nul));
        }

        [TestMethod]
        public void ExpressionEqualityComparer_GlobalParameter()
        {
            var comparer = new QueryExpressionEqualityComparer();
            var p1 = Expression.Parameter(typeof(bool));
            var p2 = Expression.Parameter(typeof(bool));

            {
                var lambdaAbstraction1 = new LambdaAbstraction(Expression.Lambda(p1), EmptyReadOnlyCollection<QueryTree>.Instance);
                var lambdaAbstraction2 = new LambdaAbstraction(Expression.Lambda(p2), EmptyReadOnlyCollection<QueryTree>.Instance);

                Assert.IsFalse(comparer.Equals(lambdaAbstraction1, lambdaAbstraction2));
            }

            {
                var lambdaAbstraction1 = new LambdaAbstraction(Expression.Lambda(p1), EmptyReadOnlyCollection<QueryTree>.Instance);
                var lambdaAbstraction2 = new LambdaAbstraction(Expression.Lambda(p1), EmptyReadOnlyCollection<QueryTree>.Instance);

                Assert.IsTrue(comparer.Equals(lambdaAbstraction1, lambdaAbstraction2));
                Assert.AreEqual(comparer.GetHashCode(lambdaAbstraction1), comparer.GetHashCode(lambdaAbstraction2));
            }
        }

        [TestMethod]
        public void QueryExpressionEqualityComparer_ScopeTracking()
        {
            var comparer = new QueryExpressionEqualityComparator();
            var p1 = Expression.Parameter(typeof(int));
            var p2 = Expression.Parameter(typeof(int));
            var p3 = Expression.Parameter(typeof(int));

            {
                var fst =
                    new WhereOperator(
                        typeof(int),
                        null,
                        new LambdaAbstraction(
                            Expression.Lambda(
                                Expression.Add(p1, p2),
                                p1,
                                p2,
                                p3
                            ),
                            new QueryTree[] { null, null, null }.ToReadOnly()
                        )
                    );

                var snd =
                    new WhereOperator(
                        typeof(int),
                        null,
                        new LambdaAbstraction(
                            Expression.Lambda(
                                Expression.Add(p1, p2),
                                p2,
                                p1,
                                p3
                            ),
                            new QueryTree[] { null, null, null }.ToReadOnly()
                        )
                    );

                Assert.IsFalse(comparer.Equals(fst, snd));
            }

            // Test that alpha renaming doesn't affect equality
            {
                var fst =
                    new WhereOperator(
                        typeof(int),
                        null,
                        new LambdaAbstraction(
                            Expression.Lambda(
                                Expression.Add(p1, p2),
                                p1,
                                p2,
                                p3
                            ),
                            new QueryTree[] { null, null, null }.ToReadOnly()
                        )
                    );

                var snd =
                    new WhereOperator(
                        typeof(int),
                        null,
                        new LambdaAbstraction(
                            Expression.Lambda(
                                Expression.Add(p2, p1),
                                p2,
                                p1,
                                p3
                            ),
                            new QueryTree[] { null, null, null }.ToReadOnly()
                        )
                    );

                Assert.IsTrue(comparer.Equals(fst, snd));
                Assert.AreEqual(comparer.GetHashCode(fst), comparer.GetHashCode(snd));
            }

            // Scope tracking for reference equal parameter expressions when they are "redefined" in inner lambda
            {
                var fst =
                    new WhereOperator(
                        typeof(int),
                        null,
                        new LambdaAbstraction(
                            Expression.Lambda(
                                p1,
                                p1
                            ),
                            new QueryTree[]
                            {
                                new LambdaAbstraction(
                                    Expression.Lambda(
                                        p1,
                                        p1
                                    ),
                                    new QueryTree[] { null }.ToReadOnly()
                                )
                            }.ToReadOnly()
                        )
                    );

                var snd =
                    new WhereOperator(
                        typeof(int),
                        null,
                        new LambdaAbstraction(
                            Expression.Lambda(
                                p1,
                                p1
                            ),
                            new QueryTree[]
                            {
                                new LambdaAbstraction(
                                    Expression.Lambda(
                                        p1,
                                        p2
                                    ),
                                    new QueryTree[] { null }.ToReadOnly()
                                )
                            }.ToReadOnly()
                        )
                    );

                Assert.ThrowsException<InvalidOperationException>(() => comparer.Equals(fst, snd));
                Assert.ThrowsException<InvalidOperationException>(() => comparer.GetHashCode(snd));
            }

            {
                var fst =
                    new WhereOperator(
                        typeof(int),
                        null,
                        new LambdaAbstraction(
                            Expression.Lambda(
                                p1,
                                p1
                            ),
                            new QueryTree[]
                            {
                                new LambdaAbstraction(
                                    Expression.Lambda(
                                        p1,
                                        p1
                                    ),
                                    new QueryTree[] { null }.ToReadOnly()
                                )
                            }.ToReadOnly()
                        )
                    );

                var snd =
                    new WhereOperator(
                        typeof(int),
                        null,
                        new LambdaAbstraction(
                            Expression.Lambda(
                                p1,
                                p1
                            ),
                            new QueryTree[]
                            {
                                new LambdaAbstraction(
                                    Expression.Lambda(
                                        p1,
                                        p1
                                    ),
                                    new QueryTree[] { null }.ToReadOnly()
                                )
                            }.ToReadOnly()
                        )
                    );

                Assert.IsTrue(comparer.Equals(fst, snd));
                Assert.AreEqual(comparer.GetHashCode(fst), comparer.GetHashCode(snd));
            }

            {
                var fst =
                    new WhereOperator(
                        typeof(int),
                        null,
                        new LambdaAbstraction(
                            Expression.Lambda(
                                p1,
                                p1
                            ),
                            new QueryTree[]
                            {
                                new LambdaAbstraction(
                                    Expression.Lambda(
                                        p1,
                                        p2
                                    ),
                                    new QueryTree[] { null }.ToReadOnly()
                                )
                            }.ToReadOnly()
                        )
                    );

                var snd =
                    new WhereOperator(
                        typeof(int),
                        null,
                        new LambdaAbstraction(
                            Expression.Lambda(
                                p1,
                                p1
                            ),
                            new QueryTree[]
                            {
                                new LambdaAbstraction(
                                    Expression.Lambda(
                                        p1,
                                        p2
                                    ),
                                    new QueryTree[] { null }.ToReadOnly()
                                )
                            }.ToReadOnly()
                        )
                    );

                Assert.ThrowsException<InvalidOperationException>(() => comparer.Equals(fst, snd));
                Assert.ThrowsException<InvalidOperationException>(() => comparer.GetHashCode(snd));
            }
        }

        private class QueryTreeDerivedClassEqualityComparer :
            QueryExpressionEqualityComparator,
            IEqualityComparer<FirstOperator>,
            IEqualityComparer<FirstPredicateOperator>,
            IEqualityComparer<SelectOperator>,
            IEqualityComparer<TakeOperator>,
            IEqualityComparer<WhereOperator>,
            IEqualityComparer<MonadAbstraction>,
            IEqualityComparer<LambdaAbstraction>
        {
            public bool Equals(FirstOperator x, FirstOperator y)
            {
                return EqualsFirst(x, y);
            }

            public int GetHashCode(FirstOperator obj)
            {
                return GetHashCodeFirst(obj);
            }

            public bool Equals(FirstPredicateOperator x, FirstPredicateOperator y)
            {
                return EqualsFirstPredicate(x, y);
            }

            public int GetHashCode(FirstPredicateOperator obj)
            {
                return GetHashCodeFirstPredicate(obj);
            }

            public bool Equals(SelectOperator x, SelectOperator y)
            {
                return EqualsSelect(x, y);
            }

            public int GetHashCode(SelectOperator obj)
            {
                return GetHashCodeSelect(obj);
            }

            public bool Equals(TakeOperator x, TakeOperator y)
            {
                return EqualsTake(x, y);
            }

            public int GetHashCode(TakeOperator obj)
            {
                return GetHashCodeTake(obj);
            }

            public bool Equals(WhereOperator x, WhereOperator y)
            {
                return EqualsWhere(x, y);
            }

            public int GetHashCode(WhereOperator obj)
            {
                return GetHashCodeWhere(obj);
            }

            public bool Equals(MonadAbstraction x, MonadAbstraction y)
            {
                return EqualsMonadAbstraction(x, y);
            }

            public int GetHashCode(MonadAbstraction obj)
            {
                return GetHashCodeMonadAbstraction(obj);
            }

            public bool Equals(LambdaAbstraction x, LambdaAbstraction y)
            {
                return EqualsLambdaAbstraction(x, y);
            }

            public int GetHashCode(LambdaAbstraction obj)
            {
                return GetHashCodeLambdaAbstraction(obj);
            }
        }

        private class QueryTreeDerivedClassEqualityComparer2 :
            QueryExpressionEqualityComparator,
            IEqualityComparer<QueryOperator>
        {
            public bool Equals(QueryOperator x, QueryOperator y)
            {
                return EqualsQueryOperator(x, y);
            }

            public int GetHashCode(QueryOperator obj)
            {
                return GetHashCodeQueryOperator(obj);
            }
        }
    }
}
