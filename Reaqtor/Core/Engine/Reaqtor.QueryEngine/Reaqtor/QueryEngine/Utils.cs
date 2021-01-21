// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Threading;

namespace Reaqtor
{
    /// <summary>
    /// Provides miscellaneous utilities that don't belong elsewhere.
    /// </summary>
    internal static class Utils
    {
        /// <summary>
        /// Converts the contents of the given <paramref name="stream"/> to a base64 string.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <returns>A base64 encoded string.</returns>
        /// <remarks>
        /// The function reads from the current <see cref="Stream.Position"/> in the <paramref name="stream"/> and restores back to the
        /// original position after it has read and convert the contents to the end of the stream. This requires the steam to support
        /// setting the position (seek).
        /// </remarks>
        public static string GetBase64Blob(this Stream stream)
        {
            if (stream == null)
            {
                return "(empty)";
            }

            var oldPosition = stream.Position;

            try
            {
                stream.Position = 0;

                var bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);

                return Convert.ToBase64String(bytes);
            }
            finally
            {
                stream.Position = oldPosition;
            }
        }

        /// <summary>
        /// Completes the <paramref name="progress"/> with a value of 100 (to indicate a percentage), if it's not null.
        /// </summary>
        /// <param name="progress">The progress to complete.</param>
        public static void CompleteIfNotNull(this IProgress<int> progress)
        {
            CompleteIfNotNull(progress, CancellationToken.None);
        }

        /// <summary>
        /// Completes the <paramref name="progress"/> with a value of 100 (to indicate a percentage), if it's not null and
        /// if no cancellation is requested.
        /// </summary>
        /// <param name="progress">The progress to complete.</param>
        /// <param name="token">Cancellation token to check; if set, the progress is not completed.</param>
        public static void CompleteIfNotNull(this IProgress<int> progress, CancellationToken token)
        {
            if (progress != null && !token.IsCancellationRequested)
            {
                progress.Report(100);
            }
        }
    }
}
