// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Memory;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Reflection.Virtualization
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void Type_Properties() => WithProviders(p =>
        {
            foreach (var t in new[]
            {
                    typeof(object),
                    typeof(int),
                    typeof(string),
                    typeof(ConsoleColor),
                    typeof(int[]),
                    typeof(string[,]),
                    typeof(IFormattable),
                    typeof(IEnumerable<>),
                    typeof(IEnumerable<int>),
                })
            {
                Assert.AreEqual(t.Assembly, p.GetAssembly(t));
                Assert.AreEqual(t.Module, p.GetModule(t));

                Assert.AreEqual(t.BaseType, p.GetBaseType(t));

                Assert.AreEqual(t.IsAssignableFrom(typeof(object)), p.IsAssignableFrom(t, typeof(object)));
                Assert.AreEqual(t.IsSubclassOf(typeof(object)), p.IsSubclassOf(t, typeof(object)));
                Assert.AreEqual(t.IsEquivalentTo(typeof(object)), p.IsEquivalentTo(t, typeof(object)));
                Assert.AreEqual(t.IsInstanceOfType("foo"), p.IsInstanceOfType(t, "foo"));

                Assert.AreEqual(t.IsAbstract, p.IsAbstract(t));
                Assert.AreEqual(t.IsAnsiClass, p.IsAnsiClass(t));
                Assert.AreEqual(t.IsArray, p.IsArray(t));
                Assert.AreEqual(t.IsAutoClass, p.IsAutoClass(t));
                Assert.AreEqual(t.IsAutoLayout, p.IsAutoLayout(t));
                Assert.AreEqual(t.IsByRef, p.IsByRef(t));
                Assert.AreEqual(t.IsClass, p.IsClass(t));
                Assert.AreEqual(t.IsConstructedGenericType, p.IsConstructedGenericType(t));
                Assert.AreEqual(t.IsEnum, p.IsEnum(t));
                Assert.AreEqual(t.IsExplicitLayout, p.IsExplicitLayout(t));
                Assert.AreEqual(t.IsGenericParameter, p.IsGenericParameter(t));
                Assert.AreEqual(t.IsGenericType, p.IsGenericType(t));
                Assert.AreEqual(t.IsGenericTypeDefinition, p.IsGenericTypeDefinition(t));
                Assert.AreEqual(t.IsImport, p.IsImport(t));
                Assert.AreEqual(t.IsInterface, p.IsInterface(t));
                Assert.AreEqual(t.IsLayoutSequential, p.IsLayoutSequential(t));
                Assert.AreEqual(t.IsNested, p.IsNested(t));
                Assert.AreEqual(t.IsNestedAssembly, p.IsNestedAssembly(t));
                Assert.AreEqual(t.IsNestedFamANDAssem, p.IsNestedFamANDAssem(t));
                Assert.AreEqual(t.IsNestedFamily, p.IsNestedFamily(t));
                Assert.AreEqual(t.IsNestedFamORAssem, p.IsNestedFamORAssem(t));
                Assert.AreEqual(t.IsNestedPrivate, p.IsNestedPrivate(t));
                Assert.AreEqual(t.IsNestedPublic, p.IsNestedPublic(t));
                Assert.AreEqual(t.IsNotPublic, p.IsNotPublic(t));
                Assert.AreEqual(t.IsPointer, p.IsPointer(t));
                Assert.AreEqual(t.IsPrimitive, p.IsPrimitive(t));
                Assert.AreEqual(t.IsPublic, p.IsPublic(t));
                Assert.AreEqual(t.IsSealed, p.IsSealed(t));
                Assert.AreEqual(t.IsSpecialName, p.IsSpecialName(t));
                Assert.AreEqual(t.IsUnicodeClass, p.IsUnicodeClass(t));
                Assert.AreEqual(t.IsValueType, p.IsValueType(t));
                Assert.AreEqual(t.IsVisible, p.IsVisible(t));

                Assert.AreEqual(t.IsCOMObject, p.IsCOMObject(t));
                Assert.AreEqual(t.IsContextful, p.IsContextful(t));
                Assert.AreEqual(t.IsMarshalByRef, p.IsMarshalByRef(t));
                Assert.AreEqual(t.IsSecurityCritical, p.IsSecurityCritical(t));
                Assert.AreEqual(t.IsSecuritySafeCritical, p.IsSecuritySafeCritical(t));
                Assert.AreEqual(t.IsSecurityTransparent, p.IsSecurityTransparent(t));
                Assert.AreEqual(t.IsSerializable, p.IsSerializable(t));

                Assert.AreEqual(t.ContainsGenericParameters, p.ContainsGenericParameters(t));
                Assert.AreEqual(t.HasElementType, p.HasElementType(t));

                Assert.AreEqual(t.AssemblyQualifiedName, p.GetAssemblyQualifiedName(t));
                Assert.AreEqual(t.Attributes, p.GetAttributes(t));
                Assert.AreEqual(t.FullName, p.GetFullName(t));
                Assert.AreEqual(t.Name, p.GetName(t));
                Assert.AreEqual(t.Namespace, p.GetNamespace(t));
                Assert.AreEqual(t.GUID, p.GetGuid(t));

                if (t.IsEnum)
                {
                    Assert.AreEqual(t.UnderlyingSystemType, p.GetUnderlyingSystemType(t));
                }
            }
        });

        [TestMethod]
        public void Type_Make() => WithProviders(p =>
        {
            foreach (var t in new[]
            {
                typeof(object),
                typeof(int),
                typeof(string),
            })
            {
                Assert.AreEqual(t.MakeByRefType(), p.MakeByRefType(t));
                Assert.AreEqual(t.MakePointerType(), p.MakePointerType(t));
                Assert.AreEqual(t.MakeArrayType(), p.MakeArrayType(t));
                Assert.AreEqual(t.MakeArrayType(rank: 2), p.MakeArrayType(t, rank: 2));
            }
        });

        [TestMethod]
        public void Type_Arrays() => WithProviders(p =>
        {
            foreach (var t in new[]
            {
                typeof(int[]),
                typeof(int).MakeByRefType(),
            })
            {
                Assert.AreEqual(t.GetElementType(), p.GetElementType(t));
            }

            foreach (var t in new[]
            {
                typeof(int[]),
                typeof(int[,]),
                typeof(int[,,]),
            })
            {
                Assert.AreEqual(t.GetArrayRank(), p.GetArrayRank(t));
            }
        });

        [TestMethod]
        public void Type_Generics() => WithProviders(p =>
        {
            foreach (var t in new[]
            {
                typeof(List<int>),
                typeof(List<string>),
                typeof(List<int>), // memoize
                typeof(Dictionary<string, int>),
                typeof(Dictionary<string, bool>),
                typeof(Dictionary<int, int>),
                typeof(Dictionary<string, int>), // memoize
            })
            {
                Assert.AreEqual(t.GetGenericTypeDefinition(), p.GetGenericTypeDefinition(t));
                CollectionAssert.AreEqual(t.GetGenericArguments(), p.GetGenericArguments(t).ToArray());
            }

            {
                var t1 = typeof(Dictionary<,>);
                var t2 = typeof(List<>);

                Assert.AreEqual(t1.MakeGenericType(new[] { typeof(int), typeof(string) }), p.MakeGenericType(t1, new[] { typeof(int), typeof(string) }));
                Assert.AreEqual(t1.MakeGenericType(new[] { typeof(bool), typeof(string) }), p.MakeGenericType(t1, new[] { typeof(bool), typeof(string) }));

                Assert.AreEqual(t2.MakeGenericType(new[] { typeof(int) }), p.MakeGenericType(t2, new[] { typeof(int) }));

                Assert.AreEqual(t1.MakeGenericType(new[] { typeof(int), typeof(string) }), p.MakeGenericType(t1, new[] { typeof(int), typeof(string) }));

                Assert.AreEqual(t2.MakeGenericType(new[] { typeof(int) }), p.MakeGenericType(t2, new[] { typeof(int) }));
                Assert.AreEqual(t2.MakeGenericType(new[] { typeof(string) }), p.MakeGenericType(t2, new[] { typeof(string) }));

                Assert.AreEqual(t1.MakeGenericType(new[] { typeof(int), typeof(bool) }), p.MakeGenericType(t1, new[] { typeof(int), typeof(bool) }));
            }
        });

        [TestMethod]
        public void Type_Members() => WithProviders(p =>
        {
            foreach (var t in new[]
            {
                typeof(object),
                typeof(int),
                typeof(string),
                typeof(int), // memoize
                typeof(string), // memoize
                typeof(Console),
                typeof(List<int>),
            })
            {
                // NB: Using AreEquivalent because some APIs do not consistently return in the same order (e.g. GetMembers).

                CollectionAssert.AreEquivalent(t.GetInterfaces(), p.GetInterfaces(t).ToArray());

                CollectionAssert.AreEquivalent(t.GetConstructors(), p.GetConstructors(t).ToArray());
                CollectionAssert.AreEquivalent(t.GetEvents(), p.GetEvents(t).ToArray());
                CollectionAssert.AreEquivalent(t.GetFields(), p.GetFields(t).ToArray());
                CollectionAssert.AreEquivalent(t.GetMembers(), p.GetMembers(t).ToArray());
                CollectionAssert.AreEquivalent(t.GetMethods(), p.GetMethods(t).ToArray());
                CollectionAssert.AreEquivalent(t.GetNestedTypes(), p.GetNestedTypes(t).ToArray());
                CollectionAssert.AreEquivalent(t.GetProperties(), p.GetProperties(t).ToArray());

                for (var i = 0; i < 3; i++)
                {
                    foreach (var f in new[]
                    {
                        BindingFlags.NonPublic | BindingFlags.Instance,
                        BindingFlags.NonPublic | BindingFlags.Static,
                        BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance,
                        BindingFlags.Public | BindingFlags.Instance,
                        BindingFlags.Public | BindingFlags.Static,
                        BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance,
                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance,
                    })
                    {
                        CollectionAssert.AreEquivalent(t.GetConstructors(f), p.GetConstructors(t, f).ToArray());
                        CollectionAssert.AreEquivalent(t.GetEvents(f), p.GetEvents(t, f).ToArray());
                        CollectionAssert.AreEquivalent(t.GetFields(f), p.GetFields(t, f).ToArray());
                        CollectionAssert.AreEquivalent(t.GetMembers(f), p.GetMembers(t, f).ToArray());
                        CollectionAssert.AreEquivalent(t.GetMethods(f), p.GetMethods(t, f).ToArray());
                        CollectionAssert.AreEquivalent(t.GetNestedTypes(f), p.GetNestedTypes(t, f).ToArray());
                        CollectionAssert.AreEquivalent(t.GetProperties(f), p.GetProperties(t, f).ToArray());
                    }
                }
            }
        });

        [TestMethod]
        public void Type_Members_Trivial() => WithProviders(p =>
        {
            var length = p.GetProperty(typeof(string), nameof(string.Length));
            Assert.AreEqual(3, length.GetValue("bar"));

            Assert.AreEqual(length, p.GetProperty(typeof(string), nameof(string.Length), typeof(int)));
            Assert.AreEqual(length, p.GetProperty(typeof(string), nameof(string.Length), Type.EmptyTypes));
            Assert.AreEqual(length, p.GetProperty(typeof(string), nameof(string.Length), typeof(int), Type.EmptyTypes));
            Assert.AreEqual(length, p.GetProperty(typeof(string), nameof(string.Length), typeof(int), Type.EmptyTypes, modifiers: null));
            Assert.AreEqual(length, p.GetProperty(typeof(string), nameof(string.Length), BindingFlags.Public | BindingFlags.Instance));
            Assert.AreEqual(length, p.GetProperty(typeof(string), nameof(string.Length), BindingFlags.Public | BindingFlags.Instance, binder: null, typeof(int), Type.EmptyTypes, modifiers: null));

            var substring = p.GetMethod(typeof(string), nameof(string.Substring), new[] { typeof(int), typeof(int) });
            Assert.AreEqual("barq", substring.Invoke("foobarqux", new object[] { 3, 4 }));

            Assert.AreEqual(substring, p.GetMethod(typeof(string), nameof(string.Substring), new[] { typeof(int), typeof(int) }, modifiers: null));
            Assert.AreEqual(substring, p.GetMethod(typeof(string), nameof(string.Substring), BindingFlags.Public | BindingFlags.Instance, binder: null, new[] { typeof(int), typeof(int) }, modifiers: null));
            Assert.AreEqual(substring, p.GetMethod(typeof(string), nameof(string.Substring), BindingFlags.Public | BindingFlags.Instance, binder: null, CallingConventions.Any, new[] { typeof(int), typeof(int) }, modifiers: null));

#if NET5_0 || NETSTANDARD2_1
            Assert.AreEqual(substring, p.GetMethod(typeof(string), nameof(string.Substring), genericParameterCount: 0, new[] { typeof(int), typeof(int) }));
            Assert.AreEqual(substring, p.GetMethod(typeof(string), nameof(string.Substring), genericParameterCount: 0, new[] { typeof(int), typeof(int) }, modifiers: null));
            Assert.AreEqual(substring, p.GetMethod(typeof(string), nameof(string.Substring), genericParameterCount: 0, BindingFlags.Public | BindingFlags.Instance, binder: null, new[] { typeof(int), typeof(int) }, modifiers: null));
            Assert.AreEqual(substring, p.GetMethod(typeof(string), nameof(string.Substring), genericParameterCount: 0, BindingFlags.Public | BindingFlags.Instance, binder: null, CallingConventions.Any, new[] { typeof(int), typeof(int) }, modifiers: null));
#endif

            var isNullOrEmpty = p.GetMethod(typeof(string), nameof(string.IsNullOrEmpty));
            Assert.AreEqual(true, isNullOrEmpty.Invoke(null, new object[] { "" }));
            Assert.AreEqual(false, isNullOrEmpty.Invoke(null, new object[] { "bar" }));

            Assert.AreEqual(isNullOrEmpty, p.GetMethod(typeof(string), nameof(string.IsNullOrEmpty), BindingFlags.Public | BindingFlags.Static));

            var ctor = p.GetConstructor(typeof(string), new[] { typeof(char), typeof(int) });
            Assert.AreEqual("***", ctor.Invoke(new object[] { '*', 3 }));

            Assert.AreEqual(ctor, p.GetConstructor(typeof(string), BindingFlags.Public | BindingFlags.Instance, binder: null, new[] { typeof(char), typeof(int) }, modifiers: null));
            Assert.AreEqual(ctor, p.GetConstructor(typeof(string), BindingFlags.Public | BindingFlags.Instance, binder: null, CallingConventions.Any, new[] { typeof(char), typeof(int) }, modifiers: null));

            var value = p.GetField(typeof(StrongBox<int>), nameof(StrongBox<int>.Value));
            var box = new StrongBox<int>();
            value.SetValue(box, 42);
            Assert.AreEqual(42, box.Value);

            Assert.AreEqual(value, p.GetField(typeof(StrongBox<int>), nameof(StrongBox<int>.Value), BindingFlags.Public | BindingFlags.Instance));

            var progressChanged = p.GetEvent(typeof(Progress<int>), nameof(Progress<int>.ProgressChanged));
            var progress = new MyProgress();
            var e = new ManualResetEvent(initialState: false);
            var val = -1;
            progressChanged.AddEventHandler(progress, new EventHandler<int>((o, x) =>
            {
                val = x;
                e.Set();
            }));
            progress.Report(42);
            e.WaitOne();
            Assert.AreEqual(42, val);

            Assert.AreEqual(progressChanged, p.GetEvent(typeof(Progress<int>), nameof(Progress<int>.ProgressChanged), BindingFlags.Public | BindingFlags.Instance));

            var a = new { bar = 42 };
            var bar = p.GetMember(a.GetType(), nameof(a.bar)).Single();
            Assert.AreEqual(MemberTypes.Property, bar.MemberType);
            Assert.AreEqual(a.GetType().GetMember(nameof(a.bar)).Single(), bar);
            Assert.AreEqual(bar, p.GetMember(a.GetType(), nameof(a.bar), BindingFlags.Public | BindingFlags.Instance).Single());
            Assert.AreEqual(bar, p.GetMember(a.GetType(), nameof(a.bar), MemberTypes.Property, BindingFlags.Public | BindingFlags.Instance).Single());

            Assert.IsNotNull(p.TypeInitializer(typeof(C)));

            var n = p.GetNestedType(typeof(C), "N", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            Assert.IsNotNull(n);

            var enumerator = p.GetNestedType(typeof(List<int>), nameof(List<int>.Enumerator));
            Assert.AreEqual(typeof(List<int>).GetNestedType(nameof(List<int>.Enumerator)), enumerator);

            Assert.AreEqual(typeof(string).GetInterface(nameof(IConvertible)), p.GetInterface(typeof(string), nameof(IConvertible)));
            Assert.AreEqual(typeof(string).GetInterface(nameof(IConvertible).ToLower(), ignoreCase: true), p.GetInterface(typeof(string), nameof(IConvertible).ToLower(), ignoreCase: true));
        });

        [TestMethod]
        public void Type_CreateInstance() => WithProviders(p =>
        {
            var ex1 = p.CreateInstance(typeof(Exception).Assembly, typeof(Exception).FullName);
            Assert.IsNotNull(ex1);

            var ex2 = p.CreateInstance(typeof(Exception).Assembly, typeof(Exception).FullName.ToLower(), ignoreCase: true);
            Assert.IsNotNull(ex2);

            var ex3 = (Exception)p.CreateInstance(typeof(Exception).Assembly, typeof(Exception).FullName, ignoreCase: true, BindingFlags.Public | BindingFlags.Instance, binder: null, new object[] { "Oops!" }, culture: null, activationAttributes: null);
            Assert.IsNotNull(ex3);
            Assert.AreEqual("Oops!", ex3.Message);
        });

        private sealed class MyProgress : Progress<int>
        {
            public void Report(int value) => OnReport(value);
        }

        [TestMethod]
        public void Methods_Properties() => WithProviders(p =>
        {
            foreach (var m in new[]
            {
                typeof(string).GetMethod(nameof(string.Substring), new[] { typeof(int) }),
                typeof(string).GetMethod(nameof(string.Equals), new[] { typeof(object) }),
                typeof(C).GetMethod(nameof(C.F), new[] { typeof(int) }),
            })
            {
                Assert.AreEqual(m.Attributes, p.GetAttributes(m));
                Assert.AreEqual(m.CallingConvention, p.GetCallingConvention(m));
                Assert.AreEqual(m.ContainsGenericParameters, p.ContainsGenericParameters(m));
                Assert.AreEqual(m.DeclaringType, p.GetDeclaringType(m));
                Assert.AreEqual(m.IsAbstract, p.IsAbstract(m));
                Assert.AreEqual(m.IsAssembly, p.IsAssembly(m));
                Assert.AreEqual(m.IsConstructor, p.IsConstructor(m));
                Assert.AreEqual(m.IsFamily, p.IsFamily(m));
                Assert.AreEqual(m.IsFamilyAndAssembly, p.IsFamilyAndAssembly(m));
                Assert.AreEqual(m.IsFamilyOrAssembly, p.IsFamilyOrAssembly(m));
                Assert.AreEqual(m.IsFinal, p.IsFinal(m));
                Assert.AreEqual(m.IsGenericMethod, p.IsGenericMethod(m));
                Assert.AreEqual(m.IsGenericMethodDefinition, p.IsGenericMethodDefinition(m));
                Assert.AreEqual(m.IsHideBySig, p.IsHideBySig(m));
                Assert.AreEqual(m.IsPrivate, p.IsPrivate(m));
                Assert.AreEqual(m.IsPublic, p.IsPublic(m));
                Assert.AreEqual(m.IsSecurityCritical, p.IsSecurityCritical(m));
                Assert.AreEqual(m.IsSecuritySafeCritical, p.IsSecuritySafeCritical(m));
                Assert.AreEqual(m.IsSecurityTransparent, p.IsSecurityTransparent(m));
                Assert.AreEqual(m.IsSpecialName, p.IsSpecialName(m));
                Assert.AreEqual(m.IsStatic, p.IsStatic(m));
                Assert.AreEqual(m.IsVirtual, p.IsVirtual(m));
                Assert.AreEqual(m.MethodHandle, p.GetMethodHandle(m));
                Assert.AreEqual(m.MethodImplementationFlags, p.GetMethodImplementationFlags(m));
                Assert.AreEqual(m.Module, p.GetModule(m));
                Assert.AreEqual(m.Name, p.GetName(m));
                Assert.AreEqual(m.ReturnType, p.GetReturnType(m));

                if (Type.GetType("Mono.Runtime") == null) // NB: Another quirk where this doesn't return instances that are deemed equal.
                {
                    Assert.AreEqual(m.ReturnParameter, p.GetReturnParameter(m));
                }

                Assert.AreEqual(m.GetBaseDefinition(), p.GetBaseDefinition(m));

                CollectionAssert.AreEqual(m.GetParameters(), p.GetParameters(m).ToArray());

                foreach (var par in m.GetParameters())
                {
                    Assert.AreEqual(par.Attributes, p.GetAttributes(par));
                    Assert.AreEqual(par.HasDefaultValue, p.HasDefaultValue(par));
                    Assert.AreEqual(par.IsIn, p.IsIn(par));
                    Assert.AreEqual(par.IsLcid, p.IsLcid(par));
                    Assert.AreEqual(par.IsOptional, p.IsOptional(par));
                    Assert.AreEqual(par.IsOut, p.IsOut(par));
                    Assert.AreEqual(par.IsRetval, p.IsRetval(par));
                    Assert.AreEqual(par.Member, p.GetMember(par));
                    Assert.AreEqual(par.MetadataToken, p.GetMetadataToken(par));
                    Assert.AreEqual(par.Name, p.GetName(par));
                    Assert.AreEqual(par.ParameterType, p.GetParameterType(par));
                    Assert.AreEqual(par.Position, p.GetPosition(par));

                    if (par.HasDefaultValue)
                    {
                        Assert.AreEqual(par.DefaultValue, p.GetDefaultValue(par));
                        Assert.AreEqual(par.RawDefaultValue, p.GetRawDefaultValue(par));
                    }
                }
            }
        });

        [TestMethod]
        public void Methods_Invoke() => WithProviders(p =>
        {
            var substring = typeof(string).GetMethod(nameof(string.Substring), new[] { typeof(int), typeof(int) });

            var str = "foobarqux";
            var args = new object[] { 3, 4 };
            var exp = substring.Invoke(str, args);

            Assert.AreEqual(exp, p.Invoke(substring, str, args));
            Assert.AreEqual(exp, p.InvokeMember(typeof(string), nameof(string.Substring), BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, binder: null, str, args));
            Assert.AreEqual(exp, p.InvokeMember(typeof(string), nameof(string.Substring), BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, binder: null, str, args, culture: null));
            Assert.AreEqual(exp, p.InvokeMember(typeof(string), nameof(string.Substring), BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, binder: null, str, args, modifiers: null, culture: null, namedParameters: null));
        });

        [TestMethod]
        public void Type_FindMethods() => WithProviders(p =>
        {
            CollectionAssert.AreEquivalent(
                typeof(string).FindInterfaces(new TypeFilter((t, _) => true), filterCriteria: null),
                p.FindInterfaces(typeof(string), new TypeFilter((t, _) => true), filterCriteria: null).ToArray()
            );

            CollectionAssert.AreEquivalent(
                typeof(string).FindMembers(MemberTypes.Method, BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance, new MemberFilter((m, _) => true), filterCriteria: null),
                p.FindMembers(typeof(string), MemberTypes.Method, BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance, new MemberFilter((m, _) => true), filterCriteria: null).ToArray()
            );
        });

        [TestMethod]
        public void Module_FindTypes() => WithProviders(p =>
        {
#pragma warning disable IDE0079 // The following supression is flagged as unnecessary on .NET Framework (but is required for other targets)
#pragma warning disable CA1847  // Use 'string.Contains(char)' instead of 'string.Contains(string)' - unavailable on .NET Framework
            CollectionAssert.AreEquivalent(
                typeof(string).Module.FindTypes(new TypeFilter((t, _) => t.Name.Contains("q")), filterCriteria: null),
                p.FindTypes(typeof(string).Module, new TypeFilter((t, _) => t.Name.Contains("q")), filterCriteria: null).ToArray()
#pragma warning restore CA1847
#pragma warning restore IDE0079
            );
        });

        [TestMethod]
        public void Methods_Generics() => WithProviders(p =>
        {
            var m1 = typeof(C).GetMethod(nameof(C.G));
            var m2 = typeof(C).GetMethod(nameof(C.H));

            Assert.AreEqual(m1.MakeGenericMethod(new[] { typeof(int) }), p.MakeGenericMethod(m1, new[] { typeof(int) }));

            Assert.AreEqual(m2.MakeGenericMethod(new[] { typeof(int), typeof(bool) }), p.MakeGenericMethod(m2, new[] { typeof(int), typeof(bool) }));

            Assert.AreEqual(m1.MakeGenericMethod(new[] { typeof(int) }), p.MakeGenericMethod(m1, new[] { typeof(int) }));
            Assert.AreEqual(m1.MakeGenericMethod(new[] { typeof(bool) }), p.MakeGenericMethod(m1, new[] { typeof(bool) }));

            Assert.AreEqual(m2.MakeGenericMethod(new[] { typeof(string), typeof(bool) }), p.MakeGenericMethod(m2, new[] { typeof(string), typeof(bool) }));
            Assert.AreEqual(m2.MakeGenericMethod(new[] { typeof(int), typeof(bool) }), p.MakeGenericMethod(m2, new[] { typeof(int), typeof(bool) }));

            var m = m2.MakeGenericMethod(new[] { typeof(int), typeof(bool) });

            Assert.AreEqual(m.GetGenericMethodDefinition(), p.GetGenericMethodDefinition(m));
            CollectionAssert.AreEqual(m.GetGenericArguments(), p.GetGenericArguments(m).ToArray());

            var arg1 = m1.GetGenericArguments()[0];
            Assert.AreEqual(arg1.DeclaringMethod, p.GetDeclaringMethod(arg1));
            Assert.AreEqual(arg1.GenericParameterAttributes, p.GetGenericParameterAttributes(arg1));
            Assert.AreEqual(arg1.GenericParameterPosition, p.GetGenericParameterPosition(arg1));
        });

        [TestMethod]
        public void Properties_Properties() => WithProviders(p =>
        {
            foreach (var prop in new[]
            {
                typeof(string).GetProperty(nameof(string.Length)),
                typeof(string).GetProperties().Single(x => x.PropertyType == typeof(char) && x.GetIndexParameters().Select(y => y.ParameterType).SequenceEqual(new[] { typeof(int) })),
                typeof(Console).GetProperty(nameof(Console.ForegroundColor)),
            })
            {
                Assert.AreEqual(prop.Attributes, p.GetAttributes(prop));
                Assert.AreEqual(prop.CanRead, p.CanRead(prop));
                Assert.AreEqual(prop.CanWrite, p.CanWrite(prop));
                Assert.AreEqual(prop.DeclaringType, p.GetDeclaringType(prop));
                Assert.AreEqual(prop.IsSpecialName, p.IsSpecialName(prop));
                Assert.AreEqual(prop.Name, p.GetName(prop));
                Assert.AreEqual(prop.PropertyType, p.GetPropertyType(prop));

                Assert.AreEqual(prop.GetGetMethod(), p.GetGetMethod(prop));
                Assert.AreEqual(prop.GetGetMethod(nonPublic: true), p.GetGetMethod(prop, nonPublic: true));
                Assert.AreEqual(prop.GetSetMethod(), p.GetSetMethod(prop));
                Assert.AreEqual(prop.GetSetMethod(nonPublic: true), p.GetSetMethod(prop, nonPublic: true));

                CollectionAssert.AreEqual(prop.GetAccessors(), p.GetAccessors(prop).ToArray());

                if (Type.GetType("Mono.Runtime") == null) // NB: Quirk on Mono with indexers.
                {
                    CollectionAssert.AreEqual(prop.GetIndexParameters(), p.GetIndexParameters(prop).ToArray());
                }
            }
        });

        [TestMethod]
        public void Properties_GetValueSetValue() => WithProviders(p =>
        {
            {
                var length = typeof(string).GetProperty(nameof(string.Length));
                var index = typeof(string).GetProperties().Single(x => x.PropertyType == typeof(char) && x.GetIndexParameters().Select(y => y.ParameterType).SequenceEqual(new[] { typeof(int) }));

                Assert.AreEqual(length.GetValue("bar"), p.GetValue(length, "bar"));
                Assert.AreEqual(index.GetValue("bar", new object[] { 1 }), p.GetValue(index, "bar", new object[] { 1 }));
            }

            {
                var item = typeof(List<int>).GetProperties().Single(x => x.PropertyType == typeof(int) && x.GetIndexParameters().Select(y => y.ParameterType).SequenceEqual(new[] { typeof(int) }));

                var xs = new List<int> { 1, 2, 3 };

                p.SetValue(item, xs, 42, new object[] { 1 });

                Assert.AreEqual(42, xs[1]);
            }

            {
                var prop = typeof(C).GetProperty(nameof(C.P));

                var c = new C();

                p.SetValue(prop, c, 42);

                Assert.AreEqual(42, c.P);
            }
        });

        [TestMethod]
        public void Field_Properties() => WithProviders(p =>
        {
            foreach (var f in new[]
            {
                typeof(C).GetField("m_x", BindingFlags.NonPublic | BindingFlags.Instance),
                typeof(C).GetField("m_y", BindingFlags.NonPublic | BindingFlags.Instance),
                typeof(C).GetField("s_x", BindingFlags.NonPublic | BindingFlags.Static),
                typeof(C).GetField("s_y", BindingFlags.NonPublic | BindingFlags.Static),
                typeof(C).GetField("A", BindingFlags.NonPublic | BindingFlags.Static),
                typeof(C).GetField("X", BindingFlags.Public | BindingFlags.Instance),
                typeof(C).GetField("Y", BindingFlags.Public | BindingFlags.Static),
            })
            {
                Assert.AreEqual(f.Attributes, p.GetAttributes(f));
                Assert.AreEqual(f.DeclaringType, p.GetDeclaringType(f));
                Assert.AreEqual(f.FieldType, p.GetFieldType(f));
                Assert.AreEqual(f.IsAssembly, p.IsAssembly(f));
                Assert.AreEqual(f.IsFamily, p.IsFamily(f));
                Assert.AreEqual(f.IsFamilyAndAssembly, p.IsFamilyAndAssembly(f));
                Assert.AreEqual(f.IsFamilyOrAssembly, p.IsFamilyOrAssembly(f));
                Assert.AreEqual(f.IsInitOnly, p.IsInitOnly(f));
                Assert.AreEqual(f.IsLiteral, p.IsLiteral(f));
                Assert.AreEqual(f.IsNotSerialized, p.IsNotSerialized(f));
                Assert.AreEqual(f.IsPinvokeImpl, p.IsPinvokeImpl(f));
                Assert.AreEqual(f.IsPrivate, p.IsPrivate(f));
                Assert.AreEqual(f.IsPublic, p.IsPublic(f));
                Assert.AreEqual(f.IsSecurityCritical, p.IsSecurityCritical(f));
                Assert.AreEqual(f.IsSecuritySafeCritical, p.IsSecuritySafeCritical(f));
                Assert.AreEqual(f.IsSecurityTransparent, p.IsSecurityTransparent(f));
                Assert.AreEqual(f.IsSpecialName, p.IsSpecialName(f));
                Assert.AreEqual(f.IsStatic, p.IsStatic(f));
                Assert.AreEqual(f.MetadataToken, p.GetMetadataToken(f));
                Assert.AreEqual(f.Name, p.GetName(f));
            }
        });

        [TestMethod]
        public void Field_GetSetValue() => WithProviders(p =>
        {
            foreach (var f in new[]
            {
                typeof(C).GetField("m_x", BindingFlags.NonPublic | BindingFlags.Instance),
                typeof(C).GetField("m_y", BindingFlags.NonPublic | BindingFlags.Instance),
                typeof(C).GetField("s_x", BindingFlags.NonPublic | BindingFlags.Static),
                typeof(C).GetField("s_y", BindingFlags.NonPublic | BindingFlags.Static),
                typeof(C).GetField("A", BindingFlags.NonPublic | BindingFlags.Static),
                typeof(C).GetField("X", BindingFlags.Public | BindingFlags.Instance),
                typeof(C).GetField("Y", BindingFlags.Public | BindingFlags.Static),
            })
            {
                if (f.IsStatic)
                {
                    Assert.AreEqual(f.GetValue(obj: null), p.GetValue(f, obj: null));

                    if (!f.IsInitOnly && !f.IsLiteral)
                    {
                        p.SetValue(f, obj: null, 43);
                        Assert.AreEqual(f.GetValue(obj: null), p.GetValue(f, obj: null));
                    }

                    if (f.IsLiteral)
                    {
                        Assert.AreEqual(f.GetRawConstantValue(), p.GetRawConstantValue(f));
                    }
                }
                else
                {
                    var c = new C();

                    Assert.AreEqual(f.GetValue(obj: c), p.GetValue(f, obj: c));

                    if (!f.IsInitOnly)
                    {
                        p.SetValue(f, obj: c, 43);
                        Assert.AreEqual(f.GetValue(obj: c), p.GetValue(f, obj: c));
                    }
                }
            }
        });

        [TestMethod]
        public void Events_Properties() => WithProviders(p =>
        {
            foreach (var e in new[]
            {
                typeof(Progress<int>).GetEvent(nameof(Progress<int>.ProgressChanged)),
            })
            {
                Assert.AreEqual(e.Attributes, p.GetAttributes(e));
                Assert.AreEqual(e.EventHandlerType, p.GetEventHandlerType(e));
                Assert.AreEqual(e.IsMulticast, p.IsMulticast(e));
                Assert.AreEqual(e.IsSpecialName, p.IsSpecialName(e));
                Assert.AreEqual(e.Name, p.GetName(e));

                Assert.AreEqual(e.GetAddMethod(), p.GetAddMethod(e));
                Assert.AreEqual(e.GetAddMethod(nonPublic: true), p.GetAddMethod(e, nonPublic: true));
                Assert.AreEqual(e.GetRemoveMethod(), p.GetRemoveMethod(e));
                Assert.AreEqual(e.GetRemoveMethod(nonPublic: true), p.GetRemoveMethod(e, nonPublic: true));
                Assert.AreEqual(e.GetRaiseMethod(), p.GetRaiseMethod(e));
                Assert.AreEqual(e.GetRaiseMethod(nonPublic: true), p.GetRaiseMethod(e, nonPublic: true));

                if (Type.GetType("Mono.Runtime") == null) // NB: NullReferenceException in `System.Reflection.RuntimeEventInfo.GetOtherMethods` on Mono.
                {
                    CollectionAssert.AreEqual(e.GetOtherMethods(), p.GetOtherMethods(e).ToArray());
                    CollectionAssert.AreEqual(e.GetOtherMethods(nonPublic: true), p.GetOtherMethods(e, nonPublic: true).ToArray());
                }
            }
        });

        [TestMethod]
        public void Events_Handlers() => WithProviders(p =>
        {
            var evt = p.GetEvent(typeof(C), nameof(C.E));

            var h1Count = 0;
            var h2Count = 0;

            var h1 = new Action(() =>
            {
                h1Count++;
            });

            var h2 = new Action(() =>
            {
                h2Count++;
            });

            var c = new C();

            c.Raise();

            Assert.AreEqual(0, h1Count);
            Assert.AreEqual(0, h2Count);

            p.AddEventHandler(evt, c, h1);

            c.Raise();

            Assert.AreEqual(1, h1Count);
            Assert.AreEqual(0, h2Count);

            p.AddEventHandler(evt, c, h2);

            c.Raise();

            Assert.AreEqual(2, h1Count);
            Assert.AreEqual(1, h2Count);

            p.RemoveEventHandler(evt, c, h1);

            c.Raise();

            Assert.AreEqual(2, h1Count);
            Assert.AreEqual(2, h2Count);

            p.RemoveEventHandler(evt, c, h2);

            c.Raise();

            Assert.AreEqual(2, h1Count);
            Assert.AreEqual(2, h2Count);
        });

        [TestMethod]
        public void Constructors_Properties() => WithProviders(p =>
        {
            foreach (var c in new[]
            {
                typeof(string).GetConstructor(new[] { typeof(char), typeof(int) }),
            })
            {
                Assert.AreEqual(c.Attributes, p.GetAttributes(c));
                Assert.AreEqual(c.IsConstructor, p.IsConstructor(c));
                Assert.AreEqual(c.IsSpecialName, p.IsSpecialName(c));
                Assert.AreEqual(c.Name, p.GetName(c));

                // NB: Most is shared with MethodBase, so just spot-checking a few things here.
            }
        });

        [TestMethod]
        public void Constructors_Invoke() => WithProviders(p =>
        {
            var ctor = typeof(string).GetConstructor(new[] { typeof(char), typeof(int) });

            Assert.AreEqual("***", p.Invoke(ctor, new object[] { '*', 3 }));
            Assert.AreEqual("***", p.Invoke(ctor, BindingFlags.Default, binder: null, new object[] { '*', 3 }, culture: null));
        });

        [TestMethod]
        public void CustomAttributes_Assembly() => WithProviders(p =>
        {
            {
                var attrs1 = typeof(string).Assembly.GetCustomAttributes().Select(a => a.GetType().FullName).ToArray();
                var attrs2 = p.GetCustomAttributes(typeof(string).Assembly).Select(a => a.GetType().FullName).ToArray();

                CollectionAssert.AreEquivalent(attrs1, attrs2);
            }

            {
                var attrs1 = typeof(string).Assembly.GetCustomAttributes(inherit: true).Select(a => a.GetType().FullName).ToArray();
                var attrs2 = p.GetCustomAttributes(typeof(string).Assembly, inherit: true).Select(a => a.GetType().FullName).ToArray();

                CollectionAssert.AreEquivalent(attrs1, attrs2);
            }

            {
                var attrs1 = typeof(string).Assembly.GetCustomAttributes<AssemblyTitleAttribute>().Select(a => a.Title).ToArray();
                var attrs2 = p.GetCustomAttributes<AssemblyTitleAttribute>(typeof(string).Assembly).Select(a => a.Title).ToArray();

                CollectionAssert.AreEquivalent(attrs1, attrs2);
            }

            {
                var attrs1 = typeof(string).Assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute)).Cast<AssemblyTitleAttribute>().Select(a => a.Title).ToArray();
                var attrs2 = p.GetCustomAttributes(typeof(string).Assembly, typeof(AssemblyTitleAttribute)).Cast<AssemblyTitleAttribute>().Select(a => a.Title).ToArray();

                CollectionAssert.AreEquivalent(attrs1, attrs2);
            }

            {
                var attrs1 = typeof(string).Assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), inherit: true).Cast<AssemblyTitleAttribute>().Select(a => a.Title).ToArray();
                var attrs2 = p.GetCustomAttributes(typeof(string).Assembly, typeof(AssemblyTitleAttribute), inherit: true).Cast<AssemblyTitleAttribute>().Select(a => a.Title).ToArray();
                var attrs3 = CustomAttributeProviderExtensions.GetCustomAttributes(p, typeof(string).Assembly, typeof(AssemblyTitleAttribute)).Cast<AssemblyTitleAttribute>().Select(a => a.Title).ToArray();

                CollectionAssert.AreEquivalent(attrs1, attrs2);
                CollectionAssert.AreEquivalent(attrs1, attrs3);
            }

            {
                var attr1 = typeof(string).Assembly.GetCustomAttribute<AssemblyTitleAttribute>().Title;
                var attr2 = p.GetCustomAttribute<AssemblyTitleAttribute>(typeof(string).Assembly).Title;

                Assert.AreEqual(attr1, attr2);
            }

            {
                var attr1 = ((AssemblyTitleAttribute)typeof(string).Assembly.GetCustomAttribute(typeof(AssemblyTitleAttribute))).Title;
                var attr2 = ((AssemblyTitleAttribute)p.GetCustomAttribute(typeof(string).Assembly, typeof(AssemblyTitleAttribute))).Title;

                Assert.AreEqual(attr1, attr2);
            }

            {
                Assert.AreEqual(
                    typeof(string).Assembly.IsDefined(typeof(AssemblyTitleAttribute)),
                    p.IsDefined(typeof(string).Assembly, typeof(AssemblyTitleAttribute))
                );
            }

            {
                Assert.AreEqual(
                    typeof(string).Assembly.IsDefined(typeof(AssemblyTitleAttribute), inherit: true),
                    p.IsDefined(typeof(string).Assembly, typeof(AssemblyTitleAttribute), inherit: true)
                );
            }
        });

        [TestMethod]
        public void CustomAttributes_Type() => WithProviders(p =>
        {
            {
                var attrs1 = typeof(string).GetCustomAttributes().Select(a => a.GetType().FullName).ToArray();
                var attrs2 = p.GetCustomAttributes(typeof(string)).Select(a => a.GetType().FullName).ToArray();

                CollectionAssert.AreEquivalent(attrs1, attrs2);
            }

            {
                var attrs1 = typeof(string).GetCustomAttributes(inherit: true).Select(a => a.GetType().FullName).ToArray();
                var attrs2 = p.GetCustomAttributes(typeof(string), inherit: true).Select(a => a.GetType().FullName).ToArray();
                var attrs3 = CustomAttributeProviderExtensions.GetCustomAttributes(p, typeof(string), inherit: true).Select(a => a.GetType().FullName).ToArray();

                CollectionAssert.AreEquivalent(attrs1, attrs2);
                CollectionAssert.AreEquivalent(attrs1, attrs3);
            }

            {
                var attrs1 = typeof(string).GetCustomAttributes<SerializableAttribute>().Select(a => a.ToString()).ToArray();
                var attrs2 = p.GetCustomAttributes<SerializableAttribute>(typeof(string)).Select(a => a.ToString()).ToArray();

                CollectionAssert.AreEquivalent(attrs1, attrs2);
            }

            {
                var attrs1 = typeof(string).GetCustomAttributes<SerializableAttribute>(inherit: true).Select(a => a.ToString()).ToArray();
                var attrs2 = p.GetCustomAttributes<SerializableAttribute>(typeof(string), inherit: true).Select(a => a.ToString()).ToArray();

                CollectionAssert.AreEquivalent(attrs1, attrs2);
            }

            {
                var attrs1 = typeof(string).GetCustomAttributes(typeof(SerializableAttribute)).Cast<SerializableAttribute>().Select(a => a.ToString()).ToArray();
                var attrs2 = p.GetCustomAttributes(typeof(string), typeof(SerializableAttribute)).Cast<SerializableAttribute>().Select(a => a.ToString()).ToArray();

                CollectionAssert.AreEquivalent(attrs1, attrs2);
            }

            {
                var attrs1 = typeof(string).GetCustomAttributes(typeof(SerializableAttribute), inherit: true).Cast<SerializableAttribute>().Select(a => a.ToString()).ToArray();
                var attrs2 = p.GetCustomAttributes(typeof(string), typeof(SerializableAttribute), inherit: true).Cast<SerializableAttribute>().Select(a => a.ToString()).ToArray();
                var attrs3 = CustomAttributeProviderExtensions.GetCustomAttributes(p, typeof(string), typeof(SerializableAttribute), inherit: true).Cast<SerializableAttribute>().Select(a => a.ToString()).ToArray();

                CollectionAssert.AreEquivalent(attrs1, attrs2);
                CollectionAssert.AreEquivalent(attrs1, attrs3);
            }

            {
                var attr1 = typeof(string).GetCustomAttribute<SerializableAttribute>().ToString();
                var attr2 = p.GetCustomAttribute<SerializableAttribute>(typeof(string)).ToString();

                Assert.AreEqual(attr1, attr2);
            }

            {
                var attr1 = ((SerializableAttribute)typeof(string).GetCustomAttribute(typeof(SerializableAttribute))).ToString();
                var attr2 = ((SerializableAttribute)p.GetCustomAttribute(typeof(string), typeof(SerializableAttribute))).ToString();

                Assert.AreEqual(attr1, attr2);
            }

            {
                Assert.AreEqual(
                    typeof(string).IsDefined(typeof(SerializableAttribute)),
                    p.IsDefined(typeof(string), typeof(SerializableAttribute))
                );
            }

            {
                Assert.AreEqual(
                    typeof(string).IsDefined(typeof(SerializableAttribute), inherit: true),
                    p.IsDefined(typeof(string), typeof(SerializableAttribute), inherit: true)
                );
            }
        });

        [TestMethod]
        public void CustomAttributes_Method() => WithProviders(p =>
        {
            var m = ((MethodCallExpression)((Expression<Func<IEnumerable<int>, int>>)(xs => xs.Count())).Body).Method;

            {
                var attrs1 = m.GetCustomAttributes().Select(a => a.GetType().FullName).ToArray();
                var attrs2 = p.GetCustomAttributes(m).Select(a => a.GetType().FullName).ToArray();

                CollectionAssert.AreEquivalent(attrs1, attrs2);
            }

            {
                var attrs1 = m.GetCustomAttributes(inherit: true).Select(a => a.GetType().FullName).ToArray();
                var attrs2 = p.GetCustomAttributes(m, inherit: true).Select(a => a.GetType().FullName).ToArray();
                var attrs3 = CustomAttributeProviderExtensions.GetCustomAttributes(p, m, inherit: true).Select(a => a.GetType().FullName).ToArray();

                CollectionAssert.AreEquivalent(attrs1, attrs2);
                CollectionAssert.AreEquivalent(attrs1, attrs3);
            }

            {
                var attrs1 = m.GetCustomAttributes<ExtensionAttribute>().Select(a => a.ToString()).ToArray();
                var attrs2 = p.GetCustomAttributes<ExtensionAttribute>(m).Select(a => a.ToString()).ToArray();

                CollectionAssert.AreEquivalent(attrs1, attrs2);
            }

            {
                var attrs1 = m.GetCustomAttributes<ExtensionAttribute>(inherit: true).Select(a => a.ToString()).ToArray();
                var attrs2 = p.GetCustomAttributes<ExtensionAttribute>(m, inherit: true).Select(a => a.ToString()).ToArray();

                CollectionAssert.AreEquivalent(attrs1, attrs2);
            }

            {
                var attrs1 = m.GetCustomAttributes(typeof(ExtensionAttribute)).Cast<ExtensionAttribute>().Select(a => a.ToString()).ToArray();
                var attrs2 = p.GetCustomAttributes(m, typeof(ExtensionAttribute)).Cast<ExtensionAttribute>().Select(a => a.ToString()).ToArray();

                CollectionAssert.AreEquivalent(attrs1, attrs2);
            }

            {
                var attrs1 = m.GetCustomAttributes(typeof(ExtensionAttribute), inherit: true).Cast<ExtensionAttribute>().Select(a => a.ToString()).ToArray();
                var attrs2 = p.GetCustomAttributes(m, typeof(ExtensionAttribute), inherit: true).Cast<ExtensionAttribute>().Select(a => a.ToString()).ToArray();
                var attrs3 = CustomAttributeProviderExtensions.GetCustomAttributes(p, m, typeof(ExtensionAttribute), inherit: true).Cast<ExtensionAttribute>().Select(a => a.ToString()).ToArray();

                CollectionAssert.AreEquivalent(attrs1, attrs2);
                CollectionAssert.AreEquivalent(attrs1, attrs3);
            }

            {
                var attr1 = m.GetCustomAttribute<ExtensionAttribute>().ToString();
                var attr2 = p.GetCustomAttribute<ExtensionAttribute>(m).ToString();

                Assert.AreEqual(attr1, attr2);
            }

            {
                var attr1 = m.GetCustomAttribute<ExtensionAttribute>(inherit: true).ToString();
                var attr2 = p.GetCustomAttribute<ExtensionAttribute>(m, inherit: true).ToString();

                Assert.AreEqual(attr1, attr2);
            }

            {
                var attr1 = ((ExtensionAttribute)m.GetCustomAttribute(typeof(ExtensionAttribute))).ToString();
                var attr2 = ((ExtensionAttribute)p.GetCustomAttribute(m, typeof(ExtensionAttribute))).ToString();

                Assert.AreEqual(attr1, attr2);
            }

            {
                var attr1 = ((ExtensionAttribute)m.GetCustomAttribute(typeof(ExtensionAttribute), inherit: true)).ToString();
                var attr2 = ((ExtensionAttribute)p.GetCustomAttribute(m, typeof(ExtensionAttribute), inherit: true)).ToString();

                Assert.AreEqual(attr1, attr2);
            }

            {
                Assert.AreEqual(
                    m.IsDefined(typeof(ExtensionAttribute)),
                    p.IsDefined(m, typeof(ExtensionAttribute))
                );
            }

            {
                Assert.AreEqual(
                    m.IsDefined(typeof(ExtensionAttribute), inherit: true),
                    p.IsDefined(m, typeof(ExtensionAttribute), inherit: true)
                );
            }
        });

        [TestMethod]
        public void CustomAttributes_Parameter() => WithProviders(p =>
        {
            var par = typeof(C).GetMethod(nameof(C.Bar)).GetParameters()[0];

            {
                var attrs1 = par.GetCustomAttributes().Select(a => a.GetType().FullName).ToArray();
                var attrs2 = p.GetCustomAttributes(par).Select(a => a.GetType().FullName).ToArray();

                CollectionAssert.AreEquivalent(attrs1, attrs2);
            }

            {
                var attrs1 = par.GetCustomAttributes(inherit: true).Select(a => a.GetType().FullName).ToArray();
                var attrs2 = p.GetCustomAttributes(par, inherit: true).Select(a => a.GetType().FullName).ToArray();
                var attrs3 = CustomAttributeProviderExtensions.GetCustomAttributes(p, par, inherit: true).Select(a => a.GetType().FullName).ToArray();

                CollectionAssert.AreEquivalent(attrs1, attrs2);
                CollectionAssert.AreEquivalent(attrs1, attrs3);
            }

            {
                var attrs1 = par.GetCustomAttributes<MyAttribute>().Select(a => a.Value).ToArray();
                var attrs2 = p.GetCustomAttributes<MyAttribute>(par).Select(a => a.Value).ToArray();

                CollectionAssert.AreEquivalent(attrs1, attrs2);
            }

            {
                var attrs1 = par.GetCustomAttributes<MyAttribute>(inherit: true).Select(a => a.Value).ToArray();
                var attrs2 = p.GetCustomAttributes<MyAttribute>(par, inherit: true).Select(a => a.Value).ToArray();

                CollectionAssert.AreEquivalent(attrs1, attrs2);
            }

            {
                var attrs1 = par.GetCustomAttributes(typeof(MyAttribute)).Cast<MyAttribute>().Select(a => a.Value).ToArray();
                var attrs2 = p.GetCustomAttributes(par, typeof(MyAttribute)).Cast<MyAttribute>().Select(a => a.Value).ToArray();

                CollectionAssert.AreEquivalent(attrs1, attrs2);
            }

            {
                var attrs1 = par.GetCustomAttributes(typeof(MyAttribute), inherit: true).Cast<MyAttribute>().Select(a => a.Value).ToArray();
                var attrs2 = p.GetCustomAttributes(par, typeof(MyAttribute), inherit: true).Cast<MyAttribute>().Select(a => a.Value).ToArray();
                var attrs3 = CustomAttributeProviderExtensions.GetCustomAttributes(p, par, typeof(MyAttribute), inherit: true).Cast<MyAttribute>().Select(a => a.Value).ToArray();

                CollectionAssert.AreEquivalent(attrs1, attrs2);
                CollectionAssert.AreEquivalent(attrs1, attrs3);
            }

            {
                var attr1 = par.GetCustomAttribute<MyAttribute>().ToString();
                var attr2 = p.GetCustomAttribute<MyAttribute>(par).ToString();

                Assert.AreEqual(attr1, attr2);
            }

            {
                var attr1 = par.GetCustomAttribute<MyAttribute>(inherit: true).ToString();
                var attr2 = p.GetCustomAttribute<MyAttribute>(par, inherit: true).ToString();

                Assert.AreEqual(attr1, attr2);
            }

            {
                var attr1 = ((MyAttribute)par.GetCustomAttribute(typeof(MyAttribute))).ToString();
                var attr2 = ((MyAttribute)p.GetCustomAttribute(par, typeof(MyAttribute))).ToString();

                Assert.AreEqual(attr1, attr2);
            }

            {
                var attr1 = ((MyAttribute)par.GetCustomAttribute(typeof(MyAttribute), inherit: true)).ToString();
                var attr2 = ((MyAttribute)p.GetCustomAttribute(par, typeof(MyAttribute), inherit: true)).ToString();

                Assert.AreEqual(attr1, attr2);
            }

            {
                Assert.AreEqual(
                    par.IsDefined(typeof(MyAttribute)),
                    p.IsDefined(par, typeof(MyAttribute))
                );
            }

            {
                Assert.AreEqual(
                    par.IsDefined(typeof(MyAttribute), inherit: true),
                    p.IsDefined(par, typeof(MyAttribute), inherit: true)
                );
            }
        });

        [TestMethod]
        public void Assembly_Properties() => WithProviders(p =>
        {
            var asm = typeof(string).Assembly;

#if !NET5_0
            Assert.AreEqual(asm.CodeBase, p.GetCodeBase(asm));
            Assert.AreEqual(asm.EscapedCodeBase, p.GetEscapedCodeBase(asm));
            Assert.AreEqual(asm.GlobalAssemblyCache, p.GetGlobalAssemblyCache(asm));
#endif

            Assert.AreEqual(asm.FullName, p.GetFullName(asm));
            Assert.AreEqual(asm.HostContext, p.GetHostContext(asm));
            Assert.AreEqual(asm.ImageRuntimeVersion, p.GetImageRuntimeVersion(asm));
            Assert.AreEqual(asm.IsDynamic, p.IsDynamic(asm));
            Assert.AreEqual(asm.IsFullyTrusted, p.IsFullyTrusted(asm));
            Assert.AreEqual(asm.Location, p.GetLocation(asm));
            Assert.AreEqual(asm.ReflectionOnly, p.GetReflectionOnly(asm));

            Assert.AreEqual(asm.GetName().ToString(), p.GetName(asm).ToString());
            Assert.AreEqual(asm.GetName(copiedName: true).ToString(), p.GetName(asm, copiedName: true).ToString());
        });

        [TestMethod]
        public void Assembly_Introspection() => WithProviders(p =>
        {
            var asm = typeof(Tests).Assembly;

            CollectionAssert.AreEquivalent(asm.DefinedTypes.ToArray(), p.GetDefinedTypes(asm).ToArray());
            CollectionAssert.AreEquivalent(asm.GetExportedTypes(), p.GetExportedTypes(asm).ToArray());
            CollectionAssert.AreEquivalent(asm.GetLoadedModules().Select(a => a.FullyQualifiedName).ToArray(), p.GetLoadedModules(asm).Select(a => a.FullyQualifiedName).ToArray());
            CollectionAssert.AreEquivalent(asm.GetLoadedModules(getResourceModules: true).Select(a => a.FullyQualifiedName).ToArray(), p.GetLoadedModules(asm, getResourceModules: true).Select(a => a.FullyQualifiedName).ToArray());
            CollectionAssert.AreEquivalent(asm.GetModules().Select(a => a.FullyQualifiedName).ToArray(), p.GetModules(asm).Select(a => a.FullyQualifiedName).ToArray());
            CollectionAssert.AreEquivalent(asm.GetReferencedAssemblies().Select(a => a.FullName).ToArray(), p.GetReferencedAssemblies(asm).Select(a => a.FullName).ToArray());
            CollectionAssert.AreEquivalent(asm.GetTypes(), p.GetTypes(asm).ToArray());

            foreach (var m in asm.Modules)
            {
                Assert.AreSame(asm.GetModule(m.Name), p.GetModule(asm, m.Name));
            }
        });

        [TestMethod]
        public void Module_Properties() => WithProviders(p =>
        {
            var mod = typeof(string).Assembly.Modules.First();

            Assert.AreEqual(mod.Assembly, p.GetAssembly(mod));
            Assert.AreEqual(mod.FullyQualifiedName, p.GetFullyQualifiedName(mod));
            Assert.AreEqual(mod.MDStreamVersion, p.GetMDStreamVersion(mod));
            Assert.AreEqual(mod.MetadataToken, p.GetMetadataToken(mod));
            Assert.AreEqual(mod.ModuleHandle, p.GetModuleHandle(mod));
            Assert.AreEqual(mod.ModuleVersionId, p.GetModuleVersionId(mod));
            Assert.AreEqual(mod.Name, p.GetName(mod));
            Assert.AreEqual(mod.ScopeName, p.GetScopeName(mod));

            Assert.AreEqual(mod.IsResource(), p.IsResource(mod));
        });

        [TestMethod]
        public void Module_Introspection() => WithProviders(p =>
        {
            var mod = typeof(Tests).Assembly.Modules.First();

            CollectionAssert.AreEquivalent(mod.GetTypes(), p.GetTypes(mod).ToArray());
        });

        private static void WithProviders(Action<ReflectionProvider> invoke)
        {
            var memoizer = Memoizer.Create(MemoizationCacheFactory.Unbounded);

            var cacheNone = new CachingDefaultReflectionProvider(memoizer, ReflectionCachingOptions.None);
            var cacheAll = new CachingDefaultReflectionProvider(memoizer);

            foreach (var p in new[]
            {
                DefaultReflectionProvider.Instance,
                cacheNone,
                cacheAll,
            })
            {
                invoke(p);
            }

            cacheNone.Clear();
            cacheAll.Clear();
        }

#pragma warning disable IDE0044 // Add readonly modifier
#pragma warning disable IDE0051 // Remove unused private members
#pragma warning disable IDE0052 // Remove unread private members
#pragma warning disable CS0414 // Assigned but never used
#pragma warning disable CA1822 // Mark members as static
        private sealed class C
        {
            private readonly int m_x = 42;
            private int m_y = 42;

            private static readonly int s_x = 42;
            private static int s_y = 42;

            private const int A = 42;

            public int X = 42;
            public static int Y = 42;

            static C() { }

            public C() { }
            public C(int x) => m_x = x;

            public static int F(int x = 1) => x;

            public static int G<T>() => 42;
            public static int H<T1, T2>() => 42;

            public int P { get; set; }

            public event Action E;

            public void Raise() => E?.Invoke();

            public int Bar([MyAttribute(42)] int x) => x;

            private sealed class N
            {
            }
        }

        [AttributeUsage(AttributeTargets.Parameter)]
        private sealed class MyAttribute : Attribute
        {
            public MyAttribute(int value) => Value = value;

            public int Value { get; }
        }
#pragma warning restore CA1822
#pragma warning restore CS0414
#pragma warning restore IDE0052
#pragma warning restore IDE0051
#pragma warning restore IDE0044
    }
}
