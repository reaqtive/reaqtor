﻿// Licensed to the .NET Foundation under one or more agreements.
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
    public class DeleteObservableMetadata : DeleteMetadataOperation
    {
        public DeleteObservableMetadata(Uri targetObjectUri)
            : base(ServiceOperationKind.DeleteObservableMetadata, targetObjectUri)
        {
        }
    }
}
