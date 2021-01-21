// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using Reaqtive.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using Tests.ReifiedOperations;
using Utilities;

namespace Tests
{
    [TestClass]
    public class SetTests : PersistedTestBase
    {
        [TestMethod]
        public void Set_CreateAndGet()
        {
            var assert = AssertOperation.WithAssert(MsTestAssert.Instance);
            var space = new PersistedObjectSpace(new SerializationFactory());

            CreateAndGetCore(assert).Accept(space);
        }

        [TestMethod]
        public void Set_Volatile_CreateAndGet()
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
                    assert.ThrowsException<ArgumentNullException>().When(PersistedObjectSpaceOperation.CreateSet<int>(null))
                ),

                //
                // Exceptions thrown by Get.
                //
                Operation.Sequence(
                    assert.ThrowsException<ArgumentNullException>().When(PersistedObjectSpaceOperation.GetSet<int>(null)),
                    assert.ThrowsException<KeyNotFoundException>().When(PersistedObjectSpaceOperation.GetSet<int>("bar"))
                ),

                //
                // Exceptions thrown by Delete.
                //
                Operation.Sequence(
                    assert.ThrowsException<KeyNotFoundException>().When(PersistedObjectSpaceOperation.Delete("bar"))
                ),

                //
                // Create a new set.
                //
                PersistedObjectSpaceOperation.CreateSet<int>("bar").Apply(set =>

                    //
                    // Check the new set is present in the object space.
                    //
                    Operation.Sequence(

                        //
                        // Assert we can get the same instance back using GetSet.
                        //
                        assert.AreSame(
                            PersistedObjectSpaceOperation.GetSet<int>("bar"),
                            set
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
                // Delete set.
                //
                PersistedObjectSpaceOperation.Delete("bar"),

                //
                // Check the set is no longer present in the object space.
                //
                Operation.Sequence(
                    assert.ThrowsException<KeyNotFoundException>().When(PersistedObjectSpaceOperation.GetSet<int>("bar")),
                    assert.ThrowsException<KeyNotFoundException>().When(PersistedObjectSpaceOperation.Delete("bar"))
                )
            );

        [TestMethod]
        public void Set_ManOrBoy()
        {
            var assert = AssertOperation.WithAssert(MsTestAssert.Instance);
            var space = new PersistedObjectSpace(new SerializationFactory());

            ManOrBoyCore(assert).Accept(space);
        }

        [TestMethod]
        public void Set_Volatile_ManOrBoy()
        {
            var assert = AssertOperation.WithAssert(MsTestAssert.Instance);
            var space = new VolatilePersistedObjectSpace();

            ManOrBoyCore(assert).Accept(space);
        }

        private static IOperation<IPersistedObjectSpace> ManOrBoyCore(IAssertOperationFactory assert) =>
            PersistedObjectSpaceOperation.CreateSet<int>("bar").Bind(set =>
                Operation.Sequence(

                    //
                    // Check the identifier
                    //
                    assert.AreEqual(
                        set.GetId(),
                        "bar"
                    ),

                    //
                    // Initially empty
                    //
                    Operation.Sequence(
                        assert.AreEqual(
                            set.Count(),
                            0
                        ),
                        assert.AreSequenceEqual(
                            set.Enumerate(),
                            Array.Empty<int>()
                        )
                    ),

                    //
                    // Created as mutable
                    //
                    assert.IsFalse(set.IsReadOnly()),

                    //
                    // Add an element
                    //
                    assert.IsTrue(set.Add(42)),

                    //
                    // Check value in set after addition
                    //
                    Operation.Sequence(
                        assert.AreEqual(
                            set.Count(),
                            1
                        ),
                        assert.AreSequenceEqual(
                            set.Enumerate(),
                            new[] { 42 }
                        )
                    ),

                    //
                    // Check membership
                    //
                    Operation.Sequence(
                        assert.IsTrue(set.Contains(42)),
                        assert.IsFalse(set.Contains(43))
                    ),

                    //
                    // Can't add an element twice
                    //
                    assert.IsFalse(set.Add(42)),

                    //
                    // Add another element
                    //
                    assert.IsTrue(set.Add(43)),

                    //
                    // Check value in set after addition
                    //
                    Operation.Sequence(
                        assert.AreEqual(
                            set.Count(),
                            2
                        ),
                        assert.AreSequenceEqual(
                            set.Enumerate().Select(xs => xs.OrderBy(x => x)),
                            new[] { 42, 43 }
                        )
                    ),

                    //
                    // Check membership
                    //
                    Operation.Sequence(
                        assert.IsTrue(set.Contains(42)),
                        assert.IsTrue(set.Contains(43))
                    ),

                    //
                    // Check various set theoretical predicates
                    //
                    Operation.Sequence(

                        //
                        // SetEquals
                        //
                        Operation.Sequence(
                            assert.IsFalse(set.SetEquals(Array.Empty<int>())),
                            assert.IsFalse(set.SetEquals(new[] { 42 })),
                            assert.IsTrue(set.SetEquals(new[] { 42, 43 })),
                            assert.IsFalse(set.SetEquals(new[] { 41, 42, 43, 44 }))
                        ),

                        //
                        // Subset
                        //
                        Operation.Sequence(
                            assert.IsFalse(set.IsSubsetOf(Array.Empty<int>())),
                            assert.IsTrue(set.IsSubsetOf(new[] { 42, 43 })),
                            assert.IsTrue(set.IsSubsetOf(new[] { 41, 42, 43, 44 })),
                            assert.IsFalse(set.IsSubsetOf(new[] { 41, 42, -1, 44 }))
                        ),

                        //
                        // ProperSubset
                        //
                        Operation.Sequence(
                            assert.IsFalse(set.IsProperSubsetOf(Array.Empty<int>())),
                            assert.IsFalse(set.IsProperSubsetOf(new[] { 42, 43 })),
                            assert.IsTrue(set.IsProperSubsetOf(new[] { 41, 42, 43, 44 })),
                            assert.IsFalse(set.IsProperSubsetOf(new[] { 41, 42, -1, 44 }))
                        ),

                        //
                        // Superset
                        //
                        Operation.Sequence(
                            assert.IsTrue(set.IsSupersetOf(Array.Empty<int>())),
                            assert.IsTrue(set.IsSupersetOf(new[] { 42 })),
                            assert.IsTrue(set.IsSupersetOf(new[] { 42, 43 })),
                            assert.IsFalse(set.IsSupersetOf(new[] { 41, 42, 43, 44 }))
                        ),

                        //
                        // ProperSuperset
                        //
                        Operation.Sequence(
                            assert.IsTrue(set.IsProperSupersetOf(Array.Empty<int>())),
                            assert.IsTrue(set.IsProperSupersetOf(new[] { 42 })),
                            assert.IsFalse(set.IsProperSupersetOf(new[] { 42, 43 })),
                            assert.IsFalse(set.IsProperSupersetOf(new[] { 41, 42, 43, 44 }))
                        ),

                        //
                        // Overlap
                        //
                        Operation.Sequence(
                            assert.IsFalse(set.Overlaps(Array.Empty<int>())),
                            assert.IsTrue(set.Overlaps(new[] { 42 })),
                            assert.IsTrue(set.Overlaps(new[] { 42, 43 })),
                            assert.IsTrue(set.Overlaps(new[] { 41, 42, 49 }))
                        )
                    ),

                    //
                    // Clear the set
                    //
                    set.Clear(),

                    //
                    // Empty again
                    //
                    Operation.Sequence(
                        assert.AreEqual(
                            set.Count(),
                            0
                        ),
                        assert.AreSequenceEqual(
                            set.Enumerate(),
                            Array.Empty<int>()
                        )
                    ),

                    //
                    // Can't find anything
                    //
                    Operation.Sequence(
                        assert.IsFalse(set.Contains(41)),
                        assert.IsFalse(set.Contains(42)),
                        assert.IsFalse(set.Contains(43)),
                        assert.IsFalse(set.Contains(44))
                    ),

                    //
                    // Add an element
                    //
                    assert.IsTrue(set.Add(1)),

                    //
                    // Can remove the element
                    //
                    assert.IsTrue(set.Remove(1)),

                    //
                    // Empty again
                    //
                    Operation.Sequence(
                        assert.AreEqual(
                            set.Count(),
                            0
                        ),
                        assert.AreSequenceEqual(
                            set.Enumerate(),
                            Array.Empty<int>()
                        )
                    ),

                    //
                    // Can't remove twice
                    //
                    assert.IsFalse(set.Remove(1)),

                    //
                    // Set operations
                    //
                    Operation.Sequence(

                        //
                        // UnionWith
                        //
                        Operation.Sequence(

                            //
                            // Using IEnumerable<T>
                            //
                            set.UnionWith(new[] { 1, 2, 1, 3, 4, 2 }),

                            //
                            // Assert sequence
                            //
                            assert.AreSequenceEqual(
                                set.Enumerate().Select(xs => xs.OrderBy(x => x)),
                                new[] { 1, 2, 3, 4 }
                            ),

                            //
                            // Using HashSet<T>
                            //
                            set.UnionWith(new HashSet<int> { 6, 3, 2, 5 }),

                            //
                            // Assert sequence
                            //
                            assert.AreSequenceEqual(
                                set.Enumerate().Select(xs => xs.OrderBy(x => x)),
                                new[] { 1, 2, 3, 4, 5, 6 }
                            ),

                            //
                            // UnionWith(this) is no-op
                            //
                            set.This().Apply(it => set.UnionWith(it)),

                            //
                            // Assert sequence
                            //
                            assert.AreSequenceEqual(
                                set.Enumerate().Select(xs => xs.OrderBy(x => x)),
                                new[] { 1, 2, 3, 4, 5, 6 }
                            )
                        ),

                        //
                        // Clear and restore
                        //
                        Operation.Sequence(
                            set.Clear(),
                            set.UnionWith(new[] { 1, 2, 3, 4, 5, 6 })
                        ),

                        //
                        // ExceptWith
                        //
                        Operation.Sequence(

                            //
                            // Using IEnumerable<T>
                            //
                            set.ExceptWith(new[] { 2, 3, 7, 2 }),

                            //
                            // Assert sequence
                            //
                            assert.AreSequenceEqual(
                                set.Enumerate().Select(xs => xs.OrderBy(x => x)),
                                new[] { 1, 4, 5, 6 }
                            ),

                            //
                            // Using HashSet<T>
                            //
                            set.ExceptWith(new HashSet<int> { 4, 0, 2, 5 }),

                            //
                            // Assert sequence
                            //
                            assert.AreSequenceEqual(
                                set.Enumerate().Select(xs => xs.OrderBy(x => x)),
                                new[] { 1, 6 }
                            ),

                            //
                            // ExceptWith(this) is equivalent to Clear
                            //
                            set.This().Apply(it => set.ExceptWith(it)),

                            //
                            // Assert sequence
                            //
                            assert.AreEqual(
                                set.Count(),
                                0
                            )
                        ),

                        //
                        // Clear and restore
                        //
                        Operation.Sequence(
                            set.Clear(),
                            set.UnionWith(new[] { 1, 2, 3, 4, 5, 6 })
                        ),

                        //
                        // IntersectWith
                        //
                        Operation.Sequence(

                            //
                            // Using IEnumerable<T>
                            //
                            set.IntersectWith(new[] { 2, 3, 7, 1, 2, 4 }),

                            //
                            // Assert sequence
                            //
                            assert.AreSequenceEqual(
                                set.Enumerate().Select(xs => xs.OrderBy(x => x)),
                                new[] { 1, 2, 3, 4 }
                            ),

                            //
                            // Using HashSet<T>
                            //
                            set.IntersectWith(new HashSet<int> { 2, 3 }),

                            //
                            // Assert sequence
                            //
                            assert.AreSequenceEqual(
                                set.Enumerate().Select(xs => xs.OrderBy(x => x)),
                                new[] { 2, 3 }
                            ),

                            //
                            // IntersectWith(this) is no-op
                            //
                            set.This().Apply(it => set.IntersectWith(it)),

                            //
                            // Assert sequence
                            //
                            assert.AreSequenceEqual(
                                set.Enumerate().Select(xs => xs.OrderBy(x => x)),
                                new[] { 2, 3 }
                            )
                        ),

                        //
                        // Clear and restore
                        //
                        Operation.Sequence(
                            set.Clear(),
                            set.UnionWith(new[] { 1, 2, 3, 4, 5, 6 })
                        ),

                        //
                        // SymmetricExceptWith
                        //
                        Operation.Sequence(

                            //
                            // Using IEnumerable<T>
                            //
                            set.SymmetricExceptWith(new[] { 2, 0, 5, 0, 8, 2 }),

                            //
                            // Assert sequence
                            //
                            assert.AreSequenceEqual(
                                set.Enumerate().Select(xs => xs.OrderBy(x => x)),
                                new[] { 0, 1, 3, 4, 6, 8 }
                            ),

                            //
                            // Using HashSet<T>
                            //
                            set.SymmetricExceptWith(new HashSet<int> { 8, 5, 0, 2 }),

                            //
                            // Assert sequence
                            //
                            assert.AreSequenceEqual(
                                set.Enumerate().Select(xs => xs.OrderBy(x => x)),
                                new[] { 1, 2, 3, 4, 5, 6 }
                            ),

                            //
                            // SymmetricExceptWith(this) is equivalent to Clear
                            //
                            set.This().Apply(it => set.SymmetricExceptWith(it)),

                            //
                            // Assert sequence
                            //
                            assert.AreEqual(
                                set.Count(),
                                0
                            )
                        )
                    )
                )
            );

        [TestMethod]
        public void Set_Recovery_CreateDefault() => WithNewSpace(s =>
        {
            //
            // Create a set.
            //

            _ = s.Space.CreateSet<int>("bar");

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
            // Assert the new space does not contain the set.
            //

            Assert.ThrowsException<KeyNotFoundException>(() => s.Space.GetSet<int>("bar"));

            //
            // Load the space from the store.
            //

            s.LoadSpace();

            //
            // Get the set.
            //

            var bar = s.Space.GetSet<int>("bar");

            //
            // Assert the set size.
            //

            Assert.AreEqual(0, bar.Count);
        });

        [TestMethod]
        public void Set_Recovery_CreateSet() => WithNewSpace(s =>
        {
            //
            // Create a set.
            //
            {
                var set = s.Space.CreateSet<int>("bar");

                set.Add(2);
                set.Add(3);
                set.Add(5);
                set.Add(7);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                });
            }

            //
            // Get and assert the set.
            //
            {
                var set = s.Space.GetSet<int>("bar");

                Assert.AreEqual(4, set.Count);
                Assert.IsTrue(new[] { 2, 3, 5, 7 }.SequenceEqual(set.OrderBy(x => x)));
            }
        });

