// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.QueryEngine.Events
{
    /// <summary>
    /// Event arguments for an <see cref="CheckpointingQueryEngine.EntityReplayFailed"/> event.
    /// </summary>
    public class ReactiveEntityReplayFailedEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ReactiveEntityReplayFailedEventArgs"/> class for the specified entity.
        /// </summary>
        /// <param name="uri">URI of the reactive entity.</param>
        /// <param name="error">The error that occured during load.</param>
        public ReactiveEntityReplayFailedEventArgs(Uri uri, Exception error)
        {
            Uri = uri;
            Error = error;
        }

        /// <summary>
        /// Gets the URI of the reactive entity.
        /// </summary>
        public Uri Uri { get; }

        /// <summary>
        /// Gets the exception that occurred during replay.
        /// </summary>
        public Exception Error { get; }

        /// <summary>
        /// Gets or sets whether the error was handled.
        /// </summary>
        public bool Handled { get; set; }
    }
}
