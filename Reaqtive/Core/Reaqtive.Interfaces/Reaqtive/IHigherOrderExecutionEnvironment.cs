// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;

namespace Reaqtive
{
    /// <summary>
    /// This interface supports hosting infrastructure for the operator library in a query engine.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IHigherOrderExecutionEnvironment : IExecutionEnvironment
    {
        /// <summary>
        /// Creates a bridge for a higher-order input subscription.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="subscribable">The upstream subscribable; should be quoted.</param>
        /// <param name="observer">The volatile downstream observer.</param>
        /// <param name="parent">The parent context.</param>
        /// <returns>A persistable subscription handle.</returns>
        ISubscription CreateBridge<T>(ISubscribable<T> subscribable, IObserver<T> observer, IOperatorContext parent);

        /// <summary>
        /// Loads a bridge from checkpoint state.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="reader">The reader to load from.</param>
        /// <param name="observer">The volatile downstream observer to reconnect.</param>
        /// <param name="parent">The parent context.</param>
        /// <returns>A persistable subscription handle.</returns>
        ISubscription LoadBridge<T>(IOperatorStateReader reader, IObserver<T> observer, IOperatorContext parent);

        /// <summary>
        /// Saves the bridge to checkpoint state.
        /// </summary>
        /// <param name="subscription">The persistable subscription obtained from Create or Load.</param>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="parent">The parent context.</param>
        void SaveBridge(ISubscription subscription, IOperatorStateWriter writer, IOperatorContext parent);

        //
        // CONSIDER: The methods below are suboptimal from a layering point of view because the caller needs to cook up URIs.
        //           It'd be better to follow the approach we took for bridges, by returning a persistable handle, so that we
        //           can hide the URI. Note we can incorporate the parent's URI in generated URIs as well, for tracing purposes.
        //           If we go that route, there will be some issues to support IDependencyOperator, and we likely want to split
        //           the CreateSimpleSubject case into a tollbooth and a collector.
        //
        //           However, this approach is already much better than the IReactive dependency in the heart of the operator
        //           library and effectively provides an injection mechanism to be used by a hosting environment.
        //

        /// <summary>
        /// Creates a simple subject.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="uri">The URI used to identify the subject.</param>
        /// <param name="parent">The parent context.</param>
        /// <returns>A persisted simple subject instance.</returns>
        IMultiSubject<T, T> CreateSimpleSubject<T>(Uri uri, IOperatorContext parent);

        /// <summary>
        /// Creates a reference counted subject.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="uri">The URI used to identify the subject.</param>
        /// <param name="tollbooth">The URI of the tollbooth subject (see remarks in HigherOrderOutputStatefulOperatorBase).</param>
        /// <param name="collector">The URI of the collector subject (see remarks in HigherOrderOutputStatefulOperatorBase).</param>
        /// <param name="parent">The parent context.</param>
        /// <returns>A persisted reference counted subject instance.</returns>
        IMultiSubject<T, T> CreateRefCountSubject<T>(Uri uri, Uri tollbooth, Uri collector, IOperatorContext parent);

        /// <summary>
        /// Deletes a subject.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="uri">The URI used to identify the subject to delete.</param>
        /// <param name="parent">The parent context.</param>
        void DeleteSubject<T>(Uri uri, IOperatorContext parent);

        /// <summary>
        /// Creates a quoted proxy around the given group subject.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys.</typeparam>
        /// <typeparam name="TElement">The type of the elements.</typeparam>
        /// <param name="subject">The subject to quote.</param>
        /// <param name="uri">The URI of the subject.</param>
        /// <returns>A proxy to the subject with a quote attached to it.</returns>
        IGroupedSubscribable<TKey, TElement> Quote<TKey, TElement>(IGroupedMultiSubject<TKey, TElement> subject, Uri uri);

        /// <summary>
        /// Creates a quoted proxy around the given subject.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="subject">The subject to quote.</param>
        /// <param name="uri">The URI of the subject.</param>
        /// <returns>A proxy to the subject with a quote attached to it.</returns>
        ISubscribable<T> Quote<T>(IMultiSubject<T, T> subject, Uri uri);
    }
}
