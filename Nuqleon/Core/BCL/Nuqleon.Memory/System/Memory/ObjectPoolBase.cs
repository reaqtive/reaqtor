// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// The contents of this file are adapted from ObjectPool`1.cs in Roslyn, see
//
//   https://github.com/dotnet/roslyn/blob/release/dev16.10/src/Dependencies/PooledObjects/ObjectPool%601.cs
//

//
// Revision history:
//
//   BD - 07/02/2014 - Created this type.
//

#region #define

//-------------------------------------------------------------------------------------
// Define TRACE_LEAKS to get additional diagnostics that can lead to the leak sources.
// Note: it will make everything about 2-3x slower.
//
#if DEBUG
#define TRACE_LEAKS
#endif

//-------------------------------------------------------------------------------------
// Define DETECT_LEAKS to detect possible leaks. This is handy for DEBUG builds.
//
#if DEBUG
#define DETECT_LEAKS
#endif

//-------------------------------------------------------------------------------------
// Define ENABLE_LOGGING to keep allocation/free logs. This is handy for DEBUG builds.
// Note: it will make everything about 5x slower.
//
#if DEBUG
#define ENABLE_LOGGING
#endif

//-------------------------------------------------------------------------------------
// Define ALL_DIAGNOSTICS to enable the whole shebang of diagnostics
//
// #define ALL_DIAGNOSTICS

#if ALL_DIAGNOSTICS
#define TRACE_LEAKS
#define DETECT_LEAKS
#define ENABLE_LOGGING
#endif

#endregion

#region Using directives

using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;

#if ENABLE_LOGGING
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
#endif

#if DETECT_LEAKS || ENABLE_LOGGING
using System.Globalization;
#endif

#endregion

namespace System.Memory
{
    //==========================================================================\\
    //    ____  _     _           _   _____            _ ____                   \\
    //   / __ \| |   (_)         | | |  __ \          | |  _ \                  \\
    //  | |  | | |__  _  ___  ___| |_| |__) |__   ___ | | |_) | __ _ ___  ___   \\
    //  | |  | | '_ \| |/ _ \/ __| __|  ___/ _ \ / _ \| |  _ < / _` / __|/ _ \  \\
    //  | |__| | |_) | |  __/ (__| |_| |  | (_) | (_) | | |_) | (_| \__ \  __/  \\
    //   \____/|_.__/| |\___|\___|\__|_|   \___/ \___/|_|____/ \__,_|___/\___|  \\
    //              _/ |                                                        \\
    //             |__/                                                         \\
    //==========================================================================\\

