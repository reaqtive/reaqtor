// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.Shebang.Client
{
    public partial class ClientContext
    {
        [KnownResource("rx://observable/empty")]
        public IAsyncReactiveQbservable<TResult> Empty<TResult>() => GetObservable<TResult>(new Uri("rx://observable/empty"));

        [KnownResource("rx://observable/never")]
        public IAsyncReactiveQbservable<TResult> Never<TResult>() => GetObservable<TResult>(new Uri("rx://observable/never"));

        [KnownResource("rx://observable/return")]
        public IAsyncReactiveQbservable<TResult> Return<TResult>(TResult value) => GetObservable<TResult, TResult>(new Uri("rx://observable/return"))(value);

        [KnownResource("rx://observable/throw")]
        public IAsyncReactiveQbservable<TResult> Throw<TResult>(Exception error) => GetObservable<Exception, TResult>(new Uri("rx://observable/throw"))(error);

        [KnownResource("rx://observable/timer/single/relative")]
        public IAsyncReactiveQbservable<long> Timer(TimeSpan dueTime) => GetObservable<TimeSpan, long>(new Uri("rx://observable/timer/single/relative"))(dueTime);

        [KnownResource("rx://observable/timer/single/absolute")]
        public IAsyncReactiveQbservable<long> Timer(DateTimeOffset dueTime) => GetObservable<DateTimeOffset, long>(new Uri("rx://observable/timer/single/absolute"))(dueTime);

        [KnownResource("rx://observable/timer/period/relative")]
        public IAsyncReactiveQbservable<long> Timer(TimeSpan dueTime, TimeSpan period) => GetObservable<TimeSpan, TimeSpan, long>(new Uri("rx://observable/timer/period/relative"))(dueTime, period);

        [KnownResource("rx://observable/timer/period/absolute")]
        public IAsyncReactiveQbservable<long> Timer(DateTimeOffset dueTime, TimeSpan period) => GetObservable<DateTimeOffset, TimeSpan, long>(new Uri("rx://observable/timer/period/absolute"))(dueTime, period);

    }
}
