// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - Feburary 2016 - Created this file.
// Auto-generated file, changes to source may be lost
//

using System;
using System.Linq.Expressions;

namespace Reaqtor
{
    /// <summary>
    /// Base class for the implementation of parameterized subscription factories represented by an expression tree.
    /// </summary>
    /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
    public abstract class ReactiveQubscriptionFactoryBase<TArg1, TArg2> : IReactiveQubscriptionFactory<TArg1, TArg2>
    {
        /// <summary>
        /// Creates a new subscription factory represented by an expression tree, using the specified associated query provider.
        /// </summary>
        /// <param name="provider">Query provider associated with the subscription factory.</param>
        protected ReactiveQubscriptionFactoryBase(IReactiveQueryProvider provider) => Provider = provider;

        /// <summary>
        /// Gets the query provider that is associated with the subscription factory.
        /// </summary>
        public IReactiveQueryProvider Provider { get; }

        /// <summary>
        /// Gets the expression tree representing the subscription factory.
        /// </summary>
        public abstract Expression Expression { get; }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="arg1">First parameter to pass to the subscription factory.</param>
        /// <param name="arg2">Second parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        public IReactiveQubscription Create(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, object state = null)
        {
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return CreateCore(subscriptionUri, arg1, arg2, state);
        }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="arg1">First parameter to pass to the subscription factory.</param>
        /// <param name="arg2">Second parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        IReactiveSubscription IReactiveSubscriptionFactory<TArg1, TArg2>.Create(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, object state)
        {
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return CreateCore(subscriptionUri, arg1, arg2, state);
        }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="arg1">First parameter to pass to the subscription factory.</param>
        /// <param name="arg2">Second parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        protected abstract IReactiveQubscription CreateCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, object state);
    }

    /// <summary>
    /// Base class for the implementation of parameterized subscription factories represented by an expression tree.
    /// </summary>
    /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg3">Type of the third parameter passed to the subscription factory.</typeparam>
    public abstract class ReactiveQubscriptionFactoryBase<TArg1, TArg2, TArg3> : IReactiveQubscriptionFactory<TArg1, TArg2, TArg3>
    {
        /// <summary>
        /// Creates a new subscription factory represented by an expression tree, using the specified associated query provider.
        /// </summary>
        /// <param name="provider">Query provider associated with the subscription factory.</param>
        protected ReactiveQubscriptionFactoryBase(IReactiveQueryProvider provider) => Provider = provider;

        /// <summary>
        /// Gets the query provider that is associated with the subscription factory.
        /// </summary>
        public IReactiveQueryProvider Provider { get; }

        /// <summary>
        /// Gets the expression tree representing the subscription factory.
        /// </summary>
        public abstract Expression Expression { get; }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="arg1">First parameter to pass to the subscription factory.</param>
        /// <param name="arg2">Second parameter to pass to the subscription factory.</param>
        /// <param name="arg3">Third parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        public IReactiveQubscription Create(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state = null)
        {
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return CreateCore(subscriptionUri, arg1, arg2, arg3, state);
        }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="arg1">First parameter to pass to the subscription factory.</param>
        /// <param name="arg2">Second parameter to pass to the subscription factory.</param>
        /// <param name="arg3">Third parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        IReactiveSubscription IReactiveSubscriptionFactory<TArg1, TArg2, TArg3>.Create(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state)
        {
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return CreateCore(subscriptionUri, arg1, arg2, arg3, state);
        }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="arg1">First parameter to pass to the subscription factory.</param>
        /// <param name="arg2">Second parameter to pass to the subscription factory.</param>
        /// <param name="arg3">Third parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        protected abstract IReactiveQubscription CreateCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state);
    }

    /// <summary>
    /// Base class for the implementation of parameterized subscription factories represented by an expression tree.
    /// </summary>
    /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg3">Type of the third parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg4">Type of the fourth parameter passed to the subscription factory.</typeparam>
    public abstract class ReactiveQubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4> : IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4>
    {
        /// <summary>
        /// Creates a new subscription factory represented by an expression tree, using the specified associated query provider.
        /// </summary>
        /// <param name="provider">Query provider associated with the subscription factory.</param>
        protected ReactiveQubscriptionFactoryBase(IReactiveQueryProvider provider) => Provider = provider;

        /// <summary>
        /// Gets the query provider that is associated with the subscription factory.
        /// </summary>
        public IReactiveQueryProvider Provider { get; }

        /// <summary>
        /// Gets the expression tree representing the subscription factory.
        /// </summary>
        public abstract Expression Expression { get; }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="arg1">First parameter to pass to the subscription factory.</param>
        /// <param name="arg2">Second parameter to pass to the subscription factory.</param>
        /// <param name="arg3">Third parameter to pass to the subscription factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        public IReactiveQubscription Create(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, object state = null)
        {
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return CreateCore(subscriptionUri, arg1, arg2, arg3, arg4, state);
        }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="arg1">First parameter to pass to the subscription factory.</param>
        /// <param name="arg2">Second parameter to pass to the subscription factory.</param>
        /// <param name="arg3">Third parameter to pass to the subscription factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        IReactiveSubscription IReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4>.Create(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, object state)
        {
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return CreateCore(subscriptionUri, arg1, arg2, arg3, arg4, state);
        }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="arg1">First parameter to pass to the subscription factory.</param>
        /// <param name="arg2">Second parameter to pass to the subscription factory.</param>
        /// <param name="arg3">Third parameter to pass to the subscription factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        protected abstract IReactiveQubscription CreateCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, object state);
    }

