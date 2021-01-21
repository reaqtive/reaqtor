// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Running;

namespace Perf.Nuqleon.Json.Serialization
{
    public static class Program
    {
        public static void Main()
        {
            _ = BenchmarkRunner.Run<Serialize>();
            _ = BenchmarkRunner.Run<Deserialize>();
        }
    }
}
