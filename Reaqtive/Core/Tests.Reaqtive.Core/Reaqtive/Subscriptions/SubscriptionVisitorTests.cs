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
    public class SubscriptionVisitorTests
    {
        [TestMethod]
        public void SubscriptionVisitor_Simple()
        {
            var g1 = new MySpecialOperator("g1");
            var c1 = new MyOperator("c1", g1);
            var c2 = new MySpecialOperator("c2");
            var c3 = new MyOperator("c3");
            var p = new MyOperator("p", c1, c2, c3);

            var log = new List<int>();
            var lst1 = new List<string>();
            var lst2 = new List<string>();

            SubscriptionVisitor
                .Do<IMyOperator>(op => { log.Add(1); lst1.Add(op.Name); })
                .Do<IMySpecialOperator>(op => { log.Add(2); lst2.Add(op.Name); })
                .Apply(p);

            Assert.IsTrue(new[] { 1, 1, 1, 2, 1, 2, 1 }.SequenceEqual(log));
            Assert.IsTrue(new[] { "p", "c1", "g1", "c2", "c3" }.SequenceEqual(lst1));
            Assert.IsTrue(new[] { "g1", "c2" }.SequenceEqual(lst2));
        }

        [TestMethod]
        public void SubscriptionVisitor_Null()
        {
            var visitor = new SubscriptionVisitor<MyOperator>(o =>
            {
                Assert.Fail();
            });

            visitor.Visit(null);
        }

        [TestMethod]
        public void SubscriptionInitializeVisitor_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new SubscriptionInitializeVisitor(null));
        }

        private interface IMySpecialOperator : IMyOperator
        {
        }

        private interface IMyOperator : IOperator
        {
            string Name { get; }
        }

        private sealed class MySpecialOperator : MyOperator, IMySpecialOperator
        {
            public MySpecialOperator(string name, params ISubscription[] inputs)
                : base(name, inputs)
            {
            }
        }

        private class MyOperator : IMyOperator, ISubscription
        {
            private readonly ISubscription[] _inputs;

            public MyOperator(string name, params ISubscription[] inputs)
            {
                Name = name;
                _inputs = inputs;
            }

            public string Name { get; private set; }

            public IEnumerable<ISubscription> Inputs => _inputs;

            public void Subscribe()
            {
            }

            public void SetContext(IOperatorContext context)
            {
                throw new NotImplementedException();
            }

            public void Start()
            {
                throw new NotImplementedException();
            }

            public void Dispose()
            {
                throw new NotImplementedException();
            }

            public void Accept(ISubscriptionVisitor visitor)
            {
                visitor.Visit(this);
            }
        }
    }
}
