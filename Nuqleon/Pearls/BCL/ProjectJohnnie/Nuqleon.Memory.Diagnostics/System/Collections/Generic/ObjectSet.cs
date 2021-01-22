// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 08/09/2017 - Created this type.
//

//#define USE_HASHSET

namespace System.Collections.Generic
{
#if USE_HASHSET
    /// <summary>
    /// Set of objects.
    /// </summary>
    public class ObjectSet : HashSet<object>
    {
        /// <summary>
        /// Creates a set of objects.
        /// </summary>
        public ObjectSet()
            : base(ReferenceEqualityComparer<object>.Instance)
        {
        }

        /// <summary>
        /// Creates a set of objects.
        /// </summary>
        /// <param name="collection">The initial set.</param>
        public ObjectSet(IEnumerable<object> collection)
            : base(collection, ReferenceEqualityComparer<object>.Instance)
        {
        }
    }
#else
    /// <summary>
    /// Set of objects.
    /// </summary>
    public class ObjectSet : ObjectSet<object>
    {
        /// <summary>
        /// Creates a set of objects.
        /// </summary>
        public ObjectSet()
            : base()
        {
        }

        /// <summary>
        /// Creates a set of objects.
        /// </summary>
        /// <param name="collection">The initial set.</param>
        public ObjectSet(IEnumerable<object> collection)
            : base(collection)
        {
        }
    }
#endif
}
