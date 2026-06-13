// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - November 2013 - Created this file.
//

namespace Reaqtor.Remoting.Platform
{
    public class TcpReactivePlatformSettings : ITcpReactivePlatformSettings
    {
        /// <summary>
        /// Gets the host domain or IP address for the Reactive platform.
        /// </summary>
        public virtual string Host => Helpers.Constants.Host;

        /// <summary>
        /// Gets the port number for the query coordinator.
        /// </summary>
        public virtual int QueryCoordinatorPort => Helpers.Constants.QueryCoordinatorPort;

        /// <summary>
        /// Gets the path for the query coordinator.
        /// </summary>
        public virtual string QueryCoordinatorUri => Helpers.Constants.QueryCoordinatorUri;

        /// <summary>
        /// Gets the port number for the metadata service.
        /// </summary>
        public virtual int MetadataPort => Helpers.Constants.MetadataPort;

        /// <summary>
        /// Gets the path for the metadata service.
        /// </summary>
        public virtual string MetadataUri => Helpers.Constants.MetadataUri;

        /// <summary>
        /// Gets the port number for the query evaluator.
        /// </summary>
        public virtual int QueryEvaluatorPort => Helpers.Constants.QueryEvaluatorPort;

        /// <summary>
        /// Gets the path for the query evaluator.
        /// </summary>
        public virtual string QueryEvaluatorUri => Helpers.Constants.QueryEvaluatorUri;

        /// <summary>
        /// Gets the port number for the messaging service.
        /// </summary>
        public virtual int MessagingPort => Helpers.Constants.MessagingPort;

        /// <summary>
        /// Gets the path for the messaging service.
        /// </summary>
        public virtual string MessagingUri => Helpers.Constants.MessagingUri;

        /// <summary>
        /// Gets the port number for the state store service.
        /// </summary>
        public virtual int StateStorePort => Helpers.Constants.StateStorePort;

        /// <summary>
        /// Gets the path for the state store service.
        /// </summary>
        public virtual string StateStoreUri => Helpers.Constants.StateStoreUri;

        public int KeyValueStorePort => Helpers.Constants.KeyValueStorePort;

        public string KeyValueStoreUri => Helpers.Constants.KeyValueStoreUri;

        public virtual int PlaybackStorePort => Helpers.Constants.PlaybackStorePort;

        public virtual string PlaybackStoreUri => Helpers.Constants.PlaybackStoreUri;

        /// <summary>
        /// Gets the executable path for a given component.
        /// </summary>
        /// <param name="component">
        /// The name of the component, e.g., MessagingHost.
        /// </param>
        /// <returns>The path to an executable for that component.</returns>
        /// <remarks>
        /// The components should be named as follows: QueryCoordinatorHost,
        /// QueryEvaluatorHost, MetadataHost, MessagingHost, StateStoreHost.
        /// </remarks>
        public virtual string GetExecutablePath(string component)
        {
            return Helpers.GetExecutablePath(component);
        }
    }
}
