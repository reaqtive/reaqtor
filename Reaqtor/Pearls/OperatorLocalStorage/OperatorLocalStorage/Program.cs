// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

//#define LOG

using System;
using System.Collections;
using System.Collections.Generic;

using Reaqtive.Storage;

using Reaqtor.QueryEngine;

using Utilities;

namespace Playground
{
    class Program
    {
        static void Main()
        {
            ObjectSpaceTests();

            EngineIntegrationTests.RunAsync().GetAwaiter().GetResult();
        }

        static void ObjectSpaceTests()
        {
            var store = new Store();

            {
                var serializer = new SerializationFactory();
                var state = new PersistedObjectSpace(serializer);

                var value = state.CreateValue<string>("bar");
                value.Value = "Hello, world!";

                Console.WriteLine("Value = " + value.Value);

                var array = state.CreateArray<int>("foo", 8);
                array[3] = 42;
                array[5] = 43;

                Console.WriteLine("Array = " + string.Join(",", array));

                var list = state.CreateList<int>("qux");
                list.Add(2);
                list.Add(3);
                list.Add(5);

                Console.WriteLine("List = " + string.Join(",", list));

                var queue = state.CreateQueue<int>("baz");
                queue.Enqueue(2);
                queue.Enqueue(3);
                queue.Enqueue(5);

                Console.WriteLine("Queue = " + string.Join(",", queue));

                var stack = state.CreateStack<int>("quux");
                stack.Push(2);
                stack.Push(3);
                stack.Push(5);

                Console.WriteLine("Stack = " + string.Join(",", stack));

                var set = state.CreateSet<int>("fred");
                set.Add(2);
                set.Add(3);
                set.Add(5);

                Console.WriteLine("Set = " + string.Join(",", set));

                var dictionary = state.CreateDictionary<string, int>("wilma");
                dictionary.Add("two", 2);
                dictionary.Add("three", 3);
                dictionary.Add("five", 5);

                Console.WriteLine("Dictionary = " + string.Join(",", dictionary));

                var linkedList = state.CreateLinkedList<int>("homer");
                linkedList.Add(3);
                linkedList.Add(7);
                linkedList.Add(13);

                Console.WriteLine("Linked list = " + string.Join(",", linkedList));

                Console.WriteLine();
                Console.Write("Saving... ");

                var writer = GetWriter(store, CheckpointKind.Full);
                state.Save(writer);
                writer.CommitAsync().GetAwaiter().GetResult();
                state.OnSaved();

                Console.WriteLine("Done");
                Console.WriteLine();
            }

            store.Print();

            {
                var serializer = new SerializationFactory();
                var state = new PersistedObjectSpace(serializer);

                Console.WriteLine();
                Console.Write("Loading... ");

                var reader = GetReader(store);
                state.Load(reader);

                Console.WriteLine("Done");
                Console.WriteLine();

                var value = state.GetValue<string>("bar");

                Console.WriteLine("Value = " + value.Value);

                var array = state.GetArray<int>("foo");

                Console.WriteLine("Array = " + string.Join(",", array));

                var list = state.GetList<int>("qux");

                Console.WriteLine("List = " + string.Join(",", list));

                var queue = state.GetQueue<int>("baz");

                Console.WriteLine("Queue = " + string.Join(",", queue));

                var stack = state.GetStack<int>("quux");

                Console.WriteLine("Stack = " + string.Join(",", stack));

                var set = state.GetSet<int>("fred");

                Console.WriteLine("Set = " + string.Join(",", set));

                var dictionary = state.GetDictionary<string, int>("wilma");

                Console.WriteLine("Dictionary = " + string.Join(",", dictionary));

                var linkedList = state.GetLinkedList<int>("homer");

                Console.WriteLine("Linked list = " + string.Join(",", linkedList));

                Console.WriteLine();

                Console.WriteLine("Applying edits...");
                Console.WriteLine();

                value.Value = "baz";

                Console.WriteLine("Value = " + value.Value);

                array[2] = 41;
                array[5] = 44;

                Console.WriteLine("Array = " + string.Join(",", array));

                list.Add(7);

                Console.WriteLine("List = " + string.Join(",", list));

                queue.Dequeue();
                queue.Enqueue(11);
                queue.Enqueue(13);

                Console.WriteLine("Queue = " + string.Join(",", queue));

                stack.Pop();
                stack.Push(11);
                stack.Push(13);

                Console.WriteLine("Stack = " + string.Join(",", stack));

                set.Remove(3);
                set.Add(7);
                set.Add(11);

                Console.WriteLine("Set = " + string.Join(",", set));

                dictionary.Remove("three");
                dictionary.Add("seven", 7);
                dictionary.Add("eleven", 11);

                Console.WriteLine("Dictionary = " + string.Join(",", dictionary));

                linkedList.AddFirst(2);
                linkedList.AddAfter(linkedList.Find(3), 5);
                linkedList.AddBefore(linkedList.FindLast(13), 11);
                linkedList.AddLast(17);

                Console.WriteLine("Linked list = " + string.Join(",", linkedList));

                Console.WriteLine();
                Console.Write("Saving... ");

                var writer = GetWriter(store, CheckpointKind.Differential);
                state.Save(writer);
                writer.CommitAsync().GetAwaiter().GetResult();
                state.OnSaved();

                Console.WriteLine("Done");
                Console.WriteLine();
            }

            store.Print();

            {
                var serializer = new SerializationFactory();
                var state = new PersistedObjectSpace(serializer);

                Console.WriteLine();
                Console.Write("Loading... ");

                var reader = GetReader(store);
                state.Load(reader);

                Console.WriteLine("Done");
                Console.WriteLine();

                var value = state.GetValue<string>("bar");

                Console.WriteLine("Value = " + value.Value);

                var array = state.GetArray<int>("foo");

                Console.WriteLine("Array = " + string.Join(",", array));

                var list = state.GetList<int>("qux");

                Console.WriteLine("List = " + string.Join(",", list));

                var queue = state.GetQueue<int>("baz");

                Console.WriteLine("Queue = " + string.Join(",", queue));

                var stack = state.GetStack<int>("quux");

                Console.WriteLine("Stack = " + string.Join(",", stack));

                var set = state.GetSet<int>("fred");

                Console.WriteLine("Set = " + string.Join(",", set));

                var dictionary = state.GetDictionary<string, int>("wilma");

                Console.WriteLine("Dictionary = " + string.Join(",", dictionary));

                var linkedList = state.GetLinkedList<int>("homer");

                Console.WriteLine("Linked list = " + string.Join(",", linkedList));
            }
        }

