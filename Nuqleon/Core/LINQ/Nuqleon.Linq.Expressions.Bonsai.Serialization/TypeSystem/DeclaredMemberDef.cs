// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
// ER - July 2013 - Small tweaks.
//

namespace System.Linq.Expressions.Bonsai.Serialization
{
    internal abstract class DeclaredMemberDef : MemberDef
    {
        #region Constructors

        public DeclaredMemberDef(TypeRef declaringType)
        {
            DeclaringType = declaringType;
        }

        #endregion

        #region Properties

        public TypeRef DeclaringType { get; }

        #endregion
    }
}
