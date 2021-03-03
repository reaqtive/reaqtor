// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD, ER - July 2013 - Created this file.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Omitted null checks similar to expression tree visitors.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Memory;
using System.Runtime.CompilerServices;

#if SUPPORT_SUBSET_TYPES
using System.Linq.CompilerServices;
using System.Reflection;
#endif

namespace Nuqleon.DataModel.TypeSystem
{
    /// <summary>
    /// Stateful comparator for objects with data model-compliant types.
    /// This comparator uses dictionaries and lists to detect recursion in objects and types.
    /// </summary>
#if SUPPORT_SUBSET_TYPES
    /// <remarks>
    /// The left object (i.e., the expected object) may contain a subset
    /// of the properties available to the right object, so long as these
    /// remaining properties have default values.
    /// </remarks>
#endif
    [KnownType]
    public class DataTypeObjectEqualityComparator : IEqualityComparer<object>, IClearable
    {
        private const uint Prime = 0xa5555529; // See CompilationPass.cpp in C# compiler codebase.

        private static readonly TypeToDataTypeConverter s_converter = TypeToDataTypeConverter.Create(allowCycles: true);

#if SUPPORT_SUBSET_TYPES
        private static Lazy<MethodInfo> _getDefaultMethod = new Lazy<MethodInfo>(() => ((MethodInfo)ReflectionHelpers.InfoOf(() => DataTypeObjectEqualityComparator.GetDefault<int>())).GetGenericMethodDefinition());
#endif

        // WARNING: This type implements IClearable. If state is added here, make sure to reset it in the Clear method.

        private readonly IDictionary<object, object> _equalMap;
        private readonly HashSet<object> _recursiveSet;

        /// <summary>
        /// Instantiates the comparator.
        /// </summary>
        public DataTypeObjectEqualityComparator()
        {
            _equalMap = new Dictionary<object, object>(IdentityComparer.Instance);
            _recursiveSet = new HashSet<object>(IdentityComparer.Instance);
        }

        /// <summary>
        /// Checks for value equality of two objects with data model-compliant types.
        /// </summary>
        /// <param name="x">The first object.</param>
        /// <param name="y">The second object.</param>
        /// <returns>true if the objects are equal, false otherwise.</returns>
        public new bool Equals(object x, object y)
        {
            if (object.ReferenceEquals(x, y))
            {
                return true;
            }
            else if (x == null || y == null)
            {
                return false;
            }

            var expectedDataType = GetDataType(x.GetType());
            var actualDataType = GetDataType(y.GetType());

            if (expectedDataType.Kind != actualDataType.Kind)
            {
                return false;
            }

            return expectedDataType.Kind switch
            {
                DataTypeKinds.Primitive => EqualsPrimitive(x, y, (PrimitiveDataType)expectedDataType, (PrimitiveDataType)actualDataType),
                DataTypeKinds.Array => EqualsArray(x, y, (ArrayDataType)expectedDataType, (ArrayDataType)actualDataType),
                DataTypeKinds.Structural => EqualsStructural(x, y, (StructuralDataType)expectedDataType, (StructuralDataType)actualDataType),
                DataTypeKinds.Function => EqualsFunction(x, y, (FunctionDataType)expectedDataType, (FunctionDataType)actualDataType),
                DataTypeKinds.Expression => EqualsExpression(x, y, (ExpressionDataType)expectedDataType, (ExpressionDataType)actualDataType),
                DataTypeKinds.Quotation => EqualsQuotation(x, y, (QuotationDataType)expectedDataType, (QuotationDataType)actualDataType),
                DataTypeKinds.Custom => EqualsCustom(x, y, expectedDataType, actualDataType),
                _ => EqualsExtension(x, y, expectedDataType, actualDataType),
            };
        }

        /// <summary>
        /// Checks the equality of a pair of objects with primitive data model types.
        /// </summary>
        /// <param name="expected">The first object.</param>
        /// <param name="actual">The second object.</param>
        /// <param name="expectedDataType">The left data type.</param>
        /// <param name="actualDataType">The right data type.</param>
        /// <returns>true if the objects are equal, false otherwise.</returns>
        protected virtual bool EqualsPrimitive(object expected, object actual, PrimitiveDataType expectedDataType, PrimitiveDataType actualDataType)
        {
            if (expectedDataType.UnderlyingType.IsEnum)
            {
                if (!actualDataType.UnderlyingType.IsEnum)
                {
                    var expectedValue = Convert.ChangeType(expected, expectedDataType.UnderlyingType.GetEnumUnderlyingType(), CultureInfo.InvariantCulture);
                    return Equals(expectedValue, actual);
                }
            }
            else
            {
                if (actualDataType.UnderlyingType.IsEnum)
                {
                    var actualValue = Convert.ChangeType(actual, actualDataType.UnderlyingType.GetEnumUnderlyingType(), CultureInfo.InvariantCulture);
                    return Equals(expected, actualValue);
                }
            }

            return EqualityComparer<object>.Default.Equals(expected, actual);
        }

