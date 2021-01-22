// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

//
// NB: This file contains a port of Rx code that should eventually get removed.
//

using System;

namespace Reaqtive.Testing
{
    /// <summary>
    /// Base class to write unit tests for applications and libraries built using Reactive Extensions.
    /// </summary>
    public class ReactiveTest
    {
        /// <summary>
        /// Default virtual time used for creation of observable sequences in <see cref="ReactiveTest"/>-based unit tests.
        /// </summary>
        public const long Created = 100;

        /// <summary>
        /// Default virtual time used to subscribe to observable sequences in <see cref="ReactiveTest"/>-based unit tests.
        /// </summary>
        public const long Subscribed = 200;

        /// <summary>
        /// Default virtual time used to dispose subscriptions in <see cref="ReactiveTest"/>-based unit tests.
        /// </summary>
        public const long Disposed = 1000;

        /// <summary>
        /// Factory method for an OnNext notification record at a given time with a given value.
        /// </summary>
        /// <typeparam name="T">The element type for the resulting notification object.</typeparam>
        /// <param name="ticks">Recorded virtual time the OnNext notification occurs.</param>
        /// <param name="value">Recorded value stored in the OnNext notification.</param>
        /// <returns>Recorded OnNext notification.</returns>
        public static Recorded<Notification<T>> OnNext<T>(long ticks, T value)
        {
            return new Recorded<Notification<T>>(ticks, Notification.CreateOnNext(value));
        }

        /// <summary>
        /// Factory method for writing an assert that checks for an OnNext notification record at a given time, using the specified predicate to check the value.
        /// </summary>
        /// <typeparam name="T">The element type for the resulting notification object.</typeparam>
        /// <param name="ticks">Recorded virtual time the OnNext notification occurs.</param>
        /// <param name="predicate">Predicate function to check the OnNext notification value against an expected value.</param>
        /// <returns>Recorded OnNext notification with a predicate to assert a given value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is null.</exception>
        public static Recorded<Notification<T>> OnNext<T>(long ticks, Func<T, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return new Recorded<Notification<T>>(ticks, new OnNextPredicate<T>(predicate));
        }

        /// <summary>
        /// Factory method for an OnCompleted notification record at a given time.
        /// </summary>
        /// <typeparam name="T">The element type for the resulting notification object.</typeparam>
        /// <param name="ticks">Recorded virtual time the OnCompleted notification occurs.</param>
        /// <returns>Recorded OnCompleted notification.</returns>
        public static Recorded<Notification<T>> OnCompleted<T>(long ticks)
        {
            return new Recorded<Notification<T>>(ticks, Notification.CreateOnCompleted<T>());
        }

#pragma warning disable IDE0060 // Remove unused parameter (used for type inference)
        /// <summary>
        /// Factory method for an OnCompleted notification record at a given time, using inference to determine the type of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The element type for the resulting notification object.</typeparam>
        /// <param name="ticks">Recorded virtual time the OnCompleted notification occurs.</param>
        /// <param name="witness">Object solely used to infer the type of the <typeparamref name="T"/> type parameter. This parameter is typically used when creating a sequence of anonymously typed elements.</param>
        /// <returns>Recorded OnCompleted notification.</returns>
        public static Recorded<Notification<T>> OnCompleted<T>(long ticks, T witness)
        {
            return new Recorded<Notification<T>>(ticks, Notification.CreateOnCompleted<T>());
        }
#pragma warning restore IDE0060 // Remove unused parameter

        /// <summary>
        /// Factory method for an OnError notification record at a given time with a given error.
        /// </summary>
        /// <typeparam name="T">The element type for the resulting notification object.</typeparam>
        /// <param name="ticks">Recorded virtual time the OnError notification occurs.</param>
        /// <param name="exception">Recorded exception stored in the OnError notification.</param>
        /// <returns>Recorded OnError notification.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="exception"/> is null.</exception>
        public static Recorded<Notification<T>> OnError<T>(long ticks, Exception exception)
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            return new Recorded<Notification<T>>(ticks, Notification.CreateOnError<T>(exception));
        }

