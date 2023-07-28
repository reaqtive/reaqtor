// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
// Service implementation with CSE capabilities.
//
// BD - September 2014
//

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace Pearls.Reaqtor.CSE
{
    // implementation steps
    // 1. query normalizer
    // 2. carve stateless hot pieces (candidates)
    // 3. registry for hot SEs
    // 4. look for reuse / cost analysis?
    // 5. ref counting of registry entries

    // TODOs
    // 1. query normalization, e.g. for predicates (mostly demonstrated)
    // 2. classification of operators according to shareability (e.g. TakeUntil(DTO) but not Take(TS))

    // thoughts
    // 1. can operators partake in CSE without tight coupling? (e.g. implement their own normalization strategy)

    /// <summary>
    /// Implementation of an event processing service.
    /// </summary>
    internal class Service
    {
        /// <summary>
        /// Counter for subscription activity logging.
        /// </summary>
        private int i;

        /// <summary>
        /// Dictionary of known expressions that are already evaluated and can be accessed through a hot artifact.
        /// </summary>
        private readonly Dictionary<Expression, HotArtifact> _hotArtifacts = new(new ExpressionEqualityComparer()); // TODO: omitted locking strategies

        /// <summary>
        /// Creates a new event processing service.
        /// </summary>
        /// <param name="registry">Registry containing resources that can be bound.</param>
        public Service(Registry registry)
        {
            Registry = registry;
        }

        /// <summary>
        /// Gets the registry containing resources that can be bound.
        /// </summary>
        public Registry Registry { get; }

        /// <summary>
        /// Gets or sets the logger for diagnostic output.
        /// </summary>
        public ILogger Logger
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a new subscription using the specified expression.
        /// </summary>
        /// <param name="expression">Expression representing a subscription.</param>
        /// <param name="share">When set to <c>true</c> (default value), sharing of sub-expressions is attempted. When set to <c>false</c>, the subscription is created as-is.</param>
        /// <returns>Disposable resource used to cancel the subscription.</returns>
        public IDisposable CreateSubscription(Expression expression, bool share = true)
        {
            var n = i++; // TODO: omitted concurrency safety

            var logger = Logger;

            logger?.WriteLine(string.Format(CultureInfo.InvariantCulture, "Creating subscription {0} = {1}.", n, expression));

            var expr = expression;
            var artifacts = Array.Empty<HotArtifact>().AsEnumerable();

            if (share)
            {
                var e = expression;

                e = new QueryOperatorNormalizer().Visit(e);

                var s = new Scanner(this);

                expr = s.Visit(e);

                artifacts = s.Artifacts;

                logger?.WriteLine(string.Format(CultureInfo.InvariantCulture, "Rewritten subscription {0} to {1}.", n, expr));
            }

            var b = (Expression<Func<IDisposable>>)new Binder(Registry).Visit(expr);

            logger?.WriteLine(string.Format(CultureInfo.InvariantCulture, "Bound subscription {0} as {1}.", n, b));

            var d = b.Compile()();

            logger?.WriteLine(string.Format(CultureInfo.InvariantCulture, "Created subscription {0}.", n));

            return new Disposable(this, d, artifacts);
        }

        /// <summary>
        /// Helper method used by tear-off disposable objects to drop references to the specified hot artifacts.
        /// </summary>
        /// <param name="artifacts">Hot artifacts to reduce the refcount for.</param>
        private void Release(IEnumerable<HotArtifact> artifacts)
        {
            // TODO: omitted concurrency safety; implement a proper strategy to support concurrent refcounting
            foreach (var artifact in artifacts)
            {
                if (--artifact.RefCount == 0)
                {
                    var logger = Logger;
                    logger?.WriteLine(string.Format(CultureInfo.InvariantCulture, "Removing CSE node {0}", artifact.Id), ConsoleColor.Red);

                    _hotArtifacts.Remove(artifact.Expression);
                    Registry.Remove(artifact.Id);
                    artifact.Subscription.Dispose();
                }
            }
        }

        // NOTE: Refcounting takes place at the definition (cold) level, rather than the usage (hot) level.
        //       This eliminates concerns around operators that don't exhibit a 0->1->0 usage count of the
        //       resources they refer to. Worst case, resources are kept alive too long. Allocator scouts
        //       with finalizers could be used to reduce this time window.

        /// <summary>
        /// Disposable resource representing a subscription that can be cancelled.
        /// </summary>
        private class Disposable : IDisposable
        {
            /// <summary>
            /// Service owning the subscription resource.
            /// </summary>
            private readonly Service _parent;

            /// <summary>
            /// Hot artifacts used by the subscription.
            /// </summary>
            private readonly IEnumerable<HotArtifact> _artifacts;

            /// <summary>
            /// Underlying disposable resource used to cancel the subscription. This is the Rx subscription.
            /// </summary>
            private IDisposable _disposable;

            /// <summary>
            /// Creates a new disposable resource representing a subscription that can be cancelled.
            /// </summary>
            /// <param name="parent">Service owning the subscription resource.</param>
            /// <param name="disposable">Underlying disposable resource used to cancel the subscription. This is the Rx subscription.</param>
            /// <param name="artifacts">Hot artifacts used by the subscription.</param>
            public Disposable(Service parent, IDisposable disposable, IEnumerable<HotArtifact> artifacts)
            {
                _parent = parent;
                _disposable = disposable;
                _artifacts = artifacts;
            }

            /// <summary>
            /// Disposes the resource. This operation is idempotent.
            /// </summary>
            public void Dispose()
            {
                var d = Interlocked.Exchange(ref _disposable, null);
                if (d != null)
                {
                    d.Dispose();
                    _parent.Release(_artifacts);
                }
            }
        }

        /// <summary>
        /// Scans an expression for reusable resources and rewrites the expression accordingly.
        /// </summary>
        private class Scanner : ExpressionVisitor
        {
            /// <summary>
            /// Service this scanner instance belongs to.
            /// </summary>
            private readonly Service _parent;

            // NOTE: When implemented properly with concurrency in mind, resources may need a transient state of "requested" to prevent
            //       them from being reclaimed in between detecting a new usage and the AddRef operation. Either way, the logic to keep
            //       lifetime management under control needs to be revisited thoroughly.

            /// <summary>
            /// List of hot artifacts that were used in the rewritten expression. Upon disposal of the subscription, these resources need to be released.
            /// </summary>
            private readonly List<HotArtifact> _artifacts = new();

            /// <summary>
            /// Creates a new scanner for expressions.
            /// </summary>
            /// <param name="parent">Service this scanner instance belongs to.</param>
            public Scanner(Service parent)
            {
                _parent = parent;
            }

            /// <summary>
            /// Gets the list of hot artifacts that were used in the rewritten expression. Upon disposal of the subscription, these resources need to be released.
            /// </summary>
            public IEnumerable<HotArtifact> Artifacts => _artifacts;

            /// <summary>
            /// Analyzes invocation expressions for possible reuse using a hot artifact.
            /// </summary>
            /// <param name="node">Expression to analyze.</param>
            /// <returns>Original expression if the node cannot be reused; otherwise, an unbond parameter expression referring to the hot artifact whose results are equivalent to the original expression's evaluation results.</returns>
            protected override Expression VisitInvocation(InvocationExpression node)
            {
                // TODO: check for IO<T>

                if (node.Expression is ParameterExpression) // TODO: omitted parameter unbound check; use ScopedExpressionVisitor
                {
                    var res = base.VisitInvocation(node);

                    if (_parent._hotArtifacts.TryGetValue(res, out var entry))
                    {
                        entry.RefCount++; // TODO: omitted concurrency safety; implement proper strategy for safe concurrent refcounting
                        _artifacts.Add(entry);

                        res = Expression.Invoke(Expression.Parameter(typeof(Func<>).MakeGenericType(node.Type), entry.Id)); // TODO: relax binder

                        var logger = _parent.Logger;
                        logger?.WriteLine(string.Format(CultureInfo.InvariantCulture, "Rewriting {0} to {1}", node, res), ConsoleColor.Green);

                        return res;
                    }
                    else
                    {
                        var sa = new ShareabilityAnalyzer(_parent);
                        sa.Visit(res);

                        // TODO: omitted shareability score; should integrate with a metrics provider, cardinality estimator, histograms, etc.
                        if (sa.CanShare && sa.ShareCount > 0)
                        {
                            var id = "rx://subjects/cse/" + Guid.NewGuid().ToString();

                            var elemType = GetElementType(res.Type);

                            var obsType = typeof(IObservable<>).MakeGenericType(elemType);
                            var obvType = typeof(IObserver<>).MakeGenericType(elemType);

                            var subType = typeof(Subject<>).MakeGenericType(elemType);
                            var sub = Activator.CreateInstance(subType, new[] { id });

                            entry = new HotArtifact();
                            entry.RefCount++;
                            entry.Id = id;
                            entry.Expression = Expression.Lambda(Expression.Constant(sub, obsType)); // TODO: relax binder

                            _parent._hotArtifacts[res] = entry;
                            _parent.Registry[id] = new Entry { IsSubject = true, CanShare = true, Expression = entry.Expression }; // before call to CreateSubscription below
                            _artifacts.Add(entry);

                            var logger = _parent.Logger;
                            logger?.WriteLine(string.Format(CultureInfo.InvariantCulture, "Creating CSE node {0} for {1}", id, res), ConsoleColor.Yellow);

                            var obv = Expression.Constant(sub, obvType);
                            var exp = Expression.Lambda<Func<IDisposable>>(Expression.Call(res, obsType.GetMethod("Subscribe"), obv));
                            var d = _parent.CreateSubscription(exp, share: false); // invoke binder but avoid endless recursion for sharing
                            entry.Subscription = d;

                            return Expression.Invoke(Expression.Parameter(typeof(Func<>).MakeGenericType(obsType), id)); // TODO: relax binder
                        }

                        return res;
                    }
                }
                else
                {
                    return base.VisitInvocation(node);
                }
            }

            /// <summary>
            /// Gets the element type of an observable type.
            /// </summary>
            /// <param name="type">Observable type whose element type to retrieve.</param>
            /// <returns>Element type of the specified observable type.</returns>
            private static Type GetElementType(Type type)
            {
                var ifs = new[] { type }.Concat(type.GetInterfaces()).ToArray();
                var obs = ifs.Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IObservable<>));
                return obs.GetGenericArguments()[0];
            }
        }

        /// <summary>
        /// Analyzes whether an expression is suiteable for sharing its results through a hot artifact.
        /// </summary>
        private class ShareabilityAnalyzer : ExpressionVisitor
        {
            // TODO: omitted purity analysis of sub-expressions; allow list scanner can mitigate this omission partially

            /// <summary>
            /// Service whose registry to use in the analysis.
            /// </summary>
            private readonly Service _parent;

            /// <summary>
            /// Creates a new shareability analyzer for expressions.
            /// </summary>
            /// <param name="parent">Service whose registry to use in the analysis.</param>
            public ShareabilityAnalyzer(Service parent)
            {
                _parent = parent;
            }

            /// <summary>
            /// Gets whether the expression that has been analyzed is suitable for sharing its results through a hot artifact.
            /// </summary>
            public bool CanShare { get; private set; } = true;

            /// <summary>
            /// Number of shareable resources that occur in the expression being analyzed. If this number is zero, sharing has no benefit.
            /// </summary>
            public int ShareCount { get; private set; }

            /// <summary>
            /// Analyzes invocation expressions for the typical pattern of invoking a known resource.
            /// </summary>
            /// <param name="node">Expression to analyze.</param>
            /// <returns>Original expression.</returns>
            protected override Expression VisitInvocation(InvocationExpression node)
            {
                if (node.Expression is ParameterExpression p) // omitted parameter unbound check
                {
                    if (_parent.Registry.TryGetValue(p.Name, out var e))
                    {
                        CanShare &= e.CanShare;

                        if (e.CanShare && !e.IsSubject)
                        {
                            ShareCount++; // number of resources that warrant sharing (without causing re-sharing)
                        }
                    }
                }

                return base.VisitInvocation(node);
            }
        }

        /// <summary>
        /// Representation of a hot artifact.
        /// </summary>
        private class HotArtifact
        {
            /// <summary>
            /// Identifier of the hot artifact.
            /// </summary>
            public string Id;

            /// <summary>
            /// Number of references to the hot artifact, used for refcounting.
            /// </summary>
            public int RefCount;

            /// <summary>
            /// Expression representing the hot artifact.
            /// </summary>
            public Expression Expression;

            /// <summary>
            /// Subscription governing the computation that's fed into the hot artifact.
            /// </summary>
            public IDisposable Subscription;
        }
    }
}
