// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using Reaqtor.Remoting.Platform;
using Reaqtor.Remoting.QueryEvaluator;

namespace Reaqtor.Remoting.QueryEvaluatorHost
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var service = new TcpRemoteServiceHost<QueryEvaluatorServiceConnection>(args);
            service.Start();
            service.Instance.Dispose();
        }
    }
}
