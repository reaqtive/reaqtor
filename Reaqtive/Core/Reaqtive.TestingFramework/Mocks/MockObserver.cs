// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

using Reaqtive.Testing;

namespace Reaqtive.TestingFramework.Mocks
{
    public class MockObserver<T> : ITestableObserver<T>
    {
        private readonly TestScheduler _scheduler;

        public MockObserver(TestScheduler scheduler)
        {
            _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
        }

        public void OnNext(T value)
        {
            Messages.Add(new Recorded<Notification<T>>(_scheduler.Clock, Notification.CreateOnNext<T>(value)));
        }

        public void OnError(Exception error)
        {
            Messages.Add(new Recorded<Notification<T>>(_scheduler.Clock, Notification.CreateOnError<T>(error)));
        }

        public void OnCompleted()
        {
            Messages.Add(new Recorded<Notification<T>>(_scheduler.Clock, Notification.CreateOnCompleted<T>()));
        }

        public IList<Recorded<Notification<T>>> Messages { get; } = new List<Recorded<Notification<T>>>();
    }
}
