// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

using Nuqleon.DataModel.TypeSystem;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Nuqleon.DataModel.CompilerServices.TypeSystem
{
    [TestClass]
    public class DataTypeVisitorTests
    {
        [TestMethod]
        public void DataTypeVisitor_ArgumentChecking()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => DataTypeVisitor.Visit<int>(nodes: null, x => x), ex => Assert.AreEqual("nodes", ex.ParamName));
            AssertEx.ThrowsException<ArgumentNullException>(() => DataTypeVisitor.Visit<int>(new List<int> { 5 }.AsReadOnly(), elementVisitor: null), ex => Assert.AreEqual("elementVisitor", ex.ParamName));
        }

        [TestMethod]
        public void DataTypeVisitor_InconsistentRewrite()
        {
            var v = new BrokenQuotationVisitor();
            Assert.ThrowsException<InvalidOperationException>(() => v.Visit(DataType.FromType(typeof(Expression<Func<int>>))));
        }

        [TestMethod]
        public void DataTypeVisitor_Simple()
        {
            var mv = new MyVisitor();

            Assert.IsNull(mv.Visit(type: null));

            foreach (var kv in new Dictionary<Type, Type>
            {
                { typeof(int), typeof(long) },
                { typeof(int?), typeof(long?) },
                { typeof(int[]), typeof(long[]) },
                { typeof(List<int>), typeof(List<long>) },
                { typeof(Func<int>), typeof(Func<long>) },
                { typeof(Func<int, bool>), typeof(Func<long, bool>) },
                { typeof(Func<bool, double, int>), typeof(Func<bool, double, long>) },
                { typeof(Tuple<bool, int, double>), typeof(Tuple<bool, long, double>) },
                { typeof(Expression<Func<int>>), typeof(Expression<Func<long>>) },
                { typeof(BinaryExpression), typeof(BinaryExpression) },
            })
            {
                var t = DataType.FromType(kv.Key);
                var e = DataType.FromType(kv.Value);
                var r = mv.Visit(t);

                Assert.AreEqual(e.ToString(), r.ToString());
            }
        }

        [TestMethod]
        public void DataTypeVisitor_Custom()
        {
            var v = new MyVisitor();

            var d = new MyDataType();
            var e = v.Visit(d);

            Assert.ThrowsException<NotImplementedException>(() => _ = new DataTypeVisitor().Visit(d));

            Assert.AreSame(d, e);
            Assert.AreEqual(d.ToString(), e.ToString());
        }

        [TestMethod]
        public void DataTypeVisitor_Change()
        {
            var v = new IncompleteVisitor();

            var d1 = DataType.FromType(typeof(int[]));
            Assert.ThrowsException<NotImplementedException>(() => v.Visit(d1));

            var d2 = DataType.FromType(typeof(bool[]));
            Assert.AreSame(d2, v.Visit(d2));

            var d3 = DataType.FromType(new { a = 1 }.GetType());
            Assert.ThrowsException<NotImplementedException>(() => v.Visit(d3));

            var d4 = DataType.FromType(new { a = true }.GetType());
            Assert.AreSame(d4, v.Visit(d4));

            var d5 = new StructuralDataType(typeof(Foo), new[] { new DataProperty(typeof(Foo).GetField("Bar"), "Bar", DataType.FromType(typeof(int))) }.ToReadOnly(), StructuralDataTypeKinds.Entity);
            Assert.ThrowsException<NotImplementedException>(() => v.Visit(d5));
        }

        private class Foo
        {
            public int Bar = 0;
        }

        [TestMethod]
        public void DataTypeVisitor_Generic_Simple()
        {
            var v = new GenericVisitor();

            Assert.IsNull(v.Visit(type: null));

            foreach (var t in new[] {
                typeof(int),
                typeof(int[]),
                typeof(Func<int, int>),
            })
            {
                var d = DataType.FromType(t);
                var r = v.Visit(d);
                Assert.AreEqual(t, r, t.ToString());
            }

            var c = new MyDataType();
            Assert.AreSame(c.UnderlyingType, v.Visit(c));

            v.Test();
        }

        private class MyVisitor : DataTypeVisitor
        {
            protected override DataType VisitPrimitive(PrimitiveDataType type)
            {
                if (type.UnderlyingType == typeof(int))
                {
                    return new PrimitiveDataType(typeof(long), PrimitiveDataTypeKinds.Atom);
                }

                if (type.UnderlyingType == typeof(int?))
                {
                    return new PrimitiveDataType(typeof(long?), PrimitiveDataTypeKinds.Atom);
                }

                return base.VisitPrimitive(type);
            }

            protected override Type ChangeUnderlyingType(Type type)
            {
                return new ChangeType().Visit(type);
            }

            protected override MemberInfo ChangeProperty(PropertyInfo property)
            {
                return ChangeUnderlyingType(property.DeclaringType).GetProperty(property.Name);
            }

            protected override DataType VisitCustom(DataType type)
            {
                return type;
            }

            private class ChangeType : TypeVisitor
            {
                public override Type Visit(Type type)
                {
                    if (type == typeof(int))
                    {
                        return typeof(long);
                    }

                    return base.Visit(type);
                }
            }
        }

        private class BrokenQuotationVisitor : DataTypeVisitor
        {
            protected override DataType VisitFunction(FunctionDataType type)
            {
                return DataType.FromType(typeof(int));
            }
        }

        private class MyDataType : DataType
        {
            public MyDataType()
                : base(typeof(int))
            {
            }

            public override DataTypeKinds Kind => DataTypeKinds.Custom;

            public override DataType Reduce()
            {
                return base.Reduce();
            }

            public override string ToString()
            {
                return "Custom(" + UnderlyingType + ")";
            }

            public override object CreateInstance(params object[] arguments)
            {
                throw new NotImplementedException();
            }
        }

        private class IncompleteVisitor : DataTypeVisitor
        {
            protected override DataType VisitPrimitive(PrimitiveDataType type)
            {
                if (type.UnderlyingType == typeof(int))
                {
                    return DataType.FromType(typeof(long));
                }

                return type;
            }
        }

        private class GenericVisitor : DataTypeVisitor<Type, Tuple<string, Type>>
        {
            protected override Type MakeArray(ArrayDataType type, Type elementType)
            {
                return elementType.MakeArrayType();
            }

            protected override Type VisitExpression(ExpressionDataType type)
            {
                return type.UnderlyingType;
            }

            protected override Type MakeFunction(FunctionDataType type, ReadOnlyCollection<Type> parameterTypes, Type returnType)
            {
                return Expression.GetFuncType(parameterTypes.Concat(new[] { returnType }).ToArray()); // don't care about actions here for testing
            }

            protected override Type VisitOpenGenericParameter(OpenGenericParameterDataType type)
            {
                return type.UnderlyingType;
            }

            protected override Type VisitPrimitive(PrimitiveDataType type)
            {
                return type.UnderlyingType; // don't care about nullable here for testing
            }

            protected override Type MakeQuotation(QuotationDataType type, Type functionType)
            {
                return typeof(Expression<>).MakeGenericType(functionType);
            }

            protected override Type MakeStructural(StructuralDataType type, ReadOnlyCollection<Tuple<string, Type>> properties)
            {
                return RuntimeCompiler.CreateAnonymousType(properties.Select(p => new KeyValuePair<string, Type>(p.Item1, p.Item2)));
            }

            protected override Type VisitCustom(DataType type)
            {
                return type.UnderlyingType;
            }

            protected override Tuple<string, Type> MakeProperty(DataProperty property, Type propertyType)
            {
                return new Tuple<string, Type>(property.Name, propertyType);
            }

            public void Test()
            {
                Assert.IsNull(VisitProperty(property: null));
            }
        }
    }
}
