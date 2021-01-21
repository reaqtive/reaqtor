// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtive
{
    /// <summary>
    /// Provides a set of methods to create multi-subjects.
    /// </summary>
    public static class MultiSubject
    {
        /// <summary>
        /// Converts a non-generic multi-subject to the generic variety.
        /// </summary>
        /// <typeparam name="TInput">The observer-side type of the subject.</typeparam>
        /// <typeparam name="TOutput">The observable-side type of the subject.</typeparam>
        /// <param name="subject">The multi-subject to wrap.</param>
        /// <returns>The wrapped multi-subject.</returns>
        public static IMultiSubject<TInput, TOutput> ToTyped<TInput, TOutput>(this IMultiSubject subject)
        {
            return new ToTypedMultiSubject<TInput, TOutput>(subject);
        }
    }

    /// <summary>
    /// Base class for multi-subjects.
    /// </summary>
    /// <typeparam name="T">Type of the elements processed by the subject.</typeparam>
    public abstract class MultiSubjectBase<T> : IMultiSubject<T>
    {
        /// <summary>
        /// Creates a new observer that can be used to push elements into the subject.
        /// </summary>
        /// <returns>An observer that can be used to push elements into the subject.</returns>
        public IObserver<T> CreateObserver()
        {
            return CreateObserverCore();
        }

        /// <summary>
        /// Creates a new observer that can be used to push elements into the subject.
        /// </summary>
        /// <returns>An observer that can be used to push elements into the subject.</returns>
        protected abstract IObserver<T> CreateObserverCore();

        /// <summary>
        /// Subscribes the specified observer to the subscribable source.
        /// </summary>
        /// <param name="observer">Observer that will receive the elements of the source.</param>
        /// <returns>Handle to the newly created subscription.</returns>
        public ISubscription Subscribe(IObserver<T> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return SubscribeCore(observer);
        }

        /// <summary>
        /// Subscribes the specified observer to the subscribable source.
        /// </summary>
        /// <param name="observer">Observer that will receive the elements of the source.</param>
        /// <returns>Handle to the newly created subscription.</returns>
        protected abstract ISubscription SubscribeCore(IObserver<T> observer);

        IDisposable IObservable<T>.Subscribe(IObserver<T> observer)
        {
            return Subscribe(observer);
        }

        /// <summary>
        /// Disposes the subject instance.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the subject instance.
        /// </summary>
        /// <param name="disposing">Indicates whether the method is called by Dispose or a finalizer.</param>
        protected abstract void Dispose(bool disposing);
    }

    // TODO: Deprecate the class below, in favor of the abstract base class and a concrete implementation of a subject.

    /// <summary>
    /// Base class for multi-subjects.
    /// </summary>
    /// <typeparam name="T">Type of the elements processed by the subject.</typeparam>
    public class MultiSubject<T> : MultiSubjectBase<T>
    {
        /// <summary>
        /// Creates a new observer that can be used to push elements into the subject.
        /// </summary>
        /// <returns>An observer that can be used to push elements into the subject.</returns>
        protected override IObserver<T> CreateObserverCore()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Subscribes the specified observer to the subscribable source.
        /// </summary>
        /// <param name="observer">Observer that will receive the elements of the source.</param>
        /// <returns>Handle to the newly created subscription.</returns>
        protected override ISubscription SubscribeCore(IObserver<T> observer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Disposes the subject instance.
        /// </summary>
        /// <param name="disposing">Indicates whether the method is called by Dispose or a finalizer.</param>
        protected override void Dispose(bool disposing)
        {
        }
    }
}