    /// <summary>
    /// Base class for the implementation of parameterized subscription factories represented by an expression tree.
    /// </summary>
    /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg3">Type of the third parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg4">Type of the fourth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg5">Type of the fifth parameter passed to the subscription factory.</typeparam>
    public abstract class ReactiveQubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4, TArg5> : IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5>
    {
        /// <summary>
        /// Creates a new subscription factory represented by an expression tree, using the specified associated query provider.
        /// </summary>
        /// <param name="provider">Query provider associated with the subscription factory.</param>
        protected ReactiveQubscriptionFactoryBase(IReactiveQueryProvider provider) => Provider = provider;

        /// <summary>
        /// Gets the query provider that is associated with the subscription factory.
        /// </summary>
        public IReactiveQueryProvider Provider { get; }

        /// <summary>
        /// Gets the expression tree representing the subscription factory.
        /// </summary>
        public abstract Expression Expression { get; }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="arg1">First parameter to pass to the subscription factory.</param>
        /// <param name="arg2">Second parameter to pass to the subscription factory.</param>
        /// <param name="arg3">Third parameter to pass to the subscription factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subscription factory.</param>
        /// <param name="arg5">Fifth parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        public IReactiveQubscription Create(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, object state = null)
        {
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return CreateCore(subscriptionUri, arg1, arg2, arg3, arg4, arg5, state);
        }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="arg1">First parameter to pass to the subscription factory.</param>
        /// <param name="arg2">Second parameter to pass to the subscription factory.</param>
        /// <param name="arg3">Third parameter to pass to the subscription factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subscription factory.</param>
        /// <param name="arg5">Fifth parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        IReactiveSubscription IReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5>.Create(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, object state)
        {
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return CreateCore(subscriptionUri, arg1, arg2, arg3, arg4, arg5, state);
        }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="arg1">First parameter to pass to the subscription factory.</param>
        /// <param name="arg2">Second parameter to pass to the subscription factory.</param>
        /// <param name="arg3">Third parameter to pass to the subscription factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subscription factory.</param>
        /// <param name="arg5">Fifth parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        protected abstract IReactiveQubscription CreateCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, object state);
    }

    /// <summary>
    /// Base class for the implementation of parameterized subscription factories represented by an expression tree.
    /// </summary>
    /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg3">Type of the third parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg4">Type of the fourth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg5">Type of the fifth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg6">Type of the sixth parameter passed to the subscription factory.</typeparam>
    public abstract class ReactiveQubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> : IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>
    {
        /// <summary>
        /// Creates a new subscription factory represented by an expression tree, using the specified associated query provider.
        /// </summary>
        /// <param name="provider">Query provider associated with the subscription factory.</param>
        protected ReactiveQubscriptionFactoryBase(IReactiveQueryProvider provider) => Provider = provider;

        /// <summary>
        /// Gets the query provider that is associated with the subscription factory.
        /// </summary>
        public IReactiveQueryProvider Provider { get; }

        /// <summary>
        /// Gets the expression tree representing the subscription factory.
        /// </summary>
        public abstract Expression Expression { get; }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="arg1">First parameter to pass to the subscription factory.</param>
        /// <param name="arg2">Second parameter to pass to the subscription factory.</param>
        /// <param name="arg3">Third parameter to pass to the subscription factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subscription factory.</param>
        /// <param name="arg5">Fifth parameter to pass to the subscription factory.</param>
        /// <param name="arg6">Sixth parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        public IReactiveQubscription Create(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, object state = null)
        {
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return CreateCore(subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, state);
        }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="arg1">First parameter to pass to the subscription factory.</param>
        /// <param name="arg2">Second parameter to pass to the subscription factory.</param>
        /// <param name="arg3">Third parameter to pass to the subscription factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subscription factory.</param>
        /// <param name="arg5">Fifth parameter to pass to the subscription factory.</param>
        /// <param name="arg6">Sixth parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        IReactiveSubscription IReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>.Create(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, object state)
        {
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return CreateCore(subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, state);
        }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="arg1">First parameter to pass to the subscription factory.</param>
        /// <param name="arg2">Second parameter to pass to the subscription factory.</param>
        /// <param name="arg3">Third parameter to pass to the subscription factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subscription factory.</param>
        /// <param name="arg5">Fifth parameter to pass to the subscription factory.</param>
        /// <param name="arg6">Sixth parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        protected abstract IReactiveQubscription CreateCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, object state);
    }

    /// <summary>
    /// Base class for the implementation of parameterized subscription factories represented by an expression tree.
    /// </summary>
    /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg3">Type of the third parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg4">Type of the fourth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg5">Type of the fifth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg6">Type of the sixth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg7">Type of the seventh parameter passed to the subscription factory.</typeparam>
    public abstract class ReactiveQubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> : IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>
    {
        /// <summary>
        /// Creates a new subscription factory represented by an expression tree, using the specified associated query provider.
        /// </summary>
        /// <param name="provider">Query provider associated with the subscription factory.</param>
        protected ReactiveQubscriptionFactoryBase(IReactiveQueryProvider provider) => Provider = provider;

        /// <summary>
        /// Gets the query provider that is associated with the subscription factory.
        /// </summary>
        public IReactiveQueryProvider Provider { get; }

