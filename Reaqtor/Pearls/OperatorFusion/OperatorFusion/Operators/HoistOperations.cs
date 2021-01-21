// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Interface for operators that support fusion. Should be replaced by a proper tree
// representation that captures the whole query expression and has a rigorous way
// of representing inputs and outputs.
//
// BD - October 2014
//

using System;

namespace OperatorFusion
{
    [Flags]
    internal enum HoistOperations
    {
        None = 0,
        OnNext = 1,
        OnError = 2,
        OnCompleted = 4,
    }
}
