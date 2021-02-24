// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtor;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor.Shared.Core.Reaqtor
{
    [TestClass]
    public class UriHelperTests
    {
        [TestMethod]
        public void UriHelper_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => UriHelper.ToCanonicalString(null));
        }

        [TestMethod]
        public void UriHelper_Simple()
        {
            var uri = new Uri("eg:/foo");
            Assert.AreEqual(uri.AbsoluteUri, uri.ToCanonicalString());
        }
    }
}
