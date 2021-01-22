// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;

using Reaqtive;
using Reaqtive.Testing;
using Reaqtive.TestingFramework;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Reaqtive.Operators
{
    /// <summary>
    /// Tests for time related operations.
    /// Parially ported from Rx.
    /// </summary>
    [TestClass]
    public class SubscribableTimeTest : OperatorTestBase
    {
        [TestInitialize]
        public void Initialize()
        {
            base.TestInitialize();
        }

        [TestCleanup]
        public void Cleanup()
        {
            base.TestCleanup();
        }

        #region + Timer +
        [TestMethod]
        public void OneShotTimer_TimeSpan_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Subscribable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(-1)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Subscribable.Timer(DateTimeOffset.Now, TimeSpan.FromSeconds(-1)));
        }

        [TestMethod]
        public void OneShotTimer_TimeSpan_Basic()
        {
            Run(client =>
            {
                var scheduler = GetContext(client);

                var trace = GetTraceSource(scheduler);
                var context = scheduler.CreateContext(trace: trace.TraceSource);

                var res = client.Start(
                    context,
                    () => scheduler.Timer(TimeSpan.FromTicks(300)),
                    Created,
                    Subscribed,
                    Disposed);

                res.Messages.AssertEqual(
                    OnNext(Subscribed + 300, 0L),
                    OnCompleted<long>(Subscribed + 300));

                trace.Logs.AssertEqual(
                    OnLog(200, msg => msg.Contains("created")),
                    OnLog(200, msg => msg.Contains("scheduled to fire '0'")),
                    OnLog(500, msg => msg.Contains("fired '0'")),
                    OnLog(500, msg => msg.Contains("disposed")));
            });
        }

        [TestMethod]
        public void OneShotTimer_TimeSpan_Zero()
        {
            Run(client =>
            {
                var scheduler = GetContext(client);

                var trace = GetTraceSource(scheduler);
                var context = scheduler.CreateContext(trace: trace.TraceSource);

                var res = client.Start(
                    context,
                    () => scheduler.Timer(TimeSpan.FromTicks(0)),
                    Created,
                    Subscribed,
                    Disposed);

                res.Messages.AssertEqual(
                    OnNext(Subscribed, 0L),
                    OnCompleted<long>(Subscribed));

                trace.Logs.AssertEqual(
                    OnLog(200, msg => msg.Contains("created")),
                    OnLog(200, msg => msg.Contains("scheduled to fire '0'")),
                    OnLog(200, msg => msg.Contains("fired '0'")),
                    OnLog(200, msg => msg.Contains("disposed")));
            });
        }

        [TestMethod]
        public void OneShotTimer_TimeSpan_Negative()
        {
            Run(client =>
            {
                var scheduler = GetContext(client);

                var trace = GetTraceSource(scheduler);
                var context = scheduler.CreateContext(trace: trace.TraceSource);

                var res = client.Start(
                    context,
                    () => scheduler.Timer(TimeSpan.FromTicks(-1)),
                    Created,
                    Subscribed,
                    Disposed);

                res.Messages.AssertEqual(
                    OnNext(Subscribed, 0L),
                    OnCompleted<long>(Subscribed));

                trace.Logs.AssertEqual(
                    OnLog(200, msg => msg.Contains("created")),
                    OnLog(200, msg => msg.Contains("scheduled to fire '0'")),
                    OnLog(200, msg => msg.Contains("fired '0'")),
                    OnLog(200, msg => msg.Contains("disposed")));
            });
        }

        [TestMethod]
        public void OneShotTimer_TimeSpan_Disposed()
        {
            Run(client =>
            {
                var scheduler = GetContext(client);

                var trace = GetTraceSource(scheduler);
                var context = scheduler.CreateContext(trace: trace.TraceSource);

                var res = client.Start(
                    context,
                    () => scheduler.Timer(TimeSpan.FromTicks(1000)),
                    Created,
                    Subscribed,
                    Disposed);

                res.Messages.AssertEqual();

                trace.Logs.AssertEqual(
                    OnLog(200, msg => msg.Contains("created")),
                    OnLog(200, msg => msg.Contains("scheduled to fire '0'")),
                    OnLog(1000, msg => msg.Contains("disposed")),
                    OnLog(1200, msg => msg.Contains("muted")));
            });
        }

        [TestMethod]
        public void OneShotTimer_TimeSpan_OnNextObserverThrows()
        {
            var xs = Subscribable.Timer(TimeSpan.FromTicks(1));
            var observer = new OnNextThrowsObserver<long>();
            var sub = xs.Subscribe(observer, Scheduler.CreateContext());

            ReactiveAssert.Throws<InvalidOperationException>(() => Scheduler.Start());
        }

        private sealed class OnNextThrowsObserver<T> : IObserver<T>
        {
            public void OnCompleted() { }

            public void OnError(Exception error) { }

            public void OnNext(T value) => throw new InvalidOperationException();
        }

        [TestMethod]
        public void OneShotTimer_TimeSpan_OnCompletedObserverThrows()
        {
            var xs = Subscribable.Timer(TimeSpan.FromTicks(1));
            var observer = new OnCompletedThrowsObserver<long>();
            var sub = xs.Subscribe(observer, Scheduler.CreateContext());

            ReactiveAssert.Throws<InvalidOperationException>(() => Scheduler.Start());
        }

        private sealed class OnCompletedThrowsObserver<T> : IObserver<T>
        {
            public void OnCompleted() => throw new InvalidOperationException();

            public void OnError(Exception error) { }

            public void OnNext(T value) { }
        }

        [TestMethod]
        public void OneShotTimer_DateTimeOffset_Basic()
        {
            Run(client =>
            {
                var scheduler = GetContext(client);

                var trace = GetTraceSource(scheduler);
                var context = scheduler.CreateContext(trace: trace.TraceSource);

                var res = client.Start(
                    context,
                    () => scheduler.Timer(new DateTimeOffset(500, TimeSpan.Zero)),
                    Created,
                    Subscribed,
                    Disposed);

                res.Messages.AssertEqual(
                    OnNext(500, 0L),
                    OnCompleted<long>(500));

                trace.Logs.AssertEqual(
                    OnLog(200, msg => msg.Contains("created")),
                    OnLog(200, msg => msg.Contains("scheduled to fire '0'")),
                    OnLog(500, msg => msg.Contains("fired '0'")),
                    OnLog(500, msg => msg.Contains("disposed")));
            });
        }

        [TestMethod]
        public void OneShotTimer_DateTimeOffset_Zero()
        {
            Run(client =>
            {
                var scheduler = GetContext(client);

                var trace = GetTraceSource(scheduler);
                var context = scheduler.CreateContext(trace: trace.TraceSource);

                var res = client.Start(
                    context,
                    () => scheduler.Timer(new DateTimeOffset(200, TimeSpan.Zero)),
                    Created,
                    Subscribed,
                    Disposed);

                res.Messages.AssertEqual(
                    OnNext(Subscribed, 0L),
                    OnCompleted<long>(Subscribed));

                trace.Logs.AssertEqual(
                    OnLog(200, msg => msg.Contains("created")),
                    OnLog(200, msg => msg.Contains("scheduled to fire '0'")),
                    OnLog(200, msg => msg.Contains("fired '0'")),
                    OnLog(200, msg => msg.Contains("disposed")));
            });
        }

        [TestMethod]
        public void OneShotTimer_DateTimeOffset_Past()
        {
            Run(client =>
            {
                var scheduler = GetContext(client);

                var trace = GetTraceSource(scheduler);
                var context = scheduler.CreateContext(trace: trace.TraceSource);

                var res = client.Start(
                    context,
                    () => scheduler.Timer(new DateTimeOffset(0, TimeSpan.Zero)),
                    Created,
                    Subscribed,
                    Disposed);

                res.Messages.AssertEqual(
                    OnNext(Subscribed, 0L),
                    OnCompleted<long>(Subscribed));

                trace.Logs.AssertEqual(
                    OnLog(200, msg => msg.Contains("created")),
                    OnLog(200, msg => msg.Contains("scheduled to fire '0'")),
                    OnLog(200, msg => msg.Contains("fired '0'")),
                    OnLog(200, msg => msg.Contains("disposed")));
            });
        }

        [TestMethod]
        public void RepeatingTimer_DateTimeOffset_TimeSpan_Simple()
        {
            Run(client =>
            {
                var scheduler = GetContext(client);

                var trace = GetTraceSource(scheduler);
                var context = scheduler.CreateContext(trace: trace.TraceSource);

                const int created = 0;
                const int subscribed = 200;
                const int disposed = 750;

                var res = client.Start(
                    context,
                    () => scheduler.Timer(new DateTimeOffset(300, TimeSpan.Zero), TimeSpan.FromTicks(100)),
                    created,
                    subscribed,
                    disposed);

                res.Messages.AssertEqual(
                    OnNext(300, 0L),
                    OnNext(400, 1L),
                    OnNext(500, 2L),
                    OnNext(600, 3L),
                    OnNext(700, 4L));

                trace.Logs.AssertEqual(
                    OnLog(200, msg => msg.Contains("created")),
                    OnLog(200, msg => msg.Contains("scheduled to fire '0'")),
                    OnLog(300, msg => msg.Contains("fired '0'")),
                    OnLog(300, msg => msg.Contains("scheduled to fire '1'")),
                    OnLog(400, msg => msg.Contains("fired '1'")),
                    OnLog(400, msg => msg.Contains("scheduled to fire '2'")),
                    OnLog(500, msg => msg.Contains("fired '2'")),
                    OnLog(500, msg => msg.Contains("scheduled to fire '3'")),
                    OnLog(600, msg => msg.Contains("fired '3'")),
                    OnLog(600, msg => msg.Contains("scheduled to fire '4'")),
                    OnLog(700, msg => msg.Contains("fired '4'")),
                    OnLog(700, msg => msg.Contains("scheduled to fire '5'")),
                    OnLog(750, msg => msg.Contains("disposed")),
                    OnLog(800, msg => msg.Contains("muted")));
            });
        }

        [TestMethod]
        public void RepeatingTimer_DateTimeOffset_TimeSpan_PastDue()
        {
            Run(client =>
            {
                var scheduler = GetContext(client);

                var trace = GetTraceSource(scheduler);
                var context = scheduler.CreateContext(trace: trace.TraceSource);

                const int created = 0;
                const int subscribed = 200;
                const int disposed = 600;

                const int period = 100;
                const int start = subscribed - 50;

                var res = client.Start(
                    context,
                    () => scheduler.Timer(new DateTimeOffset(start, TimeSpan.Zero), TimeSpan.FromTicks(period)),
                    created,
                    subscribed,
                    disposed);

                res.Messages.AssertEqual(
                    OnNext(200, 0L),
                    OnNext(250, 1L),
                    OnNext(350, 2L),
                    OnNext(450, 3L),
                    OnNext(550, 4L));

                trace.Logs.AssertEqual(
                    OnLog(200, msg => msg.Contains("created")),
                    OnLog(200, msg => msg.Contains("scheduled to fire '0'")),
                    OnLog(200, msg => msg.Contains("fired '0'")),
                    OnLog(200, msg => msg.Contains("scheduled to fire '1'")),
                    OnLog(250, msg => msg.Contains("fired '1'")),
                    OnLog(250, msg => msg.Contains("scheduled to fire '2'")),
                    OnLog(350, msg => msg.Contains("fired '2'")),
                    OnLog(350, msg => msg.Contains("scheduled to fire '3'")),
                    OnLog(450, msg => msg.Contains("fired '3'")),
                    OnLog(450, msg => msg.Contains("scheduled to fire '4'")),
                    OnLog(550, msg => msg.Contains("fired '4'")),
                    OnLog(550, msg => msg.Contains("scheduled to fire '5'")),
                    OnLog(600, msg => msg.Contains("disposed")),
                    OnLog(650, msg => msg.Contains("muted")));
            });
        }

        [TestMethod]
        public void RepeatingTimer_DateTimeOffset_TimeSpan_PastDue_CatchUp_NotApplicable()
        {
            Run(client =>
            {
                var scheduler = GetContext(client);

                var trace = GetTraceSource(scheduler);
                var context = scheduler.CreateContext(trace: trace.TraceSource);

                const int created = 0;
                const int subscribed = 200;
                const int disposed = 600;

                const int period = 100;
                const int start = subscribed - (period - 1);

                var res = client.Start(
                    context,
                    () => scheduler.Timer(new DateTimeOffset(start, TimeSpan.Zero), TimeSpan.FromTicks(period)),
                    created,
                    subscribed,
                    disposed);

                res.Messages.AssertEqual(
                    OnNext(200, 0L), // 101
                    OnNext(201, 1L),
                    OnNext(301, 2L),
                    OnNext(401, 3L),
                    OnNext(501, 4L));

                trace.Logs.AssertEqual(
                    OnLog(200, msg => msg.Contains("created")),
                    OnLog(200, msg => msg.Contains("scheduled to fire '0'")),
                    OnLog(200, msg => msg.Contains("fired '0'")),
                    OnLog(200, msg => msg.Contains("scheduled to fire '1'")),
                    OnLog(201, msg => msg.Contains("fired '1'")),
                    OnLog(201, msg => msg.Contains("scheduled to fire '2'")),
                    OnLog(301, msg => msg.Contains("fired '2'")),
                    OnLog(301, msg => msg.Contains("scheduled to fire '3'")),
                    OnLog(401, msg => msg.Contains("fired '3'")),
                    OnLog(401, msg => msg.Contains("scheduled to fire '4'")),
                    OnLog(501, msg => msg.Contains("fired '4'")),
                    OnLog(501, msg => msg.Contains("scheduled to fire '5'")),
                    OnLog(600, msg => msg.Contains("disposed")),
                    OnLog(601, msg => msg.Contains("muted")));
            });
        }

        [TestMethod]
        public void RepeatingTimer_DateTimeOffset_TimeSpan_PastDue_CatchUp_Applicable()
        {
            Run(client =>
            {
                var scheduler = GetContext(client);

                var trace = GetTraceSource(scheduler);
                var context = scheduler.CreateContext(trace: trace.TraceSource);

                const int created = 0;
                const int subscribed = 200;
                const int disposed = 600;

                const int period = 100;
                const int start = subscribed - (period + 1);

                var res = client.Start(
                    context,
                    () => scheduler.Timer(new DateTimeOffset(start, TimeSpan.Zero), TimeSpan.FromTicks(period)),
                    created,
                    subscribed,
                    disposed);

                res.Messages.AssertEqual(
                    OnNext(200, 0L), // 199 after skipping 99
                    OnNext(299, 1L),
                    OnNext(399, 2L),
                    OnNext(499, 3L),
                    OnNext(599, 4L));

                trace.Logs.AssertEqual(
                    OnLog(200, msg => msg.Contains("created")),
                    OnLog(200, msg => msg.Contains("catch-up tick skip")),
                    OnLog(200, msg => msg.Contains("scheduled to fire '0'")),
                    OnLog(200, msg => msg.Contains("fired '0'")),
                    OnLog(200, msg => msg.Contains("scheduled to fire '1'")),
                    OnLog(299, msg => msg.Contains("fired '1'")),
                    OnLog(299, msg => msg.Contains("scheduled to fire '2'")),
                    OnLog(399, msg => msg.Contains("fired '2'")),
                    OnLog(399, msg => msg.Contains("scheduled to fire '3'")),
                    OnLog(499, msg => msg.Contains("fired '3'")),
                    OnLog(499, msg => msg.Contains("scheduled to fire '4'")),
                    OnLog(599, msg => msg.Contains("fired '4'")),
                    OnLog(599, msg => msg.Contains("scheduled to fire '5'")),
                    OnLog(600, msg => msg.Contains("disposed")),
                    OnLog(699, msg => msg.Contains("muted")));
            });
        }

        [TestMethod]
        public void RepeatingTimer_DateTimeOffset_TimeSpan_PastDue_CatchUp_EdgeCase()
        {
            Run(client =>
            {
                var scheduler = GetContext(client);

                var trace = GetTraceSource(scheduler);
                var context = scheduler.CreateContext(trace: trace.TraceSource);

                const int created = 0;
                const int subscribed = 200;
                const int disposed = 600;

                const int period = 100;
                const int start = subscribed - period;

                var res = client.Start(
                    context,
                    () => scheduler.Timer(new DateTimeOffset(start, TimeSpan.Zero), TimeSpan.FromTicks(period)),
                    created,
                    subscribed,
                    disposed);

                res.Messages.AssertEqual(
                    OnNext(200, 0L),
                    OnNext(300, 1L),
                    OnNext(400, 2L),
                    OnNext(500, 3L));

                trace.Logs.AssertEqual(
                    OnLog(200, msg => msg.Contains("created")),
                    OnLog(200, msg => msg.Contains("catch-up tick skip")),
                    OnLog(200, msg => msg.Contains("scheduled to fire '0'")),
                    OnLog(200, msg => msg.Contains("fired '0'")),
                    OnLog(200, msg => msg.Contains("scheduled to fire '1'")),
                    OnLog(300, msg => msg.Contains("fired '1'")),
                    OnLog(300, msg => msg.Contains("scheduled to fire '2'")),
                    OnLog(400, msg => msg.Contains("fired '2'")),
                    OnLog(400, msg => msg.Contains("scheduled to fire '3'")),
                    OnLog(500, msg => msg.Contains("fired '3'")),
                    OnLog(500, msg => msg.Contains("scheduled to fire '4'")),
                    OnLog(600, msg => msg.Contains("disposed")),
                    OnLog(600, msg => msg.Contains("muted")));
            });
        }

        [TestMethod]
        public void RepeatingTimer_TimeSpan_TimeSpan_Simple()
        {
            Run(client =>
            {
                var scheduler = GetContext(client);

                var trace = GetTraceSource(scheduler);
                var context = scheduler.CreateContext(trace: trace.TraceSource);

                const int created = 0;
                const int subscribed = 200;
                const int disposed = 750;

                var res = client.Start(
                    context,
                    () => scheduler.Timer(TimeSpan.FromTicks(100), TimeSpan.FromTicks(100)),
                    created,
                    subscribed,
                    disposed);

                res.Messages.AssertEqual(
                    OnNext(subscribed + 100, 0L),
                    OnNext(subscribed + 200, 1L),
                    OnNext(subscribed + 300, 2L),
                    OnNext(subscribed + 400, 3L),
                    OnNext(subscribed + 500, 4L));

                trace.Logs.AssertEqual(
                    OnLog(200, msg => msg.Contains("created")),
                    OnLog(200, msg => msg.Contains("scheduled to fire '0'")),
                    OnLog(300, msg => msg.Contains("fired '0'")),
                    OnLog(300, msg => msg.Contains("scheduled to fire '1'")),
                    OnLog(400, msg => msg.Contains("fired '1'")),
                    OnLog(400, msg => msg.Contains("scheduled to fire '2'")),
                    OnLog(500, msg => msg.Contains("fired '2'")),
                    OnLog(500, msg => msg.Contains("scheduled to fire '3'")),
                    OnLog(600, msg => msg.Contains("fired '3'")),
                    OnLog(600, msg => msg.Contains("scheduled to fire '4'")),
                    OnLog(700, msg => msg.Contains("fired '4'")),
                    OnLog(700, msg => msg.Contains("scheduled to fire '5'")),
                    OnLog(750, msg => msg.Contains("disposed")),
                    OnLog(800, msg => msg.Contains("muted")));
            });
        }

        [TestMethod]
        public void RepeatingTimer_SimpleSaveAndLoad()
        {
            ISubscription sub = null;
            IOperatorContext context = Scheduler.CreateContext();

            var state = Scheduler.CreateStateContainer();
            var checkpoints = new[]
            {
                OnSave(152, state)
            };

            // Create new subscription allow the timer to fire twice.
            var res1 = Scheduler.CreateObserver<long>();
            Scheduler.ScheduleAbsolute(100, () => sub = Subscribable.Timer(TimeSpan.Zero, TimeSpan.FromTicks(50)).Apply(Scheduler, checkpoints).Subscribe(res1, context));
            Scheduler.ScheduleAbsolute(155, () => sub.Dispose());

            // Create new subscription and load the state.
            var res2 = Scheduler.CreateObserver<long>();
            Scheduler.ScheduleAbsolute(200, () => sub = Subscribable.Timer(TimeSpan.Zero, TimeSpan.FromTicks(50)).Subscribe(res2, context, state));
            Scheduler.ScheduleAbsolute(255, () => sub.Dispose());

            Scheduler.Start();

            res1.Messages.AssertEqual(
                new[]
                    {
                        OnNext(100, 0L),
                        OnNext(150, 1L)
                    });
            res2.Messages.AssertEqual(new[]
                    {
                        OnNext(200, 2L),
                        OnNext(250, 3L)
                    });
        }

        [TestMethod]
        public void RepeatingTimer_RestoreOlderCheckpoint()
        {
            var oldState = Scheduler.CreateStateContainer();
            var newState = Scheduler.CreateStateContainer();
            var checkpoints = new[]
                                  {
                                      OnSave(255, oldState),
                                      OnSave(305, newState),
                                      OnSave(405, newState),
                                      OnLoad(406, oldState)
                                  };


            var result = Scheduler.Start(
                () => Subscribable.Timer(TimeSpan.Zero, TimeSpan.FromTicks(50)).Apply(Scheduler, checkpoints),
                disposed: 460);

            result.Messages.AssertEqual(new[]
                {
                    OnNext(Subscribed, 0L),
                    OnNext(250, 1L),
                    // Saved old state here
                    OnNext(300, 2L),
                    OnNext(350, 3L),
                    OnNext(400, 4L),
                    // Loaded old state here
                    OnNext(450, 2L),
                    OnNext(450, 3L),
                    OnNext(450, 4L),
                    OnNext(450, 5L)
                });
        }

        [TestMethod]
        public void RepeatingTimer_Overflow1()
        {
            ISubscription sub = null;
            IOperatorContext context = Scheduler.CreateContext();

            var state = Scheduler.CreateStateContainer();
            var checkpoints = new[]
            {
                OnSave(152, state)
            };

            DateTimeOffset firstFire = DateTime.MinValue;

            // Create new subscription allow the timer to fire twice.
            var res1 = Scheduler.CreateObserver<long>();
            Scheduler.ScheduleAbsolute(100, () => { firstFire = Scheduler.Now; sub = Subscribable.Timer(firstFire, TimeSpan.MaxValue).Apply(Scheduler, checkpoints).Subscribe(res1, context); });
            Scheduler.ScheduleAbsolute(155, () => sub.Dispose());

            // Create new subscription and load the state.
            var res2 = Scheduler.CreateObserver<long>();
            Scheduler.ScheduleAbsolute(200, () => sub = Subscribable.Timer(firstFire, TimeSpan.MaxValue).Subscribe(res2, context, state));
            Scheduler.ScheduleAbsolute(255, () => sub.Dispose());

            Scheduler.Start();

            res1.Messages.AssertEqual(
                new[]
                    {
                        OnNext(100, 0L)
                    });
            res2.Messages.AssertEqual(Array.Empty<Recorded<Notification<long>>>());
        }

        [TestMethod]
        public void RepeatingTimer_Overflow2()
        {
            ISubscription sub = null;
            IOperatorContext context = Scheduler.CreateContext();

            var state = Scheduler.CreateStateContainer();
            var checkpoints = new[]
            {
                OnSave(152, state)
            };

            // Create new subscription allow the timer to fire twice.
            var res1 = Scheduler.CreateObserver<long>();
            Scheduler.ScheduleAbsolute(100, () => { sub = Subscribable.Timer(TimeSpan.MinValue).Apply(Scheduler, checkpoints).Subscribe(res1, context); });
            Scheduler.ScheduleAbsolute(155, () => sub.Dispose());

            // Create new subscription and load the state.
            var res2 = Scheduler.CreateObserver<long>();
            Scheduler.ScheduleAbsolute(200, () => sub = Subscribable.Timer(TimeSpan.MinValue).Subscribe(res2, context, state));
            Scheduler.ScheduleAbsolute(255, () => sub.Dispose());

            Scheduler.Start();

            res1.Messages.AssertEqual(
                new[]
                    {
                        OnNext(100, 0L),
                        OnCompleted(100, 0L),
                    });
            res2.Messages.AssertEqual(Array.Empty<Recorded<Notification<long>>>());
        }

        #endregion

        #region Helpers

