// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Runtime.Remoting.Tasks;
using System.Threading.Tasks;

namespace Tests.System.Runtime.Remoting.Tasks
{
    [TestClass]
    public class ReplyTests
    {
        [TestMethod]
        public void Reply_ArgumentChecks()
        {
            AssertEx.ThrowsException<ArgumentNullException>((Action)(() => _ = new Reply<int>(taskCompletionSource: null)), ex => Assert.AreEqual("taskCompletionSource", ex.ParamName));
        }

        [TestMethod]
        public void Reply_RemotingBehavior()
        {
            var reply = new Reply<int>(new TaskCompletionSource<int>());
            Assert.IsNull(reply.InitializeLifetimeService());
        }

        [TestMethod]
        public void Reply_OnCompleted()
        {
            var reply = new Reply<int>(new TaskCompletionSource<int>());
            Assert.ThrowsException<InvalidOperationException>(() => reply.OnCompleted());
        }
    }
}
