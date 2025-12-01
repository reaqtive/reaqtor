// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// IG - 2025/12 - Remove CLR serialization.
//

using System;
using System.Globalization;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Exception representing the failure to save a reactive entity.
    /// </summary>
    [Serializable]
    public class EntitySaveFailedException : Exception
    {
        /// <summary>
        /// Creates a new instance of the <see cref="EntitySaveFailedException"/> type.
        /// </summary>
        public EntitySaveFailedException()
        {
        }

        /// <summary>
        /// Creates a new <see cref="EntitySaveFailedException"/> instance with the specified <paramref name="message"/>.
        /// </summary>
        /// <param name="message">The message on the exception.</param>
        public EntitySaveFailedException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates a new <see cref="EntitySaveFailedException"/> instance with the specified <paramref name="message"/> and the specified <paramref name="innerException"/>.
        /// </summary>
        /// <param name="message">The message on the exception.</param>
        /// <param name="innerException">The inner exception.</param>
        public EntitySaveFailedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="EntitySaveFailedException"/> type for the entity with the specified URI and entity kind.
        /// </summary>
        /// <param name="uri">URI identifying the entity that failed to be saved.</param>
        /// <param name="type">Reactive entity kind of the entity that failed to be saved.</param>
        public EntitySaveFailedException(Uri uri, ReactiveEntityKind type)
            : base(GetMessage(uri, type))
        {
            EntityUri = uri;
            EntityType = type;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="EntitySaveFailedException"/> type for the entity with the specified URI and entity kind, including an inner exception.
        /// </summary>
        /// <param name="uri">URI identifying the entity that failed to be saved.</param>
        /// <param name="type">Reactive entity kind of the entity that failed to be saved.</param>
        /// <param name="inner">Exception that caused the entity save failure.</param>
        public EntitySaveFailedException(Uri uri, ReactiveEntityKind type, Exception inner)
            : base(GetMessage(uri, type), inner)
        {
            EntityUri = uri;
            EntityType = type;
        }

        /// <summary>
        /// Gets URI identifying the entity that failed to be saved.
        /// </summary>
        public Uri EntityUri { get; }

        /// <summary>
        /// Gets the reactive entity kind of the entity that failed to be saved.
        /// </summary>
        public ReactiveEntityKind EntityType { get; }

        private static string GetMessage(Uri uri, ReactiveEntityKind type)
        {
            return string.Format(CultureInfo.InvariantCulture, "The entity of type '{0}' with identifier '{1}' failed to save.", type, uri.ToCanonicalString());
        }
    }
}
