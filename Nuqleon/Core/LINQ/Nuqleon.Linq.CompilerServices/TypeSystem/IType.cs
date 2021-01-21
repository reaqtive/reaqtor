// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Represents a type.
    /// </summary>
    public interface IType : IEquatable<IType>
    {
        /// <summary>
        /// Checks whether objects of the current type are assignable to variables of the given type.
        /// </summary>
        /// <param name="type">Type to check assignment compatibility for.</param>
        /// <returns>true if objects of the current type are assignable to variables of the given type; otherwise, false.</returns>
        bool IsAssignableTo(IType type);
    }
}
