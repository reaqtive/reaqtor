// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

using Reaqtive;
using Reaqtive.Testing;
using Reaqtive.TestingFramework;

using Reaqtor;
using Reaqtor.Remoting.Protocol;
using Reaqtor.Remoting.TestingFramework;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Tests.Reaqtor.Remoting.Glitching;
using Tests.Reaqtor.Remoting.Glitching.Versioning;

namespace Test.Reaqtive.Operators
{
    public class OperatorTestBase : RemotingTestBase
    {
        private static readonly Settings settings = new();
        public const long Subscribed = 200L;

        protected void Run(Action<ITestReactivePlatformClient> test)
        {
            var failures = new List<string>();

            if (settings.VersioningTestsSave || settings.VersioningTestsRun)
            {
                var caller = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod();
                var store = new StateVersionTestStore();

                if (settings.VersioningTestsSave)
                {
                    SaveState(test, store, caller);
                }
                if (settings.VersioningTestsRun)
                {
                    var testCases = store.GetTestCasesByMember(caller).ToArray();
                    if (testCases.Length > 0)
                    {
                        failures = RunVersioning(test, store, testCases);
                    }
                    else
                    {
                        var originalColor = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Warning, no test cases found for '{0}'.", caller.Name);
                        Console.ForegroundColor = originalColor;
                    }
                }
            }
            else
            {
#if !FULL_GLITCHING
                // Use this helper to run the glitching tests
                failures.AddRange(RunDifferentialGlitchingTwoRuns(
                    test,
                    configuration =>
                    {
                        configuration.SchedulerType = SchedulerType.Logging;
                        configuration.EngineOptions["SerializationPolicyVersion"] = new Version(1, 0, 0, 0).ToString();
                        configuration.EngineOptions["TemplatizeExpressions"] = "false";
#if TEMPLATE_GLITCHING
                    },
                    configuration =>
                    {
                        configuration.SchedulerType = SchedulerType.Logging;
                        configuration.EngineOptions["SerializationPolicyVersion"] = new Version(3, 0, 0, 0).ToString();
                        configuration.EngineOptions["TemplatizeExpressions"] = "true";
#endif
                    }));

#else
                // Use this test to get more fine-grained information about the
                // checkpoint recovery pair that caused the test failure.
                failures.AddRange(RunDifferentialGlitchingWindowPairs(
                    test,
                    configuration =>
                    {
                        configuration.SchedulerType = SchedulerType.Logging;
                        configuration.EngineOptions["SerializationPolicyVersion"] = new Version(1, 0, 0, 0).ToString();
                        configuration.EngineOptions["TemplatizeExpressions"] = "false";
#if TEMPLATE_GLITCHING
                    },
                    configuration =>
                    {
                        configuration.SchedulerType = SchedulerType.Logging;
                        configuration.EngineOptions["SerializationPolicyVersion"] = new Version(3, 0, 0, 0).ToString();
                        configuration.EngineOptions["TemplatizeExpressions"] = "true";
#endif
                    }));
#endif
            }

            Assert.IsTrue(failures.Count == 0, string.Join("\n\n", failures));
        }

        #region Scheduling helpers

        private IList<long> GetScheduling(Action<ITestReactivePlatformClient> test)
        {
            var scheduling = default(List<long>);
            using (var client = CreateTestClient(Environment, LoggingConfiguration))
            {
                test(client);
                scheduling = ((ILoggingScheduler<long>)client.Scheduler).ScheduledTimes.ToList();
            }
            scheduling.Sort();
            scheduling.Add(scheduling[scheduling.Count - 1] + 1);
            return scheduling;
        }

        #endregion

        #region Glitching - Full checkpoint only

#if UNUSED // NB: Kept as an alternative option.
        /// <summary>
        /// Runs the glitching using a single full checkpoint.
        /// </summary>
        /// <param name="test">The test to glitch.</param>
        private List<string> RunGlitching(Action<ITestReactivePlatformClient> test)
        {
            var scheduling = GetScheduling(test).Distinct();

            var failures = new List<string>();

            foreach (var failover in scheduling)
            {
                try
                {
                    using var client = CreateTestClient(Environment, LoggingConfiguration);

                    client.Scheduler.ScheduleAbsolute(failover, () =>
                    {
                        var qe = client.Platform.QueryEvaluators.First();
                        qe.Checkpoint();
                        qe.Unload();
                        qe.Recover();
                    });

                    test(client);
                }
                catch (AssertFailedException e)
                {
                    failures.Add(
                        string.Format(
                            CultureInfo.InvariantCulture,
                            "Test failed with checkpoint/recovery @ time '{0}' with message:\n{1}",
                            failover,
                            e.Message));
                }
            }

            return failures;
        }
#endif

        #endregion

