// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Reaqtor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Test.Reaqtive.Operators
{
    [TestClass]
    public partial class TakeWhile : OperatorTestBase
    {
        [KnownResource(Reaqtor.Remoting.Platform.Constants.Identifiers.IsPrime)]
        private static bool IsPrime(int i)
        {
            throw new NotImplementedException();
        }
    }
}
