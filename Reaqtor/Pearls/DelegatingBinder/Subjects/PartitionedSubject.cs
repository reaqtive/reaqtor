// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Reflection;

namespace DelegatingBinder
{
    internal sealed class PartitionedSubject<T> : ISubject<T>, IDelegationTarget, IDisposable
    {
        private readonly Subject<T> _subject;
        private readonly Dictionary<MemberInfo, Partition> _partitions;

        public PartitionedSubject()
        {
            _subject = new Subject<T>();
            _partitions = new Dictionary<MemberInfo, Partition>();
        }

        public void OnCompleted()
        {
            _subject.OnCompleted();

            foreach (var partition in _partitions.Values)
            {
                partition.OnCompleted();
            }
        }

        public void OnError(Exception error)
        {
            _subject.OnError(error);

            foreach (var partition in _partitions.Values)
            {
                partition.OnError(error);
            }
        }

        public void OnNext(T value)
        {
            _subject.OnNext(value);

            foreach (var partition in _partitions.Values)
            {
                partition.OnNext(value);
            }
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return _subject.Subscribe(observer);
        }

        public void Dispose()
        {
            _subject.Dispose();
        }

        private IDisposable Subscribe(MemberInfo member, object value, IObserver<T> observer)
        {
            // TODO: concurrency, ref counting to remove empty partitions, etc.

            if (!_partitions.TryGetValue(member, out var partition))
            {
                var x = Expression.Parameter(typeof(T));
                var lookup = Expression.MakeMemberAccess(x, member);
                var selector = Expression.Lambda(lookup, x).Compile();

                partition = (Partition)Activator.CreateInstance(typeof(Partition<>).MakeGenericType(typeof(T), lookup.Type), selector);
                _partitions[member] = partition;
            }

            return partition.Subscribe(value, observer);
        }

        public Expression Apply(Expression expression, ParameterExpression self)
        {
            if (expression is InvocationExpression asOperator)
            {
                if (asOperator.Expression is ParameterExpression operatorId && operatorId.Name == "where") // NOTE: Delegation targets know about identifiers and are loosely coupled
                {
                    var source = asOperator.Arguments[0];
                    var operand = asOperator.Arguments[1].Unquote();

                    if (source == self && operand.NodeType == ExpressionType.Lambda)
                    {
                        var predicate = operand;
                        if (predicate.Parameters.Count == 1)
                        {
                            //
                            // TODO: match predicate based on
                            // - supported keys passed to the factory upon stream creation (an array of lambdas a la person => person.Name)
                            // - supported equality checks (==, Equals, etc.)
                            //

                            var filter = predicate.Body;
                            if (filter.NodeType == ExpressionType.Equal)
                            {
                                var equal = (BinaryExpression)filter;

                                var member = equal.Left as MemberExpression;
                                var constant = equal.Right as ConstantExpression;

                                if (constant == null && member == null)
                                {
                                    member = equal.Right as MemberExpression;
                                    constant = equal.Left as ConstantExpression;
                                }

                                if (constant != null && member != null)
                                {
                                    if (member.Expression == predicate.Parameters[0])
                                    {
                                        var newPartition = (Expression<Func<MemberInfo, object, PartitionObservable>>)((MemberInfo key, object value) => new PartitionObservable(this, key, value));
                                        var partition = BetaReducer.Reduce(Expression.Invoke(newPartition, Expression.Constant(member.Member), Expression.Constant(constant.Value, typeof(object))));
                                        return partition;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return expression;
        }

        private sealed class PartitionObservable : IObservable<T>
        {
            private readonly PartitionedSubject<T> _parent;
            private readonly MemberInfo _member;
            private readonly object _value;

            public PartitionObservable(PartitionedSubject<T> parent, MemberInfo member, object value)
            {
                _parent = parent;
                _member = member;
                _value = value;
            }

            public IDisposable Subscribe(IObserver<T> observer)
            {
                return _parent.Subscribe(_member, _value, observer);
            }
        }

        private abstract class Partition : IObserver<T>
        {
            public abstract void OnCompleted();
            public abstract void OnError(Exception error);
            public abstract void OnNext(T value);
            public abstract IDisposable Subscribe(object value, IObserver<T> observer);
        }

        private class Partition<K> : Partition
        {
            private readonly Func<T, K> _keySelector;

            // TODO: concurrency
            private readonly List<IObserver<T>> _null;
            private readonly IDictionary<K, List<IObserver<T>>> _keys;

            public Partition(Func<T, K> keySelector)
            {
                _keySelector = keySelector;
                _null = new List<IObserver<T>>();
                _keys = new Dictionary<K, List<IObserver<T>>>();
            }

            public override void OnCompleted()
            {
                foreach (var o in _null)
                {
                    o.OnCompleted();
                }

                foreach (var k in _keys.Values)
                {
                    foreach (var o in k)
                    {
                        o.OnCompleted();
                    }
                }
            }

            public override void OnError(Exception error)
            {
                foreach (var o in _null)
                {
                    o.OnError(error);
                }

                foreach (var k in _keys.Values)
                {
                    foreach (var o in k)
                    {
                        o.OnError(error);
                    }
                }
            }

            public override void OnNext(T value)
            {
                var key = _keySelector(value);

                if (key == null)
                {
                    foreach (var o in _null)
                    {
                        o.OnNext(value);
                    }
                }
                else
                {
                    if (_keys.TryGetValue(key, out var k))
                    {
                        foreach (var o in k)
                        {
                            o.OnNext(value);
                        }
                    }
                }
            }

            public override IDisposable Subscribe(object value, IObserver<T> observer)
            {
                // TODO: concurrency

                var key = (K)value;

                if (key == null)
                {
                    _null.Add(observer);

                    return Disposable.Create(() => // NOTE: idempotent
                    {
                        _null.Remove(observer); /* TODO: should remove last entry */
                    });
                }
                else
                {
                    if (!_keys.TryGetValue(key, out var k))
                    {
                        k = new List<IObserver<T>>();
                        _keys[key] = k;
                    }

                    k.Add(observer);

                    return Disposable.Create(() =>
                    {
                        k.Remove(observer); /* TODO: should remove last entry */

                        if (k.Count == 0)
                        {
                            _keys.Remove(key);
                        }
                    });
                }
            }
        }
    }
}
