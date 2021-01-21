// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

using Nuqleon.DataModel;

namespace Reaqtor.Remoting.Reactor.DomainFeeds.DataModels.News
{
    /// <summary>
    /// The news object that carries notification information for a search-query term.
    /// </summary>
    [KnownType]
    public class NewsInfo
    {
        /// <summary>
        /// Gets or sets the Id that was generated by the News service and attached to the subscription when it was created.
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/news/subscriptionid")]
        public string SubscriptionId { get; set; }

        /// <summary>
        /// Gets or sets the search-term the results are for.
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/news/searchterm")]
        public string SearchQueryTerm { get; set; }

        /// <summary>
        /// Gets or sets a list of news results relevant to the subscription.
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/news/results")]
        public List<NewsResult> Results { get; set; }
    }
}
