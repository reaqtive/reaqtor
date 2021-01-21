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
    internal sealed class GenericParameterTypeDef : TypeDef
    {
        #region Fields

        private readonly int _index;

        #endregion

        #region Constructors

        public GenericParameterTypeDef(int index)
        {
            _index = index;
        }

        #endregion

        #region Methods

        public override Json.Expression ToJson(SerializationDomain domain)
        {
            throw new NotSupportedException("Generic parameter types should not be serialized.");
        }

        public override TypeSlim ToType(DeserializationDomain domain, params TypeSlim[] genericArguments)
        {
            return genericArguments[_index];
        }

        #endregion
    }

}
