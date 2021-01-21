// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtive
{
    /// <summary>
    /// Represents a subject that supports multiple observers.
    /// </summary>
    /// <typeparam name="TInput">Type of the input elements received by the observers that push data into the subject.</typeparam>
    /// <typeparam name="TOutput">Type of the output elements produced by the subject.</typeparam>
    public interface IMultiSubject<TInput, TOutput> : ISubscribable<TOutput>, IDisposable
    {
        /// <summary>
        /// Creates a new observer that can be used to push elements into the subject.
        /// </summary>
        /// <returns>An observer that can be used to push elements into the subject.</returns>
        IObserver<TInput> CreateObserver();
    }

    /// <summary>
    /// Represents a subject that supports multiple observers.
    /// </summary>
    /// <typeparam name="T">Type of the elements processed by the subject.</typeparam>
    public interface IMultiSubject<T> : IMultiSubject<T, T>
    {
    }

    /// <summary>
    /// A non-generic multi-subject, useful to support streams of data that can
    /// be subscribed to with types that may change (i.e., due to versioning)
    /// over time.
    /// </summary>
    public interface IMultiSubject : IDisposable
    {
        /// <summary>
        /// Gets an observer to push elements into the subject.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <returns>An observer that can push elements into the subject.</returns>
        IObserver<T> GetObserver<T>();

        /// <summary>
        /// Gets a subscribable to subscribe to the subject.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <returns>A subscribable that can receive elements of the subject.</returns>
        ISubscribable<T> GetObservable<T>();
    }

    /// <summary>
    /// A multi-subject that can be typed at runtime.
    /// </summary>
    public interface IDynamicMultiSubject : IMultiSubject
    {
        /// <summary>
        /// Creates a typed version of the untyped multi-subject.
        /// </summary>
        /// <typeparam name="TInput">The input element type of the typed multi-subject.</typeparam>
        /// <typeparam name="TOutput">The output element type of the typed multi-subject.</typeparam>
        /// <returns>The typed multi-subject.</returns>
        IMultiSubject<TInput, TOutput> ToTyped<TInput, TOutput>();
    }
}
