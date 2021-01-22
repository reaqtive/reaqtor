// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System.Linq.Expressions;

namespace Reaqtor.Metadata
{
    /// <summary>
    /// Interface representing a resource available in a reactive processing service.
    /// </summary>
    public interface IReactiveResource : IKnownResource, IExpressible
    {
    }
}