        /// <summary>
        /// Gets the expression tree representing the subscription factory.
        /// </summary>
        public abstract Expression Expression { get; }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="arg1">First parameter to pass to the subscription factory.</param>
        /// <param name="arg2">Second parameter to pass to the subscription factory.</param>
        /// <param name="arg3">Third parameter to pass to the subscription factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subscription factory.</param>
        /// <param name="arg5">Fifth parameter to pass to the subscription factory.</param>
        /// <param name="arg6">Sixth parameter to pass to the subscription factory.</param>
        /// <param name="arg7">Seventh parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        public IReactiveQubscription Create(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, object state = null)
        {
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return CreateCore(subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, state);
        }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="arg1">First parameter to pass to the subscription factory.</param>
        /// <param name="arg2">Second parameter to pass to the subscription factory.</param>
        /// <param name="arg3">Third parameter to pass to the subscription factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subscription factory.</param>
        /// <param name="arg5">Fifth parameter to pass to the subscription factory.</param>
        /// <param name="arg6">Sixth parameter to pass to the subscription factory.</param>
        /// <param name="arg7">Seventh parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        IReactiveSubscription IReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>.Create(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, object state)
        {
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return CreateCore(subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, state);
        }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="arg1">First parameter to pass to the subscription factory.</param>
        /// <param name="arg2">Second parameter to pass to the subscription factory.</param>
        /// <param name="arg3">Third parameter to pass to the subscription factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subscription factory.</param>
        /// <param name="arg5">Fifth parameter to pass to the subscription factory.</param>
        /// <param name="arg6">Sixth parameter to pass to the subscription factory.</param>
        /// <param name="arg7">Seventh parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        protected abstract IReactiveQubscription CreateCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, object state);
    }

    /// <summary>
    /// Base class for the implementation of parameterized subscription factories represented by an expression tree.
    /// </summary>
    /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg3">Type of the third parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg4">Type of the fourth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg5">Type of the fifth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg6">Type of the sixth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg7">Type of the seventh parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg8">Type of the eighth parameter passed to the subscription factory.</typeparam>
    public abstract class ReactiveQubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> : IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>
    {
        /// <summary>
        /// Creates a new subscription factory represented by an expression tree, using the specified associated query provider.
        /// </summary>
        /// <param name="provider">Query provider associated with the subscription factory.</param>
        protected ReactiveQubscriptionFactoryBase(IReactiveQueryProvider provider) => Provider = provider;

        /// <summary>
        /// Gets the query provider that is associated with the subscription factory.
        /// </summary>
        public IReactiveQueryProvider Provider { get; }

        /// <summary>
        /// Gets the expression tree representing the subscription factory.
        /// </summary>
        public abstract Expression Expression { get; }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="arg1">First parameter to pass to the subscription factory.</param>
        /// <param name="arg2">Second parameter to pass to the subscription factory.</param>
        /// <param name="arg3">Third parameter to pass to the subscription factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subscription factory.</param>
        /// <param name="arg5">Fifth parameter to pass to the subscription factory.</param>
        /// <param name="arg6">Sixth parameter to pass to the subscription factory.</param>
        /// <param name="arg7">Seventh parameter to pass to the subscription factory.</param>
        /// <param name="arg8">Eighth parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        public IReactiveQubscription Create(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, object state = null)
        {
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return CreateCore(subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, state);
        }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="arg1">First parameter to pass to the subscription factory.</param>
        /// <param name="arg2">Second parameter to pass to the subscription factory.</param>
        /// <param name="arg3">Third parameter to pass to the subscription factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subscription factory.</param>
        /// <param name="arg5">Fifth parameter to pass to the subscription factory.</param>
        /// <param name="arg6">Sixth parameter to pass to the subscription factory.</param>
        /// <param name="arg7">Seventh parameter to pass to the subscription factory.</param>
        /// <param name="arg8">Eighth parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        IReactiveSubscription IReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>.Create(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, object state)
        {
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return CreateCore(subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, state);
        }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="arg1">First parameter to pass to the subscription factory.</param>
        /// <param name="arg2">Second parameter to pass to the subscription factory.</param>
        /// <param name="arg3">Third parameter to pass to the subscription factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subscription factory.</param>
        /// <param name="arg5">Fifth parameter to pass to the subscription factory.</param>
        /// <param name="arg6">Sixth parameter to pass to the subscription factory.</param>
        /// <param name="arg7">Seventh parameter to pass to the subscription factory.</param>
        /// <param name="arg8">Eighth parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        protected abstract IReactiveQubscription CreateCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, object state);
    }

    /// <summary>
    /// Base class for the implementation of parameterized subscription factories represented by an expression tree.
    /// </summary>
    /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg3">Type of the third parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg4">Type of the fourth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg5">Type of the fifth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg6">Type of the sixth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg7">Type of the seventh parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg8">Type of the eighth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg9">Type of the ninth parameter passed to the subscription factory.</typeparam>
    public abstract class ReactiveQubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> : IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>
    {
        /// <summary>
        /// Creates a new subscription factory represented by an expression tree, using the specified associated query provider.
        /// </summary>
        /// <param name="provider">Query provider associated with the subscription factory.</param>
        protected ReactiveQubscriptionFactoryBase(IReactiveQueryProvider provider) => Provider = provider;

        /// <summary>
        /// Gets the query provider that is associated with the subscription factory.
        /// </summary>
        public IReactiveQueryProvider Provider { get; }

