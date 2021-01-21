// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtor.QueryEngine;
using Reaqtor.Reliable.Expressions;

namespace Reaqtor.Shebang.Service
{
    //
    // The resolver is a mechanism to find artifacts in other query engines either locally or remotely,
    // enabling distributed event processing. Think of this as a DNS lookup mechanism for reactive artifacts
    // to IReactive* services that host these, either located peer-to-peer or through a central catalog.
    //

    public sealed class NopReactiveServiceResolver : IReactiveServiceResolver
    {
        public bool TryResolve(Uri uri, out IReactive service) => throw NotSupported();

        public bool TryResolve(Uri uri, out IReactiveProxy service) => throw NotSupported();

        public bool TryResolveReliable(Uri uri, out IReliableReactive service) => throw NotSupported();

        private static Exception NotSupported() => new NotSupportedException("Peer-to-peer resolution not supported.");
    }
}
