// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if !NET6_0
using System;
#endif
using System.Collections.Generic;
using System.Memory.Diagnostics;
using System.Runtime.CompilerServices;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class HeapEditorTests
    {
        [TestMethod]
        public void HeapEditor_Basics_Class()
        {
            var e = new MyEditor();

            var p = new Person("Bart", 21);
            e.Walk(p, _ => true);

            Assert.AreEqual("BART", p.Name);
        }

        [TestMethod]
        public void HeapEditor_Basics_Array()
        {
            var e = new MyEditor();

            var xs = new[] { "foo", "bar" };
            e.Walk(xs, _ => true);

            Assert.AreEqual("FOO", xs[0]);
            Assert.AreEqual("BAR", xs[1]);
        }

        [TestMethod]
        public void HeapEditor_Basics_MultidimensionalArray()
        {
            var e = new MyEditor();

            var xs = new string[2, 2] { { "foo", "bar" }, { "qux", "baz" } };
            e.Walk(xs, _ => true);

            Assert.AreEqual("FOO", xs[0, 0]);
            Assert.AreEqual("BAR", xs[0, 1]);
            Assert.AreEqual("QUX", xs[1, 0]);
            Assert.AreEqual("BAZ", xs[1, 1]);
        }

        [TestMethod]
        public void HeapEditor_Basics_Struct_Box()
        {
            var e = new MyEditor();

            var p = new StrongBox<KeyValuePair<string, int>>(new("Bart", 21));
            e.Walk(p, _ => true);

            Assert.AreEqual("BART", p.Value.Key);
        }

        [TestMethod]
        public void HeapEditor_Basics_Struct_Array()
        {
            var e = new MyEditor();

            var p = new KeyValuePair<string, int>[] { new("Bart", 21) };
            e.Walk(p, _ => true);

            Assert.AreEqual("BART", p[0].Key);
        }

        [TestMethod]
        public void HeapEditor_Basics_Struct_MultidimensionalArray()
        {
            var e = new MyEditor();

            var p = new KeyValuePair<string, int>[2, 2] { { new("Bart", 10), new("Lisa", 8) }, { new("Homer", 36), new("Marge", 34) } };
            e.Walk(p, _ => true);

            Assert.AreEqual("BART", p[0, 0].Key);
            Assert.AreEqual("LISA", p[0, 1].Key);
            Assert.AreEqual("HOMER", p[1, 0].Key);
            Assert.AreEqual("MARGE", p[1, 1].Key);
        }

        [TestMethod]
        public void HeapEditor_Basics_Struct_Nested1()
        {
            var e = new MyEditor();

            var p = new StrongBox<(int a, (string s, int b) t)>((0, ("foo", 1)));
            e.Walk(p, _ => true);

            Assert.AreEqual("FOO", p.Value.t.s);
        }

#if !NET6_0 // NB: Broken due to change to KeyValuePair<K, V> where fields are now read-only (where they weren't before). See test case below.
        [TestMethod]
        public void HeapEditor_Basics_Struct_Nested2()
        {
            if (Type.GetType("Mono.Runtime") != null)
            {
                Assert.Inconclusive("Issue on Mono to be investigated.");
                return;
            }

            var e = new MyEditor();

            var p = new StrongBox<KeyValuePair<int, KeyValuePair<string, int>>>(new(0, new("foo", 1)));
            e.Walk(p, _ => true);

            Assert.AreEqual("FOO", p.Value.Value.Key);
        }
#endif

#if TODO // NB: Write-backs in heap editor are shallow; this is a known limitation in the Pearl right now.
        [TestMethod]
        public void HeapEditor_Basics_Struct_Nested3()
        {
            var e = new MyEditor();

            var p = new StrongBox<Pair<int, Pair<string, int>>>(new(0, new("foo", 1)));
            e.Walk(p, _ => true);

            Assert.AreEqual("FOO", p.Value.Second.First);
        }
#endif

        private sealed class MyEditor : HeapEditor
        {
            public override object Edit(object obj)
            {
                if (obj is string s)
                {
                    return s.ToUpper();
                }

                return obj;
            }
        }

        private sealed class Person
        {
            public Person(string name, int age) => (Name, Age) = (name, age);

            public string Name { get; }
            public int Age { get; }
        }

        private readonly struct Pair<T1, T2>
        {
            public Pair(T1 t1, T2 t2) => (First, Second) = (t1, t2);

            public T1 First { get; }
            public T2 Second { get; }
        }
    }
}
