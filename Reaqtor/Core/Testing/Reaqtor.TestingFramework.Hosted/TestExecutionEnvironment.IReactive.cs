// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Linq.Expressions;

using Reaqtor.Metadata;

namespace Reaqtor.TestingFramework
{
    public partial class TestExecutionEnvironment
    {
        public IReactive Reactive => new ReactiveImpl(this);

        private sealed class ReactiveImpl : IReactive
        {
            private readonly TestExecutionEnvironment _parent;

            public ReactiveImpl(TestExecutionEnvironment parent) => _parent = parent;

            public IReactiveQubjectFactory<TInput, TOutput, TArgs> GetStreamFactory<TArgs, TInput, TOutput>(Uri uri)
            {
                if (uri == new Uri("rx://subject/bridge"))
                {
                    return new BridgeFactory<TInput, TOutput, TArgs>(_parent);
                }
                else if (uri == new Uri("rx://subject/inner/refCount"))
                {
                    return new TunnelFactory<TInput, TOutput, TArgs>(_parent);
                }

                throw new NotImplementedException();
            }

            public IReactiveQubjectFactory<TInput, TOutput> GetStreamFactory<TInput, TOutput>(Uri uri)
            {
                if (uri == new Uri("rx://subject/inner"))
                {
                    return new SimpleSubjectFactory<TInput, TOutput>(_parent);
                }
                else if (uri == new Uri("rx://subject/inner/untyped"))
                {
                    return new SimpleUntypedSubjectFactory<TInput, TOutput>(_parent);
                }

                throw new NotImplementedException();
            }

            public IReactiveQubject<TInput, TOutput> GetStream<TInput, TOutput>(Uri uri) => (IReactiveQubject<TInput, TOutput>)_parent.GetArtifact(uri);

            #region Not implemented

            public IReactiveQbservable<T> GetObservable<T>(Uri uri) => throw new NotImplementedException();

            public Func<TArgs, IReactiveQbservable<TResult>> GetObservable<TArgs, TResult>(Uri uri) => throw new NotImplementedException();

            public IReactiveQbserver<T> GetObserver<T>(Uri uri) => throw new NotImplementedException();

            public Func<TArgs, IReactiveQbserver<TResult>> GetObserver<TArgs, TResult>(Uri uri) => throw new NotImplementedException();

            public IReactiveQubscription GetSubscription(Uri uri) => throw new NotImplementedException();

            public IReactiveQueryProvider Provider => throw new NotImplementedException();

            public void DefineStreamFactory<TInput, TOutput>(Uri uri, IReactiveQubjectFactory<TInput, TOutput> streamFactory, object state) => throw new NotImplementedException();

            public void DefineStreamFactory<TArgs, TInput, TOutput>(Uri uri, IReactiveQubjectFactory<TInput, TOutput, TArgs> streamFactory, object state) => throw new NotImplementedException();

            public void UndefineStreamFactory(Uri uri) => throw new NotImplementedException();

            public void DefineObservable<T>(Uri uri, IReactiveQbservable<T> observable, object state) => throw new NotImplementedException();

            public void DefineObservable<TArgs, TResult>(Uri uri, Expression<Func<TArgs, IReactiveQbservable<TResult>>> observable, object state) => throw new NotImplementedException();

            public void UndefineObservable(Uri uri) => throw new NotImplementedException();

            public void DefineObserver<T>(Uri uri, IReactiveQbserver<T> observer, object state) => throw new NotImplementedException();

            public void DefineObserver<TArgs, TResult>(Uri uri, Expression<Func<TArgs, IReactiveQbserver<TResult>>> observer, object state) => throw new NotImplementedException();

            public void UndefineObserver(Uri uri) => throw new NotImplementedException();

            public IReactiveQubscriptionFactory GetSubscriptionFactory(Uri uri) => throw new NotImplementedException();

            public IReactiveQubscriptionFactory<TArgs> GetSubscriptionFactory<TArgs>(Uri uri) => throw new NotImplementedException();

            public void DefineSubscriptionFactory(Uri uri, IReactiveQubscriptionFactory subscriptionFactory, object state) => throw new NotImplementedException();

            public void DefineSubscriptionFactory<TArgs>(Uri uri, IReactiveQubscriptionFactory<TArgs> subscriptionFactory, object state) => throw new NotImplementedException();

            public void UndefineSubscriptionFactory(Uri uri) => throw new NotImplementedException();

            public IQueryableDictionary<Uri, IReactiveStreamFactoryDefinition> StreamFactories => throw new NotImplementedException();

            public IQueryableDictionary<Uri, IReactiveStreamProcess> Streams => throw new NotImplementedException();

            public IQueryableDictionary<Uri, IReactiveObservableDefinition> Observables => throw new NotImplementedException();

            public IQueryableDictionary<Uri, IReactiveObserverDefinition> Observers => throw new NotImplementedException();

            public IQueryableDictionary<Uri, IReactiveSubscriptionProcess> Subscriptions => throw new NotImplementedException();

            public IQueryableDictionary<Uri, IReactiveSubscriptionFactoryDefinition> SubscriptionFactories => throw new NotImplementedException();

            #endregion
        }
    }
}
