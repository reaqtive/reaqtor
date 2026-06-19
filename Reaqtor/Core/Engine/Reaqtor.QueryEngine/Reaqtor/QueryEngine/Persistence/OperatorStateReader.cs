// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;

using Reaqtive;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Factory for state readers with a specified serialization policy.
    /// </summary>
    internal sealed class OperatorStateReaderFactory : ReaderBase, IOperatorStateReaderFactory
    {
        /// <summary>
        /// Creates a state reader factory to read from the specified underlying <paramref name="stream"/>, using the
        /// specified serialization <paramref name="policy"/>.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="policy">The serialization policy used to deserialize values.</param>
        public OperatorStateReaderFactory(Stream stream, ISerializationPolicy policy)
            : base(stream, policy)
        {
        }

        /// <summary>
        /// Creates a new operator state reader for the specified operator.
        /// </summary>
        /// <param name="node">Operator whose state will be read by the created reader.</param>
        /// <returns>Operator state reader for the specified operator.</returns>
        public IOperatorStateReader Create(IStatefulOperator node)
        {
            Debug.Assert(_serializer != null, "Did you forget to call ReadHeader?");

            return new OperatorStateReader(this, _serializer, _stream, node);
        }

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA1001 // Has disposable field but isn't IDsposable. (By design; the reader doesn't own any of these.)
#pragma warning disable CA2213 // Has disposable field that doesn't get disposed. (See above.)

        internal sealed class OperatorStateReader : IOperatorStateReader
        {
            private readonly IOperatorStateReaderFactory _factory;
            private readonly ISerializer _serializer;
            private readonly Stream _stream;
            private readonly IStatefulOperator _operator;

            private readonly Stream _underlyingStream;
            private readonly long _originalPosition;

            private long _end;

            public OperatorStateReader(IOperatorStateReaderFactory factory, ISerializer serializer, Stream stream, IStatefulOperator @operator)
            {
                Debug.Assert(factory != null);
                Debug.Assert(serializer != null);
                Debug.Assert(stream != null);

                _factory = factory;
                _serializer = serializer;
                _operator = @operator;

                _underlyingStream = stream;
                _originalPosition = stream.Position;
                _end = _originalPosition;

                var lenBytes = new byte[8];
                var readCount = stream.Read(lenBytes, 0, 8);

                if (readCount != 8 && @operator is not ITransitioningOperator)
                {
                    var blob = stream.GetBase64Blob();
                    throw new InvalidDataException(string.Format(CultureInfo.InvariantCulture, "Missing length prefix. Operator = {0}/{1}, Position = {2}, State = {3}", _operator.Name, _operator.Version, _originalPosition, blob));
                }
                else if (readCount == 8)
                {
                    var length = BitConverter.ToInt64(lenBytes, 0);
                    _stream = new StreamSegment(stream, stream.Position, length);
                    _end = stream.Position + length;
                }
            }

            public T Read<T>() => _serializer.Deserialize<T>(_stream);

            public bool TryRead<T>(out T value)
            {
                if (_stream != null)
                {
                    value = Read<T>();
                    return true;
                }

                value = default;
                return false;
            }

            public void Reset()
            {
                _end = _originalPosition;
            }

            public IOperatorStateReader CreateChild() => _factory.Create(node: null);

            public void Dispose()
            {
                _underlyingStream.Position = _end;
            }
        }
    }
}
