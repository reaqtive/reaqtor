// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   PS - 10/17/2014 - Created this type.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1032 // Add other exception constructors. (This exception is only instantiated by the library.)

using System.Runtime.Serialization;

namespace System.Collections.Specialized
{
    /// <summary>
    /// The exception representing a failure to determine the size needed for an EnumDictionary.
    /// </summary>
    [Serializable]
    public sealed class EnumSizeResolutionException : Exception
    {
        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        public EnumSizeResolutionError ErrorCode { get; }

        internal EnumSizeResolutionException(EnumSizeResolutionError errorCode)
            : base()
        {
            ErrorCode = errorCode;
        }

        private EnumSizeResolutionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            ErrorCode = (EnumSizeResolutionError)info.GetInt32(nameof(ErrorCode));
        }

        /// <summary>
        /// Populates a SerializationInfo with the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The SerializationInfo to populate with data.</param>
        /// <param name="context">The destination (see StreamingContext) for this serialization.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            base.GetObjectData(info, context);
            info.AddValue(nameof(ErrorCode), ErrorCode, typeof(EnumSizeResolutionError));
        }
    }
}
