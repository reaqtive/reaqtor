// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtive;
using Reaqtive.Testing;

namespace Test.Reaqtive.Operators
{
    [TestClass]
    public partial class IsEmpty : OperatorTestBase
    {
        [TestMethod]
        public void IsEmpty_ArgumentChecking()
        {
#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.IsEmpty<int>(default(ISubscribable<int>)));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }
    }
}
