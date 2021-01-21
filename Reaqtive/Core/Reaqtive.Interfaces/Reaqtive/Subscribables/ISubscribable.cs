// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtive
{
    /// <summary>
    /// Represents a subscribable source.
    /// </summary>
    /// <typeparam name="T">Type of the elements produced by the subscribable source.</typeparam>
    public interface ISubscribable<out T> : IObservable<T>
    {
        /// <summary>
        /// Subscribes the specified observer to the subscribable source.
        /// </summary>
        /// <param name="observer">Observer that will receive the elements of the source.</param>
        /// <returns>Handle to the newly created subscription.</returns>
        new ISubscription Subscribe(IObserver<T> observer);
    }
}
