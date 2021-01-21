// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtive;

using Reaqtor.QueryEngine;

namespace Tests.Reaqtor.QueryEngine
{
    [TestClass]
    public class MultiObserverTests
    {
        [TestMethod]
        public void Engine_MultiObserver_Empty()
        {
            var actions = new Action<IObserver<int>>[]
            {
                o => o.OnNext(42),
                o => o.OnError(new Exception()),
                o => o.OnCompleted(),
            };

            foreach (var action in actions)
            {
                var observers = Array.Empty<IObserver<int>>();
                var m = new MultiObserver<int>(observers);

                action(m);
            }

            Assert.IsTrue(true); // just happy nothing bad happened
        }

        [TestMethod]
        public void Engine_MultiObserver_One()
        {
            MultiObserver_Multi_Core(1);
        }

        [TestMethod]
        public void Engine_MultiObserver_Multi()
        {
            for (var i = 2; i < 8; i++)
            {
                MultiObserver_Multi_Core(1);
            }
        }

        private static void MultiObserver_Multi_Core(int count)
        {
            var err = new Exception();

            var actions = new[]
            {
                (
                    Action : new Action<IObserver<int>>(o => o.OnNext(42)),
                    OnNext : new Action<int>(x => Assert.AreEqual(42, x)),
                    OnError : new Action<Exception>(ex => Assert.Fail()),
                    OnCompleted : new Action(() => Assert.Fail())
                ),
                (
                    Action : new Action<IObserver<int>>(o => o.OnError(err)),
                    OnNext : new Action<int>(x => Assert.Fail()),
                    OnError : new Action<Exception>(ex => Assert.AreSame(err, ex)),
                    OnCompleted : new Action(() => Assert.Fail())
                ),
                (
                    Action : new Action<IObserver<int>>(o => o.OnCompleted()),
                    OnNext : new Action<int>(x => Assert.Fail()),
                    OnError : new Action<Exception>(ex => Assert.Fail()),
                    OnCompleted : new Action(() => Assert.IsTrue(true))
                ),
            };

            foreach (var action in actions)
            {
                var i = 0;

                var onNext = new Action<int>(x =>
                {
                    i++;
                    action.OnNext(x);
                });

                var onError = new Action<Exception>(ex =>
                {
                    i++;
                    action.OnError(ex);
                });

                var onCompleted = new Action(() =>
                {
                    i++;
                    action.OnCompleted();
                });

                var observers = Enumerable.Range(0, count).Select(_ => Observer.Create<int>(onNext, onError, onCompleted)).ToArray();
                var m = new MultiObserver<int>(observers);

                action.Action(m);

                Assert.AreEqual(1, i);
            }
        }
    }
}
