// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - August 2014 - Created this file.
//

namespace System.Runtime.Remoting.Tasks
{
    /// <summary>
    /// A provider interface for cancelling operations identified by a GUID.
    /// </summary>
    public interface ICancellationProvider
    {
        /// <summary>
        /// Cancels an operation with the given GUID.
        /// </summary>
        /// <param name="identifier">The GUID.</param>
        void Cancel(Guid identifier);
    }
}
