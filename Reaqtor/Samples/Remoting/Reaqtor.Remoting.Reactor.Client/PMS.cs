// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Linq.CompilerServices;
using System.Reflection;

using Reaqtive;

using Reaqtor.Hosting.Shared.Serialization;
using Reaqtor.Remoting.Deployable.Streams;
using Reaqtor.Remoting.Platform;
using Reaqtor.Remoting.Protocol;

namespace Reaqtor.Remoting
{
    internal class PMS : PartitionedMultiSubject
    {
        private Uri _uri;
        private Manager _manager;

        public override void SetContext(IOperatorContext context)
        {
            base.SetContext(context);

            _uri = context.InstanceId;
            _manager = new Manager(this);
        }

        public override string Name => "pms";

        public override Version Version => new(1, 0, 0, 0);

        protected override void OnStart() => base.OnStart();

        protected override IObserver<T> GetObserverCoreCore<T>() => new WrappedObserver<T>(this);

        private sealed class WrappedObserver<T> : IObserver<T>
        {
            private readonly PMS _parent;

            public WrappedObserver(PMS parent) => _parent = parent;

            public void OnCompleted() => _parent._manager.OnCompleted();

            public void OnError(Exception error) => _parent._manager.OnError(error);

            public void OnNext(T value) => _parent._manager.OnNext(value);
        }

        protected override ISubscribable<T> GetObservableCoreCore<T>() => new WrappedSubscribable<T>(this);

        private sealed class WrappedSubscribable<T> : SubscribableBase<T>
        {
            private readonly PMS _parent;

            public WrappedSubscribable(PMS parent) => _parent = parent;

            protected override ISubscription SubscribeCore(IObserver<T> observer) => new _(this, observer);

            private sealed class _ : ContextSwitchOperator<WrappedSubscribable<T>, T>
            {
                private Action _disposable;

                public _(WrappedSubscribable<T> parent, IObserver<T> obv)
                    : base(parent, obv)
                {
                }

                protected override void OnStart()
                {
                    base.OnStart();

                    _disposable = Params._parent._manager.Subscribe(this);
                }

                public override void OnNext(T value)
                {
                    base.OnNext(value);
                }

                protected override void OnDispose()
                {
                    base.OnDispose();

                    _disposable();
                }

                public override string Name => "ws";

                public override Version Version => new(1, 0, 0, 0);
            }
        }

        private sealed class Manager
        {
            private readonly PMS _parent;
            private readonly ConcurrentDictionary<Guid, Tuple<Type, object, Action, Action<Exception>>> _map = new();
            private readonly SerializationHelpers _serializer = new();

            public Manager(PMS parent)
            {
                _parent = parent;
                MessageRouter.Instance.Subscribe(_parent._uri, new DeserializingObserver(this));
            }

            private sealed class DeserializingObserver : IObserver<byte[]>
            {
                private readonly Manager _parent;

                public DeserializingObserver(Manager parent) => _parent = parent;

                public void OnCompleted()
                {
                    foreach (var pair in _parent._map)
                    {
                        pair.Value.Item3();
                    }
                }

                public void OnError(Exception error)
                {
                    foreach (var pair in _parent._map)
                    {
                        pair.Value.Item4(error);
                    }
                }

                public void OnNext(byte[] value)
                {
                    foreach (var pair in _parent._map)
                    {
                        var md = ((MethodInfo)ReflectionHelpers.InfoOf((SerializationHelpers sh) => sh.Deserialize<object>(default(byte[])))).GetGenericMethodDefinition();
                        var serialized = md.MakeGenericMethod(pair.Value.Item1).Invoke(null, new object[] { _parent._serializer, value });
                        pair.Value.Item2.GetType().GetMethod("Invoke").Invoke(pair.Value.Item2, new object[] { serialized });
                    }
                }
            }

            public Action /* TODO make disposable */ Subscribe<T>(IObserver<T> obv)
            {
                var guid = Guid.NewGuid();
#pragma warning disable IDE0004 // Remove Unnecessary Cast. (Only unnecessary on C# 10 or later.)
                _map[guid] = new Tuple<Type, object, Action, Action<Exception>>(typeof(T), (Action<T>)obv.OnNext, obv.OnCompleted, obv.OnError);
#pragma warning restore IDE0004

                return () =>
                {
                    _map.TryRemove(guid, out _);
                };
            }

            public void OnNext<T>(T value)
            {
                var serialized = _serializer.ToBytes(value);
                MessageRouter.Instance.Publish(_parent._uri, ObserverNotification.CreateOnNext<byte[]>(serialized));
            }

            public void OnError(Exception error)
            {
                MessageRouter.Instance.Publish(_parent._uri, ObserverNotification.CreateOnError<byte[]>(error));
            }

            public void OnCompleted()
            {
                MessageRouter.Instance.Publish(_parent._uri, ObserverNotification.CreateOnCompleted<byte[]>());
            }
        }
    }
}
