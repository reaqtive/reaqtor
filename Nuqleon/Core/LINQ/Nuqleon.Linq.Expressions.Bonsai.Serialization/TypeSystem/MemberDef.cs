// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
// ER - July 2013 - Small tweaks.
//

using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq.CompilerServices;
using System.Reflection;
using Json = Nuqleon.Json.Expressions;

namespace System.Linq.Expressions.Bonsai.Serialization
{
    internal abstract class MemberDef
    {
        #region Fields

        private const int GenericParameterNameCount = 8;
        private static readonly string[] s_genericParameterNames = new string[] { "T0", "T1", "T2", "T3", "T4", "T5", "T6", "T7" };
        protected static readonly ReadOnlyCollection<TypeSlim> s_emptyTypeList = Array.Empty<TypeSlim>().ToReadOnly();

        #endregion

        #region Methods

        public abstract MemberInfoSlim ToMember(DeserializationDomain domain);

        public abstract Json.Expression ToJson(SerializationDomain domain);

        public static MemberDef FromJson(DeserializationDomain domain, Json.Expression member)
        {
            if (member is not Json.ArrayExpression info)
                throw new BonsaiParseException("Expected a JSON array for a member definition.", member);

            if (info.ElementCount < 1)
                throw new BonsaiParseException("Expected at least 1 JSON array element for a member definition.", member);

            var type = info.GetElement(0);
            if (type.NodeType != Json.ExpressionType.String)
                throw new BonsaiParseException("Expected a JSON string in 'node[0]' for the member type discriminator.", member);

            var kind = (string)((Json.ConstantExpression)type).Value;

            return kind switch
            {
                Discriminators.MemberInfo.Constructor => GetConstructorDef(domain, info),
                Discriminators.MemberInfo.Field => GetFieldDef(domain, info),
                Discriminators.MemberInfo.Property => GetPropertyDef(domain, info),
                Discriminators.MemberInfo.SimpleMethod => GetSimpleMethodDef(domain, info),
                Discriminators.MemberInfo.OpenGenericMethod => GetOpenGenericMethodDef(domain, info),
                Discriminators.MemberInfo.ClosedGenericMethod => GetClosedGenericMethodDef(info),
                _ => throw new BonsaiParseException(string.Format(CultureInfo.InvariantCulture, "Unexpected member type discriminator '{0}'.", kind), member),
            };
        }

        private static MemberDef GetFieldDef(DeserializationDomain domain, Json.ArrayExpression info)
        {
            var n = info.ElementCount;
            if (n is not 3 and not 4)
                throw new BonsaiParseException("Expected 3 or 4 JSON array elements for a field definition.", info);

            var typeRef = TypeRef.FromJson(info.GetElement(1));
            var declaringType = domain.GetType(typeRef).ToType(domain);

            var name = info.GetElement(2);
            if (name.NodeType != Json.ExpressionType.String)
                throw new BonsaiParseException("Expected a JSON string in 'node[2]' for the name of a field definition.", info);

            var fieldType = n == 4 ? domain.GetType(info.GetElement(3)) : null;
            var fieldName = (string)((Json.ConstantExpression)name).Value;
            var field = declaringType.GetField(fieldName, fieldType);
            return new FieldDef(typeRef, field);
        }

