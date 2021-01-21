// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using System;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtive.Storage;

using Reaqtor.QueryEngine;

using Utilities;

namespace Tests
{
    public class PersistedTestBase
    {
        protected static void WithSpace(Action<SpaceTest> action) => action(new SpaceTest());

        protected static void WithNewSpace(Action<SpaceTest> action)
        {
            var space = new SpaceTest();
            space.CreateSpace();
            action(space);
        }

        protected sealed class SpaceTest
        {
            public Store Store { get; } = new Store();

            public PersistedObjectSpace Space { get; private set; }

            public void CreateSpace() => Space = new PersistedObjectSpace(new SerializationFactory());

            public StateWriterOperation[] SaveSpace(bool differential)
            {
                //
                // NB: Full checkpoints only make sense to be applied to an empty store.
                //

                if (!differential)
                {
                    Store.Data.Clear();
                }

                using var writer = new StateWriter(Store, differential ? CheckpointKind.Differential : CheckpointKind.Full);
                using var logger = new LoggingStateWriter(writer, TextWriter.Null);

                Space.Save(logger);

                logger.CommitAsync().GetAwaiter().GetResult();

                Space.OnSaved();

                return writer.GetLog();
            }

            public void SaveSpaceFail(bool differential)
            {
                using var writer = new StateWriter(Store, differential ? CheckpointKind.Differential : CheckpointKind.Full);
                using var logger = new LoggingStateWriter(writer, TextWriter.Null);

                Space.Save(logger);
            }

            public void LoadSpace()
            {
                using var reader = new StateReader(Store);
                using var logger = new LoggingStateReader(reader, TextWriter.Null);

                Space.Load(logger);
            }

            public StateWriterOperation[] SaveAndReloadSpace(bool differential)
            {
                var log = SaveSpace(differential);

                CreateSpace();

                LoadSpace();

                return log;
            }

#pragma warning disable CA1822 // Mark members as static
            public void AssertEdits(StateWriterOperation[] actual, params (StateWriterOperationKind kind, string category, string key)[] expected)
            {
                Assert.AreEqual(expected.Length, actual.Length);

                for (var i = 0; i < actual.Length; i++)
                {
                    Assert.AreEqual(expected[i].kind, actual[i].Kind);
                    Assert.AreEqual(expected[i].category, actual[i].Category);
                    Assert.AreEqual(expected[i].key, actual[i].Key);
                }
            }
#pragma warning restore CA1822
        }
    }
}
