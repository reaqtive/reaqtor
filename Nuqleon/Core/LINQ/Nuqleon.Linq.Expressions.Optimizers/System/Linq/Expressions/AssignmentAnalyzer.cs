// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Analyze to check for variables that are never assigned to and hence will always have
    /// the default value.
    /// </summary>
    internal sealed class AssignmentAnalyzer : LvalExpressionVisitor
    {
        // NB: Because we don't track scopes, we can get false positives during assignment
        //     analysis. This is fine; we may just be missing out one some optimization
        //     opportunities, e.g. { int x; { int x; x = 5; } Print(x); } will not determine
        //     the outer variable x as being unassigned, if both variables x are reference
        //     equal. As such, we won't eliminate the outer variable.

        /// <summary>
        /// The set of variables to analyze.
        /// </summary>
        private readonly HashSet<ParameterExpression> _variables;

        /// <summary>
        /// The set of all variables that have been found to be assigned to, or that are
        /// deemed to be assignable (e.g. a variable in a <see cref="RuntimeVariablesExpression"/>
        /// can be assigned to at runtime).
        /// </summary>
        private readonly HashSet<ParameterExpression> _assigned;

        /// <summary>
        /// Indicates whether we're currently visiting within a quote expression.
        /// All variables that occur within a quote will be considered unsafe for
        /// substitution by default values or removal.
        /// </summary>
        private bool _inQuote;

        /// <summary>
        /// Creates a new assignment analyzer for the specified declared variables.
        /// </summary>
        /// <param name="variables">The declared variables to analyze assignment for.</param>
        public AssignmentAnalyzer(ReadOnlyCollection<ParameterExpression> variables)
        {
            _variables = new HashSet<ParameterExpression>(variables);
            _assigned = new HashSet<ParameterExpression>();
        }

        /// <summary>
        /// Gets the set of variables that haven't been assigned to.
        /// </summary>
        /// <remarks>
        /// This property should only be read after visiting the expression(s) to analyze.
        /// </remarks>
        public HashSet<ParameterExpression> Unassigned
        {
            get
            {
                _variables.ExceptWith(_assigned);
                return _variables;
            }
        }

        /// <summary>
        /// Visits a parameter expression with information about it being used as an
        /// assignment site.
        /// </summary>
        /// <param name="node">The parameter expression to visit.</param>
        /// <param name="isLval">Indicates whether the parameter is used as a left-value, i.e. an assignment target.</param>
        /// <returns>The original node.</returns>
        protected override Expression VisitParameter(ParameterExpression node, bool isLval)
        {
            if (isLval || _inQuote)
            {
                _assigned.Add(node);
            }

            return node;
        }

        /// <summary>
        /// Visits the operand of a <see cref="ExpressionType.Quote"/> expression in order
        /// to track whether we're currently visiting within a quote. Variables that occur
        /// within quotes will be considered to be assignable.
        /// </summary>
        /// <param name="node">The quote operand expression to visit.</param>
        /// <returns>The original node.</returns>
        protected override LambdaExpression VisitUnaryQuoteOperand(LambdaExpression node)
        {
            // NB: We'll be conservative about occurrences of variables in quotes. We don't
            //     want to change the shape of the quoted tree, so let's bail out at this
            //     level and assume variables can't be taken away if they occur in a quote.
            //
            //     Also note that users could rely on a quote causing the introduction of
            //     StrongBox<T> wrappers around the quoted value, which show up as Constant
            //     nodes with MemberAccess nodes to access the StrongBox<T>.Value. As such,
            //     users *could* cause a mutation of the StrongBox<T>.Value if they fish it
            //     out of the Constant node, thus causing mutation of a variable that's only
            //     read from if we were to believe static analysis at face value.

            var wasInQuote = _inQuote;
            _inQuote = true;

            var res = base.VisitUnaryQuoteOperand(node);

            _inQuote = wasInQuote;

            return res;
        }
    }
}
