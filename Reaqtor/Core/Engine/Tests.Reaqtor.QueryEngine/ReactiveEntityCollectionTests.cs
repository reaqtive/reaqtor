// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Reaqtor.QueryEngine;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor.QueryEngine
{
    [TestClass]
    public class ReactiveEntityCollectionTests
    {
        /// <remarks>
        /// This test was run prior to adding a lock to prevent removal during
        /// snapshot operations, and was found to fail.
        /// </remarks>
        [TestMethod]
        public void ReactiveEntityCollection_RemoveDuringClone()
        {
            var collection = new ReactiveEntityCollection<int, int>(EqualityComparer<int>.Default);
            var count = 100;
            var repeat = 1000;

            for (var i = 0; i < count; ++i)
            {
                Assert.IsTrue(collection.TryAdd(i, i * i));
            }

            var rand = new Random();
            for (var i = 0; i < repeat; ++i)
            {
                var cloned = default(ReadOnlyReactiveEntityCollection<int, int>);
                var target = rand.Next(count);

                var t1 = Task.Run(() =>
                {
                    cloned = collection.Clone();
                });

                var t2 = Task.Run(() =>
                {
                    collection.TryRemove(target, out var unused);
                });

                Task.WaitAll(t1, t2);

                foreach (var key in cloned.RemovedKeys)
                {
                    Assert.IsFalse(cloned.Entries.TryGetValue(key, out var unused));
                }
            }
        }
    }
}
