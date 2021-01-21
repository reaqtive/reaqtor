// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using System.Linq.Expressions;

namespace System.Linq.CompilerServices
{
    internal static class Errors
    {
        public static Exception NotSupportedByNarrowVisitor(object node)
        {
            var type = node is Expression expr ? expr.NodeType.ToString() : node.GetType().Name;

            return new NotSupportedException("Expression of type " + type + " is not supported.");
        }

        public static Exception InvalidChildNodeCount() => new InvalidOperationException("The number of children in the updated tree doesn't match the expected number.");

        public static Exception NotSupportedNode(ExpressionType type) => new NotSupportedException("Expression of type " + type + " is not supported.");
    }
}
