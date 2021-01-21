// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Linq.Expressions.Jit;
using System.Runtime.CompilerServices;

namespace Tests
{
    [TestClass]
    public class RuntimeOpsExTests
    {
        [TestMethod]
        public void CreateRuntimeVariables_Empty()
        {
            var c = new Closure<int>
            {
                Item1 = 42
            };

            var r = RuntimeOpsEx.CreateRuntimeVariables(c, Array.Empty<long>());
            Assert.AreEqual(0, r.Count);

            foreach (var index in new[] { -1, 0, 1 })
            {
                Assert.ThrowsException<IndexOutOfRangeException>(() => r[index]);
                Assert.ThrowsException<IndexOutOfRangeException>(() => r[index] = 42);
            }
        }

        [TestMethod]
        public void CreateRuntimeVariables_Simple1()
        {
            var c = new Closure<int>
            {
                Item1 = 42
            };

            var r = RuntimeOpsEx.CreateRuntimeVariables(c, new[] { 0L });
            Assert.AreEqual(1, r.Count);
            Assert.AreEqual(c.Item1, r[0]);
            r[0] = 43;
            Assert.AreEqual(43, r[0]);
            Assert.AreEqual(c.Item1, r[0]);

            foreach (var value in new object[] { "bar", 42L, true })
            {
                Assert.ThrowsException<InvalidCastException>(() => r[0] = value);
            }

            foreach (var index in new[] { -1, 1, 2 })
            {
                Assert.ThrowsException<IndexOutOfRangeException>(() => r[index]);
                Assert.ThrowsException<IndexOutOfRangeException>(() => r[index] = 42);
            }
        }

        [TestMethod]
        public void CreateRuntimeVariables_Simple2()
        {
            var c = new Closure<bool, int, string>
            {
                Item2 = 42
            };

            var r = RuntimeOpsEx.CreateRuntimeVariables(c, new[] { 1L });
            Assert.AreEqual(1, r.Count);
            Assert.AreEqual(c.Item2, r[0]);
            r[0] = 43;
            Assert.AreEqual(43, r[0]);
            Assert.AreEqual(c.Item2, r[0]);

            foreach (var value in new object[] { "bar", 42L, true })
            {
                Assert.ThrowsException<InvalidCastException>(() => r[0] = value);
            }

            foreach (var index in new[] { -1, 1, 2 })
            {
                Assert.ThrowsException<IndexOutOfRangeException>(() => r[index]);
                Assert.ThrowsException<IndexOutOfRangeException>(() => r[index] = 42);
            }
        }

        [TestMethod]
        public void CreateRuntimeVariables_Simple3()
        {
            var c = new Closure<bool, int, string>
            {
                Item2 = 42,
                Item1 = false,
                Item3 = "bar"
            };

            var r = RuntimeOpsEx.CreateRuntimeVariables(c, new[] { 1L, 0L, 2L });
            Assert.AreEqual(3, r.Count);
            Assert.AreEqual(c.Item2, r[0]);
            Assert.AreEqual(c.Item1, r[1]);
            Assert.AreEqual(c.Item3, r[2]);
            r[0] = 43;
            r[1] = true;
            r[2] = "foo";
            Assert.AreEqual(c.Item2, 43);
            Assert.AreEqual(c.Item1, true);
            Assert.AreEqual(c.Item3, "foo");
            Assert.AreEqual(c.Item2, r[0]);
            Assert.AreEqual(c.Item1, r[1]);
            Assert.AreEqual(c.Item3, r[2]);

            foreach (var value in new object[] { "bar", 42L, true })
            {
                Assert.ThrowsException<InvalidCastException>(() => r[0] = value);
            }

            foreach (var value in new object[] { "bar", 42L })
            {
                Assert.ThrowsException<InvalidCastException>(() => r[1] = value);
            }

            foreach (var value in new object[] { 42L, true })
            {
                Assert.ThrowsException<InvalidCastException>(() => r[2] = value);
            }

            foreach (var index in new[] { -1, 3, 4 })
            {
                Assert.ThrowsException<IndexOutOfRangeException>(() => r[index]);
                Assert.ThrowsException<IndexOutOfRangeException>(() => r[index] = 42);
            }
        }

        [TestMethod]
        public void CreateRuntimeVariables_Parent1()
        {
            var c = new Closure<Closure<bool, int, string>>
            {
                Item1 = new Closure<bool, int, string>
                {
                    Item2 = 42
                }
            };

            var r = RuntimeOpsEx.CreateRuntimeVariables(c, new[] { 1L << 32 | 1L });
            Assert.AreEqual(1, r.Count);
            Assert.AreEqual(c.Item1.Item2, r[0]);
            r[0] = 43;
            Assert.AreEqual(43, r[0]);
            Assert.AreEqual(c.Item1.Item2, r[0]);

            foreach (var value in new object[] { "bar", 42L, true })
            {
                Assert.ThrowsException<InvalidCastException>(() => r[0] = value);
            }

            foreach (var index in new[] { -1, 1, 2 })
            {
                Assert.ThrowsException<IndexOutOfRangeException>(() => r[index]);
                Assert.ThrowsException<IndexOutOfRangeException>(() => r[index] = 42);
            }
        }

