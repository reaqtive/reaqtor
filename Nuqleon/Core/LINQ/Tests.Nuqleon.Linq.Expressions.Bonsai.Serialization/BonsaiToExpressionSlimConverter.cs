// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD, ER - July 2013 - Created this file.
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions.Bonsai.Serialization;
using System.Linq.Expressions;
using System.Reflection;
using Json = Nuqleon.Json.Expressions;

namespace Tests
{
    [TestClass]
    public class BonsaiToExpressionSlimConverterTests
    {
        #region Visit

        [TestMethod]
        public void BonsaiToExpressionSlimConverter_VisitConstant()
        {
            var deserializer = new TestBonsaiDeserializer(new SerializationState(BonsaiVersion.Default).ToJson());
            Assert.ThrowsException<BonsaiParseException>(() => deserializer.Visit(Json.Expression.String("foo")));
        }

        [TestMethod]
        public void BonsaiToExpressionSlimConverter_VisitObject()
        {
            var deserializer = new TestBonsaiDeserializer(new SerializationState(BonsaiVersion.Default).ToJson());
            Assert.ThrowsException<BonsaiParseException>(() => deserializer.Visit(Json.Expression.Object(new Dictionary<string, Json.Expression>())));
        }

        [TestMethod]
        public void BonsaiToExpressionSlimConverter_Visit_EmptyArray()
        {
            var deserializer = new TestBonsaiDeserializer(new SerializationState(BonsaiVersion.Default).ToJson());
            Assert.ThrowsException<BonsaiParseException>(() => deserializer.Visit(Json.Expression.Array(Array.Empty<Json.Expression>())));
        }

        [TestMethod]
        public void BonsaiToExpressionSlimConverter_Visit_NoDiscriminator()
        {
            var p0 = Expression.Parameter(typeof(int[]));
            var p1 = Expression.Parameter(typeof(int));

            var exprs = new Expression[]
            {
                Expression.ArrayIndex(p0, p1),
            };

            OneWayParseException(exprs, typeof(MissingDescriptorBonsaiDeserializer).GetConstructors().Single());
        }

        [TestMethod]
        public void BonsaiToExpressionSlimConverter_Visit_InvalidDiscriminator()
        {
            var p0 = Expression.Parameter(typeof(int[]));
            var p1 = Expression.Parameter(typeof(int));

            var exprs = new Expression[]
            {
                Expression.ArrayIndex(p0, p1),
            };

            OneWayParseException(exprs, typeof(InvalidDescriptorBonsaiDeserializer).GetConstructors().Single());
        }

        private sealed class MissingDescriptorBonsaiDeserializer : TestBonsaiDeserializer
        {
            public MissingDescriptorBonsaiDeserializer(Json.Expression state)
                : base(state)
            {
            }

            public override ExpressionSlim VisitArray(Json.ArrayExpression node)
            {
                var newElements = new Json.Expression[node.Elements.Count];
                node.Elements.CopyTo(newElements, 0);
                newElements[0] = Json.Expression.Null();
                var newNode = Json.Expression.Array(newElements);
                return base.VisitArray(newNode);
            }
        }

        private sealed class InvalidDescriptorBonsaiDeserializer : TestBonsaiDeserializer
        {
            public InvalidDescriptorBonsaiDeserializer(Json.Expression state)
                : base(state)
            {
            }

            public override ExpressionSlim VisitArray(Json.ArrayExpression node)
            {
                var newElements = new Json.Expression[node.Elements.Count];
                node.Elements.CopyTo(newElements, 0);
                newElements[0] = Json.Expression.String("foo");
                var newNode = Json.Expression.Array(newElements);
                return base.VisitArray(newNode);
            }
        }

        #endregion

        #region VisitX Empty Arrays

