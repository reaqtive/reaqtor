// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

namespace Reaqtor
{
    /// <summary>
    /// Interface for an observer represented by an expression tree.
    /// </summary>
    /// <typeparam name="T">Type of the data received by the observer.</typeparam>
    public interface IReactiveQbserver<in T> : IReactiveObserver<T>, IReactiveQbserver
    {
    }
}
