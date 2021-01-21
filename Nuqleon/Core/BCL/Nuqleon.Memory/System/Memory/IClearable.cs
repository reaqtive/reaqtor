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
    //=====================================================\\
    //   _____ _____ _                      _     _        \\
    //  |_   _/ ____| |                    | |   | |       \\
    //    | || |    | | ___  __ _ _ __ __ _| |__ | | ___   \\
    //    | || |    | |/ _ \/ _` | '__/ _` | '_ \| |/ _ \  \\
    //   _| || |____| |  __/ (_| | | | (_| | |_) | |  __/  \\
    //  |_____\_____|_|\___|\__,_|_|  \__,_|_.__/|_|\___|  \\
    //                                                     \\
    //=====================================================\\

    /// <summary>
    /// Represents an object that can be cleared.
    /// </summary>
    public interface IClearable
    {
        /// <summary>
        /// Clears the object. This operation typically wipes out the object's contents.
        /// </summary>
        void Clear();
    }
}
