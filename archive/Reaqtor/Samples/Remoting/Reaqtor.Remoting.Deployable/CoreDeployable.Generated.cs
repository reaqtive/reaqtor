// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.CompilerServices.TypeSystem;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Reaqtor.Expressions.Core;

namespace Reaqtor.Remoting.Deployable
{
	public partial class CoreDeployable
	{
		public static async Task UndefineOperatorsExtension(ReactiveClientContext context)
		{
			#region Aggregates

			#region Average
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Average.NoSelector.Int32), CancellationToken.None)).ConfigureAwait(false);
			await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Average.Selector.Int32), CancellationToken.None)).ConfigureAwait(false);
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Average.NoSelector.Int64), CancellationToken.None)).ConfigureAwait(false);
			await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Average.Selector.Int64), CancellationToken.None)).ConfigureAwait(false);
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Average.NoSelector.Single), CancellationToken.None)).ConfigureAwait(false);
			await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Average.Selector.Single), CancellationToken.None)).ConfigureAwait(false);
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Average.NoSelector.Double), CancellationToken.None)).ConfigureAwait(false);
			await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Average.Selector.Double), CancellationToken.None)).ConfigureAwait(false);
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Average.NoSelector.Decimal), CancellationToken.None)).ConfigureAwait(false);
			await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Average.Selector.Decimal), CancellationToken.None)).ConfigureAwait(false);
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Average.NoSelector.NullableInt32), CancellationToken.None)).ConfigureAwait(false);
			await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Average.Selector.NullableInt32), CancellationToken.None)).ConfigureAwait(false);
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Average.NoSelector.NullableInt64), CancellationToken.None)).ConfigureAwait(false);
			await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Average.Selector.NullableInt64), CancellationToken.None)).ConfigureAwait(false);
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Average.NoSelector.NullableSingle), CancellationToken.None)).ConfigureAwait(false);
			await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Average.Selector.NullableSingle), CancellationToken.None)).ConfigureAwait(false);
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Average.NoSelector.NullableDouble), CancellationToken.None)).ConfigureAwait(false);
			await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Average.Selector.NullableDouble), CancellationToken.None)).ConfigureAwait(false);
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Average.NoSelector.NullableDecimal), CancellationToken.None)).ConfigureAwait(false);
			await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Average.Selector.NullableDecimal), CancellationToken.None)).ConfigureAwait(false);
			#endregion

			#region Min
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Min.NoSelector.Int32), CancellationToken.None)).ConfigureAwait(false);
			await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Min.Selector.Int32), CancellationToken.None)).ConfigureAwait(false);
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Min.NoSelector.Int64), CancellationToken.None)).ConfigureAwait(false);
			await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Min.Selector.Int64), CancellationToken.None)).ConfigureAwait(false);
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Min.NoSelector.Single), CancellationToken.None)).ConfigureAwait(false);
			await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Min.Selector.Single), CancellationToken.None)).ConfigureAwait(false);
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Min.NoSelector.Double), CancellationToken.None)).ConfigureAwait(false);
			await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Min.Selector.Double), CancellationToken.None)).ConfigureAwait(false);
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Min.NoSelector.Decimal), CancellationToken.None)).ConfigureAwait(false);
			await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Min.Selector.Decimal), CancellationToken.None)).ConfigureAwait(false);
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Min.NoSelector.NullableInt32), CancellationToken.None)).ConfigureAwait(false);
			await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Min.Selector.NullableInt32), CancellationToken.None)).ConfigureAwait(false);
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Min.NoSelector.NullableInt64), CancellationToken.None)).ConfigureAwait(false);
			await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Min.Selector.NullableInt64), CancellationToken.None)).ConfigureAwait(false);
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Min.NoSelector.NullableSingle), CancellationToken.None)).ConfigureAwait(false);
			await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Min.Selector.NullableSingle), CancellationToken.None)).ConfigureAwait(false);
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Min.NoSelector.NullableDouble), CancellationToken.None)).ConfigureAwait(false);
			await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Min.Selector.NullableDouble), CancellationToken.None)).ConfigureAwait(false);
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Min.NoSelector.NullableDecimal), CancellationToken.None)).ConfigureAwait(false);
			await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Min.Selector.NullableDecimal), CancellationToken.None)).ConfigureAwait(false);
			#endregion

			#region Max
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Max.NoSelector.Int32), CancellationToken.None)).ConfigureAwait(false);
			await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Max.Selector.Int32), CancellationToken.None)).ConfigureAwait(false);
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Max.NoSelector.Int64), CancellationToken.None)).ConfigureAwait(false);
			await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Max.Selector.Int64), CancellationToken.None)).ConfigureAwait(false);
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Max.NoSelector.Single), CancellationToken.None)).ConfigureAwait(false);
			await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Max.Selector.Single), CancellationToken.None)).ConfigureAwait(false);
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Max.NoSelector.Double), CancellationToken.None)).ConfigureAwait(false);
			await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Max.Selector.Double), CancellationToken.None)).ConfigureAwait(false);
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Max.NoSelector.Decimal), CancellationToken.None)).ConfigureAwait(false);
			await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Max.Selector.Decimal), CancellationToken.None)).ConfigureAwait(false);
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Max.NoSelector.NullableInt32), CancellationToken.None)).ConfigureAwait(false);
			await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Max.Selector.NullableInt32), CancellationToken.None)).ConfigureAwait(false);
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Max.NoSelector.NullableInt64), CancellationToken.None)).ConfigureAwait(false);
			await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Max.Selector.NullableInt64), CancellationToken.None)).ConfigureAwait(false);
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Max.NoSelector.NullableSingle), CancellationToken.None)).ConfigureAwait(false);
			await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Max.Selector.NullableSingle), CancellationToken.None)).ConfigureAwait(false);
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Max.NoSelector.NullableDouble), CancellationToken.None)).ConfigureAwait(false);
			await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Max.Selector.NullableDouble), CancellationToken.None)).ConfigureAwait(false);
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Max.NoSelector.NullableDecimal), CancellationToken.None)).ConfigureAwait(false);
			await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Max.Selector.NullableDecimal), CancellationToken.None)).ConfigureAwait(false);
			#endregion

			#region Sum
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Sum.NoSelector.Int32), CancellationToken.None)).ConfigureAwait(false);
			await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Sum.Selector.Int32), CancellationToken.None)).ConfigureAwait(false);
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Sum.NoSelector.Int64), CancellationToken.None)).ConfigureAwait(false);
			await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Sum.Selector.Int64), CancellationToken.None)).ConfigureAwait(false);
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Sum.NoSelector.Single), CancellationToken.None)).ConfigureAwait(false);
			await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Sum.Selector.Single), CancellationToken.None)).ConfigureAwait(false);
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Sum.NoSelector.Double), CancellationToken.None)).ConfigureAwait(false);
			await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Sum.Selector.Double), CancellationToken.None)).ConfigureAwait(false);
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Sum.NoSelector.Decimal), CancellationToken.None)).ConfigureAwait(false);
			await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Sum.Selector.Decimal), CancellationToken.None)).ConfigureAwait(false);
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Sum.NoSelector.NullableInt32), CancellationToken.None)).ConfigureAwait(false);
			await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Sum.Selector.NullableInt32), CancellationToken.None)).ConfigureAwait(false);
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Sum.NoSelector.NullableInt64), CancellationToken.None)).ConfigureAwait(false);
			await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Sum.Selector.NullableInt64), CancellationToken.None)).ConfigureAwait(false);
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Sum.NoSelector.NullableSingle), CancellationToken.None)).ConfigureAwait(false);
			await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Sum.Selector.NullableSingle), CancellationToken.None)).ConfigureAwait(false);
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Sum.NoSelector.NullableDouble), CancellationToken.None)).ConfigureAwait(false);
			await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Sum.Selector.NullableDouble), CancellationToken.None)).ConfigureAwait(false);
            await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Sum.NoSelector.NullableDecimal), CancellationToken.None)).ConfigureAwait(false);
			await TryUndefine(() => context.UndefineObservableAsync(new Uri(Client.Constants.Observable.Sum.Selector.NullableDecimal), CancellationToken.None)).ConfigureAwait(false);
			#endregion

			#endregion
		}

		public static async Task DefineOperatorsExtension(ReactiveClientContext context)
		{
			#region Aggregates

			#region Average
			await context.DefineObservableAsync<IAsyncReactiveQbservable<Int32>, Double>(new Uri(Client.Constants.Observable.Average.NoSelector.Int32), source => source.AsSync().Average().AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, Int32>>, Double>(new Uri(Client.Constants.Observable.Average.Selector.Int32), (source, selector) => source.AsSync().Average(selector).AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<Int64>, Double>(new Uri(Client.Constants.Observable.Average.NoSelector.Int64), source => source.AsSync().Average().AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, Int64>>, Double>(new Uri(Client.Constants.Observable.Average.Selector.Int64), (source, selector) => source.AsSync().Average(selector).AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<Single>, Single>(new Uri(Client.Constants.Observable.Average.NoSelector.Single), source => source.AsSync().Average().AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, Single>>, Single>(new Uri(Client.Constants.Observable.Average.Selector.Single), (source, selector) => source.AsSync().Average(selector).AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<Double>, Double>(new Uri(Client.Constants.Observable.Average.NoSelector.Double), source => source.AsSync().Average().AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, Double>>, Double>(new Uri(Client.Constants.Observable.Average.Selector.Double), (source, selector) => source.AsSync().Average(selector).AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<Decimal>, Decimal>(new Uri(Client.Constants.Observable.Average.NoSelector.Decimal), source => source.AsSync().Average().AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, Decimal>>, Decimal>(new Uri(Client.Constants.Observable.Average.Selector.Decimal), (source, selector) => source.AsSync().Average(selector).AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<Int32?>, Double?>(new Uri(Client.Constants.Observable.Average.NoSelector.NullableInt32), source => source.AsSync().Average().AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, Int32?>>, Double?>(new Uri(Client.Constants.Observable.Average.Selector.NullableInt32), (source, selector) => source.AsSync().Average(selector).AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<Int64?>, Double?>(new Uri(Client.Constants.Observable.Average.NoSelector.NullableInt64), source => source.AsSync().Average().AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, Int64?>>, Double?>(new Uri(Client.Constants.Observable.Average.Selector.NullableInt64), (source, selector) => source.AsSync().Average(selector).AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<Single?>, Single?>(new Uri(Client.Constants.Observable.Average.NoSelector.NullableSingle), source => source.AsSync().Average().AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, Single?>>, Single?>(new Uri(Client.Constants.Observable.Average.Selector.NullableSingle), (source, selector) => source.AsSync().Average(selector).AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<Double?>, Double?>(new Uri(Client.Constants.Observable.Average.NoSelector.NullableDouble), source => source.AsSync().Average().AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, Double?>>, Double?>(new Uri(Client.Constants.Observable.Average.Selector.NullableDouble), (source, selector) => source.AsSync().Average(selector).AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<Decimal?>, Decimal?>(new Uri(Client.Constants.Observable.Average.NoSelector.NullableDecimal), source => source.AsSync().Average().AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, Decimal?>>, Decimal?>(new Uri(Client.Constants.Observable.Average.Selector.NullableDecimal), (source, selector) => source.AsSync().Average(selector).AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			#endregion

			#region Min
			await context.DefineObservableAsync<IAsyncReactiveQbservable<Int32>, Int32>(new Uri(Client.Constants.Observable.Min.NoSelector.Int32), source => source.AsSync().Min().AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, Int32>>, Int32>(new Uri(Client.Constants.Observable.Min.Selector.Int32), (source, selector) => source.AsSync().Min(selector).AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<Int64>, Int64>(new Uri(Client.Constants.Observable.Min.NoSelector.Int64), source => source.AsSync().Min().AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, Int64>>, Int64>(new Uri(Client.Constants.Observable.Min.Selector.Int64), (source, selector) => source.AsSync().Min(selector).AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<Single>, Single>(new Uri(Client.Constants.Observable.Min.NoSelector.Single), source => source.AsSync().Min().AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, Single>>, Single>(new Uri(Client.Constants.Observable.Min.Selector.Single), (source, selector) => source.AsSync().Min(selector).AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<Double>, Double>(new Uri(Client.Constants.Observable.Min.NoSelector.Double), source => source.AsSync().Min().AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, Double>>, Double>(new Uri(Client.Constants.Observable.Min.Selector.Double), (source, selector) => source.AsSync().Min(selector).AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<Decimal>, Decimal>(new Uri(Client.Constants.Observable.Min.NoSelector.Decimal), source => source.AsSync().Min().AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, Decimal>>, Decimal>(new Uri(Client.Constants.Observable.Min.Selector.Decimal), (source, selector) => source.AsSync().Min(selector).AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<Int32?>, Int32?>(new Uri(Client.Constants.Observable.Min.NoSelector.NullableInt32), source => source.AsSync().Min().AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, Int32?>>, Int32?>(new Uri(Client.Constants.Observable.Min.Selector.NullableInt32), (source, selector) => source.AsSync().Min(selector).AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<Int64?>, Int64?>(new Uri(Client.Constants.Observable.Min.NoSelector.NullableInt64), source => source.AsSync().Min().AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, Int64?>>, Int64?>(new Uri(Client.Constants.Observable.Min.Selector.NullableInt64), (source, selector) => source.AsSync().Min(selector).AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<Single?>, Single?>(new Uri(Client.Constants.Observable.Min.NoSelector.NullableSingle), source => source.AsSync().Min().AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, Single?>>, Single?>(new Uri(Client.Constants.Observable.Min.Selector.NullableSingle), (source, selector) => source.AsSync().Min(selector).AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<Double?>, Double?>(new Uri(Client.Constants.Observable.Min.NoSelector.NullableDouble), source => source.AsSync().Min().AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, Double?>>, Double?>(new Uri(Client.Constants.Observable.Min.Selector.NullableDouble), (source, selector) => source.AsSync().Min(selector).AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<Decimal?>, Decimal?>(new Uri(Client.Constants.Observable.Min.NoSelector.NullableDecimal), source => source.AsSync().Min().AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, Decimal?>>, Decimal?>(new Uri(Client.Constants.Observable.Min.Selector.NullableDecimal), (source, selector) => source.AsSync().Min(selector).AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			#endregion

			#region Max
			await context.DefineObservableAsync<IAsyncReactiveQbservable<Int32>, Int32>(new Uri(Client.Constants.Observable.Max.NoSelector.Int32), source => source.AsSync().Max().AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, Int32>>, Int32>(new Uri(Client.Constants.Observable.Max.Selector.Int32), (source, selector) => source.AsSync().Max(selector).AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<Int64>, Int64>(new Uri(Client.Constants.Observable.Max.NoSelector.Int64), source => source.AsSync().Max().AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, Int64>>, Int64>(new Uri(Client.Constants.Observable.Max.Selector.Int64), (source, selector) => source.AsSync().Max(selector).AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<Single>, Single>(new Uri(Client.Constants.Observable.Max.NoSelector.Single), source => source.AsSync().Max().AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, Single>>, Single>(new Uri(Client.Constants.Observable.Max.Selector.Single), (source, selector) => source.AsSync().Max(selector).AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<Double>, Double>(new Uri(Client.Constants.Observable.Max.NoSelector.Double), source => source.AsSync().Max().AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, Double>>, Double>(new Uri(Client.Constants.Observable.Max.Selector.Double), (source, selector) => source.AsSync().Max(selector).AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<Decimal>, Decimal>(new Uri(Client.Constants.Observable.Max.NoSelector.Decimal), source => source.AsSync().Max().AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, Decimal>>, Decimal>(new Uri(Client.Constants.Observable.Max.Selector.Decimal), (source, selector) => source.AsSync().Max(selector).AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<Int32?>, Int32?>(new Uri(Client.Constants.Observable.Max.NoSelector.NullableInt32), source => source.AsSync().Max().AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, Int32?>>, Int32?>(new Uri(Client.Constants.Observable.Max.Selector.NullableInt32), (source, selector) => source.AsSync().Max(selector).AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<Int64?>, Int64?>(new Uri(Client.Constants.Observable.Max.NoSelector.NullableInt64), source => source.AsSync().Max().AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, Int64?>>, Int64?>(new Uri(Client.Constants.Observable.Max.Selector.NullableInt64), (source, selector) => source.AsSync().Max(selector).AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<Single?>, Single?>(new Uri(Client.Constants.Observable.Max.NoSelector.NullableSingle), source => source.AsSync().Max().AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, Single?>>, Single?>(new Uri(Client.Constants.Observable.Max.Selector.NullableSingle), (source, selector) => source.AsSync().Max(selector).AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<Double?>, Double?>(new Uri(Client.Constants.Observable.Max.NoSelector.NullableDouble), source => source.AsSync().Max().AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, Double?>>, Double?>(new Uri(Client.Constants.Observable.Max.Selector.NullableDouble), (source, selector) => source.AsSync().Max(selector).AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<Decimal?>, Decimal?>(new Uri(Client.Constants.Observable.Max.NoSelector.NullableDecimal), source => source.AsSync().Max().AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, Decimal?>>, Decimal?>(new Uri(Client.Constants.Observable.Max.Selector.NullableDecimal), (source, selector) => source.AsSync().Max(selector).AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			#endregion

			#region Sum
			await context.DefineObservableAsync<IAsyncReactiveQbservable<Int32>, Int32>(new Uri(Client.Constants.Observable.Sum.NoSelector.Int32), source => source.AsSync().Sum().AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, Int32>>, Int32>(new Uri(Client.Constants.Observable.Sum.Selector.Int32), (source, selector) => source.AsSync().Sum(selector).AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<Int64>, Int64>(new Uri(Client.Constants.Observable.Sum.NoSelector.Int64), source => source.AsSync().Sum().AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, Int64>>, Int64>(new Uri(Client.Constants.Observable.Sum.Selector.Int64), (source, selector) => source.AsSync().Sum(selector).AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<Single>, Single>(new Uri(Client.Constants.Observable.Sum.NoSelector.Single), source => source.AsSync().Sum().AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, Single>>, Single>(new Uri(Client.Constants.Observable.Sum.Selector.Single), (source, selector) => source.AsSync().Sum(selector).AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<Double>, Double>(new Uri(Client.Constants.Observable.Sum.NoSelector.Double), source => source.AsSync().Sum().AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, Double>>, Double>(new Uri(Client.Constants.Observable.Sum.Selector.Double), (source, selector) => source.AsSync().Sum(selector).AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<Decimal>, Decimal>(new Uri(Client.Constants.Observable.Sum.NoSelector.Decimal), source => source.AsSync().Sum().AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, Decimal>>, Decimal>(new Uri(Client.Constants.Observable.Sum.Selector.Decimal), (source, selector) => source.AsSync().Sum(selector).AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<Int32?>, Int32?>(new Uri(Client.Constants.Observable.Sum.NoSelector.NullableInt32), source => source.AsSync().Sum().AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, Int32?>>, Int32?>(new Uri(Client.Constants.Observable.Sum.Selector.NullableInt32), (source, selector) => source.AsSync().Sum(selector).AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<Int64?>, Int64?>(new Uri(Client.Constants.Observable.Sum.NoSelector.NullableInt64), source => source.AsSync().Sum().AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, Int64?>>, Int64?>(new Uri(Client.Constants.Observable.Sum.Selector.NullableInt64), (source, selector) => source.AsSync().Sum(selector).AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<Single?>, Single?>(new Uri(Client.Constants.Observable.Sum.NoSelector.NullableSingle), source => source.AsSync().Sum().AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, Single?>>, Single?>(new Uri(Client.Constants.Observable.Sum.Selector.NullableSingle), (source, selector) => source.AsSync().Sum(selector).AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<Double?>, Double?>(new Uri(Client.Constants.Observable.Sum.NoSelector.NullableDouble), source => source.AsSync().Sum().AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, Double?>>, Double?>(new Uri(Client.Constants.Observable.Sum.Selector.NullableDouble), (source, selector) => source.AsSync().Sum(selector).AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<Decimal?>, Decimal?>(new Uri(Client.Constants.Observable.Sum.NoSelector.NullableDecimal), source => source.AsSync().Sum().AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			await context.DefineObservableAsync<IAsyncReactiveQbservable<T>, Expression<Func<T, Decimal?>>, Decimal?>(new Uri(Client.Constants.Observable.Sum.Selector.NullableDecimal), (source, selector) => source.AsSync().Sum(selector).AsAsync(), null, CancellationToken.None).ConfigureAwait(false);
			#endregion

			#endregion
		}
	}
}
