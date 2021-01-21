// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Reader for transaction log entries to a stream.
    /// </summary>
    internal sealed class TransactionLogOperationWriter : WriterBase
    {
        /// <summary>
        /// Creates a <see cref="TransactionLogOperationWriter"/> to save operations to the specified <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The stream to save to.</param>
        /// <param name="policy">The serialization policy to use when serializing objects.</param>
        public TransactionLogOperationWriter(Stream stream, ISerializationPolicy policy)
            : base(stream, policy)
        {
        }

        /// <summary>
        /// Saves the artifact operation to the underlying stream.
        /// </summary>
        /// <param name="operation">The artifact instance to save.</param>
        public void Save(ArtifactOperation operation)
        {
            var version = TransactionStateVersion.v1;
            _serializer.Serialize(version.Major, _stream);
            _serializer.Serialize(version.Minor, _stream);
            _serializer.Serialize(version.Build, _stream);
            _serializer.Serialize(version.Revision, _stream);

            var kind = operation.OperationKind;

            _serializer.Serialize(kind, _stream);

            switch (kind)
            {
                case ArtifactOperationKind.Create:
                    {
                        _serializer.Serialize(operation.Expression, _stream);
                        _serializer.Serialize(operation.State, _stream);
                        break;
                    }
                case ArtifactOperationKind.DeleteCreate:
                    {
                        _serializer.Serialize(operation.Expression, _stream);
                        _serializer.Serialize(operation.State, _stream);
                        break;
                    }
                case ArtifactOperationKind.Delete:
                    break;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
