// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Microsoft.Azure.Cosmos.Table;

namespace Reaqtor.Remoting.Metadata
{
    /// <summary>
    /// Wrapper of Azure CloudTableClient to implement ITableClient interface.
    /// </summary>
    public class AzureTableClient : ITableClient
    {
        /// <summary>
        /// The Azure CloudTableClient.
        /// </summary>
        private readonly CloudTableClient _client;

        /// <summary>
        /// Instantiates Azure CloudTableClient wrapper.
        /// </summary>
        /// <param name="client">The Azure CloudTableClient.</param>
        public AzureTableClient(CloudTableClient client)
        {
            Contract.RequiresNotNull(client);

            _client = client;
        }

        /// <summary>
        /// Gets a storage table by name.
        /// </summary>
        /// <param name="tableName">The name of the table to retrieve.</param>
        /// <returns>The table.</returns>
        public ITable GetTableReference(string tableName)
        {
            Contract.RequiresNotNullOrEmpty(tableName);

            return new AzureTable(_client.GetTableReference(tableName));
        }

        /// <summary>
        /// Gets a service context for the table.
        /// </summary>
        /// <returns>The service context for the table.</returns>
        public ITableServiceContext GetTableServiceContext()
        {
            // NB: Removed after switch to Cosmos.Table. This used to leverage TableServiceContext.
            //return new AzureTableServiceContext(new TableServiceContext(_client));
            throw new NotSupportedException();
        }
    }
}
