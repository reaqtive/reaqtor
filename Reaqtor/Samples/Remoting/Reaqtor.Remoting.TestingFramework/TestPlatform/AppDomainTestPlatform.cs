// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Reaqtor.Remoting.Platform;
using Reaqtor.Remoting.QueryCoordinator;

namespace Reaqtor.Remoting.TestingFramework
{
    public class AppDomainTestPlatform : AppDomainReactivePlatform<QueryCoordinatorServiceConnection, TestQueryEvaluatorServiceConnection>
    {
        public AppDomainTestPlatform()
        {
        }

        public AppDomainTestPlatform(IReactiveEnvironment environment)
            : base(environment)
        {
        }
    }
}
