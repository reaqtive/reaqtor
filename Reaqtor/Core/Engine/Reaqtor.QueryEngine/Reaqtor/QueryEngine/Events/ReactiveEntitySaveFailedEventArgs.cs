﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtor.Metadata;

namespace Reaqtor.QueryEngine.Events
{
    /// <summary>
    /// Event arguments for an <see cref="CheckpointingQueryEngine.EntitySaveFailed"/> event.
    /// </summary>
    public class ReactiveEntitySaveFailedEventArgs : ReactiveEntityEventArgs
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ReactiveEntityEventArgs"/> class for the specified entity.
        /// </summary>
        /// <param name="uri">URI of the reactive entity.</param>
        /// <param name="entity">Reactive entity.</param>
        /// <param name="entityType">Kind of the reactive entity.</param>
        /// <param name="error">The error that occured during save.</param>
        public ReactiveEntitySaveFailedEventArgs(Uri uri, IReactiveResource entity, ReactiveEntityKind entityType, Exception error)
            : base(uri, entity, entityType)
        {
            Error = error;
        }

        /// <summary>
        /// Gets the exception that occurred during load.
        /// </summary>
        public Exception Error { get; }

        /// <summary>
        /// Gets or sets whether the error was handled.
        /// </summary>
        public bool Handled { get; set; }
    }
}
