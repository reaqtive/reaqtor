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

namespace Reaqtor
{
    /// <summary>
    /// Interface for a parameterized subscription factory.
    /// </summary>
    /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
    public interface IReactiveQubscriptionFactory<TArg1, TArg2> : IReactiveSubscriptionFactory<TArg1, TArg2>, IReactiveExpressible
    {
        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="arg1">First parameter to pass to the subscription factory.</param>
        /// <param name="arg2">Second parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        new IReactiveQubscription Create(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, object state = null);
    }

    /// <summary>
    /// Interface for a parameterized subscription factory.
    /// </summary>
    /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg3">Type of the third parameter passed to the subscription factory.</typeparam>
    public interface IReactiveQubscriptionFactory<TArg1, TArg2, TArg3> : IReactiveSubscriptionFactory<TArg1, TArg2, TArg3>, IReactiveExpressible
    {
        /// <summary>
        /// Creates a new subscription with the specified subscription URI.
        /// </summary>
        /// <param name="subscriptionUri">URI identifying the subscription.</param>
        /// <param name="arg1">First parameter to pass to the subscription factory.</param>
        /// <param name="arg2">Second parameter to pass to the subscription factory.</param>
        /// <param name="arg3">Third parameter to pass to the subscription factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>A subscription object, or an exception if the creation request was unsuccessful.</returns>
        new IReactiveQubscription Create(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state = null);
    }

    /// <summary>
    /// Interface for a parameterized subscription factory.
    /// </summary>
    /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg3">Type of the third parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg4">Type of the fourth parameter passed to the subscription factory.</typeparam>
    public interface IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4> : IReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4>, IReactiveExpressible
    {
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
        new IReactiveQubscription Create(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, object state = null);
    }

    /// <summary>
    /// Interface for a parameterized subscription factory.
    /// </summary>
    /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg3">Type of the third parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg4">Type of the fourth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg5">Type of the fifth parameter passed to the subscription factory.</typeparam>
    public interface IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5> : IReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5>, IReactiveExpressible
    {
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
        new IReactiveQubscription Create(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, object state = null);
    }

    /// <summary>
    /// Interface for a parameterized subscription factory.
    /// </summary>
    /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg3">Type of the third parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg4">Type of the fourth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg5">Type of the fifth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg6">Type of the sixth parameter passed to the subscription factory.</typeparam>
    public interface IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> : IReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>, IReactiveExpressible
    {
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
        new IReactiveQubscription Create(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, object state = null);
    }

    /// <summary>
    /// Interface for a parameterized subscription factory.
    /// </summary>
    /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg3">Type of the third parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg4">Type of the fourth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg5">Type of the fifth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg6">Type of the sixth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg7">Type of the seventh parameter passed to the subscription factory.</typeparam>
    public interface IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> : IReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>, IReactiveExpressible
    {
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
        new IReactiveQubscription Create(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, object state = null);
    }

    /// <summary>
    /// Interface for a parameterized subscription factory.
    /// </summary>
    /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg3">Type of the third parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg4">Type of the fourth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg5">Type of the fifth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg6">Type of the sixth parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg7">Type of the seventh parameter passed to the subscription factory.</typeparam>
    /// <typeparam name="TArg8">Type of the eighth parameter passed to the subscription factory.</typeparam>
    public interface IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> : IReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>, IReactiveExpressible
    {
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
        new IReactiveQubscription Create(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, object state = null);
    }

    /// <summary>
    /// Interface for a parameterized subscription factory.
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
    public interface IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> : IReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>, IReactiveExpressible
    {
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
        new IReactiveQubscription Create(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, object state = null);
    }

    /// <summary>
    /// Interface for a parameterized subscription factory.
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
    public interface IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> : IReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>, IReactiveExpressible
    {
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
        new IReactiveQubscription Create(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, object state = null);
    }

    /// <summary>
    /// Interface for a parameterized subscription factory.
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
    public interface IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> : IReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>, IReactiveExpressible
    {
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
        new IReactiveQubscription Create(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, object state = null);
    }

    /// <summary>
    /// Interface for a parameterized subscription factory.
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
    public interface IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> : IReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>, IReactiveExpressible
    {
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
        new IReactiveQubscription Create(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, object state = null);
    }

    /// <summary>
    /// Interface for a parameterized subscription factory.
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
    public interface IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> : IReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>, IReactiveExpressible
    {
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
        new IReactiveQubscription Create(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, object state = null);
    }

    /// <summary>
    /// Interface for a parameterized subscription factory.
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
    public interface IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> : IReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>, IReactiveExpressible
    {
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
        new IReactiveQubscription Create(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, object state = null);
    }

    /// <summary>
    /// Interface for a parameterized subscription factory.
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
    public interface IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> : IReactiveSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>, IReactiveExpressible
    {
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
        new IReactiveQubscription Create(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, TArg15 arg15, object state = null);
    }

}