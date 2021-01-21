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
    public abstract partial class ReactiveQueryProviderBase
    {
        #region CreateQbservable

        /// <summary>
        /// Creates a parameterized observable based on the given expression that will be executed by the current provider.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="expression">Expression representing the observable.</param>
        /// <returns>Parameterized observable represented by the given expression.</returns>
        public Func<TArg1, TArg2, IReactiveQbservable<TResult>> CreateQbservable<TArg1, TArg2, TResult>(Expression<Func<TArg1, TArg2, IReactiveQbservable<TResult>>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var res = new Func<TArg1, TArg2, IReactiveQbservable<TResult>>((arg1, arg2) =>
            {
                return CreateQbservable<TResult>(Expression.Invoke(expression, Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2))));
            });

            ExpressionServices.RegisterObject(res, expression);

            return res;
        }

        /// <summary>
        /// Creates a parameterized observable based on the given expression that will be executed by the current provider.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="expression">Expression representing the observable.</param>
        /// <returns>Parameterized observable represented by the given expression.</returns>
        public Func<TArg1, TArg2, TArg3, IReactiveQbservable<TResult>> CreateQbservable<TArg1, TArg2, TArg3, TResult>(Expression<Func<TArg1, TArg2, TArg3, IReactiveQbservable<TResult>>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var res = new Func<TArg1, TArg2, TArg3, IReactiveQbservable<TResult>>((arg1, arg2, arg3) =>
            {
                return CreateQbservable<TResult>(Expression.Invoke(expression, Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3))));
            });

            ExpressionServices.RegisterObject(res, expression);

            return res;
        }

        /// <summary>
        /// Creates a parameterized observable based on the given expression that will be executed by the current provider.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="expression">Expression representing the observable.</param>
        /// <returns>Parameterized observable represented by the given expression.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, IReactiveQbservable<TResult>> CreateQbservable<TArg1, TArg2, TArg3, TArg4, TResult>(Expression<Func<TArg1, TArg2, TArg3, TArg4, IReactiveQbservable<TResult>>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var res = new Func<TArg1, TArg2, TArg3, TArg4, IReactiveQbservable<TResult>>((arg1, arg2, arg3, arg4) =>
            {
                return CreateQbservable<TResult>(Expression.Invoke(expression, Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4))));
            });

            ExpressionServices.RegisterObject(res, expression);

            return res;
        }

        /// <summary>
        /// Creates a parameterized observable based on the given expression that will be executed by the current provider.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="expression">Expression representing the observable.</param>
        /// <returns>Parameterized observable represented by the given expression.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, TArg5, IReactiveQbservable<TResult>> CreateQbservable<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, IReactiveQbservable<TResult>>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var res = new Func<TArg1, TArg2, TArg3, TArg4, TArg5, IReactiveQbservable<TResult>>((arg1, arg2, arg3, arg4, arg5) =>
            {
                return CreateQbservable<TResult>(Expression.Invoke(expression, Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4)), Expression.Constant(arg5, typeof(TArg5))));
            });

            ExpressionServices.RegisterObject(res, expression);

            return res;
        }

        /// <summary>
        /// Creates a parameterized observable based on the given expression that will be executed by the current provider.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="expression">Expression representing the observable.</param>
        /// <returns>Parameterized observable represented by the given expression.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, IReactiveQbservable<TResult>> CreateQbservable<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, IReactiveQbservable<TResult>>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var res = new Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, IReactiveQbservable<TResult>>((arg1, arg2, arg3, arg4, arg5, arg6) =>
            {
                return CreateQbservable<TResult>(Expression.Invoke(expression, Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4)), Expression.Constant(arg5, typeof(TArg5)), Expression.Constant(arg6, typeof(TArg6))));
            });

            ExpressionServices.RegisterObject(res, expression);

            return res;
        }

        /// <summary>
        /// Creates a parameterized observable based on the given expression that will be executed by the current provider.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="expression">Expression representing the observable.</param>
        /// <returns>Parameterized observable represented by the given expression.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, IReactiveQbservable<TResult>> CreateQbservable<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, IReactiveQbservable<TResult>>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var res = new Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, IReactiveQbservable<TResult>>((arg1, arg2, arg3, arg4, arg5, arg6, arg7) =>
            {
                return CreateQbservable<TResult>(Expression.Invoke(expression, Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4)), Expression.Constant(arg5, typeof(TArg5)), Expression.Constant(arg6, typeof(TArg6)), Expression.Constant(arg7, typeof(TArg7))));
            });

            ExpressionServices.RegisterObject(res, expression);

            return res;
        }

        /// <summary>
        /// Creates a parameterized observable based on the given expression that will be executed by the current provider.
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
        /// <param name="expression">Expression representing the observable.</param>
        /// <returns>Parameterized observable represented by the given expression.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, IReactiveQbservable<TResult>> CreateQbservable<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, IReactiveQbservable<TResult>>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var res = new Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, IReactiveQbservable<TResult>>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
            {
                return CreateQbservable<TResult>(Expression.Invoke(expression, Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4)), Expression.Constant(arg5, typeof(TArg5)), Expression.Constant(arg6, typeof(TArg6)), Expression.Constant(arg7, typeof(TArg7)), Expression.Constant(arg8, typeof(TArg8))));
            });

            ExpressionServices.RegisterObject(res, expression);

            return res;
        }

        /// <summary>
        /// Creates a parameterized observable based on the given expression that will be executed by the current provider.
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
        /// <param name="expression">Expression representing the observable.</param>
        /// <returns>Parameterized observable represented by the given expression.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, IReactiveQbservable<TResult>> CreateQbservable<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>(Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, IReactiveQbservable<TResult>>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var res = new Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, IReactiveQbservable<TResult>>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            {
                return CreateQbservable<TResult>(Expression.Invoke(expression, Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4)), Expression.Constant(arg5, typeof(TArg5)), Expression.Constant(arg6, typeof(TArg6)), Expression.Constant(arg7, typeof(TArg7)), Expression.Constant(arg8, typeof(TArg8)), Expression.Constant(arg9, typeof(TArg9))));
            });

            ExpressionServices.RegisterObject(res, expression);

            return res;
        }

        /// <summary>
        /// Creates a parameterized observable based on the given expression that will be executed by the current provider.
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
        /// <param name="expression">Expression representing the observable.</param>
        /// <returns>Parameterized observable represented by the given expression.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, IReactiveQbservable<TResult>> CreateQbservable<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>(Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, IReactiveQbservable<TResult>>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var res = new Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, IReactiveQbservable<TResult>>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            {
                return CreateQbservable<TResult>(Expression.Invoke(expression, Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4)), Expression.Constant(arg5, typeof(TArg5)), Expression.Constant(arg6, typeof(TArg6)), Expression.Constant(arg7, typeof(TArg7)), Expression.Constant(arg8, typeof(TArg8)), Expression.Constant(arg9, typeof(TArg9)), Expression.Constant(arg10, typeof(TArg10))));
            });

            ExpressionServices.RegisterObject(res, expression);

            return res;
        }

        /// <summary>
        /// Creates a parameterized observable based on the given expression that will be executed by the current provider.
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
        /// <param name="expression">Expression representing the observable.</param>
        /// <returns>Parameterized observable represented by the given expression.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, IReactiveQbservable<TResult>> CreateQbservable<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>(Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, IReactiveQbservable<TResult>>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var res = new Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, IReactiveQbservable<TResult>>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            {
                return CreateQbservable<TResult>(Expression.Invoke(expression, Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4)), Expression.Constant(arg5, typeof(TArg5)), Expression.Constant(arg6, typeof(TArg6)), Expression.Constant(arg7, typeof(TArg7)), Expression.Constant(arg8, typeof(TArg8)), Expression.Constant(arg9, typeof(TArg9)), Expression.Constant(arg10, typeof(TArg10)), Expression.Constant(arg11, typeof(TArg11))));
            });

            ExpressionServices.RegisterObject(res, expression);

            return res;
        }

        /// <summary>
        /// Creates a parameterized observable based on the given expression that will be executed by the current provider.
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
        /// <param name="expression">Expression representing the observable.</param>
        /// <returns>Parameterized observable represented by the given expression.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, IReactiveQbservable<TResult>> CreateQbservable<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>(Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, IReactiveQbservable<TResult>>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var res = new Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, IReactiveQbservable<TResult>>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            {
                return CreateQbservable<TResult>(Expression.Invoke(expression, Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4)), Expression.Constant(arg5, typeof(TArg5)), Expression.Constant(arg6, typeof(TArg6)), Expression.Constant(arg7, typeof(TArg7)), Expression.Constant(arg8, typeof(TArg8)), Expression.Constant(arg9, typeof(TArg9)), Expression.Constant(arg10, typeof(TArg10)), Expression.Constant(arg11, typeof(TArg11)), Expression.Constant(arg12, typeof(TArg12))));
            });

            ExpressionServices.RegisterObject(res, expression);

            return res;
        }

        /// <summary>
        /// Creates a parameterized observable based on the given expression that will be executed by the current provider.
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
        /// <param name="expression">Expression representing the observable.</param>
        /// <returns>Parameterized observable represented by the given expression.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, IReactiveQbservable<TResult>> CreateQbservable<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>(Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, IReactiveQbservable<TResult>>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var res = new Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, IReactiveQbservable<TResult>>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            {
                return CreateQbservable<TResult>(Expression.Invoke(expression, Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4)), Expression.Constant(arg5, typeof(TArg5)), Expression.Constant(arg6, typeof(TArg6)), Expression.Constant(arg7, typeof(TArg7)), Expression.Constant(arg8, typeof(TArg8)), Expression.Constant(arg9, typeof(TArg9)), Expression.Constant(arg10, typeof(TArg10)), Expression.Constant(arg11, typeof(TArg11)), Expression.Constant(arg12, typeof(TArg12)), Expression.Constant(arg13, typeof(TArg13))));
            });

            ExpressionServices.RegisterObject(res, expression);

            return res;
        }

        /// <summary>
        /// Creates a parameterized observable based on the given expression that will be executed by the current provider.
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
        /// <param name="expression">Expression representing the observable.</param>
        /// <returns>Parameterized observable represented by the given expression.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, IReactiveQbservable<TResult>> CreateQbservable<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>(Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, IReactiveQbservable<TResult>>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var res = new Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, IReactiveQbservable<TResult>>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            {
                return CreateQbservable<TResult>(Expression.Invoke(expression, Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4)), Expression.Constant(arg5, typeof(TArg5)), Expression.Constant(arg6, typeof(TArg6)), Expression.Constant(arg7, typeof(TArg7)), Expression.Constant(arg8, typeof(TArg8)), Expression.Constant(arg9, typeof(TArg9)), Expression.Constant(arg10, typeof(TArg10)), Expression.Constant(arg11, typeof(TArg11)), Expression.Constant(arg12, typeof(TArg12)), Expression.Constant(arg13, typeof(TArg13)), Expression.Constant(arg14, typeof(TArg14))));
            });

            ExpressionServices.RegisterObject(res, expression);

            return res;
        }

        /// <summary>
        /// Creates a parameterized observable based on the given expression that will be executed by the current provider.
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
        /// <param name="expression">Expression representing the observable.</param>
        /// <returns>Parameterized observable represented by the given expression.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, IReactiveQbservable<TResult>> CreateQbservable<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TResult>(Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, IReactiveQbservable<TResult>>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var res = new Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, IReactiveQbservable<TResult>>((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            {
                return CreateQbservable<TResult>(Expression.Invoke(expression, Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4)), Expression.Constant(arg5, typeof(TArg5)), Expression.Constant(arg6, typeof(TArg6)), Expression.Constant(arg7, typeof(TArg7)), Expression.Constant(arg8, typeof(TArg8)), Expression.Constant(arg9, typeof(TArg9)), Expression.Constant(arg10, typeof(TArg10)), Expression.Constant(arg11, typeof(TArg11)), Expression.Constant(arg12, typeof(TArg12)), Expression.Constant(arg13, typeof(TArg13)), Expression.Constant(arg14, typeof(TArg14)), Expression.Constant(arg15, typeof(TArg15))));
            });

            ExpressionServices.RegisterObject(res, expression);

            return res;
        }

        #endregion

        #region CreateQbserver

        /// <summary>
        /// Creates a parameterized observer based on the given expression that will be executed by the current provider.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="expression">Expression representing the observer.</param>
        /// <returns>Parameterized observer represented by the given expression.</returns>
        public Func<TArg1, TArg2, IReactiveQbserver<TResult>> CreateQbserver<TArg1, TArg2, TResult>(Expression<Func<TArg1, TArg2, IReactiveQbserver<TResult>>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var res = (Func<TArg1, TArg2, IReactiveQbserver<TResult>>)((arg1, arg2) =>
            {
                return CreateQbserver<TResult>(Expression.Invoke(expression, Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2))));
            });

            ExpressionServices.RegisterObject(res, expression);

            return res;
        }

        /// <summary>
        /// Creates a parameterized observer based on the given expression that will be executed by the current provider.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="expression">Expression representing the observer.</param>
        /// <returns>Parameterized observer represented by the given expression.</returns>
        public Func<TArg1, TArg2, TArg3, IReactiveQbserver<TResult>> CreateQbserver<TArg1, TArg2, TArg3, TResult>(Expression<Func<TArg1, TArg2, TArg3, IReactiveQbserver<TResult>>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var res = (Func<TArg1, TArg2, TArg3, IReactiveQbserver<TResult>>)((arg1, arg2, arg3) =>
            {
                return CreateQbserver<TResult>(Expression.Invoke(expression, Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3))));
            });

            ExpressionServices.RegisterObject(res, expression);

            return res;
        }

        /// <summary>
        /// Creates a parameterized observer based on the given expression that will be executed by the current provider.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="expression">Expression representing the observer.</param>
        /// <returns>Parameterized observer represented by the given expression.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, IReactiveQbserver<TResult>> CreateQbserver<TArg1, TArg2, TArg3, TArg4, TResult>(Expression<Func<TArg1, TArg2, TArg3, TArg4, IReactiveQbserver<TResult>>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var res = (Func<TArg1, TArg2, TArg3, TArg4, IReactiveQbserver<TResult>>)((arg1, arg2, arg3, arg4) =>
            {
                return CreateQbserver<TResult>(Expression.Invoke(expression, Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4))));
            });

            ExpressionServices.RegisterObject(res, expression);

            return res;
        }

        /// <summary>
        /// Creates a parameterized observer based on the given expression that will be executed by the current provider.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="expression">Expression representing the observer.</param>
        /// <returns>Parameterized observer represented by the given expression.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, TArg5, IReactiveQbserver<TResult>> CreateQbserver<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, IReactiveQbserver<TResult>>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var res = (Func<TArg1, TArg2, TArg3, TArg4, TArg5, IReactiveQbserver<TResult>>)((arg1, arg2, arg3, arg4, arg5) =>
            {
                return CreateQbserver<TResult>(Expression.Invoke(expression, Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4)), Expression.Constant(arg5, typeof(TArg5))));
            });

            ExpressionServices.RegisterObject(res, expression);

            return res;
        }

        /// <summary>
        /// Creates a parameterized observer based on the given expression that will be executed by the current provider.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="expression">Expression representing the observer.</param>
        /// <returns>Parameterized observer represented by the given expression.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, IReactiveQbserver<TResult>> CreateQbserver<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, IReactiveQbserver<TResult>>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var res = (Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, IReactiveQbserver<TResult>>)((arg1, arg2, arg3, arg4, arg5, arg6) =>
            {
                return CreateQbserver<TResult>(Expression.Invoke(expression, Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4)), Expression.Constant(arg5, typeof(TArg5)), Expression.Constant(arg6, typeof(TArg6))));
            });

            ExpressionServices.RegisterObject(res, expression);

            return res;
        }

        /// <summary>
        /// Creates a parameterized observer based on the given expression that will be executed by the current provider.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="expression">Expression representing the observer.</param>
        /// <returns>Parameterized observer represented by the given expression.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, IReactiveQbserver<TResult>> CreateQbserver<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, IReactiveQbserver<TResult>>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var res = (Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, IReactiveQbserver<TResult>>)((arg1, arg2, arg3, arg4, arg5, arg6, arg7) =>
            {
                return CreateQbserver<TResult>(Expression.Invoke(expression, Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4)), Expression.Constant(arg5, typeof(TArg5)), Expression.Constant(arg6, typeof(TArg6)), Expression.Constant(arg7, typeof(TArg7))));
            });

            ExpressionServices.RegisterObject(res, expression);

            return res;
        }

        /// <summary>
        /// Creates a parameterized observer based on the given expression that will be executed by the current provider.
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
        /// <param name="expression">Expression representing the observer.</param>
        /// <returns>Parameterized observer represented by the given expression.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, IReactiveQbserver<TResult>> CreateQbserver<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, IReactiveQbserver<TResult>>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var res = (Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, IReactiveQbserver<TResult>>)((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
            {
                return CreateQbserver<TResult>(Expression.Invoke(expression, Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4)), Expression.Constant(arg5, typeof(TArg5)), Expression.Constant(arg6, typeof(TArg6)), Expression.Constant(arg7, typeof(TArg7)), Expression.Constant(arg8, typeof(TArg8))));
            });

            ExpressionServices.RegisterObject(res, expression);

            return res;
        }

        /// <summary>
        /// Creates a parameterized observer based on the given expression that will be executed by the current provider.
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
        /// <param name="expression">Expression representing the observer.</param>
        /// <returns>Parameterized observer represented by the given expression.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, IReactiveQbserver<TResult>> CreateQbserver<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>(Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, IReactiveQbserver<TResult>>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var res = (Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, IReactiveQbserver<TResult>>)((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            {
                return CreateQbserver<TResult>(Expression.Invoke(expression, Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4)), Expression.Constant(arg5, typeof(TArg5)), Expression.Constant(arg6, typeof(TArg6)), Expression.Constant(arg7, typeof(TArg7)), Expression.Constant(arg8, typeof(TArg8)), Expression.Constant(arg9, typeof(TArg9))));
            });

            ExpressionServices.RegisterObject(res, expression);

            return res;
        }

        /// <summary>
        /// Creates a parameterized observer based on the given expression that will be executed by the current provider.
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
        /// <param name="expression">Expression representing the observer.</param>
        /// <returns>Parameterized observer represented by the given expression.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, IReactiveQbserver<TResult>> CreateQbserver<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>(Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, IReactiveQbserver<TResult>>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var res = (Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, IReactiveQbserver<TResult>>)((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            {
                return CreateQbserver<TResult>(Expression.Invoke(expression, Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4)), Expression.Constant(arg5, typeof(TArg5)), Expression.Constant(arg6, typeof(TArg6)), Expression.Constant(arg7, typeof(TArg7)), Expression.Constant(arg8, typeof(TArg8)), Expression.Constant(arg9, typeof(TArg9)), Expression.Constant(arg10, typeof(TArg10))));
            });

            ExpressionServices.RegisterObject(res, expression);

            return res;
        }

        /// <summary>
        /// Creates a parameterized observer based on the given expression that will be executed by the current provider.
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
        /// <param name="expression">Expression representing the observer.</param>
        /// <returns>Parameterized observer represented by the given expression.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, IReactiveQbserver<TResult>> CreateQbserver<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>(Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, IReactiveQbserver<TResult>>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var res = (Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, IReactiveQbserver<TResult>>)((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            {
                return CreateQbserver<TResult>(Expression.Invoke(expression, Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4)), Expression.Constant(arg5, typeof(TArg5)), Expression.Constant(arg6, typeof(TArg6)), Expression.Constant(arg7, typeof(TArg7)), Expression.Constant(arg8, typeof(TArg8)), Expression.Constant(arg9, typeof(TArg9)), Expression.Constant(arg10, typeof(TArg10)), Expression.Constant(arg11, typeof(TArg11))));
            });

            ExpressionServices.RegisterObject(res, expression);

            return res;
        }

        /// <summary>
        /// Creates a parameterized observer based on the given expression that will be executed by the current provider.
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
        /// <param name="expression">Expression representing the observer.</param>
        /// <returns>Parameterized observer represented by the given expression.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, IReactiveQbserver<TResult>> CreateQbserver<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>(Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, IReactiveQbserver<TResult>>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var res = (Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, IReactiveQbserver<TResult>>)((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            {
                return CreateQbserver<TResult>(Expression.Invoke(expression, Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4)), Expression.Constant(arg5, typeof(TArg5)), Expression.Constant(arg6, typeof(TArg6)), Expression.Constant(arg7, typeof(TArg7)), Expression.Constant(arg8, typeof(TArg8)), Expression.Constant(arg9, typeof(TArg9)), Expression.Constant(arg10, typeof(TArg10)), Expression.Constant(arg11, typeof(TArg11)), Expression.Constant(arg12, typeof(TArg12))));
            });

            ExpressionServices.RegisterObject(res, expression);

            return res;
        }

        /// <summary>
        /// Creates a parameterized observer based on the given expression that will be executed by the current provider.
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
        /// <param name="expression">Expression representing the observer.</param>
        /// <returns>Parameterized observer represented by the given expression.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, IReactiveQbserver<TResult>> CreateQbserver<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>(Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, IReactiveQbserver<TResult>>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var res = (Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, IReactiveQbserver<TResult>>)((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            {
                return CreateQbserver<TResult>(Expression.Invoke(expression, Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4)), Expression.Constant(arg5, typeof(TArg5)), Expression.Constant(arg6, typeof(TArg6)), Expression.Constant(arg7, typeof(TArg7)), Expression.Constant(arg8, typeof(TArg8)), Expression.Constant(arg9, typeof(TArg9)), Expression.Constant(arg10, typeof(TArg10)), Expression.Constant(arg11, typeof(TArg11)), Expression.Constant(arg12, typeof(TArg12)), Expression.Constant(arg13, typeof(TArg13))));
            });

            ExpressionServices.RegisterObject(res, expression);

            return res;
        }

        /// <summary>
        /// Creates a parameterized observer based on the given expression that will be executed by the current provider.
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
        /// <param name="expression">Expression representing the observer.</param>
        /// <returns>Parameterized observer represented by the given expression.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, IReactiveQbserver<TResult>> CreateQbserver<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>(Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, IReactiveQbserver<TResult>>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var res = (Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, IReactiveQbserver<TResult>>)((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            {
                return CreateQbserver<TResult>(Expression.Invoke(expression, Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4)), Expression.Constant(arg5, typeof(TArg5)), Expression.Constant(arg6, typeof(TArg6)), Expression.Constant(arg7, typeof(TArg7)), Expression.Constant(arg8, typeof(TArg8)), Expression.Constant(arg9, typeof(TArg9)), Expression.Constant(arg10, typeof(TArg10)), Expression.Constant(arg11, typeof(TArg11)), Expression.Constant(arg12, typeof(TArg12)), Expression.Constant(arg13, typeof(TArg13)), Expression.Constant(arg14, typeof(TArg14))));
            });

            ExpressionServices.RegisterObject(res, expression);

            return res;
        }

        /// <summary>
        /// Creates a parameterized observer based on the given expression that will be executed by the current provider.
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
        /// <param name="expression">Expression representing the observer.</param>
        /// <returns>Parameterized observer represented by the given expression.</returns>
        public Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, IReactiveQbserver<TResult>> CreateQbserver<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TResult>(Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, IReactiveQbserver<TResult>>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var res = (Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, IReactiveQbserver<TResult>>)((arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            {
                return CreateQbserver<TResult>(Expression.Invoke(expression, Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4)), Expression.Constant(arg5, typeof(TArg5)), Expression.Constant(arg6, typeof(TArg6)), Expression.Constant(arg7, typeof(TArg7)), Expression.Constant(arg8, typeof(TArg8)), Expression.Constant(arg9, typeof(TArg9)), Expression.Constant(arg10, typeof(TArg10)), Expression.Constant(arg11, typeof(TArg11)), Expression.Constant(arg12, typeof(TArg12)), Expression.Constant(arg13, typeof(TArg13)), Expression.Constant(arg14, typeof(TArg14)), Expression.Constant(arg15, typeof(TArg15))));
            });

            ExpressionServices.RegisterObject(res, expression);

            return res;
        }

        #endregion

        #region CreateQubjectFactory

        /// <summary>
        /// Creates a parameterized subject factory based on the given expression.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TInput">Type of the data received by the subject created by the factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subject created by the factory.</typeparam>
        /// <param name="expression">Expression representing the subject factory.</param>
        /// <returns>Parameterized subject factory represented by the given expression.</returns>
        public IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2> CreateQubjectFactory<TArg1, TArg2, TInput, TOutput>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (ExpressionServices.TryGetName(expression, out var uri))
            {
                return new KnownQubjectFactory<TInput, TOutput, TArg1, TArg2>(expression, uri, this);
            }

            return new QubjectFactory<TInput, TOutput, TArg1, TArg2>(expression, this);
        }

        /// <summary>
        /// Creates a parameterized subject factory based on the given expression.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TInput">Type of the data received by the subject created by the factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subject created by the factory.</typeparam>
        /// <param name="expression">Expression representing the subject factory.</param>
        /// <returns>Parameterized subject factory represented by the given expression.</returns>
        public IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3> CreateQubjectFactory<TArg1, TArg2, TArg3, TInput, TOutput>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (ExpressionServices.TryGetName(expression, out var uri))
            {
                return new KnownQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3>(expression, uri, this);
            }

            return new QubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3>(expression, this);
        }

        /// <summary>
        /// Creates a parameterized subject factory based on the given expression.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TInput">Type of the data received by the subject created by the factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subject created by the factory.</typeparam>
        /// <param name="expression">Expression representing the subject factory.</param>
        /// <returns>Parameterized subject factory represented by the given expression.</returns>
        public IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4> CreateQubjectFactory<TArg1, TArg2, TArg3, TArg4, TInput, TOutput>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (ExpressionServices.TryGetName(expression, out var uri))
            {
                return new KnownQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4>(expression, uri, this);
            }

            return new QubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4>(expression, this);
        }

        /// <summary>
        /// Creates a parameterized subject factory based on the given expression.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TInput">Type of the data received by the subject created by the factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subject created by the factory.</typeparam>
        /// <param name="expression">Expression representing the subject factory.</param>
        /// <returns>Parameterized subject factory represented by the given expression.</returns>
        public IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5> CreateQubjectFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TInput, TOutput>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (ExpressionServices.TryGetName(expression, out var uri))
            {
                return new KnownQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5>(expression, uri, this);
            }

            return new QubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5>(expression, this);
        }

        /// <summary>
        /// Creates a parameterized subject factory based on the given expression.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TInput">Type of the data received by the subject created by the factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subject created by the factory.</typeparam>
        /// <param name="expression">Expression representing the subject factory.</param>
        /// <returns>Parameterized subject factory represented by the given expression.</returns>
        public IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> CreateQubjectFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TInput, TOutput>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (ExpressionServices.TryGetName(expression, out var uri))
            {
                return new KnownQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(expression, uri, this);
            }

            return new QubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(expression, this);
        }

        /// <summary>
        /// Creates a parameterized subject factory based on the given expression.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TInput">Type of the data received by the subject created by the factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subject created by the factory.</typeparam>
        /// <param name="expression">Expression representing the subject factory.</param>
        /// <returns>Parameterized subject factory represented by the given expression.</returns>
        public IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> CreateQubjectFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TInput, TOutput>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (ExpressionServices.TryGetName(expression, out var uri))
            {
                return new KnownQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(expression, uri, this);
            }

            return new QubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(expression, this);
        }

        /// <summary>
        /// Creates a parameterized subject factory based on the given expression.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg8">Type of the eighth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TInput">Type of the data received by the subject created by the factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subject created by the factory.</typeparam>
        /// <param name="expression">Expression representing the subject factory.</param>
        /// <returns>Parameterized subject factory represented by the given expression.</returns>
        public IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> CreateQubjectFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TInput, TOutput>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (ExpressionServices.TryGetName(expression, out var uri))
            {
                return new KnownQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(expression, uri, this);
            }

            return new QubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(expression, this);
        }

        /// <summary>
        /// Creates a parameterized subject factory based on the given expression.
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
        /// <typeparam name="TInput">Type of the data received by the subject created by the factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subject created by the factory.</typeparam>
        /// <param name="expression">Expression representing the subject factory.</param>
        /// <returns>Parameterized subject factory represented by the given expression.</returns>
        public IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> CreateQubjectFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TInput, TOutput>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (ExpressionServices.TryGetName(expression, out var uri))
            {
                return new KnownQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(expression, uri, this);
            }

            return new QubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(expression, this);
        }

        /// <summary>
        /// Creates a parameterized subject factory based on the given expression.
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
        /// <typeparam name="TInput">Type of the data received by the subject created by the factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subject created by the factory.</typeparam>
        /// <param name="expression">Expression representing the subject factory.</param>
        /// <returns>Parameterized subject factory represented by the given expression.</returns>
        public IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> CreateQubjectFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TInput, TOutput>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (ExpressionServices.TryGetName(expression, out var uri))
            {
                return new KnownQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>(expression, uri, this);
            }

            return new QubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>(expression, this);
        }

        /// <summary>
        /// Creates a parameterized subject factory based on the given expression.
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
        /// <typeparam name="TInput">Type of the data received by the subject created by the factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subject created by the factory.</typeparam>
        /// <param name="expression">Expression representing the subject factory.</param>
        /// <returns>Parameterized subject factory represented by the given expression.</returns>
        public IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> CreateQubjectFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TInput, TOutput>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (ExpressionServices.TryGetName(expression, out var uri))
            {
                return new KnownQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>(expression, uri, this);
            }

            return new QubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>(expression, this);
        }

        /// <summary>
        /// Creates a parameterized subject factory based on the given expression.
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
        /// <typeparam name="TInput">Type of the data received by the subject created by the factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subject created by the factory.</typeparam>
        /// <param name="expression">Expression representing the subject factory.</param>
        /// <returns>Parameterized subject factory represented by the given expression.</returns>
        public IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> CreateQubjectFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TInput, TOutput>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (ExpressionServices.TryGetName(expression, out var uri))
            {
                return new KnownQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>(expression, uri, this);
            }

            return new QubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>(expression, this);
        }

        /// <summary>
        /// Creates a parameterized subject factory based on the given expression.
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
        /// <typeparam name="TInput">Type of the data received by the subject created by the factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subject created by the factory.</typeparam>
        /// <param name="expression">Expression representing the subject factory.</param>
        /// <returns>Parameterized subject factory represented by the given expression.</returns>
        public IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> CreateQubjectFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TInput, TOutput>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (ExpressionServices.TryGetName(expression, out var uri))
            {
                return new KnownQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>(expression, uri, this);
            }

            return new QubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>(expression, this);
        }

        /// <summary>
        /// Creates a parameterized subject factory based on the given expression.
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
        /// <typeparam name="TInput">Type of the data received by the subject created by the factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subject created by the factory.</typeparam>
        /// <param name="expression">Expression representing the subject factory.</param>
        /// <returns>Parameterized subject factory represented by the given expression.</returns>
        public IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> CreateQubjectFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TInput, TOutput>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (ExpressionServices.TryGetName(expression, out var uri))
            {
                return new KnownQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>(expression, uri, this);
            }

            return new QubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>(expression, this);
        }

        /// <summary>
        /// Creates a parameterized subject factory based on the given expression.
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
        /// <typeparam name="TInput">Type of the data received by the subject created by the factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subject created by the factory.</typeparam>
        /// <param name="expression">Expression representing the subject factory.</param>
        /// <returns>Parameterized subject factory represented by the given expression.</returns>
        public IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> CreateQubjectFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TInput, TOutput>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (ExpressionServices.TryGetName(expression, out var uri))
            {
                return new KnownQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>(expression, uri, this);
            }

            return new QubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>(expression, this);
        }

        #endregion

        #region CreateQubscriptionFactory

        /// <summary>
        /// Creates a parameterized subscription factory based on the given expression.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
        /// <param name="expression">Expression representing the subscription factory.</param>
        /// <returns>Parameterized subscription factory represented by the given expression.</returns>
        public IReactiveQubscriptionFactory<TArg1, TArg2> CreateQubscriptionFactory<TArg1, TArg2>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (ExpressionServices.TryGetName(expression, out var uri))
            {
                return new KnownQubscriptionFactory<TArg1, TArg2>(expression, uri, this);
            }

            return new QubscriptionFactory<TArg1, TArg2>(expression, this);
        }

        /// <summary>
        /// Creates a parameterized subscription factory based on the given expression.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the subscription factory.</typeparam>
        /// <param name="expression">Expression representing the subscription factory.</param>
        /// <returns>Parameterized subscription factory represented by the given expression.</returns>
        public IReactiveQubscriptionFactory<TArg1, TArg2, TArg3> CreateQubscriptionFactory<TArg1, TArg2, TArg3>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (ExpressionServices.TryGetName(expression, out var uri))
            {
                return new KnownQubscriptionFactory<TArg1, TArg2, TArg3>(expression, uri, this);
            }

            return new QubscriptionFactory<TArg1, TArg2, TArg3>(expression, this);
        }

        /// <summary>
        /// Creates a parameterized subscription factory based on the given expression.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the subscription factory.</typeparam>
        /// <param name="expression">Expression representing the subscription factory.</param>
        /// <returns>Parameterized subscription factory represented by the given expression.</returns>
        public IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4> CreateQubscriptionFactory<TArg1, TArg2, TArg3, TArg4>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (ExpressionServices.TryGetName(expression, out var uri))
            {
                return new KnownQubscriptionFactory<TArg1, TArg2, TArg3, TArg4>(expression, uri, this);
            }

            return new QubscriptionFactory<TArg1, TArg2, TArg3, TArg4>(expression, this);
        }

        /// <summary>
        /// Creates a parameterized subscription factory based on the given expression.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the subscription factory.</typeparam>
        /// <param name="expression">Expression representing the subscription factory.</param>
        /// <returns>Parameterized subscription factory represented by the given expression.</returns>
        public IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5> CreateQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (ExpressionServices.TryGetName(expression, out var uri))
            {
                return new KnownQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5>(expression, uri, this);
            }

            return new QubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5>(expression, this);
        }

        /// <summary>
        /// Creates a parameterized subscription factory based on the given expression.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the subscription factory.</typeparam>
        /// <param name="expression">Expression representing the subscription factory.</param>
        /// <returns>Parameterized subscription factory represented by the given expression.</returns>
        public IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> CreateQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (ExpressionServices.TryGetName(expression, out var uri))
            {
                return new KnownQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(expression, uri, this);
            }

            return new QubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(expression, this);
        }

        /// <summary>
        /// Creates a parameterized subscription factory based on the given expression.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the subscription factory.</typeparam>
        /// <param name="expression">Expression representing the subscription factory.</param>
        /// <returns>Parameterized subscription factory represented by the given expression.</returns>
        public IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> CreateQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (ExpressionServices.TryGetName(expression, out var uri))
            {
                return new KnownQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(expression, uri, this);
            }

            return new QubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(expression, this);
        }

        /// <summary>
        /// Creates a parameterized subscription factory based on the given expression.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg8">Type of the eighth parameter passed to the subscription factory.</typeparam>
        /// <param name="expression">Expression representing the subscription factory.</param>
        /// <returns>Parameterized subscription factory represented by the given expression.</returns>
        public IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> CreateQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (ExpressionServices.TryGetName(expression, out var uri))
            {
                return new KnownQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(expression, uri, this);
            }

            return new QubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(expression, this);
        }

        /// <summary>
        /// Creates a parameterized subscription factory based on the given expression.
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
        /// <param name="expression">Expression representing the subscription factory.</param>
        /// <returns>Parameterized subscription factory represented by the given expression.</returns>
        public IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> CreateQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (ExpressionServices.TryGetName(expression, out var uri))
            {
                return new KnownQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(expression, uri, this);
            }

            return new QubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(expression, this);
        }

        /// <summary>
        /// Creates a parameterized subscription factory based on the given expression.
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
        /// <param name="expression">Expression representing the subscription factory.</param>
        /// <returns>Parameterized subscription factory represented by the given expression.</returns>
        public IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> CreateQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (ExpressionServices.TryGetName(expression, out var uri))
            {
                return new KnownQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>(expression, uri, this);
            }

            return new QubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>(expression, this);
        }

        /// <summary>
        /// Creates a parameterized subscription factory based on the given expression.
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
        /// <param name="expression">Expression representing the subscription factory.</param>
        /// <returns>Parameterized subscription factory represented by the given expression.</returns>
        public IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> CreateQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (ExpressionServices.TryGetName(expression, out var uri))
            {
                return new KnownQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>(expression, uri, this);
            }

            return new QubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>(expression, this);
        }

        /// <summary>
        /// Creates a parameterized subscription factory based on the given expression.
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
        /// <param name="expression">Expression representing the subscription factory.</param>
        /// <returns>Parameterized subscription factory represented by the given expression.</returns>
        public IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> CreateQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (ExpressionServices.TryGetName(expression, out var uri))
            {
                return new KnownQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>(expression, uri, this);
            }

            return new QubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>(expression, this);
        }

        /// <summary>
        /// Creates a parameterized subscription factory based on the given expression.
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
        /// <param name="expression">Expression representing the subscription factory.</param>
        /// <returns>Parameterized subscription factory represented by the given expression.</returns>
        public IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> CreateQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (ExpressionServices.TryGetName(expression, out var uri))
            {
                return new KnownQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>(expression, uri, this);
            }

            return new QubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>(expression, this);
        }

        /// <summary>
        /// Creates a parameterized subscription factory based on the given expression.
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
        /// <param name="expression">Expression representing the subscription factory.</param>
        /// <returns>Parameterized subscription factory represented by the given expression.</returns>
        public IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> CreateQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (ExpressionServices.TryGetName(expression, out var uri))
            {
                return new KnownQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>(expression, uri, this);
            }

            return new QubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>(expression, this);
        }

        /// <summary>
        /// Creates a parameterized subscription factory based on the given expression.
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
        /// <param name="expression">Expression representing the subscription factory.</param>
        /// <returns>Parameterized subscription factory represented by the given expression.</returns>
        public IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> CreateQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (ExpressionServices.TryGetName(expression, out var uri))
            {
                return new KnownQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>(expression, uri, this);
            }

            return new QubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>(expression, this);
        }

        #endregion

        #region Stream operations

        internal IReactiveQubject<TInput, TOutput> CreateStream<TArg1, TArg2, TInput, TOutput>(IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2> factory, TArg1 arg1, TArg2 arg2, Uri streamUri, object state)
        {
            return CreateStream<TInput, TOutput>(factory, new Expression[] { Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)) }, streamUri, state);
        }

        internal IReactiveQubject<TInput, TOutput> CreateStream<TArg1, TArg2, TArg3, TInput, TOutput>(IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3> factory, TArg1 arg1, TArg2 arg2, TArg3 arg3, Uri streamUri, object state)
        {
            return CreateStream<TInput, TOutput>(factory, new Expression[] { Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)) }, streamUri, state);
        }

        internal IReactiveQubject<TInput, TOutput> CreateStream<TArg1, TArg2, TArg3, TArg4, TInput, TOutput>(IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4> factory, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, Uri streamUri, object state)
        {
            return CreateStream<TInput, TOutput>(factory, new Expression[] { Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4)) }, streamUri, state);
        }

        internal IReactiveQubject<TInput, TOutput> CreateStream<TArg1, TArg2, TArg3, TArg4, TArg5, TInput, TOutput>(IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5> factory, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, Uri streamUri, object state)
        {
            return CreateStream<TInput, TOutput>(factory, new Expression[] { Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4)), Expression.Constant(arg5, typeof(TArg5)) }, streamUri, state);
        }

        internal IReactiveQubject<TInput, TOutput> CreateStream<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TInput, TOutput>(IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> factory, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, Uri streamUri, object state)
        {
            return CreateStream<TInput, TOutput>(factory, new Expression[] { Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4)), Expression.Constant(arg5, typeof(TArg5)), Expression.Constant(arg6, typeof(TArg6)) }, streamUri, state);
        }

        internal IReactiveQubject<TInput, TOutput> CreateStream<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TInput, TOutput>(IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> factory, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, Uri streamUri, object state)
        {
            return CreateStream<TInput, TOutput>(factory, new Expression[] { Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4)), Expression.Constant(arg5, typeof(TArg5)), Expression.Constant(arg6, typeof(TArg6)), Expression.Constant(arg7, typeof(TArg7)) }, streamUri, state);
        }

        internal IReactiveQubject<TInput, TOutput> CreateStream<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TInput, TOutput>(IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> factory, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, Uri streamUri, object state)
        {
            return CreateStream<TInput, TOutput>(factory, new Expression[] { Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4)), Expression.Constant(arg5, typeof(TArg5)), Expression.Constant(arg6, typeof(TArg6)), Expression.Constant(arg7, typeof(TArg7)), Expression.Constant(arg8, typeof(TArg8)) }, streamUri, state);
        }

        internal IReactiveQubject<TInput, TOutput> CreateStream<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TInput, TOutput>(IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> factory, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, Uri streamUri, object state)
        {
            return CreateStream<TInput, TOutput>(factory, new Expression[] { Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4)), Expression.Constant(arg5, typeof(TArg5)), Expression.Constant(arg6, typeof(TArg6)), Expression.Constant(arg7, typeof(TArg7)), Expression.Constant(arg8, typeof(TArg8)), Expression.Constant(arg9, typeof(TArg9)) }, streamUri, state);
        }

        internal IReactiveQubject<TInput, TOutput> CreateStream<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TInput, TOutput>(IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> factory, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, Uri streamUri, object state)
        {
            return CreateStream<TInput, TOutput>(factory, new Expression[] { Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4)), Expression.Constant(arg5, typeof(TArg5)), Expression.Constant(arg6, typeof(TArg6)), Expression.Constant(arg7, typeof(TArg7)), Expression.Constant(arg8, typeof(TArg8)), Expression.Constant(arg9, typeof(TArg9)), Expression.Constant(arg10, typeof(TArg10)) }, streamUri, state);
        }

        internal IReactiveQubject<TInput, TOutput> CreateStream<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TInput, TOutput>(IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> factory, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, Uri streamUri, object state)
        {
            return CreateStream<TInput, TOutput>(factory, new Expression[] { Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4)), Expression.Constant(arg5, typeof(TArg5)), Expression.Constant(arg6, typeof(TArg6)), Expression.Constant(arg7, typeof(TArg7)), Expression.Constant(arg8, typeof(TArg8)), Expression.Constant(arg9, typeof(TArg9)), Expression.Constant(arg10, typeof(TArg10)), Expression.Constant(arg11, typeof(TArg11)) }, streamUri, state);
        }

        internal IReactiveQubject<TInput, TOutput> CreateStream<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TInput, TOutput>(IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> factory, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, Uri streamUri, object state)
        {
            return CreateStream<TInput, TOutput>(factory, new Expression[] { Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4)), Expression.Constant(arg5, typeof(TArg5)), Expression.Constant(arg6, typeof(TArg6)), Expression.Constant(arg7, typeof(TArg7)), Expression.Constant(arg8, typeof(TArg8)), Expression.Constant(arg9, typeof(TArg9)), Expression.Constant(arg10, typeof(TArg10)), Expression.Constant(arg11, typeof(TArg11)), Expression.Constant(arg12, typeof(TArg12)) }, streamUri, state);
        }

        internal IReactiveQubject<TInput, TOutput> CreateStream<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TInput, TOutput>(IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> factory, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, Uri streamUri, object state)
        {
            return CreateStream<TInput, TOutput>(factory, new Expression[] { Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4)), Expression.Constant(arg5, typeof(TArg5)), Expression.Constant(arg6, typeof(TArg6)), Expression.Constant(arg7, typeof(TArg7)), Expression.Constant(arg8, typeof(TArg8)), Expression.Constant(arg9, typeof(TArg9)), Expression.Constant(arg10, typeof(TArg10)), Expression.Constant(arg11, typeof(TArg11)), Expression.Constant(arg12, typeof(TArg12)), Expression.Constant(arg13, typeof(TArg13)) }, streamUri, state);
        }

        internal IReactiveQubject<TInput, TOutput> CreateStream<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TInput, TOutput>(IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> factory, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, Uri streamUri, object state)
        {
            return CreateStream<TInput, TOutput>(factory, new Expression[] { Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4)), Expression.Constant(arg5, typeof(TArg5)), Expression.Constant(arg6, typeof(TArg6)), Expression.Constant(arg7, typeof(TArg7)), Expression.Constant(arg8, typeof(TArg8)), Expression.Constant(arg9, typeof(TArg9)), Expression.Constant(arg10, typeof(TArg10)), Expression.Constant(arg11, typeof(TArg11)), Expression.Constant(arg12, typeof(TArg12)), Expression.Constant(arg13, typeof(TArg13)), Expression.Constant(arg14, typeof(TArg14)) }, streamUri, state);
        }

        internal IReactiveQubject<TInput, TOutput> CreateStream<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TInput, TOutput>(IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> factory, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, TArg15 arg15, Uri streamUri, object state)
        {
            return CreateStream<TInput, TOutput>(factory, new Expression[] { Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4)), Expression.Constant(arg5, typeof(TArg5)), Expression.Constant(arg6, typeof(TArg6)), Expression.Constant(arg7, typeof(TArg7)), Expression.Constant(arg8, typeof(TArg8)), Expression.Constant(arg9, typeof(TArg9)), Expression.Constant(arg10, typeof(TArg10)), Expression.Constant(arg11, typeof(TArg11)), Expression.Constant(arg12, typeof(TArg12)), Expression.Constant(arg13, typeof(TArg13)), Expression.Constant(arg14, typeof(TArg14)), Expression.Constant(arg15, typeof(TArg15)) }, streamUri, state);
        }

        #endregion

        #region Subscription operations

        internal IReactiveQubscription CreateSubscription<TArg1, TArg2>(IReactiveQubscriptionFactory<TArg1, TArg2> factory, TArg1 arg1, TArg2 arg2, Uri subscriptionUri, object state)
        {
            return CreateSubscription(factory, new Expression[] { Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)) }, subscriptionUri, state);
        }

        internal IReactiveQubscription CreateSubscription<TArg1, TArg2, TArg3>(IReactiveQubscriptionFactory<TArg1, TArg2, TArg3> factory, TArg1 arg1, TArg2 arg2, TArg3 arg3, Uri subscriptionUri, object state)
        {
            return CreateSubscription(factory, new Expression[] { Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)) }, subscriptionUri, state);
        }

        internal IReactiveQubscription CreateSubscription<TArg1, TArg2, TArg3, TArg4>(IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4> factory, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, Uri subscriptionUri, object state)
        {
            return CreateSubscription(factory, new Expression[] { Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4)) }, subscriptionUri, state);
        }

        internal IReactiveQubscription CreateSubscription<TArg1, TArg2, TArg3, TArg4, TArg5>(IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5> factory, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, Uri subscriptionUri, object state)
        {
            return CreateSubscription(factory, new Expression[] { Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4)), Expression.Constant(arg5, typeof(TArg5)) }, subscriptionUri, state);
        }

        internal IReactiveQubscription CreateSubscription<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> factory, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, Uri subscriptionUri, object state)
        {
            return CreateSubscription(factory, new Expression[] { Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4)), Expression.Constant(arg5, typeof(TArg5)), Expression.Constant(arg6, typeof(TArg6)) }, subscriptionUri, state);
        }

        internal IReactiveQubscription CreateSubscription<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> factory, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, Uri subscriptionUri, object state)
        {
            return CreateSubscription(factory, new Expression[] { Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4)), Expression.Constant(arg5, typeof(TArg5)), Expression.Constant(arg6, typeof(TArg6)), Expression.Constant(arg7, typeof(TArg7)) }, subscriptionUri, state);
        }

        internal IReactiveQubscription CreateSubscription<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> factory, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, Uri subscriptionUri, object state)
        {
            return CreateSubscription(factory, new Expression[] { Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4)), Expression.Constant(arg5, typeof(TArg5)), Expression.Constant(arg6, typeof(TArg6)), Expression.Constant(arg7, typeof(TArg7)), Expression.Constant(arg8, typeof(TArg8)) }, subscriptionUri, state);
        }

        internal IReactiveQubscription CreateSubscription<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> factory, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, Uri subscriptionUri, object state)
        {
            return CreateSubscription(factory, new Expression[] { Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4)), Expression.Constant(arg5, typeof(TArg5)), Expression.Constant(arg6, typeof(TArg6)), Expression.Constant(arg7, typeof(TArg7)), Expression.Constant(arg8, typeof(TArg8)), Expression.Constant(arg9, typeof(TArg9)) }, subscriptionUri, state);
        }

        internal IReactiveQubscription CreateSubscription<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>(IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> factory, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, Uri subscriptionUri, object state)
        {
            return CreateSubscription(factory, new Expression[] { Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4)), Expression.Constant(arg5, typeof(TArg5)), Expression.Constant(arg6, typeof(TArg6)), Expression.Constant(arg7, typeof(TArg7)), Expression.Constant(arg8, typeof(TArg8)), Expression.Constant(arg9, typeof(TArg9)), Expression.Constant(arg10, typeof(TArg10)) }, subscriptionUri, state);
        }

        internal IReactiveQubscription CreateSubscription<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>(IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> factory, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, Uri subscriptionUri, object state)
        {
            return CreateSubscription(factory, new Expression[] { Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4)), Expression.Constant(arg5, typeof(TArg5)), Expression.Constant(arg6, typeof(TArg6)), Expression.Constant(arg7, typeof(TArg7)), Expression.Constant(arg8, typeof(TArg8)), Expression.Constant(arg9, typeof(TArg9)), Expression.Constant(arg10, typeof(TArg10)), Expression.Constant(arg11, typeof(TArg11)) }, subscriptionUri, state);
        }

        internal IReactiveQubscription CreateSubscription<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>(IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> factory, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, Uri subscriptionUri, object state)
        {
            return CreateSubscription(factory, new Expression[] { Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4)), Expression.Constant(arg5, typeof(TArg5)), Expression.Constant(arg6, typeof(TArg6)), Expression.Constant(arg7, typeof(TArg7)), Expression.Constant(arg8, typeof(TArg8)), Expression.Constant(arg9, typeof(TArg9)), Expression.Constant(arg10, typeof(TArg10)), Expression.Constant(arg11, typeof(TArg11)), Expression.Constant(arg12, typeof(TArg12)) }, subscriptionUri, state);
        }

        internal IReactiveQubscription CreateSubscription<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>(IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> factory, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, Uri subscriptionUri, object state)
        {
            return CreateSubscription(factory, new Expression[] { Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4)), Expression.Constant(arg5, typeof(TArg5)), Expression.Constant(arg6, typeof(TArg6)), Expression.Constant(arg7, typeof(TArg7)), Expression.Constant(arg8, typeof(TArg8)), Expression.Constant(arg9, typeof(TArg9)), Expression.Constant(arg10, typeof(TArg10)), Expression.Constant(arg11, typeof(TArg11)), Expression.Constant(arg12, typeof(TArg12)), Expression.Constant(arg13, typeof(TArg13)) }, subscriptionUri, state);
        }

        internal IReactiveQubscription CreateSubscription<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>(IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> factory, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, Uri subscriptionUri, object state)
        {
            return CreateSubscription(factory, new Expression[] { Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4)), Expression.Constant(arg5, typeof(TArg5)), Expression.Constant(arg6, typeof(TArg6)), Expression.Constant(arg7, typeof(TArg7)), Expression.Constant(arg8, typeof(TArg8)), Expression.Constant(arg9, typeof(TArg9)), Expression.Constant(arg10, typeof(TArg10)), Expression.Constant(arg11, typeof(TArg11)), Expression.Constant(arg12, typeof(TArg12)), Expression.Constant(arg13, typeof(TArg13)), Expression.Constant(arg14, typeof(TArg14)) }, subscriptionUri, state);
        }

        internal IReactiveQubscription CreateSubscription<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>(IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> factory, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, TArg15 arg15, Uri subscriptionUri, object state)
        {
            return CreateSubscription(factory, new Expression[] { Expression.Constant(arg1, typeof(TArg1)), Expression.Constant(arg2, typeof(TArg2)), Expression.Constant(arg3, typeof(TArg3)), Expression.Constant(arg4, typeof(TArg4)), Expression.Constant(arg5, typeof(TArg5)), Expression.Constant(arg6, typeof(TArg6)), Expression.Constant(arg7, typeof(TArg7)), Expression.Constant(arg8, typeof(TArg8)), Expression.Constant(arg9, typeof(TArg9)), Expression.Constant(arg10, typeof(TArg10)), Expression.Constant(arg11, typeof(TArg11)), Expression.Constant(arg12, typeof(TArg12)), Expression.Constant(arg13, typeof(TArg13)), Expression.Constant(arg14, typeof(TArg14)), Expression.Constant(arg15, typeof(TArg15)) }, subscriptionUri, state);
        }

        #endregion
    }
}
