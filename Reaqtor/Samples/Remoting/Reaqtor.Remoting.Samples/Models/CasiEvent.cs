// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.Samples.Models
{
    using Nuqleon.DataModel;

    public class CasiEvent
    {
        [Mapping("reactor://platform.bing.com/casi/Policy")]
        public string Policy { get; set; }

        [Mapping("reactor://platform.bing.com/casi/ItemGroupId")]
        public string ItemGroupId { get; set; }

        [Mapping("reactor://platform.bing.com/casi/ItemId")]
        public string ItemId { get; set; }

        [Mapping("reactor://platform.bing.com/casi/Action")]
        public string Action { get; set; }

        [Mapping("reactor://platform.bing.com/casi/Environment")]
        public string Environment { get; set; }
    }
}