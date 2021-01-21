// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1064 // Exceptions should be public. (Scoped to the query engine; should not escape.)

using System;
using System.Runtime.Serialization;

namespace Reaqtor.QueryEngine
{
    [Serializable]
    internal class InvariantException : Exception
    {
        public InvariantException()
        {
        }

        public InvariantException(string message)
            : base(message)
        {
        }

        public InvariantException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected InvariantException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
