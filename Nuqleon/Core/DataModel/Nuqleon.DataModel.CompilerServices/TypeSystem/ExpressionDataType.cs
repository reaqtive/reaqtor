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
using System.Linq.Expressions;

namespace Nuqleon.DataModel.TypeSystem
{
    /// <summary>
    /// Represents an expression data type, i.e. a code-as-data representation.
    /// </summary>
    public class ExpressionDataType : DataType
    {
        internal ExpressionDataType(Type type)
            : base(type)
        {
            Type = type;
        }

        /// <summary>
        /// Gets the kind of the data type.
        /// </summary>
        public override DataTypeKinds Kind => DataTypeKinds.Expression;

        /// <summary>
        /// Gets the underlying type used to represent the expression.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Gets a strongly typed expression over a value to conforms to the data type.
        /// </summary>
        /// <param name="value">Object to get a strongly typed expression for.</param>
        /// <returns>Strongly typed expression over the given value.</returns>
        public Expression GetExpression(object value)
        {
            CheckType(value);
            return (Expression)value;
        }

        /// <summary>
        /// Creates a new instance of the expression data type.
        /// </summary>
        /// <param name="arguments">Only one parameter can be specified, containing the expression object.</param>
        /// <returns>Instance of the expression data type.</returns>
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
