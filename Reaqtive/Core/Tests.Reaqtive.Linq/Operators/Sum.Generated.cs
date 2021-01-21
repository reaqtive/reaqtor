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
	public partial class Sum
	{
        [TestMethod]
        public void SumInt32_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Int32>)null).Sum());
        }

		[TestMethod]
        public void SumInt32_Selector_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Tuple<Int32>>)null).Sum(t => t.Item1));
			ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<Tuple<Int32>>.Instance.Sum(default(Func<Tuple<Int32>, Int32>)));
        }

		[TestMethod]
		public void SumInt32_SaveAndReload()
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
				xs.Sum().Apply(Scheduler, checkpoints)
			);

			res.Messages.AssertEqual(
				// state saved @225
                // state reloaded @235
				OnNext<Int32>(290, 38),
				OnCompleted<Int32>(290)
			);
		}

        [TestMethod]
        public void SumNullableInt32_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Int32?>)null).Sum());
        }

		[TestMethod]
        public void SumNullableInt32_Selector_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Tuple<Int32?>>)null).Sum(t => t.Item1));
			ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<Tuple<Int32?>>.Instance.Sum(default(Func<Tuple<Int32?>, Int32?>)));
        }

		[TestMethod]
		public void SumNullableInt32_SaveAndReload()
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
				xs.Sum().Apply(Scheduler, checkpoints)
			);

			res.Messages.AssertEqual(
				// state saved @225
                // state reloaded @235
				OnNext<Int32?>(290, 38),
				OnCompleted<Int32?>(290)
			);
		}

        [TestMethod]
        public void SumInt64_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Int64>)null).Sum());
        }

		[TestMethod]
        public void SumInt64_Selector_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Tuple<Int64>>)null).Sum(t => t.Item1));
			ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<Tuple<Int64>>.Instance.Sum(default(Func<Tuple<Int64>, Int64>)));
        }

		[TestMethod]
		public void SumInt64_SaveAndReload()
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
				xs.Sum().Apply(Scheduler, checkpoints)
			);

			res.Messages.AssertEqual(
				// state saved @225
                // state reloaded @235
				OnNext<Int64>(290, 38L),
				OnCompleted<Int64>(290)
			);
		}

        [TestMethod]
        public void SumNullableInt64_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Int64?>)null).Sum());
        }

		[TestMethod]
        public void SumNullableInt64_Selector_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Tuple<Int64?>>)null).Sum(t => t.Item1));
			ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<Tuple<Int64?>>.Instance.Sum(default(Func<Tuple<Int64?>, Int64?>)));
        }

		[TestMethod]
		public void SumNullableInt64_SaveAndReload()
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
				xs.Sum().Apply(Scheduler, checkpoints)
			);

			res.Messages.AssertEqual(
				// state saved @225
                // state reloaded @235
				OnNext<Int64?>(290, 38L),
				OnCompleted<Int64?>(290)
			);
		}

        [TestMethod]
        public void SumSingle_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Single>)null).Sum());
        }

		[TestMethod]
        public void SumSingle_Selector_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Tuple<Single>>)null).Sum(t => t.Item1));
			ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<Tuple<Single>>.Instance.Sum(default(Func<Tuple<Single>, Single>)));
        }

		[TestMethod]
		public void SumSingle_SaveAndReload()
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
				xs.Sum().Apply(Scheduler, checkpoints)
			);

			res.Messages.AssertEqual(
				// state saved @225
                // state reloaded @235
				OnNext<Single>(250, sum => Math.Abs((Single)(sum - -14.76f)) < 0.0001f),
				OnCompleted<Single>(250)
			);
		}

        [TestMethod]
        public void SumNullableSingle_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Single?>)null).Sum());
        }

		[TestMethod]
        public void SumNullableSingle_Selector_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Tuple<Single?>>)null).Sum(t => t.Item1));
			ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<Tuple<Single?>>.Instance.Sum(default(Func<Tuple<Single?>, Single?>)));
        }

		[TestMethod]
		public void SumNullableSingle_SaveAndReload()
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
				xs.Sum().Apply(Scheduler, checkpoints)
			);

			res.Messages.AssertEqual(
				// state saved @225
                // state reloaded @235
				OnNext<Single?>(250, sum => Math.Abs((Single)(sum - -14.76f)) < 0.0001f),
				OnCompleted<Single?>(250)
			);
		}

        [TestMethod]
        public void SumDouble_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Double>)null).Sum());
        }

		[TestMethod]
        public void SumDouble_Selector_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Tuple<Double>>)null).Sum(t => t.Item1));
			ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<Tuple<Double>>.Instance.Sum(default(Func<Tuple<Double>, Double>)));
        }

		[TestMethod]
		public void SumDouble_SaveAndReload()
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
				xs.Sum().Apply(Scheduler, checkpoints)
			);

			res.Messages.AssertEqual(
				// state saved @225
                // state reloaded @235
				OnNext<Double>(250, sum => Math.Abs((Double)(sum - -14.76d)) < 0.0001d),
				OnCompleted<Double>(250)
			);
		}

        [TestMethod]
        public void SumNullableDouble_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Double?>)null).Sum());
        }

		[TestMethod]
        public void SumNullableDouble_Selector_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Tuple<Double?>>)null).Sum(t => t.Item1));
			ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<Tuple<Double?>>.Instance.Sum(default(Func<Tuple<Double?>, Double?>)));
        }

		[TestMethod]
		public void SumNullableDouble_SaveAndReload()
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
				xs.Sum().Apply(Scheduler, checkpoints)
			);

			res.Messages.AssertEqual(
				// state saved @225
                // state reloaded @235
				OnNext<Double?>(250, sum => Math.Abs((Double)(sum - -14.76d)) < 0.0001d),
				OnCompleted<Double?>(250)
			);
		}

        [TestMethod]
        public void SumDecimal_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Decimal>)null).Sum());
        }

		[TestMethod]
        public void SumDecimal_Selector_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Tuple<Decimal>>)null).Sum(t => t.Item1));
			ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<Tuple<Decimal>>.Instance.Sum(default(Func<Tuple<Decimal>, Decimal>)));
        }

		[TestMethod]
		public void SumDecimal_SaveAndReload()
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
				xs.Sum().Apply(Scheduler, checkpoints)
			);

			res.Messages.AssertEqual(
				// state saved @225
                // state reloaded @235
				OnNext<Decimal>(250, 140.95m),
				OnCompleted<Decimal>(250)
			);
		}

        [TestMethod]
        public void SumNullableDecimal_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Decimal?>)null).Sum());
        }

		[TestMethod]
        public void SumNullableDecimal_Selector_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Tuple<Decimal?>>)null).Sum(t => t.Item1));
			ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<Tuple<Decimal?>>.Instance.Sum(default(Func<Tuple<Decimal?>, Decimal?>)));
        }

		[TestMethod]
		public void SumNullableDecimal_SaveAndReload()
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
				xs.Sum().Apply(Scheduler, checkpoints)
			);

			res.Messages.AssertEqual(
				// state saved @225
                // state reloaded @235
				OnNext<Decimal?>(250, 140.95m),
				OnCompleted<Decimal?>(250)
			);
		}

	}
}
