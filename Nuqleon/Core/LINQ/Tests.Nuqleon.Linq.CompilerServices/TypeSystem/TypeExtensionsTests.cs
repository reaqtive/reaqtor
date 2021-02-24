// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.CompilerServices.TypeSystem;
using System.Linq.Expressions;
using System.Reflection;
using Tests.VisualBasic;
using TypeExtensions = System.TypeExtensions;

namespace Tests.System.Linq.CompilerServices
{
    [TestClass]
    public class TypeExtensionsTests
    {
        #region IsAnonymousType

        [TestMethod]
        public void IsAnonymousType_ArgumentChecks()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => TypeExtensions.IsAnonymousType(type: null), ex => Assert.AreEqual("type", ex.ParamName));
        }

        [TestMethod]
        public void IsAnonymousType_CSharp()
        {
            Assert.IsTrue(GetAnonymousType().IsAnonymousType());
        }

        private static Type GetAnonymousType()
        {
            return new { a = 42 }.GetType();
        }

        [TestMethod]
        public void IsAnonymousType_VisualBasic()
        {
            Assert.IsTrue(VisualBasicModule.GetAnonymousObject().GetType().IsAnonymousType());
        }

        [TestMethod]
        public void IsAnonymousType_False()
        {
            Assert.IsFalse(typeof(string).IsAnonymousType());
            Assert.IsFalse(GetClosureType().IsAnonymousType());
            Assert.IsFalse(GetRecordType().IsAnonymousType());
        }

        #endregion

        #region IsClosureType

        [TestMethod]
        public void IsClosureType_ArgumentChecks()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => TypeExtensions.IsClosureClass(type: null), ex => Assert.AreEqual("type", ex.ParamName));
        }

        [TestMethod]
        public void IsClosureType_CSharp()
        {
            Assert.IsTrue(GetClosureType().IsClosureClass());
        }

        private static Type GetClosureType()
        {
            var x = 0;
            Expression<Func<int>> f = () => x;

            return ((MemberExpression)f.Body).Member.DeclaringType;
        }

        [TestMethod]
        public void IsClosureType_VisualBasic()
        {
            Expression<Func<int>> f = VisualBasicModule.GetExpressionWithClosure();

            var t = ((MemberExpression)f.Body).Member.DeclaringType;

            Assert.IsTrue(t.IsClosureClass());
        }

        [TestMethod]
        public void IsClosureClass_False()
        {
            Assert.IsFalse(typeof(string).IsClosureClass());
            Assert.IsFalse(GetAnonymousType().IsClosureClass());
            Assert.IsFalse(GetRecordType().IsClosureClass());
        }

        #endregion

        #region IsRecordType

        [TestMethod]
        public void IsRecordType_ArgumentChecks()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => TypeExtensions.IsRecordType(type: null), ex => Assert.AreEqual("type", ex.ParamName));
        }

        [TestMethod]
        public void IsRecordType_False()
        {
            Assert.IsFalse(typeof(string).IsRecordType());
            Assert.IsFalse(GetAnonymousType().IsRecordType());
            Assert.IsFalse(GetClosureType().IsRecordType());
        }

        private static Type GetRecordType()
        {
            return RuntimeCompiler.CreateRecordType(new[] { new KeyValuePair<string, Type>("bar", typeof(int)) }, valueEquality: true);
        }

        #endregion

        #region IsCompilerGenerated

        [TestMethod]
        public void IsCompilerGenerated_ArgumentChecks()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => TypeExtensions.IsCompilerGenerated(type: null), ex => Assert.AreEqual("type", ex.ParamName));
        }

        [TestMethod]
        public void IsCompilerGenerated_Simple()
        {
            var t = new { a = 42 }.GetType();
            Assert.IsTrue(t.IsCompilerGenerated());
            Assert.IsFalse(typeof(string).IsCompilerGenerated());
        }

        #endregion

        #region IsSzArray

        [TestMethod]
        public void IsSzArray_ArgumentChecks()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => TypeExtensions.IsSZArray(type: null), ex => Assert.AreEqual("type", ex.ParamName));
        }

        [TestMethod]
        public void IsSzArray_Simple()
        {
            Assert.IsTrue(typeof(int[]).IsSZArray());
            Assert.IsTrue(typeof(string[]).IsSZArray());
            Assert.IsTrue(typeof(string[][]).IsSZArray());

            Assert.IsFalse(typeof(string).IsSZArray());
            Assert.IsFalse(typeof(int[,]).IsSZArray());
            Assert.IsFalse(typeof(int).MakeArrayType(1).IsSZArray());
            Assert.IsFalse(typeof(int).MakeByRefType().IsSZArray());
            Assert.IsFalse(typeof(int).MakePointerType().IsSZArray());
            Assert.IsFalse(typeof(List<int>).MakePointerType().IsSZArray());
        }

        #endregion

        #region FindGenericType

        [TestMethod]
        public void FindGenericType_ArgumentChecks()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => TypeExtensions.FindGenericType(type: null, typeof(IEnumerable<>)), ex => Assert.AreEqual(ex.ParamName, "type"));
            AssertEx.ThrowsException<ArgumentNullException>(() => TypeExtensions.FindGenericType(typeof(string), definition: null), ex => Assert.AreEqual(ex.ParamName, "definition"));
            AssertEx.ThrowsException<ArgumentException>(() => TypeExtensions.FindGenericType(typeof(string), typeof(int)), ex => Assert.AreEqual(ex.ParamName, "definition"));
        }

        [TestMethod]
        public void FindGenericType_WithMatches()
        {
            foreach (var t in new FindGenericTypeTriples
            {
                { typeof(List<string>), typeof(IEnumerable<>), typeof(IEnumerable<string>) },
                { typeof(List<string>), typeof(List<>), typeof(List<string>) },
                { typeof(FindGenericTypeTriples), typeof(List<>), typeof(List<FindGenericTypeTriple>) },
                { typeof(FindGenericTypeTriples), typeof(IList<>), typeof(IList<FindGenericTypeTriple>) },
                { typeof(KeyValuePair<string, string>), typeof(KeyValuePair<,>), typeof(KeyValuePair<string, string>) },
                { typeof(IEnumerable<string>), typeof(IEnumerable<>), typeof(IEnumerable<string>) },
                { typeof(IDictionary<string, int>), typeof(IEnumerable<>), typeof(IEnumerable<KeyValuePair<string, int>>) },
                { typeof(Dictionary<string, int>), typeof(IEnumerable<>), typeof(IEnumerable<KeyValuePair<string, int>>) },
                { typeof(Dictionary<string, int>), typeof(Dictionary<,>), typeof(Dictionary<string, int>) },
                { typeof(Dictionary<string, int>), typeof(IDictionary<,>), typeof(IDictionary<string, int>) },
            })
            {
                Assert.AreEqual(t.Input.FindGenericType(t.TypeDefinition), t.Result);
            }
        }

        [TestMethod]
        public void FindGenericType_WithoutMatches()
        {
            foreach (var t in new FindGenericTypeTriples
            {
                { typeof(int), typeof(IEnumerable<>), null },
                { typeof(Func<int>), typeof(Func<,>), null },
                { typeof(Func<int, int, int>), typeof(Func<,>), null },
                { typeof(Func<int>), typeof(Action<>), null },
            })
            {
                Assert.AreEqual(t.Input.FindGenericType(t.TypeDefinition), t.Result);
            }
        }

        private sealed class FindGenericTypeTriple
        {
            public Type Input;
            public Type TypeDefinition;
            public Type Result;
        }

        private sealed class FindGenericTypeTriples : List<FindGenericTypeTriple>
        {
            public void Add(Type input, Type typeDefinition, Type result)
            {
                base.Add(new FindGenericTypeTriple { Input = input, TypeDefinition = typeDefinition, Result = result });
            }
        }

        #endregion

        #region ToCSharpString

        [TestMethod]
        public void ToCSharpString_ArgumentChecks()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => TypeExtensions.ToCSharpString(type: null), ex => Assert.AreEqual("type", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => TypeExtensions.ToCSharpStringPretty(type: null), ex => Assert.AreEqual("type", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => TypeExtensions.ToCSharpString(type: null, useNamespaceQualifiedNames: false, useCSharpTypeAliases: false, disallowCompilerGeneratedTypes: false), ex => Assert.AreEqual("type", ex.ParamName));
        }

        [TestMethod]
        public void ToCSharpString_Primitives_Aliases()
        {
            Assert.AreEqual("bool", typeof(bool).ToCSharpString());
            Assert.AreEqual("object", typeof(object).ToCSharpString());
            Assert.AreEqual("string", typeof(string).ToCSharpString());
            Assert.AreEqual("int", typeof(int).ToCSharpString());
            Assert.AreEqual("uint", typeof(uint).ToCSharpString());
            Assert.AreEqual("short", typeof(short).ToCSharpString());
            Assert.AreEqual("ushort", typeof(ushort).ToCSharpString());
            Assert.AreEqual("long", typeof(long).ToCSharpString());
            Assert.AreEqual("ulong", typeof(ulong).ToCSharpString());
            Assert.AreEqual("byte", typeof(byte).ToCSharpString());
            Assert.AreEqual("sbyte", typeof(sbyte).ToCSharpString());
            Assert.AreEqual("char", typeof(char).ToCSharpString());
            Assert.AreEqual("decimal", typeof(decimal).ToCSharpString());
            Assert.AreEqual("double", typeof(double).ToCSharpString());
            Assert.AreEqual("float", typeof(float).ToCSharpString());
        }

        [TestMethod]
        public void ToCSharpString_Primitives_TypeNames()
        {
            Assert.AreEqual("Boolean", typeof(bool).ToCSharpString(useNamespaceQualifiedNames: false, useCSharpTypeAliases: false, disallowCompilerGeneratedTypes: false));
            Assert.AreEqual("Object", typeof(object).ToCSharpString(useNamespaceQualifiedNames: false, useCSharpTypeAliases: false, disallowCompilerGeneratedTypes: false));
            Assert.AreEqual("String", typeof(string).ToCSharpString(useNamespaceQualifiedNames: false, useCSharpTypeAliases: false, disallowCompilerGeneratedTypes: false));
            Assert.AreEqual("Int32", typeof(int).ToCSharpString(useNamespaceQualifiedNames: false, useCSharpTypeAliases: false, disallowCompilerGeneratedTypes: false));
            Assert.AreEqual("UInt32", typeof(uint).ToCSharpString(useNamespaceQualifiedNames: false, useCSharpTypeAliases: false, disallowCompilerGeneratedTypes: false));
            Assert.AreEqual("Int16", typeof(short).ToCSharpString(useNamespaceQualifiedNames: false, useCSharpTypeAliases: false, disallowCompilerGeneratedTypes: false));
            Assert.AreEqual("UInt16", typeof(ushort).ToCSharpString(useNamespaceQualifiedNames: false, useCSharpTypeAliases: false, disallowCompilerGeneratedTypes: false));
            Assert.AreEqual("Int64", typeof(long).ToCSharpString(useNamespaceQualifiedNames: false, useCSharpTypeAliases: false, disallowCompilerGeneratedTypes: false));
            Assert.AreEqual("UInt64", typeof(ulong).ToCSharpString(useNamespaceQualifiedNames: false, useCSharpTypeAliases: false, disallowCompilerGeneratedTypes: false));
            Assert.AreEqual("Byte", typeof(byte).ToCSharpString(useNamespaceQualifiedNames: false, useCSharpTypeAliases: false, disallowCompilerGeneratedTypes: false));
            Assert.AreEqual("SByte", typeof(sbyte).ToCSharpString(useNamespaceQualifiedNames: false, useCSharpTypeAliases: false, disallowCompilerGeneratedTypes: false));
            Assert.AreEqual("Char", typeof(char).ToCSharpString(useNamespaceQualifiedNames: false, useCSharpTypeAliases: false, disallowCompilerGeneratedTypes: false));
            Assert.AreEqual("Decimal", typeof(decimal).ToCSharpString(useNamespaceQualifiedNames: false, useCSharpTypeAliases: false, disallowCompilerGeneratedTypes: false));
            Assert.AreEqual("Double", typeof(double).ToCSharpString(useNamespaceQualifiedNames: false, useCSharpTypeAliases: false, disallowCompilerGeneratedTypes: false));
            Assert.AreEqual("Single", typeof(float).ToCSharpString(useNamespaceQualifiedNames: false, useCSharpTypeAliases: false, disallowCompilerGeneratedTypes: false));
        }

        [TestMethod]
        public void ToCSharpString_Nullables()
        {
            Assert.AreEqual("bool?", typeof(bool?).ToCSharpString());
            Assert.AreEqual("int?", typeof(int?).ToCSharpString());
            Assert.AreEqual("uint?", typeof(uint?).ToCSharpString());
            Assert.AreEqual("short?", typeof(short?).ToCSharpString());
            Assert.AreEqual("ushort?", typeof(ushort?).ToCSharpString());
            Assert.AreEqual("long?", typeof(long?).ToCSharpString());
            Assert.AreEqual("ulong?", typeof(ulong?).ToCSharpString());
            Assert.AreEqual("byte?", typeof(byte?).ToCSharpString());
            Assert.AreEqual("sbyte?", typeof(sbyte?).ToCSharpString());
            Assert.AreEqual("char?", typeof(char?).ToCSharpString());
            Assert.AreEqual("decimal?", typeof(decimal?).ToCSharpString());
            Assert.AreEqual("double?", typeof(double?).ToCSharpString());
            Assert.AreEqual("float?", typeof(float?).ToCSharpString());

            Assert.AreEqual("TimeSpan?", typeof(TimeSpan?).ToCSharpString());
        }

        [TestMethod]
        public void ToCSharpString_Arrays()
        {
            Assert.AreEqual("string[]", typeof(string[]).ToCSharpString());
            Assert.AreEqual("string[,]", typeof(string[,]).ToCSharpString());
            Assert.AreEqual("string[,,]", typeof(string[,,]).ToCSharpString());
            Assert.AreEqual("string[][]", typeof(string[][]).ToCSharpString());
        }

        [TestMethod]
        public void ToCSharpString_Arrays_FullNames()
        {
            Assert.AreEqual("System.String[]", typeof(string[]).ToCSharpString(useNamespaceQualifiedNames: true, useCSharpTypeAliases: false, disallowCompilerGeneratedTypes: true));
            Assert.AreEqual("System.String[,]", typeof(string[,]).ToCSharpString(useNamespaceQualifiedNames: true, useCSharpTypeAliases: false, disallowCompilerGeneratedTypes: true));
            Assert.AreEqual("System.String[,,]", typeof(string[,,]).ToCSharpString(useNamespaceQualifiedNames: true, useCSharpTypeAliases: false, disallowCompilerGeneratedTypes: true));
            Assert.AreEqual("System.String[][]", typeof(string[][]).ToCSharpString(useNamespaceQualifiedNames: true, useCSharpTypeAliases: false, disallowCompilerGeneratedTypes: true));
        }

        [TestMethod]
        public void ToCSharpString_Generics()
        {
            Assert.AreEqual("List<int>", typeof(List<int>).ToCSharpString());
            Assert.AreEqual("Dictionary<string, int>", typeof(Dictionary<string, int>).ToCSharpString());

            Assert.AreEqual("List<T>", typeof(List<>).ToCSharpString());
            Assert.AreEqual("Dictionary<TKey, TValue>", typeof(Dictionary<,>).ToCSharpString());
        }

        [TestMethod]
        public void ToCSharpString_Generics_FullNames()
        {
            Assert.AreEqual("System.Collections.Generic.List<System.Int32>", typeof(List<int>).ToCSharpString(useNamespaceQualifiedNames: true, useCSharpTypeAliases: false, disallowCompilerGeneratedTypes: true));
            Assert.AreEqual("System.Collections.Generic.Dictionary<System.String, System.Int32>", typeof(Dictionary<string, int>).ToCSharpString(useNamespaceQualifiedNames: true, useCSharpTypeAliases: false, disallowCompilerGeneratedTypes: true));

            Assert.AreEqual("System.Collections.Generic.List<T>", typeof(List<>).ToCSharpString(useNamespaceQualifiedNames: true, useCSharpTypeAliases: false, disallowCompilerGeneratedTypes: true));
            Assert.AreEqual("System.Collections.Generic.Dictionary<TKey, TValue>", typeof(Dictionary<,>).ToCSharpString(useNamespaceQualifiedNames: true, useCSharpTypeAliases: false, disallowCompilerGeneratedTypes: true));
        }

        [TestMethod]
        public void ToCSharpString_Pointers()
        {
            Assert.AreEqual("int*", typeof(int*).ToCSharpString());
            Assert.AreEqual("int**", typeof(int**).ToCSharpString());
        }

        [TestMethod]
        public void ToCSharpString_Pointers_FullNames()
        {
            Assert.AreEqual("System.Int32*", typeof(int*).ToCSharpString(useNamespaceQualifiedNames: true, useCSharpTypeAliases: false, disallowCompilerGeneratedTypes: true));
            Assert.AreEqual("System.Int32**", typeof(int**).ToCSharpString(useNamespaceQualifiedNames: true, useCSharpTypeAliases: false, disallowCompilerGeneratedTypes: true));
        }

        [TestMethod]
        public void ToCSharpString_ByRefs()
        {
            Assert.AreEqual("ref int", typeof(int).MakeByRefType().ToCSharpString());
        }

        [TestMethod]
        public void ToCSharpString_ByRefs_FullNames()
        {
            Assert.AreEqual("ref System.Int32", typeof(int).MakeByRefType().ToCSharpString(useNamespaceQualifiedNames: true, useCSharpTypeAliases: false, disallowCompilerGeneratedTypes: true));
        }

        [TestMethod]
        public void ToCSharpString_ComplexTypes()
        {
            Assert.AreEqual("Func<bool, Dictionary<string, int[]>[,]>", typeof(Func<bool, Dictionary<string, int[]>[,]>).ToCSharpString());
            Assert.AreEqual("Func<bool, Func<int, Func<double, string>>>", typeof(Func<bool, Func<int, Func<double, string>>>).ToCSharpString());
        }

        [TestMethod]
        public void ToCSharpString_NonPrintable()
        {
            var t = new { a = 42 }.GetType();

            Assert.ThrowsException<InvalidOperationException>(() => t.ToCSharpString());

            var n = t.ToCSharpString(useNamespaceQualifiedNames: true, useCSharpTypeAliases: false, disallowCompilerGeneratedTypes: false);
            Assert.IsTrue(n.StartsWith("<>f__AnonymousType"));
            Assert.IsTrue(n.EndsWith("<System.Int32>"));
        }

        [TestMethod]
        public void ToCSharpString_MultiDimensionalOfOne()
        {
            Assert.ThrowsException<InvalidOperationException>(() => typeof(int).MakeArrayType(1).ToCSharpString());
        }

        #endregion

        #region TryUnifyExact

        [TestMethod]
        public void TryUnifyExact_ArgumentChecking()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => TypeExtensions.TryUnifyExact(type1: null, typeof(int), out _), ex => Assert.AreEqual("type1", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => TypeExtensions.TryUnifyExact(typeof(int), type2: null, out _), ex => Assert.AreEqual("type2", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => TypeExtensions.TryUnifyExact(type1: null, typeof(int), EqualityComparer<Type>.Default, out _), ex => Assert.AreEqual("type1", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => TypeExtensions.TryUnifyExact(typeof(int), type2: null, EqualityComparer<Type>.Default, out _), ex => Assert.AreEqual("type2", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => TypeExtensions.TryUnifyExact(typeof(int), typeof(int), comparer: null, out _), ex => Assert.AreEqual("comparer", ex.ParamName));
        }

        [TestMethod]
        public void TryUnifyExact_Success()
        {
            var t1 = typeof(Func<T, Func<int, R>>);
            var t2 = typeof(Func<string, Func<int, bool>>);

            Assert.IsTrue(TypeExtensions.TryUnifyExact(t1, t2, out IDictionary<Type, Type> r1));

            Assert.AreEqual(2, r1.Count);
            Assert.AreSame(r1[typeof(T)], typeof(string));
            Assert.AreSame(r1[typeof(R)], typeof(bool));

            Assert.IsTrue(TypeExtensions.TryUnifyExact(t1, t2, EqualityComparer<Type>.Default, out IDictionary<Type, Type> r2));

            Assert.AreEqual(2, r2.Count);
            Assert.AreSame(r2[typeof(T)], typeof(string));
            Assert.AreSame(r2[typeof(R)], typeof(bool));
        }

        [TestMethod]
        public void TryUnifyExact_Failure()
        {
            var t1 = typeof(Func<T, Func<int, T>>);
            var t2 = typeof(Func<string, Func<int, bool>>);

            Assert.IsFalse(TypeExtensions.TryUnifyExact(t1, t2, out _));
        }

        #endregion

        #region UnifyExact

        [TestMethod]
        public void UnifyExact_ArgumentChecking()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => TypeExtensions.UnifyExact(type1: null, typeof(int)), ex => Assert.AreEqual("type1", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => TypeExtensions.UnifyExact(typeof(int), type2: null), ex => Assert.AreEqual("type2", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => TypeExtensions.UnifyExact(type1: null, typeof(int), EqualityComparer<Type>.Default), ex => Assert.AreEqual("type1", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => TypeExtensions.UnifyExact(typeof(int), type2: null, EqualityComparer<Type>.Default), ex => Assert.AreEqual("type2", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => TypeExtensions.UnifyExact(typeof(int), typeof(int), comparer: null), ex => Assert.AreEqual("comparer", ex.ParamName));
        }

        [TestMethod]
        public void UnifyExact_Exact_Simple()
        {
            var t1 = typeof(int);
            var t2 = t1;

            var res = t1.UnifyExact(t2);

            Assert.AreEqual(0, res.Count);
        }

        [TestMethod]
        public void UnifyExact_Exact_Complex()
        {
            var t1 = typeof(IEnumerable<int[]>[,]);
            var t2 = t1;

            var res = t1.UnifyExact(t2);

            Assert.AreEqual(0, res.Count);
        }

        [TestMethod]
        public void UnifyExact_Simple()
        {
            var t1 = typeof(int);
            var t2 = typeof(T);

            var res1 = t1.UnifyExact(t2);
            Assert.AreEqual(1, res1.Count);
            Assert.AreSame(t1, res1[typeof(T)]);

            var res2 = t2.UnifyExact(t1);
            Assert.AreEqual(1, res2.Count);
            Assert.AreSame(t1, res2[typeof(T)]);
        }

        [TestMethod]
        public void UnifyExact_Simple_BigMatch()
        {
            var t1 = typeof(int[]);
            var t2 = typeof(T);

            var res1 = t1.UnifyExact(t2);
            Assert.AreEqual(1, res1.Count);
            Assert.AreSame(t1, res1[typeof(T)]);

            var res2 = t2.UnifyExact(t1);
            Assert.AreEqual(1, res2.Count);
            Assert.AreSame(t1, res2[typeof(T)]);
        }

        [TestMethod]
        public void UnifyExact_Simple_DeepMatch()
        {
            var t1 = typeof(int[]);
            var t2 = typeof(T[]);

            var res1 = t1.UnifyExact(t2);
            Assert.AreEqual(1, res1.Count);
            Assert.AreSame(typeof(int), res1[typeof(T)]);

            var res2 = t2.UnifyExact(t1);
            Assert.AreEqual(1, res2.Count);
            Assert.AreSame(typeof(int), res2[typeof(T)]);
        }

        [TestMethod]
        public void UnifyExact_Equality_Success()
        {
            var t1 = typeof(Func<int, int>);
            var t2 = typeof(Func<T, T>);

            var res1 = t1.UnifyExact(t2);
            Assert.AreEqual(1, res1.Count);
            Assert.AreSame(typeof(int), res1[typeof(T)]);

            var res2 = t2.UnifyExact(t1);
            Assert.AreEqual(1, res2.Count);
            Assert.AreSame(typeof(int), res2[typeof(T)]);
        }

        [TestMethod]
        public void UnifyExact_Equality_Failure()
        {
            var t1 = typeof(Func<int, string>);
            var t2 = typeof(Func<T, T>);

            Assert.ThrowsException<InvalidOperationException>(() => t1.UnifyExact(t2));
            Assert.ThrowsException<InvalidOperationException>(() => t2.UnifyExact(t1));
        }

        [TestMethod]
        public void UnifyExact_ComplexMatch_Success1()
        {
            var t1 = typeof(Func<int, R, int>);
            var t2 = typeof(Func<T, T, R>);

            var res1 = t1.UnifyExact(t2);
            Assert.AreEqual(2, res1.Count);
            Assert.AreSame(typeof(int), res1[typeof(T)]);
            Assert.AreSame(typeof(int), res1[typeof(R)]);

            var res2 = t2.UnifyExact(t1);
            Assert.AreEqual(2, res2.Count);
            Assert.AreSame(typeof(int), res2[typeof(T)]);
            Assert.AreSame(typeof(int), res2[typeof(R)]);
        }

        [TestMethod]
        public void UnifyExact_ComplexMatch_Success2()
        {
            var t1 = typeof(Func<R, int, int>);
            var t2 = typeof(Func<T, T, R>);

            var res1 = t1.UnifyExact(t2);
            Assert.AreEqual(2, res1.Count);
            Assert.AreSame(typeof(int), res1[typeof(T)]);
            Assert.AreSame(typeof(int), res1[typeof(R)]);

            var res2 = t2.UnifyExact(t1);
            Assert.AreEqual(2, res2.Count);
            Assert.AreSame(typeof(int), res2[typeof(T)]);
            Assert.AreSame(typeof(int), res2[typeof(R)]);
        }

        [TestMethod]
        public void UnifyExact_ComplexMatch_Success3()
        {
            var t1 = typeof(Func<int, int, R>);
            var t2 = typeof(Func<T, R, T>);

            var res1 = t1.UnifyExact(t2);
            Assert.AreEqual(2, res1.Count);
            Assert.AreSame(typeof(int), res1[typeof(T)]);
            Assert.AreSame(typeof(int), res1[typeof(R)]);

            var res2 = t2.UnifyExact(t1);
            Assert.AreEqual(2, res2.Count);
            Assert.AreSame(typeof(int), res2[typeof(T)]);
            Assert.AreSame(typeof(int), res2[typeof(R)]);
        }

        [TypeWildcard]
        private sealed class U
        {
        }

        [TestMethod]
        public void UnifyExact_ComplexMatch_Success4()
        {
            var t1 = typeof(Func<R, U, int>);
            var t2 = typeof(Func<T, T, R>);

            var res1 = t1.UnifyExact(t2);
            Assert.AreEqual(3, res1.Count);
            Assert.AreSame(typeof(int), res1[typeof(T)]);
            Assert.AreSame(typeof(int), res1[typeof(R)]);
            Assert.AreSame(typeof(int), res1[typeof(U)]);

            var res2 = t2.UnifyExact(t1);
            Assert.AreEqual(3, res2.Count);
            Assert.AreSame(typeof(int), res2[typeof(T)]);
            Assert.AreSame(typeof(int), res2[typeof(R)]);
            Assert.AreSame(typeof(int), res2[typeof(U)]);
        }

        [TestMethod]
        public void UnifyExact_ComplexMatch_Success5()
        {
            var t1 = typeof(Func<R, U, U, int>);
            var t2 = typeof(Func<T, T, R, T>);

            var res1 = t1.UnifyExact(t2);
            Assert.AreEqual(3, res1.Count);
            Assert.AreSame(typeof(int), res1[typeof(T)]);
            Assert.AreSame(typeof(int), res1[typeof(R)]);
            Assert.AreSame(typeof(int), res1[typeof(U)]);

            var res2 = t2.UnifyExact(t1);
            Assert.AreEqual(3, res2.Count);
            Assert.AreSame(typeof(int), res2[typeof(T)]);
            Assert.AreSame(typeof(int), res2[typeof(R)]);
            Assert.AreSame(typeof(int), res2[typeof(U)]);
        }

        [TypeWildcard]
        private sealed class T1
        {
        }

        [TypeWildcard]
        private sealed class T2
        {
        }

        [TypeWildcard]
        private sealed class T3
        {
        }

        [TypeWildcard]
        private sealed class T4
        {
        }

        [TestMethod]
        public void UnifyExact_ComplexMatch_Success6()
        {
            var t1 = typeof(Func<T1, T2, T3, T1>);
            var t2 = typeof(Func<T3, T1, T2, int>);

            var res1 = t1.UnifyExact(t2);
            Assert.AreEqual(3, res1.Count);
            Assert.AreSame(typeof(int), res1[typeof(T1)]);
            Assert.AreSame(typeof(int), res1[typeof(T2)]);
            Assert.AreSame(typeof(int), res1[typeof(T3)]);

            var res2 = t2.UnifyExact(t1);
            Assert.AreEqual(3, res2.Count);
            Assert.AreSame(typeof(int), res2[typeof(T1)]);
            Assert.AreSame(typeof(int), res2[typeof(T2)]);
            Assert.AreSame(typeof(int), res2[typeof(T3)]);
        }

        [TestMethod]
        public void UnifyExact_ComplexMatch_Success7()
        {
            var t1 = typeof(Func<T1, T2, T2, T1>);
            var t2 = typeof(Func<T3, T1, T3, int>);

            var res1 = t1.UnifyExact(t2);
            Assert.AreEqual(3, res1.Count);
            Assert.AreSame(typeof(int), res1[typeof(T1)]);
            Assert.AreSame(typeof(int), res1[typeof(T2)]);
            Assert.AreSame(typeof(int), res1[typeof(T3)]);

            var res2 = t2.UnifyExact(t1);
            Assert.AreEqual(3, res2.Count);
            Assert.AreSame(typeof(int), res2[typeof(T1)]);
            Assert.AreSame(typeof(int), res2[typeof(T2)]);
            Assert.AreSame(typeof(int), res2[typeof(T3)]);
        }

        [TestMethod]
        public void UnifyExact_ComplexMatch_Failure_Conflict1()
        {
            var t1 = typeof(Func<int, R, string>);
            var t2 = typeof(Func<T, T, R>);

            Assert.ThrowsException<InvalidOperationException>(() => t1.UnifyExact(t2));
            Assert.ThrowsException<InvalidOperationException>(() => t2.UnifyExact(t1));
        }

        [TestMethod]
        public void UnifyExact_ComplexMatch_Failure_Conflict2()
        {
            var t1 = typeof(Func<T1, T2, T1, T1, string>);
            var t2 = typeof(Func<T3, T1, int, R, R>);

            Assert.ThrowsException<InvalidOperationException>(() => t1.UnifyExact(t2));
            Assert.ThrowsException<InvalidOperationException>(() => t2.UnifyExact(t1));
        }

        [TestMethod]
        public void UnifyExact_ComplexMatch_Failure_Conflict3()
        {
            var t1 = typeof(Func<T1, T2, T1>);
            var t2 = typeof(Func<int, string, T2>);

            Assert.ThrowsException<InvalidOperationException>(() => t1.UnifyExact(t2));
            Assert.ThrowsException<InvalidOperationException>(() => t2.UnifyExact(t1));
        }

        [TestMethod]
        public void UnifyExact_MissingMatch()
        {
            var t1 = typeof(Func<T, R>);
            var t2 = typeof(Func<int, R>);

            Assert.ThrowsException<InvalidOperationException>(() => t1.UnifyExact(t2));
            Assert.ThrowsException<InvalidOperationException>(() => t2.UnifyExact(t1));
        }

        [TestMethod]
        public void UnifyExact_StructuralInequality()
        {
            var t1 = typeof(Func<IEnumerable<T>>);
            var t2 = typeof(Func<int[]>);

            Assert.ThrowsException<InvalidOperationException>(() => t1.UnifyExact(t2));
            Assert.ThrowsException<InvalidOperationException>(() => t2.UnifyExact(t1));
        }

        [TestMethod]
        public void UnifyExact_EquivalenceClasses1()
        {
            var t1 = typeof(Func<T1, T3, T1, T3, T1>);
            var t2 = typeof(Func<T2, T4, T3, int, T3>);

            var res1 = t1.UnifyExact(t2);
            Assert.AreEqual(4, res1.Count);
            Assert.AreSame(typeof(int), res1[typeof(T1)]);
            Assert.AreSame(typeof(int), res1[typeof(T2)]);
            Assert.AreSame(typeof(int), res1[typeof(T3)]);
            Assert.AreSame(typeof(int), res1[typeof(T4)]);

            var res2 = t2.UnifyExact(t1);
            Assert.AreEqual(4, res2.Count);
            Assert.AreSame(typeof(int), res2[typeof(T1)]);
            Assert.AreSame(typeof(int), res2[typeof(T2)]);
            Assert.AreSame(typeof(int), res2[typeof(T3)]);
            Assert.AreSame(typeof(int), res2[typeof(T4)]);
        }

        [TestMethod]
        public void UnifyExact_EquivalenceClasses2()
        {
            var t1 = typeof(Func<T1, T1, T3, T1>);
            var t2 = typeof(Func<T2, int, T4, T3>);

            var res1 = t1.UnifyExact(t2);
            Assert.AreEqual(4, res1.Count);
            Assert.AreSame(typeof(int), res1[typeof(T1)]);
            Assert.AreSame(typeof(int), res1[typeof(T2)]);
            Assert.AreSame(typeof(int), res1[typeof(T3)]);
            Assert.AreSame(typeof(int), res1[typeof(T4)]);

            var res2 = t2.UnifyExact(t1);
            Assert.AreEqual(4, res2.Count);
            Assert.AreSame(typeof(int), res2[typeof(T1)]);
            Assert.AreSame(typeof(int), res2[typeof(T2)]);
            Assert.AreSame(typeof(int), res2[typeof(T3)]);
            Assert.AreSame(typeof(int), res2[typeof(T4)]);
        }

        [TestMethod]
        public void UnifyExact_EquivalenceClasses3()
        {
            var t1 = typeof(Func<T1, T3, T3, T1>);
            var t2 = typeof(Func<T2, T4, int, T3>);

            var res1 = t1.UnifyExact(t2);
            Assert.AreEqual(4, res1.Count);
            Assert.AreSame(typeof(int), res1[typeof(T1)]);
            Assert.AreSame(typeof(int), res1[typeof(T2)]);
            Assert.AreSame(typeof(int), res1[typeof(T3)]);
            Assert.AreSame(typeof(int), res1[typeof(T4)]);

            var res2 = t2.UnifyExact(t1);
            Assert.AreEqual(4, res2.Count);
            Assert.AreSame(typeof(int), res2[typeof(T1)]);
            Assert.AreSame(typeof(int), res2[typeof(T2)]);
            Assert.AreSame(typeof(int), res2[typeof(T3)]);
            Assert.AreSame(typeof(int), res2[typeof(T4)]);
        }

        [TestMethod]
        public void UnifyExact_MethodDefinitionGenericParameter()
        {
            var mtd = ((MethodInfo)ReflectionHelpers.InfoOf(() =>
                Enumerable.Any(default(IEnumerable<object>)))).GetGenericMethodDefinition();

            var t1 = mtd.GetParameters()[0].ParameterType;
            var t2 = typeof(IEnumerable<T>);

            var res1 = t1.UnifyExact(t2);
            Assert.AreEqual(t1.GenericTypeArguments[0], res1[typeof(T)]);
            var res2 = t2.UnifyExact(t1);
            Assert.AreEqual(t1.GenericTypeArguments[0], res2[typeof(T)]);
        }

        [TestMethod]
        public void UnifyExact_EqualityComparer()
        {
            var t1 = typeof(IFoo<int>);
            var t2 = typeof(IBar<T>);
            var t3 = typeof(Func<long, string>);
            var t4 = typeof(Func<ulong, T>);

            var r1 = t1.UnifyExact(t2, new TestTypeComparer());
            var r2 = t3.UnifyExact(t4, new TestTypeComparer());
            Assert.AreEqual(1, r1.Count);
            Assert.AreEqual(t1.GenericTypeArguments[0], r1[typeof(T)]);
            Assert.AreEqual(1, r2.Count);
            Assert.AreEqual(t3.GenericTypeArguments[1], r2[typeof(T)]);
        }

        private interface IFoo<T> { }

        private interface IBar<T> { }

        private sealed class TestTypeComparer : TypeEqualityComparer
        {
            protected override bool EqualsSimple(Type x, Type y)
            {
                if (x == typeof(long) && y == typeof(ulong) ||
                    x == typeof(ulong) && y == typeof(long))
                {
                    return true;
                }

                return base.EqualsSimple(x, y);
            }

            protected override bool EqualsGenericTypeDefinition(Type x, Type y)
            {
                if (x == typeof(IFoo<>) && y == typeof(IBar<>) ||
                    x == typeof(IBar<>) && y == typeof(IFoo<>))
                {
                    return true;
                }

                return base.EqualsGenericTypeDefinition(x, y);
            }
        }

        #endregion

        #region TryUnifyWith

        [TestMethod]
        public void TryUnifyWith_ArgumentChecking()
        {
            var res = default(IDictionary<Type, Type>);
            AssertEx.ThrowsException<ArgumentNullException>(() => TypeExtensions.TryUnifyWith(left: null, typeof(int), out res), ex => Assert.AreEqual("left", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => TypeExtensions.TryUnifyWith(typeof(int), right: null, out res), ex => Assert.AreEqual("right", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => TypeExtensions.TryUnifyWith(left: null, typeof(int), EqualityComparer<Type>.Default, out res), ex => Assert.AreEqual("left", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => TypeExtensions.TryUnifyWith(typeof(int), right: null, EqualityComparer<Type>.Default, out res), ex => Assert.AreEqual("right", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => TypeExtensions.TryUnifyWith(typeof(int), typeof(int), comparer: null, out res), ex => Assert.AreEqual("comparer", ex.ParamName));
        }

        [TestMethod]
        public void TryUnifyWith_Success()
        {
            var t1 = typeof(Func<T, Func<int, R>>);
            var t2 = typeof(Func<string, Func<int, bool>>);

            Assert.IsTrue(TypeExtensions.TryUnifyWith(t1, t2, out IDictionary<Type, Type> res));

            Assert.AreEqual(2, res.Count);
            Assert.AreSame(res[typeof(T)], typeof(string));
            Assert.AreSame(res[typeof(R)], typeof(bool));

            Assert.IsTrue(TypeExtensions.TryUnifyWith(t1, t2, EqualityComparer<Type>.Default, out IDictionary<Type, Type> r2));

            Assert.AreEqual(2, r2.Count);
            Assert.AreSame(r2[typeof(T)], typeof(string));
            Assert.AreSame(r2[typeof(R)], typeof(bool));
        }

        [TestMethod]
        public void TryUnifyWith_Failure()
        {
            var t1 = typeof(Func<T, Func<int, T>>);
            var t2 = typeof(Func<string, Func<int, bool>>);

            Assert.IsFalse(TypeExtensions.TryUnifyWith(t1, t2, out _));
        }

        #endregion

        #region UnifyWith

        [TestMethod]
        public void UnifyWith_ArgumentChecking()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => TypeExtensions.UnifyWith(left: null, typeof(int)), ex => Assert.AreEqual("left", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => TypeExtensions.UnifyWith(typeof(int), right: null), ex => Assert.AreEqual("right", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => TypeExtensions.UnifyWith(left: null, typeof(int), EqualityComparer<Type>.Default), ex => Assert.AreEqual("left", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => TypeExtensions.UnifyWith(typeof(int), right: null, EqualityComparer<Type>.Default), ex => Assert.AreEqual("right", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => TypeExtensions.UnifyWith(typeof(int), typeof(int), comparer: null), ex => Assert.AreEqual("comparer", ex.ParamName));
        }

        [TestMethod]
        public void UnifyWith_Exact_Simple()
        {
            var t1 = typeof(int);
            var t2 = t1;

            var res = t1.UnifyWith(t2);

            Assert.AreEqual(0, res.Count);
        }

        [TestMethod]
        public void UnifyWith_Exact_Complex()
        {
            var t1 = typeof(IEnumerable<int[]>[,]);
            var t2 = t1;

            var res = t1.UnifyWith(t2);

            Assert.AreEqual(0, res.Count);
        }

        [TestMethod]
        public void UnifyWith_Simple()
        {
            var t1 = typeof(int);
            var t2 = typeof(T);

            var res2 = t2.UnifyWith(t1);
            Assert.AreEqual(1, res2.Count);
            Assert.AreSame(t1, res2[typeof(T)]);
        }

        [TestMethod]
        public void UnifyWith_Simple_BigMatch()
        {
            var t1 = typeof(int[]);
            var t2 = typeof(T);

            var res2 = t2.UnifyWith(t1);
            Assert.AreEqual(1, res2.Count);
            Assert.AreSame(t1, res2[typeof(T)]);
        }

        [TestMethod]
        public void UnifyWith_Simple_DeepMatch()
        {
            var t1 = typeof(int[]);
            var t2 = typeof(T[]);

            var res2 = t2.UnifyWith(t1);
            Assert.AreEqual(1, res2.Count);
            Assert.AreSame(typeof(int), res2[typeof(T)]);
        }

        [TestMethod]
        public void UnifyWith_Equality_Success()
        {
            var t1 = typeof(Func<int, int>);
            var t2 = typeof(Func<T, T>);

            var res2 = t2.UnifyWith(t1);
            Assert.AreEqual(1, res2.Count);
            Assert.AreSame(typeof(int), res2[typeof(T)]);
        }

        [TestMethod]
        public void UnifyWith_Equality_Failure()
        {
            var t1 = typeof(Func<int, string>);
            var t2 = typeof(Func<T, T>);

            Assert.ThrowsException<InvalidOperationException>(() => t1.UnifyWith(t2));
            Assert.ThrowsException<InvalidOperationException>(() => t2.UnifyWith(t1));
        }

        [TestMethod]
        public void UnifyWith_StructuralInequality()
        {
            var t1 = typeof(Func<IEnumerable<T>>);
            var t2 = typeof(Func<int[]>);

            Assert.ThrowsException<InvalidOperationException>(() => t1.UnifyWith(t2));
            Assert.ThrowsException<InvalidOperationException>(() => t2.UnifyWith(t1));
        }

        [TestMethod]
        public void UnifyWith_MethodDefinitionGenericParameter()
        {
            var mtd = ((MethodInfo)ReflectionHelpers.InfoOf(() =>
                Enumerable.Any(default(IEnumerable<object>)))).GetGenericMethodDefinition();

            var t1 = mtd.GetParameters()[0].ParameterType;
            var t2 = typeof(IEnumerable<T>);

            var res = t2.UnifyWith(t1);
            Assert.AreEqual(t1.GenericTypeArguments[0], res[typeof(T)]);
        }

        [TestMethod]
        public void UnifyWith_EqualityComparer()
        {
            var t1 = typeof(IFoo<int>);
            var t2 = typeof(IBar<T>);
            var t3 = typeof(Func<long, string>);
            var t4 = typeof(Func<ulong, T>);

            var r1 = t2.UnifyWith(t1, new TestTypeComparer());
            var r2 = t4.UnifyWith(t3, new TestTypeComparer());
            Assert.AreEqual(1, r1.Count);
            Assert.AreEqual(t1.GenericTypeArguments[0], r1[typeof(T)]);
            Assert.AreEqual(1, r2.Count);
            Assert.AreEqual(t3.GenericTypeArguments[1], r2[typeof(T)]);
        }

        #endregion

        #region Internal helpers

        [TestMethod]
        public void IsNullableType()
        {
            Assert.IsFalse(typeof(int).IsNullableType());
            Assert.IsTrue(typeof(int?).IsNullableType());
        }

        [TestMethod]
        public void GetNonNullableType()
        {
            Assert.AreSame(typeof(int), typeof(int).GetNonNullableType());
            Assert.AreSame(typeof(int), typeof(int?).GetNonNullableType());
        }

        [TestMethod]
        public void IsInteger()
        {
            foreach (var t in new[] {
                typeof(sbyte),
                typeof(byte),
                typeof(short),
                typeof(ushort),
                typeof(int),
                typeof(uint),
                typeof(long),
                typeof(ulong),
            })
            {
                Assert.IsTrue(t.IsInteger());
                Assert.IsTrue(typeof(Nullable<>).MakeGenericType(t).IsInteger());
            }

            foreach (var t in new[] {
                typeof(float),
                typeof(double),
                typeof(object),
                typeof(string),
                typeof(ConsoleColor),
            })
            {
                Assert.IsFalse(t.IsInteger());
            }
        }

        #endregion

        #region IsReferenceAssignableFrom
        [TestMethod]
        public void IsReferenceAssignableFrom_NullChecks()
        {
            Assert.ThrowsException<ArgumentNullException>(() => TypeExtensions.IsReferenceAssignableFrom(lhsType: null, typeof(int), out _));
            Assert.ThrowsException<ArgumentNullException>(() => TypeExtensions.IsReferenceAssignableFrom(typeof(int), rhsType: null, out _));
        }

        [TestMethod]
        public void IsReferenceAssignableFrom_Same()
        {
            AssertIsReferenceAssignableFrom(typeof(int), typeof(int));
            AssertIsReferenceAssignableFrom(typeof(string), typeof(string));

            AssertIsReferenceAssignableFrom(typeof(int[]), typeof(int[]));
            AssertIsReferenceAssignableFrom(typeof(Func<string>), typeof(Func<string>));
        }

        [TestMethod]
        public void IsReferenceAssignableFrom_Different()
        {
            AssertIsNotReferenceAssignableFrom(typeof(int), typeof(string));
            AssertIsNotReferenceAssignableFrom(typeof(string), typeof(int));
        }

        [TestMethod]
        public void IsReferenceAssignableFrom_Subtyping()
        {
            AssertIsReferenceAssignableFrom(typeof(object), typeof(string));
        }

        [TestMethod]
        public void IsReferenceAssignableFrom_Interface()
        {
            AssertIsReferenceAssignableFrom(typeof(IComparable), typeof(string));
            AssertIsReferenceAssignableFrom(typeof(IList), typeof(string[]));
            AssertIsReferenceAssignableFrom(typeof(IList), typeof(int[]));
            AssertIsReferenceAssignableFrom(typeof(IList<string>), typeof(string[]));
        }

        [TestMethod]
        public void IsReferenceAssignableFrom_NoBoxing()
        {
            AssertIsNotReferenceAssignableFrom(typeof(object), typeof(int));
            AssertIsNotReferenceAssignableFrom(typeof(ValueType), typeof(int));
            AssertIsNotReferenceAssignableFrom(typeof(IFormattable), typeof(int));
        }

        [TestMethod]
        public void IsReferenceAssignableFrom_Arrays()
        {
            AssertIsReferenceAssignableFrom(typeof(object), typeof(int[]));
            AssertIsReferenceAssignableFrom(typeof(object), typeof(string[]));
        }

        [TestMethod]
        public void IsReferenceAssignableFrom_ArraysNegative()
        {
            AssertIsNotReferenceAssignableFrom(typeof(int), typeof(int[]));
            AssertIsNotReferenceAssignableFrom(typeof(string), typeof(string[]));
            AssertIsNotReferenceAssignableFrom(typeof(int[]), typeof(int));
            AssertIsNotReferenceAssignableFrom(typeof(string[]), typeof(string));
        }

        [TestMethod]
        public void IsReferenceAssignableFrom_CovariantArrays()
        {
            AssertIsReferenceAssignableFrom(typeof(object[]), typeof(string[]));
            AssertIsReferenceAssignableFrom(typeof(object[,]), typeof(string[,]));
        }

        [TestMethod]
        public void IsReferenceAssignableFrom_CovariantArraysNegative()
        {
            AssertIsNotReferenceAssignableFrom(typeof(string[]), typeof(object[]));
            AssertIsNotReferenceAssignableFrom(typeof(string[,]), typeof(object[,]));
            AssertIsNotReferenceAssignableFrom(typeof(object[]), typeof(int[]));
            AssertIsNotReferenceAssignableFrom(typeof(object[,]), typeof(int[,]));
        }

        [TestMethod]
        public void IsReferenceAssignableFrom_ArraysDifferentRank()
        {
            AssertIsNotReferenceAssignableFrom(typeof(int[]), typeof(int[,]));
            AssertIsNotReferenceAssignableFrom(typeof(int[,]), typeof(int[]));
            AssertIsNotReferenceAssignableFrom(typeof(int).MakeArrayType(1), typeof(int[]));
            AssertIsNotReferenceAssignableFrom(typeof(int[]), typeof(int).MakeArrayType(1));
        }

        [TestMethod]
        public void IsReferenceAssignableFrom_Pointers()
        {
            AssertIsReferenceAssignableFrom(typeof(object).MakePointerType(), typeof(string).MakePointerType());
            AssertIsReferenceAssignableFrom(typeof(IComparable).MakePointerType(), typeof(string).MakePointerType());
            AssertIsNotReferenceAssignableFrom(typeof(string).MakePointerType(), typeof(object).MakePointerType());
            AssertIsNotReferenceAssignableFrom(typeof(string).MakePointerType(), typeof(IComparable).MakePointerType());
            AssertIsNotReferenceAssignableFrom(typeof(object).MakePointerType(), typeof(int).MakePointerType());
            AssertIsNotReferenceAssignableFrom(typeof(object).MakePointerType(), typeof(object));
            AssertIsNotReferenceAssignableFrom(typeof(object).MakePointerType(), typeof(object[]));
            AssertIsNotReferenceAssignableFrom(typeof(object).MakePointerType(), typeof(Func<object>));
        }

        [TestMethod]
        public void IsReferenceAssignableFrom_ByRefs()
        {
            AssertIsReferenceAssignableFrom(typeof(object).MakeByRefType(), typeof(string).MakeByRefType());
            AssertIsReferenceAssignableFrom(typeof(IComparable).MakeByRefType(), typeof(string).MakeByRefType());
            AssertIsNotReferenceAssignableFrom(typeof(string).MakeByRefType(), typeof(object).MakeByRefType());
            AssertIsNotReferenceAssignableFrom(typeof(string).MakeByRefType(), typeof(IComparable).MakeByRefType());
            AssertIsNotReferenceAssignableFrom(typeof(object).MakeByRefType(), typeof(int).MakeByRefType());
            AssertIsNotReferenceAssignableFrom(typeof(object).MakeByRefType(), typeof(object));
            AssertIsNotReferenceAssignableFrom(typeof(object).MakeByRefType(), typeof(object[]));
            AssertIsNotReferenceAssignableFrom(typeof(object).MakeByRefType(), typeof(Func<object>));
        }

        [TestMethod]
        public void IsReferenceAssignableFrom_GenericNegative()
        {
            AssertIsNotReferenceAssignableFrom(typeof(Func<object>), typeof(IEnumerable<object>));
            AssertIsNotReferenceAssignableFrom(typeof(Func<object>), typeof(Func<object, object>));
            AssertIsNotReferenceAssignableFrom(typeof(Func<>), typeof(Func<object>));
            AssertIsNotReferenceAssignableFrom(typeof(Func<object>), typeof(Func<>));
        }

        [TestMethod]
        public void IsReferenceAssignableFrom_GenericVariance()
        {
            AssertIsReferenceAssignableFrom(typeof(IEnumerable<object>), typeof(IEnumerable<string>));
            AssertIsReferenceAssignableFrom(typeof(Func<object>), typeof(Func<string>));
            AssertIsReferenceAssignableFrom(typeof(Func<IEnumerable<object>>), typeof(Func<IEnumerable<string>>));
            AssertIsReferenceAssignableFrom(typeof(Action<string>), typeof(Action<object>));
            AssertIsReferenceAssignableFrom(typeof(Func<int, object>), typeof(Func<int, string>));
            AssertIsReferenceAssignableFrom(typeof(Func<string, object>), typeof(Func<object, string>));
        }

        [TestMethod]
        public void IsReferenceAssignableFrom_GenericVarianceNegative()
        {
            AssertIsNotReferenceAssignableFrom(typeof(IEnumerable<object>), typeof(IEnumerable<int>));
            AssertIsNotReferenceAssignableFrom(typeof(IEnumerable<string>), typeof(IEnumerable<object>));
            AssertIsNotReferenceAssignableFrom(typeof(Func<string>), typeof(Func<object>));
            AssertIsNotReferenceAssignableFrom(typeof(Func<IEnumerable<string>>), typeof(Func<IEnumerable<object>>));
            AssertIsNotReferenceAssignableFrom(typeof(Action<object>), typeof(Action<string>));
            AssertIsNotReferenceAssignableFrom(typeof(Func<object, int>), typeof(Func<string, int>));
            AssertIsNotReferenceAssignableFrom(typeof(Func<object, string>), typeof(Func<string, object>));
        }

        [TestMethod]
        public void IsReferenceAssignableFrom_GenericInvariance()
        {
            AssertIsReferenceAssignableFrom(typeof(List<IEnumerable<string>>), typeof(List<IEnumerable<string>>));
        }

        [TestMethod]
        public void IsReferenceAssignableFrom_GenericInheritance()
        {
            AssertIsReferenceAssignableFrom(typeof(IEnumerable<IEnumerable<int>>), typeof(IEnumerable<IGrouping<string, int>>));
            AssertIsReferenceAssignableFrom(typeof(IList<IEnumerable<string>>), typeof(List<IEnumerable<string>>));
        }

        [TestMethod]
        public void IsReferenceAssignableFrom_Unification_Trivial()
        {
            AssertIsReferenceAssignableFrom(typeof(T), typeof(int), new Dictionary<Type, Type>
            {
                { typeof(T), typeof(int) }
            });

            AssertIsReferenceAssignableFrom(typeof(T[]), typeof(int[]), new Dictionary<Type, Type>
            {
                { typeof(T), typeof(int) }
            });

            AssertIsReferenceAssignableFrom(typeof(T[,]), typeof(int[,]), new Dictionary<Type, Type>
            {
                { typeof(T), typeof(int) }
            });

            AssertIsReferenceAssignableFrom(typeof(Func<T>), typeof(Func<int>), new Dictionary<Type, Type>
            {
                { typeof(T), typeof(int) }
            });

            AssertIsReferenceAssignableFrom(typeof(Action<T>), typeof(Action<int>), new Dictionary<Type, Type>
            {
                { typeof(T), typeof(int) }
            });
        }

        [TestMethod]
        public void IsReferenceAssignableFrom_Unification_Match()
        {
            AssertIsReferenceAssignableFrom(typeof(Func<T, T>), typeof(Func<int, int>), new Dictionary<Type, Type>
            {
                { typeof(T), typeof(int) }
            });

            AssertIsReferenceAssignableFrom(typeof(Func<T, T[]>), typeof(Func<string, string[]>), new Dictionary<Type, Type>
            {
                { typeof(T), typeof(string) }
            });

            AssertIsReferenceAssignableFrom(typeof(Func<T, IList<T>>), typeof(Func<string, string[]>), new Dictionary<Type, Type>
            {
                { typeof(T), typeof(string) }
            });

            AssertIsReferenceAssignableFrom(typeof(IEnumerable<IEnumerable<T>>), typeof(IEnumerable<IGrouping<int, string>>), new Dictionary<Type, Type>
            {
                { typeof(T), typeof(string) }
            });
        }

        [TestMethod]
        public void IsReferenceAssignableFrom_Unification_Failure()
        {
            AssertIsNotReferenceAssignableFrom(typeof(Func<T, T>), typeof(Func<string, int>));
            AssertIsNotReferenceAssignableFrom(typeof(Func<T, T>), typeof(Func<int, string>));
        }

        [TestMethod]
        public void IsReferenceAssignableFrom_Unification_Invariant()
        {
            AssertIsReferenceAssignableFrom(typeof(Expression<Func<T, bool>>), typeof(Expression<Func<int, bool>>), new Dictionary<Type, Type>
            {
                { typeof(T), typeof(int) }
            });
        }

        [TestMethod]
        public void IsReferenceAssignableFrom_Unification_InvariantNegative()
        {
            AssertIsNotReferenceAssignableFrom(typeof(List<IEnumerable<T>>), typeof(List<List<T>>));
            AssertIsNotReferenceAssignableFrom(typeof(List<IEnumerable<T>>), typeof(List<IList<T>>));
            AssertIsNotReferenceAssignableFrom(typeof(List<IList<T>>), typeof(List<List<T>>));
            AssertIsNotReferenceAssignableFrom(typeof(List<List<T>>), typeof(List<IList<T>>));
            AssertIsNotReferenceAssignableFrom(typeof(List<List<T>>), typeof(List<string>));
            AssertIsNotReferenceAssignableFrom(typeof(List<string>), typeof(List<List<T>>));
            AssertIsNotReferenceAssignableFrom(typeof(List<List<List<T>>>), typeof(List<List<string>>));
            AssertIsNotReferenceAssignableFrom(typeof(List<List<string>>), typeof(List<List<List<T>>>));
        }

        [TestMethod]
        public void IsReferenceAssignableFrom_Unification_NoWildcardAssignments()
        {
            Assert.ThrowsException<InvalidOperationException>(() => AssertIsNotReferenceAssignableFrom(typeof(T), typeof(R)));
        }

        [TestMethod]
        public void IsReferenceAssignableFrom_ErrorReporting()
        {
            AssertIsNotReferenceAssignableFrom(typeof(object), typeof(int), "value types have no reference conversion");
        }

        [TestMethod]
        public void IsReferenceAssignableFrom_Unification_Struct()
        {
            AssertIsReferenceAssignableFrom(typeof(Foo<T>), typeof(Foo<int>), new Dictionary<Type, Type>
            {
                { typeof(T), typeof(int) }
            });

            AssertIsReferenceAssignableFrom(typeof(Foo<T[]>), typeof(Foo<int[]>), new Dictionary<Type, Type>
            {
                { typeof(T), typeof(int) }
            });

            AssertIsReferenceAssignableFrom(typeof(Foo<T>[]), typeof(Foo<int>[]), new Dictionary<Type, Type>
            {
                { typeof(T), typeof(int) }
            });

            AssertIsReferenceAssignableFrom(typeof(List<Foo<IEnumerable<T>>>), typeof(List<Foo<IEnumerable<int>>>), new Dictionary<Type, Type>
            {
                { typeof(T), typeof(int) }
            });

            AssertIsReferenceAssignableFrom(typeof(Foo<Foo<T>>), typeof(Foo<Foo<int>>), new Dictionary<Type, Type>
            {
                { typeof(T), typeof(int) }
            });
        }

        private static void AssertIsReferenceAssignableFrom(Type target, Type value, IDictionary<Type, Type> unifications = null)
        {
            unifications ??= new Dictionary<Type, Type>();

            var res = target.IsReferenceAssignableFrom(value, out IDictionary<Type, Type> subst);
            Assert.IsTrue(res, "{0} <: {1}", target, value);

            Assert.AreEqual(unifications.Count, subst.Count);
            Assert.IsTrue(new HashSet<Type>(unifications.Keys).SetEquals(new HashSet<Type>(subst.Keys)));

            foreach (var kv in unifications)
            {
                Assert.AreSame(kv.Value, subst[kv.Key]);
            }
        }

        private static void AssertIsNotReferenceAssignableFrom(Type target, Type value, string errorSubstring = null)
        {
            var res = target.IsReferenceAssignableFrom(value, out IDictionary<Type, Type> ignored);
            Assert.IsFalse(res, "{0} !<: {1}", target, value);

            if (errorSubstring != null)
            {
                AssertEx.ThrowsException<InvalidOperationException>(() => target.IsReferenceAssignableFrom(value, out ignored, throwException: true), ex => ex.Message.Contains(errorSubstring));
            }
        }
    }

    internal struct Foo<T>
    {
    }
    #endregion
}