        #region Glitching - Full and differential checkpoints

#if FULL_GLITCHING
        /// <summary>
        /// Runs the glitching using a full checkpoint followed by a differential checkpoint.
        /// The checkpoints occur before and after a single event.
        /// </summary>
        /// <param name="test">The test to glitch.</param>
        private List<string> RunDifferentialGlitchingWindowPairs(Action<ITestReactivePlatformClient> test, params Action<IReactivePlatformConfiguration>[] configurations)
        {
            var scheduling = GetScheduling(test).Distinct();

            var failures = new List<string>();

            foreach (var configuration in configurations)
            {
                foreach (var pair in scheduling.Zip(scheduling.Skip(1), (t1, t2) => new[] { t1, t2 }))
                {
                    try
                    {
                        using var client = CreateTestClient(Environment, configuration);

                        ScheduleDifferentialGlitchesAndRun(client, test, pair);
                    }
                    catch (Exception e)
                    {
                        failures.Add(StringifyDifferentialException(e, pair));
                    }
                }
            }

            return failures;
        }
#endif

        #endregion

        #region Glitching - Full and differential checkpoints in two runs

        /// <summary>
        /// Runs glitching using a full checkpoint followed by a differential
        /// checkpoint. The test is repeated twice, alternating which events
        /// the differential checkpoint and recovery occurs between.
        /// </summary>
        /// <param name="test">The test to glitch.</param>
        private List<string> RunDifferentialGlitchingTwoRuns(Action<ITestReactivePlatformClient> test, params Action<IReactivePlatformConfiguration>[] configurations)
        {
            var scheduling = GetScheduling(test).Distinct().ToList();

            var failures = new List<string>();

            var pairs = scheduling.Where((x, i) => i % 2 == 0).Zip(scheduling.Where((x, i) => i % 2 == 1), (t1, t2) => new[] { t1, t2 }).ToArray();

            foreach (var configuration in configurations)
            {
                try
                {
                    using var client = CreateTestClient(Environment, configuration);

                    ScheduleDifferentialGlitchesAndRun(client, test, pairs);
                }
                catch (Exception e)
                {
                    failures.Add(StringifyDifferentialException(e, pairs));
                }

                pairs = scheduling.Skip(1).Where((x, i) => i % 2 == 0).Zip(scheduling.Skip(1).Where((x, i) => i % 2 == 1), (t1, t2) => new[] { t1, t2 }).ToArray();
                try
                {
                    using var client = CreateTestClient(Environment, configuration);

                    ScheduleDifferentialGlitchesAndRun(client, test, pairs);
                }
                catch (Exception e)
                {
                    failures.Add(StringifyDifferentialException(e, pairs));
                }
            }

            return failures;
        }

        #endregion

        #region Glitching - Full and differential checkpoints between all scheduling pairs

#if UNUSED // NB: Kept as an alternative option.
        // Note: these tests are no more informative than glitching with windowed pairs
        private List<string> RunDifferentialGlitchingAllPairs(Action<ITestReactivePlatformClient> test)
        {
            var scheduling = GetScheduling(test).Distinct();

            var failures = new List<string>();

            foreach (var subset in scheduling.Choose(2).Select(ss => ss.ToList()))
            {
                subset.Sort();

                try
                {
                    using var client = CreateTestClient(Environment, LoggingConfiguration);

                    ScheduleDifferentialGlitchesAndRun(client, test, subset.ToArray());
                }
                catch (Exception e)
                {
                    failures.Add(StringifyDifferentialException(e, subset.ToArray()));
                }
            }

            return failures;
        }
#endif

        #endregion

        #region Glitching - Core utils

        private static void ScheduleDifferentialGlitchesAndRun(ITestReactivePlatformClient client, Action<ITestReactivePlatformClient> test, params long[][] checkpointFailoverPairs)
        {
            foreach (var pair in checkpointFailoverPairs)
            {
                var checkpoint = pair[0];
                var failover = pair[1];
                client.Scheduler.ScheduleAbsolute(checkpoint, () =>
                {
                    client.Platform.QueryEvaluators.First().Checkpoint();
                });
                client.Scheduler.ScheduleAbsolute(failover, () =>
                {
                    var qe = client.Platform.QueryEvaluators.First();
                    qe.Checkpoint();
                    qe.Unload();
                    qe.Recover();
                });
            }

            test(client);
        }

