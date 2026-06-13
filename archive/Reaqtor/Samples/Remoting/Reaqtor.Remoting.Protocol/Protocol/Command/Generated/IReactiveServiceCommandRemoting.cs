// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   ER - 03/23/2015 - Generated the code in this file.
//

using System;

namespace Reaqtor.Remoting.Protocol
{
    public interface IReactiveServiceCommandRemoting
    {
        IReactiveServiceConnection Connection { get; }
        CommandVerb Verb { get; }
        CommandNoun Noun { get; }
        string CommandText { get; }
        IDisposable Execute(IObserver<string> result);
    }
}
