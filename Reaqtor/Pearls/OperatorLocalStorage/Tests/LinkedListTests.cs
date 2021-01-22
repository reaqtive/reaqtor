// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - February 2018
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace Tests
{
    [TestClass]
    public class LinkedListTests : PersistedTestBase
    {
        // TODO: Add CreateAndGet and ManOrBoy tests.

        [TestMethod]
        public void LinkedList_Recovery_CreateDefault() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //

            _ = s.Space.CreateLinkedList<int>("bar");

            //
            // Save the space to the store.
            //
            {
                var edits = s.SaveSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                });
            }

            //
            // Create a new space.
            //

            s.CreateSpace();

            //
            // Assert the new space does not contain the linked list.
            //

            Assert.ThrowsException<KeyNotFoundException>(() => s.Space.GetLinkedList<int>("bar"));

            //
            // Load the space from the store.
            //

            s.LoadSpace();

            //
            // Get the linked list.
            //

            var bar = s.Space.GetLinkedList<int>("bar");

            //
            // Assert the linked list size.
            //

            Assert.AreEqual(0, bar.Count);
        });

        [TestMethod]
        public void LinkedList_Recovery_CreateLinkedList() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //
            {
                var list = s.Space.CreateLinkedList<int>("bar");

                list.Add(2);
                list.Add(3);
                list.Add(5);
                list.Add(7);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "3"),
                });
            }

            //
            // Get and assert the linked list.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                AssertList(list, new[] { 2, 3, 5, 7 });
            }
        });

        [TestMethod]
        public void LinkedList_Recovery_NoChange() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //
            {
                var list = s.Space.CreateLinkedList<int>("bar");

                list.Add(2);
                list.Add(3);
                list.Add(5);
                list.Add(7);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "3"),
                });
            }

            //
            // Save differential and assert no edits took place.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits);
            }

            //
            // Get and assert the linked list.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                AssertList(list, new[] { 2, 3, 5, 7 });
            }
        });

        [TestMethod]
        public void LinkedList_Recovery_Add() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //
            {
                var list = s.Space.CreateLinkedList<int>("bar");

                list.Add(2);
                list.Add(3);
                list.Add(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                });
            }

            //
            // Get the linked list and add an element.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                list.Add(7);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "3"),
                });
            }

            //
            // Get and assert the linked list.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                AssertList(list, new[] { 2, 3, 5, 7 });
            }
        });

        [TestMethod]
        public void LinkedList_Recovery_Add_Differential() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //
            {
                var list = s.Space.CreateLinkedList<int>("bar");

                list.Add(2);
                list.Add(3);
                list.Add(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                });
            }

            //
            // Get the linked list and add an element.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                list.Add(7);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"), // Update the next reference.
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "3"),
                });
            }

            //
            // Get and assert the linked list.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                AssertList(list, new[] { 2, 3, 5, 7 });
            }
        });

        [TestMethod]
        public void LinkedList_Recovery_AddFirst() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //
            {
                var list = s.Space.CreateLinkedList<int>("bar");

                list.Add(3);
                list.Add(5);
                list.Add(7);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                });
            }

            //
            // Get the linked list and add an element.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                list.AddFirst(2);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "3"),
                });
            }

            //
            // Get and assert the linked list.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                AssertList(list, new[] { 2, 3, 5, 7 });
            }
        });

        [TestMethod]
        public void LinkedList_Recovery_AddFirst_Differential() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //
            {
                var list = s.Space.CreateLinkedList<int>("bar");

                list.Add(3);
                list.Add(5);
                list.Add(7);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                });
            }

            //
            // Get the linked list and add an element.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                list.AddFirst(2);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"), // Update the previous reference.
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "3"),
                });
            }

            //
            // Get and assert the linked list.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                AssertList(list, new[] { 2, 3, 5, 7 });
            }
        });

        [TestMethod]
        public void LinkedList_Recovery_Remove_AddFirst() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //
            {
                var list = s.Space.CreateLinkedList<int>("bar");

                list.Add(3);
                list.Add(5);
                list.Add(2);
                list.Add(7);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "3"),
                });
            }

            //
            // Get the linked list, remove an element, and add it back.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                var node = list.Find(2);
                list.Remove(node);
                list.AddFirst(node);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "3"),
                });
            }

            //
            // Get and assert the linked list.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                AssertList(list, new[] { 2, 3, 5, 7 });
            }
        });

        [TestMethod]
        public void LinkedList_Recovery_Remove_AddFirst_Differential() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //
            {
                var list = s.Space.CreateLinkedList<int>("bar");

                list.Add(3);
                list.Add(5);
                list.Add(2);
                list.Add(7);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "3"),
                });
            }

            //
            // Get the linked list, remove an element, and add it back.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                var node = list.Find(2);
                list.Remove(node);
                list.AddFirst(node);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"), // Update the previous reference.
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"), // Update the next reference.
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"), // Reusing the same storage slot; update the previous and next references.
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),     // Reusing the same storage slot; (re)write the value. REVIEW: Can we avoid having to rewrite the value? Likely not, because we used a Remove operation above.
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"), // Update the previous reference.
                });
            }

            //
            // Get and assert the linked list.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                AssertList(list, new[] { 2, 3, 5, 7 });
            }
        });

        [TestMethod]
        public void LinkedList_Recovery_AddLast() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //
            {
                var list = s.Space.CreateLinkedList<int>("bar");

                list.Add(2);
                list.Add(3);
                list.Add(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                });
            }

            //
            // Get the linked list and add an element.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                list.AddLast(7);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "3"),
                });
            }

            //
            // Get and assert the linked list.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                AssertList(list, new[] { 2, 3, 5, 7 });
            }
        });

        [TestMethod]
        public void LinkedList_Recovery_AddLast_Differential() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //
            {
                var list = s.Space.CreateLinkedList<int>("bar");

                list.Add(2);
                list.Add(3);
                list.Add(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                });
            }

            //
            // Get the linked list and add an element.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                list.AddLast(7);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"), // Update the next reference.
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "3"),
                });
            }

            //
            // Get and assert the linked list.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                AssertList(list, new[] { 2, 3, 5, 7 });
            }
        });

        [TestMethod]
        public void LinkedList_Recovery_Remove_AddLast() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //
            {
                var list = s.Space.CreateLinkedList<int>("bar");

                list.Add(2);
                list.Add(7);
                list.Add(3);
                list.Add(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "3"),
                });
            }

            //
            // Get the linked list, remove an element, and add it back.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                var node = list.FindLast(7);
                list.Remove(node);
                list.AddLast(node);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "3"),
                });
            }

            //
            // Get and assert the linked list.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                AssertList(list, new[] { 2, 3, 5, 7 });
            }
        });

        [TestMethod]
        public void LinkedList_Recovery_Remove_AddLast_Differential() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //
            {
                var list = s.Space.CreateLinkedList<int>("bar");

                list.Add(2);
                list.Add(7);
                list.Add(3);
                list.Add(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "3"),
                });
            }

            //
            // Get the linked list, remove an element, and add it back.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                var node = list.FindLast(7);
                list.Remove(node);
                list.AddLast(node);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"), // Update the next reference.
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"), // Reusing the same storage slot; update the previous and next references.
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),     // Reusing the same storage slot; (re)write the value. REVIEW: Can we avoid having to rewrite the value? Likely not, because we used a Remove operation above.
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"), // Update the previous reference.
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"), // Update the next reference.
                });
            }

            //
            // Get and assert the linked list.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                AssertList(list, new[] { 2, 3, 5, 7 });
            }
        });

        [TestMethod]
        public void LinkedList_Recovery_AddAfterFirst() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //
            {
                var list = s.Space.CreateLinkedList<int>("bar");

                list.Add(2);
                list.Add(5);
                list.Add(7);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                });
            }

            //
            // Get the linked list and add an element.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                list.AddAfter(list.First, 3);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "3"),
                });
            }

            //
            // Get and assert the linked list.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                AssertList(list, new[] { 2, 3, 5, 7 });
            }
        });

        [TestMethod]
        public void LinkedList_Recovery_AddAfterFirst_Differential() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //
            {
                var list = s.Space.CreateLinkedList<int>("bar");

                list.Add(2);
                list.Add(5);
                list.Add(7);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                });
            }

            //
            // Get the linked list and add an element.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                list.AddAfter(list.First, 3);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"), // Update the next reference.
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"), // Update the previous reference.
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "3"),
                });
            }

            //
            // Get and assert the linked list.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                AssertList(list, new[] { 2, 3, 5, 7 });
            }
        });

        [TestMethod]
        public void LinkedList_Recovery_AddAfter() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //
            {
                var list = s.Space.CreateLinkedList<int>("bar");

                list.Add(2);
                list.Add(3);
                list.Add(7);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                });
            }

            //
            // Get the linked list and add an element.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                list.AddAfter(list.First.Next, 5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "3"),
                });
            }

            //
            // Get and assert the linked list.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                AssertList(list, new[] { 2, 3, 5, 7 });
            }
        });

        [TestMethod]
        public void LinkedList_Recovery_AddAfter_Differential() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //
            {
                var list = s.Space.CreateLinkedList<int>("bar");

                list.Add(2);
                list.Add(3);
                list.Add(7);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                });
            }

            //
            // Get the linked list and add an element.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                list.AddAfter(list.First.Next, 5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"), // Update the next reference.
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"), // Update the previous reference.
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "3"),
                });
            }

            //
            // Get and assert the linked list.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                AssertList(list, new[] { 2, 3, 5, 7 });
            }
        });

        [TestMethod]
        public void LinkedList_Recovery_AddAfterLast() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //
            {
                var list = s.Space.CreateLinkedList<int>("bar");

                list.Add(2);
                list.Add(3);
                list.Add(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                });
            }

            //
            // Get the linked list and add an element.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                list.AddAfter(list.Last, 7);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "3"),
                });
            }

            //
            // Get and assert the linked list.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                AssertList(list, new[] { 2, 3, 5, 7 });
            }
        });

        [TestMethod]
        public void LinkedList_Recovery_AddAfterLast_Differential() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //
            {
                var list = s.Space.CreateLinkedList<int>("bar");

                list.Add(2);
                list.Add(3);
                list.Add(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                });
            }

            //
            // Get the linked list and add an element.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                list.AddAfter(list.Last, 7);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"), // Update the next reference.
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "3"),
                });
            }

            //
            // Get and assert the linked list.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                AssertList(list, new[] { 2, 3, 5, 7 });
            }
        });

        [TestMethod]
        public void LinkedList_Recovery_Remove_AddAfter() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //
            {
                var list = s.Space.CreateLinkedList<int>("bar");

                list.Add(2);
                list.Add(7);
                list.Add(3);
                list.Add(5);
                list.Add(11);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "4"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "4"),
                });
            }

            //
            // Get the linked list, remove an element, and add it back.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                var five = list.Find(5);
                var seven = list.Find(7);

                list.Remove(seven);
                list.AddAfter(five, seven);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "4"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "4"),
                });
            }

            //
            // Get and assert the linked list.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                AssertList(list, new[] { 2, 3, 5, 7, 11 });
            }
        });

        [TestMethod]
        public void LinkedList_Recovery_Remove_AddAfter_Differential() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //
            {
                var list = s.Space.CreateLinkedList<int>("bar");

                list.Add(2);
                list.Add(7);
                list.Add(3);
                list.Add(5);
                list.Add(11);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "4"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "4"),
                });
            }

            //
            // Get the linked list, remove an element, and add it back.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                var five = list.Find(5);
                var seven = list.Find(7);

                list.Remove(seven);
                list.AddAfter(five, seven);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"), // Update the next reference.
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"), // Reusing the same storage slot; update the previous and next references.
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),     // Reusing the same storage slot; (re)write the value. REVIEW: Can we avoid having to rewrite the value? Likely not, because we used a Remove operation above.
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"), // Update the previous reference.
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"), // Update the next reference.
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "4"), // Update the previous reference.
                });
            }

            //
            // Get and assert the linked list.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                AssertList(list, new[] { 2, 3, 5, 7, 11 });
            }
        });

        [TestMethod]
        public void LinkedList_Recovery_AddBeforeFirst() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //
            {
                var list = s.Space.CreateLinkedList<int>("bar");

                list.Add(3);
                list.Add(5);
                list.Add(7);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                });
            }

            //
            // Get the linked list and add an element.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                list.AddBefore(list.First, 2);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "3"),
                });
            }

            //
            // Get and assert the linked list.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                AssertList(list, new[] { 2, 3, 5, 7 });
            }
        });

        [TestMethod]
        public void LinkedList_Recovery_AddBeforeFirst_Differential() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //
            {
                var list = s.Space.CreateLinkedList<int>("bar");

                list.Add(3);
                list.Add(5);
                list.Add(7);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                });
            }

            //
            // Get the linked list and add an element.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                list.AddBefore(list.First, 2);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"), // Update the previous reference.
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "3"),
                });
            }

            //
            // Get and assert the linked list.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                AssertList(list, new[] { 2, 3, 5, 7 });
            }
        });

        [TestMethod]
        public void LinkedList_Recovery_AddBefore() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //
            {
                var list = s.Space.CreateLinkedList<int>("bar");

                list.Add(2);
                list.Add(5);
                list.Add(7);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                });
            }

            //
            // Get the linked list and add an element.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                list.AddBefore(list.Last.Previous, 3);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "3"),
                });
            }

            //
            // Get and assert the linked list.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                AssertList(list, new[] { 2, 3, 5, 7 });
            }
        });

        [TestMethod]
        public void LinkedList_Recovery_AddBefore_Differential() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //
            {
                var list = s.Space.CreateLinkedList<int>("bar");

                list.Add(2);
                list.Add(5);
                list.Add(7);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                });
            }

            //
            // Get the linked list and add an element.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                list.AddBefore(list.Last.Previous, 3);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"), // Update the next reference.
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"), // Update the previous reference.
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "3"),
                });
            }

            //
            // Get and assert the linked list.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                AssertList(list, new[] { 2, 3, 5, 7 });
            }
        });

        [TestMethod]
        public void LinkedList_Recovery_AddBeforeLast() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //
            {
                var list = s.Space.CreateLinkedList<int>("bar");

                list.Add(2);
                list.Add(3);
                list.Add(7);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                });
            }

            //
            // Get the linked list and add an element.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                list.AddBefore(list.Last, 5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "3"),
                });
            }

            //
            // Get and assert the linked list.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                AssertList(list, new[] { 2, 3, 5, 7 });
            }
        });

        [TestMethod]
        public void LinkedList_Recovery_AddBeforeLast_Differential() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //
            {
                var list = s.Space.CreateLinkedList<int>("bar");

                list.Add(2);
                list.Add(3);
                list.Add(7);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                });
            }

            //
            // Get the linked list and add an element.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                list.AddBefore(list.Last, 5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"), // Update the next reference.
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"), // Update the previous reference.
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "3"),
                });
            }

            //
            // Get and assert the linked list.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                AssertList(list, new[] { 2, 3, 5, 7 });
            }
        });

        [TestMethod]
        public void LinkedList_Recovery_Remove_AddBefore() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //
            {
                var list = s.Space.CreateLinkedList<int>("bar");

                list.Add(2);
                list.Add(5);
                list.Add(7);
                list.Add(3);
                list.Add(11);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "4"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "4"),
                });
            }

            //
            // Get the linked list, remove an element, and add it back.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                var five = list.Find(5);
                var three = list.Find(3);

                list.Remove(three);
                list.AddBefore(five, three);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "4"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "4"),
                });
            }

            //
            // Get and assert the linked list.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                AssertList(list, new[] { 2, 3, 5, 7, 11 });
            }
        });

        [TestMethod]
        public void LinkedList_Recovery_Remove_AddBefore_Differential() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //
            {
                var list = s.Space.CreateLinkedList<int>("bar");

                list.Add(2);
                list.Add(5);
                list.Add(7);
                list.Add(3);
                list.Add(11);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "4"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "4"),
                });
            }

            //
            // Get the linked list, remove an element, and add it back.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                var five = list.Find(5);
                var three = list.Find(3);

                list.Remove(three);
                list.AddBefore(five, three);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"), // Update the next reference.
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"), // Update the previous reference.
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"), // Update the next reference.
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"), // Reusing the same storage slot; update the previous and next references.
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "3"),     // Reusing the same storage slot; (re)write the value. REVIEW: Can we avoid having to rewrite the value? Likely not, because we used a Remove operation above.
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "4"), // Update the previous reference.
                });
            }

            //
            // Get and assert the linked list.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                AssertList(list, new[] { 2, 3, 5, 7, 11 });
            }
        });

        [TestMethod]
        public void LinkedList_Recovery_EditValue() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //
            {
                var list = s.Space.CreateLinkedList<int>("bar");

                list.Add(2);
                list.Add(4);
                list.Add(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                });
            }

            //
            // Get the linked list and edit an element.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                list.First.Next.Value = 3;
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                });
            }

            //
            // Get and assert the linked list.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                AssertList(list, new[] { 2, 3, 5 });
            }
        });

        [TestMethod]
        public void LinkedList_Recovery_EditValue_Differential() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //
            {
                var list = s.Space.CreateLinkedList<int>("bar");

                list.Add(2);
                list.Add(4);
                list.Add(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                });
            }

            //
            // Get the linked list and edit an element.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                list.First.Next.Value = 3;
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"), // Edit value.
                });
            }

            //
            // Get and assert the linked list.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                AssertList(list, new[] { 2, 3, 5 });
            }
        });

        [TestMethod]
        public void LinkedList_Recovery_RemoveFirstOnSingleton() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //
            {
                var list = s.Space.CreateLinkedList<int>("bar");

                list.Add(2);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                });
            }

            //
            // Get the linked list and remove an element.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                list.RemoveFirst();
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                });
            }

            //
            // Get and assert the linked list.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                AssertList(list, Array.Empty<int>());
            }
        });

        [TestMethod]
        public void LinkedList_Recovery_RemoveFirstOnSingleton_Differential() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //
            {
                var list = s.Space.CreateLinkedList<int>("bar");

                list.Add(2);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                });
            }

            //
            // Get the linked list and remove an element.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                list.RemoveFirst();
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.Delete, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.Delete, "state/item/bar/data", "0"),
                });
            }

            //
            // Get and assert the linked list.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                AssertList(list, Array.Empty<int>());
            }
        });

        [TestMethod]
        public void LinkedList_Recovery_RemoveFirst() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //
            {
                var list = s.Space.CreateLinkedList<int>("bar");

                list.Add(1);
                list.Add(2);
                list.Add(3);
                list.Add(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "3"),
                });
            }

            //
            // Get the linked list and remove an element.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                list.RemoveFirst();
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "3"),
                });
            }

            //
            // Get and assert the linked list.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                AssertList(list, new[] { 2, 3, 5 });
            }
        });

        [TestMethod]
        public void LinkedList_Recovery_RemoveFirst_Differential() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //
            {
                var list = s.Space.CreateLinkedList<int>("bar");

                list.Add(1);
                list.Add(2);
                list.Add(3);
                list.Add(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "3"),
                });
            }

            //
            // Get the linked list and remove an element.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                list.RemoveFirst();
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.Delete, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.Delete, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"), // Update the previous reference.
                });
            }

            //
            // Get and assert the linked list.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                AssertList(list, new[] { 2, 3, 5 });
            }
        });

        [TestMethod]
        public void LinkedList_Recovery_RemoveLastOnSingleton() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //
            {
                var list = s.Space.CreateLinkedList<int>("bar");

                list.Add(2);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                });
            }

            //
            // Get the linked list and remove an element.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                list.RemoveLast();
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                });
            }

            //
            // Get and assert the linked list.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                AssertList(list, Array.Empty<int>());
            }
        });

        [TestMethod]
        public void LinkedList_Recovery_RemoveLastOnSingleton_Differential() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //
            {
                var list = s.Space.CreateLinkedList<int>("bar");

                list.Add(2);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                });
            }

            //
            // Get the linked list and remove an element.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                list.RemoveLast();
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.Delete, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.Delete, "state/item/bar/data", "0"),
                });
            }

            //
            // Get and assert the linked list.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                AssertList(list, Array.Empty<int>());
            }
        });

        [TestMethod]
        public void LinkedList_Recovery_RemoveLast() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //
            {
                var list = s.Space.CreateLinkedList<int>("bar");

                list.Add(2);
                list.Add(3);
                list.Add(5);
                list.Add(6);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "3"),
                });
            }

            //
            // Get the linked list and remove an element.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                list.RemoveLast();
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                });
            }

            //
            // Get and assert the linked list.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                AssertList(list, new[] { 2, 3, 5 });
            }
        });

        [TestMethod]
        public void LinkedList_Recovery_RemoveLast_Differential() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //
            {
                var list = s.Space.CreateLinkedList<int>("bar");

                list.Add(2);
                list.Add(3);
                list.Add(5);
                list.Add(6);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "3"),
                });
            }

            //
            // Get the linked list and remove an element.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                list.RemoveLast();
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"), // Update the next reference.
                    (StateWriterOperationKind.Delete, "state/item/bar/metadata", "3"),
                    (StateWriterOperationKind.Delete, "state/item/bar/data", "3"),
                });
            }

            //
            // Get and assert the linked list.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                AssertList(list, new[] { 2, 3, 5 });
            }
        });

        [TestMethod]
        public void LinkedList_Recovery_Remove_First() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //
            {
                var list = s.Space.CreateLinkedList<int>("bar");

                list.Add(1);
                list.Add(2);
                list.Add(3);
                list.Add(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "3"),
                });
            }

            //
            // Get the linked list and remove an element.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                list.Remove(list.First);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "3"),
                });
            }

            //
            // Get and assert the linked list.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                AssertList(list, new[] { 2, 3, 5 });
            }
        });

        [TestMethod]
        public void LinkedList_Recovery_Remove_First_Differential() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //
            {
                var list = s.Space.CreateLinkedList<int>("bar");

                list.Add(1);
                list.Add(2);
                list.Add(3);
                list.Add(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "3"),
                });
            }

            //
            // Get the linked list and remove an element.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                list.Remove(list.First);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.Delete, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.Delete, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"), // Update the previous reference.
                });
            }

            //
            // Get and assert the linked list.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                AssertList(list, new[] { 2, 3, 5 });
            }
        });

        [TestMethod]
        public void LinkedList_Recovery_Remove_Last() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //
            {
                var list = s.Space.CreateLinkedList<int>("bar");

                list.Add(2);
                list.Add(3);
                list.Add(5);
                list.Add(6);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "3"),
                });
            }

            //
            // Get the linked list and remove an element.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                list.Remove(list.Last);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                });
            }

            //
            // Get and assert the linked list.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                AssertList(list, new[] { 2, 3, 5 });
            }
        });

        [TestMethod]
        public void LinkedList_Recovery_Remove_Last_Differential() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //
            {
                var list = s.Space.CreateLinkedList<int>("bar");

                list.Add(2);
                list.Add(3);
                list.Add(5);
                list.Add(6);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "3"),
                });
            }

            //
            // Get the linked list and remove an element.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                list.Remove(list.Last);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"), // Update the next reference.
                    (StateWriterOperationKind.Delete, "state/item/bar/metadata", "3"),
                    (StateWriterOperationKind.Delete, "state/item/bar/data", "3"),
                });
            }

            //
            // Get and assert the linked list.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                AssertList(list, new[] { 2, 3, 5 });
            }
        });

        [TestMethod]
        public void LinkedList_Recovery_Remove() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //
            {
                var list = s.Space.CreateLinkedList<int>("bar");

                list.Add(2);
                list.Add(3);
                list.Add(4);
                list.Add(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "3"),
                });
            }

            //
            // Get the linked list and remove an element.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                list.Remove(list.Last.Previous);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "3"),
                });
            }

            //
            // Get and assert the linked list.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                AssertList(list, new[] { 2, 3, 5 });
            }
        });

        [TestMethod]
        public void LinkedList_Recovery_Remove_Differential() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //
            {
                var list = s.Space.CreateLinkedList<int>("bar");

                list.Add(2);
                list.Add(3);
                list.Add(4);
                list.Add(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "3"),
                });
            }

            //
            // Get the linked list and remove an element.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                list.Remove(list.Last.Previous);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"), // Update the next reference.
                    (StateWriterOperationKind.Delete, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.Delete, "state/item/bar/data", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "3"), // Update the previous reference.
                });
            }

            //
            // Get and assert the linked list.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                AssertList(list, new[] { 2, 3, 5 });
            }
        });

        [TestMethod]
        public void LinkedList_Recovery_Clear() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //
            {
                var list = s.Space.CreateLinkedList<int>("bar");

                list.Add(2);
                list.Add(3);
                list.Add(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                });
            }

            //
            // Get the linked list and clear it.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                list.Clear();
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                });
            }

            //
            // Get and assert the linked list.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                AssertList(list, Array.Empty<int>());
            }
        });

        [TestMethod]
        public void LinkedList_Recovery_Clear_Differential() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //
            {
                var list = s.Space.CreateLinkedList<int>("bar");

                list.Add(2);
                list.Add(3);
                list.Add(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                });
            }

            //
            // Get the linked list and clear it.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                list.Clear();
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.Delete, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.Delete, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.Delete, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.Delete, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.Delete, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.Delete, "state/item/bar/data", "2"),
                });
            }

            //
            // Get and assert the linked list.
            //
            {
                var list = s.Space.GetLinkedList<int>("bar");

                AssertList(list, Array.Empty<int>());
            }
        });

        [TestMethod]
        public void LinkedList_Recovery_Delete() => WithNewSpace(s =>
        {
            //
            // Create a linked list.
            //
            {
                var list = s.Space.CreateLinkedList<int>("bar");

                list.Add(2);
                list.Add(3);
                list.Add(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "2"),
                });
            }

            //
            // Delete the linked list.
            //
            s.Space.Delete("bar");

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.Delete, "state/index", "bar"),
                    (StateWriterOperationKind.Delete, "state/item/bar/metadata", "0"),
                    (StateWriterOperationKind.Delete, "state/item/bar/data", "0"),
                    (StateWriterOperationKind.Delete, "state/item/bar/metadata", "1"),
                    (StateWriterOperationKind.Delete, "state/item/bar/data", "1"),
                    (StateWriterOperationKind.Delete, "state/item/bar/metadata", "2"),
                    (StateWriterOperationKind.Delete, "state/item/bar/data", "2"),
                });
            }

            //
            // Assert the linked list is gone.
            //

            Assert.ThrowsException<KeyNotFoundException>(() => s.Space.GetLinkedList<int>("bar"));
        });

        // TODO: Tests that reuse nodes, multiple edits to nodes, etc.

        private static void AssertList<T>(ILinkedList<T> list, T[] values)
        {
            //
            // Check Count property.
            //
            Assert.AreEqual(values.Length, list.Count);

            //
            // Check enumeration behavior.
            //
            Assert.IsTrue(list.SequenceEqual(values));

            //
            // Check CopyTo behavior.
            //
            var res = new T[values.Length];
            list.CopyTo(res, 0);
            Assert.IsTrue(res.SequenceEqual(values));

            //
            // Assert elements from head to tail.
            //
            {
                var node = list.First;

                for (var i = 0; i < values.Length; i++)
                {
                    Assert.AreEqual(values[i], node.Value);
                    node = node.Next;
                }

                Assert.IsNull(node);
            }

            //
            // Assert elements from tail to head.
            //
            {
                var node = list.Last;

                for (var i = values.Length - 1; i >= 0; i--)
                {
                    Assert.AreEqual(values[i], node.Value);
                    node = node.Previous;
                }

                Assert.IsNull(node);
            }

            //
            // Check Contains behavior.
            //
            foreach (var value in values)
            {
                Assert.IsTrue(list.Contains(value));
            }
        }
    }
}
