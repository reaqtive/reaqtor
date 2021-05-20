// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

#pragma warning disable CA1034 // Nested types should not be visible. (Legacy approach; kept for compat.)
#pragma warning disable CA1716 // Identifiers should not match keywords. (Rx nomenclature.)

namespace Reaqtor.Remoting.Client
{
    /// <summary>
    /// Constant definitions that must be exactly the same on both the client
    /// and the service. These are just for the Rx operators.
    /// </summary>
    /// <remarks>
    /// TODO: Operator declarations should be auto-generated from the registry, without the need for a constants file.
    /// </remarks>
    public static partial class Constants
    {
        /// <summary>
        /// The resource identifier for the generic identity function.
        /// </summary>
        public const string Identity = "rx://builtin/id";

        /// <summary>
        /// Resource identifiers for observable query operators and factories.
        /// </summary>
        public static partial class Observable
        {
            /// <summary>
            /// Resource identifiers for CombineLatest operators.
            /// </summary>
            public static class CombineLatest
            {
                /// <summary>
                /// The resource identifier for the CombineLatest signature that
                /// takes an observable and a function.
                /// </summary>
                public const string ObservableFunc = "rx://operators/combinelatest";
            }

            /// <summary>
            /// Resource identifiers for DelaySubscription operators.
            /// </summary>
            public static class DelaySubscription
            {
                /// <summary>
                /// The resource identifier for the DelaySubscription signature that
                /// takes a DateTimeOffset.
                /// </summary>
                public const string DateTimeOffset = "rx://operators/delaySubscription/absoluteTime/v2";

                /// <summary>
                /// The resource identifier for the DelaySubscription signature that
                /// takes a TimeSpan.
                /// </summary>
                public const string TimeSpan = "rx://operators/delaySubscription/relativeTime/v2";

                /// <summary>
                /// Resource identifiers for legacy higher order DelaySubscription operators.
                /// </summary>
                public static class V1
                {
                    /// <summary>
                    /// The resource identifier for the DelaySubscription signature that
                    /// takes a DateTimeOffset.
                    /// </summary>
                    public const string DateTimeOffset = "rx://operators/delaySubscription/absoluteTime";

                    /// <summary>
                    /// The resource identifier for the DelaySubscription signature that
                    /// takes a TimeSpan.
                    /// </summary>
                    public const string TimeSpan = "rx://operators/delaySubscription/relativeTime";
                }
            }

            /// <summary>
            /// Resource identifiers for Distinct operators.
            /// </summary>
            public static class Distinct
            {
                /// <summary>
                /// The resource identifier for the Distinct signature that
                /// takes a Func.
                /// </summary>
                public const string Func = "rx://operators/distinct/byKey";

                /// <summary>
                /// The resource identifier for the Distinct signature that
                /// takes no arguments.
                /// </summary>
                public const string NoArgument = "rx://operators/distinct";
            }

            /// <summary>
            /// Resource identifiers for DistinctUntilChanged operators.
            /// </summary>
            public static class DistinctUntilChanged
            {
                /// <summary>
                /// The resource identifier for the DistinctUntilChanged signature that
                /// takes a Func.
                /// </summary>
                public const string Func = "rx://operators/distinctUntilChanged/byKey";

                /// <summary>
                /// The resource identifier for the DistinctUntilChanged signature that
                /// takes no arguments.
                /// </summary>
                public const string NoArgument = "rx://operators/distinctUntilChanged";
            }

            /// <summary>
            /// Resource identifiers for Do operators.
            /// </summary>
            public static class Do
            {
                /// <summary>
                /// The resource identifier for the Do signature that
                /// takes an Action.
                /// </summary>
                public const string OnNext = "rx://operators/do/onNext";

                /// <summary>
                /// The resource identifier for the Do signature that takes an
                /// action for OnNext and OnError events.
                /// </summary>
                public const string OnNextOnError = "rx://operators/do/onNext/onError";

                /// <summary>
                /// The resource identifier for the Do signature that takes an
                /// action for OnNext and OnCompleted events.
                /// </summary>
                public const string OnNextOnCompleted = "rx://operators/do/onNext/onCompleted";

                /// <summary>
                /// The resource identifier for the Do signature that takes an
                /// action for OnNext, OnError, and OnCompleted events.
                /// </summary>
                public const string AllActions = "rx://operators/do/allActions";

                /// <summary>
                /// The resource identifier for the Do signature that
                /// takes an observer.
                /// </summary>
                public const string Observer = "rx://operators/do/observer";

                /// <summary>
                /// The resource identifier for the Do signature that
                /// takes an observer and a projection selector.
                /// </summary>
                public const string ObserverSelector = "rx://operators/do/observer/selector";
            }

