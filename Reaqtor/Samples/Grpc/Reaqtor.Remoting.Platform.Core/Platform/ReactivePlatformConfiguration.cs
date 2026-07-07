// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.Platform
{
    //
    // ADAPTATION (plan §3.3): the archived configuration was a private, [Serializable] nested
    // ReactivePlatformConfiguration whose members were live, proxy-typed connections that were marshalled
    // across the remoting boundary. On net10.0 nothing is marshalled by serialization, so this is a plain,
    // NON-[Serializable] class. It implements the IReactivePlatformConfiguration interface that already lives in
    // Reaqtor.Remoting.Core. The connection getters hold the transport-neutral sync connection objects that the
    // platform resolves at StartAsync time (over gRPC in the live transport, or directly in-proc for the
    // InMemory oracle).
    //
    public sealed class ReactivePlatformConfiguration : IReactivePlatformConfiguration
    {
        public SchedulerType SchedulerType { get; set; }

        public int? SchedulerThreadCount { get; set; }

        public TraceListenerType TraceListenerType { get; set; }

        public string TraceListenerFileName { get; set; }

        public Dictionary<string, string> EngineOptions { get; } = [];

        public MetadataStorageType StorageType { get; set; }

        public IReactiveStorageConnection StorageConnection { get; set; }

        public string AzureConnectionString { get; set; }

        public IReactiveMessagingConnection MessagingConnection { get; set; }

        public IReactiveStateStoreConnection StateStoreConnection { get; set; }

        public ITransactionalKeyValueStoreConnection KeyValueStoreConnection { get; set; }

        public IList<IReactiveQueryEvaluatorConnection> QueryEvaluatorConnections { get; } = [];
    }
}
