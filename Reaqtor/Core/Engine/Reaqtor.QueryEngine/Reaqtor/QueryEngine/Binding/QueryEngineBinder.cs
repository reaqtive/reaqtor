// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

using Reaqtive;

using Reaqtor.Expressions;
using Reaqtor.Expressions.Binding;
using Reaqtor.Reactive;
using Reaqtor.Reliable;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Expression binder with support for reactive entity types that are used within a query engine.
    /// </summary>
    /// <remarks>
    /// The basic operating principle of the binder is a visitor that looks for unbound variables (provided through the
    /// <see cref="ExpressionBinder"/> base class), dispatching into specialized functions depending on the type of the
    /// unbound variable (which can also be a delegate type's return type, to allow for parameterized artifacts). In
    /// the derived class, protected virtual `Lookup*` functions are provided for each of the reactive entity types,
    /// allowing derived classes to plug in lookup mechanisms (e.g. consulting a registry). The result of a lookup, if
    /// any (represented as a non-null expression), is then added to a list of bindings. At the end of the binding pass,
    /// the <see cref="ExpressionBinder.Inline"/> method takes care of inlining the bindings in the original expression.
    ///
    /// For example:
    ///
    /// <c>rx://filter(xs, x => x > 0)</c>
    ///
    /// has two unbound variables, `rx://filter` and `xs`. The binder will trigger a lookup for these, based on their
    /// type, dispatching into lookup functions:
    ///
    /// * LookupSubscribable("xs", ...)
    /// * LookupSubscribable("rx://filter", ...)
    ///
    /// where additional information about the type is passed in (including function parameter types when binding a
    /// function). These lookup functions can then return null (don't bind) or an expression (to bind):
    ///
    /// * xs = new Range(0, 10)
    /// * rx://filter = (source, filter) => Subscribable.Where(source, filter)
    ///
    /// The last step then performs inlining, which is the equivalent of beta reduction of:
    ///
    /// <c>((xs, rx://filter) => rx://filter(xs, x => x > 0))(new Range(0, 10), (source, filter) => Subscribable.Where(source, filter))</c>
    ///
    /// which results in:
    ///
    /// <c>Subscribable.Where(new Range(0, 10), x => x > 0)</c>
    ///
    /// Various derived binder types use different lookup policies (e.g. just inlining definitions for a first binding
    /// pass versus inlining all artifact types for final evaluation of expressions).
    /// </remarks>
    internal abstract class QueryEngineBinder : ExpressionBinder
    {
        private static readonly MethodInfo s_reliableObservableToSubscribableMethod = ((MethodInfo)ReflectionHelpers.InfoOf((IReliableObservable<object> x) => x.ToSubscribable())).GetGenericMethodDefinition();
        private static readonly MethodInfo s_reliableSubjectCreateObserverMethod = ((MethodInfo)ReflectionHelpers.InfoOf((IReliableMultiSubject<object, object> x) => x.ToReliableObserver())).GetGenericMethodDefinition();
        private static readonly MethodInfo s_multiSubjectGetObservableMethod = ((MethodInfo)ReflectionHelpers.InfoOf((IMultiSubject s) => s.GetObservable<object>())).GetGenericMethodDefinition();
        private static readonly MethodInfo s_multiSubjectGetObserverMethod = ((MethodInfo)ReflectionHelpers.InfoOf((IMultiSubject s) => s.GetObserver<object>())).GetGenericMethodDefinition();

        private readonly IDictionary<Type, Func<string, Type, Type, Expression>> _lookupFuncs;
        private readonly IDictionary<Type, Func<string, Type, Type, Expression>> _lookupFuncsForParameterized;

        /// <summary>
        /// Creates a binder that consults the specified <paramref name="registry"/> to look up bindings for unbound parameter expressions.
        /// </summary>
        /// <param name="registry">The query engine registry to consult during binding.</param>
        public QueryEngineBinder(IQueryEngineRegistry registry)
        {
            Registry = registry;

            _lookupFuncs = new Dictionary<Type, Func<string, Type, Type, Expression>>()
            {
                { typeof(ISubscribable<>),          OnLookupSubscribable         },
                { typeof(IGroupedSubscribable<,>),  OnLookupSubscribable         },
                { typeof(IObserver<>),              OnLookupObserver             },
                { typeof(IReliableObservable<>),    OnLookupReliableObservable   },
                { typeof(IReliableObserver<>),      OnLookupReliableObserver     },
                { typeof(IReliableMultiSubject<,>), OnLookupReliableMultiSubject },
                { typeof(ISubscription),            OnLookupSubscription         },
                { typeof(IReliableSubscription),    OnLookupReliableSubscription },
            };

            _lookupFuncsForParameterized = new Dictionary<Type, Func<string, Type, Type, Expression>>()
            {
                { typeof(ISubscribable<>),          OnLookupSubscribable                },
                { typeof(IGroupedSubscribable<,>),  OnLookupSubscribable                },
                { typeof(IObserver<>),              OnLookupObserver                    },
                { typeof(IMultiSubject),            OnLookupMultiSubjectFactory         },
                { typeof(IReliableObservable<>),    OnLookupReliableObservable          },
                { typeof(IReliableObserver<>),      OnLookupReliableObserver            },
                { typeof(IReliableMultiSubject<,>), OnLookupReliableMultiSubjectFactory },
                { typeof(ISubscription),            OnLookupSubscriptionFactory         },
                { typeof(IReliableSubscription),    OnLookupReliableSubscriptionFactory },
            };
        }

        /// <summary>
        /// Gets the query engine registry to consult during binding.
        /// </summary>
        protected IQueryEngineRegistry Registry { get; }

        public static Expression ToObserver(string id, Expression expr, Type elementType)
        {
            expr = QuoteSubject(id, expr);

            Type type = GetResultType(expr.Type);

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IObserver<>))
            {
                return expr;
            }

            Type baseType = type.FindGenericType(typeof(IObserver<>));
            if (baseType != null)
            {
                return ConvertTo(expr, baseType);
            }

            // TODO: Instead of wrapping in the adapter observer should we wrap the observable before subscribing to allow the adapter observer to participate in checkpointing?
            baseType = type.FindGenericType(typeof(IReliableMultiSubject<,>));
            if (baseType != null)
            {
                Type[] subjectTypes = baseType.GetGenericArguments();
                Type outputAdapterType = typeof(ObserverToReliableObserver<>).MakeGenericType(subjectTypes[0]);
                MethodInfo createObserverMethod = s_reliableSubjectCreateObserverMethod.MakeGenericMethod(subjectTypes[0], subjectTypes[1]);

                return Expression.Convert(
                            Expression.New(
                                outputAdapterType.GetConstructor(new Type[] { createObserverMethod.ReturnType }),
                                Expression.Call(
                                    createObserverMethod,
                                    expr)),
                            outputAdapterType.FindGenericType(typeof(IObserver<>)));
            }

            //
            // Note, the generic type `IMultiSubject<TInput, TOutput>`
            // does not currently derive from the not generic `IMultiSubject`.
            // If such a derivation is added in the future, this binding step
            // must be re-worked, to prefer the generic binding over the the
            // non-generic binding.
            //
            if (typeof(IMultiSubject).IsAssignableFrom(type))
            {
                return Expression.Call(expr, s_multiSubjectGetObserverMethod.MakeGenericMethod(elementType));
            }

            throw new ArgumentException("Can't convert expression to IObserver<T>.", nameof(expr));
        }

        public static Expression ToReliableObserver(string id, Expression expr)
        {
            expr = QuoteSubject(id, expr);

            Type type = GetResultType(expr.Type);

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IReliableObserver<>))
            {
                return expr;
            }

            Type baseType = type.FindGenericType(typeof(IReliableObserver<>));
            if (baseType != null)
            {
                return ConvertTo(expr, baseType);
            }

            baseType = type.FindGenericType(typeof(IReliableMultiSubject<,>));
            if (baseType != null)
            {
                Type[] subjectTypes = baseType.GetGenericArguments();
                MethodInfo createObserverMethod = s_reliableSubjectCreateObserverMethod.MakeGenericMethod(subjectTypes[0], subjectTypes[1]);

                return Expression.Call(null, createObserverMethod, expr);
            }

            throw new ArgumentException("Can't convert expression to IReliableObserver<T>.", nameof(expr));
        }

        public static Expression ToSubscribable(string id, Expression expr, Type elementType)
        {
            expr = QuoteSubject(id, expr);

            Type type = GetResultType(expr.Type);

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ISubscribable<>))
            {
                return expr;
            }

            Type baseType = type.FindGenericType(typeof(ISubscribable<>));
            if (baseType != null)
            {
                return ConvertTo(expr, baseType);
            }

            baseType = type.FindGenericType(typeof(IReliableObservable<>));
            if (baseType != null)
            {
                return Expression.Call(s_reliableObservableToSubscribableMethod.MakeGenericMethod(baseType.GetGenericArguments().Single()), expr);
            }

            //
            // Note, the generic type `IMultiSubject<TInput, TOutput>`
            // does not currently derive from the not generic `IMultiSubject`.
            // If such a derivation is added in the future, this binding step
            // must be re-worked, to prefer the generic binding over the the
            // non-generic binding.
            //
            if (typeof(IMultiSubject).IsAssignableFrom(type))
            {
                return Expression.Call(expr, s_multiSubjectGetObservableMethod.MakeGenericMethod(elementType));
            }

            throw new ArgumentException("Can't convert expression to ISubscribable<T>.", nameof(expr));
        }

        public static Expression ToReliableObservable(string id, Expression expr)
        {
            expr = QuoteSubject(id, expr);

            Type type = GetResultType(expr.Type);

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IReliableObservable<>))
            {
                return expr;
            }

            Type baseType = type.FindGenericType(typeof(IReliableObservable<>));
            if (baseType != null)
            {
                return ConvertTo(expr, baseType);
            }

            baseType = type.FindGenericType(typeof(ISubscribable<>));
            if (baseType != null)
            {
                // TODO: Is this needed?
            }

            throw new ArgumentException("Can't convert expression to IReliableObservable<T>.", nameof(expr));
        }

        private static Expression ConvertTo(Expression expr, Type type)
        {
            if (expr.NodeType == ExpressionType.Lambda && expr.Type.IsGenericType)
            {
                var lambda = (LambdaExpression)expr;

                Type genericType = expr.Type.GetGenericTypeDefinition();
                if (FuncTypes.Contains(genericType))
                {
                    return Expression.Lambda(Expression.Convert(lambda.Body, type), lambda.Parameters);
                }
            }

            return Expression.Convert(expr, type);
        }

        private static readonly ConstructorInfo s_ctorUri = (ConstructorInfo)ReflectionHelpers.InfoOf(() => new Uri(null));
        private static readonly ConstructorInfo s_ctorMultiSubjectProxy = (ConstructorInfo)ReflectionHelpers.InfoOf(() => new MultiSubjectProxy(null));

        private static Expression QuoteSubject(string id, Expression expr)
        {
            if (expr.NodeType == ExpressionType.Constant)
            {
                if (expr is IExpressible expressible)
                {
                    return expressible.Expression;
                }
                else
                {
                    var reliableMultiSubjectType = expr.Type.FindGenericType(typeof(IReliableMultiSubject<,>));
                    Type multiSubjectType;
                    if (reliableMultiSubjectType != null)
                    {
                        var proxyCtor = typeof(ReliableMultiSubjectProxy<,>).MakeGenericType(reliableMultiSubjectType.GetGenericArguments()).GetConstructor(new[] { typeof(Uri) });
                        return Expression.New(proxyCtor, Expression.New(s_ctorUri, Expression.Constant(id, typeof(string))));
                    }
                    // Note, right now all IMultiSubject<,> are also IReliableMultiSubject<,>
                    // so it is unlikely that this `else if` will be used.
                    else if ((multiSubjectType = expr.Type.FindGenericType(typeof(IMultiSubject<,>))) != null)
                    {
                        var proxyCtor = typeof(MultiSubjectProxy<,>).MakeGenericType(multiSubjectType.GetGenericArguments()).GetConstructor(new[] { typeof(Uri) });
                        return Expression.New(proxyCtor, Expression.New(s_ctorUri, Expression.Constant(id, typeof(string))));
                    }
                    else if (typeof(IMultiSubject).IsAssignableFrom(expr.Type))
                    {
                        return Expression.New(s_ctorMultiSubjectProxy, Expression.New(s_ctorUri, Expression.Constant(id, typeof(string))));
                    }
                }
            }

            return expr;
        }

        protected override Expression Inline(Expression expression, IDictionary<ParameterExpression, Expression> bindings)
        {
            var inlinedTemplates = TemplateInliner.Inline(expression, bindings);
            return base.Inline(inlinedTemplates, bindings);
        }

        protected bool TryGetSubjectFactoryExpression(string key, out Expression expression)
        {
            Debug.Assert(!string.IsNullOrEmpty(key));

            if (Registry.SubjectFactories.TryGetValue(key, out StreamFactoryDefinitionEntity subjectFactory) && subjectFactory.IsInitialized)
            {
                Debug.Assert(key == subjectFactory.Uri.ToCanonicalString());
                expression = subjectFactory.Expression;
                return true;
            }

            expression = null;
            return false;
        }

        protected bool TryGetSubscriptionFactoryExpression(string key, out Expression expression)
        {
            Debug.Assert(!string.IsNullOrEmpty(key));

            if (Registry.SubscriptionFactories.TryGetValue(key, out SubscriptionFactoryDefinitionEntity subscriptionFactory) && subscriptionFactory.IsInitialized)
            {
                Debug.Assert(key == subscriptionFactory.Uri.ToCanonicalString());
                expression = subscriptionFactory.Expression;
                return true;
            }

            expression = null;
            return false;
        }

        protected bool TryGetOtherExpression(string key, out Expression expression, out bool isDefinition)
        {
            if (Registry.Other.TryGetValue(key, out DefinitionEntity entity) && entity.IsInitialized)
            {
                expression = entity.Expression;
                isDefinition = true;
                return true;
            }
            else if (Registry.Templates.TryGetValue(key, out entity) && entity.IsInitialized)
            {
                expression = entity.Expression;
                isDefinition = true;
                return true;
            }

            expression = null;
            isDefinition = false;
            return false;
        }

        protected bool TryGetObserverExpression(string key, out Expression expression, out bool isDefinition)
        {
            Debug.Assert(!string.IsNullOrEmpty(key));

            if (Registry.Observers.TryGetValue(key, out ObserverDefinitionEntity observer) && observer.IsInitialized)
            {
                Debug.Assert(key == observer.Uri.ToCanonicalString());
                expression = observer.Expression;
                isDefinition = !IsSubjectExpression(expression);
                return true;
            }

            expression = null;
            isDefinition = false;
            return false;
        }

        protected bool TryGetObservableExpression(string key, out Expression expression, out bool isDefinition)
        {
            Debug.Assert(!string.IsNullOrEmpty(key));

            if (Registry.Observables.TryGetValue(key, out ObservableDefinitionEntity observable) && observable.IsInitialized)
            {
                Debug.Assert(key == observable.Uri.ToCanonicalString());
                expression = observable.Expression;
                isDefinition = !IsSubjectExpression(expression);
                return true;
            }

            expression = null;
            isDefinition = false;
            return false;
        }

        protected override Func<string, Type, Type, Expression> ResolveLookupFunc(string id, Type type, Type funcType)
        {
            if (id.StartsWith(TemplatizationHelpers.TemplateBase, StringComparison.Ordinal))
            {
                return LookupOther;
            }

            Func<string, Type, Type, Expression> lookupFunc;

            if (type.IsGenericType)
            {
                type = type.GetGenericTypeDefinition();
            }

            if (funcType != null)
            {
                if (_lookupFuncsForParameterized.TryGetValue(type, out lookupFunc))
                {
                    return lookupFunc;
                }

                return LookupOther;
            }

            if (_lookupFuncs.TryGetValue(type, out lookupFunc))
            {
                return lookupFunc;
            }

            return LookupOther;
        }

        protected abstract Expression LookupOther(string id, Type type, Type funcType);
        protected abstract Expression LookupSubscribable(string id, Type elementType);
        protected abstract Expression LookupObserver(string id, Type elementType);
        protected abstract Expression LookupMultiSubjectFactory(string id, params Type[] subjectTypes);
        protected abstract Expression LookupSubscriptionFactory(string id, params Type[] subscriptionTypes);
        protected abstract Expression LookupReliableObservable(string id, Type elementType);
        protected abstract Expression LookupReliableObserver(string id, Type elementType);
        protected abstract Expression LookupReliableMultiSubjectFactory(string id, Type inputType, Type outputType);
        protected abstract Expression LookupReliableMultiSubject(string id, Type inputType, Type outputType);
        protected abstract Expression LookupSubscription(string id);
        protected abstract Expression LookupReliableSubscriptionFactory(string id, params Type[] subscriptionTypes);
        protected abstract Expression LookupReliableSubscription(string id);

        private Expression OnLookupSubscribable(string id, Type type, Type funcType)
        {
            return LookupSubscribable(id, type.FindGenericType(typeof(ISubscribable<>)).GetGenericArguments()[0]);
        }

        private Expression OnLookupObserver(string id, Type type, Type funcType)
        {
            return LookupObserver(id, type.GetGenericArguments().First());
        }

        private Expression OnLookupMultiSubjectFactory(string id, Type type, Type funcType)
        {
            Expression factoryExpr = LookupMultiSubjectFactory(id, type.GetGenericArguments());

            if (factoryExpr == null)
            {
                return null;
            }

            Debug.Assert(funcType != null);

            if (factoryExpr.Type.FindGenericType(typeof(IReactiveSubjectFactory<,>)) == null)
            {
                // TODO: Allows for func definitions of factories. Currently only used for input/output edges. Should we remove this?
                return factoryExpr;
            }

            Expression lambda = CreateSubjectFactoryInvocationLambda(funcType, factoryExpr);

            return lambda;
        }

        private Expression OnLookupSubscriptionFactory(string id, Type type, Type funcType)
        {
            Expression factoryExpr = LookupSubscriptionFactory(id, type.GetGenericArguments());

            if (factoryExpr == null)
            {
                return null;
            }

            Debug.Assert(funcType != null);

            Expression lambda = CreateSubscriptionFactoryInvocationLambda(funcType, factoryExpr);

            return lambda;
        }

        private Expression OnLookupReliableObservable(string id, Type type, Type funcType)
        {
            return LookupReliableObservable(id, type.GetGenericArguments().First());
        }

        private Expression OnLookupReliableObserver(string id, Type type, Type funcType)
        {
            return LookupReliableObserver(id, type.GetGenericArguments().First());
        }

        private Expression OnLookupReliableMultiSubjectFactory(string id, Type type, Type funcType)
        {
            Type[] args = type.GetGenericArguments();
            Debug.Assert(args.Length == 2);

            Expression factoryExpr = LookupReliableMultiSubjectFactory(id, args[0], args[1]);

            if (factoryExpr == null)
            {
                return null;
            }

            Debug.Assert(funcType != null);

            if (factoryExpr.Type.FindGenericType(typeof(IReliableSubjectFactory<,>)) == null)
            {
                // TODO: Allows for func definitions of factories. Currently only used for input/output edges. Should we remove this?
                return factoryExpr;
            }

            Expression lambda = CreateSubjectFactoryInvocationLambda(funcType, factoryExpr);

            return lambda;
        }

        private Expression OnLookupReliableSubscriptionFactory(string id, Type type, Type funcType)
        {
            Expression factoryExpr = LookupReliableSubscriptionFactory(id, type.GetGenericArguments());

            if (factoryExpr == null)
            {
                return null;
            }

            Debug.Assert(funcType != null);

            Expression lambda = CreateSubscriptionFactoryInvocationLambda(funcType, factoryExpr);

            return lambda;
        }

        private Expression OnLookupReliableMultiSubject(string id, Type type, Type funcType)
        {
            Type[] args = type.GetGenericArguments();
            Debug.Assert(args.Length == 2);
            return LookupReliableMultiSubject(id, args[0], args[1]);
        }

        private Expression OnLookupSubscription(string id, Type type, Type funcType)
        {
            return LookupSubscription(id);
        }

        private Expression OnLookupReliableSubscription(string id, Type type, Type funcType)
        {
            return LookupReliableSubscription(id);
        }

        private static Expression CreateSubjectFactoryInvocationLambda(Type funcType, Expression factoryExpr)
        {
            return CreateFactoryInvocationLambda(funcType, factoryExpr);
        }

        private static Expression CreateSubscriptionFactoryInvocationLambda(Type funcType, Expression factoryExpr)
        {
            return CreateFactoryInvocationLambda(funcType, factoryExpr);
        }

        private static Expression CreateFactoryInvocationLambda(Type funcType, Expression factoryExpr)
        {
            Debug.Assert(funcType.IsGenericType);
            Debug.Assert(!funcType.IsGenericTypeDefinition);

            Type[] args = funcType.GetGenericArguments();

            var parameterCount = args.Length - 1;

            if (factoryExpr.NodeType == ExpressionType.Lambda)
            {
                if (parameterCount != ((LambdaExpression)factoryExpr).Parameters.Count)
                {
                    throw new InvalidOperationException("Factory parameter count mismatch.");
                }

                return factoryExpr;
            }

            ParameterExpression[] parameters = args.Take(parameterCount).Select((t, i) => Expression.Parameter(t, "p" + i)).ToArray();

            Expression lambda = Expression.Lambda(
                Expression.Call(
                    factoryExpr,
                    factoryExpr.Type.GetMethod("Create"), // TODO-SUBFACT
                    parameters),
                parameters);

            return lambda;
        }

        private static bool IsSubjectExpression(Expression expression)
        {
            return typeof(IMultiSubject).IsAssignableFrom(expression.Type) ||
                typeof(IReliableMultiSubject).IsAssignableFrom(expression.Type) ||
                expression.Type.FindGenericType(typeof(IMultiSubject<,>)) != null;
        }

        private sealed class TemplateInliner : ExpressionVisitor
        {
            private readonly IDictionary<ParameterExpression, Expression> _bindings;

            private TemplateInliner(IDictionary<ParameterExpression, Expression> bindings)
            {
                _bindings = bindings;
            }

            public static Expression Inline(Expression expression, IDictionary<ParameterExpression, Expression> bindings)
            {
                return new TemplateInliner(bindings).Visit(expression);
            }

            public override Expression Visit(Expression node)
            {
                if (node != null && node.IsTemplatized(out ParameterExpression parameter, out Expression argument))
                {
                    var template = _bindings[parameter];

                    if (argument != null)
                    {
                        var unpackedTemplate = ExpressionTupletizer.Unpack((LambdaExpression)template);
                        var unpackedArgument = ExpressionTupletizer.Unpack(argument);

                        // Note that beta reduction is not required here as the caller to the
                        // `Inline` method will reduce the result of the inlining step.
                        return Expression.Invoke(unpackedTemplate, unpackedArgument);
                    }
                    else
                    {
                        return Expression.Invoke((LambdaExpression)template);
                    }
                }

                return base.Visit(node);
            }
        }
    }
}
