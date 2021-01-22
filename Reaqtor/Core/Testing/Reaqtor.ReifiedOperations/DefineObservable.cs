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
    public class DefineObservable : DefineServiceOperation
    {
        public DefineObservable(Uri observableUri)
            : this(observableUri, observable: null, state: null)
        {
        }

        public DefineObservable(Uri observableUri, Expression observable, object state)
            : base(ServiceOperationKind.DefineObservable, observableUri, observable, state)
        {
        }
    }
}
