// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Runtime.Serialization;
using Json = Nuqleon.Json.Expressions;

namespace System.Linq.Expressions.Bonsai.Serialization
{
    /// <summary>
    /// Exception describing an error while parsing a Bonsai expression.
    /// </summary>
    public partial class BonsaiParseException
    {
        /// <summary>
        /// Creates a new Bonsai parsing exception.
        /// </summary>
        public BonsaiParseException()
        {
        }

        /// <summary>
        /// Creates a new Bonsai parsing exception.
        /// </summary>
        /// <param name="message">The message describing the parsing error.</param>
        public BonsaiParseException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates a new Bonsai parsing exception.
        /// </summary>
        /// <param name="message">The message describing the parsing error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public BonsaiParseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Creates a new Bonsai parsing exception.
        /// </summary>
        /// <param name="message">The message describing the parsing error.</param>
        /// <param name="node">The node where the parsing failure occurred.</param>
        public BonsaiParseException(string message, Json.Expression node)
            : base(message)
        {
            Node = node;
        }

        /// <summary>
        /// Creates a new Bonsai parsing exception.
        /// </summary>
        /// <param name="message">The message describing the parsing error.</param>
        /// <param name="node">The node where the parsing failure occurred.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public BonsaiParseException(string message, Json.Expression node, Exception innerException)
            : base(message, innerException)
        {
            Node = node;
        }

        /// <summary>
        /// Gets the node where the parsing failure occurred.
        /// </summary>
        public Json.Expression Node { get; }
    }

#if !NO_SERIALIZATION
    [Serializable]
    public partial class BonsaiParseException : Exception, ISerializable
    {
        /// <summary>
        /// Creates a new Bonsai parsing exception during deserialization.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Serialization streaming context.</param>
        protected BonsaiParseException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Node = Json.Expression.Parse(info.GetString("Node"), ensureTopLevelObjectOrArray: false);
        }

        /// <summary>
        /// Gets object data during serialization.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Serialization streaming context.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            // PERF: ToString is a heavy allocator. We assume exception serialization is rare enough
            //       so we don't have to worry about this too much. If it turns out this is not the
            //       case, we can consider passing in a pooled StringBuilder instance.

            info.AddValue("Node", Node.ToString());
        }
    }
#else
    public partial class BonsaiParseException : Exception
    {
    }
#endif
}
