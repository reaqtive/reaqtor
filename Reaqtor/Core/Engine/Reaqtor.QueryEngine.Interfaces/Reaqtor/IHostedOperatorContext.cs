// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Reaqtive;

namespace Reaqtor.Reactive
{
    /// <summary>
    /// This is the context provided to each reactive operator after it is instantiated.
    /// Operators must interact with the environment only through this context to 
    /// ensure that they can be properly hosted inside QueryEngines.
    /// </summary>
    public interface IHostedOperatorContext : IOperatorContext
    {
        /// <summary>
        /// The reactive service hosting the operator. Operators can use it to create new,
        /// or get a reference to an existing artifact (observable, observer, stream, subscription).
        /// </summary>
        IReactive ReactiveService { get; }
    }
}