        [TestMethod]
        public void CreateRuntimeVariables_Parent2()
        {
            var c = new Closure<Closure<Closure<bool, int, string>>>
            {
                Item1 = new Closure<Closure<bool, int, string>>
                {
                    Item1 = new Closure<bool, int, string>
                    {
                        Item2 = 42
                    }
                }
            };

            var r = RuntimeOpsEx.CreateRuntimeVariables(c, new[] { 2L << 32 | 1L });
            Assert.AreEqual(1, r.Count);
            Assert.AreEqual(c.Item1.Item1.Item2, r[0]);
            r[0] = 43;
            Assert.AreEqual(43, r[0]);
            Assert.AreEqual(c.Item1.Item1.Item2, r[0]);

            foreach (var value in new object[] { "bar", 42L, true })
            {
                Assert.ThrowsException<InvalidCastException>(() => r[0] = value);
            }

            foreach (var index in new[] { -1, 1, 2 })
            {
                Assert.ThrowsException<IndexOutOfRangeException>(() => r[index]);
                Assert.ThrowsException<IndexOutOfRangeException>(() => r[index] = 42);
            }
        }

        [TestMethod]
        public void Quote_NoHoisted1()
        {
            var h = GetEmptyHoistedLocals();
            var e = Expression.Lambda<Action>(Expression.Empty());
            var c = new Empty();
            var q = RuntimeOpsEx.Quote(e, h, c);
            Assert.AreSame(e, q);
        }

        [TestMethod]
        public void Quote_NoHoisted2()
        {
            var h = GetEmptyHoistedLocals();
            var e = (Expression<Func<int, int>>)(x => x);
            var c = new Empty();
            var q = RuntimeOpsEx.Quote(e, h, c);
            Assert.AreSame(e, q);
        }

        [TestMethod]
        public void Quote_NoHoisted3()
        {
            var h = GetEmptyHoistedLocals();
            var x = Expression.Parameter(typeof(int));
            var y = Expression.Parameter(typeof(Exception));
            var e = Expression.Lambda<Action>(Expression.Block(new[] { x }, Expression.TryCatch(Expression.Empty(), Expression.Catch(y, Expression.Empty(), Expression.Constant(true)))));
            var c = new Empty();
            var q = RuntimeOpsEx.Quote(e, h, c);
            Assert.AreSame(e, q);
        }

        [TestMethod]
        public void Quote_Hoisted_Block()
        {
            var x = Expression.Parameter(typeof(int));
            var variables = new ReadOnlyCollection<ParameterExpression>(new[] { x });
            var definitions = new Dictionary<ParameterExpression, StorageKind>
            {
                { x, StorageKind.Hoisted | StorageKind.Boxed } // NB: Using Boxed for LINQ ET compatibility.
            };
            var h = new HoistedLocals(parent: null, variables, definitions);

            var e = Expression.Lambda<Func<int>>(Expression.Block(new[] { Expression.Parameter(typeof(int)) }, x));

            Assert.AreEqual(typeof(Closure<StrongBox<int>>), h.Closure.ClosureType);

            var c = new Closure<StrongBox<int>> { Item1 = new StrongBox<int>(42) };
            var q = (Expression<Func<int>>)RuntimeOpsEx.Quote(e, h, c);
            var f = q.Compile();

            Assert.AreEqual(42, f());

            c.Item1.Value = 43;
            Assert.AreEqual(43, f());
        }

        [TestMethod]
        public void Quote_Hoisted_CatchBlock()
        {
            var x = Expression.Parameter(typeof(int));
            var variables = new ReadOnlyCollection<ParameterExpression>(new[] { x });
            var definitions = new Dictionary<ParameterExpression, StorageKind>
            {
                { x, StorageKind.Hoisted | StorageKind.Boxed } // NB: Using Boxed for LINQ ET compatibility.
            };
            var h = new HoistedLocals(parent: null, variables, definitions);

            var e = Expression.Lambda<Func<int>>(Expression.TryCatch(Expression.Throw(Expression.Constant(new Exception()), typeof(int)), Expression.Catch(typeof(Exception), x)));

            Assert.AreEqual(typeof(Closure<StrongBox<int>>), h.Closure.ClosureType);

            var c = new Closure<StrongBox<int>> { Item1 = new StrongBox<int>(42) };
            var q = (Expression<Func<int>>)RuntimeOpsEx.Quote(e, h, c);
            var f = q.Compile();

            Assert.AreEqual(42, f());

            c.Item1.Value = 43;
            Assert.AreEqual(43, f());
        }

