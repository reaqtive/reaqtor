// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
// Simplified registry for known resources.
//
// BD - September 2014
//

using System.Collections.Generic;
using System.Linq.Expressions;

namespace Pearls.Reaqtor.CSE
{
    /// <summary>
    /// Registry for known resources.
    /// </summary>
    internal class Registry : Dictionary<string, Entry>
    {
    }

    /// <summary>
    /// Entry representing a known resource in the registry.
    /// </summary>
    internal struct Entry
    {
        /// <summary>
        /// Gets a Boolean indicating whether the resource can be shared without duplication of side-effects.
        /// </summary>
        public bool CanShare; // omitted fine-grained classification (pure, stateless, hot)

        /// <summary>
        /// Gets a Boolean indicating whether the resource is a subject.
        /// </summary>
        public bool IsSubject; // omitted partitioning of registry

        /// <summary>
        /// Gets the expression representation of the known resource.
        /// </summary>
        public Expression Expression;
    }
}