            /// <summary>
            /// Resource identifiers for Empty operators.
            /// </summary>
            public static class Empty
            {
                /// <summary>
                /// The resource identifier for the Empty signature that
                /// takes no arguments.
                /// </summary>
                public const string NoArgument = "rx://operators/empty";
            }

            /// <summary>
            /// Resource identifiers for Finally operators.
            /// </summary>
            public static class Finally
            {
                /// <summary>
                /// The resource identifier for the Finally signature that
                /// takes an Action.
                /// </summary>
                public const string Action = "rx://operators/finally";
            }

            /// <summary>
            /// Resource identifiers for FirstAsync operators.
            /// </summary>
            public static class FirstAsync
            {
                /// <summary>
                /// The resource identifier for the FirstAsync signature that
                /// takes no argument.
                /// </summary>
                public const string NoArgument = "rx://operators/first";

                /// <summary>
                /// The resource identifier for the FirstAsync signature that
                /// takes a filter predicate as an argument.
                /// </summary>
                public const string Func = "rx://operators/first/filtered";
            }

            /// <summary>
            /// Resource identifiers for the FirstOrDefaultAsync operators.
            /// </summary>
            public static class FirstOrDefaultAsync
            {
                /// <summary>
                /// The resource identifier for the FirstAsync signature that
                /// takes no argument.
                /// </summary>
                public const string NoArgument = "rx://operators/firstOrDefault";

                /// <summary>
                /// The resource identifier for the FirstAsync signature that
                /// takes a filter predicate as an argument.
                /// </summary>
                public const string Func = "rx://operators/firstOrDefault/filtered";
            }

            /// <summary>
            /// Resource identifiers for the GroupBy operators.
            /// </summary>
            public static class GroupBy
            {
                /// <summary>
                /// The resource identifier for the GroupBy signature that
                /// takes a key selector as an argument.
                /// </summary>
                public const string Key = "rx://operators/groupBy";

                /// <summary>
                /// The resource identifier for the GroupBy signature that
                /// takes a key selector and an element selector as arguments.
                /// </summary>
                public const string KeyElement = "rx://operators/groupBy/element";
            }

            /// <summary>
            /// Resource identifiers for identity functions for operators.
            /// </summary>
            public static class Identity
            {
                /// <summary>
                /// The resource identifier for the identity function for IObservable instances.
                /// </summary>
                public const string Observable = "rx:/observable/id";
            }

            /// <summary>
            /// Resource identifiers for the IgnoreElements operators.
            /// </summary>
            public static class IgnoreElements
            {
                /// <summary>
                /// The resource identifier for the IgnoreElements signature
                /// that takes no argument.
                /// </summary>
                public const string NoArgument = "rx://observable/ignoreElements";
            }

            /// <summary>
            /// Resource identifiers for Merge operators.
            /// </summary>
            public static class Merge
            {
                /// <summary>
                /// The resource identifer for the Merge signature with no arguments.
                /// </summary>
                public const string NoArgument = "rx://operators/merge";

                /// <summary>
                /// The resource identifer for the Merge signature with two IObservable instances.
                /// </summary>
                public const string Binary = "rx://operators/merge/2";

                /// <summary>
                /// The resource identifer for the Merge signature with N IObservable instances.
                /// </summary>
                public const string N = "rx://operators/merge/N";
            }

            /// <summary>
            /// Resource identifiers for Retry operators.
            /// </summary>
            public static class Retry
            {
                /// <summary>
                /// The resource identifer for the Retry signature with no arguments.
                /// </summary>
                public const string NoArgument = "rx://operators/retry";

                /// <summary>
                /// The resource identifer for the Retry signature that
                /// takes a count.
                /// </summary>
                public const string Count = "rx://operators/retry/count";
            }

            /// <summary>
            /// Resource identifiers for Sample operators.
            /// </summary>
            public static class Sample
            {
                /// <summary>
                /// The resource identifier for the Sample signature that
                /// takes a TimeSpan.
                /// </summary>
                public const string TimeSpan = "rx://operators/sample/period";

                /// <summary>
                /// The resource identifier for the Sample signature that
                /// takes a sampler observable.
                /// </summary>
                public const string Observable = "rx://operators/sample/signal";
            }

            /// <summary>
            /// Resource identifiers for Select operators.
            /// </summary>
            public static class Select
            {
                /// <summary>
                /// The resource identifier for the Select signature that
                /// takes a Func.
                /// </summary>
                public const string Func = "rx://operators/map";

                /// <summary>
                /// The resource identifier for the Select signature that
                /// takes an indexed Func.
                /// </summary>
                public const string IndexedFunc = "rx://operators/map/indexed";
            }

            /// <summary>
            /// Resource identifiers for SelectMany operators.
            /// </summary>
            public static class SelectMany
            {
                /// <summary>
                /// The resource identifier for the SelectMany signature that
                /// takes a Func.
                /// </summary>
                public const string Func = "rx://operators/bind";

