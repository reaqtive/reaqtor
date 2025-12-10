// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
// IG - 2025/12   - Remove CLR serialization support.
//

using System;
using System.Linq.Expressions;

namespace Reaqtor.TestingFramework
{
    public class DefineStreamFactory : DefineServiceOperation
    {
        public DefineStreamFactory(Uri streamFactoryUri)
            : this(streamFactoryUri, streamFactory: null, state: null)
        {
        }

        public DefineStreamFactory(Uri streamFactoryUri, Expression streamFactory, object state)
            : base(ServiceOperationKind.DefineStreamFactory, streamFactoryUri, streamFactory, state)
        {
        }
    }
}