        private static MemberDef GetPropertyDef(DeserializationDomain domain, Json.ArrayExpression info)
        {
            var n = info.ElementCount;
            if (n is not 3 and not 4 and not 5)
                throw new BonsaiParseException("Expected 3, 4 or 5 JSON array elements for a property definition.", info);

            var typeRef = TypeRef.FromJson(info.GetElement(1));
            var declaringType = domain.GetType(typeRef).ToType(domain);

            var name = info.GetElement(2);
            if (name.NodeType != Json.ExpressionType.String)
                throw new BonsaiParseException("Expected a JSON string in 'node[2]' for the name of a property definition.", info);

            var propertyName = (string)((Json.ConstantExpression)name).Value;

            var readOnlyIndexParameterTypes = s_emptyTypeList;
            if (n >= 4)
            {
                if (info.GetElement(3) is not Json.ArrayExpression indexParameters)
                    throw new BonsaiParseException("Expected a JSON array in 'node[3]' for the indexer parameters.", info);

                var indexParametersCount = indexParameters.ElementCount;
                var indexParameterTypes = new TypeSlim[indexParametersCount];

                for (var i = 0; i < indexParametersCount; i++)
                {
                    var indexParameter = indexParameters.GetElement(i);
                    var indexParameterType = domain.GetType(indexParameter);
                    indexParameterTypes[i] = indexParameterType;
                }

                readOnlyIndexParameterTypes = new TrueReadOnlyCollection<TypeSlim>(/* transfer ownership */ indexParameterTypes);
            }

            var propertyType = default(TypeSlim);
            if (n >= 5)
            {
                propertyType = domain.GetType(info.GetElement(4));
            }

            //
            // TODO: parse out "can write" field in Bonsai once it's available
            // Note: this method is not used by structural types to generate properties
            // Note: CanWrite on PropertyInfoSlim is not used when translating from slim to fat PropertyInfo
            //
            var field = declaringType.GetProperty(propertyName, propertyType, readOnlyIndexParameterTypes, canWrite: true);
            return new PropertyDef(typeRef, field);
        }

        private static ConstructorDef GetConstructorDef(DeserializationDomain domain, Json.ArrayExpression info)
        {
            var n = info.ElementCount;
            if (n != 3)
                throw new BonsaiParseException("Expected 3 JSON array elements for a constructor definition.", info);

            var typeRef = TypeRef.FromJson(info.GetElement(1));
            var declaringType = domain.GetType(typeRef).ToType(domain);

            if (info.GetElement(2) is not Json.ArrayExpression parameters)
                throw new BonsaiParseException("Expected a JSON array in 'node[2]' for the parameters of a constructor definition.", info);

            var parametersCount = parameters.ElementCount;
            var parameterTypeRefs = new TypeRef[parametersCount];
            var parameterTypes = new TypeSlim[parametersCount];

            for (var i = 0; i < parametersCount; i++)
            {
                var parameter = parameters.GetElement(i);

                var parameterTypeRef = TypeRef.FromJson(parameter);
                parameterTypeRefs[i] = parameterTypeRef;

                var parameterType = domain.GetType(parameter);
                parameterTypes[i] = parameterType;
            }

            var readOnlyParameterTypes = new TrueReadOnlyCollection<TypeSlim>(/* transfer ownership */ parameterTypes);

            var constructor = declaringType.GetConstructor(readOnlyParameterTypes);
            return new ConstructorDef(typeRef, constructor, parameterTypeRefs);
        }

        private static MemberDef GetSimpleMethodDef(DeserializationDomain domain, Json.ArrayExpression info)
        {
            var n = info.ElementCount;
            if (n is not 4 and not 5)
                throw new BonsaiParseException("Expected 4 or 5 JSON array elements for a simple method definition.", info);

            var typeRef = TypeRef.FromJson(info.GetElement(1));
            var declaringType = domain.GetType(typeRef).ToType(domain);

            var name = info.GetElement(2);
            if (name.NodeType != Json.ExpressionType.String)
                throw new BonsaiParseException("Expected a JSON string in 'node[2]' for the name of a simple method definition.", info);

            var methodName = (string)((Json.ConstantExpression)name).Value;

            if (info.GetElement(3) is not Json.ArrayExpression parameters)
                throw new BonsaiParseException("Expected a JSON array in 'node[3]' for the parameters of a simple method definition.", info);

            var returnTypeRef = default(TypeRef);
            var returnType = default(TypeSlim);

            if (n == 5)
            {
                var returnExpr = info.GetElement(4);
                returnTypeRef = TypeRef.FromJson(returnExpr);
                returnType = domain.GetType(returnExpr);
            }

            var parametersCount = parameters.ElementCount;
            var parameterTypeRefs = new TypeRef[parametersCount];
            var parameterTypes = new TypeSlim[parametersCount];

            for (var i = 0; i < parametersCount; i++)
            {
                var parameter = parameters.GetElement(i);

                var parameterTypeRef = TypeRef.FromJson(parameter);
                parameterTypeRefs[i] = parameterTypeRef;

                var parameterType = domain.GetType(parameter);
                parameterTypes[i] = parameterType;
            }

            var readOnlyParameterTypes = new TrueReadOnlyCollection<TypeSlim>(/* transfer ownership */ parameterTypes);

            var method = declaringType.GetSimpleMethod(methodName, readOnlyParameterTypes, returnType);
            return new SimpleMethodDef(typeRef, method, returnTypeRef, parameterTypeRefs);
        }

