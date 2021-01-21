// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Helper class for <see cref="ITransaction" />
    /// </summary>
    public static class Transaction
    {
        /// <summary>
        /// Commit the transaction without a cancellation token.
        /// </summary>
        /// <param name="transaction">The transaction to commit.</param>
        /// <returns>A task representing the eventual completion of the commit.</returns>
        public static Task CommitAsync(this ITransaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            return transaction.CommitAsync(CancellationToken.None);
        }
    }
}
