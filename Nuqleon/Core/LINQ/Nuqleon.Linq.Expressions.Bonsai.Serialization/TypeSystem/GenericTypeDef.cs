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
    internal sealed class GenericTypeDef : TypeDef
    {
        #region Fields

        private readonly TypeRef _genericTypeDefinition;
        private readonly TypeRef[] _genericTypeArguments;
        private TypeSlim _type;

        #endregion

        #region Constructors

        public GenericTypeDef(TypeRef genericTypeDefinition, params TypeRef[] genericTypeArguments)
        {
            _genericTypeDefinition = genericTypeDefinition;
            _genericTypeArguments = genericTypeArguments;
        }

        #endregion

        #region Methods

        public override Json.Expression ToJson(SerializationDomain domain)
        {
            var count = _genericTypeArguments.Length;

            var genericTypeArguments = new Json.Expression[count];
            for (var i = 0; i < count; i++)
            {
                genericTypeArguments[i] = _genericTypeArguments[i].ToJson();
            }

            return Json.Expression.Array(
                Discriminators.Type.GenericDiscriminator,
                _genericTypeDefinition.ToJson(),
                Json.Expression.Array(genericTypeArguments)
            );
        }

        public static GenericTypeDef FromJson(Json.ArrayExpression type)
        {
            if (type.ElementCount != 3)
                throw new BonsaiParseException("Expected 3 JSON array elements for a generic type definition.", type);

            var typeDef = TypeRef.FromJson(type.GetElement(1));

            if (type.GetElement(2) is not Json.ArrayExpression args)
                throw new BonsaiParseException("Expected a JSON array in 'node[2]' for the type arguments of a generic type definition.", type);

            var n = args.ElementCount;

            if (n < 1)
                throw new BonsaiParseException("Expected at least 1 JSON array element in 'node[2]' for the type arguments of a generic type definition.", type);

            var typeArgs = new TypeRef[n];

            for (var i = 0; i < n; i++)
            {
                var arg = args.GetElement(i);
                var typeRef = TypeRef.FromJson(arg);
                typeArgs[i] = typeRef;
            }

            return new GenericTypeDef(typeDef, typeArgs);
        }

        public override TypeSlim ToType(DeserializationDomain domain, TypeSlim[] genericArguments)
        {
            if (_type == null)
            {
                var def = domain.GetType(_genericTypeDefinition).ToType(domain, genericArguments);
                GenericDefinitionTypeSlim genDef;

                switch (def.Kind)
                {
                    case TypeSlimKind.Simple:
                        var simple = (SimpleTypeSlim)def;
                        genDef = TypeSlim.GenericDefinition(simple.Assembly, simple.Name);
                        break;
                    default:
                        throw new InvalidOperationException("Expected either simple type slim discriminator for generic definition type.");
                }

                var n = _genericTypeArguments.Length;
                var argsList = new TypeSlim[n];

                for (var i = 0; i < n; i++)
                {
                    var arg = _genericTypeArguments[i];
                    var argType = domain.GetType(arg).ToType(domain, genericArguments);
                    argsList[i] = argType;
                }

                var args = new TrueReadOnlyCollection<TypeSlim>(/* transfer ownership */ argsList);
                _type = TypeSlim.Generic(genDef, args);
            }

            return _type;
        }

        #endregion
    }

}
