// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtor.Remoting.Protocol;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor.Remoting.Protocol
{
    [TestClass]
    public class ObserverNotificationTests
    {
        [TestMethod]
        public void ObserverNotification_OnNext()
        {
            var onNext1 = OnNext(42);
            var onNext2 = OnNext(42);
            var onNext3 = OnNext(43);
            var predicate = OnNext((int x) => x % 2 == 0);

            Assert.AreEqual(NotificationKind.OnNext, onNext1.Kind);
            Assert.IsTrue(onNext1.HasValue);
            Assert.AreEqual(42, onNext1.Value);
            Assert.IsNull(onNext1.Exception);
            Assert.IsFalse(onNext1.HasPredicate);
            Assert.AreEqual(onNext1.GetHashCode(), onNext2.GetHashCode());
            Assert.AreEqual("OnNext(42)", onNext1.ToString());

            Assert.IsFalse(predicate.HasValue);
            Assert.AreEqual(default, predicate.Value);
            Assert.IsTrue(predicate.HasPredicate);
            Assert.AreEqual(predicate, onNext1);
            Assert.AreEqual(onNext1, predicate);
            Assert.AreNotEqual(predicate, onNext3);
            Assert.AreNotEqual(onNext3, predicate);
            Assert.AreEqual(string.Format("OnNext({0})", typeof(Func<int, bool>).FullName), predicate.ToString());

            // This is rather unfortunate, see comment in method...
            Assert.AreEqual(predicate.GetHashCode(), onNext1.GetHashCode());
            //Assert.AreNotEqual(onNext1.GetHashCode(), onNext3.GetHashCode());
        }

        [TestMethod]
        public void ObserverNotification_OnError()
        {
            var ex1 = new Exception();
            var ex2 = new InvalidCastException();
            var onError1 = OnError<int>(ex1);
            var onError2 = OnError<int>(ex1);
            var onError3 = OnError<int>(ex2);
            var predicate = OnError<int>(ex => ex is InvalidCastException);

            Assert.AreEqual(NotificationKind.OnError, onError1.Kind);
            Assert.IsFalse(onError1.HasValue);
            Assert.AreEqual(ex1, onError1.Exception);
            Assert.AreEqual(default, onError1.Value);
            Assert.IsFalse(onError1.HasPredicate);
            Assert.AreEqual(onError1.GetHashCode(), onError2.GetHashCode());
            Assert.AreEqual("OnError(System.Exception)", onError1.ToString());

            Assert.IsFalse(predicate.HasValue);
            Assert.AreEqual(default, predicate.Value);
            Assert.IsTrue(predicate.HasPredicate);
            Assert.AreEqual(predicate, onError3);
            Assert.AreEqual(onError3, predicate);
            Assert.AreNotEqual(predicate, onError1);
            Assert.AreNotEqual(onError1, predicate);
            Assert.AreEqual(string.Format("OnError({0})", typeof(Func<Exception, bool>).FullName), predicate.ToString());

            // This is rather unfortunate, see comment in method...
            Assert.AreEqual(predicate.GetHashCode(), onError2.GetHashCode());
            //Assert.AreNotEqual(onError1.GetHashCode(), onError3.GetHashCode());
        }

        [TestMethod]
        public void ObserverNotification_OnCompleted()
        {
            var onCompleted1 = OnCompleted<int>();
            var onCompleted2 = OnCompleted<int>();

            Assert.AreEqual(NotificationKind.OnCompleted, onCompleted1.Kind);
            Assert.IsFalse(onCompleted1.HasValue);
            Assert.AreEqual(default, onCompleted1.Value);
            Assert.IsNull(onCompleted1.Exception);
            Assert.IsFalse(onCompleted1.HasPredicate);
            Assert.AreEqual(onCompleted1.GetHashCode(), onCompleted2.GetHashCode());
            Assert.AreEqual("OnCompleted()", onCompleted1.ToString());
        }

        [TestMethod]
        public void ObserverNotification_Accept()
        {
            var ex = new Exception();
            var onNext = OnNext(42);
            var onError = OnError<int>(ex);
            var onComplete = OnCompleted<int>();

            using var iv = new TestObserver(onNext, onError, onComplete);

            onNext.Accept(iv);
            onError.Accept(iv);
            onComplete.Accept(iv);
        }

        [TestMethod]
        public void ObserverNotification_DefaultEquals()
        {
            var notification1 = OnNext(42);
            var notification2 = OnNext(42);
            var notification3 = OnCompleted<int>();

            Assert.IsTrue(object.Equals(notification1, notification2));
            Assert.IsFalse(object.Equals(notification1, new object()));
            Assert.IsFalse(object.Equals(notification1, notification3));
        }

        private sealed class TestObserver : IObserver<int>, IDisposable
        {
            private readonly INotification<int>[] _expected;
            private int _idx;

            public TestObserver(params INotification<int>[] expected)
            {
                _expected = expected;
            }

            public void OnCompleted()
            {
                if (_idx >= _expected.Length) Assert.Fail();
                Assert.AreEqual(_expected[_idx++], OnCompleted<int>());
            }

            public void OnError(Exception error)
            {
                if (_idx >= _expected.Length) Assert.Fail();
                Assert.AreEqual(_expected[_idx++], OnError<int>(error));
            }

            public void OnNext(int value)
            {
                if (_idx >= _expected.Length) Assert.Fail();
                Assert.AreEqual(_expected[_idx++], OnNext<int>(value));
            }

            public void Dispose()
            {
                if (_idx != _expected.Length) Assert.Fail();
            }
        }

        private static INotification<T> OnNext<T>(T value) => ObserverNotification.CreateOnNext<T>(value);

        private static INotification<T> OnNext<T>(Func<T, bool> predicate) => ObserverNotification.CreateOnNext<T>(predicate);

        private static INotification<T> OnError<T>(Exception error) => ObserverNotification.CreateOnError<T>(error);

        private static INotification<T> OnError<T>(Func<Exception, bool> predicate) => ObserverNotification.CreateOnError<T>(predicate);

        private static INotification<T> OnCompleted<T>() => ObserverNotification.CreateOnCompleted<T>();
    }
}
