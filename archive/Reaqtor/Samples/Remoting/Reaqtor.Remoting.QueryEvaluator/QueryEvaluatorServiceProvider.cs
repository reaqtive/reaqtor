// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.CompilerServices.TypeSystem;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using Nuqleon.DataModel;
using Nuqleon.DataModel.Serialization.Json;

using Nuqleon.Json.Serialization;

using Reaqtor.QueryEngine;

namespace Reaqtor.Remoting.QueryEvaluator
{
    internal class QueryEvaluatorServiceProvider : IReactiveServiceProvider
    {
        private readonly CheckpointingQueryEngine _queryEngine;
        private readonly IReactiveMetadataCache _metadata;
        private readonly Func<Expression, Expression> _rewriter;

        public QueryEvaluatorServiceProvider(CheckpointingQueryEngine queryEngine, IReactiveMetadataCache metadata, Func<Expression, Expression> rewriter)
        {
            _queryEngine = queryEngine;
            _metadata = metadata;
            _rewriter = rewriter;

            _queryEngine.Options.ForeignFunctionBinder = ForeignFunctionsBinder.Bind;
        }

        public async Task CreateSubscriptionAsync(Uri subscriptionUri, Expression subscription, object state, CancellationToken token)
        {
            using (_metadata.CreateScope())
            {
                await CreateSubscriptionCoreAsync(subscriptionUri, subscription, state, token).ConfigureAwait(false);
            }
        }

        public virtual Task CreateSubscriptionCoreAsync(Uri subscriptionUri, Expression subscription, object state, CancellationToken token)
        {
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));

            var rewritten = _rewriter(subscription);

            // NB: The functionality to insert the hook needs to be revisited in order to have an effect on subscriptions created via subscription
            //     factories. However, its applicability will be limited when factories for things like "composite subscriptions" are used. While
            //     only observable.Subscribe(observer) subscriptions can be "hooked" using a Finally operator, it's unclear how the same technique
            //     could be applied to e.g. composite subscriptions. The better option is likely to add a mechanism that can detect the transition
            //     of a subscription to a disposed state trigger from within the engine. However, that will lack information about the cause of
            //     termination, e.g. due to an OnError or OnCompleted message (in fact, in a general notion of subscriptions there's no requirement
            //     to have an observer given that the ISubscription algebra is one that operates on "traversable resources" in the most general
            //     sense imaginable).
            //
            //     One temporary workaround could be for the QC to intercept subscription factory definition operations and scan for a single
            //     top-level occurrence of rx://observable/subscribe, and attach the hook the the observable portion. Other non-hookable expressions
            //     could be rejected for the time being. The mechanism to put in this hook could simply rewrite rx://observable/subscribe to some
            //     other subscription factory which expands into (o, v) => rx://observable/subscribe(o.CleanupHook(), v).
            //
            //     See Reaqtor.Remoting.QueryCoordinator\QueryCoordinatorServiceProvider.cs for an implementation of the latter.

            var subscribeExpressionWithCompletedHook = rewritten.GetExpressionWithSubscriptionCleanupHook(throwWhenNotAttached: false);

            var subscribeExpressionSync = ExpressionRewriteHelpers.RewriteAsyncToSync(subscribeExpressionWithCompletedHook);

