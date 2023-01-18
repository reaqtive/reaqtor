// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Reaqtor.TestingFramework;

namespace Reaqtor.ReificationFramework
{
    /// <summary>
    /// Helper methods to work with reified operations.
    /// </summary>
    public static class ReifiedOperationExtensions
    {
        /// <summary>
        /// Converts a set of query engine operations to a set of reified operations.
        /// </summary>
        /// <param name="operations">The query engine operations.</param>
        /// <returns>The query engine operations as reified operations.</returns>
        public static IEnumerable<ReifiedOperation> AsReified(this IEnumerable<QueryEngineOperation> operations)
        {
            if (operations == null)
                throw new ArgumentNullException(nameof(operations));

            return operations.Select<QueryEngineOperation, ReifiedOperation>(o => o);
        }

        /// <summary>
        /// Converts a set of service operations to a set of reified operations.
        /// </summary>
        /// <param name="operations">The service operations.</param>
        /// <returns>The service operations as reified operations.</returns>
        public static IEnumerable<ReifiedOperation> AsReified(this IEnumerable<ServiceOperation> operations)
        {
            if (operations == null)
                throw new ArgumentNullException(nameof(operations));

            return operations.Select<ServiceOperation, ReifiedOperation>(o => o);
        }

        /// <summary>
        /// Converts a query engine operation to a reified operation.
        /// </summary>
        /// <param name="operation">The query engine operation.</param>
        /// <returns>The query engine operations as a reified operation.</returns>
        public static ReifiedOperation AsReified(this QueryEngineOperation operation)
        {
            return operation;
        }

        /// <summary>
        /// Converts a query engine operation to a reified operation.
        /// </summary>
        /// <param name="operation">The query engine operation.</param>
        /// <returns>The query engine operations as a reified operation.</returns>
        public static ReifiedOperation AsReified(this ServiceOperation operation)
        {
            return operation;
        }

