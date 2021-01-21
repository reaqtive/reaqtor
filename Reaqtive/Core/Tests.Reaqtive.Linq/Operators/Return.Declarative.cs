// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Reaqtive.Testing;
using Reaqtive.TestingFramework;

#if GLITCHING
using Reaqtor;
using Reaqtor.TestingFramework;
#endif

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Reaqtive.Operators
{
    public partial class Return : OperatorTestBase
    {
        [TestMethod]
        public void Return_Simple()
        {
            Run(client =>
            {
                var context = GetContext(client);

                var res = client.Start(() =>
                    context.Return<int>(42)
                );

                res.Messages.AssertEqual(
                    OnNext<int>(Increment(200, 1), 42),
                    OnCompleted<int>(Increment(200, 1))
                );
            });
        }
    }
}
