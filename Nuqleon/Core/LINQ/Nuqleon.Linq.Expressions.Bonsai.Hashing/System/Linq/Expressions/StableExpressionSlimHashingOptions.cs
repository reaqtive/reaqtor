// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using System.Reflection;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Options to influence the behavior of stable hashing of expression trees.
    /// </summary>
    [Flags]
    public enum StableExpressionSlimHashingOptions
    {
        /// <summary>
        /// No special options are specified. Use this mode to include the hash code of constant values
        /// (which is assumed to be stable in this mode) when hashing <see cref="ObjectSlim"/> instances
        /// and to use the full assembly name when hashing <see cref="AssemblySlim"/> instances.
        /// </summary>
        None = 0,

        /// <summary>
        /// Constant values are ignored when hashing <see cref="ObjectSlim"/> instances.
        /// </summary>
        IgnoreConstants = 1,

        /// <summary>
        /// Assemblies are hashed using their simple name, i.e. ignoring version numbers, public key
        /// tokens, and cultures.
        /// </summary>
        UseAssemblySimpleName = 2,

        /// <summary>
        /// All options are enabled. See <see cref="IgnoreConstants"/> and <see cref="UseAssemblySimpleName"/>
        /// for more information about these options.
        /// </summary>
        All = IgnoreConstants | UseAssemblySimpleName,
    }
}
