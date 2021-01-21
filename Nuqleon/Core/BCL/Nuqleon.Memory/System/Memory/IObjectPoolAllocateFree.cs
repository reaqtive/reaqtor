// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/29/2015 - Created this type.
//

namespace System.Memory
{
    //===============================================================================================================\\
    //   _  ____  _     _           _   _____            _          _ _                 _       ______               \\
    //  | |/ __ \| |   (_)         | | |  __ \          | |   /\   | | |               | |     |  ____|              \\
    //  | | |  | | |__  _  ___  ___| |_| |__) |__   ___ | |  /  \  | | | ___   ___ __ _| |_ ___| |__ _ __ ___  ___   \\
    //  | | |  | | '_ \| |/ _ \/ __| __|  ___/ _ \ / _ \| | / /\ \ | | |/ _ \ / __/ _` | __/ _ \  __| '__/ _ \/ _ \  \\
    //  | | |__| | |_) | |  __/ (__| |_| |  | (_) | (_) | |/ ____ \| | | (_) | (_| (_| | ||  __/ |  | | |  __/  __/  \\
    //  |_|\____/|_.__/| |\___|\___|\__|_|   \___/ \___/|_/_/    \_\_|_|\___/ \___\__,_|\__\___|_|  |_|  \___|\___|  \\
    //                _/ |                                                                                           \\
    //               |__/                                                                                            \\
    //===============================================================================================================\\

    /// <summary>
    /// Interface for object pools with "alloc" and "free" functions.
    /// </summary>
    /// <typeparam name="T">Type of the elements stored in the pool.</typeparam>
    /// <remarks>This interface can be used to restrict pool usage to the alloc/free pattern.</remarks>
    public interface IObjectPoolAllocateFree<T>
        where T : class
    {
        /// <summary>
        /// Returns an object instance and checks it out from the pool.
        /// </summary>
        /// <returns>Object instance returned from the pool.</returns>
        T Allocate();

        /// <summary>
        /// Returns an object to the pool.
        /// </summary>
        /// <param name="obj">Object to return to the pool.</param>
        void Free(T obj);
    }
}
