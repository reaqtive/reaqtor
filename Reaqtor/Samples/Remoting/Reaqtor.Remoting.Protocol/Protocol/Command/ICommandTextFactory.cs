// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;

namespace Reaqtor.Remoting.Protocol
{
    /// <summary>
    /// Interface used to create command text for <see cref="IReactiveServiceProvider{TExpression}"/> operations.
    /// </summary>
    /// <typeparam name="TExpression">Type used for expression tree representation.</typeparam>
    public interface ICommandTextFactory<TExpression>
    {
        /// <summary>
        /// Creates the command text for verb <see cref="CommandVerb.New"/>.
        /// </summary>
        /// <param name="data">The new command data for the observable.</param>
        /// <returns>The command text.</returns>
        string CreateNewText(NewCommandData<TExpression> data);

        /// <summary>
        /// Creates the command text for verb <see cref="CommandVerb.Remove"/>.
        /// </summary>
        /// <param name="uri">The URI of the observable.</param>
        /// <returns>The command text.</returns>
        string CreateRemoveText(Uri uri);

        /// <summary>
        /// Creates the command text for verb <see cref="CommandVerb.Get"/>.
        /// </summary>
        /// <param name="expression">The metadata query expression.</param>
        /// <returns>The command text.</returns>
        string CreateGetText(Expression expression);
    }
}
