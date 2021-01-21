// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Nuqleon.DataModel;

namespace Reaqtor.Remoting.Reactor
{
    /// <summary>
    /// Object definition to contain the values needed to do an HTTP POST.
    /// This will be serialized and sent as part of subscriptions containing
    /// a DoHttpPost action.
    /// </summary>
    [KnownType]
    public class HttpPost
    {
        /// <summary>
        /// Gets or sets the URI.
        /// </summary>
        [Mapping("reactor://platform.bing.com/entities/httppost/Uri")]
        public Uri Uri
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the headers.
        /// </summary>
        [Mapping("reactor://platform.bing.com/entities/httppost/Headers")]
        public Tuple<string, string>[] Headers
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        [Mapping("reactor://platform.bing.com/entities/httppost/Body")]
        public string Body
        {
            get;
            set;
        }
    }
}
