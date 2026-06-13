// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2012
//

namespace Nuqleon.Serialization
{
    /// <summary>
    /// Object tagged with a rule name.
    /// </summary>
    /// <typeparam name="T">Type of the object to tag.</typeparam>
    public sealed class TaggedByRule<T>
    {
        /// <summary>
        /// Gets or sets the name of the rule.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the object to tag.
        /// </summary>
        public T Value { get; set; }
    }
}
