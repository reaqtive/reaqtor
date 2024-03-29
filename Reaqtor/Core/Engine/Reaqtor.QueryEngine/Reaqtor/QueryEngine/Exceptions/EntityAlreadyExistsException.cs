﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Exception indicating that an entity with a given identifier already exists.
    /// </summary>
    [Serializable]
    public class EntityAlreadyExistsException : ArgumentException
    {
        /// <summary>
        /// Creates a new instance of the <see cref="EntityAlreadyExistsException"/> class.
        /// </summary>
        public EntityAlreadyExistsException()
        {
        }

        /// <summary>
        /// Creates a new <see cref="EntityAlreadyExistsException"/> instance with the specified <paramref name="message"/>.
        /// </summary>
        /// <param name="message">The message on the exception.</param>
        public EntityAlreadyExistsException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates a new <see cref="EntityAlreadyExistsException"/> instance with the specified <paramref name="message"/> and the specified <paramref name="innerException"/>.
        /// </summary>
        /// <param name="message">The message on the exception.</param>
        /// <param name="innerException">The inner exception.</param>
        public EntityAlreadyExistsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="EntityAlreadyExistsException"/> class with the specified information about the entity.
        /// </summary>
        /// <param name="uri">URI identifying the reactive entity that already exists.</param>
        /// <param name="type">Reactive entity kind.</param>
        /// <param name="qeId">URI identifier the query engine whose registry already contains an entity with the specified URI.</param>
        /// <param name="paramName">Parameter name of the argument containing the URI that caused the conflict.</param>
        public EntityAlreadyExistsException(Uri uri, ReactiveEntityKind type, Uri qeId, string paramName)
            : base(GetMessage(uri, type, qeId), paramName)
        {
            QueryEngineUri = qeId;
            EntityUri = uri;
            EntityType = type;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="EntityAlreadyExistsException"/> class with the specified information about the entity and an inner exception.
        /// </summary>
        /// <param name="uri">URI identifying the reactive entity that already exists.</param>
        /// <param name="type">Reactive entity kind.</param>
        /// <param name="qeId">URI identifier the query engine whose registry already contains an entity with the specified URI.</param>
        /// <param name="paramName">Parameter name of the argument containing the URI that caused the conflict.</param>
        /// <param name="inner">Inner exception with additional information about the naming conflict.</param>
        public EntityAlreadyExistsException(Uri uri, ReactiveEntityKind type, Uri qeId, string paramName, Exception inner)
            : base(GetMessage(uri, type, qeId), paramName, inner)
        {
            QueryEngineUri = qeId;
            EntityUri = uri;
            EntityType = type;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="EntityAlreadyExistsException"/> class from serialized state.
        /// </summary>
        /// <param name="info">Serialization information to deserialize state from.</param>
        /// <param name="context">Streaming context to deserialize state from.</param>
        protected EntityAlreadyExistsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // NB: Names kept for backwards compat.

            QueryEngineUri = (Uri)info.GetValue("QeId", typeof(Uri));
            EntityUri = (Uri)info.GetValue("EntityUri", typeof(Uri));
            EntityType = (ReactiveEntityKind)info.GetValue("EntityKind", typeof(ReactiveEntityKind));
        }

        /// <summary>
        /// Gets the URI identifying the query engine whose registry already contains an entity with <see cref="EntityUri"/> as the entity URI.
        /// </summary>
        public Uri QueryEngineUri { get; }

        /// <summary>
        /// Gets the URI identifying the reactive entity that already exists.
        /// </summary>
        public Uri EntityUri { get; }

        /// <summary>
        /// Gets the entity kind of the reactive entity that already exists.
        /// </summary>
        public ReactiveEntityKind EntityType { get; }

        /// <summary>
        /// Sets the <see cref="System.Runtime.Serialization.SerializationInfo" /> object with the parameter name and additional exception information.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            // NB: Names and order kept for backwards compat.

            info.AddValue("EntityUri", EntityUri, typeof(Uri));
            info.AddValue("EntityKind", EntityType, typeof(ReactiveEntityKind));
            info.AddValue("QeId", QueryEngineUri, typeof(Uri));
        }

        private static string GetMessage(Uri uri, ReactiveEntityKind type, Uri qeId)
        {
            return string.Format(CultureInfo.InvariantCulture, "An entity of type '{0}' with identifier '{1}' already exists in query engine '{2}'.", type, uri.ToCanonicalString(), qeId.ToCanonicalString());
        }
    }
}
