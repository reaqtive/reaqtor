// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System;
using System.Collections;
using System.Linq.CompilerServices;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class ClrTypeTests
    {
        [TestMethod]
        public void ClrType_Void()
        {
            var int32 = new ClrType(typeof(int));

            Assert.IsTrue(ClrType.Void.IsAssignableTo(ClrType.Void));
            Assert.IsFalse(ClrType.Void.IsAssignableTo(int32));
            Assert.IsFalse(int32.IsAssignableTo(ClrType.Void)); // Counter-intuitive but true

            Assert.IsTrue(int32.Equals(int32));
            Assert.IsFalse(int32.Equals(42));
        }

        [TestMethod]
        public void ClrType_Basics()
        {
            var @object = new ClrType(typeof(object));
            var @string = new ClrType(typeof(string));
            var int32 = new ClrType(typeof(int));

            Assert.IsTrue(@object.IsAssignableTo(@object));
            Assert.IsTrue(@string.IsAssignableTo(@string));
            Assert.IsTrue(int32.IsAssignableTo(int32));

            Assert.IsTrue(@string.IsAssignableTo(@object));
            Assert.IsTrue(int32.IsAssignableTo(@object));

            Assert.IsFalse(int32.IsAssignableTo(@string));
            Assert.IsFalse(@string.IsAssignableTo(int32));

            Assert.IsFalse(@object.IsAssignableTo(int32));
            Assert.IsFalse(@object.IsAssignableTo(@string));

            Assert.IsTrue(int32.Equals(int32));
            Assert.IsTrue(int32.Equals((object)int32));
            Assert.IsFalse(int32.Equals(@string));
            Assert.IsFalse(int32.Equals((object)@string));
            Assert.IsFalse(@string.Equals(int32));

            Assert.AreEqual(int32.GetHashCode(), new ClrType(typeof(int)).GetHashCode());

            Assert.AreEqual("System.Object", @object.ToString());
            Assert.AreEqual("System.String", @string.ToString());
            Assert.AreEqual("System.Int32", int32.ToString());
        }

        [TestMethod]
        public void ClrType_Interfaces()
        {
            var arrayList = new ClrType(typeof(ArrayList));
            var iList = new ClrType(typeof(IList));
            var iEnumerable = new ClrType(typeof(IEnumerable));
            var @object = new ClrType(typeof(object));

            Assert.IsTrue(arrayList.IsAssignableTo(iList));
            Assert.IsTrue(arrayList.IsAssignableTo(iEnumerable));
            Assert.IsTrue(arrayList.IsAssignableTo(@object));

            Assert.IsTrue(iList.IsAssignableTo(iEnumerable));
            Assert.IsTrue(iList.IsAssignableTo(@object));

            Assert.IsFalse(iEnumerable.IsAssignableTo(iList));

            Assert.AreEqual("System.Collections.ArrayList", arrayList.ToString());
            Assert.AreEqual("System.Collections.IList", iList.ToString());
            Assert.AreEqual("System.Collections.IEnumerable", iEnumerable.ToString());
        }

        [TestMethod]
        public void ClrType_CrossTypeSystemCheck()
        {
            var myType = new MyType();
            var int32 = new ClrType(typeof(int));

            Assert.ThrowsException<InvalidOperationException>(() => int32.IsAssignableTo(myType));
            Assert.ThrowsException<InvalidOperationException>(() => int32.Equals(myType));
        }

        private sealed class MyType : IType
        {
            public bool IsAssignableTo(IType type) => throw new NotImplementedException();

            public bool Equals(IType other) => throw new NotImplementedException();
        }

    }
}
