// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

using Reaqtor;

namespace Reaqtor.IoT
{
    //
    // Base class for client contexts used as the entry-point for the Reactor development surface, completely
    // analogous to LINQ to SQL's DataContext. Derived types can be built to expose the top-level artifacts,
    // e.g. sensor event streams or commonly used observers.
    //
    // As an analogy:
    //
    //   class NorthwindDataContext : DataContext
    //   {
    //       [Table("Products")]
    //       public IQueryable<Product> Products => GetTable<Product>("Products");
    //   }
    //
    // is completely analogous to:
    //
    //   class SensorsReactorContext : ReactorContext
    //   {
    //       [KnownResource("iot://sensors/temperature")]
    //       public IAsyncReactiveQbservable<SensorReading> Temperature => GetObservable<SensorReading>(new Uri("iot://sensors/temperature"));
    //   }
    //
    // It's just richer with more artifact types that just tables. Reactor has observables, observers, streams
    // (equivalent to subjects in Rx, which are both observable and observer), subscriptions, and factory types.
    // The most commonly mapped artifact types are likely observables and observers.
    //

    public class ReactorContext : ReactiveClientContext
    {
        public ReactorContext(QueryEngine engine) : base(new ExpressionServices(), engine.ServiceProvider)
        {
        }

        //
        // Details of expression services are quite deep, but the essence is that these provide a place to
        // normalize and rewrite expression trees composed through the IReactive* interfaces exposed via the
        // ReactorContext parent type. Where a classic LINQ provider would translate expression trees to some
        // query DSL, Reactor normalizes expression trees and serializes them across machine boundaries.
        //
        // Typical steps involved in normalization include rewriting of method call expressions that refer to
        // methods with a KnownResource attribute to an invocation expression using an unbound parameter
        // representing the query operator. E.g.
        //
        //   xs.Where(x => f(x))    // with [KnownResource("rx://operators/filter")] on Where
        //
        // becomes
        //
        //   rx://operators/filter(xs, x => f(x))
        //
        // The target reactive service then binds the "rx://operators/filter" unbound parameter to a definition
        // found in its registry.
        //
        // Other normalizations and rewrites may include type erasure of data model entity types, allow list
        // scanning to reject (or locally evaluate) constructs not supported by the target service, etc.
        //

        private sealed class ExpressionServices : ReactiveExpressionServices
        {
            public ExpressionServices() : base(typeof(IReactiveClientProxy))
            {
            }

            public override Expression Normalize(Expression expression)
            {
                //
                // NB: Typical Reactor clients talk to some service APIs on a front-end service that does
                //     further analysis of the expression tree (after deserialization) prior to sending it
                //     to one or more query evaluator nodes. For example, a query may get split to run across
                //     multiple nodes. Here, we bind directly to the query engine, so a few more steps are
                //     involved to make this happen:
                //
                //     - The engine stores definitions in "tuple normal form" such that all operators can be
                //       defined as unary. E.g. t => t.Item1.Where(t.Item2) rather than (xs, f) => xs.Where(f).
                //     - The client refers to types in the IReactive*Proxy space with async APIs but the
                //       engine has synchronous symmetric APIs in the IReactive* space. We map types across.
                //

                var normalized = base.Normalize(expression);
                var res = Tupletize(normalized);

                var rw = new AsyncToSyncRewriter(
                    new Dictionary<Type, Type>
                    {
                        { typeof(IReactiveClientProxy),     typeof(IReactiveClient)     },
                        { typeof(IReactiveDefinitionProxy), typeof(IReactiveDefinition) },
                        { typeof(IReactiveMetadataProxy),   typeof(IReactiveMetadata)   },
                        { typeof(IReactiveProxy),           typeof(IReactive)           },
                    });

                res = rw.Rewrite(res);

                return res;
            }

            private static Expression Tupletize(Expression expression)
            {
                var inv = new InvocationTupletizer();
                var result = inv.Visit(expression);
                if (result is LambdaExpression lambda)
                {
                    result = ExpressionTupletizer.Pack(lambda);
                }

                return result;
            }

            private sealed class InvocationTupletizer : ScopedExpressionVisitor<ParameterExpression>
            {
                protected override Expression VisitInvocation(InvocationExpression node)
                {
                    var expr = Visit(node.Expression);
                    var args = Visit(node.Arguments);

                    if (expr.NodeType == ExpressionType.Parameter)
                    {
                        var parameter = (ParameterExpression)expr;

                        // Turns f(x, y, z) into f((x, y, z)) when f is an unbound parameter, i.e. representing a known resource.
                        if (IsUnboundParameter(parameter))
                        {
                            if (args.Count > 0)
                            {
                                var tuple = ExpressionTupletizer.Pack(args);
                                var funcType = Expression.GetDelegateType(tuple.Type, node.Type);
                                var function = Expression.Parameter(funcType, parameter.Name);
                                return Expression.Invoke(function, tuple);
                            }
                        }
                    }

                    return node.Update(expr, args);
                }

                protected override ParameterExpression GetState(ParameterExpression parameter) => parameter;

                private bool IsUnboundParameter(ParameterExpression parameter) => !TryLookup(parameter, out _);
            }
        }
    }
}
