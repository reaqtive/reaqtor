﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

namespace Reaqtor
{
    /// <summary>
    /// Interface for a full-blown reactive processing service.
    /// </summary>
    public interface IReactive : IReactiveClient, IReactiveDefinition, IReactiveMetadata
    {
    }
}
