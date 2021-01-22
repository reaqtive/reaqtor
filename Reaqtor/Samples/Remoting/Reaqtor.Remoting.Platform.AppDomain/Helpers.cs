// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;

namespace Reaqtor.Remoting.Platform
{
    internal static class Helpers
    {
        public static void MarshalServiceInstance(IReactiveService target, IReactiveService source)
        {
            Debug.Assert(target.Runnable is AppDomainRunnable);
            Debug.Assert(source.Runnable is AppDomainRunnable);

            ((AppDomainRunnable)target.Runnable).Marshal(source.GetInstance<object>());
        }
    }
}
