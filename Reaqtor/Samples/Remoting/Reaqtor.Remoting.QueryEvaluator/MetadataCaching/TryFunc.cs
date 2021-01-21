// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

// TODO: Relocate to IRP Core and change namespace.

namespace Reaqtor.Remoting.QueryEvaluator
{
    /// <summary>
    /// Delegate for a single-parameter function exposing the Try pattern.
    /// </summary>
    /// <typeparam name="T">Type of the argument passed to the function.</typeparam>
    /// <typeparam name="TResult">Type of the result returned by the function upon successful invocation.</typeparam>
    /// <param name="argument">Argument passed to the function.</param>
    /// <param name="result">Result returned by the function upon successful invocation.</param>
    /// <returns>true if the invocation was successful, reporting the result in the <paramref name="result"/> parameter; otherwise, false.</returns>
    internal delegate bool TryFunc<T, TResult>(T argument, out TResult result);
}
