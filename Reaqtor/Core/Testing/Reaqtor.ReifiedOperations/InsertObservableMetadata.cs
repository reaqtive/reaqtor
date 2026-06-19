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
    public class InsertObservableMetadata : InsertMetadataOperation
    {
        public InsertObservableMetadata(Uri observableUri)
            : this(observableUri, null, null)
        {
        }

        public InsertObservableMetadata(Uri observableUri, Expression expression, object state)
            : base(ServiceOperationKind.InsertObservableMetadata, observableUri, expression, state)
        {
        }
    }
}
