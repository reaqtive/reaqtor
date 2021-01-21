// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Memory;
using System.Reflection;
using System.Threading;

using Reaqtive;
using Reaqtive.Expressions;

using Reaqtor.Reactive.Expressions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Reaqtor.Expressions
{
    [TestClass]
    public class QuotationTests
    {
        [TestMethod]
        public void QuotedOfT_ArgumentChecking()
        {
#pragma warning disable IDE0034 // Simplify 'default' expression (illustrative of method signature)
            Assert.ThrowsException<ArgumentNullException>(() => new Quoted<int>(default(Expression)));
            Assert.ThrowsException<ArgumentNullException>(() => new Quoted<int>(42, default(Expression)));
            Assert.ThrowsException<ArgumentNullException>(() => new Quoted<int>(42, Expression.Constant(42), default(IExpressionPolicy)));

            Assert.ThrowsException<ArgumentException>(() => new Quoted<int>(42, Expression.Constant("hello")));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void QuotedOfT_HasValue()
        {
            var c = new ValueCell(42);

            Expression<Func<int>> f = () => c.Value;

            var e = f.Body;

            var q = new Quoted<int>(42, e);

            var v = q.Value;

            Assert.AreEqual(42, q.Value);
            Assert.AreSame(e, q.Expression);

            Assert.IsFalse(c.HasAccessed);

            Assert.IsTrue(q.ToString().Contains("42"));
        }

        [TestMethod]
        public void QuotedOfT_NoValue()
        {
            var c = new ValueCell(42);

            Expression<Func<int>> f = () => c.Value;

            var e = f.Body;

            var q = new Quoted<int>(e);

            var v = q.Value;

            Assert.AreEqual(42, q.Value);
            Assert.AreSame(e, q.Expression);

            Assert.IsTrue(c.HasAccessed);

            Assert.IsTrue(q.ToString().Contains("42"));
        }

        [TestMethod]
        public void QuotedOfT_WithExpressionPolicy()
        {
            var policy = new TestPolicy
            {
                InMemoryCache = new ExpressionHeap(),
                DelegateCache = new SimpleCompiledDelegateCache(),
                ConstantHoister = ConstantHoister.Create(useDefaultForNull: false)
            };

            var expr = Expression.Constant(42, typeof(int));

            var q = new Quoted<int>(expr, policy);
            Assert.AreEqual(42, q.Value);
            Assert.AreNotSame(expr, q.Expression);
            Assert.IsTrue(new ExpressionEqualityComparer().Equals(expr, q.Expression));
        }

        [TestMethod]
        public void QuotedSubscribableOfT_Basics()
        {
            var xs = new Return<int>(42);

            var q = new QuotedSubscribable<int>(Expression.Constant(xs));

            var values = new List<int>();
            var e = new ManualResetEvent(false);

            var s = q.Subscribe(Observer.Create<int>(values.Add, _ => Assert.Fail(), () => e.Set()));
            new SubscriptionInitializeVisitor(s).Start();
            e.WaitOne();

            Assert.IsTrue(new[] { 42 }.SequenceEqual(values));

            values.Clear();
            e.Reset();

            var d = ((IObservable<int>)q).Subscribe(values.Add, _ => Assert.Fail(), () => e.Set());
            new SubscriptionInitializeVisitor((ISubscription)d).Start();
            e.WaitOne();

            Assert.IsTrue(new[] { 42 }.SequenceEqual(values));
        }

        private sealed class Return<T> : SubscribableBase<T>
        {
            private readonly T _value;

            public Return(T value)
            {
                _value = value;
            }

            protected override ISubscription SubscribeCore(IObserver<T> observer)
            {
                return new _(this, observer);
            }

            private sealed class _ : Operator<Return<T>, T>
            {
                public _(Return<T> ret, IObserver<T> obs)
                    : base(ret, obs)
                {
                }

                protected override void OnStart()
                {
                    base.OnStart();

                    Output.OnNext(Params._value);
                    Output.OnCompleted();
                }
            }
        }

        private sealed class ValueCell
        {
            private readonly int _value;

            public ValueCell(int value)
            {
                _value = value;
                HasAccessed = false;
            }

            public int Value
            {
                get
                {
                    HasAccessed = true;
                    return _value;
                }
            }

            public bool HasAccessed { get; private set; }
        }

        private sealed class TestPolicy : IExpressionPolicy
        {
            public ICompiledDelegateCache DelegateCache
            {
                get;
                set;
            }

            public ICache<Expression> InMemoryCache
            {
                get;
                set;
            }

            public IConstantHoister ConstantHoister
            {
                get;
                set;
            }

            public bool OutlineCompilation
            {
                get;
                set;
            }

            public IReflectionProvider ReflectionProvider
            {
                get;
                set;
            }

            public IExpressionFactory ExpressionFactory
            {
                get;
                set;
            }

            public IMemoizer LiftMemoizer
            {
                get;
                set;
            }

            public IMemoizer ReduceMemoizer
            {
                get;
                set;
            }
        }
    }
}
