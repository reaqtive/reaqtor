// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
// ER - July 2014 - Created this file.
//

using Json = Nuqleon.Json.Expressions;

namespace System.Linq.Expressions.Bonsai.Serialization
{
    internal abstract class TypeRef
    {
        #region Properties

        public abstract int Index { get; }

        #endregion

        #region Methods

        public Json.Expression ToJson() => Index.ToJsonNumber();

        public static TypeRef FromJson(Json.Expression expression)
        {
            if (expression.NodeType != Json.ExpressionType.Number)
                throw new BonsaiParseException("Expected a JSON number for a type table index used in a type reference.", expression);

            var index = Helpers.ParseInt32((string)((Json.ConstantExpression)expression).Value);
            return new SimpleTypeRef(index);
        }

        #endregion
    }

    internal sealed class SimpleTypeRef : TypeRef
    {
        #region Constructors

        public SimpleTypeRef(int index)
        {
            Index = index;
        }

        #endregion

        #region Properties

        public sealed override int Index { get; }

        #endregion
    }
}
