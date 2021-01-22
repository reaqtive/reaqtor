// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - September 2013 - Created this file.
//

using System;

namespace Nuqleon.DataModel.TypeSystem
{
    /// <summary>
    /// Represents an open generic parameter type, i.e. a wildcard type.
    /// </summary>
    public class OpenGenericParameterDataType : DataType
    {
        internal OpenGenericParameterDataType(Type type)
            : base(type)
        {
        }

        /// <summary>
        /// Gets the kind of the data type.
        /// </summary>
        public override DataTypeKinds Kind => DataTypeKinds.OpenGenericParameter;

        /// <summary>
        /// Creates a new instance of the open generic parameter type.
        /// </summary>
        /// <param name="arguments">Irrelevant; this method should not be called for this data type.</param>
        /// <returns>Irrelevant; this method should not be called for this data type.</returns>
        /// <exception cref="System.InvalidOperationException">
        /// Always thrown; should not try to create an instance of an open generic parameter type.
        /// </exception>
        public override object CreateInstance(params object[] arguments) => throw new InvalidOperationException("Cannot create an instance of an open generic parameter type.");
    }
}
