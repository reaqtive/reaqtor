// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.Platform
{
    public interface IReactivePlatformService : IReactiveService
    {
        /// <summary>
        /// Gets the parent platform of the service.
        /// </summary>
        IReactivePlatform Platform { get; }
    }
}
