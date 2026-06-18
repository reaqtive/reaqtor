// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
//

using System;

namespace Reaqtor.TestingFramework
{
    [Serializable]
    public class ObserverOnNext(Uri observerUri, object value) : ObserverOperation(ServiceOperationKind.ObserverOnNext, observerUri)
    {
        public object Value { get; } = value;

        public override string ToString() => base.ToString() + " - " + Value;
    }

    [Serializable]
    public class ObserverOnNext<T>(Uri observerUri, T value) : ObserverOnNext(observerUri, value)
    {
        public new T Value => (T)base.Value;
    }
}
