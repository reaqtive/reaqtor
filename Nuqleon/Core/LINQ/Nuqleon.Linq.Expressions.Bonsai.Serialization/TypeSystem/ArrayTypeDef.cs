// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
// ER - July 2013 - Small tweaks.
//

using System.Reflection;
using Json = Nuqleon.Json.Expressions;

namespace System.Linq.Expressions.Bonsai.Serialization
{
    internal sealed class ArrayTypeDef : TypeDef
    {
        #region Fields

        private readonly TypeRef _elementType;
        private readonly int? _rank;
        private TypeSlim _type;

        #endregion

        #region Constructors

        public ArrayTypeDef(TypeRef elementType, int? rank)
        {
            _elementType = elementType;
            _rank = rank;
        }

        #endregion

        #region Methods

        public override Json.Expression ToJson(SerializationDomain domain)
        {
            if (_rank == null)
            {
                return Json.Expression.Array(
                    Discriminators.Type.ArrayDiscriminator,
                    _elementType.ToJson()
                );
            }
            else
            {
                return Json.Expression.Array(
                    Discriminators.Type.ArrayDiscriminator,
                    _elementType.ToJson(),
                    _rank.Value.ToJsonNumber()
                );
            }
        }

        public static ArrayTypeDef FromJson(Json.ArrayExpression type)
        {
            var count = type.ElementCount;

            if (count is not (2 or 3))
                throw new BonsaiParseException("Expected 2 or 3 JSON array elements for an array type definition.", type);

            var elementType = TypeRef.FromJson(type.GetElement(1));
            var rank = default(int?);

            if (count == 3)
            {
                if (type.GetElement(2) is not Json.ConstantExpression rankJson || !int.TryParse((string)rankJson.Value, out int value))
                {
                    throw new BonsaiParseException("Expected a JSON number in 'node[2]' for the rank of an array type definition.", type);
                }
                rank = value;
            }

            return new ArrayTypeDef(elementType, rank);
        }

        public override TypeSlim ToType(DeserializationDomain domain, TypeSlim[] genericArguments)
        {
            if (_type == null)
            {
                var elementType = domain.GetType(_elementType).ToType(domain, genericArguments);

                _type = _rank == null ? TypeSlim.Array(elementType) : TypeSlim.Array(elementType, _rank.Value);
            }

            return _type;
        }

        #endregion
    }
}
