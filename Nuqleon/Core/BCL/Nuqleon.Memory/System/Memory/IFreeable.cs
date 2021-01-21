// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/02/2014 - Created this type.
//

namespace System.Memory
{
    //=================================================\\
    //   _____ ______                   _     _        \\
    //  |_   _|  ____|                 | |   | |       \\
    //    | | | |__ _ __ ___  ___  __ _| |__ | | ___   \\
    //    | | |  __| '__/ _ \/ _ \/ _` | '_ \| |/ _ \  \\
    //   _| |_| |  | | |  __/  __/ (_| | |_) | |  __/  \\
    //  |_____|_|  |_|  \___|\___|\__,_|_.__/|_|\___|  \\
    //                                                 \\
    //=================================================\\

    /// <summary>
    /// Represents an object that can be freed.
    /// </summary>
    public interface IFreeable
    {
        /// <summary>
        /// Frees the object. This operation typically returns the object to its resource container.
        /// </summary>
        void Free();
    }
}
