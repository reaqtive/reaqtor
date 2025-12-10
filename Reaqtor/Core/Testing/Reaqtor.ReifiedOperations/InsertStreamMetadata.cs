// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
// IG - 2025/12      - Remove CLR serialization support.
//

using System;
using System.Linq.Expressions;

namespace Reaqtor.TestingFramework
{
    public class InsertStreamMetadata : InsertMetadataOperation
    {
        public InsertStreamMetadata(Uri streamUri)
            : this(streamUri, null, null)
        {
        }

        public InsertStreamMetadata(Uri streamUri, Expression expression, object state)
            : base(ServiceOperationKind.InsertStreamMetadata, streamUri, expression, state)
        {
        }
    }
}