        /// <summary>
        /// Gets the expression tree representing the subscription factory.
        /// </summary>
        public abstract Expression Expression { get; }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="arg1">First parameter to pass to the subscription factory.</param>
        /// <param name="arg2">Second parameter to pass to the subscription factory.</param>
        /// <param name="arg3">Third parameter to pass to the subscription factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subscription factory.</param>
        /// <param name="arg5">Fifth parameter to pass to the subscription factory.</param>
        /// <param name="arg6">Sixth parameter to pass to the subscription factory.</param>
        /// <param name="arg7">Seventh parameter to pass to the subscription factory.</param>
        /// <param name="arg8">Eighth parameter to pass to the subscription factory.</param>
        /// <param name="arg9">Ninth parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        public IReactiveQubscription Create(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, object state = null)
        {
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return CreateCore(subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, state);
        }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="arg1">First parameter to pass to the subscription factory.</param>
        /// <param name="arg2">Second parameter to pass to the subscription factory.</param>
        /// <param name="arg3">Third parameter to pass to the subscription factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subscription factory.</param>
        /// <param name="arg5">Fifth parameter to pass to the subscription factory.</param>
        /// <param name="arg6">Sixth parameter to pass to the subscription factory.</param>
        /// <param name="arg7">Seventh parameter to pass to the subscription factory.</param>
        /// <param name="arg8">Eighth parameter to pass to the subscription factory.</param>
        /// <param name="arg9">Ninth parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        IReactiveSubscription IReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>.Create(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, object state)
        {
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return CreateCore(subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, state);
        }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="arg1">First parameter to pass to the subscription factory.</param>
        /// <param name="arg2">Second parameter to pass to the subscription factory.</param>
        /// <param name="arg3">Third parameter to pass to the subscription factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subscription factory.</param>
        /// <param name="arg5">Fifth parameter to pass to the subscription factory.</param>
        /// <param name="arg6">Sixth parameter to pass to the subscription factory.</param>
        /// <param name="arg7">Seventh parameter to pass to the subscription factory.</param>
        /// <param name="arg8">Eighth parameter to pass to the subscription factory.</param>
        /// <param name="arg9">Ninth parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        protected abstract IReactiveQubscription CreateCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, object state);
    }

    /// <summary>
    /// Base class for the implementation of parameterized subscription factories represented by an expression tree.
    /// </summary>
    /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg3">Type of the third parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg4">Type of the fourth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg5">Type of the fifth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg6">Type of the sixth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg7">Type of the seventh parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg8">Type of the eighth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg9">Type of the ninth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg10">Type of the tenth parameter passed to the subscription factory.</typeparam>
    public abstract class ReactiveQubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> : IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>
    {
        /// <summary>
        /// Creates a new subscription factory represented by an expression tree, using the specified associated query provider.
        /// </summary>
        /// <param name="provider">Query provider associated with the subscription factory.</param>
        protected ReactiveQubscriptionFactoryBase(IReactiveQueryProvider provider) => Provider = provider;

        /// <summary>
        /// Gets the query provider that is associated with the subscription factory.
        /// </summary>
        public IReactiveQueryProvider Provider { get; }

        /// <summary>
        /// Gets the expression tree representing the subscription factory.
        /// </summary>
        public abstract Expression Expression { get; }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="arg1">First parameter to pass to the subscription factory.</param>
        /// <param name="arg2">Second parameter to pass to the subscription factory.</param>
        /// <param name="arg3">Third parameter to pass to the subscription factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subscription factory.</param>
        /// <param name="arg5">Fifth parameter to pass to the subscription factory.</param>
        /// <param name="arg6">Sixth parameter to pass to the subscription factory.</param>
        /// <param name="arg7">Seventh parameter to pass to the subscription factory.</param>
        /// <param name="arg8">Eighth parameter to pass to the subscription factory.</param>
        /// <param name="arg9">Ninth parameter to pass to the subscription factory.</param>
        /// <param name="arg10">Tenth parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        public IReactiveQubscription Create(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, object state = null)
        {
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return CreateCore(subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, state);
        }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="arg1">First parameter to pass to the subscription factory.</param>
        /// <param name="arg2">Second parameter to pass to the subscription factory.</param>
        /// <param name="arg3">Third parameter to pass to the subscription factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subscription factory.</param>
        /// <param name="arg5">Fifth parameter to pass to the subscription factory.</param>
        /// <param name="arg6">Sixth parameter to pass to the subscription factory.</param>
        /// <param name="arg7">Seventh parameter to pass to the subscription factory.</param>
        /// <param name="arg8">Eighth parameter to pass to the subscription factory.</param>
        /// <param name="arg9">Ninth parameter to pass to the subscription factory.</param>
        /// <param name="arg10">Tenth parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        IReactiveSubscription IReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>.Create(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, object state)
        {
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return CreateCore(subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, state);
        }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="arg1">First parameter to pass to the subscription factory.</param>
        /// <param name="arg2">Second parameter to pass to the subscription factory.</param>
        /// <param name="arg3">Third parameter to pass to the subscription factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subscription factory.</param>
        /// <param name="arg5">Fifth parameter to pass to the subscription factory.</param>
        /// <param name="arg6">Sixth parameter to pass to the subscription factory.</param>
        /// <param name="arg7">Seventh parameter to pass to the subscription factory.</param>
        /// <param name="arg8">Eighth parameter to pass to the subscription factory.</param>
        /// <param name="arg9">Ninth parameter to pass to the subscription factory.</param>
        /// <param name="arg10">Tenth parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        protected abstract IReactiveQubscription CreateCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, object state);
    }

