// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtive;
using Reaqtive.Testing;
using Reaqtive.TestingFramework;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Reaqtive.Operators
{
	public partial class Average
	{
		[TestMethod]
		public void AverageInt32_ArgumentChecking()
		{
			ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Int32>)null).Average());
		}

		[TestMethod]
		public void AverageInt32_Selector_ArgumentChecking()
		{
			ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Tuple<Int32>>)null).Average(t => t.Item1));
			ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<Tuple<Int32>>.Instance.Average(default(Func<Tuple<Int32>, Int32>)));
		}

		[TestMethod]
		public void AverageInt32_SaveAndReload()
		{
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(225, state),
                OnLoad(235, state),
            };

			var xs = Scheduler.CreateHotObservable(
				OnNext<Int32>(210, 17),
				OnNext<Int32>(220, -8),
				OnNext<Int32>(230, 25), // Missing
				OnNext<Int32>(240, 2),
				OnNext<Int32>(250, 3),
				OnNext<Int32>(260, -5),
				OnNext<Int32>(270, -7),
				OnNext<Int32>(280, 36),
				OnCompleted<Int32>(290)
			);

			var res = Scheduler.Start(() =>
				xs.Average().Apply(Scheduler, checkpoints)
			);

			res.Messages.AssertEqual(
				// state saved @225
                // state reloaded @235
				OnNext<Double>(290, avg => Math.Abs((Double)(avg - 5.42857142857143d)) < 0.0001d),
				OnCompleted<Double>(290)
			);
		}

		[TestMethod]
		public void AverageNullableInt32_ArgumentChecking()
		{
			ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Int32?>)null).Average());
		}

		[TestMethod]
		public void AverageNullableInt32_Selector_ArgumentChecking()
		{
			ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Tuple<Int32?>>)null).Average(t => t.Item1));
			ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<Tuple<Int32?>>.Instance.Average(default(Func<Tuple<Int32?>, Int32?>)));
		}

		[TestMethod]
		public void AverageNullableInt32_SaveAndReload()
		{
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(225, state),
                OnLoad(235, state),
            };

			var xs = Scheduler.CreateHotObservable(
				OnNext<Int32?>(210, 17),
				OnNext<Int32?>(220, -8),
				OnNext<Int32?>(230, 25), // Missing
				OnNext<Int32?>(240, 2),
				OnNext<Int32?>(250, 3),
				OnNext<Int32?>(260, -5),
				OnNext<Int32?>(270, -7),
				OnNext<Int32?>(280, 36),
				OnCompleted<Int32?>(290)
			);

			var res = Scheduler.Start(() =>
				xs.Average().Apply(Scheduler, checkpoints)
			);

			res.Messages.AssertEqual(
				// state saved @225
                // state reloaded @235
				OnNext<Double?>(290, avg => Math.Abs((Double)(avg - 5.42857142857143d)) < 0.0001d),
				OnCompleted<Double?>(290)
			);
		}

		[TestMethod]
		public void AverageInt64_ArgumentChecking()
		{
			ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Int64>)null).Average());
		}

		[TestMethod]
		public void AverageInt64_Selector_ArgumentChecking()
		{
			ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Tuple<Int64>>)null).Average(t => t.Item1));
			ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<Tuple<Int64>>.Instance.Average(default(Func<Tuple<Int64>, Int64>)));
		}

		[TestMethod]
		public void AverageInt64_SaveAndReload()
		{
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(225, state),
                OnLoad(235, state),
            };

			var xs = Scheduler.CreateHotObservable(
				OnNext<Int64>(210, 17L),
				OnNext<Int64>(220, -8L),
				OnNext<Int64>(230, 25L), // Missing
				OnNext<Int64>(240, 2L),
				OnNext<Int64>(250, 3L),
				OnNext<Int64>(260, -5L),
				OnNext<Int64>(270, -7L),
				OnNext<Int64>(280, 36L),
				OnCompleted<Int64>(290)
			);

			var res = Scheduler.Start(() =>
				xs.Average().Apply(Scheduler, checkpoints)
			);

			res.Messages.AssertEqual(
				// state saved @225
                // state reloaded @235
				OnNext<Double>(290, avg => Math.Abs((Double)(avg - 5.42857142857143d)) < 0.0001d),
				OnCompleted<Double>(290)
			);
		}

		[TestMethod]
		public void AverageNullableInt64_ArgumentChecking()
		{
			ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Int64?>)null).Average());
		}

		[TestMethod]
		public void AverageNullableInt64_Selector_ArgumentChecking()
		{
			ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Tuple<Int64?>>)null).Average(t => t.Item1));
			ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<Tuple<Int64?>>.Instance.Average(default(Func<Tuple<Int64?>, Int64?>)));
		}

		[TestMethod]
		public void AverageNullableInt64_SaveAndReload()
		{
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(225, state),
                OnLoad(235, state),
            };

			var xs = Scheduler.CreateHotObservable(
				OnNext<Int64?>(210, 17L),
				OnNext<Int64?>(220, -8L),
				OnNext<Int64?>(230, 25L), // Missing
				OnNext<Int64?>(240, 2L),
				OnNext<Int64?>(250, 3L),
				OnNext<Int64?>(260, -5L),
				OnNext<Int64?>(270, -7L),
				OnNext<Int64?>(280, 36L),
				OnCompleted<Int64?>(290)
			);

			var res = Scheduler.Start(() =>
				xs.Average().Apply(Scheduler, checkpoints)
			);

			res.Messages.AssertEqual(
				// state saved @225
                // state reloaded @235
				OnNext<Double?>(290, avg => Math.Abs((Double)(avg - 5.42857142857143d)) < 0.0001d),
				OnCompleted<Double?>(290)
			);
		}

		[TestMethod]
		public void AverageSingle_ArgumentChecking()
		{
			ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Single>)null).Average());
		}

		[TestMethod]
		public void AverageSingle_Selector_ArgumentChecking()
		{
			ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Tuple<Single>>)null).Average(t => t.Item1));
			ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<Tuple<Single>>.Instance.Average(default(Func<Tuple<Single>, Single>)));
		}

		[TestMethod]
		public void AverageSingle_SaveAndReload()
		{
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(225, state),
                OnLoad(235, state),
            };

			var xs = Scheduler.CreateHotObservable(
				OnNext<Single>(210, 17.8f),
				OnNext<Single>(220, -25.2f),
				OnNext<Single>(230, 3.5f), // Missing
				OnNext<Single>(240, -7.36f),
				OnCompleted<Single>(250)
			);

			var res = Scheduler.Start(() =>
				xs.Average().Apply(Scheduler, checkpoints)
			);

			res.Messages.AssertEqual(
				// state saved @225
                // state reloaded @235
				OnNext<Single>(250, avg => Math.Abs((Single)(avg - -4.920001f)) < 0.0001f),
				OnCompleted<Single>(250)
			);
		}

		[TestMethod]
		public void AverageNullableSingle_ArgumentChecking()
		{
			ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Single?>)null).Average());
		}

		[TestMethod]
		public void AverageNullableSingle_Selector_ArgumentChecking()
		{
			ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Tuple<Single?>>)null).Average(t => t.Item1));
			ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<Tuple<Single?>>.Instance.Average(default(Func<Tuple<Single?>, Single?>)));
		}

		[TestMethod]
		public void AverageNullableSingle_SaveAndReload()
		{
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(225, state),
                OnLoad(235, state),
            };

			var xs = Scheduler.CreateHotObservable(
				OnNext<Single?>(210, 17.8f),
				OnNext<Single?>(220, -25.2f),
				OnNext<Single?>(230, 3.5f), // Missing
				OnNext<Single?>(240, -7.36f),
				OnCompleted<Single?>(250)
			);

			var res = Scheduler.Start(() =>
				xs.Average().Apply(Scheduler, checkpoints)
			);

			res.Messages.AssertEqual(
				// state saved @225
                // state reloaded @235
				OnNext<Single?>(250, avg => Math.Abs((Single)(avg - -4.920001f)) < 0.0001f),
				OnCompleted<Single?>(250)
			);
		}

		[TestMethod]
		public void AverageDouble_ArgumentChecking()
		{
			ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Double>)null).Average());
		}

		[TestMethod]
		public void AverageDouble_Selector_ArgumentChecking()
		{
			ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Tuple<Double>>)null).Average(t => t.Item1));
			ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<Tuple<Double>>.Instance.Average(default(Func<Tuple<Double>, Double>)));
		}

		[TestMethod]
		public void AverageDouble_SaveAndReload()
		{
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(225, state),
                OnLoad(235, state),
            };

			var xs = Scheduler.CreateHotObservable(
				OnNext<Double>(210, 17.8d),
				OnNext<Double>(220, -25.2d),
				OnNext<Double>(230, 3.5d), // Missing
				OnNext<Double>(240, -7.36d),
				OnCompleted<Double>(250)
			);

			var res = Scheduler.Start(() =>
				xs.Average().Apply(Scheduler, checkpoints)
			);

			res.Messages.AssertEqual(
				// state saved @225
                // state reloaded @235
				OnNext<Double>(250, avg => Math.Abs((Double)(avg - -4.92d)) < 0.0001d),
				OnCompleted<Double>(250)
			);
		}

		[TestMethod]
		public void AverageNullableDouble_ArgumentChecking()
		{
			ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Double?>)null).Average());
		}

		[TestMethod]
		public void AverageNullableDouble_Selector_ArgumentChecking()
		{
			ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Tuple<Double?>>)null).Average(t => t.Item1));
			ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<Tuple<Double?>>.Instance.Average(default(Func<Tuple<Double?>, Double?>)));
		}

		[TestMethod]
		public void AverageNullableDouble_SaveAndReload()
		{
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(225, state),
                OnLoad(235, state),
            };

			var xs = Scheduler.CreateHotObservable(
				OnNext<Double?>(210, 17.8d),
				OnNext<Double?>(220, -25.2d),
				OnNext<Double?>(230, 3.5d), // Missing
				OnNext<Double?>(240, -7.36d),
				OnCompleted<Double?>(250)
			);

			var res = Scheduler.Start(() =>
				xs.Average().Apply(Scheduler, checkpoints)
			);

			res.Messages.AssertEqual(
				// state saved @225
                // state reloaded @235
				OnNext<Double?>(250, avg => Math.Abs((Double)(avg - -4.92d)) < 0.0001d),
				OnCompleted<Double?>(250)
			);
		}

		[TestMethod]
		public void AverageDecimal_ArgumentChecking()
		{
			ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Decimal>)null).Average());
		}

		[TestMethod]
		public void AverageDecimal_Selector_ArgumentChecking()
		{
			ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Tuple<Decimal>>)null).Average(t => t.Item1));
			ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<Tuple<Decimal>>.Instance.Average(default(Func<Tuple<Decimal>, Decimal>)));
		}

		[TestMethod]
		public void AverageDecimal_SaveAndReload()
		{
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(225, state),
                OnLoad(235, state),
            };

			var xs = Scheduler.CreateHotObservable(
				OnNext<Decimal>(210, 24.95m),
				OnNext<Decimal>(220, -7m),
				OnNext<Decimal>(230, 499.99m), // Missing
				OnNext<Decimal>(240, 123m),
				OnCompleted<Decimal>(250)
			);

			var res = Scheduler.Start(() =>
				xs.Average().Apply(Scheduler, checkpoints)
			);

			res.Messages.AssertEqual(
				// state saved @225
                // state reloaded @235
				OnNext<Decimal>(250, 46.983333333333333333333333333m),
				OnCompleted<Decimal>(250)
			);
		}

		[TestMethod]
		public void AverageNullableDecimal_ArgumentChecking()
		{
			ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Decimal?>)null).Average());
		}

		[TestMethod]
		public void AverageNullableDecimal_Selector_ArgumentChecking()
		{
			ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Tuple<Decimal?>>)null).Average(t => t.Item1));
			ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<Tuple<Decimal?>>.Instance.Average(default(Func<Tuple<Decimal?>, Decimal?>)));
		}

		[TestMethod]
		public void AverageNullableDecimal_SaveAndReload()
		{
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(225, state),
                OnLoad(235, state),
            };

			var xs = Scheduler.CreateHotObservable(
				OnNext<Decimal?>(210, 24.95m),
				OnNext<Decimal?>(220, -7m),
				OnNext<Decimal?>(230, 499.99m), // Missing
				OnNext<Decimal?>(240, 123m),
				OnCompleted<Decimal?>(250)
			);

			var res = Scheduler.Start(() =>
				xs.Average().Apply(Scheduler, checkpoints)
			);

			res.Messages.AssertEqual(
				// state saved @225
                // state reloaded @235
				OnNext<Decimal?>(250, 46.983333333333333333333333333m),
				OnCompleted<Decimal?>(250)
			);
		}

	}
}
