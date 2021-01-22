// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Reaqtive.Scheduler;

namespace Reaqtive.Tasks
{
    /// <summary>
    /// Interface for item processor with yielding support.
    /// </summary>
    public interface IYieldableItemProcessor : IItemProcessor
    {
        /// <summary>
        /// Processes the specified batch of items.
        /// </summary>
        /// <param name="batchSize">Size of the batch.</param>
        /// <param name="yieldToken">Token to observe yield requests.</param>
        void Process(int batchSize, YieldToken yieldToken);
    }
}
