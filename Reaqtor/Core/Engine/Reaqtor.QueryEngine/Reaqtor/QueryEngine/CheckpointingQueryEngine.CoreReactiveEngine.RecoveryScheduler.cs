// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

#if SUPPORT_SYS_CONFIG
using System.Configuration;
#endif

namespace Reaqtor.QueryEngine
{
    public partial class CheckpointingQueryEngine
    {
        private partial class CoreReactiveEngine
        {
            /// <summary>
            /// TPL scheduler for recovery tasks to limit the degree of concurrency for recovery, which is useful for environments
            /// where many QE instances can be loaded in the same process and a recovery of a QE can take place while other QEs are
            /// processing events (e.g. in a Service Fabric setup where a single host process can contain primaries and secondaries
            /// of many stateful services).
            /// </summary>
            private sealed class RecoveryScheduler : TaskScheduler, IDisposable
            {
#if SUPPORT_SYS_CONFIG
                private const string MaxRecoveryUtilizationFactorKey = "MaxRecoveryUtilitizationFactor";
#endif

                private readonly BlockingCollection<Task> _tasks;
                private readonly Thread[] _threads;

                private RecoveryScheduler()
                {
                    var threadCount = Math.Max(1, (int)(Environment.ProcessorCount * MaxRecoveryUtilizationFactor));

                    _tasks = new BlockingCollection<Task>();
                    _threads = new Thread[threadCount];

                    for (var i = 0; i < threadCount; ++i)
                    {
                        var thread = new Thread(Run) { IsBackground = true };
                        _threads[i] = thread;
                        thread.Start();
                    }
                }

                public static new RecoveryScheduler Default { get; } = new RecoveryScheduler();

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable IDE0025 // Use expression body for properties. (Due to #if.)

                private static double MaxRecoveryUtilizationFactor
                {
                    get
                    {
#if SUPPORT_SYS_CONFIG // TODO: Support another means to set this config value via the top-level ConfigurationOptions.
                        var appSetting = ConfigurationManager.AppSettings[MaxRecoveryUtilizationFactorKey];
                        if (appSetting != null && double.TryParse(appSetting, out double result))
                        {
                            return result;
                        }
#endif

                        return 1.0;
                    }
                }

#pragma warning restore IDE0025
#pragma warning restore IDE0079

                protected override IEnumerable<Task> GetScheduledTasks() => _tasks.ToArray();

                protected override void QueueTask(Task task) => _tasks.Add(task);

                protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued) => false;

                private void Run()
                {
                    foreach (var task in _tasks.GetConsumingEnumerable())
                    {
                        TryExecuteTask(task);
                    }
                }

                public void Dispose()
                {
                    //
                    // NB: A proper IDisposable.Dispose implementation is a bit of a mood point given that
                    //     we use a static singleton instance. Disposing that single instance will render all
                    //     uses of a query engine in the current app domain impossible for recovery.
                    //
                    //     We do absolutely want a single instance of the recovery scheduler when more than
                    //     one query engine is in use, in order to avoid allocating competing threads. An
                    //     alternative solution would be to have some reference counting mechanism to keep
                    //     track of engines that need a recovery scheduler and lazily allocate and deallocate
                    //     one (at the expense of repeated thread creation). Or, we could have a proper host
                    //     abstraction for QEs, containing facilities such as the PhysicalScheduler and the
                    //     recovery scheduler used here.
                    //
                    //     For the time being, we'll leave the singleton alone; if no QEs are in use, it just
                    //     leaves some idle threads behind, and the host process is typically just dedicated to
                    //     doing event processing, so having these threads is expected. However, we'll
                    //     keep the IDisposable.Dispose implementation in case we want to move the facility up.
                    //

                    _tasks.CompleteAdding();

                    foreach (var thread in _threads)
                    {
                        thread.Join();
                    }

                    _tasks.Dispose();
                }
            }
        }
    }
}
