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
using System.Linq;

namespace Nuqleon.DataModel.TypeSystem
{
    /// <summary>
    /// Represents a function data type, i.e. akin to a delegate type.
    /// </summary>
    public class FunctionDataType : DataType
    {
        internal FunctionDataType(Type type, ReadOnlyCollection<DataType> parameterTypes, DataType returnType)
            : base(type)
        {
            ParameterTypes = parameterTypes;
            ReturnType = returnType;
        }

        /// <summary>
        /// Gets the kind of the data type.
        /// </summary>
        public override DataTypeKinds Kind => DataTypeKinds.Function;

        /// <summary>
        /// Gets the parameter types of the function.
        /// </summary>
        public ReadOnlyCollection<DataType> ParameterTypes { get; }

        /// <summary>
        /// Gets the return type of the function.
        /// </summary>
        public DataType ReturnType { get; }

        /// <summary>
        /// Gets a strongly typed function over a value to conforms to the data type.
        /// </summary>
        /// <param name="value">Object to get a strongly typed function for.</param>
        /// <returns>Strongly typed function over the given value.</returns>
        public Delegate GetFunction(object value)
        {
            CheckType(value);
            return (Delegate)value;
        }

        /// <summary>
        /// Creates a new instance of the function data type.
        /// </summary>
        /// <param name="arguments">Only one parameter can be specified, containing the function object.</param>
        /// <returns>Instance of the function data type.</returns>
        public override object CreateInstance(params object[] arguments)
        {
            if (arguments == null)
                throw new ArgumentNullException(nameof(arguments));

            var expr = arguments.Single();
            CheckType(expr);
            return expr;
        }
    }
}
