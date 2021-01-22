// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   ER - 03/23/2015 - Generated the code in this file.
//

using System;
using System.Runtime.Remoting.Tasks;

namespace Reaqtor.Remoting.Protocol
{
    public partial class ReactiveServiceCommandService : RemoteServiceBase, IReactiveServiceCommandRemoting
    {
        private readonly IReactiveServiceCommand _obj;

        public ReactiveServiceCommandService(IReactiveServiceCommand obj)
        {
            _obj = obj;
        }

        public IReactiveServiceConnection Connection => _obj.Connection;

        public CommandVerb Verb => _obj.Verb;

        public CommandNoun Noun => _obj.Noun;

        public string CommandText => _obj.CommandText;

        public IDisposable Execute(IObserver<string> result)
        {
            return Invoke(token => _obj.ExecuteAsync(token), result);
        }
    }
}
