// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.CompilerServices;
using System.Linq.CompilerServices.TypeSystem;
using System.Linq.Expressions;

using Reaqtive;

using Reaqtor;
using Reaqtor.QueryEngine;
using Reaqtor.QueryEngine.Mocks;
using Reaqtor.Reactive;
using Reaqtor.Reliable;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor.QueryEngine
{
    [TestClass]
    public class QueryEngineBinderTests
    {
        private static readonly string[] Keys = new[]
        {
            new Uri("test://key0/").ToCanonicalString(),
            new Uri("test://key1/").ToCanonicalString(),
            new Uri("test://key2/").ToCanonicalString(),
        };

        private static readonly Expression ReliableStream = Expression.New(typeof(DummyReliableSubject));
        private static readonly Expression UntypedStream = Expression.New(typeof(DummyUntypedSubject));
        private static readonly Expression NullSubscribable = Expression.Default(typeof(ISubscribable<int>));
        private static readonly Expression NullSubscribableImpl = Expression.Default(typeof(DummySubscribable));
        private static readonly Expression<Func<int, ISubscribable<int>>> ParameterizedSubscribable = x => new DummySubscribable(x);
        private static readonly Expression<Func<int, DummySubscribable>> DerivedParameterizedSubscribable = x => new DummySubscribable(x);
        private static readonly Expression NullObserver = Expression.Default(typeof(IObserver<int>));
        private static readonly Expression NullObserverImpl = Expression.Default(typeof(DummyObserver));
        private static readonly Expression NullReliableObservable = Expression.Default(typeof(IReliableObservable<int>));
        private static readonly Expression NullReliableObservableImpl = Expression.Default(typeof(DummyReliableObservable));
        private static readonly Expression NullReliableObserver = Expression.Default(typeof(IReliableObserver<int>));
        private static readonly Expression NullReliableObserverImpl = Expression.Default(typeof(DummyReliableObserver));
        private static readonly Expression<Func<IReliableMultiSubject<int, int>>> FuncReliableStreamFactory = () => new DummyReliableSubject();
        private static readonly Expression ProperReliableStreamFactory = Expression.New(typeof(DummyReliableSubjectFactory));
        private static readonly Expression<Func<IMultiSubject>> FuncUntypedStreamFactory = () => new DummyUntypedSubject();
        private static readonly Expression Subscription = Expression.New(typeof(DummySubscription));
        private static readonly Expression ReliableSubscription = Expression.New(typeof(DummyReliableSubscription));
        private static readonly Expression<Func<string, string>> OtherExpression = name => "Hello, " + name;
        private static readonly Expression OtherNonLambda = Expression.New(typeof(object));

        private static readonly Func<string, Expression> SubscribableMacro = name => Expression.Parameter(typeof(ISubscribable<int>), name);
        private static readonly Func<string, Expression> ObserverMacro = name => Expression.Parameter(typeof(IObserver<int>), name);
        private static readonly Func<string, Expression> ReliableObservableMacro = name => Expression.Parameter(typeof(IReliableObservable<int>), name);
        private static readonly Func<string, Expression> ReliableObserverMacro = name => Expression.Parameter(typeof(IReliableObserver<int>), name);
        private static readonly Func<string, Expression> ReliableStreamFactoryMacro = name => Expression.Parameter(typeof(Func<IReliableMultiSubject<int, int>>), name);
        private static readonly Func<string, Expression> UntypedStreamFactoryMacro = name => Expression.Parameter(typeof(Func<IMultiSubject>), name);
        private static readonly Func<string, Expression> OtherMacro = name => Expression.Parameter(typeof(Func<string, string>), name);
        private static readonly Func<string, Expression> OtherNonLambdaMacro = name => Expression.Parameter(typeof(object), name);

        private static readonly Expression<Func<Tuple<int>>> TupleFactory = () => new Tuple<int>(42);
        private static readonly Expression<Func<Tuple<int>, ISubscribable<int>>> TemplateExpressionNoFreeVars = t => new DummySubscribable(t.Item1);
        private static readonly Expression<Func<Tuple<int>, Func<int, ISubscribable<int>>, ISubscribable<int>>> TemplateFactory = (t, paramIO) => paramIO(t.Item1);
        private static readonly Func<string, Expression> TemplateExpressionFreeVars = name => Expression.Parameter(typeof(Tuple<int>)).Let(x =>
            Expression.Lambda(BetaReducer.Reduce(Expression.Invoke(TemplateFactory, x, Expression.Parameter(typeof(Func<int, ISubscribable<int>>), name))), x));
        private static readonly Func<string, Expression> TemplatizedObservableNoFreeVars = name => Expression.Invoke(Expression.Parameter(typeof(Func<Tuple<int>, ISubscribable<int>>), name), TupleFactory.Body);
        private static readonly Func<string, Expression> TemplatizedObservableFreeVars = name => Expression.Invoke(Expression.Parameter(typeof(Func<Tuple<int>, ISubscribable<int>>), name), TupleFactory.Body);

        private static readonly Func<Expression, Type, Expression> Convert = (e, t) => Expression.Convert(e, t);

        #region DefinitionInliningBinder Tests

        [TestMethod]
        public void DefinitionInliningBinder_Subscribable_Unbound()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Observable, Keys[0], NullSubscribable }
            };

            var binder = new DefinitionInliningBinder(registry);
            var expression = Expression.Parameter(typeof(ISubscribable<int>), Keys[0]);
            Assert.AreSame(expression, binder.Bind(expression));
        }

        [TestMethod]
        public void DefinitionInliningBinder_Subscribable_Bound()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Observable, Keys[0], NullSubscribable },
                { ReactiveEntityKind.Observable, Keys[1], SubscribableMacro(Keys[0]) },
            };

            var binder = new DefinitionInliningBinder(registry);
            var expression = Expression.Parameter(typeof(ISubscribable<int>), Keys[1]);
            AssertEqual(Expression.Parameter(typeof(ISubscribable<int>), Keys[0]), binder.Bind(expression));
        }

        [TestMethod]
        public void DefinitionInliningBinder_Subscribable_NotInRegistry()
        {
            var registry = new Registry
            {
            };

            var binder = new DefinitionInliningBinder(registry);
            var expression = Expression.Parameter(typeof(ISubscribable<int>), Keys[0]);
            Assert.AreSame(expression, binder.Bind(expression));
        }

        [TestMethod]
        public void DefinitionInliningBinder_Observer_Unbound()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Observer, Keys[0], NullObserver }
            };

            var binder = new DefinitionInliningBinder(registry);
            var expression = Expression.Parameter(typeof(IObserver<int>), Keys[0]);
            Assert.AreSame(expression, binder.Bind(expression));
        }

        [TestMethod]
        public void DefinitionInliningBinder_Observer_Bound()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Observer, Keys[0], NullObserver },
                { ReactiveEntityKind.Observer, Keys[1], ObserverMacro(Keys[0]) },
            };

            var binder = new DefinitionInliningBinder(registry);
            var expression = Expression.Parameter(typeof(IObserver<int>), Keys[1]);
            AssertEqual(Expression.Parameter(typeof(IObserver<int>), Keys[0]), binder.Bind(expression));
        }

        [TestMethod]
        public void DefinitionInliningBinder_Observer_NotInRegistry()
        {
            var registry = new Registry
            {
            };

            var binder = new DefinitionInliningBinder(registry);
            var expression = Expression.Parameter(typeof(IObserver<int>), Keys[0]);
            Assert.AreSame(expression, binder.Bind(expression));
        }

        [TestMethod]
        public void DefinitionInliningBinder_ReliableObservable_Unbound()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Observable, Keys[0], NullReliableObservable },
            };

            var binder = new DefinitionInliningBinder(registry);
            var expression = Expression.Parameter(typeof(IReliableObservable<int>), Keys[0]);
            Assert.AreSame(expression, binder.Bind(expression));
        }

        [TestMethod]
        public void DefinitionInliningBinder_ReliableObservable_Bound()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Observable, Keys[0], NullReliableObservable },
                { ReactiveEntityKind.Observable, Keys[1], ReliableObservableMacro(Keys[0]) },
            };

            var binder = new DefinitionInliningBinder(registry);
            var expression = Expression.Parameter(typeof(IReliableObservable<int>), Keys[1]);
            AssertEqual(Expression.Parameter(typeof(IReliableObservable<int>), Keys[0]), binder.Bind(expression));
        }

        [TestMethod]
        public void DefinitionInliningBinder_ReliableObservable_NotInRegistry()
        {
            var registry = new Registry
            {
            };

            var binder = new DefinitionInliningBinder(registry);
            var expression = Expression.Parameter(typeof(IReliableObservable<int>), Keys[0]);
            Assert.AreSame(expression, binder.Bind(expression));
        }

        [TestMethod]
        public void DefinitionInliningBinder_ReliableObserver_Unbound()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Observer, Keys[0], NullReliableObserver },
            };

            var binder = new DefinitionInliningBinder(registry);
            var expression = Expression.Parameter(typeof(IReliableObserver<int>), Keys[0]);
            Assert.AreSame(expression, binder.Bind(expression));
        }

        [TestMethod]
        public void DefinitionInliningBinder_ReliableObserver_Bound()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Observer, Keys[0], NullReliableObserver },
                { ReactiveEntityKind.Observer, Keys[1], ReliableObserverMacro(Keys[0]) },
            };

            var binder = new DefinitionInliningBinder(registry);
            var expression = Expression.Parameter(typeof(IReliableObserver<int>), Keys[1]);
            AssertEqual(Expression.Parameter(typeof(IReliableObserver<int>), Keys[0]), binder.Bind(expression));
        }

        [TestMethod]
        public void DefinitionInliningBinder_ReliableObserver_NotInRegistry()
        {
            var registry = new Registry
            {
            };

            var binder = new DefinitionInliningBinder(registry);
            var expression = Expression.Parameter(typeof(IReliableObserver<int>), Keys[0]);
            Assert.AreSame(expression, binder.Bind(expression));
        }

        [TestMethod]
        public void DefinitionInliningBinder_ReliableStreamFactory_NotInRegistry()
        {
            var registry = new Registry
            {
            };

            var binder = new DefinitionInliningBinder(registry);
            var expression = Expression.Parameter(typeof(Func<IReliableMultiSubject<int, int>>), Keys[0]);
            Assert.AreSame(expression, binder.Bind(expression));
        }

        [TestMethod]
        public void DefinitionInliningBinder_ReliableStreamFactory_Func_Unbound()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.StreamFactory, Keys[0], FuncReliableStreamFactory },
            };

            var binder = new DefinitionInliningBinder(registry);
            var expression = Expression.Parameter(typeof(Func<IReliableMultiSubject<int, int>>), Keys[0]);
            Assert.AreSame(expression, binder.Bind(expression));
        }

        [TestMethod]
        public void DefinitionInliningBinder_ReliableStreamFactory_Func_Bound()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.StreamFactory, Keys[0], FuncReliableStreamFactory },
                { ReactiveEntityKind.StreamFactory, Keys[1], ReliableStreamFactoryMacro(Keys[0]) },
            };

            var binder = new DefinitionInliningBinder(registry);
            var expression = Expression.Parameter(typeof(Func<IReliableMultiSubject<int, int>>), Keys[1]);
            AssertEqual(Expression.Parameter(typeof(Func<IReliableMultiSubject<int, int>>), Keys[0]), binder.Bind(expression));
        }

        [TestMethod]
        public void DefinitionInliningBinder_ReliableStreamFactory_Proper_Unbound()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.StreamFactory, Keys[0], ProperReliableStreamFactory },
            };

            var binder = new DefinitionInliningBinder(registry);
            var expression = Expression.Parameter(typeof(Func<IReliableMultiSubject<int, int>>), Keys[0]);
            Assert.AreSame(expression, binder.Bind(expression));
        }

        [TestMethod]
        public void DefinitionInliningBinder_ReliableStreamFactory_Proper_Bound()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.StreamFactory, Keys[0], ProperReliableStreamFactory },
                { ReactiveEntityKind.StreamFactory, Keys[1], ReliableStreamFactoryMacro(Keys[0]) },
            };

            var binder = new DefinitionInliningBinder(registry);
            var expression = Expression.Parameter(typeof(Func<IReliableMultiSubject<int, int>>), Keys[0]);
            AssertEqual(Expression.Parameter(typeof(Func<IReliableMultiSubject<int, int>>), Keys[0]), binder.Bind(expression));
        }

        [TestMethod]
        public void DefinitionInliningBinder_UntypedStreamFactory_NotInRegistry()
        {
            var registry = new Registry
            {
            };

            var binder = new DefinitionInliningBinder(registry);
            var expression = Expression.Parameter(typeof(Func<IMultiSubject>), Keys[0]);
            Assert.AreSame(expression, binder.Bind(expression));
        }

        [TestMethod]
        public void DefinitionInliningBinder_UntypedStreamFactory_Func_Unbound()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.StreamFactory, Keys[0], FuncUntypedStreamFactory },
            };

            var binder = new DefinitionInliningBinder(registry);
            var expression = Expression.Parameter(typeof(Func<IMultiSubject>), Keys[0]);
            Assert.AreSame(expression, binder.Bind(expression));
        }

        [TestMethod]
        public void DefinitionInliningBinder_UntypedStreamFactory_Func_Bound()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.StreamFactory, Keys[0], FuncUntypedStreamFactory },
                { ReactiveEntityKind.StreamFactory, Keys[1], UntypedStreamFactoryMacro(Keys[0]) },
            };

            var binder = new DefinitionInliningBinder(registry);
            var expression = Expression.Parameter(typeof(Func<IMultiSubject>), Keys[1]);
            AssertEqual(Expression.Parameter(typeof(Func<IMultiSubject>), Keys[0]), binder.Bind(expression));
        }

        [TestMethod]
        public void DefinitionInliningBinder_ReliableStream_AsSubscribable_Unbound()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Stream, Keys[0], ReliableStream },
            };

            var binder = new DefinitionInliningBinder(registry);
            var expression = Expression.Parameter(typeof(ISubscribable<int>), Keys[0]);
            Assert.AreSame(expression, binder.Bind(expression));
        }

        [TestMethod]
        public void DefinitionInliningBinder_ReliableStream_AsObserver_Unbound()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Stream, Keys[0], ReliableStream },
            };

            var binder = new DefinitionInliningBinder(registry);
            var expression = Expression.Parameter(typeof(IObserver<int>), Keys[0]);
            Assert.AreSame(expression, binder.Bind(expression));
        }

        [TestMethod]
        public void DefinitionInliningBinder_ReliableStream_AsReliableObservable_Unbound()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Stream, Keys[0], ReliableStream },
            };

            var binder = new DefinitionInliningBinder(registry);
            var expression = Expression.Parameter(typeof(IReliableObservable<int>), Keys[0]);
            Assert.AreSame(expression, binder.Bind(expression));
        }

        [TestMethod]
        public void DefinitionInliningBinder_ReliableStream_AsReliableObserver_Unbound()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Stream, Keys[0], ReliableStream },
            };

            var binder = new DefinitionInliningBinder(registry);
            var expression = Expression.Parameter(typeof(IReliableObserver<int>), Keys[0]);
            Assert.AreSame(expression, binder.Bind(expression));
        }

        [TestMethod]
        public void DefinitionInliningBinder_UntypedStream_AsSubscribable_Unbound()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Stream, Keys[0], UntypedStream },
            };

            var binder = new DefinitionInliningBinder(registry);
            var expression = Expression.Parameter(typeof(ISubscribable<int>), Keys[0]);
            Assert.AreSame(expression, binder.Bind(expression));
        }

        [TestMethod]
        public void DefinitionInliningBinder_UntypedStream_AsObserver_Unbound()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Stream, Keys[0], UntypedStream },
            };

            var binder = new DefinitionInliningBinder(registry);
            var expression = Expression.Parameter(typeof(IObserver<int>), Keys[0]);
            Assert.AreSame(expression, binder.Bind(expression));
        }

        [TestMethod]
        public void DefinitionInliningBinder_UntypedStream_AsReliableObservable_Unbound()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Stream, Keys[0], UntypedStream },
            };

            var binder = new DefinitionInliningBinder(registry);
            var expression = Expression.Parameter(typeof(IReliableObservable<int>), Keys[0]);
            Assert.AreSame(expression, binder.Bind(expression));
        }

        [TestMethod]
        public void DefinitionInliningBinder_UntypedStream_AsReliableObserver_Unbound()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Stream, Keys[0], UntypedStream },
            };

            var binder = new DefinitionInliningBinder(registry);
            var expression = Expression.Parameter(typeof(IReliableObserver<int>), Keys[0]);
            Assert.AreSame(expression, binder.Bind(expression));
        }

        [TestMethod]
        public void DefinitionInliningBinder_Other_Unbound()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Other, Keys[0], OtherExpression },
            };

            var binder = new DefinitionInliningBinder(registry);
            var expression = Expression.Parameter(typeof(Func<string, string>), Keys[0]);
            Assert.AreSame(expression, binder.Bind(expression));
        }

        [TestMethod]
        public void DefinitionInliningBinder_Other_Bound()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Other, Keys[0], OtherExpression },
                { ReactiveEntityKind.Other, Keys[1], OtherMacro(Keys[0]) },
            };

            var binder = new DefinitionInliningBinder(registry);
            var expression = Expression.Parameter(typeof(Func<string, string>), Keys[1]);
            AssertEqual(Expression.Parameter(typeof(Func<string, string>), Keys[0]), binder.Bind(expression));
        }

        [TestMethod]
        public void DefinitionInliningBinder_Other_NonLambda_Unbound()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Other, Keys[0], OtherNonLambda },
            };

            var binder = new DefinitionInliningBinder(registry);
            var expression = Expression.Parameter(typeof(object), Keys[0]);
            Assert.AreSame(expression, binder.Bind(expression));
        }

        [TestMethod]
        public void DefinitionInliningBinder_Other_NonLambda_Bound()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Other, Keys[0], OtherNonLambda },
                { ReactiveEntityKind.Other, Keys[1], OtherNonLambdaMacro(Keys[0]) },
            };
            var binder = new DefinitionInliningBinder(registry);
            var expression = Expression.Parameter(typeof(object), Keys[1]);
            AssertEqual(Expression.Parameter(typeof(object), Keys[0]), binder.Bind(expression));
        }

        [TestMethod]
        public void DefinitionInliningBinder_Other_NotInRegistry()
        {
            var registry = new Registry
            {
            };

            var binder = new DefinitionInliningBinder(registry);
            var expression = Expression.Parameter(typeof(Func<string, string>), Keys[0]);
            Assert.AreSame(expression, binder.Bind(expression));
        }

        [TestMethod]
        public void DefinitionInliningBinder_Template_NoFreeVariables_NotUnpacked()
        {
            var templateId = "rx://template/1";
            var registry = new Registry
            {
                { ReactiveEntityKind.Template, templateId, TemplateExpressionNoFreeVars },
                { ReactiveEntityKind.Observable, Keys[0], TemplatizedObservableNoFreeVars(templateId) },
            };

            var binder = new DefinitionInliningBinder(registry);
            var expression = Expression.Parameter(typeof(ISubscribable<int>), Keys[0]);
            AssertEqual(TemplatizedObservableNoFreeVars(templateId), binder.Bind(expression));
        }

        [TestMethod]
        public void DefinitionInliningBinder_Template_FreeVariables_Unpacked()
        {
            var templateId = "rx://template/1";
            var registry = new Registry
            {
                { ReactiveEntityKind.Observable, Keys[0], ParameterizedSubscribable },
                { ReactiveEntityKind.Template, templateId, TemplateExpressionFreeVars(Keys[0]) },
                { ReactiveEntityKind.Observable, Keys[1], TemplatizedObservableFreeVars(templateId) },
            };

            var binder = new DefinitionInliningBinder(registry);
            var expression = Expression.Parameter(typeof(ISubscribable<int>), Keys[1]);
            var expected = Expression.Invoke(Expression.Parameter(typeof(Func<int, ISubscribable<int>>), Keys[0]), Expression.Constant(42));
            AssertEqual(expected, binder.Bind(expression));
        }

        [TestMethod]
        public void DefinitionInliningBinder_ReliableStream_ThrowsNotImplemented()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Stream, Keys[0], ReliableStream },
            };

            var binder = new DefinitionInliningBinder(registry);
            var expression = Expression.Parameter(typeof(IReliableMultiSubject<int, int>), Keys[0]);
            Assert.ThrowsException<NotImplementedException>(() => binder.Bind(expression));
        }

        [TestMethod]
        public void DefinitionInliningBinder_Subscription_ThrowsNotImplemented()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Subscription, Keys[0], Subscription },
            };

            var binder = new DefinitionInliningBinder(registry);
            var expression = Expression.Parameter(typeof(ISubscription), Keys[0]);
            Assert.ThrowsException<NotImplementedException>(() => binder.Bind(expression));
        }

        [TestMethod]
        public void DefinitionInliningBinder_ReliableSubscription_ThrowsNotImplemented()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.ReliableSubscription, Keys[0], ReliableSubscription },
            };

            var binder = new DefinitionInliningBinder(registry);
            var expression = Expression.Parameter(typeof(IReliableSubscription), Keys[0]);
            Assert.ThrowsException<NotImplementedException>(() => binder.Bind(expression));
        }

        [TestMethod]
        public void DefinitionInliningBinder_ForeignFunction()
        {
            var registry = new Registry();

            var toUpper = (Expression<Func<string, string>>)(s => s.ToUpper());

            var binder = new DefinitionInliningBinder(registry, id =>
            {
                if (id == "toUpper")
                {
                    return toUpper;
                }

                return null;
            });

            var e1 = Expression.Parameter(typeof(Func<string, string>), "toUpper");
            var b1 = binder.Bind(e1);
            Assert.AreSame(toUpper, b1);

            var e2 = Expression.Parameter(typeof(Func<string, string>), "toLower");
            var b2 = binder.Bind(e2);
            Assert.AreSame(e2, b2);
        }

        #endregion

        #region FullBinder Tests

        [TestMethod]
        public void FullBinder_Subscribable_Bound()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Observable, Keys[0], NullSubscribable },
            };

            var binder = new FullBinder(registry);
            var expression = Expression.Parameter(typeof(ISubscribable<int>), Keys[0]);
            Assert.AreSame(NullSubscribable, binder.Bind(expression));
        }

        [TestMethod]
        public void FullBinder_Subscribable_BoundRecursive()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Observable, Keys[0], NullSubscribable },
                { ReactiveEntityKind.Observable, Keys[1], SubscribableMacro(Keys[0]) },
            };

            var binder = new FullBinder(registry);
            var expression = Expression.Parameter(typeof(ISubscribable<int>), Keys[1]);
            Assert.AreSame(NullSubscribable, binder.Bind(expression));
        }

        [TestMethod]
        public void FullBinder_SubscribableImpl_Bound()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Observable, Keys[0], NullSubscribableImpl },
            };

            var binder = new FullBinder(registry);
            var expression = Expression.Parameter(typeof(ISubscribable<int>), Keys[0]);
            AssertEqual(Convert(NullSubscribableImpl, typeof(ISubscribable<int>)), binder.Bind(expression));
        }

        [TestMethod]
        public void FullBinder_Subscribable_NotInRegistry()
        {
            var registry = new Registry
            {
            };

            var binder = new FullBinder(registry);
            var expression = Expression.Parameter(typeof(ISubscribable<int>), Keys[0]);
            Assert.AreSame(expression, binder.Bind(expression));
        }

        [TestMethod]
        public void FullBinder_Subscribable_NotConvertible_ThrowsArgumentException()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Observable, Keys[0], NullObserver },
            };

            var binder = new FullBinder(registry);
            var expression = Expression.Parameter(typeof(ISubscribable<int>), Keys[0]);
            Assert.ThrowsException<ArgumentException>(() => binder.Bind(expression));
        }

        [TestMethod]
        public void FullBinder_Subscribable_Parameterized_Bound()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Observable, Keys[0], ParameterizedSubscribable },
            };

            var binder = new FullBinder(registry);
            var expression = Expression.Parameter(typeof(Func<int, ISubscribable<int>>), Keys[0]);
            Assert.AreSame(ParameterizedSubscribable, binder.Bind(expression));
        }

        [TestMethod]
        public void FullBinder_Subscribable_Parameterized_Derived_Bound()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Observable, Keys[0], DerivedParameterizedSubscribable },
            };

            var binder = new FullBinder(registry);
            var expression = Expression.Parameter(typeof(Func<int, ISubscribable<int>>), Keys[0]);
