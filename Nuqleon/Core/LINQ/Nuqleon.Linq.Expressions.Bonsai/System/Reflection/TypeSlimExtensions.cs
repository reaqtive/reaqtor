// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - October 2013 - Created this file.
//

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace System.Reflection
{
    /// <summary>
    /// Provides a set of extension methods for types and lightweight types.
    /// </summary>
    public static class TypeSlimExtensions
    {
#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable IDE0032 // Use auto property. (Consistency.)
#pragma warning disable format // (Formatted as a table.)
        private static readonly GenericDefinitionTypeSlim _genericExpression = (GenericDefinitionTypeSlim)typeof(Expression<>).ToTypeSlim();
        private static readonly GenericDefinitionTypeSlim _nullable = (GenericDefinitionTypeSlim)typeof(Nullable<>).ToTypeSlim();

        private static readonly SimpleTypeSlim _action   = (SimpleTypeSlim)typeof(Action  ).ToTypeSlim();
        private static readonly SimpleTypeSlim _bool     = (SimpleTypeSlim)typeof(bool    ).ToTypeSlim();
        private static readonly SimpleTypeSlim _int16    = (SimpleTypeSlim)typeof(short   ).ToTypeSlim();
        private static readonly SimpleTypeSlim _int32    = (SimpleTypeSlim)typeof(int     ).ToTypeSlim();
        private static readonly SimpleTypeSlim _int64    = (SimpleTypeSlim)typeof(long    ).ToTypeSlim();
        private static readonly SimpleTypeSlim _uint16   = (SimpleTypeSlim)typeof(ushort  ).ToTypeSlim();
        private static readonly SimpleTypeSlim _uint32   = (SimpleTypeSlim)typeof(uint    ).ToTypeSlim();
        private static readonly SimpleTypeSlim _uint64   = (SimpleTypeSlim)typeof(ulong   ).ToTypeSlim();
        private static readonly SimpleTypeSlim _byte     = (SimpleTypeSlim)typeof(byte    ).ToTypeSlim();
        private static readonly SimpleTypeSlim _sbyte    = (SimpleTypeSlim)typeof(sbyte   ).ToTypeSlim();
        private static readonly SimpleTypeSlim _single   = (SimpleTypeSlim)typeof(float   ).ToTypeSlim();
        private static readonly SimpleTypeSlim _double   = (SimpleTypeSlim)typeof(double  ).ToTypeSlim();
        private static readonly SimpleTypeSlim _decimal  = (SimpleTypeSlim)typeof(decimal ).ToTypeSlim();
        private static readonly SimpleTypeSlim _dateTime = (SimpleTypeSlim)typeof(DateTime).ToTypeSlim();
        private static readonly SimpleTypeSlim _char     = (SimpleTypeSlim)typeof(char    ).ToTypeSlim();
        private static readonly SimpleTypeSlim _string   = (SimpleTypeSlim)typeof(string  ).ToTypeSlim();
        private static readonly SimpleTypeSlim _void     = (SimpleTypeSlim)typeof(void    ).ToTypeSlim();
#pragma warning restore format
#pragma warning restore IDE0032
#pragma warning restore IDE0079

        private static readonly ConcurrentDictionary<string, GenericDefinitionTypeSlim> _delegateTypes = new();

        #region Type Conversion

        /// <summary>
        /// Converts the specified type to a lightweight representation.
        /// </summary>
        /// <param name="type">Type to convert.</param>
        /// <returns>Lightweight representation of the specified type.</returns>
        public static TypeSlim ToTypeSlim(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return new TypeToTypeSlimConverter().Visit(type);
        }

        // TODO: Introduce symmetric reflection provider concepts for the slim space.

        /// <summary>
        /// Converts the lightweight representation of a type to a type.
        /// </summary>
        /// <param name="type">Slim type to convert.</param>
        /// <returns>Type represented by the slim type.</returns>
        public static Type ToType(this TypeSlim type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return new TypeSlimToTypeConverter(DefaultReflectionProvider.Instance).Visit(type);
        }

        /// <summary>
        /// Converts the lightweight representation of a type to a type.
        /// </summary>
        /// <param name="type">Slim type to convert.</param>
        /// <param name="provider">Reflection provider to use for type lookup and type construction.</param>
        /// <returns>Type represented by the slim type.</returns>
        public static Type ToType(this TypeSlim type, IReflectionProvider provider)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            // CONSIDER: Should we accept more granular reflection provider interfaces such
            //           as IReflectionLoadingProvider and IReflectionCreationProvider?

            return new TypeSlimToTypeConverter(provider).Visit(type);
        }

        #endregion

        #region Reflection

        /// <summary>
        /// Get a slim representation of a constructor for the type.
        /// </summary>
        /// <param name="type">Type declaring the member.</param>
        /// <param name="parameterTypes">Types of the parameters.</param>
        /// <returns>The slim constructor info.</returns>
        public static ConstructorInfoSlim GetConstructor(this TypeSlim type, ReadOnlyCollection<TypeSlim> parameterTypes)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (parameterTypes == null)
                throw new ArgumentNullException(nameof(parameterTypes));

            return new ConstructorInfoSlim(type, parameterTypes);
        }

        /// <summary>
        /// Get a slim representation of a property for the type.
        /// </summary>
        /// <param name="type">Type declaring the member.</param>
        /// <param name="name">Name of the property.</param>
        /// <param name="propertyType">Type of the property.</param>
        /// <param name="indexParameterTypes">Types of the indexer parameters, if the property is indexed. Otherwise, an empty collection.</param>
        /// <param name="canWrite">true if property is writable, false otherwise</param>
        /// <returns>The slim property info.</returns>
        public static PropertyInfoSlim GetProperty(this TypeSlim type, string name, TypeSlim propertyType, ReadOnlyCollection<TypeSlim> indexParameterTypes, bool canWrite)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (name.Length == 0)
                throw new ArgumentException("Property name can not be empty.", nameof(name));
            if (indexParameterTypes == null)
                throw new ArgumentNullException(nameof(indexParameterTypes));

            return PropertyInfoSlim.Make(type, name, propertyType, indexParameterTypes, canWrite);
        }

        /// <summary>
        /// Get a slim representation of a field for the type.
        /// </summary>
        /// <param name="type">Type declaring the member.</param>
        /// <param name="name">Name of the field.</param>
        /// <param name="fieldType">Type of the field.</param>
        /// <returns>The slim field info.</returns>
        public static FieldInfoSlim GetField(this TypeSlim type, string name, TypeSlim fieldType)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (name.Length == 0)
                throw new ArgumentException("Field name can not be empty.", nameof(name));

            return new FieldInfoSlim(type, name, fieldType);
        }

        /// <summary>
        /// Get a slim representation of a simple method for the type.
        /// </summary>
        /// <param name="type">Type declaring the member.</param>
        /// <param name="name">Name of the method.</param>
        /// <param name="parameterTypes">Type of the method parameters.</param>
        /// <param name="returnType">Return type of the method.</param>
        /// <returns>The slim simple method info.</returns>
        public static SimpleMethodInfoSlim GetSimpleMethod(this TypeSlim type, string name, ReadOnlyCollection<TypeSlim> parameterTypes, TypeSlim returnType)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (name.Length == 0)
                throw new ArgumentException("Method name can not be empty.", nameof(name));
            if (parameterTypes == null)
                throw new ArgumentNullException(nameof(parameterTypes));

            return new SimpleMethodInfoSlim(type, name, parameterTypes, returnType);
        }

        /// <summary>
        /// Get a slim representation of a generic definition method for the type.
        /// </summary>
        /// <param name="type">Type declaring the member.</param>
        /// <param name="name">Name of the method.</param>
        /// <param name="genericParameterTypes">Generic parameter types.</param>
        /// <param name="parameterTypes">Type of the method parameters.</param>
        /// <param name="returnType">Return type of the method.</param>
        /// <returns>The slim generic definition method info.</returns>
        public static GenericDefinitionMethodInfoSlim GetGenericDefinitionMethod(this TypeSlim type, string name, ReadOnlyCollection<TypeSlim> genericParameterTypes, ReadOnlyCollection<TypeSlim> parameterTypes, TypeSlim returnType)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (name.Length == 0)
                throw new ArgumentException("Method name can not be empty.", nameof(name));
            if (parameterTypes == null)
                throw new ArgumentNullException(nameof(parameterTypes));
            if (genericParameterTypes == null)
                throw new ArgumentNullException(nameof(genericParameterTypes));
            if (genericParameterTypes.Count < 1)
                throw new ArgumentOutOfRangeException(nameof(genericParameterTypes));

            return new GenericDefinitionMethodInfoSlim(type, name, genericParameterTypes, parameterTypes, returnType);
        }

        /// <summary>
        /// Get a slim representation of a closed generic method for the type.
        /// </summary>
        /// <param name="type">Type declaring the member.</param>
        /// <param name="methodDefinition">Generic method definition.</param>
        /// <param name="arguments">Generic type arguments.</param>
        /// <returns>The slim closed generic method info.</returns>
        public static GenericMethodInfoSlim GetGenericMethod(this TypeSlim type, GenericDefinitionMethodInfoSlim methodDefinition, ReadOnlyCollection<TypeSlim> arguments)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (methodDefinition == null)
                throw new ArgumentNullException(nameof(methodDefinition));
            if (arguments == null)
                throw new ArgumentNullException(nameof(arguments));

            return new GenericMethodInfoSlim(type, methodDefinition, arguments);
        }

        #endregion

        #region Generic Types

        internal static bool IsGenericType(this TypeSlim type)
        {
            return type.Kind is TypeSlimKind.Generic or TypeSlimKind.GenericDefinition;
        }

        internal static bool IsGenericTypeDefinition(this TypeSlim type)
        {
            return type.Kind == TypeSlimKind.GenericDefinition;
        }

        internal static TypeSlim GetGenericTypeDefinition(this TypeSlim type)
        {
            if (type.Kind == TypeSlimKind.GenericDefinition)
            {
                return (GenericDefinitionTypeSlim)type;
            }
            else if (type.Kind == TypeSlimKind.Generic)
            {
                return ((GenericTypeSlim)type).GenericTypeDefinition;
            }

            throw new InvalidOperationException("The specified type is not a generic type.");
        }

        internal static TypeSlim[] GetGenericArguments(this TypeSlim type)
        {
            if (type.Kind == TypeSlimKind.Generic)
            {
                var genericType = (GenericTypeSlim)type;
                var genericArgumentCount = genericType.GenericArgumentCount;

                var res = new TypeSlim[genericArgumentCount];

                for (var i = 0; i < genericArgumentCount; i++)
                {
                    res[i] = genericType.GetGenericArgument(i);
                }

                return res;
            }
            else if (type.Kind == TypeSlimKind.GenericDefinition)
            {
                // TODO: implement this by adding generic parameter types to generic type definitions
                throw new NotImplementedException();
            }

            return Array.Empty<TypeSlim>();
        }

        internal static TypeSlim MakeGenericType(this TypeSlim type, ReadOnlyCollection<TypeSlim> args)
        {
            if (type.Kind == TypeSlimKind.GenericDefinition)
            {
                return TypeSlim.Generic((GenericDefinitionTypeSlim)type, args);
            }

            throw new InvalidOperationException("The specified type is not an open generic type.");
        }

        #endregion

        #region Type Derivation

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable IDE0032 // Use auto property. (Consistency.)
        internal static SimpleTypeSlim ActionType => _action;

        internal static SimpleTypeSlim BooleanType => _bool;

        internal static GenericDefinitionTypeSlim GenericExpressionType => _genericExpression;

        internal static SimpleTypeSlim IntegerType => _int32;

        internal static SimpleTypeSlim VoidType => _void;
