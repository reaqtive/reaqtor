// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtive.Storage;

using Tests.ReifiedOperations;

using Utilities;

namespace Tests
{
    [TestClass]
    public class ListTests : PersistedTestBase
    {
        [TestMethod]
        public void List_CreateAndGet()
        {
            var assert = AssertOperation.WithAssert(MsTestAssert.Instance);
            var space = new PersistedObjectSpace(new SerializationFactory());

            CreateAndGetCore(assert).Accept(space);
        }

        [TestMethod]
        public void List_Volatile_CreateAndGet()
        {
            var assert = AssertOperation.WithAssert(MsTestAssert.Instance);
            var space = new VolatilePersistedObjectSpace();

            CreateAndGetCore(assert).Accept(space);
        }

        private static IOperation<IPersistedObjectSpace> CreateAndGetCore(IAssertOperationFactory assert) =>
            Operation.Sequence(

                //
                // Exceptions thrown by Create.
                //
                Operation.Sequence(
                    assert.ThrowsException<ArgumentNullException>().When(PersistedObjectSpaceOperation.CreateList<int>(null))
                ),

                //
                // Exceptions thrown by Get.
                //
                Operation.Sequence(
                    assert.ThrowsException<ArgumentNullException>().When(PersistedObjectSpaceOperation.GetList<int>(null)),
                    assert.ThrowsException<KeyNotFoundException>().When(PersistedObjectSpaceOperation.GetList<int>("bar"))
                ),

                //
                // Exceptions thrown by Delete.
                //
                Operation.Sequence(
                    assert.ThrowsException<KeyNotFoundException>().When(PersistedObjectSpaceOperation.Delete("bar"))
                ),

                //
                // Create a new list.
                //
                PersistedObjectSpaceOperation.CreateList<int>("bar").Apply(list =>

                    //
                    // Check the new list is present in the object space.
                    //
                    Operation.Sequence(

                        //
                        // Assert we can get the same instance back using GetList.
                        //
                        assert.AreSame(
                            PersistedObjectSpaceOperation.GetList<int>("bar"),
                            list
                        ),

                        //
                        // Assert we can't create an artifact with the same name.
                        //
                        Operation.Sequence(
                            assert.ThrowsException<InvalidOperationException>().When(PersistedObjectSpaceOperation.CreateArray<int>("bar", 42)),
                            assert.ThrowsException<InvalidOperationException>().When(PersistedObjectSpaceOperation.CreateDictionary<int, int>("bar")),
                            assert.ThrowsException<InvalidOperationException>().When(PersistedObjectSpaceOperation.CreateList<int>("bar")),
                            assert.ThrowsException<InvalidOperationException>().When(PersistedObjectSpaceOperation.CreateQueue<int>("bar")),
                            assert.ThrowsException<InvalidOperationException>().When(PersistedObjectSpaceOperation.CreateSet<int>("bar")),
                            assert.ThrowsException<InvalidOperationException>().When(PersistedObjectSpaceOperation.CreateStack<int>("bar")),
                            assert.ThrowsException<InvalidOperationException>().When(PersistedObjectSpaceOperation.CreateValue<int>("bar"))
                        )
                    )
                ),

                //
                // Delete list.
                //
                PersistedObjectSpaceOperation.Delete("bar"),

                //
                // Check the list is no longer present in the object space.
                //
                Operation.Sequence(
                    assert.ThrowsException<KeyNotFoundException>().When(PersistedObjectSpaceOperation.GetList<int>("bar")),
                    assert.ThrowsException<KeyNotFoundException>().When(PersistedObjectSpaceOperation.Delete("bar"))
                )
            );

        [TestMethod]
        public void List_ManOrBoy()
        {
            var assert = AssertOperation.WithAssert(MsTestAssert.Instance);
            var space = new PersistedObjectSpace(new SerializationFactory());

            ManOrBoyCore(assert).Accept(space);
        }

