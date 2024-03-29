﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtive;

namespace Test.Reaqtive.Operators
{
    [TestClass]
    public partial class Throw : OperatorTestBase
    {
        [TestMethod]
        public void Throw_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Subscribable.Throw<int>(null));
        }
    }
}
