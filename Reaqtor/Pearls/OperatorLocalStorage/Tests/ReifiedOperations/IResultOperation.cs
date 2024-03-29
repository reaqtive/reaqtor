﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

namespace Tests.ReifiedOperations
{
    internal interface IResultOperation<in TValue, out TResult> : IOperation<TValue>
    {
        TResult GetResult(TValue value);
    }
}
