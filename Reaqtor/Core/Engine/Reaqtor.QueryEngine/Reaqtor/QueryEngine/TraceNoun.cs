// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.QueryEngine
{
    [Flags]
    internal enum TraceNoun
    {
        Definition = 0x0001,
        State = 0x0002,

        Observable = 0x0010,
        Observer = 0x0020,
        SubjectFactory = 0x0040,
        Template = 0x0080,

        Subscription = 0x0100,
        Subject = 0x0200,
        ReliableSubscription = 0x0400,
        Other = 0x0800,

        SubscriptionFactory = 0x1000,
    }
}
