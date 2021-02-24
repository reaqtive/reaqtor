// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;
using System.Threading;
using System.Threading.Tasks;

using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Platform
{
    public abstract class ReactivePlatformBase : IReactivePlatform
    {
        private readonly IReactiveEnvironment _environment;
        private readonly ReactivePlatformConfiguration _configuration;

        protected ReactivePlatformBase(IReactiveEnvironment environment)
        {
            _environment = environment;
            _configuration = new ReactivePlatformConfiguration();
        }

        public abstract IReactiveQueryCoordinator QueryCoordinator { get; }

        public abstract IEnumerable<IReactiveQueryEvaluator> QueryEvaluators { get; }

        public virtual IReactiveEnvironment Environment => _environment;

        public virtual IReactivePlatformConfiguration Configuration => _configuration;

        public virtual async Task StartAsync(CancellationToken token)
        {
            var clientProvider = new BinaryClientFormatterSinkProvider();
            var serverProvider = new BinaryServerFormatterSinkProvider { TypeFilterLevel = TypeFilterLevel.Full };
            var props = new System.Collections.Hashtable
                    {
                        { "port", 0 },
                        { "name", Guid.NewGuid().ToString() },
                        { "typeFilterLevel", TypeFilterLevel.Full }
                    };
            ChannelServices.RegisterChannel(new TcpChannel(props, clientProvider, serverProvider), ensureSecurity: false);

            _configuration.MessagingConnection = _environment.MessagingService.GetInstance<IReactiveMessagingConnection>();
            _configuration.StateStoreConnection = _environment.StateStoreService.GetInstance<IReactiveStateStoreConnection>();
            _configuration.KeyValueStoreConnection = _environment.KeyValueStoreService.GetInstance<ITransactionalKeyValueStoreConnection>();

            _configuration.StorageType = _environment.StorageType;
            if (_configuration.StorageType == MetadataStorageType.Remoting)
            {
                _configuration.StorageConnection = _environment.MetadataService.GetInstance<IReactiveStorageConnection>();
            }
            else if (_configuration.StorageType == MetadataStorageType.Azure)
            {
                _configuration.AzureConnectionString = _environment.AzureConnectionString;
            }

            _configuration.QueryEvaluatorConnections = new List<IReactiveQueryEvaluatorConnection>();

            foreach (var queryEvaluator in QueryEvaluators)
            {
                if (_configuration.StorageType == MetadataStorageType.Remoting)
                {
                    queryEvaluator.Register(_environment.MetadataService);
                }

                queryEvaluator.Register(_environment.StateStoreService);
                queryEvaluator.Register(_environment.MessagingService);
                queryEvaluator.Register(_environment.KeyValueStoreService);
                await queryEvaluator.StartAsync(token);
                _configuration.QueryEvaluatorConnections.Add(queryEvaluator.GetInstance<IReactiveQueryEvaluatorConnection>());
                QueryCoordinator.Register(queryEvaluator);
            }

            if (_configuration.StorageType == MetadataStorageType.Remoting)
            {
                QueryCoordinator.Register(_environment.MetadataService);
            }
            await QueryCoordinator.StartAsync(token);
        }

        public virtual async Task StopAsync(CancellationToken token)
        {
            await QueryCoordinator.StopAsync(token);

            foreach (var queryEvaluator in QueryEvaluators)
            {
                await queryEvaluator.StopAsync(token);
            }
        }

        public virtual IReactivePlatformClient CreateClient()
        {
            return new ReactivePlatformClient(this);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                StopAsync(CancellationToken.None).Wait();
            }
        }

        [Serializable]
        private sealed class ReactivePlatformConfiguration : IReactivePlatformConfiguration
        {
            public ReactivePlatformConfiguration()
            {
                EngineOptions = new Dictionary<string, string>();
            }

            public SchedulerType SchedulerType
            {
                get;
                set;
            }

            public int? SchedulerThreadCount
            {
                get;
                set;
            }

            public TraceListenerType TraceListenerType
            {
                get;
                set;
            }

            public string TraceListenerFileName
            {
                get;
                set;
            }

            public Dictionary<string, string> EngineOptions
            {
                get;
                set;
            }

            public MetadataStorageType StorageType
            {
                get;
                set;
            }

            public IReactiveStorageConnection StorageConnection
            {
                get;
                set;
            }

            public string AzureConnectionString
            {
                get;
                set;
            }

            public IReactiveMessagingConnection MessagingConnection
            {
                get;
                set;
            }

            public IReactiveStateStoreConnection StateStoreConnection
            {
                get;
                set;
            }

            public ITransactionalKeyValueStoreConnection KeyValueStoreConnection
            {
                get;
                set;
            }

            public List<IReactiveQueryEvaluatorConnection> QueryEvaluatorConnections
            {
                get;
                set;
            }
        }
    }
}
