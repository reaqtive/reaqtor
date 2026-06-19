// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtive
{
    /// <summary>
    /// Observer implementation that throws an exception for each access.
    /// </summary>
    /// <typeparam name="TSource">Type of the elements received by the observer.</typeparam>
    public class FaultObserver<TSource> : IObserver<TSource>
    {
        private readonly Func<Exception> _getFault;

        /// <summary>
        /// Singleton instance of an observer that throws ObjectDisposedException for each access.
        /// </summary>
        public static readonly IObserver<TSource> Disposed = new FaultObserver<TSource>(() => new ObjectDisposedException("this"));

        /// <summary>
        /// Creates a new observer that throws an exception for each access.
        /// </summary>
        /// <param name="getFault">Function to get the exception to throw for each access to the observer.</param>
        public FaultObserver(Func<Exception> getFault)
        {
            _getFault = getFault ?? throw new ArgumentNullException(nameof(getFault));
        }

        /// <summary>
        /// Processes a completion notification by throwing a fault.
        /// </summary>
        public void OnCompleted()
        {
            throw _getFault();
        }

        /// <summary>
        /// Processes an error notification by throwing a fault.
        /// </summary>
        /// <param name="error">Error to process.</param>
        public void OnError(Exception error)
        {
            throw _getFault();
        }

        /// <summary>
        /// Processes a value notification by throwing a fault.
        /// </summary>
        /// <param name="value">Value to process.</param>
        public void OnNext(TSource value)
        {
            throw _getFault();
        }
    }
}
