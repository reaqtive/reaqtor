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
    /// Represents a function quotation data type, i.e. an expression representation of a function.
    /// </summary>
    public class QuotationDataType : DataType
    {
        internal QuotationDataType(Type type, FunctionDataType function)
            : base(type)
        {
            Function = function;
        }

        /// <summary>
        /// Gets the kind of the data type.
        /// </summary>
        public override DataTypeKinds Kind => DataTypeKinds.Quotation;

        /// <summary>
        /// Gets the type of the function represented by the quotation.
        /// </summary>
        public FunctionDataType Function { get; }

        /// <summary>
        /// Gets a strongly typed function quotation expression over a value to conforms to the data type.
        /// </summary>
        /// <param name="value">Object to get a strongly typed expression for.</param>
        /// <returns>Strongly typed function quotation expression over the given value.</returns>
        public LambdaExpression GetExpression(object value)
        {
            CheckType(value);
            return (LambdaExpression)value;
        }

        /// <summary>
        /// Creates a new instance of the function quotation data type.
        /// </summary>
        /// <param name="arguments">Only one parameter can be specified, containing the function quotation object.</param>
        /// <returns>Instance of the function quotation data type.</returns>
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
