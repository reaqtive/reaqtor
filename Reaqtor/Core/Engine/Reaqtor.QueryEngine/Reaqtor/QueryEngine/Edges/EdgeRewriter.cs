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

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Rewrites an expression tree by peeking into a query engine registry for unbound parameters. If an unbound
    /// parameter is found and can be bound, it's left alone. Otherwise, the unbound parameter is replaced by an
    /// expression representing an edge (which will get processed in subsequent steps to resolve to e.g. an input
    /// or output edge, depending on the artifact type). Descriptions of the inserted edges are returned i nthe
    /// form of <see cref="EdgeDescription"/> objects.
    /// </summary>
    /// <example>
    /// Consider a query expression:
    ///
    /// <c>rx://subscribe(rx://filter(io://xs, x => x != 0), iv://out)</c>
    ///
    /// Assume the attempt to bind rx://* and io://xs succeeds, but we fail to bind iv://out. In that case, an
    /// edge will be manufactured (with a random GUID, for simplicity call it edge://foo) to replace iv://out:
    ///
    /// <c>rx://subscribe(rx://filter(io://xs, x => x != 0), edge://foo)</c>
    ///
    /// We refer to edge://foo as the internal URI and iv://out as the external URI. The edge description returned
    /// for this edge rewrite also keeps track of the original expression (in this case just the ParameterExpression
    /// whose Name was iv://out) as well as a manufactured internal subscription ID.
    ///
    /// The idea is that the topology after introducing an edge (in this case it will become an output edge because
    /// we are dealing with an unknown external observer) will look like:
    ///
    /// io://xs -> rx://filter -> edge://foo
    ///
    /// which is represented as an internal subscription (in the traditional ISubscribable/IObserver space without
    /// sequence identifiers on events), and is complemented by an additional subscription in the IReliable* space
    /// (thus with sequence numbers to allow for replay of events upon failover) that crosses the engine boundary
    /// to establish:
    ///
    /// edge://foo => iv://out
    ///
    /// (Note we use -> for local event flow and => to represent cross-engine event flow through a "fat" pipe.)
    ///
    /// The original user subscription now manages the resources for both the local operator graph, the edge subject
    /// that converts between unreliable and reliable spaces, as well as the cross-engine reliable subscription.
    ///
    /// All information to paste all the pieces together is contained within the returned EdgeDescription objects.
    /// </example>
    internal sealed class EdgeRewriter
    {
        private static readonly MethodInfo _edgeCleanupMethod = ((MethodInfo)ReflectionHelpers.InfoOf((ISubscribable<object> x) => SubscribableExtensions.EdgeCleanup(x, null))).GetGenericMethodDefinition();

        private readonly string _uriPrefix;
        private readonly IQueryEngineRegistry _registry;

        public EdgeRewriter(string uriPrefix, IQueryEngineRegistry registry)
        {
            Debug.Assert(uriPrefix != null);
            Debug.Assert(registry != null);

            _uriPrefix = uriPrefix;
            _registry = registry;
        }

        public Expression Rewrite(Expression expr, out IEnumerable<EdgeDescription> edges)
        {
            Debug.Assert(expr != null);

            if (expr.Type != typeof(ISubscription))
            {
                throw new InvalidOperationException("Expected a subscription expression.");
            }

            var visitor = new Visitor(_uriPrefix, _registry);
            Expression result = visitor.Visit(expr);
            edges = visitor.Edges;

            Debug.Assert(result == expr || edges.Any());

            if (result != expr)
            {
                var originalInvocation = (InvocationExpression)result;
                var elementType = originalInvocation.Arguments.First().Type.GetGenericArguments().Single();

                result = Expression.Invoke(
                    originalInvocation.Expression,
                    Expression.Call(_edgeCleanupMethod.MakeGenericMethod(elementType), originalInvocation.Arguments[0], Expression.Constant(edges.ToArray())),
                    originalInvocation.Arguments[1]);
            }
            else
            {
                edges = null;
            }

            return result;
        }

        private sealed class Visitor : ScopedExpressionVisitor<ParameterExpression>
        {
            private readonly string _uriPrefix;
            private readonly QueryEngineBinder _binder;
            private readonly List<EdgeDescription> _edges;

            public Visitor(string uriPrefix, IQueryEngineRegistry registry)
            {
                _uriPrefix = uriPrefix;
                _binder = new FullBinder(registry);
                _edges = new List<EdgeDescription>();
            }

            public IEnumerable<EdgeDescription> Edges => _edges;

            protected sealed override ParameterExpression GetState(ParameterExpression parameter) => parameter;

            protected sealed override Expression VisitInvocation(InvocationExpression node)
            {
                if (node.Expression is ParameterExpression func && IsEdge(func))
                {
                    var edge = CreateEdge(new Uri(func.Name), node);
                    _edges.Add(edge);
                    return Expression.Parameter(node.Type, edge.InternalUri.ToCanonicalString());
                }

                return base.VisitInvocation(node);
            }

            protected sealed override Expression VisitParameter(ParameterExpression node)
            {
                if (IsEdge(node))
                {
                    var edge = CreateEdge(new Uri(node.Name), node);
                    _edges.Add(edge);
                    return Expression.Parameter(node.Type, edge.InternalUri.ToCanonicalString());
                }

                return base.VisitParameter(node);
            }

            private bool IsEdge(ParameterExpression parameter) => IsUnboundParameter(parameter) && _binder.Lookup(parameter) == null;

            private bool IsUnboundParameter(ParameterExpression parameter) => !TryLookup(parameter, out _);

            private EdgeDescription CreateEdge(Uri externalUri, Expression originalExpression)
            {
                return new EdgeDescription
                {
                    Expression = originalExpression,
                    ExternalUri = externalUri,
                    InternalUri = new Uri(_uriPrefix + "/rbs/" + Guid.NewGuid().ToString()),
                    InternalSubscriptionUri = new Uri(_uriPrefix + "/intsub/" + Guid.NewGuid().ToString()),
                };
            }
        }
    }
}
