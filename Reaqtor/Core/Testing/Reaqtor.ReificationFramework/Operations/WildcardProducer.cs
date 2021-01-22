// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Reaqtor.ReificationFramework
{
    /// <summary>
    /// Produces wildcards, keeping track of all identifiers generated.
    /// </summary>
    public class WildcardProducer : WildcardGenerator
    {
        private readonly ConcurrentQueue<Uri> _generated;

        /// <summary>
        /// Instantiates the wildcard generator.
        /// </summary>
        public WildcardProducer()
        {
            _generated = new ConcurrentQueue<Uri>();
        }

        /// <summary>
        /// The ordered set of wildcards that have been generated.
        /// </summary>
        public IEnumerable<Uri> Wildcards => _generated;

        /// <summary>
        /// Generate a wildcard.
        /// </summary>
        /// <returns>The wildcard.</returns>
        public override Uri Generate()
        {
            var next = base.Generate();
            _generated.Enqueue(next);
            return next;
        }
    }
}
