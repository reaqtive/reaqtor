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
using System.Linq.Expressions;

namespace Reaqtor
{
    public abstract partial class ReactiveClientBase
    {
        #region GetObservable

        /// <summary>
        /// Gets the parameterized observable with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <returns>Observable object that can be used to write queries against, or to receive data by subscribing to it using an observer.</returns>
        public Func<TArg1, TArg2, IReactiveQbservable<TResult>> GetObservable<TArg1, TArg2, TResult>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetObservableCore<TArg1, TArg2, TResult>(uri);
        }

        /// <summary>
        /// Gets the parameterized observable with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <returns>Observable object that can be used to write queries against, or to receive data by subscribing to it using an observer.</returns>
        protected virtual Func<TArg1, TArg2, IReactiveQbservable<TResult>> GetObservableCore<TArg1, TArg2, TResult>(Uri uri)
        {
            var function = GetFunctionExpression<TArg1, TArg2, IReactiveQbservable<TResult>>(uri);
            return _queryProvider.CreateQbservable<TArg1, TArg2, TResult>(function);
        }

        /// <summary>
        /// Gets the parameterized observable with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <returns>Observable object that can be used to write queries against, or to receive data by subscribing to it using an observer.</returns>
        public Func<TArg1, TArg2, TArg3, IReactiveQbservable<TResult>> GetObservable<TArg1, TArg2, TArg3, TResult>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetObservableCore<TArg1, TArg2, TArg3, TResult>(uri);
        }

        /// <summary>
        /// Gets the parameterized observable with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <returns>Observable object that can be used to write queries against, or to receive data by subscribing to it using an observer.</returns>
        protected virtual Func<TArg1, TArg2, TArg3, IReactiveQbservable<TResult>> GetObservableCore<TArg1, TArg2, TArg3, TResult>(Uri uri)
        {
            var function = GetFunctionExpression<TArg1, TArg2, TArg3, IReactiveQbservable<TResult>>(uri);
            return _queryProvider.CreateQbservable<TArg1, TArg2, TArg3, TResult>(function);
        }

        /// <summary>
        /// Gets the parameterized observable with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <returns>Observable object that can be used to write queries against, or to receive data by subscribing to it using an observer.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, IReactiveQbservable<TResult>> GetObservable<TArg1, TArg2, TArg3, TArg4, TResult>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetObservableCore<TArg1, TArg2, TArg3, TArg4, TResult>(uri);
        }

        /// <summary>
        /// Gets the parameterized observable with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <returns>Observable object that can be used to write queries against, or to receive data by subscribing to it using an observer.</returns>
        protected virtual Func<TArg1, TArg2, TArg3, TArg4, IReactiveQbservable<TResult>> GetObservableCore<TArg1, TArg2, TArg3, TArg4, TResult>(Uri uri)
        {
            var function = GetFunctionExpression<TArg1, TArg2, TArg3, TArg4, IReactiveQbservable<TResult>>(uri);
            return _queryProvider.CreateQbservable<TArg1, TArg2, TArg3, TArg4, TResult>(function);
        }

        /// <summary>
        /// Gets the parameterized observable with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <returns>Observable object that can be used to write queries against, or to receive data by subscribing to it using an observer.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, TArg5, IReactiveQbservable<TResult>> GetObservable<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetObservableCore<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(uri);
        }

        /// <summary>
        /// Gets the parameterized observable with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <returns>Observable object that can be used to write queries against, or to receive data by subscribing to it using an observer.</returns>
        protected virtual Func<TArg1, TArg2, TArg3, TArg4, TArg5, IReactiveQbservable<TResult>> GetObservableCore<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(Uri uri)
        {
            var function = GetFunctionExpression<TArg1, TArg2, TArg3, TArg4, TArg5, IReactiveQbservable<TResult>>(uri);
            return _queryProvider.CreateQbservable<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(function);
        }

        /// <summary>
        /// Gets the parameterized observable with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <returns>Observable object that can be used to write queries against, or to receive data by subscribing to it using an observer.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, IReactiveQbservable<TResult>> GetObservable<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetObservableCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(uri);
        }

        /// <summary>
        /// Gets the parameterized observable with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <returns>Observable object that can be used to write queries against, or to receive data by subscribing to it using an observer.</returns>
        protected virtual Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, IReactiveQbservable<TResult>> GetObservableCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(Uri uri)
        {
            var function = GetFunctionExpression<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, IReactiveQbservable<TResult>>(uri);
            return _queryProvider.CreateQbservable<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(function);
        }

        /// <summary>
        /// Gets the parameterized observable with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <returns>Observable object that can be used to write queries against, or to receive data by subscribing to it using an observer.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, IReactiveQbservable<TResult>> GetObservable<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetObservableCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(uri);
        }

        /// <summary>
        /// Gets the parameterized observable with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <returns>Observable object that can be used to write queries against, or to receive data by subscribing to it using an observer.</returns>
        protected virtual Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, IReactiveQbservable<TResult>> GetObservableCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(Uri uri)
        {
            var function = GetFunctionExpression<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, IReactiveQbservable<TResult>>(uri);
            return _queryProvider.CreateQbservable<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(function);
        }

        /// <summary>
        /// Gets the parameterized observable with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg8">Type of the eighth parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <returns>Observable object that can be used to write queries against, or to receive data by subscribing to it using an observer.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, IReactiveQbservable<TResult>> GetObservable<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetObservableCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(uri);
        }

        /// <summary>
        /// Gets the parameterized observable with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg8">Type of the eighth parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <returns>Observable object that can be used to write queries against, or to receive data by subscribing to it using an observer.</returns>
        protected virtual Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, IReactiveQbservable<TResult>> GetObservableCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(Uri uri)
        {
            var function = GetFunctionExpression<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, IReactiveQbservable<TResult>>(uri);
            return _queryProvider.CreateQbservable<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(function);
        }

        /// <summary>
        /// Gets the parameterized observable with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg8">Type of the eighth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg9">Type of the ninth parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <returns>Observable object that can be used to write queries against, or to receive data by subscribing to it using an observer.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, IReactiveQbservable<TResult>> GetObservable<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetObservableCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>(uri);
        }

        /// <summary>
        /// Gets the parameterized observable with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg8">Type of the eighth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg9">Type of the ninth parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <returns>Observable object that can be used to write queries against, or to receive data by subscribing to it using an observer.</returns>
        protected virtual Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, IReactiveQbservable<TResult>> GetObservableCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>(Uri uri)
        {
            var function = GetFunctionExpression<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, IReactiveQbservable<TResult>>(uri);
            return _queryProvider.CreateQbservable<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>(function);
        }

        /// <summary>
        /// Gets the parameterized observable with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg8">Type of the eighth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg9">Type of the ninth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg10">Type of the tenth parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <returns>Observable object that can be used to write queries against, or to receive data by subscribing to it using an observer.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, IReactiveQbservable<TResult>> GetObservable<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetObservableCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>(uri);
        }

        /// <summary>
        /// Gets the parameterized observable with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg8">Type of the eighth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg9">Type of the ninth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg10">Type of the tenth parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <returns>Observable object that can be used to write queries against, or to receive data by subscribing to it using an observer.</returns>
        protected virtual Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, IReactiveQbservable<TResult>> GetObservableCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>(Uri uri)
        {
            var function = GetFunctionExpression<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, IReactiveQbservable<TResult>>(uri);
            return _queryProvider.CreateQbservable<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>(function);
        }

        /// <summary>
        /// Gets the parameterized observable with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg8">Type of the eighth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg9">Type of the ninth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg10">Type of the tenth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg11">Type of the eleventh parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <returns>Observable object that can be used to write queries against, or to receive data by subscribing to it using an observer.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, IReactiveQbservable<TResult>> GetObservable<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetObservableCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>(uri);
        }

        /// <summary>
        /// Gets the parameterized observable with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg8">Type of the eighth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg9">Type of the ninth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg10">Type of the tenth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg11">Type of the eleventh parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <returns>Observable object that can be used to write queries against, or to receive data by subscribing to it using an observer.</returns>
        protected virtual Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, IReactiveQbservable<TResult>> GetObservableCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>(Uri uri)
        {
            var function = GetFunctionExpression<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, IReactiveQbservable<TResult>>(uri);
            return _queryProvider.CreateQbservable<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>(function);
        }

        /// <summary>
        /// Gets the parameterized observable with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg8">Type of the eighth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg9">Type of the ninth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg10">Type of the tenth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg11">Type of the eleventh parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg12">Type of the twelfth parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <returns>Observable object that can be used to write queries against, or to receive data by subscribing to it using an observer.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, IReactiveQbservable<TResult>> GetObservable<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetObservableCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>(uri);
        }

        /// <summary>
        /// Gets the parameterized observable with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg8">Type of the eighth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg9">Type of the ninth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg10">Type of the tenth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg11">Type of the eleventh parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg12">Type of the twelfth parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <returns>Observable object that can be used to write queries against, or to receive data by subscribing to it using an observer.</returns>
        protected virtual Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, IReactiveQbservable<TResult>> GetObservableCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>(Uri uri)
        {
            var function = GetFunctionExpression<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, IReactiveQbservable<TResult>>(uri);
            return _queryProvider.CreateQbservable<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>(function);
        }

        /// <summary>
        /// Gets the parameterized observable with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg8">Type of the eighth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg9">Type of the ninth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg10">Type of the tenth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg11">Type of the eleventh parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg12">Type of the twelfth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg13">Type of the thirteenth parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <returns>Observable object that can be used to write queries against, or to receive data by subscribing to it using an observer.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, IReactiveQbservable<TResult>> GetObservable<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetObservableCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>(uri);
        }

        /// <summary>
        /// Gets the parameterized observable with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg8">Type of the eighth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg9">Type of the ninth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg10">Type of the tenth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg11">Type of the eleventh parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg12">Type of the twelfth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg13">Type of the thirteenth parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <returns>Observable object that can be used to write queries against, or to receive data by subscribing to it using an observer.</returns>
        protected virtual Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, IReactiveQbservable<TResult>> GetObservableCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>(Uri uri)
        {
            var function = GetFunctionExpression<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, IReactiveQbservable<TResult>>(uri);
            return _queryProvider.CreateQbservable<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>(function);
        }

        /// <summary>
        /// Gets the parameterized observable with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg8">Type of the eighth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg9">Type of the ninth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg10">Type of the tenth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg11">Type of the eleventh parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg12">Type of the twelfth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg13">Type of the thirteenth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg14">Type of the fourteenth parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <returns>Observable object that can be used to write queries against, or to receive data by subscribing to it using an observer.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, IReactiveQbservable<TResult>> GetObservable<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetObservableCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>(uri);
        }

        /// <summary>
        /// Gets the parameterized observable with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg8">Type of the eighth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg9">Type of the ninth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg10">Type of the tenth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg11">Type of the eleventh parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg12">Type of the twelfth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg13">Type of the thirteenth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg14">Type of the fourteenth parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <returns>Observable object that can be used to write queries against, or to receive data by subscribing to it using an observer.</returns>
        protected virtual Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, IReactiveQbservable<TResult>> GetObservableCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>(Uri uri)
        {
            var function = GetFunctionExpression<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, IReactiveQbservable<TResult>>(uri);
            return _queryProvider.CreateQbservable<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>(function);
        }

        /// <summary>
        /// Gets the parameterized observable with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg8">Type of the eighth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg9">Type of the ninth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg10">Type of the tenth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg11">Type of the eleventh parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg12">Type of the twelfth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg13">Type of the thirteenth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg14">Type of the fourteenth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg15">Type of the fifteenth parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <returns>Observable object that can be used to write queries against, or to receive data by subscribing to it using an observer.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, IReactiveQbservable<TResult>> GetObservable<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TResult>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetObservableCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TResult>(uri);
        }

        /// <summary>
        /// Gets the parameterized observable with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg8">Type of the eighth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg9">Type of the ninth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg10">Type of the tenth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg11">Type of the eleventh parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg12">Type of the twelfth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg13">Type of the thirteenth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg14">Type of the fourteenth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg15">Type of the fifteenth parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <returns>Observable object that can be used to write queries against, or to receive data by subscribing to it using an observer.</returns>
        protected virtual Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, IReactiveQbservable<TResult>> GetObservableCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TResult>(Uri uri)
        {
            var function = GetFunctionExpression<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, IReactiveQbservable<TResult>>(uri);
            return _queryProvider.CreateQbservable<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TResult>(function);
        }


        #endregion

        #region GetObserver

        /// <summary>
        /// Gets the parameterized observer with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <returns>Observer object that can be used to send data.</returns>
        public Func<TArg1, TArg2, IReactiveQbserver<TResult>> GetObserver<TArg1, TArg2, TResult>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetObserverCore<TArg1, TArg2, TResult>(uri);
        }

        /// <summary>
        /// Gets the parameterized observer with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <returns>Observer object that can be used to send data.</returns>
        protected virtual Func<TArg1, TArg2, IReactiveQbserver<TResult>> GetObserverCore<TArg1, TArg2, TResult>(Uri uri)
        {
            var function = GetFunctionExpression<TArg1, TArg2, IReactiveQbserver<TResult>>(uri);
            return _queryProvider.CreateQbserver<TArg1, TArg2, TResult>(function);
        }

        /// <summary>
        /// Gets the parameterized observer with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <returns>Observer object that can be used to send data.</returns>
        public Func<TArg1, TArg2, TArg3, IReactiveQbserver<TResult>> GetObserver<TArg1, TArg2, TArg3, TResult>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetObserverCore<TArg1, TArg2, TArg3, TResult>(uri);
        }

        /// <summary>
        /// Gets the parameterized observer with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <returns>Observer object that can be used to send data.</returns>
        protected virtual Func<TArg1, TArg2, TArg3, IReactiveQbserver<TResult>> GetObserverCore<TArg1, TArg2, TArg3, TResult>(Uri uri)
        {
            var function = GetFunctionExpression<TArg1, TArg2, TArg3, IReactiveQbserver<TResult>>(uri);
            return _queryProvider.CreateQbserver<TArg1, TArg2, TArg3, TResult>(function);
        }

        /// <summary>
        /// Gets the parameterized observer with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <returns>Observer object that can be used to send data.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, IReactiveQbserver<TResult>> GetObserver<TArg1, TArg2, TArg3, TArg4, TResult>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetObserverCore<TArg1, TArg2, TArg3, TArg4, TResult>(uri);
        }

        /// <summary>
        /// Gets the parameterized observer with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <returns>Observer object that can be used to send data.</returns>
        protected virtual Func<TArg1, TArg2, TArg3, TArg4, IReactiveQbserver<TResult>> GetObserverCore<TArg1, TArg2, TArg3, TArg4, TResult>(Uri uri)
        {
            var function = GetFunctionExpression<TArg1, TArg2, TArg3, TArg4, IReactiveQbserver<TResult>>(uri);
            return _queryProvider.CreateQbserver<TArg1, TArg2, TArg3, TArg4, TResult>(function);
        }

        /// <summary>
        /// Gets the parameterized observer with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <returns>Observer object that can be used to send data.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, TArg5, IReactiveQbserver<TResult>> GetObserver<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetObserverCore<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(uri);
        }

        /// <summary>
        /// Gets the parameterized observer with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <returns>Observer object that can be used to send data.</returns>
        protected virtual Func<TArg1, TArg2, TArg3, TArg4, TArg5, IReactiveQbserver<TResult>> GetObserverCore<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(Uri uri)
        {
            var function = GetFunctionExpression<TArg1, TArg2, TArg3, TArg4, TArg5, IReactiveQbserver<TResult>>(uri);
            return _queryProvider.CreateQbserver<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(function);
        }

        /// <summary>
        /// Gets the parameterized observer with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <returns>Observer object that can be used to send data.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, IReactiveQbserver<TResult>> GetObserver<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetObserverCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(uri);
        }

        /// <summary>
        /// Gets the parameterized observer with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <returns>Observer object that can be used to send data.</returns>
        protected virtual Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, IReactiveQbserver<TResult>> GetObserverCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(Uri uri)
        {
            var function = GetFunctionExpression<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, IReactiveQbserver<TResult>>(uri);
            return _queryProvider.CreateQbserver<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(function);
        }

        /// <summary>
        /// Gets the parameterized observer with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <returns>Observer object that can be used to send data.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, IReactiveQbserver<TResult>> GetObserver<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetObserverCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(uri);
        }

        /// <summary>
        /// Gets the parameterized observer with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <returns>Observer object that can be used to send data.</returns>
        protected virtual Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, IReactiveQbserver<TResult>> GetObserverCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(Uri uri)
        {
            var function = GetFunctionExpression<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, IReactiveQbserver<TResult>>(uri);
            return _queryProvider.CreateQbserver<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(function);
        }

        /// <summary>
        /// Gets the parameterized observer with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg8">Type of the eighth parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <returns>Observer object that can be used to send data.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, IReactiveQbserver<TResult>> GetObserver<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetObserverCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(uri);
        }

        /// <summary>
        /// Gets the parameterized observer with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg8">Type of the eighth parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <returns>Observer object that can be used to send data.</returns>
        protected virtual Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, IReactiveQbserver<TResult>> GetObserverCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(Uri uri)
        {
            var function = GetFunctionExpression<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, IReactiveQbserver<TResult>>(uri);
            return _queryProvider.CreateQbserver<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(function);
        }

        /// <summary>
        /// Gets the parameterized observer with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg8">Type of the eighth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg9">Type of the ninth parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <returns>Observer object that can be used to send data.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, IReactiveQbserver<TResult>> GetObserver<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetObserverCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>(uri);
        }

        /// <summary>
        /// Gets the parameterized observer with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg8">Type of the eighth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg9">Type of the ninth parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <returns>Observer object that can be used to send data.</returns>
        protected virtual Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, IReactiveQbserver<TResult>> GetObserverCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>(Uri uri)
        {
            var function = GetFunctionExpression<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, IReactiveQbserver<TResult>>(uri);
            return _queryProvider.CreateQbserver<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>(function);
        }

        /// <summary>
        /// Gets the parameterized observer with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg8">Type of the eighth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg9">Type of the ninth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg10">Type of the tenth parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <returns>Observer object that can be used to send data.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, IReactiveQbserver<TResult>> GetObserver<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetObserverCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>(uri);
        }

        /// <summary>
        /// Gets the parameterized observer with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg8">Type of the eighth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg9">Type of the ninth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg10">Type of the tenth parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <returns>Observer object that can be used to send data.</returns>
        protected virtual Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, IReactiveQbserver<TResult>> GetObserverCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>(Uri uri)
        {
            var function = GetFunctionExpression<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, IReactiveQbserver<TResult>>(uri);
            return _queryProvider.CreateQbserver<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>(function);
        }

        /// <summary>
        /// Gets the parameterized observer with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg8">Type of the eighth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg9">Type of the ninth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg10">Type of the tenth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg11">Type of the eleventh parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <returns>Observer object that can be used to send data.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, IReactiveQbserver<TResult>> GetObserver<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetObserverCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>(uri);
        }

        /// <summary>
        /// Gets the parameterized observer with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg8">Type of the eighth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg9">Type of the ninth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg10">Type of the tenth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg11">Type of the eleventh parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <returns>Observer object that can be used to send data.</returns>
        protected virtual Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, IReactiveQbserver<TResult>> GetObserverCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>(Uri uri)
        {
            var function = GetFunctionExpression<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, IReactiveQbserver<TResult>>(uri);
            return _queryProvider.CreateQbserver<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>(function);
        }

        /// <summary>
        /// Gets the parameterized observer with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg8">Type of the eighth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg9">Type of the ninth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg10">Type of the tenth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg11">Type of the eleventh parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg12">Type of the twelfth parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <returns>Observer object that can be used to send data.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, IReactiveQbserver<TResult>> GetObserver<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetObserverCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>(uri);
        }

        /// <summary>
        /// Gets the parameterized observer with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg8">Type of the eighth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg9">Type of the ninth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg10">Type of the tenth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg11">Type of the eleventh parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg12">Type of the twelfth parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <returns>Observer object that can be used to send data.</returns>
        protected virtual Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, IReactiveQbserver<TResult>> GetObserverCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>(Uri uri)
        {
            var function = GetFunctionExpression<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, IReactiveQbserver<TResult>>(uri);
            return _queryProvider.CreateQbserver<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>(function);
        }

        /// <summary>
        /// Gets the parameterized observer with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg8">Type of the eighth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg9">Type of the ninth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg10">Type of the tenth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg11">Type of the eleventh parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg12">Type of the twelfth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg13">Type of the thirteenth parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <returns>Observer object that can be used to send data.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, IReactiveQbserver<TResult>> GetObserver<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetObserverCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>(uri);
        }

        /// <summary>
        /// Gets the parameterized observer with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg8">Type of the eighth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg9">Type of the ninth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg10">Type of the tenth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg11">Type of the eleventh parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg12">Type of the twelfth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg13">Type of the thirteenth parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <returns>Observer object that can be used to send data.</returns>
        protected virtual Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, IReactiveQbserver<TResult>> GetObserverCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>(Uri uri)
        {
            var function = GetFunctionExpression<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, IReactiveQbserver<TResult>>(uri);
            return _queryProvider.CreateQbserver<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>(function);
        }

        /// <summary>
        /// Gets the parameterized observer with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg8">Type of the eighth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg9">Type of the ninth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg10">Type of the tenth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg11">Type of the eleventh parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg12">Type of the twelfth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg13">Type of the thirteenth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg14">Type of the fourteenth parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <returns>Observer object that can be used to send data.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, IReactiveQbserver<TResult>> GetObserver<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetObserverCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>(uri);
        }

        /// <summary>
        /// Gets the parameterized observer with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg8">Type of the eighth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg9">Type of the ninth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg10">Type of the tenth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg11">Type of the eleventh parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg12">Type of the twelfth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg13">Type of the thirteenth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg14">Type of the fourteenth parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <returns>Observer object that can be used to send data.</returns>
        protected virtual Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, IReactiveQbserver<TResult>> GetObserverCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>(Uri uri)
        {
            var function = GetFunctionExpression<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, IReactiveQbserver<TResult>>(uri);
            return _queryProvider.CreateQbserver<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>(function);
        }

        /// <summary>
        /// Gets the parameterized observer with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg8">Type of the eighth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg9">Type of the ninth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg10">Type of the tenth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg11">Type of the eleventh parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg12">Type of the twelfth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg13">Type of the thirteenth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg14">Type of the fourteenth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg15">Type of the fifteenth parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <returns>Observer object that can be used to send data.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, IReactiveQbserver<TResult>> GetObserver<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TResult>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetObserverCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TResult>(uri);
        }

        /// <summary>
        /// Gets the parameterized observer with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg8">Type of the eighth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg9">Type of the ninth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg10">Type of the tenth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg11">Type of the eleventh parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg12">Type of the twelfth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg13">Type of the thirteenth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg14">Type of the fourteenth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg15">Type of the fifteenth parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <returns>Observer object that can be used to send data.</returns>
        protected virtual Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, IReactiveQbserver<TResult>> GetObserverCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TResult>(Uri uri)
        {
            var function = GetFunctionExpression<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, IReactiveQbserver<TResult>>(uri);
            return _queryProvider.CreateQbserver<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TResult>(function);
        }


        #endregion

        #region GetStreamFactory

        /// <summary>
        /// Gets the parameterized stream factory with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <returns>Stream factory that can be used to create streams, represented as subjects.</returns>
        public IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2> GetStreamFactory<TArg1, TArg2, TInput, TOutput>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetStreamFactoryCore<TArg1, TArg2, TInput, TOutput>(uri);
        }

        /// <summary>
        /// Gets the parameterized stream factory with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <returns>Stream factory that can be used to create streams, represented as subjects.</returns>
        protected virtual IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2> GetStreamFactoryCore<TArg1, TArg2, TInput, TOutput>(Uri uri)
        {
            var expression = GetKnownResourceExpression(typeof(Func<TArg1, TArg2, IReactiveQubject<TInput, TOutput>>), uri);
            return _queryProvider.CreateQubjectFactory<TArg1, TArg2, TInput, TOutput>(expression);
        }

        /// <summary>
        /// Gets the parameterized stream factory with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <returns>Stream factory that can be used to create streams, represented as subjects.</returns>
        public IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3> GetStreamFactory<TArg1, TArg2, TArg3, TInput, TOutput>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetStreamFactoryCore<TArg1, TArg2, TArg3, TInput, TOutput>(uri);
        }

        /// <summary>
        /// Gets the parameterized stream factory with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <returns>Stream factory that can be used to create streams, represented as subjects.</returns>
        protected virtual IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3> GetStreamFactoryCore<TArg1, TArg2, TArg3, TInput, TOutput>(Uri uri)
        {
            var expression = GetKnownResourceExpression(typeof(Func<TArg1, TArg2, TArg3, IReactiveQubject<TInput, TOutput>>), uri);
            return _queryProvider.CreateQubjectFactory<TArg1, TArg2, TArg3, TInput, TOutput>(expression);
        }

        /// <summary>
        /// Gets the parameterized stream factory with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <returns>Stream factory that can be used to create streams, represented as subjects.</returns>
        public IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4> GetStreamFactory<TArg1, TArg2, TArg3, TArg4, TInput, TOutput>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetStreamFactoryCore<TArg1, TArg2, TArg3, TArg4, TInput, TOutput>(uri);
        }

        /// <summary>
        /// Gets the parameterized stream factory with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <returns>Stream factory that can be used to create streams, represented as subjects.</returns>
        protected virtual IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4> GetStreamFactoryCore<TArg1, TArg2, TArg3, TArg4, TInput, TOutput>(Uri uri)
        {
            var expression = GetKnownResourceExpression(typeof(Func<TArg1, TArg2, TArg3, TArg4, IReactiveQubject<TInput, TOutput>>), uri);
            return _queryProvider.CreateQubjectFactory<TArg1, TArg2, TArg3, TArg4, TInput, TOutput>(expression);
        }

        /// <summary>
        /// Gets the parameterized stream factory with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <returns>Stream factory that can be used to create streams, represented as subjects.</returns>
        public IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5> GetStreamFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TInput, TOutput>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetStreamFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TInput, TOutput>(uri);
        }

        /// <summary>
        /// Gets the parameterized stream factory with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <returns>Stream factory that can be used to create streams, represented as subjects.</returns>
        protected virtual IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5> GetStreamFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TInput, TOutput>(Uri uri)
        {
            var expression = GetKnownResourceExpression(typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, IReactiveQubject<TInput, TOutput>>), uri);
            return _queryProvider.CreateQubjectFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TInput, TOutput>(expression);
        }

        /// <summary>
        /// Gets the parameterized stream factory with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <returns>Stream factory that can be used to create streams, represented as subjects.</returns>
        public IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> GetStreamFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TInput, TOutput>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetStreamFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TInput, TOutput>(uri);
        }

        /// <summary>
        /// Gets the parameterized stream factory with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <returns>Stream factory that can be used to create streams, represented as subjects.</returns>
        protected virtual IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> GetStreamFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TInput, TOutput>(Uri uri)
        {
            var expression = GetKnownResourceExpression(typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, IReactiveQubject<TInput, TOutput>>), uri);
            return _queryProvider.CreateQubjectFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TInput, TOutput>(expression);
        }

        /// <summary>
        /// Gets the parameterized stream factory with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <returns>Stream factory that can be used to create streams, represented as subjects.</returns>
        public IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> GetStreamFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TInput, TOutput>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetStreamFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TInput, TOutput>(uri);
        }

        /// <summary>
        /// Gets the parameterized stream factory with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <returns>Stream factory that can be used to create streams, represented as subjects.</returns>
        protected virtual IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> GetStreamFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TInput, TOutput>(Uri uri)
        {
            var expression = GetKnownResourceExpression(typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, IReactiveQubject<TInput, TOutput>>), uri);
            return _queryProvider.CreateQubjectFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TInput, TOutput>(expression);
        }

        /// <summary>
        /// Gets the parameterized stream factory with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg8">Type of the eighth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <returns>Stream factory that can be used to create streams, represented as subjects.</returns>
        public IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> GetStreamFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TInput, TOutput>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetStreamFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TInput, TOutput>(uri);
        }

        /// <summary>
        /// Gets the parameterized stream factory with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg8">Type of the eighth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <returns>Stream factory that can be used to create streams, represented as subjects.</returns>
        protected virtual IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> GetStreamFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TInput, TOutput>(Uri uri)
        {
            var expression = GetKnownResourceExpression(typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, IReactiveQubject<TInput, TOutput>>), uri);
            return _queryProvider.CreateQubjectFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TInput, TOutput>(expression);
        }

        /// <summary>
        /// Gets the parameterized stream factory with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg8">Type of the eighth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg9">Type of the ninth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <returns>Stream factory that can be used to create streams, represented as subjects.</returns>
        public IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> GetStreamFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TInput, TOutput>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetStreamFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TInput, TOutput>(uri);
        }

        /// <summary>
        /// Gets the parameterized stream factory with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg8">Type of the eighth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg9">Type of the ninth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <returns>Stream factory that can be used to create streams, represented as subjects.</returns>
        protected virtual IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> GetStreamFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TInput, TOutput>(Uri uri)
        {
            var expression = GetKnownResourceExpression(typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, IReactiveQubject<TInput, TOutput>>), uri);
            return _queryProvider.CreateQubjectFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TInput, TOutput>(expression);
        }

        /// <summary>
        /// Gets the parameterized stream factory with the specified URI.
        /// </summary>
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
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <returns>Stream factory that can be used to create streams, represented as subjects.</returns>
        public IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> GetStreamFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TInput, TOutput>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetStreamFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TInput, TOutput>(uri);
        }

        /// <summary>
        /// Gets the parameterized stream factory with the specified URI.
        /// </summary>
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
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <returns>Stream factory that can be used to create streams, represented as subjects.</returns>
        protected virtual IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> GetStreamFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TInput, TOutput>(Uri uri)
        {
            var expression = GetKnownResourceExpression(typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, IReactiveQubject<TInput, TOutput>>), uri);
            return _queryProvider.CreateQubjectFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TInput, TOutput>(expression);
        }

        /// <summary>
        /// Gets the parameterized stream factory with the specified URI.
        /// </summary>
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
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <returns>Stream factory that can be used to create streams, represented as subjects.</returns>
        public IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> GetStreamFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TInput, TOutput>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetStreamFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TInput, TOutput>(uri);
        }

        /// <summary>
        /// Gets the parameterized stream factory with the specified URI.
        /// </summary>
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
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <returns>Stream factory that can be used to create streams, represented as subjects.</returns>
        protected virtual IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> GetStreamFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TInput, TOutput>(Uri uri)
        {
            var expression = GetKnownResourceExpression(typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, IReactiveQubject<TInput, TOutput>>), uri);
            return _queryProvider.CreateQubjectFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TInput, TOutput>(expression);
        }

        /// <summary>
        /// Gets the parameterized stream factory with the specified URI.
        /// </summary>
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
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <returns>Stream factory that can be used to create streams, represented as subjects.</returns>
        public IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> GetStreamFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TInput, TOutput>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetStreamFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TInput, TOutput>(uri);
        }

        /// <summary>
        /// Gets the parameterized stream factory with the specified URI.
        /// </summary>
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
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <returns>Stream factory that can be used to create streams, represented as subjects.</returns>
        protected virtual IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> GetStreamFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TInput, TOutput>(Uri uri)
        {
            var expression = GetKnownResourceExpression(typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, IReactiveQubject<TInput, TOutput>>), uri);
            return _queryProvider.CreateQubjectFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TInput, TOutput>(expression);
        }

        /// <summary>
        /// Gets the parameterized stream factory with the specified URI.
        /// </summary>
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
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <returns>Stream factory that can be used to create streams, represented as subjects.</returns>
        public IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> GetStreamFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TInput, TOutput>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetStreamFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TInput, TOutput>(uri);
        }

        /// <summary>
        /// Gets the parameterized stream factory with the specified URI.
        /// </summary>
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
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <returns>Stream factory that can be used to create streams, represented as subjects.</returns>
        protected virtual IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> GetStreamFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TInput, TOutput>(Uri uri)
        {
            var expression = GetKnownResourceExpression(typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, IReactiveQubject<TInput, TOutput>>), uri);
            return _queryProvider.CreateQubjectFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TInput, TOutput>(expression);
        }

        /// <summary>
        /// Gets the parameterized stream factory with the specified URI.
        /// </summary>
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
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <returns>Stream factory that can be used to create streams, represented as subjects.</returns>
        public IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> GetStreamFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TInput, TOutput>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetStreamFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TInput, TOutput>(uri);
        }

        /// <summary>
        /// Gets the parameterized stream factory with the specified URI.
        /// </summary>
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
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <returns>Stream factory that can be used to create streams, represented as subjects.</returns>
        protected virtual IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> GetStreamFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TInput, TOutput>(Uri uri)
        {
            var expression = GetKnownResourceExpression(typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, IReactiveQubject<TInput, TOutput>>), uri);
            return _queryProvider.CreateQubjectFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TInput, TOutput>(expression);
        }

        /// <summary>
        /// Gets the parameterized stream factory with the specified URI.
        /// </summary>
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
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <returns>Stream factory that can be used to create streams, represented as subjects.</returns>
        public IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> GetStreamFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TInput, TOutput>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetStreamFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TInput, TOutput>(uri);
        }

        /// <summary>
        /// Gets the parameterized stream factory with the specified URI.
        /// </summary>
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
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <returns>Stream factory that can be used to create streams, represented as subjects.</returns>
        protected virtual IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> GetStreamFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TInput, TOutput>(Uri uri)
        {
            var expression = GetKnownResourceExpression(typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, IReactiveQubject<TInput, TOutput>>), uri);
            return _queryProvider.CreateQubjectFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TInput, TOutput>(expression);
        }


        #endregion

        #region GetSubscriptionFactory

        /// <summary>
        /// Gets the parameterized subscription factory with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <returns>Subscription factory that can be used to create subscriptions.</returns>
        public IReactiveQubscriptionFactory<TArg1, TArg2> GetSubscriptionFactory<TArg1, TArg2>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetSubscriptionFactoryCore<TArg1, TArg2>(uri);
        }

        /// <summary>
        /// Gets the parameterized subscription factory with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <returns>Subscription factory that can be used to create subscriptions.</returns>
        protected virtual IReactiveQubscriptionFactory<TArg1, TArg2> GetSubscriptionFactoryCore<TArg1, TArg2>(Uri uri)
        {
            var expression = GetKnownResourceExpression(typeof(Func<TArg1, TArg2, IReactiveQubscription>), uri);
            return _queryProvider.CreateQubscriptionFactory<TArg1, TArg2>(expression);
        }

        /// <summary>
        /// Gets the parameterized subscription factory with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the subscription factory.</typeparam>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <returns>Subscription factory that can be used to create subscriptions.</returns>
        public IReactiveQubscriptionFactory<TArg1, TArg2, TArg3> GetSubscriptionFactory<TArg1, TArg2, TArg3>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetSubscriptionFactoryCore<TArg1, TArg2, TArg3>(uri);
        }

        /// <summary>
        /// Gets the parameterized subscription factory with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the subscription factory.</typeparam>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <returns>Subscription factory that can be used to create subscriptions.</returns>
        protected virtual IReactiveQubscriptionFactory<TArg1, TArg2, TArg3> GetSubscriptionFactoryCore<TArg1, TArg2, TArg3>(Uri uri)
        {
            var expression = GetKnownResourceExpression(typeof(Func<TArg1, TArg2, TArg3, IReactiveQubscription>), uri);
            return _queryProvider.CreateQubscriptionFactory<TArg1, TArg2, TArg3>(expression);
        }

        /// <summary>
        /// Gets the parameterized subscription factory with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the subscription factory.</typeparam>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <returns>Subscription factory that can be used to create subscriptions.</returns>
        public IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4> GetSubscriptionFactory<TArg1, TArg2, TArg3, TArg4>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetSubscriptionFactoryCore<TArg1, TArg2, TArg3, TArg4>(uri);
        }

        /// <summary>
        /// Gets the parameterized subscription factory with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the subscription factory.</typeparam>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <returns>Subscription factory that can be used to create subscriptions.</returns>
        protected virtual IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4> GetSubscriptionFactoryCore<TArg1, TArg2, TArg3, TArg4>(Uri uri)
        {
            var expression = GetKnownResourceExpression(typeof(Func<TArg1, TArg2, TArg3, TArg4, IReactiveQubscription>), uri);
            return _queryProvider.CreateQubscriptionFactory<TArg1, TArg2, TArg3, TArg4>(expression);
        }

        /// <summary>
        /// Gets the parameterized subscription factory with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the subscription factory.</typeparam>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <returns>Subscription factory that can be used to create subscriptions.</returns>
        public IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5> GetSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetSubscriptionFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5>(uri);
        }

        /// <summary>
        /// Gets the parameterized subscription factory with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the subscription factory.</typeparam>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <returns>Subscription factory that can be used to create subscriptions.</returns>
        protected virtual IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5> GetSubscriptionFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5>(Uri uri)
        {
            var expression = GetKnownResourceExpression(typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, IReactiveQubscription>), uri);
            return _queryProvider.CreateQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5>(expression);
        }

        /// <summary>
        /// Gets the parameterized subscription factory with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the subscription factory.</typeparam>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <returns>Subscription factory that can be used to create subscriptions.</returns>
        public IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> GetSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetSubscriptionFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(uri);
        }

        /// <summary>
        /// Gets the parameterized subscription factory with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the subscription factory.</typeparam>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <returns>Subscription factory that can be used to create subscriptions.</returns>
        protected virtual IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> GetSubscriptionFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(Uri uri)
        {
            var expression = GetKnownResourceExpression(typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, IReactiveQubscription>), uri);
            return _queryProvider.CreateQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(expression);
        }

        /// <summary>
        /// Gets the parameterized subscription factory with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the subscription factory.</typeparam>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <returns>Subscription factory that can be used to create subscriptions.</returns>
        public IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> GetSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetSubscriptionFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(uri);
        }

        /// <summary>
        /// Gets the parameterized subscription factory with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the subscription factory.</typeparam>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <returns>Subscription factory that can be used to create subscriptions.</returns>
        protected virtual IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> GetSubscriptionFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(Uri uri)
        {
            var expression = GetKnownResourceExpression(typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, IReactiveQubscription>), uri);
            return _queryProvider.CreateQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(expression);
        }

        /// <summary>
        /// Gets the parameterized subscription factory with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg8">Type of the eighth parameter passed to the subscription factory.</typeparam>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <returns>Subscription factory that can be used to create subscriptions.</returns>
        public IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> GetSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetSubscriptionFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(uri);
        }

        /// <summary>
        /// Gets the parameterized subscription factory with the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg8">Type of the eighth parameter passed to the subscription factory.</typeparam>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <returns>Subscription factory that can be used to create subscriptions.</returns>
        protected virtual IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> GetSubscriptionFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(Uri uri)
        {
            var expression = GetKnownResourceExpression(typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, IReactiveQubscription>), uri);
            return _queryProvider.CreateQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(expression);
        }

        /// <summary>
        /// Gets the parameterized subscription factory with the specified URI.
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
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <returns>Subscription factory that can be used to create subscriptions.</returns>
        public IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> GetSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetSubscriptionFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(uri);
        }

        /// <summary>
        /// Gets the parameterized subscription factory with the specified URI.
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
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <returns>Subscription factory that can be used to create subscriptions.</returns>
        protected virtual IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> GetSubscriptionFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(Uri uri)
        {
            var expression = GetKnownResourceExpression(typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, IReactiveQubscription>), uri);
            return _queryProvider.CreateQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(expression);
        }

        /// <summary>
        /// Gets the parameterized subscription factory with the specified URI.
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
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <returns>Subscription factory that can be used to create subscriptions.</returns>
        public IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> GetSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetSubscriptionFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>(uri);
        }

        /// <summary>
        /// Gets the parameterized subscription factory with the specified URI.
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
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <returns>Subscription factory that can be used to create subscriptions.</returns>
        protected virtual IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> GetSubscriptionFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>(Uri uri)
        {
            var expression = GetKnownResourceExpression(typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, IReactiveQubscription>), uri);
            return _queryProvider.CreateQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>(expression);
        }

        /// <summary>
        /// Gets the parameterized subscription factory with the specified URI.
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
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <returns>Subscription factory that can be used to create subscriptions.</returns>
        public IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> GetSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetSubscriptionFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>(uri);
        }

        /// <summary>
        /// Gets the parameterized subscription factory with the specified URI.
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
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <returns>Subscription factory that can be used to create subscriptions.</returns>
        protected virtual IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> GetSubscriptionFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>(Uri uri)
        {
            var expression = GetKnownResourceExpression(typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, IReactiveQubscription>), uri);
            return _queryProvider.CreateQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>(expression);
        }

        /// <summary>
        /// Gets the parameterized subscription factory with the specified URI.
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
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <returns>Subscription factory that can be used to create subscriptions.</returns>
        public IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> GetSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetSubscriptionFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>(uri);
        }

        /// <summary>
        /// Gets the parameterized subscription factory with the specified URI.
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
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <returns>Subscription factory that can be used to create subscriptions.</returns>
        protected virtual IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> GetSubscriptionFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>(Uri uri)
        {
            var expression = GetKnownResourceExpression(typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, IReactiveQubscription>), uri);
            return _queryProvider.CreateQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>(expression);
        }

        /// <summary>
        /// Gets the parameterized subscription factory with the specified URI.
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
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <returns>Subscription factory that can be used to create subscriptions.</returns>
        public IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> GetSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetSubscriptionFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>(uri);
        }

        /// <summary>
        /// Gets the parameterized subscription factory with the specified URI.
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
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <returns>Subscription factory that can be used to create subscriptions.</returns>
        protected virtual IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> GetSubscriptionFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>(Uri uri)
        {
            var expression = GetKnownResourceExpression(typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, IReactiveQubscription>), uri);
            return _queryProvider.CreateQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>(expression);
        }

        /// <summary>
        /// Gets the parameterized subscription factory with the specified URI.
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
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <returns>Subscription factory that can be used to create subscriptions.</returns>
        public IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> GetSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetSubscriptionFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>(uri);
        }

        /// <summary>
        /// Gets the parameterized subscription factory with the specified URI.
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
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <returns>Subscription factory that can be used to create subscriptions.</returns>
        protected virtual IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> GetSubscriptionFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>(Uri uri)
        {
            var expression = GetKnownResourceExpression(typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, IReactiveQubscription>), uri);
            return _queryProvider.CreateQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>(expression);
        }

        /// <summary>
        /// Gets the parameterized subscription factory with the specified URI.
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
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <returns>Subscription factory that can be used to create subscriptions.</returns>
        public IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> GetSubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return GetSubscriptionFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>(uri);
        }

        /// <summary>
        /// Gets the parameterized subscription factory with the specified URI.
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
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <returns>Subscription factory that can be used to create subscriptions.</returns>
        protected virtual IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> GetSubscriptionFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>(Uri uri)
        {
            var expression = GetKnownResourceExpression(typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, IReactiveQubscription>), uri);
            return _queryProvider.CreateQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>(expression);
        }


        #endregion

        #region Private implementation

        private Expression<Func<TArg1, TArg2, TResult>> GetFunctionExpression<TArg1, TArg2, TResult>(Uri uri)
        {
            var function = GetKnownResourceExpression(typeof(Func<TArg1, TArg2, TResult>), uri);
            var operand1 = Expression.Parameter(typeof(TArg1));
            var operand2 = Expression.Parameter(typeof(TArg2));
            return Expression.Lambda<Func<TArg1, TArg2, TResult>>(Expression.Invoke(function, operand1, operand2), operand1, operand2);
        }

        private Expression<Func<TArg1, TArg2, TArg3, TResult>> GetFunctionExpression<TArg1, TArg2, TArg3, TResult>(Uri uri)
        {
            var function = GetKnownResourceExpression(typeof(Func<TArg1, TArg2, TArg3, TResult>), uri);
            var operand1 = Expression.Parameter(typeof(TArg1));
            var operand2 = Expression.Parameter(typeof(TArg2));
            var operand3 = Expression.Parameter(typeof(TArg3));
            return Expression.Lambda<Func<TArg1, TArg2, TArg3, TResult>>(Expression.Invoke(function, operand1, operand2, operand3), operand1, operand2, operand3);
        }

        private Expression<Func<TArg1, TArg2, TArg3, TArg4, TResult>> GetFunctionExpression<TArg1, TArg2, TArg3, TArg4, TResult>(Uri uri)
        {
            var function = GetKnownResourceExpression(typeof(Func<TArg1, TArg2, TArg3, TArg4, TResult>), uri);
            var operand1 = Expression.Parameter(typeof(TArg1));
            var operand2 = Expression.Parameter(typeof(TArg2));
            var operand3 = Expression.Parameter(typeof(TArg3));
            var operand4 = Expression.Parameter(typeof(TArg4));
            return Expression.Lambda<Func<TArg1, TArg2, TArg3, TArg4, TResult>>(Expression.Invoke(function, operand1, operand2, operand3, operand4), operand1, operand2, operand3, operand4);
        }

        private Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>> GetFunctionExpression<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(Uri uri)
        {
            var function = GetKnownResourceExpression(typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>), uri);
            var operand1 = Expression.Parameter(typeof(TArg1));
            var operand2 = Expression.Parameter(typeof(TArg2));
            var operand3 = Expression.Parameter(typeof(TArg3));
            var operand4 = Expression.Parameter(typeof(TArg4));
            var operand5 = Expression.Parameter(typeof(TArg5));
            return Expression.Lambda<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>>(Expression.Invoke(function, operand1, operand2, operand3, operand4, operand5), operand1, operand2, operand3, operand4, operand5);
        }

        private Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>> GetFunctionExpression<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(Uri uri)
        {
            var function = GetKnownResourceExpression(typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>), uri);
            var operand1 = Expression.Parameter(typeof(TArg1));
            var operand2 = Expression.Parameter(typeof(TArg2));
            var operand3 = Expression.Parameter(typeof(TArg3));
            var operand4 = Expression.Parameter(typeof(TArg4));
            var operand5 = Expression.Parameter(typeof(TArg5));
            var operand6 = Expression.Parameter(typeof(TArg6));
            return Expression.Lambda<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>>(Expression.Invoke(function, operand1, operand2, operand3, operand4, operand5, operand6), operand1, operand2, operand3, operand4, operand5, operand6);
        }

        private Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>> GetFunctionExpression<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(Uri uri)
        {
            var function = GetKnownResourceExpression(typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>), uri);
            var operand1 = Expression.Parameter(typeof(TArg1));
            var operand2 = Expression.Parameter(typeof(TArg2));
            var operand3 = Expression.Parameter(typeof(TArg3));
            var operand4 = Expression.Parameter(typeof(TArg4));
            var operand5 = Expression.Parameter(typeof(TArg5));
            var operand6 = Expression.Parameter(typeof(TArg6));
            var operand7 = Expression.Parameter(typeof(TArg7));
            return Expression.Lambda<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>>(Expression.Invoke(function, operand1, operand2, operand3, operand4, operand5, operand6, operand7), operand1, operand2, operand3, operand4, operand5, operand6, operand7);
        }

        private Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>> GetFunctionExpression<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(Uri uri)
        {
            var function = GetKnownResourceExpression(typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>), uri);
            var operand1 = Expression.Parameter(typeof(TArg1));
            var operand2 = Expression.Parameter(typeof(TArg2));
            var operand3 = Expression.Parameter(typeof(TArg3));
            var operand4 = Expression.Parameter(typeof(TArg4));
            var operand5 = Expression.Parameter(typeof(TArg5));
            var operand6 = Expression.Parameter(typeof(TArg6));
            var operand7 = Expression.Parameter(typeof(TArg7));
            var operand8 = Expression.Parameter(typeof(TArg8));
            return Expression.Lambda<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>>(Expression.Invoke(function, operand1, operand2, operand3, operand4, operand5, operand6, operand7, operand8), operand1, operand2, operand3, operand4, operand5, operand6, operand7, operand8);
        }

        private Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>> GetFunctionExpression<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>(Uri uri)
        {
            var function = GetKnownResourceExpression(typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>), uri);
            var operand1 = Expression.Parameter(typeof(TArg1));
            var operand2 = Expression.Parameter(typeof(TArg2));
            var operand3 = Expression.Parameter(typeof(TArg3));
            var operand4 = Expression.Parameter(typeof(TArg4));
            var operand5 = Expression.Parameter(typeof(TArg5));
            var operand6 = Expression.Parameter(typeof(TArg6));
            var operand7 = Expression.Parameter(typeof(TArg7));
            var operand8 = Expression.Parameter(typeof(TArg8));
            var operand9 = Expression.Parameter(typeof(TArg9));
            return Expression.Lambda<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>>(Expression.Invoke(function, operand1, operand2, operand3, operand4, operand5, operand6, operand7, operand8, operand9), operand1, operand2, operand3, operand4, operand5, operand6, operand7, operand8, operand9);
        }

        private Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>> GetFunctionExpression<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>(Uri uri)
        {
            var function = GetKnownResourceExpression(typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>), uri);
            var operand1 = Expression.Parameter(typeof(TArg1));
            var operand2 = Expression.Parameter(typeof(TArg2));
            var operand3 = Expression.Parameter(typeof(TArg3));
            var operand4 = Expression.Parameter(typeof(TArg4));
            var operand5 = Expression.Parameter(typeof(TArg5));
            var operand6 = Expression.Parameter(typeof(TArg6));
            var operand7 = Expression.Parameter(typeof(TArg7));
            var operand8 = Expression.Parameter(typeof(TArg8));
            var operand9 = Expression.Parameter(typeof(TArg9));
            var operand10 = Expression.Parameter(typeof(TArg10));
            return Expression.Lambda<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>>(Expression.Invoke(function, operand1, operand2, operand3, operand4, operand5, operand6, operand7, operand8, operand9, operand10), operand1, operand2, operand3, operand4, operand5, operand6, operand7, operand8, operand9, operand10);
        }

        private Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>> GetFunctionExpression<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>(Uri uri)
        {
            var function = GetKnownResourceExpression(typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>), uri);
            var operand1 = Expression.Parameter(typeof(TArg1));
            var operand2 = Expression.Parameter(typeof(TArg2));
            var operand3 = Expression.Parameter(typeof(TArg3));
            var operand4 = Expression.Parameter(typeof(TArg4));
            var operand5 = Expression.Parameter(typeof(TArg5));
            var operand6 = Expression.Parameter(typeof(TArg6));
            var operand7 = Expression.Parameter(typeof(TArg7));
            var operand8 = Expression.Parameter(typeof(TArg8));
            var operand9 = Expression.Parameter(typeof(TArg9));
            var operand10 = Expression.Parameter(typeof(TArg10));
            var operand11 = Expression.Parameter(typeof(TArg11));
            return Expression.Lambda<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>>(Expression.Invoke(function, operand1, operand2, operand3, operand4, operand5, operand6, operand7, operand8, operand9, operand10, operand11), operand1, operand2, operand3, operand4, operand5, operand6, operand7, operand8, operand9, operand10, operand11);
        }

        private Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>> GetFunctionExpression<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>(Uri uri)
        {
            var function = GetKnownResourceExpression(typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>), uri);
            var operand1 = Expression.Parameter(typeof(TArg1));
            var operand2 = Expression.Parameter(typeof(TArg2));
            var operand3 = Expression.Parameter(typeof(TArg3));
            var operand4 = Expression.Parameter(typeof(TArg4));
            var operand5 = Expression.Parameter(typeof(TArg5));
            var operand6 = Expression.Parameter(typeof(TArg6));
            var operand7 = Expression.Parameter(typeof(TArg7));
            var operand8 = Expression.Parameter(typeof(TArg8));
            var operand9 = Expression.Parameter(typeof(TArg9));
            var operand10 = Expression.Parameter(typeof(TArg10));
            var operand11 = Expression.Parameter(typeof(TArg11));
            var operand12 = Expression.Parameter(typeof(TArg12));
            return Expression.Lambda<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>>(Expression.Invoke(function, operand1, operand2, operand3, operand4, operand5, operand6, operand7, operand8, operand9, operand10, operand11, operand12), operand1, operand2, operand3, operand4, operand5, operand6, operand7, operand8, operand9, operand10, operand11, operand12);
        }

        private Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>> GetFunctionExpression<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>(Uri uri)
        {
            var function = GetKnownResourceExpression(typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>), uri);
            var operand1 = Expression.Parameter(typeof(TArg1));
            var operand2 = Expression.Parameter(typeof(TArg2));
            var operand3 = Expression.Parameter(typeof(TArg3));
            var operand4 = Expression.Parameter(typeof(TArg4));
            var operand5 = Expression.Parameter(typeof(TArg5));
            var operand6 = Expression.Parameter(typeof(TArg6));
            var operand7 = Expression.Parameter(typeof(TArg7));
            var operand8 = Expression.Parameter(typeof(TArg8));
            var operand9 = Expression.Parameter(typeof(TArg9));
            var operand10 = Expression.Parameter(typeof(TArg10));
            var operand11 = Expression.Parameter(typeof(TArg11));
            var operand12 = Expression.Parameter(typeof(TArg12));
            var operand13 = Expression.Parameter(typeof(TArg13));
            return Expression.Lambda<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>>(Expression.Invoke(function, operand1, operand2, operand3, operand4, operand5, operand6, operand7, operand8, operand9, operand10, operand11, operand12, operand13), operand1, operand2, operand3, operand4, operand5, operand6, operand7, operand8, operand9, operand10, operand11, operand12, operand13);
        }

        private Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>> GetFunctionExpression<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>(Uri uri)
        {
            var function = GetKnownResourceExpression(typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>), uri);
            var operand1 = Expression.Parameter(typeof(TArg1));
            var operand2 = Expression.Parameter(typeof(TArg2));
            var operand3 = Expression.Parameter(typeof(TArg3));
            var operand4 = Expression.Parameter(typeof(TArg4));
            var operand5 = Expression.Parameter(typeof(TArg5));
            var operand6 = Expression.Parameter(typeof(TArg6));
            var operand7 = Expression.Parameter(typeof(TArg7));
            var operand8 = Expression.Parameter(typeof(TArg8));
            var operand9 = Expression.Parameter(typeof(TArg9));
            var operand10 = Expression.Parameter(typeof(TArg10));
            var operand11 = Expression.Parameter(typeof(TArg11));
            var operand12 = Expression.Parameter(typeof(TArg12));
            var operand13 = Expression.Parameter(typeof(TArg13));
            var operand14 = Expression.Parameter(typeof(TArg14));
            return Expression.Lambda<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>>(Expression.Invoke(function, operand1, operand2, operand3, operand4, operand5, operand6, operand7, operand8, operand9, operand10, operand11, operand12, operand13, operand14), operand1, operand2, operand3, operand4, operand5, operand6, operand7, operand8, operand9, operand10, operand11, operand12, operand13, operand14);
        }

        private Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TResult>> GetFunctionExpression<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TResult>(Uri uri)
        {
            var function = GetKnownResourceExpression(typeof(Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TResult>), uri);
            var operand1 = Expression.Parameter(typeof(TArg1));
            var operand2 = Expression.Parameter(typeof(TArg2));
            var operand3 = Expression.Parameter(typeof(TArg3));
            var operand4 = Expression.Parameter(typeof(TArg4));
            var operand5 = Expression.Parameter(typeof(TArg5));
            var operand6 = Expression.Parameter(typeof(TArg6));
            var operand7 = Expression.Parameter(typeof(TArg7));
            var operand8 = Expression.Parameter(typeof(TArg8));
            var operand9 = Expression.Parameter(typeof(TArg9));
            var operand10 = Expression.Parameter(typeof(TArg10));
            var operand11 = Expression.Parameter(typeof(TArg11));
            var operand12 = Expression.Parameter(typeof(TArg12));
            var operand13 = Expression.Parameter(typeof(TArg13));
            var operand14 = Expression.Parameter(typeof(TArg14));
            var operand15 = Expression.Parameter(typeof(TArg15));
            return Expression.Lambda<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TResult>>(Expression.Invoke(function, operand1, operand2, operand3, operand4, operand5, operand6, operand7, operand8, operand9, operand10, operand11, operand12, operand13, operand14, operand15), operand1, operand2, operand3, operand4, operand5, operand6, operand7, operand8, operand9, operand10, operand11, operand12, operand13, operand14, operand15);
        }

        #endregion

    }
}
