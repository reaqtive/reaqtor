// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Reaqtive
{
    internal abstract class GroupByBase<TSource, TKey, TElement> : SubscribableBase<IGroupedSubscribable<TKey, TElement>>
    {
        private readonly ISubscribable<TSource> _source;
        protected readonly Func<TSource, TKey> _keySelector;

        public GroupByBase(ISubscribable<TSource> source, Func<TSource, TKey> keySelector)
        {
            _source = source;
            _keySelector = keySelector;
        }

        protected abstract class SinkBase<TParam> : HigherOrderOutputStatefulOperator<TParam, TSource, TElement, IGroupedSubscribable<TKey, TElement>, TKey>
            where TParam : GroupByBase<TSource, TKey, TElement>
        {
            private bool _recovered;
            private RefCountSubscription _subscription;

            public SinkBase(TParam parent, IObserver<IGroupedSubscribable<TKey, TElement>> observer)
                : base(parent, observer)
            {
            }

            protected override string InnerStreamPrefix => "rx://tunnel/group/";

            protected override ISubscription OnSubscribe()
            {
                _subscription = new RefCountSubscription(this)
                {
                    Subscription = Params._source.Subscribe(this)
                };
                return _subscription;
            }

            protected override void OnStart()
            {
                base.OnStart();

                EnsureResourceManagement(_recovered);
            }

            protected override void ReleaseCore()
            {
                _subscription.Dispose();
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                _recovered = true;
            }

            protected override IGroupedSubscribable<TKey, TElement> Quote(IMultiSubject<TElement, TElement> inner, Uri innerStreamUri)
            {
                var group = (IGroupedMultiSubject<TKey, TElement>)inner;

                if (TryGetHigherOrderExecutionEnvironment(out var env))
                {
                    return env.Quote(group, innerStreamUri);
                }

                return group;
            }

            protected override IMultiSubject<TElement, TElement> CreateInner(TKey args, out Uri innerStreamUri)
            {
                var result = base.CreateInner(args, out innerStreamUri);
                return new GroupedMultiSubject<TKey, TElement>(result, args);
            }
        }

        protected abstract class Sink<TParam> : SinkBase<TParam>
            where TParam : GroupByBase<TSource, TKey, TElement>
        {
            private const string MAXGROUPCOUNTSETTING = "rx://operators/groupBy/settings/maxGroupCount";
            private int _maxGroupCount;

            private readonly object _gate = new();
            private Entry _nullGroup;
            private readonly IDictionary<TKey, Entry> _groups;
            private readonly IDictionary<Uri, TKey> _tunnels;

            public Sink(TParam parent, IObserver<IGroupedSubscribable<TKey, TElement>> observer, IEqualityComparer<TKey> comparer)
                : base(parent, observer)
            {
                // TODO: allocate upon start or recovery and use known capacity (if any) - also do this in Window
                _groups = new Dictionary<TKey, Entry>(comparer);
                _tunnels = new Dictionary<Uri, TKey>();
            }

            public override void SetContext(IOperatorContext context)
            {
                base.SetContext(context);

                context.TryGetInt32CheckGreaterThanZeroOrUseMaxValue(MAXGROUPCOUNTSETTING, out _maxGroupCount);
            }

            protected override IEnumerable<Uri> DependenciesCore
            {
                get
                {
                    if (_nullGroup != null)
                    {
                        yield return _nullGroup.Uri;
                    }

                    foreach (var tunnelUri in _tunnels.Keys)
                    {
                        yield return tunnelUri;
                    }
                }
            }

            public override void OnCompleted()
            {
                lock (_gate)
                {
                    _nullGroup?.Observer.OnCompleted();

                    foreach (var group in _groups.Values.ToList())
                    {
                        group.Observer.OnCompleted();
                    }

                    Output.OnCompleted();
                    Dispose();
                }
            }

            public override void OnError(Exception error)
            {
                lock (_gate)
                {
                    _nullGroup?.Observer.OnError(error);

                    foreach (var group in _groups.Values.ToList())
                    {
                        group.Observer.OnError(error);
                    }

                    Output.OnError(error);
                    Dispose();
                }
            }

            public override void OnNext(TSource value)
            {
                TKey key;
                try
                {
                    key = GetKey(value);
                }
                catch (Exception ex)
                {
                    lock (_gate)
                    {
                        Output.OnError(ex);
                        Dispose();
                    }

                    return;
                }

                var entry = default(Entry);
                lock (_gate)
                {
                    if (key == null)
                    {
                        if (_nullGroup == null && !OutputSubscriptionDisposed)
                        {
                            if (!TryCreateGroup(key, out entry))
                            {
                                return;
                            }

                            _nullGroup = entry;
                            StateChanged = true;
                        }

                        entry = _nullGroup;
                    }
                    else
                    {
                        bool hasGroup;

                        try
                        {
                            hasGroup = _groups.TryGetValue(key, out entry);
                        }
                        catch (Exception ex)
                        {
                            Output.OnError(ex);
                            Dispose();
                            return;
                        }

                        if (!hasGroup && !OutputSubscriptionDisposed)
                        {
                            if (!TryCreateGroup(key, out entry))
                            {
                                return;
                            }
                        }
                    }

                    TElement element;
                    try
                    {
                        element = GetElement(value);
                    }
                    catch (Exception ex)
                    {
                        lock (_gate)
                        {
                            Output.OnError(ex);
                            Dispose();
                        }

                        return;
                    }

                    entry?.Observer.OnNext(element);
                }
            }

            protected abstract TKey GetKey(TSource value);

            protected abstract TElement GetElement(TSource value);

            private bool TryCreateGroup(TKey key, out Entry entry)
            {
                // TODO: expose the key on the inner sequence

                if (_groups.Count + (_nullGroup != null ? 1 : 0) >= _maxGroupCount)
                {
                    Output.OnError(new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The number of groups produced by the GroupBy operator exceeded {0} items. Please adjust the key selector of the GroupBy operator to avoid exceeding this limit, or use GroupByUntil to expire groups.", _maxGroupCount)));
                    Dispose();

                    entry = null;
                    return false;
                }

                var group = CreateInner(key, out var groupUri);

                var observer = group.CreateObserver();

                entry = new Entry { Uri = groupUri, Observer = observer };

                lock (_gate)
                {
                    if (key != null)
                    {
                        //
                        // Adding those before calling OnNext, so we're ready to process a Collect request immediately.
                        //
                        _groups.Add(key, entry);
                        _tunnels.Add(groupUri, key);
                        StateChanged = true;
                    }

                    OnNextInner(group, groupUri); // TODO: virtualize quote creation to ensure returned quote instance exposes Key property
                }

                return true;
            }

            protected override void Collect(Uri innerUri)
            {
                lock (_gate)
                {
                    if (_nullGroup != null && _nullGroup.Uri == innerUri)
                    {
                        _nullGroup = null;
                        StateChanged = true;
                    }
                    else
                    {
                        if (_tunnels.TryGetValue(innerUri, out var key))
                        {
                            _tunnels.Remove(innerUri);
                            _groups.Remove(key);
                            StateChanged = true;
                        }
                    }
                }
            }

            protected override void LoadStateCore(IOperatorStateReader reader)
            {
                base.LoadStateCore(reader);

                var nullUri = reader.Read<Uri>();
                if (nullUri != null)
                {
                    if (TryGetInner(nullUri, out var nullGroup))
                    {
                        _nullGroup = new Entry
                        {
                            Uri = nullUri,
                            Observer = nullGroup.CreateObserver(),
                        };
                    }
                }

                var n = reader.Read<int>();
                for (var i = 0; i < n; i++)
                {
                    var key = reader.Read<TKey>();
                    var innerUri = reader.Read<Uri>();

                    if (TryGetInner(innerUri, out var group))
                    {
                        var entry = new Entry
                        {
                            Uri = innerUri,
                            Observer = group.CreateObserver(),
                        };

                        _groups.Add(key, entry);
                        _tunnels.Add(innerUri, key);
                    }
                }
            }

            protected override void SaveStateCore(IOperatorStateWriter writer)
            {
                base.SaveStateCore(writer);

                writer.Write<Uri>(_nullGroup?.Uri);

                writer.Write<int>(_groups.Count);
                foreach (var group in _groups)
                {
                    writer.Write<TKey>(group.Key);
                    writer.Write<Uri>(group.Value.Uri);
                }
            }
        }
    }

    internal sealed class GroupBy<TSource, TKey, TElement> : GroupByBase<TSource, TKey, TElement>
    {
        private readonly IEqualityComparer<TKey> _comparer;
        private readonly Func<TSource, TElement> _elementSelector;

        public GroupBy(ISubscribable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
            : base(source, keySelector)
        {
            _comparer = comparer;
            _elementSelector = elementSelector;
        }

        protected override ISubscription SubscribeCore(IObserver<IGroupedSubscribable<TKey, TElement>> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : Sink<GroupBy<TSource, TKey, TElement>>
        {
            public _(GroupBy<TSource, TKey, TElement> parent, IObserver<IGroupedSubscribable<TKey, TElement>> observer)
                : base(parent, observer, parent._comparer)
            {
            }

            public override string Name => "rc:GroupBy";

            public override Version Version => Versioning.v1;

            protected override TKey GetKey(TSource value)
            {
                return Params._keySelector(value);
            }

            protected override TElement GetElement(TSource value)
            {
                return Params._elementSelector(value);
            }
        }
    }

    internal sealed class GroupBy<TSource, TKey> : GroupByBase<TSource, TKey, TSource>
    {
        private readonly IEqualityComparer<TKey> _comparer;

        public GroupBy(ISubscribable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
            : base(source, keySelector)
        {
            _comparer = comparer;
        }

        protected override ISubscription SubscribeCore(IObserver<IGroupedSubscribable<TKey, TSource>> observer)
        {
            return new _(this, observer);
        }

        private sealed class _ : Sink<GroupBy<TSource, TKey>>
        {
            public _(GroupBy<TSource, TKey> parent, IObserver<IGroupedSubscribable<TKey, TSource>> observer)
                : base(parent, observer, parent._comparer)
            {
            }

            public override string Name => "rc:GroupBy/Element";

            public override Version Version => Versioning.v1;

            protected override TKey GetKey(TSource value)
            {
                return Params._keySelector(value);
            }

            protected override TSource GetElement(TSource value)
            {
                return value;
            }
        }
    }
}
