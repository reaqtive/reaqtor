// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using Reaqtive.Testing;
using Reaqtive.TestingFramework.Mocks;

namespace Reaqtive.TestingFramework
{
    public static class TestSubscribableExtensions
    {
        /// <summary>
        /// Subscribes the specified source.
        /// </summary>
        /// <typeparam name="T">Type of events.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="res">The result observer.</param>
        /// <param name="context">The context.</param>
        /// <returns>Subscription.</returns>
        public static ISubscription Subscribe<T>(this ISubscribable<T> source, IObserver<T> res, IOperatorContext context)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            ISubscription sub = source.Subscribe(res);
            new SubscriptionInitializeVisitor(sub).Initialize(context);
            return sub;
        }

        /// <summary>
        /// Subscribes the specified source.
        /// </summary>
        /// <typeparam name="T">Type of events.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="res">The result observer.</param>
        /// <param name="context">The context.</param>
        /// <param name="container">The container.</param>
        /// <returns>Subscription.</returns>
        public static ISubscription Subscribe<T>(this ISubscribable<T> source, IObserver<T> res, IOperatorContext context, IOperatorStateContainer container)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            ISubscription sub = source.Subscribe(res);
            new SubscriptionInitializeVisitor(sub).Initialize(context, container.CreateReader());
            return sub;
        }

        public static ISubscribable<T> Apply<T>(this ISubscribable<T> source, TestScheduler scheduler, params Recorded<SubscriptionAction>[] actions)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (actions == null)
            {
                throw new ArgumentNullException(nameof(actions));
            }

            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            return new ApplySubscribable<T>(source, scheduler, actions);
        }

        public static ISubscribable<T> Apply<T>(this ISubscribable<T> source, TestScheduler scheduler, Recorded<SubscriptionAction> action)
        {
            return Apply(source, scheduler, new[] { action });
        }
    }
}
