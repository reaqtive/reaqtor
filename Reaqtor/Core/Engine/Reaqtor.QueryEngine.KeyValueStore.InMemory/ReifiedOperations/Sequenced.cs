// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Threading;

namespace Reaqtor.QueryEngine.KeyValueStore.InMemory
{
    public static class Sequenced
    {
        private static long _current;

        public static long Next => Interlocked.Increment(ref _current);
    }
}
