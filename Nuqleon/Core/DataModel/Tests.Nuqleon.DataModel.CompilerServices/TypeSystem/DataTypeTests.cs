// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
//

#pragma warning disable IDE0050 // Convert to tuple. (Tests for anonymous types.)

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.CompilerServices.TypeSystem;
using System.Linq.Expressions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Nuqleon.DataModel;
using Nuqleon.DataModel.TypeSystem;

namespace Tests.Nuqleon.DataModel.CompilerServices.TypeSystem
{
    [TestClass]
    public class DataTypeTests
    {
        [TestMethod]
        public void DataType_FromType_ArgumentChecking()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => DataType.FromType(type: null), ex => Assert.AreEqual("type", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => DataType.FromType(type: null, allowCycles: false), ex => Assert.AreEqual("type", ex.ParamName));
        }

        [TestMethod]
        public void DataType_TryFromType_ArgumentChecking()
        {
            var dt = default(DataType);

            AssertEx.ThrowsException<ArgumentNullException>(() => DataType.TryFromType(type: null, out dt), ex => Assert.AreEqual("type", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => DataType.TryFromType(type: null, allowCycles: false, out dt), ex => Assert.AreEqual("type", ex.ParamName));
        }

        [TestMethod]
        public void DataType_Check_ArgumentChecking()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => DataType.Check(type: null), ex => Assert.AreEqual("type", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => DataType.Check(type: null, allowCycles: false), ex => Assert.AreEqual("type", ex.ParamName));
        }

