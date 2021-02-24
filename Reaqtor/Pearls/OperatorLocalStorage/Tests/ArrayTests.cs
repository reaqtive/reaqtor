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
    public class ArrayTests : PersistedTestBase
    {
        [TestMethod]
        public void Array_CreateAndGet()
        {
            var assert = AssertOperation.WithAssert(MsTestAssert.Instance);
            var space = new PersistedObjectSpace(new SerializationFactory());

            CreateAndGetCore(assert).Accept(space);
        }

        [TestMethod]
        public void Array_Volatile_CreateAndGet()
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
                    assert.ThrowsException<ArgumentNullException>().When(PersistedObjectSpaceOperation.CreateArray<int>(null, 1)),
                    assert.ThrowsException<ArgumentOutOfRangeException>().When(PersistedObjectSpaceOperation.CreateArray<int>("bar", -1))
                ),

                //
                // Exceptions thrown by Get.
                //
                Operation.Sequence(
                    assert.ThrowsException<ArgumentNullException>().When(PersistedObjectSpaceOperation.GetArray<int>(null)),
                    assert.ThrowsException<KeyNotFoundException>().When(PersistedObjectSpaceOperation.GetArray<int>("bar"))
                ),

                //
                // Exceptions thrown by Delete.
                //
                Operation.Sequence(
                    assert.ThrowsException<KeyNotFoundException>().When(PersistedObjectSpaceOperation.Delete("bar"))
                ),

                //
                // Create a new array.
                //
                PersistedObjectSpaceOperation.CreateArray<int>("bar", 42).Apply(array =>

                    //
                    // Check the new array is present in the object space.
                    //
                    Operation.Sequence(

                        //
                        // Assert we can get the same instance back using GetArray.
                        //
                        assert.AreSame(
                            PersistedObjectSpaceOperation.GetArray<int>("bar"),
                            array
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
                // Delete array.
                //
                PersistedObjectSpaceOperation.Delete("bar"),

                //
                // Check the array is no longer present in the object space.
                //
                Operation.Sequence(
                    assert.ThrowsException<KeyNotFoundException>().When(PersistedObjectSpaceOperation.GetArray<int>("bar")),
                    assert.ThrowsException<KeyNotFoundException>().When(PersistedObjectSpaceOperation.Delete("bar"))
                )
            );

        [TestMethod]
        public void Array_ManOrBoy()
        {
            var assert = AssertOperation.WithAssert(MsTestAssert.Instance);
            var space = new PersistedObjectSpace(new SerializationFactory());

            ManOrBoyCore(assert).Accept(space);
        }

        [TestMethod]
        public void Array_Volatile_ManOrBoy()
        {
            var assert = AssertOperation.WithAssert(MsTestAssert.Instance);
            var space = new VolatilePersistedObjectSpace();

            ManOrBoyCore(assert).Accept(space);
        }

        private static IOperation<IPersistedObjectSpace> ManOrBoyCore(IAssertOperationFactory assert) =>
            PersistedObjectSpaceOperation.CreateArray<int>("bar", 1).Bind(array =>
                Operation.Sequence(

                    //
                    // Check the identifier
                    //
                    assert.AreEqual(
                        array.GetId(),
                        "bar"
                    ),

                    //
                    // Created with correct length
                    //
                    Operation.Sequence(
                        assert.AreEqual(
                            array.Length(),
                            1
                        ),
                        assert.AreEqual(
                            array.Count(),
                            1
                        )
                    ),

                    //
                    // Out of bounds exceptions are thrown
                    //
                    Operation.Sequence(
                        assert.ThrowsException<IndexOutOfRangeException>().When(
                            array.Get(-1)
                        ),
                        assert.ThrowsException<IndexOutOfRangeException>().When(
                            array.Get(1)
                        )
                    ),

                    //
                    // Elements initialized to default values
                    //
                    Operation.Sequence(
                        assert.AreEqual(
                            array.Get(0),
                            0
                        ),
                        assert.AreSequenceEqual(
                            array.Enumerate(),
                            new[] { 0 }
                        )
                    ),

                    //
                    // Perform assignment
                    //
                    array.Set(0, 42),

                    //
                    // Check values after assignment
                    //
                    Operation.Sequence(
                        assert.AreEqual(
                            array.Get(0),
                            42
                        ),
                        assert.AreSequenceEqual(
                            array.Enumerate(),
                            new[] { 42 }
                        )
                    )
                )
            );

        [TestMethod]
        public void Array_Recovery_CreateDefault() => WithNewSpace(s =>
        {
            //
            // Create an array.
            //

            _ = s.Space.CreateArray<int>("bar", 3);

            //
            // Save the space to the store.
            //
            {
                var edits = s.SaveSpace(differential: false);

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
            // Create a new space.
            //

            s.CreateSpace();

            //
            // Assert the new space does not contain the array.
            //

            Assert.ThrowsException<KeyNotFoundException>(() => s.Space.GetArray<int>("bar"));

            //
            // Load the space from the store.
            //

            s.LoadSpace();

            //
            // Get the array.
            //

            var bar = s.Space.GetArray<int>("bar");

            //
            // Assert the array length and values.
            //

            Assert.AreEqual(3, bar.Length);
            Assert.IsTrue(Enumerable.Repeat(0, 3).SequenceEqual(bar));
        });

        [TestMethod]
        public void Array_Recovery_CreateArray() => WithNewSpace(s =>
        {
            //
            // Create an array.
            //
            {
                var array = s.Space.CreateArray<int>("bar", 4);

                array[0] = 2;
                array[1] = 3;
                array[2] = 5;
                array[3] = 7;
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
            // Get and assert the array.
            //
            {
                var array = s.Space.GetArray<int>("bar");

                Assert.AreEqual(4, array.Length);
                Assert.IsTrue(new[] { 2, 3, 5, 7 }.SequenceEqual(array));
            }
        });

        [TestMethod]
        public void Array_Recovery_NoChange() => WithNewSpace(s =>
        {
            //
            // Create an array.
            //
            {
                var array = s.Space.CreateArray<int>("bar", 4);

                array[0] = 2;
                array[1] = 3;
                array[2] = 5;
                array[3] = 7;
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
            // Get and assert the array.
            //
            {
                var array = s.Space.GetArray<int>("bar");

                Assert.AreEqual(4, array.Length);
                Assert.IsTrue(new[] { 2, 3, 5, 7 }.SequenceEqual(array));
            }
        });

        [TestMethod]
        public void Array_Recovery_UpdateElements_All() => WithNewSpace(s =>
        {
            //
            // Create an array.
            //
            {
                var array = s.Space.CreateArray<int>("bar", 4);

                array[0] = 2;
                array[1] = 3;
                array[2] = 5;
                array[3] = 7;
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
            // Get and update the array.
            //
            {
                var array = s.Space.GetArray<int>("bar");

                for (var i = 0; i < array.Length; i++)
                {
                    array[i] = -array[i];
                }
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
            // Get and assert the array.
            //
            {
                var array = s.Space.GetArray<int>("bar");

                Assert.AreEqual(4, array.Length);
                Assert.IsTrue(new[] { -2, -3, -5, -7 }.SequenceEqual(array));
            }
        });

        [TestMethod]
        public void Array_Recovery_UpdateElements_Some() => WithNewSpace(s =>
        {
            //
            // Create an array.
            //
            {
                var array = s.Space.CreateArray<int>("bar", 4);

                array[0] = 2;
                array[1] = 3;
                array[2] = 42;
                array[3] = 7;
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
            // Get and update the array.
            //
            {
                var array = s.Space.GetArray<int>("bar");

                array[2] = 5;
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
            // Get and assert the array.
            //
            {
                var array = s.Space.GetArray<int>("bar");

                Assert.AreEqual(4, array.Length);
                Assert.IsTrue(new[] { 2, 3, 5, 7 }.SequenceEqual(array));
            }
        });

        [TestMethod]
        public void Array_Recovery_UpdateElements_All_Differential() => WithNewSpace(s =>
        {
            //
            // Create an array.
            //
            {
                var array = s.Space.CreateArray<int>("bar", 4);

                array[0] = 2;
                array[1] = 3;
                array[2] = 5;
                array[3] = 7;
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
            // Get and update the array.
            //
            {
                var array = s.Space.GetArray<int>("bar");

                for (var i = 0; i < array.Length; i++)
                {
                    array[i] = -array[i];
                }
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                });
            }

            //
            // Get and assert the array.
            //
            {
                var array = s.Space.GetArray<int>("bar");

                Assert.AreEqual(4, array.Length);
                Assert.IsTrue(new[] { -2, -3, -5, -7 }.SequenceEqual(array));
            }
        });

        [TestMethod]
        public void Array_Recovery_UpdateElements_Some_Differential() => WithNewSpace(s =>
        {
            //
            // Create an array.
            //
            {
                var array = s.Space.CreateArray<int>("bar", 4);

                array[0] = 2;
                array[1] = 3;
                array[2] = 42;
                array[3] = 7;
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
            // Get and update the array.
            //
            {
                var array = s.Space.GetArray<int>("bar");

                array[2] = 5;
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
            // Get and assert the array.
            //
            {
                var array = s.Space.GetArray<int>("bar");

                Assert.AreEqual(4, array.Length);
                Assert.IsTrue(new[] { 2, 3, 5, 7 }.SequenceEqual(array));
            }
        });

        [TestMethod]
        public void Array_Recovery_Delete() => WithNewSpace(s =>
        {
            //
            // Create an array.
            //
            _ = s.Space.CreateArray<int>("bar", 4);

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
            // Delete the array.
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
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "3"),
                });
            }

            //
            // Assert the array is gone.
            //

            Assert.ThrowsException<KeyNotFoundException>(() => s.Space.GetArray<int>("bar"));
        });
    }
}
