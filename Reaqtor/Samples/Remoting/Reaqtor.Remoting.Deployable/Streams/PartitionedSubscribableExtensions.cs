// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

using Reaqtive;

using Reaqtor.Reactive.Expressions;

namespace Reaqtor.Remoting.Deployable.Streams
{
    public static class PartitionedSubscribableExtensions
    {
        private static class EqualityComparerReflectionHelper<T>
        {
            public static readonly MethodInfo EqualsMethod =
                ((MethodInfo)ReflectionHelpers.InfoOf((IEqualityComparer<T> c) => c.Equals(default, default)));
        }

        public static ISubscribable<T> Partition<T, TKey>(this ISubscribable<T> source, Expression filter, IEqualityComparer<TKey> keyComparer, TKey value)
        {
            return Partition(source, (Expression<Func<T, TKey>>)filter, keyComparer, value);
        }

        public static ISubscribable<T> Partition<T, TKey>(this ISubscribable<T> source, Expression<Func<T, TKey>> filter, IEqualityComparer<TKey> keyComparer, TKey value)
        {
            if (source is QuotedSubscribable<T> quotedPartitionableSubscribable)
            {
                source = quotedPartitionableSubscribable.Value;
            }

            if (source is IPartitionableSubscribable<T> partitionableSubscribable)
            {
                return partitionableSubscribable.AddPartition(filter).Bind(value, keyComparer);
            }

            if (source is IPartitionedMultiSubject<T> partitionedMultiSubject)
            {
                return partitionedMultiSubject.CreatePartitionableSubscribable().AddPartition(filter).Bind(value, keyComparer);
            }

            // This is the case where the input is not a partitionable artifact. In practice we should only delegate if the input is partitionable,
            // so this case should not be hit.
            var eq = EqualityComparerReflectionHelper<TKey>.EqualsMethod;
            var p = filter.Parameters.Single();
            var f = Expression.Lambda<Func<T, bool>>(Expression.Call(Expression.Constant(keyComparer), eq, filter.Body, Expression.Constant(value)), p);
            return source.Where(f.Compile());
        }
    }
}
