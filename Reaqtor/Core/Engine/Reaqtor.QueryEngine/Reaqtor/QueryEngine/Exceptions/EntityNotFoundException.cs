// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Exception indicating that an entity with a given identifier was not found.
    /// </summary>
    public partial class EntityNotFoundException : ArgumentException
    {
        /// <summary>
        /// Creates a new instance of the <see cref="EntityNotFoundException"/> class.
        /// </summary>
        public EntityNotFoundException()
        {
        }

        /// <summary>
        /// Creates a new <see cref="EntityNotFoundException"/> instance with the specified <paramref name="message"/>.
        /// </summary>
        /// <param name="message">The message on the exception.</param>
        public EntityNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates a new <see cref="EntityNotFoundException"/> instance with the specified <paramref name="message"/> and the specified <paramref name="innerException"/>.
        /// </summary>
        /// <param name="message">The message on the exception.</param>
        /// <param name="innerException">The inner exception.</param>
        public EntityNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="EntityNotFoundException"/> class with the specified information about the entity.
        /// </summary>
        /// <param name="uri">URI identifying the reactive entity that was not found.</param>
        /// <param name="type">Reactive entity kind.</param>
        /// <param name="qeId">URI identifier the query engine whose registry does not contain an entity with the specified URI.</param>
        /// <param name="paramName">Parameter name of the argument containing the URI of the entity that was not found.</param>
        public EntityNotFoundException(Uri uri, ReactiveEntityKind type, Uri qeId, string paramName)
            : base(GetMessage(uri, type, qeId), paramName)
        {
            QueryEngineUri = qeId;
            EntityUri = uri;
            EntityType = type;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="EntityNotFoundException"/> class with the specified information about the entity and an inner exception.
        /// </summary>
        /// <param name="uri">URI identifying the reactive entity that was not found.</param>
        /// <param name="type">Reactive entity kind.</param>
        /// <param name="qeId">URI identifier the query engine whose registry does not contain an entity with the specified URI.</param>
        /// <param name="paramName">Parameter name of the argument containing the URI of the entity that was not found.</param>
        /// <param name="inner">Inner exception with additional information about the lookup failure.</param>
        public EntityNotFoundException(Uri uri, ReactiveEntityKind type, Uri qeId, string paramName, Exception inner)
            : base(GetMessage(uri, type, qeId), paramName, inner)
        {
            QueryEngineUri = qeId;
            EntityUri = uri;
            EntityType = type;
        }

        /// <summary>
        /// Gets the URI identifying the query engine whose registry does not contain an entity with <see cref="EntityUri"/> as the entity URI.
        /// </summary>
        public Uri QueryEngineUri { get; }

        /// <summary>
        /// Gets the URI identifying the reactive entity that was not found.
        /// </summary>
        public Uri EntityUri { get; }

        /// <summary>
        /// Gets the entity kind of the reactive entity that was not found.
        /// </summary>
        public ReactiveEntityKind EntityType { get; }

        private static string GetMessage(Uri uri, ReactiveEntityKind type, Uri qeId)
        {
            return string.Format(CultureInfo.InvariantCulture, "An entity of type '{0}' with identifier '{1}' could not be found in query engine '{2}'.", type, uri.ToCanonicalString(), qeId);
        }
    }

#if !NO_SERIALIZATION
    [Serializable]
    public partial class EntityNotFoundException
    {
        /// <summary>
        /// Creates a new instance of the <see cref="EntityNotFoundException"/> class from serialized state.
        /// </summary>
        /// <param name="info">Serialization information to deserialize state from.</param>
        /// <param name="context">Streaming context to deserialize state from.</param>
        protected EntityNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            EntityUri = (Uri)info.GetValue("EntityUri", typeof(Uri));
            EntityType = (ReactiveEntityKind)info.GetValue("EntityKind", typeof(ReactiveEntityKind));
            QueryEngineUri = (Uri)info.GetValue("QeId", typeof(Uri));
        }

        /// <summary>
        /// Sets the <see cref="System.Runtime.Serialization.SerializationInfo" /> object with the parameter name and additional exception information.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("EntityUri", EntityUri, typeof(Uri));
            info.AddValue("EntityKind", EntityType, typeof(ReactiveEntityKind));
            info.AddValue("QeId", QueryEngineUri, typeof(Uri));
        }
    }
#endif
}
