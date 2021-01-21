// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

#if ENABLE_EXTENDED_OPERATORS

using System;

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
    public partial class Throw : OperatorTestBase
    {
        [TestMethod]
        public void Throw_Simple()
        {
            Run(client =>
            {
                var ex = new Exception();

                var context = GetContext(client);

                var res = client.Start(() =>
                    context.Throw<int>(ex)
                );

                res.Messages.AssertEqual(
                    OnError<int>(Increment(200, 1), ex)
                );
            });
        }
    }
}

#endif
