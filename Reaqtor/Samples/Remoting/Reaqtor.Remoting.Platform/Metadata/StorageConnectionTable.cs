// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

using Reaqtor.Remoting.Protocol;

using Microsoft.Azure.Cosmos.Table;

namespace Reaqtor.Remoting.Metadata
{
    public class StorageConnectionTable : ITable
    {
        private readonly string _name;
        private readonly IReactiveStorageConnection _connection;

        public StorageConnectionTable(string name, IReactiveStorageConnection connection)
        {
            _name = name;
            _connection = connection;
        }

        public Task<bool> CreateIfNotExistsAsync(TableRequestOptions options, object state)
        {
            return Task.FromResult(true);
        }

        public Task<TableResult> ExecuteAsync(ITableOperation operation, TableRequestOptions options, object state)
        {
            return operation.Type switch
            {
                TableOperationType.Delete => Delete(operation.Entity),
                TableOperationType.Insert => Insert(operation.Entity),
                _ => throw new NotSupportedException(
                        string.Format(
                            CultureInfo.InvariantCulture,
                            "'{0}' operation is not supported; only insert or delete operations are supported.",
                            operation.Type
                        )),
            };
        }

        private Task<TableResult> Delete(ITableEntity entity)
        {
            _connection.DeleteEntity(_name, entity.RowKey);
            return Task.FromResult(new TableResult
            {
                Etag = "mock",
                HttpStatusCode = 202,
                Result = null,
            });
        }

        private Task<TableResult> Insert(ITableEntity entity)
        {
            var properties = new Dictionary<string, StorageEntityProperty>
            {
                { "ETag", new StorageEntityProperty { Type = (int)EdmType.String, Data = entity.ETag } },
                { "PartitionKey", new StorageEntityProperty { Type = (int)EdmType.String, Data = entity.PartitionKey } },
                { "RowKey", new StorageEntityProperty { Type = (int)EdmType.String, Data = entity.RowKey } },
                { "Timestamp", new StorageEntityProperty { Type = (int)EdmType.DateTime, Data = entity.Timestamp.UtcTicks.ToString() } },
            };

            foreach (var property in entity.WriteEntity(null))
            {
                switch (property.Value.PropertyType)
                {
                    case EdmType.Boolean:
                        properties.Add(property.Key, new StorageEntityProperty { Type = (int)EdmType.Boolean, Data = property.Value.BooleanValue.Value ? "true" : "false" });
                        break;
                    case EdmType.DateTime:
                        properties.Add(property.Key, new StorageEntityProperty { Type = (int)EdmType.DateTime, Data = property.Value.DateTimeOffsetValue.Value.UtcTicks.ToString() });
                        break;
                    case EdmType.Double:
                        properties.Add(property.Key, new StorageEntityProperty { Type = (int)EdmType.Double, Data = property.Value.DoubleValue.Value.ToString() });
                        break;
                    case EdmType.Guid:
                        properties.Add(property.Key, new StorageEntityProperty { Type = (int)EdmType.Guid, Data = property.Value.GuidValue.Value.ToString() });
                        break;
                    case EdmType.Int32:
                        properties.Add(property.Key, new StorageEntityProperty { Type = (int)EdmType.Int32, Data = property.Value.Int32Value.Value.ToString() });
                        break;
                    case EdmType.Int64:
                        properties.Add(property.Key, new StorageEntityProperty { Type = (int)EdmType.Int64, Data = property.Value.Int64Value.Value.ToString() });
                        break;
                    case EdmType.String:
                        properties.Add(property.Key, new StorageEntityProperty { Type = (int)EdmType.String, Data = property.Value.StringValue });
                        break;
                    default:
                        break;
                }
            }

            _connection.AddEntity(_name, entity.RowKey, new StorageEntity(properties));

            return Task.FromResult(new TableResult
            {
                Etag = "mock",
                HttpStatusCode = 201,
                Result = null
            });
        }
    }
}
