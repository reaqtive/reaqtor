// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class CpsRewriterBaseTests
    {
        [TestMethod]
        public void CpsRewriterBase_ProtectedNulls()
        {
            new MyRewriter().Do();
        }

        private sealed class MyRewriter : CpsRewriterBase<string>
        {
            public void Do()
            {
                AssertEx.ThrowsException<ArgumentNullException>(() => base.RewriteCore(expression: null, ""), ex => Assert.AreEqual("expression", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.FindAsyncMethod(method: null), ex => Assert.AreEqual("method", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.GetAsyncMethodName(method: null), ex => Assert.AreEqual("method", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.CheckAsyncMethod(synchronousMethod: null, asynchronousMethod: null), ex => Assert.AreEqual("synchronousMethod", ex.ParamName));
            }

            protected override IEnumerable<Type> GetContinuationParameters(MethodInfo method)
            {
                throw new NotImplementedException();
            }

            protected override Expression MakeAsyncMethodCall(Expression instance, MethodInfo method, IEnumerable<Expression> arguments, string continuation)
            {
                throw new NotImplementedException();
            }

            protected override string MakeContinuation(Expression remainder, ParameterExpression resultParameter, string currentContinuation)
            {
                throw new NotImplementedException();
            }

            protected override Expression InvokeContinuation(string continuation, Expression argument)
            {
                throw new NotImplementedException();
            }
        }
    }
}
