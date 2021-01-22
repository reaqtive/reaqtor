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
    public class SortedSetTests : PersistedTestBase
    {
        [TestMethod]
        public void SortedSet_CreateAndGet()
        {
            var assert = AssertOperation.WithAssert(MsTestAssert.Instance);
            var space = new PersistedObjectSpace(new SerializationFactory());

            CreateAndGetCore(assert).Accept(space);
        }

        [TestMethod]
        public void SortedSet_Volatile_CreateAndGet()
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
                    assert.ThrowsException<ArgumentNullException>().When(PersistedObjectSpaceOperation.CreateSortedSet<int>(null))
                ),

                //
                // Exceptions thrown by Get.
                //
                Operation.Sequence(
                    assert.ThrowsException<ArgumentNullException>().When(PersistedObjectSpaceOperation.GetSortedSet<int>(null)),
                    assert.ThrowsException<KeyNotFoundException>().When(PersistedObjectSpaceOperation.GetSortedSet<int>("bar"))
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
                PersistedObjectSpaceOperation.CreateSortedSet<int>("bar").Apply(set =>

                    //
                    // Check the new set is present in the object space.
                    //
                    Operation.Sequence(

                        //
                        // Assert we can get the same instance back using GetSortedSet.
                        //
                        assert.AreSame(
                            PersistedObjectSpaceOperation.GetSortedSet<int>("bar"),
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
                            assert.ThrowsException<InvalidOperationException>().When(PersistedObjectSpaceOperation.CreateSortedSet<int>("bar")),
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
                    assert.ThrowsException<KeyNotFoundException>().When(PersistedObjectSpaceOperation.GetSortedSet<int>("bar")),
                    assert.ThrowsException<KeyNotFoundException>().When(PersistedObjectSpaceOperation.Delete("bar"))
                )
            );

        [TestMethod]
        public void SortedSet_ManOrBoy()
        {
            var assert = AssertOperation.WithAssert(MsTestAssert.Instance);
            var space = new PersistedObjectSpace(new SerializationFactory());

            ManOrBoyCore(assert).Accept(space);
        }

        [TestMethod]
        public void SortedSet_Volatile_ManOrBoy()
        {
            var assert = AssertOperation.WithAssert(MsTestAssert.Instance);
            var space = new VolatilePersistedObjectSpace();

            ManOrBoyCore(assert).Accept(space);
        }

        //
        // TODO: Add more tests for Remove, Clear, *With, etc.
        //

        private static IOperation<IPersistedObjectSpace> ManOrBoyCore(IAssertOperationFactory assert) =>
            PersistedObjectSpaceOperation.CreateSortedSet<int>("bar").Bind(set =>
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
                    // Min and Max return a default value.
                    //
                    // NB: This behavior is compatible with SortedSet<T>. Arguably, an exception would be better.
                    //
                    Operation.Sequence(
                        assert.AreEqual(
                            set.Min(),
                            0
                        ),
                        assert.AreEqual(
                            set.Max(),
                            0
                        )
                    ),

                    //
                    // Reverse returns an empty sequence
                    //
                    assert.AreSequenceEqual(
                        set.Reverse(),
                        Array.Empty<int>()
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
                    // Min and Max are set
                    //
                    Operation.Sequence(
                        assert.AreEqual(
                            set.Min(),
                            42
                        ),
                        assert.AreEqual(
                            set.Max(),
                            42
                        )
                    ),

                    //
                    // Reverse returns the element
                    //
                    assert.AreSequenceEqual(
                        set.Reverse(),
                        new[] { 42 }
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
                            set.Enumerate(),
                            new[] { 42, 43 }
                        )
                    ),

                    //
                    // Min and Max are set
                    //
                    Operation.Sequence(
                        assert.AreEqual(
                            set.Min(),
                            42
                        ),
                        assert.AreEqual(
                            set.Max(),
                            43
                        )
                    ),

                    //
                    // Reverse returns the elements in the right order
                    //
                    assert.AreSequenceEqual(
                        set.Reverse(),
                        new[] { 43, 42 }
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
                    // Add another element
                    //
                    assert.IsTrue(set.Add(41)),

                    //
                    // Check value in set after addition
                    //
                    Operation.Sequence(
                        assert.AreEqual(
                            set.Count(),
                            3
                        ),
                        assert.AreSequenceEqual(
                            set.Enumerate(),
                            new[] { 41, 42, 43 }
                        )
                    ),

                    //
                    // Min and Max are set
                    //
                    Operation.Sequence(
                        assert.AreEqual(
                            set.Min(),
                            41
                        ),
                        assert.AreEqual(
                            set.Max(),
                            43
                        )
                    ),

                    //
                    // Reverse returns the elements in the right order
                    //
                    assert.AreSequenceEqual(
                        set.Reverse(),
                        new[] { 43, 42, 41 }
                    ),

                    //
                    // Check membership
                    //
                    Operation.Sequence(
                        assert.IsTrue(set.Contains(41)),
                        assert.IsTrue(set.Contains(42)),
                        assert.IsTrue(set.Contains(43))
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
                        assert.IsFalse(set.Contains(40)),
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
                            set.UnionWith(new[] { 1, 4, 3, 2, 1, 2 }),

                            //
                            // Assert sequence
                            //
                            Operation.Sequence(
                                assert.AreSequenceEqual(
                                    set.Enumerate(),
                                    new[] { 1, 2, 3, 4 }
                                ),
                                assert.AreSequenceEqual(
                                    set.Reverse(),
                                    new[] { 4, 3, 2, 1 }
                                )
                            ),

                            //
                            // Assert Min and Max
                            //
                            Operation.Sequence(
                                assert.AreEqual(
                                    set.Min(),
                                    1
                                ),
                                assert.AreEqual(
                                    set.Max(),
                                    4
                                )
                            ),

                            //
                            // Using SortedSet<T>
                            //
                            set.UnionWith(new SortedSet<int> { 6, 3, 2, 5 }),

                            //
                            // Assert sequence
                            //
                            Operation.Sequence(
                                assert.AreSequenceEqual(
                                    set.Enumerate(),
                                    new[] { 1, 2, 3, 4, 5, 6 }
                                ),
                                assert.AreSequenceEqual(
                                    set.Reverse(),
                                    new[] { 6, 5, 4, 3, 2, 1 }
                                )
                            ),

                            //
                            // Assert Min and Max
                            //
                            Operation.Sequence(
                                assert.AreEqual(
                                    set.Min(),
                                    1
                                ),
                                assert.AreEqual(
                                    set.Max(),
                                    6
                                )
                            ),

                            //
                            // UnionWith(this) is no-op
                            //
                            set.This().Apply(it => set.UnionWith(it)),

                            //
                            // Assert sequence
                            //
                            Operation.Sequence(
                                assert.AreSequenceEqual(
                                    set.Enumerate(),
                                    new[] { 1, 2, 3, 4, 5, 6 }
                                ),
                                assert.AreSequenceEqual(
                                    set.Reverse(),
                                    new[] { 6, 5, 4, 3, 2, 1 }
                                )
                            ),

                            //
                            // Assert Min and Max
                            //
                            Operation.Sequence(
                                assert.AreEqual(
                                    set.Min(),
                                    1
                                ),
                                assert.AreEqual(
                                    set.Max(),
                                    6
                                )
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
                            set.ExceptWith(new[] { 2, 7, 3, 2 }),

                            //
                            // Assert sequence
                            //
                            Operation.Sequence(
                                assert.AreSequenceEqual(
                                    set.Enumerate(),
                                    new[] { 1, 4, 5, 6 }
                                ),
                                assert.AreSequenceEqual(
                                    set.Reverse(),
                                    new[] { 6, 5, 4, 1 }
                                )
                            ),

                            //
                            // Assert Min and Max
                            //
                            Operation.Sequence(
                                assert.AreEqual(
                                    set.Min(),
                                    1
                                ),
                                assert.AreEqual(
                                    set.Max(),
                                    6
                                )
                            ),

                            //
                            // Using SortedSet<T>
                            //
                            set.ExceptWith(new SortedSet<int> { 4, 0, 3, 6, 2 }),

                            //
                            // Assert sequence
                            //
                            Operation.Sequence(
                                assert.AreSequenceEqual(
                                    set.Enumerate(),
                                    new[] { 1, 5 }
                                ),
                                assert.AreSequenceEqual(
                                    set.Reverse(),
                                    new[] { 5, 1 }
                                )
                            ),

                            //
                            // Assert Min and Max
                            //
                            Operation.Sequence(
                                assert.AreEqual(
                                    set.Min(),
                                    1
                                ),
                                assert.AreEqual(
                                    set.Max(),
                                    5
                                )
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
                            Operation.Sequence(
                                assert.AreSequenceEqual(
                                    set.Enumerate(),
                                    new[] { 1, 2, 3, 4 }
                                ),
                                assert.AreSequenceEqual(
                                    set.Reverse(),
                                    new[] { 4, 3, 2, 1 }
                                )
                            ),

                            //
                            // Assert Min and Max
                            //
                            Operation.Sequence(
                                assert.AreEqual(
                                    set.Min(),
                                    1
                                ),
                                assert.AreEqual(
                                    set.Max(),
                                    4
                                )
                            ),

                            //
                            // Using SortedSet<T>
                            //
                            set.IntersectWith(new SortedSet<int> { 2, 3 }),

                            //
                            // Assert sequence
                            //
                            Operation.Sequence(
                                assert.AreSequenceEqual(
                                    set.Enumerate(),
                                    new[] { 2, 3 }
                                ),
                                assert.AreSequenceEqual(
                                    set.Reverse(),
                                    new[] { 3, 2 }
                                )
                            ),

                            //
                            // Assert Min and Max
                            //
                            Operation.Sequence(
                                assert.AreEqual(
                                    set.Min(),
                                    2
                                ),
                                assert.AreEqual(
                                    set.Max(),
                                    3
                                )
                            ),

                            //
                            // IntersectWith(this) is no-op
                            //
                            set.This().Apply(it => set.IntersectWith(it)),

                            //
                            // Assert sequence
                            //
                            Operation.Sequence(
                                assert.AreSequenceEqual(
                                    set.Enumerate(),
                                    new[] { 2, 3 }
                                ),
                                assert.AreSequenceEqual(
                                    set.Reverse(),
                                    new[] { 3, 2 }
                                )
                            ),

                            //
                            // Assert Min and Max
                            //
                            Operation.Sequence(
                                assert.AreEqual(
                                    set.Min(),
                                    2
                                ),
                                assert.AreEqual(
                                    set.Max(),
                                    3
                                )
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
                            Operation.Sequence(
                                assert.AreSequenceEqual(
                                    set.Enumerate(),
                                    new[] { 0, 1, 3, 4, 6, 8 }
                                ),
                                assert.AreSequenceEqual(
                                    set.Reverse(),
                                    new[] { 8, 6, 4, 3, 1, 0 }
                                )
                            ),

                            //
                            // Assert Min and Max
                            //
                            Operation.Sequence(
                                assert.AreEqual(
                                    set.Min(),
                                    0
                                ),
                                assert.AreEqual(
                                    set.Max(),
                                    8
                                )
                            ),

                            //
                            // Using an empty array
                            //
                            set.SymmetricExceptWith(Array.Empty<int>()),

                            //
                            // Assert sequence
                            //
                            Operation.Sequence(
                                assert.AreSequenceEqual(
                                    set.Enumerate(),
                                    new[] { 0, 1, 3, 4, 6, 8 }
                                ),
                                assert.AreSequenceEqual(
                                    set.Reverse(),
                                    new[] { 8, 6, 4, 3, 1, 0 }
                                )
                            ),

                            //
                            // Assert Min and Max
                            //
                            Operation.Sequence(
                                assert.AreEqual(
                                    set.Min(),
                                    0
                                ),
                                assert.AreEqual(
                                    set.Max(),
                                    8
                                )
                            ),

                            //
                            // Using SortedSet<T>
                            //
                            set.SymmetricExceptWith(new SortedSet<int> { 8, 5, 0, 2 }),

                            //
                            // Assert sequence
                            //
                            Operation.Sequence(
                                assert.AreSequenceEqual(
                                    set.Enumerate(),
                                    new[] { 1, 2, 3, 4, 5, 6 }
                                ),
                                assert.AreSequenceEqual(
                                    set.Reverse(),
                                    new[] { 6, 5, 4, 3, 2, 1 }
                                )
                            ),

                            //
                            // Assert Min and Max
                            //
                            Operation.Sequence(
                                assert.AreEqual(
                                    set.Min(),
                                    1
                                ),
                                assert.AreEqual(
                                    set.Max(),
                                    6
                                )
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
                        ),

                        //
                        // Clear and restore
                        //
                        Operation.Sequence(
                            set.Clear(),
                            set.UnionWith(new[] { 1, 2, 3, 5, 6 })
                        ),

                        //
                        // GetViewBetween
                        //
                        Operation.Sequence(
                            set.GetViewBetween(2, 5).Bind(subset =>
                                Operation.Sequence(

                                    //
                                    // Assert Min and Max
                                    //
                                    Operation.Sequence(
                                        assert.AreEqual(
                                            subset.Min(),
                                            2
                                        ),
                                        assert.AreEqual(
                                            subset.Max(),
                                            5
                                        )
                                    ),

                                    //
                                    // Contains
                                    //
                                    Operation.Sequence(
                                        assert.IsFalse(subset.Contains(1)),
                                        assert.IsTrue(subset.Contains(2)),
                                        assert.IsTrue(subset.Contains(3)),
                                        assert.IsFalse(subset.Contains(4)),
                                        assert.IsTrue(subset.Contains(5)),
                                        assert.IsFalse(subset.Contains(6))
                                    ),

                                    //
                                    // Enumerate
                                    //
                                    assert.AreSequenceEqual(
                                        subset.Enumerate(),
                                        new[] { 2, 3, 5 }
                                    ),

                                    //
                                    // Reverse
                                    //
                                    assert.AreSequenceEqual(
                                        subset.Reverse(),
                                        new[] { 5, 3, 2 }
                                    ),

                                    //
                                    // Add
                                    //
                                    Operation.Sequence(

                                        //
                                        // Add fails when out of bounds
                                        //
                                        Operation.Sequence(
                                            assert.ThrowsException<ArgumentOutOfRangeException>().When(subset.Add(0)),
                                            assert.ThrowsException<ArgumentOutOfRangeException>().When(subset.Add(7))
                                        ),

                                        //
                                        // Can add within bounds
                                        //
                                        subset.Add(4),

                                        //
                                        // Assert view
                                        //
                                        assert.AreSequenceEqual(
                                            subset.Enumerate(),
                                            new[] { 2, 3, 4, 5 }
                                        )

#if FALSE // TODO: Debug this case.
                                        //
                                        // Assert set
                                        //
                                        assert.AreSequenceEqual(
                                            set.Enumerate(),
                                            new[] { 1, 2, 3, 4, 5, 6 }
                                        )
#endif
                                    )
                                )
                            )
                        )
                    )
                )
            );

        [TestMethod]
        public void SortedSet_Recovery_CreateDefault() => WithNewSpace(s =>
        {
            //
            // Create a sorted set.
            //

            _ = s.Space.CreateSortedSet<int>("bar");

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
            // Assert the new space does not contain the sorted set.
            //

            Assert.ThrowsException<KeyNotFoundException>(() => s.Space.GetSortedSet<int>("bar"));

            //
            // Load the space from the store.
            //

            s.LoadSpace();

            //
            // Get the sorted set.
            //

            var bar = s.Space.GetSortedSet<int>("bar");

            //
            // Assert the sorted set size.
            //

            Assert.AreEqual(0, bar.Count);
        });

        [TestMethod]
        public void SortedSet_Recovery_CreateSortedSet() => WithNewSpace(s =>
        {
            //
            // Create a sorted set.
            //
            {
                var set = s.Space.CreateSortedSet<int>("bar");

                set.Add(2);
                set.Add(5);
                set.Add(7);
                set.Add(3);
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
            // Get and assert the sorted set.
            //
            {
                var set = s.Space.GetSortedSet<int>("bar");

                Assert.AreEqual(4, set.Count);
                Assert.IsTrue(new[] { 2, 3, 5, 7 }.SequenceEqual(set));
            }
        });

        [TestMethod]
        public void SortedSet_Recovery_NoChange() => WithNewSpace(s =>
        {
            //
            // Create a sorted set.
            //
            {
                var set = s.Space.CreateSortedSet<int>("bar");

                set.Add(2);
                set.Add(5);
                set.Add(7);
                set.Add(3);
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
            // Get and assert the sorted set.
            //
            {
                var set = s.Space.GetSortedSet<int>("bar");

                Assert.AreEqual(4, set.Count);
                Assert.IsTrue(new[] { 2, 3, 5, 7 }.SequenceEqual(set));
            }
        });

        [TestMethod]
        public void SortedSet_Recovery_Add() => WithNewSpace(s =>
        {
            //
            // Create a sorted set.
            //
            {
                var set = s.Space.CreateSortedSet<int>("bar");

                set.Add(2);
                set.Add(7);
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
            // Get the sorted set and add an element.
            //
            {
                var set = s.Space.GetSortedSet<int>("bar");

                set.Add(3);
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
            // Get and assert the sorted set.
            //
            {
                var set = s.Space.GetSortedSet<int>("bar");

                Assert.AreEqual(4, set.Count);
                Assert.IsTrue(new[] { 2, 3, 5, 7 }.SequenceEqual(set));
            }
        });

        [TestMethod]
        public void SortedSet_Recovery_Add_Differential() => WithNewSpace(s =>
        {
            //
            // Create a sorted set.
            //
            {
                var set = s.Space.CreateSortedSet<int>("bar");

                set.Add(2);
                set.Add(7);
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
            // Get the sorted set and add an element.
            //
            {
                var set = s.Space.GetSortedSet<int>("bar");

                set.Add(3);
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
            // Get and assert the sorted set.
            //
            {
                var set = s.Space.GetSortedSet<int>("bar");

                Assert.AreEqual(4, set.Count);
                Assert.IsTrue(new[] { 2, 3, 5, 7 }.SequenceEqual(set));
            }
        });

        [TestMethod]
        public void SortedSet_Recovery_Remove() => WithNewSpace(s =>
        {
            //
            // Create a sorted set.
            //
            {
                var set = s.Space.CreateSortedSet<int>("bar");

                set.Add(3);
                set.Add(7);
                set.Add(5);
                set.Add(2);
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
            // Get the sorted set and remove an element.
            //
            {
                var set = s.Space.GetSortedSet<int>("bar");

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
            // Get and assert the sorted set.
            //
            {
                var set = s.Space.GetSortedSet<int>("bar");

                Assert.AreEqual(3, set.Count);
                Assert.IsTrue(new[] { 2, 3, 7 }.SequenceEqual(set));
            }
        });

        [TestMethod]
        public void SortedSet_Recovery_Remove_Differential() => WithNewSpace(s =>
        {
            //
            // Create a sorted set.
            //
            {
                var set = s.Space.CreateSortedSet<int>("bar");

                set.Add(3);
                set.Add(7);
                set.Add(5);
                set.Add(2);
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
            // Get the sorted set and remove an element.
            //
            {
                var set = s.Space.GetSortedSet<int>("bar");

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
            // Get and assert the sorted set.
            //
            {
                var set = s.Space.GetSortedSet<int>("bar");

                Assert.AreEqual(3, set.Count);
                Assert.IsTrue(new[] { 2, 3, 7 }.SequenceEqual(set));
            }
        });

        [TestMethod]
        public void SortedSet_Recovery_Remove1Add1() => WithNewSpace(s =>
        {
            //
            // Create a sorted set.
            //
            {
                var set = s.Space.CreateSortedSet<int>("bar");

                set.Add(3);
                set.Add(7);
                set.Add(5);
                set.Add(2);
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
            // Get the sorted set, remove and add an element.
            //
            {
                var set = s.Space.GetSortedSet<int>("bar");

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
            // Get and assert the sorted set.
            //
            {
                var set = s.Space.GetSortedSet<int>("bar");

                Assert.AreEqual(4, set.Count);
                Assert.IsTrue(new[] { -1, 2, 3, 7 }.SequenceEqual(set));
            }
        });

        [TestMethod]
        public void SortedSet_Recovery_Remove1Add1_Differential() => WithNewSpace(s =>
        {
            //
            // Create a sorted set.
            //
            {
                var set = s.Space.CreateSortedSet<int>("bar");

                set.Add(3);
                set.Add(7);
                set.Add(5);
                set.Add(2);
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
            // Get the sorted set, remove and add an element.
            //
            {
                var set = s.Space.GetSortedSet<int>("bar");

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
            // Get and assert the sorted set.
            //
            {
                var set = s.Space.GetSortedSet<int>("bar");

                Assert.AreEqual(4, set.Count);
                Assert.IsTrue(new[] { -1, 2, 3, 7 }.SequenceEqual(set));
            }
        });

        [TestMethod]
        public void SortedSet_Recovery_Remove1Add1_Freelist() => WithNewSpace(s =>
        {
            //
            // Create a sorted set.
            //
            {
                var set = s.Space.CreateSortedSet<int>("bar");

                set.Add(3);
                set.Add(7);
                set.Add(5);
                set.Add(2);
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
            // Get the sorted set and remove an element.
            //
            {
                var set = s.Space.GetSortedSet<int>("bar");

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
            // Get the sorted set and add an element.
            //
            {
                var set = s.Space.GetSortedSet<int>("bar");

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
            // Get and assert the sorted set.
            //
            {
                var set = s.Space.GetSortedSet<int>("bar");

                Assert.AreEqual(4, set.Count);
                Assert.IsTrue(new[] { -1, 2, 3, 7 }.SequenceEqual(set));
            }
        });

        [TestMethod]
        public void SortedSet_Recovery_Remove1Add1_Freelist_Differential() => WithNewSpace(s =>
        {
            //
            // Create a sorted set.
            //
            {
                var set = s.Space.CreateSortedSet<int>("bar");

                set.Add(3);
                set.Add(7);
                set.Add(5);
                set.Add(2);
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
            // Get the sorted set and remove an element.
            //
            {
                var set = s.Space.GetSortedSet<int>("bar");

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
            // Get the sorted set and add an element.
            //
            {
                var set = s.Space.GetSortedSet<int>("bar");

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
            // Get and assert the sorted set.
            //
            {
                var set = s.Space.GetSortedSet<int>("bar");

                Assert.AreEqual(4, set.Count);
                Assert.IsTrue(new[] { -1, 2, 3, 7 }.SequenceEqual(set));
            }
        });

        [TestMethod]
        public void SortedSet_Recovery_Clear() => WithNewSpace(s =>
        {
            //
            // Create a sorted set.
            //
            {
                var set = s.Space.CreateSortedSet<int>("bar");

                set.Add(3);
                set.Add(7);
                set.Add(5);
                set.Add(2);
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
            // Get the sorted set and clear it.
            //
            {
                var set = s.Space.GetSortedSet<int>("bar");

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
            // Get and assert the sorted set.
            //
            {
                var set = s.Space.GetSortedSet<int>("bar");

                Assert.AreEqual(0, set.Count);
            }
        });

        [TestMethod]
        public void SortedSet_Recovery_Clear_Differential() => WithNewSpace(s =>
        {
            //
            // Create a sorted set.
            //
            {
                var set = s.Space.CreateSortedSet<int>("bar");

                set.Add(3);
                set.Add(7);
                set.Add(5);
                set.Add(2);
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
            // Get the sorted set and clear it.
            //
            {
                var set = s.Space.GetSortedSet<int>("bar");

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
            // Get and assert the sorted set.
            //
            {
                var set = s.Space.GetSortedSet<int>("bar");

                Assert.AreEqual(0, set.Count);
            }
        });

        [TestMethod]
        public void SortedSet_Recovery_ClearAdd() => WithNewSpace(s =>
        {
            //
            // Create a sorted set.
            //
            {
                var set = s.Space.CreateSortedSet<int>("bar");

                set.Add(3);
                set.Add(7);
                set.Add(5);
                set.Add(2);
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
            // Get the sorted set, clear it, and add some elements.
            //
            {
                var set = s.Space.GetSortedSet<int>("bar");

                set.Clear();
                set.Add(13);
                set.Add(11);
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
            // Get and assert the sorted set.
            //
            {
                var set = s.Space.GetSortedSet<int>("bar");

                Assert.AreEqual(2, set.Count);
                Assert.IsTrue(new[] { 11, 13 }.SequenceEqual(set));
            }
        });

        [TestMethod]
        public void SortedSet_Recovery_ClearAdd_Differential() => WithNewSpace(s =>
        {
            //
            // Create a sorted set.
            //
            {
                var set = s.Space.CreateSortedSet<int>("bar");

                set.Add(3);
                set.Add(7);
                set.Add(5);
                set.Add(2);
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
            // Get the sorted set, clear it, and add some elements.
            //
            {
                var set = s.Space.GetSortedSet<int>("bar");

                set.Clear();
                set.Add(13);
                set.Add(11);
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
            // Get and assert the sorted set.
            //
            {
                var set = s.Space.GetSortedSet<int>("bar");

                Assert.AreEqual(2, set.Count);
                Assert.IsTrue(new[] { 11, 13 }.SequenceEqual(set));
            }
        });

        [TestMethod]
        public void SortedSet_Recovery_Delete() => WithNewSpace(s =>
        {
            //
            // Create a sorted set.
            //
            {
                var set = s.Space.CreateSortedSet<int>("bar");

                set.Add(3);
                set.Add(2);
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
            // Delete the sorted set.
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
            // Assert the sorted set is gone.
            //

            Assert.ThrowsException<KeyNotFoundException>(() => s.Space.GetSortedSet<int>("bar"));
        });

        [TestMethod]
        public void SortedSet_Recovery_Delete_PendingRemove() => WithNewSpace(s =>
        {
            //
            // Create a sorted set.
            //
            {
                var set = s.Space.CreateSortedSet<int>("bar");

                set.Add(3);
                set.Add(2);
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
            // Get the sorted set and remove an element.
            //
            {
                var set = s.Space.GetSortedSet<int>("bar");

                set.Remove(3);
            }

            //
            // Delete the sorted set.
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
            // Assert the sorted set is gone.
            //

            Assert.ThrowsException<KeyNotFoundException>(() => s.Space.GetSortedSet<int>("bar"));
        });

        [TestMethod]
        public void SortedSet_Recovery_Delete_PendingAdd() => WithNewSpace(s =>
        {
            //
            // Create a sorted set.
            //
            {
                var set = s.Space.CreateSortedSet<int>("bar");

                set.Add(3);
                set.Add(2);
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
            // Get the sorted set and add an element.
            //
            {
                var set = s.Space.GetSortedSet<int>("bar");

                set.Add(7);
            }

            //
            // Delete the sorted set.
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
            // Assert the sorted set is gone.
            //

            Assert.ThrowsException<KeyNotFoundException>(() => s.Space.GetSortedSet<int>("bar"));
        });

        // TODO: Add tests for Min, Max, and GetViewBetween.
    }
}
