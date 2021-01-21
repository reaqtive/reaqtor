// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.ReificationFramework
{
    /// <summary>
    /// Generates unique wildcards.
    /// </summary>
    public class WildcardGenerator : IWildcardGenerator
    {
        /// <summary>
        /// Instantiates the wildcard generator.
        /// </summary>
        protected WildcardGenerator() { }

        /// <summary>
        /// The default instance.
        /// </summary>
        public static WildcardGenerator Instance { get; } = new WildcardGenerator();

        /// <summary>
        /// Generates a unique wildcard.
        /// </summary>
        /// <returns>The wildcard.</returns>
        public virtual Uri Generate()
        {
            var next = ReificationConstants.GetWildcard();
            return next;
        }
    }
}
