// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Linq.Expressions;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Reader for transaction log entries from a stream.
    /// </summary>
    internal sealed class TransactionLogOperationReader : ReaderBase
    {
        /// <summary>
        /// Creates a <see cref="TransactionLogOperationReader"/> to read operations from the specified <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The stream to load from.</param>
        /// <param name="policy">The serialization policy to use when serializing objects.</param>
        public TransactionLogOperationReader(Stream stream, ISerializationPolicy policy)
            : base(stream, policy)
        {
        }

        /// <summary>
        /// Loads the artifact operation from the underlying stream.
        /// </summary>
        /// <returns>The loaded artifact instance.</returns>
        public ArtifactOperation Load()
        {
            var major = _serializer.Deserialize<int>(_stream);
            var minor = _serializer.Deserialize<int>(_stream);
            var build = _serializer.Deserialize<int>(_stream);
            var revision = _serializer.Deserialize<int>(_stream);

            var version = new Version(major, minor, build, revision);
            if (version < TransactionStateVersion.v1)
            {
                throw new InvalidDataException("Version not supported: " + version);
            }

            var kind = _serializer.Deserialize<ArtifactOperationKind>(_stream);

            switch (kind)
            {
                case ArtifactOperationKind.Create:
                    {
                        var expr = _serializer.Deserialize<Expression>(_stream);
                        var state = _serializer.Deserialize<object>(_stream);
                        return ArtifactOperation.Create(expr, state);
                    }
                case ArtifactOperationKind.DeleteCreate:
                    {
                        var expr = _serializer.Deserialize<Expression>(_stream);
                        var state = _serializer.Deserialize<object>(_stream);
                        return ArtifactOperation.DeleteCreate(expr, state);
                    }
                case ArtifactOperationKind.Delete:
                    return ArtifactOperation.Delete();
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
