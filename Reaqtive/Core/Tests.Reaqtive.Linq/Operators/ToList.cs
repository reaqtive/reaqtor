// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

using Reaqtive;
using Reaqtive.Testing;
using Reaqtive.TestingFramework;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Reaqtive.Operators
{
    [TestClass]
    public partial class ToList : OperatorTestBase
    {
        [TestMethod]
        public void ToList_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Subscribable.ToList<int>(null));
        }

        [TestMethod]
        public void ToList_Settings_MaxListSize()
        {
            Run(client =>
            {
                var ctx = client.CreateContext(settings: new Dictionary<string, object>
                {
                    { "rx://operators/toList/settings/maxListSize", 5 },
                });

                var xs = client.CreateHotObservable<int>(
                    OnNext(210, 1),
                    OnNext(220, 2),
                    OnNext(230, 3),
                    OnNext(240, 4),
                    OnNext(250, 5),
                    OnNext(260, 6)
                );

                var res = client.Start(ctx, () =>
                    xs.ToList(),
                    100, 200, 1000);

                res.Messages.AssertEqual(
                    OnError<IList<int>>(260, ex => ex is InvalidOperationException)
                );

                xs.Subscriptions.AssertEqual(
                    Subscribe(200, 260)
                );
            });
        }
    }
}
