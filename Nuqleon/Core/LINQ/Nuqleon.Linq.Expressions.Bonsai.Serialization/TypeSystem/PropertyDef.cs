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
    internal sealed class PropertyDef : DeclaredMemberDef
    {
        #region Fields

        private readonly PropertyInfoSlim _property;

        #endregion

        #region Constructors

        public PropertyDef(TypeRef declaringType, PropertyInfoSlim property)
            : base(declaringType)
        {
            _property = property;
        }

        #endregion

        #region Methods

        public override MemberInfoSlim ToMember(DeserializationDomain domain) => _property;

        public override Json.Expression ToJson(SerializationDomain domain)
        {
            var declType = DeclaringType.ToJson();

            if (!domain.IsV08)
            {
                var indexParameterTypes = _property.IndexParameterTypes;
                var indexParameterCount = indexParameterTypes.Count;

                var indexParameterList = new Json.Expression[indexParameterCount];

                for (var i = 0; i < indexParameterCount; i++)
                {
                    indexParameterList[i] = domain.AddType(indexParameterTypes[i]).ToJson();
                }

                var indexParameters = Json.Expression.Array(indexParameterList);

                if (_property.PropertyType != null)
                {
                    var propType = domain.AddType(_property.PropertyType).ToJson();

                    return Json.Expression.Array(
                        Discriminators.MemberInfo.PropertyDiscriminator,
                        declType,
                        Json.Expression.String(_property.Name),
                        indexParameters,
                        propType
                    );
                }
                else
                {
                    return Json.Expression.Array(
                        Discriminators.MemberInfo.PropertyDiscriminator,
                        declType,
                        Json.Expression.String(_property.Name),
                        indexParameters
                    );
                }
            }
            else
            {
                return Json.Expression.Array(
                    Discriminators.MemberInfo.PropertyDiscriminator,
                    declType,
                    Json.Expression.String(_property.Name)
                );
            }
        }

        #endregion
    }
}
