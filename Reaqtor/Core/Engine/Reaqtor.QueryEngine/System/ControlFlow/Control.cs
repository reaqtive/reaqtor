// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Threading;

namespace System.ControlFlow
{
    /// <summary>
    /// Fragment of a control flow programming framework.
    /// </summary>
    internal static class Control
    {
        /// <summary>
        /// Invokes the specified <paramref name="action"/> for each element in the <paramref name="source"/> sequence.
        /// </summary>
        /// <typeparam name="T">The elements in the source sequence.</typeparam>
        /// <param name="source">The source to iterate over.</param>
        /// <param name="action">The action to invoke for each element in the sequence.</param>
        /// <param name="token">The cancellation token used to support cancellation of the iteration and invocation of the actions.</param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T, CancellationToken> action, CancellationToken token) => ForEach(source, action, throwOnCancel: true, token: token);

        /// <summary>
        /// Invokes the specified <paramref name="action"/> for each element in the <paramref name="source"/> sequence.
        /// </summary>
        /// <typeparam name="T">The elements in the source sequence.</typeparam>
        /// <param name="source">The source to iterate over.</param>
        /// <param name="action">The action to invoke for each element in the sequence.</param>
        /// <param name="throwOnCancel">true to throw if cancellation is requested; otherwise, false, to bail out silently.</param>
        /// <param name="token">The cancellation token used to support cancellation of the iteration and invocation of the actions.</param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T, CancellationToken> action, bool throwOnCancel, CancellationToken token)
        {
            foreach (var item in source)
            {
                if (token.IsCancellationRequested)
                {
                    if (throwOnCancel)
                    {
                        token.ThrowIfCancellationRequested();
                    }

                    return;
                }

                action(item, token);
            }
        }
    }
}
