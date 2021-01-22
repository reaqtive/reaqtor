// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using Reaqtor.Remoting.Protocol;
using Reaqtor.Remoting.TestingFramework;
using Reaqtive.Testing;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Reaqtor.TestingFramework
{
    public static class DeclarativeExtensions
    {
        #region Start Syntactic Sugar

        /// <summary>
        /// Starts the test scheduler and creates the subscription to the sequence obtained through the factory function.
        /// Scheduler is passed to the subscription using the provided operator context.
        /// Default virtual times are used for <see cref="ReactiveTest.Created">factory invocation</see> and <see cref="ReactiveTest.Subscribed">sequence subscription</see>.
        /// </summary>
        /// <typeparam name="T">The element type of the observable sequence being tested.</typeparam>
        /// <param name="client">The scheduler.</param>
        /// <param name="create">Factory method to create an observable sequence.</param>
        /// <returns>
        /// Observer with timestamped recordings of notification messages that were received during the virtual time window when the subscription to the source sequence was active.
        /// </returns>
        public static ITestObserver<T> Start<T>(this ITestReactivePlatformClient client, Func<IAsyncReactiveQbservable<T>> create)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            return client.Start(create, ReactiveTest.Created, ReactiveTest.Subscribed, ReactiveTest.Disposed);
        }

        /// <summary>
        /// Starts the test scheduler and uses the specified virtual time to dispose the subscription to the sequence obtained through
        /// the factory function. Scheduler is passed to the subscription using the provided operator context.
        /// Default virtual times are used for <see cref="ReactiveTest.Created">factory invocation</see> and <see cref="ReactiveTest.Subscribed">sequence subscription</see>.
        /// </summary>
        /// <typeparam name="T">The element type of the observable sequence being tested.</typeparam>
        /// <param name="client">The scheduler.</param>
        /// <param name="create">Factory method to create an observable sequence.</param>
        /// <param name="disposed">Virtual time at which to dispose the subscription.</param>
        /// <returns>
        /// Observer with timestamped recordings of notification messages that were received during the virtual time window when the subscription to the source sequence was active.
        /// </returns>
        public static ITestObserver<T> Start<T>(this ITestReactivePlatformClient client, Func<IAsyncReactiveQbservable<T>> create, long disposed)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            return client.Start(create, ReactiveTest.Created, ReactiveTest.Subscribed, disposed);
        }

        #endregion

        #region Operators

        public static IAsyncReactiveQbservable<T> SelectMany<T>(this ITestableQbservable<ITestableQbservable<T>> source, Expression<Func<IAsyncReactiveQbservable<T>, IAsyncReactiveObservable<T>>> selector)
        {
            return source.Provider.CreateQbservable<IAsyncReactiveQbservable<T>>(source.Expression).SelectMany(selector);
        }

        #endregion

        #region Assertion Helpers

        public static void AssertEqual(this IList<Subscription> subscriptions, params Subscription[] other)
        {
            subscriptions = Collapsed(subscriptions).ToList();

            if (subscriptions.Count != other.Length)
            {
                if (subscriptions.Count < other.Length)
                {
                    FailEqual(subscriptions, other);
                }

                for (var i = 0; i < other.Length; ++i)
                {
                    Assert.AreEqual(subscriptions[i].Subscribe, other[i].Subscribe, Message(subscriptions, other));
                    var tail = subscriptions.Count - other.Length;
                    Assert.AreEqual(subscriptions[tail + i].Unsubscribe, other[i].Unsubscribe, Message(subscriptions, other));
                }
            }
            else
            {
                ReactiveAssert.AssertEqual(subscriptions, other);
            }
        }

        public static void AssertEqual<T>(this IList<Recorded<INotification<T>>> actual, params Recorded<INotification<T>>[] expected)
        {
            if (actual.Count != expected.Length || !IsTimeSorted(actual) || !IsTimeSorted(expected))
            {
                FailEqual(actual, expected);
            }

            var expectedMappedMessages = expected.MapMessages();
            var actualMappedMessages = actual.MapMessages();

            foreach (var kv in expectedMappedMessages)
            {
                if (!actualMappedMessages.ContainsKey(kv.Key))
                {
                    FailEqual(actual, expected);
                }

                var actualValue = actualMappedMessages[kv.Key];

                // TODO: support bi-directional Union operation by enabling
                // bi-directional equality. I.e., currently we have:
                //   
                //   expected.Equals(actual) && !actual.Equals(expected)
                // 
                // The reason is because the expected values may contain
                // predicates, and the default equality logic for the
                // notification implementation does not yet support equality in
                // the reverse direction.
                var unionLength = kv.Value.Union(actualValue).Count();

                if (kv.Value.Count != unionLength || actualValue.Count != unionLength)
                {
                    FailEqual(actual, expected);
                }
            }
        }

        private static bool IsTimeSorted<T>(IEnumerable<Recorded<T>> events)
        {
            var last = long.MinValue;
            foreach (var e in events)
            {
                if (e.Time < last)
                {
                    return false;
                }
                last = e.Time;
            }
            return true;
        }

        private static IDictionary<long, IList<INotification<T>>> MapMessages<T>(this IList<Recorded<INotification<T>>> messages)
        {
            var mapped = new Dictionary<long, IList<INotification<T>>>();
            foreach (var message in messages)
            {
                if (!mapped.TryGetValue(message.Time, out var res))
                {
                    res = new List<INotification<T>>();
                    mapped[message.Time] = res;
                }
                res.Add(message.Value);
            }
            return mapped;
        }

        private static void FailEqual<T>(IEnumerable<T> actual, IEnumerable<T> expected)
        {
            Assert.Fail(Message(actual, expected));
        }

        private static string Message<T>(IEnumerable<T> actual, IEnumerable<T> expected)
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            sb.Append("Expected: [");
            sb.Append(string.Join(", ", expected.Select(x => x.ToString()).ToArray()));
            sb.Append(']');
            sb.AppendLine();
            sb.Append("Actual..: [");
            sb.Append(string.Join(", ", actual.Select(x => x.ToString()).ToArray()));
            sb.Append(']');
            sb.AppendLine();
            return sb.ToString();
        }

        private static IEnumerable<Subscription> Collapsed(IList<Subscription> subscriptions)
        {
            var start = -1L;

            for (var i = 0; i < subscriptions.Count; ++i)
            {
                var s = subscriptions[i];
                if (start < 0)
                {
                    start = s.Subscribe;
                }

                if (s.Unsubscribe != long.MaxValue || i == subscriptions.Count - 1)
                {
                    yield return new Subscription(start, s.Unsubscribe);
                    start = -1L;
                }
            }
        }

        #endregion
    }
}
