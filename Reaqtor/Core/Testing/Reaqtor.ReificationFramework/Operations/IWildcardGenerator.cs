// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.ReificationFramework
{
    /// <summary>
    /// Interface to generate wildcards.
    /// </summary>
    public interface IWildcardGenerator
    {
        /// <summary>
        /// Generates a wildcard.
        /// </summary>
        /// <returns>Uri representing the wildcard.</returns>
        Uri Generate();
    }
}
