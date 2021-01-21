// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Rx = Reaqtor;

namespace Tests.Reaqtor.Client
{
    internal static class Constants
    {
        public static class Observable
        {
            public const string XS = "reactor://platform.bing.com/observables/xs";
            public const string PXS = "reactor://platform.bing.com/observables/xs/p";
            public const string YS = "reactor://platform.bing.com/observables/ys";
            public const string PYS = "reactor://platform.bing.com/observables/ys/p";
            public const string ZS = "reactor://platform.bing.com/observables/zs";
            public const string PZS = "reactor://platform.bing.com/observables/zs/p";

            public const string Where = "rx://observable/where";
            public const string Select = "rx://observable/select";
            public const string Bind = "rx://observable/bind";
            public const string SelectMany = "rx://observable/selectMany";
            public const string Timer = "rx://observable/timer";
            public const string Empty = "rx://observable/empty";
            public const string Do = "rx://observable/do";
        }

        public static class Observer
        {
            public const string OB = "reactor://platform.bing.com/observers/ob";
            public const string POB = "reactor://platform.bing.com/observers/ob/p";
            public const string OB1 = "reactor://platform.bing.com/observers/ob1";
            public const string POB1 = "reactor://platform.bing.com/observers/ob1/p";
            public const string OB2 = "reactor://platform.bing.com/observers/ob2";
            public const string POB2 = "reactor://platform.bing.com/observers/ob2/p";
            public const string OB3 = "reactor://platform.bing.com/observers/ob3";
            public const string POB3 = "reactor://platform.bing.com/observers/ob3/p";
        }

        public static class Subscription
        {
            public const string SUB = "reactor://platform.bing.com/subscriptions/sub";
            public const string SUB1 = "reactor://platform.bing.com/subscriptions/sub1";
            public const string SUB2 = "reactor://platform.bing.com/subscriptions/sub2";
            public const string SUB3 = "reactor://platform.bing.com/subscriptions/sub3";
        }

        public static class StreamFactory
        {
            public const string SF = "reactor://platform.bing.com/streamFactories/sf";
            public const string PSF = "reactor://platform.bing.com/streamFactories/sf/p";
            public const string SG = "reactor://platform.bing.com/streamFactories/sf2";
            public const string PSG = "reactor://platform.bing.com/streamFactories/sf2/p";
        }

        public static class SubscriptionFactory
        {
            public const string SF = "reactor://platform.bing.com/subscriptionFactories/sf";
            public const string PSF = "reactor://platform.bing.com/subscriptionFactories/sf/p";
            public const string SG = "reactor://platform.bing.com/subscriptionFactories/sf2";
            public const string PSG = "reactor://platform.bing.com/subscriptionFactories/sf2/p";
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
