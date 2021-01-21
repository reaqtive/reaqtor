// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Omitted null checks similar to expression tree visitors.

namespace System.Reflection
{
    /// <summary>
    /// Visitor for lightweight representations of members.
    /// </summary>
    /// <typeparam name="TMember">Type representing a member.</typeparam>
    /// <typeparam name="TField">Type representing a field.</typeparam>
    /// <typeparam name="TProperty">Type representing a property.</typeparam>
    /// <typeparam name="TMethod">Type representing a method.</typeparam>
    /// <typeparam name="TSimpleMethod">Type representing a simple method.</typeparam>
    /// <typeparam name="TGenericDefinitionMethod">Type representing an open generic method definition.</typeparam>
    /// <typeparam name="TGenericMethod">Type representing a closed generic method.</typeparam>
    /// <typeparam name="TConstructor">Type representing a constructor.</typeparam>
    public abstract class MemberInfoSlimVisitor<TMember, TField, TProperty, TMethod, TSimpleMethod, TGenericDefinitionMethod, TGenericMethod, TConstructor>
        where TField : TMember
        where TProperty : TMember
        where TMethod : TMember
        where TSimpleMethod : TMethod
        where TGenericDefinitionMethod : TMethod
        where TGenericMethod : TMethod
        where TConstructor : TMember
    {
        /// <summary>
        /// Visits the specified member.
        /// </summary>
        /// <param name="member">Member to visit.</param>
        /// <returns>Result of the visit.</returns>
        public virtual TMember Visit(MemberInfoSlim member)
        {
            if (member == null)
                throw new ArgumentNullException(nameof(member));

            return member.MemberType switch
            {
                MemberTypes.Constructor => VisitConstructor((ConstructorInfoSlim)member),
                MemberTypes.Field => VisitField((FieldInfoSlim)member),
                MemberTypes.Method => VisitMethod((MethodInfoSlim)member),
                MemberTypes.Property => VisitProperty((PropertyInfoSlim)member),
                _ => throw new NotSupportedException("Unknown member kind."),
            };
        }

        /// <summary>
        /// Visits a constructor.
        /// </summary>
        /// <param name="constructor">Constructor to visit.</param>
        /// <returns>Result of the visit.</returns>
        protected abstract TConstructor VisitConstructor(ConstructorInfoSlim constructor);

        /// <summary>
        /// Visits a field.
        /// </summary>
        /// <param name="field">Field to visit.</param>
        /// <returns>Result of the visit.</returns>
        protected abstract TField VisitField(FieldInfoSlim field);

        /// <summary>
        /// Visits a method.
        /// </summary>
        /// <param name="method">Method to visit.</param>
        /// <returns>Result of the visit.</returns>
        protected virtual TMethod VisitMethod(MethodInfoSlim method)
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
        protected abstract TSimpleMethod VisitSimpleMethod(SimpleMethodInfoSlim method);

        /// <summary>
        /// Visits a generic method definition.
        /// </summary>
        /// <param name="method">Method to visit.</param>
        /// <returns>Result of the visit.</returns>
        protected abstract TGenericDefinitionMethod VisitGenericDefinitionMethod(GenericDefinitionMethodInfoSlim method);

        /// <summary>
        /// Visits a generic method.
        /// </summary>
        /// <param name="method">Method to visit.</param>
        /// <returns>Result of the visit.</returns>
        protected virtual TGenericMethod VisitGenericMethod(GenericMethodInfoSlim method)
        {
            var definition = VisitAndConvert<TGenericDefinitionMethod>(method.GenericMethodDefinition);
            return MakeGenericMethod(method, definition);
        }

        /// <summary>
        /// Constructs a closed generic method with the specified open generic method definition.
        /// </summary>
        /// <param name="method">Original method.</param>
        /// <param name="methodDefinition">Generic method definition.</param>
        /// <returns>Representation of a closed generic method with the given method definition.</returns>
        protected abstract TGenericMethod MakeGenericMethod(GenericMethodInfoSlim method, TGenericDefinitionMethod methodDefinition);

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1716 // Reserved language keyword 'property'.

        /// <summary>
        /// Visits a property.
        /// </summary>
        /// <param name="property">Property to visit.</param>
        /// <returns>Result of the visit.</returns>
        protected abstract TProperty VisitProperty(PropertyInfoSlim property);

#pragma warning restore CA1716
#pragma warning restore IDE0079

        /// <summary>
        /// Visits and converts a member.
        /// </summary>
        /// <typeparam name="TResult">Type representing the kind of member to convert to.</typeparam>
        /// <param name="member">Member to visit.</param>
        /// <returns>Result of visiting and converting the member.</returns>
        public TResult VisitAndConvert<TResult>(MemberInfoSlim member)
            where TResult : TMember
        {
            var newMember = Visit(member);

            if (newMember is not TResult)
                throw new InvalidOperationException("Member must rewrite to the same kind.");

            return (TResult)newMember;
        }
    }
}
