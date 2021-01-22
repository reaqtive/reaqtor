// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Memory;
using System.Reflection;
using System.Text;

namespace Nuqleon.DataModel.TypeSystem
{
    /// <summary>
    /// Abstract base class for data types that conform to the data model.
    /// </summary>
    public abstract class DataType
    {
        private static readonly ObjectPool<DataTypePrinter> s_printerPool = new(() => new DataTypePrinter());

        /// <summary>
        /// Creates a new data type of the specified kind.
        /// </summary>
        /// <param name="type">Underlying CLR type.</param>
        protected DataType(Type type)
        {
            UnderlyingType = type;
        }

        /// <summary>
        /// Gets the kind of the data type.
        /// </summary>
        public abstract DataTypeKinds Kind { get; }

        /// <summary>
        /// Gets the underlying CLR type.
        /// </summary>
        public Type UnderlyingType { get; }

        /// <summary>
        /// Reduces the data type to a simpler data type representation.
        /// </summary>
        /// <returns>Simpler data type representation, if available. Otherwise, the current object is returned.</returns>
        public virtual DataType Reduce() => this;

        /// <summary>
        /// Returns a friendly string representation of the type.
        /// </summary>
        /// <returns>Friendly string representation of the type.</returns>
        public override string ToString()
        {
            using var pp = s_printerPool.New();

            var printer = pp.Object;
            return printer.Print(this);
        }

        /// <summary>
        /// Converts a CLR type to a data model type. No recursive type definitions are allowed.
        /// </summary>
        /// <param name="type">CLR type to convert.</param>
        /// <returns>Data model type isomorphic to the given CLR type.</returns>
        public static DataType FromType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return FromTypeImpl(type, allowCycles: false);
        }

