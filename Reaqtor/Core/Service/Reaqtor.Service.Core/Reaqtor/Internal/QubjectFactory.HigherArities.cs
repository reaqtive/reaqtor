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
    internal class QubjectFactory<TInput, TOutput, TArg1, TArg2> : ReactiveQubjectFactoryBase<TInput, TOutput, TArg1, TArg2>
    {
        public QubjectFactory(Expression expression, IReactiveQueryProvider provider)
            : base(provider)
        {
            Expression = expression;
        }

        public override Expression Expression { get; }

        protected override IReactiveQubject<TInput, TOutput> CreateCore(Uri streamUri, TArg1 arg1, TArg2 arg2, object state)
        {
            return ((ReactiveQueryProviderBase)base.Provider).CreateStream(this, arg1, arg2, streamUri, state);
        }
    }

    internal class QubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3> : ReactiveQubjectFactoryBase<TInput, TOutput, TArg1, TArg2, TArg3>
    {
        public QubjectFactory(Expression expression, IReactiveQueryProvider provider)
            : base(provider)
        {
            Expression = expression;
        }

        public override Expression Expression { get; }

        protected override IReactiveQubject<TInput, TOutput> CreateCore(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state)
        {
            return ((ReactiveQueryProviderBase)base.Provider).CreateStream(this, arg1, arg2, arg3, streamUri, state);
        }
    }

    internal class QubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4> : ReactiveQubjectFactoryBase<TInput, TOutput, TArg1, TArg2, TArg3, TArg4>
    {
        public QubjectFactory(Expression expression, IReactiveQueryProvider provider)
            : base(provider)
        {
            Expression = expression;
        }

        public override Expression Expression { get; }

        protected override IReactiveQubject<TInput, TOutput> CreateCore(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, object state)
        {
            return ((ReactiveQueryProviderBase)base.Provider).CreateStream(this, arg1, arg2, arg3, arg4, streamUri, state);
        }
    }

    internal class QubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5> : ReactiveQubjectFactoryBase<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5>
    {
        public QubjectFactory(Expression expression, IReactiveQueryProvider provider)
            : base(provider)
        {
            Expression = expression;
        }

        public override Expression Expression { get; }

        protected override IReactiveQubject<TInput, TOutput> CreateCore(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, object state)
        {
            return ((ReactiveQueryProviderBase)base.Provider).CreateStream(this, arg1, arg2, arg3, arg4, arg5, streamUri, state);
        }
    }

    internal class QubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> : ReactiveQubjectFactoryBase<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>
    {
        public QubjectFactory(Expression expression, IReactiveQueryProvider provider)
            : base(provider)
        {
            Expression = expression;
        }

        public override Expression Expression { get; }

        protected override IReactiveQubject<TInput, TOutput> CreateCore(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, object state)
        {
            return ((ReactiveQueryProviderBase)base.Provider).CreateStream(this, arg1, arg2, arg3, arg4, arg5, arg6, streamUri, state);
        }
    }

    internal class QubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> : ReactiveQubjectFactoryBase<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>
    {
        public QubjectFactory(Expression expression, IReactiveQueryProvider provider)
            : base(provider)
        {
            Expression = expression;
        }

        public override Expression Expression { get; }

        protected override IReactiveQubject<TInput, TOutput> CreateCore(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, object state)
        {
            return ((ReactiveQueryProviderBase)base.Provider).CreateStream(this, arg1, arg2, arg3, arg4, arg5, arg6, arg7, streamUri, state);
        }
    }

    internal class QubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> : ReactiveQubjectFactoryBase<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>
    {
        public QubjectFactory(Expression expression, IReactiveQueryProvider provider)
            : base(provider)
        {
            Expression = expression;
        }

        public override Expression Expression { get; }

        protected override IReactiveQubject<TInput, TOutput> CreateCore(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, object state)
        {
            return ((ReactiveQueryProviderBase)base.Provider).CreateStream(this, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, streamUri, state);
        }
    }

    internal class QubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> : ReactiveQubjectFactoryBase<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>
    {
        public QubjectFactory(Expression expression, IReactiveQueryProvider provider)
            : base(provider)
        {
            Expression = expression;
        }

        public override Expression Expression { get; }

        protected override IReactiveQubject<TInput, TOutput> CreateCore(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, object state)
        {
            return ((ReactiveQueryProviderBase)base.Provider).CreateStream(this, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, streamUri, state);
        }
    }

    internal class QubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10> : ReactiveQubjectFactoryBase<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10>
    {
        public QubjectFactory(Expression expression, IReactiveQueryProvider provider)
            : base(provider)
        {
            Expression = expression;
        }

        public override Expression Expression { get; }

        protected override IReactiveQubject<TInput, TOutput> CreateCore(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, object state)
        {
            return ((ReactiveQueryProviderBase)base.Provider).CreateStream(this, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, streamUri, state);
        }
    }

    internal class QubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11> : ReactiveQubjectFactoryBase<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11>
    {
        public QubjectFactory(Expression expression, IReactiveQueryProvider provider)
            : base(provider)
        {
            Expression = expression;
        }

        public override Expression Expression { get; }

        protected override IReactiveQubject<TInput, TOutput> CreateCore(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, object state)
        {
            return ((ReactiveQueryProviderBase)base.Provider).CreateStream(this, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, streamUri, state);
        }
    }

    internal class QubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12> : ReactiveQubjectFactoryBase<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12>
    {
        public QubjectFactory(Expression expression, IReactiveQueryProvider provider)
            : base(provider)
        {
            Expression = expression;
        }

        public override Expression Expression { get; }

        protected override IReactiveQubject<TInput, TOutput> CreateCore(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, object state)
        {
            return ((ReactiveQueryProviderBase)base.Provider).CreateStream(this, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, streamUri, state);
        }
    }

    internal class QubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13> : ReactiveQubjectFactoryBase<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13>
    {
        public QubjectFactory(Expression expression, IReactiveQueryProvider provider)
            : base(provider)
        {
            Expression = expression;
        }

        public override Expression Expression { get; }

        protected override IReactiveQubject<TInput, TOutput> CreateCore(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, object state)
        {
            return ((ReactiveQueryProviderBase)base.Provider).CreateStream(this, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, streamUri, state);
        }
    }

    internal class QubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14> : ReactiveQubjectFactoryBase<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14>
    {
        public QubjectFactory(Expression expression, IReactiveQueryProvider provider)
            : base(provider)
        {
            Expression = expression;
        }

        public override Expression Expression { get; }

        protected override IReactiveQubject<TInput, TOutput> CreateCore(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, object state)
        {
            return ((ReactiveQueryProviderBase)base.Provider).CreateStream(this, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, streamUri, state);
        }
    }

    internal class QubjectFactory<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15> : ReactiveQubjectFactoryBase<TInput, TOutput, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15>
    {
        public QubjectFactory(Expression expression, IReactiveQueryProvider provider)
            : base(provider)
        {
            Expression = expression;
        }

        public override Expression Expression { get; }

        protected override IReactiveQubject<TInput, TOutput> CreateCore(Uri streamUri, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, TArg15 arg15, object state)
        {
            return ((ReactiveQueryProviderBase)base.Provider).CreateStream(this, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, streamUri, state);
        }
    }

}
