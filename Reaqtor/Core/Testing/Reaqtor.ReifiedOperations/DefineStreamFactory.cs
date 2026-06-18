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
    public class DefineStreamFactory(Uri streamFactoryUri, Expression streamFactory, object state) : DefineServiceOperation(ServiceOperationKind.DefineStreamFactory, streamFactoryUri, streamFactory, state)
    {
        public DefineStreamFactory(Uri streamFactoryUri)
            : this(streamFactoryUri, streamFactory: null, state: null)
        {
        }
    }
}