        [TestMethod]
        public void List_Volatile_ManOrBoy()
        {
            var assert = AssertOperation.WithAssert(MsTestAssert.Instance);
            var space = new VolatilePersistedObjectSpace();

            ManOrBoyCore(assert).Accept(space);
        }

        private static IOperation<IPersistedObjectSpace> ManOrBoyCore(IAssertOperationFactory assert) =>
            PersistedObjectSpaceOperation.CreateList<int>("bar").Bind(list =>
                Operation.Sequence(

                    //
                    // Check the identifier
                    //
                    assert.AreEqual(
                        list.GetId(),
                        "bar"
                    ),

                    //
                    // Created with zero length
                    //
                    Operation.Sequence(
                        assert.AreEqual(
                            list.Count(),
                            0
                        ),
                        assert.AreSequenceEqual(
                            list.Enumerate(),
                            Array.Empty<int>()
                        )
                    ),

                    //
                    // Created as mutable
                    //
                    assert.IsFalse(list.IsReadOnly()),

                    //
                    // Out of bounds exceptions are thrown
                    //
                    Operation.Sequence(
                        assert.ThrowsException<ArgumentOutOfRangeException>().When(
                            list.Get(-1)
                        ),
                        assert.ThrowsException<ArgumentOutOfRangeException>().When(
                            list.Get(0)
                        )
                    ),

                    //
                    // Can't find anything
                    //
                    Operation.Sequence(
                        assert.IsFalse(list.Contains(42)),
                        assert.IsTrue(list.IndexOf(42).Select(i => i < 0))
                    ),

                    //
                    // Add element
                    //
                    list.Add(42),

                    //
                    // Out of bounds exceptions are thrown
                    //
                    Operation.Sequence(
                        assert.ThrowsException<ArgumentOutOfRangeException>().When(
                            list.Get(-1)
                        ),
                        assert.ThrowsException<ArgumentOutOfRangeException>().When(
                            list.Get(1)
                        )
                    ),

                    //
                    // Check values after addition
                    //
                    Operation.Sequence(
                        assert.AreEqual(
                            list.Count(),
                            1
                        ),
                        assert.AreEqual(
                            list.Get(0),
                            42
                        ),
                        assert.AreSequenceEqual(
                            list.Enumerate(),
                            new[] { 42 }
                        )
                    ),

                    //
                    // Can find value
                    //
                    Operation.Sequence(
                        assert.IsTrue(list.Contains(42)),
                        assert.AreEqual(
                            list.IndexOf(42),
                            0
                        )
                    ),

                    //
                    // Perform assignment
                    //
                    list.Set(0, 43),

                    //
                    // Check values after assignment
                    //
                    Operation.Sequence(
                        assert.AreEqual(
                            list.Count(),
                            1
                        ),
                        assert.AreEqual(
                            list.Get(0),
                            43
                        ),
                        assert.AreSequenceEqual(
                            list.Enumerate(),
                            new[] { 43 }
                        )
                    ),

                    //
                    // Can find updated value
                    //
                    Operation.Sequence(
                        assert.IsTrue(list.Contains(43)),
                        assert.AreEqual(
                            list.IndexOf(43),
                            0
                        )
                    ),

                    //
                    // Add second value
                    //
                    list.Add(44),

                    //
                    // Out of bounds exceptions are thrown
                    //
                    Operation.Sequence(
                        assert.ThrowsException<ArgumentOutOfRangeException>().When(
                            list.Get(-1)
                        ),
                        assert.ThrowsException<ArgumentOutOfRangeException>().When(
                            list.Get(2)
                        )
                    ),

                    //
                    // Check values after addition
                    //
                    Operation.Sequence(
                        assert.AreEqual(
                            list.Count(),
                            2
                        ),
                        assert.AreEqual(
                            list.Get(0),
                            43
                        ),
                        assert.AreEqual(
                            list.Get(1),
                            44
                        ),
                        assert.AreSequenceEqual(
                            list.Enumerate(),
                            new[] { 43, 44 }
                        )
                    ),

                    //
                    // Can find values
                    //
                    Operation.Sequence(
                        assert.IsTrue(list.Contains(43)),
                        assert.AreEqual(
                            list.IndexOf(43),
                            0
                        ),
                        assert.IsTrue(list.Contains(44)),
                        assert.AreEqual(
                            list.IndexOf(44),
                            1
                        )
                    ),

                    //
                    // Insert value at the start
                    //
                    list.Insert(0, 42),

                    //
                    // Out of bounds exceptions are thrown
                    //
                    Operation.Sequence(
                        assert.ThrowsException<ArgumentOutOfRangeException>().When(
                            list.Get(-1)
                        ),
                        assert.ThrowsException<ArgumentOutOfRangeException>().When(
                            list.Get(3)
                        )
                    ),

                    //
                    // Check values after addition
                    //
                    Operation.Sequence(
                        assert.AreEqual(
                            list.Count(),
                            3
                        ),
                        assert.AreEqual(
                            list.Get(0),
                            42
                        ),
                        assert.AreEqual(
                            list.Get(1),
                            43
                        ),
                        assert.AreEqual(
                            list.Get(2),
                            44
                        ),
                        assert.AreSequenceEqual(
                            list.Enumerate(),
                            new[] { 42, 43, 44 }
                        )
                    ),

                    //
                    // Can find values
                    //
                    Operation.Sequence(
                        assert.IsTrue(list.Contains(42)),
                        assert.AreEqual(
                            list.IndexOf(42),
                            0
                        ),
                        assert.IsTrue(list.Contains(43)),
                        assert.AreEqual(
                            list.IndexOf(43),
                            1
                        ),
                        assert.IsTrue(list.Contains(44)),
                        assert.AreEqual(
                            list.IndexOf(44),
                            2
                        )
                    ),

                    //
                    // Remove in the middle
                    //
                    list.RemoveAt(1),

                    //
                    // Out of bounds exceptions are thrown
                    //
                    Operation.Sequence(
                        assert.ThrowsException<ArgumentOutOfRangeException>().When(
                            list.Get(-1)
                        ),
                        assert.ThrowsException<ArgumentOutOfRangeException>().When(
                            list.Get(2)
                        )
                    ),

                    //
                    // Check values after addition
                    //
                    Operation.Sequence(
                        assert.AreEqual(
                            list.Count(),
                            2
                        ),
                        assert.AreEqual(
                            list.Get(0),
                            42
                        ),
                        assert.AreEqual(
                            list.Get(1),
                            44
                        ),
                        assert.AreSequenceEqual(
                            list.Enumerate(),
                            new[] { 42, 44 }
                        )
                    ),

                    //
                    // Can find values
                    //
                    Operation.Sequence(
                        assert.IsTrue(list.Contains(42)),
                        assert.AreEqual(
                            list.IndexOf(42),
                            0
                        ),
                        assert.IsFalse(list.Contains(43)),
                        assert.AreEqual(
                            list.IndexOf(43),
                            -1
                        ),
                        assert.IsTrue(list.Contains(44)),
                        assert.AreEqual(
                            list.IndexOf(44),
                            1
                        )
                    ),

                    //
                    // Insert value in the middle start
                    //
                    list.Insert(1, 43),

                    //
                    // Out of bounds exceptions are thrown
                    //
                    Operation.Sequence(
                        assert.ThrowsException<ArgumentOutOfRangeException>().When(
                            list.Get(-1)
                        ),
                        assert.ThrowsException<ArgumentOutOfRangeException>().When(
                            list.Get(3)
                        )
                    ),

                    //
                    // Check values after addition
                    //
                    Operation.Sequence(
                        assert.AreEqual(
                            list.Count(),
                            3
                        ),
                        assert.AreEqual(
                            list.Get(0),
                            42
                        ),
                        assert.AreEqual(
                            list.Get(1),
                            43
                        ),
                        assert.AreEqual(
                            list.Get(2),
                            44
                        ),
                        assert.AreSequenceEqual(
                            list.Enumerate(),
                            new[] { 42, 43, 44 }
                        )
                    ),

                    //
                    // Can find values
                    //
                    Operation.Sequence(
                        assert.IsTrue(list.Contains(42)),
                        assert.AreEqual(
                            list.IndexOf(42),
                            0
                        ),
                        assert.IsTrue(list.Contains(43)),
                        assert.AreEqual(
                            list.IndexOf(43),
                            1
                        ),
                        assert.IsTrue(list.Contains(44)),
                        assert.AreEqual(
                            list.IndexOf(44),
                            2
                        )
                    ),

                    //
                    // Remove value
                    //
                    assert.IsTrue(list.Remove(43)),

                    //
                    // Out of bounds exceptions are thrown
                    //
                    Operation.Sequence(
                        assert.ThrowsException<ArgumentOutOfRangeException>().When(
                            list.Get(-1)
                        ),
                        assert.ThrowsException<ArgumentOutOfRangeException>().When(
                            list.Get(2)
                        )
                    ),

                    //
                    // Check values after addition
                    //
                    Operation.Sequence(
                        assert.AreEqual(
                            list.Count(),
                            2
                        ),
                        assert.AreEqual(
                            list.Get(0),
                            42
                        ),
                        assert.AreEqual(
                            list.Get(1),
                            44
                        ),
                        assert.AreSequenceEqual(
                            list.Enumerate(),
                            new[] { 42, 44 }
                        )
                    ),

                    //
                    // Can find values
                    //
                    Operation.Sequence(
                        assert.IsTrue(list.Contains(42)),
                        assert.AreEqual(
                            list.IndexOf(42),
                            0
                        ),
                        assert.IsTrue(list.Contains(44)),
                        assert.AreEqual(
                            list.IndexOf(44),
                            1
                        )
                    ),

                    //
                    // Can't find removed values
                    //
                    Operation.Sequence(
                        assert.IsFalse(list.Contains(43)),
                        assert.AreEqual(
                            list.IndexOf(43),
                            -1
                        ),
                        assert.IsFalse(list.Remove(43))
                    ),

                    //
                    // Clear
                    //
                    list.Clear(),

                    //
                    // List is empty
                    //
                    Operation.Sequence(
                        assert.AreEqual(
                            list.Count(),
                            0
                        ),
                        assert.AreSequenceEqual(
                            list.Enumerate(),
                            Array.Empty<int>()
                        )
                    ),

                    //
                    // Out of bounds exceptions are thrown
                    //
                    Operation.Sequence(
                        assert.ThrowsException<ArgumentOutOfRangeException>().When(
                            list.Get(-1)
                        ),
                        assert.ThrowsException<ArgumentOutOfRangeException>().When(
                            list.Get(0)
                        )
                    ),

                    //
                    // Can't find anything
                    //
                    Operation.Sequence(
                        assert.IsFalse(list.Contains(42)),
                        assert.IsFalse(list.Contains(43)),
                        assert.IsFalse(list.Contains(44))
                    )
                )
            );

