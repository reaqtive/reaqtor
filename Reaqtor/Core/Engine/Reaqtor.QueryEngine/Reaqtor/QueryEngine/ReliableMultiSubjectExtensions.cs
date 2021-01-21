// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Reaqtor.Reliable;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Utility and extension methods for reliable multi-subjects.
    /// </summary>
    internal static class ReliableMultiSubjectExtensions
    {
        /// <summary>
        /// Creates a reliable observable from a reliable multi-subject.
        /// </summary>
        /// <typeparam name="TInput">The input type of the subject.</typeparam>
        /// <typeparam name="TOutput">The output type of the subject.</typeparam>
        /// <param name="subject">The subject.</param>
        /// <returns>A reliable observer to the subject.</returns>
        /// <remarks>
        /// This method is needed to provide an accessible generic method for calling `CreateObserver()` on a reliable multi-subject.
        /// </remarks>
        public static IReliableObserver<TInput> ToReliableObserver<TInput, TOutput>(this IReliableMultiSubject<TInput, TOutput> subject) => subject.CreateObserver();
    }
}