        [TestMethod]
        public void BonsaiToExpressionSlimConverter_VisitX_IncorrectJsonArrayLength()
        {
            var p0 = Expression.Parameter(typeof(int[]));
            var p1 = Expression.Parameter(typeof(int));
            var p2 = Expression.Parameter(typeof(int));
            var p3 = Expression.Parameter(typeof(bool));
            var p4 = Expression.Parameter(typeof(string));
            var p5 = Expression.Parameter(typeof(string));
            var p6 = Expression.Parameter(typeof(List<int>));
            var m1 = typeof(List<int>).GetMethod("Add");
            var prop1 = typeof(List<int>).GetProperty("Count");

            var exprs = new Expression[]
            {
                Expression.ArrayIndex(p0, p1),
                Expression.LessThan(p1, p2),
                Expression.Multiply(p1, p2),
                Expression.Coalesce(p4, p5),
                Expression.Condition(p3, p1, p2),
                Expression.Constant(0, typeof(int)),
                Expression.Convert(p1, typeof(int)),
                Expression.Default(typeof(int)),
                //Expression.ElementInit(m1, p1, p2),
                Expression.MakeIndex(p6, prop1, Array.Empty<Expression>()),
                Expression.Invoke(Expression.Lambda(p1, p1), p1),
                Expression.MakeMemberAccess(p6, prop1),
                //Expression.MemberBind(prop1, new MemberBinding[0]),
                //Expression.ListBind(prop1, new ElementInit[0]),
                Expression.Call(p6, m1, p1),
                Expression.Negate(p1),
                Expression.NegateChecked(p1),
                Expression.New(typeof(List<int>)),
                Expression.NewArrayBounds(typeof(int[]), p1),
                Expression.NewArrayInit(typeof(int[])),
                Expression.Add(p1, p2),
                Expression.AddChecked(p1, p2),
                Expression.Quote(Expression.Lambda(p1, p1)),
                Expression.TypeAs(p4, typeof(string)),
                Expression.TypeIs(p1, typeof(int)),
                Expression.Not(p3)
            };

            OneWayParseException(exprs, typeof(EmptyArraysBonsaiDeserializer).GetConstructors().Single());
        }

        private sealed class EmptyArraysBonsaiDeserializer : TestBonsaiDeserializer
        {
            public EmptyArraysBonsaiDeserializer(Json.Expression state)
                : base(state)
            {
            }

            protected override ExpressionSlim VisitArrayIndexExpression(Json.ArrayExpression node)
            {
                var newNode = Json.Expression.Array(Array.Empty<Json.Expression>());
                return base.VisitArrayIndexExpression(newNode);
            }
            protected override ExpressionSlim VisitBinaryComparisonExpression(Json.ArrayExpression node, ExpressionType nodeType)
            {
                var newNode = Json.Expression.Array(Array.Empty<Json.Expression>());
                return base.VisitBinaryComparisonExpression(newNode, nodeType);
            }

            protected override ExpressionSlim VisitBinaryExpression(Json.ArrayExpression node, ExpressionType nodeType)
            {
                var newNode = Json.Expression.Array(Array.Empty<Json.Expression>());
                return base.VisitBinaryExpression(newNode, nodeType);
            }

            protected override ExpressionSlim VisitCoalesceExpression(Json.ArrayExpression node)
            {
                var newNode = Json.Expression.Array(Array.Empty<Json.Expression>());
                return base.VisitCoalesceExpression(newNode);
            }

            protected override ExpressionSlim VisitConditionalExpression(Json.ArrayExpression node)
            {
                var newNode = Json.Expression.Array(Array.Empty<Json.Expression>());
                return base.VisitConditionalExpression(newNode);
            }

            protected override ExpressionSlim VisitConstantExpression(Json.ArrayExpression node)
            {
                var newNode = Json.Expression.Array(Array.Empty<Json.Expression>());
                return base.VisitConstantExpression(newNode);
            }

            protected override ExpressionSlim VisitConvertExpression(Json.ArrayExpression node, bool isChecked)
            {
                var newNode = Json.Expression.Array(Array.Empty<Json.Expression>());
                return base.VisitConvertExpression(newNode, isChecked);
            }

