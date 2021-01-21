// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD, ER - August 2013 - Created this file.
// Auto-generated file, changes to source may be lost
//

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor
{
    /// <summary>
    /// Interface for a parameterized subject factory.
    /// </summary>
    /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
    /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
    /// <typeparam name="TArg1">Type of the first parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg2">Type of the second parameter passed to the subject factory.</typeparam>
    public interface IAsyncReactiveSubjectFactory<TInput, TOutput, TArg1, TArg2>
    {
        /// <summary>
        /// Creates a new subject with the specified stream URI.
        /// </summary>
        /// <param name="streamUri">URI identifying the stream.</param>
        /// <param name="arg1">First parameter to pass to the subject factory.</param>
        /// <param name="arg2">Second parameter to pass to the subject factory.</param>
        /// <param name="state">Additional state to pass to the operation. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task returning a subject object that can be used to send and receive data on the stream, or an exception if the creation request was unsuccessful.</returns>
        Task<IAsyncReactiveSubject<TInput, TOutput>> CreateAsync(Uri streamUri, TArg1 arg1, TArg2 arg2, object state = null, CancellationToken token = default);
    }

    /// <summary>
    /// Interface for a parameterized subject factory.
    /// </summary>
    /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
    /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
    /// <typeparam name="TArg1">Type of the first parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg2">Type of the second parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg3">Type of the third parameter passed to the subject factory.</typeparam>
    public interface IAsyncReactiveSubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3>
    {
        /// <summary>
        /// Creates a new subject with the specified stream URI.
        /// </summary>
        /// <param name="streamUri">URI identifying the stream.</param>
        /// <param name="arg1">First parameter to pass to the subject factory.</param>
        /// <param name="arg2">Second parameter to pass to the subject factory.</param>
        /// <param name="arg3">Third parameter to pass to the subject factory.</param>
        /// <param name="state">Additional state to pass to the operation. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task returning a subject object that can be used to send and receive data on the stream, or an exception if the creation request was unsuccessful.</returns>
        Task<IAsyncReactiveSubject<TInput, TOutput>> CreateAsync(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state = null, CancellationToken token = default);
    }

    /// <summary>
    /// Interface for a parameterized subject factory.
    /// </summary>
    /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
    /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
    /// <typeparam name="TArg1">Type of the first parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg2">Type of the second parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg3">Type of the third parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg4">Type of the fourth parameter passed to the subject factory.</typeparam>
    public interface IAsyncReactiveSubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4>
    {
        /// <summary>
        /// Creates a new subject with the specified stream URI.
        /// </summary>
        /// <param name="streamUri">URI identifying the stream.</param>
        /// <param name="arg1">First parameter to pass to the subject factory.</param>
        /// <param name="arg2">Second parameter to pass to the subject factory.</param>
        /// <param name="arg3">Third parameter to pass to the subject factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subject factory.</param>
        /// <param name="state">Additional state to pass to the operation. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task returning a subject object that can be used to send and receive data on the stream, or an exception if the creation request was unsuccessful.</returns>
        Task<IAsyncReactiveSubject<TInput, TOutput>> CreateAsync(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, object state = null, CancellationToken token = default);
    }

    /// <summary>
    /// Interface for a parameterized subject factory.
    /// </summary>
    /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
    /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
    /// <typeparam name="TArg1">Type of the first parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg2">Type of the second parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg3">Type of the third parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg4">Type of the fourth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg5">Type of the fifth parameter passed to the subject factory.</typeparam>
    public interface IAsyncReactiveSubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5>
    {
        /// <summary>
        /// Creates a new subject with the specified stream URI.
        /// </summary>
        /// <param name="streamUri">URI identifying the stream.</param>
        /// <param name="arg1">First parameter to pass to the subject factory.</param>
        /// <param name="arg2">Second parameter to pass to the subject factory.</param>
        /// <param name="arg3">Third parameter to pass to the subject factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subject factory.</param>
        /// <param name="arg5">Fifth parameter to pass to the subject factory.</param>
        /// <param name="state">Additional state to pass to the operation. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task returning a subject object that can be used to send and receive data on the stream, or an exception if the creation request was unsuccessful.</returns>
        Task<IAsyncReactiveSubject<TInput, TOutput>> CreateAsync(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, object state = null, CancellationToken token = default);
    }

    /// <summary>
    /// Interface for a parameterized subject factory.
    /// </summary>
    /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
    /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
    /// <typeparam name="TArg1">Type of the first parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg2">Type of the second parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg3">Type of the third parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg4">Type of the fourth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg5">Type of the fifth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg6">Type of the sixth parameter passed to the subject factory.</typeparam>
    public interface IAsyncReactiveSubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>
    {
        /// <summary>
        /// Creates a new subject with the specified stream URI.
        /// </summary>
        /// <param name="streamUri">URI identifying the stream.</param>
        /// <param name="arg1">First parameter to pass to the subject factory.</param>
        /// <param name="arg2">Second parameter to pass to the subject factory.</param>
        /// <param name="arg3">Third parameter to pass to the subject factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subject factory.</param>
        /// <param name="arg5">Fifth parameter to pass to the subject factory.</param>
        /// <param name="arg6">Sixth parameter to pass to the subject factory.</param>
        /// <param name="state">Additional state to pass to the operation. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task returning a subject object that can be used to send and receive data on the stream, or an exception if the creation request was unsuccessful.</returns>
        Task<IAsyncReactiveSubject<TInput, TOutput>> CreateAsync(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, object state = null, CancellationToken token = default);
    }

    /// <summary>
    /// Interface for a parameterized subject factory.
    /// </summary>
    /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
    /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
    /// <typeparam name="TArg1">Type of the first parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg2">Type of the second parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg3">Type of the third parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg4">Type of the fourth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg5">Type of the fifth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg6">Type of the sixth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg7">Type of the seventh parameter passed to the subject factory.</typeparam>
    public interface IAsyncReactiveSubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>
    {
        /// <summary>
        /// Creates a new subject with the specified stream URI.
        /// </summary>
        /// <param name="streamUri">URI identifying the stream.</param>
        /// <param name="arg1">First parameter to pass to the subject factory.</param>
        /// <param name="arg2">Second parameter to pass to the subject factory.</param>
        /// <param name="arg3">Third parameter to pass to the subject factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subject factory.</param>
        /// <param name="arg5">Fifth parameter to pass to the subject factory.</param>
        /// <param name="arg6">Sixth parameter to pass to the subject factory.</param>
        /// <param name="arg7">Seventh parameter to pass to the subject factory.</param>
        /// <param name="state">Additional state to pass to the operation. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task returning a subject object that can be used to send and receive data on the stream, or an exception if the creation request was unsuccessful.</returns>
        Task<IAsyncReactiveSubject<TInput, TOutput>> CreateAsync(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, object state = null, CancellationToken token = default);
    }

    /// <summary>
    /// Interface for a parameterized subject factory.
    /// </summary>
    /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
    /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
    /// <typeparam name="TArg1">Type of the first parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg2">Type of the second parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg3">Type of the third parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg4">Type of the fourth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg5">Type of the fifth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg6">Type of the sixth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg7">Type of the seventh parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg8">Type of the eighth parameter passed to the subject factory.</typeparam>
    public interface IAsyncReactiveSubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>
    {
        /// <summary>
        /// Creates a new subject with the specified stream URI.
        /// </summary>
        /// <param name="streamUri">URI identifying the stream.</param>
        /// <param name="arg1">First parameter to pass to the subject factory.</param>
        /// <param name="arg2">Second parameter to pass to the subject factory.</param>
        /// <param name="arg3">Third parameter to pass to the subject factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subject factory.</param>
        /// <param name="arg5">Fifth parameter to pass to the subject factory.</param>
        /// <param name="arg6">Sixth parameter to pass to the subject factory.</param>
        /// <param name="arg7">Seventh parameter to pass to the subject factory.</param>
        /// <param name="arg8">Eighth parameter to pass to the subject factory.</param>
        /// <param name="state">Additional state to pass to the operation. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task returning a subject object that can be used to send and receive data on the stream, or an exception if the creation request was unsuccessful.</returns>
        Task<IAsyncReactiveSubject<TInput, TOutput>> CreateAsync(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, object state = null, CancellationToken token = default);
    }

    /// <summary>
    /// Interface for a parameterized subject factory.
    /// </summary>
    /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
    /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
    /// <typeparam name="TArg1">Type of the first parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg2">Type of the second parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg3">Type of the third parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg4">Type of the fourth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg5">Type of the fifth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg6">Type of the sixth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg7">Type of the seventh parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg8">Type of the eighth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg9">Type of the ninth parameter passed to the subject factory.</typeparam>
    public interface IAsyncReactiveSubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>
    {
        /// <summary>
        /// Creates a new subject with the specified stream URI.
        /// </summary>
        /// <param name="streamUri">URI identifying the stream.</param>
        /// <param name="arg1">First parameter to pass to the subject factory.</param>
        /// <param name="arg2">Second parameter to pass to the subject factory.</param>
        /// <param name="arg3">Third parameter to pass to the subject factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subject factory.</param>
        /// <param name="arg5">Fifth parameter to pass to the subject factory.</param>
        /// <param name="arg6">Sixth parameter to pass to the subject factory.</param>
        /// <param name="arg7">Seventh parameter to pass to the subject factory.</param>
        /// <param name="arg8">Eighth parameter to pass to the subject factory.</param>
        /// <param name="arg9">Ninth parameter to pass to the subject factory.</param>
        /// <param name="state">Additional state to pass to the operation. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task returning a subject object that can be used to send and receive data on the stream, or an exception if the creation request was unsuccessful.</returns>
        Task<IAsyncReactiveSubject<TInput, TOutput>> CreateAsync(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, object state = null, CancellationToken token = default);
    }

    /// <summary>
    /// Interface for a parameterized subject factory.
    /// </summary>
    /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
    /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
    /// <typeparam name="TArg1">Type of the first parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg2">Type of the second parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg3">Type of the third parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg4">Type of the fourth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg5">Type of the fifth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg6">Type of the sixth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg7">Type of the seventh parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg8">Type of the eighth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg9">Type of the ninth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg10">Type of the tenth parameter passed to the subject factory.</typeparam>
    public interface IAsyncReactiveSubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>
    {
        /// <summary>
        /// Creates a new subject with the specified stream URI.
        /// </summary>
        /// <param name="streamUri">URI identifying the stream.</param>
        /// <param name="arg1">First parameter to pass to the subject factory.</param>
        /// <param name="arg2">Second parameter to pass to the subject factory.</param>
        /// <param name="arg3">Third parameter to pass to the subject factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subject factory.</param>
        /// <param name="arg5">Fifth parameter to pass to the subject factory.</param>
        /// <param name="arg6">Sixth parameter to pass to the subject factory.</param>
        /// <param name="arg7">Seventh parameter to pass to the subject factory.</param>
        /// <param name="arg8">Eighth parameter to pass to the subject factory.</param>
        /// <param name="arg9">Ninth parameter to pass to the subject factory.</param>
        /// <param name="arg10">Tenth parameter to pass to the subject factory.</param>
        /// <param name="state">Additional state to pass to the operation. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task returning a subject object that can be used to send and receive data on the stream, or an exception if the creation request was unsuccessful.</returns>
        Task<IAsyncReactiveSubject<TInput, TOutput>> CreateAsync(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, object state = null, CancellationToken token = default);
    }

    /// <summary>
    /// Interface for a parameterized subject factory.
    /// </summary>
    /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
    /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
    /// <typeparam name="TArg1">Type of the first parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg2">Type of the second parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg3">Type of the third parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg4">Type of the fourth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg5">Type of the fifth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg6">Type of the sixth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg7">Type of the seventh parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg8">Type of the eighth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg9">Type of the ninth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg10">Type of the tenth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg11">Type of the eleventh parameter passed to the subject factory.</typeparam>
    public interface IAsyncReactiveSubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>
    {
        /// <summary>
        /// Creates a new subject with the specified stream URI.
        /// </summary>
        /// <param name="streamUri">URI identifying the stream.</param>
        /// <param name="arg1">First parameter to pass to the subject factory.</param>
        /// <param name="arg2">Second parameter to pass to the subject factory.</param>
        /// <param name="arg3">Third parameter to pass to the subject factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subject factory.</param>
        /// <param name="arg5">Fifth parameter to pass to the subject factory.</param>
        /// <param name="arg6">Sixth parameter to pass to the subject factory.</param>
        /// <param name="arg7">Seventh parameter to pass to the subject factory.</param>
        /// <param name="arg8">Eighth parameter to pass to the subject factory.</param>
        /// <param name="arg9">Ninth parameter to pass to the subject factory.</param>
        /// <param name="arg10">Tenth parameter to pass to the subject factory.</param>
        /// <param name="arg11">Eleventh parameter to pass to the subject factory.</param>
        /// <param name="state">Additional state to pass to the operation. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task returning a subject object that can be used to send and receive data on the stream, or an exception if the creation request was unsuccessful.</returns>
        Task<IAsyncReactiveSubject<TInput, TOutput>> CreateAsync(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, object state = null, CancellationToken token = default);
    }

    /// <summary>
    /// Interface for a parameterized subject factory.
    /// </summary>
    /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
    /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
    /// <typeparam name="TArg1">Type of the first parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg2">Type of the second parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg3">Type of the third parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg4">Type of the fourth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg5">Type of the fifth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg6">Type of the sixth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg7">Type of the seventh parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg8">Type of the eighth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg9">Type of the ninth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg10">Type of the tenth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg11">Type of the eleventh parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg12">Type of the twelfth parameter passed to the subject factory.</typeparam>
    public interface IAsyncReactiveSubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>
    {
        /// <summary>
        /// Creates a new subject with the specified stream URI.
        /// </summary>
        /// <param name="streamUri">URI identifying the stream.</param>
        /// <param name="arg1">First parameter to pass to the subject factory.</param>
        /// <param name="arg2">Second parameter to pass to the subject factory.</param>
        /// <param name="arg3">Third parameter to pass to the subject factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subject factory.</param>
        /// <param name="arg5">Fifth parameter to pass to the subject factory.</param>
        /// <param name="arg6">Sixth parameter to pass to the subject factory.</param>
        /// <param name="arg7">Seventh parameter to pass to the subject factory.</param>
        /// <param name="arg8">Eighth parameter to pass to the subject factory.</param>
        /// <param name="arg9">Ninth parameter to pass to the subject factory.</param>
        /// <param name="arg10">Tenth parameter to pass to the subject factory.</param>
        /// <param name="arg11">Eleventh parameter to pass to the subject factory.</param>
        /// <param name="arg12">Twelfth parameter to pass to the subject factory.</param>
        /// <param name="state">Additional state to pass to the operation. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task returning a subject object that can be used to send and receive data on the stream, or an exception if the creation request was unsuccessful.</returns>
        Task<IAsyncReactiveSubject<TInput, TOutput>> CreateAsync(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, object state = null, CancellationToken token = default);
    }

    /// <summary>
    /// Interface for a parameterized subject factory.
    /// </summary>
    /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
    /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
    /// <typeparam name="TArg1">Type of the first parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg2">Type of the second parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg3">Type of the third parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg4">Type of the fourth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg5">Type of the fifth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg6">Type of the sixth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg7">Type of the seventh parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg8">Type of the eighth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg9">Type of the ninth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg10">Type of the tenth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg11">Type of the eleventh parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg12">Type of the twelfth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg13">Type of the thirteenth parameter passed to the subject factory.</typeparam>
    public interface IAsyncReactiveSubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>
    {
        /// <summary>
        /// Creates a new subject with the specified stream URI.
        /// </summary>
        /// <param name="streamUri">URI identifying the stream.</param>
        /// <param name="arg1">First parameter to pass to the subject factory.</param>
        /// <param name="arg2">Second parameter to pass to the subject factory.</param>
        /// <param name="arg3">Third parameter to pass to the subject factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subject factory.</param>
        /// <param name="arg5">Fifth parameter to pass to the subject factory.</param>
        /// <param name="arg6">Sixth parameter to pass to the subject factory.</param>
        /// <param name="arg7">Seventh parameter to pass to the subject factory.</param>
        /// <param name="arg8">Eighth parameter to pass to the subject factory.</param>
        /// <param name="arg9">Ninth parameter to pass to the subject factory.</param>
        /// <param name="arg10">Tenth parameter to pass to the subject factory.</param>
        /// <param name="arg11">Eleventh parameter to pass to the subject factory.</param>
        /// <param name="arg12">Twelfth parameter to pass to the subject factory.</param>
        /// <param name="arg13">Thirteenth parameter to pass to the subject factory.</param>
        /// <param name="state">Additional state to pass to the operation. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task returning a subject object that can be used to send and receive data on the stream, or an exception if the creation request was unsuccessful.</returns>
        Task<IAsyncReactiveSubject<TInput, TOutput>> CreateAsync(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, object state = null, CancellationToken token = default);
    }

    /// <summary>
    /// Interface for a parameterized subject factory.
    /// </summary>
    /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
    /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
    /// <typeparam name="TArg1">Type of the first parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg2">Type of the second parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg3">Type of the third parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg4">Type of the fourth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg5">Type of the fifth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg6">Type of the sixth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg7">Type of the seventh parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg8">Type of the eighth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg9">Type of the ninth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg10">Type of the tenth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg11">Type of the eleventh parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg12">Type of the twelfth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg13">Type of the thirteenth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg14">Type of the fourteenth parameter passed to the subject factory.</typeparam>
    public interface IAsyncReactiveSubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>
    {
        /// <summary>
        /// Creates a new subject with the specified stream URI.
        /// </summary>
        /// <param name="streamUri">URI identifying the stream.</param>
        /// <param name="arg1">First parameter to pass to the subject factory.</param>
        /// <param name="arg2">Second parameter to pass to the subject factory.</param>
        /// <param name="arg3">Third parameter to pass to the subject factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subject factory.</param>
        /// <param name="arg5">Fifth parameter to pass to the subject factory.</param>
        /// <param name="arg6">Sixth parameter to pass to the subject factory.</param>
        /// <param name="arg7">Seventh parameter to pass to the subject factory.</param>
        /// <param name="arg8">Eighth parameter to pass to the subject factory.</param>
        /// <param name="arg9">Ninth parameter to pass to the subject factory.</param>
        /// <param name="arg10">Tenth parameter to pass to the subject factory.</param>
        /// <param name="arg11">Eleventh parameter to pass to the subject factory.</param>
        /// <param name="arg12">Twelfth parameter to pass to the subject factory.</param>
        /// <param name="arg13">Thirteenth parameter to pass to the subject factory.</param>
        /// <param name="arg14">Fourteenth parameter to pass to the subject factory.</param>
        /// <param name="state">Additional state to pass to the operation. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task returning a subject object that can be used to send and receive data on the stream, or an exception if the creation request was unsuccessful.</returns>
        Task<IAsyncReactiveSubject<TInput, TOutput>> CreateAsync(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, object state = null, CancellationToken token = default);
    }

    /// <summary>
    /// Interface for a parameterized subject factory.
    /// </summary>
    /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
    /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
    /// <typeparam name="TArg1">Type of the first parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg2">Type of the second parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg3">Type of the third parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg4">Type of the fourth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg5">Type of the fifth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg6">Type of the sixth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg7">Type of the seventh parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg8">Type of the eighth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg9">Type of the ninth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg10">Type of the tenth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg11">Type of the eleventh parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg12">Type of the twelfth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg13">Type of the thirteenth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg14">Type of the fourteenth parameter passed to the subject factory.</typeparam>
    /// <typeparam name="TArg15">Type of the fifteenth parameter passed to the subject factory.</typeparam>
    public interface IAsyncReactiveSubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>
    {
        /// <summary>
        /// Creates a new subject with the specified stream URI.
        /// </summary>
        /// <param name="streamUri">URI identifying the stream.</param>
        /// <param name="arg1">First parameter to pass to the subject factory.</param>
        /// <param name="arg2">Second parameter to pass to the subject factory.</param>
        /// <param name="arg3">Third parameter to pass to the subject factory.</param>
        /// <param name="arg4">Fourth parameter to pass to the subject factory.</param>
        /// <param name="arg5">Fifth parameter to pass to the subject factory.</param>
        /// <param name="arg6">Sixth parameter to pass to the subject factory.</param>
        /// <param name="arg7">Seventh parameter to pass to the subject factory.</param>
        /// <param name="arg8">Eighth parameter to pass to the subject factory.</param>
        /// <param name="arg9">Ninth parameter to pass to the subject factory.</param>
        /// <param name="arg10">Tenth parameter to pass to the subject factory.</param>
        /// <param name="arg11">Eleventh parameter to pass to the subject factory.</param>
        /// <param name="arg12">Twelfth parameter to pass to the subject factory.</param>
        /// <param name="arg13">Thirteenth parameter to pass to the subject factory.</param>
        /// <param name="arg14">Fourteenth parameter to pass to the subject factory.</param>
        /// <param name="arg15">Fifteenth parameter to pass to the subject factory.</param>
        /// <param name="state">Additional state to pass to the operation. Implementations can interpret this value, or ignore it.</param>
        /// <param name="token">Token to observe for cancellation of the request.</param>
        /// <returns>Task returning a subject object that can be used to send and receive data on the stream, or an exception if the creation request was unsuccessful.</returns>
        Task<IAsyncReactiveSubject<TInput, TOutput>> CreateAsync(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, TArg15 arg15, object state = null, CancellationToken token = default);
    }

}