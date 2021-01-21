// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.Samples.Models
{
    using Nuqleon.DataModel;

    /// <summary>
    /// News parameters
    /// </summary>
    public class NewsParameters
    {
        #region Input Parameters
        /// <summary>
        /// Gets or sets the query's search term.
        /// </summary>
        /// <example>Example: <c>Barack Obama presidential debate</c></example>
        [Mapping("bing://reactiveprocessingentity/real/newsparameters/searchterm")]
        public string SearchTerm { get; set; }

        /// <summary>
        /// Gets or sets the threshold of how often to notify.
        /// </summary>
        /// <example>Example: <c>5000</c></example>
        /// <remarks>This field is optional</remarks>
        [Mapping("bing://reactiveprocessingentity/real/newsparameters/notificationthresholdinseconds")]
        public int NotificationThresholdInSeconds { get; set; }

        /// <summary>
        /// Gets or sets the flag which determines whether to send a
        /// notification immediately after subscribing.
        /// </summary>
        /// <remarks>This field is optional</remarks>
        [Mapping("bing://reactiveprocessingentity/real/newsparameters/triggerimmediately")]
        public bool? TriggerImmediately { get; set; }

        /// <summary>
        /// Gets or sets the market of the client
        /// </summary>
        /// <example>Example: <c>en-US</c></example>
        /// <remarks>This field is optional</remarks>
        [Mapping("bing://reactiveprocessingentity/real/newsparameters/market")]
        public string Market { get; set; }
        #endregion

        #region Input Parameters Specific To Unsubscribe / Status
        /// <summary>
        /// Gets or sets the Subscription id for the current news subscription
        /// </summary>
        /// <remarks>This field is optional</remarks>
        [Mapping("bing://reactiveprocessingentity/real/newsparameters/subscriptionid")]
        public string SubscriptionId { get; set; }
        #endregion
    }
}