// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Reaqtive
{
    // TODO: does this belong in Reaqtive which has very little notion of URI-based artifacts (except for persistable subscriptions)?

    /// <summary>
    /// Represents an operator that's part of a dependency graph between related artifacts.
    /// </summary>
    public interface IDependencyOperator : IOperator
    {
        /// <summary>
        /// Gets the list of known artifacts the operator depends on.
        /// </summary>
        IEnumerable<Uri> Dependencies { get; }
    }
}
