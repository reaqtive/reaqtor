// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq.CompilerServices;
using System.Linq.CompilerServices.TypeSystem;
using System.Linq.Expressions;

namespace Reaqtor.Remoting.Deployable
{
    internal static class Binder
    {
        private static readonly Dictionary<string, Expression> s_registry = new()
        {
            { "es://observer/x", (Expression<Func<int, IObserver<T>>>)(x => new X<T>(x)) },
            { "es://observer/y", (Expression<Func<string, IObserver<T>>>)(s => new Y<T>(s)) },
            { "es://observer/z", (Expression<Func<int, IObserver<T>>>)(x => new Z<T>(x)) },
            { "es://observer/f", (Expression<Func<Tuple<IObserver<T>, IObserver<T>>, IObserver<T>>>)(t => new F<T>(t.Item1, t.Item2)) },
            { "es://observer/cf", (Expression<Func<Tuple<IObserver<T>, Func<Exception, IObserver<T>>>, IObserver<T>>>)(t => new CF<T>(t.Item1, t.Item2)) },
        };

        public static Expression Bind(Expression e)
        {
            var fvs = FreeVariableScanner.Scan(e);

            var vars = new List<ParameterExpression>();
            var bindings = new List<Expression>();

            foreach (var fv in fvs)
            {
                vars.Add(fv);

                var bound = s_registry[fv.Name];

                var typeMap = fv.Type.UnifyExact(bound.Type);
                var substitutor = new TypeSubstitutionExpressionVisitor(typeMap);
                var unified = substitutor.Visit(bound);

                bindings.Add(unified);
            }

            var res = BetaReducer.ReduceEager(Expression.Invoke(Expression.Lambda(e, vars), bindings), BetaReductionNodeTypes.Unrestricted, BetaReductionRestrictions.None, throwOnCycle: false);

            return res;
        }

        private sealed class X<T> : IObserver<T>
        {
            private readonly int _x;
            private int _count;

            public X(int x)
            {
                _x = x;
            }

            public void OnCompleted()
            {
                Console.WriteLine($"X({_x}).OnCompleted()");
            }

            public void OnError(Exception error)
            {
                Console.WriteLine($"X({_x}).OnError({error})");
            }

            public void OnNext(T value)
            {
                if (++_count % _x == 0)
                    throw new InvalidOperationException("Oops");

                Console.WriteLine($"X({_x}).OnNext({value})");
            }
        }

        private sealed class Y<T> : IObserver<T>
        {
            private readonly string _s;

            public Y(string s)
            {
                _s = s;
            }

            public void OnCompleted()
            {
                Console.WriteLine($"Y({_s}).OnCompleted()");
            }

            public void OnError(Exception error)
            {
                Console.WriteLine($"Y({_s}).OnError({error})");
            }

            public void OnNext(T value)
            {
                Console.WriteLine($"Y({_s}).OnNext({value})");
            }
        }

        private sealed class Z<T> : IObserver<T>
        {
            private readonly int _x;

            public Z(int x)
            {
                _x = x;
            }

            public void OnCompleted()
            {
                Console.WriteLine($"Z({_x}).OnCompleted()");
            }

            public void OnError(Exception error)
            {
                Console.WriteLine($"Z({_x}).OnError({error})");
            }

            public void OnNext(T value)
            {
                Console.WriteLine($"Z({_x}).OnNext({value})");
            }
        }

        private sealed class F<T> : IObserver<T>
        {
            private readonly IObserver<T> _first;
            private readonly IObserver<T> _second;

            public F(IObserver<T> first, IObserver<T> second)
            {
                _first = first;
                _second = second;
            }

            public void OnCompleted()
            {
                _first.OnCompleted();
                _second.OnCompleted();
            }

            public void OnError(Exception error)
            {
                _first.OnError(error);
                _second.OnError(error);
            }

            public void OnNext(T value)
            {
                _first.OnNext(value);
                _second.OnNext(value);
            }
        }

        private sealed class CF<T> : IObserver<T>
        {
            private readonly IObserver<T> _observer;
            private readonly Func<Exception, IObserver<T>> _selector;

            public CF(IObserver<T> observer, Func<Exception, IObserver<T>> selector)
            {
                _observer = observer;
                _selector = selector;
            }

#pragma warning disable CA1031 // Do not catch general exception types. (Gets propagated through the selector.)

            public void OnCompleted()
            {
                try
                {
                    _observer.OnCompleted();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"CF.OnCompleted(catch {ex.Message}).OnCompleted()");
                    _selector(ex).OnCompleted();
                }
            }

            public void OnError(Exception error)
            {
                try
                {
                    _observer.OnError(error);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"CF.OnError(catch {ex.Message}).OnError({error})");
                    _selector(ex).OnError(error);
                }
            }

            public void OnNext(T value)
            {
                try
                {
                    _observer.OnNext(value);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"CF.OnNext(catch {ex.Message}).OnNext({value})");
                    _selector(ex).OnNext(value);
                }
            }

#pragma warning restore CA1031
        }
    }
}
