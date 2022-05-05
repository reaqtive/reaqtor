// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtive;

namespace Test.Reaqtive
{
    [TestClass]
    public class StaticCompositeSubscriptionTests
    {
        [TestMethod]
        public void StaticCompositeSubscription_ArgumentChecking()
        {
#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            AssertEx.ThrowsException<ArgumentNullException>(() => new StaticCompositeSubscription(default(IEnumerable<ISubscription>)), ex => Assert.AreEqual("subscriptions", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => new StaticCompositeSubscription(default(ISubscription[])), ex => Assert.AreEqual("subscriptions", ex.ParamName));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void StaticCompositeSubscription_AddRemove_Throws()
        {
            var cs = new StaticCompositeSubscription();

            var s1 = new MySub();
            Assert.ThrowsException<InvalidOperationException>(() => cs.Add(s1));

            var cs2 = new StaticCompositeSubscription(s1);
            Assert.ThrowsException<InvalidOperationException>(() => cs.Remove(s1));
        }

        [TestMethod]
        public void StaticCompositeSubscription_Start()
        {
            var s1 = new MySub();
            var s2 = new MySub();
            var s3 = new MySub();
            var cs = new StaticCompositeSubscription(s1, s2, s3);

            Assert.AreEqual(0, s1.StartCount);
            Assert.AreEqual(0, s2.StartCount);
            Assert.AreEqual(0, s3.StartCount);
            Start(cs);
            Assert.AreEqual(1, s1.StartCount);
            Assert.AreEqual(1, s2.StartCount);
            Assert.AreEqual(1, s3.StartCount);
        }

        [TestMethod]
        public void StaticCompositeSubscription_Dispose()
        {
            var s1 = new MySub();
            var s2 = new MySub();
            var s3 = new MySub();
            var cs = new StaticCompositeSubscription(s1, s2, s3);

            Assert.AreEqual(0, s1.DisposedCount);
            Assert.AreEqual(0, s2.DisposedCount);
            Assert.AreEqual(0, s3.DisposedCount);
            Assert.AreEqual(3, cs.Count);

            Assert.IsTrue(new[] { s1, s2, s3 }.SequenceEqual(cs));
            Assert.IsTrue(new[] { s1, s2, s3 }.SequenceEqual(cs.CastNotSmart<ISubscription>()));

            cs.Dispose();

            Assert.AreEqual(1, s1.DisposedCount);
            Assert.AreEqual(1, s2.DisposedCount);
            Assert.AreEqual(1, s3.DisposedCount);
            Assert.AreEqual(3, cs.Count);

            Assert.IsTrue(new[] { s1, s2, s3 }.SequenceEqual(cs));
            Assert.IsTrue(new[] { s1, s2, s3 }.SequenceEqual(cs.CastNotSmart<ISubscription>()));

            // Dispose is idempotent
            cs.Dispose();
            Assert.AreEqual(1, s1.DisposedCount);
            Assert.AreEqual(1, s2.DisposedCount);
            Assert.AreEqual(1, s3.DisposedCount);
            Assert.AreEqual(3, cs.Count);
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
