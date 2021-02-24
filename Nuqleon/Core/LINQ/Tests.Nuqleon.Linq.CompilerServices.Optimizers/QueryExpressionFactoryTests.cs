// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// PS - February 2015 - Created this file.
//

using System;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.CompilerServices.Optimizers;
using System.Linq.Expressions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Linq.CompilerServices.Optimizers
{
    [TestClass]
    public class QueryExpressionFactoryTests
    {
        [TestMethod]
        public void QueryExpressionFactory_ArgumentChecks()
        {
            var factory = DefaultQueryExpressionFactory.Instance;
            var unsafeFactory = new UnsafeQueryExpressionFactory();
            var comparer = new QueryExpressionEqualityComparator();
            var type1 = typeof(Exception);
            var type2 = typeof(ArgumentException);
            var type3 = typeof(ArgumentNullException);
            var lambdaAbstraction1 = factory.LambdaAbstraction(Expression.Lambda(Expression.Default(type1)));
            var lambdaAbstraction2 = factory.LambdaAbstraction(Expression.Lambda(Expression.Default(type2)));
            var monadMember1 = factory.MonadAbstraction(type1, lambdaAbstraction1);
            var monadMember2 = factory.MonadAbstraction(type2, lambdaAbstraction2);

            // First
            AssertEx.ThrowsException<ArgumentNullException>(() => factory.First(default(Type), default), AssertParameterName("elementType"));
            AssertEx.ThrowsException<ArgumentNullException>(() => factory.First(default(Type), monadMember1), AssertParameterName("elementType"));
            AssertEx.ThrowsException<ArgumentNullException>(() => factory.First(type1, source: null), AssertParameterName("source"));
            AssertEx.ThrowsException<ArgumentException>(() => factory.First(type2, monadMember1), AssertParameterName("elementType"));
            Assert.IsTrue(comparer.Equals(
                factory.First(type1, monadMember2),
                unsafeFactory.First(type1, monadMember2)
            ));

            // First Predicate
            AssertEx.ThrowsException<ArgumentNullException>(() => factory.First(elementType: null, source: null, predicate: null), AssertParameterName("elementType"));
            AssertEx.ThrowsException<ArgumentNullException>(() => factory.First(elementType: null, monadMember1, lambdaAbstraction1), AssertParameterName("elementType"));
            AssertEx.ThrowsException<ArgumentNullException>(() => factory.First(type1, source: null, lambdaAbstraction1), AssertParameterName("source"));
            AssertEx.ThrowsException<ArgumentNullException>(() => factory.First(type1, monadMember1, predicate: null), AssertParameterName("predicate"));
            AssertEx.ThrowsException<ArgumentException>(() => factory.First(type2, monadMember1, lambdaAbstraction1), AssertParameterName("elementType"));
            Assert.IsTrue(comparer.Equals(
                factory.First(type1, monadMember2, lambdaAbstraction1),
                unsafeFactory.First(type1, monadMember2, lambdaAbstraction1)
            ));

            // Select
            AssertEx.ThrowsException<ArgumentNullException>(() => factory.Take(elementType: null, source: null, count: null), AssertParameterName("elementType"));
            AssertEx.ThrowsException<ArgumentNullException>(() => factory.Take(elementType: null, monadMember1, lambdaAbstraction1), AssertParameterName("elementType"));
            AssertEx.ThrowsException<ArgumentNullException>(() => factory.Take(type1, source: null, lambdaAbstraction1), AssertParameterName("source"));
            AssertEx.ThrowsException<ArgumentNullException>(() => factory.Take(type1, monadMember1, count: null), AssertParameterName("count"));
            AssertEx.ThrowsException<ArgumentException>(() => factory.Take(type2, monadMember1, lambdaAbstraction1), AssertParameterName("elementType"));
            Assert.IsTrue(comparer.Equals(
                factory.Take(type1, monadMember2, lambdaAbstraction1),
                unsafeFactory.Take(type1, monadMember2, lambdaAbstraction1)
            ));

            // Take
            AssertEx.ThrowsException<ArgumentNullException>(() => factory.Select(elementType: null, inputElementType: null, source: null, selector: null), AssertParameterName("elementType"));
            AssertEx.ThrowsException<ArgumentNullException>(() => factory.Select(elementType: null, type3, monadMember1, lambdaAbstraction1), AssertParameterName("elementType"));
            AssertEx.ThrowsException<ArgumentNullException>(() => factory.Select(type1, inputElementType: null, monadMember1, lambdaAbstraction1), AssertParameterName("inputElementType"));
            AssertEx.ThrowsException<ArgumentNullException>(() => factory.Select(type1, type3, source: null, lambdaAbstraction1), AssertParameterName("source"));
            AssertEx.ThrowsException<ArgumentNullException>(() => factory.Select(type1, type3, monadMember1, selector: null), AssertParameterName("selector"));
            AssertEx.ThrowsException<ArgumentException>(() => factory.Select(type2, type3, monadMember1, lambdaAbstraction1), AssertParameterName("elementType"));
            Assert.IsTrue(comparer.Equals(
                factory.Select(type3, type1, monadMember2, lambdaAbstraction1),
                unsafeFactory.Select(type3, type1, monadMember2, lambdaAbstraction1)
            ));

            // Where
            AssertEx.ThrowsException<ArgumentNullException>(() => factory.Where(elementType: null, source: null, predicate: null), AssertParameterName("elementType"));
            AssertEx.ThrowsException<ArgumentNullException>(() => factory.Where(elementType: null, monadMember1, lambdaAbstraction1), AssertParameterName("elementType"));
            AssertEx.ThrowsException<ArgumentNullException>(() => factory.Where(type1, source: null, lambdaAbstraction1), AssertParameterName("source"));
            AssertEx.ThrowsException<ArgumentNullException>(() => factory.Where(type1, monadMember1, predicate: null), AssertParameterName("predicate"));
            AssertEx.ThrowsException<ArgumentException>(() => factory.Where(type2, monadMember1, lambdaAbstraction1), AssertParameterName("elementType"));
            Assert.IsTrue(comparer.Equals(
                factory.Where(type1, monadMember2, lambdaAbstraction1),
                unsafeFactory.Where(type1, monadMember2, lambdaAbstraction1)
            ));

            // Monad Member
            AssertEx.ThrowsException<ArgumentNullException>(() => factory.MonadAbstraction(elementType: null, inner: null), AssertParameterName("elementType"));
            AssertEx.ThrowsException<ArgumentNullException>(() => factory.MonadAbstraction(elementType: null, lambdaAbstraction1), AssertParameterName("elementType"));
            AssertEx.ThrowsException<ArgumentNullException>(() => factory.MonadAbstraction(type1, inner: null), AssertParameterName("inner"));
            Assert.IsTrue(comparer.Equals(
                factory.MonadAbstraction(type1, lambdaAbstraction1),
                unsafeFactory.MonadAbstraction(type1, lambdaAbstraction1)
            ));


            // Lambda Abstraction
            AssertEx.ThrowsException<ArgumentNullException>(() => factory.LambdaAbstraction(body: null), AssertParameterName("body"));
            Assert.ThrowsException<ArgumentException>(() => factory.LambdaAbstraction(Expression.Lambda(Expression.Empty(), Expression.Parameter(type1))));
            Assert.IsTrue(comparer.Equals(
                factory.LambdaAbstraction(Expression.Lambda(Expression.Empty())),
                unsafeFactory.LambdaAbstraction(Expression.Lambda(Expression.Empty()), Enumerable.Empty<QueryTree>())
            ));

            AssertEx.ThrowsException<ArgumentNullException>(() => factory.LambdaAbstraction(body: null, parameters: null), AssertParameterName("body"));
            AssertEx.ThrowsException<ArgumentNullException>(() => factory.LambdaAbstraction(body: null, Array.Empty<QueryTree>().ToReadOnly()), AssertParameterName("body"));
            AssertEx.ThrowsException<ArgumentNullException>(() => factory.LambdaAbstraction(Expression.Lambda(Expression.Empty(), Expression.Parameter(type1)), parameters: null), AssertParameterName("parameters"));

            Assert.ThrowsException<ArgumentException>(() => factory.LambdaAbstraction(Expression.Lambda(Expression.Empty(), Expression.Parameter(type1)), Array.Empty<QueryTree>().ToReadOnly()));
            Assert.IsTrue(comparer.Equals(
                factory.LambdaAbstraction(Expression.Lambda(Expression.Empty(), Expression.Parameter(type1)), new QueryTree[] { monadMember1 }.ToReadOnly()),
                unsafeFactory.LambdaAbstraction(Expression.Lambda(Expression.Empty(), Expression.Parameter(type1)), new QueryTree[] { monadMember1 }.ToReadOnly())
            ));
        }

        [TestMethod]
        public void DefaultQueryExpressionFactory_Assembly()
        {
            Type type = typeof(DefaultQueryExpressionFactory);
            string assembly = type.Assembly.GetName().Name;
            Assert.AreEqual("Nuqleon.Linq.CompilerServices.Optimizers", assembly);
        }

        [TestMethod]
        public void QueryExpressionFactory_Inference()
        {
            var factory = DefaultQueryExpressionFactory.Instance;
            var type1 = typeof(Exception);
            var type2 = typeof(ArgumentException);
            var lambdaAbstraction1 = factory.LambdaAbstraction(Expression.Lambda(Expression.Default(type1)));
            var lambdaAbstraction2 = factory.LambdaAbstraction(Expression.Lambda(Expression.Default(type2)));
            var monadMember1 = factory.MonadAbstraction(type1, lambdaAbstraction1);

            {
                var q = factory.First(monadMember1);
                Assert.AreSame(type1, q.ElementType);
            }

            {
                var q = factory.First(monadMember1, lambdaAbstraction1);
                Assert.AreSame(type1, q.ElementType);
            }

            {
                var q = factory.Select(monadMember1, lambdaAbstraction2);
                Assert.AreSame(type2, q.ElementType);
                Assert.AreSame(type1, q.InputElementType);
            }

            {
                var q = factory.Take(monadMember1, lambdaAbstraction1);
                Assert.AreSame(type1, q.ElementType);
            }

            {
                var q = factory.Where(monadMember1, lambdaAbstraction1);
                Assert.AreSame(type1, q.ElementType);
            }
        }

        private static Action<ArgumentException> AssertParameterName(string paramName)
        {
            return (ArgumentException ex) => { Assert.AreEqual(paramName, ex.ParamName); };
        }
    }
}
