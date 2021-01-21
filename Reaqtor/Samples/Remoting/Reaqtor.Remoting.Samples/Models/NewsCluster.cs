// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.Samples.Models
{
    using System.Collections.Generic;
    using Nuqleon.DataModel;

    /// <summary>
    /// News object to describe result-type 'Cluster'.
    /// </summary>
    public class NewsCluster
    {
        /// <summary>
        /// A list of articles belonging to the same cluster.
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/news/cluster/articles")]
        public List<NewsArticle> Articles { get; set; }

        /// <summary>
        /// A bing.com/news url that will show all of the results in this cluster.
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/news/cluster/internalurl")]
        public string InternalUrl { get; set; }
    }
}