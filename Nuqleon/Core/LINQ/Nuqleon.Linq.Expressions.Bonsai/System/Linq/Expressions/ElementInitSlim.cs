// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System.Collections.ObjectModel;
using System.Reflection;

namespace System.Linq.Expressions
{
    // PERF: Consider introducing specialized layouts for common arities of 1 and 2 arguments.

    /// <summary>
    /// Lightweight representation of an element initializer.
    /// </summary>
    public sealed class ElementInitSlim : IArgumentProviderSlim
    {
        internal ElementInitSlim(MethodInfoSlim addMethod, ReadOnlyCollection<ExpressionSlim> arguments)
        {
            AddMethod = addMethod;
            Arguments = arguments;
        }

        /// <summary>
        /// Gets the add method used by the element initializer.
        /// </summary>
        public MethodInfoSlim AddMethod { get; }

        /// <summary>
        /// Gets the expressions representing the arguments passed to the add method.
        /// </summary>
        public ReadOnlyCollection<ExpressionSlim> Arguments { get; }

        /// <summary>
        /// Gets the number of arguments.
        /// </summary>
        public int ArgumentCount => Arguments.Count;

        /// <summary>
        /// Gets the argument at the specified index.
        /// </summary>
        /// <param name="index">The index of the argument to retrieve.</param>
        /// <returns>The argument at the specified index.</returns>
        public ExpressionSlim GetArgument(int index) => Arguments[index];

        /// <summary>
        /// Creates a new element initializer that is like this one, but using the supplied children. If all of the children are the same, it will return this element initializer.
        /// </summary>
        /// <param name="arguments">The <see cref="Arguments"/> child node of the result.</param>
        /// <returns>This element initializer if no children are changed or an element initializer with the updated children.</returns>
        public ElementInitSlim Update(ReadOnlyCollection<ExpressionSlim> arguments)
        {
            if (arguments == Arguments)
            {
                return this;
            }

            return new ElementInitSlim(AddMethod, arguments);
        }

        /// <summary>
        /// Gets a friendly string representation of the node.
        /// </summary>
        /// <returns>String representation of the node.</returns>
        public override string ToString()
        {
            using var sb = ExpressionSlimPrettyPrinter.StringBuilderPool.New();

            new ExpressionSlimPrettyPrinter(sb.StringBuilder).VisitElementInit(this);

            return sb.StringBuilder.ToString();
        }
    }
}
