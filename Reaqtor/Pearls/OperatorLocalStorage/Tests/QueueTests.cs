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
    public class QueueTest : PersistedTestBase
    {
        [TestMethod]
        public void Queue_CreateAndGet()
        {
            var assert = AssertOperation.WithAssert(MsTestAssert.Instance);
            var space = new PersistedObjectSpace(new SerializationFactory());

            CreateAndGetCore(assert).Accept(space);
        }

        [TestMethod]
        public void Queue_Volatile_CreateAndGet()
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
                    assert.ThrowsException<ArgumentNullException>().When(PersistedObjectSpaceOperation.CreateQueue<int>(null))
                ),

                //
                // Exceptions thrown by Get.
                //
                Operation.Sequence(
                    assert.ThrowsException<ArgumentNullException>().When(PersistedObjectSpaceOperation.GetQueue<int>(null)),
                    assert.ThrowsException<KeyNotFoundException>().When(PersistedObjectSpaceOperation.GetQueue<int>("bar"))
                ),

                //
                // Exceptions thrown by Delete.
                //
                Operation.Sequence(
                    assert.ThrowsException<KeyNotFoundException>().When(PersistedObjectSpaceOperation.Delete("bar"))
                ),

                //
                // Create a new queue.
                //
                PersistedObjectSpaceOperation.CreateQueue<int>("bar").Apply(queue =>

                    //
                    // Check the new queue is present in the object space.
                    //
                    Operation.Sequence(

                        //
                        // Assert we can get the same instance back using GetQueue.
                        //
                        assert.AreSame(
                            PersistedObjectSpaceOperation.GetQueue<int>("bar"),
                            queue
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
                // Delete queue.
                //
                PersistedObjectSpaceOperation.Delete("bar"),

                //
                // Check the queue is no longer present in the object space.
                //
                Operation.Sequence(
                    assert.ThrowsException<KeyNotFoundException>().When(PersistedObjectSpaceOperation.GetQueue<int>("bar")),
                    assert.ThrowsException<KeyNotFoundException>().When(PersistedObjectSpaceOperation.Delete("bar"))
                )
            );

        [TestMethod]
        public void Queue_ManOrBoy()
        {
            var assert = AssertOperation.WithAssert(MsTestAssert.Instance);
            var space = new PersistedObjectSpace(new SerializationFactory());

            ManOrBoyCore(assert).Accept(space);
        }

        [TestMethod]
        public void Queue_Volatile_ManOrBoy()
        {
            var assert = AssertOperation.WithAssert(MsTestAssert.Instance);
            var space = new VolatilePersistedObjectSpace();

            ManOrBoyCore(assert).Accept(space);
        }

        private static IOperation<IPersistedObjectSpace> ManOrBoyCore(IAssertOperationFactory assert) =>
            PersistedObjectSpaceOperation.CreateQueue<int>("bar").Bind(queue =>
                Operation.Sequence(

                    //
                    // Check the identifier
                    //
                    assert.AreEqual(
                        queue.GetId(),
                        "bar"
                    ),

                    //
                    // Empty at the start
                    //
                    Operation.Sequence(
                        assert.AreEqual(
                            queue.Count(),
                            0
                        ),
                        assert.AreSequenceEqual(
                            queue.Enumerate(),
                            Array.Empty<int>()
                        )
                    ),

                    //
                    // Enqueue a value
                    //
                    queue.Enqueue(42),

                    //
                    // Peek and enumerate value
                    //
                    Operation.Sequence(
                        assert.AreEqual(
                            queue.Peek(),
                            42
                        ),
                        assert.AreEqual(
                            queue.Count(),
                            1
                        ),
                        assert.AreSequenceEqual(
                            queue.Enumerate(),
                            new[] { 42 }
                        )
                    ),

                    //
                    // Enqueue a value
                    //
                    queue.Enqueue(43),

                    //
                    // Peek and enumerate values
                    //
                    Operation.Sequence(
                        assert.AreEqual(
                            queue.Peek(),
                            42
                        ),
                        assert.AreEqual(
                            queue.Count(),
                            2
                        ),
                        assert.AreSequenceEqual(
                            queue.Enumerate(),
                            new[] { 42, 43 }
                        )
                    ),

                    //
                    // Dequeue a value
                    //
                    assert.AreEqual(
                        queue.Dequeue(),
                        42
                    ),

                    //
                    // Peek and enumerate value
                    //
                    Operation.Sequence(
                        assert.AreEqual(
                            queue.Peek(),
                            43
                        ),
                        assert.AreEqual(
                            queue.Count(),
                            1
                        ),
                        assert.AreSequenceEqual(
                            queue.Enumerate(),
                            new[] { 43 }
                        )
                    ),

                    //
                    // Dequeue another value
                    //
                    assert.AreEqual(
                        queue.Dequeue(),
                        43
                    ),

                    //
                    // Empty now
                    //
                    Operation.Sequence(
                        assert.AreEqual(
                            queue.Count(),
                            0
                        ),
                        assert.AreSequenceEqual(
                            queue.Enumerate(),
                            Array.Empty<int>()
                        )
                    ),

                    //
                    // Can't dequeue anymore
                    //
                    assert.ThrowsException<InvalidOperationException>().When(queue.Dequeue())
                )
            );

        [TestMethod]
        public void Queue_Recovery_CreateDefault() => WithNewSpace(s =>
        {
            //
            // Create a queue.
            //

            _ = s.Space.CreateQueue<int>("bar");

            //
            // Save the space to the store.
            //
            {
                var edits = s.SaveSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "head"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "tail"),
                });
            }

            //
            // Create a new space.
            //

            s.CreateSpace();

            //
            // Assert the new space does not contain the queue.
            //

            Assert.ThrowsException<KeyNotFoundException>(() => s.Space.GetQueue<int>("bar"));

            //
            // Load the space from the store.
            //

            s.LoadSpace();

            //
            // Get the queue.
            //

            var bar = s.Space.GetQueue<int>("bar");

            //
            // Assert the queue element count.
            //

            Assert.AreEqual(0, bar.Count);
        });

        [TestMethod]
        public void Queue_Recovery_CreateQueue() => WithNewSpace(s =>
        {
            //
            // Create a queue.
            //
            {
                var queue = s.Space.CreateQueue<int>("bar");

                queue.Enqueue(2);
                queue.Enqueue(3);
                queue.Enqueue(5);
                queue.Enqueue(7);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "head"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "tail"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                });
            }

            //
            // Get and assert the queue.
            //
            {
                var queue = s.Space.GetQueue<int>("bar");

                Assert.AreEqual(4, queue.Count);
                Assert.IsTrue(new[] { 2, 3, 5, 7 }.SequenceEqual(queue));
            }
        });

        [TestMethod]
        public void Queue_Recovery_NoChange() => WithNewSpace(s =>
        {
            //
            // Create a queue.
            //
            {
                var queue = s.Space.CreateQueue<int>("bar");

                queue.Enqueue(2);
                queue.Enqueue(3);
                queue.Enqueue(5);
                queue.Enqueue(7);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "head"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "tail"),
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
            // Get and assert the queue.
            //
            {
                var queue = s.Space.GetQueue<int>("bar");

                Assert.AreEqual(4, queue.Count);
                Assert.IsTrue(new[] { 2, 3, 5, 7 }.SequenceEqual(queue));
            }
        });

        [TestMethod]
        public void Queue_Recovery_Enqueue() => WithNewSpace(s =>
        {
            //
            // Create a queue.
            //
            {
                var queue = s.Space.CreateQueue<int>("bar");

                queue.Enqueue(2);
                queue.Enqueue(3);
                queue.Enqueue(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "head"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "tail"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                });
            }

            //
            // Get the queue and enqueue an element.
            //
            {
                var queue = s.Space.GetQueue<int>("bar");

                queue.Enqueue(7);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "head"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "tail"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                });
            }

            //
            // Get and assert the queue.
            //
            {
                var queue = s.Space.GetQueue<int>("bar");

                Assert.AreEqual(4, queue.Count);
                Assert.IsTrue(new[] { 2, 3, 5, 7 }.SequenceEqual(queue));
            }
        });

        [TestMethod]
        public void Queue_Recovery_Enqueue_Differential() => WithNewSpace(s =>
        {
            //
            // Create a queue.
            //
            {
                var queue = s.Space.CreateQueue<int>("bar");

                queue.Enqueue(2);
                queue.Enqueue(3);
                queue.Enqueue(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "head"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "tail"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                });
            }

            //
            // Get the queue and enqueue an element.
            //
            {
                var queue = s.Space.GetQueue<int>("bar");

                queue.Enqueue(7);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "tail"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                });
            }

            //
            // Get and assert the queue.
            //
            {
                var queue = s.Space.GetQueue<int>("bar");

                Assert.AreEqual(4, queue.Count);
                Assert.IsTrue(new[] { 2, 3, 5, 7 }.SequenceEqual(queue));
            }
        });

        [TestMethod]
        public void Queue_Recovery_Dequeue() => WithNewSpace(s =>
        {
            //
            // Create a queue.
            //
            {
                var queue = s.Space.CreateQueue<int>("bar");

                queue.Enqueue(2);
                queue.Enqueue(3);
                queue.Enqueue(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "head"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "tail"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                });
            }

            //
            // Get the queue and dequeue an element.
            //
            {
                var queue = s.Space.GetQueue<int>("bar");

                queue.Dequeue();
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "head"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "tail"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                });
            }

            //
            // Get and assert the queue.
            //
            {
                var queue = s.Space.GetQueue<int>("bar");

                Assert.AreEqual(2, queue.Count);
                Assert.IsTrue(new[] { 3, 5 }.SequenceEqual(queue));
            }
        });

        [TestMethod]
        public void Queue_Recovery_Dequeue_Differential() => WithNewSpace(s =>
        {
            //
            // Create a queue.
            //
            {
                var queue = s.Space.CreateQueue<int>("bar");

                queue.Enqueue(2);
                queue.Enqueue(3);
                queue.Enqueue(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "head"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "tail"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                });
            }

            //
            // Get the queue and dequeue an element.
            //
            {
                var queue = s.Space.GetQueue<int>("bar");

                queue.Dequeue();
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "head"),
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "0"),
                });
            }

            //
            // Get and assert the queue.
            //
            {
                var queue = s.Space.GetQueue<int>("bar");

                Assert.AreEqual(2, queue.Count);
                Assert.IsTrue(new[] { 3, 5 }.SequenceEqual(queue));
            }
        });

        [TestMethod]
        public void Queue_Recovery_EnqueueAndDequeue() => WithNewSpace(s =>
        {
            //
            // Create a queue.
            //
            {
                var queue = s.Space.CreateQueue<int>("bar");

                queue.Enqueue(2);
                queue.Enqueue(3);
                queue.Enqueue(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "head"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "tail"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                });
            }

            //
            // Get the queue and enqueue/dequeue some elements.
            //
            {
                var queue = s.Space.GetQueue<int>("bar");

                queue.Dequeue();
                queue.Enqueue(7);
                queue.Enqueue(11);
                queue.Dequeue();
                queue.Enqueue(13);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "head"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "tail"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "4"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "5"),
                });
            }

            //
            // Get and assert the queue.
            //
            {
                var queue = s.Space.GetQueue<int>("bar");

                Assert.AreEqual(4, queue.Count);
                Assert.IsTrue(new[] { 5, 7, 11, 13 }.SequenceEqual(queue));
            }
        });

        [TestMethod]
        public void Queue_Recovery_EnqueueAndDequeue_Differential() => WithNewSpace(s =>
        {
            //
            // Create a queue.
            //
            {
                var queue = s.Space.CreateQueue<int>("bar");

                queue.Enqueue(2);
                queue.Enqueue(3);
                queue.Enqueue(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "head"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "tail"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                });
            }

            //
            // Get the queue and enqueue/dequeue some elements.
            //
            {
                var queue = s.Space.GetQueue<int>("bar");

                queue.Dequeue();
                queue.Enqueue(7);
                queue.Enqueue(11);
                queue.Dequeue();
                queue.Enqueue(13);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "head"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "tail"),
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "4"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "5"),
                });
            }

            //
            // Get and assert the queue.
            //
            {
                var queue = s.Space.GetQueue<int>("bar");

                Assert.AreEqual(4, queue.Count);
                Assert.IsTrue(new[] { 5, 7, 11, 13 }.SequenceEqual(queue));
            }
        });

        [TestMethod]
        public void Queue_Recovery_Delete() => WithNewSpace(s =>
        {
            //
            // Create a queue.
            //
            {
                var queue = s.Space.CreateQueue<int>("bar");

                queue.Enqueue(0);
                queue.Enqueue(1);
                queue.Enqueue(2);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "head"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "tail"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                });
            }

            //
            // Delete the queue.
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
                    (StateWriterOperationKind.Delete, "state/item/bar/metadata", "head"),
                    (StateWriterOperationKind.Delete, "state/item/bar/metadata", "tail"),
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "2"),
                });
            }

            //
            // Assert the queue is gone.
            //

            Assert.ThrowsException<KeyNotFoundException>(() => s.Space.GetValue<int>("bar"));
        });

        [TestMethod]
        public void Queue_Recovery_Delete_PendingDequeue() => WithNewSpace(s =>
        {
            //
            // Create a queue.
            //
            {
                var queue = s.Space.CreateQueue<int>("bar");

                queue.Enqueue(0);
                queue.Enqueue(1);
                queue.Enqueue(2);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "head"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "tail"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                });
            }

            //
            // Get the queue and dequeue an element.
            //
            {
                var queue = s.Space.GetQueue<int>("bar");

                queue.Dequeue();
            }

            //
            // Delete the queue.
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
                    (StateWriterOperationKind.Delete, "state/item/bar/metadata", "head"),
                    (StateWriterOperationKind.Delete, "state/item/bar/metadata", "tail"),
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "2"),
                });
            }

            //
            // Assert the queue is gone.
            //

            Assert.ThrowsException<KeyNotFoundException>(() => s.Space.GetValue<int>("bar"));
        });

        [TestMethod]
        public void Queue_Recovery_Delete_PendingEnqueue() => WithNewSpace(s =>
        {
            //
            // Create a queue.
            //
            {
                var queue = s.Space.CreateQueue<int>("bar");

                queue.Enqueue(0);
                queue.Enqueue(1);
                queue.Enqueue(2);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "head"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "tail"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                });
            }

            //
            // Get the queue and enqueue an element.
            //
            {
                var queue = s.Space.GetQueue<int>("bar");

                queue.Enqueue(3);
            }

            //
            // Delete the queue.
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
                    (StateWriterOperationKind.Delete, "state/item/bar/metadata", "head"),
                    (StateWriterOperationKind.Delete, "state/item/bar/metadata", "tail"),
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "2"),
                });
            }

            //
            // Assert the queue is gone.
            //

            Assert.ThrowsException<KeyNotFoundException>(() => s.Space.GetQueue<int>("bar"));
        });
    }
}