                /// <summary>
                /// The resource identifier for the SelectMany signature that
                /// takes 2 Funcs.
                /// </summary>
                public const string FuncFunc = "rx://operators/bind/map";
            }

            /// <summary>
            /// Resource identifiers for SingleAsync operators.
            /// </summary>
            public static class SingleAsync
            {
                /// <summary>
                /// The resource identifier for the SingleAsync signature that
                /// takes no argument.
                /// </summary>
                public const string NoArgument = "rx://operators/single";

                /// <summary>
                /// The resource identifier for the SingleAsync signature that
                /// takes a filter predicate as an argument.
                /// </summary>
                public const string Func = "rx://operators/single/filtered";
            }

            /// <summary>
            /// Resource identifiers for SingleOrDefaultAsync
            /// </summary>
            public static class SingleOrDefaultAsync
            {
                /// <summary>
                /// The resource identifier for the SingleOrDefaultAsync signature
                /// that takes no argument.
                /// </summary>
                public const string NoArgument = "rx://operators/singleOrDefault";

                /// <summary>
                /// The resource identifier for the SingleOrDefaultAsync signature
                /// that takes a filter predicate as an argument.
                /// </summary>
                public const string Func = "rx://operators/singleOrDefault/filtered";
            }

            /// <summary>
            /// Resource identifiers for Skip operators.
            /// </summary>
            public static class Skip
            {
                /// <summary>
                /// The resource identifier for the Skip signature that
                /// takes an int.
                /// </summary>
                public const string Count = "rx://operators/skip/count";

                /// <summary>
                /// The resource identifier for the Skip signature that
                /// takes a TimeSpan.
                /// </summary>
                public const string TimeSpan = "rx://operators/skip/relativeTime";
            }

            /// <summary>
            /// Resource identifiers for SkipUntil operators.
            /// </summary>
            public static class SkipUntil
            {
                /// <summary>
                /// The resource identifier for the SkipUntil signature that
                /// takes a observable trigger.
                /// </summary>
                public const string Observable = "rx://operators/skipUntil/signal";

                /// <summary>
                /// The resource identifier for the SkipUntil signature that
                /// takes a DateTimeOffset.
                /// </summary>
                public const string DateTimeOffset = "rx://operators/skipUntil/absoluteTime";
            }

            /// <summary>
            /// Resource identifiers for SkipWhile operators.
            /// </summary>
            public static class SkipWhile
            {
                /// <summary>
                /// The resource identifier for the SkipWhile signature that
                /// takes a Func.
                /// </summary>
                public const string Func = "rx://operators/skipWhile";

                /// <summary>
                /// The resource identifier for the SkipWhile signature that
                /// takes a Func with an index parameter.
                /// </summary>
                public const string IndexedFunc = "rx://operators/skipWhile/indexed";
            }

            /// <summary>
            /// Resource identifiers for StartWith operators.
            /// </summary>
            public static class StartWith
            {
                /// <summary>
                /// The resource identifier for the StartWith signature that
                /// takes an array.
                /// </summary>
                public const string Array = "rx://operators/startWith";
            }

            /// <summary>
            /// Resource identifiers for Switch operators.
            /// </summary>
            public static class Switch
            {
                /// <summary>
                /// The resource identifier for the Switch signature that
                /// takes no argument.
                /// </summary>
                public const string NoArgument = "rx://operators/switchLatest";
            }

            /// <summary>
            /// Resource identifiers for Take operators.
            /// </summary>
            public static class Take
            {
                /// <summary>
                /// The resource identifier for the Take signature that
                /// takes an int.
                /// </summary>
                public const string Count = "rx://operators/take/count";

                /// <summary>
                /// The resource identifier for the Take signature that
                /// takes a TimeSpan.
                /// </summary>
                public const string TimeSpan = "rx://operators/take/relativeTime";
            }

            /// <summary>
            /// Resource identifiers for TakeWhile operators.
            /// </summary>
            public static class TakeWhile
            {
                /// <summary>
                /// The resource identifier for the TakeWhile signature that
                /// takes a Func.
                /// </summary>
                public const string Func = "rx://operators/takeWhile";

                /// <summary>
                /// The resource identifier for the Take signature that
                /// takes an index Func.
                /// </summary>
                public const string IndexedFunc = "rx://operators/takeWhile/indexed";
            }

            /// <summary>
            /// Resource identifiers for TakeUntil operators.
            /// </summary>
            public static class TakeUntil
            {
                /// <summary>
                /// The resource identifier for the TakeUntil signature that
                /// takes a DateTimeOffset.
                /// </summary>
                public const string DateTimeOffset = "rx://operators/takeUntil/absoluteTime";

