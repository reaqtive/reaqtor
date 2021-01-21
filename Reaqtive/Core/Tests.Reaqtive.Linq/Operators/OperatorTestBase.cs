// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Reaqtive;
using Reaqtive.TestingFramework;
using Reaqtive.TestingFramework.Mocks;

namespace Test.Reaqtive.Operators
{
    public class OperatorTestBase : TestBase
    {
        protected static ITestableSubscribable<T> GetObservableWitness<T>()
        {
            return default;
        }

        protected static TestScheduler GetContext(TestScheduler scheduler)
        {
            return scheduler;
        }

        protected static long Increment(long from, int repeat)
        {
            return from + repeat;
        }

        protected static void InitializeSubscription(ISubscription sub, TestScheduler scheduler)
        {
            SubscriptionInitializeVisitor.Initialize(sub, scheduler.CreateContext());
        }
    }
}
