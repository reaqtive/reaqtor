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
    public partial class MinMax
    {
        [TestMethod]
        public void MinInt32_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Int32>)null).Min());
        }

		[TestMethod]
        public void MinInt32_Selector_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Tuple<Int32>>)null).Min(t => t.Item1));
			ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<Tuple<Int32>>.Instance.Min(default(Func<Tuple<Int32>, Int32>)));
        }

        [TestMethod]
        public void MinNullableInt32_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Int32?>)null).Min());
        }

		[TestMethod]
        public void MinNullableInt32_Selector_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Tuple<Int32?>>)null).Min(t => t.Item1));
			ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<Tuple<Int32?>>.Instance.Min(default(Func<Tuple<Int32?>, Int32?>)));
        }

        [TestMethod]
        public void MinInt64_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Int64>)null).Min());
        }

		[TestMethod]
        public void MinInt64_Selector_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Tuple<Int64>>)null).Min(t => t.Item1));
			ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<Tuple<Int64>>.Instance.Min(default(Func<Tuple<Int64>, Int64>)));
        }

        [TestMethod]
        public void MinNullableInt64_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Int64?>)null).Min());
        }

		[TestMethod]
        public void MinNullableInt64_Selector_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Tuple<Int64?>>)null).Min(t => t.Item1));
			ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<Tuple<Int64?>>.Instance.Min(default(Func<Tuple<Int64?>, Int64?>)));
        }

        [TestMethod]
        public void MinSingle_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Single>)null).Min());
        }

		[TestMethod]
        public void MinSingle_Selector_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Tuple<Single>>)null).Min(t => t.Item1));
			ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<Tuple<Single>>.Instance.Min(default(Func<Tuple<Single>, Single>)));
        }

        [TestMethod]
        public void MinNullableSingle_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Single?>)null).Min());
        }

		[TestMethod]
        public void MinNullableSingle_Selector_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Tuple<Single?>>)null).Min(t => t.Item1));
			ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<Tuple<Single?>>.Instance.Min(default(Func<Tuple<Single?>, Single?>)));
        }

        [TestMethod]
        public void MinDouble_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Double>)null).Min());
        }

		[TestMethod]
        public void MinDouble_Selector_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Tuple<Double>>)null).Min(t => t.Item1));
			ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<Tuple<Double>>.Instance.Min(default(Func<Tuple<Double>, Double>)));
        }

        [TestMethod]
        public void MinNullableDouble_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Double?>)null).Min());
        }

		[TestMethod]
        public void MinNullableDouble_Selector_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Tuple<Double?>>)null).Min(t => t.Item1));
			ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<Tuple<Double?>>.Instance.Min(default(Func<Tuple<Double?>, Double?>)));
        }

        [TestMethod]
        public void MinDecimal_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Decimal>)null).Min());
        }

		[TestMethod]
        public void MinDecimal_Selector_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Tuple<Decimal>>)null).Min(t => t.Item1));
			ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<Tuple<Decimal>>.Instance.Min(default(Func<Tuple<Decimal>, Decimal>)));
        }

        [TestMethod]
        public void MinNullableDecimal_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Decimal?>)null).Min());
        }

		[TestMethod]
        public void MinNullableDecimal_Selector_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Tuple<Decimal?>>)null).Min(t => t.Item1));
			ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<Tuple<Decimal?>>.Instance.Min(default(Func<Tuple<Decimal?>, Decimal?>)));
        }

        [TestMethod]
        public void MaxInt32_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Int32>)null).Max());
        }

		[TestMethod]
        public void MaxInt32_Selector_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Tuple<Int32>>)null).Max(t => t.Item1));
			ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<Tuple<Int32>>.Instance.Max(default(Func<Tuple<Int32>, Int32>)));
        }

        [TestMethod]
        public void MaxNullableInt32_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Int32?>)null).Max());
        }

		[TestMethod]
        public void MaxNullableInt32_Selector_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Tuple<Int32?>>)null).Max(t => t.Item1));
			ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<Tuple<Int32?>>.Instance.Max(default(Func<Tuple<Int32?>, Int32?>)));
        }

        [TestMethod]
        public void MaxInt64_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Int64>)null).Max());
        }

		[TestMethod]
        public void MaxInt64_Selector_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Tuple<Int64>>)null).Max(t => t.Item1));
			ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<Tuple<Int64>>.Instance.Max(default(Func<Tuple<Int64>, Int64>)));
        }

        [TestMethod]
        public void MaxNullableInt64_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Int64?>)null).Max());
        }

		[TestMethod]
        public void MaxNullableInt64_Selector_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Tuple<Int64?>>)null).Max(t => t.Item1));
			ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<Tuple<Int64?>>.Instance.Max(default(Func<Tuple<Int64?>, Int64?>)));
        }

        [TestMethod]
        public void MaxSingle_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Single>)null).Max());
        }

		[TestMethod]
        public void MaxSingle_Selector_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Tuple<Single>>)null).Max(t => t.Item1));
			ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<Tuple<Single>>.Instance.Max(default(Func<Tuple<Single>, Single>)));
        }

        [TestMethod]
        public void MaxNullableSingle_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Single?>)null).Max());
        }

		[TestMethod]
        public void MaxNullableSingle_Selector_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Tuple<Single?>>)null).Max(t => t.Item1));
			ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<Tuple<Single?>>.Instance.Max(default(Func<Tuple<Single?>, Single?>)));
        }

        [TestMethod]
        public void MaxDouble_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Double>)null).Max());
        }

		[TestMethod]
        public void MaxDouble_Selector_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Tuple<Double>>)null).Max(t => t.Item1));
			ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<Tuple<Double>>.Instance.Max(default(Func<Tuple<Double>, Double>)));
        }

        [TestMethod]
        public void MaxNullableDouble_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Double?>)null).Max());
        }

		[TestMethod]
        public void MaxNullableDouble_Selector_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Tuple<Double?>>)null).Max(t => t.Item1));
			ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<Tuple<Double?>>.Instance.Max(default(Func<Tuple<Double?>, Double?>)));
        }

        [TestMethod]
        public void MaxDecimal_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Decimal>)null).Max());
        }

		[TestMethod]
        public void MaxDecimal_Selector_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Tuple<Decimal>>)null).Max(t => t.Item1));
			ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<Tuple<Decimal>>.Instance.Max(default(Func<Tuple<Decimal>, Decimal>)));
        }

        [TestMethod]
        public void MaxNullableDecimal_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Decimal?>)null).Max());
        }

		[TestMethod]
        public void MaxNullableDecimal_Selector_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((ISubscribable<Tuple<Decimal?>>)null).Max(t => t.Item1));
			ReactiveAssert.Throws<ArgumentNullException>(() => DummySubscribable<Tuple<Decimal?>>.Instance.Max(default(Func<Tuple<Decimal?>, Decimal?>)));
        }

    }
}
