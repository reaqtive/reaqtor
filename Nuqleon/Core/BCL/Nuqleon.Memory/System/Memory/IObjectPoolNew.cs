// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/29/2015 - Created this type.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1711 // Replace New suffix. (By design to reflect the `new` keyword.)
#pragma warning disable CA1716 // Conflict with reserved language keyword New. (See above.)

namespace System.Memory
{
    //===========================================================================\\
    //   _  ____  _     _           _   _____            _ _   _                 \\
    //  | |/ __ \| |   (_)         | | |  __ \          | | \ | |                \\
    //  | | |  | | |__  _  ___  ___| |_| |__) |__   ___ | |  \| | _____      __  \\
    //  | | |  | | '_ \| |/ _ \/ __| __|  ___/ _ \ / _ \| | . ` |/ _ \ \ /\ / /  \\
    //  | | |__| | |_) | |  __/ (__| |_| |  | (_) | (_) | | |\  |  __/\ V  V /   \\
    //  |_|\____/|_.__/| |\___|\___|\__|_|   \___/ \___/|_|_| \_|\___| \_/\_/    \\
    //                _/ |                                                       \\
    //               |__/                                                        \\
    //===========================================================================\\

    /// <summary>
    /// Interface for object pools with a "new" operator.
    /// </summary>
    /// <typeparam name="T">Type of the elements stored in the pool.</typeparam>
    /// <remarks>This interface can be used to restrict pool usage to the RAII pattern.</remarks>
    public interface IObjectPoolNew<T>
        where T : class
    {
        /// <summary>
        /// Gets a pooled object instance with RAII capabilities to return it to the pool.
        /// </summary>
        /// <returns>Pooled object instance.</returns>
        PooledObject<T> New();
    }
}
