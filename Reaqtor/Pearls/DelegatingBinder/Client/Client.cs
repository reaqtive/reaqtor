// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Reflection;

namespace DelegatingBinder
{
    internal class Client(IService service)
    {
        private readonly IQProvider _provider = new Provider(service);

        public IQbservable<T> GetObservable<T>(string id)
        {
            return new ObservableProxy<T>(Expression.Parameter(typeof(IQbservable<T>), id), _provider);
        }

        public IQbserver<T> GetObserver<T>(string id)
        {
            return new ObserverProxy<T>(Expression.Parameter(typeof(IQbserver<T>), id), _provider);
        }

        public IQubscription GetSubscription(string id)
        {
            return new SubscriptionProxy(Expression.Parameter(typeof(IQubscription), id), _provider);
        }

        public IQubjectFactory<T> GetSubjectFactory<T>(string id)
        {
            return new SubjectFactoryProxy<T>(Expression.Parameter(typeof(IQubjectFactory<T>), id), _provider);
        }

        private class ObservableProxy<T>(Expression expression, IQProvider provider) : IQbservable<T>
        {
            public Expression Expression { get; } = expression;

            public IQProvider Provider { get; } = provider;

            public IQubscription Subscribe(string id, IQbserver<T> observer)
            {
                var expression = Expression.Invoke(Expression.Parameter(typeof(Func<IQbservable<T>, string, IQbserver<T>, IQubscription>), "rx://builtin/subscribe"), Expression, Expression.Constant(id, typeof(string)), observer.Expression);
                var normalized = ExpressionService.Normalize(expression);
                Provider.Service.Evaluate(normalized);
                return Provider.CreateSubscription(Expression.Parameter(typeof(IQubscription), id));
            }
        }

        private class ObserverProxy<T>(Expression expression, IQProvider provider) : IQbserver<T>
        {
            public Expression Expression { get; } = expression;

            public IQProvider Provider { get; } = provider;

            public void OnNext(T value)
            {
                var expression = Expression.Invoke(Expression.Parameter(typeof(Action<IQbserver<T>, T>), "rx://builtin/onNext"), Expression, Expression.Constant(value, typeof(T)));
                var normalized = ExpressionService.Normalize(expression);
                Provider.Service.Evaluate(normalized);
            }
        }

        private class SubscriptionProxy(Expression expression, IQProvider provider) : IQubscription
        {
            public Expression Expression { get; } = expression;

            public IQProvider Provider { get; } = provider;

            public void Dispose()
            {
                var expression = Expression.Invoke(Expression.Parameter(typeof(Action<IQubscription>), "rx://builtin/dispose"), Expression);
                var normalized = ExpressionService.Normalize(expression);
                Provider.Service.Evaluate(normalized);
            }
        }

        private class SubjectFactoryProxy<T>(Expression expression, IQProvider provider) : IQubjectFactory<T>
        {
            public Expression Expression { get; } = expression;

            public IQProvider Provider { get; } = provider;

            public IQubject<T> Create(string id)
            {
                var expression = Expression.Invoke(Expression.Parameter(typeof(Func<IQubjectFactory<T>, string, IQubject<T>>), "rx://builtin/createSubject"), Expression, Expression.Constant(id, typeof(string)));
                var normalized = ExpressionService.Normalize(expression);
                Provider.Service.Evaluate(normalized);
                return new SubjectProxy<T>(Expression.Parameter(typeof(IQubject<T>), id), Provider);
            }
        }

        private class SubjectProxy<T>(Expression expression, IQProvider provider) : IQubject<T>
        {
            public Expression Expression { get; } = expression;

            public IQProvider Provider { get; } = provider;

            public void Dispose()
            {
                throw new NotImplementedException();
            }

            public IQubscription Subscribe(string id, IQbserver<T> observer)
            {
                throw new NotImplementedException();
            }

            public void OnNext(T value)
            {
                throw new NotImplementedException();
            }
        }

        private class Provider(IService service) : IQProvider
        {
            public IService Service { get; } = service;

            public IQbservable<T> CreateObservable<T>(Expression expression)
            {
                return new ObservableProxy<T>(expression, this);
            }

            public IQbserver<T> CreateObserver<T>(Expression expression)
            {
                return new ObserverProxy<T>(expression, this);
            }

            public IQubscription CreateSubscription(Expression expression)
            {
                return new SubscriptionProxy(expression, this);
            }

            public IQubjectFactory<T> CreateQubjectFactory<T>(Expression expression)
            {
                return new SubjectFactoryProxy<T>(expression, this);
            }
        }

        private class ExpressionService
        {
            public static Expression Normalize(Expression expression)
            {
                var rw = new ResourceRewriter().Visit(expression);
                var ce = new ClosureEliminator().Visit(rw);
                return ce;
            }

            private class ResourceRewriter : ExpressionVisitor
            {
                protected override Expression VisitMethodCall(MethodCallExpression node)
                {
                    var res = node.Method.GetCustomAttribute<ResourceAttribute>();
                    if (res != null)
                    {
                        if (!node.Method.IsStatic || node.Method.ReturnType == typeof(void)) // NOTE: just for prototyping; IRP deals with all cases
                        {
                            throw new NotSupportedException();
                        }

                        var args = Visit(node.Arguments);
                        var func = Expression.GetFuncType([.. node.Method.GetParameters().Select(p => p.ParameterType), .. new[] { node.Method.ReturnType }]); // NOTE: breaks down on > 16 args; IRP deals with this with tuples
                        return Expression.Invoke(Expression.Parameter(func, res.Id), args);
                    }

                    return base.VisitMethodCall(node);
                }
            }

            private class ClosureEliminator : ExpressionVisitor
            {
                protected override Expression VisitMember(MemberExpression node)
                {
                    if (node.Member.DeclaringType.IsClosureClass())
                    {
                        return Expression.Constant(node.Evaluate(), node.Type);
                    }

                    return base.VisitMember(node);
                }
            }
        }
    }
}
