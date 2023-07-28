// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/29/2015 - Initial work on memoization support.
//

using System.Diagnostics;
using System.Runtime.ExceptionServices;

#if DEBUG
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
#endif

namespace System.Memory
{
    internal interface ICacheEntry<T, R> : IValueOrError<R>
    {
        T Key { get; }
    }

    internal class ValueCacheEntry<T, R> : ValueOrErrorValue<R>, ICacheEntry<T, R>
    {
        public ValueCacheEntry(T key, R value)
            : base(value)
        {
            Key = key;
        }

        public T Key { get; }
    }

    internal class ErrorCacheEntry<T, R> : ValueOrErrorError<R>, ICacheEntry<T, R>
    {
        public ErrorCacheEntry(T key, Exception error)
            : base(ExceptionDispatchInfo.Capture(error))
        {
            Key = key;
        }

        public T Key { get; }
    }

    internal interface ILinkedListNode<T>
        where T : class, ILinkedListNode<T>
    {
        T Previous { get; set; }
        T Next { get; set; }
    }

    internal interface ILruCacheEntry<T, R> : ICacheEntry<T, R>, ILinkedListNode<ILruCacheEntry<T, R>>
    {
    }

    internal sealed class LruValueCacheEntry<T, R> : ValueCacheEntry<T, R>, ILruCacheEntry<T, R>
    {
        public LruValueCacheEntry(T key, R value)
            : base(key, value)
        {
        }

        public ILruCacheEntry<T, R> Previous { get; set; }
        public ILruCacheEntry<T, R> Next { get; set; }
    }

    internal sealed class LruErrorCacheEntry<T, R> : ErrorCacheEntry<T, R>, ILruCacheEntry<T, R>
    {
        public LruErrorCacheEntry(T key, Exception error)
            : base(key, error)
        {
        }

        public ILruCacheEntry<T, R> Previous { get; set; }
        public ILruCacheEntry<T, R> Next { get; set; }
    }

    internal interface IMetricsCacheEntry<T, R> : ICacheEntry<T, R>, IWritableMemoizationCacheEntryMetrics
    {
    }

    internal sealed class MetricsValueCacheEntry<T, R> : ValueCacheEntry<T, R>, IMetricsCacheEntry<T, R>
    {
        public MetricsValueCacheEntry(T key, R value)
            : base(key, value)
        {
        }

        public TimeSpan CreationTime { get; set; }
        public TimeSpan InvokeDuration { get; set; }
        public int HitCount { get; set; }
        public TimeSpan LastAccessTime { get; set; }
        public TimeSpan TotalDuration { get; set; }

        public TimeSpan AverageAccessTime => new(TotalDuration.Ticks / HitCount);

        public double SpeedupFactor => (double)InvokeDuration.Ticks / AverageAccessTime.Ticks;
    }

    internal sealed class MetricsErrorCacheEntry<T, R> : ErrorCacheEntry<T, R>, IMetricsCacheEntry<T, R>
    {
        public MetricsErrorCacheEntry(T key, Exception error)
            : base(key, error)
        {
        }

        public TimeSpan CreationTime { get; set; }
        public TimeSpan InvokeDuration { get; set; }
        public int HitCount { get; set; }
        public TimeSpan LastAccessTime { get; set; }
        public TimeSpan TotalDuration { get; set; }

        public TimeSpan AverageAccessTime => new(TotalDuration.Ticks / HitCount);

        public double SpeedupFactor => (double)InvokeDuration.Ticks / AverageAccessTime.Ticks;
    }

    internal interface IWritableMemoizationCacheEntryMetrics : IMemoizationCacheEntryMetrics
    {
        new TimeSpan CreationTime { get; set; }

        new TimeSpan InvokeDuration { get; set; }

        new int HitCount { get; set; }

        new TimeSpan LastAccessTime { get; set; }

        TimeSpan TotalDuration { get; set; }
    }

    internal struct LruLinkedList<TNode>
        where TNode : class, ILinkedListNode<TNode>
    {
        public TNode First;
        public TNode Last;
    }

