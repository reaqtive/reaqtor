// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

using Reaqtive;
using Reaqtive.Expressions;

using Reaqtor.Reactive.Expressions;

namespace Reaqtor.Reactive
{
    /// <summary>
    /// This interface supports hosting infrastructure for the operator library in a query engine.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public partial class HigherOrderExecutionEnvironment : IHigherOrderExecutionEnvironment
    {
#pragma warning disable IDE0034 // Simplify 'default' expression (used in ReflectionHelpers.InfoOf calls)

        private static readonly ConstructorInfo s_ctorUri = (ConstructorInfo)ReflectionHelpers.InfoOf(() => new Uri(default(string)));
        private static readonly Uri s_bridgeSubjectUri = new("rx://subject/bridge");
        private static readonly Uri s_innerSubjectUri = new("rx://subject/inner");
        private static readonly Uri s_refCountSubjectUri = new("rx://subject/inner/refCount");

        private readonly IExecutionEnvironment _environment;
        private readonly IReactive _reactive;

        /// <summary>
        /// Creates a new execution environment.
        /// </summary>
        /// <param name="environment">The underlying base environment.</param>
        /// <param name="reactive">The reactive service used to manage auxiliary artifacts.</param>
        public HigherOrderExecutionEnvironment(IExecutionEnvironment environment, IReactive reactive)
        {
            _environment = environment;
            _reactive = reactive;
        }

        /// <summary>
        /// Gets the subject with the specified identifier from the execution environment.
        /// </summary>
        /// <typeparam name="TInput">Type of the elements received by the subject.</typeparam>
        /// <typeparam name="TOutput">Type of the elements produced by the subject.</typeparam>
        /// <param name="uri">Identifier of the subject.</param>
        /// <returns>Subject with the specified identifier, obtained from the execution environment.</returns>
        public IMultiSubject<TInput, TOutput> GetSubject<TInput, TOutput>(Uri uri) => _environment.GetSubject<TInput, TOutput>(uri);

        /// <summary>
        /// Gets the subscription with the specified identifier from the execution environment.
        /// </summary>
        /// <param name="uri">Identifier of the subscription.</param>
        /// <returns>Subscription with the specified identifier, obtained from the execution environment.</returns>
        public ISubscription GetSubscription(Uri uri) => _environment.GetSubscription(uri);

        /// <summary>
        /// Creates a bridge for a higher-order input subscription.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="subscribable">The upstream subscribable; should be quoted.</param>
        /// <param name="observer">The volatile downstream observer.</param>
        /// <param name="parent">The parent context.</param>
        /// <returns>A persistable subscription handle.</returns>
        public ISubscription CreateBridge<T>(ISubscribable<T> subscribable, IObserver<T> observer, IOperatorContext parent)
        {
            if (subscribable == null)
                throw new ArgumentNullException(nameof(subscribable));
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (parent == null)
                throw new ArgumentNullException(nameof(parent));

            if (subscribable is not IQubscribable<T> innerQuoted)
            {
                var msg = string.Format(CultureInfo.InvariantCulture, "Higher order operator '{0}' received an input sequence that wasn't quoted and cannot be persisted for checkpointing.", GetType().FullName);
                throw new HigherOrderSubscriptionFailedException(msg);
            }

            var bridgeUri = new Uri("rx://bridge/v2/" + Guid.NewGuid().ToString("D"));

            parent.TraceSource.HigherOrderOperator_CreatingBridge(bridgeUri, parent.InstanceId, innerQuoted.Expression);

            var streamFactory = _reactive.GetStreamFactory<Expression, T, T>(s_bridgeSubjectUri);
            streamFactory.Create(bridgeUri, innerQuoted.Expression, state: null);

            return new PersistableSubscription<T>(bridgeUri, observer);
        }

        /// <summary>
        /// Loads a bridge from checkpoint state.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="reader">The reader to load from.</param>
        /// <param name="observer">The volatile downstream observer to reconnect.</param>
        /// <param name="parent">The parent context.</param>
        /// <returns>A persistable subscription handle.</returns>
        public ISubscription LoadBridge<T>(IOperatorStateReader reader, IObserver<T> observer, IOperatorContext parent)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (parent == null)
                throw new ArgumentNullException(nameof(parent));

            var subscription = new PersistableSubscription<T>(null, observer);

            subscription.Load(reader);
            subscription.SetContext(parent);

            return subscription;
        }

        /// <summary>
        /// Saves the bridge to checkpoint state.
        /// </summary>
        /// <param name="subscription">The persistable subscription obtained from Create or Load.</param>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="parent">The parent context.</param>
        public void SaveBridge(ISubscription subscription, IOperatorStateWriter writer, IOperatorContext parent)
        {
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            if (parent == null)
                throw new ArgumentNullException(nameof(parent));

            if (subscription is IPersistable persist)
            {
                persist.Save(writer);
            }
            else
            {
                throw new NotSupportedException("Specified subscription cannot be persisted.");
            }
        }

