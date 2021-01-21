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
    internal sealed class SimpleTypeDef : TypeDef
    {
        #region Fields

        private readonly string _typeName;
        private readonly int _assembly;
        private TypeSlim _type;

        #endregion

        #region Constructors

        public SimpleTypeDef(string typeName, int assembly)
        {
            _typeName = typeName;
            _assembly = assembly;
        }

        #endregion

        #region Methods

        public override Json.Expression ToJson(SerializationDomain domain)
        {
            return Json.Expression.Array(
                Discriminators.Type.SimpleDiscriminator,
                Json.Expression.String(_typeName),
                _assembly.ToJsonNumber()
            );
        }

        public static SimpleTypeDef FromJson(Json.ArrayExpression type)
        {
            if (type.ElementCount != 3)
                throw new BonsaiParseException("Expected 3 JSON array elements for a simple type definition.", type);

            var typeName = type.GetElement(1);
            if (typeName.NodeType != Json.ExpressionType.String)
                throw new BonsaiParseException("Expected a JSON string in 'node[1]' for the type name of a simple type definition.", type);

            var assemblyIndex = type.GetElement(2);
            if (assemblyIndex.NodeType != Json.ExpressionType.Number)
                throw new BonsaiParseException("Expected a JSON number in 'node[2]' for the reference to the assembly of a simple type definition.", type);

            var assemblyIndexValue = Helpers.ParseInt32((string)((Json.ConstantExpression)assemblyIndex).Value);
            return new SimpleTypeDef((string)((Json.ConstantExpression)typeName).Value, assemblyIndexValue);
        }

        public override TypeSlim ToType(DeserializationDomain domain, params TypeSlim[] genericArguments)
        {
            if (_type == null)
            {
                _type = TypeSlim.Simple(domain.GetAssembly(_assembly), _typeName);
            }

            return _type;
        }

        #endregion
    }
}
