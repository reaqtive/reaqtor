// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Linq.Expressions;

namespace Reaqtive.Expressions
{
    /// <summary>
    /// Represents a multi-subject wih an expression tree representation.
    /// </summary>
    /// <typeparam name="T">Type of the elements processed by the subject.</typeparam>
    public interface IMultiQubject<T>
    {
        /// <summary>
        /// Gets the expression representing the subscribable resource.
        /// </summary>
        Expression Expression
        {
            get;
        }
    }
}
