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
	public partial class Min
	{
		[TestMethod]
		public void MinInt32_SaveAndReload()
		{
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(225, state),
                OnLoad(235, state),
            };

			var xs = Scheduler.CreateHotObservable(
				OnNext<Int32>(210, 17),
				OnNext<Int32>(220, 25),
				OnNext<Int32>(230, -8), // Missing
				OnNext<Int32>(240, 2),
				OnNext<Int32>(250, 3),
				OnNext<Int32>(260, -5),
				OnNext<Int32>(270, -7),
				OnNext<Int32>(280, 36),
				OnCompleted<Int32>(290)
			);

			var res = Scheduler.Start(() =>
				xs.Min().Apply(Scheduler, checkpoints)
			);

			res.Messages.AssertEqual(
				// state saved @225
                // state reloaded @235
				OnNext<Int32>(290, -7),
				OnCompleted<Int32>(290)
			);
		}

		[TestMethod]
		public void MinNullableInt32_SaveAndReload()
		{
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(225, state),
                OnLoad(235, state),
            };

			var xs = Scheduler.CreateHotObservable(
				OnNext<Int32?>(210, 17),
				OnNext<Int32?>(220, 25),
				OnNext<Int32?>(230, -8), // Missing
				OnNext<Int32?>(240, 2),
				OnNext<Int32?>(250, 3),
				OnNext<Int32?>(260, -5),
				OnNext<Int32?>(270, -7),
				OnNext<Int32?>(280, 36),
				OnCompleted<Int32?>(290)
			);

			var res = Scheduler.Start(() =>
				xs.Min().Apply(Scheduler, checkpoints)
			);

			res.Messages.AssertEqual(
				// state saved @225
                // state reloaded @235
				OnNext<Int32?>(290, -7),
				OnCompleted<Int32?>(290)
			);
		}

		[TestMethod]
		public void MinInt64_SaveAndReload()
		{
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(225, state),
                OnLoad(235, state),
            };

			var xs = Scheduler.CreateHotObservable(
				OnNext<Int64>(210, 17L),
				OnNext<Int64>(220, 25L),
				OnNext<Int64>(230, -8L), // Missing
				OnNext<Int64>(240, 2L),
				OnNext<Int64>(250, 3L),
				OnNext<Int64>(260, -5L),
				OnNext<Int64>(270, -7L),
				OnNext<Int64>(280, 36L),
				OnCompleted<Int64>(290)
			);

			var res = Scheduler.Start(() =>
				xs.Min().Apply(Scheduler, checkpoints)
			);

			res.Messages.AssertEqual(
				// state saved @225
                // state reloaded @235
				OnNext<Int64>(290, -7L),
				OnCompleted<Int64>(290)
			);
		}

		[TestMethod]
		public void MinNullableInt64_SaveAndReload()
		{
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(225, state),
                OnLoad(235, state),
            };

			var xs = Scheduler.CreateHotObservable(
				OnNext<Int64?>(210, 17L),
				OnNext<Int64?>(220, 25L),
				OnNext<Int64?>(230, -8L), // Missing
				OnNext<Int64?>(240, 2L),
				OnNext<Int64?>(250, 3L),
				OnNext<Int64?>(260, -5L),
				OnNext<Int64?>(270, -7L),
				OnNext<Int64?>(280, 36L),
				OnCompleted<Int64?>(290)
			);

			var res = Scheduler.Start(() =>
				xs.Min().Apply(Scheduler, checkpoints)
			);

			res.Messages.AssertEqual(
				// state saved @225
                // state reloaded @235
				OnNext<Int64?>(290, -7L),
				OnCompleted<Int64?>(290)
			);
		}

		[TestMethod]
		public void MinSingle_SaveAndReload()
		{
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(225, state),
                OnLoad(235, state),
            };

			var xs = Scheduler.CreateHotObservable(
				OnNext<Single>(210, 17.8f),
				OnNext<Single>(220, 3.5f),
				OnNext<Single>(230, -25.2f), // Missing
				OnNext<Single>(240, -7.36f),
				OnCompleted<Single>(250)
			);

			var res = Scheduler.Start(() =>
				xs.Min().Apply(Scheduler, checkpoints)
			);

			res.Messages.AssertEqual(
				// state saved @225
                // state reloaded @235
				OnNext<Single>(250, min => Math.Abs((Single)(min - -7.36f)) < 0.0001f),
				OnCompleted<Single>(250)
			);
		}

		[TestMethod]
		public void MinNullableSingle_SaveAndReload()
		{
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(225, state),
                OnLoad(235, state),
            };

			var xs = Scheduler.CreateHotObservable(
				OnNext<Single?>(210, 17.8f),
				OnNext<Single?>(220, 3.5f),
				OnNext<Single?>(230, -25.2f), // Missing
				OnNext<Single?>(240, -7.36f),
				OnCompleted<Single?>(250)
			);

			var res = Scheduler.Start(() =>
				xs.Min().Apply(Scheduler, checkpoints)
			);

			res.Messages.AssertEqual(
				// state saved @225
                // state reloaded @235
				OnNext<Single?>(250, min => Math.Abs((Single)(min - -7.36f)) < 0.0001f),
				OnCompleted<Single?>(250)
			);
		}

		[TestMethod]
		public void MinDouble_SaveAndReload()
		{
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(225, state),
                OnLoad(235, state),
            };

			var xs = Scheduler.CreateHotObservable(
				OnNext<Double>(210, 17.8d),
				OnNext<Double>(220, 3.5d),
				OnNext<Double>(230, -25.2d), // Missing
				OnNext<Double>(240, -7.36d),
				OnCompleted<Double>(250)
			);

			var res = Scheduler.Start(() =>
				xs.Min().Apply(Scheduler, checkpoints)
			);

			res.Messages.AssertEqual(
				// state saved @225
                // state reloaded @235
				OnNext<Double>(250, min => Math.Abs((Double)(min - -7.36d)) < 0.0001d),
				OnCompleted<Double>(250)
			);
		}

		[TestMethod]
		public void MinNullableDouble_SaveAndReload()
		{
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(225, state),
                OnLoad(235, state),
            };

			var xs = Scheduler.CreateHotObservable(
				OnNext<Double?>(210, 17.8d),
				OnNext<Double?>(220, 3.5d),
				OnNext<Double?>(230, -25.2d), // Missing
				OnNext<Double?>(240, -7.36d),
				OnCompleted<Double?>(250)
			);

			var res = Scheduler.Start(() =>
				xs.Min().Apply(Scheduler, checkpoints)
			);

			res.Messages.AssertEqual(
				// state saved @225
                // state reloaded @235
				OnNext<Double?>(250, min => Math.Abs((Double)(min - -7.36d)) < 0.0001d),
				OnCompleted<Double?>(250)
			);
		}

		[TestMethod]
		public void MinDecimal_SaveAndReload()
		{
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(225, state),
                OnLoad(235, state),
            };

			var xs = Scheduler.CreateHotObservable(
				OnNext<Decimal>(210, 24.95m),
				OnNext<Decimal>(220, 19.99m),
				OnNext<Decimal>(230, -7m), // Missing
				OnNext<Decimal>(240, 499.99m),
				OnNext<Decimal>(250, 123m),
				OnCompleted<Decimal>(260)
			);

			var res = Scheduler.Start(() =>
				xs.Min().Apply(Scheduler, checkpoints)
			);

			res.Messages.AssertEqual(
				// state saved @225
                // state reloaded @235
				OnNext<Decimal>(260, 19.99m),
				OnCompleted<Decimal>(260)
			);
		}

		[TestMethod]
		public void MinNullableDecimal_SaveAndReload()
		{
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(225, state),
                OnLoad(235, state),
            };

			var xs = Scheduler.CreateHotObservable(
				OnNext<Decimal?>(210, 24.95m),
				OnNext<Decimal?>(220, 19.99m),
				OnNext<Decimal?>(230, -7m), // Missing
				OnNext<Decimal?>(240, 499.99m),
				OnNext<Decimal?>(250, 123m),
				OnCompleted<Decimal?>(260)
			);

			var res = Scheduler.Start(() =>
				xs.Min().Apply(Scheduler, checkpoints)
			);

			res.Messages.AssertEqual(
				// state saved @225
                // state reloaded @235
				OnNext<Decimal?>(260, 19.99m),
				OnCompleted<Decimal?>(260)
			);
		}

	}
}