        [TestMethod]
        public void List_Recovery_CreateDefault() => WithNewSpace(s =>
        {
            //
            // Create a list.
            //

            _ = s.Space.CreateList<int>("bar");

            //
            // Save the space to the store.
            //
            {
                var edits = s.SaveSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "length"),
                });
            }

            //
            // Create a new space.
            //

            s.CreateSpace();

            //
            // Assert the new space does not contain the list.
            //

            Assert.ThrowsException<KeyNotFoundException>(() => s.Space.GetList<int>("bar"));

            //
            // Load the space from the store.
            //

            s.LoadSpace();

            //
            // Get the list.
            //

            var bar = s.Space.GetList<int>("bar");

            //
            // Assert the list element count.
            //

            Assert.AreEqual(0, bar.Count);
        });

        [TestMethod]
        public void List_Recovery_CreateList() => WithNewSpace(s =>
        {
            //
            // Create a list.
            //
            {
                var list = s.Space.CreateList<int>("bar");

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
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "length"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                });
            }

            //
            // Get and assert the list.
            //
            {
                var list = s.Space.GetList<int>("bar");

                Assert.AreEqual(4, list.Count);
                Assert.IsTrue(new[] { 2, 3, 5, 7 }.SequenceEqual(list));
            }
        });

        [TestMethod]
        public void List_Recovery_NoChange() => WithNewSpace(s =>
        {
            //
            // Create a list.
            //
            {
                var list = s.Space.CreateList<int>("bar");

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
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "length"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
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
            // Get and assert the list.
            //
            {
                var list = s.Space.GetList<int>("bar");

                Assert.AreEqual(4, list.Count);
                Assert.IsTrue(new[] { 2, 3, 5, 7 }.SequenceEqual(list));
            }
        });

        [TestMethod]
        public void List_Recovery_Add() => WithNewSpace(s =>
        {
            //
            // Create a list.
            //
            {
                var list = s.Space.CreateList<int>("bar");

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
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "length"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                });
            }

            //
            // Get the list and add an element.
            //
            {
                var list = s.Space.GetList<int>("bar");

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
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "length"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                });
            }

            //
            // Get and assert the list.
            //
            {
                var list = s.Space.GetList<int>("bar");

                Assert.AreEqual(4, list.Count);
                Assert.IsTrue(new[] { 2, 3, 5, 7 }.SequenceEqual(list));
            }
        });

        [TestMethod]
        public void List_Recovery_Add_Differential() => WithNewSpace(s =>
        {
            //
            // Create a list.
            //
            {
                var list = s.Space.CreateList<int>("bar");

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
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "length"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                });
            }

            //
            // Get the list and add an element.
            //
            {
                var list = s.Space.GetList<int>("bar");

                list.Add(7);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "length"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                });
            }

            //
            // Get and assert the list.
            //
            {
                var list = s.Space.GetList<int>("bar");

                Assert.AreEqual(4, list.Count);
                Assert.IsTrue(new[] { 2, 3, 5, 7 }.SequenceEqual(list));
            }
        });

        [TestMethod]
        public void List_Recovery_Edit() => WithNewSpace(s =>
        {
            //
            // Create a list.
            //
            {
                var list = s.Space.CreateList<int>("bar");

                list.Add(2);
                list.Add(3);
                list.Add(42);
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
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "length"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                });
            }

            //
            // Get the list and edit an element.
            //
            {
                var list = s.Space.GetList<int>("bar");

                list[2] = 5;
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "length"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                });
            }

            //
            // Get and assert the list.
            //
            {
                var list = s.Space.GetList<int>("bar");

                Assert.AreEqual(4, list.Count);
                Assert.IsTrue(new[] { 2, 3, 5, 7 }.SequenceEqual(list));
            }
        });

        [TestMethod]
        public void List_Recovery_Edit_Differential() => WithNewSpace(s =>
        {
            //
            // Create a list.
            //
            {
                var list = s.Space.CreateList<int>("bar");

                list.Add(2);
                list.Add(3);
                list.Add(42);
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
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "length"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                });
            }

            //
            // Get the list and add an element.
            //
            {
                var list = s.Space.GetList<int>("bar");

                list[2] = 5;
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                });
            }

            //
            // Get and assert the list.
            //
            {
                var list = s.Space.GetList<int>("bar");

                Assert.AreEqual(4, list.Count);
                Assert.IsTrue(new[] { 2, 3, 5, 7 }.SequenceEqual(list));
            }
        });

        [TestMethod]
        public void List_Recovery_Edit_Many() => WithNewSpace(s =>
        {
            //
            // Create a list.
            //
            {
                var list = s.Space.CreateList<int>("bar");

                list.Add(41);
                list.Add(3);
                list.Add(42);
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
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "length"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                });
            }

            //
            // Get the list and edit an element.
            //
            {
                var list = s.Space.GetList<int>("bar");

                list[0] = 2;
                list[2] = 5;
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "length"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                });
            }

            //
            // Get and assert the list.
            //
            {
                var list = s.Space.GetList<int>("bar");

                Assert.AreEqual(4, list.Count);
                Assert.IsTrue(new[] { 2, 3, 5, 7 }.SequenceEqual(list));
            }
        });

        [TestMethod]
        public void List_Recovery_Edit_Many_Differential() => WithNewSpace(s =>
        {
            //
            // Create a list.
            //
            {
                var list = s.Space.CreateList<int>("bar");

                list.Add(41);
                list.Add(3);
                list.Add(42);
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
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "length"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                });
            }

            //
            // Get the list and add an element.
            //
            {
                var list = s.Space.GetList<int>("bar");

                list[0] = 2;
                list[2] = 5;
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                });
            }

            //
            // Get and assert the list.
            //
            {
                var list = s.Space.GetList<int>("bar");

                Assert.AreEqual(4, list.Count);
                Assert.IsTrue(new[] { 2, 3, 5, 7 }.SequenceEqual(list));
            }
        });

        [TestMethod]
        public void List_Recovery_RemoveAt() => WithNewSpace(s =>
        {
            //
            // Create a list.
            //
            {
                var list = s.Space.CreateList<int>("bar");

                list.Add(2);
                list.Add(3);
                list.Add(42);
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
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "length"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                });
            }

            //
            // Get the list and remove an element.
            //
            {
                var list = s.Space.GetList<int>("bar");

                list.RemoveAt(2);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "length"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    // NB: No delete.
                });
            }

            //
            // Get and assert the list.
            //
            {
                var list = s.Space.GetList<int>("bar");

                Assert.AreEqual(3, list.Count);
                Assert.IsTrue(new[] { 2, 3, 5 }.SequenceEqual(list));
            }
        });

        [TestMethod]
        public void List_Recovery_RemoveAt_Differential() => WithNewSpace(s =>
        {
            //
            // Create a list.
            //
            {
                var list = s.Space.CreateList<int>("bar");

                list.Add(2);
                list.Add(3);
                list.Add(42);
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
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "length"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                });
            }

            //
            // Get the list and remove an element.
            //
            {
                var list = s.Space.GetList<int>("bar");

                list.RemoveAt(2);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "length"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "3"),
                });
            }

            //
            // Get and assert the list.
            //
            {
                var list = s.Space.GetList<int>("bar");

                Assert.AreEqual(3, list.Count);
                Assert.IsTrue(new[] { 2, 3, 5 }.SequenceEqual(list));
            }
        });

        [TestMethod]
        public void List_Recovery_Insert() => WithNewSpace(s =>
        {
            //
            // Create a list.
            //
            {
                var list = s.Space.CreateList<int>("bar");

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
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "length"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                });
            }

            //
            // Get the list and insert an element.
            //
            {
                var list = s.Space.GetList<int>("bar");

                list.Insert(2, 5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "length"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                });
            }

            //
            // Get and assert the list.
            //
            {
                var list = s.Space.GetList<int>("bar");

                Assert.AreEqual(4, list.Count);
                Assert.IsTrue(new[] { 2, 3, 5, 7 }.SequenceEqual(list));
            }
        });

        [TestMethod]
        public void List_Recovery_Insert_Differential() => WithNewSpace(s =>
        {
            //
            // Create a list.
            //
            {
                var list = s.Space.CreateList<int>("bar");

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
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "length"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                });
            }

            //
            // Get the list and insert an element.
            //
            {
                var list = s.Space.GetList<int>("bar");

                list.Insert(2, 5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "length"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                });
            }

            //
            // Get and assert the list.
            //
            {
                var list = s.Space.GetList<int>("bar");

                Assert.AreEqual(4, list.Count);
                Assert.IsTrue(new[] { 2, 3, 5, 7 }.SequenceEqual(list));
            }
        });

        [TestMethod]
        public void List_Potpourri_Insert() => WithNewSpace(s =>
        {
            //
            // Create a list.
            //
            {
                var list = s.Space.CreateList<int>("bar");

                list.Add(1);
                list.Add(4);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "length"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                });
            }

            //
            // Get the list and make some edits.
            //
            {
                var list = s.Space.GetList<int>("bar");

                list.Insert(0, 0); // 0, 1, 4
                list.Add(3);       // 0, 1, 4, 3
                list.Remove(4);    // 0, 1, 3
                list.Add(5);       // 0, 1, 3, 5
                list.Insert(3, 4); // 0, 1, 3, 4, 5
                list.Insert(2, 2); // 0, 1, 2, 3, 4, 5
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "length"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "4"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "5"),
                });
            }

            //
            // Get and assert the list.
            //
            {
                var list = s.Space.GetList<int>("bar");

                Assert.AreEqual(6, list.Count);
                Assert.IsTrue(new[] { 0, 1, 2, 3, 4, 5 }.SequenceEqual(list));
            }
        });

        [TestMethod]
        public void List_Recovery_Potpourri_Differential() => WithNewSpace(s =>
        {
            //
            // Create a list.
            //
            {
                var list = s.Space.CreateList<int>("bar");

                list.Add(1);
                list.Add(4);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "length"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                });
            }

            //
            // Get the list and make some edits.
            //
            {
                var list = s.Space.GetList<int>("bar");

                list.Insert(0, 0); // 0, 1, 4
                list.Add(3);       // 0, 1, 4, 3
                list.Remove(4);    // 0, 1, 3
                list.Add(5);       // 0, 1, 3, 5
                list.Insert(3, 4); // 0, 1, 3, 4, 5
                list.Insert(2, 2); // 0, 1, 2, 3, 4, 5
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "length"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "4"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "5"),
                });
            }

            //
            // Get and assert the list.
            //
            {
                var list = s.Space.GetList<int>("bar");

                Assert.AreEqual(6, list.Count);
                Assert.IsTrue(new[] { 0, 1, 2, 3, 4, 5 }.SequenceEqual(list));
            }
        });

        [TestMethod]
        public void List_Recovery_Delete() => WithNewSpace(s =>
        {
            //
            // Create a list.
            //
            {
                var list = s.Space.CreateList<int>("bar");

                list.Add(0);
                list.Add(1);
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
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "length"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                });
            }

            //
            // Delete the list.
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
                    (StateWriterOperationKind.Delete, "state/item/bar/metadata", "length"),
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "2"),
                });
            }

            //
            // Assert the list is gone.
            //

            Assert.ThrowsException<KeyNotFoundException>(() => s.Space.GetList<int>("bar"));
        });

        [TestMethod]
        public void List_Recovery_Delete_PendingRemove() => WithNewSpace(s =>
        {
            //
            // Create a list.
            //
            {
                var list = s.Space.CreateList<int>("bar");

                list.Add(0);
                list.Add(1);
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
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "length"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                });
            }

            //
            // Get the list and remove an element.
            //
            {
                var list = s.Space.GetList<int>("bar");

                list.RemoveAt(1);
            }

            //
            // Delete the list.
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
                    (StateWriterOperationKind.Delete, "state/item/bar/metadata", "length"),
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "2"),
                });
            }

            //
            // Assert the list is gone.
            //

            Assert.ThrowsException<KeyNotFoundException>(() => s.Space.GetList<int>("bar"));
        });

        [TestMethod]
        public void List_Recovery_Delete_PendingAdd() => WithNewSpace(s =>
        {
            //
            // Create a list.
            //
            {
                var list = s.Space.CreateList<int>("bar");

                list.Add(0);
                list.Add(1);
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
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "length"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                });
            }

            //
            // Get the list and remove an element.
            //
            {
                var list = s.Space.GetList<int>("bar");

                list.Add(3);
            }

            //
            // Delete the list.
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
                    (StateWriterOperationKind.Delete, "state/item/bar/metadata", "length"),
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "2"),
                });
            }

            //
            // Assert the list is gone.
            //

            Assert.ThrowsException<KeyNotFoundException>(() => s.Space.GetList<int>("bar"));
        });
    }
}
