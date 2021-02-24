// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/02/2014 - Wrote these tests.
//   BD - 07/29/2015 - Added tests for pooling of objects without IFreeable.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Memory;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class ObjectPoolTests
    {
        [TestMethod]
        public void ObjectPool_ArgumentChecking()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _ = new ObjectPool<MyFreeable>(factory: null));
            Assert.ThrowsException<ArgumentNullException>(() => _ = new ObjectPool<MyFreeable>(factory: null, size: 4));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = new ObjectPool<MyFreeable>(() => new MyFreeable(), size: 0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _ = new ObjectPool<MyFreeable>(() => new MyFreeable(), size: -1));
        }

        [TestMethod]
        public void ObjectPool_AllocateAndFree_Sequential_Degenerate()
        {
            var alloc = 0;

            var pool = new ObjectPool<MyFreeable>(() =>
            {
                alloc++;
                return new MyFreeable();
            }, 1);

            for (var i = 0; i < 16; i++)
            {
                var obj = pool.Allocate();
                Assert.IsFalse(obj.used);
                obj.Use();
                Assert.IsTrue(obj.used);
                pool.Free(obj);
            }

            Assert.AreEqual(1, alloc);
        }

        [TestMethod]
        public void ObjectPool_AllocateAndFree_Sequential_SpecifiedSize()
        {
            var alloc = 0;

            var pool = new ObjectPool<MyFreeable>(() =>
            {
                alloc++;
                return new MyFreeable();
            }, 4);

            for (var i = 0; i < 16; i++)
            {
                var obj = pool.Allocate();
                Assert.IsFalse(obj.used);
                obj.Use();
                Assert.IsTrue(obj.used);
                pool.Free(obj);
            }

            Assert.AreEqual(1, alloc);
        }

        [TestMethod]
        public void ObjectPool_AllocateAndFree_Sequential_DefaultSize()
        {
            var alloc = 0;

            var pool = new ObjectPool<MyFreeable>(() =>
            {
                alloc++;
                return new MyFreeable();
            });

            for (var i = 0; i < 16; i++)
            {
                var obj = pool.Allocate();
                Assert.IsFalse(obj.used);
                obj.Use();
                Assert.IsTrue(obj.used);
                pool.Free(obj);
            }

            Assert.AreEqual(1, alloc);
        }

        [TestMethod]
        public void ObjectPool_AllocateAndFree_Nested()
        {
            var K = 32;

            var res = from C in Enumerable.Range(1, K)
                      from N in Enumerable.Range(1, 10)
                      from M in Enumerable.Range(1, K * 2 + 1)
                      select (C, N, M);

            foreach (var cnm in res.Trim())
            {
                ObjectPool_AllocateAndFree_Nested_Impl(cnm.C, cnm.N, cnm.M);
            }
        }

        private static void ObjectPool_AllocateAndFree_Nested_Impl(int C, int N, int M)
        {
            var alloc = 0;

            var pool = new ObjectPool<MyFreeable>(() =>
            {
                alloc++;
                return new MyFreeable();
            }, C);

            for (var n = 0; n < N; n++)
            {
                var stack = new Stack<MyFreeable>();

                for (var i = 0; i < M; i++)
                {
                    var obj = pool.Allocate();
                    Assert.IsFalse(obj.used);
                    obj.Use();
                    Assert.IsTrue(obj.used);

                    stack.Push(obj);
                }

                while (stack.Count > 0)
                {
                    var obj = stack.Pop();
                    pool.Free(obj);
                }
            }

            Assert.AreEqual(M + Math.Max(0, M - C) * (N - 1), alloc);
        }

        [TestMethod]
        public void ObjectPool_NewAndDispose_Sequential_Degenerate()
        {
            var alloc = 0;

            var pool = new ObjectPool<MyFreeable>(() =>
            {
                alloc++;
                return new MyFreeable();
            }, 1);

            for (var i = 0; i < 16; i++)
            {
                using var it = pool.New();

                var obj = it.Object;

                Assert.IsFalse(obj.used);
                obj.Use();
                Assert.IsTrue(obj.used);
            }

            Assert.AreEqual(1, alloc);
        }

        [TestMethod]
        public void ObjectPool_NoFreeable_NewAndDispose_Sequential_Degenerate()
        {
            var alloc = 0;

            var pool = new ObjectPool<MyClearable>(() =>
            {
                alloc++;
                return new MyClearable();
            }, 1);

            for (var i = 0; i < 16; i++)
            {
                using var it = pool.New();

                var obj = it.Object;

                Assert.IsFalse(obj.used);
                obj.Use();
                Assert.IsTrue(obj.used);
            }

            Assert.AreEqual(1, alloc);
        }

        [TestMethod]
        public void ObjectPool_NewAndDispose_Sequential_SpecifiedSize()
        {
            var alloc = 0;

            var pool = new ObjectPool<MyFreeable>(() =>
            {
                alloc++;
                return new MyFreeable();
            }, 4);

            for (var i = 0; i < 16; i++)
            {
                using var it = pool.New();

                var obj = it.Object;

                Assert.IsFalse(obj.used);
                obj.Use();
                Assert.IsTrue(obj.used);
            }

            Assert.AreEqual(1, alloc);
        }

        [TestMethod]
        public void ObjectPool_NoFreeable_NewAndDispose_Sequential_SpecifiedSize()
        {
            var alloc = 0;

            var pool = new ObjectPool<MyClearable>(() =>
            {
                alloc++;
                return new MyClearable();
            }, 4);

            for (var i = 0; i < 16; i++)
            {
                using var it = pool.New();

                var obj = it.Object;

                Assert.IsFalse(obj.used);
                obj.Use();
                Assert.IsTrue(obj.used);
            }

            Assert.AreEqual(1, alloc);
        }

        [TestMethod]
        public void ObjectPool_NewAndDispose_Sequential_DefaultSize()
        {
            var alloc = 0;

            var pool = new ObjectPool<MyFreeable>(() =>
            {
                alloc++;
                return new MyFreeable();
            });

            for (var i = 0; i < 16; i++)
            {
                using var it = pool.New();

                var obj = it.Object;

                Assert.IsFalse(obj.used);
                obj.Use();
                Assert.IsTrue(obj.used);
            }

            Assert.AreEqual(1, alloc);
        }

        [TestMethod]
        public void ObjectPool_NoFreeable_NewAndDispose_Sequential_DefaultSize()
        {
            var alloc = 0;

            var pool = new ObjectPool<MyClearable>(() =>
            {
                alloc++;
                return new MyClearable();
            });

            for (var i = 0; i < 16; i++)
            {
                using var it = pool.New();

                var obj = it.Object;

                Assert.IsFalse(obj.used);
                obj.Use();
                Assert.IsTrue(obj.used);
            }

            Assert.AreEqual(1, alloc);
        }

        [TestMethod]
        public void ObjectPool_NewAndDispose_Nested()
        {
            var K = 32;

            var res = from C in Enumerable.Range(1, K)
                      from N in Enumerable.Range(1, 10)
                      from M in Enumerable.Range(1, K * 2 + 1)
                      select (C, N, M);

            foreach (var cnm in res.Trim())
            {
                ObjectPool_NewAndDispose_Nested_Impl(cnm.C, cnm.N, cnm.M);
                ObjectPool_NoFreeable_NewAndDispose_Nested_Impl(cnm.C, cnm.N, cnm.M);
            }
        }

        private static void ObjectPool_NewAndDispose_Nested_Impl(int C, int N, int M)
        {
            var alloc = 0;

            var pool = new ObjectPool<MyFreeable>(() =>
            {
                alloc++;
                return new MyFreeable();
            }, C);

            for (var n = 0; n < N; n++)
            {
                var stack = new Stack<IDisposable>();

                for (var i = 0; i < M; i++)
                {
                    var it = pool.New();
                    var obj = it.Object;

                    Assert.IsFalse(obj.used);
                    obj.Use();
                    Assert.IsTrue(obj.used);

                    stack.Push(it);
                }

                while (stack.Count > 0)
                {
                    var obj = stack.Pop();
                    obj.Dispose();
                }
            }

            Assert.AreEqual(M + Math.Max(0, M - C) * (N - 1), alloc);
        }

        private static void ObjectPool_NoFreeable_NewAndDispose_Nested_Impl(int C, int N, int M)
        {
            var alloc = 0;

            var pool = new ObjectPool<MyClearable>(() =>
            {
                alloc++;
                return new MyClearable();
            }, C);

            for (var n = 0; n < N; n++)
            {
                var stack = new Stack<IDisposable>();

                for (var i = 0; i < M; i++)
                {
                    var it = pool.New();
                    var obj = it.Object;

                    Assert.IsFalse(obj.used);
                    obj.Use();
                    Assert.IsTrue(obj.used);

                    stack.Push(it);
                }

                while (stack.Count > 0)
                {
                    var obj = stack.Pop();
                    obj.Dispose();
                }
            }

            Assert.AreEqual(M + Math.Max(0, M - C) * (N - 1), alloc);
        }

        [TestMethod]
        public void ObjectPool_PooledObject_Ctor_Sequential()
        {
            var alloc = 0;

            var pool = default(ObjectPool<MyFreeable>);
            pool = new ObjectPool<MyFreeable>(() =>
            {
                alloc++;
                return new MyFreeable(pool);
            });

            for (var i = 0; i < 16; i++)
            {
                using var it = new PooledObject<MyFreeable>(pool);

                var obj = it.Object;

                Assert.IsFalse(obj.used);
                obj.Use();
                Assert.IsTrue(obj.used);
            }

            Assert.AreEqual(1, alloc);
        }

        [TestMethod]
        public void ObjectPool_NoFreeable_PooledObject_Ctor_Sequential()
        {
            var alloc = 0;

            var pool = new ObjectPool<MyClearable>(() =>
            {
                alloc++;
                return new MyClearable();
            });

            for (var i = 0; i < 16; i++)
            {
                using var it = new PooledObject<MyClearable>(pool);

                var obj = it.Object;

                Assert.IsFalse(obj.used);
                obj.Use();
                Assert.IsTrue(obj.used);
            }

            Assert.AreEqual(1, alloc);
        }

        [TestMethod]
        public void ObjectPool_PooledObject_Ctor_Nested()
        {
            var K = 32;

            var res = from C in Enumerable.Range(1, K)
                      from N in Enumerable.Range(1, 10)
                      from M in Enumerable.Range(1, K * 2 + 1)
                      select (C, N, M);

            foreach (var cnm in res.Trim())
            {
                ObjectPool_PooledObject_Ctor_Nested_Impl(cnm.C, cnm.N, cnm.M);
                ObjectPool_NoFreeable_PooledObject_Ctor_Nested_Impl(cnm.C, cnm.N, cnm.M);
            }
        }

