// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

//
// NB: This file contains a port of Rx code that should eventually get removed.
//

using Reaqtive;

namespace System
{
    /// <summary>
    /// Provides a set of extension methods for <see cref="IObservable{T}"/>.
    /// </summary>
    public static class ObservableExtensions
    {
        /// <summary>
        /// Subscribes to the specified <paramref name="source"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the observable sequence.</typeparam>
        /// <param name="source">The observable sequence to subscribe to.</param>
        /// <returns>A disposable object used to unsubscribe from the observable sequence.</returns>
        public static IDisposable Subscribe<T>(this IObservable<T> source) => Subscribe(source, _ => { }, ex => throw ex, () => { });

        /// <summary>
        /// Subscribes to the specified <paramref name="source"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the observable sequence.</typeparam>
        /// <param name="source">The observable sequence to subscribe to.</param>
        /// <param name="onNext">The action to invoke for each element produced by the sequence.</param>
        /// <returns>A disposable object used to unsubscribe from the observable sequence.</returns>
        public static IDisposable Subscribe<T>(this IObservable<T> source, Action<T> onNext) => Subscribe(source, onNext, ex => throw ex, () => { });

        /// <summary>
        /// Subscribes to the specified <paramref name="source"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the observable sequence.</typeparam>
        /// <param name="source">The observable sequence to subscribe to.</param>
        /// <param name="onNext">The action to invoke for each element produced by the sequence.</param>
        /// <param name="onError">The action to invoke for an error produced by the sequence.</param>
        /// <param name="onCompleted">The action to invoke upon successful termination of the sequence.</param>
        /// <returns>A disposable object used to unsubscribe from the observable sequence.</returns>
        public static IDisposable Subscribe<T>(this IObservable<T> source, Action<T> onNext, Action<Exception> onError, Action onCompleted) => source.Subscribe(new AnonymousObserver<T>(onNext, onError, onCompleted));
    }
}
