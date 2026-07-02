// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using System;

namespace Tests.ReifiedOperations
{
    internal abstract class OperationBase : IOperation
    {
        private readonly Lazy<string> _debugView;

        public OperationBase()
        {
            _debugView = new Lazy<string>(() => DebugViewCore);
        }

        public string DebugView => _debugView.Value;

        protected abstract string DebugViewCore { get; }

        public override string ToString() => DebugView;
    }
}
