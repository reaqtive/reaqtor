// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// PS - December 2014 - Created this file.
//

using System.Reflection;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Lightweight representation of a target of a <see cref="LabelTargetSlim" />.
    /// </summary>
    public sealed class LabelTargetSlim
    {
        internal LabelTargetSlim(TypeSlim type, string name)
        {
            Type = type;
            Name = name;
        }

        /// <summary>
        /// Gets the name of the label.
        /// </summary>
        /// <remarks>The label's name is provided for information purposes only.</remarks>
        public string Name { get; }

        /// <summary>
        /// The type of value that is passed when jumping to the label (or System.Void if no value should be passed).
        /// </summary>
        public TypeSlim Type { get; }

        /// <summary>
        /// Gets a friendly string representation of the node.
        /// </summary>
        /// <returns>String representation of the node.</returns>
        public override string ToString()
        {
            using var sb = ExpressionSlimPrettyPrinter.StringBuilderPool.New();

            new ExpressionSlimPrettyPrinter(sb.StringBuilder).VisitLabelTarget(this);

            return sb.StringBuilder.ToString();
        }
    }
}