        /// <summary>
        /// Starts an operation on a thread pool thread.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="onStart">
        /// A callback to return the task after the operation has been started.
        /// </param>
        /// <param name="token">
        /// A cancellation token to give to the task factory.
        /// </param>
        /// <returns>The wrapped operation.</returns>
        public static ReifiedOperation Async(this ReifiedOperation operation, Action<Task> onStart, CancellationToken token)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));
            if (onStart == null)
                throw new ArgumentNullException(nameof(onStart));

            return new Async(operation, onStart, token);
        }

        /// <summary>
        /// Binds a reified operation to a given environment.
        /// </summary>
        /// <typeparam name="TEnvironment">The environment type.</typeparam>
        /// <param name="operation">The operation to bind.</param>
        /// <param name="binder">The binder to use.</param>
        /// <returns>
        /// A lambda expression to evaluate the reified operation.
        /// </returns>
        public static Expression<Action<TEnvironment>> Bind<TEnvironment>(this ReifiedOperation operation, IReificationBinder<TEnvironment> binder)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));
            if (binder == null)
                throw new ArgumentNullException(nameof(binder));

            var flattener = new ReifiedOperationFlattener();
            var opBinder = new ReifiedOperationBinder<TEnvironment>(binder);
            var flattened = flattener.Visit(operation);
            var bound = opBinder.Visit(flattened);
            return binder.Optimize(bound);
        }

        /// <summary>
        /// Catches any exception thrown by the given operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="handler">The callback for any exceptions thrown.</param>
        /// <returns>The wrapped operation.</returns>
        public static ReifiedOperation Catch(this ReifiedOperation operation, Action<Exception> handler)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            return new Catch<Exception>(operation, handler);
        }

        /// <summary>
        /// Catches any exception of the given type thrown by the given operation.
        /// </summary>
        /// <typeparam name="T">The exception type.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <param name="handler">The callback for any exceptions thrown.</param>
        /// <returns>The wrapped operation.</returns>
        public static ReifiedOperation Catch<T>(this ReifiedOperation operation, Action<T> handler)
            where T : Exception
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            return new Catch<T>(operation, handler);
        }

        /// <summary>
        /// Wraps a sequence of operations as a single, ordered operation.
        /// </summary>
        /// <param name="operations">The operations.</param>
        /// <returns>The wrapped operation.</returns>
        public static ReifiedOperation Chain(this IEnumerable<ReifiedOperation> operations)
        {
            if (operations == null)
                throw new ArgumentNullException(nameof(operations));

            return new Chain(operations.First(), operations.Skip(1));
        }

        /// <summary>
        /// Wraps a sequence of operations as a single, ordered operation.
        /// </summary>
        /// <param name="first">The first operation.</param>
        /// <param name="rest">The rest of the operations.</param>
        /// <returns>The wrapped operation.</returns>
        public static ReifiedOperation Chain(this ReifiedOperation first, params ReifiedOperation[] rest)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (rest == null)
                throw new ArgumentNullException(nameof(rest));

            return new Chain(first, rest);
        }

        /// <summary>
        /// Instruments an operation with callbacks that are invoked before and after the operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="onEnter">
        /// Callback to invoke prior to evaluating the operation.
        /// </param>
        /// <param name="onExit">
        /// Callback to invoke after evaluating the operation.
        /// </param>
        /// <returns>The wrapped operation.</returns>
        public static ReifiedOperation Instrument(this ReifiedOperation operation, Action onEnter, Action onExit)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));
            if (onEnter == null)
                throw new ArgumentNullException(nameof(onEnter));
            if (onExit == null)
                throw new ArgumentNullException(nameof(onExit));

            return new Instrument(operation, onEnter, onExit);
        }

        /// <summary>
        /// Remaps all the wildcards in an operation to unique identifiers.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <returns>The wrapped operation.</returns>
        public static ReifiedOperation LiftWildcards(this ReifiedOperation operation)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            return LiftWildcards(operation, WildcardGenerator.Instance);
        }

        /// <summary>
        /// Remaps all the wildcards in an operation to identifiers taken from the wildcard generator.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="generator">The wildcard generator.</param>
        /// <returns>The wrapped operation.</returns>
        public static ReifiedOperation LiftWildcards(this ReifiedOperation operation, IWildcardGenerator generator)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));
            if (generator == null)
                throw new ArgumentNullException(nameof(generator));

            return new LiftWildcards(operation, generator);
        }

        /// <summary>
        /// Maps a URI in a service operation to a new URI.
        /// </summary>
        /// <param name="operation">The service operation.</param>
        /// <param name="original">The original URI.</param>
        /// <param name="replacement">The replacement URI.</param>
        /// <returns>The mapped operation.</returns>
        public static ServiceOperation Map(this ServiceOperation operation, Uri original, Uri replacement)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));
            if (original == null)
                throw new ArgumentNullException(nameof(original));
            if (replacement == null)
                throw new ArgumentNullException(nameof(replacement));

            var rewriter = new ServiceOperationUriRewriter(u => u == original ? replacement : u);
            return rewriter.Visit(operation);
        }

        /// <summary>
        /// Maps a wildcard in a service operation to a new URI.
        /// </summary>
        /// <param name="operation">The service operation.</param>
        /// <param name="uri">The replacement URI.</param>
        /// <returns>The mapped operation.</returns>
        public static ServiceOperation MapWildcard(this ServiceOperation operation, Uri uri)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            var rewriter = new ServiceOperationUriRewriter(u => u.IsWildcard() ? uri : u);
            return rewriter.Visit(operation);
        }

        /// <summary>
        /// Repeat an operation a given number of times.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="count">The number of times to repeat the operation.</param>
        /// <returns>The wrapped operation.</returns>
        public static ReifiedOperation Repeat(this ReifiedOperation operation, long count)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count), "Count must be at least 0.");

            return new Repeat(operation, count);
        }

        /// <summary>
        /// Repeat an operation until cancellation is requested.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The wrapped operation.</returns>
        public static ReifiedOperation RepeatUntil(this ReifiedOperation operation, CancellationToken token)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            return new RepeatUntil(operation, token);
        }

        /// <summary>
        /// Rewrites segments of the service operation expression.
        /// </summary>
        /// <param name="operation">The service operation.</param>
        /// <param name="original">The expression to rewrite.</param>
        /// <param name="replacement">The replacement expression.</param>
        /// <returns>The service operation with the expression rewritten.</returns>
        public static ServiceOperation Rewrite(this ServiceOperation operation, Expression original, Expression replacement)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));
            if (original == null)
                throw new ArgumentNullException(nameof(original));
            if (replacement == null)
                throw new ArgumentNullException(nameof(replacement));

            var rewriter = new ServiceOperationExpressionRewriter(
                new Dictionary<Expression, Expression>(Comparer.Instance) { { original, replacement } });
            return rewriter.Visit(operation);
        }

        private sealed class ReifiedOperationFlattener : ReifiedOperationVisitor
        {
            protected override ReifiedOperation VisitChain(Chain operation)
            {
                var result = base.VisitChain(operation);

                if (result is Chain chain)
                {
                    var operations = default(List<ReifiedOperation>);
                    var count = 0;
                    foreach (var op in chain.Rest)
                    {
                        if (op is Chain link)
                        {
                            if (operations == null)
                            {
                                operations = new List<ReifiedOperation>(chain.Rest.Take(count));
                            }

                            operations.Add(link.Operation);
                            operations.AddRange(link.Rest);
                        }
                        else operations?.Add(op);

                        count++;
                    }

                    if (operations != null)
                    {
                        return new Chain(chain.Operation, operations);
                    }
                }

                return result;
            }
        }

        private sealed class ServiceOperationExpressionRewriter : ServiceOperationVisitor
        {
            private readonly Visitor _visitor;

            public ServiceOperationExpressionRewriter(IDictionary<Expression, Expression> targets)
            {
                _visitor = new Visitor(targets);
            }

            protected override Expression VisitExpression(Expression expression)
            {
                return _visitor.Visit(expression);
            }

            private sealed class Visitor : ExpressionVisitor
            {
                private readonly IDictionary<Expression, Expression> _rewrites;

                public Visitor(IDictionary<Expression, Expression> rewrites)
                {
                    _rewrites = rewrites;
                }

                public override Expression Visit(Expression node)
                {
                    if (_rewrites.TryGetValue(node, out var result))
                    {
                        return result;
                    }

                    return base.Visit(node);
                }
            }
        }

        private sealed class ServiceOperationUriRewriter : ServiceOperationVisitor
        {
            private readonly Func<Uri, Uri> _uriMapping;

            public ServiceOperationUriRewriter(Func<Uri, Uri> uriMapping)
            {
                _uriMapping = uriMapping;
            }

            protected override Uri VisitUri(Uri uri)
            {
                return _uriMapping(uri);
            }

            protected override Expression VisitExpression(Expression expression)
            {
                var freeVariables = FreeVariableScanner.Scan(expression);

                var replacements = new Dictionary<ParameterExpression, ParameterExpression>();
                foreach (var parameter in freeVariables)
                {
                    var uri = new Uri(parameter.Name);
                    var newUri = _uriMapping(uri);
                    if (newUri != uri)
                    {
                        replacements.Add(parameter, Expression.Parameter(parameter.Type, newUri.ToCanonicalString()));
                    }
                }

                var substitutor = new FreeVariableSubstitutor(replacements);
                return substitutor.Visit(expression);
            }

            private sealed class FreeVariableSubstitutor : ScopedExpressionVisitor<ParameterExpression>
            {
                private readonly IDictionary<ParameterExpression, ParameterExpression> _substitutions;

                public FreeVariableSubstitutor(IDictionary<ParameterExpression, ParameterExpression> substitutions)
                {
                    _substitutions = substitutions;
                }

                protected override Expression VisitParameter(ParameterExpression node)
                {
                    if (!TryLookup(node, out _) && _substitutions.TryGetValue(node, out var replacement))
                    {
                        return replacement;
                    }

                    return base.VisitParameter(node);
                }

                protected override ParameterExpression GetState(ParameterExpression parameter)
                {
                    return parameter;
                }
            }
        }

        private sealed class Comparer : ExpressionEqualityComparer
        {
            private Comparer()
                : base(() => new Comparator())
            {
            }

            public static Comparer Instance { get; } = new Comparer();

            private sealed class Comparator : ExpressionEqualityComparator
            {
                private const uint Prime = 0xa5555529; // See CompilationPass.cpp in C# compiler codebase.

                protected override bool EqualsGlobalParameter(ParameterExpression x, ParameterExpression y)
                {
                    return x.Name == y.Name && Equals(x.Type, y.Type);
                }

                protected override int GetHashCodeGlobalParameter(ParameterExpression obj)
                {
#if NET6_0 || NETSTANDARD2_1
                    var hash = obj.Name.GetHashCode(StringComparison.Ordinal);
#else
                    var hash = obj.Name.GetHashCode();
#endif

                    unchecked
                    {
                        return (int)(hash * Prime) + GetHashCode(obj.Type);
                    }
                }
            }
        }
    }
}
