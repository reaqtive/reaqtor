// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   ER - 03/23/2015 - Generated the code in this file.
//

using System.Runtime.Remoting.Tasks;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor.Remoting.Protocol
{
    public partial class ReactiveServiceCommandProxy : RemoteProxyBase, IReactiveServiceCommand
    {
        private readonly IReactiveServiceCommandRemoting _service;

        public ReactiveServiceCommandProxy(IReactiveServiceCommandRemoting service)
        {
            _service = service;
        }

        public IReactiveServiceConnection Connection => _service.Connection;

        public CommandVerb Verb => _service.Verb;

        public CommandNoun Noun => _service.Noun;

        public string CommandText => _service.CommandText;

        public Task<string> ExecuteAsync(CancellationToken token)
        {
            return Invoke<string>(reply => _service.Execute(reply), token);
        }
    }
}
