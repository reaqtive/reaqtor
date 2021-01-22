// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Linq.Expressions;

namespace Reaqtive.Expressions
{
    /// <summary>
    /// Represents a subscribable source with an expression tree representation.
    /// </summary>
    /// <typeparam name="T">Type of the elements produced by the subscribable source.</typeparam>
    public interface IQubscribable<out T> : ISubscribable<T>
    {
        /// <summary>
        /// Gets the expression representing the subscribable resource.
        /// </summary>
        Expression Expression
        {
            get;
        }

        //
        // TODO: Complete the IQ* space as a boundary condition with classic Rx's IQbservable<T> (for standalone usage).
        //
        //IQubscription Subscribe(IQbserver<T> observer);
    }
}
