// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor
{
    /// <summary>
    /// Provides a set of extension methods for IAsyncSubjectFactory&lt;TInput, TOutput&gt; and IAsyncSubjectFactory&lt;TInput, TOutput, TArgs&gt;.
    /// </summary>
    public static partial class AsyncReactiveSubjectFactoryExtensions
    {
        #region CreateAsync

        /// <summary>
        /// Creates a new subject with the specified stream URI.
        /// </summary>
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <param name="subjectFactory">Factory used to create the subject.</param>
        /// <param name="streamUri">URI identifying the stream.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>Task returning a subject object that can be used to send and receive data on the stream, or an exception if the creation request was unsuccessful.</returns>
        public static Task<IAsyncReactiveSubject<TInput, TOutput>> CreateAsync<TInput, TOutput>(this IAsyncReactiveSubjectFactory<TInput, TOutput> subjectFactory, Uri streamUri, object state = null)
        {
            if (subjectFactory == null)
                throw new ArgumentNullException(nameof(subjectFactory));
            if (streamUri == null)
                throw new ArgumentNullException(nameof(streamUri));

            return subjectFactory.CreateAsync(streamUri, state, CancellationToken.None);
        }

        /// <summary>
        /// Creates a new subject with the specified stream UR
        /// <typeparam name="TInput">Type of the data received by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TOutput">Type of the data produced by the subjects created by the stream factory.</typeparam>
        /// <typeparam name="TArgs">Type of the parameter passed to the subject factory.</typeparam>I.
        /// </summary>
        /// <param name="subjectFactory">Factory used to create the subject.</param>
        /// <param name="streamUri">URI identifying the stream.</param>
        /// <param name="argument">Parameter to pass to the subject factory.</param>
        /// <param name="state">Additional metadata to associate with the artifact. Implementations can interpret this value, or ignore it.</param>
        /// <returns>Task returning a subject object that can be used to send and receive data on the stream, or an exception if the creation request was unsuccessful.</returns>
        public static Task<IAsyncReactiveSubject<TInput, TOutput>> CreateAsync<TInput, TOutput, TArgs>(this IAsyncReactiveSubjectFactory<TInput, TOutput, TArgs> subjectFactory, Uri streamUri, TArgs argument, object state = null)
        {
            if (subjectFactory == null)
                throw new ArgumentNullException(nameof(subjectFactory));
            if (streamUri == null)
                throw new ArgumentNullException(nameof(streamUri));

            return subjectFactory.CreateAsync(streamUri, argument, state, CancellationToken.None);
        }

        #endregion
    }
}
