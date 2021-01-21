// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtor;

namespace Tests.Reaqtor.Shared.Core.Reaqtor
{
    [TestClass]
    public class SubstituteAndUnquoteRewriterTests
    {
        private static readonly IDictionary<Type, Type> EmptyTypeMap = new Dictionary<Type, Type>();

        [TestMethod]
        public void SubstituteAndUnquoteRewriter_RemoveIdentityFunctions()
        {
            var rewriter = new SubstituteAndUnquoteRewriter(EmptyTypeMap);

            var inner = Expression.Constant(42);
            var identity = Expression.Invoke(Expression.Parameter(typeof(Func<int, int>), Constants.IdentityFunctionUri), inner);

            var result = rewriter.Apply(identity);
            Assert.AreSame(inner, result);
        }

        [TestMethod]
        public void SubstituteAndUnquoteRewriter_IdentityFunctionIncorrectSignature_ThrowsInvalidOperation()
        {
            var rewriter = new SubstituteAndUnquoteRewriter(EmptyTypeMap);

            var inner = Expression.Constant(42);
            var identity = Expression.Invoke(Expression.Parameter(typeof(Func<int, long>), Constants.IdentityFunctionUri), inner);

            Assert.ThrowsException<InvalidOperationException>(() => rewriter.Apply(identity));
        }

        [TestMethod]
        public void SubstituteAndUnquoteRewriter_IdentitySignature_Unchanged()
        {
            var rewriter = new SubstituteAndUnquoteRewriter(EmptyTypeMap);

            var inner = Expression.Constant(42);
            var notIdentity = Expression.Invoke(Expression.Parameter(typeof(Func<int, int>), "foo"), inner);

            var result = rewriter.Apply(notIdentity);
            Assert.AreSame(notIdentity, result);
        }

        [TestMethod]
        public void SubstituteAndUnquoteRewriter_Unquoted()
        {
            var inner = Expression.Lambda(Expression.Constant(42));
            var quoted = Expression.Quote(inner);
            var lambda = Expression.Lambda(quoted);

            Assert.AreEqual(typeof(Func<Expression<Func<int>>>), lambda.Type);
            var rewriter = new SubstituteAndUnquoteRewriter(EmptyTypeMap);
            var unquoted = (LambdaExpression)rewriter.Apply(lambda);
            var result = unquoted.Compile().DynamicInvoke() as Func<int>;
            Assert.IsNotNull(result);
            Assert.AreEqual(42, result());
        }

        [TestMethod]
        public void SubstituteAndUnquoteRewriter_Unchanged()
        {
            var inner = Expression.Lambda(Expression.Constant(42));
            var lambda = Expression.Lambda(inner);

            var rewriter = new SubstituteAndUnquoteRewriter(EmptyTypeMap); var result = (LambdaExpression)rewriter.Visit(lambda);
            Assert.AreSame(lambda, result);
        }

        [TestMethod]
        public void SubstituteAndUnquoteRewriter_PartialChange()
        {
            var inner = Expression.Lambda(Expression.Negate(Expression.Constant(42)));
            var quoted = Expression.Quote(inner);
            var lambda = Expression.Lambda(quoted);

            Assert.AreEqual(typeof(Func<Expression<Func<int>>>), lambda.Type);
            var rewriter = new SubstituteAndUnquoteRewriter(EmptyTypeMap);
            var unquoted = (LambdaExpression)rewriter.Apply(lambda);
            var result = unquoted.Compile().DynamicInvoke() as Func<int>;
            Assert.IsNotNull(result);
            Assert.AreEqual(-42, result());
        }
    }
}
