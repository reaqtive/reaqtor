// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.Remoting.Reactor
{
    public static class Constants
    {
        /// <summary>
        /// Resource identifiers.
        /// </summary>
        public static class Identifiers
        {
            /// <summary>
            /// Definition of identifiers of well-known observables.
            /// </summary>
            public static class Observable
            {
                /// <summary>
                /// The person observable.
                /// </summary>
                public static class Person
                {
                    /// <summary>The resource id as a string.</summary>
                    public const string String = "reactor://platform.bing.com/observable/person";

                    /// <summary>The resource id as a URI.</summary>
                    public static readonly Uri Uri = new(String);
                }

                /// <summary>
                /// The range observable.
                /// </summary>
                public static class Range
                {
                    /// <summary>The resource id as a string.</summary>
                    public const string String = "reactor://platform.bing.com/observable/range";

                    /// <summary>The resource id as a URI.</summary>
                    public static readonly Uri Uri = new(String);
                }
            }

            /// <summary>
            /// Definition of identifiers of well-known observers.
            /// </summary>
            public static class Observer
            {
                /// <summary>The Http Post action.</summary>
                public static class HttpPost
                {
                    public static class Action
                    {
                        /// <summary>The resource id as a string.</summary>
                        public const string String = "reactor://platform.bing.com/observer/action/http-post";

                        /// <summary>The resource id as a URI.</summary>
                        public static readonly Uri Uri = new(String);
                    }

                    public static class Final
                    {
                        /// <summary>
                        /// Resource identifier for the http post observer to be called
                        /// on subscription completion that takes no headers.
                        /// </summary>
                        public const string NoHeaders = "reactor://platform.bing.com/observer/final/httppost/noheaders";

                        /// <summary>
                        /// Resource identifier for the http post observer to be called
                        /// on subscription completion that takes headers.
                        /// </summary>
                        public const string WithHeaders = "reactor://platform.bing.com/observer/final/httppost/withheaders";
                    }
                }

                /// <summary>
                /// The Http action.
                /// </summary>
                public static class Http
                {
                    /// <summary>The resource id as a string.</summary>
                    public const string String = "reactor://platform.bing.com/observer/http";

                    /// <summary>The resource id as a URI.</summary>
                    public static readonly Uri Uri = new(String);

                    public static class Action
                    {
                        /// <summary>The resource id as a string.</summary>
                        public const string String = "reactor://platform.bing.com/observer/action/http";

                        /// <summary>The resource id as a URI.</summary>
                        public static readonly Uri Uri = new(String);
                    }

                    public static class Final
                    {
                        /// <summary>The resource id as a string.</summary>
                        public const string String = "reactor://platform.bing.com/observer/final/http";

                        /// <summary>The resource id as a URI.</summary>
                        public static readonly Uri Uri = new(String);
                    }
                }
            }
        }
    }
}
