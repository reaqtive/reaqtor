// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.TestingFramework
{
    public class CreateObserver : CreateServiceOperation
    {
        public CreateObserver(Uri streamUri)
            : base(ServiceOperationKind.CreateObserver, streamUri, expression: null, state: null)
        {
        }
    }
}