    internal static partial class LruLinkedList
    {
        public static void Clear<TNode>(ref LruLinkedList<TNode> list)
            where TNode : class, ILinkedListNode<TNode>
        {
            list.First = null;
            list.Last = null;

            AssertLinkedList(ref list);
        }

        public static void RemoveLast<TNode>(ref LruLinkedList<TNode> list)
            where TNode : class, ILinkedListNode<TNode>
        {
            var last = list.Last;

            Debug.Assert(last != null, "Linked list not expected to be empty.");

            var prev = last.Previous;
            if (prev != null)
            {
                prev.Next = null;
                last.Previous = null;
            }

            list.Last = prev;

            if (list.Last == null)
            {
                list.First = null;
            }

            AssertLinkedList(ref list);
        }

        public static void MoveFirst<TNode>(ref LruLinkedList<TNode> list, TNode node)
            where TNode : class, ILinkedListNode<TNode>
        {
            if (node != list.First)
            {
                var prev = node.Previous;
                var next = node.Next;

                if (prev != null)
                {
                    prev.Next = next;
                }

                if (next != null)
                {
                    next.Previous = prev;
                }

                if (node == list.Last)
                {
                    list.Last = prev;
                }

                if (list.First != null)
                {
                    list.First.Previous = node;
                }

                list.Last ??= node;

                node.Next = list.First;
                node.Previous = null;
                list.First = node;
            }

            AssertLinkedList(ref list);
        }

        public static void Remove<TNode>(ref LruLinkedList<TNode> list, TNode node)
            where TNode : class, ILinkedListNode<TNode>
        {
            var prev = node.Previous;
            var next = node.Next;

            if (prev != null)
            {
                prev.Next = next;
            }
            else
            {
                list.First = next;
            }

            if (next != null)
            {
                next.Previous = prev;
            }
            else
            {
                list.Last = prev;
            }

            AssertLinkedList(ref list);
        }

        static partial void AssertLinkedList<TNode>(ref LruLinkedList<TNode> list)
            where TNode : class, ILinkedListNode<TNode>;
    }

#if DEBUG
    internal partial class LruLinkedList
    {
        static partial void AssertLinkedList<TNode>(ref LruLinkedList<TNode> list)
            where TNode : class, ILinkedListNode<TNode>
        {
            DebuggingHelpers.AssertLinkedList(list.First, list.Last);
        }
    }

    internal static class DebuggingHelpers
    {
        [ExcludeFromCodeCoverage]
        public static void AssertLinkedList<TNode>(ILinkedListNode<TNode> first, ILinkedListNode<TNode> last)
            where TNode : class, ILinkedListNode<TNode>
        {
            if (first == null)
            {
                Debug.Assert(last == null);
                return;
            }

            Debug.Assert(last != null);

            if (first == last)
            {
                Debug.Assert(first.Next == null);
                Debug.Assert(last.Previous == null);
            }

            if (first.Next != null)
            {
                Debug.Assert(first.Next.Previous == first);
            }
            else
            {
                Debug.Assert(first == last);
            }

            if (last.Previous != null)
            {
                Debug.Assert(last.Previous.Next == last);
            }
            else
            {
                Debug.Assert(first == last);
            }

            {
                var nodes = new HashSet<ILinkedListNode<TNode>>();

                var prev = first;
                var curr = first.Next;

                var added = nodes.Add(prev);
                Debug.Assert(added);

                while (curr != null)
                {
                    Debug.Assert(nodes.Add(curr));
                    Debug.Assert(curr.Previous == prev);

                    prev = curr;
                    curr = curr.Next;
                }

                Debug.Assert(prev == last);
            }

            {
                var nodes = new HashSet<ILinkedListNode<TNode>>();

                var prev = last;
                var curr = last.Previous;

                var added = nodes.Add(prev);
                Debug.Assert(added);

                while (curr != null)
                {
                    Debug.Assert(nodes.Add(curr));
                    Debug.Assert(curr.Next == prev);

                    prev = curr;
                    curr = curr.Previous;
                }

                Debug.Assert(prev == first);
            }
        }
    }
#endif
}
