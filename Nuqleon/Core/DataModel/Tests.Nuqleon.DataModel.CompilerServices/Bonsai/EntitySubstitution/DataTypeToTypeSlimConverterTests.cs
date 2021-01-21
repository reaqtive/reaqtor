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
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

using Nuqleon.DataModel;
using Nuqleon.DataModel.CompilerServices.Bonsai;
using Nuqleon.DataModel.TypeSystem;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Nuqleon.DataModel.CompilerServices.Bonsai.EntitySubstitution
{
    [TestClass]
    public class DataTypeToTypeSlimConverterTests
    {
        [TestMethod]
        public void DataTypeToTypeSlimConverter_Simple()
        {
            var types = new[]
            {
                typeof(int),
                typeof(int[]),
                typeof(Expression),
                typeof(List<int>),
                typeof(Expression<Func<int>>),
                typeof(Expression<Func<int, int>>),
                typeof(Tuple<int>),
            };

            foreach (var type in types)
            {
                AssertRoundtrip(type);
            }
        }

        [TestMethod]
        public void DataTypeToTypeSlimConverter_DataModel_Enum()
        {
            AssertRoundtrip(typeof(TestEnum), typeof(int));
        }

        [TestMethod]
        public void DataTypeToTypeSlimConverter_DataModel_Structural()
        {
            var types = new[]
            {
                typeof(Foo),
                typeof(Rec),
            };

            foreach (var type in types)
            {
                AssertStructuralRoundtrip(type);
            }
        }

        public enum TestEnum
        {
            [Mapping("A")]
            A = 0,
        }

        public class Foo
        {
            [Mapping("bar")]
            public int Bar { get; set; }
        }

        public class Rec
        {
            [Mapping("foo")]
            public Rec Foo { get; set; }
        }

        public static void AssertRoundtrip(Type type)
        {
            Assert.AreEqual(type, Roundtrip(type));
        }

        private static void AssertStructuralRoundtrip(Type type)
        {
            var comparer = new StructuralTypeEqualityComparer(() => new EntityTypeEqualityComparatorForEquals());
            Assert.IsTrue(comparer.Equals(type, Roundtrip(type)));
        }

        private static void AssertRoundtrip(Type type, Type expected)
        {
            Assert.AreEqual(expected, Roundtrip(type));
        }

        private static Type Roundtrip(Type type)
        {
            var dataType = DataType.FromType(type, allowCycles: true);
            var converter = new RecordDataTypeToTypeSlimConverter();
            return converter.Visit(dataType).ToType();
        }

        private sealed class EntityTypeEqualityComparatorForEquals : StructuralTypeEqualityComparator
        {
            private readonly IDictionary<Type, Type> _typeMap = new Dictionary<Type, Type>();

            protected override bool EqualsStructural(Type x, Type y)
            {
                if (_typeMap.TryGetValue(x, out var yMapped) && y == yMapped)
                {
                    return true;
                }

                _typeMap.Add(x, y);

                try
                {
                    var xDataType = (StructuralDataType)DataType.FromType(x, allowCycles: true);
                    var yDataType = (StructuralDataType)DataType.FromType(y, allowCycles: true);

                    if (xDataType.Properties.Count != yDataType.Properties.Count)
                    {
                        return false;
                    }

                    var xProperties = new Dictionary<string, DataProperty>(xDataType.Properties.Count);
                    foreach (var property in xDataType.Properties)
                    {
                        xProperties.Add(property.Name, property);
                    }

                    foreach (var yProperty in yDataType.Properties)
                    {
                        if (!xProperties.TryGetValue(yProperty.Name, out var xProperty) || !Equals(xProperty.Type.UnderlyingType, yProperty.Type.UnderlyingType))
                        {
                            return false;
                        }
                    }

                    return true;
                }
                finally
                {
                    _typeMap.Remove(x);
                }
            }

            protected override bool AreStructurallyComparable(Type x, Type y)
            {
                if (DataType.TryFromType(x, allowCycles: true, out var xDataType) && DataType.TryFromType(y, allowCycles: true, out var yDataType))
                {
                    return xDataType.Kind == DataTypeKinds.Structural && yDataType.Kind == DataTypeKinds.Structural;
                }

                return false;
            }
        }
    }
}