        private static string StringifyDifferentialException(Exception e, params long[][] pairs)
        {
            if (e is AggregateException a)
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    "Test failed with checkpoint/recovery @ times '[{0}]' with message:\n{1}\n{2}",
                    string.Join(", ", pairs.Select(p => "(" + p[0] + "," + p[1] + ")")),
                    a.Message,
                    string.Join("\n", a.Flatten().InnerExceptions));
            }
            else
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    "Test failed with checkpoint/recovery @ times '[{0}]' with message:\n{1}",
                    string.Join(", ", pairs.Select(p => "(" + p[0] + "," + p[1] + ")")),
                    e);
            }
        }

        #endregion

        #region Versioning

        private void SaveState(Action<ITestReactivePlatformClient> test, StateVersionTestStore store, MemberInfo caller)
        {
            var scheduling = GetScheduling(test).Distinct().ToList();

            using var client = CreateTestClient(Environment, CreateConfiguration(settings));

            // Tail scheduling is required here because scheduling all checkpoint
            // events before the test is run leads to issues with the virtual
            // scheduler
            DoScheduling(client, scheduling, 0, time =>
            {
                client.Platform.QueryEvaluators.First().Checkpoint();
                var state = client.Platform.Environment.StateStoreService.GetInstance<IReactiveStateStoreConnection>().SerializeStateStore();
                store.Add(caller, time, state);
            });

            test(client);
        }

        private List<string> RunVersioning(Action<ITestReactivePlatformClient> test, StateVersionTestStore store, params StateVersionTestCase[] testCases)
        {
            var failures = new List<string>();

            try
            {
                foreach (var testCase in testCases)
                {
                    using var client = CreateTestClient(Environment, CreateConfiguration(settings));

                    client.Scheduler.ScheduleAbsolute(testCase.SavedAt, () =>
                    {
                        client.Platform.QueryEvaluators.First().Unload();
                        var state = store.GetState(testCase);
                        client.Platform.Environment.StateStoreService.GetInstance<IReactiveStateStoreConnection>().DeserializeStateStore(state);
                        client.Platform.Environment.KeyValueStoreService.GetInstance<ITransactionalKeyValueStoreConnection>().Clear();
                        client.Platform.QueryEvaluators.First().Recover();
                    });

                    test(client);
                }
            }
            catch (Exception e)
            {
                failures.Add(e.ToString());
            }

            return failures;
        }

        private void DoScheduling(ITestReactivePlatformClient client, List<long> scheduling, int schedulingIndex, Action<long> action)
        {
            if (schedulingIndex >= scheduling.Count)
            {
                return;
            }

            var time = scheduling[schedulingIndex];
            client.Scheduler.ScheduleAbsolute(time, () =>
            {
                action(time);
            });

            DoScheduling(client, scheduling, schedulingIndex + 1, action);
        }

        #endregion

        #region Declarative Helpers

        protected static ITestableQbservable<T> GetObservableWitness<T>()
        {
            return default;
        }

        protected static ReactiveClientContext GetContext(ITestReactivePlatformClient client)
        {
            return client.Platform.CreateClient().Context;
        }

#pragma warning disable IDE0060 // Remove unused parameter. (REVIEW: Use of multiple dates back to different scheduling policies.)
        protected static long Increment(long originalTime, int multiple)
        {
            return originalTime;
        }
#pragma warning restore IDE0060

        #endregion

        #region Notification Helpers

        protected static Recorded<INotification<T>> OnNext<T>(long time, T value)
        {
            return ObserverMessage.OnNext<T>(time, value);
        }

        protected static Recorded<INotification<T>> OnNext<T>(long time, Func<T, bool> predicate)
        {
            return ObserverMessage.OnNext<T>(time, predicate);
        }

        protected static Recorded<INotification<T>> OnError<T>(long time, Exception error)
        {
            return ObserverMessage.OnError<T>(time, error);
        }

#pragma warning disable IDE0060 // Remove unused parameter (used for type inference purposes)
        protected static Recorded<INotification<T>> OnError<T>(long time, Exception error, T dummy)
        {
            return ObserverMessage.OnError<T>(time, error);
        }
#pragma warning restore IDE0060 // Remove unused parameter

        protected static Recorded<INotification<T>> OnError<T>(long time, Func<Exception, bool> predicate)
        {
            return ObserverMessage.OnError<T>(time, predicate);
        }

        protected static Recorded<INotification<T>> OnCompleted<T>(long time)
        {
            return ObserverMessage.OnCompleted<T>(time);
        }

#pragma warning disable IDE0060 // Remove unused parameter (used for type inference purposes)
        protected static Recorded<INotification<T>> OnCompleted<T>(long time, T dummy)
        {
            return ObserverMessage.OnCompleted<T>(time);
        }
#pragma warning restore IDE0060 // Remove unused parameter

        protected static Subscription Subscrible(long start)
        {
            return new Subscription(start);
        }

        protected static Subscription Subscribe(long start, long stop)
        {
            return new Subscription(start, stop);
        }

        #endregion

        #region Settings

        private static readonly Action<IReactivePlatformConfiguration> LoggingConfiguration = c => c.SchedulerType = SchedulerType.Logging;

        private static Action<IReactivePlatformConfiguration> CreateConfiguration(Settings settings)
        {
            return configuration =>
            {
                configuration.SchedulerType = SchedulerType.Logging;
                configuration.EngineOptions = settings.Options;
            };
        }

#pragma warning disable CA1822 // Mark members as static (allows swapping for another implementation later)
        private class Settings
        {
            private const string VersioningTestsSaveKey = "VersioningTestsSave";
            private const string VersioningTestsRunKey = "VersioningTestsRun";
            private const string VersioningTestsOptionsKey = "VersioningTestsOptions";

            public bool VersioningTestsSave => GetData<bool>(VersioningTestsSaveKey);

            public bool VersioningTestsRun => GetData<bool>(VersioningTestsRunKey);

            public Dictionary<string, string> Options => GetData<Dictionary<string, string>>(VersioningTestsOptionsKey);

            private static T GetData<T>(string key)
            {
                var data = AppDomain.CurrentDomain.GetData(key);
                return data != null && typeof(T).IsAssignableFrom(data.GetType()) ? (T)data : default;
            }
        }
#pragma warning restore CA1822

        #endregion
    }
}