#pragma warning restore IDE0032
#pragma warning restore IDE0079

        internal static GenericDefinitionTypeSlim GetActionType(int parameterCount) => GetDelegateType("System.Action", parameterCount);

        internal static GenericDefinitionTypeSlim GetFuncType(int parameterCount) => GetDelegateType("System.Func", parameterCount + 1);

        private static GenericDefinitionTypeSlim GetDelegateType(string prefix, int arity)
        {
            var name = prefix + "`" + arity;

            var result = _delegateTypes.GetOrAdd(name, n =>
            {
                var type = Type.GetType(n, throwOnError: true);
                return (GenericDefinitionTypeSlim)type.ToTypeSlim();
            });

            return result;
        }

        internal static TypeSlim GetNonNullableType(this TypeSlim typeSlim)
        {
            if (typeSlim is GenericTypeSlim genericTypeSlim)
            {
                if (_nullable.Equals(genericTypeSlim.GenericTypeDefinition))
                {
                    return genericTypeSlim.GetGenericArgument(0);
                }
            }

            return typeSlim;
        }

        internal static TypeSlim GetNullableType(this TypeSlim typeSlim)
        {
            // REVIEW: Do we need to call IsValueType here? It causes TypeSlim to Type conversion.

            if (typeSlim.IsValueType() && !typeSlim.IsNullableType())
            {
                return TypeSlim.Generic(_nullable, typeSlim);
            }

            return typeSlim;
        }

        internal static bool IsActionType(this TypeSlim typeSlim) => _action.Equals(typeSlim);

        internal static bool IsArithmetic(this TypeSlim typeSlim)
        {
            var nonNullable = typeSlim.GetNonNullableType();
            if (nonNullable is SimpleTypeSlim s)
            {
                switch (s.GetTypeCodeExact())
                {
                    case TypeCode.Double:
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                    case TypeCode.Single:
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                    case TypeCode.UInt64:
                        return true;
                }
            }

            return false;
        }

        internal static bool IsBooleanType(this TypeSlim typeSlim) => _bool.Equals(typeSlim);

        internal static bool IsStringType(this TypeSlim typeSlim) => _string.Equals(typeSlim);

        internal static bool IsInteger(this TypeSlim typeSlim)
        {
            var nonNullable = typeSlim.GetNonNullableType();
            if (nonNullable is SimpleTypeSlim s)
            {
                switch (s.GetTypeCodeExact())
                {
                    case TypeCode.Byte:
                    case TypeCode.SByte:
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                    case TypeCode.UInt64:
                        return true;
                }
            }

            return false;
        }

        internal static bool IsIntegerOrBool(this TypeSlim typeSlim)
        {
            var nonNullable = typeSlim.GetNonNullableType();
            if (nonNullable is SimpleTypeSlim s)
            {
                switch (s.GetTypeCodeExact())
                {
                    case TypeCode.Boolean:
                    case TypeCode.Byte:
                    case TypeCode.SByte:
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                    case TypeCode.UInt64:
                        return true;
                }
            }

            return false;
        }

        internal static bool IsNullableType(this TypeSlim typeSlim) => typeSlim != GetNonNullableType(typeSlim);

        internal static bool IsNumeric(this TypeSlim typeSlim) => IsNumeric(typeSlim, /* For compat with similar BCL APIs. */ includeDecimal: false);

        internal static bool IsNumeric(this TypeSlim typeSlim, bool includeDecimal)
        {
            var nonNullable = typeSlim.GetNonNullableType();
            if (nonNullable is SimpleTypeSlim s)
            {
                switch (s.GetTypeCodeExact())
                {
                    case TypeCode.Byte:
                    case TypeCode.Char:
                    case TypeCode.Double:
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                    case TypeCode.SByte:
                    case TypeCode.Single:
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                    case TypeCode.UInt64:
                        return true;
                    case TypeCode.Decimal:
                        return includeDecimal;
                }
            }

            return false;
        }

        internal static bool IsUnsignedInt(this TypeSlim typeSlim)
        {
            var nonNullable = typeSlim.GetNonNullableType();
            if (nonNullable is SimpleTypeSlim s)
            {
                switch (s.GetTypeCodeExact())
                {
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                    case TypeCode.UInt64:
                        return true;
                }
            }

            return false;
        }

        internal static bool IsValueType(this TypeSlim typeSlim)
        {
            // REVIEW: Can we avoid calls to ToType here?

            if (typeSlim is SimpleTypeSlim)
            {
                return typeSlim.ToType().IsValueType;
            }

            var genDef = typeSlim as GenericDefinitionTypeSlim;

            if (genDef == null)
            {
                if (typeSlim is GenericTypeSlim genType)
                {
                    genDef = genType.GenericTypeDefinition;
                }
            }

            if (genDef != null)
            {
                return genDef.ToType().IsValueType;
            }

            return false;
        }

        internal static bool IsVoidType(this TypeSlim typeSlim) => _void.Equals(typeSlim);

        private static Dictionary<SimpleTypeSlim, TypeCode> s_typeCodes;

        internal static TypeCode GetTypeCodeExact(this SimpleTypeSlim typeSlim)
        {
            // NB: This method does not check for enum types unlike Type.GetTypeCode,
            //     hence the suffix "Exact" in its name.

            if (s_typeCodes == null)
            {
                s_typeCodes =
                    new Dictionary<SimpleTypeSlim, TypeCode>()
                    {
                        { _bool,     TypeCode.Boolean  },
                        { _int32,    TypeCode.Int32    },
                        { _uint32,   TypeCode.UInt32   },
                        { _int64,    TypeCode.Int64    },
                        { _uint64,   TypeCode.UInt64   },
                        { _int16,    TypeCode.Int16    },
                        { _uint16,   TypeCode.UInt16   },
                        { _byte,     TypeCode.Byte     },
                        { _sbyte,    TypeCode.SByte    },
                        { _single,   TypeCode.Single   },
                        { _double,   TypeCode.Double   },
                        { _decimal,  TypeCode.Decimal  },
                        { _dateTime, TypeCode.DateTime },
                        { _char,     TypeCode.Char     },
                        { _string,   TypeCode.String   },
                    };
            }

            if (s_typeCodes.TryGetValue(typeSlim, out TypeCode typeCode))
            {
                return typeCode;
            }

            return TypeCode.Object;
        }

        #endregion
    }
}
