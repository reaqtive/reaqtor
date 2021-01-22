// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

using Nuqleon.DataModel;

namespace Reaqtor.Remoting.Reactor.Client
{
    public class ClientFamily
    {
        public ClientFamily()
        {
            Members = new List<ClientPerson>();
        }

        [Mapping("reactor://platform.bing.com/observables/sample/family/members")]
        public List<ClientPerson> Members { get; set; }
    }
}
