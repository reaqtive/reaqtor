// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - February 2018
//

using System;

using Reaqtor;
using Reaqtor.QueryEngine;
using Reaqtor.Reliable.Expressions;

namespace Playground
{
    internal static partial class EngineIntegrationTests
    {
        /// <summary>
        /// Trivial implementation of <see cref="IReactiveServiceResolver"/> which throws <see cref="NotImplementedException"/> for every method.
        /// </summary>
        private sealed class Resolver : IReactiveServiceResolver
        {
            public bool TryResolve(Uri uri, out IReactive service) => throw new NotImplementedException();

            public bool TryResolve(Uri uri, out IReactiveProxy service) => throw new NotImplementedException();

            public bool TryResolveReliable(Uri uri, out IReliableReactive service) => throw new NotImplementedException();
        }
    }
}
