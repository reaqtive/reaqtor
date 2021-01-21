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
    public partial class ReactiveDefinition
    {
        #region StreamFactory

        /// <summary>
        /// Defines a parameterized stream factory identified by the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <param name="streamFactory">Stream factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineStreamFactoryCore<TArg1, TArg2, TInput, TOutput>(Uri uri, IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2> streamFactory, object state = null)
        {
            if (streamFactory == null)
                throw new ArgumentNullException(nameof(streamFactory));

            var expression = _expressionServices.Normalize(streamFactory.Expression);
            _provider.DefineStreamFactory(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized stream factory identified by the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <param name="streamFactory">Stream factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineStreamFactoryCore<TArg1, TArg2, TArg3, TInput, TOutput>(Uri uri, IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3> streamFactory, object state = null)
        {
            if (streamFactory == null)
                throw new ArgumentNullException(nameof(streamFactory));

            var expression = _expressionServices.Normalize(streamFactory.Expression);
            _provider.DefineStreamFactory(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized stream factory identified by the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <param name="streamFactory">Stream factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineStreamFactoryCore<TArg1, TArg2, TArg3, TArg4, TInput, TOutput>(Uri uri, IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4> streamFactory, object state = null)
        {
            if (streamFactory == null)
                throw new ArgumentNullException(nameof(streamFactory));

            var expression = _expressionServices.Normalize(streamFactory.Expression);
            _provider.DefineStreamFactory(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized stream factory identified by the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the subject factory.</typeparam>
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="uri">URI identifying the stream factory.</param>
        /// <param name="streamFactory">Stream factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineStreamFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TInput, TOutput>(Uri uri, IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5> streamFactory, object state = null)
        {
            if (streamFactory == null)
                throw new ArgumentNullException(nameof(streamFactory));

            var expression = _expressionServices.Normalize(streamFactory.Expression);
            _provider.DefineStreamFactory(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized stream factory identified by the specified URI.
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
        /// <param name="streamFactory">Stream factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineStreamFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TInput, TOutput>(Uri uri, IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> streamFactory, object state = null)
        {
            if (streamFactory == null)
                throw new ArgumentNullException(nameof(streamFactory));

            var expression = _expressionServices.Normalize(streamFactory.Expression);
            _provider.DefineStreamFactory(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized stream factory identified by the specified URI.
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
        /// <param name="streamFactory">Stream factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineStreamFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TInput, TOutput>(Uri uri, IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> streamFactory, object state = null)
        {
            if (streamFactory == null)
                throw new ArgumentNullException(nameof(streamFactory));

            var expression = _expressionServices.Normalize(streamFactory.Expression);
            _provider.DefineStreamFactory(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized stream factory identified by the specified URI.
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
        /// <param name="streamFactory">Stream factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineStreamFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TInput, TOutput>(Uri uri, IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> streamFactory, object state = null)
        {
            if (streamFactory == null)
                throw new ArgumentNullException(nameof(streamFactory));

            var expression = _expressionServices.Normalize(streamFactory.Expression);
            _provider.DefineStreamFactory(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized stream factory identified by the specified URI.
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
        /// <param name="streamFactory">Stream factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineStreamFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TInput, TOutput>(Uri uri, IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> streamFactory, object state = null)
        {
            if (streamFactory == null)
                throw new ArgumentNullException(nameof(streamFactory));

            var expression = _expressionServices.Normalize(streamFactory.Expression);
            _provider.DefineStreamFactory(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized stream factory identified by the specified URI.
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
        /// <param name="streamFactory">Stream factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineStreamFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TInput, TOutput>(Uri uri, IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> streamFactory, object state = null)
        {
            if (streamFactory == null)
                throw new ArgumentNullException(nameof(streamFactory));

            var expression = _expressionServices.Normalize(streamFactory.Expression);
            _provider.DefineStreamFactory(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized stream factory identified by the specified URI.
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
        /// <param name="streamFactory">Stream factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineStreamFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TInput, TOutput>(Uri uri, IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> streamFactory, object state = null)
        {
            if (streamFactory == null)
                throw new ArgumentNullException(nameof(streamFactory));

            var expression = _expressionServices.Normalize(streamFactory.Expression);
            _provider.DefineStreamFactory(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized stream factory identified by the specified URI.
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
        /// <param name="streamFactory">Stream factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineStreamFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TInput, TOutput>(Uri uri, IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> streamFactory, object state = null)
        {
            if (streamFactory == null)
                throw new ArgumentNullException(nameof(streamFactory));

            var expression = _expressionServices.Normalize(streamFactory.Expression);
            _provider.DefineStreamFactory(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized stream factory identified by the specified URI.
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
        /// <param name="streamFactory">Stream factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineStreamFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TInput, TOutput>(Uri uri, IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> streamFactory, object state = null)
        {
            if (streamFactory == null)
                throw new ArgumentNullException(nameof(streamFactory));

            var expression = _expressionServices.Normalize(streamFactory.Expression);
            _provider.DefineStreamFactory(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized stream factory identified by the specified URI.
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
        /// <param name="streamFactory">Stream factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineStreamFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TInput, TOutput>(Uri uri, IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> streamFactory, object state = null)
        {
            if (streamFactory == null)
                throw new ArgumentNullException(nameof(streamFactory));

            var expression = _expressionServices.Normalize(streamFactory.Expression);
            _provider.DefineStreamFactory(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized stream factory identified by the specified URI.
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
        /// <param name="streamFactory">Stream factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineStreamFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TInput, TOutput>(Uri uri, IReactiveQubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> streamFactory, object state = null)
        {
            if (streamFactory == null)
                throw new ArgumentNullException(nameof(streamFactory));

            var expression = _expressionServices.Normalize(streamFactory.Expression);
            _provider.DefineStreamFactory(uri, expression, state);
        }

        #endregion

        #region Observable

        /// <summary>
        /// Defines a parameterized observable identified by the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <param name="observable">Observable to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineObservableCore<TArg1, TArg2, TResult>(Uri uri, Expression<Func<TArg1, TArg2, IReactiveQbservable<TResult>>> observable, object state = null)
        {
            if (observable == null)
                throw new ArgumentNullException(nameof(observable));

            var expression = _expressionServices.Normalize(observable);
            _provider.DefineObservable(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized observable identified by the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <param name="observable">Observable to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineObservableCore<TArg1, TArg2, TArg3, TResult>(Uri uri, Expression<Func<TArg1, TArg2, TArg3, IReactiveQbservable<TResult>>> observable, object state = null)
        {
            if (observable == null)
                throw new ArgumentNullException(nameof(observable));

            var expression = _expressionServices.Normalize(observable);
            _provider.DefineObservable(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized observable identified by the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <param name="observable">Observable to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineObservableCore<TArg1, TArg2, TArg3, TArg4, TResult>(Uri uri, Expression<Func<TArg1, TArg2, TArg3, TArg4, IReactiveQbservable<TResult>>> observable, object state = null)
        {
            if (observable == null)
                throw new ArgumentNullException(nameof(observable));

            var expression = _expressionServices.Normalize(observable);
            _provider.DefineObservable(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized observable identified by the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <param name="observable">Observable to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineObservableCore<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(Uri uri, Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, IReactiveQbservable<TResult>>> observable, object state = null)
        {
            if (observable == null)
                throw new ArgumentNullException(nameof(observable));

            var expression = _expressionServices.Normalize(observable);
            _provider.DefineObservable(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized observable identified by the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observable.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the observable.</typeparam>
        /// <typeparam name="TResult">Type of the data produced by the observable.</typeparam>
        /// <param name="uri">URI identifying the observable.</param>
        /// <param name="observable">Observable to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineObservableCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(Uri uri, Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, IReactiveQbservable<TResult>>> observable, object state = null)
        {
            if (observable == null)
                throw new ArgumentNullException(nameof(observable));

            var expression = _expressionServices.Normalize(observable);
            _provider.DefineObservable(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized observable identified by the specified URI.
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
        /// <param name="observable">Observable to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineObservableCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(Uri uri, Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, IReactiveQbservable<TResult>>> observable, object state = null)
        {
            if (observable == null)
                throw new ArgumentNullException(nameof(observable));

            var expression = _expressionServices.Normalize(observable);
            _provider.DefineObservable(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized observable identified by the specified URI.
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
        /// <param name="observable">Observable to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineObservableCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(Uri uri, Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, IReactiveQbservable<TResult>>> observable, object state = null)
        {
            if (observable == null)
                throw new ArgumentNullException(nameof(observable));

            var expression = _expressionServices.Normalize(observable);
            _provider.DefineObservable(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized observable identified by the specified URI.
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
        /// <param name="observable">Observable to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineObservableCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>(Uri uri, Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, IReactiveQbservable<TResult>>> observable, object state = null)
        {
            if (observable == null)
                throw new ArgumentNullException(nameof(observable));

            var expression = _expressionServices.Normalize(observable);
            _provider.DefineObservable(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized observable identified by the specified URI.
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
        /// <param name="observable">Observable to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineObservableCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>(Uri uri, Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, IReactiveQbservable<TResult>>> observable, object state = null)
        {
            if (observable == null)
                throw new ArgumentNullException(nameof(observable));

            var expression = _expressionServices.Normalize(observable);
            _provider.DefineObservable(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized observable identified by the specified URI.
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
        /// <param name="observable">Observable to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineObservableCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>(Uri uri, Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, IReactiveQbservable<TResult>>> observable, object state = null)
        {
            if (observable == null)
                throw new ArgumentNullException(nameof(observable));

            var expression = _expressionServices.Normalize(observable);
            _provider.DefineObservable(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized observable identified by the specified URI.
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
        /// <param name="observable">Observable to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineObservableCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>(Uri uri, Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, IReactiveQbservable<TResult>>> observable, object state = null)
        {
            if (observable == null)
                throw new ArgumentNullException(nameof(observable));

            var expression = _expressionServices.Normalize(observable);
            _provider.DefineObservable(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized observable identified by the specified URI.
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
        /// <param name="observable">Observable to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineObservableCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>(Uri uri, Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, IReactiveQbservable<TResult>>> observable, object state = null)
        {
            if (observable == null)
                throw new ArgumentNullException(nameof(observable));

            var expression = _expressionServices.Normalize(observable);
            _provider.DefineObservable(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized observable identified by the specified URI.
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
        /// <param name="observable">Observable to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineObservableCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>(Uri uri, Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, IReactiveQbservable<TResult>>> observable, object state = null)
        {
            if (observable == null)
                throw new ArgumentNullException(nameof(observable));

            var expression = _expressionServices.Normalize(observable);
            _provider.DefineObservable(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized observable identified by the specified URI.
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
        /// <param name="observable">Observable to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineObservableCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TResult>(Uri uri, Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, IReactiveQbservable<TResult>>> observable, object state = null)
        {
            if (observable == null)
                throw new ArgumentNullException(nameof(observable));

            var expression = _expressionServices.Normalize(observable);
            _provider.DefineObservable(uri, expression, state);
        }

        #endregion

        #region Observer

        /// <summary>
        /// Defines a parameterized observer identified by the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <param name="observer">Observer to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineObserverCore<TArg1, TArg2, TResult>(Uri uri, Expression<Func<TArg1, TArg2, IReactiveQbserver<TResult>>> observer, object state = null)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var expression = _expressionServices.Normalize(observer);
            _provider.DefineObserver(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized observer identified by the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <param name="observer">Observer to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineObserverCore<TArg1, TArg2, TArg3, TResult>(Uri uri, Expression<Func<TArg1, TArg2, TArg3, IReactiveQbserver<TResult>>> observer, object state = null)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var expression = _expressionServices.Normalize(observer);
            _provider.DefineObserver(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized observer identified by the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <param name="observer">Observer to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineObserverCore<TArg1, TArg2, TArg3, TArg4, TResult>(Uri uri, Expression<Func<TArg1, TArg2, TArg3, TArg4, IReactiveQbserver<TResult>>> observer, object state = null)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var expression = _expressionServices.Normalize(observer);
            _provider.DefineObserver(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized observer identified by the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <param name="observer">Observer to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineObserverCore<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(Uri uri, Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, IReactiveQbserver<TResult>>> observer, object state = null)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var expression = _expressionServices.Normalize(observer);
            _provider.DefineObserver(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized observer identified by the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the observer.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the observer.</typeparam>
        /// <typeparam name="TResult">Type of the data received by the observer.</typeparam>
        /// <param name="uri">URI identifying the observer.</param>
        /// <param name="observer">Observer to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineObserverCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(Uri uri, Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, IReactiveQbserver<TResult>>> observer, object state = null)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var expression = _expressionServices.Normalize(observer);
            _provider.DefineObserver(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized observer identified by the specified URI.
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
        /// <param name="observer">Observer to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineObserverCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(Uri uri, Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, IReactiveQbserver<TResult>>> observer, object state = null)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var expression = _expressionServices.Normalize(observer);
            _provider.DefineObserver(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized observer identified by the specified URI.
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
        /// <param name="observer">Observer to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineObserverCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(Uri uri, Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, IReactiveQbserver<TResult>>> observer, object state = null)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var expression = _expressionServices.Normalize(observer);
            _provider.DefineObserver(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized observer identified by the specified URI.
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
        /// <param name="observer">Observer to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineObserverCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>(Uri uri, Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, IReactiveQbserver<TResult>>> observer, object state = null)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var expression = _expressionServices.Normalize(observer);
            _provider.DefineObserver(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized observer identified by the specified URI.
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
        /// <param name="observer">Observer to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineObserverCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>(Uri uri, Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, IReactiveQbserver<TResult>>> observer, object state = null)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var expression = _expressionServices.Normalize(observer);
            _provider.DefineObserver(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized observer identified by the specified URI.
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
        /// <param name="observer">Observer to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineObserverCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>(Uri uri, Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, IReactiveQbserver<TResult>>> observer, object state = null)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var expression = _expressionServices.Normalize(observer);
            _provider.DefineObserver(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized observer identified by the specified URI.
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
        /// <param name="observer">Observer to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineObserverCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>(Uri uri, Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, IReactiveQbserver<TResult>>> observer, object state = null)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var expression = _expressionServices.Normalize(observer);
            _provider.DefineObserver(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized observer identified by the specified URI.
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
        /// <param name="observer">Observer to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineObserverCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>(Uri uri, Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, IReactiveQbserver<TResult>>> observer, object state = null)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var expression = _expressionServices.Normalize(observer);
            _provider.DefineObserver(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized observer identified by the specified URI.
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
        /// <param name="observer">Observer to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineObserverCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>(Uri uri, Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, IReactiveQbserver<TResult>>> observer, object state = null)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var expression = _expressionServices.Normalize(observer);
            _provider.DefineObserver(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized observer identified by the specified URI.
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
        /// <param name="observer">Observer to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineObserverCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TResult>(Uri uri, Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, IReactiveQbserver<TResult>>> observer, object state = null)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var expression = _expressionServices.Normalize(observer);
            _provider.DefineObserver(uri, expression, state);
        }

        #endregion

        #region SubscriptionFactory

        /// <summary>
        /// Defines a parameterized subscription factory identified by the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <param name="subscriptionFactory">Subscription factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineSubscriptionFactoryCore<TArg1, TArg2>(Uri uri, IReactiveQubscriptionFactory<TArg1, TArg2> subscriptionFactory, object state = null)
        {
            if (subscriptionFactory == null)
                throw new ArgumentNullException(nameof(subscriptionFactory));

            var expression = _expressionServices.Normalize(subscriptionFactory.Expression);
            _provider.DefineSubscriptionFactory(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized subscription factory identified by the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the subscription factory.</typeparam>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <param name="subscriptionFactory">Subscription factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineSubscriptionFactoryCore<TArg1, TArg2, TArg3>(Uri uri, IReactiveQubscriptionFactory<TArg1, TArg2, TArg3> subscriptionFactory, object state = null)
        {
            if (subscriptionFactory == null)
                throw new ArgumentNullException(nameof(subscriptionFactory));

            var expression = _expressionServices.Normalize(subscriptionFactory.Expression);
            _provider.DefineSubscriptionFactory(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized subscription factory identified by the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the subscription factory.</typeparam>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <param name="subscriptionFactory">Subscription factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineSubscriptionFactoryCore<TArg1, TArg2, TArg3, TArg4>(Uri uri, IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4> subscriptionFactory, object state = null)
        {
            if (subscriptionFactory == null)
                throw new ArgumentNullException(nameof(subscriptionFactory));

            var expression = _expressionServices.Normalize(subscriptionFactory.Expression);
            _provider.DefineSubscriptionFactory(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized subscription factory identified by the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the subscription factory.</typeparam>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <param name="subscriptionFactory">Subscription factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineSubscriptionFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5>(Uri uri, IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5> subscriptionFactory, object state = null)
        {
            if (subscriptionFactory == null)
                throw new ArgumentNullException(nameof(subscriptionFactory));

            var expression = _expressionServices.Normalize(subscriptionFactory.Expression);
            _provider.DefineSubscriptionFactory(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized subscription factory identified by the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the subscription factory.</typeparam>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <param name="subscriptionFactory">Subscription factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineSubscriptionFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(Uri uri, IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> subscriptionFactory, object state = null)
        {
            if (subscriptionFactory == null)
                throw new ArgumentNullException(nameof(subscriptionFactory));

            var expression = _expressionServices.Normalize(subscriptionFactory.Expression);
            _provider.DefineSubscriptionFactory(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized subscription factory identified by the specified URI.
        /// </summary>
        /// <typeparam name="TArg1">Type of the first parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg2">Type of the second parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg3">Type of the third parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg4">Type of the fourth parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg5">Type of the fifth parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg6">Type of the sixth parameter passed to the subscription factory.</typeparam>
        /// <typeparam name="TArg7">Type of the seventh parameter passed to the subscription factory.</typeparam>
        /// <param name="uri">URI identifying the subscription factory.</param>
        /// <param name="subscriptionFactory">Subscription factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineSubscriptionFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(Uri uri, IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> subscriptionFactory, object state = null)
        {
            if (subscriptionFactory == null)
                throw new ArgumentNullException(nameof(subscriptionFactory));

            var expression = _expressionServices.Normalize(subscriptionFactory.Expression);
            _provider.DefineSubscriptionFactory(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized subscription factory identified by the specified URI.
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
        /// <param name="subscriptionFactory">Subscription factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineSubscriptionFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(Uri uri, IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> subscriptionFactory, object state = null)
        {
            if (subscriptionFactory == null)
                throw new ArgumentNullException(nameof(subscriptionFactory));

            var expression = _expressionServices.Normalize(subscriptionFactory.Expression);
            _provider.DefineSubscriptionFactory(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized subscription factory identified by the specified URI.
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
        /// <param name="subscriptionFactory">Subscription factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineSubscriptionFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(Uri uri, IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> subscriptionFactory, object state = null)
        {
            if (subscriptionFactory == null)
                throw new ArgumentNullException(nameof(subscriptionFactory));

            var expression = _expressionServices.Normalize(subscriptionFactory.Expression);
            _provider.DefineSubscriptionFactory(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized subscription factory identified by the specified URI.
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
        /// <param name="subscriptionFactory">Subscription factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineSubscriptionFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>(Uri uri, IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> subscriptionFactory, object state = null)
        {
            if (subscriptionFactory == null)
                throw new ArgumentNullException(nameof(subscriptionFactory));

            var expression = _expressionServices.Normalize(subscriptionFactory.Expression);
            _provider.DefineSubscriptionFactory(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized subscription factory identified by the specified URI.
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
        /// <param name="subscriptionFactory">Subscription factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineSubscriptionFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>(Uri uri, IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> subscriptionFactory, object state = null)
        {
            if (subscriptionFactory == null)
                throw new ArgumentNullException(nameof(subscriptionFactory));

            var expression = _expressionServices.Normalize(subscriptionFactory.Expression);
            _provider.DefineSubscriptionFactory(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized subscription factory identified by the specified URI.
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
        /// <param name="subscriptionFactory">Subscription factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineSubscriptionFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>(Uri uri, IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> subscriptionFactory, object state = null)
        {
            if (subscriptionFactory == null)
                throw new ArgumentNullException(nameof(subscriptionFactory));

            var expression = _expressionServices.Normalize(subscriptionFactory.Expression);
            _provider.DefineSubscriptionFactory(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized subscription factory identified by the specified URI.
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
        /// <param name="subscriptionFactory">Subscription factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineSubscriptionFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>(Uri uri, IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> subscriptionFactory, object state = null)
        {
            if (subscriptionFactory == null)
                throw new ArgumentNullException(nameof(subscriptionFactory));

            var expression = _expressionServices.Normalize(subscriptionFactory.Expression);
            _provider.DefineSubscriptionFactory(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized subscription factory identified by the specified URI.
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
        /// <param name="subscriptionFactory">Subscription factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineSubscriptionFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>(Uri uri, IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> subscriptionFactory, object state = null)
        {
            if (subscriptionFactory == null)
                throw new ArgumentNullException(nameof(subscriptionFactory));

            var expression = _expressionServices.Normalize(subscriptionFactory.Expression);
            _provider.DefineSubscriptionFactory(uri, expression, state);
        }

        /// <summary>
        /// Defines a parameterized subscription factory identified by the specified URI.
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
        /// <param name="subscriptionFactory">Subscription factory to be defined.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        protected override void DefineSubscriptionFactoryCore<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>(Uri uri, IReactiveQubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> subscriptionFactory, object state = null)
        {
            if (subscriptionFactory == null)
                throw new ArgumentNullException(nameof(subscriptionFactory));

            var expression = _expressionServices.Normalize(subscriptionFactory.Expression);
            _provider.DefineSubscriptionFactory(uri, expression, state);
        }

        #endregion
    }
}
