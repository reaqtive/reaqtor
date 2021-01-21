// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2017 - Initial prototype of JIT.
//

using static System.Linq.Expressions.Expression;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Subtype of <see cref="ExpressionVisitor"/> that addresses a number of problems that exist in the base class.
    /// </summary>
    internal class BetterExpressionVisitor : ExpressionVisitor
    {
        /// <summary>
        /// A Boolean value indicating whether the the base class has the quirk documented
        /// in https://github.com/dotnet/corefx/issues/3223.
        /// </summary>
        private static readonly bool s_hasIfThenQuirk;

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1810 // Initialize reference type static fields inline.

        /// <summary>
        /// Type initializer; detects quirks to work around.
        /// </summary>
        static BetterExpressionVisitor()
        {
            var ifThen = IfThen(Default(typeof(bool)), Empty());

            //
            // See https://github.com/dotnet/corefx/issues/3223 for the ConditionalExpression
            // quirk that leads to inadvertent cloning of expression trees when visited, even
            // if no nodes have changed.
            //
            s_hasIfThenQuirk = ifThen.IfFalse != ifThen.IfFalse;
        }

#pragma warning restore CA1810
#pragma warning restore IDE0079

        /// <summary>
        /// Visits the conditional expression and avoids inadvertening cloning of the node when unchanged due to a bug in the .NET Framework.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        /// <remarks>
        /// Works around a bug in ConditionalExpression.Update that causes inadvertent cloning of `IfThen` nodes.
        /// See https://github.com/dotnet/corefx/issues/3223 for more information.
        /// </remarks>
        protected override Expression VisitConditional(ConditionalExpression node)
        {
            //
            // No need to do some workaround if the quirk has been fixed.
            //
            if (s_hasIfThenQuirk)
            {
                //
                // The quirk exists, so only access IfFalse once to avoid triggering allocations
                // of spurious Default(typeof(void)) nodes.
                //
                var oldIfFalse = node.IfFalse;

                //
                // See ConditionalExpression.Make for the logic that leads to special-casing nodes
                // whose IfFalse child is a DefaultExpression of type void.
                //
                if (node.Type == typeof(void) && IsEmpty(oldIfFalse))
                {
                    var oldTest = node.Test;
                    var oldIfTrue = node.IfTrue;

                    //
                    // Visit in the same order as the base class would do. We still visit IfFalse,
                    // which always returns a new instance of Default(typeof(void)) when the quirk
                    // is present. This is needed because a visitor subtype may want to do override
                    // VisitDefault and change the IfFalse node.
                    //
                    var newTest = Visit(oldTest);
                    var newIfTrue = Visit(oldIfTrue);
                    var newIfFalse = Visit(oldIfFalse);

                    //
                    // Check child reference equality to avoid cloning, as we normally would do,
                    // but check the case where the new IfFalse child node is Default(typeof(void))
                    // which we will classify as no change either.
                    //
                    // NB: In case the user rewrites the expression tree to ensure uniqueness of
                    //     references of nodes (i.e. intentional cloning), it's fine to keep the
                    //     node as-is, because ConditionalExpression's behavior of returning a new
                    //     instance of Default(typeof(void)) for each access to IfFalse ensures
                    //     the same uniqueness property. Stated otherwise, the unique Default node
                    //     handed to us by such a user doesn't get stored in ConditionalExpression
                    //     anyway, in favor of GetFalse() returning new Default(typeof(void)) nodes
                    //     for each access to IfFalse.
                    //
                    if (newTest == oldTest &&
                        newIfTrue == oldIfTrue &&
                        (newIfFalse == oldIfFalse || IsEmpty(newIfFalse)))
                    {
                        return node;
                    }

                    //
                    // If anything changed for real, call Update as usual.
                    //
                    return node.Update(newTest, newIfTrue, newIfFalse);
                }
            }

            return base.VisitConditional(node);
        }

        /// <summary>
        /// Checks whether an expression is a <see cref="DefaultExpression"/> of type void.
        /// </summary>
        /// <param name="node">The expression to check.</param>
        /// <returns>true if the expression is a <see cref="DefaultExpression"/> of type void; otherwise, false.</returns>
        private static bool IsEmpty(Expression node) => node.NodeType == ExpressionType.Default && node.Type == typeof(void);
    }
}
