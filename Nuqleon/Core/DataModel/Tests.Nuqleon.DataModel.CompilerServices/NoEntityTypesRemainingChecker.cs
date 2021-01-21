// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
//

using System;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

using Nuqleon.DataModel;

namespace Tests.Nuqleon.DataModel.CompilerServices
{
    internal class NoEntityTypesRemainingChecker : ExpressionVisitorWithReflection
    {
        private readonly NoEntityTypesRemainingTypeChecker _typeChecker = new();

        public override Expression Visit(Expression node)
        {
            if (node != null)
            {
                VisitType(node.Type);
            }

            return base.Visit(node);
        }

        protected override MethodInfo VisitMethod(MethodInfo method)
        {
            if (method != null)
            {
                VisitType(method.DeclaringType);

                if (method.IsGenericMethod)
                {
                    foreach (var argType in method.GetGenericArguments())
                    {
                        VisitType(argType);
                    }
                }

                VisitType(method.ReturnType);

                foreach (var parameter in method.GetParameters())
                {
                    VisitType(parameter.ParameterType);
                }
            }

            return base.VisitMethod(method);
        }

        protected override MemberInfo VisitProperty(PropertyInfo property)
        {
            if (property != null)
            {
                VisitType(property.DeclaringType);
                VisitType(property.PropertyType);
            }

            return base.VisitProperty(property);
        }

        protected override ConstructorInfo VisitConstructor(ConstructorInfo constructor)
        {
            if (constructor != null)
            {
                VisitType(constructor.DeclaringType);

                foreach (var parameter in constructor.GetParameters())
                {
                    VisitType(parameter.ParameterType);
                }
            }

            return base.VisitConstructor(constructor);
        }

        protected override Type VisitType(Type type)
        {
            if (type != null)
            {
                _typeChecker.Visit(type);
            }

            return base.VisitType(type);
        }

        private class NoEntityTypesRemainingTypeChecker : TypeVisitor
        {
            public override Type Visit(Type type)
            {
                if (type.GetProperties().Any(p => p.IsDefined(typeof(MappingAttribute), inherit: true)))
                    throw new InvalidOperationException(string.Format("Where did '{0}' come from?", type));

                return base.Visit(type);
            }
        }
    }
}
