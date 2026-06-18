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

namespace System.Linq.Expressions.Bonsai.Serialization
{
    internal abstract class MethodDef(TypeRef declaringType, MethodInfoSlim method, TypeRef returnType, params TypeRef[] parameters) : DeclaredMemberDef(declaringType)
    {

        #region Constructors

        #endregion

        #region Properties

        protected MethodInfoSlim Method { get; } = method;

        public TypeRef ReturnType { get; } = returnType;

        public TypeRef[] Parameters { get; } = parameters;

        #endregion

        #region Methods

        public override MemberInfoSlim ToMember(DeserializationDomain domain) => Method;

        #endregion
    }
}