    /// <summary>
    /// Base class for object pools.
    /// </summary>
    /// <typeparam name="T">Type of the elements stored in the pool.</typeparam>
    public abstract partial class ObjectPoolBase<T> : IObjectPool<T>
        where T : class
    {
        /// <summary>
        /// The first element in the pool, kept in a separate field for performance
        /// reasons.
        /// </summary>
        private T _first;

        /// <summary>
        /// Array of elements holding the objects stored in the pool.
        /// </summary>
        /// <remarks>
        /// The use of an Element wrapper struct eliminates array covariance checks
        /// performed upon stelem instructions and improves performance.
        /// </remarks>
        private readonly Element[] _items;

        /// <summary>
        /// Creates a new object pool of the specified pool size.
        /// </summary>
        /// <param name="size">Number of object instances to keep in the pool.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        protected ObjectPoolBase(int size)
        {
            if (size < 1)
                throw new ArgumentOutOfRangeException(nameof(size));

            _items = new Element[size - 1];

            OnCreate();
        }

        /// <summary>
        /// Gets a pooled object instance with RAII capabilities to return it to the pool.
        /// </summary>
        /// <returns>Pooled object instance.</returns>
        public PooledObject<T> New() => new(this, /* no closure */ p => p.AllocateFast(), /* no closure */ (p, o) => p.FreeFast(o));

        /// <summary>
        /// Returns an object instance and checks it out from the pool.
        /// </summary>
        /// <returns>Object instance returned from the pool.</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public T Allocate() => AllocateFast();

        /// <summary>
        /// Returns an object instance and checks it out from the pool. This method gets inlined internally to increase performance.
        /// </summary>
        /// <returns>Object instance returned from the pool.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal T AllocateFast()
        {
            //
            // Search strategy is a simple linear probing which is chosen for its cache-friendliness.
            // Note that Free will try to store recycled objects close to the start thus statistically
            // reducing how far we will typically search.
            //

            OnAllocating();

#if ENABLE_LOGGING
            var isNew = false;
#endif

            var inst = _first;
            if (inst == null || inst != Interlocked.CompareExchange(ref _first, null, inst))
            {
#if ENABLE_LOGGING
                inst = AllocateSlow(ref isNew);
#else
                inst = AllocateSlow();
#endif
            }

#if DETECT_LEAKS
            if (s_leakTrackers != null)
            {
#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA2000 // Dispose objects before losing scope. (By design for tracker object. Gets manually disposed later.)
                var tracker = new LeakTracker();
#pragma warning restore CA2000
#pragma warning restore IDE0079
                s_leakTrackers.Add(inst, tracker);

#if TRACE_LEAKS
                var frame = new StackTrace(false);
                tracker._trace = frame;
#endif
            }
#endif

#if ENABLE_LOGGING
            OnAllocated(inst, isNew);
#endif

            return inst;
        }

#if ENABLE_LOGGING
        private T AllocateSlow(ref bool isNew)
#else
        private T AllocateSlow()
#endif
        {
            var items = _items;

            for (var i = 0; i < items.Length; i++)
            {
                // Note that the read is optimistically not synchronized. That is intentional. We
                // will use Interlocked only when we have a candidate. In a worst case we may miss
                // some recently returned objects. Not a big deal.
                var inst = items[i].Value;
                if (inst != null)
                {
                    if (inst == Interlocked.CompareExchange(ref items[i].Value, null, inst))
                    {
                        return inst;
                    }
                }
            }

#if ENABLE_LOGGING
            isNew = true;
#endif
            return CreateInstance();
        }

        /// <summary>
        /// Returns an object to the pool.
        /// </summary>
        /// <param name="obj">Object to return to the pool.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void Free(T obj) => FreeFast(obj);

        /// <summary>
        /// Returns an object to the pool. This method gets inlined internally to increase performance.
        /// </summary>
        /// <param name="obj">Object to return to the pool.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void FreeFast(T obj)
        {
            //
            // Search strategy is a simple linear probing which is chosen for it cache-friendliness.
            // Note that Free will try to store recycled objects close to the start thus statistically
            // reducing how far we will typically search.
            //
            // This strategy is borrowed from the Roslyn source code, where concurrency on pooled data
            // structures is high. Noodling around with a few other strategies shows no major gains to
            // be obtained here. Any additional interlocking, use of concurrent collections, and whatnot
            // only adds to path length with little to no benefit in return.
            //

            OnFreeing(obj);

            Validate(obj);
            ForgetTrackedObject(obj);

            if (obj is IClearable clear)
            {
                clear.Clear();
            }

#if ENABLE_LOGGING
            var wasReturned = true;
#endif

            if (_first == null)
            {
                _first = obj;
            }
            else
            {
#if ENABLE_LOGGING
                FreeSlow(obj, ref wasReturned);
#else
                FreeSlow(obj);
#endif
            }

#if ENABLE_LOGGING
            OnFreed(wasReturned);
#endif
        }

#if ENABLE_LOGGING
        private void FreeSlow(T obj, ref bool wasReturned)
#else
        private void FreeSlow(T obj)
#endif
        {
            var items = _items;

            for (var i = 0; i < items.Length; i++)
            {
                if (items[i].Value == null)
                {
                    // Intentionally *not* using Interlocked operations here. In a worst case scenario,
                    // two objects may be stored into same slot. This is very unlikely to happen and
                    // will only mean that one of the objects will get collected. Not a big deal.
                    items[i].Value = obj;
#if ENABLE_LOGGING
                    wasReturned = true;
#endif
                    break;
                }
            }
        }

        /// <summary>
        /// Creates an instance of a pooled object.
        /// </summary>
        /// <returns>Newly created pooled object instance.</returns>
        protected abstract T CreateInstance();

        /// <summary>
        /// Handler for pool creation operations.
        /// </summary>
        partial void OnCreate();

        /// <summary>
        /// Handler for initiation of allocate operations.
        /// </summary>
        partial void OnAllocating();

        /// <summary>
        /// Handler for completion of allocate operations.
        /// </summary>
        /// <param name="obj">Object that was acquired due to the allocation.</param>
        /// <param name="isNew">Indicates that the object was obtained by performing an allocation.</param>
        partial void OnAllocated(T obj, bool isNew);

        /// <summary>
        /// Handler for initiation of free operations.
        /// </summary>
        /// <param name="obj">Object that is being freed.</param>
        partial void OnFreeing(T obj);

        /// <summary>
        /// Handler for completion of free operations.
        /// </summary>
        /// <param name="wasReturned">Indicates whether the object was returned to the pool.</param>
        partial void OnFreed(bool wasReturned);

        /// <summary>
        /// Validates that the object being freed is not null or unknown to the pool.
        /// This method is only used in DEBUG builds.
        /// </summary>
        /// <param name="obj">Object to check free constraints for.</param>
        partial void Validate(T obj);

#pragma warning disable IDE0079 // Remove unnecessary suppression (only on .NET 5)
#pragma warning disable CA1822 // Can be marked static (generated code uses instance-based access; only relevant in DEBUG)
        /// <summary>
        /// Forgets about the specified object that was retrieved from the pool.
        /// This is called when an object is returned to the pool.  It may also be explicitly
        /// called if an object allocated from the pool is intentionally not being returned
        /// to the pool.  This can be of use with pooled arrays if the consumer wants to
        /// return a larger array to the pool than was originally allocated.
        /// </summary>
        /// <param name="old">Object to stop tracking.</param>
        /// <param name="replacement">Replacement object to track in lieu of the original object.</param>
        /// <remarks>
        /// This method is only used in DEBUG builds to clear up tracking tables.
        /// In regular builds, a discarded pool object instance is simply dropped.
        /// </remarks>
        [Conditional("DEBUG")]
        [ExcludeFromCodeCoverage]
        internal void ForgetTrackedObject(T old, T replacement = null)
        {
#if DETECT_LEAKS
            if (s_leakTrackers == null)
            {
                return;
            }

            if (s_leakTrackers.TryGetValue(old, out LeakTracker tracker))
            {
                tracker.Dispose();
                s_leakTrackers.Remove(old);
            }
            else
            {
                var report = string.Format(
                    CultureInfo.InvariantCulture,
                    "Object of type {0} was freed, but was not from pool. \n Callstack: \n {1} ",
                    typeof(T).ToString(),
                    new StackTrace(false)
                );

                Debug.WriteLine("TRACEOBJECTPOOLLEAKS_BEGIN\n" + report + "TRACEOBJECTPOOLLEAKS_END");
            }

            if (replacement != null)
            {
#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA2000 // Dispose objects before losing scope. (By design for tracker object. Gets manually disposed later.)
                tracker = new LeakTracker();
#pragma warning restore CA2000
#pragma warning restore IDE0079
                s_leakTrackers.Add(replacement, tracker);
            }
#endif
        }
#pragma warning restore CA1822
#pragma warning restore IDE0079

        /// <summary>
        /// Struct holding a pooled object instance.
        /// </summary>
        /// <remarks>
        /// Even though this struct is equivalent to an instance of type T in terms of the
        /// memory layout, the use of a wrapper struct eliminates array covariance checks
        /// upon performing stelem instructions into arrays.
        /// </remarks>
        private struct Element
        {
            internal T Value;
        }
    }

#if DEBUG
    public partial class ObjectPoolBase<T>
    {
        [ExcludeFromCodeCoverage]
        partial void Validate(T obj)
        {
            Debug.Assert(obj != null, "freeing null?");

            Debug.Assert(_first != obj, "freeing twice?");

            var items = _items;
            for (var i = 0; i < items.Length; i++)
            {
                var value = items[i].Value;
                if (value == null)
                {
                    return;
                }

                Debug.Assert(value != obj, "freeing twice?");
            }
        }
    }
#endif

#if DETECT_LEAKS
    public partial class ObjectPoolBase<T>
    {
        /// <summary>
        /// Conditional weak table to associate leak tracker instances with objects that were allocated
        /// from the pool. Upon dropping a pooled object instance without returning it to the pool, the
        /// entry whose key matches the pooled object in the table will get collected, causing the leak
        /// tracker to become unreachable. When the leak tracker gets collected, its finalizer will run
        /// and log an error.
        /// </summary>
        private static readonly ConditionalWeakTable<T, LeakTracker> s_leakTrackers =
            //
            // NB: We're seeing sporadic failures on Mono where TryAdd fails with 'Key already in the list'
            //     which seems related to https://github.com/mono/mono/issues/8700.
            //
            Type.GetType("Mono.Runtime") == null ? new() : null;

