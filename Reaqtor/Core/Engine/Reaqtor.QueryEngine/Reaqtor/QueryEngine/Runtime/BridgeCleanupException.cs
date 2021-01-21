// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1064 // Exceptions should be public. (Scoped to the query engine; should not escape.)

using System;

namespace Reaqtor.QueryEngine
{
    [Serializable]
    internal class BridgeCleanupException : Exception
    {
        public BridgeCleanupException() { }
        public BridgeCleanupException(string message) : base(message) { }
        public BridgeCleanupException(string message, Exception inner) : base(message, inner) { }
        protected BridgeCleanupException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
