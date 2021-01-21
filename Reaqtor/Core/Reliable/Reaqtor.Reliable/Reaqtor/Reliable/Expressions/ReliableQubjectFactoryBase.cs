// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;

using Reaqtor.Reliable.Client;

namespace Reaqtor.Reliable.Expressions
{
    public abstract class ReliableQubjectFactoryBase<TInput, TOutput> : IReliableQubjectFactory<TInput, TOutput>
    {
        protected ReliableQubjectFactoryBase(IReliableQueryProvider provider) => Provider = provider;

        public IReliableMultiQubject<TInput, TOutput> Create(Uri streamUri, object state = null)
        {
            if (streamUri == null)
                throw new ArgumentNullException(nameof(streamUri));

            return CreateCore(streamUri, state);
        }

        IReliableReactiveMultiSubject<TInput, TOutput> IReliableReactiveSubjectFactory<TInput, TOutput>.Create(Uri streamUri, object state)
        {
            if (streamUri == null)
                throw new ArgumentNullException(nameof(streamUri));

            return CreateCore(streamUri, state);
        }

        protected abstract IReliableMultiQubject<TInput, TOutput> CreateCore(Uri streamUri, object state);

        public IReliableQueryProvider Provider { get; }

        public abstract Expression Expression { get; }
    }

    public abstract class ReliableQubjectFactoryBase<TInput, TOutput, TArg> : IReliableQubjectFactory<TInput, TOutput, TArg>
    {
        protected ReliableQubjectFactoryBase(IReliableQueryProvider provider) => Provider = provider;

        public IReliableMultiQubject<TInput, TOutput> Create(Uri streamUri, TArg argument, object state = null)
        {
            if (streamUri == null)
                throw new ArgumentNullException(nameof(streamUri));

            return CreateCore(streamUri, argument, state);
        }

        IReliableReactiveMultiSubject<TInput, TOutput> IReliableReactiveSubjectFactory<TInput, TOutput, TArg>.Create(Uri streamUri, TArg argument, object state)
        {
            if (streamUri == null)
                throw new ArgumentNullException(nameof(streamUri));

            return CreateCore(streamUri, argument, state);
        }

        protected abstract IReliableMultiQubject<TInput, TOutput> CreateCore(Uri streamUri, TArg argument, object state);

        public IReliableQueryProvider Provider { get; }

        public abstract Expression Expression { get; }
    }
}
