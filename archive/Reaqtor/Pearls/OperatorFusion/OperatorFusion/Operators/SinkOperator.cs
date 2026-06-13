// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Exposes query operators in a fluent interface pattern as a stepping stone
// towards an approach based on proper binding.
//
// BD - October 2014
//

using System;
using System.Collections.Generic;
using System.Reactive.Disposables;

namespace OperatorFusion
{
    internal class SinkOperator<R> : Operator<R>
    {
        public Func<IObservable<T>, IObservable<R>> Compile<T>() // TODO: signature
        {
            // Quick and dirty way to scan the tree assuming it's a linear chain for now.

            var chain = new List<IFusionOperator>();

            var it = (Operator)this;

            while (it.Inputs.Length != 0)
            {
                chain.Insert(0, it.Factory);

                if (it.Inputs.Length > 1)
                    throw new NotImplementedException();

                it = it.Inputs[0];
            }

            var inputType = it.Type;
            var res = Compiler.Compile(inputType, chain.ToArray());
            return source => new Observable<T>(source, (o, d) => (IObserver<T>)Activator.CreateInstance(res, new object[] { o, d })); // TODO: parameters
        }

        private sealed class Observable<T> : IObservable<R>
        {
            private readonly IObservable<T> _source;
            private readonly Func<IObserver<R>, IDisposable, IObserver<T>> _getSink;

            public Observable(IObservable<T> source, Func<IObserver<R>, IDisposable, IObserver<T>> getSink)
            {
                _source = source;
                _getSink = getSink;
            }

            public IDisposable Subscribe(IObserver<R> observer)
            {
                var sad = new SingleAssignmentDisposable();

                var sink = _getSink(observer, sad);

                sad.Disposable = _source.Subscribe(sink);

                return sad;
            }
        }
    }
}
