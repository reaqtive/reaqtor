// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;

namespace Reaqtor.Remoting.Deployable
{
    /// <summary>
    /// Tracer for Platform Observables.
    /// </summary>
    internal static class Tracer
    {
        /// <summary>
        /// Gets the trace source.
        /// </summary>
        public static TraceSource TraceSource => new("PlatformObservables");
    }
}
