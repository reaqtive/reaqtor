// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Reaqtor.Expressions.Core
{
    /// <summary>
    /// Signatures of the available observers to be used in global definitions (DefineObserver).
    /// This class does not contain implementations. Expressions using the observers have to be
    /// rebound to a perticular implementation (Rx or Subscribable) before they can be executed.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ReactiveQbserver
    {
        /// <summary>
        /// Returns the nop observer.
        /// </summary>
        /// <typeparam name="T">Type of the elements processed by the observer.</typeparam>
        /// <returns>An instance of the nop observer.</returns>
        public static IReactiveQbserver<T> Nop<T>()
        {
            throw new NotImplementedException();
        }
    }
}
