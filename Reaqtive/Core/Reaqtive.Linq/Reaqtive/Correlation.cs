// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace Reaqtive
{
    /// <summary>
    /// Correlation tracker. Create one instance per operator type to avoid lock contention.
    /// </summary>
    internal class Correlation
    {
        /// <summary>
        /// Number of buckets, tuned to reduce the likelihood of hitting the same bucket from different threads,
        /// which would cause lock contention inside the ConditionalWeakTable implementation.
        /// </summary>
        private static readonly int BUCKETCOUNT = Environment.ProcessorCount * 2;

        /// <summary>
        /// Pay-for-play logs that put correlation identifiers on objects.
        /// </summary>
        private readonly ConcurrentDictionary<int, ConditionalWeakTable<object, object>> _payForPlayLogs = new();

        /// <summary>
        /// Starts tracking an object. A tracked object will not prevent it from being collected.
        /// </summary>
        /// <param name="o">Object to start tracking.</param>
        public void BeginTracking(object o)
        {
            var table = GetTable(o);
            table.Add(o, Guid.NewGuid());
        }

        /// <summary>
        /// Stops tracking an object.
        /// </summary>
        /// <param name="o">Object to stop tracking.</param>
        public void EndTracking(object o)
        {
            var table = GetTable(o);
            table.Remove(o);
        }

        /// <summary>
        /// Gets a string representation of the specified object, including a correlation identifier (if any exists).
        /// </summary>
        /// <param name="o">Object to obtain a string representation for.</param>
        /// <returns>String representation of the specified object, including a correlation identifier (if any exists).</returns>
        public string ToTraceString(object o)
        {
            var res = o == null ? "" : o.ToString();

            var table = GetTable(o);

            if (table.TryGetValue(o, out object track))
            {
                res += " [" + track + "]";
            }

            return res;
        }

        /// <summary>
        /// Gets the table where tracking information for the specified object is kept.
        /// </summary>
        /// <param name="o">Object to get a table for.</param>
        /// <returns>Table to store tracking information for the specified object in.</returns>
        private ConditionalWeakTable<object, object> GetTable(object o)
        {
            var id = RuntimeHelpers.GetHashCode(o) % BUCKETCOUNT;
            return _payForPlayLogs.GetOrAdd(id, _ => new ConditionalWeakTable<object, object>());
        }
    }
}
