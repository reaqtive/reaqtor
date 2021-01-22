// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Some sentinel singleton instances.
//
// BD - October 2014
//

using System;

namespace RuntimeLib
{
    internal static class Sentinels
    {
        public static IDisposable HasDisposed = new Disposed();

        private sealed class Disposed : IDisposable
        {
            public void Dispose()
            {
            }
        }
    }
}
