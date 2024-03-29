﻿// Licensed to the .NET Foundation under one or more agreements.
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
    public class DictionaryTests : PersistedTestBase
    {
        [TestMethod]
        public void Dictionary_CreateAndGet()
        {
            var assert = AssertOperation.WithAssert(MsTestAssert.Instance);
            var space = new PersistedObjectSpace(new SerializationFactory());

            CreateAndGetCore(assert).Accept(space);
        }

        [TestMethod]
        public void Dictionary_Volatile_CreateAndGet()
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
                    assert.ThrowsException<ArgumentNullException>().When(PersistedObjectSpaceOperation.CreateDictionary<int, int>(null))
                ),

                //
                // Exceptions thrown by Get.
                //
                Operation.Sequence(
                    assert.ThrowsException<ArgumentNullException>().When(PersistedObjectSpaceOperation.GetDictionary<int, int>(null)),
                    assert.ThrowsException<KeyNotFoundException>().When(PersistedObjectSpaceOperation.GetDictionary<int, int>("bar"))
                ),

                //
                // Exceptions thrown by Delete.
                //
                Operation.Sequence(
                    assert.ThrowsException<KeyNotFoundException>().When(PersistedObjectSpaceOperation.Delete("bar"))
                ),

                //
                // Create a new dictionary.
                //
                PersistedObjectSpaceOperation.CreateDictionary<int, int>("bar").Apply(set =>

                    //
                    // Check the new dictionary is present in the object space.
                    //
                    Operation.Sequence(

                        //
                        // Assert we can get the same instance back using GetDictionary.
                        //
                        assert.AreSame(
                            PersistedObjectSpaceOperation.GetDictionary<int, int>("bar"),
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
                            assert.ThrowsException<InvalidOperationException>().When(PersistedObjectSpaceOperation.CreateStack<int>("bar")),
                            assert.ThrowsException<InvalidOperationException>().When(PersistedObjectSpaceOperation.CreateValue<int>("bar"))
                        )
                    )
                ),

                //
                // Delete dictionary.
                //
                PersistedObjectSpaceOperation.Delete("bar"),

                //
                // Check the dictionary is no longer present in the object space.
                //
                Operation.Sequence(
                    assert.ThrowsException<KeyNotFoundException>().When(PersistedObjectSpaceOperation.GetDictionary<int, int>("bar")),
                    assert.ThrowsException<KeyNotFoundException>().When(PersistedObjectSpaceOperation.Delete("bar"))
                )
            );

        [TestMethod]
        public void Dictionary_ManOrBoy()
        {
            var assert = AssertOperation.WithAssert(MsTestAssert.Instance);
            var space = new PersistedObjectSpace(new SerializationFactory());

            ManOrBoyCore(assert).Accept(space);
        }

        [TestMethod]
        public void Dictionary_Volatile_ManOrBoy()
        {
            var assert = AssertOperation.WithAssert(MsTestAssert.Instance);
            var space = new VolatilePersistedObjectSpace();

            ManOrBoyCore(assert).Accept(space);
        }

        private static IOperation<IPersistedObjectSpace> ManOrBoyCore(IAssertOperationFactory assert) =>
            PersistedObjectSpaceOperation.CreateDictionary<int, int>("bar").Bind(dictionary =>
                Operation.Sequence(

                    //
                    // Check the identifier
                    //
                    assert.AreEqual(
                        dictionary.GetId(),
                        "bar"
                    ),

                    //
                    // Initially empty
                    //
                    Operation.Sequence(
                        assert.AreEqual(
                            dictionary.Count(),
                            0
                        ),
                        assert.AreSequenceEqual(
                            dictionary.Enumerate(),
                            Array.Empty<KeyValuePair<int, int>>()
                        )
                    ),

                    //
                    // Created as mutable
                    //
                    assert.IsFalse(dictionary.IsReadOnly()),

                    //
                    // Add an element
                    //
                    dictionary.Add(42, 7),

                    //
                    // Check value in dictionary after addition
                    //
                    Operation.Sequence(
                        assert.AreEqual(
                            dictionary.Count(),
                            1
                        ),
                        assert.AreSequenceEqual(
                            dictionary.Enumerate(),
                            new[] { new KeyValuePair<int, int>(42, 7) }
                        ),
                        assert.AreSequenceEqual(
                            dictionary.GetKeys(),
                            new[] { 42 }
                        ),
                        assert.AreSequenceEqual(
                            dictionary.GetValues(),
                            new[] { 7 }
                        ),
                        assert.AreEqual(
                            dictionary.Get(42),
                            7
                        ),
                        assert.AreEqual(
                            dictionary.TryGetValue(42).Select(t => t.Item1 + ", " + t.Item2), // NB: Working around Roslyn bug https://github.com/dotnet/roslyn/issues/24517
                            "True, 7"
                        )
                    ),

                    //
                    // Check membership
                    //
                    Operation.Sequence(
                        assert.IsTrue(dictionary.ContainsKey(42)),
                        assert.IsFalse(dictionary.ContainsKey(43)),
                        assert.IsTrue(dictionary.Contains(new KeyValuePair<int, int>(42, 7))),
                        assert.IsFalse(dictionary.Contains(new KeyValuePair<int, int>(42, 8))),
                        assert.IsFalse(dictionary.Contains(new KeyValuePair<int, int>(43, 7))),
                        assert.IsFalse(dictionary.TryGetValue(43).Select(t => t.Item1))
                    ),

                    //
                    // Can't add an element twice
                    //
                    assert.ThrowsException<ArgumentException>().When(dictionary.Add(42, -1)),

                    //
                    // Add another element
                    //
                    dictionary.Add(43, 8),

                    //
                    // Check value in dictionary after addition
                    //
                    Operation.Sequence(
                        assert.AreEqual(
                            dictionary.Count(),
                            2
                        ),
                        assert.AreSequenceEqual(
                            dictionary.Enumerate().Select(xs => xs.OrderBy(x => x.Key)),
                            new[] { new KeyValuePair<int, int>(42, 7), new KeyValuePair<int, int>(43, 8) }
                        ),
                        assert.AreSequenceEqual(
                            dictionary.GetKeys().Select(xs => xs.OrderBy(x => x)),
                            new[] { 42, 43 }
                        ),
                        assert.AreSequenceEqual(
                            dictionary.GetValues().Select(xs => xs.OrderBy(x => x)),
                            new[] { 7, 8 }
                        ),
                        assert.AreEqual(
                            dictionary.Get(42),
                            7
                        ),
                        assert.AreEqual(
                            dictionary.Get(43),
                            8
                        ),
                        assert.AreEqual(
                            dictionary.TryGetValue(42).Select(t => t.Item1 + ", " + t.Item2),
                            "True, 7"
                        ),
                        assert.AreEqual(
                            dictionary.TryGetValue(43).Select(t => t.Item1 + ", " + t.Item2),
                            "True, 8"
                        )
                    ),

                    //
                    // Check membership
                    //
                    Operation.Sequence(
                        assert.IsTrue(dictionary.ContainsKey(42)),
                        assert.IsTrue(dictionary.ContainsKey(43)),
                        assert.IsTrue(dictionary.Contains(new KeyValuePair<int, int>(42, 7))),
                        assert.IsTrue(dictionary.Contains(new KeyValuePair<int, int>(43, 8)))
                    ),

                    //
                    // Edit an element
                    //
                    dictionary.Set(42, 9),

                    //
                    // Check value in dictionary after edit
                    //
                    Operation.Sequence(
                        assert.AreEqual(
                            dictionary.Count(),
                            2
                        ),
                        assert.AreSequenceEqual(
                            dictionary.Enumerate().Select(xs => xs.OrderBy(x => x.Key)),
                            new[] { new KeyValuePair<int, int>(42, 9), new KeyValuePair<int, int>(43, 8) }
                        ),
                        assert.AreSequenceEqual(
                            dictionary.GetKeys().Select(xs => xs.OrderBy(x => x)),
                            new[] { 42, 43 }
                        ),
                        assert.AreSequenceEqual(
                            dictionary.GetValues().Select(xs => xs.OrderBy(x => x)),
                            new[] { 8, 9 }
                        ),
                        assert.AreEqual(
                            dictionary.Get(42),
                            9
                        ),
                        assert.AreEqual(
                            dictionary.Get(43),
                            8
                        ),
                        assert.AreEqual(
                            dictionary.TryGetValue(42).Select(t => t.Item1 + ", " + t.Item2),
                            "True, 9"
                        ),
                        assert.AreEqual(
                            dictionary.TryGetValue(43).Select(t => t.Item1 + ", " + t.Item2),
                            "True, 8"
                        )
                    ),

                    //
                    // Check membership
                    //
                    Operation.Sequence(
                        assert.IsTrue(dictionary.ContainsKey(42)),
                        assert.IsTrue(dictionary.ContainsKey(43)),
                        assert.IsTrue(dictionary.Contains(new KeyValuePair<int, int>(42, 9))),
                        assert.IsTrue(dictionary.Contains(new KeyValuePair<int, int>(43, 8)))
                    ),

                    //
                    // Clear the dictionary
                    //
                    dictionary.Clear(),

                    //
                    // Empty again
                    //
                    Operation.Sequence(
                        assert.AreEqual(
                            dictionary.Count(),
                            0
                        ),
                        assert.AreSequenceEqual(
                            dictionary.Enumerate(),
                            Array.Empty<KeyValuePair<int, int>>()
                        )
                    ),

                    //
                    // Can't find anything
                    //
                    Operation.Sequence(
                        assert.IsFalse(dictionary.ContainsKey(41)),
                        assert.IsFalse(dictionary.ContainsKey(42)),
                        assert.IsFalse(dictionary.ContainsKey(43)),
                        assert.IsFalse(dictionary.ContainsKey(44)),
                        assert.IsFalse(dictionary.Contains(new KeyValuePair<int, int>(42, 7))),
                        assert.IsFalse(dictionary.Contains(new KeyValuePair<int, int>(43, 8))),
                        assert.IsFalse(dictionary.TryGetValue(41).Select(t => t.Item1)),
                        assert.IsFalse(dictionary.TryGetValue(42).Select(t => t.Item1)),
                        assert.IsFalse(dictionary.TryGetValue(43).Select(t => t.Item1)),
                        assert.IsFalse(dictionary.TryGetValue(44).Select(t => t.Item1))
                    ),

                    //
                    // Add an element
                    //
                    dictionary.Add(1, 2),

                    //
                    // Check membership
                    //
                    Operation.Sequence(
                        assert.IsTrue(dictionary.ContainsKey(1)),
                        assert.IsTrue(dictionary.Contains(new KeyValuePair<int, int>(1, 2)))
                    ),

                    //
                    // Can remove the element
                    //
                    assert.IsTrue(dictionary.Remove(1)),

                    //
                    // Empty again
                    //
                    Operation.Sequence(
                        assert.AreEqual(
                            dictionary.Count(),
                            0
                        ),
                        assert.AreSequenceEqual(
                            dictionary.Enumerate(),
                            Array.Empty<KeyValuePair<int, int>>()
                        )
                    ),

                    //
                    // Can't remove twice
                    //
                    assert.IsFalse(dictionary.Remove(1)),

                    //
                    // Add an element through indexer
                    //
                    dictionary.Set(99, 42),

                    //
                    // Check membership
                    //
                    Operation.Sequence(
                        assert.IsTrue(dictionary.ContainsKey(99)),
                        assert.IsTrue(dictionary.Contains(new KeyValuePair<int, int>(99, 42)))
                    ),

                    //
                    // Attempt to remove using invalid key/value pairs
                    //
                    assert.IsFalse(dictionary.Remove(new KeyValuePair<int, int>(-99, 42))),
                    assert.IsFalse(dictionary.Remove(new KeyValuePair<int, int>(99, -42))),

                    //
                    // Can remove the element
                    //
                    assert.IsTrue(dictionary.Remove(new KeyValuePair<int, int>(99, 42))),

                    //
                    // Empty again
                    //
                    Operation.Sequence(
                        assert.AreEqual(
                            dictionary.Count(),
                            0
                        ),
                        assert.AreSequenceEqual(
                            dictionary.Enumerate(),
                            Array.Empty<KeyValuePair<int, int>>()
                        )
                    )
                )
            );

        [TestMethod]
        public void Dictionary_Recovery_CreateDefault() => WithNewSpace(s =>
        {
            //
            // Create a dictionary.
            //

            _ = s.Space.CreateDictionary<int, int>("bar");

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
            // Assert the new space does not contain the dictionary.
            //

            Assert.ThrowsException<KeyNotFoundException>(() => s.Space.GetDictionary<int, int>("bar"));

            //
            // Load the space from the store.
            //

            s.LoadSpace();

            //
            // Get the dictionary.
            //

            var bar = s.Space.GetDictionary<int, int>("bar");

            //
            // Assert the dictionary size.
            //

            Assert.AreEqual(0, bar.Count);
        });

        [TestMethod]
        public void Dictionary_Recovery_CreateDictionary() => WithNewSpace(s =>
        {
            //
            // Create a dictionary.
            //
            {
                var dictionary = s.Space.CreateDictionary<int, int>("bar");

                dictionary.Add(2, 0);
                dictionary.Add(3, 1);
                dictionary.Add(5, 2);
                dictionary.Add(7, 3);
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
            // Get and assert the dictionary.
            //
            {
                var dictionary = s.Space.GetDictionary<int, int>("bar");

                Assert.AreEqual(4, dictionary.Count);
                Assert.IsTrue(new[] { "2-0", "3-1", "5-2", "7-3" }.SequenceEqual(dictionary.OrderBy(x => x.Key).Select(kv => kv.Key + "-" + kv.Value)));
            }
        });

        [TestMethod]
        public void Dictionary_Recovery_NoChange() => WithNewSpace(s =>
        {
            //
            // Create a dictionary.
            //
            {
                var dictionary = s.Space.CreateDictionary<int, int>("bar");

                dictionary.Add(2, 0);
                dictionary.Add(3, 1);
                dictionary.Add(5, 2);
                dictionary.Add(7, 3);
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
            // Get and assert the dictionary.
            //
            {
                var dictionary = s.Space.GetDictionary<int, int>("bar");

                Assert.AreEqual(4, dictionary.Count);
                Assert.IsTrue(new[] { "2-0", "3-1", "5-2", "7-3" }.SequenceEqual(dictionary.OrderBy(x => x.Key).Select(kv => kv.Key + "-" + kv.Value)));
            }
        });

        [TestMethod]
        public void Dictionary_Recovery_Add() => WithNewSpace(s =>
        {
            //
            // Create a dictionary.
            //
            {
                var dictionary = s.Space.CreateDictionary<int, int>("bar");

                dictionary.Add(2, 0);
                dictionary.Add(3, 1);
                dictionary.Add(5, 2);
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
            // Get the dictionary and add an element.
            //
            {
                var dictionary = s.Space.GetDictionary<int, int>("bar");

                dictionary.Add(7, 3);
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
            // Get and assert the dictionary.
            //
            {
                var dictionary = s.Space.GetDictionary<int, int>("bar");

                Assert.AreEqual(4, dictionary.Count);
                Assert.IsTrue(new[] { "2-0", "3-1", "5-2", "7-3" }.SequenceEqual(dictionary.OrderBy(x => x.Key).Select(kv => kv.Key + "-" + kv.Value)));
            }
        });

        [TestMethod]
        public void Dictionary_Recovery_Add_Differential() => WithNewSpace(s =>
        {
            //
            // Create a dictionary.
            //
            {
                var dictionary = s.Space.CreateDictionary<int, int>("bar");

                dictionary.Add(2, 0);
                dictionary.Add(3, 1);
                dictionary.Add(5, 2);
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
            // Get the dictionary and add an element.
            //
            {
                var dictionary = s.Space.GetDictionary<int, int>("bar");

                dictionary.Add(7, 3);
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
            // Get and assert the dictionary.
            //
            {
                var dictionary = s.Space.GetDictionary<int, int>("bar");

                Assert.AreEqual(4, dictionary.Count);
                Assert.IsTrue(new[] { "2-0", "3-1", "5-2", "7-3" }.SequenceEqual(dictionary.OrderBy(x => x.Key).Select(kv => kv.Key + "-" + kv.Value)));
            }
        });

        [TestMethod]
        public void Dictionary_Recovery_Remove() => WithNewSpace(s =>
        {
            //
            // Create a dictionary.
            //
            {
                var dictionary = s.Space.CreateDictionary<int, int>("bar");

                dictionary.Add(2, 0);
                dictionary.Add(3, 1);
                dictionary.Add(5, 2);
                dictionary.Add(7, 3);
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
            // Get the dictionary and remove an element.
            //
            {
                var dictionary = s.Space.GetDictionary<int, int>("bar");

                dictionary.Remove(5);
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
            // Get and assert the dictionary.
            //
            {
                var dictionary = s.Space.GetDictionary<int, int>("bar");

                Assert.AreEqual(3, dictionary.Count);
                Assert.IsTrue(new[] { "2-0", "3-1", "7-3" }.SequenceEqual(dictionary.OrderBy(x => x.Key).Select(kv => kv.Key + "-" + kv.Value)));
            }
        });

        [TestMethod]
        public void Dictionary_Recovery_Remove_Differential() => WithNewSpace(s =>
        {
            //
            // Create a dictionary.
            //
            {
                var dictionary = s.Space.CreateDictionary<int, int>("bar");

                dictionary.Add(2, 0);
                dictionary.Add(3, 1);
                dictionary.Add(5, 2);
                dictionary.Add(7, 3);
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
            // Get the dictionary and remove an element.
            //
            {
                var dictionary = s.Space.GetDictionary<int, int>("bar");

                dictionary.Remove(5);
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
            // Get and assert the dictionary.
            //
            {
                var dictionary = s.Space.GetDictionary<int, int>("bar");

                Assert.AreEqual(3, dictionary.Count);
                Assert.IsTrue(new[] { "2-0", "3-1", "7-3" }.SequenceEqual(dictionary.OrderBy(x => x.Key).Select(kv => kv.Key + "-" + kv.Value)));
            }
        });

        [TestMethod]
        public void Dictionary_Recovery_Remove1Add1() => WithNewSpace(s =>
        {
            //
            // Create a dictionary.
            //
            {
                var dictionary = s.Space.CreateDictionary<int, int>("bar");

                dictionary.Add(2, 0);
                dictionary.Add(3, 1);
                dictionary.Add(5, 2);
                dictionary.Add(7, 3);
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
            // Get the dictionary, remove and add an element.
            //
            {
                var dictionary = s.Space.GetDictionary<int, int>("bar");

                dictionary.Remove(5);
                dictionary.Add(11, 4);
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
            // Get and assert the dictionary.
            //
            {
                var dictionary = s.Space.GetDictionary<int, int>("bar");

                Assert.AreEqual(4, dictionary.Count);
                Assert.IsTrue(new[] { "2-0", "3-1", "7-3", "11-4" }.SequenceEqual(dictionary.OrderBy(x => x.Key).Select(kv => kv.Key + "-" + kv.Value)));
            }
        });

        [TestMethod]
        public void Dictionary_Recovery_Remove1Add1_Differential() => WithNewSpace(s =>
        {
            //
            // Create a dictionary.
            //
            {
                var dictionary = s.Space.CreateDictionary<int, int>("bar");

                dictionary.Add(2, 0);
                dictionary.Add(3, 1);
                dictionary.Add(5, 2);
                dictionary.Add(7, 3);
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
            // Get the dictionary, remove and add an element.
            //
            {
                var dictionary = s.Space.GetDictionary<int, int>("bar");

                dictionary.Remove(5);
                dictionary.Add(11, 4);
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
            // Get and assert the dictionary.
            //
            {
                var dictionary = s.Space.GetDictionary<int, int>("bar");

                Assert.AreEqual(4, dictionary.Count);
                Assert.IsTrue(new[] { "2-0", "3-1", "7-3", "11-4" }.SequenceEqual(dictionary.OrderBy(x => x.Key).Select(kv => kv.Key + "-" + kv.Value)));
            }
        });

        [TestMethod]
        public void Dictionary_Recovery_Remove1Add1_Freelist() => WithNewSpace(s =>
        {
            //
            // Create a dictionary.
            //
            {
                var dictionary = s.Space.CreateDictionary<int, int>("bar");

                dictionary.Add(2, 0);
                dictionary.Add(3, 1);
                dictionary.Add(5, 2);
                dictionary.Add(7, 3);
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
            // Get the dictionary and remove an element.
            //
            {
                var dictionary = s.Space.GetDictionary<int, int>("bar");

                dictionary.Remove(5);
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
            // Get the dictionary and add an element.
            //
            {
                var dictionary = s.Space.GetDictionary<int, int>("bar");

                dictionary.Add(11, 4);
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
            // Get and assert the dictionary.
            //
            {
                var dictionary = s.Space.GetDictionary<int, int>("bar");

                Assert.AreEqual(4, dictionary.Count);
                Assert.IsTrue(new[] { "2-0", "3-1", "7-3", "11-4" }.SequenceEqual(dictionary.OrderBy(x => x.Key).Select(kv => kv.Key + "-" + kv.Value)));
            }
        });

        [TestMethod]
        public void Dictionary_Recovery_Remove1Add1_Freelist_Differential() => WithNewSpace(s =>
        {
            //
            // Create a dictionary.
            //
            {
                var dictionary = s.Space.CreateDictionary<int, int>("bar");

                dictionary.Add(2, 0);
                dictionary.Add(3, 1);
                dictionary.Add(5, 2);
                dictionary.Add(7, 3);
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
            // Get the dictionary and remove an element.
            //
            {
                var dictionary = s.Space.GetDictionary<int, int>("bar");

                dictionary.Remove(5);
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
            // Get the dictionary and add an element.
            //
            {
                var dictionary = s.Space.GetDictionary<int, int>("bar");

                dictionary.Add(11, 4);
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
            // Get and assert the dictionary.
            //
            {
                var dictionary = s.Space.GetDictionary<int, int>("bar");

                Assert.AreEqual(4, dictionary.Count);
                Assert.IsTrue(new[] { "2-0", "3-1", "7-3", "11-4" }.SequenceEqual(dictionary.OrderBy(x => x.Key).Select(kv => kv.Key + "-" + kv.Value)));
            }
        });

        [TestMethod]
        public void Dictionary_Recovery_Clear() => WithNewSpace(s =>
        {
            //
            // Create a dictionary.
            //
            {
                var dictionary = s.Space.CreateDictionary<int, int>("bar");

                dictionary.Add(2, 0);
                dictionary.Add(3, 1);
                dictionary.Add(5, 2);
                dictionary.Add(7, 3);
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
            // Get the dictionary and clear it.
            //
            {
                var dictionary = s.Space.GetDictionary<int, int>("bar");

                dictionary.Clear();
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
            // Get and assert the dictionary.
            //
            {
                var dictionary = s.Space.GetDictionary<int, int>("bar");

                Assert.AreEqual(0, dictionary.Count);
            }
        });

        [TestMethod]
        public void Dictionary_Recovery_Clear_Differential() => WithNewSpace(s =>
        {
            //
            // Create a dictionary.
            //
            {
                var dictionary = s.Space.CreateDictionary<int, int>("bar");

                dictionary.Add(2, 0);
                dictionary.Add(3, 1);
                dictionary.Add(5, 2);
                dictionary.Add(7, 3);
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
            // Get the dictionary and clear it.
            //
            {
                var dictionary = s.Space.GetDictionary<int, int>("bar");

                dictionary.Clear();
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
            // Get and assert the dictionary.
            //
            {
                var dictionary = s.Space.GetDictionary<int, int>("bar");

                Assert.AreEqual(0, dictionary.Count);
            }
        });

        [TestMethod]
        public void Dictionary_Recovery_ClearAdd() => WithNewSpace(s =>
        {
            //
            // Create a dictionary.
            //
            {
                var dictionary = s.Space.CreateDictionary<int, int>("bar");

                dictionary.Add(2, 0);
                dictionary.Add(3, 1);
                dictionary.Add(5, 2);
                dictionary.Add(7, 3);
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
            // Get the dictionary, clear it, and add some elements.
            //
            {
                var dictionary = s.Space.GetDictionary<int, int>("bar");

                dictionary.Clear();
                dictionary.Add(11, 4);
                dictionary.Add(13, 5);
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
            // Get and assert the dictionary.
            //
            {
                var dictionary = s.Space.GetDictionary<int, int>("bar");

                Assert.AreEqual(2, dictionary.Count);
                Assert.IsTrue(new[] { "11-4", "13-5" }.SequenceEqual(dictionary.OrderBy(x => x.Key).Select(kv => kv.Key + "-" + kv.Value)));
            }
        });

        [TestMethod]
        public void Dictionary_Recovery_ClearAdd_Differential() => WithNewSpace(s =>
        {
            //
            // Create a dictionary.
            //
            {
                var dictionary = s.Space.CreateDictionary<int, int>("bar");

                dictionary.Add(2, 0);
                dictionary.Add(3, 1);
                dictionary.Add(5, 2);
                dictionary.Add(7, 3);
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
            // Get the dictionary, clear it, and add some elements.
            //
            {
                var dictionary = s.Space.GetDictionary<int, int>("bar");

                dictionary.Clear();
                dictionary.Add(11, 4);
                dictionary.Add(13, 5);
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
            // Get and assert the dictionary.
            //
            {
                var dictionary = s.Space.GetDictionary<int, int>("bar");

                Assert.AreEqual(2, dictionary.Count);
                Assert.IsTrue(new[] { "11-4", "13-5" }.SequenceEqual(dictionary.OrderBy(x => x.Key).Select(kv => kv.Key + "-" + kv.Value)));
            }
        });

        [TestMethod]
        public void Dictionary_Recovery_Delete() => WithNewSpace(s =>
        {
            //
            // Create a dictionary.
            //
            {
                var dictionary = s.Space.CreateDictionary<int, int>("bar");

                dictionary.Add(2, 0);
                dictionary.Add(3, 1);
                dictionary.Add(5, 2);
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
            // Delete the dictionary.
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
            // Assert the dictionary is gone.
            //

            Assert.ThrowsException<KeyNotFoundException>(() => s.Space.GetDictionary<int, int>("bar"));
        });

        [TestMethod]
        public void Dictionary_Recovery_Delete_PendingRemove() => WithNewSpace(s =>
        {
            //
            // Create a dictionary.
            //
            {
                var dictionary = s.Space.CreateDictionary<int, int>("bar");

                dictionary.Add(2, 0);
                dictionary.Add(3, 1);
                dictionary.Add(5, 2);
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
            // Get the dictionary and remove an element.
            //
            {
                var dictionary = s.Space.GetDictionary<int, int>("bar");

                dictionary.Remove(3);
            }

            //
            // Delete the dictionary.
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
            // Assert the dictionary is gone.
            //

            Assert.ThrowsException<KeyNotFoundException>(() => s.Space.GetDictionary<int, int>("bar"));
        });

        [TestMethod]
        public void Dictionary_Recovery_Delete_PendingAdd() => WithNewSpace(s =>
        {
            //
            // Create a dictionary.
            //
            {
                var dictionary = s.Space.CreateDictionary<int, int>("bar");

                dictionary.Add(2, 0);
                dictionary.Add(3, 1);
                dictionary.Add(5, 2);
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
            // Get the dictionary and add an element.
            //
            {
                var dictionary = s.Space.GetDictionary<int, int>("bar");

                dictionary.Add(7, 3);
            }

            //
            // Delete the dictionary.
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
            // Assert the dictionary is gone.
            //

            Assert.ThrowsException<KeyNotFoundException>(() => s.Space.GetDictionary<int, int>("bar"));
        });
    }
}