#pragma warning disable IDE0004 // Remove Unnecessary Cast. (Conversion expected after binding.)
            Expression<Func<int, ISubscribable<int>>> expected = x => (ISubscribable<int>)new DummySubscribable(x);
#pragma warning restore IDE0004
            AssertEqual(expected, binder.Bind(expression));
        }

        [TestMethod]
        public void FullBinder_Observer_Bound()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Observer, Keys[0], NullObserver },
            };

            var binder = new FullBinder(registry);
            var expression = Expression.Parameter(typeof(IObserver<int>), Keys[0]);
            Assert.AreSame(NullObserver, binder.Bind(expression));
        }

        [TestMethod]
        public void FullBinder_Observer_BoundRecursive()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Observer, Keys[0], NullObserver },
                { ReactiveEntityKind.Observer, Keys[1], ObserverMacro(Keys[0]) },
            };

            var binder = new FullBinder(registry);
            var expression = Expression.Parameter(typeof(IObserver<int>), Keys[1]);
            Assert.AreSame(NullObserver, binder.Bind(expression));
        }

        [TestMethod]
        public void FullBinder_ObserverImpl_Bound()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Observer, Keys[0], NullObserverImpl },
            };

            var binder = new FullBinder(registry);
            var expression = Expression.Parameter(typeof(IObserver<int>), Keys[0]);
            AssertEqual(Convert(NullObserverImpl, typeof(IObserver<int>)), binder.Bind(expression));
        }

        [TestMethod]
        public void FullBinder_Observer_NotInRegistry()
        {
            var registry = new Registry
            {
            };

            var binder = new FullBinder(registry);
            var expression = Expression.Parameter(typeof(IObserver<int>), Keys[0]);
            Assert.AreSame(expression, binder.Bind(expression));
        }

        [TestMethod]
        public void FullBinder_Observer_NotConvertible_ThrowsArgumentException()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Observer, Keys[0], NullSubscribable },
            };

            var binder = new FullBinder(registry);
            var expression = Expression.Parameter(typeof(IObserver<int>), Keys[0]);
            Assert.ThrowsException<ArgumentException>(() => binder.Bind(expression));
        }

        [TestMethod]
        public void FullBinder_ReliableObservable_Bound()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Observable, Keys[0], NullReliableObservable },
            };

            var binder = new FullBinder(registry);
            var expression = Expression.Parameter(typeof(IReliableObservable<int>), Keys[0]);
            Assert.AreSame(NullReliableObservable, binder.Bind(expression));
        }

        [TestMethod]
        public void FullBinder_ReliableObservable_BoundRecursive()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Observable, Keys[0], NullReliableObservable },
                { ReactiveEntityKind.Observable, Keys[1], ReliableObservableMacro(Keys[0]) },
            };

            var binder = new FullBinder(registry);
            var expression = Expression.Parameter(typeof(IReliableObservable<int>), Keys[1]);
            Assert.AreSame(NullReliableObservable, binder.Bind(expression));
        }

        [TestMethod]
        public void FullBinder_ReliableObservableImpl_Bound()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Observable, Keys[0], NullReliableObservableImpl },
            };

            var binder = new FullBinder(registry);
            var expression = Expression.Parameter(typeof(IReliableObservable<int>), Keys[0]);
            AssertEqual(Convert(NullReliableObservableImpl, typeof(IReliableObservable<int>)), binder.Bind(expression));
        }

        [TestMethod]
        public void FullBinder_ReliableObservable_NotInRegistry()
        {
            var registry = new Registry
            {
            };

            var binder = new FullBinder(registry);
            var expression = Expression.Parameter(typeof(IReliableObservable<int>), Keys[0]);
            Assert.AreSame(expression, binder.Bind(expression));
        }

        [TestMethod]
        public void FullBinder_ReliableObserver_Bound()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Observer, Keys[0], NullReliableObserver },
            };

            var binder = new FullBinder(registry);
            var expression = Expression.Parameter(typeof(IReliableObserver<int>), Keys[0]);
            Assert.AreSame(NullReliableObserver, binder.Bind(expression));
        }

        [TestMethod]
        public void FullBinder_ReliableObserver_BoundRecursive()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Observer, Keys[0], NullReliableObserver },
                { ReactiveEntityKind.Observer, Keys[1], ReliableObserverMacro(Keys[0]) },
            };

            var binder = new FullBinder(registry);
            var expression = Expression.Parameter(typeof(IReliableObserver<int>), Keys[1]);
            Assert.AreSame(NullReliableObserver, binder.Bind(expression));
        }

        [TestMethod]
        public void FullBinder_ReliableObserverImpl_Bound()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Observer, Keys[0], NullReliableObserverImpl },
            };

            var binder = new FullBinder(registry);
            var expression = Expression.Parameter(typeof(IReliableObserver<int>), Keys[0]);
            AssertEqual(Convert(NullReliableObserverImpl, typeof(IReliableObserver<int>)), binder.Bind(expression));
        }

        [TestMethod]
        public void FullBinder_ReliableObserver_NotInRegistry()
        {
            var registry = new Registry
            {
            };

            var binder = new FullBinder(registry);
            var expression = Expression.Parameter(typeof(IReliableObserver<int>), Keys[0]);
            Assert.AreSame(expression, binder.Bind(expression));
        }

        [TestMethod]
        public void FullBinder_ReliableStreamFactory_NotInRegistry()
        {
            var registry = new Registry
            {
            };

            var binder = new FullBinder(registry);
            var expression = Expression.Parameter(typeof(Func<IReliableMultiSubject<int, int>>), Keys[0]);
            Assert.AreSame(expression, binder.Bind(expression));
        }

        [TestMethod]
        public void FullBinder_ReliableStreamFactory_Func_Bound()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.StreamFactory, Keys[0], FuncReliableStreamFactory },
            };

            var binder = new FullBinder(registry);
            var expression = Expression.Parameter(typeof(Func<IReliableMultiSubject<int, int>>), Keys[0]);
            Assert.AreSame(FuncReliableStreamFactory, binder.Bind(expression));
        }

        [TestMethod]
        public void FullBinder_ReliableStreamFactory_Func_BoundRecursive()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.StreamFactory, Keys[0], FuncReliableStreamFactory },
                { ReactiveEntityKind.StreamFactory, Keys[1], ReliableStreamFactoryMacro(Keys[0]) },
            };

            var binder = new FullBinder(registry);
            var expression = Expression.Parameter(typeof(Func<IReliableMultiSubject<int, int>>), Keys[1]);
            Assert.AreSame(FuncReliableStreamFactory, binder.Bind(expression));
        }

        [TestMethod]
        public void FullBinder_ReliableStreamFactory_Proper_Bound()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.StreamFactory, Keys[0], ProperReliableStreamFactory },
            };

            var binder = new FullBinder(registry);
            var expression = Expression.Parameter(typeof(Func<IReliableMultiSubject<int, int>>), Keys[0]);
            Expression<Func<DummyReliableSubjectFactory, IReliableMultiSubject<int, int>>> factory = sf => sf.Create();
            var expected = BetaReducer.ReduceEager(Expression.Lambda(Expression.Invoke(factory, ProperReliableStreamFactory)), BetaReductionNodeTypes.Unrestricted, BetaReductionRestrictions.None, throwOnCycle: true);
            AssertEqual(expected, binder.Bind(expression));
        }

        [TestMethod]
        public void FullBinder_ReliableStreamFactory_Proper_BoundRecursive()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.StreamFactory, Keys[0], ProperReliableStreamFactory },
                { ReactiveEntityKind.StreamFactory, Keys[1], ReliableStreamFactoryMacro(Keys[0]) },
            };

            var binder = new FullBinder(registry);
            var expression = Expression.Parameter(typeof(Func<IReliableMultiSubject<int, int>>), Keys[0]);
            Expression<Func<DummyReliableSubjectFactory, IReliableMultiSubject<int, int>>> factory = sf => sf.Create();
            var expected = BetaReducer.ReduceEager(Expression.Lambda(Expression.Invoke(factory, ProperReliableStreamFactory)), BetaReductionNodeTypes.Unrestricted, BetaReductionRestrictions.None, throwOnCycle: true);
            AssertEqual(expected, binder.Bind(expression));
        }

        [TestMethod]
        public void FullBinder_UntypedStreamFactory_NotInRegistry()
        {
            var registry = new Registry
            {
            };

            var binder = new FullBinder(registry);
            var expression = Expression.Parameter(typeof(Func<IMultiSubject>), Keys[0]);
            Assert.AreSame(expression, binder.Bind(expression));
        }

        [TestMethod]
        public void FullBinder_UntypedStreamFactory_Func_Unbound()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.StreamFactory, Keys[0], FuncUntypedStreamFactory },
            };

            var binder = new FullBinder(registry);
            var expression = Expression.Parameter(typeof(Func<IMultiSubject>), Keys[0]);
            Assert.AreSame(FuncUntypedStreamFactory, binder.Bind(expression));
        }

        [TestMethod]
        public void FullBinder_UntypedStreamFactory_Func_Bound()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.StreamFactory, Keys[0], FuncUntypedStreamFactory },
                { ReactiveEntityKind.StreamFactory, Keys[1], UntypedStreamFactoryMacro(Keys[0]) },
            };

            var binder = new FullBinder(registry);
            var expression = Expression.Parameter(typeof(Func<IMultiSubject>), Keys[1]);
            Assert.AreSame(FuncUntypedStreamFactory, binder.Bind(expression));
        }

        [TestMethod]
        public void FullBinder_ReliableStream_AsSubscribable_Bound()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Stream, Keys[0], ReliableStream },
            };

            var binder = new FullBinder(registry);
            var expression = Expression.Parameter(typeof(ISubscribable<int>), Keys[0]);
            Assert.IsTrue(registry.Subjects.TryGetValue(Keys[0], out var entity));
            Expression<Func<string, ISubscribable<int>>> toSubscribable = s => new ReliableMultiSubjectProxy<int, int>(new Uri(s)).ToSubscribable();
            var expected = BetaReducer.Reduce(Expression.Invoke(toSubscribable, Expression.Constant(entity.Uri.ToCanonicalString())));
            AssertEqual(expected, binder.Bind(expression));
        }

        [TestMethod]
        public void FullBinder_ReliableStream_AsObserver_Bound()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Stream, Keys[0], ReliableStream },
            };

            var binder = new FullBinder(registry);
            var expression = Expression.Parameter(typeof(IObserver<int>), Keys[0]);
            Assert.IsTrue(registry.Subjects.TryGetValue(Keys[0], out var entity));
            Expression<Func<string, IObserver<int>>> toObserver = s => new ObserverToReliableObserver<int>(new ReliableMultiSubjectProxy<int, int>(new Uri(s)).ToReliableObserver());
            var expected = BetaReducer.Reduce(Expression.Invoke(toObserver, Expression.Constant(entity.Uri.ToCanonicalString())));
            AssertEqual(Convert(expected, typeof(IObserver<int>)), binder.Bind(expression));
        }

        [TestMethod]
        public void FullBinder_ReliableStream_AsReliableObservable_Bound()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Stream, Keys[0], ReliableStream },
            };

            var binder = new FullBinder(registry);
            var expression = Expression.Parameter(typeof(IReliableObservable<int>), Keys[0]);
            Assert.IsTrue(registry.Subjects.TryGetValue(Keys[0], out var entity));
            Expression<Func<string, IReliableObservable<int>>> toProxy = s => new ReliableMultiSubjectProxy<int, int>(new Uri(s));
            var expected = BetaReducer.Reduce(Expression.Invoke(toProxy, Expression.Constant(entity.Uri.ToCanonicalString())));
            AssertEqual(Convert(expected, typeof(IReliableObservable<int>)), binder.Bind(expression));
        }

        [TestMethod]
        public void FullBinder_ReliableStream_AsReliableObserver_Bound()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Stream, Keys[0], ReliableStream },
            };

            var binder = new FullBinder(registry);
            var expression = Expression.Parameter(typeof(IReliableObserver<int>), Keys[0]);
            Assert.IsTrue(registry.Subjects.TryGetValue(Keys[0], out var entity));
            Expression<Func<string, IReliableObserver<int>>> toProxy = s => new ReliableMultiSubjectProxy<int, int>(new Uri(s)).ToReliableObserver();
            var expected = BetaReducer.Reduce(Expression.Invoke(toProxy, Expression.Constant(entity.Uri.ToCanonicalString())));
            AssertEqual(expected, binder.Bind(expression));
        }

        [TestMethod]
        public void FullBinder_UntypedStream_AsSubscribable_Bound()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Stream, Keys[0], UntypedStream },
            };

            var binder = new FullBinder(registry);
            var expression = Expression.Parameter(typeof(ISubscribable<int>), Keys[0]);
            Assert.IsTrue(registry.Subjects.TryGetValue(Keys[0], out var entity));
            Expression<Func<string, IMultiSubject>> toProxy = s => new MultiSubjectProxy(new Uri(s));
            Expression<Func<IMultiSubject, ISubscribable<int>>> toObservable = s => s.GetObservable<int>();
            var expected = BetaReducer.ReduceEager(
                Expression.Invoke(toObservable, Expression.Invoke(toProxy, Expression.Constant(entity.Uri.ToCanonicalString()))),
                BetaReductionNodeTypes.Unrestricted,
                BetaReductionRestrictions.None,
                true);
            AssertEqual(expected, binder.Bind(expression));
        }

        [TestMethod]
        public void FullBinder_UntypedStream_AsObserver_Bound()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Stream, Keys[0], UntypedStream },
            };

            var binder = new FullBinder(registry);
            var expression = Expression.Parameter(typeof(IObserver<int>), Keys[0]);
            Assert.IsTrue(registry.Subjects.TryGetValue(Keys[0], out var entity));
            Expression<Func<string, IMultiSubject>> toProxy = s => new MultiSubjectProxy(new Uri(s));
            Expression<Func<IMultiSubject, IObserver<int>>> toObserver = s => s.GetObserver<int>();
            var expected = BetaReducer.ReduceEager(
                Expression.Invoke(toObserver, Expression.Invoke(toProxy, Expression.Constant(entity.Uri.ToCanonicalString()))),
                BetaReductionNodeTypes.Unrestricted,
                BetaReductionRestrictions.None,
                true);
            AssertEqual(expected, binder.Bind(expression));
        }

        [TestMethod]
        public void FullBinder_UntypedStream_AsReliableObservable_ThrowsArgumentException()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Stream, Keys[0], UntypedStream },
            };

            var binder = new FullBinder(registry);
            var expression = Expression.Parameter(typeof(IReliableObservable<int>), Keys[0]);
            Assert.ThrowsException<ArgumentException>(() => binder.Bind(expression));
        }

        [TestMethod]
        public void FullBinder_UntypedStream_AsReliableObserver_ThrowsArgumentException()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Stream, Keys[0], UntypedStream },
            };

            var binder = new FullBinder(registry);
            var expression = Expression.Parameter(typeof(IReliableObserver<int>), Keys[0]);
            Assert.ThrowsException<ArgumentException>(() => binder.Bind(expression));
        }

        [TestMethod]
        public void FullBinder_Other_Bound()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Other, Keys[0], OtherExpression },
            };

            var binder = new FullBinder(registry);
            var expression = Expression.Parameter(typeof(Func<string, string>), Keys[0]);
            Assert.AreSame(OtherExpression, binder.Bind(expression));
        }

        [TestMethod]
        public void FullBinder_Other_BoundRecursive()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Other, Keys[0], OtherExpression },
                { ReactiveEntityKind.Other, Keys[1], OtherMacro(Keys[0]) },
            };

            var binder = new FullBinder(registry);
            var expression = Expression.Parameter(typeof(Func<string, string>), Keys[1]);
            Assert.AreSame(OtherExpression, binder.Bind(expression));
        }

        [TestMethod]
        public void FullBinder_Other_NotInRegistry()
        {
            var registry = new Registry
            {
            };

            var binder = new FullBinder(registry);
            var expression = Expression.Parameter(typeof(Func<string, string>), Keys[0]);
            Assert.AreSame(expression, binder.Bind(expression));
        }

        [TestMethod]
        public void FullBinder_ReliableStream_ThrowsNotImplemented()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Stream, Keys[0], ReliableStream },
            };

            var binder = new FullBinder(registry);
            var expression = Expression.Parameter(typeof(IReliableMultiSubject<int, int>), Keys[0]);
            Assert.ThrowsException<NotImplementedException>(() => binder.Bind(expression));
        }

        [TestMethod]
        public void FullBinder_Subscription_ThrowsNotImplemented()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Subscription, Keys[0], Subscription },
            };

            var binder = new FullBinder(registry);
            var expression = Expression.Parameter(typeof(ISubscription), Keys[0]);
            Assert.ThrowsException<NotImplementedException>(() => binder.Bind(expression));
        }

        [TestMethod]
        public void FullBinder_ReliableSubscription_ThrowsNotImplemented()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.ReliableSubscription, Keys[0], ReliableSubscription },
            };

            var binder = new FullBinder(registry);
            var expression = Expression.Parameter(typeof(IReliableSubscription), Keys[0]);
            Assert.ThrowsException<NotImplementedException>(() => binder.Bind(expression));
        }

        [TestMethod]
        public void FullBinder_StructurallyEquivalent_Bound()
        {
            var rt1 = RuntimeCompiler.CreateRecordType(new Dictionary<string, Type> { { "Foo", typeof(int) } }, valueEquality: true);
            var rt2 = RuntimeCompiler.CreateRecordType(new Dictionary<string, Type> { { "Foo", typeof(int) } }, valueEquality: true);
            var st1 = typeof(ISubscribable<>).MakeGenericType(rt1);
            var st2 = typeof(ISubscribable<>).MakeGenericType(rt2);

            Expression underlyingExpression = Expression.Default(st1);
            var registry = new Registry
            {
                { ReactiveEntityKind.Observable, Keys[0], underlyingExpression },
            };

            var binder = new FullBinder(registry);
            var expression = Expression.Parameter(st2, Keys[0]);
            var expected = Expression.Default(st2);
            AssertEqual(expected, binder.Bind(expression));
        }

        [TestMethod]
        public void FullBinder_NotStructurallyEquivalent_ThrowsInvalidOperation()
        {
            var rt1 = RuntimeCompiler.CreateRecordType(new Dictionary<string, Type> { { "Foo", typeof(int) } }, valueEquality: true);
            var rt2 = RuntimeCompiler.CreateRecordType(new Dictionary<string, Type> { { "Bar", typeof(int) } }, valueEquality: true);
            var st1 = typeof(ISubscribable<>).MakeGenericType(rt1);
            var st2 = typeof(ISubscribable<>).MakeGenericType(rt2);

            Expression underlyingExpression = Expression.Default(st1);
            var registry = new Registry
            {
                { ReactiveEntityKind.Observable, Keys[0], underlyingExpression },
            };

            var binder = new FullBinder(registry);
            var expression = Expression.Parameter(st2, Keys[0]);
            var expected = Expression.Default(st2);
            Assert.ThrowsException<InvalidOperationException>(() => binder.Bind(expression));
        }


        [TestMethod]
        public void FullBinder_Wildcard_Bound()
        {
            Expression underlyingExpression = Expression.Default(typeof(ISubscribable<T>));
            var registry = new Registry
            {
                { ReactiveEntityKind.Observable, Keys[0], underlyingExpression },
            };

            var binder = new FullBinder(registry);
            var expression = Expression.Parameter(typeof(ISubscribable<int>), Keys[0]);
            var expected = Expression.Default(typeof(ISubscribable<int>));
            AssertEqual(expected, binder.Bind(expression));
        }

        [TestMethod]
        public void FullBinder_ToObserver_Ordering_ReliableMultiSubjectBeforeUntypedMultiSubject()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Stream, Keys[0], Expression.New(typeof(DummyReliableMultiSubjectAndUntypedMultiSubject)) },
            };

            var binder = new FullBinder(registry);
            var expression = Expression.Parameter(typeof(IObserver<int>), Keys[0]);
            Assert.IsTrue(registry.Subjects.TryGetValue(Keys[0], out var entity));
            Expression<Func<string, IObserver<int>>> toObserver = s => new ObserverToReliableObserver<int>(new ReliableMultiSubjectProxy<int, int>(new Uri(s)).ToReliableObserver());
            var expected = BetaReducer.Reduce(Expression.Invoke(toObserver, Expression.Constant(entity.Uri.ToCanonicalString())));
            AssertEqual(Convert(expected, typeof(IObserver<int>)), binder.Bind(expression));
        }

        [TestMethod]
        public void FullBinder_ToSubscribable_Ordering_ReliableMultiSubjectBeforeUntypedMultiSubject()
        {
            var registry = new Registry
            {
                { ReactiveEntityKind.Stream, Keys[0], Expression.New(typeof(DummyReliableMultiSubjectAndUntypedMultiSubject)) },
            };

            var binder = new FullBinder(registry);
            var expression = Expression.Parameter(typeof(ISubscribable<int>), Keys[0]);
            Assert.IsTrue(registry.Subjects.TryGetValue(Keys[0], out var entity));
            Expression<Func<string, ISubscribable<int>>> toSubscribable = s => new ReliableMultiSubjectProxy<int, int>(new Uri(s)).ToSubscribable();
            var expected = BetaReducer.Reduce(Expression.Invoke(toSubscribable, Expression.Constant(entity.Uri.ToCanonicalString())));
            AssertEqual(expected, binder.Bind(expression));
        }

        #endregion

        #region DelegationBinder Tests

        [TestMethod]
        public void DelegationBinder_Simple()
        {
            var s0 = Expression.Parameter(typeof(ISubscribable<int>), Keys[0]);
            var s1 = Expression.Parameter(typeof(ISubscribable<int>), Keys[1]);
            var s2 = Expression.Parameter(typeof(ISubscribable<int>), Keys[2]);
            var delegatedSubject = Expression.Default(typeof(SimpleDelegationTarget<int, int>));
            var id = Expression.Parameter(typeof(Func<ISubscribable<int>, ISubscribable<int>>), "id");
            var bin = Expression.Parameter(typeof(Func<ISubscribable<int>, ISubscribable<int>, ISubscribable<int>>), "bin");
            var binp = Expression.Parameter(typeof(Func<ISubscribable<int>, ISubscribable<int>, ISubscribable<int>>), "bin'");

            var registry = new Registry
            {
                { ReactiveEntityKind.Stream, Keys[0], Expression.New(typeof(SimpleDelegationTarget<int, int>).GetConstructors()[0], Expression.Constant((Func<ParameterExpression, Expression, Expression>)((p, e) => BetaReducer.Reduce(Expression.Invoke(Expression.Lambda(e, p), Expression.Default(typeof(SimpleDelegationTarget<int, int>))))), typeof(Func<ParameterExpression, Expression, Expression>))) },
                { ReactiveEntityKind.Stream, Keys[1], Expression.New(typeof(SimpleDelegationTarget<int, int>).GetConstructors()[0], Expression.Constant((Func<ParameterExpression, Expression, Expression>)((p, e) => null), typeof(Func<ParameterExpression, Expression, Expression>))) },
                { ReactiveEntityKind.Stream, Keys[2], Expression.New(typeof(SimpleDelegationTarget<int, int>).GetConstructors()[0], Expression.Constant((Func<ParameterExpression, Expression, Expression>)((p, e) =>
                    {
                        if (e.NodeType == ExpressionType.Invoke)
                        {
                            var i = (InvocationExpression)e;
                            if (i.Expression.NodeType == ExpressionType.Parameter)
                            {
                                var pm = (ParameterExpression)i.Expression;
                                if (pm.Name == "id")
                                {
                                    return BetaReducer.Reduce(Expression.Invoke(Expression.Lambda(e, p), Expression.Default(typeof(SimpleDelegationTarget<int, int>))));
                                }
                                else if (pm.Name == "bin")
                                {
                                    return BetaReducer.Reduce(Expression.Invoke(Expression.Lambda(e, p, pm), Expression.Default(typeof(SimpleDelegationTarget<int, int>)), binp));
                                }
                            }
                        }

                        return null;
                    }), typeof(Func<ParameterExpression, Expression, Expression>))) },
            };

            {
                var binder = new DelegationBinder(registry);
                // id(subj0) -> id(subj0')
                var expression = Expression.Invoke(id, s0);
                AssertEqual(Expression.Invoke(id, delegatedSubject), binder.Bind(expression));
            }

            {
                var binder = new DelegationBinder(registry);
                // id(subj1) -> id(subj1)
                var expression = Expression.Invoke(id, s1);
                AssertEqual(expression, binder.Bind(expression));
            }

            {
                var binder = new DelegationBinder(registry);
                // id(subj2) -> id(subj2')
                var expression = Expression.Invoke(id, s2);
                AssertEqual(Expression.Invoke(id, delegatedSubject), binder.Bind(expression));
            }

            {
                var binder = new DelegationBinder(registry);
                // bin(subj2, null) -> bin'(subj2', null)
                var expression = Expression.Invoke(bin, s2, delegatedSubject);
                AssertEqual(Expression.Invoke(binp, delegatedSubject, delegatedSubject), binder.Bind(expression));
            }

            {
                var binder = new DelegationBinder(registry);
                // bin(subj2, id(subj2)) -> bin(subj2, id(subj2'))
                var expression = Expression.Invoke(bin, s2, Expression.Invoke(id, s2));
                AssertEqual(Expression.Invoke(bin, s2, Expression.Invoke(id, delegatedSubject)), binder.Bind(expression));
            }

            {
                var binder = new DelegationBinder(registry);
                // bin(subj2, subj2) -> bin(subj2, subj2)
                var expression = Expression.Invoke(bin, s2, s2);
                AssertEqual(Expression.Invoke(bin, s2, s2), binder.Bind(expression));
            }

            {
                var binder = new DelegationBinder(registry);
                // bin(subj2, id(null)) -> bin(subj2', id(null))
                var expression = Expression.Invoke(bin, s2, Expression.Invoke(id, Expression.Default(typeof(ISubscribable<int>))));
                AssertEqual(Expression.Invoke(binp, delegatedSubject, Expression.Invoke(id, Expression.Default(typeof(ISubscribable<int>)))), binder.Bind(expression));
            }

            {
                var binder = new DelegationBinder(registry);
                // bin(bin(subj2, null), null) -> bin'(bin'(subj2', null), null)
                var expression = Expression.Invoke(bin, Expression.Invoke(bin, s2, Expression.Default(typeof(ISubscribable<int>))), Expression.Default(typeof(ISubscribable<int>)));
                var expected = Expression.Invoke(binp, Expression.Invoke(binp, delegatedSubject, Expression.Default(typeof(ISubscribable<int>))), Expression.Default(typeof(ISubscribable<int>)));
                AssertEqual(expected, binder.Bind(expression));
            }

            {
                var binder = new DelegationBinder(registry);
                // bin'(bin(subj2, null), null) -> bin'(bin'(subj2', null), null)
                var expression = Expression.Invoke(binp, Expression.Invoke(bin, s2, Expression.Default(typeof(ISubscribable<int>))), Expression.Default(typeof(ISubscribable<int>)));
                var expected = Expression.Invoke(binp, Expression.Invoke(binp, delegatedSubject, Expression.Default(typeof(ISubscribable<int>))), Expression.Default(typeof(ISubscribable<int>)));
                AssertEqual(expected, binder.Bind(expression));
            }

            {
                var binder = new DelegationBinder(registry);
                // bin(id(subj2), id(subj2)) -> bin(id(subj2'), id(subj2'))
                var expression = Expression.Invoke(bin, Expression.Invoke(id, s2), Expression.Invoke(id, s2));
                var expected = Expression.Invoke(bin, Expression.Invoke(id, delegatedSubject), Expression.Invoke(id, delegatedSubject));
                AssertEqual(expected, binder.Bind(expression));
            }

            {
                var binder = new DelegationBinder(registry);
                // id(bin(id(subj2), id(subj2))) -> id(bin(id(subj2'), id(subj2')))
                var expression = Expression.Invoke(id, Expression.Invoke(bin, Expression.Invoke(id, s2), Expression.Invoke(id, s2)));
                var expected = Expression.Invoke(id, Expression.Invoke(bin, Expression.Invoke(id, delegatedSubject), Expression.Invoke(id, delegatedSubject)));
                AssertEqual(expected, binder.Bind(expression));
            }

            {
                var binder = new DelegationBinder(registry);
                // () => id(subj0) -> () => id(subj0')
                var expression = Expression.Lambda(Expression.Invoke(id, s0));
                var expected = Expression.Lambda(Expression.Invoke(id, delegatedSubject));
                AssertEqual(expected, binder.Bind(expression));
            }

            {
                var binder = new DelegationBinder(registry);
                // subj0 => id(subj0) -> subj0 => id(subj0)
                var expression = Expression.Lambda(Expression.Invoke(id, s0), s0);
                var expected = Expression.Lambda(Expression.Invoke(id, s0), s0);
                AssertEqual(expected, binder.Bind(expression));
            }
        }

        private class SimpleDelegationTarget<T, R> : IMultiSubject<T, R>, IDelegationTarget, IExpressible
        {
            private readonly Func<ParameterExpression, Expression, Expression> _delegationFunc;

            public SimpleDelegationTarget(Func<ParameterExpression, Expression, Expression> delegationFunc)
            {
                _delegationFunc = delegationFunc;
            }

            public bool CanDelegate(ParameterExpression node, Expression expression)
            {
                return _delegationFunc(node, expression) != null;
            }

            public Expression Delegate(ParameterExpression node, Expression expression)
            {
                return _delegationFunc(node, expression);
            }

            public IObserver<T> CreateObserver()
            {
                throw new NotImplementedException();
            }

            public ISubscription Subscribe(IObserver<R> observer)
            {
                throw new NotImplementedException();
            }

            IDisposable IObservable<R>.Subscribe(IObserver<R> observer)
            {
                throw new NotImplementedException();
            }

            public void Dispose()
            {
                throw new NotImplementedException();
            }

            public Expression Expression => Expression.Default(GetType());
        }

        #endregion

        #region Test Helpers

        private sealed class Registry : MockQueryEngineRegistry, IEnumerable
        {
            private readonly MockReactiveEngineProvider _provider;

            public Registry()
            {
                _provider = new MockReactiveEngineProvider(this);
            }

            public void Add(ReactiveEntityKind kind, string id, Expression expression)
            {
                var uri = new Uri(id);

                switch (kind)
                {
                    case ReactiveEntityKind.Observable:
                        _provider.DefineObservable(uri, expression, null);
                        break;
                    case ReactiveEntityKind.Observer:
                        _provider.DefineObserver(uri, expression, null);
                        break;
                    case ReactiveEntityKind.Stream:
                        _provider.CreateStream(uri, expression, null);
                        break;
                    case ReactiveEntityKind.StreamFactory:
                        _provider.DefineStreamFactory(uri, expression, null);
                        break;
                    case ReactiveEntityKind.Subscription:
                        _provider.CreateSubscription(uri, expression, null);
                        break;
                    case ReactiveEntityKind.Template:
                        Assert.IsTrue(Templates.TryAdd(id, new OtherDefinitionEntity(uri, expression, null)));
                        break;
                    case ReactiveEntityKind.Other:
                        Assert.IsTrue(Other.TryAdd(id, new OtherDefinitionEntity(uri, expression, null)));
                        break;
                    case ReactiveEntityKind.ReliableSubscription:
                        Assert.IsTrue(ReliableSubscriptions.TryAdd(id, new ReliableSubscriptionEntity(uri, expression, null)));
                        break;
                    case ReactiveEntityKind.None:
                    default:
                        throw new NotSupportedException();
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                // Only implemented IEnumerable to support list init expressions.
                throw new NotImplementedException();
            }
        }

        private static void AssertEqual(Expression x, Expression y)
        {
            Assert.IsTrue(new ExpressionEqualityComparer(() => new Comparator()).Equals(x, y));
        }

        private sealed class Comparator : ExpressionEqualityComparator
        {
            protected override bool EqualsGlobalParameter(ParameterExpression x, ParameterExpression y)
            {
                return x.Name == y.Name && Equals(x.Type, y.Type);
            }
        }

        #endregion

        #region Dummy Implementations

        private class DummyReliableSubject : IReliableMultiSubject<int, int>
        {
            public IReliableObserver<int> CreateObserver()
            {
                throw new NotImplementedException();
            }

            public void Dispose()
            {
                throw new NotImplementedException();
            }

            public IReliableSubscription Subscribe(IReliableObserver<int> observer)
            {
                throw new NotImplementedException();
            }
        }

        private sealed class DummyUntypedSubject : IMultiSubject
        {
            public IObserver<T> GetObserver<T>()
            {
                throw new NotImplementedException();
            }

            public ISubscribable<T> GetObservable<T>()
            {
                throw new NotImplementedException();
            }

            public void Dispose()
            {
                throw new NotImplementedException();
            }
        }

        private sealed class DummyReliableSubjectFactory : IReliableSubjectFactory<int, int>
        {
            public IReliableMultiSubject<int, int> Create()
            {
                return new DummyReliableSubject();
            }
        }

        private class DummySubscribable : ISubscribable<int>
        {
            public DummySubscribable(int x)
            {
                _ = x;
            }

            public ISubscription Subscribe(IObserver<int> observer)
            {
                throw new NotImplementedException();
            }

            IDisposable IObservable<int>.Subscribe(IObserver<int> observer)
            {
                throw new NotImplementedException();
            }
        }

        private class DummyObserver : IObserver<int>
        {
            public void OnCompleted()
            {
                throw new NotImplementedException();
            }

            public void OnError(Exception error)
            {
                throw new NotImplementedException();
            }

            public void OnNext(int value)
            {
                throw new NotImplementedException();
            }
        }

        private sealed class DummyReliableObservable : IReliableObservable<int>
        {
            public IReliableSubscription Subscribe(IReliableObserver<int> observer)
            {
                throw new NotImplementedException();
            }
        }

        private sealed class DummyReliableObserver : IReliableObserver<int>
        {
            public Uri ResubscribeUri => throw new NotImplementedException();

            public void OnNext(int item, long sequenceId)
            {
                throw new NotImplementedException();
            }

            public void OnStarted()
            {
                throw new NotImplementedException();
            }

            public void OnError(Exception error)
            {
                throw new NotImplementedException();
            }

            public void OnCompleted()
            {
                throw new NotImplementedException();
            }
        }

        private sealed class DummySubscription : ISubscription
        {
            public void Accept(ISubscriptionVisitor visitor)
            {
                throw new NotImplementedException();
            }

            public void Dispose()
            {
                throw new NotImplementedException();
            }
        }

        private sealed class DummyReliableSubscription : IReliableSubscription
        {
            public Uri ResubscribeUri => throw new NotImplementedException();

            public void Start(long sequenceId)
            {
                throw new NotImplementedException();
            }

            public void AcknowledgeRange(long sequenceId)
            {
                throw new NotImplementedException();
            }

            public void Accept(ISubscriptionVisitor visitor)
            {
                throw new NotImplementedException();
            }

            public void Dispose()
            {
                throw new NotImplementedException();
            }
        }

        private sealed class DummyObserverAndReliableMultiSubject : DummyObserver, IReliableMultiSubject<int, int>
        {
            public IReliableObserver<int> CreateObserver()
            {
                throw new NotImplementedException();
            }

            public void Dispose()
            {
                throw new NotImplementedException();
            }

            public IReliableSubscription Subscribe(IReliableObserver<int> observer)
            {
                throw new NotImplementedException();
            }
        }

        private sealed class DummySubscribableAndReliableMultiSubject : DummySubscribable, IReliableMultiSubject<int, int>
        {
            public DummySubscribableAndReliableMultiSubject()
                : base(0)
            {
            }

            public IReliableObserver<int> CreateObserver()
            {
                throw new NotImplementedException();
            }

            public void Dispose()
            {
                throw new NotImplementedException();
            }

            public IReliableSubscription Subscribe(IReliableObserver<int> observer)
            {
                throw new NotImplementedException();
            }
        }

        private sealed class DummyReliableMultiSubjectAndUntypedMultiSubject : DummyReliableSubject, IMultiSubject
        {
            public IObserver<T> GetObserver<T>()
            {
                throw new NotImplementedException();
            }

            public ISubscribable<T> GetObservable<T>()
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
