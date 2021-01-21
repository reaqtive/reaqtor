// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using System;

namespace Reaqtor.Remoting.Protocol
{
    public static class ServiceInstanceHelpers
    {
        private const string MessagingHandle = "<>__MessagingConnection";

        public static void SetMessagingServiceInstance(AppDomain domain, IReactiveMessagingConnection instance)
        {
            domain.SetData(MessagingHandle, instance);
        }

        public static IReactiveMessagingConnection GetMessagingServiceInstance()
        {
            return (IReactiveMessagingConnection)AppDomain.CurrentDomain.GetData(MessagingHandle);
        }
    }
}
