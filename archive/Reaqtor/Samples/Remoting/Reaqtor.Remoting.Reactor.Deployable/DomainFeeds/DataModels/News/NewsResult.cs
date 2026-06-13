// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Nuqleon.DataModel;

namespace Reaqtor.Remoting.Reactor.DomainFeeds.DataModels.News
{
    /// <summary>
    /// The possible types of news results.
    /// </summary>
    public enum NewsResultType
    {
        /// <summary>
        /// Result is an article
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/news/resulttype/article")]
        Article = 1,

        /// <summary>
        /// Result is cluster
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/news/resulttype/cluster")]
        Cluster = 2,

        /// <summary>
        /// Result is local
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/news/resulttype/local")]
        Local = 3,
    }

    /// <summary>
    /// News object to describe one single result.
    /// </summary>
    [KnownType]
    public class NewsResult
    {
        /// <summary>
        /// Gets or sets the type of news result. This should be used when deciding which of the fields to read from.
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/news/resulttype")]
        public NewsResultType ResultType { get; set; }

        /// <summary>
        /// Gets or sets a single news article. ResultType == Article.
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/news/article")]
        public NewsArticle Article { get; set; }

        /// <summary>
        /// Gets or sets a cluster of related news articles about the same story. ResultType == Cluster.
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/news/cluster")]
        public NewsCluster Cluster { get; set; }

        /// <summary>
        /// Gets or sets local news for a specific location. ResultType == Local.
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/news/local")]
        public NewsLocal Local { get; set; }
    }
}
