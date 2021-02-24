// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace Nuqleon.DataModel.TypeSystem
{
    /// <summary>
    /// Abstract base class for data types that conform to the data model.
    /// </summary>
    internal static class DataTypeHelpers
    {
        private static readonly ConditionalWeakTable<Type, StrongBox<bool>> s_isStructuralEntityDataType = new();
        private static readonly ConditionalWeakTable<Type, StrongBox<bool>> s_isEntityEnumDataTypeType = new();

        /// <summary>
        /// Converts a CLR type to a data model type. Support for recursive type definitions can be enabled.
        /// </summary>
        /// <param name="type">CLR type to convert.</param>
        /// <param name="allowCycles">Indicates whether cycles in type definitions are allowed. This is a specialized use case and should be considered carefully, because not all tools can deal with cycles.</param>
        /// <returns>Data model type isomorphic to the given CLR type.</returns>
        /// <remarks>Uses a conditional weak table to cache result.</remarks>
        public static DataType FromTypeCached(Type type, bool allowCycles) => DataType.FromType(type, allowCycles);

        /// <summary>
        /// Tries to convert a CLR type to a data model type. Support for recursive type definitions can be enabled.
        /// </summary>
        /// <param name="type">CLR type to convert.</param>
        /// <param name="allowCycles">Indicates whether cycles in type definitions are allowed. This is a specialized use case and should be considered carefully, because not all tools can deal with cycles.</param>
        /// <param name="result">Data model type isomorphic to the given CLR type.</param>
        /// <returns>true if the conversion succeeded; otherwise, false.</returns>
        /// <remarks>Uses a conditional weak table to cache result.</remarks>
        public static bool TryFromTypeCached(Type type, bool allowCycles, out DataType result) => DataType.TryFromType(type, allowCycles, out result);

        /// <summary>
        /// Checks whether the specified CLR type represents a user-defined structural entity data type.
        /// This method doesn't perform extensive checking of required invariants. Use FromType or Check to ensure the type is a valid entity type.
        /// </summary>
        /// <param name="type">CLR type to check.</param>
        /// <returns>true if the given CLR type represents a user-defined structural entity data type; otherwise, false.</returns>
        /// <remarks>Uses a conditional weak table to cache result.</remarks>
        public static bool IsStructuralEntityDataTypeCached(Type type) => s_isStructuralEntityDataType.GetValue(type, t => new StrongBox<bool>(DataType.IsStructuralEntityDataType(t))).Value;

        /// <summary>
        /// Checks whether the specified CLR type represents a user-defined entity enumeration data type.
        /// This method doesn't perform extensive checking of required invariants. Use FromType or Check to ensure the type is a valid entity type.
        /// </summary>
        /// <param name="type">CLR type to check.</param>
        /// <returns>true if the given CLR type represents a user-defined entity enumeration data type; otherwise, false.</returns>
        /// <remarks>Uses a conditional weak table to cache result.</remarks>
        public static bool IsEntityEnumDataTypeCached(Type type) => s_isEntityEnumDataTypeType.GetValue(type, t => new StrongBox<bool>(DataType.IsEntityEnumDataType(t))).Value;

        /// <summary>
        /// Checks whether the specified CLR type can be converted to a valid data model type. Support for recursive type definitions can be enabled.
        /// Violations against the data model type system are reported as an exception.
        /// </summary>
        /// <param name="type">CLR type to check.</param>
        /// <param name="allowCycles">Indicates whether cycles in type definitions are allowed. This is a specialized use case and should be considered carefully, because not all tools can deal with cycles.</param>
        /// <exception cref="AggregateException">Aggregate exception with DataTypeException inner exception objects to describe violations against the data model type system.</exception>
        /// <remarks>Uses a conditional weak table to cache result.</remarks>
        public static void CheckCached(Type type, bool allowCycles) => DataType.Check(type, allowCycles);

        /// <summary>
        /// Checks whether the specified CLR type can be converted to a valid data model type. Support for recursive type definitions can be enabled.
        /// Violations against the data model type system are reported through the output parameter.
        /// </summary>
        /// <param name="type">CLR type to check.</param>
        /// <param name="allowCycles">Indicates whether cycles in type definitions are allowed. This is a specialized use case and should be considered carefully, because not all tools can deal with cycles.</param>
        /// <param name="errors">Violations against the data model type system.</param>
        /// <returns>true if the given CLR type passes data model type checking; otherwise, false.</returns>
        /// <remarks>Uses a conditional weak table to cache result.</remarks>
        public static bool TryCheckCached(Type type, bool allowCycles, out ReadOnlyCollection<DataTypeError> errors) => DataType.TryCheck(type, allowCycles, out errors);
    }
}
