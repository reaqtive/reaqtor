// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Memory;
using System.Runtime.CompilerServices;

namespace Tests
{
    public partial class FunctionMemoizationExtensionsTests
    {
        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_ArgumentChecking()
        {
            var m = Memoizer.Create(MemoizationCacheFactory.Unbounded);

            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int>(default(IMemoizer), (t1) => 0));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int>(m, default(Func<int, int>)));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int>(default(IMemoizer), (t1, t2) => 0));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int>(m, default(Func<int, int, int>)));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int>(default(IMemoizer), (t1, t2, t3) => 0));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int>(m, default(Func<int, int, int, int>)));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int>(default(IMemoizer), (t1, t2, t3, t4) => 0));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int>(m, default(Func<int, int, int, int, int>)));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int>(default(IMemoizer), (t1, t2, t3, t4, t5) => 0));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int>(m, default(Func<int, int, int, int, int, int>)));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int>(default(IMemoizer), (t1, t2, t3, t4, t5, t6) => 0));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int>(m, default(Func<int, int, int, int, int, int, int>)));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int>(default(IMemoizer), (t1, t2, t3, t4, t5, t6, t7) => 0));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int>(m, default(Func<int, int, int, int, int, int, int, int>)));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int>(default(IMemoizer), (t1, t2, t3, t4, t5, t6, t7, t8) => 0));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int>(m, default(Func<int, int, int, int, int, int, int, int, int>)));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int>(default(IMemoizer), (t1, t2, t3, t4, t5, t6, t7, t8, t9) => 0));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int>(m, default(Func<int, int, int, int, int, int, int, int, int, int>)));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int>(default(IMemoizer), (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => 0));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int>(m, default(Func<int, int, int, int, int, int, int, int, int, int, int>)));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int>(default(IMemoizer), (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => 0));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int>(m, default(Func<int, int, int, int, int, int, int, int, int, int, int, int>)));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int>(default(IMemoizer), (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => 0));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int>(m, default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int>)));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(default(IMemoizer), (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => 0));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int>)));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(default(IMemoizer), (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => 0));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>)));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(default(IMemoizer), (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => 0));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>)));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(default(IMemoizer), (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => 0));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>)));
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize2()
        {
            var m = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var n = 0;

            var res = FunctionMemoizationExtensions.Memoize<int, int>(m, (t1) => { n++; return t1; });

            Assert.AreEqual(69, res.Delegate(69));
            Assert.AreEqual(1, n);

            Assert.AreEqual(66, res.Delegate(66));
            Assert.AreEqual(2, n);

            Assert.AreEqual(86, res.Delegate(86));
            Assert.AreEqual(3, n);

            Assert.AreEqual(81, res.Delegate(81));
            Assert.AreEqual(4, n);

            Assert.AreEqual(69, res.Delegate(69));
            Assert.AreEqual(4, n);

            Assert.AreEqual(66, res.Delegate(66));
            Assert.AreEqual(4, n);

            Assert.AreEqual(86, res.Delegate(86));
            Assert.AreEqual(4, n);

            Assert.AreEqual(81, res.Delegate(81));
            Assert.AreEqual(4, n);

            res.Cache.Clear();
            Assert.AreEqual(4, n);

            Assert.AreEqual(69, res.Delegate(69));
            Assert.AreEqual(5, n);

            Assert.AreEqual(66, res.Delegate(66));
            Assert.AreEqual(6, n);

            Assert.AreEqual(86, res.Delegate(86));
            Assert.AreEqual(7, n);

            Assert.AreEqual(81, res.Delegate(81));
            Assert.AreEqual(8, n);

        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize3()
        {
            var m = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var n = 0;

            var res = FunctionMemoizationExtensions.Memoize<int, int, int>(m, (t1, t2) => { n++; return t1 + t2; });

            Assert.AreEqual(99, res.Delegate(50, 49));
            Assert.AreEqual(1, n);

            Assert.AreEqual(68, res.Delegate(40, 28));
            Assert.AreEqual(2, n);

            Assert.AreEqual(80, res.Delegate(30, 50));
            Assert.AreEqual(3, n);

            Assert.AreEqual(84, res.Delegate(22, 62));
            Assert.AreEqual(4, n);

            Assert.AreEqual(99, res.Delegate(50, 49));
            Assert.AreEqual(4, n);

            Assert.AreEqual(68, res.Delegate(40, 28));
            Assert.AreEqual(4, n);

            Assert.AreEqual(80, res.Delegate(30, 50));
            Assert.AreEqual(4, n);

            Assert.AreEqual(84, res.Delegate(22, 62));
            Assert.AreEqual(4, n);

            res.Cache.Clear();
            Assert.AreEqual(4, n);

            Assert.AreEqual(99, res.Delegate(50, 49));
            Assert.AreEqual(5, n);

            Assert.AreEqual(68, res.Delegate(40, 28));
            Assert.AreEqual(6, n);

            Assert.AreEqual(80, res.Delegate(30, 50));
            Assert.AreEqual(7, n);

            Assert.AreEqual(84, res.Delegate(22, 62));
            Assert.AreEqual(8, n);

        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize4()
        {
            var m = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var n = 0;

            var res = FunctionMemoizationExtensions.Memoize<int, int, int, int>(m, (t1, t2, t3) => { n++; return t1 + t2 + t3; });

            Assert.AreEqual(99, res.Delegate(84, 13, 2));
            Assert.AreEqual(1, n);

            Assert.AreEqual(133, res.Delegate(45, 43, 45));
            Assert.AreEqual(2, n);

            Assert.AreEqual(78, res.Delegate(35, 14, 29));
            Assert.AreEqual(3, n);

            Assert.AreEqual(125, res.Delegate(35, 12, 78));
            Assert.AreEqual(4, n);

            Assert.AreEqual(99, res.Delegate(84, 13, 2));
            Assert.AreEqual(4, n);

            Assert.AreEqual(133, res.Delegate(45, 43, 45));
            Assert.AreEqual(4, n);

            Assert.AreEqual(78, res.Delegate(35, 14, 29));
            Assert.AreEqual(4, n);

            Assert.AreEqual(125, res.Delegate(35, 12, 78));
            Assert.AreEqual(4, n);

            res.Cache.Clear();
            Assert.AreEqual(4, n);

            Assert.AreEqual(99, res.Delegate(84, 13, 2));
            Assert.AreEqual(5, n);

            Assert.AreEqual(133, res.Delegate(45, 43, 45));
            Assert.AreEqual(6, n);

            Assert.AreEqual(78, res.Delegate(35, 14, 29));
            Assert.AreEqual(7, n);

            Assert.AreEqual(125, res.Delegate(35, 12, 78));
            Assert.AreEqual(8, n);

        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize5()
        {
            var m = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var n = 0;

            var res = FunctionMemoizationExtensions.Memoize<int, int, int, int, int>(m, (t1, t2, t3, t4) => { n++; return t1 + t2 + t3 + t4; });

            Assert.AreEqual(220, res.Delegate(4, 95, 84, 37));
            Assert.AreEqual(1, n);

            Assert.AreEqual(141, res.Delegate(27, 10, 26, 78));
            Assert.AreEqual(2, n);

            Assert.AreEqual(216, res.Delegate(70, 70, 6, 70));
            Assert.AreEqual(3, n);

            Assert.AreEqual(327, res.Delegate(64, 79, 93, 91));
            Assert.AreEqual(4, n);

            Assert.AreEqual(220, res.Delegate(4, 95, 84, 37));
            Assert.AreEqual(4, n);

            Assert.AreEqual(141, res.Delegate(27, 10, 26, 78));
            Assert.AreEqual(4, n);

            Assert.AreEqual(216, res.Delegate(70, 70, 6, 70));
            Assert.AreEqual(4, n);

            Assert.AreEqual(327, res.Delegate(64, 79, 93, 91));
            Assert.AreEqual(4, n);

            res.Cache.Clear();
            Assert.AreEqual(4, n);

            Assert.AreEqual(220, res.Delegate(4, 95, 84, 37));
            Assert.AreEqual(5, n);

            Assert.AreEqual(141, res.Delegate(27, 10, 26, 78));
            Assert.AreEqual(6, n);

            Assert.AreEqual(216, res.Delegate(70, 70, 6, 70));
            Assert.AreEqual(7, n);

            Assert.AreEqual(327, res.Delegate(64, 79, 93, 91));
            Assert.AreEqual(8, n);

        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize6()
        {
            var m = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var n = 0;

            var res = FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5) => { n++; return t1 + t2 + t3 + t4 + t5; });

            Assert.AreEqual(324, res.Delegate(84, 64, 68, 67, 41));
            Assert.AreEqual(1, n);

            Assert.AreEqual(279, res.Delegate(10, 99, 52, 87, 31));
            Assert.AreEqual(2, n);

            Assert.AreEqual(274, res.Delegate(79, 45, 78, 47, 25));
            Assert.AreEqual(3, n);

            Assert.AreEqual(225, res.Delegate(34, 53, 7, 77, 54));
            Assert.AreEqual(4, n);

            Assert.AreEqual(324, res.Delegate(84, 64, 68, 67, 41));
            Assert.AreEqual(4, n);

            Assert.AreEqual(279, res.Delegate(10, 99, 52, 87, 31));
            Assert.AreEqual(4, n);

            Assert.AreEqual(274, res.Delegate(79, 45, 78, 47, 25));
            Assert.AreEqual(4, n);

            Assert.AreEqual(225, res.Delegate(34, 53, 7, 77, 54));
            Assert.AreEqual(4, n);

            res.Cache.Clear();
            Assert.AreEqual(4, n);

            Assert.AreEqual(324, res.Delegate(84, 64, 68, 67, 41));
            Assert.AreEqual(5, n);

            Assert.AreEqual(279, res.Delegate(10, 99, 52, 87, 31));
            Assert.AreEqual(6, n);

            Assert.AreEqual(274, res.Delegate(79, 45, 78, 47, 25));
            Assert.AreEqual(7, n);

            Assert.AreEqual(225, res.Delegate(34, 53, 7, 77, 54));
            Assert.AreEqual(8, n);

        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize7()
        {
            var m = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var n = 0;

            var res = FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6) => { n++; return t1 + t2 + t3 + t4 + t5 + t6; });

            Assert.AreEqual(154, res.Delegate(65, 3, 0, 19, 24, 43));
            Assert.AreEqual(1, n);

            Assert.AreEqual(288, res.Delegate(91, 14, 7, 32, 80, 64));
            Assert.AreEqual(2, n);

            Assert.AreEqual(300, res.Delegate(51, 44, 29, 65, 66, 45));
            Assert.AreEqual(3, n);

            Assert.AreEqual(403, res.Delegate(36, 94, 96, 32, 49, 96));
            Assert.AreEqual(4, n);

            Assert.AreEqual(154, res.Delegate(65, 3, 0, 19, 24, 43));
            Assert.AreEqual(4, n);

            Assert.AreEqual(288, res.Delegate(91, 14, 7, 32, 80, 64));
            Assert.AreEqual(4, n);

            Assert.AreEqual(300, res.Delegate(51, 44, 29, 65, 66, 45));
            Assert.AreEqual(4, n);

            Assert.AreEqual(403, res.Delegate(36, 94, 96, 32, 49, 96));
            Assert.AreEqual(4, n);

            res.Cache.Clear();
            Assert.AreEqual(4, n);

            Assert.AreEqual(154, res.Delegate(65, 3, 0, 19, 24, 43));
            Assert.AreEqual(5, n);

            Assert.AreEqual(288, res.Delegate(91, 14, 7, 32, 80, 64));
            Assert.AreEqual(6, n);

            Assert.AreEqual(300, res.Delegate(51, 44, 29, 65, 66, 45));
            Assert.AreEqual(7, n);

            Assert.AreEqual(403, res.Delegate(36, 94, 96, 32, 49, 96));
            Assert.AreEqual(8, n);

        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize8()
        {
            var m = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var n = 0;

            var res = FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7) => { n++; return t1 + t2 + t3 + t4 + t5 + t6 + t7; });

            Assert.AreEqual(266, res.Delegate(31, 80, 0, 23, 45, 71, 16));
            Assert.AreEqual(1, n);

            Assert.AreEqual(316, res.Delegate(57, 2, 39, 26, 81, 63, 48));
            Assert.AreEqual(2, n);

            Assert.AreEqual(392, res.Delegate(43, 97, 18, 85, 45, 54, 50));
            Assert.AreEqual(3, n);

            Assert.AreEqual(294, res.Delegate(15, 93, 33, 18, 59, 68, 8));
            Assert.AreEqual(4, n);

            Assert.AreEqual(266, res.Delegate(31, 80, 0, 23, 45, 71, 16));
            Assert.AreEqual(4, n);

            Assert.AreEqual(316, res.Delegate(57, 2, 39, 26, 81, 63, 48));
            Assert.AreEqual(4, n);

            Assert.AreEqual(392, res.Delegate(43, 97, 18, 85, 45, 54, 50));
            Assert.AreEqual(4, n);

            Assert.AreEqual(294, res.Delegate(15, 93, 33, 18, 59, 68, 8));
            Assert.AreEqual(4, n);

            res.Cache.Clear();
            Assert.AreEqual(4, n);

            Assert.AreEqual(266, res.Delegate(31, 80, 0, 23, 45, 71, 16));
            Assert.AreEqual(5, n);

            Assert.AreEqual(316, res.Delegate(57, 2, 39, 26, 81, 63, 48));
            Assert.AreEqual(6, n);

            Assert.AreEqual(392, res.Delegate(43, 97, 18, 85, 45, 54, 50));
            Assert.AreEqual(7, n);

            Assert.AreEqual(294, res.Delegate(15, 93, 33, 18, 59, 68, 8));
            Assert.AreEqual(8, n);

        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize9()
        {
            var m = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var n = 0;

            var res = FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8) => { n++; return t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8; });

            Assert.AreEqual(430, res.Delegate(70, 82, 58, 32, 53, 4, 88, 43));
            Assert.AreEqual(1, n);

            Assert.AreEqual(326, res.Delegate(43, 68, 69, 35, 15, 23, 61, 12));
            Assert.AreEqual(2, n);

            Assert.AreEqual(211, res.Delegate(18, 48, 2, 17, 1, 39, 75, 11));
            Assert.AreEqual(3, n);

            Assert.AreEqual(485, res.Delegate(87, 94, 45, 16, 86, 67, 4, 86));
            Assert.AreEqual(4, n);

            Assert.AreEqual(430, res.Delegate(70, 82, 58, 32, 53, 4, 88, 43));
            Assert.AreEqual(4, n);

            Assert.AreEqual(326, res.Delegate(43, 68, 69, 35, 15, 23, 61, 12));
            Assert.AreEqual(4, n);

            Assert.AreEqual(211, res.Delegate(18, 48, 2, 17, 1, 39, 75, 11));
            Assert.AreEqual(4, n);

            Assert.AreEqual(485, res.Delegate(87, 94, 45, 16, 86, 67, 4, 86));
            Assert.AreEqual(4, n);

            res.Cache.Clear();
            Assert.AreEqual(4, n);

            Assert.AreEqual(430, res.Delegate(70, 82, 58, 32, 53, 4, 88, 43));
            Assert.AreEqual(5, n);

            Assert.AreEqual(326, res.Delegate(43, 68, 69, 35, 15, 23, 61, 12));
            Assert.AreEqual(6, n);

            Assert.AreEqual(211, res.Delegate(18, 48, 2, 17, 1, 39, 75, 11));
            Assert.AreEqual(7, n);

            Assert.AreEqual(485, res.Delegate(87, 94, 45, 16, 86, 67, 4, 86));
            Assert.AreEqual(8, n);

        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize10()
        {
            var m = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var n = 0;

            var res = FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9) => { n++; return t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9; });

            Assert.AreEqual(434, res.Delegate(3, 7, 86, 19, 80, 93, 27, 59, 60));
            Assert.AreEqual(1, n);

            Assert.AreEqual(327, res.Delegate(0, 54, 50, 16, 9, 38, 26, 53, 81));
            Assert.AreEqual(2, n);

            Assert.AreEqual(327, res.Delegate(14, 70, 56, 50, 7, 31, 7, 47, 45));
            Assert.AreEqual(3, n);

            Assert.AreEqual(503, res.Delegate(58, 58, 72, 56, 75, 63, 82, 31, 8));
            Assert.AreEqual(4, n);

            Assert.AreEqual(434, res.Delegate(3, 7, 86, 19, 80, 93, 27, 59, 60));
            Assert.AreEqual(4, n);

            Assert.AreEqual(327, res.Delegate(0, 54, 50, 16, 9, 38, 26, 53, 81));
            Assert.AreEqual(4, n);

            Assert.AreEqual(327, res.Delegate(14, 70, 56, 50, 7, 31, 7, 47, 45));
            Assert.AreEqual(4, n);

            Assert.AreEqual(503, res.Delegate(58, 58, 72, 56, 75, 63, 82, 31, 8));
            Assert.AreEqual(4, n);

            res.Cache.Clear();
            Assert.AreEqual(4, n);

            Assert.AreEqual(434, res.Delegate(3, 7, 86, 19, 80, 93, 27, 59, 60));
            Assert.AreEqual(5, n);

            Assert.AreEqual(327, res.Delegate(0, 54, 50, 16, 9, 38, 26, 53, 81));
            Assert.AreEqual(6, n);

            Assert.AreEqual(327, res.Delegate(14, 70, 56, 50, 7, 31, 7, 47, 45));
            Assert.AreEqual(7, n);

            Assert.AreEqual(503, res.Delegate(58, 58, 72, 56, 75, 63, 82, 31, 8));
            Assert.AreEqual(8, n);

        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize11()
        {
            var m = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var n = 0;

            var res = FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => { n++; return t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10; });

            Assert.AreEqual(362, res.Delegate(37, 42, 31, 25, 20, 42, 57, 0, 84, 24));
            Assert.AreEqual(1, n);

            Assert.AreEqual(481, res.Delegate(95, 77, 55, 19, 62, 5, 52, 34, 29, 53));
            Assert.AreEqual(2, n);

            Assert.AreEqual(428, res.Delegate(0, 55, 11, 33, 48, 69, 1, 88, 44, 79));
            Assert.AreEqual(3, n);

            Assert.AreEqual(527, res.Delegate(86, 33, 78, 30, 89, 11, 49, 89, 49, 13));
            Assert.AreEqual(4, n);

            Assert.AreEqual(362, res.Delegate(37, 42, 31, 25, 20, 42, 57, 0, 84, 24));
            Assert.AreEqual(4, n);

            Assert.AreEqual(481, res.Delegate(95, 77, 55, 19, 62, 5, 52, 34, 29, 53));
            Assert.AreEqual(4, n);

            Assert.AreEqual(428, res.Delegate(0, 55, 11, 33, 48, 69, 1, 88, 44, 79));
            Assert.AreEqual(4, n);

            Assert.AreEqual(527, res.Delegate(86, 33, 78, 30, 89, 11, 49, 89, 49, 13));
            Assert.AreEqual(4, n);

            res.Cache.Clear();
            Assert.AreEqual(4, n);

            Assert.AreEqual(362, res.Delegate(37, 42, 31, 25, 20, 42, 57, 0, 84, 24));
            Assert.AreEqual(5, n);

            Assert.AreEqual(481, res.Delegate(95, 77, 55, 19, 62, 5, 52, 34, 29, 53));
            Assert.AreEqual(6, n);

            Assert.AreEqual(428, res.Delegate(0, 55, 11, 33, 48, 69, 1, 88, 44, 79));
            Assert.AreEqual(7, n);

            Assert.AreEqual(527, res.Delegate(86, 33, 78, 30, 89, 11, 49, 89, 49, 13));
            Assert.AreEqual(8, n);

        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize12()
        {
            var m = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var n = 0;

            var res = FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => { n++; return t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11; });

            Assert.AreEqual(468, res.Delegate(92, 6, 46, 82, 52, 67, 3, 39, 9, 50, 22));
            Assert.AreEqual(1, n);

            Assert.AreEqual(609, res.Delegate(28, 53, 77, 7, 81, 30, 97, 76, 51, 41, 68));
            Assert.AreEqual(2, n);

            Assert.AreEqual(566, res.Delegate(56, 5, 38, 61, 99, 24, 29, 51, 56, 63, 84));
            Assert.AreEqual(3, n);

            Assert.AreEqual(667, res.Delegate(15, 60, 94, 8, 29, 81, 81, 65, 62, 79, 93));
            Assert.AreEqual(4, n);

            Assert.AreEqual(468, res.Delegate(92, 6, 46, 82, 52, 67, 3, 39, 9, 50, 22));
            Assert.AreEqual(4, n);

            Assert.AreEqual(609, res.Delegate(28, 53, 77, 7, 81, 30, 97, 76, 51, 41, 68));
            Assert.AreEqual(4, n);

            Assert.AreEqual(566, res.Delegate(56, 5, 38, 61, 99, 24, 29, 51, 56, 63, 84));
            Assert.AreEqual(4, n);

            Assert.AreEqual(667, res.Delegate(15, 60, 94, 8, 29, 81, 81, 65, 62, 79, 93));
            Assert.AreEqual(4, n);

            res.Cache.Clear();
            Assert.AreEqual(4, n);

            Assert.AreEqual(468, res.Delegate(92, 6, 46, 82, 52, 67, 3, 39, 9, 50, 22));
            Assert.AreEqual(5, n);

            Assert.AreEqual(609, res.Delegate(28, 53, 77, 7, 81, 30, 97, 76, 51, 41, 68));
            Assert.AreEqual(6, n);

            Assert.AreEqual(566, res.Delegate(56, 5, 38, 61, 99, 24, 29, 51, 56, 63, 84));
            Assert.AreEqual(7, n);

            Assert.AreEqual(667, res.Delegate(15, 60, 94, 8, 29, 81, 81, 65, 62, 79, 93));
            Assert.AreEqual(8, n);

        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize13()
        {
            var m = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var n = 0;

            var res = FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => { n++; return t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12; });

            Assert.AreEqual(559, res.Delegate(56, 57, 79, 0, 23, 7, 80, 51, 13, 98, 72, 23));
            Assert.AreEqual(1, n);

            Assert.AreEqual(671, res.Delegate(49, 41, 44, 90, 68, 78, 9, 57, 94, 59, 44, 38));
            Assert.AreEqual(2, n);

            Assert.AreEqual(657, res.Delegate(16, 13, 73, 1, 16, 95, 85, 79, 89, 62, 48, 80));
            Assert.AreEqual(3, n);

            Assert.AreEqual(602, res.Delegate(82, 98, 1, 21, 71, 4, 50, 86, 42, 36, 44, 67));
            Assert.AreEqual(4, n);

            Assert.AreEqual(559, res.Delegate(56, 57, 79, 0, 23, 7, 80, 51, 13, 98, 72, 23));
            Assert.AreEqual(4, n);

            Assert.AreEqual(671, res.Delegate(49, 41, 44, 90, 68, 78, 9, 57, 94, 59, 44, 38));
            Assert.AreEqual(4, n);

            Assert.AreEqual(657, res.Delegate(16, 13, 73, 1, 16, 95, 85, 79, 89, 62, 48, 80));
            Assert.AreEqual(4, n);

            Assert.AreEqual(602, res.Delegate(82, 98, 1, 21, 71, 4, 50, 86, 42, 36, 44, 67));
            Assert.AreEqual(4, n);

            res.Cache.Clear();
            Assert.AreEqual(4, n);

            Assert.AreEqual(559, res.Delegate(56, 57, 79, 0, 23, 7, 80, 51, 13, 98, 72, 23));
            Assert.AreEqual(5, n);

            Assert.AreEqual(671, res.Delegate(49, 41, 44, 90, 68, 78, 9, 57, 94, 59, 44, 38));
            Assert.AreEqual(6, n);

            Assert.AreEqual(657, res.Delegate(16, 13, 73, 1, 16, 95, 85, 79, 89, 62, 48, 80));
            Assert.AreEqual(7, n);

            Assert.AreEqual(602, res.Delegate(82, 98, 1, 21, 71, 4, 50, 86, 42, 36, 44, 67));
            Assert.AreEqual(8, n);

        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize14()
        {
            var m = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var n = 0;

            var res = FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => { n++; return t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 + t13; });

            Assert.AreEqual(722, res.Delegate(84, 91, 12, 87, 52, 21, 99, 97, 13, 40, 83, 9, 34));
            Assert.AreEqual(1, n);

            Assert.AreEqual(751, res.Delegate(78, 35, 17, 13, 92, 34, 87, 92, 64, 7, 69, 76, 87));
            Assert.AreEqual(2, n);

            Assert.AreEqual(736, res.Delegate(85, 90, 9, 57, 95, 80, 68, 6, 16, 24, 82, 97, 27));
            Assert.AreEqual(3, n);

            Assert.AreEqual(795, res.Delegate(67, 63, 51, 66, 41, 14, 91, 87, 92, 68, 32, 73, 50));
            Assert.AreEqual(4, n);

            Assert.AreEqual(722, res.Delegate(84, 91, 12, 87, 52, 21, 99, 97, 13, 40, 83, 9, 34));
            Assert.AreEqual(4, n);

            Assert.AreEqual(751, res.Delegate(78, 35, 17, 13, 92, 34, 87, 92, 64, 7, 69, 76, 87));
            Assert.AreEqual(4, n);

            Assert.AreEqual(736, res.Delegate(85, 90, 9, 57, 95, 80, 68, 6, 16, 24, 82, 97, 27));
            Assert.AreEqual(4, n);

            Assert.AreEqual(795, res.Delegate(67, 63, 51, 66, 41, 14, 91, 87, 92, 68, 32, 73, 50));
            Assert.AreEqual(4, n);

            res.Cache.Clear();
            Assert.AreEqual(4, n);

            Assert.AreEqual(722, res.Delegate(84, 91, 12, 87, 52, 21, 99, 97, 13, 40, 83, 9, 34));
            Assert.AreEqual(5, n);

            Assert.AreEqual(751, res.Delegate(78, 35, 17, 13, 92, 34, 87, 92, 64, 7, 69, 76, 87));
            Assert.AreEqual(6, n);

            Assert.AreEqual(736, res.Delegate(85, 90, 9, 57, 95, 80, 68, 6, 16, 24, 82, 97, 27));
            Assert.AreEqual(7, n);

            Assert.AreEqual(795, res.Delegate(67, 63, 51, 66, 41, 14, 91, 87, 92, 68, 32, 73, 50));
            Assert.AreEqual(8, n);

        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize15()
        {
            var m = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var n = 0;

            var res = FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => { n++; return t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 + t13 + t14; });

            Assert.AreEqual(592, res.Delegate(2, 57, 75, 20, 84, 43, 11, 64, 36, 9, 87, 56, 45, 3));
            Assert.AreEqual(1, n);

            Assert.AreEqual(685, res.Delegate(41, 28, 61, 11, 34, 15, 65, 66, 23, 40, 97, 65, 54, 85));
            Assert.AreEqual(2, n);

            Assert.AreEqual(760, res.Delegate(0, 93, 21, 76, 84, 45, 77, 11, 30, 96, 40, 39, 86, 62));
            Assert.AreEqual(3, n);

            Assert.AreEqual(735, res.Delegate(31, 54, 63, 10, 96, 11, 50, 59, 30, 57, 97, 57, 84, 36));
            Assert.AreEqual(4, n);

            Assert.AreEqual(592, res.Delegate(2, 57, 75, 20, 84, 43, 11, 64, 36, 9, 87, 56, 45, 3));
            Assert.AreEqual(4, n);

            Assert.AreEqual(685, res.Delegate(41, 28, 61, 11, 34, 15, 65, 66, 23, 40, 97, 65, 54, 85));
            Assert.AreEqual(4, n);

            Assert.AreEqual(760, res.Delegate(0, 93, 21, 76, 84, 45, 77, 11, 30, 96, 40, 39, 86, 62));
            Assert.AreEqual(4, n);

            Assert.AreEqual(735, res.Delegate(31, 54, 63, 10, 96, 11, 50, 59, 30, 57, 97, 57, 84, 36));
            Assert.AreEqual(4, n);

            res.Cache.Clear();
            Assert.AreEqual(4, n);

            Assert.AreEqual(592, res.Delegate(2, 57, 75, 20, 84, 43, 11, 64, 36, 9, 87, 56, 45, 3));
            Assert.AreEqual(5, n);

            Assert.AreEqual(685, res.Delegate(41, 28, 61, 11, 34, 15, 65, 66, 23, 40, 97, 65, 54, 85));
            Assert.AreEqual(6, n);

            Assert.AreEqual(760, res.Delegate(0, 93, 21, 76, 84, 45, 77, 11, 30, 96, 40, 39, 86, 62));
            Assert.AreEqual(7, n);

            Assert.AreEqual(735, res.Delegate(31, 54, 63, 10, 96, 11, 50, 59, 30, 57, 97, 57, 84, 36));
            Assert.AreEqual(8, n);

        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize16()
        {
            var m = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var n = 0;

            var res = FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => { n++; return t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 + t13 + t14 + t15; });

            Assert.AreEqual(747, res.Delegate(33, 34, 22, 18, 88, 26, 64, 42, 88, 11, 71, 99, 25, 29, 97));
            Assert.AreEqual(1, n);

            Assert.AreEqual(839, res.Delegate(65, 70, 95, 29, 3, 34, 69, 76, 87, 69, 43, 34, 40, 62, 63));
            Assert.AreEqual(2, n);

            Assert.AreEqual(928, res.Delegate(79, 26, 61, 41, 78, 95, 74, 22, 51, 60, 97, 89, 65, 52, 38));
            Assert.AreEqual(3, n);

            Assert.AreEqual(775, res.Delegate(96, 85, 21, 62, 65, 87, 1, 28, 80, 1, 64, 58, 35, 48, 44));
            Assert.AreEqual(4, n);

            Assert.AreEqual(747, res.Delegate(33, 34, 22, 18, 88, 26, 64, 42, 88, 11, 71, 99, 25, 29, 97));
            Assert.AreEqual(4, n);

            Assert.AreEqual(839, res.Delegate(65, 70, 95, 29, 3, 34, 69, 76, 87, 69, 43, 34, 40, 62, 63));
            Assert.AreEqual(4, n);

            Assert.AreEqual(928, res.Delegate(79, 26, 61, 41, 78, 95, 74, 22, 51, 60, 97, 89, 65, 52, 38));
            Assert.AreEqual(4, n);

            Assert.AreEqual(775, res.Delegate(96, 85, 21, 62, 65, 87, 1, 28, 80, 1, 64, 58, 35, 48, 44));
            Assert.AreEqual(4, n);

            res.Cache.Clear();
            Assert.AreEqual(4, n);

            Assert.AreEqual(747, res.Delegate(33, 34, 22, 18, 88, 26, 64, 42, 88, 11, 71, 99, 25, 29, 97));
            Assert.AreEqual(5, n);

            Assert.AreEqual(839, res.Delegate(65, 70, 95, 29, 3, 34, 69, 76, 87, 69, 43, 34, 40, 62, 63));
            Assert.AreEqual(6, n);

            Assert.AreEqual(928, res.Delegate(79, 26, 61, 41, 78, 95, 74, 22, 51, 60, 97, 89, 65, 52, 38));
            Assert.AreEqual(7, n);

            Assert.AreEqual(775, res.Delegate(96, 85, 21, 62, 65, 87, 1, 28, 80, 1, 64, 58, 35, 48, 44));
            Assert.AreEqual(8, n);

        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize17()
        {
            var m = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var n = 0;

            var res = FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => { n++; return t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 + t13 + t14 + t15 + t16; });

            Assert.AreEqual(768, res.Delegate(91, 23, 79, 24, 31, 44, 38, 84, 51, 1, 91, 48, 44, 69, 5, 45));
            Assert.AreEqual(1, n);

            Assert.AreEqual(851, res.Delegate(3, 23, 48, 73, 58, 13, 78, 97, 75, 77, 98, 80, 39, 14, 37, 38));
            Assert.AreEqual(2, n);

            Assert.AreEqual(796, res.Delegate(73, 6, 68, 73, 9, 40, 20, 93, 57, 0, 69, 61, 74, 38, 57, 58));
            Assert.AreEqual(3, n);

            Assert.AreEqual(964, res.Delegate(75, 56, 60, 34, 87, 75, 86, 78, 45, 81, 48, 54, 46, 58, 44, 37));
            Assert.AreEqual(4, n);

            Assert.AreEqual(768, res.Delegate(91, 23, 79, 24, 31, 44, 38, 84, 51, 1, 91, 48, 44, 69, 5, 45));
            Assert.AreEqual(4, n);

            Assert.AreEqual(851, res.Delegate(3, 23, 48, 73, 58, 13, 78, 97, 75, 77, 98, 80, 39, 14, 37, 38));
            Assert.AreEqual(4, n);

            Assert.AreEqual(796, res.Delegate(73, 6, 68, 73, 9, 40, 20, 93, 57, 0, 69, 61, 74, 38, 57, 58));
            Assert.AreEqual(4, n);

            Assert.AreEqual(964, res.Delegate(75, 56, 60, 34, 87, 75, 86, 78, 45, 81, 48, 54, 46, 58, 44, 37));
            Assert.AreEqual(4, n);

            res.Cache.Clear();
            Assert.AreEqual(4, n);

            Assert.AreEqual(768, res.Delegate(91, 23, 79, 24, 31, 44, 38, 84, 51, 1, 91, 48, 44, 69, 5, 45));
            Assert.AreEqual(5, n);

            Assert.AreEqual(851, res.Delegate(3, 23, 48, 73, 58, 13, 78, 97, 75, 77, 98, 80, 39, 14, 37, 38));
            Assert.AreEqual(6, n);

            Assert.AreEqual(796, res.Delegate(73, 6, 68, 73, 9, 40, 20, 93, 57, 0, 69, 61, 74, 38, 57, 58));
            Assert.AreEqual(7, n);

            Assert.AreEqual(964, res.Delegate(75, 56, 60, 34, 87, 75, 86, 78, 45, 81, 48, 54, 46, 58, 44, 37));
            Assert.AreEqual(8, n);

        }


        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_EqualityComparer_ArgumentChecking()
        {
            var m = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var c = EqualityComparer<int>.Default;

            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int>(default(IMemoizer), (t1, t2) => 0, MemoizationOptions.None, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int>(m, default(Func<int, int, int>), MemoizationOptions.None, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int>(m, (t1, t2) => 0, MemoizationOptions.None, default(IEqualityComparer<int>), c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int>(m, (t1, t2) => 0, MemoizationOptions.None, c, default(IEqualityComparer<int>)));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int>(default(IMemoizer), (t1, t2, t3) => 0, MemoizationOptions.None, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int>(m, default(Func<int, int, int, int>), MemoizationOptions.None, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int>(m, (t1, t2, t3) => 0, MemoizationOptions.None, default(IEqualityComparer<int>), c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int>(m, (t1, t2, t3) => 0, MemoizationOptions.None, c, default(IEqualityComparer<int>), c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int>(m, (t1, t2, t3) => 0, MemoizationOptions.None, c, c, default(IEqualityComparer<int>)));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int>(default(IMemoizer), (t1, t2, t3, t4) => 0, MemoizationOptions.None, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int>(m, default(Func<int, int, int, int, int>), MemoizationOptions.None, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int>(m, (t1, t2, t3, t4) => 0, MemoizationOptions.None, default(IEqualityComparer<int>), c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int>(m, (t1, t2, t3, t4) => 0, MemoizationOptions.None, c, default(IEqualityComparer<int>), c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int>(m, (t1, t2, t3, t4) => 0, MemoizationOptions.None, c, c, default(IEqualityComparer<int>), c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int>(m, (t1, t2, t3, t4) => 0, MemoizationOptions.None, c, c, c, default(IEqualityComparer<int>)));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int>(default(IMemoizer), (t1, t2, t3, t4, t5) => 0, MemoizationOptions.None, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int>(m, default(Func<int, int, int, int, int, int>), MemoizationOptions.None, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5) => 0, MemoizationOptions.None, default(IEqualityComparer<int>), c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5) => 0, MemoizationOptions.None, c, default(IEqualityComparer<int>), c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5) => 0, MemoizationOptions.None, c, c, default(IEqualityComparer<int>), c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5) => 0, MemoizationOptions.None, c, c, c, default(IEqualityComparer<int>), c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5) => 0, MemoizationOptions.None, c, c, c, c, default(IEqualityComparer<int>)));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int>(default(IMemoizer), (t1, t2, t3, t4, t5, t6) => 0, MemoizationOptions.None, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int>(m, default(Func<int, int, int, int, int, int, int>), MemoizationOptions.None, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6) => 0, MemoizationOptions.None, default(IEqualityComparer<int>), c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6) => 0, MemoizationOptions.None, c, default(IEqualityComparer<int>), c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6) => 0, MemoizationOptions.None, c, c, default(IEqualityComparer<int>), c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6) => 0, MemoizationOptions.None, c, c, c, default(IEqualityComparer<int>), c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6) => 0, MemoizationOptions.None, c, c, c, c, default(IEqualityComparer<int>), c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6) => 0, MemoizationOptions.None, c, c, c, c, c, default(IEqualityComparer<int>)));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int>(default(IMemoizer), (t1, t2, t3, t4, t5, t6, t7) => 0, MemoizationOptions.None, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int>(m, default(Func<int, int, int, int, int, int, int, int>), MemoizationOptions.None, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7) => 0, MemoizationOptions.None, default(IEqualityComparer<int>), c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7) => 0, MemoizationOptions.None, c, default(IEqualityComparer<int>), c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7) => 0, MemoizationOptions.None, c, c, default(IEqualityComparer<int>), c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7) => 0, MemoizationOptions.None, c, c, c, default(IEqualityComparer<int>), c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7) => 0, MemoizationOptions.None, c, c, c, c, default(IEqualityComparer<int>), c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7) => 0, MemoizationOptions.None, c, c, c, c, c, default(IEqualityComparer<int>), c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7) => 0, MemoizationOptions.None, c, c, c, c, c, c, default(IEqualityComparer<int>)));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int>(default(IMemoizer), (t1, t2, t3, t4, t5, t6, t7, t8) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int>(m, default(Func<int, int, int, int, int, int, int, int, int>), MemoizationOptions.None, c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8) => 0, MemoizationOptions.None, default(IEqualityComparer<int>), c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8) => 0, MemoizationOptions.None, c, default(IEqualityComparer<int>), c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8) => 0, MemoizationOptions.None, c, c, default(IEqualityComparer<int>), c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8) => 0, MemoizationOptions.None, c, c, c, default(IEqualityComparer<int>), c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8) => 0, MemoizationOptions.None, c, c, c, c, default(IEqualityComparer<int>), c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8) => 0, MemoizationOptions.None, c, c, c, c, c, default(IEqualityComparer<int>), c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8) => 0, MemoizationOptions.None, c, c, c, c, c, c, default(IEqualityComparer<int>), c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, default(IEqualityComparer<int>)));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int>(default(IMemoizer), (t1, t2, t3, t4, t5, t6, t7, t8, t9) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int>(m, default(Func<int, int, int, int, int, int, int, int, int, int>), MemoizationOptions.None, c, c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9) => 0, MemoizationOptions.None, default(IEqualityComparer<int>), c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9) => 0, MemoizationOptions.None, c, default(IEqualityComparer<int>), c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9) => 0, MemoizationOptions.None, c, c, default(IEqualityComparer<int>), c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9) => 0, MemoizationOptions.None, c, c, c, default(IEqualityComparer<int>), c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9) => 0, MemoizationOptions.None, c, c, c, c, default(IEqualityComparer<int>), c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9) => 0, MemoizationOptions.None, c, c, c, c, c, default(IEqualityComparer<int>), c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9) => 0, MemoizationOptions.None, c, c, c, c, c, c, default(IEqualityComparer<int>), c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, default(IEqualityComparer<int>), c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, c, default(IEqualityComparer<int>)));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int>(default(IMemoizer), (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int>(m, default(Func<int, int, int, int, int, int, int, int, int, int, int>), MemoizationOptions.None, c, c, c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => 0, MemoizationOptions.None, default(IEqualityComparer<int>), c, c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => 0, MemoizationOptions.None, c, default(IEqualityComparer<int>), c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => 0, MemoizationOptions.None, c, c, default(IEqualityComparer<int>), c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => 0, MemoizationOptions.None, c, c, c, default(IEqualityComparer<int>), c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => 0, MemoizationOptions.None, c, c, c, c, default(IEqualityComparer<int>), c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => 0, MemoizationOptions.None, c, c, c, c, c, default(IEqualityComparer<int>), c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => 0, MemoizationOptions.None, c, c, c, c, c, c, default(IEqualityComparer<int>), c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, default(IEqualityComparer<int>), c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, c, default(IEqualityComparer<int>), c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, c, c, default(IEqualityComparer<int>)));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int>(default(IMemoizer), (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int>(m, default(Func<int, int, int, int, int, int, int, int, int, int, int, int>), MemoizationOptions.None, c, c, c, c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => 0, MemoizationOptions.None, default(IEqualityComparer<int>), c, c, c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => 0, MemoizationOptions.None, c, default(IEqualityComparer<int>), c, c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => 0, MemoizationOptions.None, c, c, default(IEqualityComparer<int>), c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => 0, MemoizationOptions.None, c, c, c, default(IEqualityComparer<int>), c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => 0, MemoizationOptions.None, c, c, c, c, default(IEqualityComparer<int>), c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => 0, MemoizationOptions.None, c, c, c, c, c, default(IEqualityComparer<int>), c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => 0, MemoizationOptions.None, c, c, c, c, c, c, default(IEqualityComparer<int>), c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, default(IEqualityComparer<int>), c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, c, default(IEqualityComparer<int>), c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, c, c, default(IEqualityComparer<int>), c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, c, c, c, default(IEqualityComparer<int>)));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int>(default(IMemoizer), (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int>(m, default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int>), MemoizationOptions.None, c, c, c, c, c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => 0, MemoizationOptions.None, default(IEqualityComparer<int>), c, c, c, c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => 0, MemoizationOptions.None, c, default(IEqualityComparer<int>), c, c, c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => 0, MemoizationOptions.None, c, c, default(IEqualityComparer<int>), c, c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => 0, MemoizationOptions.None, c, c, c, default(IEqualityComparer<int>), c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => 0, MemoizationOptions.None, c, c, c, c, default(IEqualityComparer<int>), c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => 0, MemoizationOptions.None, c, c, c, c, c, default(IEqualityComparer<int>), c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => 0, MemoizationOptions.None, c, c, c, c, c, c, default(IEqualityComparer<int>), c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, default(IEqualityComparer<int>), c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, c, default(IEqualityComparer<int>), c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, c, c, default(IEqualityComparer<int>), c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, c, c, c, default(IEqualityComparer<int>), c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, c, c, c, c, default(IEqualityComparer<int>)));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(default(IMemoizer), (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int>), MemoizationOptions.None, c, c, c, c, c, c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => 0, MemoizationOptions.None, default(IEqualityComparer<int>), c, c, c, c, c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => 0, MemoizationOptions.None, c, default(IEqualityComparer<int>), c, c, c, c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => 0, MemoizationOptions.None, c, c, default(IEqualityComparer<int>), c, c, c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => 0, MemoizationOptions.None, c, c, c, default(IEqualityComparer<int>), c, c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => 0, MemoizationOptions.None, c, c, c, c, default(IEqualityComparer<int>), c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => 0, MemoizationOptions.None, c, c, c, c, c, default(IEqualityComparer<int>), c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => 0, MemoizationOptions.None, c, c, c, c, c, c, default(IEqualityComparer<int>), c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, default(IEqualityComparer<int>), c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, c, default(IEqualityComparer<int>), c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, c, c, default(IEqualityComparer<int>), c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, c, c, c, default(IEqualityComparer<int>), c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, c, c, c, c, default(IEqualityComparer<int>), c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, c, c, c, c, c, default(IEqualityComparer<int>)));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(default(IMemoizer), (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>), MemoizationOptions.None, c, c, c, c, c, c, c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => 0, MemoizationOptions.None, default(IEqualityComparer<int>), c, c, c, c, c, c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => 0, MemoizationOptions.None, c, default(IEqualityComparer<int>), c, c, c, c, c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => 0, MemoizationOptions.None, c, c, default(IEqualityComparer<int>), c, c, c, c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => 0, MemoizationOptions.None, c, c, c, default(IEqualityComparer<int>), c, c, c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => 0, MemoizationOptions.None, c, c, c, c, default(IEqualityComparer<int>), c, c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => 0, MemoizationOptions.None, c, c, c, c, c, default(IEqualityComparer<int>), c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => 0, MemoizationOptions.None, c, c, c, c, c, c, default(IEqualityComparer<int>), c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, default(IEqualityComparer<int>), c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, c, default(IEqualityComparer<int>), c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, c, c, default(IEqualityComparer<int>), c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, c, c, c, default(IEqualityComparer<int>), c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, c, c, c, c, default(IEqualityComparer<int>), c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, c, c, c, c, c, default(IEqualityComparer<int>), c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, c, c, c, c, c, c, default(IEqualityComparer<int>)));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(default(IMemoizer), (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>), MemoizationOptions.None, c, c, c, c, c, c, c, c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => 0, MemoizationOptions.None, default(IEqualityComparer<int>), c, c, c, c, c, c, c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => 0, MemoizationOptions.None, c, default(IEqualityComparer<int>), c, c, c, c, c, c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => 0, MemoizationOptions.None, c, c, default(IEqualityComparer<int>), c, c, c, c, c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => 0, MemoizationOptions.None, c, c, c, default(IEqualityComparer<int>), c, c, c, c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => 0, MemoizationOptions.None, c, c, c, c, default(IEqualityComparer<int>), c, c, c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => 0, MemoizationOptions.None, c, c, c, c, c, default(IEqualityComparer<int>), c, c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => 0, MemoizationOptions.None, c, c, c, c, c, c, default(IEqualityComparer<int>), c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, default(IEqualityComparer<int>), c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, c, default(IEqualityComparer<int>), c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, c, c, default(IEqualityComparer<int>), c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, c, c, c, default(IEqualityComparer<int>), c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, c, c, c, c, default(IEqualityComparer<int>), c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, c, c, c, c, c, default(IEqualityComparer<int>), c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, c, c, c, c, c, c, default(IEqualityComparer<int>), c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, c, c, c, c, c, c, c, default(IEqualityComparer<int>)));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(default(IMemoizer), (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, c, c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>), MemoizationOptions.None, c, c, c, c, c, c, c, c, c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => 0, MemoizationOptions.None, default(IEqualityComparer<int>), c, c, c, c, c, c, c, c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => 0, MemoizationOptions.None, c, default(IEqualityComparer<int>), c, c, c, c, c, c, c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => 0, MemoizationOptions.None, c, c, default(IEqualityComparer<int>), c, c, c, c, c, c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => 0, MemoizationOptions.None, c, c, c, default(IEqualityComparer<int>), c, c, c, c, c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => 0, MemoizationOptions.None, c, c, c, c, default(IEqualityComparer<int>), c, c, c, c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => 0, MemoizationOptions.None, c, c, c, c, c, default(IEqualityComparer<int>), c, c, c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => 0, MemoizationOptions.None, c, c, c, c, c, c, default(IEqualityComparer<int>), c, c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, default(IEqualityComparer<int>), c, c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, c, default(IEqualityComparer<int>), c, c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, c, c, default(IEqualityComparer<int>), c, c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, c, c, c, default(IEqualityComparer<int>), c, c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, c, c, c, c, default(IEqualityComparer<int>), c, c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, c, c, c, c, c, default(IEqualityComparer<int>), c, c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, c, c, c, c, c, c, default(IEqualityComparer<int>), c, c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, c, c, c, c, c, c, c, default(IEqualityComparer<int>), c));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => 0, MemoizationOptions.None, c, c, c, c, c, c, c, c, c, c, c, c, c, c, c, default(IEqualityComparer<int>)));
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_EqualityComparer3()
        {
            var m = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var c = EqualityComparer<int>.Default;
            var n = 0;

            var res = FunctionMemoizationExtensions.Memoize<int, int, int>(m, (t1, t2) => { n++; return t1 + t2; }, MemoizationOptions.None, c, c);

            Assert.AreEqual(115, res.Delegate(63, 52));
            Assert.AreEqual(1, n);

            Assert.AreEqual(113, res.Delegate(75, 38));
            Assert.AreEqual(2, n);

            Assert.AreEqual(32, res.Delegate(1, 31));
            Assert.AreEqual(3, n);

            Assert.AreEqual(97, res.Delegate(35, 62));
            Assert.AreEqual(4, n);

            Assert.AreEqual(115, res.Delegate(63, 52));
            Assert.AreEqual(4, n);

            Assert.AreEqual(113, res.Delegate(75, 38));
            Assert.AreEqual(4, n);

            Assert.AreEqual(32, res.Delegate(1, 31));
            Assert.AreEqual(4, n);

            Assert.AreEqual(97, res.Delegate(35, 62));
            Assert.AreEqual(4, n);

            res.Cache.Clear();
            Assert.AreEqual(4, n);

            Assert.AreEqual(115, res.Delegate(63, 52));
            Assert.AreEqual(5, n);

            Assert.AreEqual(113, res.Delegate(75, 38));
            Assert.AreEqual(6, n);

            Assert.AreEqual(32, res.Delegate(1, 31));
            Assert.AreEqual(7, n);

            Assert.AreEqual(97, res.Delegate(35, 62));
            Assert.AreEqual(8, n);

        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_EqualityComparer4()
        {
            var m = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var c = EqualityComparer<int>.Default;
            var n = 0;

            var res = FunctionMemoizationExtensions.Memoize<int, int, int, int>(m, (t1, t2, t3) => { n++; return t1 + t2 + t3; }, MemoizationOptions.None, c, c, c);

            Assert.AreEqual(72, res.Delegate(3, 54, 15));
            Assert.AreEqual(1, n);

            Assert.AreEqual(117, res.Delegate(57, 43, 17));
            Assert.AreEqual(2, n);

            Assert.AreEqual(79, res.Delegate(23, 37, 19));
            Assert.AreEqual(3, n);

            Assert.AreEqual(126, res.Delegate(39, 4, 83));
            Assert.AreEqual(4, n);

            Assert.AreEqual(72, res.Delegate(3, 54, 15));
            Assert.AreEqual(4, n);

            Assert.AreEqual(117, res.Delegate(57, 43, 17));
            Assert.AreEqual(4, n);

            Assert.AreEqual(79, res.Delegate(23, 37, 19));
            Assert.AreEqual(4, n);

            Assert.AreEqual(126, res.Delegate(39, 4, 83));
            Assert.AreEqual(4, n);

            res.Cache.Clear();
            Assert.AreEqual(4, n);

            Assert.AreEqual(72, res.Delegate(3, 54, 15));
            Assert.AreEqual(5, n);

            Assert.AreEqual(117, res.Delegate(57, 43, 17));
            Assert.AreEqual(6, n);

            Assert.AreEqual(79, res.Delegate(23, 37, 19));
            Assert.AreEqual(7, n);

            Assert.AreEqual(126, res.Delegate(39, 4, 83));
            Assert.AreEqual(8, n);

        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_EqualityComparer5()
        {
            var m = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var c = EqualityComparer<int>.Default;
            var n = 0;

            var res = FunctionMemoizationExtensions.Memoize<int, int, int, int, int>(m, (t1, t2, t3, t4) => { n++; return t1 + t2 + t3 + t4; }, MemoizationOptions.None, c, c, c, c);

            Assert.AreEqual(204, res.Delegate(53, 3, 51, 97));
            Assert.AreEqual(1, n);

            Assert.AreEqual(163, res.Delegate(19, 89, 28, 27));
            Assert.AreEqual(2, n);

            Assert.AreEqual(302, res.Delegate(91, 66, 47, 98));
            Assert.AreEqual(3, n);

            Assert.AreEqual(206, res.Delegate(56, 32, 97, 21));
            Assert.AreEqual(4, n);

            Assert.AreEqual(204, res.Delegate(53, 3, 51, 97));
            Assert.AreEqual(4, n);

            Assert.AreEqual(163, res.Delegate(19, 89, 28, 27));
            Assert.AreEqual(4, n);

            Assert.AreEqual(302, res.Delegate(91, 66, 47, 98));
            Assert.AreEqual(4, n);

            Assert.AreEqual(206, res.Delegate(56, 32, 97, 21));
            Assert.AreEqual(4, n);

            res.Cache.Clear();
            Assert.AreEqual(4, n);

            Assert.AreEqual(204, res.Delegate(53, 3, 51, 97));
            Assert.AreEqual(5, n);

            Assert.AreEqual(163, res.Delegate(19, 89, 28, 27));
            Assert.AreEqual(6, n);

            Assert.AreEqual(302, res.Delegate(91, 66, 47, 98));
            Assert.AreEqual(7, n);

            Assert.AreEqual(206, res.Delegate(56, 32, 97, 21));
            Assert.AreEqual(8, n);

        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_EqualityComparer6()
        {
            var m = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var c = EqualityComparer<int>.Default;
            var n = 0;

            var res = FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5) => { n++; return t1 + t2 + t3 + t4 + t5; }, MemoizationOptions.None, c, c, c, c, c);

            Assert.AreEqual(202, res.Delegate(63, 19, 57, 43, 20));
            Assert.AreEqual(1, n);

            Assert.AreEqual(250, res.Delegate(97, 31, 33, 60, 29));
            Assert.AreEqual(2, n);

            Assert.AreEqual(166, res.Delegate(35, 28, 58, 11, 34));
            Assert.AreEqual(3, n);

            Assert.AreEqual(264, res.Delegate(6, 54, 61, 83, 60));
            Assert.AreEqual(4, n);

            Assert.AreEqual(202, res.Delegate(63, 19, 57, 43, 20));
            Assert.AreEqual(4, n);

            Assert.AreEqual(250, res.Delegate(97, 31, 33, 60, 29));
            Assert.AreEqual(4, n);

            Assert.AreEqual(166, res.Delegate(35, 28, 58, 11, 34));
            Assert.AreEqual(4, n);

            Assert.AreEqual(264, res.Delegate(6, 54, 61, 83, 60));
            Assert.AreEqual(4, n);

            res.Cache.Clear();
            Assert.AreEqual(4, n);

            Assert.AreEqual(202, res.Delegate(63, 19, 57, 43, 20));
            Assert.AreEqual(5, n);

            Assert.AreEqual(250, res.Delegate(97, 31, 33, 60, 29));
            Assert.AreEqual(6, n);

            Assert.AreEqual(166, res.Delegate(35, 28, 58, 11, 34));
            Assert.AreEqual(7, n);

            Assert.AreEqual(264, res.Delegate(6, 54, 61, 83, 60));
            Assert.AreEqual(8, n);

        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_EqualityComparer7()
        {
            var m = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var c = EqualityComparer<int>.Default;
            var n = 0;

            var res = FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6) => { n++; return t1 + t2 + t3 + t4 + t5 + t6; }, MemoizationOptions.None, c, c, c, c, c, c);

            Assert.AreEqual(119, res.Delegate(1, 78, 18, 11, 3, 8));
            Assert.AreEqual(1, n);

            Assert.AreEqual(140, res.Delegate(70, 36, 6, 17, 1, 10));
            Assert.AreEqual(2, n);

            Assert.AreEqual(235, res.Delegate(19, 2, 73, 0, 81, 60));
            Assert.AreEqual(3, n);

            Assert.AreEqual(333, res.Delegate(63, 56, 71, 17, 37, 89));
            Assert.AreEqual(4, n);

            Assert.AreEqual(119, res.Delegate(1, 78, 18, 11, 3, 8));
            Assert.AreEqual(4, n);

            Assert.AreEqual(140, res.Delegate(70, 36, 6, 17, 1, 10));
            Assert.AreEqual(4, n);

            Assert.AreEqual(235, res.Delegate(19, 2, 73, 0, 81, 60));
            Assert.AreEqual(4, n);

            Assert.AreEqual(333, res.Delegate(63, 56, 71, 17, 37, 89));
            Assert.AreEqual(4, n);

            res.Cache.Clear();
            Assert.AreEqual(4, n);

            Assert.AreEqual(119, res.Delegate(1, 78, 18, 11, 3, 8));
            Assert.AreEqual(5, n);

            Assert.AreEqual(140, res.Delegate(70, 36, 6, 17, 1, 10));
            Assert.AreEqual(6, n);

            Assert.AreEqual(235, res.Delegate(19, 2, 73, 0, 81, 60));
            Assert.AreEqual(7, n);

            Assert.AreEqual(333, res.Delegate(63, 56, 71, 17, 37, 89));
            Assert.AreEqual(8, n);

        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_EqualityComparer8()
        {
            var m = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var c = EqualityComparer<int>.Default;
            var n = 0;

            var res = FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7) => { n++; return t1 + t2 + t3 + t4 + t5 + t6 + t7; }, MemoizationOptions.None, c, c, c, c, c, c, c);

            Assert.AreEqual(417, res.Delegate(54, 99, 69, 80, 31, 40, 44));
            Assert.AreEqual(1, n);

            Assert.AreEqual(331, res.Delegate(94, 49, 37, 20, 85, 0, 46));
            Assert.AreEqual(2, n);

            Assert.AreEqual(267, res.Delegate(40, 11, 26, 94, 26, 42, 28));
            Assert.AreEqual(3, n);

            Assert.AreEqual(277, res.Delegate(24, 8, 56, 37, 34, 25, 93));
            Assert.AreEqual(4, n);

            Assert.AreEqual(417, res.Delegate(54, 99, 69, 80, 31, 40, 44));
            Assert.AreEqual(4, n);

            Assert.AreEqual(331, res.Delegate(94, 49, 37, 20, 85, 0, 46));
            Assert.AreEqual(4, n);

            Assert.AreEqual(267, res.Delegate(40, 11, 26, 94, 26, 42, 28));
            Assert.AreEqual(4, n);

            Assert.AreEqual(277, res.Delegate(24, 8, 56, 37, 34, 25, 93));
            Assert.AreEqual(4, n);

            res.Cache.Clear();
            Assert.AreEqual(4, n);

            Assert.AreEqual(417, res.Delegate(54, 99, 69, 80, 31, 40, 44));
            Assert.AreEqual(5, n);

            Assert.AreEqual(331, res.Delegate(94, 49, 37, 20, 85, 0, 46));
            Assert.AreEqual(6, n);

            Assert.AreEqual(267, res.Delegate(40, 11, 26, 94, 26, 42, 28));
            Assert.AreEqual(7, n);

            Assert.AreEqual(277, res.Delegate(24, 8, 56, 37, 34, 25, 93));
            Assert.AreEqual(8, n);

        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_EqualityComparer9()
        {
            var m = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var c = EqualityComparer<int>.Default;
            var n = 0;

            var res = FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8) => { n++; return t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8; }, MemoizationOptions.None, c, c, c, c, c, c, c, c);

            Assert.AreEqual(424, res.Delegate(97, 27, 88, 83, 41, 28, 57, 3));
            Assert.AreEqual(1, n);

            Assert.AreEqual(421, res.Delegate(38, 90, 5, 66, 73, 6, 61, 82));
            Assert.AreEqual(2, n);

            Assert.AreEqual(480, res.Delegate(82, 88, 99, 35, 19, 51, 29, 77));
            Assert.AreEqual(3, n);

            Assert.AreEqual(518, res.Delegate(91, 94, 61, 29, 91, 13, 42, 97));
            Assert.AreEqual(4, n);

            Assert.AreEqual(424, res.Delegate(97, 27, 88, 83, 41, 28, 57, 3));
            Assert.AreEqual(4, n);

            Assert.AreEqual(421, res.Delegate(38, 90, 5, 66, 73, 6, 61, 82));
            Assert.AreEqual(4, n);

            Assert.AreEqual(480, res.Delegate(82, 88, 99, 35, 19, 51, 29, 77));
            Assert.AreEqual(4, n);

            Assert.AreEqual(518, res.Delegate(91, 94, 61, 29, 91, 13, 42, 97));
            Assert.AreEqual(4, n);

            res.Cache.Clear();
            Assert.AreEqual(4, n);

            Assert.AreEqual(424, res.Delegate(97, 27, 88, 83, 41, 28, 57, 3));
            Assert.AreEqual(5, n);

            Assert.AreEqual(421, res.Delegate(38, 90, 5, 66, 73, 6, 61, 82));
            Assert.AreEqual(6, n);

            Assert.AreEqual(480, res.Delegate(82, 88, 99, 35, 19, 51, 29, 77));
            Assert.AreEqual(7, n);

            Assert.AreEqual(518, res.Delegate(91, 94, 61, 29, 91, 13, 42, 97));
            Assert.AreEqual(8, n);

        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_EqualityComparer10()
        {
            var m = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var c = EqualityComparer<int>.Default;
            var n = 0;

            var res = FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9) => { n++; return t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9; }, MemoizationOptions.None, c, c, c, c, c, c, c, c, c);

            Assert.AreEqual(471, res.Delegate(15, 50, 97, 21, 48, 36, 44, 71, 89));
            Assert.AreEqual(1, n);

            Assert.AreEqual(471, res.Delegate(37, 72, 35, 89, 60, 69, 22, 62, 25));
            Assert.AreEqual(2, n);

            Assert.AreEqual(564, res.Delegate(74, 49, 34, 89, 73, 45, 97, 11, 92));
            Assert.AreEqual(3, n);

            Assert.AreEqual(304, res.Delegate(46, 67, 27, 12, 25, 48, 7, 50, 22));
            Assert.AreEqual(4, n);

            Assert.AreEqual(471, res.Delegate(15, 50, 97, 21, 48, 36, 44, 71, 89));
            Assert.AreEqual(4, n);

            Assert.AreEqual(471, res.Delegate(37, 72, 35, 89, 60, 69, 22, 62, 25));
            Assert.AreEqual(4, n);

            Assert.AreEqual(564, res.Delegate(74, 49, 34, 89, 73, 45, 97, 11, 92));
            Assert.AreEqual(4, n);

            Assert.AreEqual(304, res.Delegate(46, 67, 27, 12, 25, 48, 7, 50, 22));
            Assert.AreEqual(4, n);

            res.Cache.Clear();
            Assert.AreEqual(4, n);

            Assert.AreEqual(471, res.Delegate(15, 50, 97, 21, 48, 36, 44, 71, 89));
            Assert.AreEqual(5, n);

            Assert.AreEqual(471, res.Delegate(37, 72, 35, 89, 60, 69, 22, 62, 25));
            Assert.AreEqual(6, n);

            Assert.AreEqual(564, res.Delegate(74, 49, 34, 89, 73, 45, 97, 11, 92));
            Assert.AreEqual(7, n);

            Assert.AreEqual(304, res.Delegate(46, 67, 27, 12, 25, 48, 7, 50, 22));
            Assert.AreEqual(8, n);

        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_EqualityComparer11()
        {
            var m = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var c = EqualityComparer<int>.Default;
            var n = 0;

            var res = FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => { n++; return t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10; }, MemoizationOptions.None, c, c, c, c, c, c, c, c, c, c);

            Assert.AreEqual(498, res.Delegate(8, 39, 34, 45, 44, 27, 46, 82, 79, 94));
            Assert.AreEqual(1, n);

            Assert.AreEqual(529, res.Delegate(87, 30, 24, 39, 66, 65, 39, 92, 62, 25));
            Assert.AreEqual(2, n);

            Assert.AreEqual(465, res.Delegate(76, 51, 24, 37, 43, 97, 4, 61, 25, 47));
            Assert.AreEqual(3, n);

            Assert.AreEqual(388, res.Delegate(86, 81, 9, 47, 13, 23, 91, 28, 4, 6));
            Assert.AreEqual(4, n);

            Assert.AreEqual(498, res.Delegate(8, 39, 34, 45, 44, 27, 46, 82, 79, 94));
            Assert.AreEqual(4, n);

            Assert.AreEqual(529, res.Delegate(87, 30, 24, 39, 66, 65, 39, 92, 62, 25));
            Assert.AreEqual(4, n);

            Assert.AreEqual(465, res.Delegate(76, 51, 24, 37, 43, 97, 4, 61, 25, 47));
            Assert.AreEqual(4, n);

            Assert.AreEqual(388, res.Delegate(86, 81, 9, 47, 13, 23, 91, 28, 4, 6));
            Assert.AreEqual(4, n);

            res.Cache.Clear();
            Assert.AreEqual(4, n);

            Assert.AreEqual(498, res.Delegate(8, 39, 34, 45, 44, 27, 46, 82, 79, 94));
            Assert.AreEqual(5, n);

            Assert.AreEqual(529, res.Delegate(87, 30, 24, 39, 66, 65, 39, 92, 62, 25));
            Assert.AreEqual(6, n);

            Assert.AreEqual(465, res.Delegate(76, 51, 24, 37, 43, 97, 4, 61, 25, 47));
            Assert.AreEqual(7, n);

            Assert.AreEqual(388, res.Delegate(86, 81, 9, 47, 13, 23, 91, 28, 4, 6));
            Assert.AreEqual(8, n);

        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_EqualityComparer12()
        {
            var m = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var c = EqualityComparer<int>.Default;
            var n = 0;

            var res = FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => { n++; return t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11; }, MemoizationOptions.None, c, c, c, c, c, c, c, c, c, c, c);

            Assert.AreEqual(527, res.Delegate(43, 91, 66, 2, 23, 61, 22, 27, 60, 46, 86));
            Assert.AreEqual(1, n);

            Assert.AreEqual(493, res.Delegate(55, 45, 24, 46, 56, 15, 96, 2, 47, 23, 84));
            Assert.AreEqual(2, n);

            Assert.AreEqual(439, res.Delegate(57, 31, 7, 6, 21, 77, 26, 43, 73, 10, 88));
            Assert.AreEqual(3, n);

            Assert.AreEqual(653, res.Delegate(56, 82, 85, 85, 21, 14, 81, 74, 76, 1, 78));
            Assert.AreEqual(4, n);

            Assert.AreEqual(527, res.Delegate(43, 91, 66, 2, 23, 61, 22, 27, 60, 46, 86));
            Assert.AreEqual(4, n);

            Assert.AreEqual(493, res.Delegate(55, 45, 24, 46, 56, 15, 96, 2, 47, 23, 84));
            Assert.AreEqual(4, n);

            Assert.AreEqual(439, res.Delegate(57, 31, 7, 6, 21, 77, 26, 43, 73, 10, 88));
            Assert.AreEqual(4, n);

            Assert.AreEqual(653, res.Delegate(56, 82, 85, 85, 21, 14, 81, 74, 76, 1, 78));
            Assert.AreEqual(4, n);

            res.Cache.Clear();
            Assert.AreEqual(4, n);

            Assert.AreEqual(527, res.Delegate(43, 91, 66, 2, 23, 61, 22, 27, 60, 46, 86));
            Assert.AreEqual(5, n);

            Assert.AreEqual(493, res.Delegate(55, 45, 24, 46, 56, 15, 96, 2, 47, 23, 84));
            Assert.AreEqual(6, n);

            Assert.AreEqual(439, res.Delegate(57, 31, 7, 6, 21, 77, 26, 43, 73, 10, 88));
            Assert.AreEqual(7, n);

            Assert.AreEqual(653, res.Delegate(56, 82, 85, 85, 21, 14, 81, 74, 76, 1, 78));
            Assert.AreEqual(8, n);

        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_EqualityComparer13()
        {
            var m = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var c = EqualityComparer<int>.Default;
            var n = 0;

            var res = FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => { n++; return t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12; }, MemoizationOptions.None, c, c, c, c, c, c, c, c, c, c, c, c);

            Assert.AreEqual(595, res.Delegate(60, 31, 36, 84, 1, 56, 7, 95, 26, 57, 83, 59));
            Assert.AreEqual(1, n);

            Assert.AreEqual(582, res.Delegate(33, 34, 95, 16, 40, 45, 1, 16, 72, 75, 67, 88));
            Assert.AreEqual(2, n);

            Assert.AreEqual(792, res.Delegate(42, 60, 71, 93, 82, 20, 72, 46, 82, 78, 70, 76));
            Assert.AreEqual(3, n);

            Assert.AreEqual(674, res.Delegate(69, 36, 75, 69, 35, 78, 84, 30, 73, 23, 51, 51));
            Assert.AreEqual(4, n);

            Assert.AreEqual(595, res.Delegate(60, 31, 36, 84, 1, 56, 7, 95, 26, 57, 83, 59));
            Assert.AreEqual(4, n);

            Assert.AreEqual(582, res.Delegate(33, 34, 95, 16, 40, 45, 1, 16, 72, 75, 67, 88));
            Assert.AreEqual(4, n);

            Assert.AreEqual(792, res.Delegate(42, 60, 71, 93, 82, 20, 72, 46, 82, 78, 70, 76));
            Assert.AreEqual(4, n);

            Assert.AreEqual(674, res.Delegate(69, 36, 75, 69, 35, 78, 84, 30, 73, 23, 51, 51));
            Assert.AreEqual(4, n);

            res.Cache.Clear();
            Assert.AreEqual(4, n);

            Assert.AreEqual(595, res.Delegate(60, 31, 36, 84, 1, 56, 7, 95, 26, 57, 83, 59));
            Assert.AreEqual(5, n);

            Assert.AreEqual(582, res.Delegate(33, 34, 95, 16, 40, 45, 1, 16, 72, 75, 67, 88));
            Assert.AreEqual(6, n);

            Assert.AreEqual(792, res.Delegate(42, 60, 71, 93, 82, 20, 72, 46, 82, 78, 70, 76));
            Assert.AreEqual(7, n);

            Assert.AreEqual(674, res.Delegate(69, 36, 75, 69, 35, 78, 84, 30, 73, 23, 51, 51));
            Assert.AreEqual(8, n);

        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_EqualityComparer14()
        {
            var m = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var c = EqualityComparer<int>.Default;
            var n = 0;

            var res = FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => { n++; return t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 + t13; }, MemoizationOptions.None, c, c, c, c, c, c, c, c, c, c, c, c, c);

            Assert.AreEqual(717, res.Delegate(26, 97, 40, 28, 75, 84, 6, 85, 63, 47, 41, 40, 85));
            Assert.AreEqual(1, n);

            Assert.AreEqual(515, res.Delegate(14, 12, 5, 84, 36, 76, 55, 63, 19, 47, 4, 69, 31));
            Assert.AreEqual(2, n);

            Assert.AreEqual(707, res.Delegate(81, 93, 90, 36, 15, 19, 9, 20, 67, 84, 79, 43, 71));
            Assert.AreEqual(3, n);

            Assert.AreEqual(748, res.Delegate(98, 72, 85, 12, 22, 94, 35, 84, 21, 66, 78, 45, 36));
            Assert.AreEqual(4, n);

            Assert.AreEqual(717, res.Delegate(26, 97, 40, 28, 75, 84, 6, 85, 63, 47, 41, 40, 85));
            Assert.AreEqual(4, n);

            Assert.AreEqual(515, res.Delegate(14, 12, 5, 84, 36, 76, 55, 63, 19, 47, 4, 69, 31));
            Assert.AreEqual(4, n);

            Assert.AreEqual(707, res.Delegate(81, 93, 90, 36, 15, 19, 9, 20, 67, 84, 79, 43, 71));
            Assert.AreEqual(4, n);

            Assert.AreEqual(748, res.Delegate(98, 72, 85, 12, 22, 94, 35, 84, 21, 66, 78, 45, 36));
            Assert.AreEqual(4, n);

            res.Cache.Clear();
            Assert.AreEqual(4, n);

            Assert.AreEqual(717, res.Delegate(26, 97, 40, 28, 75, 84, 6, 85, 63, 47, 41, 40, 85));
            Assert.AreEqual(5, n);

            Assert.AreEqual(515, res.Delegate(14, 12, 5, 84, 36, 76, 55, 63, 19, 47, 4, 69, 31));
            Assert.AreEqual(6, n);

            Assert.AreEqual(707, res.Delegate(81, 93, 90, 36, 15, 19, 9, 20, 67, 84, 79, 43, 71));
            Assert.AreEqual(7, n);

            Assert.AreEqual(748, res.Delegate(98, 72, 85, 12, 22, 94, 35, 84, 21, 66, 78, 45, 36));
            Assert.AreEqual(8, n);

        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_EqualityComparer15()
        {
            var m = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var c = EqualityComparer<int>.Default;
            var n = 0;

            var res = FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => { n++; return t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 + t13 + t14; }, MemoizationOptions.None, c, c, c, c, c, c, c, c, c, c, c, c, c, c);

            Assert.AreEqual(612, res.Delegate(46, 96, 87, 6, 50, 36, 59, 43, 3, 12, 94, 26, 32, 22));
            Assert.AreEqual(1, n);

            Assert.AreEqual(612, res.Delegate(30, 64, 47, 27, 26, 41, 65, 78, 82, 78, 7, 25, 9, 33));
            Assert.AreEqual(2, n);

            Assert.AreEqual(676, res.Delegate(46, 59, 27, 11, 91, 78, 72, 13, 32, 60, 34, 42, 84, 27));
            Assert.AreEqual(3, n);

            Assert.AreEqual(822, res.Delegate(94, 60, 90, 85, 89, 72, 5, 20, 74, 38, 52, 4, 71, 68));
            Assert.AreEqual(4, n);

            Assert.AreEqual(612, res.Delegate(46, 96, 87, 6, 50, 36, 59, 43, 3, 12, 94, 26, 32, 22));
            Assert.AreEqual(4, n);

            Assert.AreEqual(612, res.Delegate(30, 64, 47, 27, 26, 41, 65, 78, 82, 78, 7, 25, 9, 33));
            Assert.AreEqual(4, n);

            Assert.AreEqual(676, res.Delegate(46, 59, 27, 11, 91, 78, 72, 13, 32, 60, 34, 42, 84, 27));
            Assert.AreEqual(4, n);

            Assert.AreEqual(822, res.Delegate(94, 60, 90, 85, 89, 72, 5, 20, 74, 38, 52, 4, 71, 68));
            Assert.AreEqual(4, n);

            res.Cache.Clear();
            Assert.AreEqual(4, n);

            Assert.AreEqual(612, res.Delegate(46, 96, 87, 6, 50, 36, 59, 43, 3, 12, 94, 26, 32, 22));
            Assert.AreEqual(5, n);

            Assert.AreEqual(612, res.Delegate(30, 64, 47, 27, 26, 41, 65, 78, 82, 78, 7, 25, 9, 33));
            Assert.AreEqual(6, n);

            Assert.AreEqual(676, res.Delegate(46, 59, 27, 11, 91, 78, 72, 13, 32, 60, 34, 42, 84, 27));
            Assert.AreEqual(7, n);

            Assert.AreEqual(822, res.Delegate(94, 60, 90, 85, 89, 72, 5, 20, 74, 38, 52, 4, 71, 68));
            Assert.AreEqual(8, n);

        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_EqualityComparer16()
        {
            var m = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var c = EqualityComparer<int>.Default;
            var n = 0;

            var res = FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => { n++; return t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 + t13 + t14 + t15; }, MemoizationOptions.None, c, c, c, c, c, c, c, c, c, c, c, c, c, c, c);

            Assert.AreEqual(691, res.Delegate(13, 9, 99, 25, 27, 25, 97, 44, 85, 82, 35, 53, 49, 17, 31));
            Assert.AreEqual(1, n);

            Assert.AreEqual(868, res.Delegate(86, 93, 83, 56, 37, 83, 22, 87, 21, 35, 37, 28, 26, 85, 89));
            Assert.AreEqual(2, n);

            Assert.AreEqual(629, res.Delegate(58, 86, 6, 3, 99, 23, 60, 9, 15, 59, 30, 50, 75, 7, 49));
            Assert.AreEqual(3, n);

            Assert.AreEqual(966, res.Delegate(36, 22, 88, 88, 87, 44, 69, 47, 33, 84, 91, 21, 77, 90, 89));
            Assert.AreEqual(4, n);

            Assert.AreEqual(691, res.Delegate(13, 9, 99, 25, 27, 25, 97, 44, 85, 82, 35, 53, 49, 17, 31));
            Assert.AreEqual(4, n);

            Assert.AreEqual(868, res.Delegate(86, 93, 83, 56, 37, 83, 22, 87, 21, 35, 37, 28, 26, 85, 89));
            Assert.AreEqual(4, n);

            Assert.AreEqual(629, res.Delegate(58, 86, 6, 3, 99, 23, 60, 9, 15, 59, 30, 50, 75, 7, 49));
            Assert.AreEqual(4, n);

            Assert.AreEqual(966, res.Delegate(36, 22, 88, 88, 87, 44, 69, 47, 33, 84, 91, 21, 77, 90, 89));
            Assert.AreEqual(4, n);

            res.Cache.Clear();
            Assert.AreEqual(4, n);

            Assert.AreEqual(691, res.Delegate(13, 9, 99, 25, 27, 25, 97, 44, 85, 82, 35, 53, 49, 17, 31));
            Assert.AreEqual(5, n);

            Assert.AreEqual(868, res.Delegate(86, 93, 83, 56, 37, 83, 22, 87, 21, 35, 37, 28, 26, 85, 89));
            Assert.AreEqual(6, n);

            Assert.AreEqual(629, res.Delegate(58, 86, 6, 3, 99, 23, 60, 9, 15, 59, 30, 50, 75, 7, 49));
            Assert.AreEqual(7, n);

            Assert.AreEqual(966, res.Delegate(36, 22, 88, 88, 87, 44, 69, 47, 33, 84, 91, 21, 77, 90, 89));
            Assert.AreEqual(8, n);

        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_EqualityComparer17()
        {
            var m = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var c = EqualityComparer<int>.Default;
            var n = 0;

            var res = FunctionMemoizationExtensions.Memoize<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => { n++; return t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 + t13 + t14 + t15 + t16; }, MemoizationOptions.None, c, c, c, c, c, c, c, c, c, c, c, c, c, c, c, c);

            Assert.AreEqual(821, res.Delegate(96, 70, 58, 95, 24, 48, 46, 45, 17, 8, 26, 84, 67, 97, 7, 33));
            Assert.AreEqual(1, n);

            Assert.AreEqual(841, res.Delegate(47, 79, 72, 99, 15, 40, 38, 97, 44, 89, 39, 72, 19, 8, 1, 82));
            Assert.AreEqual(2, n);

            Assert.AreEqual(905, res.Delegate(19, 26, 62, 59, 91, 79, 83, 0, 89, 76, 70, 80, 61, 60, 1, 49));
            Assert.AreEqual(3, n);

            Assert.AreEqual(682, res.Delegate(26, 51, 44, 42, 5, 90, 74, 56, 32, 60, 51, 34, 9, 73, 26, 9));
            Assert.AreEqual(4, n);

            Assert.AreEqual(821, res.Delegate(96, 70, 58, 95, 24, 48, 46, 45, 17, 8, 26, 84, 67, 97, 7, 33));
            Assert.AreEqual(4, n);

            Assert.AreEqual(841, res.Delegate(47, 79, 72, 99, 15, 40, 38, 97, 44, 89, 39, 72, 19, 8, 1, 82));
            Assert.AreEqual(4, n);

            Assert.AreEqual(905, res.Delegate(19, 26, 62, 59, 91, 79, 83, 0, 89, 76, 70, 80, 61, 60, 1, 49));
            Assert.AreEqual(4, n);

            Assert.AreEqual(682, res.Delegate(26, 51, 44, 42, 5, 90, 74, 56, 32, 60, 51, 34, 9, 73, 26, 9));
            Assert.AreEqual(4, n);

            res.Cache.Clear();
            Assert.AreEqual(4, n);

            Assert.AreEqual(821, res.Delegate(96, 70, 58, 95, 24, 48, 46, 45, 17, 8, 26, 84, 67, 97, 7, 33));
            Assert.AreEqual(5, n);

            Assert.AreEqual(841, res.Delegate(47, 79, 72, 99, 15, 40, 38, 97, 44, 89, 39, 72, 19, 8, 1, 82));
            Assert.AreEqual(6, n);

            Assert.AreEqual(905, res.Delegate(19, 26, 62, 59, 91, 79, 83, 0, 89, 76, 70, 80, 61, 60, 1, 49));
            Assert.AreEqual(7, n);

            Assert.AreEqual(682, res.Delegate(26, 51, 44, 42, 5, 90, 74, 56, 32, 60, 51, 34, 9, 73, 26, 9));
            Assert.AreEqual(8, n);

        }


        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_ArgumentChecking()
        {
            var m = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);

            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.MemoizeWeak<string>(default(IWeakMemoizer), () => ""));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.MemoizeWeak<string>(m, default(Func<string>)));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.MemoizeWeak<string, string>(default(IWeakMemoizer), (t1) => ""));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.MemoizeWeak<string, string>(m, default(Func<string, string>)));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.MemoizeWeak<string, string, string>(default(IWeakMemoizer), (t1, t2) => ""));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.MemoizeWeak<string, string, string>(m, default(Func<string, string, string>)));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.MemoizeWeak<string, string, string, string>(default(IWeakMemoizer), (t1, t2, t3) => ""));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.MemoizeWeak<string, string, string, string>(m, default(Func<string, string, string, string>)));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.MemoizeWeak<string, string, string, string, string>(default(IWeakMemoizer), (t1, t2, t3, t4) => ""));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.MemoizeWeak<string, string, string, string, string>(m, default(Func<string, string, string, string, string>)));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.MemoizeWeak<string, string, string, string, string, string>(default(IWeakMemoizer), (t1, t2, t3, t4, t5) => ""));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.MemoizeWeak<string, string, string, string, string, string>(m, default(Func<string, string, string, string, string, string>)));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.MemoizeWeak<string, string, string, string, string, string, string>(default(IWeakMemoizer), (t1, t2, t3, t4, t5, t6) => ""));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.MemoizeWeak<string, string, string, string, string, string, string>(m, default(Func<string, string, string, string, string, string, string>)));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.MemoizeWeak<string, string, string, string, string, string, string, string>(default(IWeakMemoizer), (t1, t2, t3, t4, t5, t6, t7) => ""));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.MemoizeWeak<string, string, string, string, string, string, string, string>(m, default(Func<string, string, string, string, string, string, string, string>)));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.MemoizeWeak<string, string, string, string, string, string, string, string, string>(default(IWeakMemoizer), (t1, t2, t3, t4, t5, t6, t7, t8) => ""));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.MemoizeWeak<string, string, string, string, string, string, string, string, string>(m, default(Func<string, string, string, string, string, string, string, string, string>)));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.MemoizeWeak<string, string, string, string, string, string, string, string, string, string>(default(IWeakMemoizer), (t1, t2, t3, t4, t5, t6, t7, t8, t9) => ""));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.MemoizeWeak<string, string, string, string, string, string, string, string, string, string>(m, default(Func<string, string, string, string, string, string, string, string, string, string>)));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.MemoizeWeak<string, string, string, string, string, string, string, string, string, string, string>(default(IWeakMemoizer), (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => ""));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.MemoizeWeak<string, string, string, string, string, string, string, string, string, string, string>(m, default(Func<string, string, string, string, string, string, string, string, string, string, string>)));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.MemoizeWeak<string, string, string, string, string, string, string, string, string, string, string, string>(default(IWeakMemoizer), (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => ""));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.MemoizeWeak<string, string, string, string, string, string, string, string, string, string, string, string>(m, default(Func<string, string, string, string, string, string, string, string, string, string, string, string>)));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.MemoizeWeak<string, string, string, string, string, string, string, string, string, string, string, string, string>(default(IWeakMemoizer), (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => ""));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.MemoizeWeak<string, string, string, string, string, string, string, string, string, string, string, string, string>(m, default(Func<string, string, string, string, string, string, string, string, string, string, string, string, string>)));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.MemoizeWeak<string, string, string, string, string, string, string, string, string, string, string, string, string, string>(default(IWeakMemoizer), (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => ""));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.MemoizeWeak<string, string, string, string, string, string, string, string, string, string, string, string, string, string>(m, default(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string>)));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.MemoizeWeak<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>(default(IWeakMemoizer), (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => ""));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.MemoizeWeak<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>(m, default(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>)));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.MemoizeWeak<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>(default(IWeakMemoizer), (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => ""));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.MemoizeWeak<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>(m, default(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>)));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.MemoizeWeak<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>(default(IWeakMemoizer), (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => ""));
            Assert.ThrowsException<ArgumentNullException>(() => FunctionMemoizationExtensions.MemoizeWeak<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>(m, default(Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>)));
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak3()
        {
            var m = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var n = 0;

            var res = FunctionMemoizationExtensions.MemoizeWeak<string, string, string>(m, (t1, t2) => { n++; return t1 + t2; });

            Assert.AreEqual("barfoo", res.Delegate("bar", "foo"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("quxfoo", res.Delegate("qux", "foo"));
            Assert.AreEqual(2, n);

            Assert.AreEqual("foofoo", res.Delegate("foo", "foo"));
            Assert.AreEqual(3, n);

            Assert.AreEqual("fooqux", res.Delegate("foo", "qux"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("barfoo", res.Delegate("bar", "foo"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("quxfoo", res.Delegate("qux", "foo"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("foofoo", res.Delegate("foo", "foo"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("fooqux", res.Delegate("foo", "qux"));
            Assert.AreEqual(4, n);

            res.Cache.Clear();
            Assert.AreEqual(4, n);

            Assert.AreEqual("barfoo", res.Delegate("bar", "foo"));
            Assert.AreEqual(5, n);

            Assert.AreEqual("quxfoo", res.Delegate("qux", "foo"));
            Assert.AreEqual(6, n);

            Assert.AreEqual("foofoo", res.Delegate("foo", "foo"));
            Assert.AreEqual(7, n);

            Assert.AreEqual("fooqux", res.Delegate("foo", "qux"));
            Assert.AreEqual(8, n);

        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak4()
        {
            var m = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var n = 0;

            var res = FunctionMemoizationExtensions.MemoizeWeak<string, string, string, string>(m, (t1, t2, t3) => { n++; return t1 + t2 + t3; });

            Assert.AreEqual("bazquxbar", res.Delegate("baz", "qux", "bar"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("fooquxqux", res.Delegate("foo", "qux", "qux"));
            Assert.AreEqual(2, n);

            Assert.AreEqual("foobazbaz", res.Delegate("foo", "baz", "baz"));
            Assert.AreEqual(3, n);

            Assert.AreEqual("bazfooqux", res.Delegate("baz", "foo", "qux"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("bazquxbar", res.Delegate("baz", "qux", "bar"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("fooquxqux", res.Delegate("foo", "qux", "qux"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("foobazbaz", res.Delegate("foo", "baz", "baz"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("bazfooqux", res.Delegate("baz", "foo", "qux"));
            Assert.AreEqual(4, n);

            res.Cache.Clear();
            Assert.AreEqual(4, n);

            Assert.AreEqual("bazquxbar", res.Delegate("baz", "qux", "bar"));
            Assert.AreEqual(5, n);

            Assert.AreEqual("fooquxqux", res.Delegate("foo", "qux", "qux"));
            Assert.AreEqual(6, n);

            Assert.AreEqual("foobazbaz", res.Delegate("foo", "baz", "baz"));
            Assert.AreEqual(7, n);

            Assert.AreEqual("bazfooqux", res.Delegate("baz", "foo", "qux"));
            Assert.AreEqual(8, n);

        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak5()
        {
            var m = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var n = 0;

            var res = FunctionMemoizationExtensions.MemoizeWeak<string, string, string, string, string>(m, (t1, t2, t3, t4) => { n++; return t1 + t2 + t3 + t4; });

            Assert.AreEqual("quxquxbazfoo", res.Delegate("qux", "qux", "baz", "foo"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("quxbarfoofoo", res.Delegate("qux", "bar", "foo", "foo"));
            Assert.AreEqual(2, n);

            Assert.AreEqual("foofoobazbar", res.Delegate("foo", "foo", "baz", "bar"));
            Assert.AreEqual(3, n);

            Assert.AreEqual("fooquxquxbar", res.Delegate("foo", "qux", "qux", "bar"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("quxquxbazfoo", res.Delegate("qux", "qux", "baz", "foo"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("quxbarfoofoo", res.Delegate("qux", "bar", "foo", "foo"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("foofoobazbar", res.Delegate("foo", "foo", "baz", "bar"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("fooquxquxbar", res.Delegate("foo", "qux", "qux", "bar"));
            Assert.AreEqual(4, n);

            res.Cache.Clear();
            Assert.AreEqual(4, n);

            Assert.AreEqual("quxquxbazfoo", res.Delegate("qux", "qux", "baz", "foo"));
            Assert.AreEqual(5, n);

            Assert.AreEqual("quxbarfoofoo", res.Delegate("qux", "bar", "foo", "foo"));
            Assert.AreEqual(6, n);

            Assert.AreEqual("foofoobazbar", res.Delegate("foo", "foo", "baz", "bar"));
            Assert.AreEqual(7, n);

            Assert.AreEqual("fooquxquxbar", res.Delegate("foo", "qux", "qux", "bar"));
            Assert.AreEqual(8, n);

        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak6()
        {
            var m = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var n = 0;

            var res = FunctionMemoizationExtensions.MemoizeWeak<string, string, string, string, string, string>(m, (t1, t2, t3, t4, t5) => { n++; return t1 + t2 + t3 + t4 + t5; });

            Assert.AreEqual("bazquxbarbazbar", res.Delegate("baz", "qux", "bar", "baz", "bar"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("bazfoofoobazfoo", res.Delegate("baz", "foo", "foo", "baz", "foo"));
            Assert.AreEqual(2, n);

            Assert.AreEqual("bazbazbarquxfoo", res.Delegate("baz", "baz", "bar", "qux", "foo"));
            Assert.AreEqual(3, n);

            Assert.AreEqual("barfooquxfoofoo", res.Delegate("bar", "foo", "qux", "foo", "foo"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("bazquxbarbazbar", res.Delegate("baz", "qux", "bar", "baz", "bar"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("bazfoofoobazfoo", res.Delegate("baz", "foo", "foo", "baz", "foo"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("bazbazbarquxfoo", res.Delegate("baz", "baz", "bar", "qux", "foo"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("barfooquxfoofoo", res.Delegate("bar", "foo", "qux", "foo", "foo"));
            Assert.AreEqual(4, n);

            res.Cache.Clear();
            Assert.AreEqual(4, n);

            Assert.AreEqual("bazquxbarbazbar", res.Delegate("baz", "qux", "bar", "baz", "bar"));
            Assert.AreEqual(5, n);

            Assert.AreEqual("bazfoofoobazfoo", res.Delegate("baz", "foo", "foo", "baz", "foo"));
            Assert.AreEqual(6, n);

            Assert.AreEqual("bazbazbarquxfoo", res.Delegate("baz", "baz", "bar", "qux", "foo"));
            Assert.AreEqual(7, n);

            Assert.AreEqual("barfooquxfoofoo", res.Delegate("bar", "foo", "qux", "foo", "foo"));
            Assert.AreEqual(8, n);

        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak7()
        {
            var m = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var n = 0;

            var res = FunctionMemoizationExtensions.MemoizeWeak<string, string, string, string, string, string, string>(m, (t1, t2, t3, t4, t5, t6) => { n++; return t1 + t2 + t3 + t4 + t5 + t6; });

            Assert.AreEqual("quxfoobazfoobarbar", res.Delegate("qux", "foo", "baz", "foo", "bar", "bar"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("foofoobazbazbazbar", res.Delegate("foo", "foo", "baz", "baz", "baz", "bar"));
            Assert.AreEqual(2, n);

            Assert.AreEqual("bazbarbazfoobazqux", res.Delegate("baz", "bar", "baz", "foo", "baz", "qux"));
            Assert.AreEqual(3, n);

            Assert.AreEqual("quxbazbarfoofoobar", res.Delegate("qux", "baz", "bar", "foo", "foo", "bar"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("quxfoobazfoobarbar", res.Delegate("qux", "foo", "baz", "foo", "bar", "bar"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("foofoobazbazbazbar", res.Delegate("foo", "foo", "baz", "baz", "baz", "bar"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("bazbarbazfoobazqux", res.Delegate("baz", "bar", "baz", "foo", "baz", "qux"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("quxbazbarfoofoobar", res.Delegate("qux", "baz", "bar", "foo", "foo", "bar"));
            Assert.AreEqual(4, n);

            res.Cache.Clear();
            Assert.AreEqual(4, n);

            Assert.AreEqual("quxfoobazfoobarbar", res.Delegate("qux", "foo", "baz", "foo", "bar", "bar"));
            Assert.AreEqual(5, n);

            Assert.AreEqual("foofoobazbazbazbar", res.Delegate("foo", "foo", "baz", "baz", "baz", "bar"));
            Assert.AreEqual(6, n);

            Assert.AreEqual("bazbarbazfoobazqux", res.Delegate("baz", "bar", "baz", "foo", "baz", "qux"));
            Assert.AreEqual(7, n);

            Assert.AreEqual("quxbazbarfoofoobar", res.Delegate("qux", "baz", "bar", "foo", "foo", "bar"));
            Assert.AreEqual(8, n);

        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak8()
        {
            var m = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var n = 0;

            var res = FunctionMemoizationExtensions.MemoizeWeak<string, string, string, string, string, string, string, string>(m, (t1, t2, t3, t4, t5, t6, t7) => { n++; return t1 + t2 + t3 + t4 + t5 + t6 + t7; });

            Assert.AreEqual("barquxbarquxbarquxbaz", res.Delegate("bar", "qux", "bar", "qux", "bar", "qux", "baz"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("bazbarfooquxquxquxbaz", res.Delegate("baz", "bar", "foo", "qux", "qux", "qux", "baz"));
            Assert.AreEqual(2, n);

            Assert.AreEqual("quxbazfoobazquxbazfoo", res.Delegate("qux", "baz", "foo", "baz", "qux", "baz", "foo"));
            Assert.AreEqual(3, n);

            Assert.AreEqual("bazbazbarbazbarfooqux", res.Delegate("baz", "baz", "bar", "baz", "bar", "foo", "qux"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("barquxbarquxbarquxbaz", res.Delegate("bar", "qux", "bar", "qux", "bar", "qux", "baz"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("bazbarfooquxquxquxbaz", res.Delegate("baz", "bar", "foo", "qux", "qux", "qux", "baz"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("quxbazfoobazquxbazfoo", res.Delegate("qux", "baz", "foo", "baz", "qux", "baz", "foo"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("bazbazbarbazbarfooqux", res.Delegate("baz", "baz", "bar", "baz", "bar", "foo", "qux"));
            Assert.AreEqual(4, n);

            res.Cache.Clear();
            Assert.AreEqual(4, n);

            Assert.AreEqual("barquxbarquxbarquxbaz", res.Delegate("bar", "qux", "bar", "qux", "bar", "qux", "baz"));
            Assert.AreEqual(5, n);

            Assert.AreEqual("bazbarfooquxquxquxbaz", res.Delegate("baz", "bar", "foo", "qux", "qux", "qux", "baz"));
            Assert.AreEqual(6, n);

            Assert.AreEqual("quxbazfoobazquxbazfoo", res.Delegate("qux", "baz", "foo", "baz", "qux", "baz", "foo"));
            Assert.AreEqual(7, n);

            Assert.AreEqual("bazbazbarbazbarfooqux", res.Delegate("baz", "baz", "bar", "baz", "bar", "foo", "qux"));
            Assert.AreEqual(8, n);

        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak9()
        {
            var m = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var n = 0;

            var res = FunctionMemoizationExtensions.MemoizeWeak<string, string, string, string, string, string, string, string, string>(m, (t1, t2, t3, t4, t5, t6, t7, t8) => { n++; return t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8; });

            Assert.AreEqual("barquxfoobarbazquxbarqux", res.Delegate("bar", "qux", "foo", "bar", "baz", "qux", "bar", "qux"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("barbazfoobarbarbarbazqux", res.Delegate("bar", "baz", "foo", "bar", "bar", "bar", "baz", "qux"));
            Assert.AreEqual(2, n);

            Assert.AreEqual("quxfooquxbazbarquxquxbar", res.Delegate("qux", "foo", "qux", "baz", "bar", "qux", "qux", "bar"));
            Assert.AreEqual(3, n);

            Assert.AreEqual("bazquxquxfooquxbarbazbaz", res.Delegate("baz", "qux", "qux", "foo", "qux", "bar", "baz", "baz"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("barquxfoobarbazquxbarqux", res.Delegate("bar", "qux", "foo", "bar", "baz", "qux", "bar", "qux"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("barbazfoobarbarbarbazqux", res.Delegate("bar", "baz", "foo", "bar", "bar", "bar", "baz", "qux"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("quxfooquxbazbarquxquxbar", res.Delegate("qux", "foo", "qux", "baz", "bar", "qux", "qux", "bar"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("bazquxquxfooquxbarbazbaz", res.Delegate("baz", "qux", "qux", "foo", "qux", "bar", "baz", "baz"));
            Assert.AreEqual(4, n);

            res.Cache.Clear();
            Assert.AreEqual(4, n);

            Assert.AreEqual("barquxfoobarbazquxbarqux", res.Delegate("bar", "qux", "foo", "bar", "baz", "qux", "bar", "qux"));
            Assert.AreEqual(5, n);

            Assert.AreEqual("barbazfoobarbarbarbazqux", res.Delegate("bar", "baz", "foo", "bar", "bar", "bar", "baz", "qux"));
            Assert.AreEqual(6, n);

            Assert.AreEqual("quxfooquxbazbarquxquxbar", res.Delegate("qux", "foo", "qux", "baz", "bar", "qux", "qux", "bar"));
            Assert.AreEqual(7, n);

            Assert.AreEqual("bazquxquxfooquxbarbazbaz", res.Delegate("baz", "qux", "qux", "foo", "qux", "bar", "baz", "baz"));
            Assert.AreEqual(8, n);

        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak10()
        {
            var m = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var n = 0;

            var res = FunctionMemoizationExtensions.MemoizeWeak<string, string, string, string, string, string, string, string, string, string>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9) => { n++; return t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9; });

            Assert.AreEqual("foofoobazquxbazfooquxbazqux", res.Delegate("foo", "foo", "baz", "qux", "baz", "foo", "qux", "baz", "qux"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("bazbazquxquxfoobazfoobazfoo", res.Delegate("baz", "baz", "qux", "qux", "foo", "baz", "foo", "baz", "foo"));
            Assert.AreEqual(2, n);

            Assert.AreEqual("foofooquxquxfoofoobarbarfoo", res.Delegate("foo", "foo", "qux", "qux", "foo", "foo", "bar", "bar", "foo"));
            Assert.AreEqual(3, n);

            Assert.AreEqual("foobazquxbarbarbazquxbazqux", res.Delegate("foo", "baz", "qux", "bar", "bar", "baz", "qux", "baz", "qux"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("foofoobazquxbazfooquxbazqux", res.Delegate("foo", "foo", "baz", "qux", "baz", "foo", "qux", "baz", "qux"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("bazbazquxquxfoobazfoobazfoo", res.Delegate("baz", "baz", "qux", "qux", "foo", "baz", "foo", "baz", "foo"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("foofooquxquxfoofoobarbarfoo", res.Delegate("foo", "foo", "qux", "qux", "foo", "foo", "bar", "bar", "foo"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("foobazquxbarbarbazquxbazqux", res.Delegate("foo", "baz", "qux", "bar", "bar", "baz", "qux", "baz", "qux"));
            Assert.AreEqual(4, n);

            res.Cache.Clear();
            Assert.AreEqual(4, n);

            Assert.AreEqual("foofoobazquxbazfooquxbazqux", res.Delegate("foo", "foo", "baz", "qux", "baz", "foo", "qux", "baz", "qux"));
            Assert.AreEqual(5, n);

            Assert.AreEqual("bazbazquxquxfoobazfoobazfoo", res.Delegate("baz", "baz", "qux", "qux", "foo", "baz", "foo", "baz", "foo"));
            Assert.AreEqual(6, n);

            Assert.AreEqual("foofooquxquxfoofoobarbarfoo", res.Delegate("foo", "foo", "qux", "qux", "foo", "foo", "bar", "bar", "foo"));
            Assert.AreEqual(7, n);

            Assert.AreEqual("foobazquxbarbarbazquxbazqux", res.Delegate("foo", "baz", "qux", "bar", "bar", "baz", "qux", "baz", "qux"));
            Assert.AreEqual(8, n);

        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak11()
        {
            var m = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var n = 0;

            var res = FunctionMemoizationExtensions.MemoizeWeak<string, string, string, string, string, string, string, string, string, string, string>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => { n++; return t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10; });

            Assert.AreEqual("barfooquxfooquxquxbarfooquxbaz", res.Delegate("bar", "foo", "qux", "foo", "qux", "qux", "bar", "foo", "qux", "baz"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("quxfoobazfooquxfooquxfoobarbaz", res.Delegate("qux", "foo", "baz", "foo", "qux", "foo", "qux", "foo", "bar", "baz"));
            Assert.AreEqual(2, n);

            Assert.AreEqual("bazfoofooquxbazfoobazbazquxbaz", res.Delegate("baz", "foo", "foo", "qux", "baz", "foo", "baz", "baz", "qux", "baz"));
            Assert.AreEqual(3, n);

            Assert.AreEqual("quxbazquxbarfoofooquxbarbazbar", res.Delegate("qux", "baz", "qux", "bar", "foo", "foo", "qux", "bar", "baz", "bar"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("barfooquxfooquxquxbarfooquxbaz", res.Delegate("bar", "foo", "qux", "foo", "qux", "qux", "bar", "foo", "qux", "baz"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("quxfoobazfooquxfooquxfoobarbaz", res.Delegate("qux", "foo", "baz", "foo", "qux", "foo", "qux", "foo", "bar", "baz"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("bazfoofooquxbazfoobazbazquxbaz", res.Delegate("baz", "foo", "foo", "qux", "baz", "foo", "baz", "baz", "qux", "baz"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("quxbazquxbarfoofooquxbarbazbar", res.Delegate("qux", "baz", "qux", "bar", "foo", "foo", "qux", "bar", "baz", "bar"));
            Assert.AreEqual(4, n);

            res.Cache.Clear();
            Assert.AreEqual(4, n);

            Assert.AreEqual("barfooquxfooquxquxbarfooquxbaz", res.Delegate("bar", "foo", "qux", "foo", "qux", "qux", "bar", "foo", "qux", "baz"));
            Assert.AreEqual(5, n);

            Assert.AreEqual("quxfoobazfooquxfooquxfoobarbaz", res.Delegate("qux", "foo", "baz", "foo", "qux", "foo", "qux", "foo", "bar", "baz"));
            Assert.AreEqual(6, n);

            Assert.AreEqual("bazfoofooquxbazfoobazbazquxbaz", res.Delegate("baz", "foo", "foo", "qux", "baz", "foo", "baz", "baz", "qux", "baz"));
            Assert.AreEqual(7, n);

            Assert.AreEqual("quxbazquxbarfoofooquxbarbazbar", res.Delegate("qux", "baz", "qux", "bar", "foo", "foo", "qux", "bar", "baz", "bar"));
            Assert.AreEqual(8, n);

        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak12()
        {
            var m = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var n = 0;

            var res = FunctionMemoizationExtensions.MemoizeWeak<string, string, string, string, string, string, string, string, string, string, string, string>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => { n++; return t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11; });

            Assert.AreEqual("foobarbazbarquxbazquxquxbarbazfoo", res.Delegate("foo", "bar", "baz", "bar", "qux", "baz", "qux", "qux", "bar", "baz", "foo"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("quxfoobarbazquxbazbazfoobarquxbar", res.Delegate("qux", "foo", "bar", "baz", "qux", "baz", "baz", "foo", "bar", "qux", "bar"));
            Assert.AreEqual(2, n);

            Assert.AreEqual("quxquxfooquxquxquxbazbarquxquxfoo", res.Delegate("qux", "qux", "foo", "qux", "qux", "qux", "baz", "bar", "qux", "qux", "foo"));
            Assert.AreEqual(3, n);

            Assert.AreEqual("barfooquxquxfoobarbazbazfoobazbaz", res.Delegate("bar", "foo", "qux", "qux", "foo", "bar", "baz", "baz", "foo", "baz", "baz"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("foobarbazbarquxbazquxquxbarbazfoo", res.Delegate("foo", "bar", "baz", "bar", "qux", "baz", "qux", "qux", "bar", "baz", "foo"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("quxfoobarbazquxbazbazfoobarquxbar", res.Delegate("qux", "foo", "bar", "baz", "qux", "baz", "baz", "foo", "bar", "qux", "bar"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("quxquxfooquxquxquxbazbarquxquxfoo", res.Delegate("qux", "qux", "foo", "qux", "qux", "qux", "baz", "bar", "qux", "qux", "foo"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("barfooquxquxfoobarbazbazfoobazbaz", res.Delegate("bar", "foo", "qux", "qux", "foo", "bar", "baz", "baz", "foo", "baz", "baz"));
            Assert.AreEqual(4, n);

            res.Cache.Clear();
            Assert.AreEqual(4, n);

            Assert.AreEqual("foobarbazbarquxbazquxquxbarbazfoo", res.Delegate("foo", "bar", "baz", "bar", "qux", "baz", "qux", "qux", "bar", "baz", "foo"));
            Assert.AreEqual(5, n);

            Assert.AreEqual("quxfoobarbazquxbazbazfoobarquxbar", res.Delegate("qux", "foo", "bar", "baz", "qux", "baz", "baz", "foo", "bar", "qux", "bar"));
            Assert.AreEqual(6, n);

            Assert.AreEqual("quxquxfooquxquxquxbazbarquxquxfoo", res.Delegate("qux", "qux", "foo", "qux", "qux", "qux", "baz", "bar", "qux", "qux", "foo"));
            Assert.AreEqual(7, n);

            Assert.AreEqual("barfooquxquxfoobarbazbazfoobazbaz", res.Delegate("bar", "foo", "qux", "qux", "foo", "bar", "baz", "baz", "foo", "baz", "baz"));
            Assert.AreEqual(8, n);

        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak13()
        {
            var m = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var n = 0;

            var res = FunctionMemoizationExtensions.MemoizeWeak<string, string, string, string, string, string, string, string, string, string, string, string, string>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => { n++; return t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12; });

            Assert.AreEqual("quxbazquxquxbarquxquxbazquxquxfoobar", res.Delegate("qux", "baz", "qux", "qux", "bar", "qux", "qux", "baz", "qux", "qux", "foo", "bar"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("quxbarquxbazbarbazquxbarfoobazbarbar", res.Delegate("qux", "bar", "qux", "baz", "bar", "baz", "qux", "bar", "foo", "baz", "bar", "bar"));
            Assert.AreEqual(2, n);

            Assert.AreEqual("quxbarbarquxbazfoofoofoofooquxbarfoo", res.Delegate("qux", "bar", "bar", "qux", "baz", "foo", "foo", "foo", "foo", "qux", "bar", "foo"));
            Assert.AreEqual(3, n);

            Assert.AreEqual("barbarquxfooquxbazbarbazquxfoobarfoo", res.Delegate("bar", "bar", "qux", "foo", "qux", "baz", "bar", "baz", "qux", "foo", "bar", "foo"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("quxbazquxquxbarquxquxbazquxquxfoobar", res.Delegate("qux", "baz", "qux", "qux", "bar", "qux", "qux", "baz", "qux", "qux", "foo", "bar"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("quxbarquxbazbarbazquxbarfoobazbarbar", res.Delegate("qux", "bar", "qux", "baz", "bar", "baz", "qux", "bar", "foo", "baz", "bar", "bar"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("quxbarbarquxbazfoofoofoofooquxbarfoo", res.Delegate("qux", "bar", "bar", "qux", "baz", "foo", "foo", "foo", "foo", "qux", "bar", "foo"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("barbarquxfooquxbazbarbazquxfoobarfoo", res.Delegate("bar", "bar", "qux", "foo", "qux", "baz", "bar", "baz", "qux", "foo", "bar", "foo"));
            Assert.AreEqual(4, n);

            res.Cache.Clear();
            Assert.AreEqual(4, n);

            Assert.AreEqual("quxbazquxquxbarquxquxbazquxquxfoobar", res.Delegate("qux", "baz", "qux", "qux", "bar", "qux", "qux", "baz", "qux", "qux", "foo", "bar"));
            Assert.AreEqual(5, n);

            Assert.AreEqual("quxbarquxbazbarbazquxbarfoobazbarbar", res.Delegate("qux", "bar", "qux", "baz", "bar", "baz", "qux", "bar", "foo", "baz", "bar", "bar"));
            Assert.AreEqual(6, n);

            Assert.AreEqual("quxbarbarquxbazfoofoofoofooquxbarfoo", res.Delegate("qux", "bar", "bar", "qux", "baz", "foo", "foo", "foo", "foo", "qux", "bar", "foo"));
            Assert.AreEqual(7, n);

            Assert.AreEqual("barbarquxfooquxbazbarbazquxfoobarfoo", res.Delegate("bar", "bar", "qux", "foo", "qux", "baz", "bar", "baz", "qux", "foo", "bar", "foo"));
            Assert.AreEqual(8, n);

        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak14()
        {
            var m = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var n = 0;

            var res = FunctionMemoizationExtensions.MemoizeWeak<string, string, string, string, string, string, string, string, string, string, string, string, string, string>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => { n++; return t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 + t13; });

            Assert.AreEqual("quxbarquxbarquxbazquxquxquxfoobarbazqux", res.Delegate("qux", "bar", "qux", "bar", "qux", "baz", "qux", "qux", "qux", "foo", "bar", "baz", "qux"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("barbarbarfoobarquxbazbarfoobazbarfoofoo", res.Delegate("bar", "bar", "bar", "foo", "bar", "qux", "baz", "bar", "foo", "baz", "bar", "foo", "foo"));
            Assert.AreEqual(2, n);

            Assert.AreEqual("quxquxbazfooquxbarbarquxbazquxquxfoobaz", res.Delegate("qux", "qux", "baz", "foo", "qux", "bar", "bar", "qux", "baz", "qux", "qux", "foo", "baz"));
            Assert.AreEqual(3, n);

            Assert.AreEqual("quxbarfoobazbazbazquxbazfooquxbazquxqux", res.Delegate("qux", "bar", "foo", "baz", "baz", "baz", "qux", "baz", "foo", "qux", "baz", "qux", "qux"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("quxbarquxbarquxbazquxquxquxfoobarbazqux", res.Delegate("qux", "bar", "qux", "bar", "qux", "baz", "qux", "qux", "qux", "foo", "bar", "baz", "qux"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("barbarbarfoobarquxbazbarfoobazbarfoofoo", res.Delegate("bar", "bar", "bar", "foo", "bar", "qux", "baz", "bar", "foo", "baz", "bar", "foo", "foo"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("quxquxbazfooquxbarbarquxbazquxquxfoobaz", res.Delegate("qux", "qux", "baz", "foo", "qux", "bar", "bar", "qux", "baz", "qux", "qux", "foo", "baz"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("quxbarfoobazbazbazquxbazfooquxbazquxqux", res.Delegate("qux", "bar", "foo", "baz", "baz", "baz", "qux", "baz", "foo", "qux", "baz", "qux", "qux"));
            Assert.AreEqual(4, n);

            res.Cache.Clear();
            Assert.AreEqual(4, n);

            Assert.AreEqual("quxbarquxbarquxbazquxquxquxfoobarbazqux", res.Delegate("qux", "bar", "qux", "bar", "qux", "baz", "qux", "qux", "qux", "foo", "bar", "baz", "qux"));
            Assert.AreEqual(5, n);

            Assert.AreEqual("barbarbarfoobarquxbazbarfoobazbarfoofoo", res.Delegate("bar", "bar", "bar", "foo", "bar", "qux", "baz", "bar", "foo", "baz", "bar", "foo", "foo"));
            Assert.AreEqual(6, n);

            Assert.AreEqual("quxquxbazfooquxbarbarquxbazquxquxfoobaz", res.Delegate("qux", "qux", "baz", "foo", "qux", "bar", "bar", "qux", "baz", "qux", "qux", "foo", "baz"));
            Assert.AreEqual(7, n);

            Assert.AreEqual("quxbarfoobazbazbazquxbazfooquxbazquxqux", res.Delegate("qux", "bar", "foo", "baz", "baz", "baz", "qux", "baz", "foo", "qux", "baz", "qux", "qux"));
            Assert.AreEqual(8, n);

        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak15()
        {
            var m = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var n = 0;

            var res = FunctionMemoizationExtensions.MemoizeWeak<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => { n++; return t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 + t13 + t14; });

            Assert.AreEqual("quxbarfoofoobarfoobazbarbarbarbazfoobazbaz", res.Delegate("qux", "bar", "foo", "foo", "bar", "foo", "baz", "bar", "bar", "bar", "baz", "foo", "baz", "baz"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("bazbazbarfoofoobarfoobarbazquxfoobarfooqux", res.Delegate("baz", "baz", "bar", "foo", "foo", "bar", "foo", "bar", "baz", "qux", "foo", "bar", "foo", "qux"));
            Assert.AreEqual(2, n);

            Assert.AreEqual("quxbarbazbazbazbazquxbazfooquxfoobarfooqux", res.Delegate("qux", "bar", "baz", "baz", "baz", "baz", "qux", "baz", "foo", "qux", "foo", "bar", "foo", "qux"));
            Assert.AreEqual(3, n);

            Assert.AreEqual("foobarfoofoobazbazquxbarfoofooquxquxfooqux", res.Delegate("foo", "bar", "foo", "foo", "baz", "baz", "qux", "bar", "foo", "foo", "qux", "qux", "foo", "qux"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("quxbarfoofoobarfoobazbarbarbarbazfoobazbaz", res.Delegate("qux", "bar", "foo", "foo", "bar", "foo", "baz", "bar", "bar", "bar", "baz", "foo", "baz", "baz"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("bazbazbarfoofoobarfoobarbazquxfoobarfooqux", res.Delegate("baz", "baz", "bar", "foo", "foo", "bar", "foo", "bar", "baz", "qux", "foo", "bar", "foo", "qux"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("quxbarbazbazbazbazquxbazfooquxfoobarfooqux", res.Delegate("qux", "bar", "baz", "baz", "baz", "baz", "qux", "baz", "foo", "qux", "foo", "bar", "foo", "qux"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("foobarfoofoobazbazquxbarfoofooquxquxfooqux", res.Delegate("foo", "bar", "foo", "foo", "baz", "baz", "qux", "bar", "foo", "foo", "qux", "qux", "foo", "qux"));
            Assert.AreEqual(4, n);

            res.Cache.Clear();
            Assert.AreEqual(4, n);

            Assert.AreEqual("quxbarfoofoobarfoobazbarbarbarbazfoobazbaz", res.Delegate("qux", "bar", "foo", "foo", "bar", "foo", "baz", "bar", "bar", "bar", "baz", "foo", "baz", "baz"));
            Assert.AreEqual(5, n);

            Assert.AreEqual("bazbazbarfoofoobarfoobarbazquxfoobarfooqux", res.Delegate("baz", "baz", "bar", "foo", "foo", "bar", "foo", "bar", "baz", "qux", "foo", "bar", "foo", "qux"));
            Assert.AreEqual(6, n);

            Assert.AreEqual("quxbarbazbazbazbazquxbazfooquxfoobarfooqux", res.Delegate("qux", "bar", "baz", "baz", "baz", "baz", "qux", "baz", "foo", "qux", "foo", "bar", "foo", "qux"));
            Assert.AreEqual(7, n);

            Assert.AreEqual("foobarfoofoobazbazquxbarfoofooquxquxfooqux", res.Delegate("foo", "bar", "foo", "foo", "baz", "baz", "qux", "bar", "foo", "foo", "qux", "qux", "foo", "qux"));
            Assert.AreEqual(8, n);

        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak16()
        {
            var m = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var n = 0;

            var res = FunctionMemoizationExtensions.MemoizeWeak<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => { n++; return t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 + t13 + t14 + t15; });

            Assert.AreEqual("fooquxbazbarbarbarquxbarfoobarfoobazfoobarfoo", res.Delegate("foo", "qux", "baz", "bar", "bar", "bar", "qux", "bar", "foo", "bar", "foo", "baz", "foo", "bar", "foo"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("foobazbarquxquxquxquxfoobarbarfoobarfooquxfoo", res.Delegate("foo", "baz", "bar", "qux", "qux", "qux", "qux", "foo", "bar", "bar", "foo", "bar", "foo", "qux", "foo"));
            Assert.AreEqual(2, n);

            Assert.AreEqual("foofooquxbarfoobazbazfoobarfoobarbarbazfoobaz", res.Delegate("foo", "foo", "qux", "bar", "foo", "baz", "baz", "foo", "bar", "foo", "bar", "bar", "baz", "foo", "baz"));
            Assert.AreEqual(3, n);

            Assert.AreEqual("barfooquxquxbazquxfoobazbazbazquxfoobazbazqux", res.Delegate("bar", "foo", "qux", "qux", "baz", "qux", "foo", "baz", "baz", "baz", "qux", "foo", "baz", "baz", "qux"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("fooquxbazbarbarbarquxbarfoobarfoobazfoobarfoo", res.Delegate("foo", "qux", "baz", "bar", "bar", "bar", "qux", "bar", "foo", "bar", "foo", "baz", "foo", "bar", "foo"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("foobazbarquxquxquxquxfoobarbarfoobarfooquxfoo", res.Delegate("foo", "baz", "bar", "qux", "qux", "qux", "qux", "foo", "bar", "bar", "foo", "bar", "foo", "qux", "foo"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("foofooquxbarfoobazbazfoobarfoobarbarbazfoobaz", res.Delegate("foo", "foo", "qux", "bar", "foo", "baz", "baz", "foo", "bar", "foo", "bar", "bar", "baz", "foo", "baz"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("barfooquxquxbazquxfoobazbazbazquxfoobazbazqux", res.Delegate("bar", "foo", "qux", "qux", "baz", "qux", "foo", "baz", "baz", "baz", "qux", "foo", "baz", "baz", "qux"));
            Assert.AreEqual(4, n);

            res.Cache.Clear();
            Assert.AreEqual(4, n);

            Assert.AreEqual("fooquxbazbarbarbarquxbarfoobarfoobazfoobarfoo", res.Delegate("foo", "qux", "baz", "bar", "bar", "bar", "qux", "bar", "foo", "bar", "foo", "baz", "foo", "bar", "foo"));
            Assert.AreEqual(5, n);

            Assert.AreEqual("foobazbarquxquxquxquxfoobarbarfoobarfooquxfoo", res.Delegate("foo", "baz", "bar", "qux", "qux", "qux", "qux", "foo", "bar", "bar", "foo", "bar", "foo", "qux", "foo"));
            Assert.AreEqual(6, n);

            Assert.AreEqual("foofooquxbarfoobazbazfoobarfoobarbarbazfoobaz", res.Delegate("foo", "foo", "qux", "bar", "foo", "baz", "baz", "foo", "bar", "foo", "bar", "bar", "baz", "foo", "baz"));
            Assert.AreEqual(7, n);

            Assert.AreEqual("barfooquxquxbazquxfoobazbazbazquxfoobazbazqux", res.Delegate("bar", "foo", "qux", "qux", "baz", "qux", "foo", "baz", "baz", "baz", "qux", "foo", "baz", "baz", "qux"));
            Assert.AreEqual(8, n);

        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak17()
        {
            var m = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var n = 0;

            var res = FunctionMemoizationExtensions.MemoizeWeak<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>(m, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => { n++; return t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 + t13 + t14 + t15 + t16; });

            Assert.AreEqual("barbarquxbazbazbarfoofooquxquxquxfoobarfooquxqux", res.Delegate("bar", "bar", "qux", "baz", "baz", "bar", "foo", "foo", "qux", "qux", "qux", "foo", "bar", "foo", "qux", "qux"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("bazbarbarbarbarquxbazquxbazbazfoobazbarquxquxbaz", res.Delegate("baz", "bar", "bar", "bar", "bar", "qux", "baz", "qux", "baz", "baz", "foo", "baz", "bar", "qux", "qux", "baz"));
            Assert.AreEqual(2, n);

            Assert.AreEqual("foofoofoobazquxbazfoobazquxbazbazbarbarbarfooqux", res.Delegate("foo", "foo", "foo", "baz", "qux", "baz", "foo", "baz", "qux", "baz", "baz", "bar", "bar", "bar", "foo", "qux"));
            Assert.AreEqual(3, n);

            Assert.AreEqual("barbarquxbarbazbazquxfoofoobazbarbazbazquxbarbaz", res.Delegate("bar", "bar", "qux", "bar", "baz", "baz", "qux", "foo", "foo", "baz", "bar", "baz", "baz", "qux", "bar", "baz"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("barbarquxbazbazbarfoofooquxquxquxfoobarfooquxqux", res.Delegate("bar", "bar", "qux", "baz", "baz", "bar", "foo", "foo", "qux", "qux", "qux", "foo", "bar", "foo", "qux", "qux"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("bazbarbarbarbarquxbazquxbazbazfoobazbarquxquxbaz", res.Delegate("baz", "bar", "bar", "bar", "bar", "qux", "baz", "qux", "baz", "baz", "foo", "baz", "bar", "qux", "qux", "baz"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("foofoofoobazquxbazfoobazquxbazbazbarbarbarfooqux", res.Delegate("foo", "foo", "foo", "baz", "qux", "baz", "foo", "baz", "qux", "baz", "baz", "bar", "bar", "bar", "foo", "qux"));
            Assert.AreEqual(4, n);

            Assert.AreEqual("barbarquxbarbazbazquxfoofoobazbarbazbazquxbarbaz", res.Delegate("bar", "bar", "qux", "bar", "baz", "baz", "qux", "foo", "foo", "baz", "bar", "baz", "baz", "qux", "bar", "baz"));
            Assert.AreEqual(4, n);

            res.Cache.Clear();
            Assert.AreEqual(4, n);

            Assert.AreEqual("barbarquxbazbazbarfoofooquxquxquxfoobarfooquxqux", res.Delegate("bar", "bar", "qux", "baz", "baz", "bar", "foo", "foo", "qux", "qux", "qux", "foo", "bar", "foo", "qux", "qux"));
            Assert.AreEqual(5, n);

            Assert.AreEqual("bazbarbarbarbarquxbazquxbazbazfoobazbarquxquxbaz", res.Delegate("baz", "bar", "bar", "bar", "bar", "qux", "baz", "qux", "baz", "baz", "foo", "baz", "bar", "qux", "qux", "baz"));
            Assert.AreEqual(6, n);

            Assert.AreEqual("foofoofoobazquxbazfoobazquxbazbazbarbarbarfooqux", res.Delegate("foo", "foo", "foo", "baz", "qux", "baz", "foo", "baz", "qux", "baz", "baz", "bar", "bar", "bar", "foo", "qux"));
            Assert.AreEqual(7, n);

            Assert.AreEqual("barbarquxbarbazbazquxfoofoobazbarbazbazquxbarbaz", res.Delegate("bar", "bar", "qux", "bar", "baz", "baz", "qux", "foo", "foo", "baz", "bar", "baz", "baz", "qux", "bar", "baz"));
            Assert.AreEqual(8, n);

        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_TDelegate_Func0()
        {
            var n = 0;

            var mem = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var f = new Func<int>(() => { n++; return 42; });

            var res = mem.Memoize<Func<int>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual(42, res.Delegate());
            Assert.AreEqual(1, n);

            Assert.AreEqual(42, res.Delegate());
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_TDelegate_Func1()
        {
            var n = 0;

            var mem = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var f = new Func<int, int>((p0) => { n++; return 42 + p0; });

            var res = mem.Memoize<Func<int, int>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual(55, res.Delegate(13));
            Assert.AreEqual(1, n);

            Assert.AreEqual(55, res.Delegate(13));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_TDelegate_Func2()
        {
            var n = 0;

            var mem = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int>((p0, p1) => { n++; return 42 + p0 + p1; });

            var res = mem.Memoize<Func<int, int, int>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual(130, res.Delegate(81, 7));
            Assert.AreEqual(1, n);

            Assert.AreEqual(130, res.Delegate(81, 7));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_TDelegate_Func3()
        {
            var n = 0;

            var mem = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int>((p0, p1, p2) => { n++; return 42 + p0 + p1 + p2; });

            var res = mem.Memoize<Func<int, int, int, int>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual(155, res.Delegate(59, 0, 54));
            Assert.AreEqual(1, n);

            Assert.AreEqual(155, res.Delegate(59, 0, 54));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_TDelegate_Func4()
        {
            var n = 0;

            var mem = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int, int>((p0, p1, p2, p3) => { n++; return 42 + p0 + p1 + p2 + p3; });

            var res = mem.Memoize<Func<int, int, int, int, int>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual(207, res.Delegate(79, 3, 58, 25));
            Assert.AreEqual(1, n);

            Assert.AreEqual(207, res.Delegate(79, 3, 58, 25));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_TDelegate_Func5()
        {
            var n = 0;

            var mem = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int, int, int>((p0, p1, p2, p3, p4) => { n++; return 42 + p0 + p1 + p2 + p3 + p4; });

            var res = mem.Memoize<Func<int, int, int, int, int, int>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual(292, res.Delegate(37, 6, 74, 82, 51));
            Assert.AreEqual(1, n);

            Assert.AreEqual(292, res.Delegate(37, 6, 74, 82, 51));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_TDelegate_Func6()
        {
            var n = 0;

            var mem = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int, int, int, int>((p0, p1, p2, p3, p4, p5) => { n++; return 42 + p0 + p1 + p2 + p3 + p4 + p5; });

            var res = mem.Memoize<Func<int, int, int, int, int, int, int>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual(273, res.Delegate(74, 47, 59, 51, 0, 0));
            Assert.AreEqual(1, n);

            Assert.AreEqual(273, res.Delegate(74, 47, 59, 51, 0, 0));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_TDelegate_Func7()
        {
            var n = 0;

            var mem = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int, int, int, int, int>((p0, p1, p2, p3, p4, p5, p6) => { n++; return 42 + p0 + p1 + p2 + p3 + p4 + p5 + p6; });

            var res = mem.Memoize<Func<int, int, int, int, int, int, int, int>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual(444, res.Delegate(29, 8, 45, 79, 79, 68, 94));
            Assert.AreEqual(1, n);

            Assert.AreEqual(444, res.Delegate(29, 8, 45, 79, 79, 68, 94));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_TDelegate_Func8()
        {
            var n = 0;

            var mem = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int, int, int, int, int, int>((p0, p1, p2, p3, p4, p5, p6, p7) => { n++; return 42 + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7; });

            var res = mem.Memoize<Func<int, int, int, int, int, int, int, int, int>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual(559, res.Delegate(87, 55, 85, 0, 78, 86, 87, 39));
            Assert.AreEqual(1, n);

            Assert.AreEqual(559, res.Delegate(87, 55, 85, 0, 78, 86, 87, 39));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_TDelegate_Func9()
        {
            var n = 0;

            var mem = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int, int, int, int, int, int, int>((p0, p1, p2, p3, p4, p5, p6, p7, p8) => { n++; return 42 + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8; });

            var res = mem.Memoize<Func<int, int, int, int, int, int, int, int, int, int>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual(551, res.Delegate(1, 80, 70, 70, 45, 68, 63, 56, 56));
            Assert.AreEqual(1, n);

            Assert.AreEqual(551, res.Delegate(1, 80, 70, 70, 45, 68, 63, 56, 56));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_TDelegate_Func10()
        {
            var n = 0;

            var mem = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int, int, int, int, int, int, int, int>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9) => { n++; return 42 + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9; });

            var res = mem.Memoize<Func<int, int, int, int, int, int, int, int, int, int, int>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual(475, res.Delegate(56, 72, 46, 26, 30, 43, 31, 14, 19, 96));
            Assert.AreEqual(1, n);

            Assert.AreEqual(475, res.Delegate(56, 72, 46, 26, 30, 43, 31, 14, 19, 96));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_TDelegate_Func11()
        {
            var n = 0;

            var mem = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int, int, int, int, int, int, int, int, int>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10) => { n++; return 42 + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10; });

            var res = mem.Memoize<Func<int, int, int, int, int, int, int, int, int, int, int, int>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual(623, res.Delegate(84, 73, 61, 80, 20, 85, 84, 16, 2, 40, 36));
            Assert.AreEqual(1, n);

            Assert.AreEqual(623, res.Delegate(84, 73, 61, 80, 20, 85, 84, 16, 2, 40, 36));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_TDelegate_Func12()
        {
            var n = 0;

            var mem = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int, int, int, int, int, int, int, int, int, int>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11) => { n++; return 42 + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11; });

            var res = mem.Memoize<Func<int, int, int, int, int, int, int, int, int, int, int, int, int>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual(774, res.Delegate(28, 88, 94, 11, 72, 67, 89, 81, 54, 32, 65, 51));
            Assert.AreEqual(1, n);

            Assert.AreEqual(774, res.Delegate(28, 88, 94, 11, 72, 67, 89, 81, 54, 32, 65, 51));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_TDelegate_Func13()
        {
            var n = 0;

            var mem = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12) => { n++; return 42 + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12; });

            var res = mem.Memoize<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual(674, res.Delegate(88, 23, 7, 22, 68, 56, 12, 53, 86, 58, 90, 3, 66));
            Assert.AreEqual(1, n);

            Assert.AreEqual(674, res.Delegate(88, 23, 7, 22, 68, 56, 12, 53, 86, 58, 90, 3, 66));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_TDelegate_Func14()
        {
            var n = 0;

            var mem = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13) => { n++; return 42 + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13; });

            var res = mem.Memoize<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual(657, res.Delegate(39, 0, 50, 84, 61, 51, 60, 15, 20, 27, 84, 52, 14, 58));
            Assert.AreEqual(1, n);

            Assert.AreEqual(657, res.Delegate(39, 0, 50, 84, 61, 51, 60, 15, 20, 27, 84, 52, 14, 58));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_TDelegate_Func15()
        {
            var n = 0;

            var mem = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14) => { n++; return 42 + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14; });

            var res = mem.Memoize<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual(814, res.Delegate(76, 42, 32, 64, 63, 18, 22, 72, 56, 12, 63, 15, 60, 90, 87));
            Assert.AreEqual(1, n);

            Assert.AreEqual(814, res.Delegate(76, 42, 32, 64, 63, 18, 22, 72, 56, 12, 63, 15, 60, 90, 87));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_TDelegate_Func16()
        {
            var n = 0;

            var mem = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15) => { n++; return 42 + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15; });

            var res = mem.Memoize<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual(1024, res.Delegate(49, 69, 97, 90, 45, 32, 66, 39, 96, 93, 80, 5, 35, 68, 95, 23));
            Assert.AreEqual(1, n);

            Assert.AreEqual(1024, res.Delegate(49, 69, 97, 90, 45, 32, 66, 39, 96, 93, 80, 5, 35, 68, 95, 23));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_TDelegate_Func17()
        {
            var n = 0;

            var mem = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16) => { n++; return 42 + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16; });

            var res = mem.Memoize<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual(983, res.Delegate(69, 54, 98, 35, 10, 54, 93, 26, 85, 43, 67, 43, 37, 21, 46, 90, 70));
            Assert.AreEqual(1, n);

            Assert.AreEqual(983, res.Delegate(69, 54, 98, 35, 10, 54, 93, 26, 85, 43, 67, 43, 37, 21, 46, 90, 70));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_TDelegate_Func18()
        {
            var n = 0;

            var mem = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17) => { n++; return 42 + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17; });

            var res = mem.Memoize<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual(984, res.Delegate(28, 70, 58, 86, 61, 68, 25, 9, 3, 36, 71, 83, 13, 86, 4, 60, 88, 93));
            Assert.AreEqual(1, n);

            Assert.AreEqual(984, res.Delegate(28, 70, 58, 86, 61, 68, 25, 9, 3, 36, 71, 83, 13, 86, 4, 60, 88, 93));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_TDelegate_Func19()
        {
            var n = 0;

            var mem = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18) => { n++; return 42 + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18; });

            var res = mem.Memoize<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual(1017, res.Delegate(61, 62, 54, 77, 95, 75, 71, 5, 1, 65, 22, 1, 75, 47, 89, 34, 7, 97, 37));
            Assert.AreEqual(1, n);

            Assert.AreEqual(1017, res.Delegate(61, 62, 54, 77, 95, 75, 71, 5, 1, 65, 22, 1, 75, 47, 89, 34, 7, 97, 37));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_TDelegate_Func20()
        {
            var n = 0;

            var mem = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19) => { n++; return 42 + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19; });

            var res = mem.Memoize<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual(960, res.Delegate(37, 8, 85, 72, 26, 6, 17, 22, 43, 72, 57, 62, 83, 48, 27, 84, 28, 16, 50, 75));
            Assert.AreEqual(1, n);

            Assert.AreEqual(960, res.Delegate(37, 8, 85, 72, 26, 6, 17, 22, 43, 72, 57, 62, 83, 48, 27, 84, 28, 16, 50, 75));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_TDelegate_Func21()
        {
            var n = 0;

            var mem = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20) => { n++; return 42 + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20; });

            var res = mem.Memoize<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual(1245, res.Delegate(82, 14, 55, 66, 60, 86, 2, 60, 24, 93, 78, 79, 7, 23, 51, 84, 76, 89, 27, 70, 77));
            Assert.AreEqual(1, n);

            Assert.AreEqual(1245, res.Delegate(82, 14, 55, 66, 60, 86, 2, 60, 24, 93, 78, 79, 7, 23, 51, 84, 76, 89, 27, 70, 77));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_TDelegate_Func22()
        {
            var n = 0;

            var mem = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21) => { n++; return 42 + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21; });

            var res = mem.Memoize<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual(914, res.Delegate(53, 28, 33, 44, 2, 39, 53, 47, 62, 61, 17, 56, 21, 55, 22, 53, 18, 12, 39, 4, 56, 97));
            Assert.AreEqual(1, n);

            Assert.AreEqual(914, res.Delegate(53, 28, 33, 44, 2, 39, 53, 47, 62, 61, 17, 56, 21, 55, 22, 53, 18, 12, 39, 4, 56, 97));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_TDelegate_Func23()
        {
            var n = 0;

            var mem = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22) => { n++; return 42 + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22; });

            var res = mem.Memoize<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual(1300, res.Delegate(50, 93, 77, 55, 59, 96, 42, 8, 39, 88, 80, 97, 28, 86, 21, 22, 57, 46, 49, 12, 62, 31, 60));
            Assert.AreEqual(1, n);

            Assert.AreEqual(1300, res.Delegate(50, 93, 77, 55, 59, 96, 42, 8, 39, 88, 80, 97, 28, 86, 21, 22, 57, 46, 49, 12, 62, 31, 60));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_TDelegate_Func24()
        {
            var n = 0;

            var mem = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23) => { n++; return 42 + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23; });

            var res = mem.Memoize<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual(1237, res.Delegate(23, 85, 68, 28, 31, 57, 76, 88, 65, 20, 55, 77, 40, 66, 46, 80, 56, 5, 53, 22, 29, 76, 23, 26));
            Assert.AreEqual(1, n);

            Assert.AreEqual(1237, res.Delegate(23, 85, 68, 28, 31, 57, 76, 88, 65, 20, 55, 77, 40, 66, 46, 80, 56, 5, 53, 22, 29, 76, 23, 26));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_TDelegate_Func25()
        {
            var n = 0;

            var mem = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24) => { n++; return 42 + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23 + p24; });

            var res = mem.Memoize<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual(1259, res.Delegate(36, 32, 95, 54, 92, 55, 44, 35, 18, 32, 54, 70, 91, 68, 11, 51, 62, 0, 14, 77, 72, 8, 80, 55, 11));
            Assert.AreEqual(1, n);

            Assert.AreEqual(1259, res.Delegate(36, 32, 95, 54, 92, 55, 44, 35, 18, 32, 54, 70, 91, 68, 11, 51, 62, 0, 14, 77, 72, 8, 80, 55, 11));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_TDelegate_Func26()
        {
            var n = 0;

            var mem = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25) => { n++; return 42 + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23 + p24 + p25; });

            var res = mem.Memoize<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual(1362, res.Delegate(66, 93, 7, 8, 9, 31, 46, 61, 41, 92, 98, 61, 22, 95, 10, 76, 20, 58, 8, 11, 76, 89, 87, 93, 2, 60));
            Assert.AreEqual(1, n);

            Assert.AreEqual(1362, res.Delegate(66, 93, 7, 8, 9, 31, 46, 61, 41, 92, 98, 61, 22, 95, 10, 76, 20, 58, 8, 11, 76, 89, 87, 93, 2, 60));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_TDelegate_Func27()
        {
            var n = 0;

            var mem = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25, p26) => { n++; return 42 + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23 + p24 + p25 + p26; });

            var res = mem.Memoize<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual(1013, res.Delegate(28, 61, 46, 53, 27, 51, 39, 43, 26, 62, 36, 27, 9, 0, 8, 9, 49, 75, 12, 89, 39, 5, 3, 0, 52, 50, 72));
            Assert.AreEqual(1, n);

            Assert.AreEqual(1013, res.Delegate(28, 61, 46, 53, 27, 51, 39, 43, 26, 62, 36, 27, 9, 0, 8, 9, 49, 75, 12, 89, 39, 5, 3, 0, 52, 50, 72));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_TDelegate_Func28()
        {
            var n = 0;

            var mem = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25, p26, p27) => { n++; return 42 + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23 + p24 + p25 + p26 + p27; });

            var res = mem.Memoize<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual(1208, res.Delegate(44, 35, 77, 5, 13, 5, 48, 3, 85, 14, 88, 65, 47, 21, 78, 69, 48, 39, 92, 49, 7, 3, 66, 40, 12, 80, 13, 20));
            Assert.AreEqual(1, n);

            Assert.AreEqual(1208, res.Delegate(44, 35, 77, 5, 13, 5, 48, 3, 85, 14, 88, 65, 47, 21, 78, 69, 48, 39, 92, 49, 7, 3, 66, 40, 12, 80, 13, 20));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_TDelegate_Func29()
        {
            var n = 0;

            var mem = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25, p26, p27, p28) => { n++; return 42 + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23 + p24 + p25 + p26 + p27 + p28; });

            var res = mem.Memoize<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual(1494, res.Delegate(23, 58, 45, 1, 77, 78, 95, 8, 48, 57, 22, 22, 60, 97, 23, 94, 60, 9, 65, 67, 61, 36, 55, 60, 59, 0, 64, 40, 68));
            Assert.AreEqual(1, n);

            Assert.AreEqual(1494, res.Delegate(23, 58, 45, 1, 77, 78, 95, 8, 48, 57, 22, 22, 60, 97, 23, 94, 60, 9, 65, 67, 61, 36, 55, 60, 59, 0, 64, 40, 68));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_TDelegate_Func30()
        {
            var n = 0;

            var mem = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25, p26, p27, p28, p29) => { n++; return 42 + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23 + p24 + p25 + p26 + p27 + p28 + p29; });

            var res = mem.Memoize<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual(1697, res.Delegate(37, 92, 32, 92, 28, 79, 26, 68, 86, 88, 68, 26, 70, 20, 90, 17, 70, 88, 10, 80, 72, 79, 3, 15, 45, 59, 87, 2, 85, 41));
            Assert.AreEqual(1, n);

            Assert.AreEqual(1697, res.Delegate(37, 92, 32, 92, 28, 79, 26, 68, 86, 88, 68, 26, 70, 20, 90, 17, 70, 88, 10, 80, 72, 79, 3, 15, 45, 59, 87, 2, 85, 41));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_TDelegate_Func31()
        {
            var n = 0;

            var mem = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25, p26, p27, p28, p29, p30) => { n++; return 42 + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23 + p24 + p25 + p26 + p27 + p28 + p29 + p30; });

            var res = mem.Memoize<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual(1714, res.Delegate(76, 14, 54, 40, 11, 64, 89, 30, 32, 17, 96, 25, 73, 20, 97, 41, 91, 15, 65, 43, 89, 11, 54, 60, 95, 58, 88, 17, 46, 69, 92));
            Assert.AreEqual(1, n);

            Assert.AreEqual(1714, res.Delegate(76, 14, 54, 40, 11, 64, 89, 30, 32, 17, 96, 25, 73, 20, 97, 41, 91, 15, 65, 43, 89, 11, 54, 60, 95, 58, 88, 17, 46, 69, 92));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_TDelegate_Func32()
        {
            var n = 0;

            var mem = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25, p26, p27, p28, p29, p30, p31) => { n++; return 42 + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23 + p24 + p25 + p26 + p27 + p28 + p29 + p30 + p31; });

            var res = mem.Memoize<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual(1660, res.Delegate(24, 83, 45, 12, 54, 71, 29, 8, 25, 27, 40, 56, 93, 84, 47, 5, 82, 18, 3, 67, 71, 37, 41, 52, 64, 59, 94, 44, 53, 75, 72, 83));
            Assert.AreEqual(1, n);

            Assert.AreEqual(1660, res.Delegate(24, 83, 45, 12, 54, 71, 29, 8, 25, 27, 40, 56, 93, 84, 47, 5, 82, 18, 3, 67, 71, 37, 41, 52, 64, 59, 94, 44, 53, 75, 72, 83));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_Memoize_TDelegate_Func33()
        {
            var n = 0;

            var mem = Memoizer.Create(MemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25, p26, p27, p28, p29, p30, p31, p32) => { n++; return 42 + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23 + p24 + p25 + p26 + p27 + p28 + p29 + p30 + p31 + p32; });

            var res = mem.Memoize<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual(1698, res.Delegate(62, 24, 71, 41, 28, 8, 42, 70, 61, 7, 39, 16, 48, 54, 61, 75, 47, 52, 6, 99, 42, 1, 21, 87, 42, 92, 47, 94, 77, 84, 54, 49, 55));
            Assert.AreEqual(1, n);

            Assert.AreEqual(1698, res.Delegate(62, 24, 71, 41, 28, 8, 42, 70, 61, 7, 39, 16, 48, 54, 61, 75, 47, 52, 6, 99, 42, 1, 21, 87, 42, 92, 47, 94, 77, 84, 54, 49, 55));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Func0()
        {
            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string>(() => { n++; return ""; });

            var res = mem.MemoizeWeak<Func<string>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual("", res.Delegate());
            Assert.AreEqual(1, n);

            Assert.AreEqual("", res.Delegate());
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Lifetime_Func0()
        {
            lock (typeof(Obj))
            {
                Obj.Reset();
                try
                {
                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static void Do()
                    {
                        var n = 0;

                        var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
                        var f = new Func<string>(() => { n++; return ""; });

                        var res = mem.MemoizeWeak<Func<string>>(f, MemoizationOptions.None);

                        Assert.AreEqual(0, n);

                        Assert.AreEqual("", Apply(res.Delegate));
                        Assert.AreEqual(1, n);
                    }

                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static string Apply(Func<string> f)
                    {
                        return f();
                    }

                    Do();

                    CollectAndCheckFinalizeCount(0);
                }
                finally
                {
                    Obj.Reset();
                }
            }
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed1_Func0()
        {
            // 1 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string>(() => { n++; return ""; });

            var res = mem.MemoizeWeak<Func<string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("", res.Delegate());
            Assert.AreEqual(1, n);

            Assert.AreEqual("", res.Delegate());
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed2_Func0()
        {
            // 1 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string>(() => { n++; return ""; });

            var res = mem.MemoizeWeak<Func<string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("", res.Delegate());
            Assert.AreEqual(1, n);

            Assert.AreEqual("", res.Delegate());
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed3_Func0()
        {
            // 1 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string>(() => { n++; return ""; });

            var res = mem.MemoizeWeak<Func<string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("", res.Delegate());
            Assert.AreEqual(1, n);

            Assert.AreEqual("", res.Delegate());
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Func1()
        {
            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, string>((p0) => { n++; return "" + p0; });

            var res = mem.MemoizeWeak<Func<string, string>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual("56", res.Delegate("56"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("56", res.Delegate("56"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Lifetime_Func1()
        {
            lock (typeof(Obj))
            {
                Obj.Reset();
                try
                {
                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static void Do()
                    {
                        var n = 0;

                        var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
                        var f = new Func<Obj, string>((p0) => { n++; return ""; });

                        var res = mem.MemoizeWeak<Func<Obj, string>>(f, MemoizationOptions.None);

                        Assert.AreEqual(0, n);

                        Assert.AreEqual("", Apply(res.Delegate));
                        Assert.AreEqual(1, n);
                    }

                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static string Apply(Func<Obj, string> f)
                    {
                        return f(new Obj());
                    }

                    Do();

                    CollectAndCheckFinalizeCount(1);
                }
                finally
                {
                    Obj.Reset();
                }
            }
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed1_Func1()
        {
            // 2 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, string>((p0) => { n++; return "" + p0; });

            var res = mem.MemoizeWeak<Func<string, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("13", res.Delegate("13"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("13", res.Delegate("13"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed2_Func1()
        {
            // 2 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, string>((p0) => { n++; return "" + p0; });

            var res = mem.MemoizeWeak<Func<string, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("76", res.Delegate("76"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("76", res.Delegate("76"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed3_Func1()
        {
            // 1 int; 1 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, string>((p0) => { n++; return "" + p0; });

            var res = mem.MemoizeWeak<Func<int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("75", res.Delegate(75));
            Assert.AreEqual(1, n);

            Assert.AreEqual("75", res.Delegate(75));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Func2()
        {
            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, string, string>((p0, p1) => { n++; return "" + p0 + p1; });

            var res = mem.MemoizeWeak<Func<string, string, string>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual("336", res.Delegate("33", "6"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("336", res.Delegate("33", "6"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Lifetime_Func2()
        {
            lock (typeof(Obj))
            {
                Obj.Reset();
                try
                {
                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static void Do()
                    {
                        var n = 0;

                        var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
                        var f = new Func<Obj, Obj, string>((p0, p1) => { n++; return ""; });

                        var res = mem.MemoizeWeak<Func<Obj, Obj, string>>(f, MemoizationOptions.None);

                        Assert.AreEqual(0, n);

                        Assert.AreEqual("", Apply(res.Delegate));
                        Assert.AreEqual(1, n);
                    }

                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static string Apply(Func<Obj, Obj, string> f)
                    {
                        return f(new Obj(), new Obj());
                    }

                    Do();

                    CollectAndCheckFinalizeCount(2);
                }
                finally
                {
                    Obj.Reset();
                }
            }
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed1_Func2()
        {
            // 2 int; 1 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, string>((p0, p1) => { n++; return "" + p0 + p1; });

            var res = mem.MemoizeWeak<Func<int, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("1097", res.Delegate(10, 97));
            Assert.AreEqual(1, n);

            Assert.AreEqual("1097", res.Delegate(10, 97));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed2_Func2()
        {
            // 3 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, string, string>((p0, p1) => { n++; return "" + p0 + p1; });

            var res = mem.MemoizeWeak<Func<string, string, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("069", res.Delegate("0", "69"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("069", res.Delegate("0", "69"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed3_Func2()
        {
            // 2 int; 1 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, string>((p0, p1) => { n++; return "" + p0 + p1; });

            var res = mem.MemoizeWeak<Func<int, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("613", res.Delegate(61, 3));
            Assert.AreEqual(1, n);

            Assert.AreEqual("613", res.Delegate(61, 3));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Func3()
        {
            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, string, string, string>((p0, p1, p2) => { n++; return "" + p0 + p1 + p2; });

            var res = mem.MemoizeWeak<Func<string, string, string, string>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual("839935", res.Delegate("83", "99", "35"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("839935", res.Delegate("83", "99", "35"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Lifetime_Func3()
        {
            lock (typeof(Obj))
            {
                Obj.Reset();
                try
                {
                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static void Do()
                    {
                        var n = 0;

                        var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
                        var f = new Func<Obj, Obj, Obj, string>((p0, p1, p2) => { n++; return ""; });

                        var res = mem.MemoizeWeak<Func<Obj, Obj, Obj, string>>(f, MemoizationOptions.None);

                        Assert.AreEqual(0, n);

                        Assert.AreEqual("", Apply(res.Delegate));
                        Assert.AreEqual(1, n);
                    }

                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static string Apply(Func<Obj, Obj, Obj, string> f)
                    {
                        return f(new Obj(), new Obj(), new Obj());
                    }

                    Do();

                    CollectAndCheckFinalizeCount(3);
                }
                finally
                {
                    Obj.Reset();
                }
            }
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed1_Func3()
        {
            // 2 string; 2 int

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, int, int, string>((p0, p1, p2) => { n++; return "" + p0 + p1 + p2; });

            var res = mem.MemoizeWeak<Func<string, int, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("609260", res.Delegate("60", 92, 60));
            Assert.AreEqual(1, n);

            Assert.AreEqual("609260", res.Delegate("60", 92, 60));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed2_Func3()
        {
            // 2 int; 2 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, string, string>((p0, p1, p2) => { n++; return "" + p0 + p1 + p2; });

            var res = mem.MemoizeWeak<Func<int, int, string, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("763124", res.Delegate(76, 31, "24"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("763124", res.Delegate(76, 31, "24"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed3_Func3()
        {
            // 3 string; 1 int

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, string, int, string>((p0, p1, p2) => { n++; return "" + p0 + p1 + p2; });

            var res = mem.MemoizeWeak<Func<string, string, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("891689", res.Delegate("89", "16", 89));
            Assert.AreEqual(1, n);

            Assert.AreEqual("891689", res.Delegate("89", "16", 89));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Func4()
        {
            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, string, string, string, string>((p0, p1, p2, p3) => { n++; return "" + p0 + p1 + p2 + p3; });

            var res = mem.MemoizeWeak<Func<string, string, string, string, string>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual("37975887", res.Delegate("37", "97", "58", "87"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("37975887", res.Delegate("37", "97", "58", "87"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Lifetime_Func4()
        {
            lock (typeof(Obj))
            {
                Obj.Reset();
                try
                {
                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static void Do()
                    {
                        var n = 0;

                        var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
                        var f = new Func<Obj, Obj, Obj, Obj, string>((p0, p1, p2, p3) => { n++; return ""; });

                        var res = mem.MemoizeWeak<Func<Obj, Obj, Obj, Obj, string>>(f, MemoizationOptions.None);

                        Assert.AreEqual(0, n);

                        Assert.AreEqual("", Apply(res.Delegate));
                        Assert.AreEqual(1, n);
                    }

                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static string Apply(Func<Obj, Obj, Obj, Obj, string> f)
                    {
                        return f(new Obj(), new Obj(), new Obj(), new Obj());
                    }

                    Do();

                    CollectAndCheckFinalizeCount(4);
                }
                finally
                {
                    Obj.Reset();
                }
            }
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed1_Func4()
        {
            // 4 int; 1 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int, string>((p0, p1, p2, p3) => { n++; return "" + p0 + p1 + p2 + p3; });

            var res = mem.MemoizeWeak<Func<int, int, int, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("40142740", res.Delegate(40, 14, 27, 40));
            Assert.AreEqual(1, n);

            Assert.AreEqual("40142740", res.Delegate(40, 14, 27, 40));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed2_Func4()
        {
            // 2 int; 3 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, string, string, int, string>((p0, p1, p2, p3) => { n++; return "" + p0 + p1 + p2 + p3; });

            var res = mem.MemoizeWeak<Func<int, string, string, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("1149455", res.Delegate(11, "49", "4", 55));
            Assert.AreEqual(1, n);

            Assert.AreEqual("1149455", res.Delegate(11, "49", "4", 55));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed3_Func4()
        {
            // 3 string; 2 int

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, int, string, int, string>((p0, p1, p2, p3) => { n++; return "" + p0 + p1 + p2 + p3; });

            var res = mem.MemoizeWeak<Func<string, int, string, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("31664693", res.Delegate("31", 66, "46", 93));
            Assert.AreEqual(1, n);

            Assert.AreEqual("31664693", res.Delegate("31", 66, "46", 93));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Func5()
        {
            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, string, string, string, string, string>((p0, p1, p2, p3, p4) => { n++; return "" + p0 + p1 + p2 + p3 + p4; });

            var res = mem.MemoizeWeak<Func<string, string, string, string, string, string>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual("8246235134", res.Delegate("82", "46", "23", "51", "34"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("8246235134", res.Delegate("82", "46", "23", "51", "34"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Lifetime_Func5()
        {
            lock (typeof(Obj))
            {
                Obj.Reset();
                try
                {
                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static void Do()
                    {
                        var n = 0;

                        var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
                        var f = new Func<Obj, Obj, Obj, Obj, Obj, string>((p0, p1, p2, p3, p4) => { n++; return ""; });

                        var res = mem.MemoizeWeak<Func<Obj, Obj, Obj, Obj, Obj, string>>(f, MemoizationOptions.None);

                        Assert.AreEqual(0, n);

                        Assert.AreEqual("", Apply(res.Delegate));
                        Assert.AreEqual(1, n);
                    }

                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static string Apply(Func<Obj, Obj, Obj, Obj, Obj, string> f)
                    {
                        return f(new Obj(), new Obj(), new Obj(), new Obj(), new Obj());
                    }

                    Do();

                    CollectAndCheckFinalizeCount(5);
                }
                finally
                {
                    Obj.Reset();
                }
            }
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed1_Func5()
        {
            // 3 int; 3 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, string, int, string, string>((p0, p1, p2, p3, p4) => { n++; return "" + p0 + p1 + p2 + p3 + p4; });

            var res = mem.MemoizeWeak<Func<int, int, string, int, string, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("1797686545", res.Delegate(17, 97, "68", 65, "45"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("1797686545", res.Delegate(17, 97, "68", 65, "45"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed2_Func5()
        {
            // 3 string; 3 int

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, int, int, int, string, string>((p0, p1, p2, p3, p4) => { n++; return "" + p0 + p1 + p2 + p3 + p4; });

            var res = mem.MemoizeWeak<Func<string, int, int, int, string, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("232294133", res.Delegate("2", 32, 29, 41, "33"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("232294133", res.Delegate("2", 32, 29, 41, "33"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed3_Func5()
        {
            // 5 int; 1 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int, int, string>((p0, p1, p2, p3, p4) => { n++; return "" + p0 + p1 + p2 + p3 + p4; });

            var res = mem.MemoizeWeak<Func<int, int, int, int, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("949076540", res.Delegate(94, 90, 76, 5, 40));
            Assert.AreEqual(1, n);

            Assert.AreEqual("949076540", res.Delegate(94, 90, 76, 5, 40));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Func6()
        {
            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, string, string, string, string, string, string>((p0, p1, p2, p3, p4, p5) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5; });

            var res = mem.MemoizeWeak<Func<string, string, string, string, string, string, string>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual("139886352275", res.Delegate("13", "98", "86", "35", "22", "75"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("139886352275", res.Delegate("13", "98", "86", "35", "22", "75"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Lifetime_Func6()
        {
            lock (typeof(Obj))
            {
                Obj.Reset();
                try
                {
                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static void Do()
                    {
                        var n = 0;

                        var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
                        var f = new Func<Obj, Obj, Obj, Obj, Obj, Obj, string>((p0, p1, p2, p3, p4, p5) => { n++; return ""; });

                        var res = mem.MemoizeWeak<Func<Obj, Obj, Obj, Obj, Obj, Obj, string>>(f, MemoizationOptions.None);

                        Assert.AreEqual(0, n);

                        Assert.AreEqual("", Apply(res.Delegate));
                        Assert.AreEqual(1, n);
                    }

                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static string Apply(Func<Obj, Obj, Obj, Obj, Obj, Obj, string> f)
                    {
                        return f(new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj());
                    }

                    Do();

                    CollectAndCheckFinalizeCount(6);
                }
                finally
                {
                    Obj.Reset();
                }
            }
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed1_Func6()
        {
            // 5 string; 2 int

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, int, string, string, int, string, string>((p0, p1, p2, p3, p4, p5) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5; });

            var res = mem.MemoizeWeak<Func<string, int, string, string, int, string, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("583256696844", res.Delegate("58", 32, "56", "69", 68, "44"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("583256696844", res.Delegate("58", 32, "56", "69", 68, "44"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed2_Func6()
        {
            // 5 int; 2 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, string, int, int, int, int, string>((p0, p1, p2, p3, p4, p5) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5; });

            var res = mem.MemoizeWeak<Func<int, string, int, int, int, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("262163253283", res.Delegate(26, "21", 63, 25, 32, 83));
            Assert.AreEqual(1, n);

            Assert.AreEqual("262163253283", res.Delegate(26, "21", 63, 25, 32, 83));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed3_Func6()
        {
            // 5 int; 2 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, string, int, int, int, string>((p0, p1, p2, p3, p4, p5) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5; });

            var res = mem.MemoizeWeak<Func<int, int, string, int, int, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("392082206615", res.Delegate(39, 20, "82", 20, 66, 15));
            Assert.AreEqual(1, n);

            Assert.AreEqual("392082206615", res.Delegate(39, 20, "82", 20, 66, 15));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Func7()
        {
            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, string, string, string, string, string, string, string>((p0, p1, p2, p3, p4, p5, p6) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6; });

            var res = mem.MemoizeWeak<Func<string, string, string, string, string, string, string, string>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual("23942654177283", res.Delegate("23", "94", "26", "54", "17", "72", "83"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("23942654177283", res.Delegate("23", "94", "26", "54", "17", "72", "83"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Lifetime_Func7()
        {
            lock (typeof(Obj))
            {
                Obj.Reset();
                try
                {
                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static void Do()
                    {
                        var n = 0;

                        var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
                        var f = new Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>((p0, p1, p2, p3, p4, p5, p6) => { n++; return ""; });

                        var res = mem.MemoizeWeak<Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>>(f, MemoizationOptions.None);

                        Assert.AreEqual(0, n);

                        Assert.AreEqual("", Apply(res.Delegate));
                        Assert.AreEqual(1, n);
                    }

                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static string Apply(Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, string> f)
                    {
                        return f(new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj());
                    }

                    Do();

                    CollectAndCheckFinalizeCount(7);
                }
                finally
                {
                    Obj.Reset();
                }
            }
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed1_Func7()
        {
            // 5 int; 3 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, string, int, int, string, int, string>((p0, p1, p2, p3, p4, p5, p6) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6; });

            var res = mem.MemoizeWeak<Func<int, int, string, int, int, string, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("28632571415244", res.Delegate(28, 63, "25", 71, 41, "52", 44));
            Assert.AreEqual(1, n);

            Assert.AreEqual("28632571415244", res.Delegate(28, 63, "25", 71, 41, "52", 44));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed2_Func7()
        {
            // 4 int; 4 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, string, int, string, int, string, int, string>((p0, p1, p2, p3, p4, p5, p6) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6; });

            var res = mem.MemoizeWeak<Func<int, string, int, string, int, string, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("1513723904188", res.Delegate(15, "13", 72, "3", 90, "41", 88));
            Assert.AreEqual(1, n);

            Assert.AreEqual("1513723904188", res.Delegate(15, "13", 72, "3", 90, "41", 88));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed3_Func7()
        {
            // 2 string; 6 int

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, int, int, int, int, int, int, string>((p0, p1, p2, p3, p4, p5, p6) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6; });

            var res = mem.MemoizeWeak<Func<string, int, int, int, int, int, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("90255121914594", res.Delegate("90", 25, 51, 21, 91, 45, 94));
            Assert.AreEqual(1, n);

            Assert.AreEqual("90255121914594", res.Delegate("90", 25, 51, 21, 91, 45, 94));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Func8()
        {
            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, string, string, string, string, string, string, string, string>((p0, p1, p2, p3, p4, p5, p6, p7) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7; });

            var res = mem.MemoizeWeak<Func<string, string, string, string, string, string, string, string, string>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual("311827520391381", res.Delegate("31", "1", "82", "75", "20", "39", "13", "81"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("311827520391381", res.Delegate("31", "1", "82", "75", "20", "39", "13", "81"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Lifetime_Func8()
        {
            lock (typeof(Obj))
            {
                Obj.Reset();
                try
                {
                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static void Do()
                    {
                        var n = 0;

                        var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
                        var f = new Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>((p0, p1, p2, p3, p4, p5, p6, p7) => { n++; return ""; });

                        var res = mem.MemoizeWeak<Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>>(f, MemoizationOptions.None);

                        Assert.AreEqual(0, n);

                        Assert.AreEqual("", Apply(res.Delegate));
                        Assert.AreEqual(1, n);
                    }

                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static string Apply(Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string> f)
                    {
                        return f(new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj());
                    }

                    Do();

                    CollectAndCheckFinalizeCount(8);
                }
                finally
                {
                    Obj.Reset();
                }
            }
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed1_Func8()
        {
            // 5 string; 4 int

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, int, int, string, string, int, int, string, string>((p0, p1, p2, p3, p4, p5, p6, p7) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7; });

            var res = mem.MemoizeWeak<Func<string, int, int, string, string, int, int, string, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("899842392210501", res.Delegate("89", 98, 42, "39", "22", 10, 50, "1"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("899842392210501", res.Delegate("89", 98, 42, "39", "22", 10, 50, "1"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed2_Func8()
        {
            // 5 string; 4 int

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, string, int, int, string, int, int, string, string>((p0, p1, p2, p3, p4, p5, p6, p7) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7; });

            var res = mem.MemoizeWeak<Func<string, string, int, int, string, int, int, string, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("74714429318326", res.Delegate("74", "71", 44, 29, "31", 83, 2, "6"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("74714429318326", res.Delegate("74", "71", 44, 29, "31", 83, 2, "6"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed3_Func8()
        {
            // 5 int; 4 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int, string, string, int, string, string>((p0, p1, p2, p3, p4, p5, p6, p7) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7; });

            var res = mem.MemoizeWeak<Func<int, int, int, int, string, string, int, string, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("7512852197745969", res.Delegate(75, 12, 85, 21, "97", "74", 59, "69"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("7512852197745969", res.Delegate(75, 12, 85, 21, "97", "74", 59, "69"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Func9()
        {
            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, string, string, string, string, string, string, string, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8; });

            var res = mem.MemoizeWeak<Func<string, string, string, string, string, string, string, string, string, string>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual("206719721122319838", res.Delegate("20", "67", "19", "72", "11", "22", "31", "98", "38"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("206719721122319838", res.Delegate("20", "67", "19", "72", "11", "22", "31", "98", "38"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Lifetime_Func9()
        {
            lock (typeof(Obj))
            {
                Obj.Reset();
                try
                {
                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static void Do()
                    {
                        var n = 0;

                        var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
                        var f = new Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8) => { n++; return ""; });

                        var res = mem.MemoizeWeak<Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>>(f, MemoizationOptions.None);

                        Assert.AreEqual(0, n);

                        Assert.AreEqual("", Apply(res.Delegate));
                        Assert.AreEqual(1, n);
                    }

                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static string Apply(Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string> f)
                    {
                        return f(new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj());
                    }

                    Do();

                    CollectAndCheckFinalizeCount(9);
                }
                finally
                {
                    Obj.Reset();
                }
            }
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed1_Func9()
        {
            // 4 string; 6 int

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, string, int, int, int, int, string, int, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8; });

            var res = mem.MemoizeWeak<Func<string, string, int, int, int, int, string, int, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("7742494991175964", res.Delegate("77", "42", 49, 4, 99, 11, "75", 9, 64));
            Assert.AreEqual(1, n);

            Assert.AreEqual("7742494991175964", res.Delegate("77", "42", 49, 4, 99, 11, "75", 9, 64));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed2_Func9()
        {
            // 8 int; 2 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int, int, int, int, int, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8; });

            var res = mem.MemoizeWeak<Func<int, int, int, int, int, int, int, int, string, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("159780747924629040", res.Delegate(15, 97, 80, 74, 79, 24, 62, 90, "40"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("159780747924629040", res.Delegate(15, 97, 80, 74, 79, 24, 62, 90, "40"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed3_Func9()
        {
            // 3 string; 7 int

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, int, int, int, int, int, int, string, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8; });

            var res = mem.MemoizeWeak<Func<string, int, int, int, int, int, int, string, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("89276921886744367", res.Delegate("89", 27, 6, 92, 18, 86, 74, "43", 67));
            Assert.AreEqual(1, n);

            Assert.AreEqual("89276921886744367", res.Delegate("89", 27, 6, 92, 18, 86, 74, "43", 67));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Func10()
        {
            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, string, string, string, string, string, string, string, string, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9; });

            var res = mem.MemoizeWeak<Func<string, string, string, string, string, string, string, string, string, string, string>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual("39514659641927449556", res.Delegate("39", "51", "46", "59", "64", "19", "27", "44", "95", "56"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("39514659641927449556", res.Delegate("39", "51", "46", "59", "64", "19", "27", "44", "95", "56"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Lifetime_Func10()
        {
            lock (typeof(Obj))
            {
                Obj.Reset();
                try
                {
                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static void Do()
                    {
                        var n = 0;

                        var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
                        var f = new Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9) => { n++; return ""; });

                        var res = mem.MemoizeWeak<Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>>(f, MemoizationOptions.None);

                        Assert.AreEqual(0, n);

                        Assert.AreEqual("", Apply(res.Delegate));
                        Assert.AreEqual(1, n);
                    }

                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static string Apply(Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string> f)
                    {
                        return f(new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj());
                    }

                    Do();

                    CollectAndCheckFinalizeCount(10);
                }
                finally
                {
                    Obj.Reset();
                }
            }
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed1_Func10()
        {
            // 4 int; 7 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, string, int, string, string, string, string, string, int, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9; });

            var res = mem.MemoizeWeak<Func<int, string, int, string, string, string, string, string, int, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("940365722399324174", res.Delegate(94, "0", 36, "57", "22", "39", "93", "24", 17, 4));
            Assert.AreEqual(1, n);

            Assert.AreEqual("940365722399324174", res.Delegate(94, "0", 36, "57", "22", "39", "93", "24", 17, 4));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed2_Func10()
        {
            // 4 string; 7 int

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, int, int, string, int, int, string, int, int, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9; });

            var res = mem.MemoizeWeak<Func<string, int, int, string, int, int, string, int, int, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("2659331350624298979", res.Delegate("26", 59, 33, "13", 50, 62, "4", 29, 89, 79));
            Assert.AreEqual(1, n);

            Assert.AreEqual("2659331350624298979", res.Delegate("26", 59, 33, "13", 50, 62, "4", 29, 89, 79));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed3_Func10()
        {
            // 9 int; 2 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int, int, int, int, int, int, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9; });

            var res = mem.MemoizeWeak<Func<int, int, int, int, int, int, int, int, int, string, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("7767936224753176775", res.Delegate(77, 67, 9, 36, 22, 47, 53, 17, 67, "75"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("7767936224753176775", res.Delegate(77, 67, 9, 36, 22, 47, 53, 17, 67, "75"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Func11()
        {
            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, string, string, string, string, string, string, string, string, string, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10; });

            var res = mem.MemoizeWeak<Func<string, string, string, string, string, string, string, string, string, string, string, string>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual("19125215235662265935", res.Delegate("19", "12", "52", "1", "52", "35", "66", "22", "6", "59", "35"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("19125215235662265935", res.Delegate("19", "12", "52", "1", "52", "35", "66", "22", "6", "59", "35"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Lifetime_Func11()
        {
            lock (typeof(Obj))
            {
                Obj.Reset();
                try
                {
                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static void Do()
                    {
                        var n = 0;

                        var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
                        var f = new Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10) => { n++; return ""; });

                        var res = mem.MemoizeWeak<Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>>(f, MemoizationOptions.None);

                        Assert.AreEqual(0, n);

                        Assert.AreEqual("", Apply(res.Delegate));
                        Assert.AreEqual(1, n);
                    }

                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static string Apply(Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string> f)
                    {
                        return f(new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj());
                    }

                    Do();

                    CollectAndCheckFinalizeCount(11);
                }
                finally
                {
                    Obj.Reset();
                }
            }
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed1_Func11()
        {
            // 7 int; 5 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, string, string, int, int, int, string, int, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10; });

            var res = mem.MemoizeWeak<Func<int, int, int, string, string, int, int, int, string, int, string, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("14598242064273645960", res.Delegate(14, 59, 8, "24", "20", 64, 27, 36, "4", 59, "60"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("14598242064273645960", res.Delegate(14, 59, 8, "24", "20", 64, 27, 36, "4", 59, "60"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed2_Func11()
        {
            // 6 string; 6 int

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, int, string, int, int, int, string, int, int, string, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10; });

            var res = mem.MemoizeWeak<Func<string, int, string, int, int, int, string, int, int, string, string, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("214724589385693134949", res.Delegate("21", 47, "24", 58, 93, 85, "69", 31, 3, "49", "49"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("214724589385693134949", res.Delegate("21", 47, "24", 58, 93, 85, "69", 31, 3, "49", "49"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed3_Func11()
        {
            // 5 int; 7 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, string, string, string, string, string, int, int, string, int, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10; });

            var res = mem.MemoizeWeak<Func<int, string, string, string, string, string, int, int, string, int, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("49430267843972633541", res.Delegate(4, "94", "30", "26", "78", "43", 97, 2, "63", 35, 41));
            Assert.AreEqual(1, n);

            Assert.AreEqual("49430267843972633541", res.Delegate(4, "94", "30", "26", "78", "43", 97, 2, "63", 35, 41));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Func12()
        {
            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, string, string, string, string, string, string, string, string, string, string, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11; });

            var res = mem.MemoizeWeak<Func<string, string, string, string, string, string, string, string, string, string, string, string, string>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual("63861996171416772561084", res.Delegate("6", "38", "61", "99", "61", "71", "41", "67", "72", "56", "10", "84"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("63861996171416772561084", res.Delegate("6", "38", "61", "99", "61", "71", "41", "67", "72", "56", "10", "84"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Lifetime_Func12()
        {
            lock (typeof(Obj))
            {
                Obj.Reset();
                try
                {
                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static void Do()
                    {
                        var n = 0;

                        var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
                        var f = new Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11) => { n++; return ""; });

                        var res = mem.MemoizeWeak<Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>>(f, MemoizationOptions.None);

                        Assert.AreEqual(0, n);

                        Assert.AreEqual("", Apply(res.Delegate));
                        Assert.AreEqual(1, n);
                    }

                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static string Apply(Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string> f)
                    {
                        return f(new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj());
                    }

                    Do();

                    CollectAndCheckFinalizeCount(12);
                }
                finally
                {
                    Obj.Reset();
                }
            }
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed1_Func12()
        {
            // 7 int; 6 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, string, string, int, string, int, string, int, string, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11; });

            var res = mem.MemoizeWeak<Func<int, int, int, string, string, int, string, int, string, int, string, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("1266571974290829391913", res.Delegate(12, 6, 65, "71", "97", 42, "90", 82, "93", 9, "19", 13));
            Assert.AreEqual(1, n);

            Assert.AreEqual("1266571974290829391913", res.Delegate(12, 6, 65, "71", "97", 42, "90", 82, "93", 9, "19", 13));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed2_Func12()
        {
            // 5 int; 8 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, string, int, string, string, string, string, int, string, string, int, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11; });

            var res = mem.MemoizeWeak<Func<int, string, int, string, string, string, string, int, string, string, int, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("33540375625714998526682", res.Delegate(3, "35", 40, "37", "56", "25", "71", 49, "98", "52", 66, 82));
            Assert.AreEqual(1, n);

            Assert.AreEqual("33540375625714998526682", res.Delegate(3, "35", 40, "37", "56", "25", "71", 49, "98", "52", 66, 82));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed3_Func12()
        {
            // 9 int; 4 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int, int, int, string, int, int, int, string, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11; });

            var res = mem.MemoizeWeak<Func<int, int, int, int, int, int, string, int, int, int, string, string, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("267025893153849437326762", res.Delegate(26, 70, 25, 89, 31, 53, "84", 94, 37, 32, "67", "62"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("267025893153849437326762", res.Delegate(26, 70, 25, 89, 31, 53, "84", 94, 37, 32, "67", "62"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Func13()
        {
            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12; });

            var res = mem.MemoizeWeak<Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual("2532568387015349626202811", res.Delegate("2", "53", "25", "68", "38", "70", "15", "34", "96", "26", "20", "28", "11"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("2532568387015349626202811", res.Delegate("2", "53", "25", "68", "38", "70", "15", "34", "96", "26", "20", "28", "11"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Lifetime_Func13()
        {
            lock (typeof(Obj))
            {
                Obj.Reset();
                try
                {
                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static void Do()
                    {
                        var n = 0;

                        var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
                        var f = new Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12) => { n++; return ""; });

                        var res = mem.MemoizeWeak<Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>>(f, MemoizationOptions.None);

                        Assert.AreEqual(0, n);

                        Assert.AreEqual("", Apply(res.Delegate));
                        Assert.AreEqual(1, n);
                    }

                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static string Apply(Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string> f)
                    {
                        return f(new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj());
                    }

                    Do();

                    CollectAndCheckFinalizeCount(13);
                }
                finally
                {
                    Obj.Reset();
                }
            }
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed1_Func13()
        {
            // 6 int; 8 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, string, int, string, string, string, string, string, int, int, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12; });

            var res = mem.MemoizeWeak<Func<int, int, int, string, int, string, string, string, string, string, int, int, string, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("38378665994996776846292590", res.Delegate(38, 37, 86, "65", 99, "49", "96", "77", "68", "46", 29, 25, "90"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("38378665994996776846292590", res.Delegate(38, 37, 86, "65", 99, "49", "96", "77", "68", "46", 29, 25, "90"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed2_Func13()
        {
            // 10 int; 4 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, string, int, int, string, int, string, int, int, int, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12; });

            var res = mem.MemoizeWeak<Func<int, int, int, string, int, int, string, int, string, int, int, int, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("4465609558088648512281212", res.Delegate(44, 65, 60, "95", 5, 80, "88", 64, "85", 12, 28, 12, 12));
            Assert.AreEqual(1, n);

            Assert.AreEqual("4465609558088648512281212", res.Delegate(44, 65, 60, "95", 5, 80, "88", 64, "85", 12, 28, 12, 12));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed3_Func13()
        {
            // 10 int; 4 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, string, int, int, int, string, int, int, int, int, int, int, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12; });

            var res = mem.MemoizeWeak<Func<int, string, int, int, int, string, int, int, int, int, int, int, string, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("3412268666194117531177241", res.Delegate(34, "12", 26, 86, 66, "19", 41, 17, 5, 31, 17, 72, "41"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("3412268666194117531177241", res.Delegate(34, "12", 26, 86, 66, "19", 41, 17, 5, 31, 17, 72, "41"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Func14()
        {
            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13; });

            var res = mem.MemoizeWeak<Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual("4837252241911294645227227", res.Delegate("48", "37", "25", "22", "4", "19", "11", "29", "4", "64", "52", "2", "72", "27"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("4837252241911294645227227", res.Delegate("48", "37", "25", "22", "4", "19", "11", "29", "4", "64", "52", "2", "72", "27"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Lifetime_Func14()
        {
            lock (typeof(Obj))
            {
                Obj.Reset();
                try
                {
                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static void Do()
                    {
                        var n = 0;

                        var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
                        var f = new Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13) => { n++; return ""; });

                        var res = mem.MemoizeWeak<Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>>(f, MemoizationOptions.None);

                        Assert.AreEqual(0, n);

                        Assert.AreEqual("", Apply(res.Delegate));
                        Assert.AreEqual(1, n);
                    }

                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static string Apply(Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string> f)
                    {
                        return f(new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj());
                    }

                    Do();

                    CollectAndCheckFinalizeCount(14);
                }
                finally
                {
                    Obj.Reset();
                }
            }
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed1_Func14()
        {
            // 6 int; 9 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, string, int, string, int, string, string, string, string, string, string, int, int, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13; });

            var res = mem.MemoizeWeak<Func<int, string, int, string, int, string, string, string, string, string, string, int, int, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("294774839392658928125919952", res.Delegate(29, "47", 74, "83", 93, "92", "65", "89", "28", "12", "59", 19, 95, 2));
            Assert.AreEqual(1, n);

            Assert.AreEqual("294774839392658928125919952", res.Delegate(29, "47", 74, "83", 93, "92", "65", "89", "28", "12", "59", 19, 95, 2));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed2_Func14()
        {
            // 4 string; 11 int

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, int, string, int, int, int, int, int, string, int, int, int, int, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13; });

            var res = mem.MemoizeWeak<Func<string, int, string, int, int, int, int, int, string, int, int, int, int, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("98317773231616276214511655", res.Delegate("98", 31, "77", 73, 23, 16, 16, 27, "62", 14, 51, 16, 5, 5));
            Assert.AreEqual(1, n);

            Assert.AreEqual("98317773231616276214511655", res.Delegate("98", 31, "77", 73, 23, 16, 16, 27, "62", 14, 51, 16, 5, 5));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed3_Func14()
        {
            // 11 int; 4 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int, int, int, int, int, string, int, int, string, int, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13; });

            var res = mem.MemoizeWeak<Func<int, int, int, int, int, int, int, int, string, int, int, string, int, string, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("559577854999196348595788633", res.Delegate(55, 9, 57, 78, 54, 99, 91, 96, "34", 85, 95, "78", 86, "33"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("559577854999196348595788633", res.Delegate(55, 9, 57, 78, 54, 99, 91, 96, "34", 85, 95, "78", 86, "33"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Func15()
        {
            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14; });

            var res = mem.MemoizeWeak<Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual("258794352971313869544915579", res.Delegate("25", "8", "7", "94", "35", "29", "71", "31", "38", "69", "54", "49", "15", "57", "9"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("258794352971313869544915579", res.Delegate("25", "8", "7", "94", "35", "29", "71", "31", "38", "69", "54", "49", "15", "57", "9"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Lifetime_Func15()
        {
            lock (typeof(Obj))
            {
                Obj.Reset();
                try
                {
                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static void Do()
                    {
                        var n = 0;

                        var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
                        var f = new Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14) => { n++; return ""; });

                        var res = mem.MemoizeWeak<Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>>(f, MemoizationOptions.None);

                        Assert.AreEqual(0, n);

                        Assert.AreEqual("", Apply(res.Delegate));
                        Assert.AreEqual(1, n);
                    }

                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static string Apply(Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string> f)
                    {
                        return f(new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj());
                    }

                    Do();

                    CollectAndCheckFinalizeCount(15);
                }
                finally
                {
                    Obj.Reset();
                }
            }
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed1_Func15()
        {
            // 11 string; 5 int

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, string, string, int, string, string, int, string, string, string, int, string, int, int, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14; });

            var res = mem.MemoizeWeak<Func<string, string, string, int, string, string, int, string, string, string, int, string, int, int, string, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("58334873984615525617151333330", res.Delegate("5", "83", "34", 87, "39", "84", 61, "55", "25", "61", 71, "51", 33, 33, "30"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("58334873984615525617151333330", res.Delegate("5", "83", "34", 87, "39", "84", 61, "55", "25", "61", 71, "51", 33, 33, "30"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed2_Func15()
        {
            // 13 int; 3 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, string, int, string, int, int, int, int, int, int, int, int, int, int, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14; });

            var res = mem.MemoizeWeak<Func<int, string, int, string, int, int, int, int, int, int, int, int, int, int, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("47826849562179188656028765443", res.Delegate(47, "82", 68, "49", 56, 21, 79, 18, 86, 56, 0, 28, 76, 54, 43));
            Assert.AreEqual(1, n);

            Assert.AreEqual("47826849562179188656028765443", res.Delegate(47, "82", 68, "49", 56, 21, 79, 18, 86, 56, 0, 28, 76, 54, 43));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed3_Func15()
        {
            // 11 int; 5 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int, int, string, int, string, int, int, int, int, string, int, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14; });

            var res = mem.MemoizeWeak<Func<int, int, int, int, int, string, int, string, int, int, int, int, string, int, string, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("6180974713898284769374719406", res.Delegate(61, 80, 97, 47, 13, "89", 82, "84", 76, 9, 37, 47, "19", 40, "6"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("6180974713898284769374719406", res.Delegate(61, 80, 97, 47, 13, "89", 82, "84", 76, 9, 37, 47, "19", 40, "6"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Func16()
        {
            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15; });

            var res = mem.MemoizeWeak<Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual("4172449834797857106029887959937", res.Delegate("41", "72", "44", "98", "34", "79", "78", "57", "10", "60", "29", "8", "87", "95", "99", "37"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("4172449834797857106029887959937", res.Delegate("41", "72", "44", "98", "34", "79", "78", "57", "10", "60", "29", "8", "87", "95", "99", "37"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Lifetime_Func16()
        {
            lock (typeof(Obj))
            {
                Obj.Reset();
                try
                {
                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static void Do()
                    {
                        var n = 0;

                        var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
                        var f = new Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15) => { n++; return ""; });

                        var res = mem.MemoizeWeak<Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>>(f, MemoizationOptions.None);

                        Assert.AreEqual(0, n);

                        Assert.AreEqual("", Apply(res.Delegate));
                        Assert.AreEqual(1, n);
                    }

                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static string Apply(Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string> f)
                    {
                        return f(new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj());
                    }

                    Do();

                    CollectAndCheckFinalizeCount(16);
                }
                finally
                {
                    Obj.Reset();
                }
            }
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed1_Func16()
        {
            // 12 string; 5 int

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, int, int, string, int, string, string, string, int, int, string, string, string, string, string, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15; });

            var res = mem.MemoizeWeak<Func<string, int, int, string, int, string, string, string, int, int, string, string, string, string, string, string, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("7771491952458610433445522104399", res.Delegate("77", 71, 49, "19", 52, "45", "86", "10", 43, 3, "44", "55", "22", "10", "43", "99"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("7771491952458610433445522104399", res.Delegate("77", 71, 49, "19", 52, "45", "86", "10", 43, 3, "44", "55", "22", "10", "43", "99"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed2_Func16()
        {
            // 10 int; 7 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, string, string, int, int, string, int, int, int, string, string, int, string, int, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15; });

            var res = mem.MemoizeWeak<Func<int, int, string, string, int, int, string, int, int, int, string, string, int, string, int, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("8468904260504588871403649991772", res.Delegate(84, 68, "90", "42", 60, 50, "45", 88, 87, 1, "40", "36", 49, "99", 17, 72));
            Assert.AreEqual(1, n);

            Assert.AreEqual("8468904260504588871403649991772", res.Delegate(84, 68, "90", "42", 60, 50, "45", 88, 87, 1, "40", "36", 49, "99", 17, 72));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed3_Func16()
        {
            // 13 int; 4 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, string, int, int, int, int, int, string, int, int, int, int, string, int, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15; });

            var res = mem.MemoizeWeak<Func<int, int, string, int, int, int, int, int, string, int, int, int, int, string, int, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("4469715319038408587268274614572", res.Delegate(44, 69, "71", 53, 19, 0, 38, 40, "85", 87, 26, 82, 74, "61", 45, 72));
            Assert.AreEqual(1, n);

            Assert.AreEqual("4469715319038408587268274614572", res.Delegate(44, 69, "71", 53, 19, 0, 38, 40, "85", 87, 26, 82, 74, "61", 45, 72));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Func17()
        {
            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16; });

            var res = mem.MemoizeWeak<Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual("103916574457026428685220549734", res.Delegate("10", "39", "16", "57", "44", "5", "70", "2", "64", "28", "68", "5", "2", "20", "54", "97", "34"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("103916574457026428685220549734", res.Delegate("10", "39", "16", "57", "44", "5", "70", "2", "64", "28", "68", "5", "2", "20", "54", "97", "34"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Lifetime_Func17()
        {
            lock (typeof(Obj))
            {
                Obj.Reset();
                try
                {
                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static void Do()
                    {
                        var n = 0;

                        var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
                        var f = new Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16) => { n++; return ""; });

                        var res = mem.MemoizeWeak<Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>>(f, MemoizationOptions.None);

                        Assert.AreEqual(0, n);

                        Assert.AreEqual("", Apply(res.Delegate));
                        Assert.AreEqual(1, n);
                    }

                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static string Apply(Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string> f)
                    {
                        return f(new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj());
                    }

                    Do();

                    CollectAndCheckFinalizeCount(17);
                }
                finally
                {
                    Obj.Reset();
                }
            }
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed1_Func17()
        {
            // 10 string; 8 int

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, string, string, string, int, string, int, int, int, string, int, int, string, string, int, int, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16; });

            var res = mem.MemoizeWeak<Func<string, string, string, string, int, string, int, int, int, string, int, int, string, string, int, int, string, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("357065444786379883853474340579428", res.Delegate("35", "70", "65", "44", 47, "86", 37, 9, 88, "38", 53, 47, "43", "40", 57, 94, "28"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("357065444786379883853474340579428", res.Delegate("35", "70", "65", "44", 47, "86", 37, 9, 88, "38", 53, 47, "43", "40", 57, 94, "28"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed2_Func17()
        {
            // 13 int; 5 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, string, int, int, int, int, int, string, string, int, int, int, int, int, string, int, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16; });

            var res = mem.MemoizeWeak<Func<int, string, int, int, int, int, int, string, string, int, int, int, int, int, string, int, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("5241443067735986804620455174389", res.Delegate(52, "41", 44, 30, 67, 7, 35, "98", "68", 0, 46, 20, 45, 51, "74", 38, 9));
            Assert.AreEqual(1, n);

            Assert.AreEqual("5241443067735986804620455174389", res.Delegate(52, "41", 44, 30, 67, 7, 35, "98", "68", 0, 46, 20, 45, 51, "74", 38, 9));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed3_Func17()
        {
            // 4 string; 14 int

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, int, string, int, int, int, int, int, int, int, int, int, int, int, string, int, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16; });

            var res = mem.MemoizeWeak<Func<string, int, string, int, int, int, int, int, int, int, int, int, int, int, string, int, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("87155097385389271251333828972758", res.Delegate("87", 15, "50", 97, 38, 53, 89, 27, 12, 51, 33, 38, 28, 9, "72", 75, 8));
            Assert.AreEqual(1, n);

            Assert.AreEqual("87155097385389271251333828972758", res.Delegate("87", 15, "50", 97, 38, 53, 89, 27, 12, 51, 33, 38, 28, 9, "72", 75, 8));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Func18()
        {
            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17; });

            var res = mem.MemoizeWeak<Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual("45122251972258459582453976577997564", res.Delegate("45", "12", "22", "51", "97", "22", "58", "45", "95", "82", "45", "3", "97", "65", "77", "99", "75", "64"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("45122251972258459582453976577997564", res.Delegate("45", "12", "22", "51", "97", "22", "58", "45", "95", "82", "45", "3", "97", "65", "77", "99", "75", "64"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Lifetime_Func18()
        {
            lock (typeof(Obj))
            {
                Obj.Reset();
                try
                {
                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static void Do()
                    {
                        var n = 0;

                        var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
                        var f = new Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17) => { n++; return ""; });

                        var res = mem.MemoizeWeak<Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>>(f, MemoizationOptions.None);

                        Assert.AreEqual(0, n);

                        Assert.AreEqual("", Apply(res.Delegate));
                        Assert.AreEqual(1, n);
                    }

                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static string Apply(Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string> f)
                    {
                        return f(new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj());
                    }

                    Do();

                    CollectAndCheckFinalizeCount(18);
                }
                finally
                {
                    Obj.Reset();
                }
            }
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed1_Func18()
        {
            // 15 string; 4 int

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, string, string, string, int, string, string, string, string, string, string, string, int, string, int, string, string, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17; });

            var res = mem.MemoizeWeak<Func<string, string, string, string, int, string, string, string, string, string, string, string, int, string, int, string, string, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("972594857545559851662984357521157", res.Delegate("97", "25", "94", "8", 57, "54", "55", "5", "98", "51", "66", "29", 84, "35", 75, "21", "1", 57));
            Assert.AreEqual(1, n);

            Assert.AreEqual("972594857545559851662984357521157", res.Delegate("97", "25", "94", "8", 57, "54", "55", "5", "98", "51", "66", "29", 84, "35", 75, "21", "1", 57));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed2_Func18()
        {
            // 10 int; 9 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, string, string, string, string, string, string, int, int, string, string, int, int, int, int, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17; });

            var res = mem.MemoizeWeak<Func<int, int, int, string, string, string, string, string, string, int, int, string, string, int, int, int, int, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("62896035378653924143878103336957148", res.Delegate(62, 89, 60, "35", "37", "86", "53", "92", "4", 14, 38, "78", "10", 33, 36, 95, 71, 48));
            Assert.AreEqual(1, n);

            Assert.AreEqual("62896035378653924143878103336957148", res.Delegate(62, 89, 60, "35", "37", "86", "53", "92", "4", 14, 38, "78", "10", 33, 36, 95, 71, 48));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed3_Func18()
        {
            // 12 int; 7 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, string, int, int, string, int, string, int, int, int, int, string, string, int, int, int, int, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17; });

            var res = mem.MemoizeWeak<Func<int, string, int, int, string, int, string, int, int, int, int, string, string, int, int, int, int, string, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("97382710483435731507163719599534226", res.Delegate(97, "38", 27, 10, "4", 83, "43", 57, 31, 50, 71, "63", "71", 95, 99, 53, 42, "26"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("97382710483435731507163719599534226", res.Delegate(97, "38", 27, 10, "4", 83, "43", 57, 31, 50, 71, "63", "71", 95, 99, 53, 42, "26"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Func19()
        {
            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18; });

            var res = mem.MemoizeWeak<Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual("14155272963786331987867997425973321", res.Delegate("14", "15", "52", "72", "9", "63", "78", "63", "31", "9", "87", "86", "7", "99", "74", "25", "97", "33", "21"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("14155272963786331987867997425973321", res.Delegate("14", "15", "52", "72", "9", "63", "78", "63", "31", "9", "87", "86", "7", "99", "74", "25", "97", "33", "21"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Lifetime_Func19()
        {
            lock (typeof(Obj))
            {
                Obj.Reset();
                try
                {
                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static void Do()
                    {
                        var n = 0;

                        var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
                        var f = new Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18) => { n++; return ""; });

                        var res = mem.MemoizeWeak<Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>>(f, MemoizationOptions.None);

                        Assert.AreEqual(0, n);

                        Assert.AreEqual("", Apply(res.Delegate));
                        Assert.AreEqual(1, n);
                    }

                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static string Apply(Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string> f)
                    {
                        return f(new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj());
                    }

                    Do();

                    CollectAndCheckFinalizeCount(19);
                }
                finally
                {
                    Obj.Reset();
                }
            }
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed1_Func19()
        {
            // 9 int; 11 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, string, int, int, string, string, string, int, int, string, int, string, string, string, string, int, string, int, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18; });

            var res = mem.MemoizeWeak<Func<int, string, int, int, string, string, string, int, int, string, int, string, string, string, string, int, string, int, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("731733603343661314766223166735564691", res.Delegate(73, "17", 33, 60, "33", "43", "66", 13, 14, "76", 62, "2", "31", "66", "73", 5, "56", 46, 91));
            Assert.AreEqual(1, n);

            Assert.AreEqual("731733603343661314766223166735564691", res.Delegate(73, "17", 33, 60, "33", "43", "66", 13, 14, "76", 62, "2", "31", "66", "73", 5, "56", 46, 91));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed2_Func19()
        {
            // 12 int; 8 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, string, string, int, int, int, string, string, int, int, int, string, string, string, int, int, int, int, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18; });

            var res = mem.MemoizeWeak<Func<int, string, string, int, int, int, string, string, int, int, int, string, string, string, int, int, int, int, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("989885205468806957802461751513444636", res.Delegate(98, "98", "85", 20, 54, 68, "80", "69", 57, 80, 24, "61", "75", "15", 1, 34, 44, 63, 6));
            Assert.AreEqual(1, n);

            Assert.AreEqual("989885205468806957802461751513444636", res.Delegate(98, "98", "85", 20, 54, 68, "80", "69", 57, 80, 24, "61", "75", "15", 1, 34, 44, 63, 6));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed3_Func19()
        {
            // 15 int; 5 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, string, int, int, int, int, int, int, string, int, string, int, int, int, int, string, int, int, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18; });

            var res = mem.MemoizeWeak<Func<int, string, int, int, int, int, int, int, string, int, string, int, int, int, int, string, int, int, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("5542536197934368643250128622194742028", res.Delegate(55, "42", 53, 61, 97, 93, 43, 68, "64", 32, "50", 12, 86, 22, 1, "94", 74, 20, 28));
            Assert.AreEqual(1, n);

            Assert.AreEqual("5542536197934368643250128622194742028", res.Delegate(55, "42", 53, 61, 97, 93, 43, 68, "64", 32, "50", 12, 86, 22, 1, "94", 74, 20, 28));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Func20()
        {
            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19; });

            var res = mem.MemoizeWeak<Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual("8737804069145699585123678696863466781", res.Delegate("87", "37", "80", "40", "69", "14", "56", "99", "58", "51", "23", "67", "8", "6", "96", "8", "63", "46", "67", "81"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("8737804069145699585123678696863466781", res.Delegate("87", "37", "80", "40", "69", "14", "56", "99", "58", "51", "23", "67", "8", "6", "96", "8", "63", "46", "67", "81"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Lifetime_Func20()
        {
            lock (typeof(Obj))
            {
                Obj.Reset();
                try
                {
                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static void Do()
                    {
                        var n = 0;

                        var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
                        var f = new Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19) => { n++; return ""; });

                        var res = mem.MemoizeWeak<Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>>(f, MemoizationOptions.None);

                        Assert.AreEqual(0, n);

                        Assert.AreEqual("", Apply(res.Delegate));
                        Assert.AreEqual(1, n);
                    }

                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static string Apply(Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string> f)
                    {
                        return f(new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj());
                    }

                    Do();

                    CollectAndCheckFinalizeCount(20);
                }
                finally
                {
                    Obj.Reset();
                }
            }
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed1_Func20()
        {
            // 13 string; 8 int

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, string, string, string, string, string, int, int, string, int, string, int, string, int, int, int, int, string, string, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19; });

            var res = mem.MemoizeWeak<Func<string, string, string, string, string, string, int, int, string, int, string, int, string, int, int, int, int, string, string, string, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("7142419435453943658533616320408269976832", res.Delegate("71", "42", "41", "94", "35", "45", 39, 43, "65", 85, "33", 61, "63", 20, 40, 82, 69, "97", "68", "32"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("7142419435453943658533616320408269976832", res.Delegate("71", "42", "41", "94", "35", "45", 39, 43, "65", 85, "33", 61, "63", 20, 40, 82, 69, "97", "68", "32"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed2_Func20()
        {
            // 7 string; 14 int

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, int, int, int, int, int, int, string, int, string, int, int, int, int, string, int, int, string, int, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19; });

            var res = mem.MemoizeWeak<Func<string, int, int, int, int, int, int, string, int, string, int, int, int, int, string, int, int, string, int, string, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("24984148075848698696225211903517986768", res.Delegate("24", 98, 41, 4, 80, 75, 84, "86", 98, "69", 62, 25, 2, 11, "90", 35, 17, "98", 67, "68"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("24984148075848698696225211903517986768", res.Delegate("24", 98, 41, 4, 80, 75, 84, "86", 98, "69", 62, 25, 2, 11, "90", 35, 17, "98", 67, "68"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed3_Func20()
        {
            // 7 string; 14 int

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, int, string, int, int, int, int, string, int, int, string, int, int, int, int, int, int, string, int, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19; });

            var res = mem.MemoizeWeak<Func<string, int, string, int, int, int, int, string, int, int, string, int, int, int, int, int, int, string, int, string, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("82849042456314684398153231321736884650", res.Delegate("82", 84, "90", 42, 45, 63, 1, "46", 84, 39, "81", 53, 23, 13, 21, 73, 68, "84", 6, "50"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("82849042456314684398153231321736884650", res.Delegate("82", 84, "90", 42, 45, 63, 1, "46", 84, 39, "81", 53, 23, 13, 21, 73, 68, "84", 6, "50"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Func21()
        {
            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20; });

            var res = mem.MemoizeWeak<Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual("7296425639849294714196351685906087103428", res.Delegate("72", "96", "42", "56", "39", "84", "9", "29", "4", "71", "41", "96", "35", "16", "85", "90", "60", "87", "10", "34", "28"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("7296425639849294714196351685906087103428", res.Delegate("72", "96", "42", "56", "39", "84", "9", "29", "4", "71", "41", "96", "35", "16", "85", "90", "60", "87", "10", "34", "28"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Lifetime_Func21()
        {
            lock (typeof(Obj))
            {
                Obj.Reset();
                try
                {
                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static void Do()
                    {
                        var n = 0;

                        var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
                        var f = new Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20) => { n++; return ""; });

                        var res = mem.MemoizeWeak<Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>>(f, MemoizationOptions.None);

                        Assert.AreEqual(0, n);

                        Assert.AreEqual("", Apply(res.Delegate));
                        Assert.AreEqual(1, n);
                    }

                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static string Apply(Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string> f)
                    {
                        return f(new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj());
                    }

                    Do();

                    CollectAndCheckFinalizeCount(21);
                }
                finally
                {
                    Obj.Reset();
                }
            }
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed1_Func21()
        {
            // 13 int; 9 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, string, string, int, string, int, int, string, int, int, string, int, int, string, int, int, string, int, string, int, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20; });

            var res = mem.MemoizeWeak<Func<int, string, string, int, string, int, int, string, int, int, string, int, int, string, int, int, string, int, string, int, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("70203182233898472684488156149157658497219", res.Delegate(70, "20", "31", 82, "23", 38, 98, "47", 26, 84, "48", 81, 56, "14", 91, 5, "76", 58, "49", 72, 19));
            Assert.AreEqual(1, n);

            Assert.AreEqual("70203182233898472684488156149157658497219", res.Delegate(70, "20", "31", 82, "23", 38, 98, "47", 26, 84, "48", 81, 56, "14", 91, 5, "76", 58, "49", 72, 19));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed2_Func21()
        {
            // 14 int; 8 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, string, int, string, int, string, int, string, int, string, int, int, int, string, int, int, int, int, string, int, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20; });

            var res = mem.MemoizeWeak<Func<int, string, int, string, int, string, int, string, int, string, int, int, int, string, int, int, int, int, string, int, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("87962590661031194783559505095413374443", res.Delegate(87, "96", 25, "90", 66, "1", 0, "31", 19, "4", 78, 35, 59, "50", 50, 95, 41, 33, "74", 4, 43));
            Assert.AreEqual(1, n);

            Assert.AreEqual("87962590661031194783559505095413374443", res.Delegate(87, "96", 25, "90", 66, "1", 0, "31", 19, "4", 78, 35, 59, "50", 50, 95, 41, 33, "74", 4, 43));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed3_Func21()
        {
            // 18 int; 4 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, string, string, int, int, int, int, int, int, int, int, int, int, int, int, int, string, int, int, int, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20; });

            var res = mem.MemoizeWeak<Func<int, string, string, int, int, int, int, int, int, int, int, int, int, int, int, int, string, int, int, int, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("183654834214171606193254099531054359093", res.Delegate(18, "36", "54", 8, 34, 21, 41, 71, 60, 61, 93, 25, 40, 99, 53, 10, "54", 3, 59, 0, 93));
            Assert.AreEqual(1, n);

            Assert.AreEqual("183654834214171606193254099531054359093", res.Delegate(18, "36", "54", 8, 34, 21, 41, 71, 60, 61, 93, 25, 40, 99, 53, 10, "54", 3, 59, 0, 93));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Func22()
        {
            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21; });

            var res = mem.MemoizeWeak<Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual("454097932318768590913107069660272820596613", res.Delegate("45", "40", "97", "93", "23", "18", "76", "85", "90", "9", "13", "10", "70", "69", "6", "60", "27", "28", "20", "59", "66", "13"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("454097932318768590913107069660272820596613", res.Delegate("45", "40", "97", "93", "23", "18", "76", "85", "90", "9", "13", "10", "70", "69", "6", "60", "27", "28", "20", "59", "66", "13"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Lifetime_Func22()
        {
            lock (typeof(Obj))
            {
                Obj.Reset();
                try
                {
                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static void Do()
                    {
                        var n = 0;

                        var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
                        var f = new Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21) => { n++; return ""; });

                        var res = mem.MemoizeWeak<Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>>(f, MemoizationOptions.None);

                        Assert.AreEqual(0, n);

                        Assert.AreEqual("", Apply(res.Delegate));
                        Assert.AreEqual(1, n);
                    }

                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static string Apply(Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string> f)
                    {
                        return f(new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj());
                    }

                    Do();

                    CollectAndCheckFinalizeCount(22);
                }
                finally
                {
                    Obj.Reset();
                }
            }
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed1_Func22()
        {
            // 12 int; 11 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, string, int, string, int, string, string, int, string, int, int, int, int, int, int, string, string, int, int, string, string, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21; });

            var res = mem.MemoizeWeak<Func<int, string, int, string, int, string, string, int, string, int, int, int, int, int, int, string, string, int, int, string, string, string, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("169095921218179479435081252734969456352", res.Delegate(16, "90", 95, "9", 2, "12", "18", 17, "94", 79, 43, 50, 8, 1, 25, "2", "73", 49, 69, "45", "63", "52"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("169095921218179479435081252734969456352", res.Delegate(16, "90", 95, "9", 2, "12", "18", 17, "94", 79, 43, 50, 8, 1, 25, "2", "73", 49, 69, "45", "63", "52"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed2_Func22()
        {
            // 9 string; 14 int

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, int, int, int, int, string, int, string, int, string, string, int, int, string, string, int, int, string, int, int, int, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21; });

            var res = mem.MemoizeWeak<Func<string, int, int, int, int, string, int, string, int, string, string, int, int, string, string, int, int, string, int, int, int, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("888807528282352443466338535153733599877867", res.Delegate("88", 88, 0, 75, 2, "82", 82, "35", 24, "43", "46", 63, 38, "53", "51", 53, 73, "35", 99, 87, 78, 67));
            Assert.AreEqual(1, n);

            Assert.AreEqual("888807528282352443466338535153733599877867", res.Delegate("88", 88, 0, 75, 2, "82", 82, "35", 24, "43", "46", 63, 38, "53", "51", 53, 73, "35", 99, 87, 78, 67));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed3_Func22()
        {
            // 6 string; 17 int

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, int, int, int, int, int, int, int, int, string, string, string, int, int, int, int, int, int, int, int, string, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21; });

            var res = mem.MemoizeWeak<Func<string, int, int, int, int, int, int, int, int, string, string, string, int, int, int, int, int, int, int, int, string, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("223540808594402021586920061735614463369533", res.Delegate("22", 35, 40, 80, 85, 94, 40, 20, 21, "58", "69", "20", 0, 61, 73, 56, 14, 46, 33, 69, "53", 3));
            Assert.AreEqual(1, n);

            Assert.AreEqual("223540808594402021586920061735614463369533", res.Delegate("22", 35, 40, 80, 85, 94, 40, 20, 21, "58", "69", "20", 0, 61, 73, 56, 14, 46, 33, 69, "53", 3));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Func23()
        {
            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22; });

            var res = mem.MemoizeWeak<Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual("563308241055646111176915665865071469214835", res.Delegate("56", "33", "0", "82", "4", "10", "55", "64", "61", "11", "17", "69", "15", "66", "5", "86", "50", "7", "14", "69", "21", "48", "35"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("563308241055646111176915665865071469214835", res.Delegate("56", "33", "0", "82", "4", "10", "55", "64", "61", "11", "17", "69", "15", "66", "5", "86", "50", "7", "14", "69", "21", "48", "35"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Lifetime_Func23()
        {
            lock (typeof(Obj))
            {
                Obj.Reset();
                try
                {
                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static void Do()
                    {
                        var n = 0;

                        var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
                        var f = new Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22) => { n++; return ""; });

                        var res = mem.MemoizeWeak<Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>>(f, MemoizationOptions.None);

                        Assert.AreEqual(0, n);

                        Assert.AreEqual("", Apply(res.Delegate));
                        Assert.AreEqual(1, n);
                    }

                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static string Apply(Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string> f)
                    {
                        return f(new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj());
                    }

                    Do();

                    CollectAndCheckFinalizeCount(23);
                }
                finally
                {
                    Obj.Reset();
                }
            }
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed1_Func23()
        {
            // 12 int; 12 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, string, int, int, int, int, string, string, string, string, string, string, int, int, string, int, string, string, int, string, int, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22; });

            var res = mem.MemoizeWeak<Func<int, int, string, int, int, int, int, string, string, string, string, string, string, int, int, string, int, string, string, int, string, int, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("986471684464517830394182729282626803771813", res.Delegate(98, 6, "47", 16, 84, 4, 64, "51", "78", "30", "39", "41", "82", 72, 92, "82", 6, "26", "80", 37, "71", 8, 13));
            Assert.AreEqual(1, n);

            Assert.AreEqual("986471684464517830394182729282626803771813", res.Delegate(98, 6, "47", 16, 84, 4, 64, "51", "78", "30", "39", "41", "82", 72, 92, "82", 6, "26", "80", 37, "71", 8, 13));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed2_Func23()
        {
            // 15 int; 9 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int, int, int, int, string, string, string, string, int, string, int, int, int, string, int, string, int, string, int, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22; });

            var res = mem.MemoizeWeak<Func<int, int, int, int, int, int, int, string, string, string, string, int, string, int, int, int, string, int, string, int, string, int, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("38595107292459168992574375725137996213916320", res.Delegate(38, 59, 51, 0, 72, 9, 24, "59", "16", "89", "92", 57, "43", 75, 72, 51, "37", 99, "62", 13, "91", 63, 20));
            Assert.AreEqual(1, n);

            Assert.AreEqual("38595107292459168992574375725137996213916320", res.Delegate(38, 59, 51, 0, 72, 9, 24, "59", "16", "89", "92", 57, "43", 75, 72, 51, "37", 99, "62", 13, "91", 63, 20));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed3_Func23()
        {
            // 7 string; 17 int

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, int, int, int, int, int, string, string, int, int, string, int, int, string, int, int, int, int, int, string, int, int, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22; });

            var res = mem.MemoizeWeak<Func<string, int, int, int, int, int, string, string, int, int, string, int, int, string, int, int, int, int, int, string, int, int, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("659576518197459847439806574532438201541475", res.Delegate("65", 95, 7, 65, 18, 19, "74", "59", 84, 74, "39", 80, 65, "74", 53, 24, 38, 2, 0, "15", 41, 47, 5));
            Assert.AreEqual(1, n);

            Assert.AreEqual("659576518197459847439806574532438201541475", res.Delegate("65", 95, 7, 65, 18, 19, "74", "59", 84, 74, "39", 80, 65, "74", 53, 24, 38, 2, 0, "15", 41, 47, 5));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Func24()
        {
            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23; });

            var res = mem.MemoizeWeak<Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual("999025934120605841443260925495693561295338147", res.Delegate("99", "90", "25", "93", "4", "12", "0", "60", "58", "41", "44", "32", "60", "92", "54", "95", "69", "3", "56", "12", "95", "33", "81", "47"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("999025934120605841443260925495693561295338147", res.Delegate("99", "90", "25", "93", "4", "12", "0", "60", "58", "41", "44", "32", "60", "92", "54", "95", "69", "3", "56", "12", "95", "33", "81", "47"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Lifetime_Func24()
        {
            lock (typeof(Obj))
            {
                Obj.Reset();
                try
                {
                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static void Do()
                    {
                        var n = 0;

                        var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
                        var f = new Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23) => { n++; return ""; });

                        var res = mem.MemoizeWeak<Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>>(f, MemoizationOptions.None);

                        Assert.AreEqual(0, n);

                        Assert.AreEqual("", Apply(res.Delegate));
                        Assert.AreEqual(1, n);
                    }

                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static string Apply(Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string> f)
                    {
                        return f(new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj());
                    }

                    Do();

                    CollectAndCheckFinalizeCount(24);
                }
                finally
                {
                    Obj.Reset();
                }
            }
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed1_Func24()
        {
            // 11 int; 14 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, string, string, int, string, string, int, int, int, string, int, string, string, string, int, string, int, int, string, int, string, int, string, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23; });

            var res = mem.MemoizeWeak<Func<int, string, string, int, string, string, int, int, int, string, int, string, string, string, int, string, int, int, string, int, string, int, string, string, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("3392881524226891145679429164838042661563691234", res.Delegate(33, "9", "28", 81, "52", "42", 26, 89, 11, "45", 67, "94", "29", "1", 64, "83", 80, 42, "66", 15, "63", 69, "12", "34"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("3392881524226891145679429164838042661563691234", res.Delegate(33, "9", "28", 81, "52", "42", 26, 89, 11, "45", 67, "94", "29", "1", 64, "83", 80, 42, "66", 15, "63", 69, "12", "34"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed2_Func24()
        {
            // 18 int; 7 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, string, int, string, string, string, int, int, int, int, int, int, int, int, string, int, int, int, string, int, int, int, int, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23; });

            var res = mem.MemoizeWeak<Func<int, string, int, string, string, string, int, int, int, int, int, int, int, int, string, int, int, int, string, int, int, int, int, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("31319041138679649693276831162889763894122897651", res.Delegate(31, "31", 90, "41", "13", "86", 79, 64, 96, 93, 27, 68, 31, 1, "62", 88, 97, 63, "89", 41, 22, 89, 76, 51));
            Assert.AreEqual(1, n);

            Assert.AreEqual("31319041138679649693276831162889763894122897651", res.Delegate(31, "31", 90, "41", "13", "86", 79, 64, 96, 93, 27, 68, 31, 1, "62", 88, 97, 63, "89", 41, 22, 89, 76, 51));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed3_Func24()
        {
            // 17 int; 8 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, string, int, int, int, string, int, int, string, int, string, int, int, int, int, string, int, int, int, int, int, string, string, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23; });

            var res = mem.MemoizeWeak<Func<int, string, int, int, int, string, int, int, string, int, string, int, int, int, int, string, int, int, int, int, int, string, string, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("90921776263415439131322878474403952477655691", res.Delegate(9, "0", 92, 17, 76, "26", 3, 41, "54", 39, "13", 13, 22, 87, 84, "74", 40, 39, 52, 47, 7, "65", "56", 91));
            Assert.AreEqual(1, n);

            Assert.AreEqual("90921776263415439131322878474403952477655691", res.Delegate(9, "0", 92, 17, 76, "26", 3, 41, "54", 39, "13", 13, 22, 87, 84, "74", 40, 39, 52, 47, 7, "65", "56", 91));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Func25()
        {
            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23 + p24; });

            var res = mem.MemoizeWeak<Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual("7371328116349836613817930351189805242929377942129", res.Delegate("73", "71", "32", "8", "11", "63", "49", "83", "66", "13", "81", "79", "30", "35", "11", "89", "80", "52", "42", "92", "93", "77", "94", "21", "29"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("7371328116349836613817930351189805242929377942129", res.Delegate("73", "71", "32", "8", "11", "63", "49", "83", "66", "13", "81", "79", "30", "35", "11", "89", "80", "52", "42", "92", "93", "77", "94", "21", "29"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Lifetime_Func25()
        {
            lock (typeof(Obj))
            {
                Obj.Reset();
                try
                {
                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static void Do()
                    {
                        var n = 0;

                        var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
                        var f = new Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24) => { n++; return ""; });

                        var res = mem.MemoizeWeak<Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>>(f, MemoizationOptions.None);

                        Assert.AreEqual(0, n);

                        Assert.AreEqual("", Apply(res.Delegate));
                        Assert.AreEqual(1, n);
                    }

                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static string Apply(Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string> f)
                    {
                        return f(new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj());
                    }

                    Do();

                    CollectAndCheckFinalizeCount(25);
                }
                finally
                {
                    Obj.Reset();
                }
            }
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed1_Func25()
        {
            // 13 string; 13 int

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, string, int, int, string, int, string, string, string, string, int, int, string, int, string, int, string, int, int, int, int, int, int, string, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23 + p24; });

            var res = mem.MemoizeWeak<Func<string, string, int, int, string, int, string, string, string, string, int, int, string, int, string, int, string, int, int, int, int, int, int, string, string, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("633085152559673529198720793472782162299238673968", res.Delegate("63", "30", 85, 15, "25", 59, "67", "35", "29", "19", 87, 20, "79", 3, "47", 27, "82", 16, 22, 99, 2, 38, 67, "39", "68"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("633085152559673529198720793472782162299238673968", res.Delegate("63", "30", 85, 15, "25", 59, "67", "35", "29", "19", 87, 20, "79", 3, "47", 27, "82", 16, 22, 99, 2, 38, 67, "39", "68"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed2_Func25()
        {
            // 14 int; 12 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, string, string, string, string, string, string, string, int, string, int, int, int, int, string, int, int, int, int, int, string, int, string, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23 + p24; });

            var res = mem.MemoizeWeak<Func<int, int, string, string, string, string, string, string, string, int, string, int, int, int, int, string, int, int, int, int, int, string, int, string, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("1468995102462464756445514623854481768813686637576", res.Delegate(14, 68, "99", "51", "0", "24", "62", "46", "47", 56, "44", 55, 14, 62, 38, "54", 48, 17, 68, 81, 36, "86", 63, "75", 76));
            Assert.AreEqual(1, n);

            Assert.AreEqual("1468995102462464756445514623854481768813686637576", res.Delegate(14, 68, "99", "51", "0", "24", "62", "46", "47", 56, "44", 55, 14, 62, 38, "54", 48, 17, 68, 81, 36, "86", 63, "75", 76));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed3_Func25()
        {
            // 7 string; 19 int

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, int, int, int, int, int, int, int, int, string, string, int, string, int, int, string, int, int, int, int, int, int, int, string, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23 + p24; });

            var res = mem.MemoizeWeak<Func<string, int, int, int, int, int, int, int, int, string, string, int, string, int, int, string, int, int, int, int, int, int, int, string, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("6738576342285247550475590964455877210262028565877", res.Delegate("67", 38, 57, 63, 42, 28, 5, 24, 75, "50", "47", 55, "90", 96, 44, "55", 87, 72, 10, 26, 20, 28, 56, "58", 77));
            Assert.AreEqual(1, n);

            Assert.AreEqual("6738576342285247550475590964455877210262028565877", res.Delegate("67", 38, 57, 63, 42, 28, 5, 24, 75, "50", "47", 55, "90", 96, 44, "55", 87, 72, 10, 26, 20, 28, 56, "58", 77));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Func26()
        {
            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23 + p24 + p25; });

            var res = mem.MemoizeWeak<Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual("913766728127886268373324992469441062485452524779", res.Delegate("91", "37", "66", "72", "8", "12", "78", "86", "26", "83", "73", "32", "4", "99", "24", "6", "9", "44", "10", "62", "48", "54", "52", "52", "47", "79"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("913766728127886268373324992469441062485452524779", res.Delegate("91", "37", "66", "72", "8", "12", "78", "86", "26", "83", "73", "32", "4", "99", "24", "6", "9", "44", "10", "62", "48", "54", "52", "52", "47", "79"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Lifetime_Func26()
        {
            lock (typeof(Obj))
            {
                Obj.Reset();
                try
                {
                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static void Do()
                    {
                        var n = 0;

                        var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
                        var f = new Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25) => { n++; return ""; });

                        var res = mem.MemoizeWeak<Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>>(f, MemoizationOptions.None);

                        Assert.AreEqual(0, n);

                        Assert.AreEqual("", Apply(res.Delegate));
                        Assert.AreEqual(1, n);
                    }

                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static string Apply(Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string> f)
                    {
                        return f(new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj());
                    }

                    Do();

                    CollectAndCheckFinalizeCount(26);
                }
                finally
                {
                    Obj.Reset();
                }
            }
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed1_Func26()
        {
            // 10 string; 17 int

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, int, int, int, int, int, int, string, int, int, int, string, string, int, int, int, string, int, string, string, int, int, string, string, int, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23 + p24 + p25; });

            var res = mem.MemoizeWeak<Func<string, int, int, int, int, int, int, string, int, int, int, string, string, int, int, int, string, int, string, string, int, int, string, string, int, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("8224969117596543525515525253791777744687989642552", res.Delegate("82", 24, 96, 91, 1, 75, 96, "54", 35, 2, 55, "15", "5", 25, 25, 37, "91", 77, "77", "44", 68, 79, "89", "64", 25, 52));
            Assert.AreEqual(1, n);

            Assert.AreEqual("8224969117596543525515525253791777744687989642552", res.Delegate("82", 24, 96, 91, 1, 75, 96, "54", 35, 2, 55, "15", "5", 25, 25, 37, "91", 77, "77", "44", 68, 79, "89", "64", 25, 52));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed2_Func26()
        {
            // 17 int; 10 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, string, int, string, int, int, int, string, int, int, int, int, int, int, int, int, int, int, string, string, string, string, int, string, string, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23 + p24 + p25; });

            var res = mem.MemoizeWeak<Func<int, string, int, string, int, int, int, string, int, int, int, int, int, int, int, int, int, int, string, string, string, string, int, string, string, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("24671235326648234347610570609323535876022782357", res.Delegate(2, "46", 71, "2", 35, 32, 66, "48", 23, 43, 47, 61, 0, 5, 70, 60, 93, 23, "5", "35", "87", "60", 22, "78", "23", 57));
            Assert.AreEqual(1, n);

            Assert.AreEqual("24671235326648234347610570609323535876022782357", res.Delegate(2, "46", 71, "2", 35, 32, 66, "48", 23, 43, 47, 61, 0, 5, 70, 60, 93, 23, "5", "35", "87", "60", 22, "78", "23", 57));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed3_Func26()
        {
            // 21 int; 6 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int, string, string, int, int, int, int, int, string, int, int, int, int, int, int, int, int, int, int, int, int, string, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23 + p24 + p25; });

            var res = mem.MemoizeWeak<Func<int, int, int, int, string, string, int, int, int, int, int, string, int, int, int, int, int, int, int, int, int, int, int, int, string, string, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("344530412493787730742413238067165772876456447466644", res.Delegate(34, 45, 30, 41, "24", "93", 78, 77, 30, 74, 24, "13", 23, 80, 67, 16, 57, 72, 87, 64, 56, 4, 47, 46, "66", "44"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("344530412493787730742413238067165772876456447466644", res.Delegate(34, 45, 30, 41, "24", "93", 78, 77, 30, 74, 24, "13", 23, 80, 67, 16, 57, 72, 87, 64, 56, 4, 47, 46, "66", "44"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Func27()
        {
            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25, p26) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23 + p24 + p25 + p26; });

            var res = mem.MemoizeWeak<Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual("36292643295760103248536737950886914167954612613998979", res.Delegate("36", "29", "26", "43", "29", "57", "60", "10", "32", "48", "53", "6", "73", "79", "50", "88", "69", "14", "16", "79", "54", "61", "26", "13", "99", "89", "79"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("36292643295760103248536737950886914167954612613998979", res.Delegate("36", "29", "26", "43", "29", "57", "60", "10", "32", "48", "53", "6", "73", "79", "50", "88", "69", "14", "16", "79", "54", "61", "26", "13", "99", "89", "79"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Lifetime_Func27()
        {
            lock (typeof(Obj))
            {
                Obj.Reset();
                try
                {
                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static void Do()
                    {
                        var n = 0;

                        var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
                        var f = new Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25, p26) => { n++; return ""; });

                        var res = mem.MemoizeWeak<Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>>(f, MemoizationOptions.None);

                        Assert.AreEqual(0, n);

                        Assert.AreEqual("", Apply(res.Delegate));
                        Assert.AreEqual(1, n);
                    }

                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static string Apply(Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string> f)
                    {
                        return f(new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj());
                    }

                    Do();

                    CollectAndCheckFinalizeCount(27);
                }
                finally
                {
                    Obj.Reset();
                }
            }
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed1_Func27()
        {
            // 15 int; 13 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, string, string, string, int, int, int, int, string, int, string, string, string, int, int, string, int, int, string, int, int, int, string, string, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25, p26) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23 + p24 + p25 + p26; });

            var res = mem.MemoizeWeak<Func<int, int, int, string, string, string, int, int, int, int, string, int, string, string, string, int, int, string, int, int, string, int, int, int, string, string, string, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("7069849464548889824538424887655456892204861230516379", res.Delegate(70, 69, 84, "94", "64", "54", 88, 89, 82, 45, "38", 42, "4", "88", "76", 55, 45, "68", 92, 20, "4", 86, 12, 30, "51", "63", "79"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("7069849464548889824538424887655456892204861230516379", res.Delegate(70, 69, 84, "94", "64", "54", 88, 89, 82, 45, "38", 42, "4", "88", "76", 55, 45, "68", 92, 20, "4", 86, 12, 30, "51", "63", "79"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed2_Func27()
        {
            // 19 int; 9 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, string, int, string, int, int, int, string, string, string, int, int, int, int, string, int, string, int, int, int, int, string, int, int, int, int, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25, p26) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23 + p24 + p25 + p26; });

            var res = mem.MemoizeWeak<Func<int, string, int, string, int, int, int, string, string, string, int, int, int, int, string, int, string, int, int, int, int, string, int, int, int, int, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("8584575442074274121497647558169823682767714756301198", res.Delegate(85, "84", 57, "54", 42, 0, 74, "27", "41", "21", 49, 76, 47, 55, "81", 69, "82", 36, 82, 76, 77, "1", 47, 56, 30, 11, 98));
            Assert.AreEqual(1, n);

            Assert.AreEqual("8584575442074274121497647558169823682767714756301198", res.Delegate(85, "84", 57, "54", 42, 0, 74, "27", "41", "21", 49, 76, 47, 55, "81", 69, "82", 36, 82, 76, 77, "1", 47, 56, 30, 11, 98));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed3_Func27()
        {
            // 12 string; 16 int

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, int, int, int, string, string, string, int, string, int, int, int, int, string, string, int, int, int, int, int, int, string, string, string, int, int, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25, p26) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23 + p24 + p25 + p26; });

            var res = mem.MemoizeWeak<Func<string, int, int, int, string, string, string, int, string, int, int, int, int, string, string, int, int, int, int, int, int, string, string, string, int, int, string, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("498436024301561679289073799258206470963562383050818", res.Delegate("49", 84, 36, 0, "24", "30", "1", 56, "16", 79, 28, 90, 73, "79", "92", 58, 20, 64, 70, 96, 35, "62", "38", "30", 50, 8, "18"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("498436024301561679289073799258206470963562383050818", res.Delegate("49", 84, 36, 0, "24", "30", "1", 56, "16", 79, 28, 90, 73, "79", "92", 58, 20, 64, 70, 96, 35, "62", "38", "30", 50, 8, "18"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Func28()
        {
            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25, p26, p27) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23 + p24 + p25 + p26 + p27; });

            var res = mem.MemoizeWeak<Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual("8937305961322177885499434848598838816491316225938605845", res.Delegate("89", "37", "30", "59", "61", "3", "22", "17", "78", "85", "49", "94", "34", "84", "85", "98", "83", "88", "16", "49", "13", "16", "22", "59", "38", "60", "58", "45"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("8937305961322177885499434848598838816491316225938605845", res.Delegate("89", "37", "30", "59", "61", "3", "22", "17", "78", "85", "49", "94", "34", "84", "85", "98", "83", "88", "16", "49", "13", "16", "22", "59", "38", "60", "58", "45"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Lifetime_Func28()
        {
            lock (typeof(Obj))
            {
                Obj.Reset();
                try
                {
                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static void Do()
                    {
                        var n = 0;

                        var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
                        var f = new Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25, p26, p27) => { n++; return ""; });

                        var res = mem.MemoizeWeak<Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>>(f, MemoizationOptions.None);

                        Assert.AreEqual(0, n);

                        Assert.AreEqual("", Apply(res.Delegate));
                        Assert.AreEqual(1, n);
                    }

                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static string Apply(Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string> f)
                    {
                        return f(new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj());
                    }

                    Do();

                    CollectAndCheckFinalizeCount(28);
                }
                finally
                {
                    Obj.Reset();
                }
            }
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed1_Func28()
        {
            // 14 int; 15 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, string, int, int, string, string, int, string, string, int, int, int, string, string, int, string, string, string, int, int, int, int, string, string, string, int, string, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25, p26, p27) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23 + p24 + p25 + p26 + p27; });

            var res = mem.MemoizeWeak<Func<int, string, int, int, string, string, int, string, string, int, int, int, string, string, int, string, string, string, int, int, int, int, string, string, string, int, string, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("90641656738414996741733182927291133984509221459527164", res.Delegate(90, "64", 16, 56, "7", "38", 4, "14", "99", 67, 41, 73, "31", "82", 92, "72", "91", "13", 39, 84, 50, 92, "21", "4", "59", 52, "71", 64));
            Assert.AreEqual(1, n);

            Assert.AreEqual("90641656738414996741733182927291133984509221459527164", res.Delegate(90, "64", 16, 56, "7", "38", 4, "14", "99", 67, 41, 73, "31", "82", 92, "72", "91", "13", 39, 84, 50, 92, "21", "4", "59", 52, "71", 64));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed2_Func28()
        {
            // 17 int; 12 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int, int, string, string, int, string, int, int, string, string, int, string, string, string, string, int, int, string, string, int, int, int, int, int, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25, p26, p27) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23 + p24 + p25 + p26 + p27; });

            var res = mem.MemoizeWeak<Func<int, int, int, int, int, string, string, int, string, int, int, string, string, int, string, string, string, string, int, int, string, string, int, int, int, int, int, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("431196556739324891429225407789709852251296716343151766", res.Delegate(43, 11, 96, 55, 67, "39", "32", 48, "91", 42, 92, "25", "40", 77, "89", "70", "98", "5", 22, 51, "29", "67", 16, 34, 31, 51, 7, 66));
            Assert.AreEqual(1, n);

            Assert.AreEqual("431196556739324891429225407789709852251296716343151766", res.Delegate(43, 11, 96, 55, 67, "39", "32", 48, "91", 42, 92, "25", "40", 77, "89", "70", "98", "5", 22, 51, "29", "67", 16, 34, 31, 51, 7, 66));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed3_Func28()
        {
            // 13 string; 16 int

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, int, string, int, int, int, int, string, int, int, string, string, string, int, int, string, string, int, int, string, string, int, string, int, string, int, int, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25, p26, p27) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23 + p24 + p25 + p26 + p27; });

            var res = mem.MemoizeWeak<Func<string, int, string, int, int, int, int, string, int, int, string, string, string, int, int, string, string, int, int, string, string, int, string, int, string, int, int, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("9562231532667228492193976184638390861457013472273969", res.Delegate("95", 62, "23", 15, 32, 66, 72, "28", 49, 21, "93", "97", "6", 18, 46, "38", "39", 0, 86, "14", "5", 70, "1", 34, "72", 27, 39, 69));
            Assert.AreEqual(1, n);

            Assert.AreEqual("9562231532667228492193976184638390861457013472273969", res.Delegate("95", 62, "23", 15, 32, 66, 72, "28", 49, 21, "93", "97", "6", 18, 46, "38", "39", 0, 86, "14", "5", 70, "1", 34, "72", 27, 39, 69));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Func29()
        {
            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25, p26, p27, p28) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23 + p24 + p25 + p26 + p27 + p28; });

            var res = mem.MemoizeWeak<Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual("4992819759575623650899942339611281738597560873653702560", res.Delegate("49", "92", "81", "9", "75", "95", "75", "62", "36", "50", "89", "99", "42", "33", "96", "11", "2", "81", "73", "8", "59", "75", "60", "87", "36", "53", "70", "25", "60"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("4992819759575623650899942339611281738597560873653702560", res.Delegate("49", "92", "81", "9", "75", "95", "75", "62", "36", "50", "89", "99", "42", "33", "96", "11", "2", "81", "73", "8", "59", "75", "60", "87", "36", "53", "70", "25", "60"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Lifetime_Func29()
        {
            lock (typeof(Obj))
            {
                Obj.Reset();
                try
                {
                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static void Do()
                    {
                        var n = 0;

                        var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
                        var f = new Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25, p26, p27, p28) => { n++; return ""; });

                        var res = mem.MemoizeWeak<Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>>(f, MemoizationOptions.None);

                        Assert.AreEqual(0, n);

                        Assert.AreEqual("", Apply(res.Delegate));
                        Assert.AreEqual(1, n);
                    }

                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static string Apply(Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string> f)
                    {
                        return f(new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj());
                    }

                    Do();

                    CollectAndCheckFinalizeCount(29);
                }
                finally
                {
                    Obj.Reset();
                }
            }
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed1_Func29()
        {
            // 19 string; 11 int

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, string, int, string, string, string, string, string, int, int, string, int, string, int, int, string, string, string, string, string, int, string, string, int, int, string, int, string, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25, p26, p27, p28) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23 + p24 + p25 + p26 + p27 + p28; });

            var res = mem.MemoizeWeak<Func<string, string, int, string, string, string, string, string, int, int, string, int, string, int, int, string, string, string, string, string, int, string, string, int, int, string, int, string, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("94935264611988592825481271234163619133191653305917224225", res.Delegate("94", "93", 5, "26", "46", "11", "98", "85", 92, 82, "54", 81, "27", 12, 34, "16", "36", "19", "13", "31", 91, "65", "33", 0, 59, "17", 22, "42", 25));
            Assert.AreEqual(1, n);

            Assert.AreEqual("94935264611988592825481271234163619133191653305917224225", res.Delegate("94", "93", 5, "26", "46", "11", "98", "85", 92, 82, "54", 81, "27", 12, 34, "16", "36", "19", "13", "31", 91, "65", "33", 0, 59, "17", 22, "42", 25));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed2_Func29()
        {
            // 19 int; 11 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, string, int, int, string, int, int, string, int, int, string, string, int, string, string, int, int, int, string, string, int, int, int, int, string, int, int, int, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25, p26, p27, p28) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23 + p24 + p25 + p26 + p27 + p28; });

            var res = mem.MemoizeWeak<Func<int, string, int, int, string, int, int, string, int, int, string, string, int, string, string, int, int, int, string, string, int, int, int, int, string, int, int, int, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("662988565992526425943489672495669139241817069859542847947", res.Delegate(66, "29", 88, 56, "59", 92, 52, "64", 25, 94, "34", "89", 67, "24", "95", 6, 69, 13, "92", "41", 81, 70, 69, 85, "95", 42, 84, 79, 47));
            Assert.AreEqual(1, n);

            Assert.AreEqual("662988565992526425943489672495669139241817069859542847947", res.Delegate(66, "29", 88, 56, "59", 92, 52, "64", 25, 94, "34", "89", 67, "24", "95", 6, 69, 13, "92", "41", 81, 70, 69, 85, "95", 42, 84, 79, 47));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed3_Func29()
        {
            // 19 int; 11 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int, string, int, string, int, int, string, int, string, int, string, int, string, int, string, int, string, int, int, int, string, int, int, int, string, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25, p26, p27, p28) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23 + p24 + p25 + p26 + p27 + p28; });

            var res = mem.MemoizeWeak<Func<int, int, int, int, string, int, string, int, int, string, int, string, int, string, int, string, int, string, int, string, int, int, int, string, int, int, int, string, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("60177721789208542379087649511027615833827942310862812", res.Delegate(60, 17, 7, 72, "17", 89, "20", 85, 4, "23", 79, "0", 87, "64", 95, "11", 0, "27", 6, "15", 83, 38, 27, "94", 23, 10, 86, "28", 12));
            Assert.AreEqual(1, n);

            Assert.AreEqual("60177721789208542379087649511027615833827942310862812", res.Delegate(60, 17, 7, 72, "17", 89, "20", 85, 4, "23", 79, "0", 87, "64", 95, "11", 0, "27", 6, "15", 83, 38, 27, "94", 23, 10, 86, "28", 12));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Func30()
        {
            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25, p26, p27, p28, p29) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23 + p24 + p25 + p26 + p27 + p28 + p29; });

            var res = mem.MemoizeWeak<Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual("65204834164407267377333175291115549854924189690441922891349", res.Delegate("65", "20", "48", "34", "16", "44", "0", "72", "67", "37", "73", "33", "17", "52", "91", "11", "55", "49", "85", "49", "24", "18", "96", "90", "44", "19", "22", "89", "13", "49"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("65204834164407267377333175291115549854924189690441922891349", res.Delegate("65", "20", "48", "34", "16", "44", "0", "72", "67", "37", "73", "33", "17", "52", "91", "11", "55", "49", "85", "49", "24", "18", "96", "90", "44", "19", "22", "89", "13", "49"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Lifetime_Func30()
        {
            lock (typeof(Obj))
            {
                Obj.Reset();
                try
                {
                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static void Do()
                    {
                        var n = 0;

                        var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
                        var f = new Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25, p26, p27, p28, p29) => { n++; return ""; });

                        var res = mem.MemoizeWeak<Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>>(f, MemoizationOptions.None);

                        Assert.AreEqual(0, n);

                        Assert.AreEqual("", Apply(res.Delegate));
                        Assert.AreEqual(1, n);
                    }

                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static string Apply(Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string> f)
                    {
                        return f(new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj());
                    }

                    Do();

                    CollectAndCheckFinalizeCount(30);
                }
                finally
                {
                    Obj.Reset();
                }
            }
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed1_Func30()
        {
            // 16 string; 15 int

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, int, string, string, int, string, string, int, int, string, int, string, string, string, int, string, int, string, int, string, int, string, string, string, int, int, int, int, int, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25, p26, p27, p28, p29) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23 + p24 + p25 + p26 + p27 + p28 + p29; });

            var res = mem.MemoizeWeak<Func<string, int, string, string, int, string, string, int, int, string, int, string, string, string, int, string, int, string, int, string, int, string, string, string, int, int, int, int, int, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("797276729984791459613921724727981362426217026448457283784", res.Delegate("79", 72, "76", "72", 99, "84", "79", 14, 59, "61", 39, "21", "72", "47", 2, "79", 81, "36", 24, "2", 62, "17", "0", "26", 44, 84, 57, 28, 37, 84));
            Assert.AreEqual(1, n);

            Assert.AreEqual("797276729984791459613921724727981362426217026448457283784", res.Delegate("79", 72, "76", "72", 99, "84", "79", 14, 59, "61", 39, "21", "72", "47", 2, "79", 81, "36", 24, "2", 62, "17", "0", "26", 44, 84, 57, 28, 37, 84));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed2_Func30()
        {
            // 19 int; 12 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int, int, string, string, int, int, string, int, int, string, int, int, string, int, string, int, int, string, string, int, string, int, string, string, int, int, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25, p26, p27, p28, p29) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23 + p24 + p25 + p26 + p27 + p28 + p29; });

            var res = mem.MemoizeWeak<Func<int, int, int, int, int, string, string, int, int, string, int, int, string, int, int, string, int, string, int, int, string, string, int, string, int, string, string, int, int, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("275177749310604937956775715497907043132050993521240799347", res.Delegate(27, 51, 77, 74, 93, "10", "60", 49, 37, "95", 67, 75, "71", 54, 97, "90", 70, "4", 3, 13, "20", "50", 99, "35", 21, "2", "40", 79, 93, 47));
            Assert.AreEqual(1, n);

            Assert.AreEqual("275177749310604937956775715497907043132050993521240799347", res.Delegate(27, 51, 77, 74, 93, "10", "60", 49, 37, "95", 67, 75, "71", 54, 97, "90", 70, "4", 3, 13, "20", "50", 99, "35", 21, "2", "40", 79, 93, 47));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed3_Func30()
        {
            // 24 int; 7 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int, string, int, int, int, string, int, string, int, int, int, int, int, int, int, int, int, int, int, int, int, string, string, int, int, int, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25, p26, p27, p28, p29) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23 + p24 + p25 + p26 + p27 + p28 + p29; });

            var res = mem.MemoizeWeak<Func<int, int, int, int, string, int, int, int, string, int, string, int, int, int, int, int, int, int, int, int, int, int, int, int, string, string, int, int, int, string, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("708155896059720124297982904358166133869555566615635666", res.Delegate(70, 81, 55, 89, "60", 5, 97, 20, "12", 42, "97", 98, 2, 90, 43, 5, 81, 66, 1, 33, 8, 6, 95, 55, "56", "66", 15, 63, 56, "66"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("708155896059720124297982904358166133869555566615635666", res.Delegate(70, 81, 55, 89, "60", 5, 97, 20, "12", 42, "97", 98, 2, 90, 43, 5, 81, 66, 1, 33, 8, 6, 95, 55, "56", "66", 15, 63, 56, "66"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Func31()
        {
            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25, p26, p27, p28, p29, p30) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23 + p24 + p25 + p26 + p27 + p28 + p29 + p30; });

            var res = mem.MemoizeWeak<Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual("352181042885979732890722141409266567089948539208264850339489", res.Delegate("3", "52", "18", "10", "42", "88", "59", "79", "73", "28", "90", "72", "21", "41", "40", "92", "66", "56", "70", "89", "94", "85", "39", "20", "82", "64", "85", "0", "33", "94", "89"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("352181042885979732890722141409266567089948539208264850339489", res.Delegate("3", "52", "18", "10", "42", "88", "59", "79", "73", "28", "90", "72", "21", "41", "40", "92", "66", "56", "70", "89", "94", "85", "39", "20", "82", "64", "85", "0", "33", "94", "89"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Lifetime_Func31()
        {
            lock (typeof(Obj))
            {
                Obj.Reset();
                try
                {
                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static void Do()
                    {
                        var n = 0;

                        var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
                        var f = new Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25, p26, p27, p28, p29, p30) => { n++; return ""; });

                        var res = mem.MemoizeWeak<Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>>(f, MemoizationOptions.None);

                        Assert.AreEqual(0, n);

                        Assert.AreEqual("", Apply(res.Delegate));
                        Assert.AreEqual(1, n);
                    }

                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static string Apply(Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string> f)
                    {
                        return f(new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj());
                    }

                    Do();

                    CollectAndCheckFinalizeCount(31);
                }
                finally
                {
                    Obj.Reset();
                }
            }
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed1_Func31()
        {
            // 17 string; 15 int

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, int, int, int, int, string, string, string, int, string, int, int, string, string, string, string, string, int, int, int, int, string, int, int, int, string, string, string, int, string, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25, p26, p27, p28, p29, p30) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23 + p24 + p25 + p26 + p27 + p28 + p29 + p30; });

            var res = mem.MemoizeWeak<Func<string, int, int, int, int, string, string, string, int, string, int, int, string, string, string, string, string, int, int, int, int, string, int, int, int, string, string, string, int, string, string, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("69518656143386278521850244978247825462182987273768481612079", res.Delegate("69", 51, 86, 56, 14, "33", "86", "27", 8, "52", 18, 50, "24", "49", "78", "24", "78", 25, 46, 21, 82, "98", 72, 73, 76, "84", "8", "16", 12, "0", "79"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("69518656143386278521850244978247825462182987273768481612079", res.Delegate("69", 51, 86, 56, 14, "33", "86", "27", 8, "52", 18, 50, "24", "49", "78", "24", "78", 25, 46, 21, 82, "98", 72, 73, 76, "84", "8", "16", 12, "0", "79"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed2_Func31()
        {
            // 26 int; 6 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int, int, int, int, int, int, int, int, int, int, string, int, int, int, int, int, int, string, int, string, string, int, int, int, int, int, string, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25, p26, p27, p28, p29, p30) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23 + p24 + p25 + p26 + p27 + p28 + p29 + p30; });

            var res = mem.MemoizeWeak<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, string, int, int, int, int, int, int, string, int, string, string, int, int, int, int, int, string, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("1477253191291468424813316427955581427544048479713269854256", res.Delegate(14, 7, 72, 53, 19, 12, 9, 14, 68, 42, 48, 13, 31, "64", 27, 95, 5, 58, 14, 27, "54", 40, "4", "84", 79, 71, 32, 69, 85, "42", 56));
            Assert.AreEqual(1, n);

            Assert.AreEqual("1477253191291468424813316427955581427544048479713269854256", res.Delegate(14, 7, 72, 53, 19, 12, 9, 14, 68, 42, 48, 13, 31, "64", 27, 95, 5, 58, 14, 27, "54", 40, "4", "84", 79, 71, 32, 69, 85, "42", 56));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed3_Func31()
        {
            // 26 int; 6 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int, int, int, int, int, int, string, int, int, int, int, string, int, int, int, string, int, string, int, int, int, int, int, string, int, int, int, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25, p26, p27, p28, p29, p30) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23 + p24 + p25 + p26 + p27 + p28 + p29 + p30; });

            var res = mem.MemoizeWeak<Func<int, int, int, int, int, int, int, int, int, string, int, int, int, int, string, int, int, int, string, int, string, int, int, int, int, int, string, int, int, int, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("2825851612606390915957556365037842172623435319229601640", res.Delegate(28, 25, 85, 1, 6, 12, 60, 63, 90, "91", 59, 57, 55, 6, "36", 50, 37, 8, "42", 17, "26", 2, 34, 35, 31, 9, "2", 29, 60, 16, 40));
            Assert.AreEqual(1, n);

            Assert.AreEqual("2825851612606390915957556365037842172623435319229601640", res.Delegate(28, 25, 85, 1, 6, 12, 60, 63, 90, "91", 59, 57, 55, 6, "36", 50, 37, 8, "42", 17, "26", 2, 34, 35, 31, 9, "2", 29, 60, 16, 40));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Func32()
        {
            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25, p26, p27, p28, p29, p30, p31) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23 + p24 + p25 + p26 + p27 + p28 + p29 + p30 + p31; });

            var res = mem.MemoizeWeak<Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual("507731197868456415547992176257429375251655621325915069969312", res.Delegate("50", "77", "31", "19", "78", "68", "45", "64", "15", "54", "79", "92", "17", "62", "57", "42", "9", "37", "52", "51", "65", "5", "62", "13", "25", "91", "50", "69", "96", "9", "31", "2"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("507731197868456415547992176257429375251655621325915069969312", res.Delegate("50", "77", "31", "19", "78", "68", "45", "64", "15", "54", "79", "92", "17", "62", "57", "42", "9", "37", "52", "51", "65", "5", "62", "13", "25", "91", "50", "69", "96", "9", "31", "2"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Lifetime_Func32()
        {
            lock (typeof(Obj))
            {
                Obj.Reset();
                try
                {
                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static void Do()
                    {
                        var n = 0;

                        var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
                        var f = new Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25, p26, p27, p28, p29, p30, p31) => { n++; return ""; });

                        var res = mem.MemoizeWeak<Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>>(f, MemoizationOptions.None);

                        Assert.AreEqual(0, n);

                        Assert.AreEqual("", Apply(res.Delegate));
                        Assert.AreEqual(1, n);
                    }

                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static string Apply(Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string> f)
                    {
                        return f(new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj());
                    }

                    Do();

                    CollectAndCheckFinalizeCount(32);
                }
                finally
                {
                    Obj.Reset();
                }
            }
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed1_Func32()
        {
            // 20 int; 13 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, string, string, int, int, int, string, string, int, int, int, int, int, string, string, string, int, int, int, int, string, int, string, int, int, string, string, int, int, int, string, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25, p26, p27, p28, p29, p30, p31) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23 + p24 + p25 + p26 + p27 + p28 + p29 + p30 + p31; });

            var res = mem.MemoizeWeak<Func<int, string, string, int, int, int, string, string, int, int, int, int, int, string, string, string, int, int, int, int, string, int, string, int, int, string, string, int, int, int, string, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("7520386990491996497012394536614517712408139898382991901213288", res.Delegate(75, "20", "38", 69, 90, 49, "19", "96", 49, 70, 12, 39, 4, "53", "66", "14", 51, 77, 12, 40, "81", 39, "89", 83, 82, "9", "91", 90, 12, 13, "28", 8));
            Assert.AreEqual(1, n);

            Assert.AreEqual("7520386990491996497012394536614517712408139898382991901213288", res.Delegate(75, "20", "38", 69, 90, 49, "19", "96", 49, 70, 12, 39, 4, "53", "66", "14", 51, 77, 12, 40, "81", 39, "89", 83, 82, "9", "91", 90, 12, 13, "28", 8));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed2_Func32()
        {
            // 21 int; 12 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, string, int, int, int, string, int, int, int, string, int, int, int, int, string, int, int, int, string, int, string, string, int, int, int, string, string, string, int, int, int, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25, p26, p27, p28, p29, p30, p31) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23 + p24 + p25 + p26 + p27 + p28 + p29 + p30 + p31; });

            var res = mem.MemoizeWeak<Func<int, string, int, int, int, string, int, int, int, string, int, int, int, int, string, int, int, int, string, int, string, string, int, int, int, string, string, string, int, int, int, string, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("42434587953525288828259425627470936259124979121025612446728", res.Delegate(42, "4", 34, 58, 7, "9", 53, 52, 52, "88", 82, 82, 59, 42, "56", 27, 4, 70, "93", 62, "59", "12", 49, 79, 12, "10", "25", "6", 12, 44, 67, "28"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("42434587953525288828259425627470936259124979121025612446728", res.Delegate(42, "4", 34, 58, 7, "9", 53, 52, 52, "88", 82, 82, 59, 42, "56", 27, 4, 70, "93", 62, "59", "12", 49, 79, 12, "10", "25", "6", 12, 44, 67, "28"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed3_Func32()
        {
            // 27 int; 6 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, int, int, int, int, int, int, int, int, string, int, string, int, int, int, string, int, int, int, int, int, string, int, int, int, int, int, int, int, string, int, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25, p26, p27, p28, p29, p30, p31) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23 + p24 + p25 + p26 + p27 + p28 + p29 + p30 + p31; });

            var res = mem.MemoizeWeak<Func<int, int, int, int, int, int, int, int, int, string, int, string, int, int, int, string, int, int, int, int, int, string, int, int, int, int, int, int, int, string, int, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("21536714383375756565584979376233369536779929367746865117451", res.Delegate(21, 53, 67, 14, 38, 33, 7, 57, 56, "5", 65, "58", 49, 79, 37, "62", 33, 36, 9, 53, 67, "79", 92, 93, 67, 74, 68, 65, 1, "17", 4, 51));
            Assert.AreEqual(1, n);

            Assert.AreEqual("21536714383375756565584979376233369536779929367746865117451", res.Delegate(21, 53, 67, 14, 38, 33, 7, 57, 56, "5", 65, "58", 49, 79, 37, "62", 33, 36, 9, 53, 67, "79", 92, 93, 67, 74, 68, 65, 1, "17", 4, 51));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Func33()
        {
            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25, p26, p27, p28, p29, p30, p31, p32) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23 + p24 + p25 + p26 + p27 + p28 + p29 + p30 + p31 + p32; });

            var res = mem.MemoizeWeak<Func<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>>(f, MemoizationOptions.None);

            Assert.AreEqual(0, n);

            Assert.AreEqual("60544988826150645118302955188074178262463787404160734764654156391", res.Delegate("60", "54", "49", "88", "82", "61", "50", "64", "51", "18", "30", "29", "55", "18", "80", "74", "17", "82", "62", "46", "37", "87", "40", "41", "60", "73", "47", "64", "65", "41", "56", "39", "1"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("60544988826150645118302955188074178262463787404160734764654156391", res.Delegate("60", "54", "49", "88", "82", "61", "50", "64", "51", "18", "30", "29", "55", "18", "80", "74", "17", "82", "62", "46", "37", "87", "40", "41", "60", "73", "47", "64", "65", "41", "56", "39", "1"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Lifetime_Func33()
        {
            lock (typeof(Obj))
            {
                Obj.Reset();
                try
                {
                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static void Do()
                    {
                        var n = 0;

                        var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
                        var f = new Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25, p26, p27, p28, p29, p30, p31, p32) => { n++; return ""; });

                        var res = mem.MemoizeWeak<Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string>>(f, MemoizationOptions.None);

                        Assert.AreEqual(0, n);

                        Assert.AreEqual("", Apply(res.Delegate));
                        Assert.AreEqual(1, n);
                    }

                    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
                    static string Apply(Func<Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, Obj, string> f)
                    {
                        return f(new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj(), new Obj());
                    }

                    Do();

                    CollectAndCheckFinalizeCount(33);
                }
                finally
                {
                    Obj.Reset();
                }
            }
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed1_Func33()
        {
            // 13 string; 21 int

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, int, int, string, int, string, int, string, string, int, int, string, string, int, int, int, string, int, int, int, string, int, string, int, int, int, int, int, string, int, int, string, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25, p26, p27, p28, p29, p30, p31, p32) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23 + p24 + p25 + p26 + p27 + p28 + p29 + p30 + p31 + p32; });

            var res = mem.MemoizeWeak<Func<string, int, int, string, int, string, int, string, string, int, int, string, string, int, int, int, string, int, int, int, string, int, string, int, int, int, int, int, string, int, int, string, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("573596297507815649621882216307317162816247834195521493132614692", res.Delegate("57", 35, 96, "29", 7, "50", 78, "15", "64", 96, 21, "88", "22", 16, 30, 73, "17", 16, 28, 16, "24", 7, "83", 41, 95, 5, 21, 49, "31", 32, 61, "46", 92));
            Assert.AreEqual(1, n);

            Assert.AreEqual("573596297507815649621882216307317162816247834195521493132614692", res.Delegate("57", 35, 96, "29", 7, "50", 78, "15", "64", 96, 21, "88", "22", 16, 30, 73, "17", 16, 28, 16, "24", 7, "83", 41, 95, 5, 21, 49, "31", 32, 61, "46", 92));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed2_Func33()
        {
            // 10 string; 24 int

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<string, int, int, string, string, int, int, int, int, int, int, int, string, int, int, string, int, int, int, int, string, int, int, int, int, int, string, string, int, int, int, int, string, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25, p26, p27, p28, p29, p30, p31, p32) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23 + p24 + p25 + p26 + p27 + p28 + p29 + p30 + p31 + p32; });

            var res = mem.MemoizeWeak<Func<string, int, int, string, string, int, int, int, int, int, int, int, string, int, int, string, int, int, int, int, string, int, int, int, int, int, string, string, int, int, int, int, string, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("961882673349527128734625946155933927829163983108333619186378714", res.Delegate("96", 18, 82, "67", "33", 49, 52, 7, 1, 2, 87, 34, "62", 59, 46, "15", 59, 33, 92, 78, "29", 16, 39, 83, 10, 83, "33", "61", 91, 86, 37, 87, "14"));
            Assert.AreEqual(1, n);

            Assert.AreEqual("961882673349527128734625946155933927829163983108333619186378714", res.Delegate("96", 18, 82, "67", "33", 49, 52, 7, 1, 2, 87, 34, "62", 59, 46, "15", 59, 33, 92, 78, "29", 16, 39, 83, 10, 83, "33", "61", 91, 86, 37, 87, "14"));
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void FunctionMemoizationExtensions_MemoizeWeak_TDelegate_Mixed3_Func33()
        {
            // 21 int; 13 string

            var n = 0;

            var mem = Memoizer.CreateWeak(WeakMemoizationCacheFactory.Unbounded);
            var f = new Func<int, string, string, int, string, int, int, int, string, int, int, int, int, int, int, string, int, string, string, int, int, int, string, int, string, int, string, int, string, string, int, int, int, string>((p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, p24, p25, p26, p27, p28, p29, p30, p31, p32) => { n++; return "" + p0 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9 + p10 + p11 + p12 + p13 + p14 + p15 + p16 + p17 + p18 + p19 + p20 + p21 + p22 + p23 + p24 + p25 + p26 + p27 + p28 + p29 + p30 + p31 + p32; });

            var res = mem.MemoizeWeak<Func<int, string, string, int, string, int, int, int, string, int, int, int, int, int, int, string, int, string, string, int, int, int, string, int, string, int, string, int, string, string, int, int, int, string>>(f, MemoizationOptions.None, Memoizer.Create(MemoizationCacheFactory.Unbounded));

            Assert.AreEqual(0, n);

            Assert.AreEqual("205719691376924720427820939059677188748585960602365147440925996", res.Delegate(20, "57", "19", 6, "91", 37, 69, 2, "47", 20, 42, 78, 20, 93, 90, "59", 6, "77", "18", 87, 48, 58, "59", 60, "60", 23, "65", 14, "74", "40", 92, 59, 96));
            Assert.AreEqual(1, n);

            Assert.AreEqual("205719691376924720427820939059677188748585960602365147440925996", res.Delegate(20, "57", "19", 6, "91", 37, 69, 2, "47", 20, 42, 78, 20, 93, 90, "59", 6, "77", "18", 87, 48, 58, "59", 60, "60", 23, "65", 14, "74", "40", 92, 59, 96));
            Assert.AreEqual(1, n);
        }

        private static void CollectAndCheckFinalizeCount(int count)
        {
            // NB: This has shown to be flaky on Mono.
            if (Type.GetType("Mono.Runtime") == null)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();

                Assert.AreEqual(count, Obj.FinalizeCount);
            }
        }
    }

    internal delegate R Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, R>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17);
    internal delegate R Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, R>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18);
    internal delegate R Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, R>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18, T19 arg19);
    internal delegate R Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, R>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18, T19 arg19, T20 arg20);
    internal delegate R Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, R>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18, T19 arg19, T20 arg20, T21 arg21);
    internal delegate R Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, R>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18, T19 arg19, T20 arg20, T21 arg21, T22 arg22);
    internal delegate R Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, R>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18, T19 arg19, T20 arg20, T21 arg21, T22 arg22, T23 arg23);
    internal delegate R Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, R>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18, T19 arg19, T20 arg20, T21 arg21, T22 arg22, T23 arg23, T24 arg24);
    internal delegate R Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, R>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18, T19 arg19, T20 arg20, T21 arg21, T22 arg22, T23 arg23, T24 arg24, T25 arg25);
    internal delegate R Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, R>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18, T19 arg19, T20 arg20, T21 arg21, T22 arg22, T23 arg23, T24 arg24, T25 arg25, T26 arg26);
    internal delegate R Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, R>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18, T19 arg19, T20 arg20, T21 arg21, T22 arg22, T23 arg23, T24 arg24, T25 arg25, T26 arg26, T27 arg27);
    internal delegate R Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, R>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18, T19 arg19, T20 arg20, T21 arg21, T22 arg22, T23 arg23, T24 arg24, T25 arg25, T26 arg26, T27 arg27, T28 arg28);
    internal delegate R Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, R>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18, T19 arg19, T20 arg20, T21 arg21, T22 arg22, T23 arg23, T24 arg24, T25 arg25, T26 arg26, T27 arg27, T28 arg28, T29 arg29);
    internal delegate R Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, R>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18, T19 arg19, T20 arg20, T21 arg21, T22 arg22, T23 arg23, T24 arg24, T25 arg25, T26 arg26, T27 arg27, T28 arg28, T29 arg29, T30 arg30);
    internal delegate R Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, R>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18, T19 arg19, T20 arg20, T21 arg21, T22 arg22, T23 arg23, T24 arg24, T25 arg25, T26 arg26, T27 arg27, T28 arg28, T29 arg29, T30 arg30, T31 arg31);
    internal delegate R Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, R>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18, T19 arg19, T20 arg20, T21 arg21, T22 arg22, T23 arg23, T24 arg24, T25 arg25, T26 arg26, T27 arg27, T28 arg28, T29 arg29, T30 arg30, T31 arg31, T32 arg32);
    internal delegate R Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, R>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18, T19 arg19, T20 arg20, T21 arg21, T22 arg22, T23 arg23, T24 arg24, T25 arg25, T26 arg26, T27 arg27, T28 arg28, T29 arg29, T30 arg30, T31 arg31, T32 arg32, T33 arg33);

    internal sealed class Obj
    {
        public static int FinalizeCount;

        public static void Reset()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            FinalizeCount = 0;
        }

        ~Obj()
        {
            FinalizeCount++;
        }
    }
}
