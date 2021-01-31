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

            var d3 = f.Compile(CompilationOptions.PreferInterpretation);
            Assert.AreEqual(42, d3());

            var d4 = f.Compile(CompilationOptions.EnableJustInTimeCompilation);
            Assert.AreEqual(42, d4());

            var d5 = f.Compile(CompilationOptions.EnableJustInTimeCompilation | CompilationOptions.PreferInterpretation);
            Assert.AreEqual(42, d5());

            var d6 = f.Compile(CompilationOptions.EnableJustInTimeCompilation | CompilationOptions.TieredCompilation);

            for (int i = 0; i < 8; i++)
            {
                Assert.AreEqual(42, d6());
            }

            var d11 = ((LambdaExpression)f).Compile(CompilationOptions.None);
            Assert.AreEqual(42, d11.DynamicInvoke());

            var d12 = ((LambdaExpression)f).Compile(CompilationOptions.Optimize);
            Assert.AreEqual(42, d12.DynamicInvoke());

            var d13 = ((LambdaExpression)f).Compile(CompilationOptions.PreferInterpretation);
            Assert.AreEqual(42, d13.DynamicInvoke());

            var d14 = ((LambdaExpression)f).Compile(CompilationOptions.EnableJustInTimeCompilation);
            Assert.AreEqual(42, d14.DynamicInvoke());

            var d15 = ((LambdaExpression)f).Compile(CompilationOptions.EnableJustInTimeCompilation | CompilationOptions.PreferInterpretation);
            Assert.AreEqual(42, d15.DynamicInvoke());

            var d16 = ((LambdaExpression)f).Compile(CompilationOptions.EnableJustInTimeCompilation | CompilationOptions.TieredCompilation);

            for (int i = 0; i < 8; i++)
            {
                Assert.AreEqual(42, d16.DynamicInvoke());
            }
        }

        [TestMethod]
        public void Compile_Jit_Closures()
        {
            Expression<Func<int, Func<int, int>>> f = x => y => x + y;

            foreach (var options in new[]
            {
                CompilationOptions.EnableJustInTimeCompilation,
                CompilationOptions.EnableJustInTimeCompilation | CompilationOptions.PreferInterpretation,
                CompilationOptions.EnableJustInTimeCompilation | CompilationOptions.TieredCompilation,
            })
            {
                var d = f.Compile(options);

                for (int i = 0; i < 8; i++)
                {
                    var g1 = d(3);
                    Assert.AreEqual(7, g1(4));
                    Assert.AreEqual(8, g1(5));

                    var g2 = d(5);
                    Assert.AreEqual(9, g2(4));
                    Assert.AreEqual(7, g2(2));

                    for (int j = 0; j < 8; j++)
                    {
                        Assert.AreEqual(6, g2(1));
                        Assert.AreEqual(4, g1(1));
                    }
                }
            }
        }

        //
        // TODO: Add tests for the following node types.
        //
        //       - Block
        //       - Catch
        //       - Quote
        //       - RuntimeVariables
        //
    }
}