        /// <summary>
        /// Leak tracker used to detect orphaned pooled objects that weren't returned to their owning pool.
        /// </summary>
        [ExcludeFromCodeCoverage]
        private sealed class LeakTracker : IDisposable
        {
            /// <summary>
            /// Indicates whether the leak tracker was explicitly disposed, therefore suppressing leak detection.
            /// </summary>
            private volatile bool _disposed;

#if TRACE_LEAKS
            /// <summary>
            /// Stack trace of the allocation site of the tracked object. This will reveal the place
            /// where the object got checked out from the pool, which is the closest clue to figuring
            /// out who leaked it.
            /// </summary>
            internal volatile StackTrace _trace = null;
#endif

            /// <summary>
            /// Disposes the leak tracker, indicating that the pooled object associated to this tracker
            /// was correctly returned to its pool.
            /// </summary>
            public void Dispose()
            {
                _disposed = true;
                GC.SuppressFinalize(this);
            }

            /// <summary>
            /// Gets the stack trace information that can be used to identify the location of the leak.
            /// </summary>
            /// <returns>Stack trace of the allocation site of the pooled object.</returns>
            private string GetTrace()
            {
#if TRACE_LEAKS
                return _trace == null ? "" : _trace.ToString();
#else
                return "Leak tracing information is disabled. Define TRACE_LEAKS in ObjectPoolBase.cs to get more info. \n";
#endif
            }