    /// <summary>
    /// Base class for the implementation of parameterized subscription factories represented by an expression tree.
    /// </summary>
    /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg3">Type of the third parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg4">Type of the fourth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg5">Type of the fifth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg6">Type of the sixth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg7">Type of the seventh parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg8">Type of the eighth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg9">Type of the ninth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg10">Type of the tenth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg11">Type of the eleventh parameter passed to the subscription factory.</typeparam>
    public abstract class ReactiveQubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> : IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>
    {
        /// <summary>
        /// Creates a new subscription factory represented by an expression tree, using the specified associated query provider.
        /// </summary>
        /// <param name="provider">Query provider associated with the subscription factory.</param>
        protected ReactiveQubscriptionFactoryBase(IReactiveQueryProvider provider) => Provider = provider;

        /// <summary>
        /// Gets the query provider that is associated with the subscription factory.
        /// </summary>
        public IReactiveQueryProvider Provider { get; }

        /// <summary>
        /// Gets the expression tree representing the subscription factory.
        /// </summary>
        public abstract Expression Expression { get; }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="arg1">First parameter to pass to the subscription factory.</param>
        /// <param name="arg2">Second parameter to pass to the subscription factory.</param>
        /// <param name="arg3">Third parameter to pass to the subscription factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subscription factory.</param>
        /// <param name="arg5">Fifth parameter to pass to the subscription factory.</param>
        /// <param name="arg6">Sixth parameter to pass to the subscription factory.</param>
        /// <param name="arg7">Seventh parameter to pass to the subscription factory.</param>
        /// <param name="arg8">Eighth parameter to pass to the subscription factory.</param>
        /// <param name="arg9">Ninth parameter to pass to the subscription factory.</param>
        /// <param name="arg10">Tenth parameter to pass to the subscription factory.</param>
        /// <param name="arg11">Eleventh parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        public IReactiveQubscription Create(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, object state = null)
        {
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return CreateCore(subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, state);
        }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="arg1">First parameter to pass to the subscription factory.</param>
        /// <param name="arg2">Second parameter to pass to the subscription factory.</param>
        /// <param name="arg3">Third parameter to pass to the subscription factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subscription factory.</param>
        /// <param name="arg5">Fifth parameter to pass to the subscription factory.</param>
        /// <param name="arg6">Sixth parameter to pass to the subscription factory.</param>
        /// <param name="arg7">Seventh parameter to pass to the subscription factory.</param>
        /// <param name="arg8">Eighth parameter to pass to the subscription factory.</param>
        /// <param name="arg9">Ninth parameter to pass to the subscription factory.</param>
        /// <param name="arg10">Tenth parameter to pass to the subscription factory.</param>
        /// <param name="arg11">Eleventh parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        IReactiveSubscription IReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>.Create(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, object state)
        {
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return CreateCore(subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, state);
        }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="arg1">First parameter to pass to the subscription factory.</param>
        /// <param name="arg2">Second parameter to pass to the subscription factory.</param>
        /// <param name="arg3">Third parameter to pass to the subscription factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subscription factory.</param>
        /// <param name="arg5">Fifth parameter to pass to the subscription factory.</param>
        /// <param name="arg6">Sixth parameter to pass to the subscription factory.</param>
        /// <param name="arg7">Seventh parameter to pass to the subscription factory.</param>
        /// <param name="arg8">Eighth parameter to pass to the subscription factory.</param>
        /// <param name="arg9">Ninth parameter to pass to the subscription factory.</param>
        /// <param name="arg10">Tenth parameter to pass to the subscription factory.</param>
        /// <param name="arg11">Eleventh parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        protected abstract IReactiveQubscription CreateCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, object state);
    }

    /// <summary>
    /// Base class for the implementation of parameterized subscription factories represented by an expression tree.
    /// </summary>
    /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg3">Type of the third parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg4">Type of the fourth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg5">Type of the fifth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg6">Type of the sixth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg7">Type of the seventh parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg8">Type of the eighth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg9">Type of the ninth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg10">Type of the tenth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg11">Type of the eleventh parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg12">Type of the twelfth parameter passed to the subscription factory.</typeparam>
    public abstract class ReactiveQubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> : IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>
    {
        /// <summary>
        /// Creates a new subscription factory represented by an expression tree, using the specified associated query provider.
        /// </summary>
        /// <param name="provider">Query provider associated with the subscription factory.</param>
        protected ReactiveQubscriptionFactoryBase(IReactiveQueryProvider provider) => Provider = provider;

        /// <summary>
        /// Gets the query provider that is associated with the subscription factory.
        /// </summary>
        public IReactiveQueryProvider Provider { get; }

