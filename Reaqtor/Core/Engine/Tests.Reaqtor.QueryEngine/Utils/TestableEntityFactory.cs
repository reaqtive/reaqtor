// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

using Reaqtive;
using Reaqtive.Testing;
using Reaqtive.TestingFramework.Mocks;

namespace Tests.Reaqtor.QueryEngine
{
    using TestScheduler = global::Reaqtive.TestingFramework.TestScheduler;

    public static class TestableEntityFactory
    {
        private static readonly Dictionary<string, object> Observers = new();
        private static readonly Dictionary<string, object> Subscribables = new();

        public static TestScheduler Scheduler { get; set; }

        public static IObserver<T> CreateObserver<T>(string id)
        {
            if (!Observers.ContainsKey(id))
            {
                Observers.Add(id, Scheduler.CreateObserver<T>());
            }
            return Observers[id] as IObserver<T>;
        }

        public static ITestableObserver<T> GetObserver<T>(string id)
        {
            if (!Observers.ContainsKey(id))
            {
                return null;
            }

            return Observers[id] as ITestableObserver<T>;
        }

        public static ISubscribable<T> CreateColdSubscribable<T>(string id, Recorded<Notification<T>>[] messages)
        {
            if (!Subscribables.ContainsKey(id))
            {
                Subscribables.Add(id, Scheduler.CreateColdObservable(messages));
            }
            return Subscribables[id] as ISubscribable<T>;
        }

        public static ITestableSubscribable<T> GetSubscribable<T>(string id)
        {
            if (!Subscribables.ContainsKey(id))
            {
                return null;
            }

            return Subscribables[id] as ITestableSubscribable<T>;
        }

        public static void Clear()
        {
            Observers.Clear();
            Subscribables.Clear();
        }
    }
}
