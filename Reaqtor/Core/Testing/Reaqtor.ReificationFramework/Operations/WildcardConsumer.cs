// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;

namespace Reaqtor.ReificationFramework
{
    /// <summary>
    /// Produces wildcards from a ring buffer.
    /// </summary>
    public class WildcardConsumer : IWildcardGenerator
    {
        private readonly Uri[] _wildcards;
        private long _index;

        /// <summary>
        /// Instantiates the wildcard generator.
        /// </summary>
        /// <param name="wildcards">The set of wildcards to use.</param>
        public WildcardConsumer(params Uri[] wildcards)
        {
            if (wildcards == null)
            {
                throw new ArgumentNullException(nameof(wildcards));
            }
            else if (wildcards.Length == 0)
            {
                throw new ArgumentException("Expected at least one element.", nameof(wildcards));
            }

            _wildcards = wildcards;
        }

        /// <summary>
        /// Gets the next wildcard from the buffer.
        /// </summary>
        /// <returns>The wildcard.</returns>
        public Uri Generate()
        {
            _ = Interlocked.Increment(ref _index);
            return _wildcards[(_index - 1) % _wildcards.Length];
        }
    }
}
