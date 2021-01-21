// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

namespace Tests.ReifiedOperations
{
    internal interface IOperation
    {
        string DebugView { get; }
    }

    internal interface IOperation<in TValue> : IOperation
    {
        void Accept(TValue value);
    }
}
