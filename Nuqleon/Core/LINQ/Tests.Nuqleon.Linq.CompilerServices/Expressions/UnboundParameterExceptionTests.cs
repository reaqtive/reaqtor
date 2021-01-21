// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using System;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

#if !NET5_0 // https://aka.ms/binaryformatter
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
#endif

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class UnboundParameterExceptionTests
    {
        [TestMethod]
        public void UnboundParameterException_ArgumentChecking()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => new UnboundParameterException("", expression: null, Array.Empty<ParameterExpression>()), ex => Assert.AreEqual("expression", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => new UnboundParameterException("", Expression.Constant(42), parameters: null), ex => Assert.AreEqual("parameters", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => UnboundParameterException.ThrowIfOpen(expression: null, ""), ex => Assert.AreEqual("expression", ex.ParamName));
        }

        [TestMethod]
        public void UnboundParameterException_Simple()
        {
            var e = Expression.Constant(42);
            var p = new[] { Expression.Parameter(typeof(int)) };
            var ex = new UnboundParameterException("Oops", e, p);
            Assert.IsTrue(ex.Message.StartsWith("Oops"));
            Assert.AreSame(e, ex.Expression);
            Assert.IsTrue(p.SequenceEqual(ex.Parameters));
        }

#if !NET5_0 // https://aka.ms/binaryformatter
        [TestMethod]
        public void UnboundParameterException_Serialize()
        {
            var ex = new UnboundParameterException("Oops", Expression.Constant(42), new[] { Expression.Parameter(typeof(int)) });

            var ms = new MemoryStream();
            new BinaryFormatter().Serialize(ms, ex);
            ms.Position = 0;

            var err = (UnboundParameterException)new BinaryFormatter().Deserialize(ms);
            Assert.IsNotNull(err);
            Assert.AreEqual(ex.Message, err.Message);
        }
#endif

        [TestMethod]
        public void UnboundParameterException_ThrowIfOpen_Positive()
        {
            var f = (Expression<Func<int, int>>)(x => x + 1);

            AssertEx.ThrowsException<UnboundParameterException>(() => UnboundParameterException.ThrowIfOpen(f.Body, "Oops"), ex =>
            {
                Assert.AreSame(f.Body, ex.Expression);
                Assert.IsTrue(f.Parameters.SequenceEqual(ex.Parameters));
            });
        }

        [TestMethod]
        public void UnboundParameterException_ThrowIfOpen_Negative()
        {
            var f = (Expression<Func<int, int>>)(x => x + 1);

            UnboundParameterException.ThrowIfOpen(f, "Oops");

            Assert.IsTrue(true);
        }
    }
}
