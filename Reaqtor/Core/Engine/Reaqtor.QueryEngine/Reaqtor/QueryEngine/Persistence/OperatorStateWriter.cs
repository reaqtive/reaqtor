// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.IO;

using Reaqtive;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Factory for state writers with a specified serialization policy.
    /// </summary>
    internal sealed class OperatorStateWriterFactory : WriterBase, IOperatorStateWriterFactory
    {
        /// <summary>
        /// Creates a state writer factory to write to the specified underlying <paramref name="stream"/>, using the
        /// specified serialization <paramref name="policy"/>.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="policy">The serialization policy used to serialize values.</param>
        public OperatorStateWriterFactory(Stream stream, ISerializationPolicy policy)
            : base(stream, policy)
        {
        }

        /// <summary>
        /// Creates a new operator state writer for the specified operator.
        /// </summary>
        /// <param name="node">Operator whose state will be written by the created reader.</param>
        /// <returns>Operator state writer for the specified operator.</returns>
        public IOperatorStateWriter Create(IStatefulOperator node)
        {
            Debug.Assert(_serializer != null, "Did you forget to call WriteHeader?");

            return new OperatorStateWriter(this, _serializer, _stream);
        }

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA1001 // Has disposable field but isn't IDsposable. (By design; the writer doesn't own any of these.)
#pragma warning disable CA2213 // Has disposable field that doesn't get disposed. (See above.)

        internal sealed class OperatorStateWriter : IOperatorStateWriter
        {
            private readonly IOperatorStateWriterFactory _factory;
            private readonly ISerializer _serializer;
            private readonly Stream _stream;
            private readonly long _begin;
            private static readonly byte[] s_zeroLength = BitConverter.GetBytes(0L);

            public OperatorStateWriter(IOperatorStateWriterFactory factory, ISerializer serializer, Stream stream)
            {
                Debug.Assert(factory != null);
                Debug.Assert(serializer != null);
                Debug.Assert(stream != null);

                _factory = factory;
                _serializer = serializer;
                _stream = stream;

                _stream.Write(s_zeroLength, 0, s_zeroLength.Length);
                _begin = _stream.Position;
            }

            public void Write<T>(T value) => _serializer.Serialize(value, _stream);

            public IOperatorStateWriter CreateChild() => _factory.Create(node: null);

            public void Dispose()
            {
                var end = _stream.Position;

                var length = end - _begin;
                var lenBytes = BitConverter.GetBytes(length);

                _stream.Position = _begin - lenBytes.Length;
                _stream.Write(lenBytes, 0, lenBytes.Length);

                _stream.Position = end;
            }
        }
    }
}