        [TestMethod]
        public void DataType_TryCheck_ArgumentChecking()
        {
            var err = default(ReadOnlyCollection<DataTypeError>);

            AssertEx.ThrowsException<ArgumentNullException>(() => DataType.TryCheck(type: null, out err), ex => Assert.AreEqual("type", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => DataType.TryCheck(type: null, allowCycles: false, out err), ex => Assert.AreEqual("type", ex.ParamName));
        }

        [TestMethod]
        public void DataType_FromType_Cyclic()
        {
            Assert.ThrowsException<InvalidOperationException>(() => DataType.FromType(typeof(Bar), allowCycles: false));

            var bar = DataType.FromType(typeof(Bar), allowCycles: true);
            Assert.AreEqual(DataTypeKinds.Structural, bar.Kind);

            var barStruct = (StructuralDataType)bar;
            Assert.AreEqual(1, barStruct.Properties.Count);

            var foosProp = barStruct.Properties[0];
            Assert.AreEqual("foos", foosProp.Name);
            Assert.AreEqual(DataTypeKinds.Array, foosProp.Type.Kind);

            var foosArray = (ArrayDataType)foosProp.Type;
            Assert.AreEqual(DataTypeKinds.Structural, foosArray.ElementType.Kind);

            var fooStruct = (StructuralDataType)foosArray.ElementType;
            Assert.AreEqual(1, fooStruct.Properties.Count);

            var barProp = fooStruct.Properties[0];
            Assert.AreEqual("bar", barProp.Name);
            Assert.AreSame(barStruct, barProp.Type);
            /* don't indent */
            var expected = @"let t1 = { foos : t2[] }
and t2 = { bar : t1 }
in  t1
";
            /* don't indent */
            Assert.AreEqual(expected.Replace("\r", string.Empty), bar.ToString().Replace("\r", string.Empty));
        }

        [TestMethod]
        public void DataType_ToString()
        {
            var tests = new Dictionary<Type, string>
            {
                { typeof(int), "int" },
                { typeof(int?), "int?" },
                { typeof(int[]), "int[]" },
                { typeof(List<int>), "int[]" },
                { typeof(Func<int, double, bool>), "(int, double) => bool" },
                { new { a = 1, b = "" }.GetType(), "{ a : int; b : string }" },
                { typeof(Tuple<string, int>), "{ Item1 : string; Item2 : int }" },
                { typeof(BinaryExpression), "BinaryExpression" },
                { typeof(Expression<Func<int, bool>>), "@{ (int) => bool }" },
            };

            foreach (var kv in tests)
            {
                var s = DataType.FromType(kv.Key).ToString();
                Assert.AreEqual(kv.Value, s);
            }
        }

        [TestMethod]
        public void DataType_Primitive_Reference()
        {
            var s = DataType.FromType(typeof(string));

            Assert.AreEqual(DataTypeKinds.Primitive, s.Kind);
            var p = (PrimitiveDataType)s;

            Assert.IsTrue(p.IsNullable);
            Assert.AreSame(typeof(string), s.UnderlyingType);

            Assert.ThrowsException<ArgumentNullException>(() => s.CreateInstance(default));
            Assert.ThrowsException<InvalidOperationException>(() => s.CreateInstance());
            Assert.ThrowsException<InvalidOperationException>(() => s.CreateInstance("foo", "bar"));
            Assert.ThrowsException<InvalidOperationException>(() => s.CreateInstance(123));

            var x = s.CreateInstance("bar");
            Assert.AreEqual("bar", x);
        }

        [TestMethod]
        public void DataType_Primitive_NonNullableValue()
        {
            var s = DataType.FromType(typeof(int));

            Assert.AreEqual(DataTypeKinds.Primitive, s.Kind);
            var p = (PrimitiveDataType)s;

            Assert.IsFalse(p.IsNullable);
            Assert.AreSame(typeof(int), s.UnderlyingType);

            Assert.ThrowsException<InvalidOperationException>(() => s.CreateInstance());
            Assert.ThrowsException<InvalidOperationException>(() => s.CreateInstance(1, 2));
            Assert.ThrowsException<InvalidOperationException>(() => s.CreateInstance("bar"));
            Assert.ThrowsException<InvalidOperationException>(() => s.CreateInstance(new object[1] { null }));

            var x = s.CreateInstance(42);
            Assert.AreEqual(42, x);
        }

        [TestMethod]
        public void DataType_Primitive_NullableValue()
        {
            var s = DataType.FromType(typeof(int?));

            Assert.AreEqual(DataTypeKinds.Primitive, s.Kind);
            var p = (PrimitiveDataType)s;

            Assert.IsTrue(p.IsNullable);
            Assert.AreSame(typeof(int?), s.UnderlyingType);

            Assert.ThrowsException<InvalidOperationException>(() => s.CreateInstance());
            Assert.ThrowsException<InvalidOperationException>(() => s.CreateInstance(1, 2));
            Assert.ThrowsException<InvalidOperationException>(() => s.CreateInstance("bar"));

            var x = s.CreateInstance(42);
            Assert.AreEqual(42, x);

            var y = s.CreateInstance(new object[1] { null });
            Assert.IsNull(y);

            var z = s.CreateInstance((int?)42);
            Assert.AreEqual(42, z);
        }

        [TestMethod]
        public void DataType_Structural_Anonymous()
        {
            var t = DataType.FromType(new { a = 1, b = "bar" }.GetType());

            Assert.AreEqual(DataTypeKinds.Structural, t.Kind);
            var s = (StructuralDataType)t;

            Assert.AreEqual(StructuralDataTypeKinds.Anonymous, s.StructuralKind);
            Assert.AreEqual(2, s.Properties.Count);

            Assert.ThrowsException<ArgumentNullException>(() => s.CreateInstance(default));
            Assert.ThrowsException<InvalidOperationException>(() => s.CreateInstance());
            Assert.ThrowsException<InvalidOperationException>(() => s.CreateInstance(42));
            Assert.ThrowsException<InvalidOperationException>(() => s.CreateInstance(42, "foo", 12));
            Assert.ThrowsException<MissingMethodException>(() => s.CreateInstance("foo", 42)); // Acceptable error

            var o = t.CreateInstance(42, "foo");

            Assert.AreEqual(new { a = 42, b = "foo" }, o);

            Assert.AreEqual(42, s.Properties[0].GetValue(o));

            Assert.ThrowsException<ArgumentException>(() => s.Properties[0].SetValue(o, 42)); // Acceptable error
        }

        [TestMethod]
        public void DataType_Structural_Record()
        {
            var rec = RuntimeCompiler.CreateRecordType(new[] { new KeyValuePair<string, Type>("a", typeof(int)), new KeyValuePair<string, Type>("b", typeof(string)) }, valueEquality: true);
            var t = DataType.FromType(rec);

            Assert.AreEqual(DataTypeKinds.Structural, t.Kind);
            var s = (StructuralDataType)t;

            Assert.AreEqual(StructuralDataTypeKinds.Record, s.StructuralKind);
            Assert.AreEqual(2, s.Properties.Count);

            Assert.ThrowsException<ArgumentNullException>(() => s.CreateInstance(default));
            Assert.ThrowsException<InvalidOperationException>(() => s.CreateInstance(42));
            Assert.ThrowsException<InvalidOperationException>(() => s.CreateInstance(42, "foo", 12));
            Assert.ThrowsException<InvalidOperationException>(() => s.CreateInstance("foo", 42));

            var o = t.CreateInstance();
            var a = s.Properties.Single(p => p.Name == "a");
            a.SetValue(o, 42);
            var b = s.Properties.Single(p => p.Name == "b");
            b.SetValue(o, "foo");

            Assert.AreEqual(42, a.GetValue(o));
            Assert.AreEqual("foo", b.GetValue(o));
        }

        [TestMethod]
        public void DataType_Structural_Tuple()
        {
            var t = DataType.FromType(typeof(Tuple<int, string>));

            Assert.AreEqual(DataTypeKinds.Structural, t.Kind);
            var s = (StructuralDataType)t;

            Assert.AreEqual(StructuralDataTypeKinds.Tuple, s.StructuralKind);
            Assert.AreEqual(2, s.Properties.Count);

            Assert.ThrowsException<InvalidOperationException>(() => s.CreateInstance());
            Assert.ThrowsException<InvalidOperationException>(() => s.CreateInstance(42));
            Assert.ThrowsException<InvalidOperationException>(() => s.CreateInstance(42, "foo", 12));
            Assert.ThrowsException<MissingMethodException>(() => s.CreateInstance("foo", 42)); // Acceptable error

            var o = t.CreateInstance(42, "foo");

            Assert.AreEqual(new Tuple<int, string>(42, "foo"), o);

            Assert.AreEqual(42, s.Properties[0].GetValue(o));

            Assert.ThrowsException<ArgumentException>(() => s.Properties[0].SetValue(o, 42)); // Acceptable error
        }

        [TestMethod]
        public void DataType_Structural_Array()
        {
            var t = DataType.FromType(typeof(int[]));

            Assert.AreEqual(DataTypeKinds.Array, t.Kind);
            var a = (ArrayDataType)t;

            var l = a.GetList(new int[] { 1, 2, 3 });
            Assert.IsTrue(new[] { 1, 2, 3 }.SequenceEqual(l.Cast<int>()));

            l[0] = 5;
            Assert.IsTrue(new[] { 5, 2, 3 }.SequenceEqual(l.Cast<int>()));

            Assert.AreEqual(3, l[2]);

            Assert.AreEqual(3, l.Count);

            Assert.ThrowsException<NotSupportedException>(() => l.Remove(2));
            Assert.ThrowsException<NotSupportedException>(() => l.RemoveAt(0));
            Assert.ThrowsException<NotSupportedException>(() => l.Add(7));

            var b = t.CreateInstance(4);
            Assert.IsTrue(b is int[]);
            var c = a.GetList(b);
            Assert.AreEqual(4, c.Count);

            Assert.ThrowsException<ArgumentNullException>(() => a.CreateInstance(default));
            Assert.ThrowsException<InvalidOperationException>(() => t.CreateInstance());
            Assert.ThrowsException<InvalidOperationException>(() => t.CreateInstance(1, 2));
        }

        [TestMethod]
        public void DataType_Structural_List()
        {
            var t = DataType.FromType(typeof(IList<int>));

            Assert.AreEqual(DataTypeKinds.Array, t.Kind);
            var a = (ArrayDataType)t;

            var l = a.GetList(new List<int> { 1, 2, 3 });
            Assert.IsTrue(new[] { 1, 2, 3 }.SequenceEqual(l.Cast<int>()));

            l[0] = 5;
            Assert.IsTrue(new[] { 5, 2, 3 }.SequenceEqual(l.Cast<int>()));

            Assert.AreEqual(3, l[2]);

            Assert.AreEqual(3, l.Count);

            var b = t.CreateInstance(4);
            Assert.IsTrue(b is List<int>);
            var c = a.GetList(b);

            c.Add(1);
            c.Add(2);
            c.Add(3);
            c.Add(4);

            Assert.AreEqual(4, c.Count);

            Assert.IsTrue(new[] { 1, 2, 3, 4 }.SequenceEqual(c.Cast<int>()));

            Assert.ThrowsException<InvalidOperationException>(() => t.CreateInstance());
            Assert.ThrowsException<InvalidOperationException>(() => t.CreateInstance(1, 2));
        }

        [TestMethod]
        public void DataType_Expression()
        {
            var t = DataType.FromType(typeof(BinaryExpression));

            Assert.AreEqual(DataTypeKinds.Expression, t.Kind);
            var e = (ExpressionDataType)t;

            var a = Expression.Add(Expression.Constant(1), Expression.Constant(2));
            var b = e.GetExpression(a);
            Assert.AreSame(a, b);

            Assert.ThrowsException<ArgumentNullException>(() => e.CreateInstance(default));
            var o = e.CreateInstance(a);
            Assert.AreSame(a, o);
        }

        [TestMethod]
        public void DataType_Function()
        {
            var t = DataType.FromType(typeof(Func<int, int>));

            Assert.AreEqual(DataTypeKinds.Function, t.Kind);
            var f = (FunctionDataType)t;

            var g = new Func<int, int>(x => x);
            var h = f.GetFunction(g);
            Assert.AreSame(h, g);

            Assert.ThrowsException<ArgumentNullException>(() => f.CreateInstance(default));
            var o = f.CreateInstance(g);
            Assert.AreSame(g, o);
        }

        [TestMethod]
        public void DataType_IsStructuralEntityDataType_ArgumentChecking()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => DataType.IsStructuralEntityDataType(type: null), ex => Assert.AreEqual("type", ex.ParamName));
        }

        [TestMethod]
        public void DataType_Wildcard()
        {
            var t = DataType.FromType(typeof(Func<T, R>));

            Assert.AreEqual(DataTypeKinds.Function, t.Kind);
            var f = (FunctionDataType)t;

            Assert.AreEqual(1, f.ParameterTypes.Count);
            Assert.AreEqual(DataTypeKinds.OpenGenericParameter, f.ReturnType.Kind);
            Assert.AreEqual(DataTypeKinds.OpenGenericParameter, f.ParameterTypes[0].Kind);

            var g = new Func<int, int>(x => x);
            // TODO: OpenGenericParameterDataType should support unification of wildcards.
            Assert.ThrowsException<InvalidOperationException>(() => f.GetFunction(g));

            Assert.ThrowsException<ArgumentNullException>(() => f.CreateInstance(default));

            // TODO: OpenGenericParameterDataType should support unification of wildcards.
            Assert.ThrowsException<InvalidOperationException>(() => f.CreateInstance(g));
        }

        [TestMethod]
        public void DataType_FromTypeCached()
        {
            var dt1 = DataTypeHelpers.FromTypeCached(typeof(CacheTest), allowCycles: true);
            var dt2 = DataTypeHelpers.FromTypeCached(typeof(CacheTest), allowCycles: true);
            var dt3 = DataTypeHelpers.FromTypeCached(typeof(CacheTest), allowCycles: false);
            var dt4 = DataTypeHelpers.FromTypeCached(typeof(CacheTest), allowCycles: false);

            Assert.AreSame(dt1, dt2);
            Assert.AreSame(dt3, dt4);

            Assert.ThrowsException<NotSupportedException>(() => DataTypeHelpers.FromTypeCached(typeof(NonDTCacheTest), allowCycles: true));
            Assert.ThrowsException<NotSupportedException>(() => DataTypeHelpers.FromTypeCached(typeof(NonDTCacheTest), allowCycles: false));
        }

        [TestMethod]
        public void DataType_TryFromTypeCached()
        {
            Assert.IsTrue(DataTypeHelpers.TryFromTypeCached(typeof(CacheTest), allowCycles: true, out var dt1));
            Assert.IsTrue(DataTypeHelpers.TryFromTypeCached(typeof(CacheTest), allowCycles: true, out var dt2));
            Assert.IsTrue(DataTypeHelpers.TryFromTypeCached(typeof(CacheTest), allowCycles: false, out var dt3));
            Assert.IsTrue(DataTypeHelpers.TryFromTypeCached(typeof(CacheTest), allowCycles: false, out var dt4));

            Assert.AreSame(dt1, dt2);
            Assert.AreSame(dt3, dt4);

            Assert.ThrowsException<NotSupportedException>(() => DataTypeHelpers.FromTypeCached(typeof(NonDTCacheTest), allowCycles: true));
            Assert.ThrowsException<NotSupportedException>(() => DataTypeHelpers.FromTypeCached(typeof(NonDTCacheTest), allowCycles: false));

            Assert.IsFalse(DataTypeHelpers.TryFromTypeCached(typeof(NonDTCacheTest), allowCycles: true, out var d));
            Assert.IsFalse(DataTypeHelpers.TryFromTypeCached(typeof(NonDTCacheTest), allowCycles: false, out d));
        }

        [TestMethod]
        public void DataType_IsStructuralEntityDataTypeCached()
        {
            Assert.IsTrue(DataTypeHelpers.IsStructuralEntityDataTypeCached(typeof(CacheTest)));
            Assert.IsFalse(DataTypeHelpers.IsStructuralEntityDataTypeCached(typeof(NonDTCacheTest)));
        }

        [TestMethod]
        public void DataType_IsEntityEnumDataTypeCached()
        {
            Assert.IsTrue(DataTypeHelpers.IsEntityEnumDataTypeCached(typeof(EnumCacheTest)));
            Assert.IsFalse(DataTypeHelpers.IsEntityEnumDataTypeCached(typeof(ConsoleColor)));
        }

        [TestMethod]
        public void DataType_CheckCached()
        {
            DataTypeHelpers.CheckCached(typeof(CacheTest), allowCycles: true);
            DataTypeHelpers.CheckCached(typeof(CacheTest), allowCycles: false);

            Assert.ThrowsException<AggregateException>(() => DataTypeHelpers.CheckCached(typeof(NonDTCacheTest), allowCycles: true));
            Assert.ThrowsException<AggregateException>(() => DataTypeHelpers.CheckCached(typeof(NonDTCacheTest), allowCycles: false));
        }

        [TestMethod]
        public void DataType_TryCheckCached()
        {

            Assert.IsTrue(DataTypeHelpers.TryCheckCached(typeof(CacheTest), allowCycles: false, out var e1));
            Assert.AreEqual(0, e1.Count);

            Assert.IsFalse(DataTypeHelpers.TryCheckCached(typeof(NonDTCacheTest), allowCycles: false, out var e2));
            Assert.IsFalse(DataTypeHelpers.TryCheckCached(typeof(NonDTCacheTest), allowCycles: false, out var e3));
            Assert.AreSame(e2, e3);
        }

        private class CacheTest
        {
            [Mapping("Foo")]
            public int Foo { get; set; }
        }

        private class NonDTCacheTest
        {
            public int Foo { get; set; }
        }

        private enum EnumCacheTest
        {
            [Mapping("x")]
            X,
        }

        private class Bar
        {
            [Mapping("foos")]
            public Foo[] Foos { get; set; }
        }

        private class Foo
        {
            [Mapping("bar")]
            public Bar Bar { get; set; }
        }
    }
}
