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
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    [TestClass]
    public class StateChangeManagerTests
    {
        [TestMethod]
        public void LoadState()
        {
            var s = new StateChangedManager();

            //
            // Not loaded from state.
            //
            Assert.IsTrue(s.StateChanged);

            //
            // Load state.
            //
            s.LoadState();

            //
            // Loaded from state.
            //
            Assert.IsFalse(s.StateChanged);
        }

        [TestMethod]
        public void LoadStateEdit()
        {
            var s = new StateChangedManager();

            //
            // Not loaded from state.
            //
            Assert.IsTrue(s.StateChanged);

            //
            // Load state.
            //
            s.LoadState();

            //
            // Loaded from state.
            //
            Assert.IsFalse(s.StateChanged);

            //
            // Edit state.
            //
            s.StateChanged = true;

            //
            // State is dirty.
            //
            Assert.IsTrue(s.StateChanged);
        }

        [TestMethod]
        public void SaveState()
        {
            var s = new StateChangedManager();

            //
            // Not loaded from state.
            //
            Assert.IsTrue(s.StateChanged);

            //
            // Save state.
            //
            s.SaveState();

            //
            // Saving state but not yet committed.
            //
            Assert.IsTrue(s.StateChanged);

            //
            // Successful commit.
            //
            s.OnStateSaved();

            //
            // State saved.
            //
            Assert.IsFalse(s.StateChanged);
        }

        [TestMethod]
        public void SaveStateEditAfterSaved()
        {
            var s = new StateChangedManager();

            //
            // Not loaded from state.
            //
            Assert.IsTrue(s.StateChanged);

            //
            // Save state.
            //
            s.SaveState();

            //
            // Saving state but not yet committed.
            //
            Assert.IsTrue(s.StateChanged);

            //
            // Successful commit.
            //
            s.OnStateSaved();

            //
            // State saved.
            //
            Assert.IsFalse(s.StateChanged);

            //
            // Edit state.
            //
            s.StateChanged = true;

            //
            // State is dirty.
            //
            Assert.IsTrue(s.StateChanged);
        }

        [TestMethod]
        public void SaveStateEditBeforeSaved()
        {
            var s = new StateChangedManager();

            //
            // Not loaded from state.
            //
            Assert.IsTrue(s.StateChanged);

            //
            // Save state.
            //
            s.SaveState();

            //
            // Saving state but not yet committed.
            //
            Assert.IsTrue(s.StateChanged);

            //
            // Edit state before commit.
            //
            s.StateChanged = true;

            //
            // Saving state but not yet committed and dirty again.
            //
            Assert.IsTrue(s.StateChanged);

            //
            // Successful commit.
            //
            s.OnStateSaved();

            //
            // State is dirty.
            //
            Assert.IsTrue(s.StateChanged);
        }

        [TestMethod]
        public void SaveStateEditFailToSave()
        {
            var s = new StateChangedManager();

            //
            // Not loaded from state.
            //
            Assert.IsTrue(s.StateChanged);

            //
            // Save state.
            //
            s.SaveState();

            //
            // Saving state but not yet committed.
            //
            Assert.IsTrue(s.StateChanged);

            //
            // Save state.
            //
            s.SaveState();

            //
            // Successful commit.
            //
            s.OnStateSaved();

            //
            // State saved.
            //
            Assert.IsFalse(s.StateChanged);

            //
            // Edit state.
            //
            s.StateChanged = true;

            //
            // State is dirty.
            //
            Assert.IsTrue(s.StateChanged);
        }

        [TestMethod]
        public void StateManagerStackBasics()
        {
            var s = new StateChangedManager<List<int>>();

            //
            // Empty edit page.
            //
            Assert.AreEqual(0, s.State.Count);

            //
            // Add to most recent edit page.
            //
            s.State.Add(42);

            //
            // Check most recent edit page.
            //
            Assert.IsTrue(new[] { 42 }.SequenceEqual(s.State));

            //
            // Enumerate edit pages.
            //
            Assert.IsTrue(new[] { 42 }.SequenceEqual(s.SelectMany(page => page)));

            //
            // Save state.
            //
            var save1 = s.SaveState();

            //
            // Snapshot reflects all edits.
            //
            Assert.IsTrue(new[] { 42 }.SequenceEqual(save1.SelectMany(page => page)));

            //
            // Enumerate edit pages; non-committed state is still there.
            //
            Assert.IsTrue(new[] { 42 }.SequenceEqual(s.SelectMany(page => page)));

            //
            // Most recent edit page is empty.
            //
            Assert.AreEqual(0, s.State.Count);

            //
            // Add to most recent edit page.
            //
            s.State.Add(43);

            //
            // Check most recent edit page.
            //
            Assert.IsTrue(new[] { 43 }.SequenceEqual(s.State));

            //
            // Enumerate edit pages; non-committed state is still there, prepended by the new edit (reverse chronological enumeration).
            //
            Assert.IsTrue(new[] { 43, 42 }.SequenceEqual(s.SelectMany(page => page)));

            //
            // Snapshot is immutable.
            //
            Assert.IsTrue(new[] { 42 }.SequenceEqual(save1.SelectMany(page => page)));

            //
            // State saved.
            //
            s.OnStateSaved();

            //
            // Check most recent edit page.
            //
            Assert.IsTrue(new[] { 43 }.SequenceEqual(s.State));

            //
            // Enumerate edit pages; committed state has been removed.
            //
            Assert.IsTrue(new[] { 43 }.SequenceEqual(s.SelectMany(page => page)));
        }
    }
}
