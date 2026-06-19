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
    public class QueryVisitorTests
    {
        [TestMethod]
        public void QueryVisitor_Simple()
        {
            var type = typeof(ArgumentException);
            var type2 = typeof(Exception);
            var lambdaAbstraction1 = DefaultQueryExpressionFactory.Instance.LambdaAbstraction(Expression.Lambda(Expression.Empty()));
            var lambdaAbstraction2 = DefaultQueryExpressionFactory.Instance.LambdaAbstraction(Expression.Lambda(Expression.Empty()));
            var monadMember1 = DefaultQueryExpressionFactory.Instance.MonadAbstraction(type, lambdaAbstraction1);
            var monadMember2 = DefaultQueryExpressionFactory.Instance.MonadAbstraction(type, lambdaAbstraction2);

            var cmp = new QueryExpressionEqualityComparer();

            {
                var op = new FirstOperator(type, monadMember1);
                var rewritten = new ReferenceEqualSubstitutionVisitor(monadMember1, monadMember2).Visit(op);
                Assert.AreNotSame(op, rewritten);
                Assert.IsTrue(cmp.Equals(op, rewritten));
            }

            {
                var op = new FirstPredicateOperator(type, monadMember1, lambdaAbstraction1);
                var rewritten1 = new ReferenceEqualSubstitutionVisitor(monadMember1, monadMember2).Visit(op);
                Assert.AreNotSame(op, rewritten1);
                Assert.IsTrue(cmp.Equals(op, rewritten1));

                var rewritten2 = new ReferenceEqualSubstitutionVisitor(lambdaAbstraction1, lambdaAbstraction2).Visit(op);
                Assert.AreNotSame(op, rewritten2);
                Assert.IsTrue(cmp.Equals(op, rewritten2));
            }

            {
                var op = new SelectOperator(type, type2, monadMember1, lambdaAbstraction1);
                var rewritten1 = new ReferenceEqualSubstitutionVisitor(monadMember1, monadMember2).Visit(op);
                Assert.AreNotSame(op, rewritten1);
                Assert.IsTrue(cmp.Equals(op, rewritten1));

                var rewritten2 = new ReferenceEqualSubstitutionVisitor(lambdaAbstraction1, lambdaAbstraction2).Visit(op);
                Assert.AreNotSame(op, rewritten2);
                Assert.IsTrue(cmp.Equals(op, rewritten2));
            }

            {
                var op = new TakeOperator(type, monadMember1, lambdaAbstraction1);
                var rewritten1 = new ReferenceEqualSubstitutionVisitor(monadMember1, monadMember2).Visit(op);
                Assert.AreNotSame(op, rewritten1);
                Assert.IsTrue(cmp.Equals(op, rewritten1));

                var rewritten2 = new ReferenceEqualSubstitutionVisitor(lambdaAbstraction1, lambdaAbstraction2).Visit(op);
                Assert.AreNotSame(op, rewritten2);
                Assert.IsTrue(cmp.Equals(op, rewritten2));
            }

            {
                var op = new WhereOperator(type, monadMember1, lambdaAbstraction1);
                var rewritten1 = new ReferenceEqualSubstitutionVisitor(monadMember1, monadMember2).Visit(op);
                Assert.AreNotSame(op, rewritten1);
                Assert.IsTrue(cmp.Equals(op, rewritten1));

                var rewritten2 = new ReferenceEqualSubstitutionVisitor(lambdaAbstraction1, lambdaAbstraction2).Visit(op);
                Assert.AreNotSame(op, rewritten2);
                Assert.IsTrue(cmp.Equals(op, rewritten2));
            }

            {
                var op = new MonadAbstraction(type, lambdaAbstraction1);
                var rewritten1 = new ReferenceEqualSubstitutionVisitor(lambdaAbstraction1, lambdaAbstraction2).Visit(op);
                Assert.AreNotSame(op, rewritten1);
                Assert.IsTrue(cmp.Equals(op, rewritten1));
            }

            {
                var lambda = Expression.Lambda(Expression.Empty(), Expression.Parameter(type));
                var parameters = new QueryTree[] { monadMember1 }.ToReadOnly();
                var op = new LambdaAbstraction(lambda, parameters);
                var rewritten1 = new ReferenceEqualSubstitutionVisitor(monadMember1, monadMember2).Visit(op);
                Assert.AreNotSame(op, rewritten1);
                Assert.IsTrue(cmp.Equals(op, rewritten1));
            }
        }

        [TestMethod]
        public void QueryVisitor_Reflection_Identity()
        {
            var type = typeof(int);
            var type2 = typeof(bool);
            var type3 = typeof(object);
            var lambdaAbstraction1 = DefaultQueryExpressionFactory.Instance.LambdaAbstraction(Expression.Lambda(Expression.Empty()));
            var monadMember1 = DefaultQueryExpressionFactory.Instance.MonadAbstraction(type, lambdaAbstraction1);

            var cmp = new QueryExpressionEqualityComparer();

            {
                var op = new FirstOperator(type, monadMember1);
                var rewritten1 = new ReferenceEqualTypeSubstitutionVisitor(type, type).Visit(op);
                Assert.AreSame(op, rewritten1);

                var rewritten2 = new ReferenceEqualTypeSubstitutionVisitor(type, type2).Visit(op);
                Assert.IsFalse(cmp.Equals(op, rewritten2));

                var rewritten3 = new ReferenceEqualTypeSubstitutionVisitor(type2, type).Visit(op);
                Assert.IsTrue(cmp.Equals(op, rewritten3));
            }

            {
                var op = new FirstPredicateOperator(type, monadMember1, lambdaAbstraction1);
                var rewritten1 = new ReferenceEqualTypeSubstitutionVisitor(type, type).Visit(op);
                Assert.AreSame(op, rewritten1);

                var rewritten2 = new ReferenceEqualTypeSubstitutionVisitor(type, type2).Visit(op);
                Assert.IsFalse(cmp.Equals(op, rewritten2));

                var rewritten3 = new ReferenceEqualTypeSubstitutionVisitor(type2, type).Visit(op);
                Assert.IsTrue(cmp.Equals(op, rewritten3));
            }

            {
                var op = new SelectOperator(type, type3, monadMember1, lambdaAbstraction1);
                var rewritten1 = new ReferenceEqualTypeSubstitutionVisitor(type, type).Visit(op);
                Assert.AreSame(op, rewritten1);

                var rewritten2 = new ReferenceEqualTypeSubstitutionVisitor(type, type2).Visit(op);
                Assert.IsFalse(cmp.Equals(op, rewritten2));

                var rewritten3 = new ReferenceEqualTypeSubstitutionVisitor(type2, type).Visit(op);
                Assert.IsTrue(cmp.Equals(op, rewritten3));
            }

            {
                var op = new TakeOperator(type, monadMember1, lambdaAbstraction1);
                var rewritten1 = new ReferenceEqualTypeSubstitutionVisitor(type, type).Visit(op);
                Assert.AreSame(op, rewritten1);

                var rewritten2 = new ReferenceEqualTypeSubstitutionVisitor(type, type2).Visit(op);
                Assert.IsFalse(cmp.Equals(op, rewritten2));

                var rewritten3 = new ReferenceEqualTypeSubstitutionVisitor(type2, type).Visit(op);
                Assert.IsTrue(cmp.Equals(op, rewritten3));
            }

            {
                var op = new WhereOperator(type, monadMember1, lambdaAbstraction1);
                var rewritten1 = new ReferenceEqualTypeSubstitutionVisitor(type, type).Visit(op);
                Assert.AreSame(op, rewritten1);

                var rewritten2 = new ReferenceEqualTypeSubstitutionVisitor(type, type2).Visit(op);
                Assert.IsFalse(cmp.Equals(op, rewritten2));

                var rewritten3 = new ReferenceEqualTypeSubstitutionVisitor(type2, type).Visit(op);
                Assert.IsTrue(cmp.Equals(op, rewritten3));
            }

            {
                var op = new MonadAbstraction(type, lambdaAbstraction1);
                var rewritten1 = new ReferenceEqualTypeSubstitutionVisitor(type, type).Visit(op);
                Assert.AreSame(op, rewritten1);

                var rewritten2 = new ReferenceEqualTypeSubstitutionVisitor(type, type2).Visit(op);
                Assert.IsFalse(cmp.Equals(op, rewritten2));

                var rewritten3 = new ReferenceEqualTypeSubstitutionVisitor(type2, type).Visit(op);
                Assert.IsTrue(cmp.Equals(op, rewritten3));
            }
        }

        [TestMethod]
        public void QueryVisitor_UpdateNodes()
        {
            var type = typeof(int);
            var type2 = typeof(bool);
            var lambdaAbstraction1 = DefaultQueryExpressionFactory.Instance.LambdaAbstraction(Expression.Lambda(Expression.Empty()));
            var lambdaAbstraction2 = DefaultQueryExpressionFactory.Instance.LambdaAbstraction(Expression.Lambda(Expression.Empty()));
            var monadMember1 = DefaultQueryExpressionFactory.Instance.MonadAbstraction(type, lambdaAbstraction1);
            var monadMember2 = DefaultQueryExpressionFactory.Instance.MonadAbstraction(type, lambdaAbstraction2);

            var cmp = new QueryExpressionEqualityComparer();

            {
                var op = new FirstOperator(type, monadMember1);
                var rewritten = new ReferenceEqualSubstitutionVisitorWithReflection(monadMember1, monadMember2).Visit(op);
                Assert.AreNotSame(op, rewritten);
                Assert.IsTrue(cmp.Equals(op, rewritten));
            }

            {
                var op = new FirstPredicateOperator(type, monadMember1, lambdaAbstraction1);
                var rewritten1 = new ReferenceEqualSubstitutionVisitorWithReflection(monadMember1, monadMember2).Visit(op);
                Assert.AreNotSame(op, rewritten1);
                Assert.IsTrue(cmp.Equals(op, rewritten1));

                var rewritten2 = new ReferenceEqualSubstitutionVisitorWithReflection(lambdaAbstraction1, lambdaAbstraction2).Visit(op);
                Assert.AreNotSame(op, rewritten2);
                Assert.IsTrue(cmp.Equals(op, rewritten2));
            }

            {
                var op = new SelectOperator(type2, type, monadMember1, lambdaAbstraction1);
                var rewritten1 = new ReferenceEqualSubstitutionVisitorWithReflection(monadMember1, monadMember2).Visit(op);
                Assert.AreNotSame(op, rewritten1);
                Assert.IsTrue(cmp.Equals(op, rewritten1));

                var rewritten2 = new ReferenceEqualSubstitutionVisitorWithReflection(lambdaAbstraction1, lambdaAbstraction2).Visit(op);
                Assert.AreNotSame(op, rewritten2);
                Assert.IsTrue(cmp.Equals(op, rewritten2));
            }

            {
                var op = new TakeOperator(type, monadMember1, lambdaAbstraction1);
                var rewritten1 = new ReferenceEqualSubstitutionVisitorWithReflection(monadMember1, monadMember2).Visit(op);
                Assert.AreNotSame(op, rewritten1);
                Assert.IsTrue(cmp.Equals(op, rewritten1));

                var rewritten2 = new ReferenceEqualSubstitutionVisitorWithReflection(lambdaAbstraction1, lambdaAbstraction2).Visit(op);
                Assert.AreNotSame(op, rewritten2);
                Assert.IsTrue(cmp.Equals(op, rewritten2));
            }

            {
                var op = new WhereOperator(type, monadMember1, lambdaAbstraction1);
                var rewritten1 = new ReferenceEqualSubstitutionVisitorWithReflection(monadMember1, monadMember2).Visit(op);
                Assert.AreNotSame(op, rewritten1);
                Assert.IsTrue(cmp.Equals(op, rewritten1));

                var rewritten2 = new ReferenceEqualSubstitutionVisitorWithReflection(lambdaAbstraction1, lambdaAbstraction2).Visit(op);
                Assert.AreNotSame(op, rewritten2);
                Assert.IsTrue(cmp.Equals(op, rewritten2));
            }

            {
                var op = new MonadAbstraction(type, lambdaAbstraction1);
                var rewritten1 = new ReferenceEqualSubstitutionVisitorWithReflection(lambdaAbstraction1, lambdaAbstraction2).Visit(op);
                Assert.AreNotSame(op, rewritten1);
                Assert.IsTrue(cmp.Equals(op, rewritten1));
            }

            {
                var lambda = Expression.Lambda(Expression.Empty(), Expression.Parameter(type));
                var parameters = new QueryTree[] { monadMember1 }.ToReadOnly();
                var op = new LambdaAbstraction(lambda, parameters);
                var rewritten1 = new ReferenceEqualSubstitutionVisitorWithReflection(monadMember1, monadMember2).Visit(op);
                Assert.AreNotSame(op, rewritten1);
                Assert.IsTrue(cmp.Equals(op, rewritten1));
            }
        }

        private class ReferenceEqualSubstitutionVisitor : QueryVisitor
        {
            private readonly QueryTree _tree;
            private readonly QueryTree _replacement;

            public ReferenceEqualSubstitutionVisitor(QueryTree tree, QueryTree replacement)
            {
                _tree = tree;
                _replacement = replacement;
            }

            public override QueryTree Visit(QueryTree expression)
            {
                if (object.ReferenceEquals(_tree, expression))
                    return _replacement;

                return base.Visit(expression);
            }
        }

        private class ReferenceEqualSubstitutionVisitorWithReflection : QueryVisitorWithReflection
        {
            private readonly QueryTree _tree;
            private readonly QueryTree _replacement;

            public ReferenceEqualSubstitutionVisitorWithReflection(QueryTree tree, QueryTree replacement)
            {
                _tree = tree;
                _replacement = replacement;
            }

            public override QueryTree Visit(QueryTree expression)
            {
                if (object.ReferenceEquals(_tree, expression))
                    return _replacement;

                return base.Visit(expression);
            }
        }

        private class ReferenceEqualTypeSubstitutionVisitor : QueryVisitorWithReflection
        {
            private readonly Type _origType;
            private readonly Type _replacementType;

            public ReferenceEqualTypeSubstitutionVisitor(Type tree, Type replacement)
            {
                _origType = tree;
                _replacementType = replacement;
            }

            protected override Type VisitType(Type type)
            {
                if (type == _origType)
                    return _replacementType;

                return type;
            }
        }
    }
}
