// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - February 2017 - Created this file.
//

using System.Reflection;

namespace System.Linq.Expressions
{
    public partial class ExpressionOptimizer
    {
        /// <summary>
        /// Indicates whether the expression being visited is part of an assignment to a parameter that has
        /// <c>const</c> behavior, allowing for relaxations in the checks for constant folding.
        /// </summary>
        private bool _isConst;

        /// <summary>
        /// Visits the expression <paramref name="argument"/> using the corresponding <paramref name="parameter"/>
        /// to determine whether the argument is assigned to a <c>ref</c> or <c>out</c> parameter. An argument that
        /// is passed by reference gets visited using <see cref="LvalExpressionVisitor.VisitLval"/>.
        /// </summary>
        /// <param name="argument">The argument expression to visit.</param>
        /// <param name="parameter">The parameter corresponding to the argument.</param>
        /// <returns>The result of visiting the expression arguments.</returns>
        protected override Expression VisitArgument(Expression argument, ParameterInfo parameter)
        {
            var isConst = IsConst(parameter);
            var wasConst = default(bool);

            if (isConst)
            {
                wasConst = _isConst;
                _isConst = true;
            }

            var res = base.VisitArgument(argument, parameter);

            if (isConst)
            {
                _isConst = wasConst;
            }

            return res;
        }
    }
}
