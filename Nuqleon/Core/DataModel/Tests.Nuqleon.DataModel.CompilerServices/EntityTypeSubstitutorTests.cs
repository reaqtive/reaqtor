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
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

using Nuqleon.DataModel;
using Nuqleon.DataModel.CompilerServices;
using Nuqleon.DataModel.TypeSystem;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Nuqleon.DataModel.CompilerServices
{
    [TestClass]
    public class EntityTypeSubstitutorTests
    {
        [TestMethod]
        public void EntityTypeSubstitutor_ArgumentChecking()
        {
            var subst = new MyEntityTypeSubstitutor();
            subst.Test();
        }

        [TestMethod]
        public void EntityTypeSubstitutor_ResolutionFailures()
        {
            var eta = new ExpressionEntityTypeAnonymizer();
            var etr = new ExpressionEntityTypeRecordizer();

            var e1 = (Expression<Func<F>>)(() => new F(new Grmbl()));
            var e2 = (Expression<Func<F, Grmbl>>)((F f) => f.GrmblProperty);
            var e3 = (Expression<Func<F, Grmbl>>)((F f) => f.GrmblMethod());
            var e4 = (Expression<Func<F, Grmbl>>)((F f) => f.grmblMember);
            var e5 = (Expression<Func<F, Grmbl>>)((F f) => f[0]);

            foreach (var s in new ExpressionEntityTypeSubstitutor[] { eta, etr })
            {
                foreach (var e in new Expression[] { e1, e2, e3, e4, e5 })
                {
                    AssertEx.ThrowsException<InvalidOperationException>(() => s.Apply(e), ex => Assert.IsTrue(ex.Message.Contains("Grmbl")));
                }
            }
        }

        [TestMethod]
        public void EntityTypeSubstitutor_ResolutionFailureInKnownTypeConstructor()
        {
            // Grmbl is not a `KnownType`; non-KTs are disallowed due to type erasure.
            var eta = new ExpressionEntityTypeAnonymizer();
            var etr = new ExpressionEntityTypeRecordizer();

            var e1 = (Expression<Func<KT>>)(() => new KT(new Grmbl()));
            var e2 = (Expression<Func<KT, Grmbl>>)((KT kt) => kt.GrmblProperty);
            var e3 = (Expression<Func<KT, Grmbl>>)((KT kt) => kt.GrmblMethod());
            var e4 = (Expression<Func<KT, Grmbl>>)((KT kt) => kt.grmblMember);
            var e5 = (Expression<Func<KT, Grmbl>>)((KT kt) => kt[0]);

            foreach (var s in new ExpressionEntityTypeSubstitutor[] { eta, etr })
            {
                foreach (var e in new Expression[] { e1, e2, e3, e4, e5 })
                {
                    AssertEx.ThrowsException<InvalidOperationException>(() => s.Apply(e), ex => Assert.IsTrue(ex.Message.Contains("Grmbl")));
                }
            }
        }

        [TestMethod]
        public void EntityTypeSubstitutor_KnownTypeResolutionSuccess()
        {
            // KT is a `KnownType`; KT rewriting should be successful.
            var eta = new ExpressionEntityTypeAnonymizer();
            var etr = new ExpressionEntityTypeRecordizer();

            var e1 = (Expression<Func<F>>)(() => new F(new KT()));
            var e2 = (Expression<Func<F, KT>>)((F f) => f.KTProperty);
            var e3 = (Expression<Func<F, KT>>)((F f) => f.KTMethod());
            var e4 = (Expression<Func<F, KT>>)((F f) => f.ktMember);
            var e5 = (Expression<Func<F, KT>>)((F f) => f[0.0]);

            foreach (var s in new ExpressionEntityTypeSubstitutor[] { eta, etr })
            {
                foreach (var e in new Expression[] { e1, e2, e3, e4, e5 })
                {
                    var rewr = s.Apply(e);  // no exception, thus rewriting was successful
                }
            }
        }

        [TestMethod]
        public void EntityTypeSubstitutor_NestedKnownTypeResolutionSuccess()
        {
            // KT is a `KnownType`; KT rewriting should be successful.
            var eta = new ExpressionEntityTypeAnonymizer();
            var etr = new ExpressionEntityTypeRecordizer();

            var e1 = (Expression<Func<KT>>)(() => new KT(new KT()));

            foreach (var s in new ExpressionEntityTypeSubstitutor[] { eta, etr })
            {
                foreach (var e in new Expression[] { e1 })
                {
                    Assert.AreSame(e, s.Apply(e));  // no exception, thus rewriting was successful
                }
            }
        }

        [TestMethod]
        public void EntityTypeSubstitutor_NestedKnownTypeResolutionFailure()
        {
            // Grmbl is not a `KnownType`; non-KTs are disallowed due to type erasure.
            var eta = new ExpressionEntityTypeAnonymizer();
            var etr = new ExpressionEntityTypeRecordizer();

            var e1 = (Expression<Func<KT>>)(() => new KT(new KT(new Grmbl())));
            var e2 = (Expression<Func<KT, Grmbl>>)((KT kt) => kt._kt._grmbl);

            foreach (var s in new ExpressionEntityTypeSubstitutor[] { eta, etr })
            {
                foreach (var e in new Expression[] { e1, e2 })
                {
                    // rewrite unsuccessful
                    AssertEx.ThrowsException<InvalidOperationException>(() => s.Apply(e), ex => Assert.IsTrue(ex.Message.Contains("Grmbl")));
                }
            }
        }

        [TestMethod]
        public void EntityTypeSubstitutor_MemberMemberBindingSuccess()
        {
            // Member member binding with `KnownType`s should result in no rewrite.
            var eta = new ExpressionEntityTypeAnonymizer();
            var etr = new ExpressionEntityTypeRecordizer();

            var e1 = (Expression<Func<F>>)(() => new F(new KT { KTProperty = { KTProperty = { X = 1 } } }));

            foreach (var s in new ExpressionEntityTypeSubstitutor[] { eta, etr })
            {
                foreach (var e in new Expression[] { e1 })
                {
                    Assert.AreSame(e, s.Apply(e));  // no exception, thus rewriting was successful
                }
            }
        }

        [TestMethod]
        public void EntityTypeSubstitutor_MemberMemberBindingNonKTSuccess()
        {
            // `Grmbl` is not attributed with `KnownType`, but the expression hook does not pick it up
            // in the member member bindings. Expression is thus rewritten successfully. Note this
            // fails if you use the `Grmbl` constructor instead.
            var eta = new ExpressionEntityTypeAnonymizer();
            var etr = new ExpressionEntityTypeRecordizer();

            var e1 = (Expression<Func<F>>)(() => new F(new KT { GrmblProperty = { X = 1 } }));

            foreach (var s in new ExpressionEntityTypeSubstitutor[] { eta, etr })
            {
                Assert.AreSame(e1, s.Apply(e1));
            }
        }

        private class MyEntityTypeSubstitutor : EntityTypeSubstitutor
        {
            public MyEntityTypeSubstitutor()
                : base(new Dictionary<Type, Type>())
            {
            }

            public void Test()
            {
                AssertEx.ThrowsException<ArgumentNullException>(() => base.Apply(expression: null), ex => Assert.AreEqual("expression", ex.ParamName));

                var prop = (PropertyInfo)ReflectionHelpers.InfoOf(() => DateTime.Now);

                AssertEx.ThrowsException<ArgumentNullException>(() => base.ResolveProperty(originalProperty: null, typeof(int), typeof(int), new[] { typeof(int) }), ex => Assert.AreEqual("originalProperty", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.ResolveProperty(prop, declaringType: null, typeof(int), new[] { typeof(int) }), ex => Assert.AreEqual("declaringType", ex.ParamName));
                AssertEx.ThrowsException<ArgumentNullException>(() => base.ResolveProperty(prop, typeof(int), propertyType: null, new[] { typeof(int) }), ex => Assert.AreEqual("propertyType", ex.ParamName));

                var anon1 = new { a = 1 };
                var anon2 = new { a = 1, b = 2 };
                var atyp1 = (StructuralDataType)DataType.FromType(anon1.GetType());
                var atyp2 = (StructuralDataType)DataType.FromType(anon2.GetType());

                Assert.ThrowsException<InvalidOperationException>(() => base.ConvertConstantStructuralAnonymous(anon1, atyp1, atyp2));

                var tupl1 = new Tuple<int>(1);
                var tupl2 = new Tuple<int, int>(1, 2);
                var ttyp1 = (StructuralDataType)DataType.FromType(tupl1.GetType());
                var ttyp2 = (StructuralDataType)DataType.FromType(tupl2.GetType());

                Assert.ThrowsException<InvalidOperationException>(() => base.ConvertConstantStructuralTuple(tupl1, ttyp1, ttyp2));

                var rcrt1 = RuntimeCompiler.CreateRecordType(new[] { new KeyValuePair<string, Type>("a", typeof(int)) }, valueEquality: true);
                var rcrt2 = RuntimeCompiler.CreateRecordType(new[] { new KeyValuePair<string, Type>("a", typeof(int)), new KeyValuePair<string, Type>("b", typeof(int)) }, valueEquality: true);
                var rtyp1 = (StructuralDataType)DataType.FromType(rcrt1);
                var rtyp2 = (StructuralDataType)DataType.FromType(rcrt2);
                var rcrd1 = Activator.CreateInstance(rcrt1);

                Assert.ThrowsException<InvalidOperationException>(() => base.ConvertConstantStructuralRecord(rcrd1, rtyp1, rtyp2));

                var func1 = new Func<int>(() => 1);
                var func2 = new Func<int, int>(x => x);
                var ftyp1 = (FunctionDataType)DataType.FromType(func1.GetType());
                var ftyp2 = (FunctionDataType)DataType.FromType(func2.GetType());

                Assert.ThrowsException<InvalidOperationException>(() => base.ConvertConstantFunction(func1, ftyp1, ftyp2));
            }

            protected override Expression CreateNewExpression(Type type, IDictionary<MemberInfo, Expression> memberAssignments)
            {
                throw new NotImplementedException();
            }

            protected override object ConvertConstantStructuralCore(object originalValue, StructuralDataType oldDataType, StructuralDataType newDataType)
            {
                throw new NotImplementedException();
            }
        }

#pragma warning disable IDE0060 // Remove unused parameter (test code)
        private class F
        {
            public F()
            {
            }

            public F(Grmbl g)
            {
            }

            public F(KT kt)
            {
            }

            public Grmbl this[int x] => throw new NotImplementedException();

            public Grmbl GrmblProperty { get; set; }

            public Grmbl GrmblMethod()
            {
                throw new NotImplementedException();
            }

            public Grmbl grmblMember = null;

            public KT this[double d] => throw new NotImplementedException();

            public KT KTProperty { get; set; }

            public KT KTMethod()
            {
                throw new NotImplementedException();
            }

            public KT ktMember = null;
        }

        [KnownType]
        private class KT
        {
            public KT()
            {
            }

            public KT(Grmbl g)
            {
                _grmbl = g;
            }

            public KT(KT kt)
            {
                _kt = kt;
            }

            public Grmbl this[int x] => throw new NotImplementedException();

            public Grmbl GrmblProperty { get; set; }

            public Grmbl GrmblMethod() { throw new NotImplementedException(); }

            public Grmbl grmblMember = null;

            public Grmbl _grmbl;

            public KT this[double d] => throw new NotImplementedException();

            public KT KTProperty { get; set; }

            public KT KTMethod()
            {
                throw new NotImplementedException();
            }

            public KT ktMember = null;

            public KT _kt;

            public int X { get; set; }
        }

        private class Grmbl
        {
            [Mapping("x")]
            public int X { get; set; }
        }
#pragma warning restore IDE0060 // Remove unused parameter
    }
}