        private static IStateReader GetReader(Store store)
        {
            return new Reader(store)
#if LOG
                .WithLogging()
#endif
                ;
        }

        private static IStateWriter GetWriter(Store store, CheckpointKind checkpointKind)
        {
            return new Writer(store, checkpointKind)
#if LOG
                .WithLogging()
#endif
                ;
        }

#pragma warning disable IDE0051 // Remove unused private members (compile-time test to ensure interfaces are correct)
        private static void InterfaceSanityCheck<T>(IPersistedObjectSpace space)
        {
            //
            // Value
            //

            var value = space.GetValue<int>("foo");

            // IPersisted
            _ = value.Id;

            // IPersistedValue<T>, IValue<T>
            _ = value.Value;
            value.Value = default;

            // IReadOnlyValue<T>
            _ = ((IReadOnlyValue<int>)value).Value;


            //
            // Array
            //

            var array = space.GetArray<int>("foo");

            // IPersisted
            _ = array.Id;

            // IArray<T>
            _ = array.Length;
            _ = array[default];
            array[default] = default;

            // IReadOnlyCollection<T>
            _ = array.Count;

            // IEnumerable<T>
            _ = array.GetEnumerator();

            // IEnumerable
            _ = ((IEnumerable)array).GetEnumerator();


            //
            // List
            //

            var list = space.GetList<int>("foo");

            // IPersisted
            _ = list.Id;

            // IPersistedList<T>
            _ = list[default];
            list[default] = default;
            _ = list.Count;

            // IList<T>
            _ = ((IList<int>)list)[default];
            ((IList<int>)list)[default] = default;
            list.IndexOf(default);
            list.Insert(default, default);
            list.RemoveAt(default);

            // ICollection<T>
            _ = ((ICollection<int>)list).Count;
            _ = list.IsReadOnly;
            list.Add(default);
            list.Clear();
            _ = list.Contains(default);
            list.CopyTo(default, default);
            _ = list.Remove(default);

            // IReadOnlyCollection<T>
            _ = ((IReadOnlyCollection<int>)list).Count;

            // IEnumerable<T>
            _ = list.GetEnumerator();

            // IEnumerable
            _ = ((IEnumerable)list).GetEnumerator();


            //
            // Stack
            //

            var stack = space.GetStack<int>("foo");

            // IPersisted
            _ = stack.Id;

            // IStack<T>
            stack.Push(default);
            _ = stack.Peek();
            _ = stack.Pop();

            // IReadOnlyCollection<T>
            _ = stack.Count;

            // IEnumerable<T>
            _ = stack.GetEnumerator();

            // IEnumerable
            _ = ((IEnumerable)stack).GetEnumerator();


            //
            // Queue
            //

            var queue = space.GetQueue<int>("foo");

            // IPersisted
            _ = queue.Id;

            // IQueue<T>
            queue.Enqueue(default);
            _ = queue.Peek();
            _ = queue.Dequeue();

            // IReadOnlyCollection<T>
            _ = queue.Count;

            // IEnumerable<T>
            _ = queue.GetEnumerator();

            // IEnumerable
            _ = ((IEnumerable)queue).GetEnumerator();


            //
            // Set
            //

            var set = space.GetSet<int>("foo");

            // IPersisted
            _ = set.Id;

            // IPersistedSet<T>
            _ = set.Count;

            // ISet<T>
            set.Add(1);
            set.ExceptWith(default);
            set.IntersectWith(default);
            set.IsProperSubsetOf(default);
            set.IsProperSupersetOf(default);
            set.IsSubsetOf(default);
            set.IsSupersetOf(default);
            set.Overlaps(default);
            set.SetEquals(default);
            set.SymmetricExceptWith(default);
            set.UnionWith(default);

            // ICollection<T>
            _ = ((ICollection<int>)set).Count;
            _ = set.IsReadOnly;
            ((ICollection<int>)set).Add(default);
            set.Clear();
            _ = set.Contains(default);
            set.CopyTo(default, default);
            _ = set.Remove(default);

            // IReadOnlyCollection<T>
            _ = ((IReadOnlyCollection<int>)set).Count;

            // IEnumerable<T>
            _ = set.GetEnumerator();

            // IEnumerable
            _ = ((IEnumerable)set).GetEnumerator();


            //
            // SortedSet
            //

            var sortedSet = space.GetSortedSet<int>("foo");

            // IPersisted
            _ = sortedSet.Id;

            // IPersistedSortedSet<T>
            _ = sortedSet.Count;

            // ISortedSet<T>
            _ = sortedSet.Min;
            _ = sortedSet.Max;
            _ = sortedSet.Reverse();
            _ = sortedSet.GetViewBetween(default, default);

            // ISet<T>
            sortedSet.Add(1);
            sortedSet.ExceptWith(default);
            sortedSet.IntersectWith(default);
            sortedSet.IsProperSubsetOf(default);
            sortedSet.IsProperSupersetOf(default);
            sortedSet.IsSubsetOf(default);
            sortedSet.IsSupersetOf(default);
            sortedSet.Overlaps(default);
            sortedSet.SetEquals(default);
            sortedSet.SymmetricExceptWith(default);
            sortedSet.UnionWith(default);

            // ICollection<T>
            _ = ((ICollection<int>)set).Count;
            _ = sortedSet.IsReadOnly;
            ((ICollection<int>)set).Add(default);
            sortedSet.Clear();
            _ = sortedSet.Contains(default);
            sortedSet.CopyTo(default, default);
            _ = sortedSet.Remove(default);

            // IReadOnlyCollection<T>
            _ = ((IReadOnlyCollection<int>)sortedSet).Count;

            // IEnumerable<T>
            _ = sortedSet.GetEnumerator();

            // IEnumerable
            _ = ((IEnumerable)sortedSet).GetEnumerator();


            //
            // Dictionary
            //

            var dictionary = space.GetDictionary<string, int>("foo");

            // IPersisted
            _ = dictionary.Id;

            // IPersistedDictionary<TKey, TValue>
            _ = dictionary[default];
            dictionary[default] = default;
            _ = dictionary.Count;
            _ = dictionary.Keys;
            _ = dictionary.Values;
            _ = dictionary.ContainsKey(default);
            _ = dictionary.TryGetValue(default, out var _);

            // IDictionary<TKey, TValue>
            _ = ((IDictionary<string, int>)dictionary)[default];
            ((IDictionary<string, int>)dictionary)[default] = default;
            _ = ((IDictionary<string, int>)dictionary).Count;
            _ = ((IDictionary<string, int>)dictionary).Keys;
            _ = ((IDictionary<string, int>)dictionary).Values;
            _ = ((IDictionary<string, int>)dictionary).ContainsKey(default);
            _ = ((IDictionary<string, int>)dictionary).TryGetValue(default, out var _);
            dictionary.Add(default, default);
            _ = dictionary.Remove(default);

            // IReadOnlyDictionary<TKey, TValue>
            _ = ((IReadOnlyDictionary<string, int>)dictionary)[default];
            ((IDictionary<string, int>)dictionary)[default] = default;
            _ = ((IReadOnlyDictionary<string, int>)dictionary).Count;
            _ = ((IReadOnlyDictionary<string, int>)dictionary).Keys;
            _ = ((IReadOnlyDictionary<string, int>)dictionary).Values;
            _ = ((IReadOnlyDictionary<string, int>)dictionary).ContainsKey(default);
            _ = ((IReadOnlyDictionary<string, int>)dictionary).TryGetValue(default, out var _);

            // ICollection<KeyValuePair<TKey, TValue>>
            _ = dictionary.Count;
            _ = dictionary.IsReadOnly;
            dictionary.Add(default);
            dictionary.Clear();
            _ = dictionary.Contains(default);
            dictionary.CopyTo(default, default);
            _ = ((ICollection<KeyValuePair<string, int>>)dictionary).Remove(default);

            // IReadOnlyCollection<KeyValuePair<TKey, TValue>>
            _ = ((IReadOnlyCollection<KeyValuePair<string, int>>)dictionary).Count;

            // IEnumerable<KeyValuePair<TKey, TValue>>
            _ = dictionary.GetEnumerator();

            // IEnumerable
            _ = ((IEnumerable)dictionary).GetEnumerator();


            //
            // SortedDictionary
            //

            var sortedDictionary = space.GetSortedDictionary<string, int>("foo");

            // IPersisted
            _ = sortedDictionary.Id;

            // IPersistedSortedDictionary<TKey, TValue>
            _ = sortedDictionary[default];
            sortedDictionary[default] = default;
            _ = sortedDictionary.Count;
            _ = sortedDictionary.Keys;
            _ = sortedDictionary.Values;
            _ = sortedDictionary.ContainsKey(default);
            _ = sortedDictionary.TryGetValue(default, out var _);

            // IDictionary<TKey, TValue>
            _ = ((IDictionary<string, int>)sortedDictionary)[default];
            ((IDictionary<string, int>)sortedDictionary)[default] = default;
            _ = ((IDictionary<string, int>)sortedDictionary).Count;
            _ = ((IDictionary<string, int>)sortedDictionary).Keys;
            _ = ((IDictionary<string, int>)sortedDictionary).Values;
            _ = ((IDictionary<string, int>)sortedDictionary).ContainsKey(default);
            _ = ((IDictionary<string, int>)sortedDictionary).TryGetValue(default, out var _);
            sortedDictionary.Add(default, default);
            _ = sortedDictionary.Remove(default);

            // IReadOnlyDictionary<TKey, TValue>
            _ = ((IReadOnlyDictionary<string, int>)sortedDictionary)[default];
            ((IDictionary<string, int>)sortedDictionary)[default] = default;
            _ = ((IReadOnlyDictionary<string, int>)sortedDictionary).Count;
            _ = ((IReadOnlyDictionary<string, int>)sortedDictionary).Keys;
            _ = ((IReadOnlyDictionary<string, int>)sortedDictionary).Values;
            _ = ((IReadOnlyDictionary<string, int>)sortedDictionary).ContainsKey(default);
            _ = ((IReadOnlyDictionary<string, int>)sortedDictionary).TryGetValue(default, out var _);

            // ICollection<KeyValuePair<TKey, TValue>>
            _ = sortedDictionary.Count;
            _ = sortedDictionary.IsReadOnly;
            sortedDictionary.Add(default);
            sortedDictionary.Clear();
            _ = sortedDictionary.Contains(default);
            sortedDictionary.CopyTo(default, default);
            _ = ((ICollection<KeyValuePair<string, int>>)sortedDictionary).Remove(default);

            // IReadOnlyCollection<KeyValuePair<TKey, TValue>>
            _ = ((IReadOnlyCollection<KeyValuePair<string, int>>)sortedDictionary).Count;

            // IEnumerable<KeyValuePair<TKey, TValue>>
            _ = sortedDictionary.GetEnumerator();

            // IEnumerable
            _ = ((IEnumerable)sortedDictionary).GetEnumerator();


            //
            // LinkedList
            //

            var linkedList = space.GetLinkedList<int>("foo");

            // IPersisted
            _ = linkedList.Id;

            // ILinkedList<T>
            _ = linkedList.Count;
            _ = linkedList.First;
            _ = linkedList.Last;
            _ = linkedList.AddAfter(default, default(int));
            linkedList.AddAfter(default, default(ILinkedListNode<int>));
            _ = linkedList.AddBefore(default, default(int));
            linkedList.AddBefore(default, default(ILinkedListNode<int>));
            _ = linkedList.AddFirst(default(int));
            linkedList.AddFirst(default(ILinkedListNode<int>));
            _ = linkedList.AddLast(default(int));
            linkedList.AddLast(default(ILinkedListNode<int>));
            _ = linkedList.Find(default);
            _ = linkedList.FindLast(default);
            linkedList.Remove(default);
            linkedList.RemoveFirst();
            linkedList.RemoveLast();

            // IReadOnlyLinkedList<T>
            _ = ((IReadOnlyLinkedList<int>)linkedList).First;
            _ = ((IReadOnlyLinkedList<int>)linkedList).Last;

            // ICollection<T>
            _ = ((ICollection<int>)linkedList).Count;
            _ = linkedList.IsReadOnly;
            linkedList.Add(default);
            linkedList.Clear();
            _ = linkedList.Contains(default);
            linkedList.CopyTo(default, default);
            _ = ((ICollection<int>)linkedList).Remove(default);

            // IReadOnlyCollection<T>
            _ = ((IReadOnlyCollection<int>)linkedList).Count;

            // IEnumerable<T>
            _ = linkedList.GetEnumerator();

            // IEnumerable
            _ = ((IEnumerable)linkedList).GetEnumerator();
        }
#pragma warning restore IDE0051 // Remove unused private members
    }
}
