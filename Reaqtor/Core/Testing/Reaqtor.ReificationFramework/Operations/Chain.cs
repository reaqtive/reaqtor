// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Reaqtor.ReificationFramework
{
    /// <summary>
    /// Operation to combine operations into a sequence.
    /// </summary>
    public sealed class Chain : OperationBase
    {
        internal Chain(ReifiedOperation first, IEnumerable<ReifiedOperation> rest)
            : base(ReifiedOperationKind.Chain, first)
        {
            if (rest == null)
            {
                throw new ArgumentNullException(nameof(rest));
            }
            else if (rest.FirstOrDefault() == null)
            {
                throw new ArgumentException("Expected at least one operation in the chain.", nameof(rest));
            }
            else if (rest.Any(o => o == null))
            {
                throw new ArgumentException("Chained operations must not be null.", nameof(rest));
            }

            Rest = rest;
        }

        /// <summary>
        /// The remaining operations.
        /// </summary>
        public IEnumerable<ReifiedOperation> Rest { get; }
    }
}
