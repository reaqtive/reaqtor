// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using System.Collections.Generic;
using System.Linq.Expressions;

#if USE_SLIM
namespace System.Linq.CompilerServices.Bonsai
#else
namespace System.Linq.CompilerServices
#endif
{
#if USE_SLIM
    #region Aliases

    using ParameterExpression = ParameterExpressionSlim;

    #endregion
#endif

    /// <summary>
    /// Expression visitor with scope tracking for ParameterExpression nodes.
    /// </summary>
    /// <typeparam name="TState">Type of the state objects that will be associated with parameter expressions during the expression tree visit.</typeparam>
    /// <typeparam name="TExpression">Target type for expressions.</typeparam>
    /// <typeparam name="TLambdaExpression">Target type for lambda expressions. This type has to derive from TExpression.</typeparam>
    /// <typeparam name="TParameterExpression">Target type for parameter expressions. This type has to derive from TExpression.</typeparam>
    /// <typeparam name="TNewExpression">Target type for new expressions. This type has to derive from TExpression.</typeparam>
    /// <typeparam name="TElementInit">Target type for element initializers.</typeparam>
    /// <typeparam name="TMemberBinding">Target type for member bindings.</typeparam>
    /// <typeparam name="TMemberAssignment">Target type for member assignments. This type has to derive from TMemberBinding.</typeparam>
    /// <typeparam name="TMemberListBinding">Target type for member list bindings. This type has to derive from TMemberBinding.</typeparam>
    /// <typeparam name="TMemberMemberBinding">Target type for member member bindings. This type has to derive from TMemberBinding.</typeparam>
    /// <typeparam name="TCatchBlock">Target type for catch blocks.</typeparam>
    /// <typeparam name="TSwitchCase">Target type for switch cases.</typeparam>
    /// <typeparam name="TLabelTarget">Target type for label targets.</typeparam>
#if USE_SLIM
    public abstract class ScopedExpressionSlimVisitor<TState, TExpression, TLambdaExpression, TParameterExpression, TNewExpression, TElementInit, TMemberBinding, TMemberAssignment, TMemberListBinding, TMemberMemberBinding, TCatchBlock, TSwitchCase, TLabelTarget> : ScopedExpressionSlimVisitorBase<TExpression, TLambdaExpression, TParameterExpression, TNewExpression, TElementInit, TMemberBinding, TMemberAssignment, TMemberListBinding, TMemberMemberBinding, TCatchBlock, TSwitchCase, TLabelTarget>
#else
    public abstract class ScopedExpressionVisitor<TState, TExpression, TLambdaExpression, TParameterExpression, TNewExpression, TElementInit, TMemberBinding, TMemberAssignment, TMemberListBinding, TMemberMemberBinding, TCatchBlock, TSwitchCase, TLabelTarget> : ScopedExpressionVisitorBase<TExpression, TLambdaExpression, TParameterExpression, TNewExpression, TElementInit, TMemberBinding, TMemberAssignment, TMemberListBinding, TMemberMemberBinding, TCatchBlock, TSwitchCase, TLabelTarget>
#endif
        where TLambdaExpression : TExpression
        where TParameterExpression : TExpression
        where TNewExpression : TExpression
        where TMemberAssignment : TMemberBinding
        where TMemberListBinding : TMemberBinding
        where TMemberMemberBinding : TMemberBinding
    {
        private readonly ScopedSymbolTable<ParameterExpression, TState> _symbolTable;

        /// <summary>
        /// Creates a new expression visitor with scope tracking facilities.
        /// </summary>
#if USE_SLIM
        protected ScopedExpressionSlimVisitor()
#else
        protected ScopedExpressionVisitor()
#endif
        {
            _symbolTable = new ScopedSymbolTable<ParameterExpression, TState>();
        }

        /// <summary>
        /// Gets the global scope's symbol table which can be used to track unbound parameters.
        /// </summary>
        public SymbolTable<ParameterExpression, TState> GlobalScope => _symbolTable.GlobalScope;

        /// <summary>
        /// Maps the specified parameter on state tracked in the scoped symbol table. This method gets called for each declaration site of a variable.
        /// </summary>
        /// <param name="parameter">Parameter expression that's being declared.</param>
        /// <returns>State to associate with the given parameter.</returns>
        protected abstract TState GetState(ParameterExpression parameter);

        /// <summary>
        /// Looks up the state associated with the specified parameter expression by scanning the scoped symbol table, starting from the current scope.
        /// </summary>
        /// <param name="parameter">Parameter expression to look up.</param>
        /// <param name="state">State associated with the specified parameter expression.</param>
        /// <returns>true if the symbol was found; otherwise, false.</returns>
        protected bool TryLookup(ParameterExpression parameter, out TState state)
        {
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));

            if (_symbolTable.TryLookup(parameter, out Indexed<Indexed<TState>> value))
            {
                state = value.Value.Value;
                return true;
            }

            state = default;
            return false;
        }

        /// <summary>
        /// Pushes the parameters of a new declaration site into a new scope in the symbol table.
        /// </summary>
        /// <param name="parameters">Parameters of the declaration site.</param>
        protected override void Push(IEnumerable<ParameterExpression> parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            PushCore(parameters.Select(p => new KeyValuePair<ParameterExpression, TState>(p, GetState(p))));
        }

        /// <summary>
        /// Pushes a new scope in the symbol table.
        /// </summary>
        /// <param name="scope">New scope to push in the symbol table.</param>
        protected virtual void Push(IEnumerable<KeyValuePair<ParameterExpression, TState>> scope)
        {
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            PushCore(scope);
        }

        /// <summary>
        /// Pops a scope from the symbol table.
        /// </summary>
        protected override void Pop() => _symbolTable.Pop();

        private void PushCore(IEnumerable<KeyValuePair<ParameterExpression, TState>> scope)
        {
            _symbolTable.Push();

            foreach (var entry in scope)
            {
                _symbolTable.Add(entry.Key, entry.Value);
            }
        }
    }
}
