// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using System;
using System.Linq.Expressions;

namespace Reaqtor.TestingFramework
{
    [Serializable]
    public class InsertObserverMetadata : InsertMetadataOperation
    {
        public InsertObserverMetadata(Uri observerUri)
            : this(observerUri, null, null)
        {
        }

        public InsertObserverMetadata(Uri observerUri, Expression expression, object state)
            : base(ServiceOperationKind.InsertObserverMetadata, observerUri, expression, state)
        {
        }
    }
}