#if DEBUG || ENABLE_LOGGING
        [TestMethod]
        public void ObjectPool_DebugView()
        {
            var pool = new ObjectPool<MyFreeable>(() => new MyFreeable());

            Assert.IsTrue(!string.IsNullOrEmpty(pool.DebugView));

            var obj = pool.Allocate();

            Assert.IsTrue(!string.IsNullOrEmpty(pool.DebugView));

            pool.Free(obj);

            Assert.IsTrue(!string.IsNullOrEmpty(pool.DebugView));
        }
#endif

        private static void ObjectPool_PooledObject_Ctor_Nested_Impl(int C, int N, int M)
        {
            var alloc = 0;

            var pool = default(ObjectPool<MyFreeable>);
            pool = new ObjectPool<MyFreeable>(() =>
            {
                alloc++;
                return new MyFreeable(pool);
            }, C);

            for (var n = 0; n < N; n++)
            {
                var stack = new Stack<IDisposable>();

                for (var i = 0; i < M; i++)
                {
                    var it = new PooledObject<MyFreeable>(pool);
                    var obj = it.Object;

                    Assert.IsFalse(obj.used);
                    obj.Use();
                    Assert.IsTrue(obj.used);

                    stack.Push(it);
                }

                while (stack.Count > 0)
                {
                    var obj = stack.Pop();
                    obj.Dispose();
                }
            }

            Assert.AreEqual(M + Math.Max(0, M - C) * (N - 1), alloc);
        }

        private static void ObjectPool_NoFreeable_PooledObject_Ctor_Nested_Impl(int C, int N, int M)
        {
            var alloc = 0;

            var pool = new ObjectPool<MyClearable>(() =>
            {
                alloc++;
                return new MyClearable();
            }, C);

            for (var n = 0; n < N; n++)
            {
                var stack = new Stack<IDisposable>();

                for (var i = 0; i < M; i++)
                {
                    var it = new PooledObject<MyClearable>(pool);
                    var obj = it.Object;

                    Assert.IsFalse(obj.used);
                    obj.Use();
                    Assert.IsTrue(obj.used);

                    stack.Push(it);
                }

                while (stack.Count > 0)
                {
                    var obj = stack.Pop();
                    obj.Dispose();
                }
            }

            Assert.AreEqual(M + Math.Max(0, M - C) * (N - 1), alloc);
        }
    }
}
