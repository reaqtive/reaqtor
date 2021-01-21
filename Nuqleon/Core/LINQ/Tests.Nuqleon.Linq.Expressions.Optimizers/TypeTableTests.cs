// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq.Expressions;

namespace Tests.System.Linq.Expressions.Optimizers
{
    [TestClass]
    public partial class TypeTableTests
    {
        [TestMethod]
        public void TypeTable_Add_ArgumentChecking()
        {
            var tt = new TypeTable();

            Assert.ThrowsException<ArgumentNullException>(() => tt.Add(default(Type)));
            Assert.ThrowsException<ArgumentNullException>(() => tt.Add(default(TypeTable)));
        }

        [TestMethod]
        public void TypeTable_Contains_ArgumentChecking()
        {
            var tt = new TypeTable();

            Assert.ThrowsException<ArgumentNullException>(() => tt.Contains(default));
        }

        [TestMethod]
        public void TypeTable_ReadOnly()
        {
            var tt = new TypeTable();
            var rtt = tt.ToReadOnly();

            Assert.ThrowsException<InvalidOperationException>(() => rtt.Add(typeof(int)));
        }
    }
}
