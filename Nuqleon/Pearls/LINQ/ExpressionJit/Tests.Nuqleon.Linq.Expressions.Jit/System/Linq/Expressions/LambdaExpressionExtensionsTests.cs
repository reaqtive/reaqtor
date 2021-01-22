// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Linq.Expressions
{
    [TestClass]
    public class LambdaExpressionExtensionsTests
    {
        [TestMethod]
        public void Compile_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => LambdaExpressionExtensions.Compile(expression: null, CompilationOptions.None));
            Assert.ThrowsException<ArgumentNullException>(() => LambdaExpressionExtensions.Compile<Action>(expression: null, CompilationOptions.None));
        }

        [TestMethod]
        public void Compile()
        {
            Expression<Func<int>> f = () => 42;

            var d1 = f.Compile(CompilationOptions.None);
            Assert.AreEqual(42, d1());

            var d2 = f.Compile(CompilationOptions.Optimize);
            Assert.AreEqual(42, d2());

            var d3 = ((LambdaExpression)f).Compile(CompilationOptions.None);
            Assert.AreEqual(42, d3.DynamicInvoke());

            var d4 = ((LambdaExpression)f).Compile(CompilationOptions.Optimize);
            Assert.AreEqual(42, d4.DynamicInvoke());
        }
    }
}
