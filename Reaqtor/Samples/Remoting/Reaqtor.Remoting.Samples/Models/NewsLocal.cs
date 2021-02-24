// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.Samples.Models
{
    using System.Collections.Generic;
    using Nuqleon.DataModel;

    /// <summary>
    /// News object to describe result-type 'Local'.
    /// </summary>
    public class NewsLocal
    {
        /// <summary>
        /// A list of news for a specific location.
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/news/local/articles")]
        public List<NewsArticle> Articles { get; set; }

        /// <summary>
        /// A bing.com/news url that will show all of the local news results.
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/news/local/internalurl")]
        public string InternalUrl { get; set; }

        /// <summary>
        /// A human readable location (i.e. “Redmond, Washington”)
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/news/local/location")]
        public string Location { get; set; }
    }
}
