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
	public partial class Max
	{
		[TestMethod]
		public void MaxInt32_SaveAndReload()
		{
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(225, state),
                OnLoad(235, state),
            };

			var xs = Scheduler.CreateHotObservable(
				OnNext<Int32>(210, 17),
				OnNext<Int32>(220, 25),
				OnNext<Int32>(230, 1999), // Missing
				OnNext<Int32>(240, -8),
				OnNext<Int32>(250, 2),
				OnNext<Int32>(260, 3),
				OnNext<Int32>(270, -5),
				OnNext<Int32>(280, -7),
				OnNext<Int32>(290, 36),
				OnCompleted<Int32>(300)
			);

			var res = Scheduler.Start(() =>
				xs.Max().Apply(Scheduler, checkpoints)
			);

			res.Messages.AssertEqual(
				// state saved @225
                // state reloaded @235
				OnNext<Int32>(300, 36),
				OnCompleted<Int32>(300)
			);
		}

		[TestMethod]
		public void MaxNullableInt32_SaveAndReload()
		{
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(225, state),
                OnLoad(235, state),
            };

			var xs = Scheduler.CreateHotObservable(
				OnNext<Int32?>(210, 17),
				OnNext<Int32?>(220, 25),
				OnNext<Int32?>(230, 1999), // Missing
				OnNext<Int32?>(240, -8),
				OnNext<Int32?>(250, 2),
				OnNext<Int32?>(260, 3),
				OnNext<Int32?>(270, -5),
				OnNext<Int32?>(280, -7),
				OnNext<Int32?>(290, 36),
				OnCompleted<Int32?>(300)
			);

			var res = Scheduler.Start(() =>
				xs.Max().Apply(Scheduler, checkpoints)
			);

			res.Messages.AssertEqual(
				// state saved @225
                // state reloaded @235
				OnNext<Int32?>(300, 36),
				OnCompleted<Int32?>(300)
			);
		}

		[TestMethod]
		public void MaxInt64_SaveAndReload()
		{
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(225, state),
                OnLoad(235, state),
            };

			var xs = Scheduler.CreateHotObservable(
				OnNext<Int64>(210, 17L),
				OnNext<Int64>(220, 25L),
				OnNext<Int64>(230, 1999L), // Missing
				OnNext<Int64>(240, -8L),
				OnNext<Int64>(250, 2L),
				OnNext<Int64>(260, 3L),
				OnNext<Int64>(270, -5L),
				OnNext<Int64>(280, -7L),
				OnNext<Int64>(290, 36L),
				OnCompleted<Int64>(300)
			);

			var res = Scheduler.Start(() =>
				xs.Max().Apply(Scheduler, checkpoints)
			);

			res.Messages.AssertEqual(
				// state saved @225
                // state reloaded @235
				OnNext<Int64>(300, 36L),
				OnCompleted<Int64>(300)
			);
		}

		[TestMethod]
		public void MaxNullableInt64_SaveAndReload()
		{
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(225, state),
                OnLoad(235, state),
            };

			var xs = Scheduler.CreateHotObservable(
				OnNext<Int64?>(210, 17L),
				OnNext<Int64?>(220, 25L),
				OnNext<Int64?>(230, 1999L), // Missing
				OnNext<Int64?>(240, -8L),
				OnNext<Int64?>(250, 2L),
				OnNext<Int64?>(260, 3L),
				OnNext<Int64?>(270, -5L),
				OnNext<Int64?>(280, -7L),
				OnNext<Int64?>(290, 36L),
				OnCompleted<Int64?>(300)
			);

			var res = Scheduler.Start(() =>
				xs.Max().Apply(Scheduler, checkpoints)
			);

			res.Messages.AssertEqual(
				// state saved @225
                // state reloaded @235
				OnNext<Int64?>(300, 36L),
				OnCompleted<Int64?>(300)
			);
		}

		[TestMethod]
		public void MaxSingle_SaveAndReload()
		{
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(225, state),
                OnLoad(235, state),
            };

			var xs = Scheduler.CreateHotObservable(
				OnNext<Single>(210, 17.8f),
				OnNext<Single>(220, 3.5f),
				OnNext<Single>(230, 199.9f), // Missing
				OnNext<Single>(240, -25.2f),
				OnNext<Single>(250, -7.36f),
				OnCompleted<Single>(260)
			);

			var res = Scheduler.Start(() =>
				xs.Max().Apply(Scheduler, checkpoints)
			);

			res.Messages.AssertEqual(
				// state saved @225
                // state reloaded @235
				OnNext<Single>(260, max => Math.Abs((Single)(max - 17.8f)) < 0.0001f),
				OnCompleted<Single>(260)
			);
		}

		[TestMethod]
		public void MaxNullableSingle_SaveAndReload()
		{
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(225, state),
                OnLoad(235, state),
            };

			var xs = Scheduler.CreateHotObservable(
				OnNext<Single?>(210, 17.8f),
				OnNext<Single?>(220, 3.5f),
				OnNext<Single?>(230, 199.9f), // Missing
				OnNext<Single?>(240, -25.2f),
				OnNext<Single?>(250, -7.36f),
				OnCompleted<Single?>(260)
			);

			var res = Scheduler.Start(() =>
				xs.Max().Apply(Scheduler, checkpoints)
			);

			res.Messages.AssertEqual(
				// state saved @225
                // state reloaded @235
				OnNext<Single?>(260, max => Math.Abs((Single)(max - 17.8f)) < 0.0001f),
				OnCompleted<Single?>(260)
			);
		}

		[TestMethod]
		public void MaxDouble_SaveAndReload()
		{
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(225, state),
                OnLoad(235, state),
            };

			var xs = Scheduler.CreateHotObservable(
				OnNext<Double>(210, 17.8d),
				OnNext<Double>(220, 3.5d),
				OnNext<Double>(230, 199.9d), // Missing
				OnNext<Double>(240, -25.2d),
				OnNext<Double>(250, -7.36d),
				OnCompleted<Double>(260)
			);

			var res = Scheduler.Start(() =>
				xs.Max().Apply(Scheduler, checkpoints)
			);

			res.Messages.AssertEqual(
				// state saved @225
                // state reloaded @235
				OnNext<Double>(260, max => Math.Abs((Double)(max - 17.8d)) < 0.0001d),
				OnCompleted<Double>(260)
			);
		}

		[TestMethod]
		public void MaxNullableDouble_SaveAndReload()
		{
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(225, state),
                OnLoad(235, state),
            };

			var xs = Scheduler.CreateHotObservable(
				OnNext<Double?>(210, 17.8d),
				OnNext<Double?>(220, 3.5d),
				OnNext<Double?>(230, 199.9d), // Missing
				OnNext<Double?>(240, -25.2d),
				OnNext<Double?>(250, -7.36d),
				OnCompleted<Double?>(260)
			);

			var res = Scheduler.Start(() =>
				xs.Max().Apply(Scheduler, checkpoints)
			);

			res.Messages.AssertEqual(
				// state saved @225
                // state reloaded @235
				OnNext<Double?>(260, max => Math.Abs((Double)(max - 17.8d)) < 0.0001d),
				OnCompleted<Double?>(260)
			);
		}

		[TestMethod]
		public void MaxDecimal_SaveAndReload()
		{
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(225, state),
                OnLoad(235, state),
            };

			var xs = Scheduler.CreateHotObservable(
				OnNext<Decimal>(210, 24.95m),
				OnNext<Decimal>(220, 19.99m),
				OnNext<Decimal>(230, 1999m), // Missing
				OnNext<Decimal>(240, -7m),
				OnNext<Decimal>(250, 499.99m),
				OnNext<Decimal>(260, 123m),
				OnCompleted<Decimal>(270)
			);

			var res = Scheduler.Start(() =>
				xs.Max().Apply(Scheduler, checkpoints)
			);

			res.Messages.AssertEqual(
				// state saved @225
                // state reloaded @235
				OnNext<Decimal>(270, 499.99m),
				OnCompleted<Decimal>(270)
			);
		}

		[TestMethod]
		public void MaxNullableDecimal_SaveAndReload()
		{
            var state = Scheduler.CreateStateContainer();

            var checkpoints = new[] {
                OnSave(225, state),
                OnLoad(235, state),
            };

			var xs = Scheduler.CreateHotObservable(
				OnNext<Decimal?>(210, 24.95m),
				OnNext<Decimal?>(220, 19.99m),
				OnNext<Decimal?>(230, 1999m), // Missing
				OnNext<Decimal?>(240, -7m),
				OnNext<Decimal?>(250, 499.99m),
				OnNext<Decimal?>(260, 123m),
				OnCompleted<Decimal?>(270)
			);

			var res = Scheduler.Start(() =>
				xs.Max().Apply(Scheduler, checkpoints)
			);

			res.Messages.AssertEqual(
				// state saved @225
                // state reloaded @235
				OnNext<Decimal?>(270, 499.99m),
				OnCompleted<Decimal?>(270)
			);
		}

	}
}
