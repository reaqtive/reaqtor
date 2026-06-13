// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Reaqtor.Remoting.Protocol.FaultHandling
{
#pragma warning disable CA1032 // Implement standard exception constructors. (Overloads with default parameters for transient suffice.)

    /// <summary>Base exception for all exceptions thrown by RIPP</summary>
    [Serializable]
    public class BaseException : Exception
    {
        /// <summary>
        /// A flag indicating if the exceptions is due to a transient error. Transient errors indicate errors that are potentially resolved with retries.
        /// </summary>
        private readonly bool _transient;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseException"/> class.
        /// </summary>
        /// <param name="transient">A flag indicating if the exception is due to a transient error</param>
        public BaseException(bool transient = false)
            : base()
        {
            _transient = transient;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="transient">A flag indicating if the exception is due to a transient error</param>
        public BaseException(string message, bool transient = false)
            : base(message)
        {
            _transient = transient;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The inner exception</param>
        /// <param name="transient">A flag indicating if the exception is due to a transient error</param>
        public BaseException(string message, Exception exception, bool transient = false)
            : base(message, exception)
        {
            _transient = transient;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseException"/> class.
        /// </summary>
        /// <param name="serializationInfo">Serialization info.</param>
        /// <param name="streamingContext">Serialization context.</param>
        protected BaseException(SerializationInfo serializationInfo, StreamingContext streamingContext)
        {
            Debug.Assert(serializationInfo != null);

            _transient = serializationInfo.GetBoolean(nameof(IsTransient));
        }

        /// <summary>
        /// Gets a flag indicating if the error was due to a transient error
        /// </summary>
        /// <returns><code>true</code> if the exception is due to a transient error</returns>
        public bool IsTransient() => _transient;

        /// <summary>
        /// When overridden in a derived class, sets the <see cref="SerializationInfo" /> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="SerializationFormatter" />
        /// </PermissionSet>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            info.AddValue(nameof(IsTransient), IsTransient());

            base.GetObjectData(info, context);
        }
    }
}
