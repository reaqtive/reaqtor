// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq.CompilerServices;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class TypeSubstitutorTests
    {
        [TestMethod]
        public void TypeSubstitutor_ArgumentChecks()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _ = new TypeSubstitutor(map: null));
        }

        [TestMethod]
        public void TypeSubstitutor_Simple()
        {
            var subst = new TypeSubstitutor(new Dictionary<Type, Type>
            {
                { typeof(int), typeof(long) }
            });

            Assert.IsNull(subst.Visit(type: null));
            Assert.AreSame(typeof(string), subst.Visit(typeof(string)));
            Assert.AreSame(typeof(List<string[]>[,]), subst.Visit(typeof(List<string[]>[,])));

            Assert.AreEqual(typeof(long), subst.Visit(typeof(int)));
            Assert.AreEqual(typeof(long[]), subst.Visit(typeof(int[])));
            Assert.AreEqual(typeof(long[,]), subst.Visit(typeof(int[,])));
            Assert.AreEqual(typeof(Func<long>), subst.Visit(typeof(Func<int>)));
            Assert.AreEqual(typeof(Func<bool, Func<object, long[]>, double>), subst.Visit(typeof(Func<bool, Func<object, int[]>, double>)));
            Assert.AreEqual(typeof(long*), subst.Visit(typeof(int*)));
            Assert.AreEqual(typeof(long).MakeByRefType(), subst.Visit(typeof(int).MakeByRefType()));
        }

        [TestMethod]
        public void TypeSubstitutor_Array()
        {
            var subst = new TypeSubstitutor(new Dictionary<Type, Type>
            {
                { typeof(int[]), typeof(long[]) }
            });

            Assert.IsNull(subst.Visit(type: null));
            Assert.AreSame(typeof(string), subst.Visit(typeof(string)));
            Assert.AreSame(typeof(List<string[]>[,]), subst.Visit(typeof(List<string[]>[,])));

            Assert.AreSame(typeof(int), subst.Visit(typeof(int)));
            Assert.AreEqual(typeof(long[]), subst.Visit(typeof(int[])));
            Assert.AreSame(typeof(int[,]), subst.Visit(typeof(int[,])));
            Assert.AreSame(typeof(Func<int>), subst.Visit(typeof(Func<int>)));
            Assert.AreEqual(typeof(Func<bool, Func<object, long[]>, double>), subst.Visit(typeof(Func<bool, Func<object, int[]>, double>)));
            Assert.AreSame(typeof(int*), subst.Visit(typeof(int*)));
            Assert.AreSame(typeof(int).MakeByRefType(), subst.Visit(typeof(int).MakeByRefType()));
        }
    }
}
