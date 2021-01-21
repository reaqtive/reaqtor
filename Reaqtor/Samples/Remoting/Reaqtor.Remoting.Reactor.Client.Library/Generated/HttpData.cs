// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Nuqleon.DataModel;

namespace Reaqtor.Remoting.Reactor
{
    /// <summary>
    /// Object definition to contain the values needed to do an HTTP request.
    /// This will be serialized and sent as part of subscriptions.
    /// </summary>
    [KnownType]
    public class HttpData
    {
        /// <summary>
        /// Gets or sets the Http body.
        /// </summary>
        [Mapping("reactor://platform.bing.com/entities/http/Body")]
        public string Body
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Http method.
        /// </summary>
        [Mapping("reactor://platform.bing.com/entities/http/Method")]
        public string Method
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Http headers.
        /// </summary>
        [Mapping("reactor://platform.bing.com/entities/http/Headers")]
        public Tuple<string, string>[] Headers
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the onNext URI.
        /// </summary>
        [Mapping("reactor://platform.bing.com/entities/http/OnNextUri")]
        public Uri OnNext
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the retry policy data.
        /// </summary>
        [Mapping("reactor://platform.bing.com/entities/http/RetryPolicy")]
        public RetryData RetryData
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the URI.
        /// </summary>
        [Mapping("reactor://platform.bing.com/entities/http/Timeout")]
        public TimeSpan Timeout
        {
            get;
            set;
        }
    }
}