        /// <summary>
        /// Gets the expression tree representing the subscription factory.
        /// </summary>
        public abstract Expression Expression { get; }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="arg1">First parameter to pass to the subscription factory.</param>
        /// <param name="arg2">Second parameter to pass to the subscription factory.</param>
        /// <param name="arg3">Third parameter to pass to the subscription factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subscription factory.</param>
        /// <param name="arg5">Fifth parameter to pass to the subscription factory.</param>
        /// <param name="arg6">Sixth parameter to pass to the subscription factory.</param>
        /// <param name="arg7">Seventh parameter to pass to the subscription factory.</param>
        /// <param name="arg8">Eighth parameter to pass to the subscription factory.</param>
        /// <param name="arg9">Ninth parameter to pass to the subscription factory.</param>
        /// <param name="arg10">Tenth parameter to pass to the subscription factory.</param>
        /// <param name="arg11">Eleventh parameter to pass to the subscription factory.</param>
        /// <param name="arg12">Twelfth parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        public IReactiveQubscription Create(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, object state = null)
        {
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return CreateCore(subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, state);
        }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="arg1">First parameter to pass to the subscription factory.</param>
        /// <param name="arg2">Second parameter to pass to the subscription factory.</param>
        /// <param name="arg3">Third parameter to pass to the subscription factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subscription factory.</param>
        /// <param name="arg5">Fifth parameter to pass to the subscription factory.</param>
        /// <param name="arg6">Sixth parameter to pass to the subscription factory.</param>
        /// <param name="arg7">Seventh parameter to pass to the subscription factory.</param>
        /// <param name="arg8">Eighth parameter to pass to the subscription factory.</param>
        /// <param name="arg9">Ninth parameter to pass to the subscription factory.</param>
        /// <param name="arg10">Tenth parameter to pass to the subscription factory.</param>
        /// <param name="arg11">Eleventh parameter to pass to the subscription factory.</param>
        /// <param name="arg12">Twelfth parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        IReactiveSubscription IReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>.Create(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, object state)
        {
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return CreateCore(subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, state);
        }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="arg1">First parameter to pass to the subscription factory.</param>
        /// <param name="arg2">Second parameter to pass to the subscription factory.</param>
        /// <param name="arg3">Third parameter to pass to the subscription factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subscription factory.</param>
        /// <param name="arg5">Fifth parameter to pass to the subscription factory.</param>
        /// <param name="arg6">Sixth parameter to pass to the subscription factory.</param>
        /// <param name="arg7">Seventh parameter to pass to the subscription factory.</param>
        /// <param name="arg8">Eighth parameter to pass to the subscription factory.</param>
        /// <param name="arg9">Ninth parameter to pass to the subscription factory.</param>
        /// <param name="arg10">Tenth parameter to pass to the subscription factory.</param>
        /// <param name="arg11">Eleventh parameter to pass to the subscription factory.</param>
        /// <param name="arg12">Twelfth parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        protected abstract IReactiveQubscription CreateCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, object state);
    }

    /// <summary>
    /// Base class for the implementation of parameterized subscription factories represented by an expression tree.
    /// </summary>
    /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg3">Type of the third parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg4">Type of the fourth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg5">Type of the fifth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg6">Type of the sixth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg7">Type of the seventh parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg8">Type of the eighth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg9">Type of the ninth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg10">Type of the tenth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg11">Type of the eleventh parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg12">Type of the twelfth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg13">Type of the thirteenth parameter passed to the subscription factory.</typeparam>
    public abstract class ReactiveQubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> : IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>
    {
        /// <summary>
        /// Creates a new subscription factory represented by an expression tree, using the specified associated query provider.
        /// </summary>
        /// <param name="provider">Query provider associated with the subscription factory.</param>
        protected ReactiveQubscriptionFactoryBase(IReactiveQueryProvider provider) => Provider = provider;

        /// <summary>
        /// Gets the query provider that is associated with the subscription factory.
        /// </summary>
        public IReactiveQueryProvider Provider { get; }

        /// <summary>
        /// Gets the expression tree representing the subscription factory.
        /// </summary>
        public abstract Expression Expression { get; }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="arg1">First parameter to pass to the subscription factory.</param>
        /// <param name="arg2">Second parameter to pass to the subscription factory.</param>
        /// <param name="arg3">Third parameter to pass to the subscription factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subscription factory.</param>
        /// <param name="arg5">Fifth parameter to pass to the subscription factory.</param>
        /// <param name="arg6">Sixth parameter to pass to the subscription factory.</param>
        /// <param name="arg7">Seventh parameter to pass to the subscription factory.</param>
        /// <param name="arg8">Eighth parameter to pass to the subscription factory.</param>
        /// <param name="arg9">Ninth parameter to pass to the subscription factory.</param>
        /// <param name="arg10">Tenth parameter to pass to the subscription factory.</param>
        /// <param name="arg11">Eleventh parameter to pass to the subscription factory.</param>
        /// <param name="arg12">Twelfth parameter to pass to the subscription factory.</param>
        /// <param name="arg13">Thirteenth parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        public IReactiveQubscription Create(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, object state = null)
        {
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return CreateCore(subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, state);
        }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="arg1">First parameter to pass to the subscription factory.</param>
        /// <param name="arg2">Second parameter to pass to the subscription factory.</param>
        /// <param name="arg3">Third parameter to pass to the subscription factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subscription factory.</param>
        /// <param name="arg5">Fifth parameter to pass to the subscription factory.</param>
        /// <param name="arg6">Sixth parameter to pass to the subscription factory.</param>
        /// <param name="arg7">Seventh parameter to pass to the subscription factory.</param>
        /// <param name="arg8">Eighth parameter to pass to the subscription factory.</param>
        /// <param name="arg9">Ninth parameter to pass to the subscription factory.</param>
        /// <param name="arg10">Tenth parameter to pass to the subscription factory.</param>
        /// <param name="arg11">Eleventh parameter to pass to the subscription factory.</param>
        /// <param name="arg12">Twelfth parameter to pass to the subscription factory.</param>
        /// <param name="arg13">Thirteenth parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        IReactiveSubscription IReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>.Create(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, object state)
        {
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return CreateCore(subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, state);
        }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="arg1">First parameter to pass to the subscription factory.</param>
        /// <param name="arg2">Second parameter to pass to the subscription factory.</param>
        /// <param name="arg3">Third parameter to pass to the subscription factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subscription factory.</param>
        /// <param name="arg5">Fifth parameter to pass to the subscription factory.</param>
        /// <param name="arg6">Sixth parameter to pass to the subscription factory.</param>
        /// <param name="arg7">Seventh parameter to pass to the subscription factory.</param>
        /// <param name="arg8">Eighth parameter to pass to the subscription factory.</param>
        /// <param name="arg9">Ninth parameter to pass to the subscription factory.</param>
        /// <param name="arg10">Tenth parameter to pass to the subscription factory.</param>
        /// <param name="arg11">Eleventh parameter to pass to the subscription factory.</param>
        /// <param name="arg12">Twelfth parameter to pass to the subscription factory.</param>
        /// <param name="arg13">Thirteenth parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        protected abstract IReactiveQubscription CreateCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, object state);
    }

