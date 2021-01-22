// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using System;

namespace Reaqtor.TestingFramework
{
    [Serializable]
    public class DeleteMetadataOperation : MetadataOperation
    {
        public DeleteMetadataOperation(ServiceOperationKind kind, Uri targetObjectUri)
            : base(kind, targetObjectUri, expression: null, state: null)
        {
        }
    }
}
