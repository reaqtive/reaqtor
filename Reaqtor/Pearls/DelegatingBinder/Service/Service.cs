// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.CompilerServices.TypeSystem;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace DelegatingBinder
{
    internal class Service : IService
    {
        private readonly IDictionary<string, object> _registry;

        public Service()
        {
            _registry = new Dictionary<string, object>
            {
                { "rx://builtin/createSubject", (Expression<Func<ISubjectFactory<T>, string, ISubject<T>>>)((sf, id) => this.CreateSubject<T>(sf, id)) },
                { "rx://builtin/subscribe", (Expression<Func<IObservable<T>, string, IObserver<T>, IDisposable>>)((io, id, iv) => this.Subscribe<T>(io, id, iv)) },
                { "rx://builtin/onNext", (Expression<Action<IObserver<T>, T>>)((iv, value) => iv.OnNext(value)) },
                { "rx://builtin/dispose", (Expression<Action<IDisposable>>)(d => d.Dispose()) },

                // NOTE: Not building out the Define* support for prototyping sake + not adding automatic unquoting but using QueryLanguage forwarders instead
                { "select", (Expression<Func<IObservable<T>, Expression<Func<T, R>>, IObservable<R>>>)((xs, f) => xs.Select(f.Compile())) },
                { "where", (Expression<Func<IObservable<T>, Expression<Func<T, bool>>, IObservable<T>>>)((xs, f) => xs.Where(f.Compile())) },
                { "cout", Expression.Convert(Expression.New(typeof(Cout<T>).GetConstructors()[0]), typeof(IObserver<T>)) },
                { "nop", Expression.Convert(Expression.New(typeof(Nop<T>).GetConstructors()[0]), typeof(IObserver<T>)) },
                { "subject", Expression.Convert(Expression.New(typeof(SubjectFactory<T>).GetConstructors()[0]), typeof(ISubjectFactory<T>)) },
                { "subject/partitioned", Expression.Convert(Expression.New(typeof(PartitionedSubjectFactory<T>).GetConstructors()[0]), typeof(ISubjectFactory<T>)) },
            };
        }

        public void Evaluate(Expression expression)
        {
            var subst = new TypeSubstitutionExpressionVisitor(new Dictionary<Type, Type>
            {
                { typeof(IQubjectFactory<>), typeof(ISubjectFactory<>) },
                { typeof(IQubject<>), typeof(ISubject<>) },
                { typeof(IQbservable<>), typeof(IObservable<>) },
                { typeof(IQbserver<>), typeof(IObserver<>) },
                { typeof(IQubscription), typeof(IDisposable) },
            });

            var expr = subst.Visit(expression);

            var fvs = FreeVariableScanner.Scan(expr);

            var map = new Dictionary<ParameterExpression, Expression>();

            foreach (var fv in fvs)
            {
                if (_registry.TryGetValue(fv.Name, out var res))
                {
                    if (res is Expression e)
                    {
                        var u = e.Type.UnifyExact(fv.Type);
                        var f = new TypeSubstitutionExpressionVisitor(u).Visit(e);
                        map.Add(fv, f);
                        continue;
                    }
                    else
                    {
                        e = Expression.Constant(res, fv.Type);
                        map.Add(fv, e);
                        continue;
                    }
                }

                throw new InvalidOperationException(string.Format("Could not bind '{0}'.", fv.Name));
            }

            var bound = new Binder(map).Bind(expr);

            if (bound.Type == typeof(void))
            {
                // TODO: Evaluate method could be more forgiving for void-returning delegates (no automatic coercion to object)
                bound = Expression.Block(bound, Expression.Default(typeof(object)));
            }

            bound.Evaluate();
        }

        private class Binder
        {
            private readonly IDictionary<ParameterExpression, Expression> _bindings;

            public Binder(IDictionary<ParameterExpression, Expression> bindings)
            {
                _bindings = bindings;
            }

            public Expression Bind(Expression expression)
            {
                return new Impl(this).Visit(expression);
            }

            private class Impl : ScopedExpressionVisitor<ParameterExpression>
            {
                private readonly Binder _parent;

                public Impl(Binder parent)
                {
                    _parent = parent;
                }

                protected override ParameterExpression GetState(ParameterExpression parameter)
                {
                    return parameter;
                }

                private readonly Stack<Expression> _invocations = new();

                protected override Expression VisitInvocation(InvocationExpression node)
                {
                    var isOperator = false;

                    if (node.Expression is ParameterExpression parameter && !base.TryLookup(parameter, out _))
                    {
                        isOperator = true;
                        _invocations.Push(node);
                    }

                    var res = base.VisitInvocation(node);

                    if (isOperator)
                    {
                        var newExpr = _invocations.Pop();
                        if (newExpr != node)
                        {
                            res = newExpr;
                        }
                        else
                        {
                            // TODO: continue delegating if newExpr is an IDelegationTarget;
                            //       will need to put a placeholder in the expression for "self"
                        }
                    }

                    return res;
                }

                protected override Expression VisitParameter(ParameterExpression node)
                {
                    if (!base.TryLookup(node, out _))
                    {
                        var bound = _parent._bindings[node];

                        if (_invocations.Count > 0)
                        {
                            if (bound is ConstantExpression resource)
                            {
                                if (resource.Value is IDelegationTarget del)
                                {
                                    var lastOperator = _invocations.Pop();

                                    var afterDelegation = del.Apply(lastOperator, node);
                                    _invocations.Push(afterDelegation);
                                }
                            }
                        }

                        return bound;
                    }

                    return base.VisitParameter(node);
                }
            }
        }

        public ISubject<T> CreateSubject<T>(ISubjectFactory<T> factory, string id)
        {
            var res = factory.Create();
            _registry.Add(id, res);
            return res;
        }

        public IDisposable Subscribe<T>(IObservable<T> observable, string id, IObserver<T> observer)
        {
            var d = observable.Subscribe(observer);
            _registry.Add(id, d);
            return d;
        }
    }
}
