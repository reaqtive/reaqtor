// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

using Nuqleon.DataModel;

namespace Reaqtor.Remoting.Reactor.Client
{

    /// <summary>
    /// News object to describe result-type 'Cluster'.
    /// </summary>
    public class NewsCluster
    {
        /// <summary>
        /// Gets or sets a list of articles belonging to the same cluster.
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/news/cluster/articles")]
        public List<NewsArticle> Articles { get; set; }

        /// <summary>
        /// Gets or sets a bing.com/news url that will show all of the results in this cluster.
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/news/cluster/internalurl")]
        public string InternalUrl { get; set; }
    }
}
