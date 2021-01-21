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
    internal sealed class ConstructorDef : DeclaredMemberDef
    {
        #region Fields

        private readonly ConstructorInfoSlim _constructor;
        private readonly TypeRef[] _parameters;

        #endregion

        #region Constructors

        public ConstructorDef(TypeRef declaringType, ConstructorInfoSlim constructor, params TypeRef[] parameters)
            : base(declaringType)
        {
            _constructor = constructor;
            _parameters = parameters;
        }

        #endregion

        #region Methods

        public override MemberInfoSlim ToMember(DeserializationDomain domain) => _constructor;

        public override Json.Expression ToJson(SerializationDomain domain)
        {
            var count = _parameters.Length;

            var parameters = new Json.Expression[count];
            for (var i = 0; i < count; i++)
            {
                parameters[i] = _parameters[i].ToJson();
            }

            return Json.Expression.Array(
                Discriminators.MemberInfo.ConstructorDiscriminator,
                DeclaringType.ToJson(),
                Json.Expression.Array(parameters)
            );
        }

        #endregion
    }
}
