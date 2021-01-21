// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 08/03/2015 - Wrote these tests.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Memory;

namespace Tests
{
    [TestClass]
    public class ValueOrErrorTests
    {
        [TestMethod]
        public void ValueOrError_CreateValue()
        {
            var val = ValueOrError.CreateValue<int>(42);

            Assert.AreEqual(42, val.Value);
            Assert.AreEqual(ValueOrErrorKind.Value, val.Kind);
            Assert.ThrowsException<InvalidOperationException>(() => val.Exception);
        }

        [TestMethod]
        public void ValueOrError_CreateError_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => ValueOrError.CreateError<int>(exception: null));
        }

        [TestMethod]
        public void ValueOrError_CreateError()
        {
            var ex = new Exception();
            var val = ValueOrError.CreateError<int>(ex);

            Assert.AreSame(ex, val.Exception);
            Assert.AreEqual(ValueOrErrorKind.Error, val.Kind);
            AssertEx.ThrowsException<Exception>(() => val.Value, err => object.ReferenceEquals(err, ex));
        }
    }
}
