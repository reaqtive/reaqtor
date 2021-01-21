// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.Remoting.Protocol
{
    /// <summary>
    /// Reified observer operation interface.
    /// </summary>
    /// <typeparam name="T">The observer notification type.</typeparam>
    public interface INotification<T> : IEquatable<INotification<T>>
    {
        /// <summary>
        /// The kind of notification.
        /// </summary>
        NotificationKind Kind { get; }

        /// <summary>
        /// The notification exception.
        /// </summary>
        Exception Exception { get; }

        /// <summary>
        /// <b>true</b> if the notification has a value, <b>false</b> otherwise.
        /// </summary>
        bool HasValue { get; }

        /// <summary>
        /// <b>true</b> if the notification is predicate-based, <b>false</b> otherwise.
        /// </summary>
        bool HasPredicate { get; }

        /// <summary>
        /// The notification value.
        /// </summary>
        T Value { get; }

        /// <summary>
        /// Applies the notification to the given observer.
        /// </summary>
        /// <param name="observer">The observer.</param>
        void Accept(IObserver<T> observer);
    }
}
