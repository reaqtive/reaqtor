// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Represents a series of operations that are all applied or all not applied (in the case of errors).
    /// </summary>
    public interface ITransaction : IDisposable
    {
        /// <summary>
        /// Commits the transaction.
        /// </summary>
        /// <param name="token">The token to commit operation.</param>
        /// <returns>A task representing the eventual completion of the commit.</returns>
        Task CommitAsync(CancellationToken token);

        /// <summary>
        /// Cleans up intermediate state and partially committed data.
        /// </summary>
        void Rollback();
    }
}
