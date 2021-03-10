// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.CompilerServices.TypeSystem;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Reaqtive;
using Reaqtive.Scheduler;

using Reaqtor.Shebang.Extensions;
using Reaqtor.Shebang.Linq;

namespace Reaqtor.Shebang.Service
{
    public static class QueryEngineFactory
    {
        private static PhysicalScheduler s_scheduler;

        internal static PhysicalScheduler Scheduler => s_scheduler ??= PhysicalScheduler.Create();

        public static async Task<SimplerCheckpointingQueryEngine> CreateNewAsync(IQueryEngineStateStore store, IReadOnlyDictionary<string, object> context = null, IIngressEgressManager ingressEgressManager = null, TraceSource traceSource = null)
        {
#pragma warning disable CA2000 // Dispose objects before losing scope. (Engine takes ownership.)
            var sch = new LogicalScheduler(Scheduler);
#pragma warning restore CA2000

            var engine = new SimplerCheckpointingQueryEngine(new Uri("reaqtor://engine/" + Guid.NewGuid().ToString("D")), sch, store, context, ingressEgressManager, traceSource);

            var ctx = engine.Client;

            await DeployQueryOperators.DefineAsync(ctx).ConfigureAwait(false);

            await ctx.DefineObserverAsync(new Uri("reaqtor://shebang/observers/cout"), ctx.Provider.CreateQbserver<T>(Expression.New(typeof(ConsoleObserver<T>))), null, CancellationToken.None).ConfigureAwait(false);
            await ctx.DefineObservableAsync<TimeSpan, DateTimeOffset>(new Uri("reaqtor://shebang/observables/timer"), t => new TimerObservable(t).AsAsyncQbservable(), null, CancellationToken.None).ConfigureAwait(false);

            await ctx.DefineObserverAsync<string, T>(new Uri("reaqtor://shebang/observers/egress"), stream => new EgressObserver<T>(stream).AsAsyncQbserver(), null, CancellationToken.None).ConfigureAwait(false);
            await ctx.DefineObservableAsync<string, T>(new Uri("reaqtor://shebang/observables/ingress"), stream => new IngressObservable<T>(stream).AsAsyncQbservable(), null, CancellationToken.None).ConfigureAwait(false);

            await ctx.DefineObservableAsync<IAsyncReactiveQbservable<object>, T>(new Uri("rx://observable/oftype"), source => source.Where(o => o is T).Select(o => (T)o), null, CancellationToken.None).ConfigureAwait(false);
            await ctx.DefineObservableAsync<IAsyncReactiveQbservable<object>, T>(new Uri("rx://observable/cast"), source => source.Select(o => (T)o), null, CancellationToken.None).ConfigureAwait(false);

            await ctx.DefineObserverAsync<T>(new Uri("reaqtor://shebang/observers/nop"), () => NopObserver<T>.Instance.AsAsyncQbserver(), null, CancellationToken.None).ConfigureAwait(false);

            await engine.CheckpointAsync().ConfigureAwait(false);

            return engine;
        }

        public static async Task<SimplerCheckpointingQueryEngine> RecoverAsync(IQueryEngineStateStore store, IReadOnlyDictionary<string, object> context = null, IIngressEgressManager ingressEgressManager = null, TraceSource traceSource = null)
        {
#pragma warning disable CA2000 // Dispose objects before losing scope. (Engine takes ownership.)
            var sch = new LogicalScheduler(Scheduler);
#pragma warning restore CA2000

            var engine = new SimplerCheckpointingQueryEngine(new Uri("reaqtor://engine/" + Guid.NewGuid().ToString("D")), sch, store, context, ingressEgressManager, traceSource);

            await engine.RecoverAsync().ConfigureAwait(false);

            return engine;
        }
    }
}
