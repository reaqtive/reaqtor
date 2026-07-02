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
    public class StackTest : PersistedTestBase
    {
        [TestMethod]
        public void Stack_CreateAndGet()
        {
            var assert = AssertOperation.WithAssert(MsTestAssert.Instance);
            var space = new PersistedObjectSpace(new SerializationFactory());

            CreateAndGetCore(assert).Accept(space);
        }

        [TestMethod]
        public void Stack_Volatile_CreateAndGet()
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
                    assert.ThrowsException<ArgumentNullException>().When(PersistedObjectSpaceOperation.CreateStack<int>(null))
                ),

                //
                // Exceptions thrown by Get.
                //
                Operation.Sequence(
                    assert.ThrowsException<ArgumentNullException>().When(PersistedObjectSpaceOperation.GetStack<int>(null)),
                    assert.ThrowsException<KeyNotFoundException>().When(PersistedObjectSpaceOperation.GetStack<int>("bar"))
                ),

                //
                // Exceptions thrown by Delete.
                //
                Operation.Sequence(
                    assert.ThrowsException<KeyNotFoundException>().When(PersistedObjectSpaceOperation.Delete("bar"))
                ),

                //
                // Create a new stack.
                //
                PersistedObjectSpaceOperation.CreateStack<int>("bar").Apply(stack =>

                    //
                    // Check the new stack is present in the object space.
                    //
                    Operation.Sequence(

                        //
                        // Assert we can get the same instance back using GetStack.
                        //
                        assert.AreSame(
                            PersistedObjectSpaceOperation.GetStack<int>("bar"),
                            stack
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
                // Delete stack.
                //
                PersistedObjectSpaceOperation.Delete("bar"),

                //
                // Check the stack is no longer present in the object space.
                //
                Operation.Sequence(
                    assert.ThrowsException<KeyNotFoundException>().When(PersistedObjectSpaceOperation.GetStack<int>("bar")),
                    assert.ThrowsException<KeyNotFoundException>().When(PersistedObjectSpaceOperation.Delete("bar"))
                )
            );

        [TestMethod]
        public void Stack_ManOrBoy()
        {
            var assert = AssertOperation.WithAssert(MsTestAssert.Instance);
            var space = new PersistedObjectSpace(new SerializationFactory());

            ManOrBoyCore(assert).Accept(space);
        }

        [TestMethod]
        public void Stack_Volatile_ManOrBoy()
        {
            var assert = AssertOperation.WithAssert(MsTestAssert.Instance);
            var space = new VolatilePersistedObjectSpace();

            ManOrBoyCore(assert).Accept(space);
        }

        private static IOperation<IPersistedObjectSpace> ManOrBoyCore(IAssertOperationFactory assert) =>
            PersistedObjectSpaceOperation.CreateStack<int>("bar").Bind(stack =>
                Operation.Sequence(

                    //
                    // Check the identifier
                    //
                    assert.AreEqual(
                        stack.GetId(),
                        "bar"
                    ),

                    //
                    // Empty at the start
                    //
                    Operation.Sequence(
                        assert.AreEqual(
                            stack.Count(),
                            0
                        ),
                        assert.AreSequenceEqual(
                            stack.Enumerate(),
                            Array.Empty<int>()
                        )
                    ),

                    //
                    // Push a value
                    //
                    stack.Push(42),

                    //
                    // Peek and enumerate value
                    //
                    Operation.Sequence(
                        assert.AreEqual(
                            stack.Peek(),
                            42
                        ),
                        assert.AreEqual(
                            stack.Count(),
                            1
                        ),
                        assert.AreSequenceEqual(
                            stack.Enumerate(),
                            new[] { 42 }
                        )
                    ),

                    //
                    // Push a value
                    //
                    stack.Push(43),

                    //
                    // Peek and enumerate values
                    //
                    Operation.Sequence(
                        assert.AreEqual(
                            stack.Peek(),
                            43
                        ),
                        assert.AreEqual(
                            stack.Count(),
                            2
                        ),
                        assert.AreSequenceEqual(
                            stack.Enumerate(),
                            new[] { 43, 42 }
                        )
                    ),

                    //
                    // Pop a value
                    //
                    assert.AreEqual(
                        stack.Pop(),
                        43
                    ),

                    //
                    // Peek and enumerate value
                    //
                    Operation.Sequence(
                        assert.AreEqual(
                            stack.Peek(),
                            42
                        ),
                        assert.AreEqual(
                            stack.Count(),
                            1
                        ),
                        assert.AreSequenceEqual(
                            stack.Enumerate(),
                            new[] { 42 }
                        )
                    ),

                    //
                    // Pop another value
                    //
                    assert.AreEqual(
                        stack.Pop(),
                        42
                    ),

                    //
                    // Empty now
                    //
                    Operation.Sequence(
                        assert.AreEqual(
                            stack.Count(),
                            0
                        ),
                        assert.AreSequenceEqual(
                            stack.Enumerate(),
                            Array.Empty<int>()
                        )
                    ),

                    //
                    // Can't pop anymore
                    //
                    assert.ThrowsException<InvalidOperationException>().When(stack.Pop())
                )
            );

        [TestMethod]
        public void Stack_Recovery_CreateDefault() => WithNewSpace(s =>
        {
            //
            // Create a stack.
            //

            _ = s.Space.CreateStack<int>("bar");

            //
            // Save the space to the store.
            //
            {
                var edits = s.SaveSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "count"),
                });
            }

            //
            // Create a new space.
            //

            s.CreateSpace();

            //
            // Assert the new space does not contain the stack.
            //

            Assert.ThrowsException<KeyNotFoundException>(() => s.Space.GetStack<int>("bar"));

            //
            // Load the space from the store.
            //

            s.LoadSpace();

            //
            // Get the stack.
            //

            var bar = s.Space.GetStack<int>("bar");

            //
            // Assert the stack element count.
            //

            Assert.AreEqual(0, bar.Count);
        });

        [TestMethod]
        public void Stack_Recovery_CreateStack() => WithNewSpace(s =>
        {
            //
            // Create a stack.
            //
            {
                var stack = s.Space.CreateStack<int>("bar");

                stack.Push(2);
                stack.Push(3);
                stack.Push(5);
                stack.Push(7);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "count"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                });
            }

            //
            // Get and assert the stack.
            //
            {
                var stack = s.Space.GetStack<int>("bar");

                Assert.AreEqual(4, stack.Count);
                Assert.IsTrue(new[] { 7, 5, 3, 2 }.SequenceEqual(stack));
            }
        });

        [TestMethod]
        public void Stack_Recovery_NoChange() => WithNewSpace(s =>
        {
            //
            // Create a stack.
            //
            {
                var stack = s.Space.CreateStack<int>("bar");

                stack.Push(2);
                stack.Push(3);
                stack.Push(5);
                stack.Push(7);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "count"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
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
            // Get and assert the stack.
            //
            {
                var stack = s.Space.GetStack<int>("bar");

                Assert.AreEqual(4, stack.Count);
                Assert.IsTrue(new[] { 7, 5, 3, 2 }.SequenceEqual(stack));
            }
        });

        [TestMethod]
        public void Stack_Recovery_Push1() => WithNewSpace(s =>
        {
            //
            // Create a stack.
            //
            {
                var stack = s.Space.CreateStack<int>("bar");

                stack.Push(2);
                stack.Push(3);
                stack.Push(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "count"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                });
            }

            //
            // Get the stack and push an element.
            //
            {
                var stack = s.Space.GetStack<int>("bar");

                stack.Push(7);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "count"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                });
            }

            //
            // Get and assert the stack.
            //
            {
                var stack = s.Space.GetStack<int>("bar");

                Assert.AreEqual(4, stack.Count);
                Assert.IsTrue(new[] { 7, 5, 3, 2 }.SequenceEqual(stack));
            }
        });

        [TestMethod]
        public void Stack_Recovery_Push1_Differential() => WithNewSpace(s =>
        {
            //
            // Create a stack.
            //
            {
                var stack = s.Space.CreateStack<int>("bar");

                stack.Push(2);
                stack.Push(3);
                stack.Push(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "count"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                });
            }

            //
            // Get the stack and push an element.
            //
            {
                var stack = s.Space.GetStack<int>("bar");

                stack.Push(7);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "count"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                });
            }

            //
            // Get and assert the stack.
            //
            {
                var stack = s.Space.GetStack<int>("bar");

                Assert.AreEqual(4, stack.Count);
                Assert.IsTrue(new[] { 7, 5, 3, 2 }.SequenceEqual(stack));
            }
        });

        [TestMethod]
        public void Stack_Recovery_Push2() => WithNewSpace(s =>
        {
            //
            // Create a stack.
            //
            {
                var stack = s.Space.CreateStack<int>("bar");

                stack.Push(2);
                stack.Push(3);
                stack.Push(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "count"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                });
            }

            //
            // Get the stack and push two elements.
            //
            {
                var stack = s.Space.GetStack<int>("bar");

                stack.Push(7);
                stack.Push(11);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "count"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "4"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                });
            }

            //
            // Get and assert the stack.
            //
            {
                var stack = s.Space.GetStack<int>("bar");

                Assert.AreEqual(5, stack.Count);
                Assert.IsTrue(new[] { 11, 7, 5, 3, 2 }.SequenceEqual(stack));
            }
        });

        [TestMethod]
        public void Stack_Recovery_Push2_Differential() => WithNewSpace(s =>
        {
            //
            // Create a stack.
            //
            {
                var stack = s.Space.CreateStack<int>("bar");

                stack.Push(2);
                stack.Push(3);
                stack.Push(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "count"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                });
            }

            //
            // Get the stack and push two elements.
            //
            {
                var stack = s.Space.GetStack<int>("bar");

                stack.Push(7);
                stack.Push(11);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "count"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "4"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                });
            }

            //
            // Get and assert the stack.
            //
            {
                var stack = s.Space.GetStack<int>("bar");

                Assert.AreEqual(5, stack.Count);
                Assert.IsTrue(new[] { 11, 7, 5, 3, 2 }.SequenceEqual(stack));
            }
        });

        [TestMethod]
        public void Stack_Recovery_Push2Pop1() => WithNewSpace(s =>
        {
            //
            // Create a stack.
            //
            {
                var stack = s.Space.CreateStack<int>("bar");

                stack.Push(2);
                stack.Push(3);
                stack.Push(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "count"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                });
            }

            //
            // Get the stack, push two elements, and pop one element.
            //
            {
                var stack = s.Space.GetStack<int>("bar");

                stack.Push(7);
                stack.Push(11);
                stack.Pop();
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "count"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                });
            }

            //
            // Get and assert the stack.
            //
            {
                var stack = s.Space.GetStack<int>("bar");

                Assert.AreEqual(4, stack.Count);
                Assert.IsTrue(new[] { 7, 5, 3, 2 }.SequenceEqual(stack));
            }
        });

        [TestMethod]
        public void Stack_Recovery_Push2Pop1_Differential() => WithNewSpace(s =>
        {
            //
            // Create a stack.
            //
            {
                var stack = s.Space.CreateStack<int>("bar");

                stack.Push(2);
                stack.Push(3);
                stack.Push(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "count"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                });
            }

            //
            // Get the stack, push two elements, and pop one element.
            //
            {
                var stack = s.Space.GetStack<int>("bar");

                stack.Push(7);
                stack.Push(11);
                stack.Pop();
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "count"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                });
            }

            //
            // Get and assert the stack.
            //
            {
                var stack = s.Space.GetStack<int>("bar");

                Assert.AreEqual(4, stack.Count);
                Assert.IsTrue(new[] { 7, 5, 3, 2 }.SequenceEqual(stack));
            }
        });

        [TestMethod]
        public void Stack_Recovery_Pop1() => WithNewSpace(s =>
        {
            //
            // Create a stack.
            //
            {
                var stack = s.Space.CreateStack<int>("bar");

                stack.Push(2);
                stack.Push(3);
                stack.Push(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "count"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                });
            }

            //
            // Get the stack and pop an element.
            //
            {
                var stack = s.Space.GetStack<int>("bar");

                stack.Pop();
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "count"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                });
            }

            //
            // Get and assert the stack.
            //
            {
                var stack = s.Space.GetStack<int>("bar");

                Assert.AreEqual(2, stack.Count);
                Assert.IsTrue(new[] { 3, 2 }.SequenceEqual(stack));
            }
        });

        [TestMethod]
        public void Stack_Recovery_Pop1_Differential() => WithNewSpace(s =>
        {
            //
            // Create a stack.
            //
            {
                var stack = s.Space.CreateStack<int>("bar");

                stack.Push(2);
                stack.Push(3);
                stack.Push(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "count"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                });
            }

            //
            // Get the stack and pop an element.
            //
            {
                var stack = s.Space.GetStack<int>("bar");

                stack.Pop();
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "count"),
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "2"),
                });
            }

            //
            // Get and assert the stack.
            //
            {
                var stack = s.Space.GetStack<int>("bar");

                Assert.AreEqual(2, stack.Count);
                Assert.IsTrue(new[] { 3, 2 }.SequenceEqual(stack));
            }
        });

        [TestMethod]
        public void Stack_Recovery_Pop2() => WithNewSpace(s =>
        {
            //
            // Create a stack.
            //
            {
                var stack = s.Space.CreateStack<int>("bar");

                stack.Push(2);
                stack.Push(3);
                stack.Push(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "count"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                });
            }

            //
            // Get the stack and pop two elements.
            //
            {
                var stack = s.Space.GetStack<int>("bar");

                stack.Pop();
                stack.Pop();
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "count"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                });
            }

            //
            // Get and assert the stack.
            //
            {
                var stack = s.Space.GetStack<int>("bar");

                Assert.AreEqual(1, stack.Count);
                Assert.IsTrue(new[] { 2 }.SequenceEqual(stack));
            }
        });

        [TestMethod]
        public void Stack_Recovery_Pop2_Differential() => WithNewSpace(s =>
        {
            //
            // Create a stack.
            //
            {
                var stack = s.Space.CreateStack<int>("bar");

                stack.Push(2);
                stack.Push(3);
                stack.Push(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "count"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                });
            }

            //
            // Get the stack and pop two elements.
            //
            {
                var stack = s.Space.GetStack<int>("bar");

                stack.Pop();
                stack.Pop();
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "count"),
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "1"),
                });
            }

            //
            // Get and assert the stack.
            //
            {
                var stack = s.Space.GetStack<int>("bar");

                Assert.AreEqual(1, stack.Count);
                Assert.IsTrue(new[] { 2 }.SequenceEqual(stack));
            }
        });

        [TestMethod]
        public void Stack_Recovery_Pop2Push1() => WithNewSpace(s =>
        {
            //
            // Create a stack.
            //
            {
                var stack = s.Space.CreateStack<int>("bar");

                stack.Push(2);
                stack.Push(3);
                stack.Push(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "count"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                });
            }

            //
            // Get the stack, pop two elements, and push one element.
            //
            {
                var stack = s.Space.GetStack<int>("bar");

                stack.Pop();
                stack.Pop();
                stack.Push(7);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "count"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                });
            }

            //
            // Get and assert the stack.
            //
            {
                var stack = s.Space.GetStack<int>("bar");

                Assert.AreEqual(2, stack.Count);
                Assert.IsTrue(new[] { 7, 2 }.SequenceEqual(stack));
            }
        });

        [TestMethod]
        public void Stack_Recovery_Pop2Push1_Differential() => WithNewSpace(s =>
        {
            //
            // Create a stack.
            //
            {
                var stack = s.Space.CreateStack<int>("bar");

                stack.Push(2);
                stack.Push(3);
                stack.Push(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "count"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                });
            }

            //
            // Get the stack, pop two elements, and push one element.
            //
            {
                var stack = s.Space.GetStack<int>("bar");

                stack.Pop();
                stack.Pop();
                stack.Push(7);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "count"),
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                });
            }

            //
            // Get and assert the stack.
            //
            {
                var stack = s.Space.GetStack<int>("bar");

                Assert.AreEqual(2, stack.Count);
                Assert.IsTrue(new[] { 7, 2 }.SequenceEqual(stack));
            }
        });

        [TestMethod]
        public void Stack_Recovery_PushAndPop() => WithNewSpace(s =>
        {
            //
            // Create a stack.
            //
            {
                var stack = s.Space.CreateStack<int>("bar");

                stack.Push(2);
                stack.Push(3);
                stack.Push(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "count"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                });
            }

            //
            // Get the stack and push/pop some elements.
            //
            {
                var stack = s.Space.GetStack<int>("bar");

                stack.Pop();
                stack.Push(7);
                stack.Push(11);
                stack.Pop();
                stack.Push(13);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "count"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                });
            }

            //
            // Get and assert the stack.
            //
            {
                var stack = s.Space.GetStack<int>("bar");

                Assert.AreEqual(4, stack.Count);
                Assert.IsTrue(new[] { 13, 7, 3, 2 }.SequenceEqual(stack));
            }
        });

        [TestMethod]
        public void Stack_Recovery_PushAndPop_Differential() => WithNewSpace(s =>
        {
            //
            // Create a stack.
            //
            {
                var stack = s.Space.CreateStack<int>("bar");

                stack.Push(2);
                stack.Push(3);
                stack.Push(5);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "count"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                });
            }

            //
            // Get the stack and push/pop some elements.
            //
            {
                var stack = s.Space.GetStack<int>("bar");

                stack.Pop();
                stack.Push(7);
                stack.Push(11);
                stack.Pop();
                stack.Push(13);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "count"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "3"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                });
            }

            //
            // Get and assert the stack.
            //
            {
                var stack = s.Space.GetStack<int>("bar");

                Assert.AreEqual(4, stack.Count);
                Assert.IsTrue(new[] { 13, 7, 3, 2 }.SequenceEqual(stack));
            }
        });

        [TestMethod]
        public void Stack_Recovery_Delete() => WithNewSpace(s =>
        {
            //
            // Create a stack.
            //
            {
                var stack = s.Space.CreateStack<int>("bar");

                stack.Push(0);
                stack.Push(1);
                stack.Push(2);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "count"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                });
            }

            //
            // Delete the stack.
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
                    (StateWriterOperationKind.Delete, "state/item/bar/metadata", "count"),
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "2"),
                });
            }

            //
            // Assert the stack is gone.
            //

            Assert.ThrowsException<KeyNotFoundException>(() => s.Space.GetValue<int>("bar"));
        });

        [TestMethod]
        public void Stack_Recovery_Delete_PendingPop() => WithNewSpace(s =>
        {
            //
            // Create a stack.
            //
            {
                var stack = s.Space.CreateStack<int>("bar");

                stack.Push(0);
                stack.Push(1);
                stack.Push(2);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "count"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                });
            }

            //
            // Get the stack and pop an element.
            //
            {
                var stack = s.Space.GetStack<int>("bar");

                stack.Pop();
            }

            //
            // Delete the stack.
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
                    (StateWriterOperationKind.Delete, "state/item/bar/metadata", "count"),
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "2"),
                });
            }

            //
            // Assert the stack is gone.
            //

            Assert.ThrowsException<KeyNotFoundException>(() => s.Space.GetValue<int>("bar"));
        });

        [TestMethod]
        public void Stack_Recovery_Delete_PendingPush() => WithNewSpace(s =>
        {
            //
            // Create a stack.
            //
            {
                var stack = s.Space.CreateStack<int>("bar");

                stack.Push(0);
                stack.Push(1);
                stack.Push(2);
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/metadata", "count"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "2"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/items", "0"),
                });
            }

            //
            // Get the stack and push an element.
            //
            {
                var stack = s.Space.GetStack<int>("bar");

                stack.Push(3);
            }

            //
            // Delete the stack.
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
                    (StateWriterOperationKind.Delete, "state/item/bar/metadata", "count"),
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "0"),
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "1"),
                    (StateWriterOperationKind.Delete, "state/item/bar/items", "2"),
                });
            }

            //
            // Assert the stack is gone.
            //

            Assert.ThrowsException<KeyNotFoundException>(() => s.Space.GetStack<int>("bar"));
        });
    }
}
