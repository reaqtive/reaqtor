// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor.QueryEngine
{
    using global::Reaqtor;

    [TestClass]
    public class UtilsTests
    {
        #region CompleteIfNotNull

        [TestMethod]
        public void Utils_CompleteIfNotNull_Null()
        {
            Utils.CompleteIfNotNull(null);
            Utils.CompleteIfNotNull(null, CancellationToken.None);

            // nothing has thrown
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void Utils_CompleteIfNotNull_Simple()
        {
            var progress = new SimpleProgress();

            Utils.CompleteIfNotNull(progress);

            Assert.AreEqual(100, progress.Value);
        }

        [TestMethod]
        public void Utils_CompleteIfNotNull_Simple_NotCancelled()
        {
            var progress = new SimpleProgress();

            Utils.CompleteIfNotNull(progress, CancellationToken.None);

            Assert.AreEqual(100, progress.Value);
        }

        [TestMethod]
        public void Utils_CompleteIfNotNull_Simple_Cancelled()
        {
            var progress = new SimpleProgress();

            var cts = new CancellationTokenSource();
            cts.Cancel();

            Utils.CompleteIfNotNull(progress, cts.Token);

            Assert.AreEqual(0, progress.Value);
        }

        private sealed class SimpleProgress : IProgress<int>
        {
            public int Value;

            public void Report(int value)
            {
                Value = value;
            }
        }

        #endregion

        #region GetBase64Blob

        [TestMethod]
        public void Utils_GetBase64Blob_Null()
        {
            var res = Utils.GetBase64Blob(null);

            Assert.AreEqual("(empty)", res);
        }

        [TestMethod]
        public void Utils_GetBase64Blob_Simple()
        {
            var bs = new byte[1024];
            new Random().NextBytes(bs);

            var ms = new MemoryStream(bs)
            {
                Position = 256
            };

            var res = Utils.GetBase64Blob(ms);

            Assert.AreEqual(256, ms.Position);

            var b64 = Convert.ToBase64String(bs);

            Assert.AreEqual(b64, res);
        }

        #endregion

#if OLD
        [TestMethod]
        public void Utils_Sequential_NoCancel()
        {
            var token = CancellationToken.None;

            var log = "";

            Utils.Execute(token,
                ct => { log += "a"; },
                ct => { log += "b"; },
                ct => { log += "c"; }
            );

            Assert.AreEqual("abc", log);
        }

        [TestMethod]
        public void Utils_Sequential_Cancel()
        {
            var cts = new CancellationTokenSource();

            var log = "";

            Utils.Execute(cts.Token,
                ct => { log += "a"; },
                ct => { log += "b"; cts.Cancel(); },
                ct => { log += "c"; }
            );

            Assert.AreEqual("ab", log);
        }

        [TestMethod]
        public void Utils_Sequential_Progress_NoCancel()
        {
            var token = CancellationToken.None;

            var log = "";
            var prg = "";
            var rp = new Action(() => { prg += "."; });

            Utils.Execute(token, rp,
                ct => { log += "a"; },
                ct => { log += "b"; },
                ct => { log += "c"; }
            );

            Assert.AreEqual("abc", log);
            Assert.AreEqual("...", prg);
        }

        [TestMethod]
        public void Utils_Sequential_Progress_Cancel()
        {
            var cts = new CancellationTokenSource();

            var log = "";
            var prg = "";
            var rp = new Action(() => { prg += "."; });

            Utils.Execute(cts.Token, rp,
                ct => { log += "a"; },
                ct => { log += "b"; cts.Cancel(); },
                ct => { log += "c"; }
            );

            Assert.AreEqual("ab", log);
            Assert.AreEqual("..", prg);
        }

        [TestMethod]
        public void ForEach_Sequential_NoCancel()
        {
            var token = CancellationToken.None;

            var log = "";

            new[] { "a", "b", "c" }.ForEach(token, (ct, s) =>
            {
                log += s;
            });

            Assert.AreEqual("abc", log);
        }

        [TestMethod]
        public void ForEach_Sequential_Cancel()
        {
            var cts = new CancellationTokenSource();

            var log = "";

            new[] { "a", "b", "c" }.ForEach(cts.Token, (ct, s) =>
            {
                log += s;

                if (s == "b")
                    cts.Cancel();
            });

            Assert.AreEqual("ab", log);
        }

        [TestMethod]
        public void ForEach_Sequential_Progress_NoCancel()
        {
            var token = CancellationToken.None;

            var log = "";
            var prg = "";
            var rp = new Action(() => { prg += "."; });

            new[] { "a", "b", "c" }.ForEach(token, rp, (ct, s) =>
            {
                log += s;
            });

            Assert.AreEqual("abc", log);
            Assert.AreEqual("...", prg);
        }

        [TestMethod]
        public void ForEach_Sequential_Progress_Cancel()
        {
            var cts = new CancellationTokenSource();

            var log = "";
            var prg = "";
            var rp = new Action(() => { prg += "."; });

            new[] { "a", "b", "c" }.ForEach(cts.Token, rp, (ct, s) =>
            {
                log += s;

                if (s == "b")
                    cts.Cancel();
            });

            Assert.AreEqual("ab", log);
            Assert.AreEqual("..", prg);
        }
#endif
    }
}
