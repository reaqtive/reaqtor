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
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtor.QueryEngine;

using Utilities;

namespace Tests
{
    [TestClass]
    public class LoggingStateReaderTests
    {
        [TestMethod]
        public void LoggingStateReader_Basics()
        {
            var sb = new StringBuilder();

            using (var sw = new StringWriter(sb))
            {
                using var r = new MyReader();
                using var reader = new LoggingStateReader(r, sw);

                var cat = reader.GetCategories();
                CollectionAssert.AreEqual(new[] { "A" }, cat.ToList());

                var f1 = reader.TryGetItemKeys("foo", out var keys);
                Assert.IsTrue(f1);
                CollectionAssert.AreEqual(new[] { "B" }, keys.ToList());

                var f2 = reader.TryGetItemReader("foo", "bar", out var s);
                Assert.IsTrue(f2);
                Assert.IsNotNull(s);
            }

            var log = sb.ToString();

            foreach (var entry in new[]
            {
                "GetCategories()/Start",
                "GetCategories()/Stop",

                "TryGetItemKeys(foo)/Start",
                "TryGetItemKeys(foo)/Stop",

                "TryGetItemReader(foo, bar)/Start",
                "TryGetItemReader(foo, bar)/Stop",

                "Dispose()/Start",
                "Dispose()/Stop",
            })
            {
                Assert.IsTrue(log.Contains(entry), "Not found: '" + entry + "'");
            }
        }

        [TestMethod]
        public void LoggingStateReader_Errors()
        {
            var sb = new StringBuilder();

            using (var sw = new StringWriter(sb))
            {
                using var r = new MyReader { Throw = true };
                using var reader = new LoggingStateReader(r, sw);

                Assert.ThrowsException<NotImplementedException>(() => _ = reader.GetCategories());
                Assert.ThrowsException<NotImplementedException>(() => _ = reader.TryGetItemKeys("foo", out _));
                Assert.ThrowsException<NotImplementedException>(() => _ = reader.TryGetItemReader("foo", "bar", out _));
                Assert.ThrowsException<NotImplementedException>(() => reader.Dispose());

                r.Throw = false;
            }

            var log = sb.ToString();

            foreach (var entry in new[]
            {
                "GetCategories()/Start",
                "GetCategories()/Error",
                "GetCategories()/Stop",

                "TryGetItemKeys(foo)/Start",
                "TryGetItemKeys(foo)/Error",
                "TryGetItemKeys(foo)/Stop",

                "TryGetItemReader(foo, bar)/Start",
                "TryGetItemReader(foo, bar)/Error",
                "TryGetItemReader(foo, bar)/Stop",

                "Dispose()/Start",
                "Dispose()/Error",
                "Dispose()/Stop",
            })
            {
                Assert.IsTrue(log.Contains(entry), "Not found: '" + entry + "'");
            }
        }

        private sealed class MyReader : IStateReader
        {
            public bool Throw;

            public void Dispose()
            {
                if (Throw)
                    throw new NotImplementedException();
            }

            public IEnumerable<string> GetCategories()
            {
                if (Throw)
                    throw new NotImplementedException();

                return new[] { "A" };
            }

            public bool TryGetItemKeys(string category, out IEnumerable<string> keys)
            {
                if (Throw)
                    throw new NotImplementedException();

                keys = new[] { "B" };
                return true;
            }

            public bool TryGetItemReader(string category, string key, out Stream stream)
            {
                if (Throw)
                    throw new NotImplementedException();

                stream = new MemoryStream();
                return true;
            }
        }
    }
}