            protected override ExpressionSlim VisitDefaultExpression(Json.ArrayExpression node)
            {
                var newNode = Json.Expression.Array(Array.Empty<Json.Expression>());
                return base.VisitDefaultExpression(newNode);
            }

            protected override ElementInitSlim VisitElementInit(Json.Expression node)
            {
                var newNode = Json.Expression.Array(Array.Empty<Json.Expression>());
                return base.VisitElementInit(newNode);
            }

            protected override ExpressionSlim VisitIndexExpression(Json.ArrayExpression node)
            {
                var newNode = Json.Expression.Array(Array.Empty<Json.Expression>());
                return base.VisitIndexExpression(newNode);
            }

            protected override ExpressionSlim VisitInvocationExpression(Json.ArrayExpression node)
            {
                var newNode = Json.Expression.Array(Array.Empty<Json.Expression>());
                return base.VisitInvocationExpression(newNode);
            }

            protected override ExpressionSlim VisitLambdaExpression(Json.ArrayExpression node)
            {
                var newNode = Json.Expression.Array(Array.Empty<Json.Expression>());
                return base.VisitLambdaExpression(newNode);
            }

            protected override ExpressionSlim VisitMemberAccessExpression(Json.ArrayExpression node)
            {
                var newNode = Json.Expression.Array(Array.Empty<Json.Expression>());
                return base.VisitMemberAccessExpression(newNode);
            }

            protected override MemberBindingSlim VisitMemberAssignment(Json.ArrayExpression node)
            {
                var newNode = Json.Expression.Array(Array.Empty<Json.Expression>());
                return base.VisitMemberAssignment(newNode);
            }

            protected override ExpressionSlim VisitMemberInitExpression(Json.ArrayExpression node)
            {
                var newNode = Json.Expression.Array(Array.Empty<Json.Expression>());
                return base.VisitMemberInitExpression(newNode);
            }

            protected override MemberBindingSlim VisitMemberListBinding(Json.ArrayExpression node)
            {
                var newNode = Json.Expression.Array(Array.Empty<Json.Expression>());
                return base.VisitMemberListBinding(newNode);
            }

            protected override MemberBindingSlim VisitMemberMemberBinding(Json.ArrayExpression node)
            {
                var newNode = Json.Expression.Array(Array.Empty<Json.Expression>());
                return base.VisitMemberMemberBinding(newNode);
            }

            protected override ExpressionSlim VisitMethodCallExpression(Json.ArrayExpression node)
            {
                var newNode = Json.Expression.Array(Array.Empty<Json.Expression>());
                return base.VisitMethodCallExpression(newNode);
            }

            protected override ExpressionSlim VisitMinusDollarExpression(Json.ArrayExpression node)
            {
                var newNode = Json.Expression.Array(Array.Empty<Json.Expression>());
                return base.VisitMinusDollarExpression(newNode);
            }

            protected override ExpressionSlim VisitMinusExpression(Json.ArrayExpression node)
            {
                var newNode = Json.Expression.Array(Array.Empty<Json.Expression>());
                return base.VisitMinusExpression(newNode);
            }

            protected override ExpressionSlim VisitNewArrayBoundsExpression(Json.ArrayExpression node)
            {
                var newNode = Json.Expression.Array(Array.Empty<Json.Expression>());
                return base.VisitNewArrayBoundsExpression(newNode);
            }

            protected override ExpressionSlim VisitNewArrayInitExpression(Json.ArrayExpression node)
            {
                var newNode = Json.Expression.Array(Array.Empty<Json.Expression>());
                return base.VisitNewArrayInitExpression(newNode);
            }

            protected override ExpressionSlim VisitNewExpression(Json.ArrayExpression node)
            {
                var newNode = Json.Expression.Array(Array.Empty<Json.Expression>());
                return base.VisitNewExpression(newNode);
            }