#if UNUSED
        private static Recorded<LogEntry> OnLog(long time, string message)
        {
            return new Recorded<LogEntry>(time, new LogEntry(message));
        }
#endif

        private static Recorded<LogEntry> OnLog(long time, Expression<Func<string, bool>> messagePredicate)
        {
            return new Recorded<LogEntry>(time, new LogEntry(messagePredicate));
        }

        private class LogEntry
        {
            private readonly string _message;
            private readonly Func<string, bool> _predicate;
            private readonly string _predicateString;

            public LogEntry(string message)
            {
                _message = message;
            }

            public LogEntry(Expression<Func<string, bool>> predicate)
            {
                _predicate = predicate.Compile();
                _predicateString = predicate.ToString();
            }

            public override bool Equals(object obj)
            {
                if (obj is not LogEntry other)
                {
                    return false;
                }

                if (_predicate != null)
                {
                    if (other._predicate != null)
                    {
                        return false;
                    }

                    return _predicate(other._message);
                }
                else
                {
                    if (other._predicate != null)
                    {
                        return other._predicate(_message);
                    }
                    else
                    {
                        return other._message == _message;
                    }
                }
            }

            public override int GetHashCode()
            {
                if (_message != null)
                {
                    return _message.GetHashCode();
                }

                return base.GetHashCode();
            }

            public override string ToString()
            {
                if (_message != null)
                {
                    return _message;
                }
                else
                {
                    return _predicateString;
                }
            }
        }

        private static TestLogger GetTraceSource(IClockable<long> clock)
        {
            var res = new List<Recorded<LogEntry>>();

            var trace = GetTraceSource(clock, (time, message) => res.Add(new Recorded<LogEntry>(time, new LogEntry(message))));

            return new TestLogger(res, trace);
        }

        private static TraceSource GetTraceSource(IClockable<long> clock, Action<long, string> onMessage)
        {
            var trace = new TraceSource("Tests_" + Guid.NewGuid().ToString(), SourceLevels.All);
            trace.Listeners.Add(new MyListener(clock, onMessage));
            return trace;
        }

        private class TestLogger
        {
            public TestLogger(List<Recorded<LogEntry>> logs, TraceSource trace)
            {
                Logs = logs;
                TraceSource = trace;
            }

            public IEnumerable<Recorded<LogEntry>> Logs { get; }
            public TraceSource TraceSource { get; }
        }

        private class MyListener : TraceListener
        {
            private readonly IClockable<long> _clock;
            private readonly Action<long, string> _onMessage;
            private readonly List<string> _buffer;

            public MyListener(IClockable<long> clock, Action<long, string> onMessage)
            {
                _clock = clock;
                _onMessage = onMessage;
                _buffer = new List<string>();
            }

            public override void Write(string message)
            {
                _buffer.Add(message);
            }

            public override void WriteLine(string message)
            {
                _buffer.Add(message);
                var res = string.Concat(_buffer);
                _buffer.Clear();
                _onMessage(_clock.Clock, res);
            }
        }

        #endregion
    }
}
