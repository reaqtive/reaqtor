// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;

namespace Reaqtor.Remoting.Protocol
{
    /// <summary>
    /// An interface used for parsing command text for <see cref="IReactiveServiceProvider{TExpression}" /> operations.
    /// </summary>
    /// <typeparam name="TExpression">Type used for expression tree representation.</typeparam>
    public interface ICommandTextParser<TExpression>
    {
        /// <summary>
        /// Parses the command text for verb <see cref="CommandVerb.New"/>.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <returns>The new command data for the observable.</returns>
        NewCommandData<TExpression> ParseNewText(string commandText);

        /// <summary>
        /// Parses the command text for verb <see cref="CommandVerb.Remove"/>.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <returns>The URI of the observable.</returns>
        Uri ParseRemoveText(string commandText);

        /// <summary>
        /// Parses the command text for verb <see cref="CommandVerb.Get"/>.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <returns>The metadata query expression.</returns>
        Expression ParseGetText(string commandText);
    }
}