        [TestMethod]
        public void Quote_Hoisted_Parent()
        {
            var x = Expression.Parameter(typeof(int));
            var variables = new ReadOnlyCollection<ParameterExpression>(new[] { x });
            var definitions = new Dictionary<ParameterExpression, StorageKind>
            {
                { x, StorageKind.Hoisted | StorageKind.Boxed } // NB: Using Boxed for LINQ ET compatibility.
            };
            var p = new HoistedLocals(parent: null, variables, definitions);
            var h = new HoistedLocals(p, new ReadOnlyCollection<ParameterExpression>(Array.Empty<ParameterExpression>()), new Dictionary<ParameterExpression, StorageKind>());

            var e = Expression.Lambda<Func<int>>(x);

            Assert.AreEqual(typeof(Closure<Closure<StrongBox<int>>>), h.Closure.ClosureType);

            var c = new Closure<Closure<StrongBox<int>>> { Item1 = new Closure<StrongBox<int>> { Item1 = new StrongBox<int>(42) } };
            var q = (Expression<Func<int>>)RuntimeOpsEx.Quote(e, h, c);
            var f = q.Compile();

            Assert.AreEqual(42, f());

            c.Item1.Item1.Value = 43;
            Assert.AreEqual(43, f());
        }

        [TestMethod]
        public void Quote_RuntimeVariables_Hoisted1()
        {
            var x = Expression.Parameter(typeof(int));
            var variables = new ReadOnlyCollection<ParameterExpression>(new[] { x });
            var definitions = new Dictionary<ParameterExpression, StorageKind>
            {
                { x, StorageKind.Hoisted | StorageKind.Boxed } // NB: Using Boxed for LINQ ET compatibility.
            };
            var h = new HoistedLocals(parent: null, variables, definitions);

            var e = Expression.Lambda<Func<IRuntimeVariables>>(Expression.RuntimeVariables(x));

            Assert.AreEqual(typeof(Closure<StrongBox<int>>), h.Closure.ClosureType);

            var c = new Closure<StrongBox<int>> { Item1 = new StrongBox<int>(42) };
            var q = (Expression<Func<IRuntimeVariables>>)RuntimeOpsEx.Quote(e, h, c);
            var f = q.Compile();

            Assert.AreEqual(42, f()[0]);
            Assert.AreEqual(1, f().Count);

            c.Item1.Value = 43;
            Assert.AreEqual(43, f()[0]);

            var r = f();
            r[0] = 44;
            Assert.AreEqual(44, r[0]);
            Assert.AreEqual(44, c.Item1.Value);
        }

        [TestMethod]
        public void Quote_RuntimeVariables_Hoisted2()
        {
            var x = Expression.Parameter(typeof(int));
            var variables = new ReadOnlyCollection<ParameterExpression>(Array.Empty<ParameterExpression>());
            var definitions = new Dictionary<ParameterExpression, StorageKind>();
            var h = new HoistedLocals(parent: null, variables, definitions);

            var e = Expression.Lambda<Func<int, IRuntimeVariables>>(Expression.RuntimeVariables(x), x);

            Assert.AreEqual(typeof(Empty), h.Closure.ClosureType);

            var c = new Empty();
            var q = (Expression<Func<int, IRuntimeVariables>>)RuntimeOpsEx.Quote(e, h, c);
            var f = q.Compile();

            Assert.AreEqual(42, f(42)[0]);
            Assert.AreEqual(1, f(41).Count);

            var r = f(43);
            r[0] = 44;
            Assert.AreEqual(44, r[0]);
        }

        [TestMethod]
        public void Quote_RuntimeVariables_Hoisted3()
        {
            var x = Expression.Parameter(typeof(int));
            var y = Expression.Parameter(typeof(int));
            var variables = new ReadOnlyCollection<ParameterExpression>(new[] { y });
            var definitions = new Dictionary<ParameterExpression, StorageKind>
            {
                { y, StorageKind.Hoisted | StorageKind.Boxed } // NB: Using Boxed for LINQ ET compatibility.
            };
            var h = new HoistedLocals(parent: null, variables, definitions);

            var e = Expression.Lambda<Func<int, IRuntimeVariables>>(Expression.RuntimeVariables(x, y), x);

            Assert.AreEqual(typeof(Closure<StrongBox<int>>), h.Closure.ClosureType);

            var c = new Closure<StrongBox<int>> { Item1 = new StrongBox<int>(42) };
            var q = (Expression<Func<int, IRuntimeVariables>>)RuntimeOpsEx.Quote(e, h, c);
            var f = q.Compile();

            Assert.AreEqual(41, f(41)[0]);
            Assert.AreEqual(42, f(41)[1]);
            Assert.AreEqual(2, f(41).Count);

            var r = f(43);
            r[0] = 44;
            r[1] = 45;
            Assert.AreEqual(44, r[0]);
            Assert.AreEqual(45, r[1]);
            Assert.AreEqual(45, c.Item1.Value);
        }

        private static HoistedLocals GetEmptyHoistedLocals() => new(parent: null, new ReadOnlyCollection<ParameterExpression>(Array.Empty<ParameterExpression>()), new Dictionary<ParameterExpression, StorageKind>());
    }
}
