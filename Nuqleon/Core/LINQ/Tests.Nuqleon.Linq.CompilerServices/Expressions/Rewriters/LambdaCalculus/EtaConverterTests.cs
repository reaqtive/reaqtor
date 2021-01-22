// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class EtaConverterTests
    {
        [TestMethod]
        public void EtaConverter_ArgumentChecking()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => EtaConverter.Convert(expression: null), ex => Assert.AreEqual("expression", ex.ParamName));
        }

        [TestMethod]
        public void EtaConverter_Simple()
        {
            var e = (Expression<Func<Func<int, int, int>, Func<int, int, int>>>)(f => (x, y) => f(x, y));
            var c = EtaConverter.Convert(e.Body);
            Assert.AreSame(e.Parameters[0], c);
        }

        [TestMethod]
        public void EtaConverter_Negative()
        {
            var e = (Expression<Func<Func<int, int, int>, Func<int, int, int>>>)(f => (x, y) => f(y, x));
            var c = EtaConverter.Convert(e);
            Assert.AreSame(e, c);
        }
    }
}
