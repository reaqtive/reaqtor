// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - August 2014 - Created this file.
//

using System;
using System.Linq.Expressions;

namespace Reaqtor.TestingFramework
{
    [Serializable]
    public class MetadataQuery : MetadataOperation
    {
        public MetadataQuery()
            : this(expression: null)
        {
        }

        public MetadataQuery(Expression expression)
            : base(ServiceOperationKind.MetadataQuery, targetObjectUri: null, expression, state: null)
        {
        }
    }
}
