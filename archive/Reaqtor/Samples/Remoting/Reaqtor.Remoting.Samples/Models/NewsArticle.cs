// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.Samples.Models
{
    using System;

    using Nuqleon.DataModel;

    /// <summary>
    /// News object to describe result-type 'Article'.
    /// </summary>
    public class NewsArticle
    {
        /// <summary>
        /// The title of the article, parsed from the publisher site.
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/news/article/title")]
        public string Title { get; set; }

        /// <summary>
        /// The url of the article on the publisher site.
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/news/article/url")]
        public string Url { get; set; }

        /// <summary>
        /// The name of the publisher. For example “New York Times”.
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/news/article/source")]
        public string Source { get; set; }

        /// <summary>
        /// A short snippet parsed from the article.
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/news/article/snippet")]
        public string Snippet { get; set; }

        /// <summary>
        /// The time when the article was published.
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/news/article/publishedtime")]
        public DateTime PublishTime { get; set; }
    }
}
