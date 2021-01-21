// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ControlFlow;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor.QueryEngine
{
    [TestClass]
    public class ControlFlowTests
    {
        [TestMethod]
        public void ControlFlow_ForEach_NoCancel()
        {
            var res = 0;

            Control.ForEach(new[] { 1, 2, 3 }, (x, ct) =>
            {
                res += x;
            }, CancellationToken.None);

            Assert.AreEqual(6, res);

            foreach (var b in new[] { true, false })
            {
                res = 0;

                Control.ForEach(new[] { 1, 2, 3 }, (x, ct) =>
                {
                    res += x;
                }, b, CancellationToken.None);

                Assert.AreEqual(6, res);
            }
        }

        [TestMethod]
        public void ControlFlow_ForEach_Cancel_Throw1()
        {
            ControlFlow_ForEach_Cancel_Throw_Test((xs, ct, body) =>
            {
                Control.ForEach(xs, body, ct);
            }, true);
        }

        [TestMethod]
        public void ControlFlow_ForEach_Cancel_Throw2()
        {
            ControlFlow_ForEach_Cancel_Throw_Test((xs, ct, body) =>
            {
                Control.ForEach(xs, body, true, ct);
            }, true);
        }

        [TestMethod]
        public void ControlFlow_ForEach_Cancel_NoThrow()
        {
            ControlFlow_ForEach_Cancel_Throw_Test((xs, ct, body) =>
            {
                Control.ForEach(xs, body, false, ct);
            }, false);
        }

        private static void ControlFlow_ForEach_Cancel_Throw_Test(Action<IEnumerable<int>, CancellationToken, Action<int, CancellationToken>> getCall, bool expectThrow)
        {
            var cts = new CancellationTokenSource();

            var tcs = new TaskCompletionSource<bool>();

            var done = tcs.Task.ContinueWith(_ =>
            {
                cts.Cancel();
            }, TaskContinuationOptions.ExecuteSynchronously);

            var res = 0;

            var a = new Action(() =>
            {
                getCall(new[] { 1, 2, 3 }, cts.Token, (x, ct) =>
                {
                    res += x;

                    if (x == 2)
                    {
                        tcs.SetResult(true);
                        done.Wait(CancellationToken.None);
                        Assert.IsTrue(ct.IsCancellationRequested);
                    }
                });
            });

            if (expectThrow)
            {
                Assert.ThrowsException<OperationCanceledException>(a);
            }
            else
            {
                a();
            }

            Assert.AreEqual(3, res);
        }
    }
}
