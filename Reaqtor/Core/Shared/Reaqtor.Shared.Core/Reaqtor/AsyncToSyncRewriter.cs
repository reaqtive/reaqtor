// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - September 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor
{
    /// <summary>
    /// Provides expression rewriting capabilities to move from an asynchronous representation to a synchronous representation of a reactive expression.
    /// </summary>
    public class AsyncToSyncRewriter
    {
        private static readonly IDictionary<Type, Type> s_builtInReactiveInterfacesTypeMap = new Dictionary<Type, Type>
        {
            { typeof(IAsyncReactiveObservable<>),           typeof(IReactiveObservable<>)           },
            { typeof(IAsyncReactiveGroupedObservable<,>),   typeof(IReactiveGroupedObservable<,>)   },
            { typeof(IAsyncReactiveObserver<>),             typeof(IReactiveObserver<>)             },
            { typeof(IAsyncReactiveSubject<>),              typeof(IReactiveSubject<>)              },
            { typeof(IAsyncReactiveSubject<,>),             typeof(IReactiveSubject<,>)             },
            { typeof(IAsyncReactiveSubjectFactory<,>),      typeof(IReactiveSubjectFactory<,>)      },
            { typeof(IAsyncReactiveSubjectFactory<,,>),     typeof(IReactiveSubjectFactory<,,>)     },
            { typeof(IAsyncReactiveSubscription),           typeof(IReactiveSubscription)           },
            { typeof(IAsyncReactiveSubscriptionFactory<>),  typeof(IReactiveSubscriptionFactory<>)  },
            { typeof(IAsyncReactiveSubscriptionFactory<,>), typeof(IReactiveSubscriptionFactory<,>) },

            { typeof(IAsyncReactiveQbservable<>),           typeof(IReactiveQbservable<>)           },
            { typeof(IAsyncReactiveGroupedQbservable<,>),   typeof(IReactiveGroupedQbservable<,>)   },
            { typeof(IAsyncReactiveQbserver<>),             typeof(IReactiveQbserver<>)             },
            { typeof(IAsyncReactiveQubject),                typeof(IReactiveQubject)                },
            { typeof(IAsyncReactiveQubject<>),              typeof(IReactiveQubject<>)              },
            { typeof(IAsyncReactiveQubject<,>),             typeof(IReactiveQubject<,>)             },
            { typeof(IAsyncReactiveQubjectFactory<,>),      typeof(IReactiveQubjectFactory<,>)      },
            { typeof(IAsyncReactiveQubjectFactory<,,>),     typeof(IReactiveQubjectFactory<,,>)     },
            { typeof(IAsyncReactiveQubscription),           typeof(IReactiveQubscription)           },
            { typeof(IAsyncReactiveQubscriptionFactory<>),  typeof(IReactiveQubscriptionFactory<>)  },
            { typeof(IAsyncReactiveQubscriptionFactory<,>), typeof(IReactiveQubscriptionFactory<,>) },
        };

        private readonly TypeSubst _typeSubst;

        /// <summary>
        /// Creates a new async to sync rewriter using the specified map for reactive interfaces.
        /// </summary>
        /// <param name="reactiveInterfacesTypeMap">Dictionary mapping asynchronous reactive interface types to their synchronous counterparts.</param>
        public AsyncToSyncRewriter(IDictionary<Type, Type> reactiveInterfacesTypeMap)
        {
            var map = new ReadOnlyChainedDictionary<Type, Type>(reactiveInterfacesTypeMap, s_builtInReactiveInterfacesTypeMap);

            _typeSubst = new TypeSubst(map);
        }

        /// <summary>
        /// Rewrites an asynchronous reactive expression to its synchronous equivalent.
        /// </summary>
        /// <param name="expression">Asynchronous reactive expression to rewrite.</param>
        /// <returns>Synchronous equivalent of the specified expression.</returns>
        public Expression Rewrite(Expression expression)
        {
            var re1 = _typeSubst.Visit(expression);
            var re2 = new AsyncToSyncSubscriptionRewriter().Visit(re1);

            return re2;
        }

        private sealed class TypeSubst : TypeSubstitutionExpressionVisitor
        {
            private static readonly Dictionary<Type, HashSet<string>> s_droppable = new Dictionary<Type, HashSet<string>>
            {
                { typeof(IReactiveObserver<>),             new HashSet<string> { "OnNext", "OnError", "OnCompleted" } },
                { typeof(IReactiveObservable<>),           new HashSet<string> { "Subscribe" } },
                { typeof(IReactiveQbservable<>),           new HashSet<string> { "Subscribe" } },
                { typeof(IReactiveSubjectFactory<,>),      new HashSet<string> { "Create" } },
                { typeof(IReactiveQubjectFactory<,>),      new HashSet<string> { "Create" } },
                { typeof(IReactiveSubjectFactory<,,>),     new HashSet<string> { "Create" } },
                { typeof(IReactiveQubjectFactory<,,>),     new HashSet<string> { "Create" } },
                { typeof(IReactiveSubscriptionFactory<>),  new HashSet<string> { "Create" } },
                { typeof(IReactiveQubscriptionFactory<>),  new HashSet<string> { "Create" } },
                { typeof(IReactiveSubscriptionFactory<,>), new HashSet<string> { "Create" } },
                { typeof(IReactiveQubscriptionFactory<,>), new HashSet<string> { "Create" } },
            };

            private static readonly Dictionary<Type, HashSet<string>> s_resolvable = new Dictionary<Type, HashSet<string>>
            {
                { typeof(IAsyncReactiveObserver<>),             new HashSet<string> { "OnNextAsync", "OnErrorAsync", "OnCompletedAsync" } },
                { typeof(IAsyncReactiveObservable<>),           new HashSet<string> { "SubscribeAsync" } },
                { typeof(IAsyncReactiveQbservable<>),           new HashSet<string> { "SubscribeAsync" } },
                { typeof(IAsyncReactiveSubjectFactory<,>),      new HashSet<string> { "CreateAsync" } },
                { typeof(IAsyncReactiveQubjectFactory<,>),      new HashSet<string> { "CreateAsync" } },
                { typeof(IAsyncReactiveSubjectFactory<,,>),     new HashSet<string> { "CreateAsync" } },
                { typeof(IAsyncReactiveQubjectFactory<,,>),     new HashSet<string> { "CreateAsync" } },
                { typeof(IAsyncReactiveSubscriptionFactory<>),  new HashSet<string> { "CreateAsync" } },
                { typeof(IAsyncReactiveQubscriptionFactory<>),  new HashSet<string> { "CreateAsync" } },
                { typeof(IAsyncReactiveSubscriptionFactory<,>), new HashSet<string> { "CreateAsync" } },
                { typeof(IAsyncReactiveQubscriptionFactory<,>), new HashSet<string> { "CreateAsync" } },
            };

            public TypeSubst(IDictionary<Type, Type> map)
                : base(map)
            {
            }

            protected override Type ResolveType(Type originalType)
            {
                if (originalType.IsGenericType && originalType.GetGenericTypeDefinition() == typeof(Func<,>))
                {
                    var originalTypeArgs = originalType.GetGenericArguments();

                    var argType = ResolveType(originalTypeArgs[0]);
                    var returnType = originalTypeArgs[1];

                    var taskType = returnType.FindGenericType(typeof(Task<>));
                    if (taskType != null)
                    {
                        var innerType = taskType.GetGenericArguments()[0];
                        var resolvedType = ResolveType(innerType);
                        if (innerType != resolvedType)
                        {
                            var res = typeof(Func<,>).MakeGenericType(new[] { argType, resolvedType });
                            return res;
                        }
                    }
                }

                return base.ResolveType(originalType);
            }

            protected override Expression MakeMethodCall(Expression instance, MethodInfo method, IEnumerable<Expression> arguments)
            {
                var shouldRewrite = arguments.Last().Type == typeof(CancellationToken);
                if (shouldRewrite)
                {
                    var decl = method.DeclaringType;
                    if (decl.IsGenericType && s_droppable.TryGetValue(decl.GetGenericTypeDefinition(), out var names) && names.Contains(method.Name))
                    {
                        var count = arguments.Count();
                        arguments = arguments.Take(count - 1);
                    }
                }

                return base.MakeMethodCall(instance, method, arguments);
            }

            protected override MethodInfo FailResolveMethod(MethodInfo originalMethod, Type declaringType, Type[] genericArguments, Type[] parameters, Type returnType)
            {
                var shouldRewrite = ResolveType(originalMethod.DeclaringType) != originalMethod.DeclaringType;
                if (shouldRewrite)
                {
                    var decl = originalMethod.DeclaringType;
                    if (decl.IsGenericType && s_resolvable.TryGetValue(decl.GetGenericTypeDefinition(), out var names) && names.Contains(originalMethod.Name))
                    {
                        var newName = originalMethod.Name;
                        newName = newName.Substring(0, newName.LastIndexOf("Async", StringComparison.Ordinal));

                        var newMethod = declaringType.GetMethods().SingleOrDefault(m => m.Name == newName);

                        if (newMethod != default)
                        {
                            return newMethod;
                        }
                    }
                }

                return base.FailResolveMethod(originalMethod, declaringType, genericArguments, parameters, returnType);
            }
        }

        private sealed class AsyncToSyncSubscriptionRewriter : ScopedExpressionVisitor<ParameterExpression>
        {
            protected override ParameterExpression GetState(ParameterExpression parameter)
            {
                if (parameter.Name == Constants.SubscribeUri)
                {
                    // Assuming the expression is tupletized.

                    var type = parameter.Type;
                    if (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(Func<,>))
                    {
                        throw new InvalidOperationException("Expected asynchronous subscribe signature.");
                    }

                    var typeArgs = type.GetGenericArguments();

                    var argType = typeArgs[0];

                    Debug.Assert(argType.IsGenericType && argType.GetGenericTypeDefinition() == typeof(Tuple<,>));

                    var returnType = typeArgs[1];

                    var subscribeSignature = typeof(Func<,>).MakeGenericType(argType, returnType);

                    return Expression.Parameter(subscribeSignature, parameter.Name);
                }

                return parameter;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (!TryLookup(node, out ParameterExpression res))
                {
                    res = GetState(node);
                    GlobalScope.Add(node, res);
                }

                return res;
            }
        }
    }
}
