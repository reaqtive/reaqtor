// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Reaqtive;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Reaqtive
{
    [TestClass]
    public class StableCompositeSubscriptionTests
    {
        [TestMethod]
        public void StableCompositeSubscription_ArgumentChecking()
        {
#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            AssertEx.ThrowsException<ArgumentNullException>(() => new StableCompositeSubscription(default(IEnumerable<ISubscription>)), ex => Assert.AreEqual("subscriptions", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => new StableCompositeSubscription(default(ISubscription[])), ex => Assert.AreEqual("subscriptions", ex.ParamName));
#pragma warning restore IDE0034 // Simplify 'default' expression

            var cs = new StableCompositeSubscription();

            AssertEx.ThrowsException<ArgumentNullException>(() => cs.Add(null), ex => Assert.AreEqual("subscription", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => cs.AddRange(null), ex => Assert.AreEqual("subscriptions", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => cs.Remove(null), ex => Assert.AreEqual("subscription", ex.ParamName));
        }

        [TestMethod]
        public void StableCompositeSubscription_AddRemove()
        {
            var cs = new StableCompositeSubscription();

            var s1 = new MySub();
            cs.Add(s1);
            Start(cs);
            Assert.AreEqual(1, s1.StartCount);
            Assert.AreEqual(1, cs.Count);

            var s2 = new MySub();
            cs.Add(s2);
            Start(cs);
            Assert.AreEqual(2, s1.StartCount);
            Assert.AreEqual(1, s2.StartCount);
            Assert.AreEqual(2, cs.Count);

            cs.Remove(s1);
            Assert.AreEqual(1, s1.DisposedCount);
            Assert.AreEqual(1, cs.Count);

            Start(cs);
            Assert.AreEqual(2, s1.StartCount);
            Assert.AreEqual(2, s2.StartCount);

            cs.Remove(s2);
            Assert.AreEqual(1, s1.DisposedCount);
            Assert.AreEqual(1, s2.DisposedCount);
            Assert.AreEqual(0, cs.Count);

            Start(cs);
            Assert.AreEqual(2, s1.StartCount);
            Assert.AreEqual(2, s2.StartCount);

            var s3 = new MySub();
            cs.Add(s3);
            Start(cs);
            Assert.AreEqual(2, s1.StartCount);
            Assert.AreEqual(2, s2.StartCount);
            Assert.AreEqual(1, s3.StartCount);
            Assert.AreEqual(1, cs.Count);

            cs.Remove(s3);
            Assert.AreEqual(1, s3.DisposedCount);
            Assert.AreEqual(0, cs.Count);

            cs.AddRange(new[] { s1, s2, s3 });
            Assert.AreEqual(3, cs.Count);
            cs.Remove(s3);
            Assert.AreEqual(2, s3.DisposedCount);
            Assert.AreEqual(2, cs.Count);
            cs.Add(s3);
            Assert.AreEqual(3, cs.Count);
            cs.Remove(s2);
            Assert.AreEqual(2, s2.DisposedCount);
            Assert.AreEqual(2, cs.Count);
            cs.Add(s2);
            Assert.AreEqual(3, cs.Count);
            cs.Remove(s1);
            Assert.AreEqual(2, s2.DisposedCount);
            Assert.AreEqual(2, cs.Count);
        }

        [TestMethod]
        public void StableCompositeSubscription_AddRange()
        {
            var cs = new StableCompositeSubscription();

            var s1 = new MySub();
            cs.AddRange(new ISubscription[] { s1 });
            Start(cs);
            Assert.AreEqual(1, s1.StartCount);
            Assert.AreEqual(1, cs.Count);

            var s2 = new MySub();
            cs.AddRange(new ISubscription[] { s2 });
            Start(cs);
            Assert.AreEqual(2, s1.StartCount);
            Assert.AreEqual(1, s2.StartCount);
            Assert.AreEqual(2, cs.Count);

            cs.Remove(s1);
            Assert.AreEqual(1, s1.DisposedCount);
            Assert.AreEqual(1, cs.Count);

            Start(cs);
            Assert.AreEqual(2, s1.StartCount);
            Assert.AreEqual(2, s2.StartCount);

            cs.Remove(s2);
            Assert.AreEqual(1, s1.DisposedCount);
            Assert.AreEqual(1, s2.DisposedCount);
            Assert.AreEqual(0, cs.Count);

            Start(cs);
            Assert.AreEqual(2, s1.StartCount);
            Assert.AreEqual(2, s2.StartCount);

            var s3 = new MySub();
            cs.AddRange(new ISubscription[] { s3 });
            Start(cs);
            Assert.AreEqual(2, s1.StartCount);
            Assert.AreEqual(2, s2.StartCount);
            Assert.AreEqual(1, s3.StartCount);
            Assert.AreEqual(1, cs.Count);

            cs.AddRange(Array.Empty<ISubscription>());
            Start(cs);
            Assert.AreEqual(2, s1.StartCount);
            Assert.AreEqual(2, s2.StartCount);
            Assert.AreEqual(2, s3.StartCount);
            Assert.AreEqual(1, cs.Count);
        }

        [TestMethod]
        public void StableCompositeSubscription_Dispose()
        {
            var cs = new StableCompositeSubscription();

            var s1 = new MySub();
            cs.Add(s1);

            var s2 = new MySub();
            cs.Add(s2);

            var s3 = new MySub();
            cs.Add(s3);

            Assert.AreEqual(0, s1.DisposedCount);
            Assert.AreEqual(0, s2.DisposedCount);
            Assert.AreEqual(0, s3.DisposedCount);
            Assert.AreEqual(3, cs.Count);

            Assert.IsTrue(new[] { s1, s2, s3 }.SequenceEqual(cs));
            Assert.IsTrue(new[] { s1, s2, s3 }.SequenceEqual(((IEnumerable)cs).CastNotSmart<ISubscription>()));

            cs.Dispose();

            Assert.AreEqual(1, s1.DisposedCount);
            Assert.AreEqual(1, s2.DisposedCount);
            Assert.AreEqual(1, s3.DisposedCount);
            Assert.AreEqual(0, cs.Count);

            Assert.IsTrue(Enumerable.Empty<ISubscription>().SequenceEqual(cs));
            Assert.IsTrue(Enumerable.Empty<ISubscription>().SequenceEqual(((IEnumerable)cs).CastNotSmart<ISubscription>()));

            var s4 = new MySub();
            cs.Add(s4);

            Assert.AreEqual(1, s4.DisposedCount);
            Assert.AreEqual(0, cs.Count);

            Assert.IsTrue(Enumerable.Empty<ISubscription>().SequenceEqual(cs));
            Assert.IsTrue(Enumerable.Empty<ISubscription>().SequenceEqual(((IEnumerable)cs).CastNotSmart<ISubscription>()));

            cs.AddRange(new ISubscription[] { s1, s2, s3, s4 });

            Assert.AreEqual(2, s1.DisposedCount);
            Assert.AreEqual(2, s2.DisposedCount);
            Assert.AreEqual(2, s3.DisposedCount);
            Assert.AreEqual(2, s4.DisposedCount);
            Assert.AreEqual(0, cs.Count);

            Assert.IsTrue(Enumerable.Empty<ISubscription>().SequenceEqual(cs));
            Assert.IsTrue(Enumerable.Empty<ISubscription>().SequenceEqual(((IEnumerable)cs).CastNotSmart<ISubscription>()));

            cs.Dispose();

            Assert.AreEqual(2, s1.DisposedCount);
            Assert.AreEqual(2, s2.DisposedCount);
            Assert.AreEqual(2, s3.DisposedCount);
            Assert.AreEqual(2, s4.DisposedCount);
            Assert.AreEqual(0, cs.Count);

            Assert.IsTrue(Enumerable.Empty<ISubscription>().SequenceEqual(cs));
            Assert.IsTrue(Enumerable.Empty<ISubscription>().SequenceEqual(((IEnumerable)cs).CastNotSmart<ISubscription>()));
        }

        [TestMethod]
        public void StableCompositeSubscription_ShrinkingBehavior()
        {
            var cs = new StableCompositeSubscription();

            var N = 100;

            var ds1 = Enumerable.Range(0, N).Select(i => (i, s: new MySub())).ToList();

            foreach (var (i, s) in ds1)
            {
                cs.Add(s);
            }

            Start(cs);

            foreach (var (i, s) in ds1)
            {
                Assert.AreEqual(1, s.StartCount);
            }

            var rand = new Random(1983);

            ds1 = ds1.OrderBy(_ => rand.Next()).ToList();

            var R = 2 * N / 3;

            var rs = ds1.Take(R);

            var n = ds1.Count;

            foreach (var (i, s) in rs)
            {
                var d = s;

                cs.Remove(d);
                Assert.AreEqual(1, d.DisposedCount);

                Assert.AreEqual(--n, cs.Count);
            }

            var ds2 = Enumerable.Range(0, N).Select(i => (i, s: new MySub())).ToList();

            foreach (var (i, s) in ds2)
            {
                cs.Add(s);
            }

            Start(cs);

            foreach (var (i, s) in ds1.Skip(R))
            {
                Assert.AreEqual(2, s.StartCount);
            }

            foreach (var (i, s) in ds2)
            {
                Assert.AreEqual(1, s.StartCount);
            }

            var es = ds1.Skip(R).Concat(ds2).OrderBy(_ => rand.Next()).ToList();

            var Q = 9 * es.Count / 10;

            var qs = es.Take(Q);

            n = cs.Count;

            foreach (var (i, s) in qs)
            {
                var d = s;

                cs.Remove(d);
                Assert.AreEqual(1, d.DisposedCount);

                Assert.AreEqual(--n, cs.Count);
            }

            var ts = es.Skip(Q).ToList();

            foreach (var (i, s) in ts)
            {
                var d = s;

                Assert.AreEqual(0, d.DisposedCount);
            }

            cs.Dispose();

            foreach (var (i, s) in ts)
            {
                var d = s;

                Assert.AreEqual(1, d.DisposedCount);
            }
        }

        private static void Start(ISubscription o)
        {
            SubscriptionVisitor.Do<IOperator>(op => op.Start()).Apply(o);
        }

        private sealed class MySub : ISubscription, IOperator
        {
            public void Accept(ISubscriptionVisitor visitor)
            {
                visitor.Visit(this);
            }

            public int DisposedCount = 0;

            public void Dispose()
            {
                DisposedCount++;
            }

            public IEnumerable<ISubscription> Inputs
            {
                get { yield break; }
            }

            public void Subscribe()
            {
            }

            public void SetContext(IOperatorContext context)
            {
                throw new NotImplementedException();
            }

            public int StartCount = 0;

            public void Start()
            {
                StartCount++;
            }
        }
    }
}
