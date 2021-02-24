// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

using Reaqtive;
using Reaqtive.Scheduler;
using Reaqtive.Tasks;

namespace Reaqtor
{
    public class StatefulTransitionObservable : SubscribableBase<int>
    {
        private readonly int _count;

        public StatefulTransitionObservable(int count)
        {
            _count = count;
        }

        public static bool IsStateful
        {
            get;
            set;
        }

        protected override ISubscription SubscribeCore(IObserver<int> observer)
        {
            if (IsStateful)
            {
                return new s(this, observer);
            }
            else
            {
                return new _(this, observer);
            }
        }

        private sealed class _ : Operator<StatefulTransitionObservable, int>
        {
            public _(StatefulTransitionObservable parent, IObserver<int> observer)
                : base(parent, observer)
            {
            }
        }

        private sealed class s : StatefulUnaryOperator<StatefulTransitionObservable, int>, ITransitioningOperator
        {
            private readonly List<int> _values;
            private IScheduler _scheduler;

            public s(StatefulTransitionObservable parent, IObserver<int> observer)
                : base(parent, observer)
            {
                _values = new List<int>();
            }

            public override string Name => "rc:MyStatefulOperator";

            public override Version Version => Versioning.v1;

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                var values = Enumerable.Range(1, Params._count).ToList();
                writer.Write(values.Count);
                values.ForEach(writer.Write<int>);
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                var length = reader.Read<int>();
                for (var i = 0; i < length; ++i)
                {
                    _values.Add(reader.Read<int>());
                }
            }

            public override void SetContext(IOperatorContext context)
            {
                base.SetContext(context);

                _scheduler = context.Scheduler;
            }

            protected override void OnStart()
            {
                StateChanged = true;

                _scheduler.Schedule(new ActionTask(() =>
                {
                    foreach (var value in _values)
                    {
                        Output.OnNext(value);
                    }
                }));
            }
        }
    }

    public class StatefulTransitionOperator : SubscribableBase<int>
    {
        private readonly ISubscribable<int> _source;
        private readonly int _count;

        public StatefulTransitionOperator(ISubscribable<int> source, int count)
        {
            _source = source;
            _count = count;
        }

        public static bool IsStateful
        {
            get;
            set;
        }

        protected override ISubscription SubscribeCore(IObserver<int> observer)
        {
            if (IsStateful)
            {
                return new s(this, observer);
            }
            else
            {
                return new _(this, observer);
            }
        }

        private sealed class _ : Operator<StatefulTransitionOperator, int>
        {
            public _(StatefulTransitionOperator parent, IObserver<int> observer)
                : base(parent, observer)
            {
            }

            protected override IEnumerable<ISubscription> OnSubscribe()
            {
                return new[] { Params._source.Subscribe(Output) };
            }
        }

        private sealed class s : StatefulUnaryOperator<StatefulTransitionOperator, int>, ITransitioningOperator
        {
            private readonly List<int> _values;
            private IScheduler _scheduler;

            public s(StatefulTransitionOperator parent, IObserver<int> observer)
                : base(parent, observer)
            {
                _values = new List<int>();
            }

            public override string Name => "rc:MyStatefulOperator";

            public override Version Version => Versioning.v1;

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                var values = Enumerable.Range(1, Params._count).ToList();
                writer.Write(values.Count);
                values.ForEach(writer.Write<int>);
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                var length = reader.Read<int>();
                for (var i = 0; i < length; ++i)
                {
                    _values.Add(reader.Read<int>());
                }
            }

            public override void SetContext(IOperatorContext context)
            {
                base.SetContext(context);

                _scheduler = context.Scheduler;
            }

            protected override void OnStart()
            {
                StateChanged = true;

                _scheduler.Schedule(new ActionTask(() =>
                {
                    foreach (var value in _values)
                    {
                        Output.OnNext(value);
                    }
                }));
            }

            protected override ISubscription OnSubscribe()
            {
                return Params._source.Subscribe(Output);
            }
        }
    }

    public class StatefulTransitionObserver
    {
        public static bool IsStateful
        {
            get;
            set;
        }

        public static IObserver<int> CreateInstance(IObserver<int> inner)
        {
            if (IsStateful)
            {
                return new s(inner);
            }
            else
            {
                return new _(inner);
            }
        }

        private sealed class _ : Observer<int>
        {
            private readonly IObserver<int> _inner;

            public _(IObserver<int> inner)
            {
                _inner = inner;
            }

            protected override void OnCompletedCore()
            {
                _inner.OnCompleted();
            }

            protected override void OnErrorCore(Exception error)
            {
                _inner.OnError(error);
            }

            protected override void OnNextCore(int value)
            {
                _inner.OnNext(value);
            }
        }

        private sealed class s : StatefulObserver<int>, ITransitioningOperator
        {
            private readonly IObserver<int> _inner;

            public s(IObserver<int> inner)
            {
                _inner = inner;
            }

            public override string Name => "rc:StatefulTransitionObserver";

            public override Version Version => Versioning.v1;

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _inner.OnNext(42);
            }

            protected override void OnCompletedCore()
            {
                _inner.OnCompleted();
            }

            protected override void OnErrorCore(Exception error)
            {
                _inner.OnError(error);
            }

            protected override void OnNextCore(int value)
            {
                _inner.OnNext(value);
            }
        }
    }

    public class SaveFailureOperator : SubscribableBase<int>
    {
        protected override ISubscription SubscribeCore(IObserver<int> observer)
        {
            return new _(this, observer);
        }

        public class TestException : InvalidOperationException { }

        private sealed class _ : StatefulUnaryOperator<SaveFailureOperator, int>
        {
            public _(SaveFailureOperator parent, IObserver<int> observer)
                : base(parent, observer)
            {
            }

            public override string Name => "rc:SaveFailureOperator";

            public override Version Version => Versioning.v1;

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                throw new TestException();
            }
        }
    }
}
