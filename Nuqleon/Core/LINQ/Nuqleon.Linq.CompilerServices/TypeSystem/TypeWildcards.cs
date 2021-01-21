// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

namespace System.Linq.CompilerServices.TypeSystem
{
    /// <summary>
    /// Attribute to annotate types as wildcards, which can be used to represent missing type information.
    /// Unification can be used to match wildcards to types.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public sealed class TypeWildcardAttribute : Attribute
    {
    }

#pragma warning disable IDE0051 // Remove unused private members

    /// <summary>
    /// Unconstrained type wildcard.
    /// </summary>
    [TypeWildcard]
    public sealed class T
    {
        private T(bool _) // Should not satisfy new() constraint
        {
        }
    }

    /// <summary>
    /// Unconstrained type wildcard.
    /// </summary>
    [TypeWildcard]
    public sealed class T1
    {
        private T1(bool _) // Should not satisfy new() constraint
        {
        }
    }

    /// <summary>
    /// Unconstrained type wildcard.
    /// </summary>
    [TypeWildcard]
    public sealed class T2
    {
        private T2(bool _) // Should not satisfy new() constraint
        {
        }
    }

    /// <summary>
    /// Unconstrained type wildcard.
    /// </summary>
    [TypeWildcard]
    public sealed class T3
    {
        private T3(bool _) // Should not satisfy new() constraint
        {
        }
    }

    /// <summary>
    /// Unconstrained type wildcard.
    /// </summary>
    [TypeWildcard]
    public sealed class R
    {
        private R(bool _) // Should not satisfy new() constraint
        {
        }
    }

#pragma warning restore IDE0051 // Remove unused private members
}
