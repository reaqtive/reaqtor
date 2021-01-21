// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq.CompilerServices.TypeSystem;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Reaqtive.Scheduler;

using Reaqtor.Shebang.Extensions;
using Reaqtor.Shebang.Linq;

namespace Reaqtor.Shebang.Service
{
    public static class QueryEngineFactory
    {
        private static PhysicalScheduler s_scheduler;

        internal static PhysicalScheduler Scheduler => s_scheduler ??= PhysicalScheduler.Create();

        public static async Task<QueryEngine> CreateNewAsync(IQueryEngineStateStore store, IReadOnlyDictionary<string, object> context = null)
        {
            var sch = new LogicalScheduler(Scheduler);

            var engine = new QueryEngine(new Uri("reaqtor://engine/" + Guid.NewGuid().ToString("D")), sch, store, context);

            var ctx = engine.GetClient();

            await DeployQueryOperators.DefineAsync(ctx);

            await ctx.DefineObserverAsync(new Uri("reaqtor://shebang/observers/cout"), ctx.Provider.CreateQbserver<T>(Expression.New(typeof(ConsoleObserver<T>))), null, CancellationToken.None);
            await ctx.DefineObservableAsync<TimeSpan, DateTimeOffset>(new Uri("reaqtor://shebang/observables/timer"), t => new TimerObservable(t).AsAsyncQbservable(), null, CancellationToken.None);

            await engine.CheckpointAsync();

            return engine;
        }

        public static async Task<QueryEngine> RecoverAsync(IQueryEngineStateStore store, IReadOnlyDictionary<string, object> context = null)
        {
            var sch = new LogicalScheduler(Scheduler);

            var engine = new QueryEngine(new Uri("reaqtor://engine/" + Guid.NewGuid().ToString("D")), sch, store, context);

            await engine.RecoverAsync();

            return engine;
        }
    }
}
