// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Tests
{
    [TestClass]
    public class CombineWithTupletTests
    {
        [TestMethod]
        public void CombineWithTuplet2()
        {
            var eq = EqualityComparerExtensions.CombineWithTuplet<int, int>(EqualityComparer<int>.Default, EqualityComparer<int>.Default);

            var args1 = new Tuplet<int, int>(1493878331, 1431177679);
            var args2 = new Tuplet<int, int>(1849429681, 1751256899);

            Assert.IsTrue(eq.Equals(args1, args1));

            Assert.AreNotEqual(eq.GetHashCode(args1), eq.GetHashCode(args2));
            Assert.IsFalse(eq.Equals(args1, args2));
            Assert.IsFalse(eq.Equals(args2, args1));
        }

        [TestMethod]
        public void CombineWithTuplet3()
        {
            var eq = EqualityComparerExtensions.CombineWithTuplet<int, int, int>(EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default);

            var args1 = new Tuplet<int, int, int>(1078584633, 1069952473, 864272798);
            var args2 = new Tuplet<int, int, int>(606339120, 660923900, 1089055706);

            Assert.IsTrue(eq.Equals(args1, args1));

            Assert.AreNotEqual(eq.GetHashCode(args1), eq.GetHashCode(args2));
            Assert.IsFalse(eq.Equals(args1, args2));
            Assert.IsFalse(eq.Equals(args2, args1));
        }

        [TestMethod]
        public void CombineWithTuplet4()
        {
            var eq = EqualityComparerExtensions.CombineWithTuplet<int, int, int, int>(EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default);

            var args1 = new Tuplet<int, int, int, int>(490792769, 1340157553, 1821299733, 291436766);
            var args2 = new Tuplet<int, int, int, int>(56609777, 977758626, 932880312, 974984698);

            Assert.IsTrue(eq.Equals(args1, args1));

            Assert.AreNotEqual(eq.GetHashCode(args1), eq.GetHashCode(args2));
            Assert.IsFalse(eq.Equals(args1, args2));
            Assert.IsFalse(eq.Equals(args2, args1));
        }

        [TestMethod]
        public void CombineWithTuplet5()
        {
            var eq = EqualityComparerExtensions.CombineWithTuplet<int, int, int, int, int>(EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default);

            var args1 = new Tuplet<int, int, int, int, int>(768277902, 304671234, 642495331, 753171965, 276676877);
            var args2 = new Tuplet<int, int, int, int, int>(1686856785, 97220460, 2057705716, 1820941261, 795007812);

            Assert.IsTrue(eq.Equals(args1, args1));

            Assert.AreNotEqual(eq.GetHashCode(args1), eq.GetHashCode(args2));
            Assert.IsFalse(eq.Equals(args1, args2));
            Assert.IsFalse(eq.Equals(args2, args1));
        }

        [TestMethod]
        public void CombineWithTuplet6()
        {
            var eq = EqualityComparerExtensions.CombineWithTuplet<int, int, int, int, int, int>(EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default);

            var args1 = new Tuplet<int, int, int, int, int, int>(595253483, 234458665, 572489588, 1694483652, 1522560149, 1510948432);
            var args2 = new Tuplet<int, int, int, int, int, int>(137241414, 1508071593, 1389568860, 1703299212, 2009351709, 1956271747);

            Assert.IsTrue(eq.Equals(args1, args1));

            Assert.AreNotEqual(eq.GetHashCode(args1), eq.GetHashCode(args2));
            Assert.IsFalse(eq.Equals(args1, args2));
            Assert.IsFalse(eq.Equals(args2, args1));
        }

        [TestMethod]
        public void CombineWithTuplet7()
        {
            var eq = EqualityComparerExtensions.CombineWithTuplet<int, int, int, int, int, int, int>(EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default);

            var args1 = new Tuplet<int, int, int, int, int, int, int>(1817177677, 1375769249, 1472875537, 1455274364, 893442372, 222821153, 2139036453);
            var args2 = new Tuplet<int, int, int, int, int, int, int>(1122443037, 1875720571, 672543699, 1704580922, 982962167, 1675057635, 1027430421);

            Assert.IsTrue(eq.Equals(args1, args1));

            Assert.AreNotEqual(eq.GetHashCode(args1), eq.GetHashCode(args2));
            Assert.IsFalse(eq.Equals(args1, args2));
            Assert.IsFalse(eq.Equals(args2, args1));
        }

        [TestMethod]
        public void CombineWithTuplet8()
        {
            var eq = EqualityComparerExtensions.CombineWithTuplet<int, int, int, int, int, int, int, int>(EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default);

            var args1 = new Tuplet<int, int, int, int, int, int, int, int>(537590800, 740706366, 1154500802, 162572896, 1654036439, 1168362564, 1396494859, 69264986);
            var args2 = new Tuplet<int, int, int, int, int, int, int, int>(11085637, 426465235, 516566118, 943792764, 1965081051, 310351301, 154195352, 696021831);

            Assert.IsTrue(eq.Equals(args1, args1));

            Assert.AreNotEqual(eq.GetHashCode(args1), eq.GetHashCode(args2));
            Assert.IsFalse(eq.Equals(args1, args2));
            Assert.IsFalse(eq.Equals(args2, args1));
        }

        [TestMethod]
        public void CombineWithTuplet9()
        {
            var eq = EqualityComparerExtensions.CombineWithTuplet<int, int, int, int, int, int, int, int, int>(EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default);

            var args1 = new Tuplet<int, int, int, int, int, int, int, int, int>(1735673413, 1377064747, 1113116636, 959489802, 634977204, 1414209729, 1427780075, 968886160, 793414413);
            var args2 = new Tuplet<int, int, int, int, int, int, int, int, int>(2021882954, 2066152910, 698498224, 1066770888, 2070193431, 677361390, 1737011068, 19426017, 495129728);

            Assert.IsTrue(eq.Equals(args1, args1));

            Assert.AreNotEqual(eq.GetHashCode(args1), eq.GetHashCode(args2));
            Assert.IsFalse(eq.Equals(args1, args2));
            Assert.IsFalse(eq.Equals(args2, args1));
        }

        [TestMethod]
        public void CombineWithTuplet10()
        {
            var eq = EqualityComparerExtensions.CombineWithTuplet<int, int, int, int, int, int, int, int, int, int>(EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default);

            var args1 = new Tuplet<int, int, int, int, int, int, int, int, int, int>(973357632, 1544018695, 353570791, 1226995964, 49262773, 840989145, 559776888, 1747912691, 1364683612, 1046410302);
            var args2 = new Tuplet<int, int, int, int, int, int, int, int, int, int>(938708246, 2097133255, 405223749, 1828685152, 968247685, 1179698740, 1084353933, 327516175, 2017329178, 715567833);

            Assert.IsTrue(eq.Equals(args1, args1));

            Assert.AreNotEqual(eq.GetHashCode(args1), eq.GetHashCode(args2));
            Assert.IsFalse(eq.Equals(args1, args2));
            Assert.IsFalse(eq.Equals(args2, args1));
        }

        [TestMethod]
        public void CombineWithTuplet11()
        {
            var eq = EqualityComparerExtensions.CombineWithTuplet<int, int, int, int, int, int, int, int, int, int, int>(EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default);

            var args1 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(392453217, 1270864718, 1460409938, 185614642, 1516642130, 1779637132, 1249693301, 697996635, 1149977745, 88375853, 1896587492);
            var args2 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int>(927038697, 924366747, 1469951323, 1484477316, 757660304, 342451040, 508677449, 1327801974, 272127491, 399712914, 1034548160);

            Assert.IsTrue(eq.Equals(args1, args1));

            Assert.AreNotEqual(eq.GetHashCode(args1), eq.GetHashCode(args2));
            Assert.IsFalse(eq.Equals(args1, args2));
            Assert.IsFalse(eq.Equals(args2, args1));
        }

        [TestMethod]
        public void CombineWithTuplet12()
        {
            var eq = EqualityComparerExtensions.CombineWithTuplet<int, int, int, int, int, int, int, int, int, int, int, int>(EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default);

            var args1 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(49526117, 381369773, 30177914, 843764805, 1616659205, 237467758, 1877734186, 2034555795, 985839498, 349845215, 1867165537, 1451341831);
            var args2 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int>(102676511, 1849976561, 83608757, 167956149, 1857837481, 417109288, 1738779491, 2009263900, 597934946, 1276307759, 1297306457, 11669549);

            Assert.IsTrue(eq.Equals(args1, args1));

            Assert.AreNotEqual(eq.GetHashCode(args1), eq.GetHashCode(args2));
            Assert.IsFalse(eq.Equals(args1, args2));
            Assert.IsFalse(eq.Equals(args2, args1));
        }

        [TestMethod]
        public void CombineWithTuplet13()
        {
            var eq = EqualityComparerExtensions.CombineWithTuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default);

            var args1 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(1172766508, 1082756073, 344207836, 210587381, 837247700, 575676484, 1147197848, 1745201687, 315854919, 1505388704, 1221338601, 1079040165, 155436728);
            var args2 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int>(672877325, 162977927, 1012225543, 967746096, 1262905597, 1250020002, 1546742277, 1207356807, 1620508563, 1367274812, 1781984402, 674051547, 174494891);

            Assert.IsTrue(eq.Equals(args1, args1));

            Assert.AreNotEqual(eq.GetHashCode(args1), eq.GetHashCode(args2));
            Assert.IsFalse(eq.Equals(args1, args2));
            Assert.IsFalse(eq.Equals(args2, args1));
        }

        [TestMethod]
        public void CombineWithTuplet14()
        {
            var eq = EqualityComparerExtensions.CombineWithTuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default);

            var args1 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(798323615, 910692686, 680831647, 537932661, 436613214, 920702005, 1231546963, 18508365, 1818481944, 533903132, 2040743569, 1667146805, 1197308095, 410163014);
            var args2 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1350131014, 121963850, 1135486912, 744771454, 628637960, 1152052239, 12519421, 1184960156, 254131361, 726553948, 1041517804, 1482512996, 26287757, 1898047827);

            Assert.IsTrue(eq.Equals(args1, args1));

            Assert.AreNotEqual(eq.GetHashCode(args1), eq.GetHashCode(args2));
            Assert.IsFalse(eq.Equals(args1, args2));
            Assert.IsFalse(eq.Equals(args2, args1));
        }

        [TestMethod]
        public void CombineWithTuplet15()
        {
            var eq = EqualityComparerExtensions.CombineWithTuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default);

            var args1 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(951796389, 1699741592, 1862964908, 709707081, 1684019481, 662752809, 1924836516, 236505162, 1064370040, 1925405905, 1068775490, 300636596, 1994976849, 136928363, 1001879028);
            var args2 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1776558442, 1118965621, 1448082938, 65597502, 839856988, 196611263, 1085392957, 485021651, 622503358, 1153346442, 1669482955, 161975470, 1760847106, 656561325, 2101761346);

            Assert.IsTrue(eq.Equals(args1, args1));

            Assert.AreNotEqual(eq.GetHashCode(args1), eq.GetHashCode(args2));
            Assert.IsFalse(eq.Equals(args1, args2));
            Assert.IsFalse(eq.Equals(args2, args1));
        }

        [TestMethod]
        public void CombineWithTuplet16()
        {
            var eq = EqualityComparerExtensions.CombineWithTuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default, EqualityComparer<int>.Default);

            var args1 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1643898504, 1101583865, 894414248, 1480982783, 1214195623, 118740352, 818421871, 1331036488, 2130610971, 534555286, 632810145, 1113625852, 1205077457, 1357564654, 1823479611, 328001364);
            var args2 = new Tuplet<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(1304559037, 2023074705, 183081128, 625056566, 1755071974, 1740918513, 1416915494, 1333914416, 1701436564, 2013887079, 1214719941, 1240461550, 1703844286, 14536526, 500777339, 163989410);

            Assert.IsTrue(eq.Equals(args1, args1));

            Assert.AreNotEqual(eq.GetHashCode(args1), eq.GetHashCode(args2));
            Assert.IsFalse(eq.Equals(args1, args2));
            Assert.IsFalse(eq.Equals(args2, args1));
        }

    }
}
