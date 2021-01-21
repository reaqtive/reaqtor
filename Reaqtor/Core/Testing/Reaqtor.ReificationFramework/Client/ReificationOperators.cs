// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.ReificationFramework
{
    /// <summary>
    /// Builtin operators for the reification framework.
    /// </summary>
    public static class ReificationOperators
    {
        /// <summary>
        /// Get the source observable.
        /// </summary>
        /// <typeparam name="T">The element type of the source observable.</typeparam>
        /// <param name="context">The client context.</param>
        /// <returns>The source observable.</returns>
        /// <remarks>
        /// This is a "wildcard" source, meaning it is intended for use with any observable.
        /// </remarks>
        [KnownResource(ReificationConstants.Source.String)]
        public static IAsyncReactiveQbservable<T> GetSourceObservable<T>(this IReactiveProxy context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            return context.GetObservable<T>(ReificationConstants.Source.Uri);
        }

        /// <summary>
        /// Get the parameterized source observable.
        /// </summary>
        /// <typeparam name="TParam">The parameter type.</typeparam>
        /// <typeparam name="TResult">The element type of the source observable.</typeparam>
        /// <param name="context">The client context.</param>
        /// <returns>The parameterized source observable.</returns>
        /// <remarks>
        /// This is a "wildcard" source, meaning it is intended for use with any observable.
        /// </remarks>
        [KnownResource(ReificationConstants.ParameterizedSource.String)]
        public static Func<TParam, IAsyncReactiveQbservable<TResult>> GetParameterizedSourceObservable<TParam, TResult>(this IReactiveProxy context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            return context.GetObservable<TParam, TResult>(ReificationConstants.ParameterizedSource.Uri);
        }

        /// <summary>
        /// Get the sink observer.
        /// </summary>
        /// <typeparam name="T">The element type of the sink observer.</typeparam>
        /// <param name="context">The client context.</param>
        /// <returns>The sink observer.</returns>
        /// <remarks>
        /// This is a "wildcard" sink, meaning it is intended for use with any observer.
        /// </remarks>
        [KnownResource(ReificationConstants.Sink.String)]
        public static IAsyncReactiveQbserver<T> GetSinkObserver<T>(this IReactiveProxy context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            return context.GetObserver<T>(ReificationConstants.Sink.Uri);
        }
    }
}