        /// <summary>
        /// Checks the equality of a pair of objects with array data model types.
        /// </summary>
        /// <param name="expected">The first object.</param>
        /// <param name="actual">The second object.</param>
        /// <param name="expectedDataType">The left data type.</param>
        /// <param name="actualDataType">The right data type.</param>
        /// <returns>true if the objects are equal, false otherwise.</returns>
        protected virtual bool EqualsArray(object expected, object actual, ArrayDataType expectedDataType, ArrayDataType actualDataType)
        {
            var expectedEnumerator = expectedDataType.GetList(expected).GetEnumerator();
            var actualEnumerator = actualDataType.GetList(actual).GetEnumerator();

            while (expectedEnumerator.MoveNext())
            {
                if (!actualEnumerator.MoveNext() || !Equals(expectedEnumerator.Current, actualEnumerator.Current))
                {
                    return false;
                }
            }

            if (actualEnumerator.MoveNext())
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks the equality of a pair of objects with structural data model types.
        /// </summary>
        /// <param name="expected">The first object.</param>
        /// <param name="actual">The second object.</param>
        /// <param name="expectedDataType">The left data type.</param>
        /// <param name="actualDataType">The right data type.</param>
        /// <returns>true if the objects are equal, false otherwise.</returns>
        /// <remarks>
        /// If the useStrict flag is not set to true in the constructor, the right data
        /// type may have properties that do not exist on the left data type, so long
        /// as the right object has default values for these properties.
        /// </remarks>
        protected virtual bool EqualsStructural(object expected, object actual, StructuralDataType expectedDataType, StructuralDataType actualDataType)
        {
            if (_equalMap.TryGetValue(expected, out var mappedActual))
            {
                return object.ReferenceEquals(actual, mappedActual);
            }
            else
            {
                _equalMap.Add(expected, actual);

                try
                {
                    // CONSIDER: Use pooled dictionaries if this shows up as a performance bottleneck.
                    var actualPropertiesMap = actualDataType.Properties.ToDictionary(p => p.Name, p => new PropertyEntry(p));

                    foreach (var expectedProperty in expectedDataType.Properties)
                    {
                        if (!actualPropertiesMap.TryGetValue(expectedProperty.Name, out var actualProperty))
                        {
                            return false;
                        }

                        actualProperty.HasVisited = true;

                        var expectedPropertyValue = expectedProperty.GetValue(expected);
                        var actualPropertyValue = actualProperty.Property.GetValue(actual);

                        if (!Equals(expectedPropertyValue, actualPropertyValue))
                        {
                            return false;
                        }
                    }

#if SUPPORT_SUBSET_TYPES
                    foreach (var entry in actualPropertiesMap)
                    {
                        if (!entry.Value.HasVisited)
                        {
                            var remainingProperty = actualPropertiesMap[entry.Key].Property;

                            var defaultValue = GetDefault(remainingProperty.Type.UnderlyingType);
                            if (!object.Equals(remainingProperty.GetValue(actual), defaultValue))
                            {
                                return false;
                            }
                        }
                    }
#else
                    foreach (var entry in actualPropertiesMap)
                    {
                        if (!entry.Value.HasVisited)
                        {
                            return false;
                        }
                    }
#endif

                    return true;
                }
                finally
                {
                    _equalMap.Remove(expected);
                }
            }
        }

        /// <summary>
        /// Checks the equality of a pair of objects with function data model types.
        /// </summary>
        /// <param name="expected">The first object.</param>
        /// <param name="actual">The second object.</param>
        /// <param name="expectedDataType">The left data type.</param>
        /// <param name="actualDataType">The right data type.</param>
        /// <returns>true if the objects are equal, false otherwise.</returns>
        protected virtual bool EqualsFunction(object expected, object actual, FunctionDataType expectedDataType, FunctionDataType actualDataType) => EqualityComparer<object>.Default.Equals(expected, actual);

        /// <summary>
        /// Checks the equality of a pair of objects with expression data model types.
        /// </summary>
        /// <param name="expected">The first object.</param>
        /// <param name="actual">The second object.</param>
        /// <param name="expectedDataType">The left data type.</param>
        /// <param name="actualDataType">The right data type.</param>
        /// <returns>true if the objects are equal, false otherwise.</returns>
        protected virtual bool EqualsExpression(object expected, object actual, ExpressionDataType expectedDataType, ExpressionDataType actualDataType) => EqualityComparer<object>.Default.Equals(expected, actual);

        /// <summary>
        /// Checks the equality of a pair of objects with quotation data model types.
        /// </summary>
        /// <param name="expected">The first object.</param>
        /// <param name="actual">The second object.</param>
        /// <param name="expectedDataType">The left data type.</param>
        /// <param name="actualDataType">The right data type.</param>
        /// <returns>true if the objects are equal, false otherwise.</returns>
        protected virtual bool EqualsQuotation(object expected, object actual, QuotationDataType expectedDataType, QuotationDataType actualDataType) => EqualityComparer<object>.Default.Equals(expected, actual);

        /// <summary>
        /// Checks the equality of a pair of objects with custom data model types.
        /// </summary>
        /// <param name="expected">The first object.</param>
        /// <param name="actual">The second object.</param>
        /// <param name="expectedDataType">The left data type.</param>
        /// <param name="actualDataType">The right data type.</param>
        /// <returns>true if the objects are equal, false otherwise.</returns>
        protected virtual bool EqualsCustom(object expected, object actual, DataType expectedDataType, DataType actualDataType) => EqualityComparer<object>.Default.Equals(expected, actual);

        /// <summary>
        /// Checks the equality of a pair of objects with extension data model types.
        /// </summary>
        /// <param name="expected">The first object.</param>
        /// <param name="actual">The second object.</param>
        /// <param name="expectedDataType">The left data type.</param>
        /// <param name="actualDataType">The right data type.</param>
        /// <returns>true if the objects are equal, false otherwise.</returns>
        protected virtual bool EqualsExtension(object expected, object actual, DataType expectedDataType, DataType actualDataType) => EqualityComparer<object>.Default.Equals(expected, actual);

        /// <summary>
        /// Gets the hash code of an object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>The hash code of the object.</returns>
        public int GetHashCode(object obj)
        {
            if (obj == null)
            {
                return EqualityComparer<object>.Default.GetHashCode(obj);
            }

            var dataType = GetDataType(obj.GetType());

            return dataType.Kind switch
            {
                DataTypeKinds.Primitive => GetHashCodePrimitive(obj, (PrimitiveDataType)dataType),
                DataTypeKinds.Array => GetHashCodeArray(obj, (ArrayDataType)dataType),
                DataTypeKinds.Structural => GetHashCodeStructural(obj, (StructuralDataType)dataType),
                DataTypeKinds.Function => GetHashCodeFunction(obj, (FunctionDataType)dataType),
                DataTypeKinds.Expression => GetHashCodeExpression(obj, (ExpressionDataType)dataType),
                DataTypeKinds.Quotation => GetHashCodeQuotation(obj, (QuotationDataType)dataType),
                DataTypeKinds.Custom => GetHashCodeCustom(obj, dataType),
                _ => GetHashCodeExtension(obj, dataType),
            };
        }

        /// <summary>
        /// Gets the hash code of an instance of a primitive data model type.
        /// </summary>
        /// <param name="obj">The instance.</param>
        /// <param name="dataType">The data model type.</param>
        /// <returns>The hash code.</returns>
        protected virtual int GetHashCodePrimitive(object obj, PrimitiveDataType dataType) => EqualityComparer<object>.Default.GetHashCode(obj);

        /// <summary>
        /// Gets the hash code of an instance of an array data model type.
        /// </summary>
        /// <param name="obj">The instance.</param>
        /// <param name="dataType">The data model type.</param>
        /// <returns>The hash code.</returns>
        protected virtual int GetHashCodeArray(object obj, ArrayDataType dataType)
        {
            var objList = dataType.GetList(obj);

            var hash = 0;
            foreach (var element in objList)
            {
                unchecked
                {
                    hash = (int)(hash * Prime) + GetHashCode(element);
                }
            }
            return hash;
        }

        /// <summary>
        /// Gets the hash code of an instance of a structural data model type.
        /// </summary>
        /// <param name="obj">The instance.</param>
        /// <param name="dataType">The data model type.</param>
        /// <returns>The hash code.</returns>
        /// <remarks>
        /// If the useStrict flag is not set to true in the constructor, properties
        /// with default values are not included in the hash code value.
        /// </remarks>
        protected virtual int GetHashCodeStructural(object obj, StructuralDataType dataType)
        {
            if (_recursiveSet.Contains(obj))
            {
                unchecked
                {
                    return (int)Prime;
                }
            }
            else
            {
                _recursiveSet.Add(obj);
            }

            var orderedProperties = dataType.Properties.OrderBy(p => p.Name); // CONSIDER: should we sort properties earlier?

            var hash = 0;
            foreach (var property in orderedProperties)
            {
                unchecked
                {
#if SUPPORT_SUBSET_TYPES
                    hash = (int)(hash * prime) + property.Name.GetHashCode();
                    var defaultValue = GetDefault(property.Type.UnderlyingType);
                    var value = property.GetValue(obj);
                    if (!object.Equals(value, defaultValue))
                    {
                        hash = (int)(hash * prime) + GetHashCode(value);
                    }
#else
                    hash = (int)(hash * Prime) +
#if NET5_0 || NETSTANDARD2_1
                        property.Name.GetHashCode(StringComparison.Ordinal)
#else
                        property.Name.GetHashCode()
#endif
                        ;
                    hash = (int)(hash * Prime) + GetHashCode(property.GetValue(obj));
#endif
                }
            }
            return hash;
        }

        /// <summary>
        /// Gets the hash code of an instance of a function data model type.
        /// </summary>
        /// <param name="obj">The instance.</param>
        /// <param name="dataType">The data model type.</param>
        /// <returns>The hash code.</returns>
        protected virtual int GetHashCodeFunction(object obj, FunctionDataType dataType) => EqualityComparer<object>.Default.GetHashCode(obj);

        /// <summary>
        /// Gets the hash code of an instance of an expression data model type.
        /// </summary>
        /// <param name="obj">The instance.</param>
        /// <param name="dataType">The data model type.</param>
        /// <returns>The hash code.</returns>
        protected virtual int GetHashCodeExpression(object obj, ExpressionDataType dataType) => EqualityComparer<object>.Default.GetHashCode(obj);

        /// <summary>
        /// Gets the hash code of an instance of a quotation data model type.
        /// </summary>
        /// <param name="obj">The instance.</param>
        /// <param name="dataType">The data model type.</param>
        /// <returns>The hash code.</returns>
        protected virtual int GetHashCodeQuotation(object obj, QuotationDataType dataType) => EqualityComparer<object>.Default.GetHashCode(obj);

        /// <summary>
        /// Gets the hash code of an instance of a custom data model type.
        /// </summary>
        /// <param name="obj">The instance.</param>
        /// <param name="dataType">The data model type.</param>
        /// <returns>The hash code.</returns>
        protected virtual int GetHashCodeCustom(object obj, DataType dataType) => EqualityComparer<object>.Default.GetHashCode(obj);

        /// <summary>
        /// Gets the hash code of an instance of an extension data model type.
        /// </summary>
        /// <param name="obj">The instance.</param>
        /// <param name="dataType">The data model type.</param>
        /// <returns>The hash code.</returns>
        protected virtual int GetHashCodeExtension(object obj, DataType dataType) => EqualityComparer<object>.Default.GetHashCode(obj);

        private static DataType GetDataType(Type type)
        {
            if (!s_converter.TryConvert(type, out var res))
            {
                return s_converter.ConvertKnownType(type);
            }

            return res;
        }

        /// <summary>
        /// Clears the state in the object for reuse in an object pool.
        /// </summary>
        void IClearable.Clear() => Clear();

        /// <summary>
        /// Clears the state in the object for reuse in an object pool.
        /// </summary>
        protected void Clear()
        {
            _recursiveSet.Clear();
            _equalMap.Clear();
        }

        // CONSIDER: Generalize the ShouldReturnToPool pattern to be supported in System.Memory natively.

        internal bool ShouldReturnToPool
        {
            get
            {
                // NB: Both _recursiveSet and _equalMap are linear in the number of structural types. If too many such entries are encountered,
                //     the pool to recycle these may permanently remain bloated. Note that Dictionary<,> and HashSet<> don't trim their internal
                //     data structures upon calls to Clear, so we have to drop the comparator instance from the pool if we believe it's too big.

                // NB: The following value is chosen rather arbitrarily but based on the observation that a structural type would need to have
                //     an awful lot of distinct referenced structural types to reach this threshold.

                const int MaxNumberOfStructuralTypeEntries = 64;

                return _recursiveSet.Count < MaxNumberOfStructuralTypeEntries && _equalMap.Count < MaxNumberOfStructuralTypeEntries;
            }
        }

#if SUPPORT_SUBSET_TYPES
        private object GetDefault(Type t)
        {
            return _getDefaultMethod.Value.MakeGenericMethod(t).Invoke(this, null);
        }

        private static T GetDefault<T>()
        {
            return default(T);
        }
#endif

        private sealed class IdentityComparer : IEqualityComparer<object>
        {
            public static readonly IEqualityComparer<object> Instance = new IdentityComparer();

            public new bool Equals(object x, object y) => object.ReferenceEquals(x, y);

            public int GetHashCode(object obj) => RuntimeHelpers.GetHashCode(obj);
        }

        private sealed class PropertyEntry
        {
            public PropertyEntry(DataProperty property) => Property = property;

            public DataProperty Property;
            public bool HasVisited;
        }
    }
}
