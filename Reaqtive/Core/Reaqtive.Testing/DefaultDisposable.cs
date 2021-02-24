﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

//
// NB: This file contains a port of Rx code that should eventually get removed.
//

using System;

namespace Reaqtive.Disposables
{
    /// <summary>
    /// Represents a disposable that does nothing on disposal.
    /// </summary>
    internal sealed class DefaultDisposable : IDisposable
    {
        /// <summary>
        /// Singleton default disposable.
        /// </summary>
        public static readonly DefaultDisposable Instance = new();

        private DefaultDisposable()
        {
        }

        /// <summary>
        /// Does nothing.
        /// </summary>
        public void Dispose()
        {
            // no op
        }
    }
}
