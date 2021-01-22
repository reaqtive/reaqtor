// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - August 2010 - Created this file.
//

using System;
using System.Linq;
using System.Text;

namespace Nuqleon.Json.Expressions
{
    /// <summary>
    /// Pretty printer for JSON expression trees.
    /// </summary>
    /// <remarks>Recommended for debugging purposes.</remarks>
    public sealed class PrettyPrinter : ExpressionVisitor<string>
    {
        private static readonly char[] s_CrLf = new[] { '\r', '\n' };

        private readonly string _indent;

        /// <summary>
        /// Creates a new pretty printer with indentation of two spaces.
        /// </summary>
        public PrettyPrinter()
            : this(2, ' ')
        {
        }

        /// <summary>
        /// Creates a new pretty printer with the specified indentation width and character.
        /// </summary>
        /// <param name="indentationWidth">Indentation width.</param>
        /// <param name="indentationCharacter">Indentation character.</param>
        public PrettyPrinter(int indentationWidth, char indentationCharacter)
        {
            _indent = new string(indentationCharacter, indentationWidth);
        }

        /// <summary>
        /// Visits a JSON expression tree object node to produce a pretty printing string.
        /// </summary>
        /// <param name="node">JSON expression tree object node to visit.</param>
        /// <returns>Pretty printing string representation of the object node.</returns>
        public override string VisitObject(ObjectExpression node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            // PERF: This is currently only used for debugging purposes, but we can improve this a lot.

            var sb = new StringBuilder();
            sb.AppendLine("{");
            foreach (var line in string.Join(",\n", node.Members.Select(value => value.Key.ToJsonString() + ": " + Visit(value.Value).TrimStart(' '))).Split('\n'))
            {
                sb.Append(_indent);
                sb.AppendLine(line.TrimEnd(s_CrLf));
            }
            sb.Append('}');
            return sb.ToString();
        }

        /// <summary>
        /// Visits a JSON expression tree array node to produce a pretty printing string.
        /// </summary>
        /// <param name="node">JSON expression tree array node to visit.</param>
        /// <returns>Pretty printing string representation of the array node.</returns>
        public override string VisitArray(ArrayExpression node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            // PERF: This is currently only used for debugging purposes, but we can improve this a lot.

            var sb = new StringBuilder();
            sb.AppendLine("[");
            foreach (var line in string.Join(",\n", Enumerable.Range(0, node.ElementCount).Select(i => Visit(node.GetElement(i)))).Split('\n'))
            {
                sb.Append(_indent);
                sb.AppendLine(line.TrimEnd(s_CrLf));
            }
            sb.Append(']');
            return sb.ToString();
        }

        /// <summary>
        /// Visits a JSON expression tree constant node to produce a pretty printing string.
        /// </summary>
        /// <param name="node">JSON expression tree constant node to visit.</param>
        /// <returns>Pretty printing string representation of the constant node.</returns>
        public override string VisitConstant(ConstantExpression node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            return node.ToString();
        }
    }
}
