// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER, BD - July 2013 - Created this file.
//

using System;
using System.Linq.Expressions;
using System.Linq.Expressions.Bonsai.Serialization;

using Nuqleon.DataModel.CompilerServices.Bonsai;

using Json = Nuqleon.Json.Expressions;

namespace Tests.Nuqleon.DataModel.CompilerServices
{
    internal class RecordizingBonsaiSerializer : BonsaiExpressionSerializer
    {
        public RecordizingBonsaiSerializer(Func<Type, Func<object, Json.Expression>> liftFactory, Func<Type, Func<Json.Expression, object>> reduceFactory)
            : base(liftFactory, reduceFactory)
        {
        }

        public override ExpressionSlim Lift(Expression expression)
        {
            return new ExpressionToExpressionSlimConverter(new DataModelTypeSpace()).Visit(expression);
        }

        public override string Serialize(ExpressionSlim expression)
        {
            var substituted = new ExpressionSlimEntityTypeRecordizer().Apply(expression);
            return base.Serialize(substituted);
        }
    }
}