        /// <summary>
        /// Converts a CLR type to a data model type. Support for recursive type definitions can be enabled.
        /// </summary>
        /// <param name="type">CLR type to convert.</param>
        /// <param name="allowCycles">Indicates whether cycles in type definitions are allowed. This is a specialized use case and should be considered carefully, because not all tools can deal with cycles.</param>
        /// <returns>Data model type isomorphic to the given CLR type.</returns>
        public static DataType FromType(Type type, bool allowCycles)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return FromTypeImpl(type, allowCycles);
        }

        private static DataType FromTypeImpl(Type type, bool allowCycles)
        {
            var res = TypeToDataTypeConverter.Create(allowCycles).Convert(type);
            return res;
        }

        /// <summary>
        /// Tries to convert a CLR type to a data model type. No recursive type definitions are allowed.
        /// </summary>
        /// <param name="type">CLR type to convert.</param>
        /// <param name="result">Data model type isomorphic to the given CLR type.</param>
        /// <returns>true if the conversion succeeded; otherwise, false.</returns>
        public static bool TryFromType(Type type, out DataType result)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return TryFromTypeImpl(type, allowCycles: false, out result);
        }

        /// <summary>
        /// Tries to convert a CLR type to a data model type. Support for recursive type definitions can be enabled.
        /// </summary>
        /// <param name="type">CLR type to convert.</param>
        /// <param name="allowCycles">Indicates whether cycles in type definitions are allowed. This is a specialized use case and should be considered carefully, because not all tools can deal with cycles.</param>
        /// <param name="result">Data model type isomorphic to the given CLR type.</param>
        /// <returns>true if the conversion succeeded; otherwise, false.</returns>
        public static bool TryFromType(Type type, bool allowCycles, out DataType result)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return TryFromTypeImpl(type, allowCycles, out result);
        }

        private static bool TryFromTypeImpl(Type type, bool allowCycles, out DataType result)
        {
            var res = TypeToDataTypeConverter.Create(allowCycles).TryConvert(type, out result);
            return res;
        }

        /// <summary>
        /// Checks whether the specified CLR type can be converted to a valid data model type. No recursive type definitions are allowed.
        /// Violations against the data model type system are reported as an exception.
        /// </summary>
        /// <param name="type">CLR type to check.</param>
        /// <exception cref="AggregateException">Aggregate exception with DataTypeException inner exception objects to describe violations against the data model type system.</exception>
        public static void Check(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            CheckImpl(type, allowCycles: false);
        }

        /// <summary>
        /// Checks whether the specified CLR type can be converted to a valid data model type. Support for recursive type definitions can be enabled.
        /// Violations against the data model type system are reported as an exception.
        /// </summary>
        /// <param name="type">CLR type to check.</param>
        /// <param name="allowCycles">Indicates whether cycles in type definitions are allowed. This is a specialized use case and should be considered carefully, because not all tools can deal with cycles.</param>
        /// <exception cref="AggregateException">Aggregate exception with DataTypeException inner exception objects to describe violations against the data model type system.</exception>
        public static void Check(Type type, bool allowCycles)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            CheckImpl(type, allowCycles);
        }

        private static void CheckImpl(Type type, bool allowCycles)
        {
            if (!TryCheckImpl(type, allowCycles, out var errors))
            {
                throw new AggregateException(
                    errors.Select(e => e.ToException())
                );
            }
        }

        /// <summary>
        /// Checks whether the specified CLR type can be converted to a valid data model type. No recursive type definitions are allowed.
        /// Violations against the data model type system are reported through the output parameter.
        /// </summary>
        /// <param name="type">CLR type to check.</param>
        /// <param name="errors">Violations against the data model type system.</param>
        /// <returns>true if the given CLR type passes data model type checking; otherwise, false.</returns>
        public static bool TryCheck(Type type, out ReadOnlyCollection<DataTypeError> errors)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return TryCheckImpl(type, allowCycles: false, out errors);
        }

        /// <summary>
        /// Checks whether the specified CLR type can be converted to a valid data model type. Support for recursive type definitions can be enabled.
        /// Violations against the data model type system are reported through the output parameter.
        /// </summary>
        /// <param name="type">CLR type to check.</param>
        /// <param name="allowCycles">Indicates whether cycles in type definitions are allowed. This is a specialized use case and should be considered carefully, because not all tools can deal with cycles.</param>
        /// <param name="errors">Violations against the data model type system.</param>
        /// <returns>true if the given CLR type passes data model type checking; otherwise, false.</returns>
        public static bool TryCheck(Type type, bool allowCycles, out ReadOnlyCollection<DataTypeError> errors)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return TryCheckImpl(type, allowCycles, out errors);
        }

        private static bool TryCheckImpl(Type type, bool allowCycles, out ReadOnlyCollection<DataTypeError> errors)
        {
            return DataTypeChecker.TryCheck(type, allowCycles, out errors);
        }

        /// <summary>
        /// Checks whether the specified CLR type represents a user-defined structural entity data type.
        /// This method doesn't perform extensive checking of required invariants. Use FromType or Check to ensure the type is a valid entity type.
        /// </summary>
        /// <param name="type">CLR type to check.</param>
        /// <returns>true if the given CLR type represents a user-defined structural entity data type; otherwise, false.</returns>
        public static bool IsStructuralEntityDataType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var props = type.GetProperties();

            return props.Any(p => p.IsDefined(typeof(MappingAttribute), inherit: false));
        }

        /// <summary>
        /// Checks whether the specified CLR type represents a user-defined entity enumeration data type.
        /// This method doesn't perform extensive checking of required invariants. Use FromType or Check to ensure the type is a valid entity type.
        /// </summary>
        /// <param name="type">CLR type to check.</param>
        /// <returns>true if the given CLR type represents a user-defined entity enumeration data type; otherwise, false.</returns>
        public static bool IsEntityEnumDataType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var enumType = default(Type);

            if (type.IsEnum)
            {
                enumType = type;
            }
            else
            {
                var underlyingType = Nullable.GetUnderlyingType(type);
                if (underlyingType != null && underlyingType.IsEnum)
                {
                    enumType = underlyingType;
                }
            }

            if (enumType != null)
            {
                var fields = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);

                return fields.Any(f => f.IsDefined(typeof(MappingAttribute), inherit: true));
            }

            return false;
        }

        /// <summary>
        /// Checks whether the specified object is assignable to the data type.
        /// </summary>
        /// <param name="value">Object to check for assignment compatibility.</param>
        protected void CheckType(object value)
        {
            if (!TryCheckType(value))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Object '{0}' of data type kind '{1}' does not match the expected underlying data type '{2}'.", value, Kind, UnderlyingType));
            }
        }

        /// <summary>
        /// Tries to check whether the specified object is assignable to the data type.
        /// </summary>
        /// <param name="value">Object to check for assignment compatibility.</param>
        /// <returns>true if the object can be assigned to the data type; otherwise, false.</returns>
        protected bool TryCheckType(object value)
        {
            if (value != null)
            {
                if (!UnderlyingType.IsAssignableFrom(value.GetType()))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Creates a new instance of the data type, using the specified construction arguments.
        /// </summary>
        /// <param name="arguments">Arguments used for construction of the data type instance.</param>
        /// <returns>New instance of the data type.</returns>
        public abstract object CreateInstance(params object[] arguments);

        private sealed class DataTypePrinter : DataTypeVisitor<string, string>, IClearable
        {
            private readonly Dictionary<DataType, string> _types;

            private const string CYCLEMARK = "$$$CYCLE$$$";
            private const string TYPEPREFIX = "t";
            private bool _hasCycles;
            private Dictionary<DataType, NamedType> _namedTypes;

            public DataTypePrinter()
            {
                _types = new Dictionary<DataType, string>();
                _hasCycles = false;
            }

            public string Print(DataType type)
            {
                string res;

                try
                {
                    res = Visit(type);
                }
                catch (CycleException)
                {
                    res = PrintCycleRich(type);
                }

                return res;
            }

            private string PrintCycleRich(DataType type)
            {
                _hasCycles = true;
                _namedTypes = new Dictionary<DataType, NamedType>();

                var res = Visit(type);

                using var psb = PooledStringBuilder.New();

                var sb = psb.StringBuilder;

                var sortedTypes =
                    _namedTypes.OrderBy(kv =>
                        int.Parse(
#if NET5_0 || NETSTANDARD2_1
                            kv.Value.Name[TYPEPREFIX.Length..],
#else
                            kv.Value.Name.Substring(TYPEPREFIX.Length),
#endif
                            CultureInfo.InvariantCulture
                        )
                    );

                var i = 0;
                foreach (var kv in sortedTypes)
                {
                    var namedType = kv.Value;

                    var keyword = i == 0 ? "let" : "and";

                    sb.AppendFormat(CultureInfo.InvariantCulture, "{0} {1} = {2}\r\n", keyword, namedType.Name, namedType.Declaration);

                    i++;
                }

                sb.AppendLine("in  " + res);

                return sb.ToString();
            }

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1032 // Exception never leaks and is used to bail out from algorithms.
#pragma warning disable CA1064 // Other constructors not needed.
            private sealed class CycleException : Exception
            {
            }
#pragma warning restore CA1064
#pragma warning restore CA1032
#pragma warning restore IDE0079

            private sealed class NamedType
            {
                public string Name;
                public string Declaration;
            }

            public override string Visit(DataType type)
            {
                if (_hasCycles)
                {
                    return VisitCycleRich(type);
                }
                else
                {
                    return VisitCycleFree(type);
                }
            }

            private string VisitCycleRich(DataType type)
            {
                if (type is StructuralDataTypeReference sdt)
                {
                    if (!_namedTypes.TryGetValue(sdt, out var namedType))
                    {
                        var name = TYPEPREFIX + (_namedTypes.Count + 1);

                        namedType = new NamedType { Name = name };

                        _namedTypes[sdt] = namedType;

                        namedType.Declaration = base.Visit(type);
                    }
                    else
                    {
                        return namedType.Name;
                    }

                    return namedType.Name;
                }
                else
                {
                    return base.Visit(type);
                }
            }

            private string VisitCycleFree(DataType type)
            {

                if (_types.TryGetValue(type, out var res))
                {
                    if (res == CYCLEMARK)
                    {
                        throw new CycleException();
                    }
                }
                else
                {
                    _types[type] = CYCLEMARK;

                    res = base.Visit(type);

                    _types[type] = res;
                }

                return res;
            }

            protected override string MakeArray(ArrayDataType type, string elementType)
            {
                return elementType + "[]";
            }

            protected override string VisitExpression(ExpressionDataType type)
            {
                return type.Type.ToCSharpString();
            }

            protected override string MakeFunction(FunctionDataType type, ReadOnlyCollection<string> parameterTypes, string returnType)
            {
                return "(" + string.Join(", ", parameterTypes) + ") => " + returnType;
            }

            protected override string VisitPrimitive(PrimitiveDataType type)
            {
                return type.UnderlyingType.ToCSharpString();
            }

            protected override string VisitOpenGenericParameter(OpenGenericParameterDataType type)
            {
                return type.UnderlyingType.ToCSharpString();
            }

            protected override string MakeQuotation(QuotationDataType type, string functionType)
            {
                return "@{ " + functionType + " }";
            }

            protected override string MakeStructural(StructuralDataType type, ReadOnlyCollection<string> properties)
            {
                return "{ " + string.Join("; ", properties) + " }";
            }

            protected override string VisitCustom(DataType type)
            {
                return type.ToString();
            }

            protected override string MakeProperty(DataProperty property, string propertyType)
            {
                return property.Name + " : " + propertyType;
            }

            public void Clear()
            {
                // CONSIDER: Add the ShouldReturnToPool pattern to keep big instances from polluting the pool.
                // CONSIDER: Generalize the ShouldReturnToPool pattern to be supported in System.Memory natively.

                _hasCycles = false;
                _types.Clear();
                _namedTypes = null;
            }
        }
    }
}