                /// <summary>
                /// The resource identifier for the TakeUntil signature that
                /// takes an Observable.
                /// </summary>
                public const string Observable = "rx://operators/takeUntil/signal";
            }

            /// <summary>
            /// Resource identifiers for Throttle operators.
            /// </summary>
            public static class Throttle
            {
                /// <summary>
                /// The resource identifier for the Throttle signature that
                /// takes a TimeSpan.
                /// </summary>
                public const string TimeSpan = "rx://operators/throttle/relativeTime";
            }

            /// <summary>
            /// Resource identifiers for Timer factories.
            /// </summary>
            public static class Timer
            {
                /// <summary>
                /// The resource identifier for the timer signature that takes
                /// a DateTimeOffset dueTime.
                /// </summary>
                public const string DateTimeOffset = "rx://operators/timer/absoluteTime";

                /// <summary>
                /// The resource identifier for the timer signature that takes
                /// a DateTimeOffset dueTime and a TimeSpan period.
                /// </summary>
                public const string DateTimeOffsetTimeSpan = "rx://operators/timer/absoluteTime/period";

                /// <summary>
                /// The resource identifier for the timer signature that takes
                /// a TimeSpan dueTime.
                /// </summary>
                public const string TimeSpan = "rx://operators/timer/relativeTime";

                /// <summary>
                /// The resource identifier for the timer signature that takes
                /// a TimeSpan dueTime and a TimeSpan period.
                /// </summary>
                public const string TimeSpanTimeSpan = "rx://operators/timer/relativeTime/period";
            }

            /// <summary>
            /// Resource identifiers for Where operators.
            /// </summary>
            public static class Where
            {
                /// <summary>
                /// The resource identifier for the Where signature that
                /// takes a Func.
                /// </summary>
                public const string Func = "rx://operators/filter";

                /// <summary>
                /// The resource identifier for the Where signature that
                /// takes an indexed Func.
                /// </summary>
                public const string IndexedFunc = "rx://operators/filter/indexed";
            }

            /// <summary>
            /// Resource identifiers for Window operators.
            /// </summary>
            public static class Window
            {
                /// <summary>
                /// The resource identifier for the Window signature that
                /// takes a TimeSpan duration.
                /// </summary>
                public const string TimeDuration = "rx://operators/window/time/duration";

                /// <summary>
                /// The resource identifier for the Window signature that
                /// takes a TimeSpan duration and a TimeSpan shift.
                /// </summary>
                public const string TimeDurationShift = "rx://operators/window/time/duration/shift";

                /// <summary>
                /// The resource identifier for the Window signature that
                /// takes an int count.
                /// </summary>
                public const string Count = "rx://operators/window/count";

                /// <summary>
                /// The resource identifier for the Window signature that
                /// takes an int count and an int skip.
                /// </summary>
                public const string CountSkip = "rx://operators/window/count/skip";

                /// <summary>
                /// The resource identifier for the Window signature that
                /// takes a TimeSpan duration and an int count.
                /// </summary>
                public const string TimeCount = "rx://operators/window/duration/count";
            }

            /// <summary>
            /// Resource identifiers for Buffer operators.
            /// </summary>
            public static class Buffer
            {
                /// <summary>
                /// The resource identifier for the Buffer signature that
                /// takes a TimeSpan duration.
                /// </summary>
                public const string TimeDuration = "rx://operators/buffer/time/duration";

                /// <summary>
                /// The resource identifier for the Buffer signature that
                /// takes a TimeSpan duration and a TimeSpan shift.
                /// </summary>
                public const string TimeDurationShift = "rx://operators/buffer/time/duration/shift";

                /// <summary>
                /// The resource identifier for the Buffer signature that
                /// takes an int count.
                /// </summary>
                public const string Count = "rx://operators/buffer/count";

                /// <summary>
                /// The resource identifier for the Buffer signature that
                /// takes an int count and an int skip.
                /// </summary>
                public const string CountSkip = "rx://operators/buffer/count/skip";

                /// <summary>
                /// The resource identifier for the Buffer signature that
                /// takes a TimeSpan duration and an int count.
                /// </summary>
                public const string TimeCount = "rx://operators/buffer/duration/count";
            }

            /// <summary>
            /// Resource identifiers for SequenceEqual operators.
            /// </summary>
            public static class SequenceEqual
            {
                /// <summary>
                /// The resource identifier for the SequenceEqual signature that
                /// takes two sequences.
                /// </summary>
                public const string NoArgument = "rx://operators/sequenceEqual";
            }
        }

        /// <summary>
        /// Resource identifiers for observers.
        /// </summary>
        public static class Observer
        {
            /// <summary>
            /// The resource identifier for the no-op observer.
            /// </summary>
            public const string Nop = "rx://observers/nop";
        }
    }
}
