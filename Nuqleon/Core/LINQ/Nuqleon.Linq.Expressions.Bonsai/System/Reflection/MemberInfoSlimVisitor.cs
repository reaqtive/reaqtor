// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
// ER - July 2013 - Small tweaks.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Omitted null checks similar to expression tree visitors.

namespace System.Reflection
{
    /// <summary>
    /// Visitor for lightweight representations of members.
    /// </summary>
    public class MemberInfoSlimVisitor
    {
        /// <summary>
        /// Visits the specified member.
        /// </summary>
        /// <param name="member">Member to visit.</param>
        /// <returns>Result of the visit.</returns>
        public MemberInfoSlim Visit(MemberInfoSlim member)
        {
            if (member == null)
                throw new ArgumentNullException(nameof(member));

            return member.MemberType switch
            {
                MemberTypes.Constructor => VisitConstructor((ConstructorInfoSlim)member),
                MemberTypes.Field => VisitField((FieldInfoSlim)member),
                MemberTypes.Method => VisitMethod((MethodInfoSlim)member),
                MemberTypes.Property => VisitProperty((PropertyInfoSlim)member),
                _ => throw new NotSupportedException("Unknown type kind."),
            };
        }

        /// <summary>
        /// Visits a constructor.
        /// </summary>
        /// <param name="constructor">Constructor to visit.</param>
        /// <returns>Result of the visit.</returns>
        protected virtual MemberInfoSlim VisitConstructor(ConstructorInfoSlim constructor)
        {
            return constructor;
        }

        /// <summary>
        /// Visits a field.
        /// </summary>
        /// <param name="field">Field to visit.</param>
        /// <returns>Result of the visit.</returns>
        protected virtual MemberInfoSlim VisitField(FieldInfoSlim field)
        {
            return field;
        }

        /// <summary>
        /// Visits a method.
        /// </summary>
        /// <param name="method">Method to visit.</param>
        /// <returns>Result of the visit.</returns>
        protected virtual MemberInfoSlim VisitMethod(MethodInfoSlim method)
        {
            return method.Kind switch
            {
                MethodInfoSlimKind.Simple => VisitSimpleMethod((SimpleMethodInfoSlim)method),
                MethodInfoSlimKind.GenericDefinition => VisitGenericDefinitionMethod((GenericDefinitionMethodInfoSlim)method),
                MethodInfoSlimKind.Generic => VisitGenericMethod((GenericMethodInfoSlim)method),
                _ => throw new NotSupportedException("Unknown method kind."),
            };
        }

        /// <summary>
        /// Visits a simple method.
        /// </summary>
        /// <param name="method">Method to visit.</param>
        /// <returns>Result of the visit.</returns>
        protected virtual MemberInfoSlim VisitSimpleMethod(SimpleMethodInfoSlim method)
        {
            return method;
        }

        /// <summary>
        /// Visits a generic method definition.
        /// </summary>
        /// <param name="method">Method to visit.</param>
        /// <returns>Result of the visit.</returns>
        protected virtual MemberInfoSlim VisitGenericDefinitionMethod(GenericDefinitionMethodInfoSlim method)
        {
            return method;
        }

        /// <summary>
        /// Visits a generic method.
        /// </summary>
        /// <param name="method">Method to visit.</param>
        /// <returns>Result of the visit.</returns>
        protected virtual MemberInfoSlim VisitGenericMethod(GenericMethodInfoSlim method)
        {
            var definition = VisitAndConvert<GenericDefinitionMethodInfoSlim>(method.GenericMethodDefinition);
            if (definition != method.GenericMethodDefinition)
            {
                return new GenericMethodInfoSlim(definition.DeclaringType, definition, method.GenericArguments);
            }
            else
            {
                return method;
            }
        }

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1716 // Reserved language keyword 'property'.

        /// <summary>
        /// Visits a property.
        /// </summary>
        /// <param name="property">Property to visit.</param>
        /// <returns>Result of the visit.</returns>
        protected virtual MemberInfoSlim VisitProperty(PropertyInfoSlim property)
        {
            return property;
        }

#pragma warning restore CA1716
#pragma warning restore IDE0079

        /// <summary>
        /// Visits and converts a member.
        /// </summary>
        /// <typeparam name="T">Type representing the kind of member to convert to.</typeparam>
        /// <param name="member">Member to visit.</param>
        /// <returns>Result of visiting and converting the member.</returns>
        public T VisitAndConvert<T>(T member)
            where T : MemberInfoSlim
        {
            if (Visit(member) is T newMember)
            {
                return newMember;
            }

            throw new InvalidOperationException("Member must rewrite to the same kind.");
        }
    }
}
