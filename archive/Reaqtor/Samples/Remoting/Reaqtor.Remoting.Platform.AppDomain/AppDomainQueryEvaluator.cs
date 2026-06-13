// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using System.Globalization;

using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Platform
{
    internal sealed class AppDomainQueryEvaluator<TQueryEvaluator> : ReactiveQueryEvaluatorBase, IReactiveQueryEvaluator
       where TQueryEvaluator : IReactiveQueryEvaluatorConnection
    {
        public AppDomainQueryEvaluator(IReactivePlatform platform, string uri)
#pragma warning disable CA2000 // Dispose objects before losing scope. (Ownership transfer.)
            : base(platform, new AppDomainRunnable(string.Format(CultureInfo.InvariantCulture, "QueryEvaluator({0})", uri), typeof(TQueryEvaluator)), ReactiveServiceType.QueryEvaluator)
#pragma warning restore CA2000
        {
        }

        protected override void RegisterMetadataService(IReactiveMetadataService service)
        {
            Helpers.MarshalServiceInstance(this, service);
        }

        protected override void RegisterMessagingService(IReactiveMessagingService service)
        {
            Helpers.MarshalServiceInstance(this, service);
        }

        protected override void RegisterStateStoreService(IReactiveStateStoreService service)
        {
            Helpers.MarshalServiceInstance(this, service);
        }

        protected override void RegisterKeyValueStoreService(IKeyValueStoreService service)
        {
            Helpers.MarshalServiceInstance(this, service);
        }
    }
}
