// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
// ER - July 2013 - Small tweaks.
//

using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq.CompilerServices;
using System.Reflection;
using Json = Nuqleon.Json.Expressions;

namespace System.Linq.Expressions.Bonsai.Serialization
{
    internal abstract class TypeDef
    {
        #region Fields

        protected static readonly ReadOnlyCollection<TypeSlim> s_emptyTypeList = Array.Empty<TypeSlim>().ToReadOnly();

        #endregion

        #region Methods

        public abstract Json.Expression ToJson(SerializationDomain domain);
        public abstract TypeSlim ToType(DeserializationDomain domain, params TypeSlim[] genericArguments);

        public static TypeDef FromJson(DeserializationDomain domain, Json.Expression expression)
        {
            if (expression is not Json.ArrayExpression type)
                throw new BonsaiParseException("Expected a JSON array containing a type definition.", expression);

            if (type.ElementCount == 0)
                throw new BonsaiParseException("Expected at least one JSON array element containing a type discriminator.", expression);

            var kind = type.GetElement(0);
            if (kind.NodeType != Json.ExpressionType.String)
                throw new BonsaiParseException("Expected a JSON string at 'node[0]' containing a type discriminator.", expression);

            var typeDiscriminator = (string)((Json.ConstantExpression)kind).Value;

            return typeDiscriminator switch
            {
                Discriminators.Type.Simple => SimpleTypeDef.FromJson(type),
                Discriminators.Type.Generic => GenericTypeDef.FromJson(type),
                Discriminators.Type.Array => ArrayTypeDef.FromJson(type),
                Discriminators.Type.Anonymous => AnonymousStructuralTypeDef.FromJson(type),
                Discriminators.Type.Record => RecordStructuralTypeDef.FromJson(domain, type),
                _ => throw new BonsaiParseException(string.Format(CultureInfo.InvariantCulture, "Unexpected type discriminator '{0}' at 'node[0]'.", typeDiscriminator), expression),
            };
        }

        #endregion
    }
}
