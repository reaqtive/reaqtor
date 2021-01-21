// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtor.Reliable.Expressions;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Represents a service resolver used to locate a reactive service hosting artifacts with the specified URIs.
    /// </summary>
    public interface IReactiveServiceResolver
    {
        /// <summary>
        /// Tries to resolve the IReactive instance providing access to the artifact with the specified URI.
        /// </summary>
        /// <param name="uri">URI of the artifact to resolve.</param>
        /// <param name="service">Service containing the artifact. This parameter is set to a default value if the artifact was not resolved.</param>
        /// <returns><c>true</c> if the artifact was located on the returned <paramref name="service"/>; otherwise, <c>false</c>.</returns>
        bool TryResolve(Uri uri, out IReactive service);

        /// <summary>
        /// Tries to resolve the IReactiveProxy instance providing access to the artifact with the specified URI.
        /// </summary>
        /// <param name="uri">URI of the artifact to resolve.</param>
        /// <param name="service">Service containing the artifact. This parameter is set to a default value if the artifact was not resolved.</param>
        /// <returns><c>true</c> if the artifact was located on the returned <paramref name="service"/>; otherwise, <c>false</c>.</returns>
        bool TryResolve(Uri uri, out IReactiveProxy service);

        /// <summary>
        /// Tries to resolve the IReliableReactive instance providing access to the artifact with the specified URI.
        /// </summary>
        /// <param name="uri">URI of the artifact to resolve.</param>
        /// <param name="service">Service containing the artifact. This parameter is set to a default value if the artifact was not resolved.</param>
        /// <returns><c>true</c> if the artifact was located on the returned <paramref name="service"/>; otherwise, <c>false</c>.</returns>
        bool TryResolveReliable(Uri uri, out IReliableReactive service);
    }
}
