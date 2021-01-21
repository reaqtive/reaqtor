// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq.CompilerServices;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor.Shared.Core.Reaqtor
{
    [TestClass]
    public class StructuralSubstitutingTypeComparatorTests
    {
        [TestMethod]
        public void StructuralSubstitutingTypeComparator_DoubleMapping_ThrowsInvalidOperation()
        {
            var type1 = RuntimeCompiler.CreateRecordType(new Dictionary<string, Type>
            {
                { "Foo", typeof(int) }
            }, valueEquality: true);

            var type2 = RuntimeCompiler.CreateRecordType(new Dictionary<string, Type>
            {
                { "Foo", typeof(int) }
            }, valueEquality: true);

            var type3 = RuntimeCompiler.CreateRecordType(new Dictionary<string, Type>
            {
                { "Foo", typeof(int) }
            }, valueEquality: true);

            var comparer = new StructuralSubstitutingTypeComparator();
            Assert.IsTrue(comparer.Equals(type1, type2));
            Assert.ThrowsException<InvalidOperationException>(() => comparer.Equals(type3, type2));
        }

        [TestMethod]
        public void StructuralSubstitutingTypeComparator_AreEqual()
        {
            var type1 = RuntimeCompiler.CreateRecordType(new Dictionary<string, Type>
            {
                { "Foo", typeof(int) }
            }, valueEquality: true);

            var type2 = RuntimeCompiler.CreateRecordType(new Dictionary<string, Type>
            {
                { "Foo", typeof(int) }
            }, valueEquality: true);


            var comparer = new StructuralSubstitutingTypeComparator();

            Assert.IsTrue(comparer.Equals(type1, type1));
            Assert.AreEqual(0, comparer.Substitutions.Count);
            Assert.IsTrue(comparer.Equals(type1, type2));
            Assert.AreEqual(type1, comparer.Substitutions[type2]);
            Assert.AreEqual(1, comparer.Substitutions.Count);
        }

        [TestMethod]
        public void StructuralSubstitutingTypeComparator_AreNotEqual()
        {
            var type1 = RuntimeCompiler.CreateRecordType(new Dictionary<string, Type>
            {
                { "Foo", typeof(int) }
            }, valueEquality: true);

            var type2 = RuntimeCompiler.CreateRecordType(new Dictionary<string, Type>
            {
                { "Foo", typeof(long) }
            }, valueEquality: true);

            var comparer = new StructuralSubstitutingTypeComparator();
            Assert.IsFalse(comparer.Equals(type1, type2));
        }
    }
}