    /// <summary>
    /// Base class for the implementation of parameterized subscription factories represented by an expression tree.
    /// </summary>
    /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg3">Type of the third parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg4">Type of the fourth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg5">Type of the fifth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg6">Type of the sixth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg7">Type of the seventh parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg8">Type of the eighth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg9">Type of the ninth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg10">Type of the tenth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg11">Type of the eleventh parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg12">Type of the twelfth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg13">Type of the thirteenth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg14">Type of the fourteenth parameter passed to the subscription factory.</typeparam>
    public abstract class ReactiveQubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> : IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>
    {
        /// <summary>
        /// Creates a new subscription factory represented by an expression tree, using the specified associated query provider.
        /// </summary>
        /// <param name="provider">Query provider associated with the subscription factory.</param>
        protected ReactiveQubscriptionFactoryBase(IReactiveQueryProvider provider) => Provider = provider;

        /// <summary>
        /// Gets the query provider that is associated with the subscription factory.
        /// </summary>
        public IReactiveQueryProvider Provider { get; }

        /// <summary>
        /// Gets the expression tree representing the subscription factory.
        /// </summary>
        public abstract Expression Expression { get; }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="arg1">First parameter to pass to the subscription factory.</param>
        /// <param name="arg2">Second parameter to pass to the subscription factory.</param>
        /// <param name="arg3">Third parameter to pass to the subscription factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subscription factory.</param>
        /// <param name="arg5">Fifth parameter to pass to the subscription factory.</param>
        /// <param name="arg6">Sixth parameter to pass to the subscription factory.</param>
        /// <param name="arg7">Seventh parameter to pass to the subscription factory.</param>
        /// <param name="arg8">Eighth parameter to pass to the subscription factory.</param>
        /// <param name="arg9">Ninth parameter to pass to the subscription factory.</param>
        /// <param name="arg10">Tenth parameter to pass to the subscription factory.</param>
        /// <param name="arg11">Eleventh parameter to pass to the subscription factory.</param>
        /// <param name="arg12">Twelfth parameter to pass to the subscription factory.</param>
        /// <param name="arg13">Thirteenth parameter to pass to the subscription factory.</param>
        /// <param name="arg14">Fourteenth parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        public IReactiveQubscription Create(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, object state = null)
        {
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return CreateCore(subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, state);
        }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="arg1">First parameter to pass to the subscription factory.</param>
        /// <param name="arg2">Second parameter to pass to the subscription factory.</param>
        /// <param name="arg3">Third parameter to pass to the subscription factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subscription factory.</param>
        /// <param name="arg5">Fifth parameter to pass to the subscription factory.</param>
        /// <param name="arg6">Sixth parameter to pass to the subscription factory.</param>
        /// <param name="arg7">Seventh parameter to pass to the subscription factory.</param>
        /// <param name="arg8">Eighth parameter to pass to the subscription factory.</param>
        /// <param name="arg9">Ninth parameter to pass to the subscription factory.</param>
        /// <param name="arg10">Tenth parameter to pass to the subscription factory.</param>
        /// <param name="arg11">Eleventh parameter to pass to the subscription factory.</param>
        /// <param name="arg12">Twelfth parameter to pass to the subscription factory.</param>
        /// <param name="arg13">Thirteenth parameter to pass to the subscription factory.</param>
        /// <param name="arg14">Fourteenth parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        IReactiveSubscription IReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>.Create(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, object state)
        {
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return CreateCore(subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, state);
        }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="arg1">First parameter to pass to the subscription factory.</param>
        /// <param name="arg2">Second parameter to pass to the subscription factory.</param>
        /// <param name="arg3">Third parameter to pass to the subscription factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subscription factory.</param>
        /// <param name="arg5">Fifth parameter to pass to the subscription factory.</param>
        /// <param name="arg6">Sixth parameter to pass to the subscription factory.</param>
        /// <param name="arg7">Seventh parameter to pass to the subscription factory.</param>
        /// <param name="arg8">Eighth parameter to pass to the subscription factory.</param>
        /// <param name="arg9">Ninth parameter to pass to the subscription factory.</param>
        /// <param name="arg10">Tenth parameter to pass to the subscription factory.</param>
        /// <param name="arg11">Eleventh parameter to pass to the subscription factory.</param>
        /// <param name="arg12">Twelfth parameter to pass to the subscription factory.</param>
        /// <param name="arg13">Thirteenth parameter to pass to the subscription factory.</param>
        /// <param name="arg14">Fourteenth parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        protected abstract IReactiveQubscription CreateCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, object state);
    }

