// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor.Remoting.Platform
{
    public abstract class ReactiveServiceBase : IReactiveService
    {
        protected ReactiveServiceBase(IRunnable runnable, ReactiveServiceType serviceType)
        {
            Runnable = runnable;
            ServiceType = serviceType;
        }

        public IRunnable Runnable { get; }

        public ReactiveServiceType ServiceType { get; }

        public virtual async Task StartAsync(CancellationToken token)
        {
            await Runnable.RunAsync(token).ConfigureAwait(false);
        }

        public virtual Task StopAsync(CancellationToken token)
        {
            Runnable.Dispose();
            return Task.FromResult(true);
        }

        public void Register(IReactiveService service)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));

            switch (service.ServiceType)
            {
                case ReactiveServiceType.QueryCoordinator:
                    RegisterQueryCoordinator((IReactiveQueryCoordinator)service);
                    break;
                case ReactiveServiceType.QueryEvaluator:
                    RegisterQueryEvaluator((IReactiveQueryEvaluator)service);
                    break;
                case ReactiveServiceType.MetadataService:
                    RegisterMetadataService((IReactiveMetadataService)service);
                    break;
                case ReactiveServiceType.MessagingService:
                    RegisterMessagingService((IReactiveMessagingService)service);
                    break;
                case ReactiveServiceType.StateStoreService:
                    RegisterStateStoreService((IReactiveStateStoreService)service);
                    break;
                case ReactiveServiceType.KeyValueStoreService:
                    RegisterKeyValueStoreService((IKeyValueStoreService)service);
                    break;
                default:
                    RegisterOtherService(service);
                    break;
            }
        }

        protected virtual void RegisterQueryCoordinator(IReactiveQueryCoordinator service)
        {
        }

        protected virtual void RegisterQueryEvaluator(IReactiveQueryEvaluator service)
        {
        }

        protected virtual void RegisterMetadataService(IReactiveMetadataService service)
        {
        }

        protected virtual void RegisterMessagingService(IReactiveMessagingService service)
        {
        }

        protected virtual void RegisterStateStoreService(IReactiveStateStoreService service)
        {
        }

        protected virtual void RegisterKeyValueStoreService(IKeyValueStoreService service)
        {
        }

        protected virtual void RegisterOtherService(IReactiveService service)
        {
        }

        public virtual TInstance GetInstance<TInstance>()
        {
            return (TInstance)Runnable.Instance;
        }
    }
}
