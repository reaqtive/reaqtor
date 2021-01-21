// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

using Reaqtive;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Reaqtive
{
    [TestClass]
    public class ListObserverTests
    {
        [TestMethod]
        public void ListObserver_ArgumentChecking()
        {
            //Assert.ThrowsException<ArgumentNullException>(() => new ListObserver<int>(default(IObserver<int>[])));

            var lo = new ListObserver<int>();

            Assert.ThrowsException<ArgumentNullException>(() => lo.Add(default));
            Assert.ThrowsException<ArgumentNullException>(() => lo.Remove(default));
        }

        [TestMethod]
        public void ListObserver_Basics()
        {
            var lo = new ListObserver<int>();

            lo.OnNext(42);

            var x1 = new List<int>();
            var e1 = default(Exception);
            var d1 = default(bool);
            var o1 = Observer.Create<int>(x1.Add, ex => e1 = ex, () => d1 = true);

            var l1 = lo.Add(o1);

            Assert.AreNotSame(lo, l1);

            l1.OnNext(43);
            l1.OnNext(44);
            l1.OnCompleted();

            Assert.IsTrue(new[] { 43, 44 }.SequenceEqual(x1));
            Assert.IsTrue(d1);

            var l1l = ListObserver<int>.Create(l1);

            Assert.ThrowsException<InvalidOperationException>(() => l1l.Remove(Observer.Create<int>(_ => { }, _ => { }, () => { })));

            var l2 = l1l.Remove(o1);

            Assert.AreNotSame(lo, l2);
            Assert.AreNotSame(l1, l2);

            l2.OnNext(45);

            Assert.IsTrue(new[] { 43, 44 }.SequenceEqual(x1));
            Assert.IsTrue(d1);
        }

        [TestMethod]
        public void ListObserver_DuplicateObservers()
        {
            var lo = new ListObserver<int>();

            var log = new List<string>();
            var o1 = Observer.Create<int>(x => log.Add("o1 - " + x), _ => { }, () => { });
            var o2 = Observer.Create<int>(x => log.Add("o2 - " + x), _ => { }, () => { });
            var o3 = Observer.Create<int>(x => log.Add("o3 - " + x), _ => { }, () => { });

            lo.OnNext(1);

            var l1 = ListObserver<int>.Create(lo.Add(o1));

            l1.OnNext(2);

            var l2 = ListObserver<int>.Create(l1.Add(o2));

            l2.OnNext(3);

            var l3 = ListObserver<int>.Create(l2.Add(o1));

            l3.OnNext(4);

            var l4 = ListObserver<int>.Create(l3.Add(o3));

            l4.OnNext(5);

            var l5 = ListObserver<int>.Create(l4.Remove(o1));

            l5.OnNext(6);

            var l6 = ListObserver<int>.Create(l5.Remove(o1));

            l6.OnNext(7);

            Assert.IsTrue(new[] {
                "o1 - 2",
                "o1 - 3", "o2 - 3",
                "o1 - 4", "o2 - 4", "o1 - 4",
                "o1 - 5", "o2 - 5", "o1 - 5", "o3 - 5",
                "o1 - 6", "o2 - 6", "o3 - 6",
                "o2 - 7", "o3 - 7",
            }.SequenceEqual(log));

            var l7 = l6.Remove(o2);
            var l8 = l6.Remove(o3);

            Assert.AreSame(o3, l7);
            Assert.AreSame(o2, l8);
        }

        [TestMethod]
        public void ListObserver_OnErrorAll()
        {
            var cd = new CountdownEvent(3);

            var o1 = Observer.Create<int>(x => { Assert.Fail(); }, _ => { cd.Signal(); }, () => { Assert.Fail(); });
            var o2 = Observer.Create<int>(x => { Assert.Fail(); }, _ => { cd.Signal(); }, () => { Assert.Fail(); });
            var o3 = Observer.Create<int>(x => { Assert.Fail(); }, _ => { cd.Signal(); }, () => { Assert.Fail(); });

            var lo = new ListObserver<int>(o1, o2, o3);

            lo.OnError(new Exception());

            cd.Wait();
        }

        private sealed class ListObserver<T> : IObserver<T>
        {
            private static readonly Lazy<Type> s_listObserver = new(() => typeof(SimpleSubject<>).Assembly.GetType("Reaqtive.ListObserver`1").MakeGenericType(typeof(T)));
            private static readonly Lazy<MethodInfo> s_add = new(() => s_listObserver.Value.GetMethod("Add"));
            private static readonly Lazy<MethodInfo> s_remove = new(() => s_listObserver.Value.GetMethod("Remove"));
            private object _instance;

            public ListObserver(params IObserver<T>[] observers)
            {
                Do(() =>
                {
                    _instance = Activator.CreateInstance(s_listObserver.Value, new object[] { observers });
                });
            }

            private ListObserver(object instance)
            {
                _instance = instance;
            }

            public static ListObserver<T> Create(object instance)
            {
                return new ListObserver<T>(instance);
            }

            public IObserver<T> Add(IObserver<T> observer)
            {
                return Do(() =>
                {
                    return (IObserver<T>)s_add.Value.Invoke(_instance, new object[] { observer });
                });
            }

            public IObserver<T> Remove(IObserver<T> observer)
            {
                return Do(() =>
                {
                    return (IObserver<T>)s_remove.Value.Invoke(_instance, new object[] { observer });
                });
            }

            public void OnCompleted()
            {
                ((IObserver<T>)_instance).OnCompleted();
            }

            public void OnError(Exception error)
            {
                ((IObserver<T>)_instance).OnError(error);
            }

            public void OnNext(T value)
            {
                ((IObserver<T>)_instance).OnNext(value);
            }

            private static void Do(Action a)
            {
                try
                {
                    a();
                }
                catch (TargetInvocationException ex)
                {
                    throw ex.InnerException;
                }
            }

            private static R Do<R>(Func<R> a)
            {
                try
                {
                    return a();
                }
                catch (TargetInvocationException ex)
                {
                    throw ex.InnerException;
                }
            }
        }
    }
}
