// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
//

using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Nuqleon.DataModel.TypeSystem
{
    /// <summary>
    /// Exception for data type violations, containing diagnostic information.
    /// </summary>
    public partial class DataTypeException : Exception
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

#if !NO_SERIALIZATION
    [Serializable]
    public partial class DataTypeException
    {
        /// <summary>
        /// Creates a new data type violation exception from serialization information.
        /// </summary>
        /// <param name="info">Serialization information.</param>
        /// <param name="context">Streaming context.</param>
        protected DataTypeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Error = (DataTypeError)info.GetValue("Error", typeof(DataTypeError));
        }

        /// <summary>
        /// Serializes the object instance to the specified serialization info object.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            base.GetObjectData(info, context);
            info.AddValue("Error", Error);
        }
    }
#endif
}
