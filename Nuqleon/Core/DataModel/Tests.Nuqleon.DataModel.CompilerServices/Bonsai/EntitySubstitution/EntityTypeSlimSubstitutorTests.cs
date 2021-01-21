// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER, BD - July 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Nuqleon.DataModel;
using Nuqleon.DataModel.CompilerServices.Bonsai;
using Nuqleon.DataModel.TypeSystem;

namespace Tests.Nuqleon.DataModel.CompilerServices
{
    [TestClass]
    public class EntityTypeSlimSubstitutorTests
    {
        [TestMethod]
        public void EntityTypeSlimSubstitutor_ConstantConvert_Entity()
        {
            AssertRewrite(new MyPerson { Name = "Bart", Age = 21 }, StructuralTypeSlimKind.Record);
        }

        [TestMethod]
        public void EntityTypeSlimSubstitutor_ConstantConvert_Anonymous()
        {
#pragma warning disable IDE0050 // Convert to tuple. (Test for anonymous types.)
            AssertRewrite(new { name = "Bart", age = 21 }, StructuralTypeSlimKind.Record);
#pragma warning restore IDE0050
        }

        [TestMethod]
        public void EntityTypeSlimSubstitutor_ConstantConvert_Record()
        {
            var rec = RuntimeCompiler.CreateRecordType(new KeyValuePair<string, Type>[] { new("name", typeof(string)), new("age", typeof(int)) }, valueEquality: true);
            var obj = Activator.CreateInstance(rec);
            rec.GetProperty("name").SetValue(obj, "Bart");
            rec.GetProperty("age").SetValue(obj, 21);
            AssertRewrite(obj, StructuralTypeSlimKind.Anonymous);
        }

        private static void AssertRewrite(object value, StructuralTypeSlimKind targetKind)
        {
            var expr = Expression.Constant(value);

            var oldType = expr.Type.ToTypeSlim();

            var newType = StructuralTypeSlimReference.Create(hasValueEqualitySemantics: true, targetKind);

            var nameProp = newType.GetProperty("name", typeof(string).ToTypeSlim(), new ReadOnlyCollection<TypeSlim>(Array.Empty<TypeSlim>()), canWrite: true);
            newType.AddProperty(nameProp);

            var ageProp = newType.GetProperty("age", typeof(int).ToTypeSlim(), new ReadOnlyCollection<TypeSlim>(Array.Empty<TypeSlim>()), canWrite: true);
            newType.AddProperty(ageProp);

            newType.Freeze();

            //
            // NB: This is the internal mechanism used to trigger constant conversion. Without this facility, the
            //     constants are kept as-is, under the assumption that future rewrite steps do understand the value
            //     equivalence between the original Type and the resulting structural TypeSlim (which can save a
            //     whole lot of allocations). This test exercises the case where deep conversion takes place.
            //

            newType.CarryType(expr.Type);

            var map = new Dictionary<TypeSlim, TypeSlim>
            {
                { oldType, newType }
            };

            var subst = new Subst(map);

            var res = subst.Apply(expr.ToExpressionSlim());

            Assert.IsTrue(res is ConstantExpressionSlim);
        }

        private sealed class Subst : EntityTypeSlimSubstitutor
        {
            public Subst(Dictionary<TypeSlim, TypeSlim> typeMap)
                : base(typeMap)
            {
            }

#if DEBUG
            protected override void CheckConstantStructuralCore(ObjectSlim newValue, StructuralDataType oldDataType) { }
#endif

            protected override object ConvertConstantStructuralCore(object originalValue, StructuralDataType oldDataType, StructuralDataType newDataType) => originalValue;

            protected override ExpressionSlim CreateNewExpression(TypeSlim type, IDictionary<MemberInfoSlim, ExpressionSlim> memberAssignments) => throw new NotImplementedException();
        }
    }

    public sealed class MyPerson
    {
        [Mapping("name")]
        public string Name { get; set; }

        [Mapping("age")]
        public int Age { get; set; }
    }
}
