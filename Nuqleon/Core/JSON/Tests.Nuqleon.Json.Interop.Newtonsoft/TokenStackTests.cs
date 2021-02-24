// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2016 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Nuqleon.Json.Expressions;
using Nuqleon.Json.Interop.Newtonsoft;

namespace Tests.Nuqleon.Json.Interop.Newtonsoft
{
    [TestClass]
    public class TokenStackTests
    {
        [TestMethod]
        public void TokenStack_Push1()
        {
            var tokens = new TokenStack();

            Assert.IsFalse(tokens.TryPop(out _));
            Assert.AreEqual(0, tokens.Count);

            for (var i = 0; i < 99; i++)
            {
                tokens.Push(new Token { Expression = Expression.Number(i.ToString()) });
                Assert.AreEqual(i + 1, tokens.Count);
            }

            for (var i = 98; i >= 0; i--)
            {
                Assert.IsTrue(tokens.TryPop(out var token));
                Assert.AreEqual(i, tokens.Count);
                Assert.AreEqual(i.ToString(), ((ConstantExpression)token.Expression).Value);
            }
        }

        [TestMethod]
        public void TokenStack_PushN()
        {
            var tokens = new TokenStack();

            Assert.IsFalse(tokens.TryPop(out _));
            Assert.AreEqual(0, tokens.Count);

            tokens.Push(99);
            Assert.AreEqual(99, tokens.Count);

            for (var i = 0; i < 99; i++)
            {
                tokens[i] = new Token { Expression = Expression.Number(i.ToString()) };
            }

            for (var i = 98; i >= 0; i--)
            {
                Assert.IsTrue(tokens.TryPop(out var token));
                Assert.AreEqual(i, tokens.Count);
                Assert.AreEqual(i.ToString(), ((ConstantExpression)token.Expression).Value);
            }
        }
    }
}
