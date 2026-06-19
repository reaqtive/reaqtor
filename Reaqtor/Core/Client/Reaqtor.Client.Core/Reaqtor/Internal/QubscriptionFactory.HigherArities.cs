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
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor
{
    internal class QubscriptionFactory<TArg1, TArg2> : AsyncReactiveQubscriptionFactoryBase<TArg1, TArg2>
    {
        public QubscriptionFactory(Expression expression, IAsyncReactiveQueryProvider provider)
            : base(provider)
        {
            Expression = expression;
        }

        public override Expression Expression { get; }

        protected override Task<IAsyncReactiveQubscription> CreateAsyncCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, object state, CancellationToken token)
        {
            return ((AsyncReactiveQueryProviderBase)base.Provider).CreateSubscriptionAsync(this, arg1, arg2, subscriptionUri, state, token);
        }
    }

    internal class QubscriptionFactory<TArg1, TArg2, TArg3> : AsyncReactiveQubscriptionFactoryBase<TArg1, TArg2, TArg3>
    {
        public QubscriptionFactory(Expression expression, IAsyncReactiveQueryProvider provider)
            : base(provider)
        {
            Expression = expression;
        }

        public override Expression Expression { get; }

        protected override Task<IAsyncReactiveQubscription> CreateAsyncCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state, CancellationToken token)
        {
            return ((AsyncReactiveQueryProviderBase)base.Provider).CreateSubscriptionAsync(this, arg1, arg2, arg3, subscriptionUri, state, token);
        }
    }

    internal class QubscriptionFactory<TArg1, TArg2, TArg3, TArg4> : AsyncReactiveQubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4>
    {
        public QubscriptionFactory(Expression expression, IAsyncReactiveQueryProvider provider)
            : base(provider)
        {
            Expression = expression;
        }

        public override Expression Expression { get; }

        protected override Task<IAsyncReactiveQubscription> CreateAsyncCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, object state, CancellationToken token)
        {
            return ((AsyncReactiveQueryProviderBase)base.Provider).CreateSubscriptionAsync(this, arg1, arg2, arg3, arg4, subscriptionUri, state, token);
        }
    }

    internal class QubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5> : AsyncReactiveQubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4, TArg5>
    {
        public QubscriptionFactory(Expression expression, IAsyncReactiveQueryProvider provider)
            : base(provider)
        {
            Expression = expression;
        }

        public override Expression Expression { get; }

        protected override Task<IAsyncReactiveQubscription> CreateAsyncCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, object state, CancellationToken token)
        {
            return ((AsyncReactiveQueryProviderBase)base.Provider).CreateSubscriptionAsync(this, arg1, arg2, arg3, arg4, arg5, subscriptionUri, state, token);
        }
    }

    internal class QubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> : AsyncReactiveQubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>
    {
        public QubscriptionFactory(Expression expression, IAsyncReactiveQueryProvider provider)
            : base(provider)
        {
            Expression = expression;
        }

        public override Expression Expression { get; }

        protected override Task<IAsyncReactiveQubscription> CreateAsyncCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, object state, CancellationToken token)
        {
            return ((AsyncReactiveQueryProviderBase)base.Provider).CreateSubscriptionAsync(this, arg1, arg2, arg3, arg4, arg5, arg6, subscriptionUri, state, token);
        }
    }

    internal class QubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> : AsyncReactiveQubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>
    {
        public QubscriptionFactory(Expression expression, IAsyncReactiveQueryProvider provider)
            : base(provider)
        {
            Expression = expression;
        }

        public override Expression Expression { get; }

        protected override Task<IAsyncReactiveQubscription> CreateAsyncCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, object state, CancellationToken token)
        {
            return ((AsyncReactiveQueryProviderBase)base.Provider).CreateSubscriptionAsync(this, arg1, arg2, arg3, arg4, arg5, arg6, arg7, subscriptionUri, state, token);
        }
    }

    internal class QubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> : AsyncReactiveQubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>
    {
        public QubscriptionFactory(Expression expression, IAsyncReactiveQueryProvider provider)
            : base(provider)
        {
            Expression = expression;
        }

        public override Expression Expression { get; }

        protected override Task<IAsyncReactiveQubscription> CreateAsyncCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, object state, CancellationToken token)
        {
            return ((AsyncReactiveQueryProviderBase)base.Provider).CreateSubscriptionAsync(this, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, subscriptionUri, state, token);
        }
    }

    internal class QubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> : AsyncReactiveQubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>
    {
        public QubscriptionFactory(Expression expression, IAsyncReactiveQueryProvider provider)
            : base(provider)
        {
            Expression = expression;
        }

        public override Expression Expression { get; }

        protected override Task<IAsyncReactiveQubscription> CreateAsyncCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, object state, CancellationToken token)
        {
            return ((AsyncReactiveQueryProviderBase)base.Provider).CreateSubscriptionAsync(this, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, subscriptionUri, state, token);
        }
    }

    internal class QubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> : AsyncReactiveQubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>
    {
        public QubscriptionFactory(Expression expression, IAsyncReactiveQueryProvider provider)
            : base(provider)
        {
            Expression = expression;
        }

        public override Expression Expression { get; }

        protected override Task<IAsyncReactiveQubscription> CreateAsyncCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, object state, CancellationToken token)
        {
            return ((AsyncReactiveQueryProviderBase)base.Provider).CreateSubscriptionAsync(this, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, subscriptionUri, state, token);
        }
    }

    internal class QubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> : AsyncReactiveQubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>
    {
        public QubscriptionFactory(Expression expression, IAsyncReactiveQueryProvider provider)
            : base(provider)
        {
            Expression = expression;
        }

        public override Expression Expression { get; }

        protected override Task<IAsyncReactiveQubscription> CreateAsyncCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, object state, CancellationToken token)
        {
            return ((AsyncReactiveQueryProviderBase)base.Provider).CreateSubscriptionAsync(this, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, subscriptionUri, state, token);
        }
    }

    internal class QubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> : AsyncReactiveQubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>
    {
        public QubscriptionFactory(Expression expression, IAsyncReactiveQueryProvider provider)
            : base(provider)
        {
            Expression = expression;
        }

        public override Expression Expression { get; }

        protected override Task<IAsyncReactiveQubscription> CreateAsyncCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, object state, CancellationToken token)
        {
            return ((AsyncReactiveQueryProviderBase)base.Provider).CreateSubscriptionAsync(this, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, subscriptionUri, state, token);
        }
    }

    internal class QubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> : AsyncReactiveQubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>
    {
        public QubscriptionFactory(Expression expression, IAsyncReactiveQueryProvider provider)
            : base(provider)
        {
            Expression = expression;
        }

        public override Expression Expression { get; }

        protected override Task<IAsyncReactiveQubscription> CreateAsyncCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, object state, CancellationToken token)
        {
            return ((AsyncReactiveQueryProviderBase)base.Provider).CreateSubscriptionAsync(this, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, subscriptionUri, state, token);
        }
    }

    internal class QubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> : AsyncReactiveQubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>
    {
        public QubscriptionFactory(Expression expression, IAsyncReactiveQueryProvider provider)
            : base(provider)
        {
            Expression = expression;
        }

        public override Expression Expression { get; }

        protected override Task<IAsyncReactiveQubscription> CreateAsyncCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, object state, CancellationToken token)
        {
            return ((AsyncReactiveQueryProviderBase)base.Provider).CreateSubscriptionAsync(this, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, subscriptionUri, state, token);
        }
    }

    internal class QubscriptionFactory<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> : AsyncReactiveQubscriptionFactoryBase<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>
    {
        public QubscriptionFactory(Expression expression, IAsyncReactiveQueryProvider provider)
            : base(provider)
        {
            Expression = expression;
        }

        public override Expression Expression { get; }

        protected override Task<IAsyncReactiveQubscription> CreateAsyncCore(Uri subscriptionUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, TArg15 arg15, object state, CancellationToken token)
        {
            return ((AsyncReactiveQueryProviderBase)base.Provider).CreateSubscriptionAsync(this, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, subscriptionUri, state, token);
        }
    }

}
