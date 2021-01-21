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
    internal sealed class SimpleMethodDef : MethodDef
    {
        #region Constructors

        public SimpleMethodDef(TypeRef declaringType, SimpleMethodInfoSlim method, TypeRef returnType, params TypeRef[] parameters)
            : base(declaringType, method, returnType, parameters)
        {
        }

        #endregion

        #region Methods

        public override Json.Expression ToJson(SerializationDomain domain)
        {
            var count = Parameters.Length;

            var parameters = new Json.Expression[count];
            for (var i = 0; i < count; i++)
            {
                parameters[i] = Parameters[i].ToJson();
            }

            return Json.Expression.Array(
                Discriminators.MemberInfo.SimpleMethodDiscriminator,
                DeclaringType.ToJson(),
                Json.Expression.String(((SimpleMethodInfoSlim)Method).Name),
                Json.Expression.Array(parameters),
                ReturnType.ToJson()
            );
        }

        #endregion
    }
}
