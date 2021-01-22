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
using Tests.ReifiedOperations;
using Utilities;

namespace Tests
{
    [TestClass]
    public class ValueTests : PersistedTestBase
    {
        [TestMethod]
        public void Value_CreateAndGet()
        {
            var assert = AssertOperation.WithAssert(MsTestAssert.Instance);
            var space = new PersistedObjectSpace(new SerializationFactory());

            CreateAndGetCore(assert).Accept(space);
        }

        [TestMethod]
        public void Value_Volatile_CreateAndGet()
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
                    assert.ThrowsException<ArgumentNullException>().When(PersistedObjectSpaceOperation.CreateValue<int>(null))
                ),

                //
                // Exceptions thrown by Get.
                //
                Operation.Sequence(
                    assert.ThrowsException<ArgumentNullException>().When(PersistedObjectSpaceOperation.GetValue<int>(null)),
                    assert.ThrowsException<KeyNotFoundException>().When(PersistedObjectSpaceOperation.GetValue<int>("bar"))
                ),

                //
                // Exceptions thrown by Delete.
                //
                Operation.Sequence(
                    assert.ThrowsException<KeyNotFoundException>().When(PersistedObjectSpaceOperation.Delete("bar"))
                ),

                //
                // Create a new value.
                //
                PersistedObjectSpaceOperation.CreateValue<int>("bar").Apply(value =>

                    //
                    // Check the new value is present in the object space.
                    //
                    Operation.Sequence(

                        //
                        // Assert we can get the same instance back using GetValue.
                        //
                        assert.AreSame(
                            PersistedObjectSpaceOperation.GetValue<int>("bar"),
                            value
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
                // Delete value.
                //
                PersistedObjectSpaceOperation.Delete("bar"),

                //
                // Check the value is no longer present in the object space.
                //
                Operation.Sequence(
                    assert.ThrowsException<KeyNotFoundException>().When(PersistedObjectSpaceOperation.GetValue<int>("bar")),
                    assert.ThrowsException<KeyNotFoundException>().When(PersistedObjectSpaceOperation.Delete("bar"))
                )
            );

        [TestMethod]
        public void Value_ManOrBoy()
        {
            var assert = AssertOperation.WithAssert(MsTestAssert.Instance);
            var space = new PersistedObjectSpace(new SerializationFactory());

            ManOrBoyCore(assert).Accept(space);
        }

        [TestMethod]
        public void Value_Volatile_ManOrBoy()
        {
            var assert = AssertOperation.WithAssert(MsTestAssert.Instance);
            var space = new VolatilePersistedObjectSpace();

            ManOrBoyCore(assert).Accept(space);
        }

        private static IOperation<IPersistedObjectSpace> ManOrBoyCore(IAssertOperationFactory assert) =>
            PersistedObjectSpaceOperation.CreateValue<int>("bar").Bind(value =>
                Operation.Sequence(

                    //
                    // Check the identifier
                    //
                    assert.AreEqual(
                        value.GetId(),
                        "bar"
                    ),

                    //
                    // Initialized with default value
                    //
                    assert.AreEqual(
                        value.Get(),
                        0
                    ),

                    //
                    // Perform assignment
                    //
                    value.Set(42),

                    //
                    // Check value after assignment
                    //
                    assert.AreEqual(
                        value.Get(),
                        42
                    )
                )
            );

        [TestMethod]
        public void Value_Recovery_CreateDefault() => WithNewSpace(s =>
        {
            //
            // Create a value.
            //

            _ = s.Space.CreateValue<int>("bar");

            //
            // Save the space to the store.
            //
            {
                var edits = s.SaveSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "value"),
                });
            }

            //
            // Create a new space.
            //

            s.CreateSpace();

            //
            // Assert the new space does not contain the value.
            //

            Assert.ThrowsException<KeyNotFoundException>(() => s.Space.GetValue<int>("bar"));

            //
            // Load the space from the store.
            //

            s.LoadSpace();

            //
            // Get the value.
            //

            var bar = s.Space.GetValue<int>("bar");

            //
            // Assert the value.
            //

            Assert.AreEqual(0, bar.Value);
        });

        [TestMethod]
        public void Value_Recovery_CreateValue() => WithNewSpace(s =>
        {
            //
            // Create a value.
            //
            {
                var value = s.Space.CreateValue<int>("bar");
                value.Value = 42;
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "value"),
                });
            }

            //
            // Get and assert the value.
            //
            {
                var value = s.Space.GetValue<int>("bar");
                Assert.AreEqual(42, value.Value);
            }
        });

        [TestMethod]
        public void Value_Recovery_NoChange() => WithNewSpace(s =>
        {
            //
            // Create a value.
            //
            {
                var value = s.Space.CreateValue<int>("bar");
                value.Value = 42;
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "value"),
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
            // Get and assert the value.
            //
            {
                var value = s.Space.GetValue<int>("bar");
                Assert.AreEqual(42, value.Value);
            }
        });

        [TestMethod]
        public void Value_Recovery_UpdateValue() => WithNewSpace(s =>
        {
            //
            // Create a value.
            //
            {
                var value = s.Space.CreateValue<int>("bar");
                value.Value = 42;
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "value"),
                });
            }

            //
            // Get and update the value.
            //
            {
                var value = s.Space.GetValue<int>("bar");
                value.Value = 43;
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "value"),
                });
            }

            //
            // Get and assert the value.
            //
            {
                var value = s.Space.GetValue<int>("bar");
                Assert.AreEqual(43, value.Value);
            }
        });

        [TestMethod]
        public void Value_Recovery_UpdateValue_Differential() => WithNewSpace(s =>
        {
            //
            // Create a value.
            //
            {
                var value = s.Space.CreateValue<int>("bar");
                value.Value = 42;
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "value"),
                });
            }

            //
            // Get and update the value.
            //
            {
                var value = s.Space.GetValue<int>("bar");
                value.Value = 43;
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "value"),
                });
            }

            //
            // Get and assert the value.
            //
            {
                var value = s.Space.GetValue<int>("bar");
                Assert.AreEqual(43, value.Value);
            }
        });

        [TestMethod]
        public void Value_Recovery_UpdateValue_Differential_SaveFailure_Once() => WithNewSpace(s =>
        {
            //
            // Create a value.
            //
            {
                var value = s.Space.CreateValue<int>("bar");
                value.Value = 42;
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "value"),
                });
            }

            //
            // Get and update the value.
            //
            {
                var value = s.Space.GetValue<int>("bar");
                value.Value = 43;
            }

            //
            // Fail to save the space.
            //
            s.SaveSpaceFail(differential: true);

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "value"),
                });
            }

            //
            // Get and assert the value.
            //
            {
                var value = s.Space.GetValue<int>("bar");
                Assert.AreEqual(43, value.Value);
            }
        });

        [TestMethod]
        public void Value_Recovery_UpdateValue_Differential_SaveFailure_Twice() => WithNewSpace(s =>
        {
            //
            // Create a value.
            //
            {
                var value = s.Space.CreateValue<int>("bar");
                value.Value = 42;
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "value"),
                });
            }

            //
            // Get and update the value.
            //
            {
                var value = s.Space.GetValue<int>("bar");
                value.Value = 43;
            }

            //
            // Fail to save the space.
            //
            s.SaveSpaceFail(differential: true);

            //
            // Get and update the value.
            //
            {
                var value = s.Space.GetValue<int>("bar");
                value.Value = 44;
            }

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: true);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "value"),
                });
            }

            //
            // Get and assert the value.
            //
            {
                var value = s.Space.GetValue<int>("bar");
                Assert.AreEqual(44, value.Value);
            }
        });

        [TestMethod]
        public void Value_Recovery_Delete() => WithNewSpace(s =>
        {
            //
            // Create a value.
            //
            _ = s.Space.CreateValue<int>("bar");

            //
            // Save the space to the store, create a new space, and load from the store.
            //
            {
                var edits = s.SaveAndReloadSpace(differential: false);

                s.AssertEdits(edits, new[]
                {
                    (StateWriterOperationKind.AddOrUpdate, "state/index", "bar"),
                    (StateWriterOperationKind.AddOrUpdate, "state/item/bar/data", "value"),
                });
            }

            //
            // Delete the value.
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
                    (StateWriterOperationKind.Delete, "state/item/bar/data", "value"),
                });
            }

            //
            // Assert the value is gone.
            //

            Assert.ThrowsException<KeyNotFoundException>(() => s.Space.GetValue<int>("bar"));
        });
    }
}
