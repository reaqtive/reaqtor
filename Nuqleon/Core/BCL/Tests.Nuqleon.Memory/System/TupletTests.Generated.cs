﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Memory;

namespace Tests
{
    partial class TupletTests
    {
        [TestMethod]
        public void Tuplet1()
        {
            var args1 = new Tuplet<int>(1493878331);
            var args2 = new Tuplet<int>(1431177679);

            Assert.AreEqual(1493878331, args1.Item1);
            Assert.AreEqual(1431177679, args2.Item1);

            Assert.AreEqual(args1, args1);

            var copy1 = args1;
            Assert.IsTrue(args1 == copy1);
            Assert.IsFalse(args1 != copy1);

            Assert.AreEqual(0, args1.CompareTo(copy1));

            Assert.AreNotEqual(args1.GetHashCode(), args2.GetHashCode());
            Assert.AreNotEqual(args1, args2);
            Assert.AreNotEqual(args2, args1);

            var scm1 = (IStructuralComparable)args1;
            var cmp1 = (IComparable)args1;

            Assert.AreNotEqual(0, args1.CompareTo(args2));
            Assert.AreNotEqual(0, scm1.CompareTo(args2, Comparer<object>.Default));
            Assert.AreNotEqual(0, cmp1.CompareTo(args2));
            Assert.AreEqual(1, scm1.CompareTo(other: null, Comparer<object>.Default));

            Assert.ThrowsException<ArgumentException>(() => scm1.CompareTo("foo", Comparer<object>.Default));

            Assert.IsTrue(args1.CompareTo(new Tuplet<int>(1493878332)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int>(1493878330)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int>(1493878332), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int>(1493878330), Comparer<object>.Default) > 0);

            Assert.IsFalse(args1 == args2);
            Assert.IsTrue(args1 != args2);

            Assert.AreNotEqual(args1, "foo");
            Assert.AreNotEqual(args1, null);

            var seq1 = (IStructuralEquatable)args1;
            Assert.AreEqual(args1.GetHashCode(), seq1.GetHashCode(EqualityComparer<object>.Default));
            Assert.AreEqual(0, seq1.GetHashCode(new ConstantHashEqualityComparer(0)));
            Assert.IsTrue(seq1.Equals(args1, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals(args2, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals("foo", EqualityComparer<object>.Default));

            Assert.AreEqual("(1493878331)", args1.ToString());
            Assert.AreEqual("(1431177679)", args2.ToString());
        }

        [TestMethod]
        public void Tuplet2()
        {
            var args1 = new Tuplet<int, int>(1849429681, 1751256899);
            var args2 = new Tuplet<int, int>(1078584633, 1069952473);

            Assert.AreEqual(1849429681, args1.Item1);
            Assert.AreEqual(1078584633, args2.Item1);
            Assert.AreEqual(1751256899, args1.Item2);
            Assert.AreEqual(1069952473, args2.Item2);

            Assert.AreEqual(args1, args1);

            var copy1 = args1;
            Assert.IsTrue(args1 == copy1);
            Assert.IsFalse(args1 != copy1);

            Assert.AreEqual(0, args1.CompareTo(copy1));

            Assert.AreNotEqual(args1.GetHashCode(), args2.GetHashCode());
            Assert.AreNotEqual(args1, args2);
            Assert.AreNotEqual(args2, args1);

            var scm1 = (IStructuralComparable)args1;
            var cmp1 = (IComparable)args1;

            Assert.AreNotEqual(0, args1.CompareTo(args2));
            Assert.AreNotEqual(0, scm1.CompareTo(args2, Comparer<object>.Default));
            Assert.AreNotEqual(0, cmp1.CompareTo(args2));
            Assert.AreEqual(1, scm1.CompareTo(other: null, Comparer<object>.Default));

            Assert.ThrowsException<ArgumentException>(() => scm1.CompareTo("foo", Comparer<object>.Default));

            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int>(1849429682, 1751256899)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int>(1849429680, 1751256899)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int>(1849429682, 1751256899), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int>(1849429680, 1751256899), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int>(1849429681, 1751256900)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int>(1849429681, 1751256898)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int>(1849429681, 1751256900), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int>(1849429681, 1751256898), Comparer<object>.Default) > 0);

            Assert.IsFalse(args1 == args2);
            Assert.IsTrue(args1 != args2);

            Assert.AreNotEqual(args1, "foo");
            Assert.AreNotEqual(args1, null);

            var seq1 = (IStructuralEquatable)args1;
            Assert.AreEqual(args1.GetHashCode(), seq1.GetHashCode(EqualityComparer<object>.Default));
            Assert.AreEqual(0, seq1.GetHashCode(new ConstantHashEqualityComparer(0)));
            Assert.IsTrue(seq1.Equals(args1, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals(args2, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals("foo", EqualityComparer<object>.Default));

            Assert.AreEqual("(1849429681, 1751256899)", args1.ToString());
            Assert.AreEqual("(1078584633, 1069952473)", args2.ToString());
        }

        [TestMethod]
        public void Tuplet3()
        {
            var args1 = new Tuplet<int, int, int>(864272798, 606339120, 660923900);
            var args2 = new Tuplet<int, int, int>(1089055706, 490792769, 1340157553);

            Assert.AreEqual(864272798, args1.Item1);
            Assert.AreEqual(1089055706, args2.Item1);
            Assert.AreEqual(606339120, args1.Item2);
            Assert.AreEqual(490792769, args2.Item2);
            Assert.AreEqual(660923900, args1.Item3);
            Assert.AreEqual(1340157553, args2.Item3);

            Assert.AreEqual(args1, args1);

            var copy1 = args1;
            Assert.IsTrue(args1 == copy1);
            Assert.IsFalse(args1 != copy1);

            Assert.AreEqual(0, args1.CompareTo(copy1));

            Assert.AreNotEqual(args1.GetHashCode(), args2.GetHashCode());
            Assert.AreNotEqual(args1, args2);
            Assert.AreNotEqual(args2, args1);

            var scm1 = (IStructuralComparable)args1;
            var cmp1 = (IComparable)args1;

            Assert.AreNotEqual(0, args1.CompareTo(args2));
            Assert.AreNotEqual(0, scm1.CompareTo(args2, Comparer<object>.Default));
            Assert.AreNotEqual(0, cmp1.CompareTo(args2));
            Assert.AreEqual(1, scm1.CompareTo(other: null, Comparer<object>.Default));

            Assert.ThrowsException<ArgumentException>(() => scm1.CompareTo("foo", Comparer<object>.Default));

            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int>(864272799, 606339120, 660923900)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int>(864272797, 606339120, 660923900)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int>(864272799, 606339120, 660923900), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int>(864272797, 606339120, 660923900), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int>(864272798, 606339121, 660923900)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int>(864272798, 606339119, 660923900)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int>(864272798, 606339121, 660923900), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int>(864272798, 606339119, 660923900), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int>(864272798, 606339120, 660923901)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int>(864272798, 606339120, 660923899)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int>(864272798, 606339120, 660923901), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int>(864272798, 606339120, 660923899), Comparer<object>.Default) > 0);

            Assert.IsFalse(args1 == args2);
            Assert.IsTrue(args1 != args2);

            Assert.AreNotEqual(args1, "foo");
            Assert.AreNotEqual(args1, null);

            var seq1 = (IStructuralEquatable)args1;
            Assert.AreEqual(args1.GetHashCode(), seq1.GetHashCode(EqualityComparer<object>.Default));
            Assert.AreEqual(0, seq1.GetHashCode(new ConstantHashEqualityComparer(0)));
            Assert.IsTrue(seq1.Equals(args1, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals(args2, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals("foo", EqualityComparer<object>.Default));

            Assert.AreEqual("(864272798, 606339120, 660923900)", args1.ToString());
            Assert.AreEqual("(1089055706, 490792769, 1340157553)", args2.ToString());
        }

        [TestMethod]
        public void Tuplet4()
        {
            var args1 = new Tuplet<int, int, int, int>(1821299733, 291436766, 56609777, 977758626);
            var args2 = new Tuplet<int, int, int, int>(932880312, 974984698, 768277902, 304671234);

            Assert.AreEqual(1821299733, args1.Item1);
            Assert.AreEqual(932880312, args2.Item1);
            Assert.AreEqual(291436766, args1.Item2);
            Assert.AreEqual(974984698, args2.Item2);
            Assert.AreEqual(56609777, args1.Item3);
            Assert.AreEqual(768277902, args2.Item3);
            Assert.AreEqual(977758626, args1.Item4);
            Assert.AreEqual(304671234, args2.Item4);

            Assert.AreEqual(args1, args1);

            var copy1 = args1;
            Assert.IsTrue(args1 == copy1);
            Assert.IsFalse(args1 != copy1);

            Assert.AreEqual(0, args1.CompareTo(copy1));

            Assert.AreNotEqual(args1.GetHashCode(), args2.GetHashCode());
            Assert.AreNotEqual(args1, args2);
            Assert.AreNotEqual(args2, args1);

            var scm1 = (IStructuralComparable)args1;
            var cmp1 = (IComparable)args1;

            Assert.AreNotEqual(0, args1.CompareTo(args2));
            Assert.AreNotEqual(0, scm1.CompareTo(args2, Comparer<object>.Default));
            Assert.AreNotEqual(0, cmp1.CompareTo(args2));
            Assert.AreEqual(1, scm1.CompareTo(other: null, Comparer<object>.Default));

            Assert.ThrowsException<ArgumentException>(() => scm1.CompareTo("foo", Comparer<object>.Default));

            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int>(1821299734, 291436766, 56609777, 977758626)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int>(1821299732, 291436766, 56609777, 977758626)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int>(1821299734, 291436766, 56609777, 977758626), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int>(1821299732, 291436766, 56609777, 977758626), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int>(1821299733, 291436767, 56609777, 977758626)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int>(1821299733, 291436765, 56609777, 977758626)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int>(1821299733, 291436767, 56609777, 977758626), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int>(1821299733, 291436765, 56609777, 977758626), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int>(1821299733, 291436766, 56609778, 977758626)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int>(1821299733, 291436766, 56609776, 977758626)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int>(1821299733, 291436766, 56609778, 977758626), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int>(1821299733, 291436766, 56609776, 977758626), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int>(1821299733, 291436766, 56609777, 977758627)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int>(1821299733, 291436766, 56609777, 977758625)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int>(1821299733, 291436766, 56609777, 977758627), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int>(1821299733, 291436766, 56609777, 977758625), Comparer<object>.Default) > 0);

            Assert.IsFalse(args1 == args2);
            Assert.IsTrue(args1 != args2);

            Assert.AreNotEqual(args1, "foo");
            Assert.AreNotEqual(args1, null);

            var seq1 = (IStructuralEquatable)args1;
            Assert.AreEqual(args1.GetHashCode(), seq1.GetHashCode(EqualityComparer<object>.Default));
            Assert.AreEqual(0, seq1.GetHashCode(new ConstantHashEqualityComparer(0)));
            Assert.IsTrue(seq1.Equals(args1, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals(args2, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals("foo", EqualityComparer<object>.Default));

            Assert.AreEqual("(1821299733, 291436766, 56609777, 977758626)", args1.ToString());
            Assert.AreEqual("(932880312, 974984698, 768277902, 304671234)", args2.ToString());
        }

        [TestMethod]
        public void Tuplet5()
        {
            var args1 = new Tuplet<int, int, int, int, int>(642495331, 753171965, 276676877, 1686856785, 97220460);
            var args2 = new Tuplet<int, int, int, int, int>(2057705716, 1820941261, 795007812, 595253483, 234458665);

            Assert.AreEqual(642495331, args1.Item1);
            Assert.AreEqual(2057705716, args2.Item1);
            Assert.AreEqual(753171965, args1.Item2);
            Assert.AreEqual(1820941261, args2.Item2);
            Assert.AreEqual(276676877, args1.Item3);
            Assert.AreEqual(795007812, args2.Item3);
            Assert.AreEqual(1686856785, args1.Item4);
            Assert.AreEqual(595253483, args2.Item4);
            Assert.AreEqual(97220460, args1.Item5);
            Assert.AreEqual(234458665, args2.Item5);

            Assert.AreEqual(args1, args1);

            var copy1 = args1;
            Assert.IsTrue(args1 == copy1);
            Assert.IsFalse(args1 != copy1);

            Assert.AreEqual(0, args1.CompareTo(copy1));

            Assert.AreNotEqual(args1.GetHashCode(), args2.GetHashCode());
            Assert.AreNotEqual(args1, args2);
            Assert.AreNotEqual(args2, args1);

            var scm1 = (IStructuralComparable)args1;
            var cmp1 = (IComparable)args1;

            Assert.AreNotEqual(0, args1.CompareTo(args2));
            Assert.AreNotEqual(0, scm1.CompareTo(args2, Comparer<object>.Default));
            Assert.AreNotEqual(0, cmp1.CompareTo(args2));
            Assert.AreEqual(1, scm1.CompareTo(other: null, Comparer<object>.Default));

            Assert.ThrowsException<ArgumentException>(() => scm1.CompareTo("foo", Comparer<object>.Default));

            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int>(642495332, 753171965, 276676877, 1686856785, 97220460)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int>(642495330, 753171965, 276676877, 1686856785, 97220460)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int>(642495332, 753171965, 276676877, 1686856785, 97220460), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int>(642495330, 753171965, 276676877, 1686856785, 97220460), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int>(642495331, 753171966, 276676877, 1686856785, 97220460)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int>(642495331, 753171964, 276676877, 1686856785, 97220460)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int>(642495331, 753171966, 276676877, 1686856785, 97220460), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int>(642495331, 753171964, 276676877, 1686856785, 97220460), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int>(642495331, 753171965, 276676878, 1686856785, 97220460)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int>(642495331, 753171965, 276676876, 1686856785, 97220460)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int>(642495331, 753171965, 276676878, 1686856785, 97220460), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int>(642495331, 753171965, 276676876, 1686856785, 97220460), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int>(642495331, 753171965, 276676877, 1686856786, 97220460)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int>(642495331, 753171965, 276676877, 1686856784, 97220460)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int>(642495331, 753171965, 276676877, 1686856786, 97220460), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int>(642495331, 753171965, 276676877, 1686856784, 97220460), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int>(642495331, 753171965, 276676877, 1686856785, 97220461)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int>(642495331, 753171965, 276676877, 1686856785, 97220459)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int>(642495331, 753171965, 276676877, 1686856785, 97220461), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int>(642495331, 753171965, 276676877, 1686856785, 97220459), Comparer<object>.Default) > 0);

            Assert.IsFalse(args1 == args2);
            Assert.IsTrue(args1 != args2);

            Assert.AreNotEqual(args1, "foo");
            Assert.AreNotEqual(args1, null);

            var seq1 = (IStructuralEquatable)args1;
            Assert.AreEqual(args1.GetHashCode(), seq1.GetHashCode(EqualityComparer<object>.Default));
            Assert.AreEqual(0, seq1.GetHashCode(new ConstantHashEqualityComparer(0)));
            Assert.IsTrue(seq1.Equals(args1, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals(args2, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals("foo", EqualityComparer<object>.Default));

            Assert.AreEqual("(642495331, 753171965, 276676877, 1686856785, 97220460)", args1.ToString());
            Assert.AreEqual("(2057705716, 1820941261, 795007812, 595253483, 234458665)", args2.ToString());
        }

        [TestMethod]
        public void Tuplet6()
        {
            var args1 = new Tuplet<int, int, int, int, int, int>(572489588, 1694483652, 1522560149, 1510948432, 137241414, 1508071593);
            var args2 = new Tuplet<int, int, int, int, int, int>(1389568860, 1703299212, 2009351709, 1956271747, 1817177677, 1375769249);

            Assert.AreEqual(572489588, args1.Item1);
            Assert.AreEqual(1389568860, args2.Item1);
            Assert.AreEqual(1694483652, args1.Item2);
            Assert.AreEqual(1703299212, args2.Item2);
            Assert.AreEqual(1522560149, args1.Item3);
            Assert.AreEqual(2009351709, args2.Item3);
            Assert.AreEqual(1510948432, args1.Item4);
            Assert.AreEqual(1956271747, args2.Item4);
            Assert.AreEqual(137241414, args1.Item5);
            Assert.AreEqual(1817177677, args2.Item5);
            Assert.AreEqual(1508071593, args1.Item6);
            Assert.AreEqual(1375769249, args2.Item6);

            Assert.AreEqual(args1, args1);

            var copy1 = args1;
            Assert.IsTrue(args1 == copy1);
            Assert.IsFalse(args1 != copy1);

            Assert.AreEqual(0, args1.CompareTo(copy1));

            Assert.AreNotEqual(args1.GetHashCode(), args2.GetHashCode());
            Assert.AreNotEqual(args1, args2);
            Assert.AreNotEqual(args2, args1);

            var scm1 = (IStructuralComparable)args1;
            var cmp1 = (IComparable)args1;

            Assert.AreNotEqual(0, args1.CompareTo(args2));
            Assert.AreNotEqual(0, scm1.CompareTo(args2, Comparer<object>.Default));
            Assert.AreNotEqual(0, cmp1.CompareTo(args2));
            Assert.AreEqual(1, scm1.CompareTo(other: null, Comparer<object>.Default));

            Assert.ThrowsException<ArgumentException>(() => scm1.CompareTo("foo", Comparer<object>.Default));

            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int>(572489589, 1694483652, 1522560149, 1510948432, 137241414, 1508071593)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int>(572489587, 1694483652, 1522560149, 1510948432, 137241414, 1508071593)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int>(572489589, 1694483652, 1522560149, 1510948432, 137241414, 1508071593), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int>(572489587, 1694483652, 1522560149, 1510948432, 137241414, 1508071593), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int>(572489588, 1694483653, 1522560149, 1510948432, 137241414, 1508071593)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int>(572489588, 1694483651, 1522560149, 1510948432, 137241414, 1508071593)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int>(572489588, 1694483653, 1522560149, 1510948432, 137241414, 1508071593), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int>(572489588, 1694483651, 1522560149, 1510948432, 137241414, 1508071593), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int>(572489588, 1694483652, 1522560150, 1510948432, 137241414, 1508071593)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int>(572489588, 1694483652, 1522560148, 1510948432, 137241414, 1508071593)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int>(572489588, 1694483652, 1522560150, 1510948432, 137241414, 1508071593), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int>(572489588, 1694483652, 1522560148, 1510948432, 137241414, 1508071593), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int>(572489588, 1694483652, 1522560149, 1510948433, 137241414, 1508071593)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int>(572489588, 1694483652, 1522560149, 1510948431, 137241414, 1508071593)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int>(572489588, 1694483652, 1522560149, 1510948433, 137241414, 1508071593), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int>(572489588, 1694483652, 1522560149, 1510948431, 137241414, 1508071593), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int>(572489588, 1694483652, 1522560149, 1510948432, 137241415, 1508071593)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int>(572489588, 1694483652, 1522560149, 1510948432, 137241413, 1508071593)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int>(572489588, 1694483652, 1522560149, 1510948432, 137241415, 1508071593), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int>(572489588, 1694483652, 1522560149, 1510948432, 137241413, 1508071593), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int>(572489588, 1694483652, 1522560149, 1510948432, 137241414, 1508071594)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int>(572489588, 1694483652, 1522560149, 1510948432, 137241414, 1508071592)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int>(572489588, 1694483652, 1522560149, 1510948432, 137241414, 1508071594), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int>(572489588, 1694483652, 1522560149, 1510948432, 137241414, 1508071592), Comparer<object>.Default) > 0);

            Assert.IsFalse(args1 == args2);
            Assert.IsTrue(args1 != args2);

            Assert.AreNotEqual(args1, "foo");
            Assert.AreNotEqual(args1, null);

            var seq1 = (IStructuralEquatable)args1;
            Assert.AreEqual(args1.GetHashCode(), seq1.GetHashCode(EqualityComparer<object>.Default));
            Assert.AreEqual(0, seq1.GetHashCode(new ConstantHashEqualityComparer(0)));
            Assert.IsTrue(seq1.Equals(args1, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals(args2, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals("foo", EqualityComparer<object>.Default));

            Assert.AreEqual("(572489588, 1694483652, 1522560149, 1510948432, 137241414, 1508071593)", args1.ToString());
            Assert.AreEqual("(1389568860, 1703299212, 2009351709, 1956271747, 1817177677, 1375769249)", args2.ToString());
        }

        [TestMethod]
        public void Tuplet7()
        {
            var args1 = new Tuplet<int, int, int, int, int, int, int>(1472875537, 1455274364, 893442372, 222821153, 2139036453, 1122443037, 1875720571);
            var args2 = new Tuplet<int, int, int, int, int, int, int>(672543699, 1704580922, 982962167, 1675057635, 1027430421, 537590800, 740706366);

            Assert.AreEqual(1472875537, args1.Item1);
            Assert.AreEqual(672543699, args2.Item1);
            Assert.AreEqual(1455274364, args1.Item2);
            Assert.AreEqual(1704580922, args2.Item2);
            Assert.AreEqual(893442372, args1.Item3);
            Assert.AreEqual(982962167, args2.Item3);
            Assert.AreEqual(222821153, args1.Item4);
            Assert.AreEqual(1675057635, args2.Item4);
            Assert.AreEqual(2139036453, args1.Item5);
            Assert.AreEqual(1027430421, args2.Item5);
            Assert.AreEqual(1122443037, args1.Item6);
            Assert.AreEqual(537590800, args2.Item6);
            Assert.AreEqual(1875720571, args1.Item7);
            Assert.AreEqual(740706366, args2.Item7);

            Assert.AreEqual(args1, args1);

            var copy1 = args1;
            Assert.IsTrue(args1 == copy1);
            Assert.IsFalse(args1 != copy1);

            Assert.AreEqual(0, args1.CompareTo(copy1));

            Assert.AreNotEqual(args1.GetHashCode(), args2.GetHashCode());
            Assert.AreNotEqual(args1, args2);
            Assert.AreNotEqual(args2, args1);

            var scm1 = (IStructuralComparable)args1;
            var cmp1 = (IComparable)args1;

            Assert.AreNotEqual(0, args1.CompareTo(args2));
            Assert.AreNotEqual(0, scm1.CompareTo(args2, Comparer<object>.Default));
            Assert.AreNotEqual(0, cmp1.CompareTo(args2));
            Assert.AreEqual(1, scm1.CompareTo(other: null, Comparer<object>.Default));

            Assert.ThrowsException<ArgumentException>(() => scm1.CompareTo("foo", Comparer<object>.Default));

            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int>(1472875538, 1455274364, 893442372, 222821153, 2139036453, 1122443037, 1875720571)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int>(1472875536, 1455274364, 893442372, 222821153, 2139036453, 1122443037, 1875720571)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int>(1472875538, 1455274364, 893442372, 222821153, 2139036453, 1122443037, 1875720571), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int>(1472875536, 1455274364, 893442372, 222821153, 2139036453, 1122443037, 1875720571), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int>(1472875537, 1455274365, 893442372, 222821153, 2139036453, 1122443037, 1875720571)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int>(1472875537, 1455274363, 893442372, 222821153, 2139036453, 1122443037, 1875720571)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int>(1472875537, 1455274365, 893442372, 222821153, 2139036453, 1122443037, 1875720571), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int>(1472875537, 1455274363, 893442372, 222821153, 2139036453, 1122443037, 1875720571), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int>(1472875537, 1455274364, 893442373, 222821153, 2139036453, 1122443037, 1875720571)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int>(1472875537, 1455274364, 893442371, 222821153, 2139036453, 1122443037, 1875720571)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int>(1472875537, 1455274364, 893442373, 222821153, 2139036453, 1122443037, 1875720571), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int>(1472875537, 1455274364, 893442371, 222821153, 2139036453, 1122443037, 1875720571), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int>(1472875537, 1455274364, 893442372, 222821154, 2139036453, 1122443037, 1875720571)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int>(1472875537, 1455274364, 893442372, 222821152, 2139036453, 1122443037, 1875720571)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int>(1472875537, 1455274364, 893442372, 222821154, 2139036453, 1122443037, 1875720571), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int>(1472875537, 1455274364, 893442372, 222821152, 2139036453, 1122443037, 1875720571), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int>(1472875537, 1455274364, 893442372, 222821153, 2139036454, 1122443037, 1875720571)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int>(1472875537, 1455274364, 893442372, 222821153, 2139036452, 1122443037, 1875720571)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int>(1472875537, 1455274364, 893442372, 222821153, 2139036454, 1122443037, 1875720571), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int>(1472875537, 1455274364, 893442372, 222821153, 2139036452, 1122443037, 1875720571), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int>(1472875537, 1455274364, 893442372, 222821153, 2139036453, 1122443038, 1875720571)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int>(1472875537, 1455274364, 893442372, 222821153, 2139036453, 1122443036, 1875720571)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int>(1472875537, 1455274364, 893442372, 222821153, 2139036453, 1122443038, 1875720571), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int>(1472875537, 1455274364, 893442372, 222821153, 2139036453, 1122443036, 1875720571), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int>(1472875537, 1455274364, 893442372, 222821153, 2139036453, 1122443037, 1875720572)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int>(1472875537, 1455274364, 893442372, 222821153, 2139036453, 1122443037, 1875720570)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int>(1472875537, 1455274364, 893442372, 222821153, 2139036453, 1122443037, 1875720572), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int>(1472875537, 1455274364, 893442372, 222821153, 2139036453, 1122443037, 1875720570), Comparer<object>.Default) > 0);

            Assert.IsFalse(args1 == args2);
            Assert.IsTrue(args1 != args2);

            Assert.AreNotEqual(args1, "foo");
            Assert.AreNotEqual(args1, null);

            var seq1 = (IStructuralEquatable)args1;
            Assert.AreEqual(args1.GetHashCode(), seq1.GetHashCode(EqualityComparer<object>.Default));
            Assert.AreEqual(0, seq1.GetHashCode(new ConstantHashEqualityComparer(0)));
            Assert.IsTrue(seq1.Equals(args1, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals(args2, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals("foo", EqualityComparer<object>.Default));

            Assert.AreEqual("(1472875537, 1455274364, 893442372, 222821153, 2139036453, 1122443037, 1875720571)", args1.ToString());
            Assert.AreEqual("(672543699, 1704580922, 982962167, 1675057635, 1027430421, 537590800, 740706366)", args2.ToString());
        }

        [TestMethod]
        public void Tuplet8()
        {
            var args1 = new Tuplet<int, int, int, int, int, int, int, int>(1154500802, 162572896, 1654036439, 1168362564, 1396494859, 69264986, 11085637, 426465235);
            var args2 = new Tuplet<int, int, int, int, int, int, int, int>(516566118, 943792764, 1965081051, 310351301, 154195352, 696021831, 1735673413, 1377064747);

            Assert.AreEqual(1154500802, args1.Item1);
            Assert.AreEqual(516566118, args2.Item1);
            Assert.AreEqual(162572896, args1.Item2);
            Assert.AreEqual(943792764, args2.Item2);
            Assert.AreEqual(1654036439, args1.Item3);
            Assert.AreEqual(1965081051, args2.Item3);
            Assert.AreEqual(1168362564, args1.Item4);
            Assert.AreEqual(310351301, args2.Item4);
            Assert.AreEqual(1396494859, args1.Item5);
            Assert.AreEqual(154195352, args2.Item5);
            Assert.AreEqual(69264986, args1.Item6);
            Assert.AreEqual(696021831, args2.Item6);
            Assert.AreEqual(11085637, args1.Item7);
            Assert.AreEqual(1735673413, args2.Item7);
            Assert.AreEqual(426465235, args1.Item8);
            Assert.AreEqual(1377064747, args2.Item8);

            Assert.AreEqual(args1, args1);

            var copy1 = args1;
            Assert.IsTrue(args1 == copy1);
            Assert.IsFalse(args1 != copy1);

            Assert.AreEqual(0, args1.CompareTo(copy1));

            Assert.AreNotEqual(args1.GetHashCode(), args2.GetHashCode());
            Assert.AreNotEqual(args1, args2);
            Assert.AreNotEqual(args2, args1);

            var scm1 = (IStructuralComparable)args1;
            var cmp1 = (IComparable)args1;

            Assert.AreNotEqual(0, args1.CompareTo(args2));
            Assert.AreNotEqual(0, scm1.CompareTo(args2, Comparer<object>.Default));
            Assert.AreNotEqual(0, cmp1.CompareTo(args2));
            Assert.AreEqual(1, scm1.CompareTo(other: null, Comparer<object>.Default));

            Assert.ThrowsException<ArgumentException>(() => scm1.CompareTo("foo", Comparer<object>.Default));

            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int>(1154500803, 162572896, 1654036439, 1168362564, 1396494859, 69264986, 11085637, 426465235)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int>(1154500801, 162572896, 1654036439, 1168362564, 1396494859, 69264986, 11085637, 426465235)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int>(1154500803, 162572896, 1654036439, 1168362564, 1396494859, 69264986, 11085637, 426465235), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int>(1154500801, 162572896, 1654036439, 1168362564, 1396494859, 69264986, 11085637, 426465235), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int>(1154500802, 162572897, 1654036439, 1168362564, 1396494859, 69264986, 11085637, 426465235)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int>(1154500802, 162572895, 1654036439, 1168362564, 1396494859, 69264986, 11085637, 426465235)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int>(1154500802, 162572897, 1654036439, 1168362564, 1396494859, 69264986, 11085637, 426465235), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int>(1154500802, 162572895, 1654036439, 1168362564, 1396494859, 69264986, 11085637, 426465235), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int>(1154500802, 162572896, 1654036440, 1168362564, 1396494859, 69264986, 11085637, 426465235)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int>(1154500802, 162572896, 1654036438, 1168362564, 1396494859, 69264986, 11085637, 426465235)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int>(1154500802, 162572896, 1654036440, 1168362564, 1396494859, 69264986, 11085637, 426465235), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int>(1154500802, 162572896, 1654036438, 1168362564, 1396494859, 69264986, 11085637, 426465235), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int>(1154500802, 162572896, 1654036439, 1168362565, 1396494859, 69264986, 11085637, 426465235)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int>(1154500802, 162572896, 1654036439, 1168362563, 1396494859, 69264986, 11085637, 426465235)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int>(1154500802, 162572896, 1654036439, 1168362565, 1396494859, 69264986, 11085637, 426465235), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int>(1154500802, 162572896, 1654036439, 1168362563, 1396494859, 69264986, 11085637, 426465235), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int>(1154500802, 162572896, 1654036439, 1168362564, 1396494860, 69264986, 11085637, 426465235)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int>(1154500802, 162572896, 1654036439, 1168362564, 1396494858, 69264986, 11085637, 426465235)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int>(1154500802, 162572896, 1654036439, 1168362564, 1396494860, 69264986, 11085637, 426465235), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int>(1154500802, 162572896, 1654036439, 1168362564, 1396494858, 69264986, 11085637, 426465235), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int>(1154500802, 162572896, 1654036439, 1168362564, 1396494859, 69264987, 11085637, 426465235)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int>(1154500802, 162572896, 1654036439, 1168362564, 1396494859, 69264985, 11085637, 426465235)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int>(1154500802, 162572896, 1654036439, 1168362564, 1396494859, 69264987, 11085637, 426465235), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int>(1154500802, 162572896, 1654036439, 1168362564, 1396494859, 69264985, 11085637, 426465235), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int>(1154500802, 162572896, 1654036439, 1168362564, 1396494859, 69264986, 11085638, 426465235)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int>(1154500802, 162572896, 1654036439, 1168362564, 1396494859, 69264986, 11085636, 426465235)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int>(1154500802, 162572896, 1654036439, 1168362564, 1396494859, 69264986, 11085638, 426465235), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int>(1154500802, 162572896, 1654036439, 1168362564, 1396494859, 69264986, 11085636, 426465235), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int>(1154500802, 162572896, 1654036439, 1168362564, 1396494859, 69264986, 11085637, 426465236)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int>(1154500802, 162572896, 1654036439, 1168362564, 1396494859, 69264986, 11085637, 426465234)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int>(1154500802, 162572896, 1654036439, 1168362564, 1396494859, 69264986, 11085637, 426465236), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int>(1154500802, 162572896, 1654036439, 1168362564, 1396494859, 69264986, 11085637, 426465234), Comparer<object>.Default) > 0);

            Assert.IsFalse(args1 == args2);
            Assert.IsTrue(args1 != args2);

            Assert.AreNotEqual(args1, "foo");
            Assert.AreNotEqual(args1, null);

            var seq1 = (IStructuralEquatable)args1;
            Assert.AreEqual(args1.GetHashCode(), seq1.GetHashCode(EqualityComparer<object>.Default));
            Assert.AreEqual(0, seq1.GetHashCode(new ConstantHashEqualityComparer(0)));
            Assert.IsTrue(seq1.Equals(args1, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals(args2, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals("foo", EqualityComparer<object>.Default));

            Assert.AreEqual("(1154500802, 162572896, 1654036439, 1168362564, 1396494859, 69264986, 11085637, 426465235)", args1.ToString());
            Assert.AreEqual("(516566118, 943792764, 1965081051, 310351301, 154195352, 696021831, 1735673413, 1377064747)", args2.ToString());
        }

        [TestMethod]
        public void Tuplet9()
        {
            var args1 = new Tuplet<int, int, int, int, int, int, int, int, int>(1113116636, 959489802, 634977204, 1414209729, 1427780075, 968886160, 793414413, 2021882954, 2066152910);
            var args2 = new Tuplet<int, int, int, int, int, int, int, int, int>(698498224, 1066770888, 2070193431, 677361390, 1737011068, 19426017, 495129728, 973357632, 1544018695);

            Assert.AreEqual(1113116636, args1.Item1);
            Assert.AreEqual(698498224, args2.Item1);
            Assert.AreEqual(959489802, args1.Item2);
            Assert.AreEqual(1066770888, args2.Item2);
            Assert.AreEqual(634977204, args1.Item3);
            Assert.AreEqual(2070193431, args2.Item3);
            Assert.AreEqual(1414209729, args1.Item4);
            Assert.AreEqual(677361390, args2.Item4);
            Assert.AreEqual(1427780075, args1.Item5);
            Assert.AreEqual(1737011068, args2.Item5);
            Assert.AreEqual(968886160, args1.Item6);
            Assert.AreEqual(19426017, args2.Item6);
            Assert.AreEqual(793414413, args1.Item7);
            Assert.AreEqual(495129728, args2.Item7);
            Assert.AreEqual(2021882954, args1.Item8);
            Assert.AreEqual(973357632, args2.Item8);
            Assert.AreEqual(2066152910, args1.Item9);
            Assert.AreEqual(1544018695, args2.Item9);

            Assert.AreEqual(args1, args1);

            var copy1 = args1;
            Assert.IsTrue(args1 == copy1);
            Assert.IsFalse(args1 != copy1);

            Assert.AreEqual(0, args1.CompareTo(copy1));

            Assert.AreNotEqual(args1.GetHashCode(), args2.GetHashCode());
            Assert.AreNotEqual(args1, args2);
            Assert.AreNotEqual(args2, args1);

            var scm1 = (IStructuralComparable)args1;
            var cmp1 = (IComparable)args1;

            Assert.AreNotEqual(0, args1.CompareTo(args2));
            Assert.AreNotEqual(0, scm1.CompareTo(args2, Comparer<object>.Default));
            Assert.AreNotEqual(0, cmp1.CompareTo(args2));
            Assert.AreEqual(1, scm1.CompareTo(other: null, Comparer<object>.Default));

            Assert.ThrowsException<ArgumentException>(() => scm1.CompareTo("foo", Comparer<object>.Default));

            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int>(1113116637, 959489802, 634977204, 1414209729, 1427780075, 968886160, 793414413, 2021882954, 2066152910)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int>(1113116635, 959489802, 634977204, 1414209729, 1427780075, 968886160, 793414413, 2021882954, 2066152910)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int>(1113116637, 959489802, 634977204, 1414209729, 1427780075, 968886160, 793414413, 2021882954, 2066152910), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int>(1113116635, 959489802, 634977204, 1414209729, 1427780075, 968886160, 793414413, 2021882954, 2066152910), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int>(1113116636, 959489803, 634977204, 1414209729, 1427780075, 968886160, 793414413, 2021882954, 2066152910)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int>(1113116636, 959489801, 634977204, 1414209729, 1427780075, 968886160, 793414413, 2021882954, 2066152910)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int>(1113116636, 959489803, 634977204, 1414209729, 1427780075, 968886160, 793414413, 2021882954, 2066152910), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int>(1113116636, 959489801, 634977204, 1414209729, 1427780075, 968886160, 793414413, 2021882954, 2066152910), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int>(1113116636, 959489802, 634977205, 1414209729, 1427780075, 968886160, 793414413, 2021882954, 2066152910)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int>(1113116636, 959489802, 634977203, 1414209729, 1427780075, 968886160, 793414413, 2021882954, 2066152910)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int>(1113116636, 959489802, 634977205, 1414209729, 1427780075, 968886160, 793414413, 2021882954, 2066152910), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int>(1113116636, 959489802, 634977203, 1414209729, 1427780075, 968886160, 793414413, 2021882954, 2066152910), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int>(1113116636, 959489802, 634977204, 1414209730, 1427780075, 968886160, 793414413, 2021882954, 2066152910)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int>(1113116636, 959489802, 634977204, 1414209728, 1427780075, 968886160, 793414413, 2021882954, 2066152910)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int>(1113116636, 959489802, 634977204, 1414209730, 1427780075, 968886160, 793414413, 2021882954, 2066152910), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int>(1113116636, 959489802, 634977204, 1414209728, 1427780075, 968886160, 793414413, 2021882954, 2066152910), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int>(1113116636, 959489802, 634977204, 1414209729, 1427780076, 968886160, 793414413, 2021882954, 2066152910)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int>(1113116636, 959489802, 634977204, 1414209729, 1427780074, 968886160, 793414413, 2021882954, 2066152910)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int>(1113116636, 959489802, 634977204, 1414209729, 1427780076, 968886160, 793414413, 2021882954, 2066152910), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int>(1113116636, 959489802, 634977204, 1414209729, 1427780074, 968886160, 793414413, 2021882954, 2066152910), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int>(1113116636, 959489802, 634977204, 1414209729, 1427780075, 968886161, 793414413, 2021882954, 2066152910)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int>(1113116636, 959489802, 634977204, 1414209729, 1427780075, 968886159, 793414413, 2021882954, 2066152910)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int>(1113116636, 959489802, 634977204, 1414209729, 1427780075, 968886161, 793414413, 2021882954, 2066152910), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int>(1113116636, 959489802, 634977204, 1414209729, 1427780075, 968886159, 793414413, 2021882954, 2066152910), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int>(1113116636, 959489802, 634977204, 1414209729, 1427780075, 968886160, 793414414, 2021882954, 2066152910)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int>(1113116636, 959489802, 634977204, 1414209729, 1427780075, 968886160, 793414412, 2021882954, 2066152910)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int>(1113116636, 959489802, 634977204, 1414209729, 1427780075, 968886160, 793414414, 2021882954, 2066152910), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int>(1113116636, 959489802, 634977204, 1414209729, 1427780075, 968886160, 793414412, 2021882954, 2066152910), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int>(1113116636, 959489802, 634977204, 1414209729, 1427780075, 968886160, 793414413, 2021882955, 2066152910)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int>(1113116636, 959489802, 634977204, 1414209729, 1427780075, 968886160, 793414413, 2021882953, 2066152910)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int>(1113116636, 959489802, 634977204, 1414209729, 1427780075, 968886160, 793414413, 2021882955, 2066152910), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int>(1113116636, 959489802, 634977204, 1414209729, 1427780075, 968886160, 793414413, 2021882953, 2066152910), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int>(1113116636, 959489802, 634977204, 1414209729, 1427780075, 968886160, 793414413, 2021882954, 2066152911)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int>(1113116636, 959489802, 634977204, 1414209729, 1427780075, 968886160, 793414413, 2021882954, 2066152909)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int>(1113116636, 959489802, 634977204, 1414209729, 1427780075, 968886160, 793414413, 2021882954, 2066152911), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int>(1113116636, 959489802, 634977204, 1414209729, 1427780075, 968886160, 793414413, 2021882954, 2066152909), Comparer<object>.Default) > 0);

            Assert.IsFalse(args1 == args2);
            Assert.IsTrue(args1 != args2);

            Assert.AreNotEqual(args1, "foo");
            Assert.AreNotEqual(args1, null);

            var seq1 = (IStructuralEquatable)args1;
            Assert.AreEqual(args1.GetHashCode(), seq1.GetHashCode(EqualityComparer<object>.Default));
            Assert.AreEqual(0, seq1.GetHashCode(new ConstantHashEqualityComparer(0)));
            Assert.IsTrue(seq1.Equals(args1, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals(args2, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals("foo", EqualityComparer<object>.Default));

            Assert.AreEqual("(1113116636, 959489802, 634977204, 1414209729, 1427780075, 968886160, 793414413, 2021882954, 2066152910)", args1.ToString());
            Assert.AreEqual("(698498224, 1066770888, 2070193431, 677361390, 1737011068, 19426017, 495129728, 973357632, 1544018695)", args2.ToString());
        }

        [TestMethod]
        public void Tuplet10()
        {
            var args1 = new Tuplet<int, int, int, int, int, int, int, int, int, int>(353570791, 1226995964, 49262773, 840989145, 559776888, 1747912691, 1364683612, 1046410302, 938708246, 2097133255);
            var args2 = new Tuplet<int, int, int, int, int, int, int, int, int, int>(405223749, 1828685152, 968247685, 1179698740, 1084353933, 327516175, 2017329178, 715567833, 392453217, 1270864718);

            Assert.AreEqual(353570791, args1.Item1);
            Assert.AreEqual(405223749, args2.Item1);
            Assert.AreEqual(1226995964, args1.Item2);
            Assert.AreEqual(1828685152, args2.Item2);
            Assert.AreEqual(49262773, args1.Item3);
            Assert.AreEqual(968247685, args2.Item3);
            Assert.AreEqual(840989145, args1.Item4);
            Assert.AreEqual(1179698740, args2.Item4);
            Assert.AreEqual(559776888, args1.Item5);
            Assert.AreEqual(1084353933, args2.Item5);
            Assert.AreEqual(1747912691, args1.Item6);
            Assert.AreEqual(327516175, args2.Item6);
            Assert.AreEqual(1364683612, args1.Item7);
            Assert.AreEqual(2017329178, args2.Item7);
            Assert.AreEqual(1046410302, args1.Item8);
            Assert.AreEqual(715567833, args2.Item8);
            Assert.AreEqual(938708246, args1.Item9);
            Assert.AreEqual(392453217, args2.Item9);
            Assert.AreEqual(2097133255, args1.Item10);
            Assert.AreEqual(1270864718, args2.Item10);

            Assert.AreEqual(args1, args1);

            var copy1 = args1;
            Assert.IsTrue(args1 == copy1);
            Assert.IsFalse(args1 != copy1);

            Assert.AreEqual(0, args1.CompareTo(copy1));

            Assert.AreNotEqual(args1.GetHashCode(), args2.GetHashCode());
            Assert.AreNotEqual(args1, args2);
            Assert.AreNotEqual(args2, args1);

            var scm1 = (IStructuralComparable)args1;
            var cmp1 = (IComparable)args1;

            Assert.AreNotEqual(0, args1.CompareTo(args2));
            Assert.AreNotEqual(0, scm1.CompareTo(args2, Comparer<object>.Default));
            Assert.AreNotEqual(0, cmp1.CompareTo(args2));
            Assert.AreEqual(1, scm1.CompareTo(other: null, Comparer<object>.Default));

            Assert.ThrowsException<ArgumentException>(() => scm1.CompareTo("foo", Comparer<object>.Default));

            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int>(353570792, 1226995964, 49262773, 840989145, 559776888, 1747912691, 1364683612, 1046410302, 938708246, 2097133255)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int>(353570790, 1226995964, 49262773, 840989145, 559776888, 1747912691, 1364683612, 1046410302, 938708246, 2097133255)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int>(353570792, 1226995964, 49262773, 840989145, 559776888, 1747912691, 1364683612, 1046410302, 938708246, 2097133255), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int>(353570790, 1226995964, 49262773, 840989145, 559776888, 1747912691, 1364683612, 1046410302, 938708246, 2097133255), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int>(353570791, 1226995965, 49262773, 840989145, 559776888, 1747912691, 1364683612, 1046410302, 938708246, 2097133255)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int>(353570791, 1226995963, 49262773, 840989145, 559776888, 1747912691, 1364683612, 1046410302, 938708246, 2097133255)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int>(353570791, 1226995965, 49262773, 840989145, 559776888, 1747912691, 1364683612, 1046410302, 938708246, 2097133255), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int>(353570791, 1226995963, 49262773, 840989145, 559776888, 1747912691, 1364683612, 1046410302, 938708246, 2097133255), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int>(353570791, 1226995964, 49262774, 840989145, 559776888, 1747912691, 1364683612, 1046410302, 938708246, 2097133255)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int>(353570791, 1226995964, 49262772, 840989145, 559776888, 1747912691, 1364683612, 1046410302, 938708246, 2097133255)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int>(353570791, 1226995964, 49262774, 840989145, 559776888, 1747912691, 1364683612, 1046410302, 938708246, 2097133255), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int>(353570791, 1226995964, 49262772, 840989145, 559776888, 1747912691, 1364683612, 1046410302, 938708246, 2097133255), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int>(353570791, 1226995964, 49262773, 840989146, 559776888, 1747912691, 1364683612, 1046410302, 938708246, 2097133255)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int>(353570791, 1226995964, 49262773, 840989144, 559776888, 1747912691, 1364683612, 1046410302, 938708246, 2097133255)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int>(353570791, 1226995964, 49262773, 840989146, 559776888, 1747912691, 1364683612, 1046410302, 938708246, 2097133255), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int>(353570791, 1226995964, 49262773, 840989144, 559776888, 1747912691, 1364683612, 1046410302, 938708246, 2097133255), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int>(353570791, 1226995964, 49262773, 840989145, 559776889, 1747912691, 1364683612, 1046410302, 938708246, 2097133255)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int>(353570791, 1226995964, 49262773, 840989145, 559776887, 1747912691, 1364683612, 1046410302, 938708246, 2097133255)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int>(353570791, 1226995964, 49262773, 840989145, 559776889, 1747912691, 1364683612, 1046410302, 938708246, 2097133255), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int>(353570791, 1226995964, 49262773, 840989145, 559776887, 1747912691, 1364683612, 1046410302, 938708246, 2097133255), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int>(353570791, 1226995964, 49262773, 840989145, 559776888, 1747912692, 1364683612, 1046410302, 938708246, 2097133255)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int>(353570791, 1226995964, 49262773, 840989145, 559776888, 1747912690, 1364683612, 1046410302, 938708246, 2097133255)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int>(353570791, 1226995964, 49262773, 840989145, 559776888, 1747912692, 1364683612, 1046410302, 938708246, 2097133255), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int>(353570791, 1226995964, 49262773, 840989145, 559776888, 1747912690, 1364683612, 1046410302, 938708246, 2097133255), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int>(353570791, 1226995964, 49262773, 840989145, 559776888, 1747912691, 1364683613, 1046410302, 938708246, 2097133255)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int>(353570791, 1226995964, 49262773, 840989145, 559776888, 1747912691, 1364683611, 1046410302, 938708246, 2097133255)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int>(353570791, 1226995964, 49262773, 840989145, 559776888, 1747912691, 1364683613, 1046410302, 938708246, 2097133255), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int>(353570791, 1226995964, 49262773, 840989145, 559776888, 1747912691, 1364683611, 1046410302, 938708246, 2097133255), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int>(353570791, 1226995964, 49262773, 840989145, 559776888, 1747912691, 1364683612, 1046410303, 938708246, 2097133255)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int>(353570791, 1226995964, 49262773, 840989145, 559776888, 1747912691, 1364683612, 1046410301, 938708246, 2097133255)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int>(353570791, 1226995964, 49262773, 840989145, 559776888, 1747912691, 1364683612, 1046410303, 938708246, 2097133255), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int>(353570791, 1226995964, 49262773, 840989145, 559776888, 1747912691, 1364683612, 1046410301, 938708246, 2097133255), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int>(353570791, 1226995964, 49262773, 840989145, 559776888, 1747912691, 1364683612, 1046410302, 938708247, 2097133255)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int>(353570791, 1226995964, 49262773, 840989145, 559776888, 1747912691, 1364683612, 1046410302, 938708245, 2097133255)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int>(353570791, 1226995964, 49262773, 840989145, 559776888, 1747912691, 1364683612, 1046410302, 938708247, 2097133255), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int>(353570791, 1226995964, 49262773, 840989145, 559776888, 1747912691, 1364683612, 1046410302, 938708245, 2097133255), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int>(353570791, 1226995964, 49262773, 840989145, 559776888, 1747912691, 1364683612, 1046410302, 938708246, 2097133256)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int>(353570791, 1226995964, 49262773, 840989145, 559776888, 1747912691, 1364683612, 1046410302, 938708246, 2097133254)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int>(353570791, 1226995964, 49262773, 840989145, 559776888, 1747912691, 1364683612, 1046410302, 938708246, 2097133256), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int>(353570791, 1226995964, 49262773, 840989145, 559776888, 1747912691, 1364683612, 1046410302, 938708246, 2097133254), Comparer<object>.Default) > 0);

            Assert.IsFalse(args1 == args2);
            Assert.IsTrue(args1 != args2);

            Assert.AreNotEqual(args1, "foo");
            Assert.AreNotEqual(args1, null);

            var seq1 = (IStructuralEquatable)args1;
            Assert.AreEqual(args1.GetHashCode(), seq1.GetHashCode(EqualityComparer<object>.Default));
            Assert.AreEqual(0, seq1.GetHashCode(new ConstantHashEqualityComparer(0)));
            Assert.IsTrue(seq1.Equals(args1, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals(args2, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals("foo", EqualityComparer<object>.Default));

            Assert.AreEqual("(353570791, 1226995964, 49262773, 840989145, 559776888, 1747912691, 1364683612, 1046410302, 938708246, 2097133255)", args1.ToString());
            Assert.AreEqual("(405223749, 1828685152, 968247685, 1179698740, 1084353933, 327516175, 2017329178, 715567833, 392453217, 1270864718)", args2.ToString());
        }

        [TestMethod]
        public void Tuplet11()
        {
            var args1 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1460409938, 185614642, 1516642130, 1779637132, 1249693301, 697996635, 1149977745, 88375853, 1896587492, 927038697, 924366747);
            var args2 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1469951323, 1484477316, 757660304, 342451040, 508677449, 1327801974, 272127491, 399712914, 1034548160, 49526117, 381369773);

            Assert.AreEqual(1460409938, args1.Item1);
            Assert.AreEqual(1469951323, args2.Item1);
            Assert.AreEqual(185614642, args1.Item2);
            Assert.AreEqual(1484477316, args2.Item2);
            Assert.AreEqual(1516642130, args1.Item3);
            Assert.AreEqual(757660304, args2.Item3);
            Assert.AreEqual(1779637132, args1.Item4);
            Assert.AreEqual(342451040, args2.Item4);
            Assert.AreEqual(1249693301, args1.Item5);
            Assert.AreEqual(508677449, args2.Item5);
            Assert.AreEqual(697996635, args1.Item6);
            Assert.AreEqual(1327801974, args2.Item6);
            Assert.AreEqual(1149977745, args1.Item7);
            Assert.AreEqual(272127491, args2.Item7);
            Assert.AreEqual(88375853, args1.Item8);
            Assert.AreEqual(399712914, args2.Item8);
            Assert.AreEqual(1896587492, args1.Item9);
            Assert.AreEqual(1034548160, args2.Item9);
            Assert.AreEqual(927038697, args1.Item10);
            Assert.AreEqual(49526117, args2.Item10);
            Assert.AreEqual(924366747, args1.Item11);
            Assert.AreEqual(381369773, args2.Item11);

            Assert.AreEqual(args1, args1);

            var copy1 = args1;
            Assert.IsTrue(args1 == copy1);
            Assert.IsFalse(args1 != copy1);

            Assert.AreEqual(0, args1.CompareTo(copy1));

            Assert.AreNotEqual(args1.GetHashCode(), args2.GetHashCode());
            Assert.AreNotEqual(args1, args2);
            Assert.AreNotEqual(args2, args1);

            var scm1 = (IStructuralComparable)args1;
            var cmp1 = (IComparable)args1;

            Assert.AreNotEqual(0, args1.CompareTo(args2));
            Assert.AreNotEqual(0, scm1.CompareTo(args2, Comparer<object>.Default));
            Assert.AreNotEqual(0, cmp1.CompareTo(args2));
            Assert.AreEqual(1, scm1.CompareTo(other: null, Comparer<object>.Default));

            Assert.ThrowsException<ArgumentException>(() => scm1.CompareTo("foo", Comparer<object>.Default));

            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1460409939, 185614642, 1516642130, 1779637132, 1249693301, 697996635, 1149977745, 88375853, 1896587492, 927038697, 924366747)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1460409937, 185614642, 1516642130, 1779637132, 1249693301, 697996635, 1149977745, 88375853, 1896587492, 927038697, 924366747)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1460409939, 185614642, 1516642130, 1779637132, 1249693301, 697996635, 1149977745, 88375853, 1896587492, 927038697, 924366747), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1460409937, 185614642, 1516642130, 1779637132, 1249693301, 697996635, 1149977745, 88375853, 1896587492, 927038697, 924366747), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1460409938, 185614643, 1516642130, 1779637132, 1249693301, 697996635, 1149977745, 88375853, 1896587492, 927038697, 924366747)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1460409938, 185614641, 1516642130, 1779637132, 1249693301, 697996635, 1149977745, 88375853, 1896587492, 927038697, 924366747)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1460409938, 185614643, 1516642130, 1779637132, 1249693301, 697996635, 1149977745, 88375853, 1896587492, 927038697, 924366747), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1460409938, 185614641, 1516642130, 1779637132, 1249693301, 697996635, 1149977745, 88375853, 1896587492, 927038697, 924366747), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1460409938, 185614642, 1516642131, 1779637132, 1249693301, 697996635, 1149977745, 88375853, 1896587492, 927038697, 924366747)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1460409938, 185614642, 1516642129, 1779637132, 1249693301, 697996635, 1149977745, 88375853, 1896587492, 927038697, 924366747)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1460409938, 185614642, 1516642131, 1779637132, 1249693301, 697996635, 1149977745, 88375853, 1896587492, 927038697, 924366747), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1460409938, 185614642, 1516642129, 1779637132, 1249693301, 697996635, 1149977745, 88375853, 1896587492, 927038697, 924366747), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1460409938, 185614642, 1516642130, 1779637133, 1249693301, 697996635, 1149977745, 88375853, 1896587492, 927038697, 924366747)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1460409938, 185614642, 1516642130, 1779637131, 1249693301, 697996635, 1149977745, 88375853, 1896587492, 927038697, 924366747)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1460409938, 185614642, 1516642130, 1779637133, 1249693301, 697996635, 1149977745, 88375853, 1896587492, 927038697, 924366747), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1460409938, 185614642, 1516642130, 1779637131, 1249693301, 697996635, 1149977745, 88375853, 1896587492, 927038697, 924366747), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1460409938, 185614642, 1516642130, 1779637132, 1249693302, 697996635, 1149977745, 88375853, 1896587492, 927038697, 924366747)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1460409938, 185614642, 1516642130, 1779637132, 1249693300, 697996635, 1149977745, 88375853, 1896587492, 927038697, 924366747)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1460409938, 185614642, 1516642130, 1779637132, 1249693302, 697996635, 1149977745, 88375853, 1896587492, 927038697, 924366747), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1460409938, 185614642, 1516642130, 1779637132, 1249693300, 697996635, 1149977745, 88375853, 1896587492, 927038697, 924366747), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1460409938, 185614642, 1516642130, 1779637132, 1249693301, 697996636, 1149977745, 88375853, 1896587492, 927038697, 924366747)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1460409938, 185614642, 1516642130, 1779637132, 1249693301, 697996634, 1149977745, 88375853, 1896587492, 927038697, 924366747)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1460409938, 185614642, 1516642130, 1779637132, 1249693301, 697996636, 1149977745, 88375853, 1896587492, 927038697, 924366747), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1460409938, 185614642, 1516642130, 1779637132, 1249693301, 697996634, 1149977745, 88375853, 1896587492, 927038697, 924366747), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1460409938, 185614642, 1516642130, 1779637132, 1249693301, 697996635, 1149977746, 88375853, 1896587492, 927038697, 924366747)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1460409938, 185614642, 1516642130, 1779637132, 1249693301, 697996635, 1149977744, 88375853, 1896587492, 927038697, 924366747)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1460409938, 185614642, 1516642130, 1779637132, 1249693301, 697996635, 1149977746, 88375853, 1896587492, 927038697, 924366747), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1460409938, 185614642, 1516642130, 1779637132, 1249693301, 697996635, 1149977744, 88375853, 1896587492, 927038697, 924366747), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1460409938, 185614642, 1516642130, 1779637132, 1249693301, 697996635, 1149977745, 88375854, 1896587492, 927038697, 924366747)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1460409938, 185614642, 1516642130, 1779637132, 1249693301, 697996635, 1149977745, 88375852, 1896587492, 927038697, 924366747)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1460409938, 185614642, 1516642130, 1779637132, 1249693301, 697996635, 1149977745, 88375854, 1896587492, 927038697, 924366747), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1460409938, 185614642, 1516642130, 1779637132, 1249693301, 697996635, 1149977745, 88375852, 1896587492, 927038697, 924366747), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1460409938, 185614642, 1516642130, 1779637132, 1249693301, 697996635, 1149977745, 88375853, 1896587493, 927038697, 924366747)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1460409938, 185614642, 1516642130, 1779637132, 1249693301, 697996635, 1149977745, 88375853, 1896587491, 927038697, 924366747)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1460409938, 185614642, 1516642130, 1779637132, 1249693301, 697996635, 1149977745, 88375853, 1896587493, 927038697, 924366747), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1460409938, 185614642, 1516642130, 1779637132, 1249693301, 697996635, 1149977745, 88375853, 1896587491, 927038697, 924366747), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1460409938, 185614642, 1516642130, 1779637132, 1249693301, 697996635, 1149977745, 88375853, 1896587492, 927038698, 924366747)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1460409938, 185614642, 1516642130, 1779637132, 1249693301, 697996635, 1149977745, 88375853, 1896587492, 927038696, 924366747)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1460409938, 185614642, 1516642130, 1779637132, 1249693301, 697996635, 1149977745, 88375853, 1896587492, 927038698, 924366747), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1460409938, 185614642, 1516642130, 1779637132, 1249693301, 697996635, 1149977745, 88375853, 1896587492, 927038696, 924366747), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1460409938, 185614642, 1516642130, 1779637132, 1249693301, 697996635, 1149977745, 88375853, 1896587492, 927038697, 924366748)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1460409938, 185614642, 1516642130, 1779637132, 1249693301, 697996635, 1149977745, 88375853, 1896587492, 927038697, 924366746)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1460409938, 185614642, 1516642130, 1779637132, 1249693301, 697996635, 1149977745, 88375853, 1896587492, 927038697, 924366748), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1460409938, 185614642, 1516642130, 1779637132, 1249693301, 697996635, 1149977745, 88375853, 1896587492, 927038697, 924366746), Comparer<object>.Default) > 0);

            Assert.IsFalse(args1 == args2);
            Assert.IsTrue(args1 != args2);

            Assert.AreNotEqual(args1, "foo");
            Assert.AreNotEqual(args1, null);

            var seq1 = (IStructuralEquatable)args1;
            Assert.AreEqual(args1.GetHashCode(), seq1.GetHashCode(EqualityComparer<object>.Default));
            Assert.AreEqual(0, seq1.GetHashCode(new ConstantHashEqualityComparer(0)));
            Assert.IsTrue(seq1.Equals(args1, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals(args2, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals("foo", EqualityComparer<object>.Default));

            Assert.AreEqual("(1460409938, 185614642, 1516642130, 1779637132, 1249693301, 697996635, 1149977745, 88375853, 1896587492, 927038697, 924366747)", args1.ToString());
            Assert.AreEqual("(1469951323, 1484477316, 757660304, 342451040, 508677449, 1327801974, 272127491, 399712914, 1034548160, 49526117, 381369773)", args2.ToString());
        }

        [TestMethod]
        public void Tuplet12()
        {
            var args1 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177914, 843764805, 1616659205, 237467758, 1877734186, 2034555795, 985839498, 349845215, 1867165537, 1451341831, 102676511, 1849976561);
            var args2 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(83608757, 167956149, 1857837481, 417109288, 1738779491, 2009263900, 597934946, 1276307759, 1297306457, 11669549, 1172766508, 1082756073);

            Assert.AreEqual(30177914, args1.Item1);
            Assert.AreEqual(83608757, args2.Item1);
            Assert.AreEqual(843764805, args1.Item2);
            Assert.AreEqual(167956149, args2.Item2);
            Assert.AreEqual(1616659205, args1.Item3);
            Assert.AreEqual(1857837481, args2.Item3);
            Assert.AreEqual(237467758, args1.Item4);
            Assert.AreEqual(417109288, args2.Item4);
            Assert.AreEqual(1877734186, args1.Item5);
            Assert.AreEqual(1738779491, args2.Item5);
            Assert.AreEqual(2034555795, args1.Item6);
            Assert.AreEqual(2009263900, args2.Item6);
            Assert.AreEqual(985839498, args1.Item7);
            Assert.AreEqual(597934946, args2.Item7);
            Assert.AreEqual(349845215, args1.Item8);
            Assert.AreEqual(1276307759, args2.Item8);
            Assert.AreEqual(1867165537, args1.Item9);
            Assert.AreEqual(1297306457, args2.Item9);
            Assert.AreEqual(1451341831, args1.Item10);
            Assert.AreEqual(11669549, args2.Item10);
            Assert.AreEqual(102676511, args1.Item11);
            Assert.AreEqual(1172766508, args2.Item11);
            Assert.AreEqual(1849976561, args1.Item12);
            Assert.AreEqual(1082756073, args2.Item12);

            Assert.AreEqual(args1, args1);

            var copy1 = args1;
            Assert.IsTrue(args1 == copy1);
            Assert.IsFalse(args1 != copy1);

            Assert.AreEqual(0, args1.CompareTo(copy1));

            Assert.AreNotEqual(args1.GetHashCode(), args2.GetHashCode());
            Assert.AreNotEqual(args1, args2);
            Assert.AreNotEqual(args2, args1);

            var scm1 = (IStructuralComparable)args1;
            var cmp1 = (IComparable)args1;

            Assert.AreNotEqual(0, args1.CompareTo(args2));
            Assert.AreNotEqual(0, scm1.CompareTo(args2, Comparer<object>.Default));
            Assert.AreNotEqual(0, cmp1.CompareTo(args2));
            Assert.AreEqual(1, scm1.CompareTo(other: null, Comparer<object>.Default));

            Assert.ThrowsException<ArgumentException>(() => scm1.CompareTo("foo", Comparer<object>.Default));

            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177915, 843764805, 1616659205, 237467758, 1877734186, 2034555795, 985839498, 349845215, 1867165537, 1451341831, 102676511, 1849976561)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177913, 843764805, 1616659205, 237467758, 1877734186, 2034555795, 985839498, 349845215, 1867165537, 1451341831, 102676511, 1849976561)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177915, 843764805, 1616659205, 237467758, 1877734186, 2034555795, 985839498, 349845215, 1867165537, 1451341831, 102676511, 1849976561), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177913, 843764805, 1616659205, 237467758, 1877734186, 2034555795, 985839498, 349845215, 1867165537, 1451341831, 102676511, 1849976561), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177914, 843764806, 1616659205, 237467758, 1877734186, 2034555795, 985839498, 349845215, 1867165537, 1451341831, 102676511, 1849976561)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177914, 843764804, 1616659205, 237467758, 1877734186, 2034555795, 985839498, 349845215, 1867165537, 1451341831, 102676511, 1849976561)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177914, 843764806, 1616659205, 237467758, 1877734186, 2034555795, 985839498, 349845215, 1867165537, 1451341831, 102676511, 1849976561), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177914, 843764804, 1616659205, 237467758, 1877734186, 2034555795, 985839498, 349845215, 1867165537, 1451341831, 102676511, 1849976561), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177914, 843764805, 1616659206, 237467758, 1877734186, 2034555795, 985839498, 349845215, 1867165537, 1451341831, 102676511, 1849976561)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177914, 843764805, 1616659204, 237467758, 1877734186, 2034555795, 985839498, 349845215, 1867165537, 1451341831, 102676511, 1849976561)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177914, 843764805, 1616659206, 237467758, 1877734186, 2034555795, 985839498, 349845215, 1867165537, 1451341831, 102676511, 1849976561), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177914, 843764805, 1616659204, 237467758, 1877734186, 2034555795, 985839498, 349845215, 1867165537, 1451341831, 102676511, 1849976561), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177914, 843764805, 1616659205, 237467759, 1877734186, 2034555795, 985839498, 349845215, 1867165537, 1451341831, 102676511, 1849976561)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177914, 843764805, 1616659205, 237467757, 1877734186, 2034555795, 985839498, 349845215, 1867165537, 1451341831, 102676511, 1849976561)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177914, 843764805, 1616659205, 237467759, 1877734186, 2034555795, 985839498, 349845215, 1867165537, 1451341831, 102676511, 1849976561), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177914, 843764805, 1616659205, 237467757, 1877734186, 2034555795, 985839498, 349845215, 1867165537, 1451341831, 102676511, 1849976561), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177914, 843764805, 1616659205, 237467758, 1877734187, 2034555795, 985839498, 349845215, 1867165537, 1451341831, 102676511, 1849976561)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177914, 843764805, 1616659205, 237467758, 1877734185, 2034555795, 985839498, 349845215, 1867165537, 1451341831, 102676511, 1849976561)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177914, 843764805, 1616659205, 237467758, 1877734187, 2034555795, 985839498, 349845215, 1867165537, 1451341831, 102676511, 1849976561), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177914, 843764805, 1616659205, 237467758, 1877734185, 2034555795, 985839498, 349845215, 1867165537, 1451341831, 102676511, 1849976561), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177914, 843764805, 1616659205, 237467758, 1877734186, 2034555796, 985839498, 349845215, 1867165537, 1451341831, 102676511, 1849976561)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177914, 843764805, 1616659205, 237467758, 1877734186, 2034555794, 985839498, 349845215, 1867165537, 1451341831, 102676511, 1849976561)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177914, 843764805, 1616659205, 237467758, 1877734186, 2034555796, 985839498, 349845215, 1867165537, 1451341831, 102676511, 1849976561), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177914, 843764805, 1616659205, 237467758, 1877734186, 2034555794, 985839498, 349845215, 1867165537, 1451341831, 102676511, 1849976561), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177914, 843764805, 1616659205, 237467758, 1877734186, 2034555795, 985839499, 349845215, 1867165537, 1451341831, 102676511, 1849976561)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177914, 843764805, 1616659205, 237467758, 1877734186, 2034555795, 985839497, 349845215, 1867165537, 1451341831, 102676511, 1849976561)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177914, 843764805, 1616659205, 237467758, 1877734186, 2034555795, 985839499, 349845215, 1867165537, 1451341831, 102676511, 1849976561), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177914, 843764805, 1616659205, 237467758, 1877734186, 2034555795, 985839497, 349845215, 1867165537, 1451341831, 102676511, 1849976561), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177914, 843764805, 1616659205, 237467758, 1877734186, 2034555795, 985839498, 349845216, 1867165537, 1451341831, 102676511, 1849976561)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177914, 843764805, 1616659205, 237467758, 1877734186, 2034555795, 985839498, 349845214, 1867165537, 1451341831, 102676511, 1849976561)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177914, 843764805, 1616659205, 237467758, 1877734186, 2034555795, 985839498, 349845216, 1867165537, 1451341831, 102676511, 1849976561), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177914, 843764805, 1616659205, 237467758, 1877734186, 2034555795, 985839498, 349845214, 1867165537, 1451341831, 102676511, 1849976561), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177914, 843764805, 1616659205, 237467758, 1877734186, 2034555795, 985839498, 349845215, 1867165538, 1451341831, 102676511, 1849976561)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177914, 843764805, 1616659205, 237467758, 1877734186, 2034555795, 985839498, 349845215, 1867165536, 1451341831, 102676511, 1849976561)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177914, 843764805, 1616659205, 237467758, 1877734186, 2034555795, 985839498, 349845215, 1867165538, 1451341831, 102676511, 1849976561), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177914, 843764805, 1616659205, 237467758, 1877734186, 2034555795, 985839498, 349845215, 1867165536, 1451341831, 102676511, 1849976561), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177914, 843764805, 1616659205, 237467758, 1877734186, 2034555795, 985839498, 349845215, 1867165537, 1451341832, 102676511, 1849976561)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177914, 843764805, 1616659205, 237467758, 1877734186, 2034555795, 985839498, 349845215, 1867165537, 1451341830, 102676511, 1849976561)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177914, 843764805, 1616659205, 237467758, 1877734186, 2034555795, 985839498, 349845215, 1867165537, 1451341832, 102676511, 1849976561), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177914, 843764805, 1616659205, 237467758, 1877734186, 2034555795, 985839498, 349845215, 1867165537, 1451341830, 102676511, 1849976561), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177914, 843764805, 1616659205, 237467758, 1877734186, 2034555795, 985839498, 349845215, 1867165537, 1451341831, 102676512, 1849976561)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177914, 843764805, 1616659205, 237467758, 1877734186, 2034555795, 985839498, 349845215, 1867165537, 1451341831, 102676510, 1849976561)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177914, 843764805, 1616659205, 237467758, 1877734186, 2034555795, 985839498, 349845215, 1867165537, 1451341831, 102676512, 1849976561), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177914, 843764805, 1616659205, 237467758, 1877734186, 2034555795, 985839498, 349845215, 1867165537, 1451341831, 102676510, 1849976561), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177914, 843764805, 1616659205, 237467758, 1877734186, 2034555795, 985839498, 349845215, 1867165537, 1451341831, 102676511, 1849976562)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177914, 843764805, 1616659205, 237467758, 1877734186, 2034555795, 985839498, 349845215, 1867165537, 1451341831, 102676511, 1849976560)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177914, 843764805, 1616659205, 237467758, 1877734186, 2034555795, 985839498, 349845215, 1867165537, 1451341831, 102676511, 1849976562), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(30177914, 843764805, 1616659205, 237467758, 1877734186, 2034555795, 985839498, 349845215, 1867165537, 1451341831, 102676511, 1849976560), Comparer<object>.Default) > 0);

            Assert.IsFalse(args1 == args2);
            Assert.IsTrue(args1 != args2);

            Assert.AreNotEqual(args1, "foo");
            Assert.AreNotEqual(args1, null);

            var seq1 = (IStructuralEquatable)args1;
            Assert.AreEqual(args1.GetHashCode(), seq1.GetHashCode(EqualityComparer<object>.Default));
            Assert.AreEqual(0, seq1.GetHashCode(new ConstantHashEqualityComparer(0)));
            Assert.IsTrue(seq1.Equals(args1, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals(args2, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals("foo", EqualityComparer<object>.Default));

            Assert.AreEqual("(30177914, 843764805, 1616659205, 237467758, 1877734186, 2034555795, 985839498, 349845215, 1867165537, 1451341831, 102676511, 1849976561)", args1.ToString());
            Assert.AreEqual("(83608757, 167956149, 1857837481, 417109288, 1738779491, 2009263900, 597934946, 1276307759, 1297306457, 11669549, 1172766508, 1082756073)", args2.ToString());
        }

        [TestMethod]
        public void Tuplet13()
        {
            var args1 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587381, 837247700, 575676484, 1147197848, 1745201687, 315854919, 1505388704, 1221338601, 1079040165, 155436728, 672877325, 162977927);
            var args2 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(1012225543, 967746096, 1262905597, 1250020002, 1546742277, 1207356807, 1620508563, 1367274812, 1781984402, 674051547, 174494891, 798323615, 910692686);

            Assert.AreEqual(344207836, args1.Item1);
            Assert.AreEqual(1012225543, args2.Item1);
            Assert.AreEqual(210587381, args1.Item2);
            Assert.AreEqual(967746096, args2.Item2);
            Assert.AreEqual(837247700, args1.Item3);
            Assert.AreEqual(1262905597, args2.Item3);
            Assert.AreEqual(575676484, args1.Item4);
            Assert.AreEqual(1250020002, args2.Item4);
            Assert.AreEqual(1147197848, args1.Item5);
            Assert.AreEqual(1546742277, args2.Item5);
            Assert.AreEqual(1745201687, args1.Item6);
            Assert.AreEqual(1207356807, args2.Item6);
            Assert.AreEqual(315854919, args1.Item7);
            Assert.AreEqual(1620508563, args2.Item7);
            Assert.AreEqual(1505388704, args1.Item8);
            Assert.AreEqual(1367274812, args2.Item8);
            Assert.AreEqual(1221338601, args1.Item9);
            Assert.AreEqual(1781984402, args2.Item9);
            Assert.AreEqual(1079040165, args1.Item10);
            Assert.AreEqual(674051547, args2.Item10);
            Assert.AreEqual(155436728, args1.Item11);
            Assert.AreEqual(174494891, args2.Item11);
            Assert.AreEqual(672877325, args1.Item12);
            Assert.AreEqual(798323615, args2.Item12);
            Assert.AreEqual(162977927, args1.Item13);
            Assert.AreEqual(910692686, args2.Item13);

            Assert.AreEqual(args1, args1);

            var copy1 = args1;
            Assert.IsTrue(args1 == copy1);
            Assert.IsFalse(args1 != copy1);

            Assert.AreEqual(0, args1.CompareTo(copy1));

            Assert.AreNotEqual(args1.GetHashCode(), args2.GetHashCode());
            Assert.AreNotEqual(args1, args2);
            Assert.AreNotEqual(args2, args1);

            var scm1 = (IStructuralComparable)args1;
            var cmp1 = (IComparable)args1;

            Assert.AreNotEqual(0, args1.CompareTo(args2));
            Assert.AreNotEqual(0, scm1.CompareTo(args2, Comparer<object>.Default));
            Assert.AreNotEqual(0, cmp1.CompareTo(args2));
            Assert.AreEqual(1, scm1.CompareTo(other: null, Comparer<object>.Default));

            Assert.ThrowsException<ArgumentException>(() => scm1.CompareTo("foo", Comparer<object>.Default));

            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207837, 210587381, 837247700, 575676484, 1147197848, 1745201687, 315854919, 1505388704, 1221338601, 1079040165, 155436728, 672877325, 162977927)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207835, 210587381, 837247700, 575676484, 1147197848, 1745201687, 315854919, 1505388704, 1221338601, 1079040165, 155436728, 672877325, 162977927)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207837, 210587381, 837247700, 575676484, 1147197848, 1745201687, 315854919, 1505388704, 1221338601, 1079040165, 155436728, 672877325, 162977927), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207835, 210587381, 837247700, 575676484, 1147197848, 1745201687, 315854919, 1505388704, 1221338601, 1079040165, 155436728, 672877325, 162977927), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587382, 837247700, 575676484, 1147197848, 1745201687, 315854919, 1505388704, 1221338601, 1079040165, 155436728, 672877325, 162977927)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587380, 837247700, 575676484, 1147197848, 1745201687, 315854919, 1505388704, 1221338601, 1079040165, 155436728, 672877325, 162977927)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587382, 837247700, 575676484, 1147197848, 1745201687, 315854919, 1505388704, 1221338601, 1079040165, 155436728, 672877325, 162977927), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587380, 837247700, 575676484, 1147197848, 1745201687, 315854919, 1505388704, 1221338601, 1079040165, 155436728, 672877325, 162977927), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587381, 837247701, 575676484, 1147197848, 1745201687, 315854919, 1505388704, 1221338601, 1079040165, 155436728, 672877325, 162977927)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587381, 837247699, 575676484, 1147197848, 1745201687, 315854919, 1505388704, 1221338601, 1079040165, 155436728, 672877325, 162977927)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587381, 837247701, 575676484, 1147197848, 1745201687, 315854919, 1505388704, 1221338601, 1079040165, 155436728, 672877325, 162977927), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587381, 837247699, 575676484, 1147197848, 1745201687, 315854919, 1505388704, 1221338601, 1079040165, 155436728, 672877325, 162977927), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587381, 837247700, 575676485, 1147197848, 1745201687, 315854919, 1505388704, 1221338601, 1079040165, 155436728, 672877325, 162977927)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587381, 837247700, 575676483, 1147197848, 1745201687, 315854919, 1505388704, 1221338601, 1079040165, 155436728, 672877325, 162977927)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587381, 837247700, 575676485, 1147197848, 1745201687, 315854919, 1505388704, 1221338601, 1079040165, 155436728, 672877325, 162977927), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587381, 837247700, 575676483, 1147197848, 1745201687, 315854919, 1505388704, 1221338601, 1079040165, 155436728, 672877325, 162977927), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587381, 837247700, 575676484, 1147197849, 1745201687, 315854919, 1505388704, 1221338601, 1079040165, 155436728, 672877325, 162977927)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587381, 837247700, 575676484, 1147197847, 1745201687, 315854919, 1505388704, 1221338601, 1079040165, 155436728, 672877325, 162977927)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587381, 837247700, 575676484, 1147197849, 1745201687, 315854919, 1505388704, 1221338601, 1079040165, 155436728, 672877325, 162977927), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587381, 837247700, 575676484, 1147197847, 1745201687, 315854919, 1505388704, 1221338601, 1079040165, 155436728, 672877325, 162977927), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587381, 837247700, 575676484, 1147197848, 1745201688, 315854919, 1505388704, 1221338601, 1079040165, 155436728, 672877325, 162977927)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587381, 837247700, 575676484, 1147197848, 1745201686, 315854919, 1505388704, 1221338601, 1079040165, 155436728, 672877325, 162977927)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587381, 837247700, 575676484, 1147197848, 1745201688, 315854919, 1505388704, 1221338601, 1079040165, 155436728, 672877325, 162977927), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587381, 837247700, 575676484, 1147197848, 1745201686, 315854919, 1505388704, 1221338601, 1079040165, 155436728, 672877325, 162977927), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587381, 837247700, 575676484, 1147197848, 1745201687, 315854920, 1505388704, 1221338601, 1079040165, 155436728, 672877325, 162977927)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587381, 837247700, 575676484, 1147197848, 1745201687, 315854918, 1505388704, 1221338601, 1079040165, 155436728, 672877325, 162977927)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587381, 837247700, 575676484, 1147197848, 1745201687, 315854920, 1505388704, 1221338601, 1079040165, 155436728, 672877325, 162977927), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587381, 837247700, 575676484, 1147197848, 1745201687, 315854918, 1505388704, 1221338601, 1079040165, 155436728, 672877325, 162977927), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587381, 837247700, 575676484, 1147197848, 1745201687, 315854919, 1505388705, 1221338601, 1079040165, 155436728, 672877325, 162977927)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587381, 837247700, 575676484, 1147197848, 1745201687, 315854919, 1505388703, 1221338601, 1079040165, 155436728, 672877325, 162977927)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587381, 837247700, 575676484, 1147197848, 1745201687, 315854919, 1505388705, 1221338601, 1079040165, 155436728, 672877325, 162977927), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587381, 837247700, 575676484, 1147197848, 1745201687, 315854919, 1505388703, 1221338601, 1079040165, 155436728, 672877325, 162977927), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587381, 837247700, 575676484, 1147197848, 1745201687, 315854919, 1505388704, 1221338602, 1079040165, 155436728, 672877325, 162977927)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587381, 837247700, 575676484, 1147197848, 1745201687, 315854919, 1505388704, 1221338600, 1079040165, 155436728, 672877325, 162977927)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587381, 837247700, 575676484, 1147197848, 1745201687, 315854919, 1505388704, 1221338602, 1079040165, 155436728, 672877325, 162977927), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587381, 837247700, 575676484, 1147197848, 1745201687, 315854919, 1505388704, 1221338600, 1079040165, 155436728, 672877325, 162977927), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587381, 837247700, 575676484, 1147197848, 1745201687, 315854919, 1505388704, 1221338601, 1079040166, 155436728, 672877325, 162977927)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587381, 837247700, 575676484, 1147197848, 1745201687, 315854919, 1505388704, 1221338601, 1079040164, 155436728, 672877325, 162977927)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587381, 837247700, 575676484, 1147197848, 1745201687, 315854919, 1505388704, 1221338601, 1079040166, 155436728, 672877325, 162977927), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587381, 837247700, 575676484, 1147197848, 1745201687, 315854919, 1505388704, 1221338601, 1079040164, 155436728, 672877325, 162977927), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587381, 837247700, 575676484, 1147197848, 1745201687, 315854919, 1505388704, 1221338601, 1079040165, 155436729, 672877325, 162977927)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587381, 837247700, 575676484, 1147197848, 1745201687, 315854919, 1505388704, 1221338601, 1079040165, 155436727, 672877325, 162977927)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587381, 837247700, 575676484, 1147197848, 1745201687, 315854919, 1505388704, 1221338601, 1079040165, 155436729, 672877325, 162977927), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587381, 837247700, 575676484, 1147197848, 1745201687, 315854919, 1505388704, 1221338601, 1079040165, 155436727, 672877325, 162977927), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587381, 837247700, 575676484, 1147197848, 1745201687, 315854919, 1505388704, 1221338601, 1079040165, 155436728, 672877326, 162977927)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587381, 837247700, 575676484, 1147197848, 1745201687, 315854919, 1505388704, 1221338601, 1079040165, 155436728, 672877324, 162977927)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587381, 837247700, 575676484, 1147197848, 1745201687, 315854919, 1505388704, 1221338601, 1079040165, 155436728, 672877326, 162977927), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587381, 837247700, 575676484, 1147197848, 1745201687, 315854919, 1505388704, 1221338601, 1079040165, 155436728, 672877324, 162977927), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587381, 837247700, 575676484, 1147197848, 1745201687, 315854919, 1505388704, 1221338601, 1079040165, 155436728, 672877325, 162977928)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587381, 837247700, 575676484, 1147197848, 1745201687, 315854919, 1505388704, 1221338601, 1079040165, 155436728, 672877325, 162977926)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587381, 837247700, 575676484, 1147197848, 1745201687, 315854919, 1505388704, 1221338601, 1079040165, 155436728, 672877325, 162977928), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(344207836, 210587381, 837247700, 575676484, 1147197848, 1745201687, 315854919, 1505388704, 1221338601, 1079040165, 155436728, 672877325, 162977926), Comparer<object>.Default) > 0);

            Assert.IsFalse(args1 == args2);
            Assert.IsTrue(args1 != args2);

            Assert.AreNotEqual(args1, "foo");
            Assert.AreNotEqual(args1, null);

            var seq1 = (IStructuralEquatable)args1;
            Assert.AreEqual(args1.GetHashCode(), seq1.GetHashCode(EqualityComparer<object>.Default));
            Assert.AreEqual(0, seq1.GetHashCode(new ConstantHashEqualityComparer(0)));
            Assert.IsTrue(seq1.Equals(args1, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals(args2, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals("foo", EqualityComparer<object>.Default));

            Assert.AreEqual("(344207836, 210587381, 837247700, 575676484, 1147197848, 1745201687, 315854919, 1505388704, 1221338601, 1079040165, 155436728, 672877325, 162977927)", args1.ToString());
            Assert.AreEqual("(1012225543, 967746096, 1262905597, 1250020002, 1546742277, 1207356807, 1620508563, 1367274812, 1781984402, 674051547, 174494891, 798323615, 910692686)", args2.ToString());
        }

        [TestMethod]
        public void Tuplet14()
        {
            var args1 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613214, 920702005, 1231546963, 18508365, 1818481944, 533903132, 2040743569, 1667146805, 1197308095, 410163014, 1350131014, 121963850);
            var args2 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1135486912, 744771454, 628637960, 1152052239, 12519421, 1184960156, 254131361, 726553948, 1041517804, 1482512996, 26287757, 1898047827, 951796389, 1699741592);

            Assert.AreEqual(680831647, args1.Item1);
            Assert.AreEqual(1135486912, args2.Item1);
            Assert.AreEqual(537932661, args1.Item2);
            Assert.AreEqual(744771454, args2.Item2);
            Assert.AreEqual(436613214, args1.Item3);
            Assert.AreEqual(628637960, args2.Item3);
            Assert.AreEqual(920702005, args1.Item4);
            Assert.AreEqual(1152052239, args2.Item4);
            Assert.AreEqual(1231546963, args1.Item5);
            Assert.AreEqual(12519421, args2.Item5);
            Assert.AreEqual(18508365, args1.Item6);
            Assert.AreEqual(1184960156, args2.Item6);
            Assert.AreEqual(1818481944, args1.Item7);
            Assert.AreEqual(254131361, args2.Item7);
            Assert.AreEqual(533903132, args1.Item8);
            Assert.AreEqual(726553948, args2.Item8);
            Assert.AreEqual(2040743569, args1.Item9);
            Assert.AreEqual(1041517804, args2.Item9);
            Assert.AreEqual(1667146805, args1.Item10);
            Assert.AreEqual(1482512996, args2.Item10);
            Assert.AreEqual(1197308095, args1.Item11);
            Assert.AreEqual(26287757, args2.Item11);
            Assert.AreEqual(410163014, args1.Item12);
            Assert.AreEqual(1898047827, args2.Item12);
            Assert.AreEqual(1350131014, args1.Item13);
            Assert.AreEqual(951796389, args2.Item13);
            Assert.AreEqual(121963850, args1.Item14);
            Assert.AreEqual(1699741592, args2.Item14);

            Assert.AreEqual(args1, args1);

            var copy1 = args1;
            Assert.IsTrue(args1 == copy1);
            Assert.IsFalse(args1 != copy1);

            Assert.AreEqual(0, args1.CompareTo(copy1));

            Assert.AreNotEqual(args1.GetHashCode(), args2.GetHashCode());
            Assert.AreNotEqual(args1, args2);
            Assert.AreNotEqual(args2, args1);

            var scm1 = (IStructuralComparable)args1;
            var cmp1 = (IComparable)args1;

            Assert.AreNotEqual(0, args1.CompareTo(args2));
            Assert.AreNotEqual(0, scm1.CompareTo(args2, Comparer<object>.Default));
            Assert.AreNotEqual(0, cmp1.CompareTo(args2));
            Assert.AreEqual(1, scm1.CompareTo(other: null, Comparer<object>.Default));

            Assert.ThrowsException<ArgumentException>(() => scm1.CompareTo("foo", Comparer<object>.Default));

            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831648, 537932661, 436613214, 920702005, 1231546963, 18508365, 1818481944, 533903132, 2040743569, 1667146805, 1197308095, 410163014, 1350131014, 121963850)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831646, 537932661, 436613214, 920702005, 1231546963, 18508365, 1818481944, 533903132, 2040743569, 1667146805, 1197308095, 410163014, 1350131014, 121963850)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831648, 537932661, 436613214, 920702005, 1231546963, 18508365, 1818481944, 533903132, 2040743569, 1667146805, 1197308095, 410163014, 1350131014, 121963850), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831646, 537932661, 436613214, 920702005, 1231546963, 18508365, 1818481944, 533903132, 2040743569, 1667146805, 1197308095, 410163014, 1350131014, 121963850), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932662, 436613214, 920702005, 1231546963, 18508365, 1818481944, 533903132, 2040743569, 1667146805, 1197308095, 410163014, 1350131014, 121963850)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932660, 436613214, 920702005, 1231546963, 18508365, 1818481944, 533903132, 2040743569, 1667146805, 1197308095, 410163014, 1350131014, 121963850)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932662, 436613214, 920702005, 1231546963, 18508365, 1818481944, 533903132, 2040743569, 1667146805, 1197308095, 410163014, 1350131014, 121963850), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932660, 436613214, 920702005, 1231546963, 18508365, 1818481944, 533903132, 2040743569, 1667146805, 1197308095, 410163014, 1350131014, 121963850), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613215, 920702005, 1231546963, 18508365, 1818481944, 533903132, 2040743569, 1667146805, 1197308095, 410163014, 1350131014, 121963850)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613213, 920702005, 1231546963, 18508365, 1818481944, 533903132, 2040743569, 1667146805, 1197308095, 410163014, 1350131014, 121963850)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613215, 920702005, 1231546963, 18508365, 1818481944, 533903132, 2040743569, 1667146805, 1197308095, 410163014, 1350131014, 121963850), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613213, 920702005, 1231546963, 18508365, 1818481944, 533903132, 2040743569, 1667146805, 1197308095, 410163014, 1350131014, 121963850), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613214, 920702006, 1231546963, 18508365, 1818481944, 533903132, 2040743569, 1667146805, 1197308095, 410163014, 1350131014, 121963850)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613214, 920702004, 1231546963, 18508365, 1818481944, 533903132, 2040743569, 1667146805, 1197308095, 410163014, 1350131014, 121963850)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613214, 920702006, 1231546963, 18508365, 1818481944, 533903132, 2040743569, 1667146805, 1197308095, 410163014, 1350131014, 121963850), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613214, 920702004, 1231546963, 18508365, 1818481944, 533903132, 2040743569, 1667146805, 1197308095, 410163014, 1350131014, 121963850), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613214, 920702005, 1231546964, 18508365, 1818481944, 533903132, 2040743569, 1667146805, 1197308095, 410163014, 1350131014, 121963850)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613214, 920702005, 1231546962, 18508365, 1818481944, 533903132, 2040743569, 1667146805, 1197308095, 410163014, 1350131014, 121963850)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613214, 920702005, 1231546964, 18508365, 1818481944, 533903132, 2040743569, 1667146805, 1197308095, 410163014, 1350131014, 121963850), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613214, 920702005, 1231546962, 18508365, 1818481944, 533903132, 2040743569, 1667146805, 1197308095, 410163014, 1350131014, 121963850), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613214, 920702005, 1231546963, 18508366, 1818481944, 533903132, 2040743569, 1667146805, 1197308095, 410163014, 1350131014, 121963850)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613214, 920702005, 1231546963, 18508364, 1818481944, 533903132, 2040743569, 1667146805, 1197308095, 410163014, 1350131014, 121963850)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613214, 920702005, 1231546963, 18508366, 1818481944, 533903132, 2040743569, 1667146805, 1197308095, 410163014, 1350131014, 121963850), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613214, 920702005, 1231546963, 18508364, 1818481944, 533903132, 2040743569, 1667146805, 1197308095, 410163014, 1350131014, 121963850), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613214, 920702005, 1231546963, 18508365, 1818481945, 533903132, 2040743569, 1667146805, 1197308095, 410163014, 1350131014, 121963850)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613214, 920702005, 1231546963, 18508365, 1818481943, 533903132, 2040743569, 1667146805, 1197308095, 410163014, 1350131014, 121963850)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613214, 920702005, 1231546963, 18508365, 1818481945, 533903132, 2040743569, 1667146805, 1197308095, 410163014, 1350131014, 121963850), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613214, 920702005, 1231546963, 18508365, 1818481943, 533903132, 2040743569, 1667146805, 1197308095, 410163014, 1350131014, 121963850), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613214, 920702005, 1231546963, 18508365, 1818481944, 533903133, 2040743569, 1667146805, 1197308095, 410163014, 1350131014, 121963850)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613214, 920702005, 1231546963, 18508365, 1818481944, 533903131, 2040743569, 1667146805, 1197308095, 410163014, 1350131014, 121963850)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613214, 920702005, 1231546963, 18508365, 1818481944, 533903133, 2040743569, 1667146805, 1197308095, 410163014, 1350131014, 121963850), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613214, 920702005, 1231546963, 18508365, 1818481944, 533903131, 2040743569, 1667146805, 1197308095, 410163014, 1350131014, 121963850), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613214, 920702005, 1231546963, 18508365, 1818481944, 533903132, 2040743570, 1667146805, 1197308095, 410163014, 1350131014, 121963850)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613214, 920702005, 1231546963, 18508365, 1818481944, 533903132, 2040743568, 1667146805, 1197308095, 410163014, 1350131014, 121963850)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613214, 920702005, 1231546963, 18508365, 1818481944, 533903132, 2040743570, 1667146805, 1197308095, 410163014, 1350131014, 121963850), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613214, 920702005, 1231546963, 18508365, 1818481944, 533903132, 2040743568, 1667146805, 1197308095, 410163014, 1350131014, 121963850), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613214, 920702005, 1231546963, 18508365, 1818481944, 533903132, 2040743569, 1667146806, 1197308095, 410163014, 1350131014, 121963850)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613214, 920702005, 1231546963, 18508365, 1818481944, 533903132, 2040743569, 1667146804, 1197308095, 410163014, 1350131014, 121963850)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613214, 920702005, 1231546963, 18508365, 1818481944, 533903132, 2040743569, 1667146806, 1197308095, 410163014, 1350131014, 121963850), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613214, 920702005, 1231546963, 18508365, 1818481944, 533903132, 2040743569, 1667146804, 1197308095, 410163014, 1350131014, 121963850), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613214, 920702005, 1231546963, 18508365, 1818481944, 533903132, 2040743569, 1667146805, 1197308096, 410163014, 1350131014, 121963850)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613214, 920702005, 1231546963, 18508365, 1818481944, 533903132, 2040743569, 1667146805, 1197308094, 410163014, 1350131014, 121963850)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613214, 920702005, 1231546963, 18508365, 1818481944, 533903132, 2040743569, 1667146805, 1197308096, 410163014, 1350131014, 121963850), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613214, 920702005, 1231546963, 18508365, 1818481944, 533903132, 2040743569, 1667146805, 1197308094, 410163014, 1350131014, 121963850), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613214, 920702005, 1231546963, 18508365, 1818481944, 533903132, 2040743569, 1667146805, 1197308095, 410163015, 1350131014, 121963850)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613214, 920702005, 1231546963, 18508365, 1818481944, 533903132, 2040743569, 1667146805, 1197308095, 410163013, 1350131014, 121963850)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613214, 920702005, 1231546963, 18508365, 1818481944, 533903132, 2040743569, 1667146805, 1197308095, 410163015, 1350131014, 121963850), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613214, 920702005, 1231546963, 18508365, 1818481944, 533903132, 2040743569, 1667146805, 1197308095, 410163013, 1350131014, 121963850), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613214, 920702005, 1231546963, 18508365, 1818481944, 533903132, 2040743569, 1667146805, 1197308095, 410163014, 1350131015, 121963850)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613214, 920702005, 1231546963, 18508365, 1818481944, 533903132, 2040743569, 1667146805, 1197308095, 410163014, 1350131013, 121963850)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613214, 920702005, 1231546963, 18508365, 1818481944, 533903132, 2040743569, 1667146805, 1197308095, 410163014, 1350131015, 121963850), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613214, 920702005, 1231546963, 18508365, 1818481944, 533903132, 2040743569, 1667146805, 1197308095, 410163014, 1350131013, 121963850), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613214, 920702005, 1231546963, 18508365, 1818481944, 533903132, 2040743569, 1667146805, 1197308095, 410163014, 1350131014, 121963851)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613214, 920702005, 1231546963, 18508365, 1818481944, 533903132, 2040743569, 1667146805, 1197308095, 410163014, 1350131014, 121963849)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613214, 920702005, 1231546963, 18508365, 1818481944, 533903132, 2040743569, 1667146805, 1197308095, 410163014, 1350131014, 121963851), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(680831647, 537932661, 436613214, 920702005, 1231546963, 18508365, 1818481944, 533903132, 2040743569, 1667146805, 1197308095, 410163014, 1350131014, 121963849), Comparer<object>.Default) > 0);

            Assert.IsFalse(args1 == args2);
            Assert.IsTrue(args1 != args2);

            Assert.AreNotEqual(args1, "foo");
            Assert.AreNotEqual(args1, null);

            var seq1 = (IStructuralEquatable)args1;
            Assert.AreEqual(args1.GetHashCode(), seq1.GetHashCode(EqualityComparer<object>.Default));
            Assert.AreEqual(0, seq1.GetHashCode(new ConstantHashEqualityComparer(0)));
            Assert.IsTrue(seq1.Equals(args1, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals(args2, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals("foo", EqualityComparer<object>.Default));

            Assert.AreEqual("(680831647, 537932661, 436613214, 920702005, 1231546963, 18508365, 1818481944, 533903132, 2040743569, 1667146805, 1197308095, 410163014, 1350131014, 121963850)", args1.ToString());
            Assert.AreEqual("(1135486912, 744771454, 628637960, 1152052239, 12519421, 1184960156, 254131361, 726553948, 1041517804, 1482512996, 26287757, 1898047827, 951796389, 1699741592)", args2.ToString());
        }

        [TestMethod]
        public void Tuplet15()
        {
            var args1 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752809, 1924836516, 236505162, 1064370040, 1925405905, 1068775490, 300636596, 1994976849, 136928363, 1001879028, 1776558442, 1118965621);
            var args2 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1448082938, 65597502, 839856988, 196611263, 1085392957, 485021651, 622503358, 1153346442, 1669482955, 161975470, 1760847106, 656561325, 2101761346, 1643898504, 1101583865);

            Assert.AreEqual(1862964908, args1.Item1);
            Assert.AreEqual(1448082938, args2.Item1);
            Assert.AreEqual(709707081, args1.Item2);
            Assert.AreEqual(65597502, args2.Item2);
            Assert.AreEqual(1684019481, args1.Item3);
            Assert.AreEqual(839856988, args2.Item3);
            Assert.AreEqual(662752809, args1.Item4);
            Assert.AreEqual(196611263, args2.Item4);
            Assert.AreEqual(1924836516, args1.Item5);
            Assert.AreEqual(1085392957, args2.Item5);
            Assert.AreEqual(236505162, args1.Item6);
            Assert.AreEqual(485021651, args2.Item6);
            Assert.AreEqual(1064370040, args1.Item7);
            Assert.AreEqual(622503358, args2.Item7);
            Assert.AreEqual(1925405905, args1.Item8);
            Assert.AreEqual(1153346442, args2.Item8);
            Assert.AreEqual(1068775490, args1.Item9);
            Assert.AreEqual(1669482955, args2.Item9);
            Assert.AreEqual(300636596, args1.Item10);
            Assert.AreEqual(161975470, args2.Item10);
            Assert.AreEqual(1994976849, args1.Item11);
            Assert.AreEqual(1760847106, args2.Item11);
            Assert.AreEqual(136928363, args1.Item12);
            Assert.AreEqual(656561325, args2.Item12);
            Assert.AreEqual(1001879028, args1.Item13);
            Assert.AreEqual(2101761346, args2.Item13);
            Assert.AreEqual(1776558442, args1.Item14);
            Assert.AreEqual(1643898504, args2.Item14);
            Assert.AreEqual(1118965621, args1.Item15);
            Assert.AreEqual(1101583865, args2.Item15);

            Assert.AreEqual(args1, args1);

            var copy1 = args1;
            Assert.IsTrue(args1 == copy1);
            Assert.IsFalse(args1 != copy1);

            Assert.AreEqual(0, args1.CompareTo(copy1));

            Assert.AreNotEqual(args1.GetHashCode(), args2.GetHashCode());
            Assert.AreNotEqual(args1, args2);
            Assert.AreNotEqual(args2, args1);

            var scm1 = (IStructuralComparable)args1;
            var cmp1 = (IComparable)args1;

            Assert.AreNotEqual(0, args1.CompareTo(args2));
            Assert.AreNotEqual(0, scm1.CompareTo(args2, Comparer<object>.Default));
            Assert.AreNotEqual(0, cmp1.CompareTo(args2));
            Assert.AreEqual(1, scm1.CompareTo(other: null, Comparer<object>.Default));

            Assert.ThrowsException<ArgumentException>(() => scm1.CompareTo("foo", Comparer<object>.Default));

            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964909, 709707081, 1684019481, 662752809, 1924836516, 236505162, 1064370040, 1925405905, 1068775490, 300636596, 1994976849, 136928363, 1001879028, 1776558442, 1118965621)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964907, 709707081, 1684019481, 662752809, 1924836516, 236505162, 1064370040, 1925405905, 1068775490, 300636596, 1994976849, 136928363, 1001879028, 1776558442, 1118965621)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964909, 709707081, 1684019481, 662752809, 1924836516, 236505162, 1064370040, 1925405905, 1068775490, 300636596, 1994976849, 136928363, 1001879028, 1776558442, 1118965621), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964907, 709707081, 1684019481, 662752809, 1924836516, 236505162, 1064370040, 1925405905, 1068775490, 300636596, 1994976849, 136928363, 1001879028, 1776558442, 1118965621), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707082, 1684019481, 662752809, 1924836516, 236505162, 1064370040, 1925405905, 1068775490, 300636596, 1994976849, 136928363, 1001879028, 1776558442, 1118965621)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707080, 1684019481, 662752809, 1924836516, 236505162, 1064370040, 1925405905, 1068775490, 300636596, 1994976849, 136928363, 1001879028, 1776558442, 1118965621)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707082, 1684019481, 662752809, 1924836516, 236505162, 1064370040, 1925405905, 1068775490, 300636596, 1994976849, 136928363, 1001879028, 1776558442, 1118965621), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707080, 1684019481, 662752809, 1924836516, 236505162, 1064370040, 1925405905, 1068775490, 300636596, 1994976849, 136928363, 1001879028, 1776558442, 1118965621), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019482, 662752809, 1924836516, 236505162, 1064370040, 1925405905, 1068775490, 300636596, 1994976849, 136928363, 1001879028, 1776558442, 1118965621)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019480, 662752809, 1924836516, 236505162, 1064370040, 1925405905, 1068775490, 300636596, 1994976849, 136928363, 1001879028, 1776558442, 1118965621)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019482, 662752809, 1924836516, 236505162, 1064370040, 1925405905, 1068775490, 300636596, 1994976849, 136928363, 1001879028, 1776558442, 1118965621), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019480, 662752809, 1924836516, 236505162, 1064370040, 1925405905, 1068775490, 300636596, 1994976849, 136928363, 1001879028, 1776558442, 1118965621), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752810, 1924836516, 236505162, 1064370040, 1925405905, 1068775490, 300636596, 1994976849, 136928363, 1001879028, 1776558442, 1118965621)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752808, 1924836516, 236505162, 1064370040, 1925405905, 1068775490, 300636596, 1994976849, 136928363, 1001879028, 1776558442, 1118965621)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752810, 1924836516, 236505162, 1064370040, 1925405905, 1068775490, 300636596, 1994976849, 136928363, 1001879028, 1776558442, 1118965621), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752808, 1924836516, 236505162, 1064370040, 1925405905, 1068775490, 300636596, 1994976849, 136928363, 1001879028, 1776558442, 1118965621), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752809, 1924836517, 236505162, 1064370040, 1925405905, 1068775490, 300636596, 1994976849, 136928363, 1001879028, 1776558442, 1118965621)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752809, 1924836515, 236505162, 1064370040, 1925405905, 1068775490, 300636596, 1994976849, 136928363, 1001879028, 1776558442, 1118965621)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752809, 1924836517, 236505162, 1064370040, 1925405905, 1068775490, 300636596, 1994976849, 136928363, 1001879028, 1776558442, 1118965621), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752809, 1924836515, 236505162, 1064370040, 1925405905, 1068775490, 300636596, 1994976849, 136928363, 1001879028, 1776558442, 1118965621), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752809, 1924836516, 236505163, 1064370040, 1925405905, 1068775490, 300636596, 1994976849, 136928363, 1001879028, 1776558442, 1118965621)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752809, 1924836516, 236505161, 1064370040, 1925405905, 1068775490, 300636596, 1994976849, 136928363, 1001879028, 1776558442, 1118965621)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752809, 1924836516, 236505163, 1064370040, 1925405905, 1068775490, 300636596, 1994976849, 136928363, 1001879028, 1776558442, 1118965621), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752809, 1924836516, 236505161, 1064370040, 1925405905, 1068775490, 300636596, 1994976849, 136928363, 1001879028, 1776558442, 1118965621), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752809, 1924836516, 236505162, 1064370041, 1925405905, 1068775490, 300636596, 1994976849, 136928363, 1001879028, 1776558442, 1118965621)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752809, 1924836516, 236505162, 1064370039, 1925405905, 1068775490, 300636596, 1994976849, 136928363, 1001879028, 1776558442, 1118965621)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752809, 1924836516, 236505162, 1064370041, 1925405905, 1068775490, 300636596, 1994976849, 136928363, 1001879028, 1776558442, 1118965621), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752809, 1924836516, 236505162, 1064370039, 1925405905, 1068775490, 300636596, 1994976849, 136928363, 1001879028, 1776558442, 1118965621), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752809, 1924836516, 236505162, 1064370040, 1925405906, 1068775490, 300636596, 1994976849, 136928363, 1001879028, 1776558442, 1118965621)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752809, 1924836516, 236505162, 1064370040, 1925405904, 1068775490, 300636596, 1994976849, 136928363, 1001879028, 1776558442, 1118965621)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752809, 1924836516, 236505162, 1064370040, 1925405906, 1068775490, 300636596, 1994976849, 136928363, 1001879028, 1776558442, 1118965621), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752809, 1924836516, 236505162, 1064370040, 1925405904, 1068775490, 300636596, 1994976849, 136928363, 1001879028, 1776558442, 1118965621), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752809, 1924836516, 236505162, 1064370040, 1925405905, 1068775491, 300636596, 1994976849, 136928363, 1001879028, 1776558442, 1118965621)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752809, 1924836516, 236505162, 1064370040, 1925405905, 1068775489, 300636596, 1994976849, 136928363, 1001879028, 1776558442, 1118965621)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752809, 1924836516, 236505162, 1064370040, 1925405905, 1068775491, 300636596, 1994976849, 136928363, 1001879028, 1776558442, 1118965621), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752809, 1924836516, 236505162, 1064370040, 1925405905, 1068775489, 300636596, 1994976849, 136928363, 1001879028, 1776558442, 1118965621), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752809, 1924836516, 236505162, 1064370040, 1925405905, 1068775490, 300636597, 1994976849, 136928363, 1001879028, 1776558442, 1118965621)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752809, 1924836516, 236505162, 1064370040, 1925405905, 1068775490, 300636595, 1994976849, 136928363, 1001879028, 1776558442, 1118965621)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752809, 1924836516, 236505162, 1064370040, 1925405905, 1068775490, 300636597, 1994976849, 136928363, 1001879028, 1776558442, 1118965621), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752809, 1924836516, 236505162, 1064370040, 1925405905, 1068775490, 300636595, 1994976849, 136928363, 1001879028, 1776558442, 1118965621), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752809, 1924836516, 236505162, 1064370040, 1925405905, 1068775490, 300636596, 1994976850, 136928363, 1001879028, 1776558442, 1118965621)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752809, 1924836516, 236505162, 1064370040, 1925405905, 1068775490, 300636596, 1994976848, 136928363, 1001879028, 1776558442, 1118965621)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752809, 1924836516, 236505162, 1064370040, 1925405905, 1068775490, 300636596, 1994976850, 136928363, 1001879028, 1776558442, 1118965621), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752809, 1924836516, 236505162, 1064370040, 1925405905, 1068775490, 300636596, 1994976848, 136928363, 1001879028, 1776558442, 1118965621), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752809, 1924836516, 236505162, 1064370040, 1925405905, 1068775490, 300636596, 1994976849, 136928364, 1001879028, 1776558442, 1118965621)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752809, 1924836516, 236505162, 1064370040, 1925405905, 1068775490, 300636596, 1994976849, 136928362, 1001879028, 1776558442, 1118965621)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752809, 1924836516, 236505162, 1064370040, 1925405905, 1068775490, 300636596, 1994976849, 136928364, 1001879028, 1776558442, 1118965621), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752809, 1924836516, 236505162, 1064370040, 1925405905, 1068775490, 300636596, 1994976849, 136928362, 1001879028, 1776558442, 1118965621), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752809, 1924836516, 236505162, 1064370040, 1925405905, 1068775490, 300636596, 1994976849, 136928363, 1001879029, 1776558442, 1118965621)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752809, 1924836516, 236505162, 1064370040, 1925405905, 1068775490, 300636596, 1994976849, 136928363, 1001879027, 1776558442, 1118965621)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752809, 1924836516, 236505162, 1064370040, 1925405905, 1068775490, 300636596, 1994976849, 136928363, 1001879029, 1776558442, 1118965621), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752809, 1924836516, 236505162, 1064370040, 1925405905, 1068775490, 300636596, 1994976849, 136928363, 1001879027, 1776558442, 1118965621), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752809, 1924836516, 236505162, 1064370040, 1925405905, 1068775490, 300636596, 1994976849, 136928363, 1001879028, 1776558443, 1118965621)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752809, 1924836516, 236505162, 1064370040, 1925405905, 1068775490, 300636596, 1994976849, 136928363, 1001879028, 1776558441, 1118965621)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752809, 1924836516, 236505162, 1064370040, 1925405905, 1068775490, 300636596, 1994976849, 136928363, 1001879028, 1776558443, 1118965621), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752809, 1924836516, 236505162, 1064370040, 1925405905, 1068775490, 300636596, 1994976849, 136928363, 1001879028, 1776558441, 1118965621), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752809, 1924836516, 236505162, 1064370040, 1925405905, 1068775490, 300636596, 1994976849, 136928363, 1001879028, 1776558442, 1118965622)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752809, 1924836516, 236505162, 1064370040, 1925405905, 1068775490, 300636596, 1994976849, 136928363, 1001879028, 1776558442, 1118965620)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752809, 1924836516, 236505162, 1064370040, 1925405905, 1068775490, 300636596, 1994976849, 136928363, 1001879028, 1776558442, 1118965622), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1862964908, 709707081, 1684019481, 662752809, 1924836516, 236505162, 1064370040, 1925405905, 1068775490, 300636596, 1994976849, 136928363, 1001879028, 1776558442, 1118965620), Comparer<object>.Default) > 0);

            Assert.IsFalse(args1 == args2);
            Assert.IsTrue(args1 != args2);

            Assert.AreNotEqual(args1, "foo");
            Assert.AreNotEqual(args1, null);

            var seq1 = (IStructuralEquatable)args1;
            Assert.AreEqual(args1.GetHashCode(), seq1.GetHashCode(EqualityComparer<object>.Default));
            Assert.AreEqual(0, seq1.GetHashCode(new ConstantHashEqualityComparer(0)));
            Assert.IsTrue(seq1.Equals(args1, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals(args2, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals("foo", EqualityComparer<object>.Default));

            Assert.AreEqual("(1862964908, 709707081, 1684019481, 662752809, 1924836516, 236505162, 1064370040, 1925405905, 1068775490, 300636596, 1994976849, 136928363, 1001879028, 1776558442, 1118965621)", args1.ToString());
            Assert.AreEqual("(1448082938, 65597502, 839856988, 196611263, 1085392957, 485021651, 622503358, 1153346442, 1669482955, 161975470, 1760847106, 656561325, 2101761346, 1643898504, 1101583865)", args2.ToString());
        }

        [TestMethod]
        public void Tuplet16()
        {
            var args1 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421871, 1331036488, 2130610971, 534555286, 632810145, 1113625852, 1205077457, 1357564654, 1823479611, 328001364, 1304559037, 2023074705);
            var args2 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(183081128, 625056566, 1755071974, 1740918513, 1416915494, 1333914416, 1701436564, 2013887079, 1214719941, 1240461550, 1703844286, 14536526, 500777339, 163989410, 1727427484, 1110092341);

            Assert.AreEqual(894414248, args1.Item1);
            Assert.AreEqual(183081128, args2.Item1);
            Assert.AreEqual(1480982783, args1.Item2);
            Assert.AreEqual(625056566, args2.Item2);
            Assert.AreEqual(1214195623, args1.Item3);
            Assert.AreEqual(1755071974, args2.Item3);
            Assert.AreEqual(118740352, args1.Item4);
            Assert.AreEqual(1740918513, args2.Item4);
            Assert.AreEqual(818421871, args1.Item5);
            Assert.AreEqual(1416915494, args2.Item5);
            Assert.AreEqual(1331036488, args1.Item6);
            Assert.AreEqual(1333914416, args2.Item6);
            Assert.AreEqual(2130610971, args1.Item7);
            Assert.AreEqual(1701436564, args2.Item7);
            Assert.AreEqual(534555286, args1.Item8);
            Assert.AreEqual(2013887079, args2.Item8);
            Assert.AreEqual(632810145, args1.Item9);
            Assert.AreEqual(1214719941, args2.Item9);
            Assert.AreEqual(1113625852, args1.Item10);
            Assert.AreEqual(1240461550, args2.Item10);
            Assert.AreEqual(1205077457, args1.Item11);
            Assert.AreEqual(1703844286, args2.Item11);
            Assert.AreEqual(1357564654, args1.Item12);
            Assert.AreEqual(14536526, args2.Item12);
            Assert.AreEqual(1823479611, args1.Item13);
            Assert.AreEqual(500777339, args2.Item13);
            Assert.AreEqual(328001364, args1.Item14);
            Assert.AreEqual(163989410, args2.Item14);
            Assert.AreEqual(1304559037, args1.Item15);
            Assert.AreEqual(1727427484, args2.Item15);
            Assert.AreEqual(2023074705, args1.Item16);
            Assert.AreEqual(1110092341, args2.Item16);

            Assert.AreEqual(args1, args1);

            var copy1 = args1;
            Assert.IsTrue(args1 == copy1);
            Assert.IsFalse(args1 != copy1);

            Assert.AreEqual(0, args1.CompareTo(copy1));

            Assert.AreNotEqual(args1.GetHashCode(), args2.GetHashCode());
            Assert.AreNotEqual(args1, args2);
            Assert.AreNotEqual(args2, args1);

            var scm1 = (IStructuralComparable)args1;
            var cmp1 = (IComparable)args1;

            Assert.AreNotEqual(0, args1.CompareTo(args2));
            Assert.AreNotEqual(0, scm1.CompareTo(args2, Comparer<object>.Default));
            Assert.AreNotEqual(0, cmp1.CompareTo(args2));
            Assert.AreEqual(1, scm1.CompareTo(other: null, Comparer<object>.Default));

            Assert.ThrowsException<ArgumentException>(() => scm1.CompareTo("foo", Comparer<object>.Default));

            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414249, 1480982783, 1214195623, 118740352, 818421871, 1331036488, 2130610971, 534555286, 632810145, 1113625852, 1205077457, 1357564654, 1823479611, 328001364, 1304559037, 2023074705)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414247, 1480982783, 1214195623, 118740352, 818421871, 1331036488, 2130610971, 534555286, 632810145, 1113625852, 1205077457, 1357564654, 1823479611, 328001364, 1304559037, 2023074705)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414249, 1480982783, 1214195623, 118740352, 818421871, 1331036488, 2130610971, 534555286, 632810145, 1113625852, 1205077457, 1357564654, 1823479611, 328001364, 1304559037, 2023074705), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414247, 1480982783, 1214195623, 118740352, 818421871, 1331036488, 2130610971, 534555286, 632810145, 1113625852, 1205077457, 1357564654, 1823479611, 328001364, 1304559037, 2023074705), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982784, 1214195623, 118740352, 818421871, 1331036488, 2130610971, 534555286, 632810145, 1113625852, 1205077457, 1357564654, 1823479611, 328001364, 1304559037, 2023074705)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982782, 1214195623, 118740352, 818421871, 1331036488, 2130610971, 534555286, 632810145, 1113625852, 1205077457, 1357564654, 1823479611, 328001364, 1304559037, 2023074705)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982784, 1214195623, 118740352, 818421871, 1331036488, 2130610971, 534555286, 632810145, 1113625852, 1205077457, 1357564654, 1823479611, 328001364, 1304559037, 2023074705), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982782, 1214195623, 118740352, 818421871, 1331036488, 2130610971, 534555286, 632810145, 1113625852, 1205077457, 1357564654, 1823479611, 328001364, 1304559037, 2023074705), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195624, 118740352, 818421871, 1331036488, 2130610971, 534555286, 632810145, 1113625852, 1205077457, 1357564654, 1823479611, 328001364, 1304559037, 2023074705)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195622, 118740352, 818421871, 1331036488, 2130610971, 534555286, 632810145, 1113625852, 1205077457, 1357564654, 1823479611, 328001364, 1304559037, 2023074705)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195624, 118740352, 818421871, 1331036488, 2130610971, 534555286, 632810145, 1113625852, 1205077457, 1357564654, 1823479611, 328001364, 1304559037, 2023074705), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195622, 118740352, 818421871, 1331036488, 2130610971, 534555286, 632810145, 1113625852, 1205077457, 1357564654, 1823479611, 328001364, 1304559037, 2023074705), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740353, 818421871, 1331036488, 2130610971, 534555286, 632810145, 1113625852, 1205077457, 1357564654, 1823479611, 328001364, 1304559037, 2023074705)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740351, 818421871, 1331036488, 2130610971, 534555286, 632810145, 1113625852, 1205077457, 1357564654, 1823479611, 328001364, 1304559037, 2023074705)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740353, 818421871, 1331036488, 2130610971, 534555286, 632810145, 1113625852, 1205077457, 1357564654, 1823479611, 328001364, 1304559037, 2023074705), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740351, 818421871, 1331036488, 2130610971, 534555286, 632810145, 1113625852, 1205077457, 1357564654, 1823479611, 328001364, 1304559037, 2023074705), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421872, 1331036488, 2130610971, 534555286, 632810145, 1113625852, 1205077457, 1357564654, 1823479611, 328001364, 1304559037, 2023074705)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421870, 1331036488, 2130610971, 534555286, 632810145, 1113625852, 1205077457, 1357564654, 1823479611, 328001364, 1304559037, 2023074705)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421872, 1331036488, 2130610971, 534555286, 632810145, 1113625852, 1205077457, 1357564654, 1823479611, 328001364, 1304559037, 2023074705), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421870, 1331036488, 2130610971, 534555286, 632810145, 1113625852, 1205077457, 1357564654, 1823479611, 328001364, 1304559037, 2023074705), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421871, 1331036489, 2130610971, 534555286, 632810145, 1113625852, 1205077457, 1357564654, 1823479611, 328001364, 1304559037, 2023074705)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421871, 1331036487, 2130610971, 534555286, 632810145, 1113625852, 1205077457, 1357564654, 1823479611, 328001364, 1304559037, 2023074705)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421871, 1331036489, 2130610971, 534555286, 632810145, 1113625852, 1205077457, 1357564654, 1823479611, 328001364, 1304559037, 2023074705), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421871, 1331036487, 2130610971, 534555286, 632810145, 1113625852, 1205077457, 1357564654, 1823479611, 328001364, 1304559037, 2023074705), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421871, 1331036488, 2130610972, 534555286, 632810145, 1113625852, 1205077457, 1357564654, 1823479611, 328001364, 1304559037, 2023074705)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421871, 1331036488, 2130610970, 534555286, 632810145, 1113625852, 1205077457, 1357564654, 1823479611, 328001364, 1304559037, 2023074705)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421871, 1331036488, 2130610972, 534555286, 632810145, 1113625852, 1205077457, 1357564654, 1823479611, 328001364, 1304559037, 2023074705), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421871, 1331036488, 2130610970, 534555286, 632810145, 1113625852, 1205077457, 1357564654, 1823479611, 328001364, 1304559037, 2023074705), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421871, 1331036488, 2130610971, 534555287, 632810145, 1113625852, 1205077457, 1357564654, 1823479611, 328001364, 1304559037, 2023074705)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421871, 1331036488, 2130610971, 534555285, 632810145, 1113625852, 1205077457, 1357564654, 1823479611, 328001364, 1304559037, 2023074705)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421871, 1331036488, 2130610971, 534555287, 632810145, 1113625852, 1205077457, 1357564654, 1823479611, 328001364, 1304559037, 2023074705), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421871, 1331036488, 2130610971, 534555285, 632810145, 1113625852, 1205077457, 1357564654, 1823479611, 328001364, 1304559037, 2023074705), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421871, 1331036488, 2130610971, 534555286, 632810146, 1113625852, 1205077457, 1357564654, 1823479611, 328001364, 1304559037, 2023074705)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421871, 1331036488, 2130610971, 534555286, 632810144, 1113625852, 1205077457, 1357564654, 1823479611, 328001364, 1304559037, 2023074705)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421871, 1331036488, 2130610971, 534555286, 632810146, 1113625852, 1205077457, 1357564654, 1823479611, 328001364, 1304559037, 2023074705), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421871, 1331036488, 2130610971, 534555286, 632810144, 1113625852, 1205077457, 1357564654, 1823479611, 328001364, 1304559037, 2023074705), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421871, 1331036488, 2130610971, 534555286, 632810145, 1113625853, 1205077457, 1357564654, 1823479611, 328001364, 1304559037, 2023074705)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421871, 1331036488, 2130610971, 534555286, 632810145, 1113625851, 1205077457, 1357564654, 1823479611, 328001364, 1304559037, 2023074705)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421871, 1331036488, 2130610971, 534555286, 632810145, 1113625853, 1205077457, 1357564654, 1823479611, 328001364, 1304559037, 2023074705), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421871, 1331036488, 2130610971, 534555286, 632810145, 1113625851, 1205077457, 1357564654, 1823479611, 328001364, 1304559037, 2023074705), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421871, 1331036488, 2130610971, 534555286, 632810145, 1113625852, 1205077458, 1357564654, 1823479611, 328001364, 1304559037, 2023074705)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421871, 1331036488, 2130610971, 534555286, 632810145, 1113625852, 1205077456, 1357564654, 1823479611, 328001364, 1304559037, 2023074705)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421871, 1331036488, 2130610971, 534555286, 632810145, 1113625852, 1205077458, 1357564654, 1823479611, 328001364, 1304559037, 2023074705), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421871, 1331036488, 2130610971, 534555286, 632810145, 1113625852, 1205077456, 1357564654, 1823479611, 328001364, 1304559037, 2023074705), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421871, 1331036488, 2130610971, 534555286, 632810145, 1113625852, 1205077457, 1357564655, 1823479611, 328001364, 1304559037, 2023074705)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421871, 1331036488, 2130610971, 534555286, 632810145, 1113625852, 1205077457, 1357564653, 1823479611, 328001364, 1304559037, 2023074705)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421871, 1331036488, 2130610971, 534555286, 632810145, 1113625852, 1205077457, 1357564655, 1823479611, 328001364, 1304559037, 2023074705), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421871, 1331036488, 2130610971, 534555286, 632810145, 1113625852, 1205077457, 1357564653, 1823479611, 328001364, 1304559037, 2023074705), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421871, 1331036488, 2130610971, 534555286, 632810145, 1113625852, 1205077457, 1357564654, 1823479612, 328001364, 1304559037, 2023074705)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421871, 1331036488, 2130610971, 534555286, 632810145, 1113625852, 1205077457, 1357564654, 1823479610, 328001364, 1304559037, 2023074705)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421871, 1331036488, 2130610971, 534555286, 632810145, 1113625852, 1205077457, 1357564654, 1823479612, 328001364, 1304559037, 2023074705), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421871, 1331036488, 2130610971, 534555286, 632810145, 1113625852, 1205077457, 1357564654, 1823479610, 328001364, 1304559037, 2023074705), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421871, 1331036488, 2130610971, 534555286, 632810145, 1113625852, 1205077457, 1357564654, 1823479611, 328001365, 1304559037, 2023074705)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421871, 1331036488, 2130610971, 534555286, 632810145, 1113625852, 1205077457, 1357564654, 1823479611, 328001363, 1304559037, 2023074705)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421871, 1331036488, 2130610971, 534555286, 632810145, 1113625852, 1205077457, 1357564654, 1823479611, 328001365, 1304559037, 2023074705), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421871, 1331036488, 2130610971, 534555286, 632810145, 1113625852, 1205077457, 1357564654, 1823479611, 328001363, 1304559037, 2023074705), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421871, 1331036488, 2130610971, 534555286, 632810145, 1113625852, 1205077457, 1357564654, 1823479611, 328001364, 1304559038, 2023074705)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421871, 1331036488, 2130610971, 534555286, 632810145, 1113625852, 1205077457, 1357564654, 1823479611, 328001364, 1304559036, 2023074705)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421871, 1331036488, 2130610971, 534555286, 632810145, 1113625852, 1205077457, 1357564654, 1823479611, 328001364, 1304559038, 2023074705), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421871, 1331036488, 2130610971, 534555286, 632810145, 1113625852, 1205077457, 1357564654, 1823479611, 328001364, 1304559036, 2023074705), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421871, 1331036488, 2130610971, 534555286, 632810145, 1113625852, 1205077457, 1357564654, 1823479611, 328001364, 1304559037, 2023074706)) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421871, 1331036488, 2130610971, 534555286, 632810145, 1113625852, 1205077457, 1357564654, 1823479611, 328001364, 1304559037, 2023074704)) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421871, 1331036488, 2130610971, 534555286, 632810145, 1113625852, 1205077457, 1357564654, 1823479611, 328001364, 1304559037, 2023074706), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(894414248, 1480982783, 1214195623, 118740352, 818421871, 1331036488, 2130610971, 534555286, 632810145, 1113625852, 1205077457, 1357564654, 1823479611, 328001364, 1304559037, 2023074704), Comparer<object>.Default) > 0);

            Assert.IsFalse(args1 == args2);
            Assert.IsTrue(args1 != args2);

            Assert.AreNotEqual(args1, "foo");
            Assert.AreNotEqual(args1, null);

            var seq1 = (IStructuralEquatable)args1;
            Assert.AreEqual(args1.GetHashCode(), seq1.GetHashCode(EqualityComparer<object>.Default));
            Assert.AreEqual(0, seq1.GetHashCode(new ConstantHashEqualityComparer(0)));
            Assert.IsTrue(seq1.Equals(args1, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals(args2, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals("foo", EqualityComparer<object>.Default));

            Assert.AreEqual("(894414248, 1480982783, 1214195623, 118740352, 818421871, 1331036488, 2130610971, 534555286, 632810145, 1113625852, 1205077457, 1357564654, 1823479611, 328001364, 1304559037, 2023074705)", args1.ToString());
            Assert.AreEqual("(183081128, 625056566, 1755071974, 1740918513, 1416915494, 1333914416, 1701436564, 2013887079, 1214719941, 1240461550, 1703844286, 14536526, 500777339, 163989410, 1727427484, 1110092341)", args2.ToString());
        }

        [TestMethod]
        public void Tuplet17()
        {
            var args1 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int>>(281507401, 2114675272, 1553705995, 513994066, 1070216387, 883138676, 958136571, 1935412780, 1464955614, 1678525863, 207046843, 1230469058, 2027799147, 1274940644, 946507394, 825345078, new Tuplet<int>(364923918));
            var args2 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int>>(286384412, 1577765978, 31504759, 346689372, 2050463638, 1832152018, 1707983479, 1927029866, 1347792191, 1051504058, 1725443968, 1774675849, 2116074445, 33777947, 468820735, 1533682015, new Tuplet<int>(94985116));

            Assert.AreEqual(281507401, args1.Item1);
            Assert.AreEqual(286384412, args2.Item1);
            Assert.AreEqual(2114675272, args1.Item2);
            Assert.AreEqual(1577765978, args2.Item2);
            Assert.AreEqual(1553705995, args1.Item3);
            Assert.AreEqual(31504759, args2.Item3);
            Assert.AreEqual(513994066, args1.Item4);
            Assert.AreEqual(346689372, args2.Item4);
            Assert.AreEqual(1070216387, args1.Item5);
            Assert.AreEqual(2050463638, args2.Item5);
            Assert.AreEqual(883138676, args1.Item6);
            Assert.AreEqual(1832152018, args2.Item6);
            Assert.AreEqual(958136571, args1.Item7);
            Assert.AreEqual(1707983479, args2.Item7);
            Assert.AreEqual(1935412780, args1.Item8);
            Assert.AreEqual(1927029866, args2.Item8);
            Assert.AreEqual(1464955614, args1.Item9);
            Assert.AreEqual(1347792191, args2.Item9);
            Assert.AreEqual(1678525863, args1.Item10);
            Assert.AreEqual(1051504058, args2.Item10);
            Assert.AreEqual(207046843, args1.Item11);
            Assert.AreEqual(1725443968, args2.Item11);
            Assert.AreEqual(1230469058, args1.Item12);
            Assert.AreEqual(1774675849, args2.Item12);
            Assert.AreEqual(2027799147, args1.Item13);
            Assert.AreEqual(2116074445, args2.Item13);
            Assert.AreEqual(1274940644, args1.Item14);
            Assert.AreEqual(33777947, args2.Item14);
            Assert.AreEqual(946507394, args1.Item15);
            Assert.AreEqual(468820735, args2.Item15);
            Assert.AreEqual(825345078, args1.Item16);
            Assert.AreEqual(1533682015, args2.Item16);
            Assert.AreEqual(364923918, args1.Rest.Item1);
            Assert.AreEqual(94985116, args2.Rest.Item1);

            Assert.AreEqual(args1, args1);

            var copy1 = args1;
            Assert.IsTrue(args1 == copy1);
            Assert.IsFalse(args1 != copy1);

            Assert.AreNotEqual(args1.GetHashCode(), args2.GetHashCode());
            Assert.AreNotEqual(args1, args2);
            Assert.AreNotEqual(args2, args1);

            var scm1 = (IStructuralComparable)args1;
            var cmp1 = (IComparable)args1;

            Assert.AreNotEqual(0, args1.CompareTo(args2));
            Assert.AreNotEqual(0, scm1.CompareTo(args2, Comparer<object>.Default));
            Assert.AreNotEqual(0, cmp1.CompareTo(args2));
            Assert.AreEqual(1, scm1.CompareTo(other: null, Comparer<object>.Default));

            Assert.ThrowsException<ArgumentException>(() => scm1.CompareTo("foo", Comparer<object>.Default));

            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int>>(281507402, 2114675272, 1553705995, 513994066, 1070216387, 883138676, 958136571, 1935412780, 1464955614, 1678525863, 207046843, 1230469058, 2027799147, 1274940644, 946507394, 825345078, new Tuplet<int>(364923918))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int>>(281507400, 2114675272, 1553705995, 513994066, 1070216387, 883138676, 958136571, 1935412780, 1464955614, 1678525863, 207046843, 1230469058, 2027799147, 1274940644, 946507394, 825345078, new Tuplet<int>(364923918))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int>>(281507402, 2114675272, 1553705995, 513994066, 1070216387, 883138676, 958136571, 1935412780, 1464955614, 1678525863, 207046843, 1230469058, 2027799147, 1274940644, 946507394, 825345078, new Tuplet<int>(364923918)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int>>(281507400, 2114675272, 1553705995, 513994066, 1070216387, 883138676, 958136571, 1935412780, 1464955614, 1678525863, 207046843, 1230469058, 2027799147, 1274940644, 946507394, 825345078, new Tuplet<int>(364923918)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int>>(281507401, 2114675273, 1553705995, 513994066, 1070216387, 883138676, 958136571, 1935412780, 1464955614, 1678525863, 207046843, 1230469058, 2027799147, 1274940644, 946507394, 825345078, new Tuplet<int>(364923918))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int>>(281507401, 2114675271, 1553705995, 513994066, 1070216387, 883138676, 958136571, 1935412780, 1464955614, 1678525863, 207046843, 1230469058, 2027799147, 1274940644, 946507394, 825345078, new Tuplet<int>(364923918))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int>>(281507401, 2114675273, 1553705995, 513994066, 1070216387, 883138676, 958136571, 1935412780, 1464955614, 1678525863, 207046843, 1230469058, 2027799147, 1274940644, 946507394, 825345078, new Tuplet<int>(364923918)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int>>(281507401, 2114675271, 1553705995, 513994066, 1070216387, 883138676, 958136571, 1935412780, 1464955614, 1678525863, 207046843, 1230469058, 2027799147, 1274940644, 946507394, 825345078, new Tuplet<int>(364923918)), Comparer<object>.Default) > 0);

            Assert.IsFalse(args1 == args2);
            Assert.IsTrue(args1 != args2);

            Assert.AreNotEqual(args1, "foo");
            Assert.AreNotEqual(args1, null);

            var seq1 = (IStructuralEquatable)args1;
            Assert.AreEqual(args1.GetHashCode(), seq1.GetHashCode(EqualityComparer<object>.Default));
            Assert.AreEqual(0, seq1.GetHashCode(new ConstantHashEqualityComparer(0)));
            Assert.IsTrue(seq1.Equals(args1, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals(args2, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals("foo", EqualityComparer<object>.Default));

            Assert.AreEqual("(281507401, 2114675272, 1553705995, 513994066, 1070216387, 883138676, 958136571, 1935412780, 1464955614, 1678525863, 207046843, 1230469058, 2027799147, 1274940644, 946507394, 825345078, 364923918)", args1.ToString());
            Assert.AreEqual("(286384412, 1577765978, 31504759, 346689372, 2050463638, 1832152018, 1707983479, 1927029866, 1347792191, 1051504058, 1725443968, 1774675849, 2116074445, 33777947, 468820735, 1533682015, 94985116)", args2.ToString());
        }

        [TestMethod]
        public void Tuplet18()
        {
            var args1 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int>>(1076057253, 1856287986, 921779016, 790564971, 952858318, 1447426099, 1814403642, 1967142841, 275962899, 1885873278, 1126867573, 470967506, 2133571579, 2087262944, 293954156, 878499208, new Tuplet<int, int>(1797096255, 214392927));
            var args2 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int>>(733707079, 1695922725, 763402969, 378527410, 282523254, 1993206163, 734447847, 1869907843, 1979118265, 1380176250, 160736931, 1496364816, 1644747916, 1885709755, 1844270690, 1932814031, new Tuplet<int, int>(198883391, 1237703055));

            Assert.AreEqual(1076057253, args1.Item1);
            Assert.AreEqual(733707079, args2.Item1);
            Assert.AreEqual(1856287986, args1.Item2);
            Assert.AreEqual(1695922725, args2.Item2);
            Assert.AreEqual(921779016, args1.Item3);
            Assert.AreEqual(763402969, args2.Item3);
            Assert.AreEqual(790564971, args1.Item4);
            Assert.AreEqual(378527410, args2.Item4);
            Assert.AreEqual(952858318, args1.Item5);
            Assert.AreEqual(282523254, args2.Item5);
            Assert.AreEqual(1447426099, args1.Item6);
            Assert.AreEqual(1993206163, args2.Item6);
            Assert.AreEqual(1814403642, args1.Item7);
            Assert.AreEqual(734447847, args2.Item7);
            Assert.AreEqual(1967142841, args1.Item8);
            Assert.AreEqual(1869907843, args2.Item8);
            Assert.AreEqual(275962899, args1.Item9);
            Assert.AreEqual(1979118265, args2.Item9);
            Assert.AreEqual(1885873278, args1.Item10);
            Assert.AreEqual(1380176250, args2.Item10);
            Assert.AreEqual(1126867573, args1.Item11);
            Assert.AreEqual(160736931, args2.Item11);
            Assert.AreEqual(470967506, args1.Item12);
            Assert.AreEqual(1496364816, args2.Item12);
            Assert.AreEqual(2133571579, args1.Item13);
            Assert.AreEqual(1644747916, args2.Item13);
            Assert.AreEqual(2087262944, args1.Item14);
            Assert.AreEqual(1885709755, args2.Item14);
            Assert.AreEqual(293954156, args1.Item15);
            Assert.AreEqual(1844270690, args2.Item15);
            Assert.AreEqual(878499208, args1.Item16);
            Assert.AreEqual(1932814031, args2.Item16);
            Assert.AreEqual(1797096255, args1.Rest.Item1);
            Assert.AreEqual(198883391, args2.Rest.Item1);
            Assert.AreEqual(214392927, args1.Rest.Item2);
            Assert.AreEqual(1237703055, args2.Rest.Item2);

            Assert.AreEqual(args1, args1);

            var copy1 = args1;
            Assert.IsTrue(args1 == copy1);
            Assert.IsFalse(args1 != copy1);

            Assert.AreNotEqual(args1.GetHashCode(), args2.GetHashCode());
            Assert.AreNotEqual(args1, args2);
            Assert.AreNotEqual(args2, args1);

            var scm1 = (IStructuralComparable)args1;
            var cmp1 = (IComparable)args1;

            Assert.AreNotEqual(0, args1.CompareTo(args2));
            Assert.AreNotEqual(0, scm1.CompareTo(args2, Comparer<object>.Default));
            Assert.AreNotEqual(0, cmp1.CompareTo(args2));
            Assert.AreEqual(1, scm1.CompareTo(other: null, Comparer<object>.Default));

            Assert.ThrowsException<ArgumentException>(() => scm1.CompareTo("foo", Comparer<object>.Default));

            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int>>(1076057254, 1856287986, 921779016, 790564971, 952858318, 1447426099, 1814403642, 1967142841, 275962899, 1885873278, 1126867573, 470967506, 2133571579, 2087262944, 293954156, 878499208, new Tuplet<int, int>(1797096255, 214392927))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int>>(1076057252, 1856287986, 921779016, 790564971, 952858318, 1447426099, 1814403642, 1967142841, 275962899, 1885873278, 1126867573, 470967506, 2133571579, 2087262944, 293954156, 878499208, new Tuplet<int, int>(1797096255, 214392927))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int>>(1076057254, 1856287986, 921779016, 790564971, 952858318, 1447426099, 1814403642, 1967142841, 275962899, 1885873278, 1126867573, 470967506, 2133571579, 2087262944, 293954156, 878499208, new Tuplet<int, int>(1797096255, 214392927)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int>>(1076057252, 1856287986, 921779016, 790564971, 952858318, 1447426099, 1814403642, 1967142841, 275962899, 1885873278, 1126867573, 470967506, 2133571579, 2087262944, 293954156, 878499208, new Tuplet<int, int>(1797096255, 214392927)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int>>(1076057253, 1856287987, 921779016, 790564971, 952858318, 1447426099, 1814403642, 1967142841, 275962899, 1885873278, 1126867573, 470967506, 2133571579, 2087262944, 293954156, 878499208, new Tuplet<int, int>(1797096255, 214392927))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int>>(1076057253, 1856287985, 921779016, 790564971, 952858318, 1447426099, 1814403642, 1967142841, 275962899, 1885873278, 1126867573, 470967506, 2133571579, 2087262944, 293954156, 878499208, new Tuplet<int, int>(1797096255, 214392927))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int>>(1076057253, 1856287987, 921779016, 790564971, 952858318, 1447426099, 1814403642, 1967142841, 275962899, 1885873278, 1126867573, 470967506, 2133571579, 2087262944, 293954156, 878499208, new Tuplet<int, int>(1797096255, 214392927)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int>>(1076057253, 1856287985, 921779016, 790564971, 952858318, 1447426099, 1814403642, 1967142841, 275962899, 1885873278, 1126867573, 470967506, 2133571579, 2087262944, 293954156, 878499208, new Tuplet<int, int>(1797096255, 214392927)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int>>(1076057253, 1856287986, 921779017, 790564971, 952858318, 1447426099, 1814403642, 1967142841, 275962899, 1885873278, 1126867573, 470967506, 2133571579, 2087262944, 293954156, 878499208, new Tuplet<int, int>(1797096255, 214392927))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int>>(1076057253, 1856287986, 921779015, 790564971, 952858318, 1447426099, 1814403642, 1967142841, 275962899, 1885873278, 1126867573, 470967506, 2133571579, 2087262944, 293954156, 878499208, new Tuplet<int, int>(1797096255, 214392927))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int>>(1076057253, 1856287986, 921779017, 790564971, 952858318, 1447426099, 1814403642, 1967142841, 275962899, 1885873278, 1126867573, 470967506, 2133571579, 2087262944, 293954156, 878499208, new Tuplet<int, int>(1797096255, 214392927)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int>>(1076057253, 1856287986, 921779015, 790564971, 952858318, 1447426099, 1814403642, 1967142841, 275962899, 1885873278, 1126867573, 470967506, 2133571579, 2087262944, 293954156, 878499208, new Tuplet<int, int>(1797096255, 214392927)), Comparer<object>.Default) > 0);

            Assert.IsFalse(args1 == args2);
            Assert.IsTrue(args1 != args2);

            Assert.AreNotEqual(args1, "foo");
            Assert.AreNotEqual(args1, null);

            var seq1 = (IStructuralEquatable)args1;
            Assert.AreEqual(args1.GetHashCode(), seq1.GetHashCode(EqualityComparer<object>.Default));
            Assert.AreEqual(0, seq1.GetHashCode(new ConstantHashEqualityComparer(0)));
            Assert.IsTrue(seq1.Equals(args1, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals(args2, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals("foo", EqualityComparer<object>.Default));

            Assert.AreEqual("(1076057253, 1856287986, 921779016, 790564971, 952858318, 1447426099, 1814403642, 1967142841, 275962899, 1885873278, 1126867573, 470967506, 2133571579, 2087262944, 293954156, 878499208, 1797096255, 214392927)", args1.ToString());
            Assert.AreEqual("(733707079, 1695922725, 763402969, 378527410, 282523254, 1993206163, 734447847, 1869907843, 1979118265, 1380176250, 160736931, 1496364816, 1644747916, 1885709755, 1844270690, 1932814031, 198883391, 1237703055)", args2.ToString());
        }

        [TestMethod]
        public void Tuplet19()
        {
            var args1 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int>>(2051049709, 1721842594, 1481009741, 130339879, 364584764, 527030178, 1774500739, 2093762387, 581115906, 1456062360, 1361704259, 1111724761, 1431489812, 896176641, 318978190, 1966868667, new Tuplet<int, int, int>(1882597303, 1985242937, 1479065794));
            var args2 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int>>(697529843, 1573764732, 1076056500, 56117124, 1230434122, 1615791481, 434227392, 1806405910, 927081730, 241125362, 1388641465, 774180463, 200757548, 1888379553, 1203734748, 974933146, new Tuplet<int, int, int>(75253661, 880866833, 603367200));

            Assert.AreEqual(2051049709, args1.Item1);
            Assert.AreEqual(697529843, args2.Item1);
            Assert.AreEqual(1721842594, args1.Item2);
            Assert.AreEqual(1573764732, args2.Item2);
            Assert.AreEqual(1481009741, args1.Item3);
            Assert.AreEqual(1076056500, args2.Item3);
            Assert.AreEqual(130339879, args1.Item4);
            Assert.AreEqual(56117124, args2.Item4);
            Assert.AreEqual(364584764, args1.Item5);
            Assert.AreEqual(1230434122, args2.Item5);
            Assert.AreEqual(527030178, args1.Item6);
            Assert.AreEqual(1615791481, args2.Item6);
            Assert.AreEqual(1774500739, args1.Item7);
            Assert.AreEqual(434227392, args2.Item7);
            Assert.AreEqual(2093762387, args1.Item8);
            Assert.AreEqual(1806405910, args2.Item8);
            Assert.AreEqual(581115906, args1.Item9);
            Assert.AreEqual(927081730, args2.Item9);
            Assert.AreEqual(1456062360, args1.Item10);
            Assert.AreEqual(241125362, args2.Item10);
            Assert.AreEqual(1361704259, args1.Item11);
            Assert.AreEqual(1388641465, args2.Item11);
            Assert.AreEqual(1111724761, args1.Item12);
            Assert.AreEqual(774180463, args2.Item12);
            Assert.AreEqual(1431489812, args1.Item13);
            Assert.AreEqual(200757548, args2.Item13);
            Assert.AreEqual(896176641, args1.Item14);
            Assert.AreEqual(1888379553, args2.Item14);
            Assert.AreEqual(318978190, args1.Item15);
            Assert.AreEqual(1203734748, args2.Item15);
            Assert.AreEqual(1966868667, args1.Item16);
            Assert.AreEqual(974933146, args2.Item16);
            Assert.AreEqual(1882597303, args1.Rest.Item1);
            Assert.AreEqual(75253661, args2.Rest.Item1);
            Assert.AreEqual(1985242937, args1.Rest.Item2);
            Assert.AreEqual(880866833, args2.Rest.Item2);
            Assert.AreEqual(1479065794, args1.Rest.Item3);
            Assert.AreEqual(603367200, args2.Rest.Item3);

            Assert.AreEqual(args1, args1);

            var copy1 = args1;
            Assert.IsTrue(args1 == copy1);
            Assert.IsFalse(args1 != copy1);

            Assert.AreNotEqual(args1.GetHashCode(), args2.GetHashCode());
            Assert.AreNotEqual(args1, args2);
            Assert.AreNotEqual(args2, args1);

            var scm1 = (IStructuralComparable)args1;
            var cmp1 = (IComparable)args1;

            Assert.AreNotEqual(0, args1.CompareTo(args2));
            Assert.AreNotEqual(0, scm1.CompareTo(args2, Comparer<object>.Default));
            Assert.AreNotEqual(0, cmp1.CompareTo(args2));
            Assert.AreEqual(1, scm1.CompareTo(other: null, Comparer<object>.Default));

            Assert.ThrowsException<ArgumentException>(() => scm1.CompareTo("foo", Comparer<object>.Default));

            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int>>(2051049710, 1721842594, 1481009741, 130339879, 364584764, 527030178, 1774500739, 2093762387, 581115906, 1456062360, 1361704259, 1111724761, 1431489812, 896176641, 318978190, 1966868667, new Tuplet<int, int, int>(1882597303, 1985242937, 1479065794))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int>>(2051049708, 1721842594, 1481009741, 130339879, 364584764, 527030178, 1774500739, 2093762387, 581115906, 1456062360, 1361704259, 1111724761, 1431489812, 896176641, 318978190, 1966868667, new Tuplet<int, int, int>(1882597303, 1985242937, 1479065794))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int>>(2051049710, 1721842594, 1481009741, 130339879, 364584764, 527030178, 1774500739, 2093762387, 581115906, 1456062360, 1361704259, 1111724761, 1431489812, 896176641, 318978190, 1966868667, new Tuplet<int, int, int>(1882597303, 1985242937, 1479065794)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int>>(2051049708, 1721842594, 1481009741, 130339879, 364584764, 527030178, 1774500739, 2093762387, 581115906, 1456062360, 1361704259, 1111724761, 1431489812, 896176641, 318978190, 1966868667, new Tuplet<int, int, int>(1882597303, 1985242937, 1479065794)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int>>(2051049709, 1721842595, 1481009741, 130339879, 364584764, 527030178, 1774500739, 2093762387, 581115906, 1456062360, 1361704259, 1111724761, 1431489812, 896176641, 318978190, 1966868667, new Tuplet<int, int, int>(1882597303, 1985242937, 1479065794))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int>>(2051049709, 1721842593, 1481009741, 130339879, 364584764, 527030178, 1774500739, 2093762387, 581115906, 1456062360, 1361704259, 1111724761, 1431489812, 896176641, 318978190, 1966868667, new Tuplet<int, int, int>(1882597303, 1985242937, 1479065794))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int>>(2051049709, 1721842595, 1481009741, 130339879, 364584764, 527030178, 1774500739, 2093762387, 581115906, 1456062360, 1361704259, 1111724761, 1431489812, 896176641, 318978190, 1966868667, new Tuplet<int, int, int>(1882597303, 1985242937, 1479065794)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int>>(2051049709, 1721842593, 1481009741, 130339879, 364584764, 527030178, 1774500739, 2093762387, 581115906, 1456062360, 1361704259, 1111724761, 1431489812, 896176641, 318978190, 1966868667, new Tuplet<int, int, int>(1882597303, 1985242937, 1479065794)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int>>(2051049709, 1721842594, 1481009742, 130339879, 364584764, 527030178, 1774500739, 2093762387, 581115906, 1456062360, 1361704259, 1111724761, 1431489812, 896176641, 318978190, 1966868667, new Tuplet<int, int, int>(1882597303, 1985242937, 1479065794))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int>>(2051049709, 1721842594, 1481009740, 130339879, 364584764, 527030178, 1774500739, 2093762387, 581115906, 1456062360, 1361704259, 1111724761, 1431489812, 896176641, 318978190, 1966868667, new Tuplet<int, int, int>(1882597303, 1985242937, 1479065794))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int>>(2051049709, 1721842594, 1481009742, 130339879, 364584764, 527030178, 1774500739, 2093762387, 581115906, 1456062360, 1361704259, 1111724761, 1431489812, 896176641, 318978190, 1966868667, new Tuplet<int, int, int>(1882597303, 1985242937, 1479065794)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int>>(2051049709, 1721842594, 1481009740, 130339879, 364584764, 527030178, 1774500739, 2093762387, 581115906, 1456062360, 1361704259, 1111724761, 1431489812, 896176641, 318978190, 1966868667, new Tuplet<int, int, int>(1882597303, 1985242937, 1479065794)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int>>(2051049709, 1721842594, 1481009741, 130339880, 364584764, 527030178, 1774500739, 2093762387, 581115906, 1456062360, 1361704259, 1111724761, 1431489812, 896176641, 318978190, 1966868667, new Tuplet<int, int, int>(1882597303, 1985242937, 1479065794))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int>>(2051049709, 1721842594, 1481009741, 130339878, 364584764, 527030178, 1774500739, 2093762387, 581115906, 1456062360, 1361704259, 1111724761, 1431489812, 896176641, 318978190, 1966868667, new Tuplet<int, int, int>(1882597303, 1985242937, 1479065794))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int>>(2051049709, 1721842594, 1481009741, 130339880, 364584764, 527030178, 1774500739, 2093762387, 581115906, 1456062360, 1361704259, 1111724761, 1431489812, 896176641, 318978190, 1966868667, new Tuplet<int, int, int>(1882597303, 1985242937, 1479065794)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int>>(2051049709, 1721842594, 1481009741, 130339878, 364584764, 527030178, 1774500739, 2093762387, 581115906, 1456062360, 1361704259, 1111724761, 1431489812, 896176641, 318978190, 1966868667, new Tuplet<int, int, int>(1882597303, 1985242937, 1479065794)), Comparer<object>.Default) > 0);

            Assert.IsFalse(args1 == args2);
            Assert.IsTrue(args1 != args2);

            Assert.AreNotEqual(args1, "foo");
            Assert.AreNotEqual(args1, null);

            var seq1 = (IStructuralEquatable)args1;
            Assert.AreEqual(args1.GetHashCode(), seq1.GetHashCode(EqualityComparer<object>.Default));
            Assert.AreEqual(0, seq1.GetHashCode(new ConstantHashEqualityComparer(0)));
            Assert.IsTrue(seq1.Equals(args1, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals(args2, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals("foo", EqualityComparer<object>.Default));

            Assert.AreEqual("(2051049709, 1721842594, 1481009741, 130339879, 364584764, 527030178, 1774500739, 2093762387, 581115906, 1456062360, 1361704259, 1111724761, 1431489812, 896176641, 318978190, 1966868667, 1882597303, 1985242937, 1479065794)", args1.ToString());
            Assert.AreEqual("(697529843, 1573764732, 1076056500, 56117124, 1230434122, 1615791481, 434227392, 1806405910, 927081730, 241125362, 1388641465, 774180463, 200757548, 1888379553, 1203734748, 974933146, 75253661, 880866833, 603367200)", args2.ToString());
        }

        [TestMethod]
        public void Tuplet20()
        {
            var args1 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int>>(1331337961, 236372791, 751510318, 336244514, 1412090257, 1425869134, 508203584, 867393504, 2096170085, 1412043937, 1177386626, 1825362896, 3112452, 2006511400, 453748237, 1648837195, new Tuplet<int, int, int, int>(1811421970, 974993209, 1665725470, 250575619));
            var args2 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int>>(662032045, 2077841019, 868107915, 847419009, 1852637025, 1339958088, 681881897, 1160946711, 1370828855, 227755064, 2068727142, 243724529, 1086001834, 1279230103, 653904976, 1242693003, new Tuplet<int, int, int, int>(2093503172, 1237520218, 1811449890, 777731637));

            Assert.AreEqual(1331337961, args1.Item1);
            Assert.AreEqual(662032045, args2.Item1);
            Assert.AreEqual(236372791, args1.Item2);
            Assert.AreEqual(2077841019, args2.Item2);
            Assert.AreEqual(751510318, args1.Item3);
            Assert.AreEqual(868107915, args2.Item3);
            Assert.AreEqual(336244514, args1.Item4);
            Assert.AreEqual(847419009, args2.Item4);
            Assert.AreEqual(1412090257, args1.Item5);
            Assert.AreEqual(1852637025, args2.Item5);
            Assert.AreEqual(1425869134, args1.Item6);
            Assert.AreEqual(1339958088, args2.Item6);
            Assert.AreEqual(508203584, args1.Item7);
            Assert.AreEqual(681881897, args2.Item7);
            Assert.AreEqual(867393504, args1.Item8);
            Assert.AreEqual(1160946711, args2.Item8);
            Assert.AreEqual(2096170085, args1.Item9);
            Assert.AreEqual(1370828855, args2.Item9);
            Assert.AreEqual(1412043937, args1.Item10);
            Assert.AreEqual(227755064, args2.Item10);
            Assert.AreEqual(1177386626, args1.Item11);
            Assert.AreEqual(2068727142, args2.Item11);
            Assert.AreEqual(1825362896, args1.Item12);
            Assert.AreEqual(243724529, args2.Item12);
            Assert.AreEqual(3112452, args1.Item13);
            Assert.AreEqual(1086001834, args2.Item13);
            Assert.AreEqual(2006511400, args1.Item14);
            Assert.AreEqual(1279230103, args2.Item14);
            Assert.AreEqual(453748237, args1.Item15);
            Assert.AreEqual(653904976, args2.Item15);
            Assert.AreEqual(1648837195, args1.Item16);
            Assert.AreEqual(1242693003, args2.Item16);
            Assert.AreEqual(1811421970, args1.Rest.Item1);
            Assert.AreEqual(2093503172, args2.Rest.Item1);
            Assert.AreEqual(974993209, args1.Rest.Item2);
            Assert.AreEqual(1237520218, args2.Rest.Item2);
            Assert.AreEqual(1665725470, args1.Rest.Item3);
            Assert.AreEqual(1811449890, args2.Rest.Item3);
            Assert.AreEqual(250575619, args1.Rest.Item4);
            Assert.AreEqual(777731637, args2.Rest.Item4);

            Assert.AreEqual(args1, args1);

            var copy1 = args1;
            Assert.IsTrue(args1 == copy1);
            Assert.IsFalse(args1 != copy1);

            Assert.AreNotEqual(args1.GetHashCode(), args2.GetHashCode());
            Assert.AreNotEqual(args1, args2);
            Assert.AreNotEqual(args2, args1);

            var scm1 = (IStructuralComparable)args1;
            var cmp1 = (IComparable)args1;

            Assert.AreNotEqual(0, args1.CompareTo(args2));
            Assert.AreNotEqual(0, scm1.CompareTo(args2, Comparer<object>.Default));
            Assert.AreNotEqual(0, cmp1.CompareTo(args2));
            Assert.AreEqual(1, scm1.CompareTo(other: null, Comparer<object>.Default));

            Assert.ThrowsException<ArgumentException>(() => scm1.CompareTo("foo", Comparer<object>.Default));

            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int>>(1331337962, 236372791, 751510318, 336244514, 1412090257, 1425869134, 508203584, 867393504, 2096170085, 1412043937, 1177386626, 1825362896, 3112452, 2006511400, 453748237, 1648837195, new Tuplet<int, int, int, int>(1811421970, 974993209, 1665725470, 250575619))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int>>(1331337960, 236372791, 751510318, 336244514, 1412090257, 1425869134, 508203584, 867393504, 2096170085, 1412043937, 1177386626, 1825362896, 3112452, 2006511400, 453748237, 1648837195, new Tuplet<int, int, int, int>(1811421970, 974993209, 1665725470, 250575619))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int>>(1331337962, 236372791, 751510318, 336244514, 1412090257, 1425869134, 508203584, 867393504, 2096170085, 1412043937, 1177386626, 1825362896, 3112452, 2006511400, 453748237, 1648837195, new Tuplet<int, int, int, int>(1811421970, 974993209, 1665725470, 250575619)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int>>(1331337960, 236372791, 751510318, 336244514, 1412090257, 1425869134, 508203584, 867393504, 2096170085, 1412043937, 1177386626, 1825362896, 3112452, 2006511400, 453748237, 1648837195, new Tuplet<int, int, int, int>(1811421970, 974993209, 1665725470, 250575619)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int>>(1331337961, 236372792, 751510318, 336244514, 1412090257, 1425869134, 508203584, 867393504, 2096170085, 1412043937, 1177386626, 1825362896, 3112452, 2006511400, 453748237, 1648837195, new Tuplet<int, int, int, int>(1811421970, 974993209, 1665725470, 250575619))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int>>(1331337961, 236372790, 751510318, 336244514, 1412090257, 1425869134, 508203584, 867393504, 2096170085, 1412043937, 1177386626, 1825362896, 3112452, 2006511400, 453748237, 1648837195, new Tuplet<int, int, int, int>(1811421970, 974993209, 1665725470, 250575619))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int>>(1331337961, 236372792, 751510318, 336244514, 1412090257, 1425869134, 508203584, 867393504, 2096170085, 1412043937, 1177386626, 1825362896, 3112452, 2006511400, 453748237, 1648837195, new Tuplet<int, int, int, int>(1811421970, 974993209, 1665725470, 250575619)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int>>(1331337961, 236372790, 751510318, 336244514, 1412090257, 1425869134, 508203584, 867393504, 2096170085, 1412043937, 1177386626, 1825362896, 3112452, 2006511400, 453748237, 1648837195, new Tuplet<int, int, int, int>(1811421970, 974993209, 1665725470, 250575619)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int>>(1331337961, 236372791, 751510319, 336244514, 1412090257, 1425869134, 508203584, 867393504, 2096170085, 1412043937, 1177386626, 1825362896, 3112452, 2006511400, 453748237, 1648837195, new Tuplet<int, int, int, int>(1811421970, 974993209, 1665725470, 250575619))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int>>(1331337961, 236372791, 751510317, 336244514, 1412090257, 1425869134, 508203584, 867393504, 2096170085, 1412043937, 1177386626, 1825362896, 3112452, 2006511400, 453748237, 1648837195, new Tuplet<int, int, int, int>(1811421970, 974993209, 1665725470, 250575619))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int>>(1331337961, 236372791, 751510319, 336244514, 1412090257, 1425869134, 508203584, 867393504, 2096170085, 1412043937, 1177386626, 1825362896, 3112452, 2006511400, 453748237, 1648837195, new Tuplet<int, int, int, int>(1811421970, 974993209, 1665725470, 250575619)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int>>(1331337961, 236372791, 751510317, 336244514, 1412090257, 1425869134, 508203584, 867393504, 2096170085, 1412043937, 1177386626, 1825362896, 3112452, 2006511400, 453748237, 1648837195, new Tuplet<int, int, int, int>(1811421970, 974993209, 1665725470, 250575619)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int>>(1331337961, 236372791, 751510318, 336244515, 1412090257, 1425869134, 508203584, 867393504, 2096170085, 1412043937, 1177386626, 1825362896, 3112452, 2006511400, 453748237, 1648837195, new Tuplet<int, int, int, int>(1811421970, 974993209, 1665725470, 250575619))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int>>(1331337961, 236372791, 751510318, 336244513, 1412090257, 1425869134, 508203584, 867393504, 2096170085, 1412043937, 1177386626, 1825362896, 3112452, 2006511400, 453748237, 1648837195, new Tuplet<int, int, int, int>(1811421970, 974993209, 1665725470, 250575619))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int>>(1331337961, 236372791, 751510318, 336244515, 1412090257, 1425869134, 508203584, 867393504, 2096170085, 1412043937, 1177386626, 1825362896, 3112452, 2006511400, 453748237, 1648837195, new Tuplet<int, int, int, int>(1811421970, 974993209, 1665725470, 250575619)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int>>(1331337961, 236372791, 751510318, 336244513, 1412090257, 1425869134, 508203584, 867393504, 2096170085, 1412043937, 1177386626, 1825362896, 3112452, 2006511400, 453748237, 1648837195, new Tuplet<int, int, int, int>(1811421970, 974993209, 1665725470, 250575619)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int>>(1331337961, 236372791, 751510318, 336244514, 1412090258, 1425869134, 508203584, 867393504, 2096170085, 1412043937, 1177386626, 1825362896, 3112452, 2006511400, 453748237, 1648837195, new Tuplet<int, int, int, int>(1811421970, 974993209, 1665725470, 250575619))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int>>(1331337961, 236372791, 751510318, 336244514, 1412090256, 1425869134, 508203584, 867393504, 2096170085, 1412043937, 1177386626, 1825362896, 3112452, 2006511400, 453748237, 1648837195, new Tuplet<int, int, int, int>(1811421970, 974993209, 1665725470, 250575619))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int>>(1331337961, 236372791, 751510318, 336244514, 1412090258, 1425869134, 508203584, 867393504, 2096170085, 1412043937, 1177386626, 1825362896, 3112452, 2006511400, 453748237, 1648837195, new Tuplet<int, int, int, int>(1811421970, 974993209, 1665725470, 250575619)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int>>(1331337961, 236372791, 751510318, 336244514, 1412090256, 1425869134, 508203584, 867393504, 2096170085, 1412043937, 1177386626, 1825362896, 3112452, 2006511400, 453748237, 1648837195, new Tuplet<int, int, int, int>(1811421970, 974993209, 1665725470, 250575619)), Comparer<object>.Default) > 0);

            Assert.IsFalse(args1 == args2);
            Assert.IsTrue(args1 != args2);

            Assert.AreNotEqual(args1, "foo");
            Assert.AreNotEqual(args1, null);

            var seq1 = (IStructuralEquatable)args1;
            Assert.AreEqual(args1.GetHashCode(), seq1.GetHashCode(EqualityComparer<object>.Default));
            Assert.AreEqual(0, seq1.GetHashCode(new ConstantHashEqualityComparer(0)));
            Assert.IsTrue(seq1.Equals(args1, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals(args2, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals("foo", EqualityComparer<object>.Default));

            Assert.AreEqual("(1331337961, 236372791, 751510318, 336244514, 1412090257, 1425869134, 508203584, 867393504, 2096170085, 1412043937, 1177386626, 1825362896, 3112452, 2006511400, 453748237, 1648837195, 1811421970, 974993209, 1665725470, 250575619)", args1.ToString());
            Assert.AreEqual("(662032045, 2077841019, 868107915, 847419009, 1852637025, 1339958088, 681881897, 1160946711, 1370828855, 227755064, 2068727142, 243724529, 1086001834, 1279230103, 653904976, 1242693003, 2093503172, 1237520218, 1811449890, 777731637)", args2.ToString());
        }

        [TestMethod]
        public void Tuplet21()
        {
            var args1 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int>>(722230538, 748397977, 485540954, 394361973, 1897178751, 563246113, 1385529013, 915152710, 1894492958, 239542358, 1539796425, 2147423584, 557011838, 630291214, 2088818802, 1400980589, new Tuplet<int, int, int, int, int>(1515748523, 2051574956, 631091136, 72132169, 743987237));
            var args2 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int>>(1494740520, 1644048296, 1868415021, 1490800442, 933662097, 739361062, 871365996, 1352606424, 1358538881, 1702817670, 573901752, 1311026966, 887993833, 1675828728, 2061117715, 1592300065, new Tuplet<int, int, int, int, int>(473745942, 1097723905, 1289390912, 2101912722, 1914212834));

            Assert.AreEqual(722230538, args1.Item1);
            Assert.AreEqual(1494740520, args2.Item1);
            Assert.AreEqual(748397977, args1.Item2);
            Assert.AreEqual(1644048296, args2.Item2);
            Assert.AreEqual(485540954, args1.Item3);
            Assert.AreEqual(1868415021, args2.Item3);
            Assert.AreEqual(394361973, args1.Item4);
            Assert.AreEqual(1490800442, args2.Item4);
            Assert.AreEqual(1897178751, args1.Item5);
            Assert.AreEqual(933662097, args2.Item5);
            Assert.AreEqual(563246113, args1.Item6);
            Assert.AreEqual(739361062, args2.Item6);
            Assert.AreEqual(1385529013, args1.Item7);
            Assert.AreEqual(871365996, args2.Item7);
            Assert.AreEqual(915152710, args1.Item8);
            Assert.AreEqual(1352606424, args2.Item8);
            Assert.AreEqual(1894492958, args1.Item9);
            Assert.AreEqual(1358538881, args2.Item9);
            Assert.AreEqual(239542358, args1.Item10);
            Assert.AreEqual(1702817670, args2.Item10);
            Assert.AreEqual(1539796425, args1.Item11);
            Assert.AreEqual(573901752, args2.Item11);
            Assert.AreEqual(2147423584, args1.Item12);
            Assert.AreEqual(1311026966, args2.Item12);
            Assert.AreEqual(557011838, args1.Item13);
            Assert.AreEqual(887993833, args2.Item13);
            Assert.AreEqual(630291214, args1.Item14);
            Assert.AreEqual(1675828728, args2.Item14);
            Assert.AreEqual(2088818802, args1.Item15);
            Assert.AreEqual(2061117715, args2.Item15);
            Assert.AreEqual(1400980589, args1.Item16);
            Assert.AreEqual(1592300065, args2.Item16);
            Assert.AreEqual(1515748523, args1.Rest.Item1);
            Assert.AreEqual(473745942, args2.Rest.Item1);
            Assert.AreEqual(2051574956, args1.Rest.Item2);
            Assert.AreEqual(1097723905, args2.Rest.Item2);
            Assert.AreEqual(631091136, args1.Rest.Item3);
            Assert.AreEqual(1289390912, args2.Rest.Item3);
            Assert.AreEqual(72132169, args1.Rest.Item4);
            Assert.AreEqual(2101912722, args2.Rest.Item4);
            Assert.AreEqual(743987237, args1.Rest.Item5);
            Assert.AreEqual(1914212834, args2.Rest.Item5);

            Assert.AreEqual(args1, args1);

            var copy1 = args1;
            Assert.IsTrue(args1 == copy1);
            Assert.IsFalse(args1 != copy1);

            Assert.AreNotEqual(args1.GetHashCode(), args2.GetHashCode());
            Assert.AreNotEqual(args1, args2);
            Assert.AreNotEqual(args2, args1);

            var scm1 = (IStructuralComparable)args1;
            var cmp1 = (IComparable)args1;

            Assert.AreNotEqual(0, args1.CompareTo(args2));
            Assert.AreNotEqual(0, scm1.CompareTo(args2, Comparer<object>.Default));
            Assert.AreNotEqual(0, cmp1.CompareTo(args2));
            Assert.AreEqual(1, scm1.CompareTo(other: null, Comparer<object>.Default));

            Assert.ThrowsException<ArgumentException>(() => scm1.CompareTo("foo", Comparer<object>.Default));

            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int>>(722230539, 748397977, 485540954, 394361973, 1897178751, 563246113, 1385529013, 915152710, 1894492958, 239542358, 1539796425, 2147423584, 557011838, 630291214, 2088818802, 1400980589, new Tuplet<int, int, int, int, int>(1515748523, 2051574956, 631091136, 72132169, 743987237))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int>>(722230537, 748397977, 485540954, 394361973, 1897178751, 563246113, 1385529013, 915152710, 1894492958, 239542358, 1539796425, 2147423584, 557011838, 630291214, 2088818802, 1400980589, new Tuplet<int, int, int, int, int>(1515748523, 2051574956, 631091136, 72132169, 743987237))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int>>(722230539, 748397977, 485540954, 394361973, 1897178751, 563246113, 1385529013, 915152710, 1894492958, 239542358, 1539796425, 2147423584, 557011838, 630291214, 2088818802, 1400980589, new Tuplet<int, int, int, int, int>(1515748523, 2051574956, 631091136, 72132169, 743987237)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int>>(722230537, 748397977, 485540954, 394361973, 1897178751, 563246113, 1385529013, 915152710, 1894492958, 239542358, 1539796425, 2147423584, 557011838, 630291214, 2088818802, 1400980589, new Tuplet<int, int, int, int, int>(1515748523, 2051574956, 631091136, 72132169, 743987237)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int>>(722230538, 748397978, 485540954, 394361973, 1897178751, 563246113, 1385529013, 915152710, 1894492958, 239542358, 1539796425, 2147423584, 557011838, 630291214, 2088818802, 1400980589, new Tuplet<int, int, int, int, int>(1515748523, 2051574956, 631091136, 72132169, 743987237))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int>>(722230538, 748397976, 485540954, 394361973, 1897178751, 563246113, 1385529013, 915152710, 1894492958, 239542358, 1539796425, 2147423584, 557011838, 630291214, 2088818802, 1400980589, new Tuplet<int, int, int, int, int>(1515748523, 2051574956, 631091136, 72132169, 743987237))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int>>(722230538, 748397978, 485540954, 394361973, 1897178751, 563246113, 1385529013, 915152710, 1894492958, 239542358, 1539796425, 2147423584, 557011838, 630291214, 2088818802, 1400980589, new Tuplet<int, int, int, int, int>(1515748523, 2051574956, 631091136, 72132169, 743987237)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int>>(722230538, 748397976, 485540954, 394361973, 1897178751, 563246113, 1385529013, 915152710, 1894492958, 239542358, 1539796425, 2147423584, 557011838, 630291214, 2088818802, 1400980589, new Tuplet<int, int, int, int, int>(1515748523, 2051574956, 631091136, 72132169, 743987237)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int>>(722230538, 748397977, 485540955, 394361973, 1897178751, 563246113, 1385529013, 915152710, 1894492958, 239542358, 1539796425, 2147423584, 557011838, 630291214, 2088818802, 1400980589, new Tuplet<int, int, int, int, int>(1515748523, 2051574956, 631091136, 72132169, 743987237))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int>>(722230538, 748397977, 485540953, 394361973, 1897178751, 563246113, 1385529013, 915152710, 1894492958, 239542358, 1539796425, 2147423584, 557011838, 630291214, 2088818802, 1400980589, new Tuplet<int, int, int, int, int>(1515748523, 2051574956, 631091136, 72132169, 743987237))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int>>(722230538, 748397977, 485540955, 394361973, 1897178751, 563246113, 1385529013, 915152710, 1894492958, 239542358, 1539796425, 2147423584, 557011838, 630291214, 2088818802, 1400980589, new Tuplet<int, int, int, int, int>(1515748523, 2051574956, 631091136, 72132169, 743987237)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int>>(722230538, 748397977, 485540953, 394361973, 1897178751, 563246113, 1385529013, 915152710, 1894492958, 239542358, 1539796425, 2147423584, 557011838, 630291214, 2088818802, 1400980589, new Tuplet<int, int, int, int, int>(1515748523, 2051574956, 631091136, 72132169, 743987237)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int>>(722230538, 748397977, 485540954, 394361974, 1897178751, 563246113, 1385529013, 915152710, 1894492958, 239542358, 1539796425, 2147423584, 557011838, 630291214, 2088818802, 1400980589, new Tuplet<int, int, int, int, int>(1515748523, 2051574956, 631091136, 72132169, 743987237))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int>>(722230538, 748397977, 485540954, 394361972, 1897178751, 563246113, 1385529013, 915152710, 1894492958, 239542358, 1539796425, 2147423584, 557011838, 630291214, 2088818802, 1400980589, new Tuplet<int, int, int, int, int>(1515748523, 2051574956, 631091136, 72132169, 743987237))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int>>(722230538, 748397977, 485540954, 394361974, 1897178751, 563246113, 1385529013, 915152710, 1894492958, 239542358, 1539796425, 2147423584, 557011838, 630291214, 2088818802, 1400980589, new Tuplet<int, int, int, int, int>(1515748523, 2051574956, 631091136, 72132169, 743987237)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int>>(722230538, 748397977, 485540954, 394361972, 1897178751, 563246113, 1385529013, 915152710, 1894492958, 239542358, 1539796425, 2147423584, 557011838, 630291214, 2088818802, 1400980589, new Tuplet<int, int, int, int, int>(1515748523, 2051574956, 631091136, 72132169, 743987237)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int>>(722230538, 748397977, 485540954, 394361973, 1897178752, 563246113, 1385529013, 915152710, 1894492958, 239542358, 1539796425, 2147423584, 557011838, 630291214, 2088818802, 1400980589, new Tuplet<int, int, int, int, int>(1515748523, 2051574956, 631091136, 72132169, 743987237))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int>>(722230538, 748397977, 485540954, 394361973, 1897178750, 563246113, 1385529013, 915152710, 1894492958, 239542358, 1539796425, 2147423584, 557011838, 630291214, 2088818802, 1400980589, new Tuplet<int, int, int, int, int>(1515748523, 2051574956, 631091136, 72132169, 743987237))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int>>(722230538, 748397977, 485540954, 394361973, 1897178752, 563246113, 1385529013, 915152710, 1894492958, 239542358, 1539796425, 2147423584, 557011838, 630291214, 2088818802, 1400980589, new Tuplet<int, int, int, int, int>(1515748523, 2051574956, 631091136, 72132169, 743987237)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int>>(722230538, 748397977, 485540954, 394361973, 1897178750, 563246113, 1385529013, 915152710, 1894492958, 239542358, 1539796425, 2147423584, 557011838, 630291214, 2088818802, 1400980589, new Tuplet<int, int, int, int, int>(1515748523, 2051574956, 631091136, 72132169, 743987237)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int>>(722230538, 748397977, 485540954, 394361973, 1897178751, 563246114, 1385529013, 915152710, 1894492958, 239542358, 1539796425, 2147423584, 557011838, 630291214, 2088818802, 1400980589, new Tuplet<int, int, int, int, int>(1515748523, 2051574956, 631091136, 72132169, 743987237))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int>>(722230538, 748397977, 485540954, 394361973, 1897178751, 563246112, 1385529013, 915152710, 1894492958, 239542358, 1539796425, 2147423584, 557011838, 630291214, 2088818802, 1400980589, new Tuplet<int, int, int, int, int>(1515748523, 2051574956, 631091136, 72132169, 743987237))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int>>(722230538, 748397977, 485540954, 394361973, 1897178751, 563246114, 1385529013, 915152710, 1894492958, 239542358, 1539796425, 2147423584, 557011838, 630291214, 2088818802, 1400980589, new Tuplet<int, int, int, int, int>(1515748523, 2051574956, 631091136, 72132169, 743987237)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int>>(722230538, 748397977, 485540954, 394361973, 1897178751, 563246112, 1385529013, 915152710, 1894492958, 239542358, 1539796425, 2147423584, 557011838, 630291214, 2088818802, 1400980589, new Tuplet<int, int, int, int, int>(1515748523, 2051574956, 631091136, 72132169, 743987237)), Comparer<object>.Default) > 0);

            Assert.IsFalse(args1 == args2);
            Assert.IsTrue(args1 != args2);

            Assert.AreNotEqual(args1, "foo");
            Assert.AreNotEqual(args1, null);

            var seq1 = (IStructuralEquatable)args1;
            Assert.AreEqual(args1.GetHashCode(), seq1.GetHashCode(EqualityComparer<object>.Default));
            Assert.AreEqual(0, seq1.GetHashCode(new ConstantHashEqualityComparer(0)));
            Assert.IsTrue(seq1.Equals(args1, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals(args2, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals("foo", EqualityComparer<object>.Default));

            Assert.AreEqual("(722230538, 748397977, 485540954, 394361973, 1897178751, 563246113, 1385529013, 915152710, 1894492958, 239542358, 1539796425, 2147423584, 557011838, 630291214, 2088818802, 1400980589, 1515748523, 2051574956, 631091136, 72132169, 743987237)", args1.ToString());
            Assert.AreEqual("(1494740520, 1644048296, 1868415021, 1490800442, 933662097, 739361062, 871365996, 1352606424, 1358538881, 1702817670, 573901752, 1311026966, 887993833, 1675828728, 2061117715, 1592300065, 473745942, 1097723905, 1289390912, 2101912722, 1914212834)", args2.ToString());
        }

        [TestMethod]
        public void Tuplet22()
        {
            var args1 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int>>(1413937400, 1131286497, 835442286, 2068787205, 1834196338, 455710620, 1337894948, 1400408034, 1874428127, 41928216, 606429082, 1739317721, 33744400, 1374973665, 1251833328, 764609580, new Tuplet<int, int, int, int, int, int>(1051045178, 963516654, 1971368698, 514163017, 1710029933, 535954077));
            var args2 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int>>(684208335, 965894673, 836396618, 1816501652, 1101946133, 27701087, 1956164171, 1042002581, 953851051, 1489183871, 117703094, 977258050, 80803120, 512761799, 1032972735, 1569496884, new Tuplet<int, int, int, int, int, int>(1246949406, 283650442, 1680954695, 2099682037, 1631594401, 1660889454));

            Assert.AreEqual(1413937400, args1.Item1);
            Assert.AreEqual(684208335, args2.Item1);
            Assert.AreEqual(1131286497, args1.Item2);
            Assert.AreEqual(965894673, args2.Item2);
            Assert.AreEqual(835442286, args1.Item3);
            Assert.AreEqual(836396618, args2.Item3);
            Assert.AreEqual(2068787205, args1.Item4);
            Assert.AreEqual(1816501652, args2.Item4);
            Assert.AreEqual(1834196338, args1.Item5);
            Assert.AreEqual(1101946133, args2.Item5);
            Assert.AreEqual(455710620, args1.Item6);
            Assert.AreEqual(27701087, args2.Item6);
            Assert.AreEqual(1337894948, args1.Item7);
            Assert.AreEqual(1956164171, args2.Item7);
            Assert.AreEqual(1400408034, args1.Item8);
            Assert.AreEqual(1042002581, args2.Item8);
            Assert.AreEqual(1874428127, args1.Item9);
            Assert.AreEqual(953851051, args2.Item9);
            Assert.AreEqual(41928216, args1.Item10);
            Assert.AreEqual(1489183871, args2.Item10);
            Assert.AreEqual(606429082, args1.Item11);
            Assert.AreEqual(117703094, args2.Item11);
            Assert.AreEqual(1739317721, args1.Item12);
            Assert.AreEqual(977258050, args2.Item12);
            Assert.AreEqual(33744400, args1.Item13);
            Assert.AreEqual(80803120, args2.Item13);
            Assert.AreEqual(1374973665, args1.Item14);
            Assert.AreEqual(512761799, args2.Item14);
            Assert.AreEqual(1251833328, args1.Item15);
            Assert.AreEqual(1032972735, args2.Item15);
            Assert.AreEqual(764609580, args1.Item16);
            Assert.AreEqual(1569496884, args2.Item16);
            Assert.AreEqual(1051045178, args1.Rest.Item1);
            Assert.AreEqual(1246949406, args2.Rest.Item1);
            Assert.AreEqual(963516654, args1.Rest.Item2);
            Assert.AreEqual(283650442, args2.Rest.Item2);
            Assert.AreEqual(1971368698, args1.Rest.Item3);
            Assert.AreEqual(1680954695, args2.Rest.Item3);
            Assert.AreEqual(514163017, args1.Rest.Item4);
            Assert.AreEqual(2099682037, args2.Rest.Item4);
            Assert.AreEqual(1710029933, args1.Rest.Item5);
            Assert.AreEqual(1631594401, args2.Rest.Item5);
            Assert.AreEqual(535954077, args1.Rest.Item6);
            Assert.AreEqual(1660889454, args2.Rest.Item6);

            Assert.AreEqual(args1, args1);

            var copy1 = args1;
            Assert.IsTrue(args1 == copy1);
            Assert.IsFalse(args1 != copy1);

            Assert.AreNotEqual(args1.GetHashCode(), args2.GetHashCode());
            Assert.AreNotEqual(args1, args2);
            Assert.AreNotEqual(args2, args1);

            var scm1 = (IStructuralComparable)args1;
            var cmp1 = (IComparable)args1;

            Assert.AreNotEqual(0, args1.CompareTo(args2));
            Assert.AreNotEqual(0, scm1.CompareTo(args2, Comparer<object>.Default));
            Assert.AreNotEqual(0, cmp1.CompareTo(args2));
            Assert.AreEqual(1, scm1.CompareTo(other: null, Comparer<object>.Default));

            Assert.ThrowsException<ArgumentException>(() => scm1.CompareTo("foo", Comparer<object>.Default));

            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int>>(1413937401, 1131286497, 835442286, 2068787205, 1834196338, 455710620, 1337894948, 1400408034, 1874428127, 41928216, 606429082, 1739317721, 33744400, 1374973665, 1251833328, 764609580, new Tuplet<int, int, int, int, int, int>(1051045178, 963516654, 1971368698, 514163017, 1710029933, 535954077))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int>>(1413937399, 1131286497, 835442286, 2068787205, 1834196338, 455710620, 1337894948, 1400408034, 1874428127, 41928216, 606429082, 1739317721, 33744400, 1374973665, 1251833328, 764609580, new Tuplet<int, int, int, int, int, int>(1051045178, 963516654, 1971368698, 514163017, 1710029933, 535954077))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int>>(1413937401, 1131286497, 835442286, 2068787205, 1834196338, 455710620, 1337894948, 1400408034, 1874428127, 41928216, 606429082, 1739317721, 33744400, 1374973665, 1251833328, 764609580, new Tuplet<int, int, int, int, int, int>(1051045178, 963516654, 1971368698, 514163017, 1710029933, 535954077)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int>>(1413937399, 1131286497, 835442286, 2068787205, 1834196338, 455710620, 1337894948, 1400408034, 1874428127, 41928216, 606429082, 1739317721, 33744400, 1374973665, 1251833328, 764609580, new Tuplet<int, int, int, int, int, int>(1051045178, 963516654, 1971368698, 514163017, 1710029933, 535954077)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int>>(1413937400, 1131286498, 835442286, 2068787205, 1834196338, 455710620, 1337894948, 1400408034, 1874428127, 41928216, 606429082, 1739317721, 33744400, 1374973665, 1251833328, 764609580, new Tuplet<int, int, int, int, int, int>(1051045178, 963516654, 1971368698, 514163017, 1710029933, 535954077))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int>>(1413937400, 1131286496, 835442286, 2068787205, 1834196338, 455710620, 1337894948, 1400408034, 1874428127, 41928216, 606429082, 1739317721, 33744400, 1374973665, 1251833328, 764609580, new Tuplet<int, int, int, int, int, int>(1051045178, 963516654, 1971368698, 514163017, 1710029933, 535954077))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int>>(1413937400, 1131286498, 835442286, 2068787205, 1834196338, 455710620, 1337894948, 1400408034, 1874428127, 41928216, 606429082, 1739317721, 33744400, 1374973665, 1251833328, 764609580, new Tuplet<int, int, int, int, int, int>(1051045178, 963516654, 1971368698, 514163017, 1710029933, 535954077)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int>>(1413937400, 1131286496, 835442286, 2068787205, 1834196338, 455710620, 1337894948, 1400408034, 1874428127, 41928216, 606429082, 1739317721, 33744400, 1374973665, 1251833328, 764609580, new Tuplet<int, int, int, int, int, int>(1051045178, 963516654, 1971368698, 514163017, 1710029933, 535954077)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int>>(1413937400, 1131286497, 835442287, 2068787205, 1834196338, 455710620, 1337894948, 1400408034, 1874428127, 41928216, 606429082, 1739317721, 33744400, 1374973665, 1251833328, 764609580, new Tuplet<int, int, int, int, int, int>(1051045178, 963516654, 1971368698, 514163017, 1710029933, 535954077))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int>>(1413937400, 1131286497, 835442285, 2068787205, 1834196338, 455710620, 1337894948, 1400408034, 1874428127, 41928216, 606429082, 1739317721, 33744400, 1374973665, 1251833328, 764609580, new Tuplet<int, int, int, int, int, int>(1051045178, 963516654, 1971368698, 514163017, 1710029933, 535954077))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int>>(1413937400, 1131286497, 835442287, 2068787205, 1834196338, 455710620, 1337894948, 1400408034, 1874428127, 41928216, 606429082, 1739317721, 33744400, 1374973665, 1251833328, 764609580, new Tuplet<int, int, int, int, int, int>(1051045178, 963516654, 1971368698, 514163017, 1710029933, 535954077)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int>>(1413937400, 1131286497, 835442285, 2068787205, 1834196338, 455710620, 1337894948, 1400408034, 1874428127, 41928216, 606429082, 1739317721, 33744400, 1374973665, 1251833328, 764609580, new Tuplet<int, int, int, int, int, int>(1051045178, 963516654, 1971368698, 514163017, 1710029933, 535954077)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int>>(1413937400, 1131286497, 835442286, 2068787206, 1834196338, 455710620, 1337894948, 1400408034, 1874428127, 41928216, 606429082, 1739317721, 33744400, 1374973665, 1251833328, 764609580, new Tuplet<int, int, int, int, int, int>(1051045178, 963516654, 1971368698, 514163017, 1710029933, 535954077))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int>>(1413937400, 1131286497, 835442286, 2068787204, 1834196338, 455710620, 1337894948, 1400408034, 1874428127, 41928216, 606429082, 1739317721, 33744400, 1374973665, 1251833328, 764609580, new Tuplet<int, int, int, int, int, int>(1051045178, 963516654, 1971368698, 514163017, 1710029933, 535954077))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int>>(1413937400, 1131286497, 835442286, 2068787206, 1834196338, 455710620, 1337894948, 1400408034, 1874428127, 41928216, 606429082, 1739317721, 33744400, 1374973665, 1251833328, 764609580, new Tuplet<int, int, int, int, int, int>(1051045178, 963516654, 1971368698, 514163017, 1710029933, 535954077)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int>>(1413937400, 1131286497, 835442286, 2068787204, 1834196338, 455710620, 1337894948, 1400408034, 1874428127, 41928216, 606429082, 1739317721, 33744400, 1374973665, 1251833328, 764609580, new Tuplet<int, int, int, int, int, int>(1051045178, 963516654, 1971368698, 514163017, 1710029933, 535954077)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int>>(1413937400, 1131286497, 835442286, 2068787205, 1834196339, 455710620, 1337894948, 1400408034, 1874428127, 41928216, 606429082, 1739317721, 33744400, 1374973665, 1251833328, 764609580, new Tuplet<int, int, int, int, int, int>(1051045178, 963516654, 1971368698, 514163017, 1710029933, 535954077))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int>>(1413937400, 1131286497, 835442286, 2068787205, 1834196337, 455710620, 1337894948, 1400408034, 1874428127, 41928216, 606429082, 1739317721, 33744400, 1374973665, 1251833328, 764609580, new Tuplet<int, int, int, int, int, int>(1051045178, 963516654, 1971368698, 514163017, 1710029933, 535954077))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int>>(1413937400, 1131286497, 835442286, 2068787205, 1834196339, 455710620, 1337894948, 1400408034, 1874428127, 41928216, 606429082, 1739317721, 33744400, 1374973665, 1251833328, 764609580, new Tuplet<int, int, int, int, int, int>(1051045178, 963516654, 1971368698, 514163017, 1710029933, 535954077)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int>>(1413937400, 1131286497, 835442286, 2068787205, 1834196337, 455710620, 1337894948, 1400408034, 1874428127, 41928216, 606429082, 1739317721, 33744400, 1374973665, 1251833328, 764609580, new Tuplet<int, int, int, int, int, int>(1051045178, 963516654, 1971368698, 514163017, 1710029933, 535954077)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int>>(1413937400, 1131286497, 835442286, 2068787205, 1834196338, 455710621, 1337894948, 1400408034, 1874428127, 41928216, 606429082, 1739317721, 33744400, 1374973665, 1251833328, 764609580, new Tuplet<int, int, int, int, int, int>(1051045178, 963516654, 1971368698, 514163017, 1710029933, 535954077))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int>>(1413937400, 1131286497, 835442286, 2068787205, 1834196338, 455710619, 1337894948, 1400408034, 1874428127, 41928216, 606429082, 1739317721, 33744400, 1374973665, 1251833328, 764609580, new Tuplet<int, int, int, int, int, int>(1051045178, 963516654, 1971368698, 514163017, 1710029933, 535954077))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int>>(1413937400, 1131286497, 835442286, 2068787205, 1834196338, 455710621, 1337894948, 1400408034, 1874428127, 41928216, 606429082, 1739317721, 33744400, 1374973665, 1251833328, 764609580, new Tuplet<int, int, int, int, int, int>(1051045178, 963516654, 1971368698, 514163017, 1710029933, 535954077)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int>>(1413937400, 1131286497, 835442286, 2068787205, 1834196338, 455710619, 1337894948, 1400408034, 1874428127, 41928216, 606429082, 1739317721, 33744400, 1374973665, 1251833328, 764609580, new Tuplet<int, int, int, int, int, int>(1051045178, 963516654, 1971368698, 514163017, 1710029933, 535954077)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int>>(1413937400, 1131286497, 835442286, 2068787205, 1834196338, 455710620, 1337894949, 1400408034, 1874428127, 41928216, 606429082, 1739317721, 33744400, 1374973665, 1251833328, 764609580, new Tuplet<int, int, int, int, int, int>(1051045178, 963516654, 1971368698, 514163017, 1710029933, 535954077))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int>>(1413937400, 1131286497, 835442286, 2068787205, 1834196338, 455710620, 1337894947, 1400408034, 1874428127, 41928216, 606429082, 1739317721, 33744400, 1374973665, 1251833328, 764609580, new Tuplet<int, int, int, int, int, int>(1051045178, 963516654, 1971368698, 514163017, 1710029933, 535954077))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int>>(1413937400, 1131286497, 835442286, 2068787205, 1834196338, 455710620, 1337894949, 1400408034, 1874428127, 41928216, 606429082, 1739317721, 33744400, 1374973665, 1251833328, 764609580, new Tuplet<int, int, int, int, int, int>(1051045178, 963516654, 1971368698, 514163017, 1710029933, 535954077)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int>>(1413937400, 1131286497, 835442286, 2068787205, 1834196338, 455710620, 1337894947, 1400408034, 1874428127, 41928216, 606429082, 1739317721, 33744400, 1374973665, 1251833328, 764609580, new Tuplet<int, int, int, int, int, int>(1051045178, 963516654, 1971368698, 514163017, 1710029933, 535954077)), Comparer<object>.Default) > 0);

            Assert.IsFalse(args1 == args2);
            Assert.IsTrue(args1 != args2);

            Assert.AreNotEqual(args1, "foo");
            Assert.AreNotEqual(args1, null);

            var seq1 = (IStructuralEquatable)args1;
            Assert.AreEqual(args1.GetHashCode(), seq1.GetHashCode(EqualityComparer<object>.Default));
            Assert.AreEqual(0, seq1.GetHashCode(new ConstantHashEqualityComparer(0)));
            Assert.IsTrue(seq1.Equals(args1, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals(args2, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals("foo", EqualityComparer<object>.Default));

            Assert.AreEqual("(1413937400, 1131286497, 835442286, 2068787205, 1834196338, 455710620, 1337894948, 1400408034, 1874428127, 41928216, 606429082, 1739317721, 33744400, 1374973665, 1251833328, 764609580, 1051045178, 963516654, 1971368698, 514163017, 1710029933, 535954077)", args1.ToString());
            Assert.AreEqual("(684208335, 965894673, 836396618, 1816501652, 1101946133, 27701087, 1956164171, 1042002581, 953851051, 1489183871, 117703094, 977258050, 80803120, 512761799, 1032972735, 1569496884, 1246949406, 283650442, 1680954695, 2099682037, 1631594401, 1660889454)", args2.ToString());
        }

        [TestMethod]
        public void Tuplet23()
        {
            var args1 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int>>(2114956317, 1719192892, 854249433, 300855063, 809284387, 827690485, 1570184411, 134207251, 1465505861, 1587749705, 204182901, 877983323, 447078162, 2017031260, 1232390587, 17694686, new Tuplet<int, int, int, int, int, int, int>(1501248134, 1310193861, 1591727510, 832425546, 1235560812, 1264728858, 1621614627));
            var args2 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int>>(1203969997, 1294170545, 739071529, 1879120492, 1629031941, 1864050895, 1687718256, 980691969, 1757831543, 1051843323, 1170802528, 998422003, 1264687373, 962252219, 801091070, 1365900347, new Tuplet<int, int, int, int, int, int, int>(1128473686, 1619301817, 819643800, 23678010, 677437036, 773075149, 1350303444));

            Assert.AreEqual(2114956317, args1.Item1);
            Assert.AreEqual(1203969997, args2.Item1);
            Assert.AreEqual(1719192892, args1.Item2);
            Assert.AreEqual(1294170545, args2.Item2);
            Assert.AreEqual(854249433, args1.Item3);
            Assert.AreEqual(739071529, args2.Item3);
            Assert.AreEqual(300855063, args1.Item4);
            Assert.AreEqual(1879120492, args2.Item4);
            Assert.AreEqual(809284387, args1.Item5);
            Assert.AreEqual(1629031941, args2.Item5);
            Assert.AreEqual(827690485, args1.Item6);
            Assert.AreEqual(1864050895, args2.Item6);
            Assert.AreEqual(1570184411, args1.Item7);
            Assert.AreEqual(1687718256, args2.Item7);
            Assert.AreEqual(134207251, args1.Item8);
            Assert.AreEqual(980691969, args2.Item8);
            Assert.AreEqual(1465505861, args1.Item9);
            Assert.AreEqual(1757831543, args2.Item9);
            Assert.AreEqual(1587749705, args1.Item10);
            Assert.AreEqual(1051843323, args2.Item10);
            Assert.AreEqual(204182901, args1.Item11);
            Assert.AreEqual(1170802528, args2.Item11);
            Assert.AreEqual(877983323, args1.Item12);
            Assert.AreEqual(998422003, args2.Item12);
            Assert.AreEqual(447078162, args1.Item13);
            Assert.AreEqual(1264687373, args2.Item13);
            Assert.AreEqual(2017031260, args1.Item14);
            Assert.AreEqual(962252219, args2.Item14);
            Assert.AreEqual(1232390587, args1.Item15);
            Assert.AreEqual(801091070, args2.Item15);
            Assert.AreEqual(17694686, args1.Item16);
            Assert.AreEqual(1365900347, args2.Item16);
            Assert.AreEqual(1501248134, args1.Rest.Item1);
            Assert.AreEqual(1128473686, args2.Rest.Item1);
            Assert.AreEqual(1310193861, args1.Rest.Item2);
            Assert.AreEqual(1619301817, args2.Rest.Item2);
            Assert.AreEqual(1591727510, args1.Rest.Item3);
            Assert.AreEqual(819643800, args2.Rest.Item3);
            Assert.AreEqual(832425546, args1.Rest.Item4);
            Assert.AreEqual(23678010, args2.Rest.Item4);
            Assert.AreEqual(1235560812, args1.Rest.Item5);
            Assert.AreEqual(677437036, args2.Rest.Item5);
            Assert.AreEqual(1264728858, args1.Rest.Item6);
            Assert.AreEqual(773075149, args2.Rest.Item6);
            Assert.AreEqual(1621614627, args1.Rest.Item7);
            Assert.AreEqual(1350303444, args2.Rest.Item7);

            Assert.AreEqual(args1, args1);

            var copy1 = args1;
            Assert.IsTrue(args1 == copy1);
            Assert.IsFalse(args1 != copy1);

            Assert.AreNotEqual(args1.GetHashCode(), args2.GetHashCode());
            Assert.AreNotEqual(args1, args2);
            Assert.AreNotEqual(args2, args1);

            var scm1 = (IStructuralComparable)args1;
            var cmp1 = (IComparable)args1;

            Assert.AreNotEqual(0, args1.CompareTo(args2));
            Assert.AreNotEqual(0, scm1.CompareTo(args2, Comparer<object>.Default));
            Assert.AreNotEqual(0, cmp1.CompareTo(args2));
            Assert.AreEqual(1, scm1.CompareTo(other: null, Comparer<object>.Default));

            Assert.ThrowsException<ArgumentException>(() => scm1.CompareTo("foo", Comparer<object>.Default));

            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int>>(2114956318, 1719192892, 854249433, 300855063, 809284387, 827690485, 1570184411, 134207251, 1465505861, 1587749705, 204182901, 877983323, 447078162, 2017031260, 1232390587, 17694686, new Tuplet<int, int, int, int, int, int, int>(1501248134, 1310193861, 1591727510, 832425546, 1235560812, 1264728858, 1621614627))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int>>(2114956316, 1719192892, 854249433, 300855063, 809284387, 827690485, 1570184411, 134207251, 1465505861, 1587749705, 204182901, 877983323, 447078162, 2017031260, 1232390587, 17694686, new Tuplet<int, int, int, int, int, int, int>(1501248134, 1310193861, 1591727510, 832425546, 1235560812, 1264728858, 1621614627))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int>>(2114956318, 1719192892, 854249433, 300855063, 809284387, 827690485, 1570184411, 134207251, 1465505861, 1587749705, 204182901, 877983323, 447078162, 2017031260, 1232390587, 17694686, new Tuplet<int, int, int, int, int, int, int>(1501248134, 1310193861, 1591727510, 832425546, 1235560812, 1264728858, 1621614627)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int>>(2114956316, 1719192892, 854249433, 300855063, 809284387, 827690485, 1570184411, 134207251, 1465505861, 1587749705, 204182901, 877983323, 447078162, 2017031260, 1232390587, 17694686, new Tuplet<int, int, int, int, int, int, int>(1501248134, 1310193861, 1591727510, 832425546, 1235560812, 1264728858, 1621614627)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int>>(2114956317, 1719192893, 854249433, 300855063, 809284387, 827690485, 1570184411, 134207251, 1465505861, 1587749705, 204182901, 877983323, 447078162, 2017031260, 1232390587, 17694686, new Tuplet<int, int, int, int, int, int, int>(1501248134, 1310193861, 1591727510, 832425546, 1235560812, 1264728858, 1621614627))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int>>(2114956317, 1719192891, 854249433, 300855063, 809284387, 827690485, 1570184411, 134207251, 1465505861, 1587749705, 204182901, 877983323, 447078162, 2017031260, 1232390587, 17694686, new Tuplet<int, int, int, int, int, int, int>(1501248134, 1310193861, 1591727510, 832425546, 1235560812, 1264728858, 1621614627))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int>>(2114956317, 1719192893, 854249433, 300855063, 809284387, 827690485, 1570184411, 134207251, 1465505861, 1587749705, 204182901, 877983323, 447078162, 2017031260, 1232390587, 17694686, new Tuplet<int, int, int, int, int, int, int>(1501248134, 1310193861, 1591727510, 832425546, 1235560812, 1264728858, 1621614627)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int>>(2114956317, 1719192891, 854249433, 300855063, 809284387, 827690485, 1570184411, 134207251, 1465505861, 1587749705, 204182901, 877983323, 447078162, 2017031260, 1232390587, 17694686, new Tuplet<int, int, int, int, int, int, int>(1501248134, 1310193861, 1591727510, 832425546, 1235560812, 1264728858, 1621614627)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int>>(2114956317, 1719192892, 854249434, 300855063, 809284387, 827690485, 1570184411, 134207251, 1465505861, 1587749705, 204182901, 877983323, 447078162, 2017031260, 1232390587, 17694686, new Tuplet<int, int, int, int, int, int, int>(1501248134, 1310193861, 1591727510, 832425546, 1235560812, 1264728858, 1621614627))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int>>(2114956317, 1719192892, 854249432, 300855063, 809284387, 827690485, 1570184411, 134207251, 1465505861, 1587749705, 204182901, 877983323, 447078162, 2017031260, 1232390587, 17694686, new Tuplet<int, int, int, int, int, int, int>(1501248134, 1310193861, 1591727510, 832425546, 1235560812, 1264728858, 1621614627))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int>>(2114956317, 1719192892, 854249434, 300855063, 809284387, 827690485, 1570184411, 134207251, 1465505861, 1587749705, 204182901, 877983323, 447078162, 2017031260, 1232390587, 17694686, new Tuplet<int, int, int, int, int, int, int>(1501248134, 1310193861, 1591727510, 832425546, 1235560812, 1264728858, 1621614627)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int>>(2114956317, 1719192892, 854249432, 300855063, 809284387, 827690485, 1570184411, 134207251, 1465505861, 1587749705, 204182901, 877983323, 447078162, 2017031260, 1232390587, 17694686, new Tuplet<int, int, int, int, int, int, int>(1501248134, 1310193861, 1591727510, 832425546, 1235560812, 1264728858, 1621614627)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int>>(2114956317, 1719192892, 854249433, 300855064, 809284387, 827690485, 1570184411, 134207251, 1465505861, 1587749705, 204182901, 877983323, 447078162, 2017031260, 1232390587, 17694686, new Tuplet<int, int, int, int, int, int, int>(1501248134, 1310193861, 1591727510, 832425546, 1235560812, 1264728858, 1621614627))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int>>(2114956317, 1719192892, 854249433, 300855062, 809284387, 827690485, 1570184411, 134207251, 1465505861, 1587749705, 204182901, 877983323, 447078162, 2017031260, 1232390587, 17694686, new Tuplet<int, int, int, int, int, int, int>(1501248134, 1310193861, 1591727510, 832425546, 1235560812, 1264728858, 1621614627))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int>>(2114956317, 1719192892, 854249433, 300855064, 809284387, 827690485, 1570184411, 134207251, 1465505861, 1587749705, 204182901, 877983323, 447078162, 2017031260, 1232390587, 17694686, new Tuplet<int, int, int, int, int, int, int>(1501248134, 1310193861, 1591727510, 832425546, 1235560812, 1264728858, 1621614627)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int>>(2114956317, 1719192892, 854249433, 300855062, 809284387, 827690485, 1570184411, 134207251, 1465505861, 1587749705, 204182901, 877983323, 447078162, 2017031260, 1232390587, 17694686, new Tuplet<int, int, int, int, int, int, int>(1501248134, 1310193861, 1591727510, 832425546, 1235560812, 1264728858, 1621614627)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int>>(2114956317, 1719192892, 854249433, 300855063, 809284388, 827690485, 1570184411, 134207251, 1465505861, 1587749705, 204182901, 877983323, 447078162, 2017031260, 1232390587, 17694686, new Tuplet<int, int, int, int, int, int, int>(1501248134, 1310193861, 1591727510, 832425546, 1235560812, 1264728858, 1621614627))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int>>(2114956317, 1719192892, 854249433, 300855063, 809284386, 827690485, 1570184411, 134207251, 1465505861, 1587749705, 204182901, 877983323, 447078162, 2017031260, 1232390587, 17694686, new Tuplet<int, int, int, int, int, int, int>(1501248134, 1310193861, 1591727510, 832425546, 1235560812, 1264728858, 1621614627))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int>>(2114956317, 1719192892, 854249433, 300855063, 809284388, 827690485, 1570184411, 134207251, 1465505861, 1587749705, 204182901, 877983323, 447078162, 2017031260, 1232390587, 17694686, new Tuplet<int, int, int, int, int, int, int>(1501248134, 1310193861, 1591727510, 832425546, 1235560812, 1264728858, 1621614627)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int>>(2114956317, 1719192892, 854249433, 300855063, 809284386, 827690485, 1570184411, 134207251, 1465505861, 1587749705, 204182901, 877983323, 447078162, 2017031260, 1232390587, 17694686, new Tuplet<int, int, int, int, int, int, int>(1501248134, 1310193861, 1591727510, 832425546, 1235560812, 1264728858, 1621614627)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int>>(2114956317, 1719192892, 854249433, 300855063, 809284387, 827690486, 1570184411, 134207251, 1465505861, 1587749705, 204182901, 877983323, 447078162, 2017031260, 1232390587, 17694686, new Tuplet<int, int, int, int, int, int, int>(1501248134, 1310193861, 1591727510, 832425546, 1235560812, 1264728858, 1621614627))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int>>(2114956317, 1719192892, 854249433, 300855063, 809284387, 827690484, 1570184411, 134207251, 1465505861, 1587749705, 204182901, 877983323, 447078162, 2017031260, 1232390587, 17694686, new Tuplet<int, int, int, int, int, int, int>(1501248134, 1310193861, 1591727510, 832425546, 1235560812, 1264728858, 1621614627))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int>>(2114956317, 1719192892, 854249433, 300855063, 809284387, 827690486, 1570184411, 134207251, 1465505861, 1587749705, 204182901, 877983323, 447078162, 2017031260, 1232390587, 17694686, new Tuplet<int, int, int, int, int, int, int>(1501248134, 1310193861, 1591727510, 832425546, 1235560812, 1264728858, 1621614627)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int>>(2114956317, 1719192892, 854249433, 300855063, 809284387, 827690484, 1570184411, 134207251, 1465505861, 1587749705, 204182901, 877983323, 447078162, 2017031260, 1232390587, 17694686, new Tuplet<int, int, int, int, int, int, int>(1501248134, 1310193861, 1591727510, 832425546, 1235560812, 1264728858, 1621614627)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int>>(2114956317, 1719192892, 854249433, 300855063, 809284387, 827690485, 1570184412, 134207251, 1465505861, 1587749705, 204182901, 877983323, 447078162, 2017031260, 1232390587, 17694686, new Tuplet<int, int, int, int, int, int, int>(1501248134, 1310193861, 1591727510, 832425546, 1235560812, 1264728858, 1621614627))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int>>(2114956317, 1719192892, 854249433, 300855063, 809284387, 827690485, 1570184410, 134207251, 1465505861, 1587749705, 204182901, 877983323, 447078162, 2017031260, 1232390587, 17694686, new Tuplet<int, int, int, int, int, int, int>(1501248134, 1310193861, 1591727510, 832425546, 1235560812, 1264728858, 1621614627))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int>>(2114956317, 1719192892, 854249433, 300855063, 809284387, 827690485, 1570184412, 134207251, 1465505861, 1587749705, 204182901, 877983323, 447078162, 2017031260, 1232390587, 17694686, new Tuplet<int, int, int, int, int, int, int>(1501248134, 1310193861, 1591727510, 832425546, 1235560812, 1264728858, 1621614627)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int>>(2114956317, 1719192892, 854249433, 300855063, 809284387, 827690485, 1570184410, 134207251, 1465505861, 1587749705, 204182901, 877983323, 447078162, 2017031260, 1232390587, 17694686, new Tuplet<int, int, int, int, int, int, int>(1501248134, 1310193861, 1591727510, 832425546, 1235560812, 1264728858, 1621614627)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int>>(2114956317, 1719192892, 854249433, 300855063, 809284387, 827690485, 1570184411, 134207252, 1465505861, 1587749705, 204182901, 877983323, 447078162, 2017031260, 1232390587, 17694686, new Tuplet<int, int, int, int, int, int, int>(1501248134, 1310193861, 1591727510, 832425546, 1235560812, 1264728858, 1621614627))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int>>(2114956317, 1719192892, 854249433, 300855063, 809284387, 827690485, 1570184411, 134207250, 1465505861, 1587749705, 204182901, 877983323, 447078162, 2017031260, 1232390587, 17694686, new Tuplet<int, int, int, int, int, int, int>(1501248134, 1310193861, 1591727510, 832425546, 1235560812, 1264728858, 1621614627))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int>>(2114956317, 1719192892, 854249433, 300855063, 809284387, 827690485, 1570184411, 134207252, 1465505861, 1587749705, 204182901, 877983323, 447078162, 2017031260, 1232390587, 17694686, new Tuplet<int, int, int, int, int, int, int>(1501248134, 1310193861, 1591727510, 832425546, 1235560812, 1264728858, 1621614627)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int>>(2114956317, 1719192892, 854249433, 300855063, 809284387, 827690485, 1570184411, 134207250, 1465505861, 1587749705, 204182901, 877983323, 447078162, 2017031260, 1232390587, 17694686, new Tuplet<int, int, int, int, int, int, int>(1501248134, 1310193861, 1591727510, 832425546, 1235560812, 1264728858, 1621614627)), Comparer<object>.Default) > 0);

            Assert.IsFalse(args1 == args2);
            Assert.IsTrue(args1 != args2);

            Assert.AreNotEqual(args1, "foo");
            Assert.AreNotEqual(args1, null);

            var seq1 = (IStructuralEquatable)args1;
            Assert.AreEqual(args1.GetHashCode(), seq1.GetHashCode(EqualityComparer<object>.Default));
            Assert.AreEqual(0, seq1.GetHashCode(new ConstantHashEqualityComparer(0)));
            Assert.IsTrue(seq1.Equals(args1, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals(args2, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals("foo", EqualityComparer<object>.Default));

            Assert.AreEqual("(2114956317, 1719192892, 854249433, 300855063, 809284387, 827690485, 1570184411, 134207251, 1465505861, 1587749705, 204182901, 877983323, 447078162, 2017031260, 1232390587, 17694686, 1501248134, 1310193861, 1591727510, 832425546, 1235560812, 1264728858, 1621614627)", args1.ToString());
            Assert.AreEqual("(1203969997, 1294170545, 739071529, 1879120492, 1629031941, 1864050895, 1687718256, 980691969, 1757831543, 1051843323, 1170802528, 998422003, 1264687373, 962252219, 801091070, 1365900347, 1128473686, 1619301817, 819643800, 23678010, 677437036, 773075149, 1350303444)", args2.ToString());
        }

        [TestMethod]
        public void Tuplet24()
        {
            var args1 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int>>(65683637, 1163425122, 337106297, 1229254720, 929885955, 370760834, 507954527, 799168855, 425328642, 850227459, 97578265, 1797763083, 1154168165, 70212858, 1096053640, 2088636117, new Tuplet<int, int, int, int, int, int, int, int>(417640003, 1925271252, 607057736, 593835005, 1973623647, 1423759281, 1018609257, 2115186861));
            var args2 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int>>(1202926114, 700157064, 2091777161, 463253824, 1360607376, 415917012, 1241050848, 944177591, 430894848, 2091350748, 673387892, 715695370, 1291925644, 634796175, 757832301, 609931135, new Tuplet<int, int, int, int, int, int, int, int>(1249877016, 252674468, 745473886, 148194544, 1167109108, 1311972783, 1794406552, 1295687489));

            Assert.AreEqual(65683637, args1.Item1);
            Assert.AreEqual(1202926114, args2.Item1);
            Assert.AreEqual(1163425122, args1.Item2);
            Assert.AreEqual(700157064, args2.Item2);
            Assert.AreEqual(337106297, args1.Item3);
            Assert.AreEqual(2091777161, args2.Item3);
            Assert.AreEqual(1229254720, args1.Item4);
            Assert.AreEqual(463253824, args2.Item4);
            Assert.AreEqual(929885955, args1.Item5);
            Assert.AreEqual(1360607376, args2.Item5);
            Assert.AreEqual(370760834, args1.Item6);
            Assert.AreEqual(415917012, args2.Item6);
            Assert.AreEqual(507954527, args1.Item7);
            Assert.AreEqual(1241050848, args2.Item7);
            Assert.AreEqual(799168855, args1.Item8);
            Assert.AreEqual(944177591, args2.Item8);
            Assert.AreEqual(425328642, args1.Item9);
            Assert.AreEqual(430894848, args2.Item9);
            Assert.AreEqual(850227459, args1.Item10);
            Assert.AreEqual(2091350748, args2.Item10);
            Assert.AreEqual(97578265, args1.Item11);
            Assert.AreEqual(673387892, args2.Item11);
            Assert.AreEqual(1797763083, args1.Item12);
            Assert.AreEqual(715695370, args2.Item12);
            Assert.AreEqual(1154168165, args1.Item13);
            Assert.AreEqual(1291925644, args2.Item13);
            Assert.AreEqual(70212858, args1.Item14);
            Assert.AreEqual(634796175, args2.Item14);
            Assert.AreEqual(1096053640, args1.Item15);
            Assert.AreEqual(757832301, args2.Item15);
            Assert.AreEqual(2088636117, args1.Item16);
            Assert.AreEqual(609931135, args2.Item16);
            Assert.AreEqual(417640003, args1.Rest.Item1);
            Assert.AreEqual(1249877016, args2.Rest.Item1);
            Assert.AreEqual(1925271252, args1.Rest.Item2);
            Assert.AreEqual(252674468, args2.Rest.Item2);
            Assert.AreEqual(607057736, args1.Rest.Item3);
            Assert.AreEqual(745473886, args2.Rest.Item3);
            Assert.AreEqual(593835005, args1.Rest.Item4);
            Assert.AreEqual(148194544, args2.Rest.Item4);
            Assert.AreEqual(1973623647, args1.Rest.Item5);
            Assert.AreEqual(1167109108, args2.Rest.Item5);
            Assert.AreEqual(1423759281, args1.Rest.Item6);
            Assert.AreEqual(1311972783, args2.Rest.Item6);
            Assert.AreEqual(1018609257, args1.Rest.Item7);
            Assert.AreEqual(1794406552, args2.Rest.Item7);
            Assert.AreEqual(2115186861, args1.Rest.Item8);
            Assert.AreEqual(1295687489, args2.Rest.Item8);

            Assert.AreEqual(args1, args1);

            var copy1 = args1;
            Assert.IsTrue(args1 == copy1);
            Assert.IsFalse(args1 != copy1);

            Assert.AreNotEqual(args1.GetHashCode(), args2.GetHashCode());
            Assert.AreNotEqual(args1, args2);
            Assert.AreNotEqual(args2, args1);

            var scm1 = (IStructuralComparable)args1;
            var cmp1 = (IComparable)args1;

            Assert.AreNotEqual(0, args1.CompareTo(args2));
            Assert.AreNotEqual(0, scm1.CompareTo(args2, Comparer<object>.Default));
            Assert.AreNotEqual(0, cmp1.CompareTo(args2));
            Assert.AreEqual(1, scm1.CompareTo(other: null, Comparer<object>.Default));

            Assert.ThrowsException<ArgumentException>(() => scm1.CompareTo("foo", Comparer<object>.Default));

            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int>>(65683638, 1163425122, 337106297, 1229254720, 929885955, 370760834, 507954527, 799168855, 425328642, 850227459, 97578265, 1797763083, 1154168165, 70212858, 1096053640, 2088636117, new Tuplet<int, int, int, int, int, int, int, int>(417640003, 1925271252, 607057736, 593835005, 1973623647, 1423759281, 1018609257, 2115186861))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int>>(65683636, 1163425122, 337106297, 1229254720, 929885955, 370760834, 507954527, 799168855, 425328642, 850227459, 97578265, 1797763083, 1154168165, 70212858, 1096053640, 2088636117, new Tuplet<int, int, int, int, int, int, int, int>(417640003, 1925271252, 607057736, 593835005, 1973623647, 1423759281, 1018609257, 2115186861))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int>>(65683638, 1163425122, 337106297, 1229254720, 929885955, 370760834, 507954527, 799168855, 425328642, 850227459, 97578265, 1797763083, 1154168165, 70212858, 1096053640, 2088636117, new Tuplet<int, int, int, int, int, int, int, int>(417640003, 1925271252, 607057736, 593835005, 1973623647, 1423759281, 1018609257, 2115186861)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int>>(65683636, 1163425122, 337106297, 1229254720, 929885955, 370760834, 507954527, 799168855, 425328642, 850227459, 97578265, 1797763083, 1154168165, 70212858, 1096053640, 2088636117, new Tuplet<int, int, int, int, int, int, int, int>(417640003, 1925271252, 607057736, 593835005, 1973623647, 1423759281, 1018609257, 2115186861)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int>>(65683637, 1163425123, 337106297, 1229254720, 929885955, 370760834, 507954527, 799168855, 425328642, 850227459, 97578265, 1797763083, 1154168165, 70212858, 1096053640, 2088636117, new Tuplet<int, int, int, int, int, int, int, int>(417640003, 1925271252, 607057736, 593835005, 1973623647, 1423759281, 1018609257, 2115186861))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int>>(65683637, 1163425121, 337106297, 1229254720, 929885955, 370760834, 507954527, 799168855, 425328642, 850227459, 97578265, 1797763083, 1154168165, 70212858, 1096053640, 2088636117, new Tuplet<int, int, int, int, int, int, int, int>(417640003, 1925271252, 607057736, 593835005, 1973623647, 1423759281, 1018609257, 2115186861))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int>>(65683637, 1163425123, 337106297, 1229254720, 929885955, 370760834, 507954527, 799168855, 425328642, 850227459, 97578265, 1797763083, 1154168165, 70212858, 1096053640, 2088636117, new Tuplet<int, int, int, int, int, int, int, int>(417640003, 1925271252, 607057736, 593835005, 1973623647, 1423759281, 1018609257, 2115186861)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int>>(65683637, 1163425121, 337106297, 1229254720, 929885955, 370760834, 507954527, 799168855, 425328642, 850227459, 97578265, 1797763083, 1154168165, 70212858, 1096053640, 2088636117, new Tuplet<int, int, int, int, int, int, int, int>(417640003, 1925271252, 607057736, 593835005, 1973623647, 1423759281, 1018609257, 2115186861)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int>>(65683637, 1163425122, 337106298, 1229254720, 929885955, 370760834, 507954527, 799168855, 425328642, 850227459, 97578265, 1797763083, 1154168165, 70212858, 1096053640, 2088636117, new Tuplet<int, int, int, int, int, int, int, int>(417640003, 1925271252, 607057736, 593835005, 1973623647, 1423759281, 1018609257, 2115186861))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int>>(65683637, 1163425122, 337106296, 1229254720, 929885955, 370760834, 507954527, 799168855, 425328642, 850227459, 97578265, 1797763083, 1154168165, 70212858, 1096053640, 2088636117, new Tuplet<int, int, int, int, int, int, int, int>(417640003, 1925271252, 607057736, 593835005, 1973623647, 1423759281, 1018609257, 2115186861))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int>>(65683637, 1163425122, 337106298, 1229254720, 929885955, 370760834, 507954527, 799168855, 425328642, 850227459, 97578265, 1797763083, 1154168165, 70212858, 1096053640, 2088636117, new Tuplet<int, int, int, int, int, int, int, int>(417640003, 1925271252, 607057736, 593835005, 1973623647, 1423759281, 1018609257, 2115186861)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int>>(65683637, 1163425122, 337106296, 1229254720, 929885955, 370760834, 507954527, 799168855, 425328642, 850227459, 97578265, 1797763083, 1154168165, 70212858, 1096053640, 2088636117, new Tuplet<int, int, int, int, int, int, int, int>(417640003, 1925271252, 607057736, 593835005, 1973623647, 1423759281, 1018609257, 2115186861)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int>>(65683637, 1163425122, 337106297, 1229254721, 929885955, 370760834, 507954527, 799168855, 425328642, 850227459, 97578265, 1797763083, 1154168165, 70212858, 1096053640, 2088636117, new Tuplet<int, int, int, int, int, int, int, int>(417640003, 1925271252, 607057736, 593835005, 1973623647, 1423759281, 1018609257, 2115186861))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int>>(65683637, 1163425122, 337106297, 1229254719, 929885955, 370760834, 507954527, 799168855, 425328642, 850227459, 97578265, 1797763083, 1154168165, 70212858, 1096053640, 2088636117, new Tuplet<int, int, int, int, int, int, int, int>(417640003, 1925271252, 607057736, 593835005, 1973623647, 1423759281, 1018609257, 2115186861))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int>>(65683637, 1163425122, 337106297, 1229254721, 929885955, 370760834, 507954527, 799168855, 425328642, 850227459, 97578265, 1797763083, 1154168165, 70212858, 1096053640, 2088636117, new Tuplet<int, int, int, int, int, int, int, int>(417640003, 1925271252, 607057736, 593835005, 1973623647, 1423759281, 1018609257, 2115186861)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int>>(65683637, 1163425122, 337106297, 1229254719, 929885955, 370760834, 507954527, 799168855, 425328642, 850227459, 97578265, 1797763083, 1154168165, 70212858, 1096053640, 2088636117, new Tuplet<int, int, int, int, int, int, int, int>(417640003, 1925271252, 607057736, 593835005, 1973623647, 1423759281, 1018609257, 2115186861)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int>>(65683637, 1163425122, 337106297, 1229254720, 929885956, 370760834, 507954527, 799168855, 425328642, 850227459, 97578265, 1797763083, 1154168165, 70212858, 1096053640, 2088636117, new Tuplet<int, int, int, int, int, int, int, int>(417640003, 1925271252, 607057736, 593835005, 1973623647, 1423759281, 1018609257, 2115186861))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int>>(65683637, 1163425122, 337106297, 1229254720, 929885954, 370760834, 507954527, 799168855, 425328642, 850227459, 97578265, 1797763083, 1154168165, 70212858, 1096053640, 2088636117, new Tuplet<int, int, int, int, int, int, int, int>(417640003, 1925271252, 607057736, 593835005, 1973623647, 1423759281, 1018609257, 2115186861))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int>>(65683637, 1163425122, 337106297, 1229254720, 929885956, 370760834, 507954527, 799168855, 425328642, 850227459, 97578265, 1797763083, 1154168165, 70212858, 1096053640, 2088636117, new Tuplet<int, int, int, int, int, int, int, int>(417640003, 1925271252, 607057736, 593835005, 1973623647, 1423759281, 1018609257, 2115186861)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int>>(65683637, 1163425122, 337106297, 1229254720, 929885954, 370760834, 507954527, 799168855, 425328642, 850227459, 97578265, 1797763083, 1154168165, 70212858, 1096053640, 2088636117, new Tuplet<int, int, int, int, int, int, int, int>(417640003, 1925271252, 607057736, 593835005, 1973623647, 1423759281, 1018609257, 2115186861)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int>>(65683637, 1163425122, 337106297, 1229254720, 929885955, 370760835, 507954527, 799168855, 425328642, 850227459, 97578265, 1797763083, 1154168165, 70212858, 1096053640, 2088636117, new Tuplet<int, int, int, int, int, int, int, int>(417640003, 1925271252, 607057736, 593835005, 1973623647, 1423759281, 1018609257, 2115186861))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int>>(65683637, 1163425122, 337106297, 1229254720, 929885955, 370760833, 507954527, 799168855, 425328642, 850227459, 97578265, 1797763083, 1154168165, 70212858, 1096053640, 2088636117, new Tuplet<int, int, int, int, int, int, int, int>(417640003, 1925271252, 607057736, 593835005, 1973623647, 1423759281, 1018609257, 2115186861))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int>>(65683637, 1163425122, 337106297, 1229254720, 929885955, 370760835, 507954527, 799168855, 425328642, 850227459, 97578265, 1797763083, 1154168165, 70212858, 1096053640, 2088636117, new Tuplet<int, int, int, int, int, int, int, int>(417640003, 1925271252, 607057736, 593835005, 1973623647, 1423759281, 1018609257, 2115186861)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int>>(65683637, 1163425122, 337106297, 1229254720, 929885955, 370760833, 507954527, 799168855, 425328642, 850227459, 97578265, 1797763083, 1154168165, 70212858, 1096053640, 2088636117, new Tuplet<int, int, int, int, int, int, int, int>(417640003, 1925271252, 607057736, 593835005, 1973623647, 1423759281, 1018609257, 2115186861)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int>>(65683637, 1163425122, 337106297, 1229254720, 929885955, 370760834, 507954528, 799168855, 425328642, 850227459, 97578265, 1797763083, 1154168165, 70212858, 1096053640, 2088636117, new Tuplet<int, int, int, int, int, int, int, int>(417640003, 1925271252, 607057736, 593835005, 1973623647, 1423759281, 1018609257, 2115186861))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int>>(65683637, 1163425122, 337106297, 1229254720, 929885955, 370760834, 507954526, 799168855, 425328642, 850227459, 97578265, 1797763083, 1154168165, 70212858, 1096053640, 2088636117, new Tuplet<int, int, int, int, int, int, int, int>(417640003, 1925271252, 607057736, 593835005, 1973623647, 1423759281, 1018609257, 2115186861))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int>>(65683637, 1163425122, 337106297, 1229254720, 929885955, 370760834, 507954528, 799168855, 425328642, 850227459, 97578265, 1797763083, 1154168165, 70212858, 1096053640, 2088636117, new Tuplet<int, int, int, int, int, int, int, int>(417640003, 1925271252, 607057736, 593835005, 1973623647, 1423759281, 1018609257, 2115186861)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int>>(65683637, 1163425122, 337106297, 1229254720, 929885955, 370760834, 507954526, 799168855, 425328642, 850227459, 97578265, 1797763083, 1154168165, 70212858, 1096053640, 2088636117, new Tuplet<int, int, int, int, int, int, int, int>(417640003, 1925271252, 607057736, 593835005, 1973623647, 1423759281, 1018609257, 2115186861)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int>>(65683637, 1163425122, 337106297, 1229254720, 929885955, 370760834, 507954527, 799168856, 425328642, 850227459, 97578265, 1797763083, 1154168165, 70212858, 1096053640, 2088636117, new Tuplet<int, int, int, int, int, int, int, int>(417640003, 1925271252, 607057736, 593835005, 1973623647, 1423759281, 1018609257, 2115186861))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int>>(65683637, 1163425122, 337106297, 1229254720, 929885955, 370760834, 507954527, 799168854, 425328642, 850227459, 97578265, 1797763083, 1154168165, 70212858, 1096053640, 2088636117, new Tuplet<int, int, int, int, int, int, int, int>(417640003, 1925271252, 607057736, 593835005, 1973623647, 1423759281, 1018609257, 2115186861))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int>>(65683637, 1163425122, 337106297, 1229254720, 929885955, 370760834, 507954527, 799168856, 425328642, 850227459, 97578265, 1797763083, 1154168165, 70212858, 1096053640, 2088636117, new Tuplet<int, int, int, int, int, int, int, int>(417640003, 1925271252, 607057736, 593835005, 1973623647, 1423759281, 1018609257, 2115186861)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int>>(65683637, 1163425122, 337106297, 1229254720, 929885955, 370760834, 507954527, 799168854, 425328642, 850227459, 97578265, 1797763083, 1154168165, 70212858, 1096053640, 2088636117, new Tuplet<int, int, int, int, int, int, int, int>(417640003, 1925271252, 607057736, 593835005, 1973623647, 1423759281, 1018609257, 2115186861)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int>>(65683637, 1163425122, 337106297, 1229254720, 929885955, 370760834, 507954527, 799168855, 425328643, 850227459, 97578265, 1797763083, 1154168165, 70212858, 1096053640, 2088636117, new Tuplet<int, int, int, int, int, int, int, int>(417640003, 1925271252, 607057736, 593835005, 1973623647, 1423759281, 1018609257, 2115186861))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int>>(65683637, 1163425122, 337106297, 1229254720, 929885955, 370760834, 507954527, 799168855, 425328641, 850227459, 97578265, 1797763083, 1154168165, 70212858, 1096053640, 2088636117, new Tuplet<int, int, int, int, int, int, int, int>(417640003, 1925271252, 607057736, 593835005, 1973623647, 1423759281, 1018609257, 2115186861))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int>>(65683637, 1163425122, 337106297, 1229254720, 929885955, 370760834, 507954527, 799168855, 425328643, 850227459, 97578265, 1797763083, 1154168165, 70212858, 1096053640, 2088636117, new Tuplet<int, int, int, int, int, int, int, int>(417640003, 1925271252, 607057736, 593835005, 1973623647, 1423759281, 1018609257, 2115186861)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int>>(65683637, 1163425122, 337106297, 1229254720, 929885955, 370760834, 507954527, 799168855, 425328641, 850227459, 97578265, 1797763083, 1154168165, 70212858, 1096053640, 2088636117, new Tuplet<int, int, int, int, int, int, int, int>(417640003, 1925271252, 607057736, 593835005, 1973623647, 1423759281, 1018609257, 2115186861)), Comparer<object>.Default) > 0);

            Assert.IsFalse(args1 == args2);
            Assert.IsTrue(args1 != args2);

            Assert.AreNotEqual(args1, "foo");
            Assert.AreNotEqual(args1, null);

            var seq1 = (IStructuralEquatable)args1;
            Assert.AreEqual(args1.GetHashCode(), seq1.GetHashCode(EqualityComparer<object>.Default));
            Assert.AreEqual(0, seq1.GetHashCode(new ConstantHashEqualityComparer(0)));
            Assert.IsTrue(seq1.Equals(args1, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals(args2, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals("foo", EqualityComparer<object>.Default));

            Assert.AreEqual("(65683637, 1163425122, 337106297, 1229254720, 929885955, 370760834, 507954527, 799168855, 425328642, 850227459, 97578265, 1797763083, 1154168165, 70212858, 1096053640, 2088636117, 417640003, 1925271252, 607057736, 593835005, 1973623647, 1423759281, 1018609257, 2115186861)", args1.ToString());
            Assert.AreEqual("(1202926114, 700157064, 2091777161, 463253824, 1360607376, 415917012, 1241050848, 944177591, 430894848, 2091350748, 673387892, 715695370, 1291925644, 634796175, 757832301, 609931135, 1249877016, 252674468, 745473886, 148194544, 1167109108, 1311972783, 1794406552, 1295687489)", args2.ToString());
        }

        [TestMethod]
        public void Tuplet25()
        {
            var args1 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int>>(32420046, 1678149347, 402003797, 245890405, 70379300, 179240144, 1524163444, 789408003, 144815865, 369403083, 26328606, 229728891, 426467320, 44700703, 1586045126, 9411630, new Tuplet<int, int, int, int, int, int, int, int, int>(1756660258, 1300884321, 1366868235, 1210301064, 1544308613, 380358270, 796710473, 1930327475, 1167438951));
            var args2 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int>>(2144610248, 1491441636, 1720949179, 678285395, 870414713, 948077753, 2038436978, 1053234159, 796089672, 430833778, 1829941676, 13913215, 995160443, 873798291, 251654704, 567187304, new Tuplet<int, int, int, int, int, int, int, int, int>(2031463536, 570879505, 922522561, 608467569, 528103410, 183463815, 1205176313, 814112989, 736062256));

            Assert.AreEqual(32420046, args1.Item1);
            Assert.AreEqual(2144610248, args2.Item1);
            Assert.AreEqual(1678149347, args1.Item2);
            Assert.AreEqual(1491441636, args2.Item2);
            Assert.AreEqual(402003797, args1.Item3);
            Assert.AreEqual(1720949179, args2.Item3);
            Assert.AreEqual(245890405, args1.Item4);
            Assert.AreEqual(678285395, args2.Item4);
            Assert.AreEqual(70379300, args1.Item5);
            Assert.AreEqual(870414713, args2.Item5);
            Assert.AreEqual(179240144, args1.Item6);
            Assert.AreEqual(948077753, args2.Item6);
            Assert.AreEqual(1524163444, args1.Item7);
            Assert.AreEqual(2038436978, args2.Item7);
            Assert.AreEqual(789408003, args1.Item8);
            Assert.AreEqual(1053234159, args2.Item8);
            Assert.AreEqual(144815865, args1.Item9);
            Assert.AreEqual(796089672, args2.Item9);
            Assert.AreEqual(369403083, args1.Item10);
            Assert.AreEqual(430833778, args2.Item10);
            Assert.AreEqual(26328606, args1.Item11);
            Assert.AreEqual(1829941676, args2.Item11);
            Assert.AreEqual(229728891, args1.Item12);
            Assert.AreEqual(13913215, args2.Item12);
            Assert.AreEqual(426467320, args1.Item13);
            Assert.AreEqual(995160443, args2.Item13);
            Assert.AreEqual(44700703, args1.Item14);
            Assert.AreEqual(873798291, args2.Item14);
            Assert.AreEqual(1586045126, args1.Item15);
            Assert.AreEqual(251654704, args2.Item15);
            Assert.AreEqual(9411630, args1.Item16);
            Assert.AreEqual(567187304, args2.Item16);
            Assert.AreEqual(1756660258, args1.Rest.Item1);
            Assert.AreEqual(2031463536, args2.Rest.Item1);
            Assert.AreEqual(1300884321, args1.Rest.Item2);
            Assert.AreEqual(570879505, args2.Rest.Item2);
            Assert.AreEqual(1366868235, args1.Rest.Item3);
            Assert.AreEqual(922522561, args2.Rest.Item3);
            Assert.AreEqual(1210301064, args1.Rest.Item4);
            Assert.AreEqual(608467569, args2.Rest.Item4);
            Assert.AreEqual(1544308613, args1.Rest.Item5);
            Assert.AreEqual(528103410, args2.Rest.Item5);
            Assert.AreEqual(380358270, args1.Rest.Item6);
            Assert.AreEqual(183463815, args2.Rest.Item6);
            Assert.AreEqual(796710473, args1.Rest.Item7);
            Assert.AreEqual(1205176313, args2.Rest.Item7);
            Assert.AreEqual(1930327475, args1.Rest.Item8);
            Assert.AreEqual(814112989, args2.Rest.Item8);
            Assert.AreEqual(1167438951, args1.Rest.Item9);
            Assert.AreEqual(736062256, args2.Rest.Item9);

            Assert.AreEqual(args1, args1);

            var copy1 = args1;
            Assert.IsTrue(args1 == copy1);
            Assert.IsFalse(args1 != copy1);

            Assert.AreNotEqual(args1.GetHashCode(), args2.GetHashCode());
            Assert.AreNotEqual(args1, args2);
            Assert.AreNotEqual(args2, args1);

            var scm1 = (IStructuralComparable)args1;
            var cmp1 = (IComparable)args1;

            Assert.AreNotEqual(0, args1.CompareTo(args2));
            Assert.AreNotEqual(0, scm1.CompareTo(args2, Comparer<object>.Default));
            Assert.AreNotEqual(0, cmp1.CompareTo(args2));
            Assert.AreEqual(1, scm1.CompareTo(other: null, Comparer<object>.Default));

            Assert.ThrowsException<ArgumentException>(() => scm1.CompareTo("foo", Comparer<object>.Default));

            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int>>(32420047, 1678149347, 402003797, 245890405, 70379300, 179240144, 1524163444, 789408003, 144815865, 369403083, 26328606, 229728891, 426467320, 44700703, 1586045126, 9411630, new Tuplet<int, int, int, int, int, int, int, int, int>(1756660258, 1300884321, 1366868235, 1210301064, 1544308613, 380358270, 796710473, 1930327475, 1167438951))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int>>(32420045, 1678149347, 402003797, 245890405, 70379300, 179240144, 1524163444, 789408003, 144815865, 369403083, 26328606, 229728891, 426467320, 44700703, 1586045126, 9411630, new Tuplet<int, int, int, int, int, int, int, int, int>(1756660258, 1300884321, 1366868235, 1210301064, 1544308613, 380358270, 796710473, 1930327475, 1167438951))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int>>(32420047, 1678149347, 402003797, 245890405, 70379300, 179240144, 1524163444, 789408003, 144815865, 369403083, 26328606, 229728891, 426467320, 44700703, 1586045126, 9411630, new Tuplet<int, int, int, int, int, int, int, int, int>(1756660258, 1300884321, 1366868235, 1210301064, 1544308613, 380358270, 796710473, 1930327475, 1167438951)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int>>(32420045, 1678149347, 402003797, 245890405, 70379300, 179240144, 1524163444, 789408003, 144815865, 369403083, 26328606, 229728891, 426467320, 44700703, 1586045126, 9411630, new Tuplet<int, int, int, int, int, int, int, int, int>(1756660258, 1300884321, 1366868235, 1210301064, 1544308613, 380358270, 796710473, 1930327475, 1167438951)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int>>(32420046, 1678149348, 402003797, 245890405, 70379300, 179240144, 1524163444, 789408003, 144815865, 369403083, 26328606, 229728891, 426467320, 44700703, 1586045126, 9411630, new Tuplet<int, int, int, int, int, int, int, int, int>(1756660258, 1300884321, 1366868235, 1210301064, 1544308613, 380358270, 796710473, 1930327475, 1167438951))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int>>(32420046, 1678149346, 402003797, 245890405, 70379300, 179240144, 1524163444, 789408003, 144815865, 369403083, 26328606, 229728891, 426467320, 44700703, 1586045126, 9411630, new Tuplet<int, int, int, int, int, int, int, int, int>(1756660258, 1300884321, 1366868235, 1210301064, 1544308613, 380358270, 796710473, 1930327475, 1167438951))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int>>(32420046, 1678149348, 402003797, 245890405, 70379300, 179240144, 1524163444, 789408003, 144815865, 369403083, 26328606, 229728891, 426467320, 44700703, 1586045126, 9411630, new Tuplet<int, int, int, int, int, int, int, int, int>(1756660258, 1300884321, 1366868235, 1210301064, 1544308613, 380358270, 796710473, 1930327475, 1167438951)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int>>(32420046, 1678149346, 402003797, 245890405, 70379300, 179240144, 1524163444, 789408003, 144815865, 369403083, 26328606, 229728891, 426467320, 44700703, 1586045126, 9411630, new Tuplet<int, int, int, int, int, int, int, int, int>(1756660258, 1300884321, 1366868235, 1210301064, 1544308613, 380358270, 796710473, 1930327475, 1167438951)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int>>(32420046, 1678149347, 402003798, 245890405, 70379300, 179240144, 1524163444, 789408003, 144815865, 369403083, 26328606, 229728891, 426467320, 44700703, 1586045126, 9411630, new Tuplet<int, int, int, int, int, int, int, int, int>(1756660258, 1300884321, 1366868235, 1210301064, 1544308613, 380358270, 796710473, 1930327475, 1167438951))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int>>(32420046, 1678149347, 402003796, 245890405, 70379300, 179240144, 1524163444, 789408003, 144815865, 369403083, 26328606, 229728891, 426467320, 44700703, 1586045126, 9411630, new Tuplet<int, int, int, int, int, int, int, int, int>(1756660258, 1300884321, 1366868235, 1210301064, 1544308613, 380358270, 796710473, 1930327475, 1167438951))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int>>(32420046, 1678149347, 402003798, 245890405, 70379300, 179240144, 1524163444, 789408003, 144815865, 369403083, 26328606, 229728891, 426467320, 44700703, 1586045126, 9411630, new Tuplet<int, int, int, int, int, int, int, int, int>(1756660258, 1300884321, 1366868235, 1210301064, 1544308613, 380358270, 796710473, 1930327475, 1167438951)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int>>(32420046, 1678149347, 402003796, 245890405, 70379300, 179240144, 1524163444, 789408003, 144815865, 369403083, 26328606, 229728891, 426467320, 44700703, 1586045126, 9411630, new Tuplet<int, int, int, int, int, int, int, int, int>(1756660258, 1300884321, 1366868235, 1210301064, 1544308613, 380358270, 796710473, 1930327475, 1167438951)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int>>(32420046, 1678149347, 402003797, 245890406, 70379300, 179240144, 1524163444, 789408003, 144815865, 369403083, 26328606, 229728891, 426467320, 44700703, 1586045126, 9411630, new Tuplet<int, int, int, int, int, int, int, int, int>(1756660258, 1300884321, 1366868235, 1210301064, 1544308613, 380358270, 796710473, 1930327475, 1167438951))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int>>(32420046, 1678149347, 402003797, 245890404, 70379300, 179240144, 1524163444, 789408003, 144815865, 369403083, 26328606, 229728891, 426467320, 44700703, 1586045126, 9411630, new Tuplet<int, int, int, int, int, int, int, int, int>(1756660258, 1300884321, 1366868235, 1210301064, 1544308613, 380358270, 796710473, 1930327475, 1167438951))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int>>(32420046, 1678149347, 402003797, 245890406, 70379300, 179240144, 1524163444, 789408003, 144815865, 369403083, 26328606, 229728891, 426467320, 44700703, 1586045126, 9411630, new Tuplet<int, int, int, int, int, int, int, int, int>(1756660258, 1300884321, 1366868235, 1210301064, 1544308613, 380358270, 796710473, 1930327475, 1167438951)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int>>(32420046, 1678149347, 402003797, 245890404, 70379300, 179240144, 1524163444, 789408003, 144815865, 369403083, 26328606, 229728891, 426467320, 44700703, 1586045126, 9411630, new Tuplet<int, int, int, int, int, int, int, int, int>(1756660258, 1300884321, 1366868235, 1210301064, 1544308613, 380358270, 796710473, 1930327475, 1167438951)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int>>(32420046, 1678149347, 402003797, 245890405, 70379301, 179240144, 1524163444, 789408003, 144815865, 369403083, 26328606, 229728891, 426467320, 44700703, 1586045126, 9411630, new Tuplet<int, int, int, int, int, int, int, int, int>(1756660258, 1300884321, 1366868235, 1210301064, 1544308613, 380358270, 796710473, 1930327475, 1167438951))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int>>(32420046, 1678149347, 402003797, 245890405, 70379299, 179240144, 1524163444, 789408003, 144815865, 369403083, 26328606, 229728891, 426467320, 44700703, 1586045126, 9411630, new Tuplet<int, int, int, int, int, int, int, int, int>(1756660258, 1300884321, 1366868235, 1210301064, 1544308613, 380358270, 796710473, 1930327475, 1167438951))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int>>(32420046, 1678149347, 402003797, 245890405, 70379301, 179240144, 1524163444, 789408003, 144815865, 369403083, 26328606, 229728891, 426467320, 44700703, 1586045126, 9411630, new Tuplet<int, int, int, int, int, int, int, int, int>(1756660258, 1300884321, 1366868235, 1210301064, 1544308613, 380358270, 796710473, 1930327475, 1167438951)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int>>(32420046, 1678149347, 402003797, 245890405, 70379299, 179240144, 1524163444, 789408003, 144815865, 369403083, 26328606, 229728891, 426467320, 44700703, 1586045126, 9411630, new Tuplet<int, int, int, int, int, int, int, int, int>(1756660258, 1300884321, 1366868235, 1210301064, 1544308613, 380358270, 796710473, 1930327475, 1167438951)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int>>(32420046, 1678149347, 402003797, 245890405, 70379300, 179240145, 1524163444, 789408003, 144815865, 369403083, 26328606, 229728891, 426467320, 44700703, 1586045126, 9411630, new Tuplet<int, int, int, int, int, int, int, int, int>(1756660258, 1300884321, 1366868235, 1210301064, 1544308613, 380358270, 796710473, 1930327475, 1167438951))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int>>(32420046, 1678149347, 402003797, 245890405, 70379300, 179240143, 1524163444, 789408003, 144815865, 369403083, 26328606, 229728891, 426467320, 44700703, 1586045126, 9411630, new Tuplet<int, int, int, int, int, int, int, int, int>(1756660258, 1300884321, 1366868235, 1210301064, 1544308613, 380358270, 796710473, 1930327475, 1167438951))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int>>(32420046, 1678149347, 402003797, 245890405, 70379300, 179240145, 1524163444, 789408003, 144815865, 369403083, 26328606, 229728891, 426467320, 44700703, 1586045126, 9411630, new Tuplet<int, int, int, int, int, int, int, int, int>(1756660258, 1300884321, 1366868235, 1210301064, 1544308613, 380358270, 796710473, 1930327475, 1167438951)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int>>(32420046, 1678149347, 402003797, 245890405, 70379300, 179240143, 1524163444, 789408003, 144815865, 369403083, 26328606, 229728891, 426467320, 44700703, 1586045126, 9411630, new Tuplet<int, int, int, int, int, int, int, int, int>(1756660258, 1300884321, 1366868235, 1210301064, 1544308613, 380358270, 796710473, 1930327475, 1167438951)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int>>(32420046, 1678149347, 402003797, 245890405, 70379300, 179240144, 1524163445, 789408003, 144815865, 369403083, 26328606, 229728891, 426467320, 44700703, 1586045126, 9411630, new Tuplet<int, int, int, int, int, int, int, int, int>(1756660258, 1300884321, 1366868235, 1210301064, 1544308613, 380358270, 796710473, 1930327475, 1167438951))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int>>(32420046, 1678149347, 402003797, 245890405, 70379300, 179240144, 1524163443, 789408003, 144815865, 369403083, 26328606, 229728891, 426467320, 44700703, 1586045126, 9411630, new Tuplet<int, int, int, int, int, int, int, int, int>(1756660258, 1300884321, 1366868235, 1210301064, 1544308613, 380358270, 796710473, 1930327475, 1167438951))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int>>(32420046, 1678149347, 402003797, 245890405, 70379300, 179240144, 1524163445, 789408003, 144815865, 369403083, 26328606, 229728891, 426467320, 44700703, 1586045126, 9411630, new Tuplet<int, int, int, int, int, int, int, int, int>(1756660258, 1300884321, 1366868235, 1210301064, 1544308613, 380358270, 796710473, 1930327475, 1167438951)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int>>(32420046, 1678149347, 402003797, 245890405, 70379300, 179240144, 1524163443, 789408003, 144815865, 369403083, 26328606, 229728891, 426467320, 44700703, 1586045126, 9411630, new Tuplet<int, int, int, int, int, int, int, int, int>(1756660258, 1300884321, 1366868235, 1210301064, 1544308613, 380358270, 796710473, 1930327475, 1167438951)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int>>(32420046, 1678149347, 402003797, 245890405, 70379300, 179240144, 1524163444, 789408004, 144815865, 369403083, 26328606, 229728891, 426467320, 44700703, 1586045126, 9411630, new Tuplet<int, int, int, int, int, int, int, int, int>(1756660258, 1300884321, 1366868235, 1210301064, 1544308613, 380358270, 796710473, 1930327475, 1167438951))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int>>(32420046, 1678149347, 402003797, 245890405, 70379300, 179240144, 1524163444, 789408002, 144815865, 369403083, 26328606, 229728891, 426467320, 44700703, 1586045126, 9411630, new Tuplet<int, int, int, int, int, int, int, int, int>(1756660258, 1300884321, 1366868235, 1210301064, 1544308613, 380358270, 796710473, 1930327475, 1167438951))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int>>(32420046, 1678149347, 402003797, 245890405, 70379300, 179240144, 1524163444, 789408004, 144815865, 369403083, 26328606, 229728891, 426467320, 44700703, 1586045126, 9411630, new Tuplet<int, int, int, int, int, int, int, int, int>(1756660258, 1300884321, 1366868235, 1210301064, 1544308613, 380358270, 796710473, 1930327475, 1167438951)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int>>(32420046, 1678149347, 402003797, 245890405, 70379300, 179240144, 1524163444, 789408002, 144815865, 369403083, 26328606, 229728891, 426467320, 44700703, 1586045126, 9411630, new Tuplet<int, int, int, int, int, int, int, int, int>(1756660258, 1300884321, 1366868235, 1210301064, 1544308613, 380358270, 796710473, 1930327475, 1167438951)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int>>(32420046, 1678149347, 402003797, 245890405, 70379300, 179240144, 1524163444, 789408003, 144815866, 369403083, 26328606, 229728891, 426467320, 44700703, 1586045126, 9411630, new Tuplet<int, int, int, int, int, int, int, int, int>(1756660258, 1300884321, 1366868235, 1210301064, 1544308613, 380358270, 796710473, 1930327475, 1167438951))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int>>(32420046, 1678149347, 402003797, 245890405, 70379300, 179240144, 1524163444, 789408003, 144815864, 369403083, 26328606, 229728891, 426467320, 44700703, 1586045126, 9411630, new Tuplet<int, int, int, int, int, int, int, int, int>(1756660258, 1300884321, 1366868235, 1210301064, 1544308613, 380358270, 796710473, 1930327475, 1167438951))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int>>(32420046, 1678149347, 402003797, 245890405, 70379300, 179240144, 1524163444, 789408003, 144815866, 369403083, 26328606, 229728891, 426467320, 44700703, 1586045126, 9411630, new Tuplet<int, int, int, int, int, int, int, int, int>(1756660258, 1300884321, 1366868235, 1210301064, 1544308613, 380358270, 796710473, 1930327475, 1167438951)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int>>(32420046, 1678149347, 402003797, 245890405, 70379300, 179240144, 1524163444, 789408003, 144815864, 369403083, 26328606, 229728891, 426467320, 44700703, 1586045126, 9411630, new Tuplet<int, int, int, int, int, int, int, int, int>(1756660258, 1300884321, 1366868235, 1210301064, 1544308613, 380358270, 796710473, 1930327475, 1167438951)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int>>(32420046, 1678149347, 402003797, 245890405, 70379300, 179240144, 1524163444, 789408003, 144815865, 369403084, 26328606, 229728891, 426467320, 44700703, 1586045126, 9411630, new Tuplet<int, int, int, int, int, int, int, int, int>(1756660258, 1300884321, 1366868235, 1210301064, 1544308613, 380358270, 796710473, 1930327475, 1167438951))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int>>(32420046, 1678149347, 402003797, 245890405, 70379300, 179240144, 1524163444, 789408003, 144815865, 369403082, 26328606, 229728891, 426467320, 44700703, 1586045126, 9411630, new Tuplet<int, int, int, int, int, int, int, int, int>(1756660258, 1300884321, 1366868235, 1210301064, 1544308613, 380358270, 796710473, 1930327475, 1167438951))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int>>(32420046, 1678149347, 402003797, 245890405, 70379300, 179240144, 1524163444, 789408003, 144815865, 369403084, 26328606, 229728891, 426467320, 44700703, 1586045126, 9411630, new Tuplet<int, int, int, int, int, int, int, int, int>(1756660258, 1300884321, 1366868235, 1210301064, 1544308613, 380358270, 796710473, 1930327475, 1167438951)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int>>(32420046, 1678149347, 402003797, 245890405, 70379300, 179240144, 1524163444, 789408003, 144815865, 369403082, 26328606, 229728891, 426467320, 44700703, 1586045126, 9411630, new Tuplet<int, int, int, int, int, int, int, int, int>(1756660258, 1300884321, 1366868235, 1210301064, 1544308613, 380358270, 796710473, 1930327475, 1167438951)), Comparer<object>.Default) > 0);

            Assert.IsFalse(args1 == args2);
            Assert.IsTrue(args1 != args2);

            Assert.AreNotEqual(args1, "foo");
            Assert.AreNotEqual(args1, null);

            var seq1 = (IStructuralEquatable)args1;
            Assert.AreEqual(args1.GetHashCode(), seq1.GetHashCode(EqualityComparer<object>.Default));
            Assert.AreEqual(0, seq1.GetHashCode(new ConstantHashEqualityComparer(0)));
            Assert.IsTrue(seq1.Equals(args1, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals(args2, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals("foo", EqualityComparer<object>.Default));

            Assert.AreEqual("(32420046, 1678149347, 402003797, 245890405, 70379300, 179240144, 1524163444, 789408003, 144815865, 369403083, 26328606, 229728891, 426467320, 44700703, 1586045126, 9411630, 1756660258, 1300884321, 1366868235, 1210301064, 1544308613, 380358270, 796710473, 1930327475, 1167438951)", args1.ToString());
            Assert.AreEqual("(2144610248, 1491441636, 1720949179, 678285395, 870414713, 948077753, 2038436978, 1053234159, 796089672, 430833778, 1829941676, 13913215, 995160443, 873798291, 251654704, 567187304, 2031463536, 570879505, 922522561, 608467569, 528103410, 183463815, 1205176313, 814112989, 736062256)", args2.ToString());
        }

        [TestMethod]
        public void Tuplet26()
        {
            var args1 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int>>(539017933, 2013708434, 2092588195, 584105488, 1898862523, 1799545423, 881438874, 619159969, 1225935101, 73252699, 835282155, 1950697912, 111122608, 1421884799, 1568808977, 135375275, new Tuplet<int, int, int, int, int, int, int, int, int, int>(1323978379, 1777861295, 1761350572, 1903587097, 2142982062, 761499815, 427086030, 1115213531, 643113760, 1660328724));
            var args2 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int>>(1956962412, 2021671559, 1321859906, 639335541, 1961146433, 286265323, 906836190, 2089706786, 331396780, 1081852966, 2093332430, 469128671, 1044710796, 778772002, 948502802, 1542236893, new Tuplet<int, int, int, int, int, int, int, int, int, int>(1916708989, 800545592, 1563856196, 763973039, 1920340928, 1296478353, 1501197231, 473092294, 1351608678, 553086167));

            Assert.AreEqual(539017933, args1.Item1);
            Assert.AreEqual(1956962412, args2.Item1);
            Assert.AreEqual(2013708434, args1.Item2);
            Assert.AreEqual(2021671559, args2.Item2);
            Assert.AreEqual(2092588195, args1.Item3);
            Assert.AreEqual(1321859906, args2.Item3);
            Assert.AreEqual(584105488, args1.Item4);
            Assert.AreEqual(639335541, args2.Item4);
            Assert.AreEqual(1898862523, args1.Item5);
            Assert.AreEqual(1961146433, args2.Item5);
            Assert.AreEqual(1799545423, args1.Item6);
            Assert.AreEqual(286265323, args2.Item6);
            Assert.AreEqual(881438874, args1.Item7);
            Assert.AreEqual(906836190, args2.Item7);
            Assert.AreEqual(619159969, args1.Item8);
            Assert.AreEqual(2089706786, args2.Item8);
            Assert.AreEqual(1225935101, args1.Item9);
            Assert.AreEqual(331396780, args2.Item9);
            Assert.AreEqual(73252699, args1.Item10);
            Assert.AreEqual(1081852966, args2.Item10);
            Assert.AreEqual(835282155, args1.Item11);
            Assert.AreEqual(2093332430, args2.Item11);
            Assert.AreEqual(1950697912, args1.Item12);
            Assert.AreEqual(469128671, args2.Item12);
            Assert.AreEqual(111122608, args1.Item13);
            Assert.AreEqual(1044710796, args2.Item13);
            Assert.AreEqual(1421884799, args1.Item14);
            Assert.AreEqual(778772002, args2.Item14);
            Assert.AreEqual(1568808977, args1.Item15);
            Assert.AreEqual(948502802, args2.Item15);
            Assert.AreEqual(135375275, args1.Item16);
            Assert.AreEqual(1542236893, args2.Item16);
            Assert.AreEqual(1323978379, args1.Rest.Item1);
            Assert.AreEqual(1916708989, args2.Rest.Item1);
            Assert.AreEqual(1777861295, args1.Rest.Item2);
            Assert.AreEqual(800545592, args2.Rest.Item2);
            Assert.AreEqual(1761350572, args1.Rest.Item3);
            Assert.AreEqual(1563856196, args2.Rest.Item3);
            Assert.AreEqual(1903587097, args1.Rest.Item4);
            Assert.AreEqual(763973039, args2.Rest.Item4);
            Assert.AreEqual(2142982062, args1.Rest.Item5);
            Assert.AreEqual(1920340928, args2.Rest.Item5);
            Assert.AreEqual(761499815, args1.Rest.Item6);
            Assert.AreEqual(1296478353, args2.Rest.Item6);
            Assert.AreEqual(427086030, args1.Rest.Item7);
            Assert.AreEqual(1501197231, args2.Rest.Item7);
            Assert.AreEqual(1115213531, args1.Rest.Item8);
            Assert.AreEqual(473092294, args2.Rest.Item8);
            Assert.AreEqual(643113760, args1.Rest.Item9);
            Assert.AreEqual(1351608678, args2.Rest.Item9);
            Assert.AreEqual(1660328724, args1.Rest.Item10);
            Assert.AreEqual(553086167, args2.Rest.Item10);

            Assert.AreEqual(args1, args1);

            var copy1 = args1;
            Assert.IsTrue(args1 == copy1);
            Assert.IsFalse(args1 != copy1);

            Assert.AreNotEqual(args1.GetHashCode(), args2.GetHashCode());
            Assert.AreNotEqual(args1, args2);
            Assert.AreNotEqual(args2, args1);

            var scm1 = (IStructuralComparable)args1;
            var cmp1 = (IComparable)args1;

            Assert.AreNotEqual(0, args1.CompareTo(args2));
            Assert.AreNotEqual(0, scm1.CompareTo(args2, Comparer<object>.Default));
            Assert.AreNotEqual(0, cmp1.CompareTo(args2));
            Assert.AreEqual(1, scm1.CompareTo(other: null, Comparer<object>.Default));

            Assert.ThrowsException<ArgumentException>(() => scm1.CompareTo("foo", Comparer<object>.Default));

            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int>>(539017934, 2013708434, 2092588195, 584105488, 1898862523, 1799545423, 881438874, 619159969, 1225935101, 73252699, 835282155, 1950697912, 111122608, 1421884799, 1568808977, 135375275, new Tuplet<int, int, int, int, int, int, int, int, int, int>(1323978379, 1777861295, 1761350572, 1903587097, 2142982062, 761499815, 427086030, 1115213531, 643113760, 1660328724))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int>>(539017932, 2013708434, 2092588195, 584105488, 1898862523, 1799545423, 881438874, 619159969, 1225935101, 73252699, 835282155, 1950697912, 111122608, 1421884799, 1568808977, 135375275, new Tuplet<int, int, int, int, int, int, int, int, int, int>(1323978379, 1777861295, 1761350572, 1903587097, 2142982062, 761499815, 427086030, 1115213531, 643113760, 1660328724))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int>>(539017934, 2013708434, 2092588195, 584105488, 1898862523, 1799545423, 881438874, 619159969, 1225935101, 73252699, 835282155, 1950697912, 111122608, 1421884799, 1568808977, 135375275, new Tuplet<int, int, int, int, int, int, int, int, int, int>(1323978379, 1777861295, 1761350572, 1903587097, 2142982062, 761499815, 427086030, 1115213531, 643113760, 1660328724)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int>>(539017932, 2013708434, 2092588195, 584105488, 1898862523, 1799545423, 881438874, 619159969, 1225935101, 73252699, 835282155, 1950697912, 111122608, 1421884799, 1568808977, 135375275, new Tuplet<int, int, int, int, int, int, int, int, int, int>(1323978379, 1777861295, 1761350572, 1903587097, 2142982062, 761499815, 427086030, 1115213531, 643113760, 1660328724)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int>>(539017933, 2013708435, 2092588195, 584105488, 1898862523, 1799545423, 881438874, 619159969, 1225935101, 73252699, 835282155, 1950697912, 111122608, 1421884799, 1568808977, 135375275, new Tuplet<int, int, int, int, int, int, int, int, int, int>(1323978379, 1777861295, 1761350572, 1903587097, 2142982062, 761499815, 427086030, 1115213531, 643113760, 1660328724))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int>>(539017933, 2013708433, 2092588195, 584105488, 1898862523, 1799545423, 881438874, 619159969, 1225935101, 73252699, 835282155, 1950697912, 111122608, 1421884799, 1568808977, 135375275, new Tuplet<int, int, int, int, int, int, int, int, int, int>(1323978379, 1777861295, 1761350572, 1903587097, 2142982062, 761499815, 427086030, 1115213531, 643113760, 1660328724))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int>>(539017933, 2013708435, 2092588195, 584105488, 1898862523, 1799545423, 881438874, 619159969, 1225935101, 73252699, 835282155, 1950697912, 111122608, 1421884799, 1568808977, 135375275, new Tuplet<int, int, int, int, int, int, int, int, int, int>(1323978379, 1777861295, 1761350572, 1903587097, 2142982062, 761499815, 427086030, 1115213531, 643113760, 1660328724)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int>>(539017933, 2013708433, 2092588195, 584105488, 1898862523, 1799545423, 881438874, 619159969, 1225935101, 73252699, 835282155, 1950697912, 111122608, 1421884799, 1568808977, 135375275, new Tuplet<int, int, int, int, int, int, int, int, int, int>(1323978379, 1777861295, 1761350572, 1903587097, 2142982062, 761499815, 427086030, 1115213531, 643113760, 1660328724)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int>>(539017933, 2013708434, 2092588196, 584105488, 1898862523, 1799545423, 881438874, 619159969, 1225935101, 73252699, 835282155, 1950697912, 111122608, 1421884799, 1568808977, 135375275, new Tuplet<int, int, int, int, int, int, int, int, int, int>(1323978379, 1777861295, 1761350572, 1903587097, 2142982062, 761499815, 427086030, 1115213531, 643113760, 1660328724))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int>>(539017933, 2013708434, 2092588194, 584105488, 1898862523, 1799545423, 881438874, 619159969, 1225935101, 73252699, 835282155, 1950697912, 111122608, 1421884799, 1568808977, 135375275, new Tuplet<int, int, int, int, int, int, int, int, int, int>(1323978379, 1777861295, 1761350572, 1903587097, 2142982062, 761499815, 427086030, 1115213531, 643113760, 1660328724))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int>>(539017933, 2013708434, 2092588196, 584105488, 1898862523, 1799545423, 881438874, 619159969, 1225935101, 73252699, 835282155, 1950697912, 111122608, 1421884799, 1568808977, 135375275, new Tuplet<int, int, int, int, int, int, int, int, int, int>(1323978379, 1777861295, 1761350572, 1903587097, 2142982062, 761499815, 427086030, 1115213531, 643113760, 1660328724)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int>>(539017933, 2013708434, 2092588194, 584105488, 1898862523, 1799545423, 881438874, 619159969, 1225935101, 73252699, 835282155, 1950697912, 111122608, 1421884799, 1568808977, 135375275, new Tuplet<int, int, int, int, int, int, int, int, int, int>(1323978379, 1777861295, 1761350572, 1903587097, 2142982062, 761499815, 427086030, 1115213531, 643113760, 1660328724)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int>>(539017933, 2013708434, 2092588195, 584105489, 1898862523, 1799545423, 881438874, 619159969, 1225935101, 73252699, 835282155, 1950697912, 111122608, 1421884799, 1568808977, 135375275, new Tuplet<int, int, int, int, int, int, int, int, int, int>(1323978379, 1777861295, 1761350572, 1903587097, 2142982062, 761499815, 427086030, 1115213531, 643113760, 1660328724))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int>>(539017933, 2013708434, 2092588195, 584105487, 1898862523, 1799545423, 881438874, 619159969, 1225935101, 73252699, 835282155, 1950697912, 111122608, 1421884799, 1568808977, 135375275, new Tuplet<int, int, int, int, int, int, int, int, int, int>(1323978379, 1777861295, 1761350572, 1903587097, 2142982062, 761499815, 427086030, 1115213531, 643113760, 1660328724))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int>>(539017933, 2013708434, 2092588195, 584105489, 1898862523, 1799545423, 881438874, 619159969, 1225935101, 73252699, 835282155, 1950697912, 111122608, 1421884799, 1568808977, 135375275, new Tuplet<int, int, int, int, int, int, int, int, int, int>(1323978379, 1777861295, 1761350572, 1903587097, 2142982062, 761499815, 427086030, 1115213531, 643113760, 1660328724)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int>>(539017933, 2013708434, 2092588195, 584105487, 1898862523, 1799545423, 881438874, 619159969, 1225935101, 73252699, 835282155, 1950697912, 111122608, 1421884799, 1568808977, 135375275, new Tuplet<int, int, int, int, int, int, int, int, int, int>(1323978379, 1777861295, 1761350572, 1903587097, 2142982062, 761499815, 427086030, 1115213531, 643113760, 1660328724)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int>>(539017933, 2013708434, 2092588195, 584105488, 1898862524, 1799545423, 881438874, 619159969, 1225935101, 73252699, 835282155, 1950697912, 111122608, 1421884799, 1568808977, 135375275, new Tuplet<int, int, int, int, int, int, int, int, int, int>(1323978379, 1777861295, 1761350572, 1903587097, 2142982062, 761499815, 427086030, 1115213531, 643113760, 1660328724))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int>>(539017933, 2013708434, 2092588195, 584105488, 1898862522, 1799545423, 881438874, 619159969, 1225935101, 73252699, 835282155, 1950697912, 111122608, 1421884799, 1568808977, 135375275, new Tuplet<int, int, int, int, int, int, int, int, int, int>(1323978379, 1777861295, 1761350572, 1903587097, 2142982062, 761499815, 427086030, 1115213531, 643113760, 1660328724))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int>>(539017933, 2013708434, 2092588195, 584105488, 1898862524, 1799545423, 881438874, 619159969, 1225935101, 73252699, 835282155, 1950697912, 111122608, 1421884799, 1568808977, 135375275, new Tuplet<int, int, int, int, int, int, int, int, int, int>(1323978379, 1777861295, 1761350572, 1903587097, 2142982062, 761499815, 427086030, 1115213531, 643113760, 1660328724)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int>>(539017933, 2013708434, 2092588195, 584105488, 1898862522, 1799545423, 881438874, 619159969, 1225935101, 73252699, 835282155, 1950697912, 111122608, 1421884799, 1568808977, 135375275, new Tuplet<int, int, int, int, int, int, int, int, int, int>(1323978379, 1777861295, 1761350572, 1903587097, 2142982062, 761499815, 427086030, 1115213531, 643113760, 1660328724)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int>>(539017933, 2013708434, 2092588195, 584105488, 1898862523, 1799545424, 881438874, 619159969, 1225935101, 73252699, 835282155, 1950697912, 111122608, 1421884799, 1568808977, 135375275, new Tuplet<int, int, int, int, int, int, int, int, int, int>(1323978379, 1777861295, 1761350572, 1903587097, 2142982062, 761499815, 427086030, 1115213531, 643113760, 1660328724))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int>>(539017933, 2013708434, 2092588195, 584105488, 1898862523, 1799545422, 881438874, 619159969, 1225935101, 73252699, 835282155, 1950697912, 111122608, 1421884799, 1568808977, 135375275, new Tuplet<int, int, int, int, int, int, int, int, int, int>(1323978379, 1777861295, 1761350572, 1903587097, 2142982062, 761499815, 427086030, 1115213531, 643113760, 1660328724))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int>>(539017933, 2013708434, 2092588195, 584105488, 1898862523, 1799545424, 881438874, 619159969, 1225935101, 73252699, 835282155, 1950697912, 111122608, 1421884799, 1568808977, 135375275, new Tuplet<int, int, int, int, int, int, int, int, int, int>(1323978379, 1777861295, 1761350572, 1903587097, 2142982062, 761499815, 427086030, 1115213531, 643113760, 1660328724)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int>>(539017933, 2013708434, 2092588195, 584105488, 1898862523, 1799545422, 881438874, 619159969, 1225935101, 73252699, 835282155, 1950697912, 111122608, 1421884799, 1568808977, 135375275, new Tuplet<int, int, int, int, int, int, int, int, int, int>(1323978379, 1777861295, 1761350572, 1903587097, 2142982062, 761499815, 427086030, 1115213531, 643113760, 1660328724)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int>>(539017933, 2013708434, 2092588195, 584105488, 1898862523, 1799545423, 881438875, 619159969, 1225935101, 73252699, 835282155, 1950697912, 111122608, 1421884799, 1568808977, 135375275, new Tuplet<int, int, int, int, int, int, int, int, int, int>(1323978379, 1777861295, 1761350572, 1903587097, 2142982062, 761499815, 427086030, 1115213531, 643113760, 1660328724))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int>>(539017933, 2013708434, 2092588195, 584105488, 1898862523, 1799545423, 881438873, 619159969, 1225935101, 73252699, 835282155, 1950697912, 111122608, 1421884799, 1568808977, 135375275, new Tuplet<int, int, int, int, int, int, int, int, int, int>(1323978379, 1777861295, 1761350572, 1903587097, 2142982062, 761499815, 427086030, 1115213531, 643113760, 1660328724))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int>>(539017933, 2013708434, 2092588195, 584105488, 1898862523, 1799545423, 881438875, 619159969, 1225935101, 73252699, 835282155, 1950697912, 111122608, 1421884799, 1568808977, 135375275, new Tuplet<int, int, int, int, int, int, int, int, int, int>(1323978379, 1777861295, 1761350572, 1903587097, 2142982062, 761499815, 427086030, 1115213531, 643113760, 1660328724)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int>>(539017933, 2013708434, 2092588195, 584105488, 1898862523, 1799545423, 881438873, 619159969, 1225935101, 73252699, 835282155, 1950697912, 111122608, 1421884799, 1568808977, 135375275, new Tuplet<int, int, int, int, int, int, int, int, int, int>(1323978379, 1777861295, 1761350572, 1903587097, 2142982062, 761499815, 427086030, 1115213531, 643113760, 1660328724)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int>>(539017933, 2013708434, 2092588195, 584105488, 1898862523, 1799545423, 881438874, 619159970, 1225935101, 73252699, 835282155, 1950697912, 111122608, 1421884799, 1568808977, 135375275, new Tuplet<int, int, int, int, int, int, int, int, int, int>(1323978379, 1777861295, 1761350572, 1903587097, 2142982062, 761499815, 427086030, 1115213531, 643113760, 1660328724))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int>>(539017933, 2013708434, 2092588195, 584105488, 1898862523, 1799545423, 881438874, 619159968, 1225935101, 73252699, 835282155, 1950697912, 111122608, 1421884799, 1568808977, 135375275, new Tuplet<int, int, int, int, int, int, int, int, int, int>(1323978379, 1777861295, 1761350572, 1903587097, 2142982062, 761499815, 427086030, 1115213531, 643113760, 1660328724))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int>>(539017933, 2013708434, 2092588195, 584105488, 1898862523, 1799545423, 881438874, 619159970, 1225935101, 73252699, 835282155, 1950697912, 111122608, 1421884799, 1568808977, 135375275, new Tuplet<int, int, int, int, int, int, int, int, int, int>(1323978379, 1777861295, 1761350572, 1903587097, 2142982062, 761499815, 427086030, 1115213531, 643113760, 1660328724)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int>>(539017933, 2013708434, 2092588195, 584105488, 1898862523, 1799545423, 881438874, 619159968, 1225935101, 73252699, 835282155, 1950697912, 111122608, 1421884799, 1568808977, 135375275, new Tuplet<int, int, int, int, int, int, int, int, int, int>(1323978379, 1777861295, 1761350572, 1903587097, 2142982062, 761499815, 427086030, 1115213531, 643113760, 1660328724)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int>>(539017933, 2013708434, 2092588195, 584105488, 1898862523, 1799545423, 881438874, 619159969, 1225935102, 73252699, 835282155, 1950697912, 111122608, 1421884799, 1568808977, 135375275, new Tuplet<int, int, int, int, int, int, int, int, int, int>(1323978379, 1777861295, 1761350572, 1903587097, 2142982062, 761499815, 427086030, 1115213531, 643113760, 1660328724))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int>>(539017933, 2013708434, 2092588195, 584105488, 1898862523, 1799545423, 881438874, 619159969, 1225935100, 73252699, 835282155, 1950697912, 111122608, 1421884799, 1568808977, 135375275, new Tuplet<int, int, int, int, int, int, int, int, int, int>(1323978379, 1777861295, 1761350572, 1903587097, 2142982062, 761499815, 427086030, 1115213531, 643113760, 1660328724))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int>>(539017933, 2013708434, 2092588195, 584105488, 1898862523, 1799545423, 881438874, 619159969, 1225935102, 73252699, 835282155, 1950697912, 111122608, 1421884799, 1568808977, 135375275, new Tuplet<int, int, int, int, int, int, int, int, int, int>(1323978379, 1777861295, 1761350572, 1903587097, 2142982062, 761499815, 427086030, 1115213531, 643113760, 1660328724)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int>>(539017933, 2013708434, 2092588195, 584105488, 1898862523, 1799545423, 881438874, 619159969, 1225935100, 73252699, 835282155, 1950697912, 111122608, 1421884799, 1568808977, 135375275, new Tuplet<int, int, int, int, int, int, int, int, int, int>(1323978379, 1777861295, 1761350572, 1903587097, 2142982062, 761499815, 427086030, 1115213531, 643113760, 1660328724)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int>>(539017933, 2013708434, 2092588195, 584105488, 1898862523, 1799545423, 881438874, 619159969, 1225935101, 73252700, 835282155, 1950697912, 111122608, 1421884799, 1568808977, 135375275, new Tuplet<int, int, int, int, int, int, int, int, int, int>(1323978379, 1777861295, 1761350572, 1903587097, 2142982062, 761499815, 427086030, 1115213531, 643113760, 1660328724))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int>>(539017933, 2013708434, 2092588195, 584105488, 1898862523, 1799545423, 881438874, 619159969, 1225935101, 73252698, 835282155, 1950697912, 111122608, 1421884799, 1568808977, 135375275, new Tuplet<int, int, int, int, int, int, int, int, int, int>(1323978379, 1777861295, 1761350572, 1903587097, 2142982062, 761499815, 427086030, 1115213531, 643113760, 1660328724))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int>>(539017933, 2013708434, 2092588195, 584105488, 1898862523, 1799545423, 881438874, 619159969, 1225935101, 73252700, 835282155, 1950697912, 111122608, 1421884799, 1568808977, 135375275, new Tuplet<int, int, int, int, int, int, int, int, int, int>(1323978379, 1777861295, 1761350572, 1903587097, 2142982062, 761499815, 427086030, 1115213531, 643113760, 1660328724)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int>>(539017933, 2013708434, 2092588195, 584105488, 1898862523, 1799545423, 881438874, 619159969, 1225935101, 73252698, 835282155, 1950697912, 111122608, 1421884799, 1568808977, 135375275, new Tuplet<int, int, int, int, int, int, int, int, int, int>(1323978379, 1777861295, 1761350572, 1903587097, 2142982062, 761499815, 427086030, 1115213531, 643113760, 1660328724)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int>>(539017933, 2013708434, 2092588195, 584105488, 1898862523, 1799545423, 881438874, 619159969, 1225935101, 73252699, 835282156, 1950697912, 111122608, 1421884799, 1568808977, 135375275, new Tuplet<int, int, int, int, int, int, int, int, int, int>(1323978379, 1777861295, 1761350572, 1903587097, 2142982062, 761499815, 427086030, 1115213531, 643113760, 1660328724))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int>>(539017933, 2013708434, 2092588195, 584105488, 1898862523, 1799545423, 881438874, 619159969, 1225935101, 73252699, 835282154, 1950697912, 111122608, 1421884799, 1568808977, 135375275, new Tuplet<int, int, int, int, int, int, int, int, int, int>(1323978379, 1777861295, 1761350572, 1903587097, 2142982062, 761499815, 427086030, 1115213531, 643113760, 1660328724))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int>>(539017933, 2013708434, 2092588195, 584105488, 1898862523, 1799545423, 881438874, 619159969, 1225935101, 73252699, 835282156, 1950697912, 111122608, 1421884799, 1568808977, 135375275, new Tuplet<int, int, int, int, int, int, int, int, int, int>(1323978379, 1777861295, 1761350572, 1903587097, 2142982062, 761499815, 427086030, 1115213531, 643113760, 1660328724)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int>>(539017933, 2013708434, 2092588195, 584105488, 1898862523, 1799545423, 881438874, 619159969, 1225935101, 73252699, 835282154, 1950697912, 111122608, 1421884799, 1568808977, 135375275, new Tuplet<int, int, int, int, int, int, int, int, int, int>(1323978379, 1777861295, 1761350572, 1903587097, 2142982062, 761499815, 427086030, 1115213531, 643113760, 1660328724)), Comparer<object>.Default) > 0);

            Assert.IsFalse(args1 == args2);
            Assert.IsTrue(args1 != args2);

            Assert.AreNotEqual(args1, "foo");
            Assert.AreNotEqual(args1, null);

            var seq1 = (IStructuralEquatable)args1;
            Assert.AreEqual(args1.GetHashCode(), seq1.GetHashCode(EqualityComparer<object>.Default));
            Assert.AreEqual(0, seq1.GetHashCode(new ConstantHashEqualityComparer(0)));
            Assert.IsTrue(seq1.Equals(args1, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals(args2, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals("foo", EqualityComparer<object>.Default));

            Assert.AreEqual("(539017933, 2013708434, 2092588195, 584105488, 1898862523, 1799545423, 881438874, 619159969, 1225935101, 73252699, 835282155, 1950697912, 111122608, 1421884799, 1568808977, 135375275, 1323978379, 1777861295, 1761350572, 1903587097, 2142982062, 761499815, 427086030, 1115213531, 643113760, 1660328724)", args1.ToString());
            Assert.AreEqual("(1956962412, 2021671559, 1321859906, 639335541, 1961146433, 286265323, 906836190, 2089706786, 331396780, 1081852966, 2093332430, 469128671, 1044710796, 778772002, 948502802, 1542236893, 1916708989, 800545592, 1563856196, 763973039, 1920340928, 1296478353, 1501197231, 473092294, 1351608678, 553086167)", args2.ToString());
        }

        [TestMethod]
        public void Tuplet27()
        {
            var args1 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309388, 1058009539, 740563841, 1925001765, 1586622404, 977374664, 2088475375, 238533799, 1990066658, 1007250962, 1444783710, 586599560, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982));
            var args2 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(2026624368, 1887471443, 660484059, 520474328, 848767612, 1435210510, 1408060266, 842439582, 1996310298, 1349142945, 553878662, 1642714209, 1115957766, 528136943, 806176997, 936188991, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(2088735487, 97453183, 1330109429, 540955679, 1014839364, 1867594964, 1751441459, 205990334, 1014241220, 283565802, 496758970));

            Assert.AreEqual(1591309388, args1.Item1);
            Assert.AreEqual(2026624368, args2.Item1);
            Assert.AreEqual(1058009539, args1.Item2);
            Assert.AreEqual(1887471443, args2.Item2);
            Assert.AreEqual(740563841, args1.Item3);
            Assert.AreEqual(660484059, args2.Item3);
            Assert.AreEqual(1925001765, args1.Item4);
            Assert.AreEqual(520474328, args2.Item4);
            Assert.AreEqual(1586622404, args1.Item5);
            Assert.AreEqual(848767612, args2.Item5);
            Assert.AreEqual(977374664, args1.Item6);
            Assert.AreEqual(1435210510, args2.Item6);
            Assert.AreEqual(2088475375, args1.Item7);
            Assert.AreEqual(1408060266, args2.Item7);
            Assert.AreEqual(238533799, args1.Item8);
            Assert.AreEqual(842439582, args2.Item8);
            Assert.AreEqual(1990066658, args1.Item9);
            Assert.AreEqual(1996310298, args2.Item9);
            Assert.AreEqual(1007250962, args1.Item10);
            Assert.AreEqual(1349142945, args2.Item10);
            Assert.AreEqual(1444783710, args1.Item11);
            Assert.AreEqual(553878662, args2.Item11);
            Assert.AreEqual(586599560, args1.Item12);
            Assert.AreEqual(1642714209, args2.Item12);
            Assert.AreEqual(259589913, args1.Item13);
            Assert.AreEqual(1115957766, args2.Item13);
            Assert.AreEqual(549016832, args1.Item14);
            Assert.AreEqual(528136943, args2.Item14);
            Assert.AreEqual(1043861722, args1.Item15);
            Assert.AreEqual(806176997, args2.Item15);
            Assert.AreEqual(168899469, args1.Item16);
            Assert.AreEqual(936188991, args2.Item16);
            Assert.AreEqual(1090488019, args1.Rest.Item1);
            Assert.AreEqual(2088735487, args2.Rest.Item1);
            Assert.AreEqual(486956011, args1.Rest.Item2);
            Assert.AreEqual(97453183, args2.Rest.Item2);
            Assert.AreEqual(189526492, args1.Rest.Item3);
            Assert.AreEqual(1330109429, args2.Rest.Item3);
            Assert.AreEqual(854849708, args1.Rest.Item4);
            Assert.AreEqual(540955679, args2.Rest.Item4);
            Assert.AreEqual(733150499, args1.Rest.Item5);
            Assert.AreEqual(1014839364, args2.Rest.Item5);
            Assert.AreEqual(982578570, args1.Rest.Item6);
            Assert.AreEqual(1867594964, args2.Rest.Item6);
            Assert.AreEqual(955084295, args1.Rest.Item7);
            Assert.AreEqual(1751441459, args2.Rest.Item7);
            Assert.AreEqual(600745169, args1.Rest.Item8);
            Assert.AreEqual(205990334, args2.Rest.Item8);
            Assert.AreEqual(992274473, args1.Rest.Item9);
            Assert.AreEqual(1014241220, args2.Rest.Item9);
            Assert.AreEqual(1774024085, args1.Rest.Item10);
            Assert.AreEqual(283565802, args2.Rest.Item10);
            Assert.AreEqual(1698840982, args1.Rest.Item11);
            Assert.AreEqual(496758970, args2.Rest.Item11);

            Assert.AreEqual(args1, args1);

            var copy1 = args1;
            Assert.IsTrue(args1 == copy1);
            Assert.IsFalse(args1 != copy1);

            Assert.AreNotEqual(args1.GetHashCode(), args2.GetHashCode());
            Assert.AreNotEqual(args1, args2);
            Assert.AreNotEqual(args2, args1);

            var scm1 = (IStructuralComparable)args1;
            var cmp1 = (IComparable)args1;

            Assert.AreNotEqual(0, args1.CompareTo(args2));
            Assert.AreNotEqual(0, scm1.CompareTo(args2, Comparer<object>.Default));
            Assert.AreNotEqual(0, cmp1.CompareTo(args2));
            Assert.AreEqual(1, scm1.CompareTo(other: null, Comparer<object>.Default));

            Assert.ThrowsException<ArgumentException>(() => scm1.CompareTo("foo", Comparer<object>.Default));

            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309389, 1058009539, 740563841, 1925001765, 1586622404, 977374664, 2088475375, 238533799, 1990066658, 1007250962, 1444783710, 586599560, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309387, 1058009539, 740563841, 1925001765, 1586622404, 977374664, 2088475375, 238533799, 1990066658, 1007250962, 1444783710, 586599560, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309389, 1058009539, 740563841, 1925001765, 1586622404, 977374664, 2088475375, 238533799, 1990066658, 1007250962, 1444783710, 586599560, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309387, 1058009539, 740563841, 1925001765, 1586622404, 977374664, 2088475375, 238533799, 1990066658, 1007250962, 1444783710, 586599560, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309388, 1058009540, 740563841, 1925001765, 1586622404, 977374664, 2088475375, 238533799, 1990066658, 1007250962, 1444783710, 586599560, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309388, 1058009538, 740563841, 1925001765, 1586622404, 977374664, 2088475375, 238533799, 1990066658, 1007250962, 1444783710, 586599560, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309388, 1058009540, 740563841, 1925001765, 1586622404, 977374664, 2088475375, 238533799, 1990066658, 1007250962, 1444783710, 586599560, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309388, 1058009538, 740563841, 1925001765, 1586622404, 977374664, 2088475375, 238533799, 1990066658, 1007250962, 1444783710, 586599560, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309388, 1058009539, 740563842, 1925001765, 1586622404, 977374664, 2088475375, 238533799, 1990066658, 1007250962, 1444783710, 586599560, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309388, 1058009539, 740563840, 1925001765, 1586622404, 977374664, 2088475375, 238533799, 1990066658, 1007250962, 1444783710, 586599560, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309388, 1058009539, 740563842, 1925001765, 1586622404, 977374664, 2088475375, 238533799, 1990066658, 1007250962, 1444783710, 586599560, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309388, 1058009539, 740563840, 1925001765, 1586622404, 977374664, 2088475375, 238533799, 1990066658, 1007250962, 1444783710, 586599560, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309388, 1058009539, 740563841, 1925001766, 1586622404, 977374664, 2088475375, 238533799, 1990066658, 1007250962, 1444783710, 586599560, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309388, 1058009539, 740563841, 1925001764, 1586622404, 977374664, 2088475375, 238533799, 1990066658, 1007250962, 1444783710, 586599560, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309388, 1058009539, 740563841, 1925001766, 1586622404, 977374664, 2088475375, 238533799, 1990066658, 1007250962, 1444783710, 586599560, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309388, 1058009539, 740563841, 1925001764, 1586622404, 977374664, 2088475375, 238533799, 1990066658, 1007250962, 1444783710, 586599560, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309388, 1058009539, 740563841, 1925001765, 1586622405, 977374664, 2088475375, 238533799, 1990066658, 1007250962, 1444783710, 586599560, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309388, 1058009539, 740563841, 1925001765, 1586622403, 977374664, 2088475375, 238533799, 1990066658, 1007250962, 1444783710, 586599560, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309388, 1058009539, 740563841, 1925001765, 1586622405, 977374664, 2088475375, 238533799, 1990066658, 1007250962, 1444783710, 586599560, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309388, 1058009539, 740563841, 1925001765, 1586622403, 977374664, 2088475375, 238533799, 1990066658, 1007250962, 1444783710, 586599560, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309388, 1058009539, 740563841, 1925001765, 1586622404, 977374665, 2088475375, 238533799, 1990066658, 1007250962, 1444783710, 586599560, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309388, 1058009539, 740563841, 1925001765, 1586622404, 977374663, 2088475375, 238533799, 1990066658, 1007250962, 1444783710, 586599560, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309388, 1058009539, 740563841, 1925001765, 1586622404, 977374665, 2088475375, 238533799, 1990066658, 1007250962, 1444783710, 586599560, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309388, 1058009539, 740563841, 1925001765, 1586622404, 977374663, 2088475375, 238533799, 1990066658, 1007250962, 1444783710, 586599560, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309388, 1058009539, 740563841, 1925001765, 1586622404, 977374664, 2088475376, 238533799, 1990066658, 1007250962, 1444783710, 586599560, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309388, 1058009539, 740563841, 1925001765, 1586622404, 977374664, 2088475374, 238533799, 1990066658, 1007250962, 1444783710, 586599560, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309388, 1058009539, 740563841, 1925001765, 1586622404, 977374664, 2088475376, 238533799, 1990066658, 1007250962, 1444783710, 586599560, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309388, 1058009539, 740563841, 1925001765, 1586622404, 977374664, 2088475374, 238533799, 1990066658, 1007250962, 1444783710, 586599560, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309388, 1058009539, 740563841, 1925001765, 1586622404, 977374664, 2088475375, 238533800, 1990066658, 1007250962, 1444783710, 586599560, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309388, 1058009539, 740563841, 1925001765, 1586622404, 977374664, 2088475375, 238533798, 1990066658, 1007250962, 1444783710, 586599560, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309388, 1058009539, 740563841, 1925001765, 1586622404, 977374664, 2088475375, 238533800, 1990066658, 1007250962, 1444783710, 586599560, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309388, 1058009539, 740563841, 1925001765, 1586622404, 977374664, 2088475375, 238533798, 1990066658, 1007250962, 1444783710, 586599560, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309388, 1058009539, 740563841, 1925001765, 1586622404, 977374664, 2088475375, 238533799, 1990066659, 1007250962, 1444783710, 586599560, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309388, 1058009539, 740563841, 1925001765, 1586622404, 977374664, 2088475375, 238533799, 1990066657, 1007250962, 1444783710, 586599560, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309388, 1058009539, 740563841, 1925001765, 1586622404, 977374664, 2088475375, 238533799, 1990066659, 1007250962, 1444783710, 586599560, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309388, 1058009539, 740563841, 1925001765, 1586622404, 977374664, 2088475375, 238533799, 1990066657, 1007250962, 1444783710, 586599560, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309388, 1058009539, 740563841, 1925001765, 1586622404, 977374664, 2088475375, 238533799, 1990066658, 1007250963, 1444783710, 586599560, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309388, 1058009539, 740563841, 1925001765, 1586622404, 977374664, 2088475375, 238533799, 1990066658, 1007250961, 1444783710, 586599560, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309388, 1058009539, 740563841, 1925001765, 1586622404, 977374664, 2088475375, 238533799, 1990066658, 1007250963, 1444783710, 586599560, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309388, 1058009539, 740563841, 1925001765, 1586622404, 977374664, 2088475375, 238533799, 1990066658, 1007250961, 1444783710, 586599560, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309388, 1058009539, 740563841, 1925001765, 1586622404, 977374664, 2088475375, 238533799, 1990066658, 1007250962, 1444783711, 586599560, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309388, 1058009539, 740563841, 1925001765, 1586622404, 977374664, 2088475375, 238533799, 1990066658, 1007250962, 1444783709, 586599560, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309388, 1058009539, 740563841, 1925001765, 1586622404, 977374664, 2088475375, 238533799, 1990066658, 1007250962, 1444783711, 586599560, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309388, 1058009539, 740563841, 1925001765, 1586622404, 977374664, 2088475375, 238533799, 1990066658, 1007250962, 1444783709, 586599560, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309388, 1058009539, 740563841, 1925001765, 1586622404, 977374664, 2088475375, 238533799, 1990066658, 1007250962, 1444783710, 586599561, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309388, 1058009539, 740563841, 1925001765, 1586622404, 977374664, 2088475375, 238533799, 1990066658, 1007250962, 1444783710, 586599559, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309388, 1058009539, 740563841, 1925001765, 1586622404, 977374664, 2088475375, 238533799, 1990066658, 1007250962, 1444783710, 586599561, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int>>(1591309388, 1058009539, 740563841, 1925001765, 1586622404, 977374664, 2088475375, 238533799, 1990066658, 1007250962, 1444783710, 586599559, 259589913, 549016832, 1043861722, 168899469, new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982)), Comparer<object>.Default) > 0);

            Assert.IsFalse(args1 == args2);
            Assert.IsTrue(args1 != args2);

            Assert.AreNotEqual(args1, "foo");
            Assert.AreNotEqual(args1, null);

            var seq1 = (IStructuralEquatable)args1;
            Assert.AreEqual(args1.GetHashCode(), seq1.GetHashCode(EqualityComparer<object>.Default));
            Assert.AreEqual(0, seq1.GetHashCode(new ConstantHashEqualityComparer(0)));
            Assert.IsTrue(seq1.Equals(args1, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals(args2, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals("foo", EqualityComparer<object>.Default));

            Assert.AreEqual("(1591309388, 1058009539, 740563841, 1925001765, 1586622404, 977374664, 2088475375, 238533799, 1990066658, 1007250962, 1444783710, 586599560, 259589913, 549016832, 1043861722, 168899469, 1090488019, 486956011, 189526492, 854849708, 733150499, 982578570, 955084295, 600745169, 992274473, 1774024085, 1698840982)", args1.ToString());
            Assert.AreEqual("(2026624368, 1887471443, 660484059, 520474328, 848767612, 1435210510, 1408060266, 842439582, 1996310298, 1349142945, 553878662, 1642714209, 1115957766, 528136943, 806176997, 936188991, 2088735487, 97453183, 1330109429, 540955679, 1014839364, 1867594964, 1751441459, 205990334, 1014241220, 283565802, 496758970)", args2.ToString());
        }

        [TestMethod]
        public void Tuplet28()
        {
            var args1 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730818, 102925244, 139818672, 932727292, 1960081966, 1426017329, 61851007, 498546003, 1329582599, 486776634, 596016098, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618));
            var args2 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(159029404, 136029984, 454493725, 1653716755, 565201810, 938451540, 1588124598, 233708764, 1893385054, 1209324273, 1768635017, 1830115890, 1837424084, 466285936, 307630994, 1754090039, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(1601958853, 1648920732, 31236732, 1689426032, 1308262114, 672559893, 784201288, 1816864624, 22515771, 1209997076, 170046205, 2040445813));

            Assert.AreEqual(1967419315, args1.Item1);
            Assert.AreEqual(159029404, args2.Item1);
            Assert.AreEqual(608730818, args1.Item2);
            Assert.AreEqual(136029984, args2.Item2);
            Assert.AreEqual(102925244, args1.Item3);
            Assert.AreEqual(454493725, args2.Item3);
            Assert.AreEqual(139818672, args1.Item4);
            Assert.AreEqual(1653716755, args2.Item4);
            Assert.AreEqual(932727292, args1.Item5);
            Assert.AreEqual(565201810, args2.Item5);
            Assert.AreEqual(1960081966, args1.Item6);
            Assert.AreEqual(938451540, args2.Item6);
            Assert.AreEqual(1426017329, args1.Item7);
            Assert.AreEqual(1588124598, args2.Item7);
            Assert.AreEqual(61851007, args1.Item8);
            Assert.AreEqual(233708764, args2.Item8);
            Assert.AreEqual(498546003, args1.Item9);
            Assert.AreEqual(1893385054, args2.Item9);
            Assert.AreEqual(1329582599, args1.Item10);
            Assert.AreEqual(1209324273, args2.Item10);
            Assert.AreEqual(486776634, args1.Item11);
            Assert.AreEqual(1768635017, args2.Item11);
            Assert.AreEqual(596016098, args1.Item12);
            Assert.AreEqual(1830115890, args2.Item12);
            Assert.AreEqual(1298872697, args1.Item13);
            Assert.AreEqual(1837424084, args2.Item13);
            Assert.AreEqual(999013294, args1.Item14);
            Assert.AreEqual(466285936, args2.Item14);
            Assert.AreEqual(1854060897, args1.Item15);
            Assert.AreEqual(307630994, args2.Item15);
            Assert.AreEqual(1195035071, args1.Item16);
            Assert.AreEqual(1754090039, args2.Item16);
            Assert.AreEqual(967240171, args1.Rest.Item1);
            Assert.AreEqual(1601958853, args2.Rest.Item1);
            Assert.AreEqual(536609357, args1.Rest.Item2);
            Assert.AreEqual(1648920732, args2.Rest.Item2);
            Assert.AreEqual(991725449, args1.Rest.Item3);
            Assert.AreEqual(31236732, args2.Rest.Item3);
            Assert.AreEqual(1221052373, args1.Rest.Item4);
            Assert.AreEqual(1689426032, args2.Rest.Item4);
            Assert.AreEqual(326712765, args1.Rest.Item5);
            Assert.AreEqual(1308262114, args2.Rest.Item5);
            Assert.AreEqual(2074457149, args1.Rest.Item6);
            Assert.AreEqual(672559893, args2.Rest.Item6);
            Assert.AreEqual(46389579, args1.Rest.Item7);
            Assert.AreEqual(784201288, args2.Rest.Item7);
            Assert.AreEqual(1013832455, args1.Rest.Item8);
            Assert.AreEqual(1816864624, args2.Rest.Item8);
            Assert.AreEqual(503291986, args1.Rest.Item9);
            Assert.AreEqual(22515771, args2.Rest.Item9);
            Assert.AreEqual(1809648691, args1.Rest.Item10);
            Assert.AreEqual(1209997076, args2.Rest.Item10);
            Assert.AreEqual(1233068406, args1.Rest.Item11);
            Assert.AreEqual(170046205, args2.Rest.Item11);
            Assert.AreEqual(684001618, args1.Rest.Item12);
            Assert.AreEqual(2040445813, args2.Rest.Item12);

            Assert.AreEqual(args1, args1);

            var copy1 = args1;
            Assert.IsTrue(args1 == copy1);
            Assert.IsFalse(args1 != copy1);

            Assert.AreNotEqual(args1.GetHashCode(), args2.GetHashCode());
            Assert.AreNotEqual(args1, args2);
            Assert.AreNotEqual(args2, args1);

            var scm1 = (IStructuralComparable)args1;
            var cmp1 = (IComparable)args1;

            Assert.AreNotEqual(0, args1.CompareTo(args2));
            Assert.AreNotEqual(0, scm1.CompareTo(args2, Comparer<object>.Default));
            Assert.AreNotEqual(0, cmp1.CompareTo(args2));
            Assert.AreEqual(1, scm1.CompareTo(other: null, Comparer<object>.Default));

            Assert.ThrowsException<ArgumentException>(() => scm1.CompareTo("foo", Comparer<object>.Default));

            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419316, 608730818, 102925244, 139818672, 932727292, 1960081966, 1426017329, 61851007, 498546003, 1329582599, 486776634, 596016098, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419314, 608730818, 102925244, 139818672, 932727292, 1960081966, 1426017329, 61851007, 498546003, 1329582599, 486776634, 596016098, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419316, 608730818, 102925244, 139818672, 932727292, 1960081966, 1426017329, 61851007, 498546003, 1329582599, 486776634, 596016098, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419314, 608730818, 102925244, 139818672, 932727292, 1960081966, 1426017329, 61851007, 498546003, 1329582599, 486776634, 596016098, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730819, 102925244, 139818672, 932727292, 1960081966, 1426017329, 61851007, 498546003, 1329582599, 486776634, 596016098, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730817, 102925244, 139818672, 932727292, 1960081966, 1426017329, 61851007, 498546003, 1329582599, 486776634, 596016098, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730819, 102925244, 139818672, 932727292, 1960081966, 1426017329, 61851007, 498546003, 1329582599, 486776634, 596016098, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730817, 102925244, 139818672, 932727292, 1960081966, 1426017329, 61851007, 498546003, 1329582599, 486776634, 596016098, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730818, 102925245, 139818672, 932727292, 1960081966, 1426017329, 61851007, 498546003, 1329582599, 486776634, 596016098, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730818, 102925243, 139818672, 932727292, 1960081966, 1426017329, 61851007, 498546003, 1329582599, 486776634, 596016098, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730818, 102925245, 139818672, 932727292, 1960081966, 1426017329, 61851007, 498546003, 1329582599, 486776634, 596016098, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730818, 102925243, 139818672, 932727292, 1960081966, 1426017329, 61851007, 498546003, 1329582599, 486776634, 596016098, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730818, 102925244, 139818673, 932727292, 1960081966, 1426017329, 61851007, 498546003, 1329582599, 486776634, 596016098, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730818, 102925244, 139818671, 932727292, 1960081966, 1426017329, 61851007, 498546003, 1329582599, 486776634, 596016098, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730818, 102925244, 139818673, 932727292, 1960081966, 1426017329, 61851007, 498546003, 1329582599, 486776634, 596016098, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730818, 102925244, 139818671, 932727292, 1960081966, 1426017329, 61851007, 498546003, 1329582599, 486776634, 596016098, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730818, 102925244, 139818672, 932727293, 1960081966, 1426017329, 61851007, 498546003, 1329582599, 486776634, 596016098, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730818, 102925244, 139818672, 932727291, 1960081966, 1426017329, 61851007, 498546003, 1329582599, 486776634, 596016098, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730818, 102925244, 139818672, 932727293, 1960081966, 1426017329, 61851007, 498546003, 1329582599, 486776634, 596016098, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730818, 102925244, 139818672, 932727291, 1960081966, 1426017329, 61851007, 498546003, 1329582599, 486776634, 596016098, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730818, 102925244, 139818672, 932727292, 1960081967, 1426017329, 61851007, 498546003, 1329582599, 486776634, 596016098, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730818, 102925244, 139818672, 932727292, 1960081965, 1426017329, 61851007, 498546003, 1329582599, 486776634, 596016098, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730818, 102925244, 139818672, 932727292, 1960081967, 1426017329, 61851007, 498546003, 1329582599, 486776634, 596016098, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730818, 102925244, 139818672, 932727292, 1960081965, 1426017329, 61851007, 498546003, 1329582599, 486776634, 596016098, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730818, 102925244, 139818672, 932727292, 1960081966, 1426017330, 61851007, 498546003, 1329582599, 486776634, 596016098, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730818, 102925244, 139818672, 932727292, 1960081966, 1426017328, 61851007, 498546003, 1329582599, 486776634, 596016098, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730818, 102925244, 139818672, 932727292, 1960081966, 1426017330, 61851007, 498546003, 1329582599, 486776634, 596016098, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730818, 102925244, 139818672, 932727292, 1960081966, 1426017328, 61851007, 498546003, 1329582599, 486776634, 596016098, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730818, 102925244, 139818672, 932727292, 1960081966, 1426017329, 61851008, 498546003, 1329582599, 486776634, 596016098, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730818, 102925244, 139818672, 932727292, 1960081966, 1426017329, 61851006, 498546003, 1329582599, 486776634, 596016098, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730818, 102925244, 139818672, 932727292, 1960081966, 1426017329, 61851008, 498546003, 1329582599, 486776634, 596016098, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730818, 102925244, 139818672, 932727292, 1960081966, 1426017329, 61851006, 498546003, 1329582599, 486776634, 596016098, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730818, 102925244, 139818672, 932727292, 1960081966, 1426017329, 61851007, 498546004, 1329582599, 486776634, 596016098, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730818, 102925244, 139818672, 932727292, 1960081966, 1426017329, 61851007, 498546002, 1329582599, 486776634, 596016098, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730818, 102925244, 139818672, 932727292, 1960081966, 1426017329, 61851007, 498546004, 1329582599, 486776634, 596016098, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730818, 102925244, 139818672, 932727292, 1960081966, 1426017329, 61851007, 498546002, 1329582599, 486776634, 596016098, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730818, 102925244, 139818672, 932727292, 1960081966, 1426017329, 61851007, 498546003, 1329582600, 486776634, 596016098, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730818, 102925244, 139818672, 932727292, 1960081966, 1426017329, 61851007, 498546003, 1329582598, 486776634, 596016098, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730818, 102925244, 139818672, 932727292, 1960081966, 1426017329, 61851007, 498546003, 1329582600, 486776634, 596016098, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730818, 102925244, 139818672, 932727292, 1960081966, 1426017329, 61851007, 498546003, 1329582598, 486776634, 596016098, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730818, 102925244, 139818672, 932727292, 1960081966, 1426017329, 61851007, 498546003, 1329582599, 486776635, 596016098, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730818, 102925244, 139818672, 932727292, 1960081966, 1426017329, 61851007, 498546003, 1329582599, 486776633, 596016098, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730818, 102925244, 139818672, 932727292, 1960081966, 1426017329, 61851007, 498546003, 1329582599, 486776635, 596016098, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730818, 102925244, 139818672, 932727292, 1960081966, 1426017329, 61851007, 498546003, 1329582599, 486776633, 596016098, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730818, 102925244, 139818672, 932727292, 1960081966, 1426017329, 61851007, 498546003, 1329582599, 486776634, 596016099, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730818, 102925244, 139818672, 932727292, 1960081966, 1426017329, 61851007, 498546003, 1329582599, 486776634, 596016097, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730818, 102925244, 139818672, 932727292, 1960081966, 1426017329, 61851007, 498546003, 1329582599, 486776634, 596016099, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730818, 102925244, 139818672, 932727292, 1960081966, 1426017329, 61851007, 498546003, 1329582599, 486776634, 596016097, 1298872697, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730818, 102925244, 139818672, 932727292, 1960081966, 1426017329, 61851007, 498546003, 1329582599, 486776634, 596016098, 1298872698, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730818, 102925244, 139818672, 932727292, 1960081966, 1426017329, 61851007, 498546003, 1329582599, 486776634, 596016098, 1298872696, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730818, 102925244, 139818672, 932727292, 1960081966, 1426017329, 61851007, 498546003, 1329582599, 486776634, 596016098, 1298872698, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>>(1967419315, 608730818, 102925244, 139818672, 932727292, 1960081966, 1426017329, 61851007, 498546003, 1329582599, 486776634, 596016098, 1298872696, 999013294, 1854060897, 1195035071, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618)), Comparer<object>.Default) > 0);

            Assert.IsFalse(args1 == args2);
            Assert.IsTrue(args1 != args2);

            Assert.AreNotEqual(args1, "foo");
            Assert.AreNotEqual(args1, null);

            var seq1 = (IStructuralEquatable)args1;
            Assert.AreEqual(args1.GetHashCode(), seq1.GetHashCode(EqualityComparer<object>.Default));
            Assert.AreEqual(0, seq1.GetHashCode(new ConstantHashEqualityComparer(0)));
            Assert.IsTrue(seq1.Equals(args1, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals(args2, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals("foo", EqualityComparer<object>.Default));

            Assert.AreEqual("(1967419315, 608730818, 102925244, 139818672, 932727292, 1960081966, 1426017329, 61851007, 498546003, 1329582599, 486776634, 596016098, 1298872697, 999013294, 1854060897, 1195035071, 967240171, 536609357, 991725449, 1221052373, 326712765, 2074457149, 46389579, 1013832455, 503291986, 1809648691, 1233068406, 684001618)", args1.ToString());
            Assert.AreEqual("(159029404, 136029984, 454493725, 1653716755, 565201810, 938451540, 1588124598, 233708764, 1893385054, 1209324273, 1768635017, 1830115890, 1837424084, 466285936, 307630994, 1754090039, 1601958853, 1648920732, 31236732, 1689426032, 1308262114, 672559893, 784201288, 1816864624, 22515771, 1209997076, 170046205, 2040445813)", args2.ToString());
        }

        [TestMethod]
        public void Tuplet29()
        {
            var args1 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010333, 1270562248, 727013560, 742015711, 2050305250, 362516019, 875088874, 980543526, 30814288, 360421157, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343));
            var args2 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(785112748, 1631200984, 1502688381, 768405335, 1695162432, 1818851172, 656808618, 1572797587, 498072769, 1103102330, 1095408373, 563464333, 2092598622, 879001165, 621415327, 1618106444, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(1818299222, 131053689, 1835393628, 1370909876, 1026285390, 901406637, 860906212, 1826368787, 309619376, 273619658, 122558052, 1824702834, 782155432));

            Assert.AreEqual(562341239, args1.Item1);
            Assert.AreEqual(785112748, args2.Item1);
            Assert.AreEqual(1236576436, args1.Item2);
            Assert.AreEqual(1631200984, args2.Item2);
            Assert.AreEqual(1784010333, args1.Item3);
            Assert.AreEqual(1502688381, args2.Item3);
            Assert.AreEqual(1270562248, args1.Item4);
            Assert.AreEqual(768405335, args2.Item4);
            Assert.AreEqual(727013560, args1.Item5);
            Assert.AreEqual(1695162432, args2.Item5);
            Assert.AreEqual(742015711, args1.Item6);
            Assert.AreEqual(1818851172, args2.Item6);
            Assert.AreEqual(2050305250, args1.Item7);
            Assert.AreEqual(656808618, args2.Item7);
            Assert.AreEqual(362516019, args1.Item8);
            Assert.AreEqual(1572797587, args2.Item8);
            Assert.AreEqual(875088874, args1.Item9);
            Assert.AreEqual(498072769, args2.Item9);
            Assert.AreEqual(980543526, args1.Item10);
            Assert.AreEqual(1103102330, args2.Item10);
            Assert.AreEqual(30814288, args1.Item11);
            Assert.AreEqual(1095408373, args2.Item11);
            Assert.AreEqual(360421157, args1.Item12);
            Assert.AreEqual(563464333, args2.Item12);
            Assert.AreEqual(1558372343, args1.Item13);
            Assert.AreEqual(2092598622, args2.Item13);
            Assert.AreEqual(1620352133, args1.Item14);
            Assert.AreEqual(879001165, args2.Item14);
            Assert.AreEqual(1449133664, args1.Item15);
            Assert.AreEqual(621415327, args2.Item15);
            Assert.AreEqual(1905399545, args1.Item16);
            Assert.AreEqual(1618106444, args2.Item16);
            Assert.AreEqual(915457987, args1.Rest.Item1);
            Assert.AreEqual(1818299222, args2.Rest.Item1);
            Assert.AreEqual(1309093206, args1.Rest.Item2);
            Assert.AreEqual(131053689, args2.Rest.Item2);
            Assert.AreEqual(1531111936, args1.Rest.Item3);
            Assert.AreEqual(1835393628, args2.Rest.Item3);
            Assert.AreEqual(2007910476, args1.Rest.Item4);
            Assert.AreEqual(1370909876, args2.Rest.Item4);
            Assert.AreEqual(1766826155, args1.Rest.Item5);
            Assert.AreEqual(1026285390, args2.Rest.Item5);
            Assert.AreEqual(439783187, args1.Rest.Item6);
            Assert.AreEqual(901406637, args2.Rest.Item6);
            Assert.AreEqual(1559357249, args1.Rest.Item7);
            Assert.AreEqual(860906212, args2.Rest.Item7);
            Assert.AreEqual(1001854901, args1.Rest.Item8);
            Assert.AreEqual(1826368787, args2.Rest.Item8);
            Assert.AreEqual(1778411959, args1.Rest.Item9);
            Assert.AreEqual(309619376, args2.Rest.Item9);
            Assert.AreEqual(1691126021, args1.Rest.Item10);
            Assert.AreEqual(273619658, args2.Rest.Item10);
            Assert.AreEqual(1523223151, args1.Rest.Item11);
            Assert.AreEqual(122558052, args2.Rest.Item11);
            Assert.AreEqual(1633953158, args1.Rest.Item12);
            Assert.AreEqual(1824702834, args2.Rest.Item12);
            Assert.AreEqual(1499312343, args1.Rest.Item13);
            Assert.AreEqual(782155432, args2.Rest.Item13);

            Assert.AreEqual(args1, args1);

            var copy1 = args1;
            Assert.IsTrue(args1 == copy1);
            Assert.IsFalse(args1 != copy1);

            Assert.AreNotEqual(args1.GetHashCode(), args2.GetHashCode());
            Assert.AreNotEqual(args1, args2);
            Assert.AreNotEqual(args2, args1);

            var scm1 = (IStructuralComparable)args1;
            var cmp1 = (IComparable)args1;

            Assert.AreNotEqual(0, args1.CompareTo(args2));
            Assert.AreNotEqual(0, scm1.CompareTo(args2, Comparer<object>.Default));
            Assert.AreNotEqual(0, cmp1.CompareTo(args2));
            Assert.AreEqual(1, scm1.CompareTo(other: null, Comparer<object>.Default));

            Assert.ThrowsException<ArgumentException>(() => scm1.CompareTo("foo", Comparer<object>.Default));

            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341240, 1236576436, 1784010333, 1270562248, 727013560, 742015711, 2050305250, 362516019, 875088874, 980543526, 30814288, 360421157, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341238, 1236576436, 1784010333, 1270562248, 727013560, 742015711, 2050305250, 362516019, 875088874, 980543526, 30814288, 360421157, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341240, 1236576436, 1784010333, 1270562248, 727013560, 742015711, 2050305250, 362516019, 875088874, 980543526, 30814288, 360421157, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341238, 1236576436, 1784010333, 1270562248, 727013560, 742015711, 2050305250, 362516019, 875088874, 980543526, 30814288, 360421157, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576437, 1784010333, 1270562248, 727013560, 742015711, 2050305250, 362516019, 875088874, 980543526, 30814288, 360421157, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576435, 1784010333, 1270562248, 727013560, 742015711, 2050305250, 362516019, 875088874, 980543526, 30814288, 360421157, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576437, 1784010333, 1270562248, 727013560, 742015711, 2050305250, 362516019, 875088874, 980543526, 30814288, 360421157, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576435, 1784010333, 1270562248, 727013560, 742015711, 2050305250, 362516019, 875088874, 980543526, 30814288, 360421157, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010334, 1270562248, 727013560, 742015711, 2050305250, 362516019, 875088874, 980543526, 30814288, 360421157, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010332, 1270562248, 727013560, 742015711, 2050305250, 362516019, 875088874, 980543526, 30814288, 360421157, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010334, 1270562248, 727013560, 742015711, 2050305250, 362516019, 875088874, 980543526, 30814288, 360421157, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010332, 1270562248, 727013560, 742015711, 2050305250, 362516019, 875088874, 980543526, 30814288, 360421157, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010333, 1270562249, 727013560, 742015711, 2050305250, 362516019, 875088874, 980543526, 30814288, 360421157, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010333, 1270562247, 727013560, 742015711, 2050305250, 362516019, 875088874, 980543526, 30814288, 360421157, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010333, 1270562249, 727013560, 742015711, 2050305250, 362516019, 875088874, 980543526, 30814288, 360421157, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010333, 1270562247, 727013560, 742015711, 2050305250, 362516019, 875088874, 980543526, 30814288, 360421157, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010333, 1270562248, 727013561, 742015711, 2050305250, 362516019, 875088874, 980543526, 30814288, 360421157, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010333, 1270562248, 727013559, 742015711, 2050305250, 362516019, 875088874, 980543526, 30814288, 360421157, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010333, 1270562248, 727013561, 742015711, 2050305250, 362516019, 875088874, 980543526, 30814288, 360421157, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010333, 1270562248, 727013559, 742015711, 2050305250, 362516019, 875088874, 980543526, 30814288, 360421157, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010333, 1270562248, 727013560, 742015712, 2050305250, 362516019, 875088874, 980543526, 30814288, 360421157, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010333, 1270562248, 727013560, 742015710, 2050305250, 362516019, 875088874, 980543526, 30814288, 360421157, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010333, 1270562248, 727013560, 742015712, 2050305250, 362516019, 875088874, 980543526, 30814288, 360421157, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010333, 1270562248, 727013560, 742015710, 2050305250, 362516019, 875088874, 980543526, 30814288, 360421157, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010333, 1270562248, 727013560, 742015711, 2050305251, 362516019, 875088874, 980543526, 30814288, 360421157, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010333, 1270562248, 727013560, 742015711, 2050305249, 362516019, 875088874, 980543526, 30814288, 360421157, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010333, 1270562248, 727013560, 742015711, 2050305251, 362516019, 875088874, 980543526, 30814288, 360421157, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010333, 1270562248, 727013560, 742015711, 2050305249, 362516019, 875088874, 980543526, 30814288, 360421157, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010333, 1270562248, 727013560, 742015711, 2050305250, 362516020, 875088874, 980543526, 30814288, 360421157, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010333, 1270562248, 727013560, 742015711, 2050305250, 362516018, 875088874, 980543526, 30814288, 360421157, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010333, 1270562248, 727013560, 742015711, 2050305250, 362516020, 875088874, 980543526, 30814288, 360421157, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010333, 1270562248, 727013560, 742015711, 2050305250, 362516018, 875088874, 980543526, 30814288, 360421157, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010333, 1270562248, 727013560, 742015711, 2050305250, 362516019, 875088875, 980543526, 30814288, 360421157, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010333, 1270562248, 727013560, 742015711, 2050305250, 362516019, 875088873, 980543526, 30814288, 360421157, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010333, 1270562248, 727013560, 742015711, 2050305250, 362516019, 875088875, 980543526, 30814288, 360421157, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010333, 1270562248, 727013560, 742015711, 2050305250, 362516019, 875088873, 980543526, 30814288, 360421157, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010333, 1270562248, 727013560, 742015711, 2050305250, 362516019, 875088874, 980543527, 30814288, 360421157, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010333, 1270562248, 727013560, 742015711, 2050305250, 362516019, 875088874, 980543525, 30814288, 360421157, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010333, 1270562248, 727013560, 742015711, 2050305250, 362516019, 875088874, 980543527, 30814288, 360421157, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010333, 1270562248, 727013560, 742015711, 2050305250, 362516019, 875088874, 980543525, 30814288, 360421157, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010333, 1270562248, 727013560, 742015711, 2050305250, 362516019, 875088874, 980543526, 30814289, 360421157, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010333, 1270562248, 727013560, 742015711, 2050305250, 362516019, 875088874, 980543526, 30814287, 360421157, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010333, 1270562248, 727013560, 742015711, 2050305250, 362516019, 875088874, 980543526, 30814289, 360421157, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010333, 1270562248, 727013560, 742015711, 2050305250, 362516019, 875088874, 980543526, 30814287, 360421157, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010333, 1270562248, 727013560, 742015711, 2050305250, 362516019, 875088874, 980543526, 30814288, 360421158, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010333, 1270562248, 727013560, 742015711, 2050305250, 362516019, 875088874, 980543526, 30814288, 360421156, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010333, 1270562248, 727013560, 742015711, 2050305250, 362516019, 875088874, 980543526, 30814288, 360421158, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010333, 1270562248, 727013560, 742015711, 2050305250, 362516019, 875088874, 980543526, 30814288, 360421156, 1558372343, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010333, 1270562248, 727013560, 742015711, 2050305250, 362516019, 875088874, 980543526, 30814288, 360421157, 1558372344, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010333, 1270562248, 727013560, 742015711, 2050305250, 362516019, 875088874, 980543526, 30814288, 360421157, 1558372342, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010333, 1270562248, 727013560, 742015711, 2050305250, 362516019, 875088874, 980543526, 30814288, 360421157, 1558372344, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010333, 1270562248, 727013560, 742015711, 2050305250, 362516019, 875088874, 980543526, 30814288, 360421157, 1558372342, 1620352133, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010333, 1270562248, 727013560, 742015711, 2050305250, 362516019, 875088874, 980543526, 30814288, 360421157, 1558372343, 1620352134, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010333, 1270562248, 727013560, 742015711, 2050305250, 362516019, 875088874, 980543526, 30814288, 360421157, 1558372343, 1620352132, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010333, 1270562248, 727013560, 742015711, 2050305250, 362516019, 875088874, 980543526, 30814288, 360421157, 1558372343, 1620352134, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>>(562341239, 1236576436, 1784010333, 1270562248, 727013560, 742015711, 2050305250, 362516019, 875088874, 980543526, 30814288, 360421157, 1558372343, 1620352132, 1449133664, 1905399545, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343)), Comparer<object>.Default) > 0);

            Assert.IsFalse(args1 == args2);
            Assert.IsTrue(args1 != args2);

            Assert.AreNotEqual(args1, "foo");
            Assert.AreNotEqual(args1, null);

            var seq1 = (IStructuralEquatable)args1;
            Assert.AreEqual(args1.GetHashCode(), seq1.GetHashCode(EqualityComparer<object>.Default));
            Assert.AreEqual(0, seq1.GetHashCode(new ConstantHashEqualityComparer(0)));
            Assert.IsTrue(seq1.Equals(args1, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals(args2, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals("foo", EqualityComparer<object>.Default));

            Assert.AreEqual("(562341239, 1236576436, 1784010333, 1270562248, 727013560, 742015711, 2050305250, 362516019, 875088874, 980543526, 30814288, 360421157, 1558372343, 1620352133, 1449133664, 1905399545, 915457987, 1309093206, 1531111936, 2007910476, 1766826155, 439783187, 1559357249, 1001854901, 1778411959, 1691126021, 1523223151, 1633953158, 1499312343)", args1.ToString());
            Assert.AreEqual("(785112748, 1631200984, 1502688381, 768405335, 1695162432, 1818851172, 656808618, 1572797587, 498072769, 1103102330, 1095408373, 563464333, 2092598622, 879001165, 621415327, 1618106444, 1818299222, 131053689, 1835393628, 1370909876, 1026285390, 901406637, 860906212, 1826368787, 309619376, 273619658, 122558052, 1824702834, 782155432)", args2.ToString());
        }

        [TestMethod]
        public void Tuplet30()
        {
            var args1 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352092, 1010687323, 89976126, 1496826189, 675609554, 1739499469, 2010693558, 1948984608, 792325046, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959));
            var args2 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1421542774, 1696293120, 979589431, 790642155, 1005922480, 2067214791, 1876615813, 147112241, 1081911299, 789025039, 1272072785, 942496890, 78799753, 267843778, 2033892667, 578584830, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(693683432, 484021419, 654915336, 1390665224, 1012656880, 599392125, 561776030, 886760912, 1398406975, 1679521199, 1770782501, 1678446684, 153308810, 537660370));

            Assert.AreEqual(1639633936, args1.Item1);
            Assert.AreEqual(1421542774, args2.Item1);
            Assert.AreEqual(1183371186, args1.Item2);
            Assert.AreEqual(1696293120, args2.Item2);
            Assert.AreEqual(1366276207, args1.Item3);
            Assert.AreEqual(979589431, args2.Item3);
            Assert.AreEqual(416352092, args1.Item4);
            Assert.AreEqual(790642155, args2.Item4);
            Assert.AreEqual(1010687323, args1.Item5);
            Assert.AreEqual(1005922480, args2.Item5);
            Assert.AreEqual(89976126, args1.Item6);
            Assert.AreEqual(2067214791, args2.Item6);
            Assert.AreEqual(1496826189, args1.Item7);
            Assert.AreEqual(1876615813, args2.Item7);
            Assert.AreEqual(675609554, args1.Item8);
            Assert.AreEqual(147112241, args2.Item8);
            Assert.AreEqual(1739499469, args1.Item9);
            Assert.AreEqual(1081911299, args2.Item9);
            Assert.AreEqual(2010693558, args1.Item10);
            Assert.AreEqual(789025039, args2.Item10);
            Assert.AreEqual(1948984608, args1.Item11);
            Assert.AreEqual(1272072785, args2.Item11);
            Assert.AreEqual(792325046, args1.Item12);
            Assert.AreEqual(942496890, args2.Item12);
            Assert.AreEqual(332601958, args1.Item13);
            Assert.AreEqual(78799753, args2.Item13);
            Assert.AreEqual(417385218, args1.Item14);
            Assert.AreEqual(267843778, args2.Item14);
            Assert.AreEqual(205990876, args1.Item15);
            Assert.AreEqual(2033892667, args2.Item15);
            Assert.AreEqual(435703563, args1.Item16);
            Assert.AreEqual(578584830, args2.Item16);
            Assert.AreEqual(1444446143, args1.Rest.Item1);
            Assert.AreEqual(693683432, args2.Rest.Item1);
            Assert.AreEqual(1821711180, args1.Rest.Item2);
            Assert.AreEqual(484021419, args2.Rest.Item2);
            Assert.AreEqual(1708265669, args1.Rest.Item3);
            Assert.AreEqual(654915336, args2.Rest.Item3);
            Assert.AreEqual(937941922, args1.Rest.Item4);
            Assert.AreEqual(1390665224, args2.Rest.Item4);
            Assert.AreEqual(1531232104, args1.Rest.Item5);
            Assert.AreEqual(1012656880, args2.Rest.Item5);
            Assert.AreEqual(2107596384, args1.Rest.Item6);
            Assert.AreEqual(599392125, args2.Rest.Item6);
            Assert.AreEqual(1560072332, args1.Rest.Item7);
            Assert.AreEqual(561776030, args2.Rest.Item7);
            Assert.AreEqual(1835313170, args1.Rest.Item8);
            Assert.AreEqual(886760912, args2.Rest.Item8);
            Assert.AreEqual(263043282, args1.Rest.Item9);
            Assert.AreEqual(1398406975, args2.Rest.Item9);
            Assert.AreEqual(473026953, args1.Rest.Item10);
            Assert.AreEqual(1679521199, args2.Rest.Item10);
            Assert.AreEqual(2031189758, args1.Rest.Item11);
            Assert.AreEqual(1770782501, args2.Rest.Item11);
            Assert.AreEqual(770294772, args1.Rest.Item12);
            Assert.AreEqual(1678446684, args2.Rest.Item12);
            Assert.AreEqual(1823803241, args1.Rest.Item13);
            Assert.AreEqual(153308810, args2.Rest.Item13);
            Assert.AreEqual(458785959, args1.Rest.Item14);
            Assert.AreEqual(537660370, args2.Rest.Item14);

            Assert.AreEqual(args1, args1);

            var copy1 = args1;
            Assert.IsTrue(args1 == copy1);
            Assert.IsFalse(args1 != copy1);

            Assert.AreNotEqual(args1.GetHashCode(), args2.GetHashCode());
            Assert.AreNotEqual(args1, args2);
            Assert.AreNotEqual(args2, args1);

            var scm1 = (IStructuralComparable)args1;
            var cmp1 = (IComparable)args1;

            Assert.AreNotEqual(0, args1.CompareTo(args2));
            Assert.AreNotEqual(0, scm1.CompareTo(args2, Comparer<object>.Default));
            Assert.AreNotEqual(0, cmp1.CompareTo(args2));
            Assert.AreEqual(1, scm1.CompareTo(other: null, Comparer<object>.Default));

            Assert.ThrowsException<ArgumentException>(() => scm1.CompareTo("foo", Comparer<object>.Default));

            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633937, 1183371186, 1366276207, 416352092, 1010687323, 89976126, 1496826189, 675609554, 1739499469, 2010693558, 1948984608, 792325046, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633935, 1183371186, 1366276207, 416352092, 1010687323, 89976126, 1496826189, 675609554, 1739499469, 2010693558, 1948984608, 792325046, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633937, 1183371186, 1366276207, 416352092, 1010687323, 89976126, 1496826189, 675609554, 1739499469, 2010693558, 1948984608, 792325046, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633935, 1183371186, 1366276207, 416352092, 1010687323, 89976126, 1496826189, 675609554, 1739499469, 2010693558, 1948984608, 792325046, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371187, 1366276207, 416352092, 1010687323, 89976126, 1496826189, 675609554, 1739499469, 2010693558, 1948984608, 792325046, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371185, 1366276207, 416352092, 1010687323, 89976126, 1496826189, 675609554, 1739499469, 2010693558, 1948984608, 792325046, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371187, 1366276207, 416352092, 1010687323, 89976126, 1496826189, 675609554, 1739499469, 2010693558, 1948984608, 792325046, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371185, 1366276207, 416352092, 1010687323, 89976126, 1496826189, 675609554, 1739499469, 2010693558, 1948984608, 792325046, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276208, 416352092, 1010687323, 89976126, 1496826189, 675609554, 1739499469, 2010693558, 1948984608, 792325046, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276206, 416352092, 1010687323, 89976126, 1496826189, 675609554, 1739499469, 2010693558, 1948984608, 792325046, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276208, 416352092, 1010687323, 89976126, 1496826189, 675609554, 1739499469, 2010693558, 1948984608, 792325046, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276206, 416352092, 1010687323, 89976126, 1496826189, 675609554, 1739499469, 2010693558, 1948984608, 792325046, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352093, 1010687323, 89976126, 1496826189, 675609554, 1739499469, 2010693558, 1948984608, 792325046, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352091, 1010687323, 89976126, 1496826189, 675609554, 1739499469, 2010693558, 1948984608, 792325046, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352093, 1010687323, 89976126, 1496826189, 675609554, 1739499469, 2010693558, 1948984608, 792325046, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352091, 1010687323, 89976126, 1496826189, 675609554, 1739499469, 2010693558, 1948984608, 792325046, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352092, 1010687324, 89976126, 1496826189, 675609554, 1739499469, 2010693558, 1948984608, 792325046, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352092, 1010687322, 89976126, 1496826189, 675609554, 1739499469, 2010693558, 1948984608, 792325046, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352092, 1010687324, 89976126, 1496826189, 675609554, 1739499469, 2010693558, 1948984608, 792325046, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352092, 1010687322, 89976126, 1496826189, 675609554, 1739499469, 2010693558, 1948984608, 792325046, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352092, 1010687323, 89976127, 1496826189, 675609554, 1739499469, 2010693558, 1948984608, 792325046, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352092, 1010687323, 89976125, 1496826189, 675609554, 1739499469, 2010693558, 1948984608, 792325046, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352092, 1010687323, 89976127, 1496826189, 675609554, 1739499469, 2010693558, 1948984608, 792325046, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352092, 1010687323, 89976125, 1496826189, 675609554, 1739499469, 2010693558, 1948984608, 792325046, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352092, 1010687323, 89976126, 1496826190, 675609554, 1739499469, 2010693558, 1948984608, 792325046, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352092, 1010687323, 89976126, 1496826188, 675609554, 1739499469, 2010693558, 1948984608, 792325046, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352092, 1010687323, 89976126, 1496826190, 675609554, 1739499469, 2010693558, 1948984608, 792325046, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352092, 1010687323, 89976126, 1496826188, 675609554, 1739499469, 2010693558, 1948984608, 792325046, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352092, 1010687323, 89976126, 1496826189, 675609555, 1739499469, 2010693558, 1948984608, 792325046, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352092, 1010687323, 89976126, 1496826189, 675609553, 1739499469, 2010693558, 1948984608, 792325046, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352092, 1010687323, 89976126, 1496826189, 675609555, 1739499469, 2010693558, 1948984608, 792325046, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352092, 1010687323, 89976126, 1496826189, 675609553, 1739499469, 2010693558, 1948984608, 792325046, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352092, 1010687323, 89976126, 1496826189, 675609554, 1739499470, 2010693558, 1948984608, 792325046, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352092, 1010687323, 89976126, 1496826189, 675609554, 1739499468, 2010693558, 1948984608, 792325046, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352092, 1010687323, 89976126, 1496826189, 675609554, 1739499470, 2010693558, 1948984608, 792325046, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352092, 1010687323, 89976126, 1496826189, 675609554, 1739499468, 2010693558, 1948984608, 792325046, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352092, 1010687323, 89976126, 1496826189, 675609554, 1739499469, 2010693559, 1948984608, 792325046, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352092, 1010687323, 89976126, 1496826189, 675609554, 1739499469, 2010693557, 1948984608, 792325046, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352092, 1010687323, 89976126, 1496826189, 675609554, 1739499469, 2010693559, 1948984608, 792325046, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352092, 1010687323, 89976126, 1496826189, 675609554, 1739499469, 2010693557, 1948984608, 792325046, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352092, 1010687323, 89976126, 1496826189, 675609554, 1739499469, 2010693558, 1948984609, 792325046, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352092, 1010687323, 89976126, 1496826189, 675609554, 1739499469, 2010693558, 1948984607, 792325046, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352092, 1010687323, 89976126, 1496826189, 675609554, 1739499469, 2010693558, 1948984609, 792325046, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352092, 1010687323, 89976126, 1496826189, 675609554, 1739499469, 2010693558, 1948984607, 792325046, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352092, 1010687323, 89976126, 1496826189, 675609554, 1739499469, 2010693558, 1948984608, 792325047, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352092, 1010687323, 89976126, 1496826189, 675609554, 1739499469, 2010693558, 1948984608, 792325045, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352092, 1010687323, 89976126, 1496826189, 675609554, 1739499469, 2010693558, 1948984608, 792325047, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352092, 1010687323, 89976126, 1496826189, 675609554, 1739499469, 2010693558, 1948984608, 792325045, 332601958, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352092, 1010687323, 89976126, 1496826189, 675609554, 1739499469, 2010693558, 1948984608, 792325046, 332601959, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352092, 1010687323, 89976126, 1496826189, 675609554, 1739499469, 2010693558, 1948984608, 792325046, 332601957, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352092, 1010687323, 89976126, 1496826189, 675609554, 1739499469, 2010693558, 1948984608, 792325046, 332601959, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352092, 1010687323, 89976126, 1496826189, 675609554, 1739499469, 2010693558, 1948984608, 792325046, 332601957, 417385218, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352092, 1010687323, 89976126, 1496826189, 675609554, 1739499469, 2010693558, 1948984608, 792325046, 332601958, 417385219, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352092, 1010687323, 89976126, 1496826189, 675609554, 1739499469, 2010693558, 1948984608, 792325046, 332601958, 417385217, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352092, 1010687323, 89976126, 1496826189, 675609554, 1739499469, 2010693558, 1948984608, 792325046, 332601958, 417385219, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352092, 1010687323, 89976126, 1496826189, 675609554, 1739499469, 2010693558, 1948984608, 792325046, 332601958, 417385217, 205990876, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352092, 1010687323, 89976126, 1496826189, 675609554, 1739499469, 2010693558, 1948984608, 792325046, 332601958, 417385218, 205990877, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352092, 1010687323, 89976126, 1496826189, 675609554, 1739499469, 2010693558, 1948984608, 792325046, 332601958, 417385218, 205990875, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352092, 1010687323, 89976126, 1496826189, 675609554, 1739499469, 2010693558, 1948984608, 792325046, 332601958, 417385218, 205990877, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(1639633936, 1183371186, 1366276207, 416352092, 1010687323, 89976126, 1496826189, 675609554, 1739499469, 2010693558, 1948984608, 792325046, 332601958, 417385218, 205990875, 435703563, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959)), Comparer<object>.Default) > 0);

            Assert.IsFalse(args1 == args2);
            Assert.IsTrue(args1 != args2);

            Assert.AreNotEqual(args1, "foo");
            Assert.AreNotEqual(args1, null);

            var seq1 = (IStructuralEquatable)args1;
            Assert.AreEqual(args1.GetHashCode(), seq1.GetHashCode(EqualityComparer<object>.Default));
            Assert.AreEqual(0, seq1.GetHashCode(new ConstantHashEqualityComparer(0)));
            Assert.IsTrue(seq1.Equals(args1, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals(args2, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals("foo", EqualityComparer<object>.Default));

            Assert.AreEqual("(1639633936, 1183371186, 1366276207, 416352092, 1010687323, 89976126, 1496826189, 675609554, 1739499469, 2010693558, 1948984608, 792325046, 332601958, 417385218, 205990876, 435703563, 1444446143, 1821711180, 1708265669, 937941922, 1531232104, 2107596384, 1560072332, 1835313170, 263043282, 473026953, 2031189758, 770294772, 1823803241, 458785959)", args1.ToString());
            Assert.AreEqual("(1421542774, 1696293120, 979589431, 790642155, 1005922480, 2067214791, 1876615813, 147112241, 1081911299, 789025039, 1272072785, 942496890, 78799753, 267843778, 2033892667, 578584830, 693683432, 484021419, 654915336, 1390665224, 1012656880, 599392125, 561776030, 886760912, 1398406975, 1679521199, 1770782501, 1678446684, 153308810, 537660370)", args2.ToString());
        }

        [TestMethod]
        public void Tuplet31()
        {
            var args1 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150784, 252691488, 1960219262, 1689443450, 1558946385, 286259732, 706571397, 1297333902, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290));
            var args2 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(198169129, 2141287078, 544250929, 582755024, 545541368, 2090690577, 945569890, 1826176641, 1781201179, 765849215, 1151723629, 1072558681, 368655604, 684093827, 1862806625, 2007075891, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1790019047, 1220891775, 809671761, 1798208215, 478553947, 1877026181, 468850358, 758316849, 806585323, 611151981, 566151943, 1834584431, 1914483782, 1265658045, 1867390743));

            Assert.AreEqual(206270015, args1.Item1);
            Assert.AreEqual(198169129, args2.Item1);
            Assert.AreEqual(726531417, args1.Item2);
            Assert.AreEqual(2141287078, args2.Item2);
            Assert.AreEqual(999289960, args1.Item3);
            Assert.AreEqual(544250929, args2.Item3);
            Assert.AreEqual(1280713510, args1.Item4);
            Assert.AreEqual(582755024, args2.Item4);
            Assert.AreEqual(589150784, args1.Item5);
            Assert.AreEqual(545541368, args2.Item5);
            Assert.AreEqual(252691488, args1.Item6);
            Assert.AreEqual(2090690577, args2.Item6);
            Assert.AreEqual(1960219262, args1.Item7);
            Assert.AreEqual(945569890, args2.Item7);
            Assert.AreEqual(1689443450, args1.Item8);
            Assert.AreEqual(1826176641, args2.Item8);
            Assert.AreEqual(1558946385, args1.Item9);
            Assert.AreEqual(1781201179, args2.Item9);
            Assert.AreEqual(286259732, args1.Item10);
            Assert.AreEqual(765849215, args2.Item10);
            Assert.AreEqual(706571397, args1.Item11);
            Assert.AreEqual(1151723629, args2.Item11);
            Assert.AreEqual(1297333902, args1.Item12);
            Assert.AreEqual(1072558681, args2.Item12);
            Assert.AreEqual(739799881, args1.Item13);
            Assert.AreEqual(368655604, args2.Item13);
            Assert.AreEqual(919240630, args1.Item14);
            Assert.AreEqual(684093827, args2.Item14);
            Assert.AreEqual(1813352784, args1.Item15);
            Assert.AreEqual(1862806625, args2.Item15);
            Assert.AreEqual(588735214, args1.Item16);
            Assert.AreEqual(2007075891, args2.Item16);
            Assert.AreEqual(2028796631, args1.Rest.Item1);
            Assert.AreEqual(1790019047, args2.Rest.Item1);
            Assert.AreEqual(1292228554, args1.Rest.Item2);
            Assert.AreEqual(1220891775, args2.Rest.Item2);
            Assert.AreEqual(1948904150, args1.Rest.Item3);
            Assert.AreEqual(809671761, args2.Rest.Item3);
            Assert.AreEqual(1831942099, args1.Rest.Item4);
            Assert.AreEqual(1798208215, args2.Rest.Item4);
            Assert.AreEqual(1926827168, args1.Rest.Item5);
            Assert.AreEqual(478553947, args2.Rest.Item5);
            Assert.AreEqual(1547168339, args1.Rest.Item6);
            Assert.AreEqual(1877026181, args2.Rest.Item6);
            Assert.AreEqual(115379436, args1.Rest.Item7);
            Assert.AreEqual(468850358, args2.Rest.Item7);
            Assert.AreEqual(433138017, args1.Rest.Item8);
            Assert.AreEqual(758316849, args2.Rest.Item8);
            Assert.AreEqual(1593612726, args1.Rest.Item9);
            Assert.AreEqual(806585323, args2.Rest.Item9);
            Assert.AreEqual(822150649, args1.Rest.Item10);
            Assert.AreEqual(611151981, args2.Rest.Item10);
            Assert.AreEqual(1134517090, args1.Rest.Item11);
            Assert.AreEqual(566151943, args2.Rest.Item11);
            Assert.AreEqual(92828519, args1.Rest.Item12);
            Assert.AreEqual(1834584431, args2.Rest.Item12);
            Assert.AreEqual(1539718827, args1.Rest.Item13);
            Assert.AreEqual(1914483782, args2.Rest.Item13);
            Assert.AreEqual(1473884928, args1.Rest.Item14);
            Assert.AreEqual(1265658045, args2.Rest.Item14);
            Assert.AreEqual(296432290, args1.Rest.Item15);
            Assert.AreEqual(1867390743, args2.Rest.Item15);

            Assert.AreEqual(args1, args1);

            var copy1 = args1;
            Assert.IsTrue(args1 == copy1);
            Assert.IsFalse(args1 != copy1);

            Assert.AreNotEqual(args1.GetHashCode(), args2.GetHashCode());
            Assert.AreNotEqual(args1, args2);
            Assert.AreNotEqual(args2, args1);

            var scm1 = (IStructuralComparable)args1;
            var cmp1 = (IComparable)args1;

            Assert.AreNotEqual(0, args1.CompareTo(args2));
            Assert.AreNotEqual(0, scm1.CompareTo(args2, Comparer<object>.Default));
            Assert.AreNotEqual(0, cmp1.CompareTo(args2));
            Assert.AreEqual(1, scm1.CompareTo(other: null, Comparer<object>.Default));

            Assert.ThrowsException<ArgumentException>(() => scm1.CompareTo("foo", Comparer<object>.Default));

            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270016, 726531417, 999289960, 1280713510, 589150784, 252691488, 1960219262, 1689443450, 1558946385, 286259732, 706571397, 1297333902, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270014, 726531417, 999289960, 1280713510, 589150784, 252691488, 1960219262, 1689443450, 1558946385, 286259732, 706571397, 1297333902, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270016, 726531417, 999289960, 1280713510, 589150784, 252691488, 1960219262, 1689443450, 1558946385, 286259732, 706571397, 1297333902, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270014, 726531417, 999289960, 1280713510, 589150784, 252691488, 1960219262, 1689443450, 1558946385, 286259732, 706571397, 1297333902, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531418, 999289960, 1280713510, 589150784, 252691488, 1960219262, 1689443450, 1558946385, 286259732, 706571397, 1297333902, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531416, 999289960, 1280713510, 589150784, 252691488, 1960219262, 1689443450, 1558946385, 286259732, 706571397, 1297333902, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531418, 999289960, 1280713510, 589150784, 252691488, 1960219262, 1689443450, 1558946385, 286259732, 706571397, 1297333902, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531416, 999289960, 1280713510, 589150784, 252691488, 1960219262, 1689443450, 1558946385, 286259732, 706571397, 1297333902, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289961, 1280713510, 589150784, 252691488, 1960219262, 1689443450, 1558946385, 286259732, 706571397, 1297333902, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289959, 1280713510, 589150784, 252691488, 1960219262, 1689443450, 1558946385, 286259732, 706571397, 1297333902, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289961, 1280713510, 589150784, 252691488, 1960219262, 1689443450, 1558946385, 286259732, 706571397, 1297333902, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289959, 1280713510, 589150784, 252691488, 1960219262, 1689443450, 1558946385, 286259732, 706571397, 1297333902, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713511, 589150784, 252691488, 1960219262, 1689443450, 1558946385, 286259732, 706571397, 1297333902, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713509, 589150784, 252691488, 1960219262, 1689443450, 1558946385, 286259732, 706571397, 1297333902, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713511, 589150784, 252691488, 1960219262, 1689443450, 1558946385, 286259732, 706571397, 1297333902, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713509, 589150784, 252691488, 1960219262, 1689443450, 1558946385, 286259732, 706571397, 1297333902, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150785, 252691488, 1960219262, 1689443450, 1558946385, 286259732, 706571397, 1297333902, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150783, 252691488, 1960219262, 1689443450, 1558946385, 286259732, 706571397, 1297333902, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150785, 252691488, 1960219262, 1689443450, 1558946385, 286259732, 706571397, 1297333902, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150783, 252691488, 1960219262, 1689443450, 1558946385, 286259732, 706571397, 1297333902, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150784, 252691489, 1960219262, 1689443450, 1558946385, 286259732, 706571397, 1297333902, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150784, 252691487, 1960219262, 1689443450, 1558946385, 286259732, 706571397, 1297333902, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150784, 252691489, 1960219262, 1689443450, 1558946385, 286259732, 706571397, 1297333902, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150784, 252691487, 1960219262, 1689443450, 1558946385, 286259732, 706571397, 1297333902, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150784, 252691488, 1960219263, 1689443450, 1558946385, 286259732, 706571397, 1297333902, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150784, 252691488, 1960219261, 1689443450, 1558946385, 286259732, 706571397, 1297333902, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150784, 252691488, 1960219263, 1689443450, 1558946385, 286259732, 706571397, 1297333902, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150784, 252691488, 1960219261, 1689443450, 1558946385, 286259732, 706571397, 1297333902, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150784, 252691488, 1960219262, 1689443451, 1558946385, 286259732, 706571397, 1297333902, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150784, 252691488, 1960219262, 1689443449, 1558946385, 286259732, 706571397, 1297333902, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150784, 252691488, 1960219262, 1689443451, 1558946385, 286259732, 706571397, 1297333902, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150784, 252691488, 1960219262, 1689443449, 1558946385, 286259732, 706571397, 1297333902, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150784, 252691488, 1960219262, 1689443450, 1558946386, 286259732, 706571397, 1297333902, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150784, 252691488, 1960219262, 1689443450, 1558946384, 286259732, 706571397, 1297333902, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150784, 252691488, 1960219262, 1689443450, 1558946386, 286259732, 706571397, 1297333902, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150784, 252691488, 1960219262, 1689443450, 1558946384, 286259732, 706571397, 1297333902, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150784, 252691488, 1960219262, 1689443450, 1558946385, 286259733, 706571397, 1297333902, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150784, 252691488, 1960219262, 1689443450, 1558946385, 286259731, 706571397, 1297333902, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150784, 252691488, 1960219262, 1689443450, 1558946385, 286259733, 706571397, 1297333902, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150784, 252691488, 1960219262, 1689443450, 1558946385, 286259731, 706571397, 1297333902, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150784, 252691488, 1960219262, 1689443450, 1558946385, 286259732, 706571398, 1297333902, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150784, 252691488, 1960219262, 1689443450, 1558946385, 286259732, 706571396, 1297333902, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150784, 252691488, 1960219262, 1689443450, 1558946385, 286259732, 706571398, 1297333902, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150784, 252691488, 1960219262, 1689443450, 1558946385, 286259732, 706571396, 1297333902, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150784, 252691488, 1960219262, 1689443450, 1558946385, 286259732, 706571397, 1297333903, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150784, 252691488, 1960219262, 1689443450, 1558946385, 286259732, 706571397, 1297333901, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150784, 252691488, 1960219262, 1689443450, 1558946385, 286259732, 706571397, 1297333903, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150784, 252691488, 1960219262, 1689443450, 1558946385, 286259732, 706571397, 1297333901, 739799881, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150784, 252691488, 1960219262, 1689443450, 1558946385, 286259732, 706571397, 1297333902, 739799882, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150784, 252691488, 1960219262, 1689443450, 1558946385, 286259732, 706571397, 1297333902, 739799880, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150784, 252691488, 1960219262, 1689443450, 1558946385, 286259732, 706571397, 1297333902, 739799882, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150784, 252691488, 1960219262, 1689443450, 1558946385, 286259732, 706571397, 1297333902, 739799880, 919240630, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150784, 252691488, 1960219262, 1689443450, 1558946385, 286259732, 706571397, 1297333902, 739799881, 919240631, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150784, 252691488, 1960219262, 1689443450, 1558946385, 286259732, 706571397, 1297333902, 739799881, 919240629, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150784, 252691488, 1960219262, 1689443450, 1558946385, 286259732, 706571397, 1297333902, 739799881, 919240631, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150784, 252691488, 1960219262, 1689443450, 1558946385, 286259732, 706571397, 1297333902, 739799881, 919240629, 1813352784, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150784, 252691488, 1960219262, 1689443450, 1558946385, 286259732, 706571397, 1297333902, 739799881, 919240630, 1813352785, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150784, 252691488, 1960219262, 1689443450, 1558946385, 286259732, 706571397, 1297333902, 739799881, 919240630, 1813352783, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150784, 252691488, 1960219262, 1689443450, 1558946385, 286259732, 706571397, 1297333902, 739799881, 919240630, 1813352785, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150784, 252691488, 1960219262, 1689443450, 1558946385, 286259732, 706571397, 1297333902, 739799881, 919240630, 1813352783, 588735214, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150784, 252691488, 1960219262, 1689443450, 1558946385, 286259732, 706571397, 1297333902, 739799881, 919240630, 1813352784, 588735215, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150784, 252691488, 1960219262, 1689443450, 1558946385, 286259732, 706571397, 1297333902, 739799881, 919240630, 1813352784, 588735213, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150784, 252691488, 1960219262, 1689443450, 1558946385, 286259732, 706571397, 1297333902, 739799881, 919240630, 1813352784, 588735215, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(206270015, 726531417, 999289960, 1280713510, 589150784, 252691488, 1960219262, 1689443450, 1558946385, 286259732, 706571397, 1297333902, 739799881, 919240630, 1813352784, 588735213, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290)), Comparer<object>.Default) > 0);

            Assert.IsFalse(args1 == args2);
            Assert.IsTrue(args1 != args2);

            Assert.AreNotEqual(args1, "foo");
            Assert.AreNotEqual(args1, null);

            var seq1 = (IStructuralEquatable)args1;
            Assert.AreEqual(args1.GetHashCode(), seq1.GetHashCode(EqualityComparer<object>.Default));
            Assert.AreEqual(0, seq1.GetHashCode(new ConstantHashEqualityComparer(0)));
            Assert.IsTrue(seq1.Equals(args1, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals(args2, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals("foo", EqualityComparer<object>.Default));

            Assert.AreEqual("(206270015, 726531417, 999289960, 1280713510, 589150784, 252691488, 1960219262, 1689443450, 1558946385, 286259732, 706571397, 1297333902, 739799881, 919240630, 1813352784, 588735214, 2028796631, 1292228554, 1948904150, 1831942099, 1926827168, 1547168339, 115379436, 433138017, 1593612726, 822150649, 1134517090, 92828519, 1539718827, 1473884928, 296432290)", args1.ToString());
            Assert.AreEqual("(198169129, 2141287078, 544250929, 582755024, 545541368, 2090690577, 945569890, 1826176641, 1781201179, 765849215, 1151723629, 1072558681, 368655604, 684093827, 1862806625, 2007075891, 1790019047, 1220891775, 809671761, 1798208215, 478553947, 1877026181, 468850358, 758316849, 806585323, 611151981, 566151943, 1834584431, 1914483782, 1265658045, 1867390743)", args2.ToString());
        }

        [TestMethod]
        public void Tuplet32()
        {
            var args1 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506));
            var args2 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(515543134, 1045942119, 1001999006, 987497224, 378828162, 175691559, 559276154, 1811526939, 1453533441, 2100564006, 164143477, 714981474, 1012502034, 1709323210, 1550241121, 2130696957, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(331975665, 864428149, 817107753, 2103778330, 951925377, 1921160002, 847970352, 1567161204, 409384744, 171949099, 39775673, 1778577398, 409614872, 560315905, 1333422029, 1268473297));

            Assert.AreEqual(149724623, args1.Item1);
            Assert.AreEqual(515543134, args2.Item1);
            Assert.AreEqual(85061457, args1.Item2);
            Assert.AreEqual(1045942119, args2.Item2);
            Assert.AreEqual(2137311089, args1.Item3);
            Assert.AreEqual(1001999006, args2.Item3);
            Assert.AreEqual(508402268, args1.Item4);
            Assert.AreEqual(987497224, args2.Item4);
            Assert.AreEqual(1303530471, args1.Item5);
            Assert.AreEqual(378828162, args2.Item5);
            Assert.AreEqual(195548952, args1.Item6);
            Assert.AreEqual(175691559, args2.Item6);
            Assert.AreEqual(336485606, args1.Item7);
            Assert.AreEqual(559276154, args2.Item7);
            Assert.AreEqual(1267811416, args1.Item8);
            Assert.AreEqual(1811526939, args2.Item8);
            Assert.AreEqual(645528284, args1.Item9);
            Assert.AreEqual(1453533441, args2.Item9);
            Assert.AreEqual(1083226741, args1.Item10);
            Assert.AreEqual(2100564006, args2.Item10);
            Assert.AreEqual(1613535560, args1.Item11);
            Assert.AreEqual(164143477, args2.Item11);
            Assert.AreEqual(167702971, args1.Item12);
            Assert.AreEqual(714981474, args2.Item12);
            Assert.AreEqual(1066092884, args1.Item13);
            Assert.AreEqual(1012502034, args2.Item13);
            Assert.AreEqual(775103539, args1.Item14);
            Assert.AreEqual(1709323210, args2.Item14);
            Assert.AreEqual(474609658, args1.Item15);
            Assert.AreEqual(1550241121, args2.Item15);
            Assert.AreEqual(1894207479, args1.Item16);
            Assert.AreEqual(2130696957, args2.Item16);
            Assert.AreEqual(1896527837, args1.Rest.Item1);
            Assert.AreEqual(331975665, args2.Rest.Item1);
            Assert.AreEqual(1878289748, args1.Rest.Item2);
            Assert.AreEqual(864428149, args2.Rest.Item2);
            Assert.AreEqual(962558405, args1.Rest.Item3);
            Assert.AreEqual(817107753, args2.Rest.Item3);
            Assert.AreEqual(1491981690, args1.Rest.Item4);
            Assert.AreEqual(2103778330, args2.Rest.Item4);
            Assert.AreEqual(1019420391, args1.Rest.Item5);
            Assert.AreEqual(951925377, args2.Rest.Item5);
            Assert.AreEqual(730047066, args1.Rest.Item6);
            Assert.AreEqual(1921160002, args2.Rest.Item6);
            Assert.AreEqual(1823160360, args1.Rest.Item7);
            Assert.AreEqual(847970352, args2.Rest.Item7);
            Assert.AreEqual(1965361990, args1.Rest.Item8);
            Assert.AreEqual(1567161204, args2.Rest.Item8);
            Assert.AreEqual(468626595, args1.Rest.Item9);
            Assert.AreEqual(409384744, args2.Rest.Item9);
            Assert.AreEqual(1672436720, args1.Rest.Item10);
            Assert.AreEqual(171949099, args2.Rest.Item10);
            Assert.AreEqual(1933417727, args1.Rest.Item11);
            Assert.AreEqual(39775673, args2.Rest.Item11);
            Assert.AreEqual(1923653348, args1.Rest.Item12);
            Assert.AreEqual(1778577398, args2.Rest.Item12);
            Assert.AreEqual(2081873034, args1.Rest.Item13);
            Assert.AreEqual(409614872, args2.Rest.Item13);
            Assert.AreEqual(1524538634, args1.Rest.Item14);
            Assert.AreEqual(560315905, args2.Rest.Item14);
            Assert.AreEqual(1258469106, args1.Rest.Item15);
            Assert.AreEqual(1333422029, args2.Rest.Item15);
            Assert.AreEqual(2059176506, args1.Rest.Item16);
            Assert.AreEqual(1268473297, args2.Rest.Item16);

            Assert.AreEqual(args1, args1);

            var copy1 = args1;
            Assert.IsTrue(args1 == copy1);
            Assert.IsFalse(args1 != copy1);

            Assert.AreNotEqual(args1.GetHashCode(), args2.GetHashCode());
            Assert.AreNotEqual(args1, args2);
            Assert.AreNotEqual(args2, args1);

            var scm1 = (IStructuralComparable)args1;
            var cmp1 = (IComparable)args1;

            Assert.AreNotEqual(0, args1.CompareTo(args2));
            Assert.AreNotEqual(0, scm1.CompareTo(args2, Comparer<object>.Default));
            Assert.AreNotEqual(0, cmp1.CompareTo(args2));
            Assert.AreEqual(1, scm1.CompareTo(other: null, Comparer<object>.Default));

            Assert.ThrowsException<ArgumentException>(() => scm1.CompareTo("foo", Comparer<object>.Default));

            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724624, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724622, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724624, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724622, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061458, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061456, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061458, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061456, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311090, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311088, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311090, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311088, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402269, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402267, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402269, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402267, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530472, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530470, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530472, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530470, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548953, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548951, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548953, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548951, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485607, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485605, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485607, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485605, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811417, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811415, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811417, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811415, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528285, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528283, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528285, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528283, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226742, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226740, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226742, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226740, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535561, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535559, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535561, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535559, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702972, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702970, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702972, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702970, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092885, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092883, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092885, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092883, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103540, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103538, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103540, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103538, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609659, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609657, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609659, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609657, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207480, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207478, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207480, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207478, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506)), Comparer<object>.Default) > 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527838, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506))) < 0);
            Assert.IsTrue(args1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527836, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506))) > 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527838, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506)), Comparer<object>.Default) < 0);
            Assert.IsTrue(scm1.CompareTo(new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207479, new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1896527836, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506)), Comparer<object>.Default) > 0);

            Assert.IsFalse(args1 == args2);
            Assert.IsTrue(args1 != args2);

            Assert.AreNotEqual(args1, "foo");
            Assert.AreNotEqual(args1, null);

            var seq1 = (IStructuralEquatable)args1;
            Assert.AreEqual(args1.GetHashCode(), seq1.GetHashCode(EqualityComparer<object>.Default));
            Assert.AreEqual(0, seq1.GetHashCode(new ConstantHashEqualityComparer(0)));
            Assert.IsTrue(seq1.Equals(args1, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals(args2, EqualityComparer<object>.Default));
            Assert.IsFalse(seq1.Equals("foo", EqualityComparer<object>.Default));

            Assert.AreEqual("(149724623, 85061457, 2137311089, 508402268, 1303530471, 195548952, 336485606, 1267811416, 645528284, 1083226741, 1613535560, 167702971, 1066092884, 775103539, 474609658, 1894207479, 1896527837, 1878289748, 962558405, 1491981690, 1019420391, 730047066, 1823160360, 1965361990, 468626595, 1672436720, 1933417727, 1923653348, 2081873034, 1524538634, 1258469106, 2059176506)", args1.ToString());
            Assert.AreEqual("(515543134, 1045942119, 1001999006, 987497224, 378828162, 175691559, 559276154, 1811526939, 1453533441, 2100564006, 164143477, 714981474, 1012502034, 1709323210, 1550241121, 2130696957, 331975665, 864428149, 817107753, 2103778330, 951925377, 1921160002, 847970352, 1567161204, 409384744, 171949099, 39775673, 1778577398, 409614872, 560315905, 1333422029, 1268473297)", args2.ToString());
        }

    }
}