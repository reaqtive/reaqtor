// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

using Nuqleon.DataModel;
using Nuqleon.DataModel.CompilerServices;
using Nuqleon.DataModel.TypeSystem;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Nuqleon.DataModel.CompilerServices
{
    [TestClass]
    public class AnonymizationTests
    {
        [TestMethod]
        public void AnonymizationTests_Suite()
        {
            var testers = new AnonymizeAndAssertBase[]
            {
                new AnonymizeAndAssert(),
                new RecordizeAndAssert(),
                new RecordizeSlimAndAssert(),
            };

            foreach (var tester in testers)
            {
                tester.AreStructurallyEqual(() => new KnownTypeType { A = 42 });
                tester.AreNotAnonymized(() => new KnownTypeType { A = 42 }, typeof(KnownTypeType));

                tester.AreStructurallyEqual(() => new KnownTypeType { A = 42 }.A);
                tester.AreNotAnonymized(() => new KnownTypeType { A = 42 }.A, typeof(KnownTypeType));

                tester.AreStructurallyEqual(() => new StructuralTypeWithKnownType { B = new KnownTypeType() });
                tester.IsAnonymized(() => new StructuralTypeWithKnownType { B = new KnownTypeType() }, new[] { typeof(KnownTypeType) }, new[] { typeof(StructuralTypeWithKnownType) });

                tester.AreStructurallyEqual(() => new StructuralTypeWithKnownType { B = new KnownTypeType() }.B);
                tester.IsAnonymized(() => new StructuralTypeWithKnownType { B = new KnownTypeType() }.B, new[] { typeof(KnownTypeType) }, new[] { typeof(StructuralTypeWithKnownType) });

                tester.AreStructurallyEqual(() => new StructuralTypeWithNonEntityEnum { E = NonEntityEnum.X });
                tester.IsAnonymized(() => new StructuralTypeWithNonEntityEnum { E = NonEntityEnum.X }, new[] { typeof(NonEntityEnum) }, new[] { typeof(StructuralTypeWithNonEntityEnum) });

                tester.AreStructurallyEqual(() => new StructuralTypeWithNonEntityEnum { E = NonEntityEnum.X }.E);
                tester.IsAnonymized(() => new StructuralTypeWithNonEntityEnum { E = NonEntityEnum.X }.E, new[] { typeof(NonEntityEnum) }, new[] { typeof(StructuralTypeWithNonEntityEnum) });

                tester.AreStructurallyEqual(() => new StructuralTypeWithEntityEnum { F = EntityEnum.Y });
                tester.AreAnonymized(() => new StructuralTypeWithEntityEnum { F = EntityEnum.Y }, typeof(StructuralTypeWithEntityEnum), typeof(EntityEnum));

                tester.AreStructurallyEqual(() => new StructuralTypeWithEntityEnum { F = EntityEnum.Y }.F);
                tester.AreAnonymized(() => new StructuralTypeWithEntityEnum { F = EntityEnum.Y }.F, typeof(StructuralTypeWithEntityEnum), typeof(EntityEnum));
            }
        }

        #region DataModel Types

        [KnownType]
        public class KnownTypeType
        {
            [Mapping("A")]
            public int A { get; set; }
        }

        public class StructuralTypeWithKnownType
        {
            [Mapping("B")]
            public KnownTypeType B { get; set; }
        }

        public class StructuralTypeWithList
        {
            [Mapping("C")]
            public List<int> C { get; set; }
        }

        public class StructuralTypeWithArray
        {
            [Mapping("D")]
            public int[] D { get; set; }
        }

        public enum NonEntityEnum
        {
            X,
        }

        public enum EntityEnum
        {
            [Mapping("Y")]
            Y,
        }

        public class StructuralTypeWithNonEntityEnum
        {
            [Mapping("E")]
            public NonEntityEnum E { get; set; }
        }

        public class StructuralTypeWithEntityEnum
        {
            [Mapping("F")]
            public EntityEnum F { get; set; }
        }

        private class PrivateStructuralType
        {
            [Mapping("H")]
            public int H { get; set; }
        }

        internal class InternalStructuralType
        {
            [Mapping("I")]
            public int I { get; set; }
        }

        public class RecursiveStructuralType
        {
            [Mapping("J")]
            public RecursiveStructuralType J { get; set; }
        }

        public class InnerRecursiveStructuralType
        {
            [Mapping("Inner")]
            public LayeredRecursiveStructuralType Inner { get; set; }
        }

        public class LayeredRecursiveStructuralType
        {
            [Mapping("Outer")]
            public InnerRecursiveStructuralType Outer { get; set; }
        }

        public class GenericStructuralType<T>
        {
            [Mapping("Value")]
            public T Value { get; set; }
        }

        [KnownType]
        public class GenericKnownType<T>
        {
            [Mapping("KnownValue")]
            public T KnownValue { get; set; }
        }

        public class StructuralTypeWithExpression
        {
            [Mapping("Expression")]
            public Expression Expression { get; set; }
        }

        public class StructuralTypeWithField
        {
            [Mapping("M")]
            public int M;
        }

        public class StructuralTypeWithConstructor
        {
            public StructuralTypeWithConstructor([Mapping("N")] int n)
            {
                N = n;
            }

            [Mapping("N")]
            public int N { get; set; }
        }

        #endregion

        #region Assertions

        #region Equality

        private static void AssertStructurallyEqual(Expression x, Expression y)
        {
            Assert.IsTrue(new ExpressionComparator().Equals(x, y), "Expected: {0}{1}Actual: {2}", x.ToCSharpString(allowCompilerGeneratedNames: true), Environment.NewLine, y.ToCSharpString(allowCompilerGeneratedNames: true));
        }

        private class ExpressionComparator : ExpressionEqualityComparator
        {
            public ExpressionComparator()
                : this(new TypeComparator(), new ObjectComparator())
            {
            }

            private ExpressionComparator(TypeComparator typeComparator, ObjectComparator objectComparator)
                : base(typeComparator, typeComparator.MemberComparer, objectComparator, EqualityComparer<CallSiteBinder>.Default)
            {
                objectComparator.ExpressionComparer = this;
            }

            public override bool Equals(Expression x, Expression y)
            {
                if (x == null && y == null)
                {
                    return true;
                }

                if (x == null || y == null)
                {
                    return false;
                }

                if (x.NodeType != y.NodeType)
                {
                    if (x.NodeType == ExpressionType.MemberInit && y.NodeType == ExpressionType.New)
                    {
                        return Equals((MemberInitExpression)x, (NewExpression)y);
                    }
                    else if (x.NodeType == ExpressionType.New && y.NodeType == ExpressionType.MemberInit)
                    {
                        return Equals((MemberInitExpression)y, (NewExpression)x);
                    }
                }

                return base.Equals(x, y);
            }

            protected override bool EqualsNew(NewExpression x, NewExpression y)
            {
                if (x == null && y == null)
                {
                    return true;
                }

                if (x == null || y == null)
                {
                    return false;
                }

                return
                    Equals(x.Type, y.Type) && // Constructor can be null
                    Equals(x.Constructor, y.Constructor) &&
                    Equals(x.Arguments, y.Arguments) &&
                    EqualsMembers(x.Members, y.Members);

            }

            private static bool EqualsMembers(IReadOnlyList<MemberInfo> x, IReadOnlyList<MemberInfo> y)
            {
                if (x == null && y != null)
                {
                    return y.Count == 0;
                }

                if (y == null && x != null)
                {
                    return x.Count == 0;
                }

                return Equals(x, y);
            }

            private bool Equals(MemberInitExpression x, NewExpression y)
            {
                if (!Equals(x.Type, y.Type))
                {
                    return false;
                }

                var xPropertyMap = Populate(x);
                var yPropertyMap = Populate(y);

                return EqualsByKey(xPropertyMap, yPropertyMap, kv => kv.Key, (kv1, kv2) => Equals(kv1.Value, kv2.Value));
            }

            private static IDictionary<string, Expression> Populate(MemberInitExpression expression)
            {
                var dataType = (StructuralDataType)DataType.FromType(expression.Type, allowCycles: true);
                var memberBindings = dataType.Properties.ToDictionary(p => p.Name, p => (Expression)new UnassignedExpression(p.Type.UnderlyingType));

                foreach (var binding in expression.Bindings)
                {
                    switch (binding.BindingType)
                    {
                        case MemberBindingType.Assignment:
                            {
                                var mapping = binding.Member.GetCustomAttribute<MappingAttribute>(inherit: false);
                                var name = mapping != null ? mapping.Uri : binding.Member.Name;
                                Assign(memberBindings, name, ((MemberAssignment)binding).Expression);
                                break;
                            }
                        case MemberBindingType.ListBinding:
                            {
                                var mapping = binding.Member.GetCustomAttribute<MappingAttribute>(inherit: false);
                                var name = mapping != null ? mapping.Uri : binding.Member.Name;
                                Assign(
                                    memberBindings,
                                    name,
                                    Expression.ListInit(
                                        Expression.New(GetMemberType(binding.Member)),
                                        ((MemberListBinding)binding).Initializers
                                    )
                                );
                                break;
                            }
                        case MemberBindingType.MemberBinding:
                            {
                                var mapping = binding.Member.GetCustomAttribute<MappingAttribute>(inherit: false);
                                var name = mapping != null ? mapping.Uri : binding.Member.Name;
                                Assign(
                                    memberBindings,
                                    name,
                                    Expression.MemberInit(
                                        Expression.New(GetMemberType(binding.Member)),
                                        ((MemberMemberBinding)binding).Bindings
                                    )
                                );
                                break;
                            }
                        default:
                            throw new InvalidOperationException("Cannot extract mapped expression from binding.");
                    }
                }

                var reducer = new UnassignedExpressionReducer();
                return memberBindings.ToDictionary(kv => kv.Key, kv => reducer.Visit(kv.Value));
            }

            private static IDictionary<string, Expression> Populate(NewExpression expression)
            {
                var dataType = (StructuralDataType)DataType.FromType(expression.Type, allowCycles: true);
                var memberBindings = dataType.Properties.ToDictionary(p => p.Name, p => (Expression)new UnassignedExpression(p.Type.UnderlyingType));

                var parameters = expression.Constructor.GetParameters();
                for (var i = 0; i < expression.Arguments.Count; ++i)
                {
                    var parameter = parameters[i];
                    var name = default(string);
                    var mapping = parameter.GetCustomAttribute<MappingAttribute>(inherit: false);
                    if (mapping != null)
                    {
                        name = mapping.Uri;
                    }
                    else if (expression.Members.Count == expression.Arguments.Count)
                    {
                        mapping = expression.Members[i].GetCustomAttribute<MappingAttribute>(inherit: false);
                        name = mapping != null ? mapping.Uri : expression.Members[i].Name;
                    }
                    if (name == null)
                    {
                        throw new InvalidOperationException("Cannot determine parameter mapping.");
                    }

                    Assign(memberBindings, name, expression.Arguments[i]);
                }

                var reducer = new UnassignedExpressionReducer();
                return memberBindings.ToDictionary(kv => kv.Key, kv => reducer.Visit(kv.Value));
            }

            private static void Assign(IDictionary<string, Expression> dictionary, string key, Expression value)
            {
                if (!dictionary.TryGetValue(key, out var replaced) || !(replaced is UnassignedExpression))
                {
                    var replacedStr = replaced?.ToCSharpString(allowCompilerGeneratedNames: true);
                    throw new InvalidOperationException(string.Format("Invalid assignment of '{0}' with '{1}'.", key, replacedStr));
                }
                dictionary[key] = value;
            }

            private static Type GetMemberType(MemberInfo member)
            {
                return member.MemberType switch
                {
                    MemberTypes.Field => ((FieldInfo)member).FieldType,
                    MemberTypes.Property => ((PropertyInfo)member).PropertyType,
                    _ => throw new InvalidOperationException("Cannot determine member type."),
                };
            }

            private class TypeComparator : StructuralTypeEqualityComparator
            {
                private readonly IDictionary<Type, Type> _recursionMap;

                public TypeComparator()
                    : this(new MemberComparator())
                {
                }

                private TypeComparator(StructuralMemberInfoEqualityComparator memberComparer)
                    : base(memberComparer)
                {
                    memberComparer.TypeComparer = this;
                    _recursionMap = new Dictionary<Type, Type>();
                }

                protected override bool EqualsSimple(Type x, Type y)
                {
                    var result = base.EqualsSimple(x, y);

                    if (!result)
                    {
                        if (x.IsEnum && !y.IsEnum)
                        {
                            return Equals(x.GetEnumUnderlyingType(), y);
                        }
                        else if (!x.IsEnum && y.IsEnum)
                        {
                            return Equals(x, y.GetEnumUnderlyingType());
                        }
                    }

                    return result;
                }

                protected override bool EqualsStructural(Type x, Type y)
                {
                    if (_recursionMap.TryGetValue(x, out var recursedY))
                    {
                        return y == recursedY;
                    }
                    else
                    {
                        _recursionMap.Add(x, y);
                    }

                    try
                    {
                        var dataTypeX = (StructuralDataType)DataType.FromType(x, allowCycles: true);
                        var dataTypeY = (StructuralDataType)DataType.FromType(y, allowCycles: true);

                        if (EqualsByKey(dataTypeX.Properties, dataTypeY.Properties, p => p.Name, (p1, p2) => MemberComparer.Equals(p1.Property, p2.Property)))
                        {
                            return true;
                        }

                        return base.EqualsStructural(x, y);
                    }
                    finally
                    {
                        _recursionMap.Remove(x);
                    }
                }

                protected override bool AreStructurallyComparable(Type x, Type y)
                {
                    return ((DataType.IsStructuralEntityDataType(x) || x.IsAnonymousType() || x.IsRecordType())
                        && (DataType.IsStructuralEntityDataType(y) || y.IsAnonymousType()) || y.IsRecordType())
                        || base.AreStructurallyComparable(x, y);
                }

                private class MemberComparator : StructuralMemberInfoEqualityComparator
                {
                    public override bool Equals(MemberInfo x, MemberInfo y)
                    {
                        if (x == null && y == null)
                        {
                            return true;
                        }
                        if (x == null || y == null)
                        {
                            return false;
                        }

                        if (x.MemberType != y.MemberType)
                        {
                            if (x.MemberType == MemberTypes.Property && y.MemberType == MemberTypes.Field)
                            {
                                return EqualsPropertyToField((PropertyInfo)x, (FieldInfo)y);
                            }
                            if (y.MemberType == MemberTypes.Property && x.MemberType == MemberTypes.Field)
                            {
                                return EqualsPropertyToField((PropertyInfo)y, (FieldInfo)x);
                            }
                        }

                        return base.Equals(x, y);
                    }

                    protected override bool EqualsProperty(PropertyInfo x, PropertyInfo y)
                    {
                        if (!base.EqualsProperty(x, y))
                        {
                            var xMapping = x.GetCustomAttribute<MappingAttribute>(inherit: false);
                            var yMapping = y.GetCustomAttribute<MappingAttribute>(inherit: false);

                            var xName = xMapping != null ? xMapping.Uri : x.Name;
                            var yName = yMapping != null ? yMapping.Uri : y.Name;

                            return xName == yName && TypeComparer.Equals(x.PropertyType, y.PropertyType);
                        }

                        return true;
                    }

                    private bool EqualsPropertyToField(PropertyInfo x, FieldInfo y)
                    {
                        var xMapping = x.GetCustomAttribute<MappingAttribute>(inherit: false);
                        var yMapping = y.GetCustomAttribute<MappingAttribute>(inherit: false);

                        var xName = xMapping != null ? xMapping.Uri : x.Name;
                        var yName = yMapping != null ? yMapping.Uri : y.Name;

                        return xName == yName && TypeComparer.Equals(x.PropertyType, y.FieldType) && x.GetIndexParameters().Length == 0;
                    }
                }
            }

            private class ObjectComparator : DataTypeObjectEqualityComparator
            {
                public IEqualityComparer<Expression> ExpressionComparer
                {
                    get;
                    set;
                }

                protected override bool EqualsQuotation(object expected, object actual, QuotationDataType expectedDataType, QuotationDataType actualDataType)
                {
                    return ExpressionComparer.Equals((Expression)expected, (Expression)actual);
                }
            }

            private class UnassignedExpression : Expression
            {
                /// <summary>
                /// Creates a new sentinel expression with the specified underlying type.
                /// </summary>
                /// <param name="type">Underlying type of the expression.</param>
                public UnassignedExpression(Type type)
                {
                    Type = type;
                }

                /// <summary>
                /// Returns Extension.
                /// </summary>
                public override ExpressionType NodeType => ExpressionType.Extension;

                /// <summary>
                /// Gets the underlying type.
                /// </summary>
                public override Type Type { get; }

                /// <summary>
                /// Always returns true.
                /// </summary>
                public override bool CanReduce => true;

                /// <summary>
                /// Reduces to default expressions.
                /// </summary>
                /// <returns>DefaultExpression instance of the same underlying type as the sentinel.</returns>
                public override Expression Reduce()
                {
                    return Expression.Default(Type);
                }
            }

            private class UnassignedExpressionReducer : ExpressionVisitor
            {
                //
                // No implementation needed - will have the side-effect of calling Reduce on our nodes.
                //
            }
        }

        private static bool EqualsByKey<T, TKey>(IEnumerable<T> x, IEnumerable<T> y, Func<T, TKey> keySelector, Func<T, T, bool> areEqual)
        {
            var xLookup = x.ToDictionary(keySelector);
            var yEnumerator = y.GetEnumerator();

            var count = 0;
            while (yEnumerator.MoveNext())
            {
                var yValue = yEnumerator.Current;
                var yKey = keySelector(yValue);
                if (!xLookup.TryGetValue(yKey, out var xValue) || !areEqual(xValue, yValue))
                {
                    return false;
                }

                ++count;
            }

            return count == xLookup.Count;
        }

        #endregion

        #region Correctness

        private class ExpressionTypeAsserter : ExpressionVisitorWithReflection
        {
            private readonly Action<Type> _assertType;

            public ExpressionTypeAsserter(Action<Type> assertType)
            {
                _assertType = assertType;
            }

            protected override Type VisitType(Type type)
            {
                _assertType(type);
                return base.VisitType(type);
            }
        }

        private abstract class AnonymizeAndAssertBase
        {
            public void AreStructurallyEqual<T>(Expression<Func<T>> expression)
            {
                var anonymized = Anonymizer(expression);
                AssertStructurallyEqual(expression, anonymized);
            }

            public void AreAnonymized<T>(Expression<Func<T>> expression, params Type[] disallow)
            {
                IsAnonymized<T>(expression, Type.EmptyTypes, disallow);
            }

            public void AreNotAnonymized<T>(Expression<Func<T>> expression, params Type[] allow)
            {
                IsAnonymized<T>(expression, allow, Type.EmptyTypes);
            }

            public void IsAnonymized<T>(Expression<Func<T>> expression, Type[] allow, Type[] disallow)
            {
                var anonymized = Anonymizer(expression);
                var visitor = new ExpressionTypeAsserter(type => AssertAnonymized(type, allow, disallow));
                visitor.Visit(anonymized);
            }

            protected abstract Func<Expression, Expression> Anonymizer
            {
                get;
            }

            protected abstract bool IsAnonymized(Type type);

            private void AssertAnonymized(Type type, Type[] allow, Type[] disallow)
            {
                var dataType = DataType.FromType(type);
                var checker = new AnonymizationChecker(allow, disallow, st => Assert.IsTrue(IsAnonymized(st), "Type '{0}' is not anonymized.", st));
                try
                {
                    checker.Visit(dataType);
                }
                catch (CheckFailedException ex)
                {
                    Assert.Fail("Check failed for type '{0}' with message '{1}'.", ex.Type, ex.Message);
                }
            }
        }

        private class AnonymizeAndAssert : AnonymizeAndAssertBase
        {
            private readonly ExpressionEntityTypeAnonymizer _anonymizer = new();

            protected override Func<Expression, Expression> Anonymizer => _anonymizer.Apply;

            protected override bool IsAnonymized(Type type) => type.IsAnonymousType();
        }

        private class RecordizeAndAssert : AnonymizeAndAssertBase
        {
            private readonly ExpressionEntityTypeRecordizer _recordizer = new();

            protected override Func<Expression, Expression> Anonymizer => _recordizer.Apply;

            protected override bool IsAnonymized(Type type) => type.IsRecordType();
        }

        private class RecordizeSlimAndAssert : AnonymizeAndAssertBase
        {
            protected override Func<Expression, Expression> Anonymizer => Roundtrip;

            protected override bool IsAnonymized(Type type) => type.IsRecordType();

            private Expression Roundtrip(Expression expression)
            {
                var serializer = new SerializationHelper((liftFactory, reduceFactory) => new RecordizingBonsaiSerializer(liftFactory, reduceFactory));
                var bonsai = serializer.Serialize(expression);
                return serializer.Deserialize<Expression>(bonsai);
            }
        }

        private class AnonymizationChecker : DataTypeVisitor
        {
            private readonly Type[] _allow;
            private readonly Type[] _disallow;
            private readonly Action<Type> _assertStructural;

            public AnonymizationChecker(Type[] allow, Type[] disallow, Action<Type> assertStructural)
            {
                _allow = allow;
                _disallow = disallow;
                _assertStructural = assertStructural;
            }

            public override DataType Visit(DataType type)
            {
                if (_allow.Contains(type.UnderlyingType))
                {
                    return type;
                }
                else if (_disallow.Contains(type.UnderlyingType))
                {
                    throw new CheckFailedException("Type is not allowed.", type.UnderlyingType);
                }

                return base.Visit(type);
            }

            protected override DataType VisitStructural(StructuralDataType type)
            {
                _assertStructural(type.UnderlyingType);
                return base.VisitStructural(type);
            }
        }

        private class CheckFailedException : Exception
        {
            public CheckFailedException(string message, Type type)
                : base(message)
            {
                Type = type;
            }

            public Type Type { get; }
        }

        #endregion

        #endregion
    }
}