        /// <summary>
        /// Factory method for writing an assert that checks for an OnError notification record at a given time, using the specified predicate to check the exception.
        /// </summary>
        /// <typeparam name="T">The element type for the resulting notification object.</typeparam>
        /// <param name="ticks">Recorded virtual time the OnError notification occurs.</param>
        /// <param name="predicate">Predicate function to check the OnError notification value against an expected exception.</param>
        /// <returns>Recorded OnError notification with a predicate to assert a given exception.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is null.</exception>
        public static Recorded<Notification<T>> OnError<T>(long ticks, Func<Exception, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return new Recorded<Notification<T>>(ticks, new OnErrorPredicate<T>(predicate));
        }

#pragma warning disable IDE0060 // Remove unused parameter (used for type inference)
        /// <summary>
        /// Factory method for an OnError notification record at a given time with a given error, using inference to determine the type of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The element type for the resulting notification object.</typeparam>
        /// <param name="ticks">Recorded virtual time the OnError notification occurs.</param>
        /// <param name="exception">Recorded exception stored in the OnError notification.</param>
        /// <param name="witness">Object solely used to infer the type of the <typeparamref name="T"/> type parameter. This parameter is typically used when creating a sequence of anonymously typed elements.</param>
        /// <returns>Recorded OnError notification.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="exception"/> is null.</exception>
        public static Recorded<Notification<T>> OnError<T>(long ticks, Exception exception, T witness)
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            return new Recorded<Notification<T>>(ticks, Notification.CreateOnError<T>(exception));
        }
#pragma warning restore IDE0060 // Remove unused parameter

#pragma warning disable IDE0060 // Remove unused parameter (used for type inference)
        /// <summary>
        /// Factory method for writing an assert that checks for an OnError notification record at a given time, using the specified predicate to check the exception and inference to determine the type of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The element type for the resulting notification object.</typeparam>
        /// <param name="ticks">Recorded virtual time the OnError notification occurs.</param>
        /// <param name="predicate">Predicate function to check the OnError notification value against an expected exception.</param>
        /// <param name="witness">Object solely used to infer the type of the <typeparamref name="T"/> type parameter. This parameter is typically used when creating a sequence of anonymously typed elements.</param>
        /// <returns>Recorded OnError notification with a predicate to assert a given exception.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is null.</exception>
        public static Recorded<Notification<T>> OnError<T>(long ticks, Func<Exception, bool> predicate, T witness)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return new Recorded<Notification<T>>(ticks, new OnErrorPredicate<T>(predicate));
        }
#pragma warning restore IDE0060 // Remove unused parameter

        /// <summary>
        /// Factory method for a subscription record based on a given subscription and disposal time.
        /// </summary>
        /// <param name="start">Virtual time indicating when the subscription was created.</param>
        /// <param name="end">Virtual time indicating when the subscription was disposed.</param>
        /// <returns>Subscription object.</returns>
        public static Subscription Subscribe(long start, long end)
        {
            return new Subscription(start, end);
        }

        /// <summary>
        /// Factory method for a subscription record based on a given subscription time.
        /// </summary>
        /// <param name="start">Virtual time indicating when the subscription was created.</param>
        /// <returns>Subscription object.</returns>
        public static Subscription Subscribe(long start)
        {
            return new Subscription(start);
        }

        #region Predicate-based notification assert helper classes

        private sealed class OnNextPredicate<T> : PredicateNotification<T>
        {
            private readonly Func<T, bool> _predicate;

            public OnNextPredicate(Func<T, bool> predicate)
            {
                _predicate = predicate;
            }

            public override bool Equals(Notification<T> other)
            {
                if (ReferenceEquals(this, other))
                    return true;
                if (other is null)
                    return false;
                if (other.Kind != NotificationKind.OnNext)
                    return false;

                return _predicate(other.Value);
            }
        }

        private sealed class OnErrorPredicate<T> : PredicateNotification<T>
        {
            private readonly Func<Exception, bool> _predicate;

            public OnErrorPredicate(Func<Exception, bool> predicate)
            {
                _predicate = predicate;
            }

            public override bool Equals(Notification<T> other)
            {
                if (ReferenceEquals(this, other))
                    return true;
                if (other is null)
                    return false;
                if (other.Kind != NotificationKind.OnError)
                    return false;

                return _predicate(other.Exception);
            }
        }

        private abstract class PredicateNotification<T> : Notification<T>
        {
            #region Non-implemented members (by design)

            public override T Value => throw new NotSupportedException();

            public override bool HasValue => throw new NotSupportedException();

            public override Exception Exception => throw new NotSupportedException();

            public override NotificationKind Kind => throw new NotSupportedException();

            public override void Accept(IObserver<T> observer) => throw new NotSupportedException();

            #endregion
        }

        #endregion
    }
}
