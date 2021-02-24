// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Linq.CompilerServices;
using System.Linq.CompilerServices.Bonsai;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Linq.Expressions.Bonsai.CompilerServices
{
    [TestClass]
    public class TypeSlimSubstitutorTests : TestBase
    {
        [TestMethod]
        public void TypeSubstitutor_ArgumentChecks()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _ = new TypeSubstitutor(map: null));
        }

        [TestMethod]
        public void TypeSubstitutor_Simple()
        {
            var subst = new TypeSlimSubstitutor(
                new Dictionary<TypeSlim, TypeSlim>
                {
                    { typeof(int).ToTypeSlim(), typeof(long).ToTypeSlim() }
                });

            AssertIsNull(null, subst);
            AssertUnchanged(typeof(string), subst);
            AssertUnchanged(typeof(List<string[]>[,]), subst);

            AssertAreEqual(typeof(long), typeof(int), subst);
            AssertAreEqual(typeof(long[]), typeof(int[]), subst);
            AssertAreEqual(typeof(long[,]), typeof(int[,]), subst);
            AssertAreEqual(typeof(Func<long>), typeof(Func<int>), subst);
            AssertAreEqual(typeof(Func<bool, Func<object, long[]>, double>), typeof(Func<bool, Func<object, int[]>, double>), subst);
        }

        [TestMethod]
        public void TypeSubstitutor_Array()
        {
            var subst = new TypeSlimSubstitutor(
                new Dictionary<TypeSlim, TypeSlim>
                {
                    { typeof(int[]).ToTypeSlim(), typeof(long[]).ToTypeSlim() }
                });

            AssertIsNull(null, subst);
            AssertUnchanged(typeof(string), subst);
            AssertUnchanged(typeof(List<string[]>[,]), subst);

            AssertUnchanged(typeof(int), subst);
            AssertAreEqual(typeof(long[]), typeof(int[]), subst);
            AssertUnchanged(typeof(int[,]), subst);
            AssertUnchanged(typeof(Func<int>), subst);
            AssertAreEqual(typeof(Func<bool, Func<object, long[]>, double>), typeof(Func<bool, Func<object, int[]>, double>), subst);
        }

        private static void AssertIsNull(TypeSlim type, TypeSlimSubstitutor substitutor)
        {
            Assert.IsNull(substitutor.Visit(type));
        }

        private static void AssertUnchanged(Type type, TypeSlimSubstitutor substitutor)
        {
            var typeSlim = type.ToTypeSlim();
            Assert.AreSame(typeSlim, substitutor.Visit(typeSlim));
        }

        private static void AssertAreEqual(Type expected, Type target, TypeSlimSubstitutor substitutor)
        {
            Assert.IsTrue(expected.ToTypeSlim().Equals(substitutor.Visit(target.ToTypeSlim())));
        }
    }
}
