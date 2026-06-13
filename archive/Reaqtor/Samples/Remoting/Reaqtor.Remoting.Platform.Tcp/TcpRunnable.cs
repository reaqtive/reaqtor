// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;

namespace Reaqtor.Remoting.Platform
{
    internal sealed class TcpRunnable<TInstance> : ProcessRunnable
    {
        private readonly int _port;
        private readonly string _uri;

        public TcpRunnable(string executablePath, int port, string uri)
            : base(executablePath, port.ToString(CultureInfo.InvariantCulture), uri)
        {
            _port = port;
            _uri = uri;
        }

        public override object Instance
        {
            get
            {
                var address = Helpers.GetTcpEndpoint(Helpers.Constants.Host, _port, _uri);
                return Activator.GetObject(typeof(TInstance), address);
            }
        }
    }
}