        private static MemberDef GetOpenGenericMethodDef(DeserializationDomain domain, Json.ArrayExpression info)
        {
            var n = info.ElementCount;
            if (n is not 5 and not 6)
                throw new BonsaiParseException("Expected 5 or 6 JSON array elements for an open generic method definition.", info);

            var first = info.GetElement(1);
            var declaringTypeRef = TypeRef.FromJson(first);
            var declaringType = domain.GetType(first);

            var name = info.GetElement(2);
            if (name.NodeType != Json.ExpressionType.String)
                throw new BonsaiParseException("Expected a JSON string in 'node[2]' for the name of an open generic method definition.", info);

            var methodName = (string)((Json.ConstantExpression)name).Value;

            var arity = info.GetElement(3);
            if (arity.NodeType != Json.ExpressionType.Number)
                throw new BonsaiParseException("Expected a JSON number in 'node[3]' for the generic arity of an open generic method definition.", info);

            var genericArity = Helpers.ParseInt32((string)((Json.ConstantExpression)arity).Value);

            var genericParameters = new TypeSlim[genericArity];
            for (var i = 0; i < genericArity; ++i)
            {
                genericParameters[i] = TypeSlim.GenericParameter(GetGenericParameterName(i));
            }

            var readOnlyGenericParameters = new TrueReadOnlyCollection<TypeSlim>(/* transfer ownership */ genericParameters);

            if (info.GetElement(4) is not Json.ArrayExpression parameters)
                throw new BonsaiParseException("Expected a JSON array in 'node[4]' for the parameters of an open generic method definition.", info);

            var parameterCount = parameters.ElementCount;
            var parameterTypes = new TypeSlim[parameterCount];

            for (var i = 0; i < parameterCount; i++)
            {
                var parameter = parameters.GetElement(i);
                var parameterType = domain.GetType(parameter, genericParameters);
                parameterTypes[i] = parameterType;
            }

            var readOnlyParameterTypes = new TrueReadOnlyCollection<TypeSlim>(/* transfer ownership */ parameterTypes);

            var returnTypeRef = default(TypeRef);
            var returnType = default(TypeSlim);

            if (n == 6)
            {
                var returnExpr = info.GetElement(5);
                returnTypeRef = TypeRef.FromJson(returnExpr);
                returnType = domain.GetType(returnExpr, genericParameters);
            }

            var targetMethodDefinition = declaringType.GetGenericDefinitionMethod(methodName, readOnlyGenericParameters, readOnlyParameterTypes, returnType);

            return new OpenGenericMethodDef(declaringTypeRef, targetMethodDefinition, returnTypeRef, parameters: null); // TODO: need to restore params?
        }

        private static MemberDef GetClosedGenericMethodDef(Json.ArrayExpression info)
        {
            var n = info.ElementCount;
            if (n != 3)
                throw new BonsaiParseException("Expected 3 JSON array elements for a closed generic method definition.", info);

            var genericMethodDefinition = info.GetElement(1);

            if (info.GetElement(2) is not Json.ArrayExpression args)
                throw new BonsaiParseException("Expected a JSON array in 'node[2]' for the type arguments of a closed generic method definition.", info);

            var genericArgumentsCount = args.ElementCount;
            var genericArguments = new TypeRef[genericArgumentsCount];

            for (var i = 0; i < genericArgumentsCount; i++)
            {
                var arg = args.GetElement(i);
                var typeRef = TypeRef.FromJson(arg);
                genericArguments[i] = typeRef;
            }

            return new ClosedGenericMethodDef(genericMethodDefinition, genericArguments);
        }

        private static string GetGenericParameterName(int i)
        {
            Debug.Assert(i >= 0);

            if (i < GenericParameterNameCount)
            {
                return s_genericParameterNames[i];
            }
            else
            {
                return "T" + i;
            }
        }

        #endregion
    }
}
