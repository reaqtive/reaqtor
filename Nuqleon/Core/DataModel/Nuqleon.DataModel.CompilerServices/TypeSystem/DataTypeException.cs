// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
// IG - 2025/12   - Removed CLR serialization.
//

using System;
using System.Globalization;

namespace Nuqleon.DataModel.TypeSystem
{
    /// <summary>
    /// Exception for data type violations, containing diagnostic information.
    /// </summary>
    [Serializable]
    public class DataTypeException : Exception
    {
        /// <summary>
        /// Creates a new data type violation exception.
        /// </summary>
        public DataTypeException()
        {
        }

        /// <summary>
        /// Creates a new data type violation exception with the specified error message.
        /// </summary>
        /// <param name="message">Error message.</param>
        public DataTypeException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates a new data type violation exception with the specified error message and inner exception.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <param name="inner">Inner exception.</param>
        public DataTypeException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Creates a new data type violation exception with the specified diagnostic error object.
        /// </summary>
        /// <param name="error">Diagnostic error object.</param>
        public DataTypeException(DataTypeError error)
            : base(GetErrorMessage(error))
        {
            Error = error ?? throw new ArgumentNullException(nameof(error));
        }

        private static string GetErrorMessage(DataTypeError error)
        {
            if (error == null)
                return null;

            return string.Format(CultureInfo.InvariantCulture, "Data model type checking failed for type '{0}'. {1}", error.Type.ToCSharpString(), error.Message);
        }

        /// <summary>
        /// Gets the diagnostic error object describing the data type violation.
        /// </summary>
        public DataTypeError Error { get; }

        /// <summary>
        /// Returns a friendly string representation of the error.
        /// </summary>
        /// <returns>Friendly string representation of the error.</returns>
        public override string ToString()
        {
            if (Error == null)
            {
                return base.ToString();
            }

            return Error.ToString();
        }
    }
}
