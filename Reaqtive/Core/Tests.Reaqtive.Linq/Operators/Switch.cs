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
    public partial class Switch : OperatorTestBase
    {
        [TestMethod]
        public void Switch_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Subscribable.Switch((ISubscribable<ISubscribable<int>>)null));
        }
    }
}
