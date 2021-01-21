// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtive;

namespace Reaqtor.Reactive
{
#pragma warning disable IDE0079 // Remove unnecessary suppression (only on .NET 5.0)
#pragma warning disable CA1063 // Provide overridable Dispose(bool) (non-traditional use of IDisposable)
#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize (non-traditional use of IDisposable)

    /// <summary>
    /// A proxy for subjects.
    /// </summary>
    public class MultiSubjectProxy : IMultiSubject
    {
        private readonly Uri _uri;

        /// <summary>
        /// Create the subject proxy.
        /// </summary>
        /// <param name="uri">The subject URI.</param>
        public MultiSubjectProxy(Uri uri)
        {
            _uri = uri ?? throw new ArgumentNullException(nameof(uri));
        }

        /// <summary>
        /// Gets an observer to push elements into the subject.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <returns>An observer that can push elements into the subject.</returns>
        public IObserver<T> GetObserver<T>()
        {
            return new MultiSubjectObserverProxy<T>(_uri);
        }

        /// <summary>
        /// Gets a subscribable to subscribe to the subject.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <returns>A subscribable that can receive elements of the subject.</returns>
        public ISubscribable<T> GetObservable<T>()
        {
            return new ObservableProxy<T>(_uri);
        }

        /// <summary>
        /// Dispose the subject proxy.
        /// </summary>
        public void Dispose() { }

        private sealed class ObservableProxy<T> : SubscribableBase<T>
        {
            private readonly Uri _uri;

            public ObservableProxy(Uri uri)
            {
                _uri = uri;
            }

            protected override ISubscription SubscribeCore(IObserver<T> observer)
            {
                return new MultiSubjectSubscriptionProxy<T, T>(_uri, observer);
            }
        }
    }

    /// <summary>
    /// A proxy for subjects.
    /// </summary>
    /// <typeparam name="TInput">The subject input type.</typeparam>
    /// <typeparam name="TOutput">The subject output type.</typeparam>
    public class MultiSubjectProxy<TInput, TOutput> : IMultiSubject<TInput, TOutput>
    {
        private readonly Uri _uri;

        /// <summary>
        /// Create the subject proxy.
        /// </summary>
        /// <param name="uri">The subject URI.</param>
        public MultiSubjectProxy(Uri uri)
        {
            _uri = uri ?? throw new ArgumentNullException(nameof(uri));
        }

        /// <summary>
        /// Creates a new observer that can be used to push elements into the subject.
        /// </summary>
        /// <returns>An observer that can be used to push elements into the subject.</returns>
        public IObserver<TInput> CreateObserver()
        {
            return new MultiSubjectObserverProxy<TInput>(_uri);
        }

        /// <summary>
        /// Subscribes the specified observer to the subscribable source.
        /// </summary>
        /// <param name="observer">Observer that will receive the elements of the source.</param>
        /// <returns>Handle to the newly created subscription.</returns>
        public ISubscription Subscribe(IObserver<TOutput> observer)
        {
            return new MultiSubjectSubscriptionProxy<TInput, TOutput>(_uri, observer);
        }

        /// <summary>
        /// Subscribes the specified observer to the observable source.
        /// </summary>
        /// <param name="observer">Observer that will receive the elements of the source.</param>
        /// <returns>A disposable handle to the newly created subscription.</returns>
        IDisposable IObservable<TOutput>.Subscribe(IObserver<TOutput> observer)
        {
            return Subscribe(observer);
        }

        /// <summary>
        /// Dispose the subject proxy.
        /// </summary>
        public void Dispose() { }
    }

#pragma warning restore CA1816
#pragma warning restore IDE0079
}
