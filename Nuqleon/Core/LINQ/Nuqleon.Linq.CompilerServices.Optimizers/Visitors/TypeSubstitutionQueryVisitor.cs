// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// PS - February 2015 - Created this file.
//

using System.Linq.Expressions;

namespace System.Linq.CompilerServices.Optimizers
{
    /// <summary>
    /// Query expression visitor which substitutes types.
    /// </summary>
    public class TypeSubstitutionQueryVisitor : QueryVisitorWithReflection
    {
        private readonly TypeSubstitutor _subst;
        private readonly TypeSubstitutionExpressionVisitor _lambdaBodyConverter;

        /// <summary>
        /// Creates a new type substitution query expression visitor with the specified type substitutor.
        /// </summary>
        /// <param name="typeSubstitutor">Type substitutor to map source types onto target types.</param>
        public TypeSubstitutionQueryVisitor(TypeSubstitutor typeSubstitutor)
        {
            _subst = typeSubstitutor ?? throw new ArgumentNullException(nameof(typeSubstitutor));
            _lambdaBodyConverter = new TypeSubstitutionExpressionVisitor(_subst);
        }

        /// <summary>
        /// Visits and substitutes types of an expression which is the body of a lambda abstraction.
        /// </summary>
        /// <param name="body">The expression to visit.</param>
        /// <returns>Result of visiting the expression.</returns>
        protected override LambdaExpression VisitLambdaAbstractionBody(LambdaExpression body)
        {
            return (LambdaExpression)_lambdaBodyConverter.Visit(body);
        }

        /// <summary>
        /// Visits the type with a substitutor.
        /// </summary>
        /// <param name="type">Type to visit.</param>
        /// <returns>Result of the visit.</returns>
        protected override Type VisitType(Type type) => _subst.Visit(type);
    }
}
