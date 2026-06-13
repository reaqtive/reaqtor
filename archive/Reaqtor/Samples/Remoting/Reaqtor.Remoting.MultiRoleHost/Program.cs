// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - June 2014 - Created this file.
//

using System;
using System.Globalization;
using System.Threading;

using Reaqtor.Remoting.KeyValueStore;
using Reaqtor.Remoting.Messaging;
using Reaqtor.Remoting.Metadata;
using Reaqtor.Remoting.Platform;
using Reaqtor.Remoting.QueryCoordinator;
using Reaqtor.Remoting.QueryEvaluator;
using Reaqtor.Remoting.StateStore;

namespace Reaqtor.Remoting.MultiRoleHost
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var serviceArguments = ParseArguments(args);
            var started = serviceArguments.Length;

            var autoReset = new AutoResetEvent(false);

            foreach (var s in serviceArguments)
            {
                switch (s.Type)
                {
                    case ReactiveServiceType.MetadataService:
                        {
                            var service = new TcpRemoteServiceHost<StorageConnection>(new[] { s.Port, s.Uri });
                            Start(service, autoReset, --started);
                            break;
                        }
                    case ReactiveServiceType.QueryCoordinator:
                        {
                            var service = new TcpRemoteServiceHost<QueryCoordinatorServiceConnection>(new[] { s.Port, s.Uri });
                            Start(service, autoReset, --started);
                            break;
                        }
                    case ReactiveServiceType.QueryEvaluator:
                        {
                            var service = new TcpRemoteServiceHost<QueryEvaluatorServiceConnection>(new[] { s.Port, s.Uri });
                            Start(service, autoReset, --started);
                            break;
                        }
                    case ReactiveServiceType.MessagingService:
                        {
                            var service = new TcpRemoteServiceHost<MessagingConnection>(new[] { s.Port, s.Uri });
                            Start(service, autoReset, --started);
                            break;
                        }
                    case ReactiveServiceType.StateStoreService:
                        {
                            var service = new TcpRemoteServiceHost<StateStoreConnection>(new[] { s.Port, s.Uri });
                            Start(service, autoReset, --started);
                            break;
                        }
                    case ReactiveServiceType.KeyValueStoreService:
                        {
                            var service = new TcpRemoteServiceHost<KeyValueStoreConnection>(new[] { s.Port, s.Uri });
                            Start(service, autoReset, --started);
                            break;
                        }
                    case ReactiveServiceType.DeployerService:
                    default:
                        throw new NotSupportedException(
                            string.Format(CultureInfo.InvariantCulture, "Service type '{0}' is not supported.", s.Type));
                }
            }
        }

        private static void Start<T>(TcpRemoteServiceHost<T> service, AutoResetEvent autoReset, int count)
            where T : new()
        {
            if (count > 0)
            {
                autoReset.Set();
                service.Start(autoReset);
            }
            else
            {
                service.Start(autoReset);
            }
        }

        private static ServiceSettings[] ParseArguments(string[] args)
        {
            if (args.Length == 0 || args.Length % 3 != 0)
            {
                throw new ArgumentException("Expected at least three arguments, with count divisible by 3.");
            }

            var serviceArguments = new ServiceSettings[args.Length / 3];
            for (var i = 0; i < args.Length / 3; ++i)
            {
                serviceArguments[i] = new ServiceSettings
                {
                    Type = ParseType(args[3 * i + 0]),
                    Uri = args[3 * i + 1],
                    Port = args[3 * i + 2],
                };
            }
            return serviceArguments;
        }

        private static ReactiveServiceType ParseType(string value)
        {
            return value switch
            {
                "MetadataService" => ReactiveServiceType.MetadataService,
                "MessagingService" => ReactiveServiceType.MessagingService,
                "QueryCoordinator" => ReactiveServiceType.QueryCoordinator,
                "QueryEvaluator" => ReactiveServiceType.QueryEvaluator,
                "StateStoreService" => ReactiveServiceType.StateStoreService,
                "KeyValueStoreService" => ReactiveServiceType.KeyValueStoreService,
                _ => throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Service type '{0}' is not supported.", value)),
            };
        }

        private struct ServiceSettings
        {
            public ReactiveServiceType Type;
            public string Uri;
            public string Port;
        }
    }
}
