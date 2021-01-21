// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Linq;

namespace Reaqtor.Remoting.Metadata
{
    /// <summary>
    /// Provides an extension method to insert a local/remote query execution transition marker.
    /// </summary>
    internal static class QueryableExtensions
    {
        /// <summary>
        /// Marker to indicate the transition from remote query execution of the specified query to local execution of further query compositions.
        /// </summary>
        /// <typeparam name="T">Element type of the query result.</typeparam>
        /// <param name="source">Query to execute remotely but to hide from accepting further query composition.</param>
        /// <returns>Query object that hides the original query and causes further query composition to execute locally.</returns>
        public static IQueryable<T> ToLocal<T>(this IQueryable<T> source)
        {
            Contract.RequiresNotNull(source);

            return source.AsEnumerable().Select(_ => _).AsQueryable();
        }
    }
}
