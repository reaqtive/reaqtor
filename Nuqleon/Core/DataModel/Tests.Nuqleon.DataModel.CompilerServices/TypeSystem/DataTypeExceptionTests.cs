// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
//

using System;
using System.Collections.Generic;

using Nuqleon.DataModel.TypeSystem;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Nuqleon.DataModel.CompilerServices.TypeSystem
{
    [TestClass]
    public class DataTypeExceptionTests
    {
        [TestMethod]
        public void DataTypeException_Constructors()
        {
            var ex1 = new DataTypeException();
            Assert.IsNull(ex1.InnerException);

            var ex2 = new DataTypeException("foo");
            Assert.AreEqual("foo", ex2.Message);
            Assert.IsNull(ex2.InnerException);
            Assert.IsTrue(ex2.ToString().Contains("foo"));

            var iex = new Exception();
            var ex3 = new DataTypeException("foo", iex);
            Assert.AreEqual("foo", ex3.Message);
            Assert.AreSame(iex, ex3.InnerException);
            Assert.IsTrue(ex3.ToString().Contains("foo"));

            AssertEx.ThrowsException<ArgumentNullException>(() => new DataTypeException(default(DataTypeError)), ex => Assert.AreEqual("error", ex.ParamName));

            var err = new DataTypeError(typeof(int), "bar", new[] { typeof(List<int>) });
            var ex4 = new DataTypeException(err);
            Assert.AreSame(err, ex4.Error);
            Assert.IsTrue(ex4.ToString().Contains("bar"));
        }

#if !NETSTD
        [TestMethod]
        public void DataTypeException_Serialize()
        {
            var err = new DataTypeError(typeof(int), "bar", new[] { typeof(List<int>) });
            var ex = new DataTypeException(err);

            var ms = new MemoryStream();
            new BinaryFormatter().Serialize(ms, ex);

            ms.Position = 0;
            var res = (DataTypeException)new BinaryFormatter().Deserialize(ms);

            Assert.AreEqual(err.Message, res.Error.Message);
            Assert.AreEqual(err.Type, res.Error.Type);
            Assert.IsTrue(err.Stack.SequenceEqual(res.Error.Stack));
        }
#endif
    }
}
