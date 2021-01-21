// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtor.QueryEngine;
using Reaqtor.Reliable.Expressions;

namespace Reaqtor.Remoting
{
    internal sealed class MockReactiveServiceResolver : IReactiveServiceResolver
    {
        public bool TryResolve(Uri uri, out IReactive service)
        {
            throw new NotImplementedException();
        }

        public bool TryResolve(Uri uri, out IReactiveProxy service)
        {
            throw new NotImplementedException();
        }

        public bool TryResolveReliable(Uri uri, out IReliableReactive service)
        {
            throw new NotImplementedException();
        }
    }
}
