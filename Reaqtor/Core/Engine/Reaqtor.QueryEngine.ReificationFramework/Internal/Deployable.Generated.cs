// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

#if FALSE // NB: Disabled until reification framework host is ported and artifacts are refactored out of remoting into separate libraries.

using System;
using System.Linq.CompilerServices.TypeSystem;
using System.Linq.Expressions;

using Reaqtor.Expressions.Core;

using ReactiveConstants = Reaqtor.Remoting.Client.Constants;

namespace Reaqtor.QueryEngine.ReificationFramework
{
	static partial class Deployable
	{
		private static void DefineOperatorsExtension(ReactiveServiceContext context)
		{
			#region Aggregates

			#region Average
			context.DefineObservable<IReactiveQbservable<Int32>, Double>(new Uri(ReactiveConstants.Observable.Average.NoSelector.Int32), source => source.Average(), null);
			context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, Int32>>, Double>(new Uri(ReactiveConstants.Observable.Average.Selector.Int32), (source, selector) => source.Average(selector), null);
			context.DefineObservable<IReactiveQbservable<Int64>, Double>(new Uri(ReactiveConstants.Observable.Average.NoSelector.Int64), source => source.Average(), null);
			context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, Int64>>, Double>(new Uri(ReactiveConstants.Observable.Average.Selector.Int64), (source, selector) => source.Average(selector), null);
			context.DefineObservable<IReactiveQbservable<Single>, Single>(new Uri(ReactiveConstants.Observable.Average.NoSelector.Single), source => source.Average(), null);
			context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, Single>>, Single>(new Uri(ReactiveConstants.Observable.Average.Selector.Single), (source, selector) => source.Average(selector), null);
			context.DefineObservable<IReactiveQbservable<Double>, Double>(new Uri(ReactiveConstants.Observable.Average.NoSelector.Double), source => source.Average(), null);
			context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, Double>>, Double>(new Uri(ReactiveConstants.Observable.Average.Selector.Double), (source, selector) => source.Average(selector), null);
			context.DefineObservable<IReactiveQbservable<Decimal>, Decimal>(new Uri(ReactiveConstants.Observable.Average.NoSelector.Decimal), source => source.Average(), null);
			context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, Decimal>>, Decimal>(new Uri(ReactiveConstants.Observable.Average.Selector.Decimal), (source, selector) => source.Average(selector), null);
			context.DefineObservable<IReactiveQbservable<Int32?>, Double?>(new Uri(ReactiveConstants.Observable.Average.NoSelector.NullableInt32), source => source.Average(), null);
			context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, Int32?>>, Double?>(new Uri(ReactiveConstants.Observable.Average.Selector.NullableInt32), (source, selector) => source.Average(selector), null);
			context.DefineObservable<IReactiveQbservable<Int64?>, Double?>(new Uri(ReactiveConstants.Observable.Average.NoSelector.NullableInt64), source => source.Average(), null);
			context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, Int64?>>, Double?>(new Uri(ReactiveConstants.Observable.Average.Selector.NullableInt64), (source, selector) => source.Average(selector), null);
			context.DefineObservable<IReactiveQbservable<Single?>, Single?>(new Uri(ReactiveConstants.Observable.Average.NoSelector.NullableSingle), source => source.Average(), null);
			context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, Single?>>, Single?>(new Uri(ReactiveConstants.Observable.Average.Selector.NullableSingle), (source, selector) => source.Average(selector), null);
			context.DefineObservable<IReactiveQbservable<Double?>, Double?>(new Uri(ReactiveConstants.Observable.Average.NoSelector.NullableDouble), source => source.Average(), null);
			context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, Double?>>, Double?>(new Uri(ReactiveConstants.Observable.Average.Selector.NullableDouble), (source, selector) => source.Average(selector), null);
			context.DefineObservable<IReactiveQbservable<Decimal?>, Decimal?>(new Uri(ReactiveConstants.Observable.Average.NoSelector.NullableDecimal), source => source.Average(), null);
			context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, Decimal?>>, Decimal?>(new Uri(ReactiveConstants.Observable.Average.Selector.NullableDecimal), (source, selector) => source.Average(selector), null);
			#endregion

			#region Min
			context.DefineObservable<IReactiveQbservable<Int32>, Int32>(new Uri(ReactiveConstants.Observable.Min.NoSelector.Int32), source => source.Min(), null);
			context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, Int32>>, Int32>(new Uri(ReactiveConstants.Observable.Min.Selector.Int32), (source, selector) => source.Min(selector), null);
			context.DefineObservable<IReactiveQbservable<Int64>, Int64>(new Uri(ReactiveConstants.Observable.Min.NoSelector.Int64), source => source.Min(), null);
			context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, Int64>>, Int64>(new Uri(ReactiveConstants.Observable.Min.Selector.Int64), (source, selector) => source.Min(selector), null);
			context.DefineObservable<IReactiveQbservable<Single>, Single>(new Uri(ReactiveConstants.Observable.Min.NoSelector.Single), source => source.Min(), null);
			context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, Single>>, Single>(new Uri(ReactiveConstants.Observable.Min.Selector.Single), (source, selector) => source.Min(selector), null);
			context.DefineObservable<IReactiveQbservable<Double>, Double>(new Uri(ReactiveConstants.Observable.Min.NoSelector.Double), source => source.Min(), null);
			context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, Double>>, Double>(new Uri(ReactiveConstants.Observable.Min.Selector.Double), (source, selector) => source.Min(selector), null);
			context.DefineObservable<IReactiveQbservable<Decimal>, Decimal>(new Uri(ReactiveConstants.Observable.Min.NoSelector.Decimal), source => source.Min(), null);
			context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, Decimal>>, Decimal>(new Uri(ReactiveConstants.Observable.Min.Selector.Decimal), (source, selector) => source.Min(selector), null);
			context.DefineObservable<IReactiveQbservable<Int32?>, Int32?>(new Uri(ReactiveConstants.Observable.Min.NoSelector.NullableInt32), source => source.Min(), null);
			context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, Int32?>>, Int32?>(new Uri(ReactiveConstants.Observable.Min.Selector.NullableInt32), (source, selector) => source.Min(selector), null);
			context.DefineObservable<IReactiveQbservable<Int64?>, Int64?>(new Uri(ReactiveConstants.Observable.Min.NoSelector.NullableInt64), source => source.Min(), null);
			context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, Int64?>>, Int64?>(new Uri(ReactiveConstants.Observable.Min.Selector.NullableInt64), (source, selector) => source.Min(selector), null);
			context.DefineObservable<IReactiveQbservable<Single?>, Single?>(new Uri(ReactiveConstants.Observable.Min.NoSelector.NullableSingle), source => source.Min(), null);
			context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, Single?>>, Single?>(new Uri(ReactiveConstants.Observable.Min.Selector.NullableSingle), (source, selector) => source.Min(selector), null);
			context.DefineObservable<IReactiveQbservable<Double?>, Double?>(new Uri(ReactiveConstants.Observable.Min.NoSelector.NullableDouble), source => source.Min(), null);
			context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, Double?>>, Double?>(new Uri(ReactiveConstants.Observable.Min.Selector.NullableDouble), (source, selector) => source.Min(selector), null);
			context.DefineObservable<IReactiveQbservable<Decimal?>, Decimal?>(new Uri(ReactiveConstants.Observable.Min.NoSelector.NullableDecimal), source => source.Min(), null);
			context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, Decimal?>>, Decimal?>(new Uri(ReactiveConstants.Observable.Min.Selector.NullableDecimal), (source, selector) => source.Min(selector), null);
			#endregion

			#region Max
			context.DefineObservable<IReactiveQbservable<Int32>, Int32>(new Uri(ReactiveConstants.Observable.Max.NoSelector.Int32), source => source.Max(), null);
			context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, Int32>>, Int32>(new Uri(ReactiveConstants.Observable.Max.Selector.Int32), (source, selector) => source.Max(selector), null);
			context.DefineObservable<IReactiveQbservable<Int64>, Int64>(new Uri(ReactiveConstants.Observable.Max.NoSelector.Int64), source => source.Max(), null);
			context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, Int64>>, Int64>(new Uri(ReactiveConstants.Observable.Max.Selector.Int64), (source, selector) => source.Max(selector), null);
			context.DefineObservable<IReactiveQbservable<Single>, Single>(new Uri(ReactiveConstants.Observable.Max.NoSelector.Single), source => source.Max(), null);
			context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, Single>>, Single>(new Uri(ReactiveConstants.Observable.Max.Selector.Single), (source, selector) => source.Max(selector), null);
			context.DefineObservable<IReactiveQbservable<Double>, Double>(new Uri(ReactiveConstants.Observable.Max.NoSelector.Double), source => source.Max(), null);
			context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, Double>>, Double>(new Uri(ReactiveConstants.Observable.Max.Selector.Double), (source, selector) => source.Max(selector), null);
			context.DefineObservable<IReactiveQbservable<Decimal>, Decimal>(new Uri(ReactiveConstants.Observable.Max.NoSelector.Decimal), source => source.Max(), null);
			context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, Decimal>>, Decimal>(new Uri(ReactiveConstants.Observable.Max.Selector.Decimal), (source, selector) => source.Max(selector), null);
			context.DefineObservable<IReactiveQbservable<Int32?>, Int32?>(new Uri(ReactiveConstants.Observable.Max.NoSelector.NullableInt32), source => source.Max(), null);
			context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, Int32?>>, Int32?>(new Uri(ReactiveConstants.Observable.Max.Selector.NullableInt32), (source, selector) => source.Max(selector), null);
			context.DefineObservable<IReactiveQbservable<Int64?>, Int64?>(new Uri(ReactiveConstants.Observable.Max.NoSelector.NullableInt64), source => source.Max(), null);
			context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, Int64?>>, Int64?>(new Uri(ReactiveConstants.Observable.Max.Selector.NullableInt64), (source, selector) => source.Max(selector), null);
			context.DefineObservable<IReactiveQbservable<Single?>, Single?>(new Uri(ReactiveConstants.Observable.Max.NoSelector.NullableSingle), source => source.Max(), null);
			context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, Single?>>, Single?>(new Uri(ReactiveConstants.Observable.Max.Selector.NullableSingle), (source, selector) => source.Max(selector), null);
			context.DefineObservable<IReactiveQbservable<Double?>, Double?>(new Uri(ReactiveConstants.Observable.Max.NoSelector.NullableDouble), source => source.Max(), null);
			context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, Double?>>, Double?>(new Uri(ReactiveConstants.Observable.Max.Selector.NullableDouble), (source, selector) => source.Max(selector), null);
			context.DefineObservable<IReactiveQbservable<Decimal?>, Decimal?>(new Uri(ReactiveConstants.Observable.Max.NoSelector.NullableDecimal), source => source.Max(), null);
			context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, Decimal?>>, Decimal?>(new Uri(ReactiveConstants.Observable.Max.Selector.NullableDecimal), (source, selector) => source.Max(selector), null);
			#endregion

			#region Sum
			context.DefineObservable<IReactiveQbservable<Int32>, Int32>(new Uri(ReactiveConstants.Observable.Sum.NoSelector.Int32), source => source.Sum(), null);
			context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, Int32>>, Int32>(new Uri(ReactiveConstants.Observable.Sum.Selector.Int32), (source, selector) => source.Sum(selector), null);
			context.DefineObservable<IReactiveQbservable<Int64>, Int64>(new Uri(ReactiveConstants.Observable.Sum.NoSelector.Int64), source => source.Sum(), null);
			context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, Int64>>, Int64>(new Uri(ReactiveConstants.Observable.Sum.Selector.Int64), (source, selector) => source.Sum(selector), null);
			context.DefineObservable<IReactiveQbservable<Single>, Single>(new Uri(ReactiveConstants.Observable.Sum.NoSelector.Single), source => source.Sum(), null);
			context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, Single>>, Single>(new Uri(ReactiveConstants.Observable.Sum.Selector.Single), (source, selector) => source.Sum(selector), null);
			context.DefineObservable<IReactiveQbservable<Double>, Double>(new Uri(ReactiveConstants.Observable.Sum.NoSelector.Double), source => source.Sum(), null);
			context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, Double>>, Double>(new Uri(ReactiveConstants.Observable.Sum.Selector.Double), (source, selector) => source.Sum(selector), null);
			context.DefineObservable<IReactiveQbservable<Decimal>, Decimal>(new Uri(ReactiveConstants.Observable.Sum.NoSelector.Decimal), source => source.Sum(), null);
			context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, Decimal>>, Decimal>(new Uri(ReactiveConstants.Observable.Sum.Selector.Decimal), (source, selector) => source.Sum(selector), null);
			context.DefineObservable<IReactiveQbservable<Int32?>, Int32?>(new Uri(ReactiveConstants.Observable.Sum.NoSelector.NullableInt32), source => source.Sum(), null);
			context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, Int32?>>, Int32?>(new Uri(ReactiveConstants.Observable.Sum.Selector.NullableInt32), (source, selector) => source.Sum(selector), null);
			context.DefineObservable<IReactiveQbservable<Int64?>, Int64?>(new Uri(ReactiveConstants.Observable.Sum.NoSelector.NullableInt64), source => source.Sum(), null);
			context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, Int64?>>, Int64?>(new Uri(ReactiveConstants.Observable.Sum.Selector.NullableInt64), (source, selector) => source.Sum(selector), null);
			context.DefineObservable<IReactiveQbservable<Single?>, Single?>(new Uri(ReactiveConstants.Observable.Sum.NoSelector.NullableSingle), source => source.Sum(), null);
			context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, Single?>>, Single?>(new Uri(ReactiveConstants.Observable.Sum.Selector.NullableSingle), (source, selector) => source.Sum(selector), null);
			context.DefineObservable<IReactiveQbservable<Double?>, Double?>(new Uri(ReactiveConstants.Observable.Sum.NoSelector.NullableDouble), source => source.Sum(), null);
			context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, Double?>>, Double?>(new Uri(ReactiveConstants.Observable.Sum.Selector.NullableDouble), (source, selector) => source.Sum(selector), null);
			context.DefineObservable<IReactiveQbservable<Decimal?>, Decimal?>(new Uri(ReactiveConstants.Observable.Sum.NoSelector.NullableDecimal), source => source.Sum(), null);
			context.DefineObservable<IReactiveQbservable<T>, Expression<Func<T, Decimal?>>, Decimal?>(new Uri(ReactiveConstants.Observable.Sum.Selector.NullableDecimal), (source, selector) => source.Sum(selector), null);
			#endregion

			#endregion
		}
	}
}

#endif
