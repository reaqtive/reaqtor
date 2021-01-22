// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Nuqleon.DataModel.TypeSystem
{
    /// <summary>
    /// Represents an array data type, i.e. a single-dimensional array with a data model type as the element type.
    /// </summary>
    public class ArrayDataType : DataType
    {
        internal ArrayDataType(Type type, DataType elementType)
            : base(type)
        {
            ElementType = elementType;
        }

        /// <summary>
        /// Gets the kind of the data type.
        /// </summary>
        public override DataTypeKinds Kind => DataTypeKinds.Array;

        /// <summary>
        /// Gets the element type of the array.
        /// </summary>
        public DataType ElementType { get; }

        /// <summary>
        /// Gets a list accessor over a value to conforms to the data type.
        /// </summary>
        /// <param name="value">Object to get a list accessor for.</param>
        /// <returns>List accessor over the given value.</returns>
        public IList GetList(object value)
        {
            CheckType(value);
            return (IList)value;
        }

        /// <summary>
        /// Creates a new instance of the array data type.
        /// </summary>
        /// <param name="arguments">Only one parameter can be specified, containing the size of the array.</param>
        /// <returns>Instance of the array data type.</returns>
        public override object CreateInstance(params object[] arguments)
        {
            if (arguments == null)
                throw new ArgumentNullException(nameof(arguments));

            var length = (int)arguments.Single();

            if (UnderlyingType.IsArray)
            {
                return Array.CreateInstance(ElementType.UnderlyingType, length);
            }
            else
            {
                Type list;

                if (UnderlyingType.IsInterface) // Important: keep in sync with DataModelTypeVisitorBase code.
                {
                    list = typeof(List<>).MakeGenericType(ElementType.UnderlyingType);
                }
                else
                {
                    list = UnderlyingType;
                }

                return Activator.CreateInstance(list, arguments);
            }
        }
    }
}
