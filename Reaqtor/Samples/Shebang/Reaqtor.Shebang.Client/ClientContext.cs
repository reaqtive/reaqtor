// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

namespace Reaqtor.Shebang.Client
{
    //
    // Base class for client contexts used as the entry-point for the Reaqtor development surface, completely
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
    //   class SensorsContext : ClientContext
    //   {
    //       [KnownResource("iot://sensors/temperature")]
    //       public IAsyncReactiveQbservable<SensorReading> Temperature => GetObservable<SensorReading>(new Uri("iot://sensors/temperature"));
    //   }
    //
    // It's just richer with more artifact types that just tables. Reaqtor has observables, observers, streams
    // (equivalent to subjects in Rx, which are both observable and observer), subscriptions, and factory types.
    // The most commonly mapped artifact types are likely observables and observers.
    //

    public partial class ClientContext : ReactiveClientContext
    {
        public ClientContext(IReactiveServiceProvider provider) : base(new ExpressionServices(), provider)
        {
        }

        // NB: SimpleTimer shows how to build a custom "source" observable. It's named SimpleTimer to avoid conflict with the built-in Timer.

        [KnownResource("reaqtor://shebang/observables/timer")]
        public IAsyncReactiveQbservable<DateTimeOffset> SimpleTimer(TimeSpan period) => GetObservable<TimeSpan, DateTimeOffset>(new Uri("reaqtor://shebang/observables/timer"))(period);

        [KnownResource("reaqtor://shebang/observables/ingress")]
        public IAsyncReactiveQbservable<T> GetIngress<T>(string name) => GetObservable<string, T>(new Uri("reaqtor://shebang/observables/ingress"))(name);

        [KnownResource("reaqtor://shebang/observers/egress")]
        public IAsyncReactiveQbserver<T> GetEgress<T>(string name) => GetObserver<string, T>(new Uri("reaqtor://shebang/observers/egress"))(name);

        [KnownResource("reaqtor://shebang/observers/cout")]
        public IAsyncReactiveQbserver<string> ConsoleOut => GetObserver<string>(new Uri("reaqtor://shebang/observers/cout"));

        [KnownResource("reaqtor://shebang/observers/nop")]
        public IAsyncReactiveQbserver<T> Nop<T>() => GetObserver<T>(new Uri("reaqtor://shebang/observers/nop"));

        //
        // Details of expression services are quite deep, but the essence is that these provide a place to
        // normalize and rewrite expression trees composed through the IReactive* interfaces exposed via the
        // ClientContext parent type. Where a classic LINQ provider would translate expression trees to some
        // query DSL, Reaqtor normalizes expression trees and serializes them across machine boundaries.
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
                // NB: Typical Reaqtor clients talk to some service APIs on a front-end service that does
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

                var erased = AnonymousTypeTupletizer.Tupletize(normalized, Expression.Constant(null), excludeVisibleTypes: true);

                return erased;
            }
        }
    }
}