            /// <summary>
            /// Finalizer of the leak tracker instance. When the leak tracker gets collected because
            /// it got dropped from the leak tracking table, this method will check whether the object
            /// was properly returned to the pool, as indicated by a call to Dispose on the current
            /// leak tracker instance. If not, an error will be logged.
            /// </summary>
            ~LeakTracker()
            {
                if (!_disposed
                    && !Environment.HasShutdownStarted
                    && !AppDomain.CurrentDomain.IsFinalizingForUnload()
                )
                {
                    var report = string.Format(
                        CultureInfo.InvariantCulture,
                        "Pool detected potential leaking of {0}. \n Location of the leak: \n {1} ",
                        typeof(T).ToString(),
                        GetTrace()
                    );

                    // If you are seeing this message it means that object has been allocated from the pool
                    // and has not been returned back. This is not critical, but turns pool into rather
                    // inefficient kind of "new".
                    Debug.WriteLine("TRACEOBJECTPOOLLEAKS_BEGIN\n" + report + "TRACEOBJECTPOOLLEAKS_END");
                }
            }
        }
    }
#endif

#if ENABLE_LOGGING
    public partial class ObjectPoolBase<T>
    {
        private TraceLog _createData;

        private int _allocationCount;
        private int _freeCount;

        private int _createInstanceCount;
        private int _dropCount;

        private readonly ConditionalWeakTable<T, History> _objectLogs = new();
        private readonly ConcurrentDictionary<History, bool> _trackedObjs = new();

        partial void OnCreate() => _createData = TraceLog.Create(Operation.CreatePool);

        partial void OnAllocating() => Interlocked.Increment(ref _allocationCount);

        partial void OnAllocated(T obj, bool isNew)
        {
            if (isNew)
            {
                Interlocked.Increment(ref _createInstanceCount);
            }

            var h = GetLogs(obj);
            h.Add(TraceLog.Create(Operation.Allocate));

            _trackedObjs[h] = true;
        }

        partial void OnFreeing(T obj)
        {
            Interlocked.Increment(ref _freeCount);

            var h = GetLogs(obj);
            h.Add(TraceLog.Create(Operation.Free));

            _trackedObjs[h] = false;
        }

        partial void OnFreed(bool wasReturned)
        {
            if (!wasReturned)
            {
                Interlocked.Increment(ref _dropCount);
            }
        }

        private History GetLogs(T obj)
        {
            return _objectLogs.GetValue(obj, o =>
            {
                var h = new History(o, me =>
                {
                    _trackedObjs.TryRemove(me, out _);
                });

                _trackedObjs.TryAdd(h, value: false);
                return h;
            });
        }

