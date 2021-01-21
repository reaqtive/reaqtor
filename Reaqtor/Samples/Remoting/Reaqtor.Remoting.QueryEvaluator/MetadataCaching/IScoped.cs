// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

// TODO: Relocate to IRP Core and change namespace prior.

namespace Reaqtor.Remoting.QueryEvaluator
{
    /// <summary>
    /// Interface describing a mechanism to push and pop logical scopes.
    /// </summary>
    public interface IScoped
    {
        /// <summary>
        /// Pushes a new scope.
        /// </summary>
        /// <returns>Disposable to pop the newly created scope.</returns>
        /// <remarks>The IDisposable return type allows for lexical scoping with the <c>using</c> statement.</remarks>
        IDisposable CreateScope();
    }
}
