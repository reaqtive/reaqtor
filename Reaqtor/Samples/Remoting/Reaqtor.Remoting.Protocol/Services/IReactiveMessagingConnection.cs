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
    public interface IReactiveMessagingConnection : IReactiveConnection
    {
        void Publish(string topic, INotification<byte[]> data);
        IDisposable Subscribe(string topic, Action<INotification<byte[]>> receive);
    }
}
