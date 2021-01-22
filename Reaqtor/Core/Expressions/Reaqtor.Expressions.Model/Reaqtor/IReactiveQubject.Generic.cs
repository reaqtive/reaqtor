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
    /// Interface for a subject represented by an expression tree.
    /// </summary>
    /// <typeparam name="T">Type of the data received and produced by the subject.</typeparam>
    public interface IReactiveQubject<T> : IReactiveQubject<T, T>
    {
    }

    /// <summary>
    /// Interface for a subject represented by an expression tree.
    /// </summary>
    /// <typeparam name="TInput">Type of the data received by the subject.</typeparam>
    /// <typeparam name="TOutput">Type of the data produced by the subject.</typeparam>
    public interface IReactiveQubject<in TInput, out TOutput> : IReactiveSubject<TInput, TOutput>, IReactiveQbserver<TInput>, IReactiveQbservable<TOutput>, IReactiveQubject
    {
    }
}