        /// <summary>
        /// Gets a string representation of the pool's contents and historical allocation reports.
        /// </summary>
        /// <returns>String representation to inspect the pool's behavior during debugging.</returns>
        public string DebugView
        {
            get
            {
                var sb = new StringBuilder();

                WriteHeader(sb, ToString() + " : ObjectPoolBase<" + typeof(T).ToString() + ">", '=');

                WriteHeader(sb, "Creation trace", '-');
                {
                    Write(sb, _createData.ToString());
                    WriteLine(sb);
                }

                WriteHeader(sb, "Statistics", '-');
                {
                    var count = 1 + _items.Length;
                    var standbyCount = (_first != null ? 1 : 0) + _items.Count(x => x.Value != null);

                    Indent();
                    Write(sb, "# slots:   " + count);
                    Write(sb, "# standby: " + standbyCount);
                    Write(sb, "# allocs:  " + _allocationCount);
                    Write(sb, "# frees:   " + _freeCount);
                    Write(sb, "# create:  " + _createInstanceCount);
                    Write(sb, "# drops:   " + _dropCount);

                    var stats = _trackedObjs.Aggregate((count: 0, total: 0L, max: 0L), (avg, x) => (count: avg.count + x.Key.useCount, total: avg.total + x.Key.totalUseTime.Ticks, max: Math.Max(avg.total, x.Key.longestUseTime.Ticks)));

                    if (stats.total > 0)
                    {
                        WriteLine(sb);
                        Write(sb, "avg use time: " + new TimeSpan(stats.total / stats.count));
                        Write(sb, "max use time: " + new TimeSpan(stats.max));
                    }

#if LEGEND
                    Indent();
                    WriteLine(sb);
                    WriteHeader(sb, "Legend", '~');
                    Write(sb, "# slots   - number of objects the pool can hold, i.e. its capacity");
                    Write(sb, "# standby - number of objects in the pool that are available to be checked out");
                    Write(sb, "# allocs  - number of allocations");
                    Write(sb, "# frees   - number of free operations");
                    Write(sb, "# create  - number of object instantiations; a number higher than # slots could indicate the pool is too small");
                    Write(sb, "# drops   - number of objects that were freed but weren't checked into the pool; a high number could indicate pool inefficiencies");
                    Outdent();
#endif

                    Outdent();
                    WriteLine(sb);
                    WriteLine(sb);
                }

                WriteHeader(sb, "Pool entries", '-');
                {
                    Indent();

                    var i = 0;
                    var w = (1 + _items.Length).ToString(CultureInfo.InvariantCulture).Length;

                    foreach (var item in new[] { new Element { Value = _first } }.Concat(_items))
                    {
                        var id = "[" + i + "]";
                        id = id.PadRight(w + 2);

                        var value = item.Value;
                        if (value != null)
                        {
                            if (_objectLogs.TryGetValue(value, out History h))
                            {
                                Write(sb, id + " = #" + h.Id);
                            }
                            else
                            {
                                Write(sb, id + " = <no info>");
                            }
                        }
                        else
                        {
                            Write(sb, id + " = <empty>");
                        }

                        i++;
                    }

                    Outdent();
                    WriteLine(sb);
                    WriteLine(sb);
                }

                WriteHeader(sb, "Tracked objects", '-');
                {
                    Indent();

                    var w = _trackedObjs.Count.ToString(CultureInfo.InvariantCulture).Length;

                    var res = _trackedObjs.Select(kv =>
                    {
                        var status = kv.Value ? "checked out" : "standby";

                        if (!kv.Key.Ref.TryGetTarget(out T it) && !_items.Any(x => x.Value == it))
                        {
                            status = "discarded";
                        }

                        return (kv, status);
                    });

                    var getPriority = new Func<string, int>(s =>
                    {
                        if (s == "checked out")
                            return 0;
                        else if (s == "standby")
                            return 1;
                        else if (s == "discarded")
                            return 2;
                        else return -1;
                    });

                    foreach (var (kv, status) in res.OrderBy(x => getPriority(x.status)).ThenBy(x => x.kv.Key.Id))
                    {
                        var h = kv.Key;
                        var id = "#" + h.Id;
                        id = id.PadRight(w);

                        Write(sb, id + " - " + status + "\n");

                        Indent();

                        WriteHeader(sb, "Stats", '~');
                        Indent();
                        Write(sb, kv.Key.GetStats());
                        Outdent();

                        WriteHeader(sb, "History", '~');
                        Indent();
                        Write(sb, kv.Key.ToString());
                        Outdent();

                        Outdent();
                    }

                    Outdent();
                    WriteLine(sb);
                    WriteLine(sb);
                }

                return sb.ToString();
            }
        }

