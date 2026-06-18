// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
//

using System;
using System.Linq.Expressions;

namespace Reaqtor.TestingFramework
{
    [Serializable]
    public class DefineObserver(Uri observerUri, Expression observer, object state) : DefineServiceOperation(ServiceOperationKind.DefineObserver, observerUri, observer, state)
    {
        public DefineObserver(Uri observerUri)
            : this(observerUri, observer: null, state: null)
        {
        }
    }
}
