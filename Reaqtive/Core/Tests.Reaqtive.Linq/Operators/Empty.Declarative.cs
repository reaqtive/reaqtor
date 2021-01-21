// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Reaqtive;
using Reaqtive.Testing;
using Reaqtive.TestingFramework;

#if GLITCHING
using Reaqtor;
using Reaqtor.TestingFramework;
#endif

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Reaqtive.Operators
{
    public partial class Empty : OperatorTestBase
    {
        [TestMethod]
        public void Empty_Simple()
        {
            Run(client =>
            {
                var context = GetContext(client);

                var res = client.Start(() =>
                    context.Empty<int>()
                );

                res.Messages.AssertEqual(
                    OnCompleted<int>(Increment(200, 1))
                );
            });
        }

        [TestMethod]
        public void Empty_Chained()
        {
            Run(client =>
            {
                var context = GetContext(client);

                var res = client.Start(() =>
                    context.Empty<int>().StartWith(1, 2, 3)
                );

                res.Messages.AssertEqual(
                    OnNext(Increment(200, 1), 1),
                    OnNext(Increment(200, 2), 2),
                    OnNext(Increment(200, 3), 3),
                    OnCompleted<int>(Increment(200, 4))
                );
            });
        }
    }
}
