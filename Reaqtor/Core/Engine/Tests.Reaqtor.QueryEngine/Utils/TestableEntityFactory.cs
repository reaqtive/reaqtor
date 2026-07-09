// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Reaqtive;
using Reaqtive.Testing;
using Reaqtive.TestingFramework.Mocks;

namespace Tests.Reaqtor.QueryEngine;

using TestScheduler = global::Reaqtive.TestingFramework.TestScheduler;

public static class TestableEntityFactory
{
    private static readonly Dictionary<string, object> Observers = [];
    private static readonly Dictionary<string, object> Subscribables = [];

    public static TestScheduler Scheduler { get; set; }

    public static IObserver<T> CreateObserver<T>(string id)
    {
        if (!Observers.TryGetValue(id, out var observer))
        {
            observer = Scheduler.CreateObserver<T>();
            Observers.Add(id, observer);
        }
        return observer as IObserver<T>;
    }

    public static ITestableObserver<T> GetObserver<T>(string id)
    {
        if (!Observers.TryGetValue(id, out var observer))
        {
            return null;
        }

        return observer as ITestableObserver<T>;
    }

    public static ISubscribable<T> CreateColdSubscribable<T>(string id, Recorded<Notification<T>>[] messages)
    {
        if (!Subscribables.TryGetValue(id, out var subscribable))
        {
            subscribable = Scheduler.CreateColdObservable(messages);
            Subscribables.Add(id, subscribable);
        }
        return subscribable as ISubscribable<T>;
    }

    public static ITestableSubscribable<T> GetSubscribable<T>(string id)
    {
        if (!Subscribables.TryGetValue(id, out var subscribable))
        {
            return null;
        }

        return subscribable as ITestableSubscribable<T>;
    }

    public static void Clear()
    {
        Observers.Clear();
        Subscribables.Clear();
    }
}
