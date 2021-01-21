// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Reaqtor.Remoting.Messaging;
using Reaqtor.Remoting.Platform;

namespace Reaqtor.Remoting.MessagingHost
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var service = new TcpRemoteServiceHost<MessagingConnection>(args);
            service.Start();
        }
    }
}
