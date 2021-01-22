// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2012
//

using System;

namespace Nuqleon.Linq.Expressions.Serialization
{
    /// <summary>
    /// Flags for expression tree serialization rule table configuration options.
    /// </summary>
    [Flags]
    public enum RuleOptions
    {
        /// <summary>
        /// No options set. Basic expression trees will be supported.
        /// </summary>
        None = 0,

        /// <summary>
        /// Rule table should be read-only.
        /// </summary>
        ReadOnly = 1,

        /// <summary>
        /// Include support for closure serialization.
        /// </summary>
        CaptureClosures = 2,

#if !BACKPORT35
        /// <summary>
        /// Include support for statement trees.
        /// </summary>
        StatementTrees = 4,

        /// <summary>
        /// Include support for C# dynamic expressions.
        /// </summary>
        CSharpDynamic = 8,
#endif

        /// <summary>
        /// Default configuration options, without read-only flag.
        /// </summary>
        Default = CaptureClosures
#if !BACKPORT35
                | StatementTrees
                | CSharpDynamic
#endif
    }
}
