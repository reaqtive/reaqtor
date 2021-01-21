// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
// ER - July 2013 - Small tweaks.
//

using System.Linq.CompilerServices;
using System.Reflection;
using Json = Nuqleon.Json.Expressions;

namespace System.Linq.Expressions.Bonsai.Serialization
{
    internal sealed class ClosedGenericMethodDef : MemberDef
    {
        #region Fields

        private readonly Json.Expression _genericMethodDefinition;
        private readonly TypeRef[] _genericArguments;

        #endregion

        #region Constructors

        public ClosedGenericMethodDef(Json.Expression genericMethodDefinition, TypeRef[] genericArguments)
        {
            _genericMethodDefinition = genericMethodDefinition;
            _genericArguments = genericArguments;
        }

        #endregion

        #region Properties

        public override MemberInfoSlim ToMember(DeserializationDomain domain)
        {
            // PERF: This is a heavy allocator; should we support caching the result?

            var def = ((GenericDefinitionMethodInfoSlim)domain.GetMember(_genericMethodDefinition));

            var count = _genericArguments.Length;

            var argsList = new TypeSlim[count];
            for (var i = 0; i < count; i++)
            {
                argsList[i] = domain.GetType(_genericArguments[i]).ToType(domain);
            }

            var args = new TrueReadOnlyCollection<TypeSlim>(/* transfer ownership */ argsList);
            return def.DeclaringType.GetGenericMethod(def, args);
        }

        #endregion

        #region Methods

        public override Json.Expression ToJson(SerializationDomain domain)
        {
            var count = _genericArguments.Length;

            var arguments = new Json.Expression[count];
            for (var i = 0; i < count; i++)
            {
                arguments[i] = _genericArguments[i].ToJson();
            }

            return Json.Expression.Array(
                Discriminators.MemberInfo.ClosedGenericMethodDiscriminator,
                _genericMethodDefinition,
                Json.Expression.Array(arguments)
            );
        }

        #endregion
    }
}