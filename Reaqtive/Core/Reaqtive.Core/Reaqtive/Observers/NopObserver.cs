// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtive
{
    /// <summary>
    /// Observer implementation that simply ignores any notifications it receives.
    /// </summary>
    /// <typeparam name="TSource">Type of the elements received by the observer.</typeparam>
    public class NopObserver<TSource> : IObserver<TSource>
    {
        /// <summary>
        /// Singleton instance of an observer that ignores any notifications it receives.
        /// </summary>
        public static readonly NopObserver<TSource> Instance = new();

        /// <summary>
        /// Creates a new observer that ignores notifications it receives.
        /// </summary>
        public NopObserver() { }

        /// <summary>
        /// Processes an OnCompleted notification without any side-effects.
        /// </summary>
        public void OnCompleted()
        {
        }

        /// <summary>
        /// Processes an OnError notification without any side-effects.
        /// </summary>
        /// <param name="error">Error received by the observer.</param>
        public void OnError(Exception error)
        {
        }

        /// <summary>
        /// Processes an OnNext notification without any side-effects.
        /// </summary>
        /// <param name="value">Element value received by the observer.</param>
        public void OnNext(TSource value)
        {
        }
    }
}
