// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - December 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Linq.CompilerServices;
using System.Linq.CompilerServices.TypeSystem;
using System.Linq.Expressions;

using Reaqtive;

using Reaqtor.Remoting.QueryEvaluator;

namespace Reaqtor.Remoting.TestingFramework
{
    public class TestQueryEvaluatorServiceConnection : QueryEvaluatorServiceConnection
    {
        public TestQueryEvaluatorServiceConnection()
        {
            EventTimelines = new();
            TestObservers = new();
            TestSubscriptions = new();
        }

        public TimelineStoreConnection EventTimelines { get; }

        public TestObserverStoreConnection TestObservers { get; }

        public TestSubscriptionStoreConnection TestSubscriptions { get; }

        protected override Func<Expression, Expression> Rewriter => e => base.Rewriter(new CanaryInstrumenter(Constants.Test.AssertStateTransitionCanaryObservable.String).Visit(e));

        protected override Dictionary<string, object> OperatorContextElements
        {
            get
            {
                var baseElements = base.OperatorContextElements;
                baseElements.Add(TimelineStoreConnection.ContextHandle, EventTimelines);
                baseElements.Add(TestObserverStoreConnection.ContextHandle, TestObservers);
                baseElements.Add(TestSubscriptionStoreConnection.ContextHandle, TestSubscriptions);
                return baseElements;
            }
        }

        protected override void DefineBuiltinOperators()
        {
            base.DefineBuiltinOperators();

            TryDefineObservable<Tuple<IReactiveQbservable<T>>, T>(
                Constants.Test.AssertStateTransitionCanaryObservable.Uri,
                source => new AssertStateTransitionCanary<T>(source.Item1.To<IReactiveQbservable<T>, ISubscribable<T>>()).To<ISubscribable<T>, IReactiveQbservable<T>>());

            TryDefineObservable<Tuple<IReactiveQbservable<T>>, T>(
                Constants.Test.StatefulAugmentationObservable.Uri,
                source => new StatefulAugmentation<T>(source.Item1.To<IReactiveQbservable<T>, ISubscribable<T>>()).To<ISubscribable<T>, IReactiveQbservable<T>>());
        }

        private sealed class CanaryInstrumenter : TypeBasedExpressionRewriter
        {
            private readonly string _observableCanaryId;

            public CanaryInstrumenter(string observableCanaryId)
            {
                _observableCanaryId = observableCanaryId;
                AddDefinition(typeof(IAsyncReactiveQbservable<>), RewriteObservable);
            }

            private Expression RewriteObservable(Expression expr)
            {
                var observableType = expr.Type;

                //
                // We don't have an AssertStateTransitionCanary<T> that's an IGroupedSubscribable<K, T> so let's not bother instrumenting this.
                // If we leave out this exception, the binder will choke when it realizes there's no Key property to bind to.
                //
                if (observableType.FindGenericType(typeof(IAsyncReactiveGroupedQbservable<,>)) != null)
                {
                    return expr;
                }

                return Expression.Invoke
                (
                    Expression.Parameter
                    (
                        typeof(Func<,>).MakeGenericType(observableType, observableType),
                        _observableCanaryId
                    ),
                    expr
                );
            }

            protected override Expression VisitLambdaCore<T>(Expression<T> node)
            {
                return Expression.Lambda<T>(Visit(node.Body), node.Name, node.TailCall, node.Parameters);
            }
        }
    }
}
