// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1716 // Reserved language keyword 'GetType'. (Mirroring System.Object.GetType() API.)

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Represents an object that has a type.
    /// </summary>
    public interface ITyped
    {
        /// <summary>
        /// Gets the object's type.
        /// </summary>
        /// <returns>The type of the object.</returns>
        IType GetType();
    }
}
