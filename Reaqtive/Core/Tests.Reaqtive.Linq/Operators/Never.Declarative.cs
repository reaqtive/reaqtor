// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

#if GLITCHING
using Reaqtive;
#endif

using Reaqtive.Testing;
using Reaqtive.TestingFramework;

#if GLITCHING
using Reaqtor;
using Reaqtor.TestingFramework;
#endif

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Reaqtive.Operators
{
    public partial class Never : OperatorTestBase
    {
        [TestMethod]
        public void Never_Simple()
        {
            Run(client =>
            {
                var context = GetContext(client);

                var res = client.Start(() =>
                    context.Never<int>()
                );

                res.Messages.AssertEqual(
                );
            });
        }
    }
}
