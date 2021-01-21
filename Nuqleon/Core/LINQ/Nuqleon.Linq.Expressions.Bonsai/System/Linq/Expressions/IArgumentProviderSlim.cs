// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2016 - Created this file.
//

namespace System.Linq.Expressions
{
    /// <summary>
    /// Interface providing access to an expression node's arguments.
    /// </summary>
    public interface IArgumentProviderSlim
    {
        /// <summary>
        /// Gets the number of arguments.
        /// </summary>
        int ArgumentCount { get; }

        /// <summary>
        /// Gets the argument at the specified index.
        /// </summary>
        /// <param name="index">The index of the argument to retrieve.</param>
        /// <returns>The argument at the specified index.</returns>
        ExpressionSlim GetArgument(int index);
    }
}