            protected override ExpressionSlim VisitParameterExpression(Json.ArrayExpression node)
            {
                var newNode = Json.Expression.Array(Array.Empty<Json.Expression>());
                return base.VisitParameterExpression(newNode);
            }

            protected override ExpressionSlim VisitPlusDollarExpression(Json.ArrayExpression node)
            {
                var newNode = Json.Expression.Array(Array.Empty<Json.Expression>());
                return base.VisitPlusDollarExpression(newNode);
            }

            protected override ExpressionSlim VisitPlusExpression(Json.ArrayExpression node)
            {
                var newNode = Json.Expression.Array(Array.Empty<Json.Expression>());
                return base.VisitPlusExpression(newNode);
            }

            protected override ExpressionSlim VisitQuoteExpression(Json.ArrayExpression node)
            {
                var newNode = Json.Expression.Array(Array.Empty<Json.Expression>());
                return base.VisitQuoteExpression(newNode);
            }

            protected override ExpressionSlim VisitTypeAsExpression(Json.ArrayExpression node)
            {
                var newNode = Json.Expression.Array(Array.Empty<Json.Expression>());
                return base.VisitTypeAsExpression(newNode);
            }

            protected override ExpressionSlim VisitTypeIsExpression(Json.ArrayExpression node)
            {
                var newNode = Json.Expression.Array(Array.Empty<Json.Expression>());
                return base.VisitTypeIsExpression(newNode);
            }

            protected override ExpressionSlim VisitUnaryExpression(Json.ArrayExpression node, ExpressionType nodeType)
            {
                var newNode = Json.Expression.Array(Array.Empty<Json.Expression>());
                return base.VisitUnaryExpression(newNode, nodeType);
            }
        }

        #endregion

        #region VisitMemberBinding

        [TestMethod]
        public void BonsaiToExpressionSlimConverter_Visit_InvalidMemberBindings()
        {
            var p1 = Expression.Parameter(typeof(int));
            var prop1 = typeof(A).GetProperty("B");

            var exprs = new[] {
                Expression.MemberInit(Expression.New(typeof(A)), Expression.Bind(prop1, p1))
            };

            OneWayParseException(exprs, typeof(InvalidMemberBindingBonsaiDeserializer1).GetConstructors().Single());
            OneWayParseException(exprs, typeof(InvalidMemberBindingBonsaiDeserializer2).GetConstructors().Single());
            OneWayParseException(exprs, typeof(InvalidMemberBindingBonsaiDeserializer3).GetConstructors().Single());
            OneWayParseException(exprs, typeof(InvalidMemberBindingBonsaiDeserializer4).GetConstructors().Single());
        }

        private sealed class A
        {
            public int B { get; set; }
        }

        private sealed class InvalidMemberBindingBonsaiDeserializer1 : TestBonsaiDeserializer
        {
            public InvalidMemberBindingBonsaiDeserializer1(Json.Expression state)
                : base(state)
            {
            }

            protected override MemberBindingSlim VisitMemberBinding(Json.Expression node)
            {
                return base.VisitMemberBinding(Json.Expression.Object(new Dictionary<string, Json.Expression>()));
            }
        }

        private sealed class InvalidMemberBindingBonsaiDeserializer2 : TestBonsaiDeserializer
        {
            public InvalidMemberBindingBonsaiDeserializer2(Json.Expression state)
                : base(state)
            {
            }

            protected override MemberBindingSlim VisitMemberBinding(Json.Expression node)
            {
                return base.VisitMemberBinding(Json.Expression.Array(Array.Empty<Json.Expression>()));
            }
        }

        private sealed class InvalidMemberBindingBonsaiDeserializer3 : TestBonsaiDeserializer
        {
            public InvalidMemberBindingBonsaiDeserializer3(Json.Expression state)
                : base(state)
            {
            }

            protected override MemberBindingSlim VisitMemberBinding(Json.Expression node)
            {
                return base.VisitMemberBinding(Json.Expression.Array(new[] { Json.Expression.Number("0") }));
            }
        }

