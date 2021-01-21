// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

#if NETCOREAPP2_1

using System;
using System.Reflection;
using Nuqleon.Reflection.Emit;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Nuqleon.Linq.CompilerServices.Reflection.Emit
{
    [TestClass]
    public class EmitTests
    {
        [TestMethod]
        public void Emit_LateBound()
        {
            var asm = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("bar"), AssemblyBuilderAccess.RunAndCollect);
            var mod = asm.DefineDynamicModule("foo");
            var typ = mod.DefineType("qux", TypeAttributes.Public | TypeAttributes.Class);

            Assert.AreEqual("bar", typ.Assembly.GetName().Name);
            Assert.IsNotNull(typ.Module);

            Assert.AreEqual(typeof(object), typ.BaseType);

            Assert.AreEqual("qux", typ.FullName);
            Assert.AreEqual("qux", typ.Name);
            Assert.AreEqual("qux", typ.UnderlyingSystemType.Name);
            Assert.AreEqual("qux, bar, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", typ.AssemblyQualifiedName);
            Assert.AreEqual("", typ.Namespace);

            Assert.ThrowsException<NotSupportedException>(() => _ = typ.GUID);
            Assert.ThrowsException<NotSupportedException>(() => _ = typ.GetElementType());

            Assert.IsFalse(typ.HasElementType);
            Assert.IsFalse(typ.IsAbstract);
            Assert.IsFalse(typ.IsArray);
            Assert.IsFalse(typ.IsByRef);
            Assert.IsFalse(typ.IsCOMObject);
            Assert.IsFalse(typ.IsPointer);
            Assert.IsFalse(typ.IsPrimitive);

            var arr1 = typ.MakeArrayType();
            Assert.IsTrue(arr1.IsArray);
            Assert.AreSame(typ.UnderlyingSystemType, arr1.GetElementType());

            var arr2 = typ.MakeArrayType(2);
            Assert.IsTrue(arr2.IsArray);
            Assert.AreSame(typ.UnderlyingSystemType, arr2.GetElementType());

            var @ref = typ.MakeByRefType();
            Assert.IsTrue(@ref.IsByRef);
            Assert.AreSame(typ.UnderlyingSystemType, @ref.GetElementType());

            var ptr = typ.MakePointerType();
            Assert.IsTrue(ptr.IsPointer);
            Assert.AreSame(typ.UnderlyingSystemType, ptr.GetElementType());

            var fld = typ.DefineField("x", typeof(int), FieldAttributes.Public);

            Assert.AreEqual("x", fld.Name);
            Assert.AreSame(typeof(int), fld.FieldType);
            Assert.AreEqual(FieldAttributes.Public, fld.Attributes);
            Assert.AreSame(typ.UnderlyingSystemType, fld.DeclaringType);
            Assert.AreSame(typ.UnderlyingSystemType, fld.ReflectedType);
            Assert.IsTrue(fld.IsPublic);

            Assert.ThrowsException<NotSupportedException>(() => _ = fld.FieldHandle);
            Assert.ThrowsException<NotSupportedException>(() => _ = fld.GetValue(new object()));
            Assert.ThrowsException<NotSupportedException>(() => fld.SetValue(new object(), new object(), BindingFlags.Public, binder: null, culture: null));

            Assert.ThrowsException<NotSupportedException>(() => _ = typ.GetField("x", BindingFlags.Public));
            Assert.ThrowsException<NotSupportedException>(() => _ = typ.GetFields(BindingFlags.Public));

            Assert.ThrowsException<NotSupportedException>(() => _ = fld.IsDefined(typeof(ObsoleteAttribute)));
            Assert.ThrowsException<NotSupportedException>(() => _ = fld.GetCustomAttributes(inherit: true));
            Assert.ThrowsException<NotSupportedException>(() => _ = fld.GetCustomAttributes(typeof(ObsoleteAttribute), inherit: true));

            var prp = typ.DefineProperty("X", PropertyAttributes.None, typeof(int), Type.EmptyTypes);

            Assert.AreEqual("X", prp.Name);
            Assert.AreSame(typeof(int), prp.PropertyType);
            Assert.AreEqual(PropertyAttributes.None, prp.Attributes);
            Assert.AreSame(typ.UnderlyingSystemType, prp.DeclaringType);
            Assert.AreSame(typ.UnderlyingSystemType, prp.ReflectedType);

            Assert.IsFalse(prp.CanRead);
            Assert.IsFalse(prp.CanWrite);

            var prp_get = typ.DefineMethod("get_X", MethodAttributes.Public, typeof(int), Type.EmptyTypes);
            var prp_set = typ.DefineMethod("set_X", MethodAttributes.Public, typeof(void), new[] { typeof(int) });

            prp.SetGetMethod(prp_get);
            prp.SetSetMethod(prp_set);

            Assert.IsTrue(prp.CanRead);
            Assert.IsTrue(prp.CanWrite);

            Assert.AreSame(prp_get.Unwrap(), prp.GetGetMethod());
            Assert.AreSame(prp_set.Unwrap(), prp.GetSetMethod());

            Assert.ThrowsException<NotSupportedException>(() => _ = prp.GetAccessors());

            Assert.ThrowsException<NotSupportedException>(() => _ = prp.GetValue(new object()));
            Assert.ThrowsException<NotSupportedException>(() => prp.SetValue(new object(), new object(), BindingFlags.Public, index: null, binder: null, culture: null));

            Assert.ThrowsException<NotSupportedException>(() => _ = typ.GetProperty("X", BindingFlags.Public));
            Assert.ThrowsException<NotSupportedException>(() => _ = typ.GetProperties(BindingFlags.Public));

            Assert.ThrowsException<NotSupportedException>(() => _ = prp.IsDefined(typeof(ObsoleteAttribute)));
            Assert.ThrowsException<NotSupportedException>(() => _ = prp.GetCustomAttributes(inherit: true));
            Assert.ThrowsException<NotSupportedException>(() => _ = prp.GetCustomAttributes(typeof(ObsoleteAttribute), inherit: true));

            Assert.AreEqual("get_X", prp_get.Name);
            Assert.AreEqual(MethodAttributes.Public, prp_get.Attributes);
            Assert.AreSame(typ.UnderlyingSystemType, prp_get.DeclaringType);
            Assert.AreSame(typ.UnderlyingSystemType, prp_get.ReflectedType);
            Assert.AreEqual("get_X", prp_get.GetBaseDefinition().Name);
            Assert.AreEqual(MethodImplAttributes.IL, prp_get.GetMethodImplementationFlags());

            Assert.ThrowsException<NotSupportedException>(() => _ = prp_get.MethodHandle);

            Assert.IsNull(prp_get.ReturnTypeCustomAttributes);
            Assert.ThrowsException<NotSupportedException>(() => _ = prp_get.IsDefined(typeof(ObsoleteAttribute)));
            Assert.ThrowsException<NotSupportedException>(() => _ = prp_get.GetCustomAttributes(inherit: true));
            Assert.ThrowsException<NotSupportedException>(() => _ = prp_get.GetCustomAttributes(typeof(ObsoleteAttribute), inherit: true));
            Assert.ThrowsException<NotSupportedException>(() => _ = prp_get.GetParameters());
            Assert.ThrowsException<NotSupportedException>(() => _ = prp_get.Invoke(new object(), Array.Empty<object>()));

            var ctor = typ.DefineConstructor(MethodAttributes.Public | MethodAttributes.SpecialName, CallingConventions.Standard, new[] { typeof(int) });

            Assert.AreEqual(".ctor", ctor.Name);
            Assert.AreEqual(MethodAttributes.Public | MethodAttributes.SpecialName, ctor.Attributes);
            Assert.AreSame(typ.UnderlyingSystemType, ctor.DeclaringType);
            Assert.AreSame(typ.UnderlyingSystemType, ctor.ReflectedType);
            Assert.AreEqual(MethodImplAttributes.IL, ctor.GetMethodImplementationFlags());

            Assert.ThrowsException<NotSupportedException>(() => _ = ctor.MethodHandle);

            Assert.ThrowsException<NotSupportedException>(() => _ = ctor.IsDefined(typeof(ObsoleteAttribute)));
            Assert.ThrowsException<NotSupportedException>(() => _ = ctor.GetCustomAttributes(inherit: true));
            Assert.ThrowsException<NotSupportedException>(() => _ = ctor.GetCustomAttributes(typeof(ObsoleteAttribute), inherit: true));
            Assert.ThrowsException<NotSupportedException>(() => _ = ctor.GetParameters());
            Assert.ThrowsException<NotSupportedException>(() => _ = ctor.Invoke(Array.Empty<object>()));

            Assert.ThrowsException<NotSupportedException>(() => _ = typ.GetConstructor(new[] { typeof(int) }));
            Assert.ThrowsException<NotSupportedException>(() => _ = typ.GetConstructors(BindingFlags.Public));
        }
    }
}

#endif