        /// <summary>
        /// Creates a simple subject.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="uri">The URI used to identify the subject.</param>
        /// <param name="parent">The parent context.</param>
        /// <returns>A persisted simple subject instance.</returns>
        public IMultiSubject<T, T> CreateSimpleSubject<T>(Uri uri, IOperatorContext parent)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));
            if (parent == null)
                throw new ArgumentNullException(nameof(parent));

            var factory = _reactive.GetStreamFactory<T, T>(s_innerSubjectUri);
            factory.Create(uri, state: null);

            return _environment.GetSubject<T, T>(uri);
        }

        /// <summary>
        /// Creates a reference counted subject.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="uri">The URI used to identify the subject.</param>
        /// <param name="tollbooth">The URI of the tollbooth subject (see remarks in HigherOrderOutputStatefulOperatorBase).</param>
        /// <param name="collector">The URI of the collector subject (see remarks in HigherOrderOutputStatefulOperatorBase).</param>
        /// <param name="parent">The parent context.</param>
        /// <returns>A persisted reference counted subject instance.</returns>
        public IMultiSubject<T, T> CreateRefCountSubject<T>(Uri uri, Uri tollbooth, Uri collector, IOperatorContext parent)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));
            if (tollbooth == null)
                throw new ArgumentNullException(nameof(tollbooth));
            if (collector == null)
                throw new ArgumentNullException(nameof(collector));
            if (parent == null)
                throw new ArgumentNullException(nameof(parent));

            var factory = _reactive.GetStreamFactory<Tuple<Uri, Uri>, T, T>(s_refCountSubjectUri);
            var resourceManagementUris = Tuple.Create(tollbooth, collector);
            factory.Create(uri, resourceManagementUris, state: null);

            return _environment.GetSubject<T, T>(uri);
        }

        /// <summary>
        /// Deletes a subject.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="uri">The URI used to identify the subject to delete.</param>
        /// <param name="parent">The parent context.</param>
        public void DeleteSubject<T>(Uri uri, IOperatorContext parent)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));
            if (parent == null)
                throw new ArgumentNullException(nameof(parent));

            _reactive.GetStream<T, T>(uri).Dispose();
        }

        /// <summary>
        /// Creates a quoted proxy around the given group subject.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys.</typeparam>
        /// <typeparam name="TElement">The type of the elements.</typeparam>
        /// <param name="subject">The subject to quote.</param>
        /// <param name="uri">The URI of the subject.</param>
        /// <returns>A proxy to the subject with a quote attached to it.</returns>
        public IGroupedSubscribable<TKey, TElement> Quote<TKey, TElement>(IGroupedMultiSubject<TKey, TElement> subject, Uri uri)
        {
            if (subject == null)
                throw new ArgumentNullException(nameof(subject));
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            var expression = Expression.New(
                GroupedMultiSubjectProxyInfo<TKey, TElement>.Constructor,
                Expression.New(s_ctorUri, Expression.Constant(uri.AbsoluteUri)),
                Expression.Constant(subject.Key, typeof(TKey))
            );

            return new QuotedGroupedSubscribable<TKey, TElement>(subject, expression);
        }

        private static class GroupedMultiSubjectProxyInfo<TKey, TElement>
        {
            /// <summary>
            /// Constructor info for new GroupedMultiSubjectProxy{TKey, TElement}(Uri, TKey).
            /// </summary>
            public static readonly ConstructorInfo Constructor = (ConstructorInfo)ReflectionHelpers.InfoOf(() => new GroupedMultiSubjectProxy<TKey, TElement>(default(Uri), default(TKey)));
        }

        /// <summary>
        /// Creates a quoted proxy around the given subject.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="subject">The subject to quote.</param>
        /// <param name="uri">The URI of the subject.</param>
        /// <returns>A proxy to the subject with a quote attached to it.</returns>
        public ISubscribable<T> Quote<T>(IMultiSubject<T, T> subject, Uri uri)
        {
            if (subject == null)
                throw new ArgumentNullException(nameof(subject));
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            var expression = Expression.New(
                MultiSubjectProxyInfo<T>.Constructor,
                Expression.New(s_ctorUri, Expression.Constant(uri.AbsoluteUri))
            );

            return new QuotedSubscribable<T>(subject, expression);
        }

        private static class MultiSubjectProxyInfo<T>
        {
            /// <summary>
            /// Constructor info for new MultiSubjectProxy{TSource, TSource}(Uri).
            /// </summary>
            public static readonly ConstructorInfo Constructor = (ConstructorInfo)ReflectionHelpers.InfoOf(() => new MultiSubjectProxy<T, T>(default(Uri)));
        }

#pragma warning restore IDE0034 // Simplify 'default' expression
    }
}
