// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.Samples.Models
{
    using System;
    using System.Collections.Generic;

    using Nuqleon.DataModel;

    /// <summary>
    /// Data contract for msnjv alert data provider (push mechanism)
    /// </summary>
    public class MsnjvAlert
    {
        /// <summary>
        /// Gets or sets the event ID
        /// </summary>
        [Mapping("reactor://platform.bing.com/msnjv/alert/eventid")]
        public string EventId { get; set; }

        /// <summary>
        /// Gets or sets the Alert level
        /// </summary>
        [Mapping("reactor://platform.bing.com/msnjv/alert/level")]
        public int Level { get; set; }

        /// <summary>
        /// Gets or sets the Alert domain
        /// </summary>
        [Mapping("reactor://platform.bing.com/msnjv/alert/domain")]
        public string Domain { get; set; }

        /// <summary>
        /// Gets or sets the Alert scenario
        /// </summary>
        [Mapping("reactor://platform.bing.com/msnjv/alert/scenario")]
        public string Scenario { get; set; }

        /// <summary>
        /// Gets or sets the Alert title
        /// </summary>
        [Mapping("reactor://platform.bing.com/msnjv/alert/title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the Alert description
        /// </summary>
        [Mapping("reactor://platform.bing.com/msnjv/alert/description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Alert update time
        /// </summary>
        [Mapping("reactor://platform.bing.com/msnjv/alert/updatetime")]
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// Gets or sets the Alert frequency
        /// </summary>
        [Mapping("reactor://platform.bing.com/msnjv/alert/freq")]
        public List<int> Freq { get; set; }

        /// <summary>
        /// Gets or sets the Alert level gap
        /// </summary>
        [Mapping("reactor://platform.bing.com/msnjv/alert/levelgap")]
        public int LevelGap { get; set; }

        /// <summary>
        /// Gets or sets the Alert exprire time
        /// </summary>
        [Mapping("reactor://platform.bing.com/msnjv/alert/expiretime")]
        public DateTime ExpireTime { get; set; }

        /// <summary>
        /// Gets or sets the Alert expire date offset
        /// </summary>
        [Mapping("reactor://platform.bing.com/msnjv/alert/expiredateoffset")]
        public int ExpireDateOffset { get; set; }

        /// <summary>
        /// Gets or sets the Alert alerting time
        /// </summary>
        [Mapping("reactor://platform.bing.com/msnjv/alert/alerttime")]
        public DateTime AlertTime { get; set; }

        /// <summary>
        /// Gets or sets the Alert alerting date offset
        /// </summary>
        [Mapping("reactor://platform.bing.com/msnjv/alert/alertdateoffset")]
        public int AlertDateOffset { get; set; }

        /// <summary>
        /// Gets or sets the Alert transit before alert
        /// </summary>
        [Mapping("reactor://platform.bing.com/msnjv/alert/transitbeforealert")]
        public string TransitBeforeAlert { get; set; }

        /// <summary>
        /// Gets or sets the Alert deduplication key
        /// </summary>
        [Mapping("reactor://platform.bing.com/msnjv/alert/dedupkey")]
        public string DedupKey { get; set; }

        /// <summary>
        /// Gets or sets the Alert additional details
        /// </summary>
        [Mapping("reactor://platform.bing.com/msnjv/alert/additionaldetails")]
        public List<Tuple<string, string>> AdditionalDetails { get; set; }
    }
}
