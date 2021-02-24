// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.ReificationFramework
{
    /// <summary>
    /// Operation to repeat an operation a given number of times.
    /// </summary>
    public sealed class Repeat : OperationBase
    {
        internal Repeat(ReifiedOperation operation, long count)
            : base(ReifiedOperationKind.Repeat, operation)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), "Count must be at least 0.");
            }

            Count = count;
        }

        /// <summary>
        /// The number of times to repeat the operation.
        /// </summary>
        public long Count { get; }
    }
}
