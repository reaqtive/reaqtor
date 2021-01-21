// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;

using Nuqleon.DataModel;

using Reaqtive;
using Reaqtive.Scheduler;
using Reaqtive.Testing;

using Reaqtor.Hosting.Shared.Serialization;
using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting.TestingFramework
{
    [KnownType]
    public sealed class TestObserver<T> : IObserver<T>, IOperator, ISubscription
    {
        #region Constructors & Fields

        private readonly SerializationHelpers _helpers;
        private readonly Uri _name;

        private TestObserverStoreConnection _observerStore;

        public TestObserver(Uri name)
        {
            _name = name;
            _helpers = new SerializationHelpers();
        }

        #endregion

        #region Properties

        private Impl Recorder
        {
            get
            {
                if (!_observerStore.TryGetValue(_name.ToCanonicalString(), out var recorder))
                {
                    recorder = new Impl();
                    _observerStore.TryAdd(_name.ToCanonicalString(), recorder);
                }
                return (Impl)recorder;
            }
        }

        #endregion

        #region IObserver

        public void OnCompleted()
        {
            Recorder.OnCompleted();
        }

        public void OnError(Exception error)
        {
            Recorder.OnError(error);
        }

        public void OnNext(T value)
        {
            Recorder.OnNext(_helpers.Serialize<T>(value));
        }

        #endregion

        #region ISubscription

        public void Accept(ISubscriptionVisitor visitor)
        {
            visitor.Visit(this);
        }

        #endregion

        #region IOperator

        public void Subscribe() { }

        public void SetContext(IOperatorContext context)
        {
            context.TryGetElement<TestObserverStoreConnection>(
                TestObserverStoreConnection.ContextHandle,
                out _observerStore);
            Debug.Assert(_observerStore != null);
            Recorder.Scheduler = context.Scheduler;
        }

        public IEnumerable<ISubscription> Inputs => Array.Empty<ISubscription>();

        public void Start()
        {
        }

        public void Dispose()
        {
        }

        #endregion

        #region ITestObserver<string>

        private sealed class Impl : MarshalByRefObject, ITestObserver<string>
        {
            private readonly List<Recorded<INotification<string>>> _messages;

            public Impl()
            {
                _messages = new List<Recorded<INotification<string>>>();
            }

            public IScheduler Scheduler
            {
                get;
                set;
            }

            public IList<Recorded<INotification<string>>> Messages => _messages;

            private long Now => Scheduler != null ? Scheduler.Now.Ticks : -1L;

            public void OnCompleted()
            {
                _messages.Add(ObserverMessage.OnCompleted<string>(Now));
            }

            public void OnError(Exception error)
            {
                _messages.Add(ObserverMessage.OnError<string>(Now, error));
            }

            public void OnNext(string value)
            {
                _messages.Add(ObserverMessage.OnNext<string>(Now, value));
            }

            public override object InitializeLifetimeService() => null;
        }

        #endregion
    }
}
