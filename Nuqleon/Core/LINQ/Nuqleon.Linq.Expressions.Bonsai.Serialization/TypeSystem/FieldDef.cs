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
    internal sealed class FieldDef : DeclaredMemberDef
    {
        #region Fields

        private readonly FieldInfoSlim _field;

        #endregion

        #region Constructors

        public FieldDef(TypeRef declaringType, FieldInfoSlim field)
            : base(declaringType)
        {
            _field = field;
        }

        #endregion

        #region Methods

        public override MemberInfoSlim ToMember(DeserializationDomain domain) => _field;

        public override Json.Expression ToJson(SerializationDomain domain)
        {
            if (!domain.IsV08 && _field.FieldType != null)
            {
                return Json.Expression.Array(
                    Discriminators.MemberInfo.FieldDiscriminator,
                    DeclaringType.ToJson(),
                    Json.Expression.String(_field.Name),
                    domain.AddType(_field.FieldType).ToJson()
                );
            }
            else
            {
                return Json.Expression.Array(
                    Discriminators.MemberInfo.FieldDiscriminator,
                    DeclaringType.ToJson(),
                    Json.Expression.String(_field.Name)
                );
            }
        }

        #endregion
    }
}
