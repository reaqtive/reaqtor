// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Text;

using Nuqleon.DataModel;

namespace Reaqtor.Remoting.Reactor.DomainFeeds.DataModels.News
{
    /// <summary>
    /// News parameters
    /// </summary>
    [KnownType]
    public class NewsParameters
    {
        #region Input Parameters
        /// <summary>
        /// Gets or sets the query's search term.
        /// </summary>
        /// <example>Example: <c>Barack Obama presedential debate</c></example>
        [Mapping("bing://reactiveprocessingentity/real/newsparameters/searchterm")]
        public string SearchTerm { get; set; }

        /// <summary>
        /// Gets or sets the threshold of how often to notify.
        /// </summary>
        /// <example>Example: <c>5000</c></example>
        [Mapping("bing://reactiveprocessingentity/real/newsparameters/notificationthresholdinseconds")]
        public int NotificationThresholdInSeconds { get; set; }

        /// <summary>
        /// Gets or sets the flag which determines whether to send a
        /// notification immediately after subscribing.
        /// </summary>
        [Mapping("bing://reactiveprocessingentity/real/newsparameters/triggerimmediately")]
        public bool? TriggerImmediately { get; set; }
        #endregion

        #region Input Parameters Specific To Unsubscribe / Status
        /// <summary>
        /// Gets or sets the Subscription id for the current news subscription
        /// </summary>
        /// <remarks>This field is optional</remarks>
        [Mapping("bing://reactiveprocessingentity/real/newsparameters/subscriptionid")]
        public string SubscriptionId { get; set; }
        #endregion

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append('[');
            sb.AppendFormat("SearchTerm:{0},", SearchTerm);
            sb.AppendFormat("TriggerImmediately:{0},", (TriggerImmediately.HasValue && TriggerImmediately.Value ? "true" : "false"));
            sb.AppendFormat("NotificationThresholdInSeconds:{0},", NotificationThresholdInSeconds);
            sb.AppendFormat("SubscriptionId:{0},", SubscriptionId);
            sb.Append(']');

            return sb.ToString();
        }
    }
}
