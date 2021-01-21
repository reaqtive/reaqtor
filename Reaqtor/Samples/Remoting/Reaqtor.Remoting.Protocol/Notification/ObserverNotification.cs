// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Globalization;

namespace Reaqtor.Remoting.Protocol
{
    /// <summary>
    /// Factory for reified observer notifications.
    /// </summary>
    public static class ObserverNotification
    {
        /// <summary>
        /// Creates a reification of an `OnNext` operation.
        /// </summary>
        /// <typeparam name="T">The observer notification type.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>The reified operation.</returns>
        public static INotification<T> CreateOnNext<T>(T value) => new OnNextNotification<T>(value);

        /// <summary>
        /// Creates a reification of an `OnNext` operation.
        /// </summary>
        /// <typeparam name="T">The observer notification type.</typeparam>
        /// <param name="predicate">
        /// A predicate to match against other notifications during equality checks.
        /// </param>
        /// <returns>The reified operation.</returns>
        public static INotification<T> CreateOnNext<T>(Func<T, bool> predicate) => new OnNextNotification<T>(predicate);

        /// <summary>
        /// Creates a reification of an `OnError` operation.
        /// </summary>
        /// <typeparam name="T">The observer notification type.</typeparam>
        /// <param name="error">The exception.</param>
        /// <returns>The reified operation.</returns>
        public static INotification<T> CreateOnError<T>(Exception error) => new OnErrorNotification<T>(error);

        /// <summary>
        /// Creates a reification of an `OnError` operation.
        /// </summary>
        /// <typeparam name="T">The observer notification type.</typeparam>
        /// <param name="predicate">
        /// A predicate to match against other notifications during equality checks.
        /// </param>
        /// <returns>The reified operation.</returns>
        public static INotification<T> CreateOnError<T>(Func<Exception, bool> predicate) => new OnErrorNotification<T>(predicate);

        /// <summary>
        /// Creates a reification of an `OnCompleted` operation.
        /// </summary>
        /// <typeparam name="T">The observer notification type.</typeparam>
        /// <returns>The reified operation.</returns>
        public static INotification<T> CreateOnCompleted<T>() => new OnCompletedNotification<T>();

        [Serializable]
        private abstract class NotificationBase<T> : INotification<T>
        {
            public abstract NotificationKind Kind { get; }

            public abstract Exception Exception { get; }

            public abstract bool HasValue { get; }

            public abstract bool HasPredicate { get; }

            public abstract T Value { get; }

            public bool Equals(INotification<T> other) => other != null && Kind == other.Kind && EqualsCore(other);

            protected abstract bool EqualsCore(INotification<T> other);

            public override bool Equals(object obj) => obj is INotification<T> other && Equals(other);

            public override int GetHashCode()
            {
                // The hash code cannot be based on the underlying exception or
                // value as some instances use a predicate for equality checks
                // instead of comparing based on properties.
                return Kind.GetHashCode();
            }

            public void Accept(IObserver<T> observer)
            {
                switch (Kind)
                {
                    case NotificationKind.OnCompleted:
                        observer.OnCompleted();
                        break;
                    case NotificationKind.OnError:
                        observer.OnError(Exception);
                        break;
                    case NotificationKind.OnNext:
                        observer.OnNext(Value);
                        break;
                    default:
                        throw new InvalidOperationException("Unexpected notification type.");
                }
            }
        }

        [Serializable]
        private sealed class OnNextNotification<TValue> : NotificationBase<TValue>
        {
            private readonly TValue _value;
            private readonly Func<TValue, bool> _predicate;

            public OnNextNotification(TValue value) => _value = value;

            public OnNextNotification(Func<TValue, bool> predicate) => _predicate = predicate;

            public override Exception Exception => null;

            public override bool HasValue => _predicate == null;

            public override bool HasPredicate => _predicate != null;

            public override NotificationKind Kind => NotificationKind.OnNext;

            public override TValue Value => _value;

            protected override bool EqualsCore(INotification<TValue> other)
            {
                return other.HasPredicate
                    ? !HasPredicate && other.Equals(this)
                    : HasPredicate
                        ? _predicate(other.Value)
                        : EqualityComparer<TValue>.Default.Equals(_value, other.Value);
            }

            public override string ToString() => HasPredicate
                    ? string.Format(CultureInfo.CurrentCulture, "OnNext({0})", _predicate.GetType().FullName)
                    : string.Format(CultureInfo.CurrentCulture, "OnNext({0})", Value);
        }

        [Serializable]
        private sealed class OnCompletedNotification<TValue> : NotificationBase<TValue>
        {
            public override Exception Exception => null;

            public override bool HasValue => false;

            public override bool HasPredicate => false;

            public override NotificationKind Kind => NotificationKind.OnCompleted;

            public override TValue Value => default;

            protected override bool EqualsCore(INotification<TValue> other) => true;

            public override string ToString() => "OnCompleted()";
        }

        [Serializable]
        private sealed class OnErrorNotification<TValue> : NotificationBase<TValue>
        {
            private readonly Exception _error;
            private readonly Func<Exception, bool> _predicate;

            public OnErrorNotification(Exception error) => _error = error;

            public OnErrorNotification(Func<Exception, bool> predicate) => _predicate = predicate;

            public override Exception Exception => _error;

            public override bool HasValue => false;

            public override bool HasPredicate => _predicate != null;

            public override NotificationKind Kind => NotificationKind.OnError;

            public override TValue Value => default;

            protected override bool EqualsCore(INotification<TValue> other)
            {
                return other.HasPredicate
                    ? !HasPredicate && other.Equals(this)
                    : HasPredicate
                        ? _predicate(other.Exception)
                        : _error == other.Exception;
            }

            public override string ToString() => HasPredicate
                    ? string.Format(CultureInfo.CurrentCulture, "OnError({0})", _predicate.GetType().FullName)
                    : string.Format(CultureInfo.CurrentCulture, "OnError({0})", Exception.GetType().FullName);
        }
    }
}
