// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// IG - 2025/12 - Remove CLR serialization support.
//

using Json = Nuqleon.Json.Expressions;

namespace System.Linq.Expressions.Bonsai.Serialization
{
    /// <summary>
    /// Exception describing an error while parsing a Bonsai expression.
    /// </summary>
    public class BonsaiParseException : Exception
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
}
