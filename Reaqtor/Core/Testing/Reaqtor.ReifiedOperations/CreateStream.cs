// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1711 // Rename type so that it does not end in 'Stream'. (By design for reified operations.)

using System;
using System.Linq.Expressions;

namespace Reaqtor.TestingFramework
{
    [Serializable]
    public class CreateStream(Uri streamUri, Expression stream, object state) : CreateServiceOperation(ServiceOperationKind.CreateStream, streamUri, stream, state)
    {
        public CreateStream(Uri streamUri)
            : this(streamUri, stream: null, state: null)
        {
        }
    }
}
