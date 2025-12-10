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

namespace Reaqtor.TestingFramework
{
    public class ObserverOperation : ServiceOperation
    {
        public ObserverOperation(ServiceOperationKind kind, Uri observerUri)
            : base(kind, observerUri, state: null)
        {
        }
    }
}
