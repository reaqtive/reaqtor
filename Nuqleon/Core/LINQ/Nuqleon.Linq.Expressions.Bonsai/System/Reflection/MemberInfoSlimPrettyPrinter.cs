// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace System.Reflection
{
    internal sealed class MemberInfoSlimPrettyPrinter : MemberInfoSlimVisitor<string, string, string, string, string, string, string, string>
    {
        private readonly TypeSlimPrettyPrinter _typePrinter;

        public MemberInfoSlimPrettyPrinter()
        {
            _typePrinter = new TypeSlimPrettyPrinter();
        }

        protected override string VisitConstructor(ConstructorInfoSlim constructor)
        {
            var declaringType = _typePrinter.Visit(constructor.DeclaringType);
            var parameterTypes = string.Join(", ", _typePrinter.Visit(constructor.ParameterTypes));
            return string.Format(CultureInfo.InvariantCulture, "{0}..ctor({1})", declaringType, parameterTypes);
        }

        protected override string VisitField(FieldInfoSlim field)
        {
            var declaringType = _typePrinter.Visit(field.DeclaringType);
            return string.Format(CultureInfo.InvariantCulture, "{0}.{1}", declaringType, field.Name);
        }

        protected override string VisitSimpleMethod(SimpleMethodInfoSlim method)
        {
            var declaringType = _typePrinter.Visit(method.DeclaringType);
            var parameterTypes = string.Join(", ", _typePrinter.Visit(method.ParameterTypes));
            var returnType = _typePrinter.Visit(method.ReturnType);
            return string.Format(CultureInfo.InvariantCulture, "{3} {0}.{1}({2})", declaringType, method.Name, parameterTypes, returnType);
        }

        protected override string VisitGenericDefinitionMethod(GenericDefinitionMethodInfoSlim method)
        {
            var declaringType = _typePrinter.Visit(method.DeclaringType);
            var parameterTypes = string.Join(", ", _typePrinter.Visit(method.ParameterTypes));
            var returnType = _typePrinter.Visit(method.ReturnType);
            var genericParameterTypes = string.Join(", ", _typePrinter.Visit(method.GenericParameterTypes));
            return string.Format(CultureInfo.InvariantCulture, "{3} {0}.{1}<{4}>({2})", declaringType, method.Name, parameterTypes, returnType, genericParameterTypes);
        }

        protected override string VisitGenericMethod(GenericMethodInfoSlim method)
        {
            var map = method.GenericMethodDefinition.GenericParameterTypes.Zip(method.GenericArguments, (p, a) => (p, a)).ToDictionary(pa => pa.p, pa => pa.a);
            var subst = new GenericParameterTypeSubstPrinter(map);

            var declaringType = _typePrinter.Visit(method.DeclaringType);
            var parameterTypes = string.Join(", ", subst.Visit(method.GenericMethodDefinition.ParameterTypes));
            var returnType = subst.Visit(method.GenericMethodDefinition.ReturnType);
            var genericArgumentTypes = string.Join(", ", _typePrinter.Visit(method.GenericArguments));
            return string.Format(CultureInfo.InvariantCulture, "{3} {0}.{1}<{4}>({2})", declaringType, method.GenericMethodDefinition.Name, parameterTypes, returnType, genericArgumentTypes);
        }

        private sealed class GenericParameterTypeSubstPrinter : TypeSlimPrettyPrinter
        {
            private readonly Dictionary<TypeSlim, TypeSlim> _map;

            public GenericParameterTypeSubstPrinter(Dictionary<TypeSlim, TypeSlim> map)
            {
                _map = map;
            }

            protected override string VisitGenericParameter(GenericParameterTypeSlim type)
            {
                return Visit(_map[type]);
            }
        }

        protected override string MakeGenericMethod(GenericMethodInfoSlim method, string methodDefinition)
        {
            throw new NotImplementedException();
        }

        protected override string VisitProperty(PropertyInfoSlim property)
        {
            var declaringType = _typePrinter.Visit(property.DeclaringType);
            var indexParameters = property.IndexParameterTypes;
            if (indexParameters != null && indexParameters.Count > 0)
            {
                var indexParameterTypes = string.Join(", ", _typePrinter.Visit(property.IndexParameterTypes));
                return string.Format(CultureInfo.InvariantCulture, "{0}.{1}[{2}]", declaringType, property.Name, indexParameterTypes);
            }
            else
            {
                return string.Format(CultureInfo.InvariantCulture, "{0}.{1}", declaringType, property.Name);
            }
        }
    }
}
