// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// PS - February 2015 - Created this file.
//

using System;
using System.Linq.CompilerServices;
using System.Linq.CompilerServices.Optimizers;
using System.Linq.Expressions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Linq.CompilerServices.Optimizers
{
    [TestClass]
    public class QueryTreeTests
    {
        [TestMethod]
        public void QueryTree_Simple()
        {
            var customType = typeof(int);
            var customType2 = typeof(int);
            var monadMember = (MonadMember)new MonadAbstraction(customType, inner: null);
            var lambdaAbstraction = new LambdaAbstraction(body: null, parameters: null);

            {
                var op = new FirstOperator(customType, monadMember);
                Assert.AreEqual(QueryNodeType.Operator, op.QueryNodeType);
                Assert.AreEqual(OperatorType.First, op.NodeType);
                Assert.AreSame(customType, op.ElementType);
                Assert.AreSame(monadMember, op.Source);
            }

            {
                var op = new FirstPredicateOperator(customType, monadMember, lambdaAbstraction);
                Assert.AreEqual(QueryNodeType.Operator, op.QueryNodeType);
                Assert.AreEqual(OperatorType.FirstPredicate, op.NodeType);
                Assert.AreSame(customType, op.ElementType);
                Assert.AreSame(monadMember, op.Source);
                Assert.AreSame(lambdaAbstraction, op.Predicate);
            }

            {
                var op = new SelectOperator(customType, customType2, monadMember, lambdaAbstraction);
                Assert.AreEqual(QueryNodeType.Operator, op.QueryNodeType);
                Assert.AreEqual(OperatorType.Select, op.NodeType);
                Assert.AreSame(customType, op.ElementType);
                Assert.AreSame(customType2, op.InputElementType);
                Assert.AreSame(monadMember, op.Source);
                Assert.AreSame(lambdaAbstraction, op.Selector);
            }

            {
                var op = new TakeOperator(customType, monadMember, lambdaAbstraction);
                Assert.AreEqual(QueryNodeType.Operator, op.QueryNodeType);
                Assert.AreEqual(OperatorType.Take, op.NodeType);
                Assert.AreSame(customType, op.ElementType);
                Assert.AreSame(monadMember, op.Source);
                Assert.AreSame(lambdaAbstraction, op.Count);
            }

            {
                var op = new WhereOperator(customType, monadMember, lambdaAbstraction);
                Assert.AreEqual(QueryNodeType.Operator, op.QueryNodeType);
                Assert.AreEqual(OperatorType.Where, op.NodeType);
                Assert.AreSame(customType, op.ElementType);
                Assert.AreSame(monadMember, op.Source);
                Assert.AreSame(lambdaAbstraction, op.Predicate);
            }

            {
                var op = new MonadAbstraction(customType, lambdaAbstraction);
                Assert.AreEqual(QueryNodeType.MonadAbstraction, op.QueryNodeType);
                Assert.AreSame(customType, op.ElementType);
                Assert.AreSame(lambdaAbstraction, op.Inner);
            }

            {
                var lambda = Expression.Lambda<Action>(Expression.Empty());
                var parameters = new QueryTree[] { monadMember }.ToReadOnly();
                var op = new LambdaAbstraction(lambda, parameters);
                Assert.AreEqual(QueryNodeType.Lambda, op.QueryNodeType);
                Assert.AreSame(lambda, op.Body);
                Assert.AreSame(parameters, op.Parameters);
            }
        }

        [TestMethod]
        public void QueryTree_Update()
        {
            var customType = typeof(int);
            var customType2 = typeof(int);
            var monadMember = (MonadMember)new MonadAbstraction(customType, inner: null);
            var lambdaAbstraction = new LambdaAbstraction(body: null, parameters: null);

            var updatedCustomType = typeof(int);
            var updatedMonadMember = (MonadMember)new MonadAbstraction(updatedCustomType, inner: null);
            var updatedLambdaAbstraction = new LambdaAbstraction(body: null, parameters: null);

            {
                var op = new FirstOperator(customType, monadMember);
                var unchanged = op.Update(monadMember);

                Assert.AreSame(op, unchanged);

                var op2 = op.Update(updatedMonadMember);

                Assert.AreNotSame(op, op2);
                Assert.AreSame(updatedMonadMember, op2.Source);
            }

            {
                var op = new FirstPredicateOperator(customType, monadMember, lambdaAbstraction);
                var unchanged = op.Update(monadMember, lambdaAbstraction);

                Assert.AreSame(op, unchanged);

                var op2 = op.Update(updatedMonadMember, lambdaAbstraction);

                Assert.AreNotSame(op, op2);
                Assert.AreSame(updatedMonadMember, op2.Source);
                Assert.AreSame(lambdaAbstraction, op2.Predicate);

                var op3 = op.Update(monadMember, updatedLambdaAbstraction);

                Assert.AreNotSame(op, op3);
                Assert.AreSame(monadMember, op3.Source);
                Assert.AreSame(updatedLambdaAbstraction, op3.Predicate);
            }

            {
                var op = new SelectOperator(customType, customType2, monadMember, lambdaAbstraction);
                var unchanged = op.Update(monadMember, lambdaAbstraction);

                Assert.AreSame(op, unchanged);

                var op2 = op.Update(updatedMonadMember, lambdaAbstraction);

                Assert.AreNotSame(op, op2);
                Assert.AreSame(updatedMonadMember, op2.Source);
                Assert.AreSame(lambdaAbstraction, op2.Selector);

                var op3 = op.Update(monadMember, updatedLambdaAbstraction);

                Assert.AreNotSame(op, op3);
                Assert.AreSame(monadMember, op3.Source);
                Assert.AreSame(updatedLambdaAbstraction, op3.Selector);
            }

            {
                var op = new TakeOperator(customType, monadMember, lambdaAbstraction);
                var unchanged = op.Update(monadMember, lambdaAbstraction);

                Assert.AreSame(op, unchanged);

                var op2 = op.Update(updatedMonadMember, lambdaAbstraction);

                Assert.AreNotSame(op, op2);
                Assert.AreSame(updatedMonadMember, op2.Source);
                Assert.AreSame(lambdaAbstraction, op2.Count);

                var op3 = op.Update(monadMember, updatedLambdaAbstraction);

                Assert.AreNotSame(op, op3);
                Assert.AreSame(monadMember, op3.Source);
                Assert.AreSame(updatedLambdaAbstraction, op3.Count);
            }

            {
                var op = new WhereOperator(customType, monadMember, lambdaAbstraction);
                var unchanged = op.Update(monadMember, lambdaAbstraction);

                Assert.AreSame(op, unchanged);

                var op2 = op.Update(updatedMonadMember, lambdaAbstraction);

                Assert.AreNotSame(op, op2);
                Assert.AreSame(updatedMonadMember, op2.Source);
                Assert.AreSame(lambdaAbstraction, op2.Predicate);

                var op3 = op.Update(monadMember, updatedLambdaAbstraction);

                Assert.AreNotSame(op, op3);
                Assert.AreSame(monadMember, op3.Source);
                Assert.AreSame(updatedLambdaAbstraction, op3.Predicate);
            }

            {
                var ma = new MonadAbstraction(customType, lambdaAbstraction);
                var unchanged = ma.Update(lambdaAbstraction);

                Assert.AreSame(ma, unchanged);

                var ma2 = ma.Update(updatedLambdaAbstraction);

                Assert.AreNotSame(ma, ma2);
                Assert.AreSame(updatedLambdaAbstraction, ma2.Inner);
            }

            {
                var lambda = Expression.Lambda(Expression.Empty(), Expression.Parameter(typeof(int)));
                var parameters = new QueryTree[] { monadMember }.ToReadOnly();
                var updatedLambda = Expression.Lambda(Expression.Empty(), Expression.Parameter(typeof(int)));
                var updatedParameters = new QueryTree[] { monadMember }.ToReadOnly();

                var la = new LambdaAbstraction(lambda, parameters);
                var unchanged = la.Update(lambda, parameters);

                Assert.AreSame(la, unchanged);

                var la2 = la.Update(updatedLambda, parameters);

                Assert.AreNotSame(la, la2);
                Assert.AreSame(updatedLambda, la2.Body);
                CollectionAssert.AreEqual(parameters, la2.Parameters);  // A ToReadOnly is called on the IE<QT>

                var la3 = la.Update(lambda, updatedParameters);

                Assert.AreNotSame(la, la3);
                Assert.AreSame(lambda, la3.Body);
                CollectionAssert.AreEqual(updatedParameters, la3.Parameters);
            }
        }

        [TestMethod]
        public void QueryTree_Visit()
        {
            var type = typeof(int);

            var query =
                new FirstOperator(
                    type,
                    new FirstPredicateOperator(
                        type,
                        new SelectOperator(
                            type,
                            type,
                            new WhereOperator(
                                type,
                                new TakeOperator(
                                    type,
                                    new MonadAbstraction(
                                        type,
                                        new LambdaAbstraction(
                                            Expression.Lambda(Expression.Empty()),
                                            EmptyReadOnlyCollection<QueryTree>.Instance
                                        )
                                    ),
                                    new LambdaAbstraction(
                                        Expression.Lambda(Expression.Empty()),
                                        EmptyReadOnlyCollection<QueryTree>.Instance
                                    )
                                ),
                                new LambdaAbstraction(
                                    Expression.Lambda(Expression.Empty()),
                                    EmptyReadOnlyCollection<QueryTree>.Instance
                                )
                            ),
                            new LambdaAbstraction(
                                Expression.Lambda(Expression.Empty()),
                                EmptyReadOnlyCollection<QueryTree>.Instance
                            )
                        ),
                        new LambdaAbstraction(
                            Expression.Lambda(Expression.Empty()),
                            EmptyReadOnlyCollection<QueryTree>.Instance
                        )
                    )
                );

            var visited = new QueryVisitor().Visit(query);

            var visited2 = new QueryVisitorWithReflection().Visit(query);

            Assert.AreSame(query, visited);
            Assert.AreSame(query, visited2);
        }
    }
}
