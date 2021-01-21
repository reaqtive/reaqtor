// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtive;
using Reaqtive.Testing;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Reaqtive.Operators
{
    [TestClass]
    public partial class All : OperatorTestBase
    {
        [TestMethod]
        public void All_ArgumentChecking()
        {
#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.All<int>(default(ISubscribable<int>), x => true));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.All<int>(DummySubscribable<int>.Instance, default(Func<int, bool>)));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }
    }
}