        private string pad = "";
        private int indent;

        private void Indent()
        {
            indent++;
            pad = new string(' ', indent * 2);
        }

        private void Outdent()
        {
            indent--;
            pad = new string(' ', indent * 2);
        }

        private void WriteHeader(StringBuilder sb, string header, char separator)
        {
            sb.AppendLine(pad + header);
            sb.AppendLine(pad + new string(separator, header.Length));
            sb.AppendLine();
        }

        private void Write(StringBuilder sb, string str)
        {
            var parts =
#if NET8_0 || NETSTANDARD2_1
                str.Replace("\r\n", "\n", StringComparison.Ordinal)
#else
                str.Replace("\r\n", "\n")
#endif
                   .Split('\n');

            foreach (var part in parts)
            {
                sb.AppendLine(pad + part);
            }
        }

        private static void WriteLine(StringBuilder sb) => sb.AppendLine();

        private struct TraceLog
        {
            public Operation Operation;
            public StackTrace Trace;
            public int ThreadId;
            public string ThreadName;
            public int DomainId;
            public string DomainName;
            public DateTime Time;

            public static TraceLog Create(Operation operation)
            {
                var trace = new StackTrace(2, fNeedFileInfo: false);

                return new TraceLog
                {
                    Operation = operation,
                    Trace = trace,
                    ThreadId = Environment.CurrentManagedThreadId,
                    ThreadName = Thread.CurrentThread.Name,
                    DomainId = AppDomain.CurrentDomain.Id,
                    DomainName = AppDomain.CurrentDomain.FriendlyName,
                    Time = DateTime.UtcNow
                };
            }

            public override readonly string ToString()
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    "{0:yy/MM/dd HH:mm:ss.fffffff} [{1} ago] @ ~{2} ({3}) in {4} ({5})\n{6}",
                    Time,
                    DateTime.UtcNow - Time,
                    ThreadId,
                    ThreadName,
                    DomainId,
                    DomainName,
                    Trace
                );
            }
        }

        private sealed class History
        {
            private const int MAX = 8;

            private static int s_counter;

            private readonly Action<History> _remove;
            private readonly ConcurrentQueue<TraceLog> _traces;

            public History(T obj, Action<History> remove)
            {
                Id = Interlocked.Increment(ref s_counter);
                Ref = new WeakReference<T>(obj);
                _remove = remove;
                _traces = new ConcurrentQueue<TraceLog>();
            }

            public int Id;
            public WeakReference<T> Ref;

            public void Add(TraceLog log)
            {
                if (log.Operation == Operation.Free)
                {
                    var prev = _traces.Last();
                    if (prev.Operation == Operation.Allocate)
                    {
                        var useTime = log.Time - prev.Time;
                        LogUsage(useTime);
                    }
                }

                while (_traces.Count >= MAX && _traces.TryDequeue(out _))
                {
                }

                _traces.Enqueue(log);
            }

            public TimeSpan shortestUseTime = TimeSpan.MaxValue;
            public TimeSpan longestUseTime = TimeSpan.Zero;
            public TimeSpan totalUseTime = TimeSpan.Zero;
            public int useCount;

            private void LogUsage(TimeSpan useTime)
            {
                // A race is possible between OnAllocated and OnFreeing.
                // We don't care much for statistics; results may be approximate.

                if (useTime < shortestUseTime)
                {
                    shortestUseTime = useTime;
                }

                if (useTime > longestUseTime)
                {
                    longestUseTime = useTime;
                }

                totalUseTime += useTime;
                useCount++;
            }

            public string GetStats()
            {
                var sb = new StringBuilder();

                sb.AppendLine("Usage count =  " + useCount);
                if (useCount > 0)
                {
                    sb.AppendLine("Max use time = " + longestUseTime);
                    sb.AppendLine("Avg use time = " + new TimeSpan(totalUseTime.Ticks / useCount));
                }

                return sb.ToString();
            }

            public override string ToString()
            {
                var sb = new StringBuilder();

                foreach (var trace in _traces.Reverse())
                {
                    var op = trace.Operation == Operation.Allocate ? "Alloc" : "Free ";
                    sb.Append(op + " - " + trace.ToString());
                }

                return sb.ToString();
            }

            ~History()
            {
                _remove(this);
            }
        }

        private enum Operation
        {
            CreatePool,
            Allocate,
            Free,
        }
    }
#endif
}
