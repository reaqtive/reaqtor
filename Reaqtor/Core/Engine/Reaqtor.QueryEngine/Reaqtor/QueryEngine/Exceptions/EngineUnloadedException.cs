// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.Serialization;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Exception used to signal the engine has unloaded.
    /// </summary>
    public partial class EngineUnloadedException : InvalidOperationException
    {
        private const string UNLOADED = "The engine has been unloaded and cannot serve further requests.";

        /// <summary>
        /// Creates a new instance of the <see cref="EngineUnloadedException"/> class.
        /// </summary>
        public EngineUnloadedException()
            : base(UNLOADED)
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="EngineUnloadedException"/> class with the specified error message.
        /// </summary>
        /// <param name="message">Error message.</param>
        public EngineUnloadedException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="EngineUnloadedException"/> class with the specified inner exception.
        /// </summary>
        /// <param name="innerException">Inner exception.</param>
        public EngineUnloadedException(Exception innerException)
            : base(UNLOADED, innerException)
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="EngineUnloadedException"/> class with the specified error message and inner exception.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <param name="innerException">Inner exception.</param>
        public EngineUnloadedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

#if !NO_SERIALIZATION
    [Serializable]
    public partial class EngineUnloadedException
    {
        /// <summary>
        /// Creates a new instance of the <see cref="EngineUnloadedException"/> class from serialized state.
        /// </summary>
        /// <param name="info">Serialization information to deserialize state from.</param>
        /// <param name="context">Streaming context to deserialize state from.</param>
        protected EngineUnloadedException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
#endif
}
