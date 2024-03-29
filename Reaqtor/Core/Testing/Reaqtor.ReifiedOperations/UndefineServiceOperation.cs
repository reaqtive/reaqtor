﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
//

using System;

namespace Reaqtor.TestingFramework
{
    [Serializable]
    public class UndefineServiceOperation : ServiceOperation
    {
        public UndefineServiceOperation(ServiceOperationKind kind, Uri targetObjectUri)
            : base(kind, targetObjectUri, null)
        {
        }
    }
}
