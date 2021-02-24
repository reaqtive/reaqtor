// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System;
using System.Linq;

namespace Nuqleon.DataModel.TypeSystem
{
    /// <summary>
    /// Represents a primitive data type, i.e. a type treated as an atom by the data model.
    /// </summary>
    public class PrimitiveDataType : DataType
    {
        private readonly Type _type;

        internal PrimitiveDataType(Type type, PrimitiveDataTypeKinds kind)
            : base(type)
        {
            _type = type;
            PrimitiveKind = kind;
        }

        /// <summary>
        /// Gets the kind of the data type.
        /// </summary>
        public override DataTypeKinds Kind => DataTypeKinds.Primitive;

        /// <summary>
        /// Indicates whether the type has a null value.
        /// </summary>
        public bool IsNullable => !_type.IsValueType || Nullable.GetUnderlyingType(_type) != null;

        /// <summary>
        /// Gets the primitive data type kind.
        /// </summary>
        public PrimitiveDataTypeKinds PrimitiveKind { get; }

        /// <summary>
        /// Creates a new instance of the primitive data type.
        /// </summary>
        /// <param name="arguments">Only one parameter can be specified, containing the value.</param>
        /// <returns>Instance of the primitive data type.</returns>
        public override object CreateInstance(params object[] arguments)
        {
            if (arguments == null)
                throw new ArgumentNullException(nameof(arguments));

            var value = arguments.Single();

            if (value == null)
            {
                if (!IsNullable)
                {
                    throw new InvalidOperationException("Cannot assign a null value to a value of non-nullable primitive data type.");
                }
            }

            CheckType(value);

            return value;
        }
    }
}
