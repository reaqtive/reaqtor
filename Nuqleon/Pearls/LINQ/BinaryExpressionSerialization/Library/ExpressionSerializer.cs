// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2014 - Created this file.
//

using System.IO;
using System.Memory;

namespace System.Linq.Expressions.Bonsai.Serialization.Binary
{
#if USE_SLIM
    using Expression = System.Linq.Expressions.ExpressionSlim;
    using ExpressionFactory = System.Linq.Expressions.ExpressionSlimFactory;
    using IExpressionFactory = System.Linq.Expressions.IExpressionSlimFactory;
#endif

    /// <summary>
    /// Binary expression serializer.
    /// </summary>
    public sealed partial class ExpressionSerializer
    {
        /// <summary>
        /// Factory for expression trees used during deserialization.
        /// </summary>
        private readonly IExpressionFactory _factory;

        /// <summary>
        /// Object serializer used during serialization and deserialization.
        /// </summary>
        private readonly IObjectSerializer _serializer;

        /// <summary>
        /// Object pool for serialization context objects.
        /// </summary>
        private readonly ObjectPool<SerializationContext> _serializerContexts;

        /// <summary>
        /// Object pool for deserialization context objects.
        /// </summary>
        private readonly ObjectPool<DeserializationContext> _deserializerContexts;

        /// <summary>
        /// Creates a new expression serializer using the specified object serializer.
        /// </summary>
        /// <param name="serializer">Object serializer used to serialize constant expression node values.</param>
        public ExpressionSerializer(IObjectSerializer serializer)
            : this(serializer, ExpressionFactory.Instance)
        {
        }

        /// <summary>
        /// Creates a new expression serializer using the specified object serializer and expression factory.
        /// </summary>
        /// <param name="serializer">Object serializer used to serialize constant expression node values.</param>
        /// <param name="expressionFactory">Expression factory used to construct expression nodes during deserialization.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="serializer"/> or <paramref name="expressionFactory"/> is null.</exception>
        public ExpressionSerializer(IObjectSerializer serializer, IExpressionFactory expressionFactory)
        {
            _factory = expressionFactory ?? throw new ArgumentNullException(nameof(expressionFactory));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            _serializerContexts = new ObjectPool<SerializationContext>(() => new SerializationContext(this));
            _deserializerContexts = new ObjectPool<DeserializationContext>(() => new DeserializationContext(this));
        }

        /// <summary>
        /// Serializes the specified expression to a byte array.
        /// </summary>
        /// <param name="expression">Expression (or a null reference) to serialize.</param>
        /// <returns>Byte array containing the serialized expression.</returns>
        public byte[] Serialize(Expression expression)
        {
            using var ms = new MemoryStream();

            Serialize(ms, expression);
            return ms.ToArray();
        }

        /// <summary>
        /// Serializes the specified expression to the stream.
        /// </summary>
        /// <param name="stream">Stream to write the serialized expression to.</param>
        /// <param name="expression">Expression (or a null reference) to serialize.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="stream"/> is null.</exception>
        public void Serialize(Stream stream, Expression expression)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            WriteHeader(stream);
            WriteVersion(stream, 1, 0, 0, 0);

            var contextPointerPosition = stream.Position;
            stream.Position += 4;

            using var pooledContext = _serializerContexts.New();

            var context = pooledContext.Object;

            var serializer = new Serializer(stream, context);
            serializer.Visit(expression);

            var contextPosition = stream.Position;
            context.Serialize(stream);

            stream.Position = contextPointerPosition;
            stream.WriteInt32(checked((int)contextPosition));
        }

        /// <summary>
        /// Deserializes an expression from a byte array.
        /// </summary>
        /// <param name="data">Byte array to deserialize an expression from.</param>
        /// <returns>Deserialized expression, or a null reference.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="data"/> is null.</exception>
        public Expression Deserialize(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            using var ms = new MemoryStream(data);

            return Deserialize(ms);
        }

        /// <summary>
        /// Deserializes an expression from a stream.
        /// </summary>
        /// <param name="stream">Stream to deserialize an expression from.</param>
        /// <returns>Deserialized expression, or a null reference.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="stream"/> is null.</exception>
        public Expression Deserialize(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            ReadHeader(stream);
            ReadVersion(stream, out byte maj, out byte min, out _, out _);

            if (maj != 1 || min != 0)
            {
                throw new NotSupportedException(); // TODO
            }

            var contextPosition = stream.ReadInt32();

            var expressionPayloadPosition = stream.Position;

            stream.Position = contextPosition;

            using var pooledContext = _deserializerContexts.New();

            var context = pooledContext.Object;

            context.Deserialize(stream);

            stream.Position = expressionPayloadPosition;

            var deserializer = new Deserializer(this, stream, context);
            return deserializer.Deserialize();
        }

        /// <summary>
        /// Writes the magic header to the stream.
        /// </summary>
        /// <param name="stream">Stream to write the magic header to.</param>
        private static void WriteHeader(Stream stream)
        {
            stream.WriteByte((byte)'E'); // expression
            stream.WriteByte((byte)'B'); // binary
        }

        /// <summary>
        /// Reads the magic header from the stream.
        /// </summary>
        /// <param name="stream">Stream to read the magic header from.</param>
        /// <exception cref="InvalidDataException">Thrown if the magic header was not found.</exception>
        private static void ReadHeader(Stream stream)
        {
            var e = stream.ReadByte(); // expression
            var b = stream.ReadByte(); // binary

            if (e != 'E' || b != 'B')
            {
                throw new InvalidDataException("Stream does not start with the expected magic header.");
            }
        }

        /// <summary>
        /// Writes the specified version number to the stream.
        /// </summary>
        /// <param name="stream">Stream to write the version number to.</param>
        /// <param name="major">Major component of the version number.</param>
        /// <param name="minor">Minor component of the version number.</param>
        /// <param name="build">Build number component of the version number.</param>
        /// <param name="revision">Revision number component of the version number.</param>
        private static void WriteVersion(Stream stream, byte major, byte minor, byte build, byte revision)
        {
            stream.WriteByte(major);
            stream.WriteByte(minor);
            stream.WriteByte(build);
            stream.WriteByte(revision);
        }

        /// <summary>
        /// Reads a version number from the stream.
        /// </summary>
        /// <param name="stream">Stream to read the version number from.</param>
        /// <param name="major">Major component of the version number.</param>
        /// <param name="minor">Minor component of the version number.</param>
        /// <param name="build">Build number component of the version number.</param>
        /// <param name="revision">Revision number component of the version number.</param>
        private static void ReadVersion(Stream stream, out byte major, out byte minor, out byte build, out byte revision)
        {
            var maj = stream.ReadByte();
            var min = stream.ReadByte();
            var bld = stream.ReadByte();
            var rev = stream.ReadByte();

            if (rev < 0)
            {
                throw new EndOfStreamException("Expected 4 bytes for version number.");
            }

            major = (byte)maj;
            minor = (byte)min;
            build = (byte)bld;
            revision = (byte)rev;
        }
    }
}
