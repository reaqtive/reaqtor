// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.QueryEvaluator
{
    /// <summary>
    /// Represents a reactive metadata service with a caching policy applied.
    /// </summary>
    public interface IReactiveMetadataCache : IReactiveMetadata, IScoped
    {
    }
}