        [TestMethod]
        public void Set_Recovery_NoChange() => WithNewSpace(s =>
        {
            //
            // Create a set.
            //
            {
                var set = s.Space.CreateSet<int>("bar");

                set.Add(2);
                set.Add(3);
                set.Add(5);
                set.Add(7);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
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
            // Get and assert the set.
            //
            {
                var set = s.Space.GetSet<int>("bar");

                Assert.AreEqual(4, set.Count);
                Assert.IsTrue(new[] { 2, 3, 5, 7 }.SequenceEqual(set.OrderBy(x => x)));
            }
        });

        [TestMethod]
        public void Set_Recovery_Add() => WithNewSpace(s =>
        {
            //
            // Create a set.
            //
            {
                var set = s.Space.CreateSet<int>("bar");

                set.Add(2);
                set.Add(3);
                set.Add(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                });
            }

            //
            // Get the set and add an element.
            //
            {
                var set = s.Space.GetSet<int>("bar");

                set.Add(7);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                });
            }

            //
            // Get and assert the set.
            //
            {
                var set = s.Space.GetSet<int>("bar");

                Assert.AreEqual(4, set.Count);
                Assert.IsTrue(new[] { 2, 3, 5, 7 }.SequenceEqual(set.OrderBy(x => x)));
            }
        });

        [TestMethod]
        public void Set_Recovery_Add_Differential() => WithNewSpace(s =>
        {
            //
            // Create a set.
            //
            {
                var set = s.Space.CreateSet<int>("bar");

                set.Add(2);
                set.Add(3);
                set.Add(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                });
            }

            //
            // Get the set and add an element.
            //
            {
                var set = s.Space.GetSet<int>("bar");

                set.Add(7);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                });
            }

            //
            // Get and assert the set.
            //
            {
                var set = s.Space.GetSet<int>("bar");

                Assert.AreEqual(4, set.Count);
                Assert.IsTrue(new[] { 2, 3, 5, 7 }.SequenceEqual(set.OrderBy(x => x)));
            }
        });

        [TestMethod]
        public void Set_Recovery_Remove() => WithNewSpace(s =>
        {
            //
            // Create a set.
            //
            {
                var set = s.Space.CreateSet<int>("bar");

                set.Add(2);
                set.Add(3);
                set.Add(5);
                set.Add(7);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                });
            }

            //
            // Get the set and remove an element.
            //
            {
                var set = s.Space.GetSet<int>("bar");

                set.Remove(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                });
            }

            //
            // Get and assert the set.
            //
            {
                var set = s.Space.GetSet<int>("bar");

                Assert.AreEqual(3, set.Count);
                Assert.IsTrue(new[] { 2, 3, 7 }.SequenceEqual(set.OrderBy(x => x)));
            }
        });

        [TestMethod]
        public void Set_Recovery_Remove_Differential() => WithNewSpace(s =>
        {
            //
            // Create a set.
            //
            {
                var set = s.Space.CreateSet<int>("bar");

                set.Add(2);
                set.Add(3);
                set.Add(5);
                set.Add(7);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                });
            }

            //
            // Get the set and remove an element.
            //
            {
                var set = s.Space.GetSet<int>("bar");

                set.Remove(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "2"),
                });
            }

            //
            // Get and assert the set.
            //
            {
                var set = s.Space.GetSet<int>("bar");

                Assert.AreEqual(3, set.Count);
                Assert.IsTrue(new[] { 2, 3, 7 }.SequenceEqual(set.OrderBy(x => x)));
            }
        });

        [TestMethod]
        public void Set_Recovery_Remove1Add1() => WithNewSpace(s =>
        {
            //
            // Create a set.
            //
            {
                var set = s.Space.CreateSet<int>("bar");

                set.Add(2);
                set.Add(3);
                set.Add(5);
                set.Add(7);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                });
            }

            //
            // Get the set, remove and add an element.
            //
            {
                var set = s.Space.GetSet<int>("bar");

                set.Remove(5);
                set.Add(-1);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"), // NB: Slot will be reused.
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                });
            }

            //
            // Get and assert the set.
            //
            {
                var set = s.Space.GetSet<int>("bar");

                Assert.AreEqual(4, set.Count);
                Assert.IsTrue(new[] { -1, 2, 3, 7 }.SequenceEqual(set.OrderBy(x => x)));
            }
        });

        [TestMethod]
        public void Set_Recovery_Remove1Add1_Differential() => WithNewSpace(s =>
        {
            //
            // Create a set.
            //
            {
                var set = s.Space.CreateSet<int>("bar");

                set.Add(2);
                set.Add(3);
                set.Add(5);
                set.Add(7);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                });
            }

            //
            // Get the set, remove and add an element.
            //
            {
                var set = s.Space.GetSet<int>("bar");

                set.Remove(5);
                set.Add(-1);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"), // NB: Slot will be reused.
                });
            }

            //
            // Get and assert the set.
            //
            {
                var set = s.Space.GetSet<int>("bar");

                Assert.AreEqual(4, set.Count);
                Assert.IsTrue(new[] { -1, 2, 3, 7 }.SequenceEqual(set.OrderBy(x => x)));
            }
        });

        [TestMethod]
        public void Set_Recovery_Remove1Add1_Freelist() => WithNewSpace(s =>
        {
            //
            // Create a set.
            //
            {
                var set = s.Space.CreateSet<int>("bar");

                set.Add(2);
                set.Add(3);
                set.Add(5);
                set.Add(7);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                });
            }

            //
            // Get the set and remove an element.
            //
            {
                var set = s.Space.GetSet<int>("bar");

                set.Remove(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                });
            }

            //
            // Get the set and add an element.
            //
            {
                var set = s.Space.GetSet<int>("bar");

                set.Add(-1);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"), // NB: Slot gets reused from the free list.
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                });
            }

            //
            // Get and assert the set.
            //
            {
                var set = s.Space.GetSet<int>("bar");

                Assert.AreEqual(4, set.Count);
                Assert.IsTrue(new[] { -1, 2, 3, 7 }.SequenceEqual(set.OrderBy(x => x)));
            }
        });

        [TestMethod]
        public void Set_Recovery_Remove1Add1_Freelist_Differential() => WithNewSpace(s =>
        {
            //
            // Create a set.
            //
            {
                var set = s.Space.CreateSet<int>("bar");

                set.Add(2);
                set.Add(3);
                set.Add(5);
                set.Add(7);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                });
            }

            //
            // Get the set and remove an element.
            //
            {
                var set = s.Space.GetSet<int>("bar");

                set.Remove(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "2"),
                });
            }

            //
            // Get the set and add an element.
            //
            {
                var set = s.Space.GetSet<int>("bar");

                set.Add(-1);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"), // NB: Slot gets reused from the free list.
                });
            }

            //
            // Get and assert the set.
            //
            {
                var set = s.Space.GetSet<int>("bar");

                Assert.AreEqual(4, set.Count);
                Assert.IsTrue(new[] { -1, 2, 3, 7 }.SequenceEqual(set.OrderBy(x => x)));
            }
        });

        [TestMethod]
        public void Set_Recovery_Clear() => WithNewSpace(s =>
        {
            //
            // Create a set.
            //
            {
                var set = s.Space.CreateSet<int>("bar");

                set.Add(2);
                set.Add(3);
                set.Add(5);
                set.Add(7);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                });
            }

            //
            // Get the set and clear it.
            //
            {
                var set = s.Space.GetSet<int>("bar");

                set.Clear();
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
            // Get and assert the set.
            //
            {
                var set = s.Space.GetSet<int>("bar");

                Assert.AreEqual(0, set.Count);
            }
        });

        [TestMethod]
        public void Set_Recovery_Clear_Differential() => WithNewSpace(s =>
        {
            //
            // Create a set.
            //
            {
                var set = s.Space.CreateSet<int>("bar");

                set.Add(2);
                set.Add(3);
                set.Add(5);
                set.Add(7);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                });
            }

            //
            // Get the set and clear it.
            //
            {
                var set = s.Space.GetSet<int>("bar");

                set.Clear();
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "3"),
                });
            }

            //
            // Get and assert the set.
            //
            {
                var set = s.Space.GetSet<int>("bar");

                Assert.AreEqual(0, set.Count);
            }
        });

        [TestMethod]
        public void Set_Recovery_ClearAdd() => WithNewSpace(s =>
        {
            //
            // Create a set.
            //
            {
                var set = s.Space.CreateSet<int>("bar");

                set.Add(2);
                set.Add(3);
                set.Add(5);
                set.Add(7);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                });
            }

            //
            // Get the set, clear it, and add some elements.
            //
            {
                var set = s.Space.GetSet<int>("bar");

                set.Clear();
                set.Add(11);
                set.Add(13);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                });
            }

            //
            // Get and assert the set.
            //
            {
                var set = s.Space.GetSet<int>("bar");

                Assert.AreEqual(2, set.Count);
                Assert.IsTrue(new[] { 11, 13 }.SequenceEqual(set.OrderBy(x => x)));
            }
        });

        [TestMethod]
        public void Set_Recovery_ClearAdd_Differential() => WithNewSpace(s =>
        {
            //
            // Create a set.
            //
            {
                var set = s.Space.CreateSet<int>("bar");

                set.Add(2);
                set.Add(3);
                set.Add(5);
                set.Add(7);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                });
            }

            //
            // Get the set, clear it, and add some elements.
            //
            {
                var set = s.Space.GetSet<int>("bar");

                set.Clear();
                set.Add(11);
                set.Add(13);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"), // NB: These slots get reused.
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "3"),
                });
            }

            //
            // Get and assert the set.
            //
            {
                var set = s.Space.GetSet<int>("bar");

                Assert.AreEqual(2, set.Count);
                Assert.IsTrue(new[] { 11, 13 }.SequenceEqual(set.OrderBy(x => x)));
            }
        });

        [TestMethod]
        public void Set_Recovery_Delete() => WithNewSpace(s =>
        {
            //
            // Create a set.
            //
            {
                var set = s.Space.CreateSet<int>("bar");

                set.Add(2);
                set.Add(3);
                set.Add(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                });
            }

            //
            // Delete the set.
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
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "2"),
                });
            }

            //
            // Assert the set is gone.
            //

            Assert.ThrowsException<KeyNotFoundException>(() => s.Space.GetSet<int>("bar"));
        });

        [TestMethod]
        public void Set_Recovery_Delete_PendingRemove() => WithNewSpace(s =>
        {
            //
            // Create a set.
            //
            {
                var set = s.Space.CreateSet<int>("bar");

                set.Add(2);
                set.Add(3);
                set.Add(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                });
            }

            //
            // Get the set and remove an element.
            //
            {
                var set = s.Space.GetSet<int>("bar");

                set.Remove(3);
            }

            //
            // Delete the set.
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
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "2"),
                });
            }

            //
            // Assert the set is gone.
            //

            Assert.ThrowsException<KeyNotFoundException>(() => s.Space.GetSet<int>("bar"));
        });

        [TestMethod]
        public void Set_Recovery_Delete_PendingAdd() => WithNewSpace(s =>
        {
            //
            // Create a set.
            //
            {
                var set = s.Space.CreateSet<int>("bar");

                set.Add(2);
                set.Add(3);
                set.Add(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                });
            }

            //
            // Get the set and add an element.
            //
            {
                var set = s.Space.GetSet<int>("bar");

                set.Add(7);
            }

            //
            // Delete the set.
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
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "2"),
                });
            }

            //
            // Assert the set is gone.
            //

            Assert.ThrowsException<KeyNotFoundException>(() => s.Space.GetSet<int>("bar"));
        });
    }
}
