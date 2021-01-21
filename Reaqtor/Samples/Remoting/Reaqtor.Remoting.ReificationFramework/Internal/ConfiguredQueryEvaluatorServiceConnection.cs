// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

using Newtonsoft.Json.Linq;

using Reaqtive.Scheduler;

using Reaqtor.QueryEngine;
using Reaqtor.Remoting.QueryEvaluator;

namespace Reaqtor.Remoting.ReificationFramework
{
    internal sealed class ConfiguredQueryEvaluatorServiceConnection : QueryEvaluatorServiceConnection
    {
        protected override CheckpointingQueryEngine CreateQueryEngine(System.Uri uri, IReactiveServiceResolver resolver, IScheduler scheduler, IReactiveMetadata metadata, IKeyValueStore keyValueStore, TraceSource traceSource, IReadOnlyDictionary<string, object> contextElements)
        {
            var engine = base.CreateQueryEngine(uri, resolver, scheduler, metadata, keyValueStore, traceSource, contextElements);

#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
#pragma warning disable CA1305 // Specify IFormatProvider (captured in expression tree)
            var constantHoister = ConstantHoister.Create(
                true,
                (Expression<Func<string, string>>)(c => string.Format(c, default(object))),
                (Expression<Func<string, string>>)(c => string.Format(c, default(object), default(object))),
                (Expression<Func<string, string>>)(c => string.Format(c, default(object), default(object), default(object))),
                (Expression<Func<string, string>>)(c => string.Format(c, default(object[]))),
                (Expression<Func<string, string>>)(c => string.Format(default(IFormatProvider), c, default(object[]))),
                (Expression<Func<string, JProperty>>)(c => new JProperty(c, default(object))),
                (Expression<Func<string, JProperty>>)(c => new JProperty(c, default(object[]))));
#pragma warning restore CA1305 // Specify IFormatProvider
#pragma warning restore IDE0034 // Simplify 'default' expression

            engine.Options.ExpressionPolicy.DelegateCache = new SimpleCompiledDelegateCache();
            engine.Options.ExpressionPolicy.InMemoryCache = new QueryEvaluatorExpressionHeap(constantHoister);
            engine.Options.ExpressionPolicy.ConstantHoister = constantHoister;

            return engine;
        }

        private sealed class QueryEvaluatorExpressionHeap : ExpressionHeap
        {
            public QueryEvaluatorExpressionHeap(IConstantHoister hoister)
                : base(hoister)
            {
            }

            protected override bool ShouldShareGlobal(ParameterExpression node)
            {
                return node.Name != null
                    && (node.Name.StartsWith("rx://operator", StringComparison.Ordinal)
                    || node.Name.StartsWith("rx://observer", StringComparison.Ordinal)
                    || node.Name.StartsWith("rx://subject", StringComparison.Ordinal)
                    || node.Name.StartsWith("rx://observable", StringComparison.Ordinal)
                    || node.Name.StartsWith("rx://builtin", StringComparison.Ordinal));
            }
        }
    }
}