        private sealed class InvalidMemberBindingBonsaiDeserializer4 : TestBonsaiDeserializer
        {
            public InvalidMemberBindingBonsaiDeserializer4(Json.Expression state)
                : base(state)
            {
            }

            protected override MemberBindingSlim VisitMemberBinding(Json.Expression node)
            {
                return base.VisitMemberBinding(Json.Expression.Array(new[] { Json.Expression.String("foo") }));
            }
        }

        #endregion

        #region VisitNew

        [TestMethod]
        public void BonsaiToExpressionSlimConverter_Visit_InvalidNewExpression()
        {
            var p = Expression.Parameter(typeof(int));
            var prop1 = typeof(Tuple<int>).GetProperty("Item1");

            var exprs = new[] {
                Expression.New(typeof(Tuple<int>).GetConstructors().Single(), new[] { p }, new MemberInfo[] { prop1 })
            };

            OneWayParseException(exprs, typeof(InvalidNewExpressionBonsaiDeserializer1).GetConstructors().Single());
            OneWayParseException(exprs, typeof(InvalidNewExpressionBonsaiDeserializer2).GetConstructors().Single());
        }

        private sealed class InvalidNewExpressionBonsaiDeserializer1 : TestBonsaiDeserializer
        {
            public InvalidNewExpressionBonsaiDeserializer1(Json.Expression state)
                : base(state)
            {
            }

            protected override ExpressionSlim VisitNewExpression(Json.ArrayExpression node)
            {
                var newElements = new Json.Expression[node.Elements.Count];
                node.Elements.CopyTo(newElements, 0);
                newElements[2] = Json.Expression.Null();
                var newNode = Json.Expression.Array(newElements);
                return base.VisitNewExpression(newNode);
            }
        }

        private sealed class InvalidNewExpressionBonsaiDeserializer2 : TestBonsaiDeserializer
        {
            public InvalidNewExpressionBonsaiDeserializer2(Json.Expression state)
                : base(state)
            {
            }

            protected override ExpressionSlim VisitNewExpression(Json.ArrayExpression node)
            {
                var newElements = new Json.Expression[node.Elements.Count];
                node.Elements.CopyTo(newElements, 0);
                newElements[3] = Json.Expression.Null();
                var newNode = Json.Expression.Array(newElements);
                return base.VisitNewExpression(newNode);
            }
        }

        #endregion

        private static void OneWayParseException(IEnumerable<Expression> expressions, ConstructorInfo deserializerCtor)
        {
            var state = new SerializationState(BonsaiVersion.Default);
            var serializer = new TestBonsaiSerializer(state);
            var slimifier = new ExpressionToExpressionSlimConverter();

            foreach (var expr in expressions)
            {
                var slim = slimifier.Visit(expr);
                var json = serializer.Visit(slim);
                var deserializer = (BonsaiToExpressionSlimConverter)deserializerCtor.Invoke(new object[] { state.ToJson() });
                Assert.ThrowsException<BonsaiParseException>(() => deserializer.Visit(json));
            }
        }
    }

    internal class TestBonsaiSerializer : ExpressionSlimToBonsaiConverter
    {
        public TestBonsaiSerializer(SerializationState state)
            : base(state)
        {
        }

        protected override Json.Expression VisitConstantValue(ObjectSlim value)
        {
            return Json.Expression.String("0");
        }
    }

    internal class TestBonsaiDeserializer : BonsaiToExpressionSlimConverter
    {
        public TestBonsaiDeserializer(Json.Expression state)
            : base(new DeserializationState(state, BonsaiVersion.Default))
        {
        }

        protected override ObjectSlim VisitConstantValue(Json.Expression value, TypeSlim type)
        {
            return null;
        }
    }

    internal sealed class InvalidIndexExpressionBonsaiDeserializer : TestBonsaiDeserializer
    {
        public InvalidIndexExpressionBonsaiDeserializer(Json.Expression state)
            : base(state)
        {
        }
    }
}
