// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System;

namespace Reaqtor
{
    /// <summary>
    /// Interface for observers of event streams.
    /// </summary>
    /// <typeparam name="T">Type of the data received by the observer.</typeparam>
    public interface IReactiveObserver<in T> : IObserver<T>
    {
        /// <summary>
        /// Sends a value to the stream.
        /// </summary>
        /// <param name="value">Object to send to the stream.</param>
        /// <remarks>If observer calls are not awaited in a sequential manner, the order of events in the stream as observed by the server is undefined.</remarks>
        new void OnNext(T value);

#pragma warning disable CA1716 // Identifiers should not match keywords. (Using error from `IObserver<T>.OnError(Exception error)`.)

        /// <summary>
        /// Reports an error to the stream.
        /// </summary>
        /// <param name="error">Error to report on the stream.</param>
        /// <remarks>If observer calls are not awaited in a sequential manner, the order of events in the stream as observed by the server is undefined.</remarks>
        new void OnError(Exception error);

#pragma warning restore CA1716

        /// <summary>
        /// Reports completion of the stream.
        /// </summary>
        /// <remarks>If observer calls are not awaited in a sequential manner, the order of events in the stream as observed by the server is undefined.</remarks>
        new void OnCompleted();
    }
}
