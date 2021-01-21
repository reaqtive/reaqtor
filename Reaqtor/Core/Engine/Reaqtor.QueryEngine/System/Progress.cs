// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace System
{
    /// <summary>
    /// Provides a set of methods to work with <see cref="IProgress{T}"/> objects.
    /// </summary>
    public static class Progress
    {
        #region Catch

        /// <summary>
        /// Protects a progress reporter against exceptions thrown by the Report method of the specified progress update provider.
        /// </summary>
        /// <typeparam name="T">Type of the progress update value.</typeparam>
        /// <param name="progress">Progress update provider to handle exceptions for.</param>
        /// <returns>Progress update provider with exception handling behavior applied.</returns>
        public static IProgress<T> Catch<T>(this IProgress<T> progress)
        {
            if (progress == null)
                throw new ArgumentNullException(nameof(progress));

            return CatchCore<T, Exception>(progress, _ => true);
        }

        /// <summary>
        /// Protects a progress reporter against exceptions of the specified type thrown by the Report method of the specified progress update provider.
        /// </summary>
        /// <typeparam name="T">Type of the progress update value.</typeparam>
        /// <typeparam name="TException">Type of the exception to handle.</typeparam>
        /// <param name="progress">Progress update provider to handle exceptions for.</param>
        /// <returns>Progress update provider with exception handling behavior applied.</returns>
        public static IProgress<T> Catch<T, TException>(this IProgress<T> progress) where TException : Exception
        {
            if (progress == null)
                throw new ArgumentNullException(nameof(progress));

            return CatchCore<T, TException>(progress, _ => true);
        }

        /// <summary>
        /// Handles exceptions of the specified type thrown by the Report method of the specified progress update provider using the specified exception handler.
        /// </summary>
        /// <typeparam name="T">Type of the progress update value.</typeparam>
        /// <typeparam name="TException">Type of the exception to handle.</typeparam>
        /// <param name="progress">Progress update provider to handle exceptions for.</param>
        /// <param name="handler">Exception handler function. If the function returns <c>true</c>, the exception is considered to be handled. Otherwise, the exception will propagate.</param>
        /// <returns>Progress update provider with exception handling behavior applied.</returns>
        public static IProgress<T> Catch<T, TException>(this IProgress<T> progress, Func<TException, bool> handler) where TException : Exception
        {
            if (progress == null)
                throw new ArgumentNullException(nameof(progress));
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            return CatchCore<T, TException>(progress, handler);
        }

        private static IProgress<T> CatchCore<T, TException>(IProgress<T> progress, Func<TException, bool> handler) where TException : Exception
        {
            return new AnonymousProgress<T>(x =>
            {
                try
                {
                    progress.Report(x);
                }
                catch (TException e)
                {
                    if (!handler(e))
                        throw;
                }
            });
        }

        #endregion

        #region Create

        /// <summary>
        /// Creates a new progress update provider with the specified progress reporting action.
        /// </summary>
        /// <typeparam name="T">Type of the progress update value.</typeparam>
        /// <param name="report">Progress reporting action.</param>
        /// <returns>Progress update provider using the specified reporting action for its Report implementation.</returns>
        public static IProgress<T> Create<T>(Action<T> report)
        {
            if (report == null)
                throw new ArgumentNullException(nameof(report));

            return new AnonymousProgress<T>(report);
        }

        #endregion

        #region DistinctUntilChanged

        /// <summary>
        /// Ensures distinct consecutive progress values get reported to the specified progress update provider using the default comparer to compare progress values.
        /// </summary>
        /// <typeparam name="T">Type of the progress update value.</typeparam>
        /// <param name="progress">Progress update provider to report consecutive distinct progress values to.</param>
        /// <returns>Progress update provider whose progress reports will be filtered for consecutive distinct progress values. If a newly reported progress value is different from the previous reported progress value, it will get propagated to the specified progress update provider.</returns>
        public static IProgress<T> DistinctUntilChanged<T>(this IProgress<T> progress)
        {
            if (progress == null)
                throw new ArgumentNullException(nameof(progress));

            return DistinctUntilChangedCore(progress, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// Ensures distinct consecutive progress values get reported to the specified progress update provider using the specified comparer to compare progress values.
        /// </summary>
        /// <typeparam name="T">Type of the progress update value.</typeparam>
        /// <param name="progress">Progress update provider to report consecutive distinct progress values to.</param>
        /// <param name="comparer">Comparer used to ensure consecutive progress values are distinct.</param>
        /// <returns>Progress update provider whose progress reports will be filtered for consecutive distinct progress values. If a newly reported progress value is different from the previous reported progress value, it will get propagated to the specified progress update provider.</returns>
        public static IProgress<T> DistinctUntilChanged<T>(this IProgress<T> progress, IEqualityComparer<T> comparer)
        {
            if (progress == null)
                throw new ArgumentNullException(nameof(progress));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return DistinctUntilChangedCore(progress, comparer);
        }

        private static IProgress<T> DistinctUntilChangedCore<T>(IProgress<T> progress, IEqualityComparer<T> comparer)
        {
            var hasLatest = false;
            var latest = default(T);

            return new AnonymousProgress<T>(x =>
            {
                if (!hasLatest)
                {
                    latest = x;
                    hasLatest = true;
                    progress.Report(x);
                }
                else
                {
                    if (!comparer.Equals(latest, x))
                    {
                        latest = x;
                        progress.Report(x);
                    }
                }
            });
        }

        #endregion

        #region Monotonic

        /// <summary>
        /// Ensures monotonically increasing progress values get reported to the specified progress update provider using the default comparer to ensure monotonicity of the reported progress values.
        /// </summary>
        /// <typeparam name="T">Type of the progress update value.</typeparam>
        /// <param name="progress">Progress update provider to report monotonically increasing progress values to.</param>
        /// <returns>Progress update provider whose progress reports will be filtered for monotonic increases. If a newly reported progress value is higher than the previous reported progress value, it will get propagated to the specified progress update provider.</returns>
        public static IProgress<T> Monotonic<T>(this IProgress<T> progress)
        {
            if (progress == null)
                throw new ArgumentNullException(nameof(progress));

            return MonotonicCore<T>(progress, Comparer<T>.Default);
        }

        /// <summary>
        /// Ensures monotonically increasing progress values get reported to the specified progress update provider using the specified comparer to ensure monotonicity of the reported progress values.
        /// </summary>
        /// <typeparam name="T">Type of the progress update value.</typeparam>
        /// <param name="progress">Progress update provider to report monotonically increasing progress values to.</param>
        /// <param name="comparer">Comparer used to ensure monotonicity of the reported progress values.</param>
        /// <returns>Progress update provider whose progress reports will be filtered for monotonic increases. If a newly reported progress value is higher than the previous reported progress value, it will get propagated to the specified progress update provider.</returns>
        public static IProgress<T> Monotonic<T>(this IProgress<T> progress, IComparer<T> comparer)
        {
            if (progress == null)
                throw new ArgumentNullException(nameof(progress));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return MonotonicCore<T>(progress, comparer);
        }

        private static IProgress<T> MonotonicCore<T>(IProgress<T> progress, IComparer<T> comparer)
        {
            var hasLatest = false;
            var latest = default(T);

            return new AnonymousProgress<T>(x =>
            {
                if (!hasLatest)
                {
                    latest = x;
                    hasLatest = true;
                    progress.Report(x);
                }
                else
                {
                    if (comparer.Compare(latest, x) < 0)
                    {
                        latest = x;
                        progress.Report(x);
                    }
                }
            });
        }

        #endregion

        #region Range

        /// <summary>
        /// Limits the range of values reported on the specified progress update provider using the default comparer.
        /// </summary>
        /// <typeparam name="T">Type of the progress update value.</typeparam>
        /// <param name="progress">Progress update provider whose progress values to limit to the specified range.</param>
        /// <param name="minValue">Minimum progress value that may be reported to the specified progress update provider.</param>
        /// <param name="maxValue">Maximum progress value that may be reported to the specified progress update provider.</param>
        /// <returns>Returns a progress update provider that will limit the range of progress values reported to the specified progress update provider.</returns>
        public static IProgress<T> Range<T>(this IProgress<T> progress, T minValue, T maxValue)
        {
            if (progress == null)
                throw new ArgumentNullException(nameof(progress));

            return RangeCore<T>(progress, minValue, maxValue, Comparer<T>.Default);
        }

        /// <summary>
        /// Limits the range of values reported on the specified progress update provider using the specified comparer.
        /// </summary>
        /// <typeparam name="T">Type of the progress update value.</typeparam>
        /// <param name="progress">Progress update provider whose progress values to limit to the specified range.</param>
        /// <param name="minValue">Minimum progress value that may be reported to the specified progress update provider.</param>
        /// <param name="maxValue">Maximum progress value that may be reported to the specified progress update provider.</param>
        /// <param name="comparer">Comparer used to perform the range check of reported progress values.</param>
        /// <returns>Returns a progress update provider that will limit the range of progress values reported to the specified progress update provider.</returns>
        public static IProgress<T> Range<T>(this IProgress<T> progress, T minValue, T maxValue, IComparer<T> comparer)
        {
            if (progress == null)
                throw new ArgumentNullException(nameof(progress));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return RangeCore<T>(progress, minValue, maxValue, comparer);
        }

        private static IProgress<T> RangeCore<T>(IProgress<T> progress, T minValue, T maxValue, IComparer<T> comparer)
        {
            return new AnonymousProgress<T>(x =>
            {
                if (comparer.Compare(minValue, x) <= 0 && comparer.Compare(x, maxValue) <= 0)
                {
                    progress.Report(x);
                }
            });
        }

        #endregion

        #region Select

        /// <summary>
        /// Projects progress values reported on the resulting progress update provider prior to propagating them to the specified progress update provider.
        /// </summary>
        /// <typeparam name="T">Type of the progress update value received by the resulting progress update provider.</typeparam>
        /// <typeparam name="TResult">Type of the progress update value to report to the specified progress update provider.</typeparam>
        /// <param name="progress">Progress update provider to report progress to.</param>
        /// <param name="selector">Selector function to project reported progress values.</param>
        /// <returns>Progress update provider whose reported progress values will be projected prior to propagating them to the specified progress update provider.</returns>
        public static IProgress<T> Select<T, TResult>(this IProgress<TResult> progress, Func<T, TResult> selector)
        {
            if (progress == null)
                throw new ArgumentNullException(nameof(progress));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new AnonymousProgress<T>(x =>
            {
                progress.Report(selector(x));
            });
        }

        #endregion

        #region Split

        /// <summary>
        /// Splits a progress update provider into two progress update providers that combine their progress using the specified combiner function.
        /// </summary>
        /// <typeparam name="T">Type of the progress update value.</typeparam>
        /// <typeparam name="T1">Type of the progress update value reported by the first resulting progress update provider.</typeparam>
        /// <typeparam name="T2">Type of the progress update value reported by the second resulting progress update provider.</typeparam>
        /// <param name="progress">Progress update provider to split into two progress update providers.</param>
        /// <param name="combine">Combine function used to combine the latest progress of the two returned progress update providers.</param>
        /// <returns>Tuple of two progress update providers whose latest progress values will get combined into progress values reported to the specified progress update provider.</returns>
        public static Tuple<IProgress<T1>, IProgress<T2>> Split<T, T1, T2>(this IProgress<T> progress, Func<T1, T2, T> combine)
        {
            if (progress == null)
                throw new ArgumentNullException(nameof(progress));
            if (combine == null)
                throw new ArgumentNullException(nameof(combine));

            var gate = new object();

            var _t1 = default(T1);
            var _t2 = default(T2);

            var report = new Action(() =>
            {
                progress.Report(combine(_t1, _t2));
            });

            return new Tuple<IProgress<T1>, IProgress<T2>>(
                new AnonymousProgress<T1>(t1 => { lock (gate) { _t1 = t1; report(); } }),
                new AnonymousProgress<T2>(t2 => { lock (gate) { _t2 = t2; report(); } })
            );
        }

        /// <summary>
        /// Splits a progress update provider into the specified number of progress update providers that combine their progress using the specified combiner function.
        /// </summary>
        /// <typeparam name="T">Type of the progress update value.</typeparam>
        /// <param name="progress">Progress update provider to split into the specified number of progress update providers.</param>
        /// <param name="count">The number of progress update providers to return.</param>
        /// <param name="combine">Combine function used to combine the latest progress of the returned progress update providers.</param>
        /// <returns>Array of progress update providers whose latest progress values will get combined into progress values reported to the specified progress update provider.</returns>
        public static IProgress<T>[] Split<T>(this IProgress<T> progress, int count, Func<T[], T> combine)
        {
            if (progress == null)
                throw new ArgumentNullException(nameof(progress));
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (combine == null)
                throw new ArgumentNullException(nameof(combine));

            var values = new T[count];
            return Enumerable.Range(0, count).Select(i => new AnonymousProgress<T>(x => { lock (values) { values[i] = x; progress.Report(combine(values)); } })).ToArray();
        }

        #endregion

        #region SplitWeight

        /// <summary>
        /// Splits the specified progress update provider into an array of progress update providers whose reported progress values will be combined according to the specified weights.
        /// </summary>
        /// <typeparam name="T">Type of the progress update value.</typeparam>
        /// <typeparam name="TWeight">Type of weights.</typeparam>
        /// <param name="progress">Progress update provider to split according to the specified weights.</param>
        /// <param name="addProgress">Function to combine progress values.</param>
        /// <param name="addWeight">Function to combine weights.</param>
        /// <param name="multiply">Function to multiple a progress value and a weight.</param>
        /// <param name="divide">Function to divide a progress value by a weight.</param>
        /// <param name="weights">Array of weights to split the progress update provider by.</param>
        /// <returns>Array of progress update providers that combine into progress value reports on the specified progress update provider according to the array of weights specified.</returns>
        public static IProgress<T>[] SplitWeight<T, TWeight>(this IProgress<T> progress, Func<T, T, T> addProgress, Func<TWeight, TWeight, TWeight> addWeight, Func<T, TWeight, T> multiply, Func<T, TWeight, T> divide, params TWeight[] weights)
        {
            if (progress == null)
                throw new ArgumentNullException(nameof(progress));
            if (addProgress == null)
                throw new ArgumentNullException(nameof(addProgress));
            if (addWeight == null)
                throw new ArgumentNullException(nameof(addWeight));
            if (multiply == null)
                throw new ArgumentNullException(nameof(multiply));
            if (divide == null)
                throw new ArgumentNullException(nameof(divide));
            if (weights == null)
                throw new ArgumentNullException(nameof(weights));

            if (weights.Length == 0)
                return Array.Empty<IProgress<T>>();

            var totalWeight = weights.Aggregate(addWeight);

            return progress.Split(weights.Length, xs => divide(xs.Select((x, i) => multiply(x, weights[i])).Aggregate(addProgress), totalWeight));
        }

        /// <summary>
        /// Splits the specified progress update provider into an array of progress update providers whose reported progress values will be combined according to the specified weights.
        /// </summary>
        /// <param name="progress">Progress update provider to split according to the specified weights.</param>
        /// <param name="weights">Array of weights to split the progress update provider by.</param>
        /// <returns>Array of progress update providers that combine into progress value reports on the specified progress update provider according to the array of weights specified.</returns>
        public static IProgress<int>[] SplitWeight(this IProgress<int> progress, params int[] weights)
        {
            if (progress == null)
                throw new ArgumentNullException(nameof(progress));
            if (weights == null)
                throw new ArgumentNullException(nameof(weights));

            return progress.SplitWeight((x, y) => x + y, (x, y) => x + y, (x, y) => x * y, (x, y) => x / y, weights);
        }

        #endregion

        #region Throttle

        /// <summary>
        /// Throttles progress reporting to the specified progress update provider using the specified throttling interval.
        /// </summary>
        /// <typeparam name="T">Type of the progress update value.</typeparam>
        /// <param name="progress">Progress update provider to throttle progress reporting for.</param>
        /// <param name="interval">Throttling interval. If a progress value is reported within the specified interval from the previous progress value report, it will get discarded.</param>
        /// <returns>Progress update provider throttling progress reports to the specified progress update provider.</returns>
        public static IProgress<T> Throttle<T>(this IProgress<T> progress, TimeSpan interval)
        {
            if (progress == null)
                throw new ArgumentNullException(nameof(progress));
            if (interval < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(interval));

            return ThrottleCore<T>(progress, interval, new StopwatchImpl(new Stopwatch()));
        }

        /// <summary>
        /// Throttles progress reporting to the specified progress update provider using the specified throttling interval and stopwatch.
        /// </summary>
        /// <typeparam name="T">Type of the progress update value.</typeparam>
        /// <param name="progress">Progress update provider to throttle progress reporting for.</param>
        /// <param name="interval">Throttling interval. If a progress value is reported within the specified interval from the previous progress value report, it will get discarded.</param>
        /// <param name="stopwatch">Stopwatch to use for measuring throttling intervals.</param>
        /// <returns>Progress update provider throttling progress reports to the specified progress update provider.</returns>
        public static IProgress<T> Throttle<T>(this IProgress<T> progress, TimeSpan interval, IStopwatch stopwatch)
        {
            if (progress == null)
                throw new ArgumentNullException(nameof(progress));
            if (interval < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(interval));
            if (stopwatch == null)
                throw new ArgumentNullException(nameof(stopwatch));

            return ThrottleCore<T>(progress, interval, stopwatch);
        }

        private static IProgress<T> ThrottleCore<T>(IProgress<T> progress, TimeSpan interval, IStopwatch stopwatch)
        {
            var hasFirst = false;

            return new AnonymousProgress<T>(x =>
            {
                if (!hasFirst)
                {
                    hasFirst = true;
                    progress.Report(x);
                    stopwatch.Start();
                }
                else if (stopwatch.Elapsed >= interval)
                {
                    stopwatch.Restart();
                    progress.Report(x);
                }
            });
        }

        #endregion

        #region ToPercentageIncrement

        /// <summary>
        /// Obtains an action that reports progress percentage to the specified progress update provider.
        /// </summary>
        /// <param name="progress">Progress update provider to report progress to. Reported values will be between 0 and 100 (bounds included).</param>
        /// <param name="total">Total number of action invocations required to reach a 100% progress.</param>
        /// <returns>Action to invoke for each increment of progress.</returns>
        public static Action ToPercentageIncrement(this IProgress<int> progress, int total)
        {
            if (progress == null)
                throw new ArgumentNullException(nameof(progress));
            if (total < 0)
                throw new ArgumentOutOfRangeException(nameof(total));

            if (total > 0)
            {
                var gate = new object();

                var i = 0;
                return () =>
                {
                    lock (gate)
                    {
                        if (++i <= total)
                        {
                            progress.Report(i * 100 / total);
                        }
                    }
                };
            }
            else
            {
                progress.Report(100);

                return () => { };
            }
        }

        #endregion

        #region Where

        /// <summary>
        /// Filters progress values that get propagated to the specified progress update provider.
        /// </summary>
        /// <typeparam name="T">Type of the progress update value.</typeparam>
        /// <param name="progress">Progress update provider to filter progress values for.</param>
        /// <param name="predicate">Predicate to filter progress notifications.</param>
        /// <returns>Progress update provider appyling the filter prior to propagating the value to the specified progress update provider.</returns>
        public static IProgress<T> Where<T>(this IProgress<T> progress, Func<T, bool> predicate)
        {
            if (progress == null)
                throw new ArgumentNullException(nameof(progress));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return new AnonymousProgress<T>(x =>
            {
                if (predicate(x))
                {
                    progress.Report(x);
                }
            });
        }

        #endregion

        private sealed class AnonymousProgress<T> : IProgress<T>
        {
            private readonly Action<T> _report;

            public AnonymousProgress(Action<T> report) => _report = report;

            public void Report(T value) => _report(value);
        }
    }
}
