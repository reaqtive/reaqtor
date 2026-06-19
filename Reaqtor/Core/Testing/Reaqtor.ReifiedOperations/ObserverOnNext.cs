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
    public class ObserverOnNext : ObserverOperation
    {
        public ObserverOnNext(Uri observerUri, object value)
            : base(ServiceOperationKind.ObserverOnNext, observerUri)
        {
            Value = value;
        }

        public object Value { get; }

        public override string ToString() => base.ToString() + " - " + Value;
    }

    [Serializable]
    public class ObserverOnNext<T> : ObserverOnNext
    {
        public ObserverOnNext(Uri observerUri, T value)
            : base(observerUri, value)
        {
        }

        public new T Value => (T)base.Value;
    }
}
