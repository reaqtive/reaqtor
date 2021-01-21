// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.


using System;

#if !NET5_0
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
#endif

using Reaqtor.QueryEngine;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor.QueryEngine
{
    [TestClass]
    public class ExceptionTests
    {
        #region EntityAlreadyExistsException

        [TestMethod]
        public void EntityAlreadyExistsException_Default()
        {
            var ex = new EntityAlreadyExistsException();

            Assert.IsNull(ex.EntityUri);
            Assert.IsNull(ex.QueryEngineUri);
        }

        [TestMethod]
        public void EntityAlreadyExistsException_Ctor1()
        {
            var id = new Uri("bar://foo");
            var qe = new Uri("qe://qux");
            var kind = ReactiveEntityKind.Observable;
            var param = "baz";

            var ex = new EntityAlreadyExistsException(id, kind, qe, param);

            Assert.AreSame(id, ex.EntityUri);
            Assert.AreSame(qe, ex.QueryEngineUri);
            Assert.AreEqual(ReactiveEntityKind.Observable, ex.EntityType);
            Assert.AreEqual(param, ex.ParamName);
            Assert.IsTrue(ex.Message.Contains("already exists"));
        }

        [TestMethod]
        public void EntityAlreadyExistsException_Ctor2()
        {
            var id = new Uri("bar://foo");
            var qe = new Uri("qe://qux");
            var kind = ReactiveEntityKind.Observable;
            var param = "baz";
            var inner = new Exception();

            var ex = new EntityAlreadyExistsException(id, kind, qe, param, inner);

            Assert.AreSame(id, ex.EntityUri);
            Assert.AreSame(qe, ex.QueryEngineUri);
            Assert.AreEqual(ReactiveEntityKind.Observable, ex.EntityType);
            Assert.AreEqual(param, ex.ParamName);
            Assert.IsTrue(ex.Message.Contains("already exists"));
            Assert.AreSame(inner, ex.InnerException);
        }

#if !NET5_0 // https://aka.ms/binaryformatter
        [TestMethod]
        public void EntityAlreadyExistsException_Serialization()
        {
            var id = new Uri("bar://foo");
            var qe = new Uri("qe://qux");
            var kind = ReactiveEntityKind.Observable;
            var param = "baz";

            var ex = new EntityAlreadyExistsException(id, kind, qe, param);

            var bf = new BinaryFormatter();
            var ms = new MemoryStream();
            bf.Serialize(ms, ex);
            ms.Position = 0;

            ex = (EntityAlreadyExistsException)bf.Deserialize(ms);

            Assert.AreEqual(id, ex.EntityUri);
            Assert.AreEqual(qe, ex.QueryEngineUri);
            Assert.AreEqual(ReactiveEntityKind.Observable, ex.EntityType);
            Assert.AreEqual(param, ex.ParamName);
            Assert.IsTrue(ex.Message.Contains("already exists"));
        }
#endif

        #endregion

        #region EntityNotFoundException

        [TestMethod]
        public void EntityNotFoundException_Default()
        {
            var ex = new EntityNotFoundException();

            Assert.IsNull(ex.EntityUri);
            Assert.IsNull(ex.QueryEngineUri);
        }

        [TestMethod]
        public void EntityNotFoundException_Ctor1()
        {
            var id = new Uri("bar://foo");
            var qe = new Uri("qe://qux");
            var kind = ReactiveEntityKind.Observable;
            var param = "baz";

            var ex = new EntityNotFoundException(id, kind, qe, param);

            Assert.AreSame(id, ex.EntityUri);
            Assert.AreSame(qe, ex.QueryEngineUri);
            Assert.AreEqual(ReactiveEntityKind.Observable, ex.EntityType);
            Assert.AreEqual(param, ex.ParamName);
            Assert.IsTrue(ex.Message.Contains("could not be found"));
        }

        [TestMethod]
        public void EntityNotFoundException_Ctor2()
        {
            var id = new Uri("bar://foo");
            var qe = new Uri("qe://qux");
            var kind = ReactiveEntityKind.Observable;
            var param = "baz";
            var inner = new Exception();

            var ex = new EntityNotFoundException(id, kind, qe, param, inner);

            Assert.AreSame(id, ex.EntityUri);
            Assert.AreSame(qe, ex.QueryEngineUri);
            Assert.AreEqual(ReactiveEntityKind.Observable, ex.EntityType);
            Assert.AreEqual(param, ex.ParamName);
            Assert.IsTrue(ex.Message.Contains("could not be found"));
            Assert.AreSame(inner, ex.InnerException);
        }

#if !NET5_0 // https://aka.ms/binaryformatter
        [TestMethod]
        public void EntityNotFoundException_Serialization()
        {
            var id = new Uri("bar://foo");
            var qe = new Uri("qe://qux");
            var kind = ReactiveEntityKind.Observable;
            var param = "baz";

            var ex = new EntityNotFoundException(id, kind, qe, param);

            var bf = new BinaryFormatter();
            var ms = new MemoryStream();
            bf.Serialize(ms, ex);
            ms.Position = 0;

            ex = (EntityNotFoundException)bf.Deserialize(ms);

            Assert.AreEqual(id, ex.EntityUri);
            Assert.AreEqual(qe, ex.QueryEngineUri);
            Assert.AreEqual(ReactiveEntityKind.Observable, ex.EntityType);
            Assert.AreEqual(param, ex.ParamName);
            Assert.IsTrue(ex.Message.Contains("could not be found"));
        }
#endif

        #endregion

        #region EntityLoadFailedException

        [TestMethod]
        public void EntityLoadFailedException_Default()
        {
            var ex = new EntityLoadFailedException();

            Assert.IsNull(ex.EntityUri);
        }

        [TestMethod]
        public void EntityLoadFailedException_Ctor1()
        {
            var id = new Uri("bar://foo");
            var kind = ReactiveEntityKind.Observable;

            var ex = new EntityLoadFailedException(id, kind);

            Assert.AreSame(id, ex.EntityUri);
            Assert.AreEqual(ReactiveEntityKind.Observable, ex.EntityType);
            Assert.IsTrue(ex.Message.Contains("failed to load"));
        }

        [TestMethod]
        public void EntityLoadFailedException_Ctor2()
        {
            var id = new Uri("bar://foo");
            var kind = ReactiveEntityKind.Observable;
            var inner = new Exception();

            var ex = new EntityLoadFailedException(id, kind, inner);

            Assert.AreSame(id, ex.EntityUri);
            Assert.AreEqual(ReactiveEntityKind.Observable, ex.EntityType);
            Assert.IsTrue(ex.Message.Contains("failed to load"));
            Assert.AreSame(inner, ex.InnerException);
        }

#if !NET5_0 // https://aka.ms/binaryformatter
        [TestMethod]
        public void EntityLoadFailedException_Serialization()
        {
            var id = new Uri("bar://foo");
            var kind = ReactiveEntityKind.Observable;

            var ex = new EntityLoadFailedException(id, kind);

            var bf = new BinaryFormatter();
            var ms = new MemoryStream();
            bf.Serialize(ms, ex);
            ms.Position = 0;

            ex = (EntityLoadFailedException)bf.Deserialize(ms);

            Assert.AreEqual(id, ex.EntityUri);
            Assert.AreEqual(ReactiveEntityKind.Observable, ex.EntityType);
            Assert.IsTrue(ex.Message.Contains("failed to load"));
        }
#endif

        #endregion

        #region EntitySaveFailedException

        [TestMethod]
        public void EntitySaveFailedException_Default()
        {
            var ex = new EntitySaveFailedException();

            Assert.IsNull(ex.EntityUri);
        }

        [TestMethod]
        public void EntitySaveFailedException_Ctor1()
        {
            var id = new Uri("bar://foo");
            var kind = ReactiveEntityKind.Observable;

            var ex = new EntitySaveFailedException(id, kind);

            Assert.AreSame(id, ex.EntityUri);
            Assert.AreEqual(ReactiveEntityKind.Observable, ex.EntityType);
            Assert.IsTrue(ex.Message.Contains("failed to save"));
        }

        [TestMethod]
        public void EntitySaveFailedException_Ctor2()
        {
            var id = new Uri("bar://foo");
            var kind = ReactiveEntityKind.Observable;
            var inner = new Exception();

            var ex = new EntitySaveFailedException(id, kind, inner);

            Assert.AreSame(id, ex.EntityUri);
            Assert.AreEqual(ReactiveEntityKind.Observable, ex.EntityType);
            Assert.IsTrue(ex.Message.Contains("failed to save"));
            Assert.AreSame(inner, ex.InnerException);
        }

#if !NET5_0 // https://aka.ms/binaryformatter
        [TestMethod]
        public void EntitySaveFailedException_Serialization()
        {
            var id = new Uri("bar://foo");
            var kind = ReactiveEntityKind.Observable;

            var ex = new EntitySaveFailedException(id, kind);

            var bf = new BinaryFormatter();
            var ms = new MemoryStream();
            bf.Serialize(ms, ex);
            ms.Position = 0;

            ex = (EntitySaveFailedException)bf.Deserialize(ms);

            Assert.AreEqual(id, ex.EntityUri);
            Assert.AreEqual(ReactiveEntityKind.Observable, ex.EntityType);
            Assert.IsTrue(ex.Message.Contains("failed to save"));
        }
#endif

        #endregion

        #region EngineUnloadedException

        [TestMethod]
        public void EngineUnloadedException_Default()
        {
            var ex = new EngineUnloadedException();

            Assert.IsTrue(ex.Message.Contains("unloaded"));
        }

        [TestMethod]
        public void EngineUnloadedException_Message()
        {
            var ex = new EngineUnloadedException("foo");

            Assert.AreEqual("foo", ex.Message);
        }

        [TestMethod]
        public void EngineUnloadedException_Inner()
        {
            var ie = new Exception();
            var ex = new EngineUnloadedException(ie);

            Assert.AreSame(ie, ex.InnerException);
            Assert.IsTrue(ex.Message.Contains("unloaded"));
        }

        [TestMethod]
        public void EngineUnloadedException_MessageInner()
        {
            var ie = new Exception();
            var ex = new EngineUnloadedException("foo", ie);

            Assert.AreSame(ie, ex.InnerException);
            Assert.AreEqual("foo", ex.Message);
        }

#if !NET5_0 // https://aka.ms/binaryformatter
        [TestMethod]
        public void EngineUnloadedException_Serialize()
        {
            var bf = new BinaryFormatter();

            var ex = new EngineUnloadedException();

            var ms = new MemoryStream();
            bf.Serialize(ms, ex);

            ms.Position = 0;
            var ef = (EngineUnloadedException)bf.Deserialize(ms);

            Assert.IsTrue(ef.Message.Contains("unloaded"));
        }
#endif

        #endregion
    }
}
