// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Linq.Expressions;

using Nuqleon.DataModel.CompilerServices.Bonsai;

namespace Reaqtor.Hosting.Shared.Serialization
{
    /// <summary>
    /// Helper class to serialize and deserialize expressions and data model-
    /// compliant objects.
    /// </summary>
    /// <remarks>
    /// This serializer also ensures that all structural data model types
    /// are fully anonymized prior to serializing the expression.
    /// </remarks>
    public class ClientSerializationHelpers : SerializationHelpers
    {
        /// <summary>
        /// Creates an expression serializer.
        /// </summary>
        /// <returns>An expression serializer.</returns>
        protected override IExpressionSerializer CreateExpressionSerializer()
        {
            return new AnonymizingExpressionSerializer(this);
        }

        private sealed class AnonymizingExpressionSerializer : SerializationHelpersExpressionSerializer
        {
            public AnonymizingExpressionSerializer(SerializationHelpers parent)
                : base(parent)
            {
            }

            public override ExpressionSlim Lift(Expression expression)
            {
                var slim = base.Lift(expression);
                var recordizer = new ExpressionSlimEntityTypeRecordizer();
                return recordizer.Apply(slim);
            }

            protected override ExpressionToExpressionSlimConverter CreateLifter()
            {
                return new ExpressionToExpressionSlimConverter(new DataModelTypeSpace());
            }
        }
    }
}
