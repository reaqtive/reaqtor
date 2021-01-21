// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// PS - February 2015 - Created this file.
//

using System.Collections.ObjectModel;
using System.Globalization;

namespace System.Linq.CompilerServices.Optimizers
{
    /// <summary>
    /// Creates a textual representation of the query expression.
    /// </summary>
    internal sealed class QueryDebugger : QueryVisitor<string, string, string>
    {
        /// <summary>
        /// Creates a textual representation of the FirstOperator with the given children.
        /// </summary>
        /// <param name="node">Original query expression.</param>
        /// <param name="source">Source query expression.</param>
        /// <returns>Representation of the original query expression.</returns>
        protected override string MakeFirst(FirstOperator node, string source)
        {
            return string.Format(CultureInfo.InvariantCulture, "@First({0})", source);
        }

        /// <summary>
        /// Creates a textual representation of the FirstPredicateOperator with the given children.
        /// </summary>
        /// <param name="node">Original query expression.</param>
        /// <param name="source">Source query expression.</param>
        /// <param name="predicate">Predicate query expression.</param>
        /// <returns>Representation of the original query expression.</returns>
        protected override string MakeFirstPredicate(FirstPredicateOperator node, string source, string predicate)
        {
            return string.Format(CultureInfo.InvariantCulture, "@First({0}, {1})", source, predicate);
        }

        /// <summary>
        /// Creates a textual representation of the LambdaAbstraction with the given children.
        /// </summary>
        /// <param name="node">Original query expression.</param>
        /// <param name="arguments">Argument query expressions.</param>
        /// <returns>Representation of the original query expression.</returns>
        protected override string MakeLambdaAbstraction(LambdaAbstraction node, ReadOnlyCollection<string> arguments)
        {
            if (node.Parameters.Count == 0)
                return string.Format(CultureInfo.InvariantCulture, "@LambdaAbstraction({0})", node.Body.ToString());

            return string.Format(CultureInfo.InvariantCulture, "@LambdaAbstraction({0}, {1})", node.Body.ToString(), string.Join(", ", arguments));
        }

        /// <summary>
        /// Creates a textual representation of the MonadAbstraction with the given children.
        /// </summary>
        /// <param name="node">Original query expression.</param>
        /// <param name="inner">Inner query expression.</param>
        /// <returns>Representation of the original query expression.</returns>
        protected override string MakeMonadAbstraction(MonadAbstraction node, string inner)
        {
            return string.Format(CultureInfo.InvariantCulture, "@MonadAbstraction({0})", inner);
        }

        /// <summary>
        /// Creates a textual representation of the SelectOperator with the given children.
        /// </summary>
        /// <param name="node">Original query expression.</param>
        /// <param name="source">Source query expression.</param>
        /// <param name="selector">Selector query expression.</param>
        /// <returns>Representation of the original query expression.</returns>
        protected override string MakeSelect(SelectOperator node, string source, string selector)
        {
            return string.Format(CultureInfo.InvariantCulture, "@Select({0}, {1})", source, selector);
        }

        /// <summary>
        /// Creates a textual representation of the TakeOperator with the given children.
        /// </summary>
        /// <param name="node">Original query expression.</param>
        /// <param name="source">Source query expression.</param>
        /// <param name="count">Count query expression.</param>
        /// <returns>Representation of the original query expression.</returns>
        protected override string MakeTake(TakeOperator node, string source, string count)
        {
            return string.Format(CultureInfo.InvariantCulture, "@Take({0}, {1})", source, count);
        }

        /// <summary>
        /// Creates a textual representation of the WhereOperator with the given children.
        /// </summary>
        /// <param name="node">Original query expression.</param>
        /// <param name="source">Source query expression.</param>
        /// <param name="predicate">Predicate query expression.</param>
        /// <returns>Representation of the original query expression.</returns>
        protected override string MakeWhere(WhereOperator node, string source, string predicate)
        {
            return string.Format(CultureInfo.InvariantCulture, "@Where({0}, {1})", source, predicate);
        }
    }
}
