// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - December 2013 - Created this file.
//

using System.Collections.Generic;

namespace Reaqtor.Remoting.Protocol
{
    /// <summary>
    /// The configuration used to initialize remote services for the Reactive platform types.
    /// </summary>
    public interface IReactivePlatformConfiguration
    {
        /// <summary>
        /// Sets the scheduler type for the query evaluators.
        /// </summary>
        SchedulerType SchedulerType { get; set; }

        /// <summary>
        /// Sets the scheduler thread count for the query evaluator.
        /// </summary>
        int? SchedulerThreadCount { get; set; }

        /// <summary>
        /// Sets the trace listener types for the trace observer.
        /// </summary>
        TraceListenerType TraceListenerType { get; set; }

        /// <summary>
        /// Sets the output file path for the trace observer.
        /// </summary>
        string TraceListenerFileName { get; set; }

        /// <summary>
        /// Sets the configuration options that should be threaded to the query engine.
        /// </summary>
        Dictionary<string, string> EngineOptions { get; set; }

        /// <summary>
        /// Sets the type of metadata storage that should be used.
        /// </summary>
        MetadataStorageType StorageType { get; }

        /// <summary>
        /// Sets the instance or transparent proxy to the metadata storage connection.
        /// </summary>
        IReactiveStorageConnection StorageConnection { get; }

        /// <summary>
        /// Sets the connection string to use for Azure.
        /// </summary>
        string AzureConnectionString { get; }

        /// <summary>
        /// Sets the instance or transparent proxy to the messaging connection.
        /// </summary>
        IReactiveMessagingConnection MessagingConnection { get; }

        /// <summary>
        /// Sets the instance or transparent proxy to the state store connection.
        /// </summary>
        IReactiveStateStoreConnection StateStoreConnection { get; }

        /// <summary>
        /// Sets the instance or transparent proxy to the key value store connection.
        /// </summary>
        ITransactionalKeyValueStoreConnection KeyValueStoreConnection { get; }

        /// <summary>
        /// Sets the instances or transparent proxies to the query evaluator connections.
        /// </summary>
        List<IReactiveQueryEvaluatorConnection> QueryEvaluatorConnections { get; }
    }
}
