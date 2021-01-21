// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtive.Testing;

namespace Reaqtive.TestingFramework
{
#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1001 // Types that own disposable fields should be disposable. (TestCleanup takes care of disposing.)

    public class TestBase : ReactiveTest
    {
        protected TestScheduler Scheduler { get; private set; }

        protected void TestInitialize()
        {
            Scheduler = new TestScheduler();
        }

        protected void TestCleanup()
        {
            if (Scheduler != null)
            {
                Scheduler.Dispose();
                Scheduler = null;
            }
        }

#pragma warning disable CA1062 // Validate arguments of public methods. (Trusting test code to call Run with non-null delegate.)

        protected static void Run(Action<TestScheduler> test)
        {
            using var scheduler = new TestScheduler();

            test(scheduler);
        }

#pragma warning restore CA1062

        protected static Func<TestScheduler, TResult> FromContext<TResult>(Func<TestScheduler, TResult> f)
        {
            return f;
        }

        protected static Recorded<SubscriptionAction> OnLoad(long time, IOperatorStateContainer state)
        {
            return new Recorded<SubscriptionAction>(time, new LoadState(state));
        }

        protected static Recorded<SubscriptionAction> OnSave(long time, IOperatorStateContainer state)
        {
            return new Recorded<SubscriptionAction>(time, new SaveState(state));
        }
    }

#pragma warning restore CA1001
#pragma warning restore IDE0079
}