            return _queryEngine.ServiceProvider.CreateSubscriptionAsync(subscriptionUri, subscribeExpressionSync, state, token);
        }

        public Task DeleteSubscriptionAsync(Uri subscriptionUri, CancellationToken token)
        {
            using (_metadata.CreateScope())
            {
                DeleteSubscriptionCoreAsync(subscriptionUri, token);
                return Task.FromResult(true);
            }
        }

        public virtual Task DeleteSubscriptionCoreAsync(Uri subscriptionUri, CancellationToken token)
        {
            if (subscriptionUri == null)
                throw new ArgumentNullException(nameof(subscriptionUri));

            return _queryEngine.ServiceProvider.DeleteSubscriptionAsync(subscriptionUri, token);
        }

        public async Task CreateStreamAsync(Uri streamUri, Expression stream, object state, CancellationToken token)
        {
            using (_metadata.CreateScope())
            {
                await CreateStreamCoreAsync(streamUri, stream, state, token).ConfigureAwait(false);
            }
        }

        public virtual Task CreateStreamCoreAsync(Uri streamUri, Expression stream, object state, CancellationToken token)
        {
            if (streamUri == null)
                throw new ArgumentNullException(nameof(streamUri));
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            var syncStream = ExpressionRewriteHelpers.RewriteAsyncToSync(stream);

            return _queryEngine.ServiceProvider.CreateStreamAsync(streamUri, syncStream, state, token);
        }

        public async Task DeleteStreamAsync(Uri streamUri, CancellationToken token)
        {
            using (_metadata.CreateScope())
            {
                await DeleteStreamCoreAsync(streamUri, token).ConfigureAwait(false);
            }
        }

        public virtual Task DeleteStreamCoreAsync(Uri streamUri, CancellationToken token)
        {
            if (streamUri == null)
                throw new ArgumentNullException(nameof(streamUri));

            return _queryEngine.ServiceProvider.DeleteStreamAsync(streamUri, token);
        }

        #region Not Implemented

        public Task<IAsyncReactiveObserver<T>> GetObserverAsync<T>(Uri observerUri, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task DefineObservableAsync(Uri observableUri, Expression observable, object state, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task UndefineObservableAsync(Uri observableUri, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task DefineObserverAsync(Uri observerUri, Expression observer, object state, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task UndefineObserverAsync(Uri observerUri, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task DefineStreamFactoryAsync(Uri streamFactoryUri, Expression streamFactory, object state, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task UndefineStreamFactoryAsync(Uri streamFactoryUri, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task DefineSubscriptionFactoryAsync(Uri subscriptionFactoryUri, Expression subscriptionFactory, object state, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task UndefineSubscriptionFactoryAsync(Uri subscriptionFactoryUri, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public IQueryProvider Provider => throw new NotImplementedException();

        #endregion

        private static class ForeignFunctionsBinder
        {
            private static readonly IDictionary<string, LambdaExpression> externalFunctions = new Dictionary<string, LambdaExpression>
            {
                { Platform.Constants.Identifiers.GenerateLanguage, (Expression<Func<T, string, string>>)((value, lang) => Generate<T>(value, lang)) },
                { Platform.Constants.Identifiers.IsPrime, (Expression<Func<int, bool>>)(number => IsPrime(number)) },
                { Platform.Constants.Identifiers.DataModelJsonSerializeV1, (Expression<Func<T, string>>)(obj => SerializeJsonDataModelClassic(obj)) },
                { Platform.Constants.Identifiers.DataModelJsonSerializeV2, (Expression<Func<T, string>>)(obj => SerializeJsonDataModelFast(obj)) },
            };

            public static Expression Bind(string key)
            {
                if (externalFunctions.TryGetValue(key, out var expr))
                {
                    return expr;
                }

                return null;
            }

            private static string Generate<T>(T value, string lang)
            {
                return string.Format(CultureInfo.InvariantCulture, "Generating output for '{0}' in language '{1}'.", value, lang);
            }

            private static bool IsPrime(int i)
            {
                if (i <= 1)
                    return false;

                var max = (int)Math.Sqrt(i);
                for (var j = 2; j <= max; ++j)
                    if (i % j == 0)
                        return false;

                return true;
            }

            private static readonly DataSerializer s_serializer = DataSerializer.Create();

            private static string SerializeJsonDataModelClassic<T>(T value)
            {
                using var ms = new MemoryStream();

                s_serializer.Serialize(value, ms);

                ms.Position = 0;

                using var sr = new StreamReader(ms);

                return sr.ReadToEnd();
            }

            private static readonly ConditionalWeakTable<Type, object> s_serializers = new();
            private static readonly MethodInfo s_createSerializerOfT = ((MethodInfo)ReflectionHelpers.InfoOf(() => CreateSerializer<T>())).GetGenericMethodDefinition();

            private static string SerializeJsonDataModelFast<T>(T value)
            {
                var ser = (IFastJsonSerializer<T>)s_serializers.GetValue(typeof(T), CreateSerializer);
                return ser.Serialize(value);
            }

            private static object CreateSerializer(Type type)
            {
                return s_createSerializerOfT.MakeGenericMethod(type).Invoke(null, null);
            }

            private static object CreateSerializer<T>()
            {
                return FastJsonSerializerFactory.CreateSerializer<T>(NameProvider.Instance, FastJsonConcurrencyMode.ThreadSafe);
            }

            private sealed class NameProvider : INameProvider
            {
                public static readonly INameProvider Instance = new NameProvider();

                public string GetName(FieldInfo field)
                {
                    var mapping = field.GetCustomAttribute<MappingAttribute>();
                    if (mapping != null)
                    {
                        return mapping.Uri;
                    }
                    else
                    {
                        return field.Name;
                    }
                }

                public string GetName(PropertyInfo property)
                {
                    var mapping = property.GetCustomAttribute<MappingAttribute>();
                    if (mapping != null)
                    {
                        return mapping.Uri;
                    }
                    else
                    {
                        return property.Name;
                    }
                }
            }
        }
    }
}