    /// <summary>
    /// Base class for the implementation of parameterized subscription factories represented by an expression tree.
    /// </summary>
    /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg3">Type of the third parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg4">Type of the fourth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg5">Type of the fifth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg6">Type of the sixth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg7">Type of the seventh parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg8">Type of the eighth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg9">Type of the ninth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg10">Type of the tenth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg11">Type of the eleventh parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg12">Type of the twelfth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg13">Type of the thirteenth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg14">Type of the fourteenth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg15">Type of the fifteenth parameter passed to the subscription factory.</typeparam>
    public abstract class ReactiveQubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> : IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>
    {
        /// <summary>
        /// Creates a new subscription factory represented by an expression tree, using the specified associated query provider.
        /// </summary>
        /// <param name="provider">Query provider associated with the subscription factory.</param>
        protected ReactiveQubscriptionFactoryBase(IReactiveQueryProvider provider) => Provider = provider;

        /// <summary>
        /// Gets the query provider that is associated with the subscription factory.
        /// </summary>
        public IReactiveQueryProvider Provider { get; }

        /// <summary>
        /// Gets the expression tree representing the subscription factory.
        /// </summary>
        public abstract Expression Expression { get; }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="arg1">First parameter to pass to the subscription factory.</param>
        /// <param name="arg2">Second parameter to pass to the subscription factory.</param>
        /// <param name="arg3">Third parameter to pass to the subscription factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subscription factory.</param>
        /// <param name="arg5">Fifth parameter to pass to the subscription factory.</param>
        /// <param name="arg6">Sixth parameter to pass to the subscription factory.</param>
        /// <param name="arg7">Seventh parameter to pass to the subscription factory.</param>
        /// <param name="arg8">Eighth parameter to pass to the subscription factory.</param>
        /// <param name="arg9">Ninth parameter to pass to the subscription factory.</param>
        /// <param name="arg10">Tenth parameter to pass to the subscription factory.</param>
        /// <param name="arg11">Eleventh parameter to pass to the subscription factory.</param>
        /// <param name="arg12">Twelfth parameter to pass to the subscription factory.</param>
        /// <param name="arg13">Thirteenth parameter to pass to the subscription factory.</param>
        /// <param name="arg14">Fourteenth parameter to pass to the subscription factory.</param>
        /// <param name="arg15">Fifteenth parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        public IReactiveQubscription Create(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, TArg15 arg15, object state = null)
        {
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return CreateCore(subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, state);
        }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="arg1">First parameter to pass to the subscription factory.</param>
        /// <param name="arg2">Second parameter to pass to the subscription factory.</param>
        /// <param name="arg3">Third parameter to pass to the subscription factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subscription factory.</param>
        /// <param name="arg5">Fifth parameter to pass to the subscription factory.</param>
        /// <param name="arg6">Sixth parameter to pass to the subscription factory.</param>
        /// <param name="arg7">Seventh parameter to pass to the subscription factory.</param>
        /// <param name="arg8">Eighth parameter to pass to the subscription factory.</param>
        /// <param name="arg9">Ninth parameter to pass to the subscription factory.</param>
        /// <param name="arg10">Tenth parameter to pass to the subscription factory.</param>
        /// <param name="arg11">Eleventh parameter to pass to the subscription factory.</param>
        /// <param name="arg12">Twelfth parameter to pass to the subscription factory.</param>
        /// <param name="arg13">Thirteenth parameter to pass to the subscription factory.</param>
        /// <param name="arg14">Fourteenth parameter to pass to the subscription factory.</param>
        /// <param name="arg15">Fifteenth parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        IReactiveSubscription IReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>.Create(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, TArg15 arg15, object state)
        {
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return CreateCore(subscriptionUri, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, state);
        }

        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="arg1">First parameter to pass to the subscription factory.</param>
        /// <param name="arg2">Second parameter to pass to the subscription factory.</param>
        /// <param name="arg3">Third parameter to pass to the subscription factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subscription factory.</param>
        /// <param name="arg5">Fifth parameter to pass to the subscription factory.</param>
        /// <param name="arg6">Sixth parameter to pass to the subscription factory.</param>
        /// <param name="arg7">Seventh parameter to pass to the subscription factory.</param>
        /// <param name="arg8">Eighth parameter to pass to the subscription factory.</param>
        /// <param name="arg9">Ninth parameter to pass to the subscription factory.</param>
        /// <param name="arg10">Tenth parameter to pass to the subscription factory.</param>
        /// <param name="arg11">Eleventh parameter to pass to the subscription factory.</param>
        /// <param name="arg12">Twelfth parameter to pass to the subscription factory.</param>
        /// <param name="arg13">Thirteenth parameter to pass to the subscription factory.</param>
        /// <param name="arg14">Fourteenth parameter to pass to the subscription factory.</param>
        /// <param name="arg15">Fifteenth parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        protected abstract IReactiveQubscription CreateCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, TArg15 arg15, object state);
    }

}