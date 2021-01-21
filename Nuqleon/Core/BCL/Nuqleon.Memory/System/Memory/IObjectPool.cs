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
    //==========================================================\\
    //   _____ ____  _     _           _   _____            _   \\
    //  |_   _/ __ \| |   (_)         | | |  __ \          | |  \\
    //    | || |  | | |__  _  ___  ___| |_| |__) |__   ___ | |  \\
    //    | || |  | | '_ \| |/ _ \/ __| __|  ___/ _ \ / _ \| |  \\
    //   _| || |__| | |_) | |  __/ (__| |_| |  | (_) | (_) | |  \\
    //  |_____\____/|_.__/| |\___|\___|\__|_|   \___/ \___/|_|  \\
    //                   _/ |                                   \\
    //                  |__/                                    \\
    //==========================================================\\

    /// <summary>
    /// Interface for object pools.
    /// </summary>
    /// <typeparam name="T">Type of the elements stored in the pool.</typeparam>
    public interface IObjectPool<T> : IObjectPoolAllocateFree<T>, IObjectPoolNew<T>
        where T : class
    {
    }
}
