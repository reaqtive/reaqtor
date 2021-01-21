// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace System.Diagnostics
{
    internal sealed class StopwatchImpl : IStopwatch
    {
        private readonly Stopwatch _stopwatch;

        public StopwatchImpl(Stopwatch stopwatch) => _stopwatch = stopwatch;

        public TimeSpan Elapsed => _stopwatch.Elapsed;

        public void Restart() => _stopwatch.Restart();

        public void Start() => _stopwatch.Start();
    }
}
