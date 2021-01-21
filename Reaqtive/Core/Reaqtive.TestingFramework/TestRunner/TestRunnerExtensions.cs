// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtive.TestingFramework.TestRunner
{
    public static class TestRunnerExtensions
    {
        public static void Run(this TestAssemblyRunner runner)
        {
            if (runner == null)
                throw new ArgumentNullException(nameof(runner));

            runner.Run(1);
        }
    }
}
