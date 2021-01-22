// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Reaqtive.Scheduler;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtive.Scheduler
{
    [TestClass]
    public class PriorityQueueTests
    {
        [TestMethod]
        public void HeapBasedPriorityQueue_ArgumentChecking()
        {
#pragma warning disable IDE0034 // Simplify 'default' expression (documents signature)
            Assert.ThrowsException<ArgumentNullException>(() => new HeapBasedPriorityQueue<int>(default(IComparer<int>)));
            Assert.ThrowsException<ArgumentNullException>(() => new HeapBasedPriorityQueue<int>(10, default(IComparer<int>)));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new HeapBasedPriorityQueue<int>(-1, Comparer<int>.Default));
#pragma warning restore IDE0034 // Simplify 'default' expression
        }

        [TestMethod]
        public void HeapBasedPriorityQueue_Simple()
        {
            var queue = new HeapBasedPriorityQueue<int>(Comparer<int>.Default);

            Assert.ThrowsException<InvalidOperationException>(() => queue.Peek());
            Assert.ThrowsException<InvalidOperationException>(() => queue.Dequeue());
            Assert.AreEqual(0, queue.Count);

            queue.Enqueue(2);
            queue.Enqueue(1);
            queue.Enqueue(3);
            queue.Enqueue(4);
            queue.Enqueue(5);

            Assert.AreEqual(5, queue.Count);
            Assert.AreEqual(1, queue.Peek());

            for (var i = 0; i <= 9; i++)
            {
                if (i is >= 1 and <= 5)
                {
                    Assert.IsTrue(queue.Contains(i));
                }
                else
                {
                    Assert.IsFalse(queue.Contains(i));
                }
            }

            for (var i = 1; i <= 5; i++)
            {
                Assert.AreEqual(i, queue.Dequeue());
            }

            for (var i = 0; i <= 9; i++)
            {
                Assert.IsFalse(queue.Contains(i));
            }

            Assert.ThrowsException<InvalidOperationException>(() => queue.Peek());
            Assert.ThrowsException<InvalidOperationException>(() => queue.Dequeue());
            Assert.AreEqual(0, queue.Count);
        }

        [TestMethod]
        public void HeapBasedPriorityQueue_Randomized_Simple()
        {
            var r = new Random(1983);

            for (int i = 0; i < 100; i++)
            {
                var n = r.Next(1000);
                var xs = Enumerable.Repeat(1, n).Select(_ => r.Next()).ToList();

                var queue = new HeapBasedPriorityQueue<int>(Comparer<int>.Default);

                var chi = queue.GetType().GetMethod("CheckHeapInvariant", BindingFlags.NonPublic | BindingFlags.Instance);
                var check = (Action)Delegate.CreateDelegate(typeof(Action), queue, chi);

                foreach (var x in xs)
                {
                    check();
                    queue.Enqueue(x);
                }

                var res = new List<int>();

                while (queue.Count > 0)
                {
                    check();
                    res.Add(queue.Dequeue());
                }

                check();

                Assert.IsTrue(xs.OrderBy(x => x).SequenceEqual(res));
            }
        }

        [TestMethod]
        public void HeapBasedPriorityQueue_Randomized_WithRemove()
        {
            var r = new Random(1983);

            for (int i = 0; i < 1000; i++)
            {
                var tw = new StringWriter();
                tw.WriteLine("var pq = new HeapBasedPriorityQueue<int>(Comparer<int>.Default);");

                var n = r.Next(200);
                var xs = Enumerable.Repeat(1, n).Select(_ => r.Next()).ToList();

                var added = new List<int>();

                var queue = new HeapBasedPriorityQueue<int>(Comparer<int>.Default);

                foreach (var x in xs)
                {
                    if (r.Next() % 2 == 0 && added.Count > 0)
                    {
                        var j = r.Next(0, added.Count);
                        var y = added[j];
                        added.RemoveAt(j);

                        tw.WriteLine("pq.Remove(" + y + ");");

                        queue.Remove(y);
                    }
                    else
                    {
                        tw.WriteLine("pq.Enqueue(" + x + ");");

                        queue.Enqueue(x);
                        added.Add(x);
                    }
                }

                var res = new List<int>();

                while (queue.Count > 0)
                {
                    res.Add(queue.Dequeue());
                }

                var exp = res.OrderBy(x => x).ToArray();
                var act = res.ToArray();

                Assert.IsTrue(exp.SequenceEqual(act), tw.ToString() + "\r\n" + string.Join(", ", exp) + " != " + string.Join(", ", act));
            }
        }

        [TestMethod]
        public void HeapBasedPriorityQueue_Repro1()
        {
            var pq = new HeapBasedPriorityQueue<int>(Comparer<int>.Default);
            pq.Enqueue(974933146);
            pq.Enqueue(75253661);
            pq.Enqueue(880866833);
            pq.Enqueue(603367200);
            pq.Enqueue(1331337961);
            pq.Enqueue(236372791);
            pq.Remove(880866833);
            pq.Remove(75253661);
            pq.Enqueue(1412090257);
            pq.Remove(1331337961);
            pq.Remove(1412090257);
            pq.Enqueue(867393504);
            pq.Enqueue(2096170085);
            pq.Remove(2096170085);
            pq.Remove(603367200);
            pq.Enqueue(1825362896);
            pq.Enqueue(3112452);
            pq.Enqueue(2006511400);
            pq.Enqueue(453748237);
            pq.Enqueue(1648837195);
            pq.Enqueue(1811421970);
            pq.Enqueue(974993209);
            pq.Enqueue(1665725470);
            pq.Enqueue(250575619);
            pq.Remove(250575619);
            pq.Enqueue(2077841019);
            pq.Remove(1811421970);
            pq.Enqueue(847419009);
            pq.Remove(3112452);
            pq.Remove(1648837195);
            pq.Enqueue(681881897);
            pq.Enqueue(1160946711);
            pq.Enqueue(1370828855);
            pq.Enqueue(227755064);
            pq.Remove(681881897);
            pq.Enqueue(243724529);
            pq.Remove(974993209);
            pq.Enqueue(1279230103);

            var res = new List<int>();
            while (pq.Count > 0)
            {
                res.Add(pq.Dequeue());
            }

            var exp = res.OrderBy(x => x).ToArray();
            var act = res.ToArray();

            Assert.IsTrue(exp.SequenceEqual(act), string.Join(", ", exp) + " != " + string.Join(", ", act));
        }

        [TestMethod]
        public void HeapBasedPriorityQueue_Repro2()
        {
            var pq = new HeapBasedPriorityQueue<int>(Comparer<int>.Default);
            pq.Enqueue(1431177679);
            pq.Enqueue(1849429681);
            pq.Enqueue(1751256899);
            pq.Remove(1849429681);
            pq.Enqueue(1069952473);
            pq.Remove(1751256899);
            pq.Enqueue(606339120);
            pq.Enqueue(660923900);
            pq.Remove(1069952473);
            pq.Enqueue(490792769);
            pq.Enqueue(1340157553);
            pq.Remove(660923900);
            pq.Remove(606339120);
            pq.Enqueue(56609777);
            pq.Remove(490792769);
            pq.Enqueue(932880312);
            pq.Enqueue(974984698);
            pq.Enqueue(768277902);
            pq.Remove(56609777);
            pq.Enqueue(642495331);
            pq.Remove(932880312);
            pq.Enqueue(276676877);
            pq.Enqueue(1686856785);
            pq.Enqueue(97220460);
            pq.Remove(97220460);
            pq.Enqueue(1820941261);
            pq.Enqueue(795007812);
            pq.Remove(768277902);
            pq.Remove(1686856785);
            pq.Remove(795007812);
            pq.Enqueue(1694483652);
            pq.Remove(1820941261);
            pq.Remove(1340157553);
            pq.Enqueue(137241414);
            pq.Enqueue(1508071593);
            pq.Enqueue(1389568860);
            pq.Enqueue(1703299212);
            pq.Enqueue(2009351709);
            pq.Enqueue(1956271747);
            pq.Remove(2009351709);
            pq.Enqueue(1375769249);
            pq.Remove(137241414);
            pq.Remove(642495331);
            pq.Enqueue(893442372);
            pq.Remove(1431177679);
            pq.Remove(1508071593);
            pq.Remove(1956271747);
            pq.Enqueue(1875720571);
            pq.Enqueue(672543699);
            pq.Enqueue(1704580922);
            pq.Remove(1694483652);
            pq.Enqueue(1675057635);
            pq.Remove(1704580922);
            pq.Enqueue(537590800);
            pq.Enqueue(740706366);
            pq.Enqueue(1154500802);
            pq.Enqueue(162572896);
            pq.Enqueue(1654036439);
            pq.Enqueue(1168362564);
            pq.Enqueue(1396494859);
            pq.Enqueue(69264986);
            pq.Enqueue(11085637);
            pq.Remove(1396494859);
            pq.Remove(893442372);
            pq.Remove(276676877);
            pq.Enqueue(1965081051);
            pq.Enqueue(310351301);
            pq.Remove(537590800);
            pq.Remove(1389568860);
            pq.Remove(162572896);
            pq.Enqueue(1377064747);
            pq.Remove(1965081051);
            pq.Remove(1375769249);
            pq.Enqueue(634977204);
            pq.Enqueue(1414209729);
            pq.Remove(974984698);
            pq.Remove(69264986);
            pq.Enqueue(793414413);
            pq.Remove(634977204);
            pq.Enqueue(2066152910);
            pq.Remove(1875720571);
            pq.Enqueue(1066770888);
            pq.Enqueue(2070193431);
            pq.Remove(2066152910);
            pq.Remove(1154500802);
            pq.Enqueue(19426017);
            pq.Remove(1675057635);
            pq.Remove(11085637);
            pq.Remove(740706366);
            pq.Enqueue(353570791);
            pq.Enqueue(1226995964);
            pq.Remove(353570791);
            pq.Enqueue(840989145);
            pq.Remove(2070193431);
            pq.Remove(19426017);
            pq.Enqueue(1364683612);
            pq.Enqueue(1046410302);
            pq.Enqueue(938708246);
            pq.Enqueue(2097133255);
            pq.Enqueue(405223749);
            pq.Enqueue(1828685152);
            pq.Remove(1364683612);
            pq.Remove(938708246);
            pq.Enqueue(1084353933);
            pq.Enqueue(327516175);
            pq.Remove(1828685152);
            pq.Remove(1046410302);
            pq.Enqueue(392453217);
            pq.Remove(1066770888);

            var res = new List<int>();
            while (pq.Count > 0)
            {
                res.Add(pq.Dequeue());
            }

            var exp = res.OrderBy(x => x).ToArray();
            var act = res.ToArray();

            Assert.IsTrue(exp.SequenceEqual(act), string.Join(", ", exp) + " != " + string.Join(", ", act));
        }

        [TestMethod]
        public void HeapBasedPriorityQueue_EnqueueDequeue_Remove_Generated()
        {
            {
                var pq = new HeapBasedPriorityQueue<int>(Comparer<int>.Default);
                pq.Enqueue(41);
                pq.Enqueue(80);
                pq.Enqueue(79);
                pq.Enqueue(67);
                pq.Enqueue(14);
                pq.Enqueue(0);
                pq.Enqueue(83);
                pq.Enqueue(24);
                pq.Remove(24);

                var res = new List<int>();
                while (pq.Count > 0)
                {
                    res.Add(pq.Dequeue());
                }

                Assert.IsTrue(new[] { 0, 14, 41, 67, 79, 80, 83 }.SequenceEqual(res));
            }

            {
                var pq = new HeapBasedPriorityQueue<int>(Comparer<int>.Default);
                pq.Enqueue(1);
                pq.Enqueue(50);
                pq.Enqueue(94);
                pq.Enqueue(97);
                pq.Enqueue(45);
                pq.Enqueue(55);
                pq.Remove(45);
                pq.Enqueue(97);

                var res = new List<int>();
                while (pq.Count > 0)
                {
                    res.Add(pq.Dequeue());
                }

                Assert.IsTrue(new[] { 1, 50, 55, 94, 97, 97 }.SequenceEqual(res));
            }

            {
                var pq = new HeapBasedPriorityQueue<int>(Comparer<int>.Default);
                pq.Enqueue(7);
                pq.Enqueue(19);
                pq.Enqueue(96);
                pq.Enqueue(23);
                pq.Remove(7);
                pq.Enqueue(52);
                pq.Enqueue(56);
                pq.Enqueue(68);
                pq.Remove(23);

                var res = new List<int>();
                while (pq.Count > 0)
                {
                    res.Add(pq.Dequeue());
                }

                Assert.IsTrue(new[] { 19, 52, 56, 68, 96 }.SequenceEqual(res));
            }

            {
                var pq = new HeapBasedPriorityQueue<int>(Comparer<int>.Default);
                pq.Enqueue(56);
                pq.Enqueue(52);
                pq.Enqueue(96);
                pq.Enqueue(21);
                pq.Enqueue(5);
                pq.Enqueue(24);
                pq.Enqueue(83);
                pq.Remove(21);
                pq.Enqueue(84);

                var res = new List<int>();
                while (pq.Count > 0)
                {
                    res.Add(pq.Dequeue());
                }

                Assert.IsTrue(new[] { 5, 24, 52, 56, 83, 84, 96 }.SequenceEqual(res));
            }

            {
                var pq = new HeapBasedPriorityQueue<int>(Comparer<int>.Default);
                pq.Enqueue(16);
                pq.Enqueue(51);
                pq.Enqueue(97);
                pq.Enqueue(42);
                pq.Enqueue(69);
                pq.Enqueue(63);
                pq.Enqueue(89);
                pq.Remove(42);
                pq.Enqueue(98);

                var res = new List<int>();
                while (pq.Count > 0)
                {
                    res.Add(pq.Dequeue());
                }

                Assert.IsTrue(new[] { 16, 51, 63, 69, 89, 97, 98 }.SequenceEqual(res));
            }

            {
                var pq = new HeapBasedPriorityQueue<int>(Comparer<int>.Default);
                pq.Enqueue(55);
                pq.Enqueue(52);
                pq.Enqueue(68);
                pq.Enqueue(3);
                pq.Enqueue(73);
                pq.Enqueue(90);
                pq.Remove(52);

                var res = new List<int>();
                while (pq.Count > 0)
                {
                    res.Add(pq.Dequeue());
                }

                Assert.IsTrue(new[] { 3, 55, 68, 73, 90 }.SequenceEqual(res));
            }

            {
                var pq = new HeapBasedPriorityQueue<int>(Comparer<int>.Default);
                pq.Enqueue(57);
                pq.Enqueue(29);
                pq.Enqueue(23);
                pq.Enqueue(22);
                pq.Enqueue(83);
                pq.Enqueue(74);
                pq.Remove(23);
                pq.Enqueue(85);

                var res = new List<int>();
                while (pq.Count > 0)
                {
                    res.Add(pq.Dequeue());
                }

                Assert.IsTrue(new[] { 22, 29, 57, 74, 83, 85 }.SequenceEqual(res));
            }

            {
                var pq = new HeapBasedPriorityQueue<int>(Comparer<int>.Default);
                pq.Enqueue(6);
                pq.Enqueue(8);
                pq.Enqueue(60);
                pq.Enqueue(27);
                pq.Enqueue(46);
                pq.Remove(8);
                pq.Enqueue(77);

                var res = new List<int>();
                while (pq.Count > 0)
                {
                    res.Add(pq.Dequeue());
                }

                Assert.IsTrue(new[] { 6, 27, 46, 60, 77 }.SequenceEqual(res));
            }

            {
                var pq = new HeapBasedPriorityQueue<int>(Comparer<int>.Default);
                pq.Enqueue(20);
                pq.Enqueue(51);
                pq.Enqueue(21);
                pq.Enqueue(42);
                pq.Enqueue(78);
                pq.Remove(21);
                pq.Enqueue(57);
                pq.Remove(42);
                pq.Enqueue(57);

                var res = new List<int>();
                while (pq.Count > 0)
                {
                    res.Add(pq.Dequeue());
                }

                Assert.IsTrue(new[] { 20, 51, 57, 57, 78 }.SequenceEqual(res));
            }

            {
                var pq = new HeapBasedPriorityQueue<int>(Comparer<int>.Default);
                pq.Enqueue(51);
                pq.Enqueue(32);
                pq.Enqueue(34);
                pq.Enqueue(32);
                pq.Enqueue(64);
                pq.Enqueue(18);
                pq.Enqueue(84);
                pq.Enqueue(54);
                pq.Remove(32);

                var res = new List<int>();
                while (pq.Count > 0)
                {
                    res.Add(pq.Dequeue());
                }

                Assert.IsTrue(new[] { 18, 32, 34, 51, 54, 64, 84 }.SequenceEqual(res));
            }
        }
    }
}
