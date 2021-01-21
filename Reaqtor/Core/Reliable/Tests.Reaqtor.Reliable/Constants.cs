// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Rx = Reaqtor;

namespace Tests.Reaqtor.Reliable
{
    internal static class Constants
    {
        public static class Observable
        {
            public const string XS = "reactor://platform.bing.com/observables/xs";
            public const string YS = "reactor://platform.bing.com/observables/ys";
            public const string ZS = "reactor://platform.bing.com/observables/zs";

            public const string Where = "rx://observable/where";
            public const string Select = "rx://observable/select";
            public const string Bind = "rx://observable/bind";
            public const string SelectMany = "rx://observable/selectMany";
            public const string Timer = "rx://observable/timer";
        }

        public static class Observer
        {
            public const string OB = "reactor://platform.bing.com/observers/ob";
            public const string OC = "reactor://platform.bing.com/observers/oc";
        }

        public static class Subscription
        {
            public const string SUB = "reactor://platform.bing.com/subscriptions/sub";
            public const string SUB1 = "reactor://platform.bing.com/subscriptions/sub1";
            public const string SUB2 = "reactor://platform.bing.com/subscriptions/sub2";
        }

        public static class StreamFactory
        {
            public const string SF = "reactor://platform.bing.com/streamFactories/sf";
            public const string SG = "reactor://platform.bing.com/streamFactories/sf2";
        }

        public static class Stream
        {
            public const string FOO = "reactor://platform.bing.com/streams/foo";
            public const string BAR = "reactor://platform.bing.com/streams/bar";
            public const string QUX = "reactor://platform.bing.com/streams/qux";
        }

        public const string SubscribeUri = Rx.Constants.SubscribeUri;
    }
}
