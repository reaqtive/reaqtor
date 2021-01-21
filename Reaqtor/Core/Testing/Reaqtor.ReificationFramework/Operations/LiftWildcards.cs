// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.ReificationFramework
{
    /// <summary>
    /// Operation to lift the wildcards in a reified operation.
    /// </summary>
    public class LiftWildcards : OperationBase
    {
        internal LiftWildcards(ReifiedOperation operation, IWildcardGenerator generator)
            : base(ReifiedOperationKind.LiftWildcards, operation)
        {
            Generator = generator ?? throw new ArgumentNullException(nameof(generator));
        }

        /// <summary>
        /// The generator used to produce replacement IDs for wildcard occurrences.
        /// </summary>
        public IWildcardGenerator Generator { get; }
    }
}
