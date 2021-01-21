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
    public abstract class MetadataOperation : ServiceOperation
    {
        protected MetadataOperation(ServiceOperationKind kind, Uri targetObjectUri, Expression expression, object state)
            : base(kind, targetObjectUri, state)
        {
            Expression = expression;
        }

        [field: NonSerialized]
        public Expression Expression { get; }

        public override string ToString() => base.ToString() + " - " + Expression;
    }
}
