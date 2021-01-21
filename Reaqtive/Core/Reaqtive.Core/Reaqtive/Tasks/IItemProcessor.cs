// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtive.Tasks
{
    /// <summary>
    /// Interface for item processors.
    /// </summary>
    public interface IItemProcessor
    {
        /// <summary>
        /// Gets the item count.
        /// </summary>
        int ItemCount { get; }

        /// <summary>
        /// Processes the specified batch of items.
        /// </summary>
        /// <param name="batchSize">Size of the batch.</param>
        void Process(int batchSize);
    }
}
