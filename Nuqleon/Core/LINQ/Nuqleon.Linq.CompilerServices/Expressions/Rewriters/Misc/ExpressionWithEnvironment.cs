// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
//

using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Text;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Represents an expression bound by an environment.
    /// </summary>
    public sealed class ExpressionWithEnvironment : IExpressionWithEnvironment
    {
        private readonly IReadOnlyList<Binding> _bindings;
        private IReadOnlyDictionary<ParameterExpression, object> _environment;

        /// <summary>
        /// Creates a new expression with an environment.
        /// </summary>
        /// <param name="expression">Expression bound by the specified environment.</param>
        /// <param name="bindings">Environment binding the specified expression.</param>
        internal ExpressionWithEnvironment(Expression expression, IReadOnlyList<Binding> bindings)
        {
            Expression = expression;
            _bindings = bindings;
        }

        /// <summary>
        /// Gets the expression bound to the environment.
        /// </summary>
        public Expression Expression { get; }

        /// <summary>
        /// Gets the environment containing bindings of parameters to values in the expression.
        /// </summary>
        IReadOnlyList<Binding> IExpressionWithEnvironment.Bindings => _bindings;

        /// <summary>
        /// Gets the environment of the expression.
        /// </summary>
        public IReadOnlyDictionary<ParameterExpression, object> Environment
        {
            get
            {
                if (_environment == null)
                {
                    var res = new Dictionary<ParameterExpression, object>(_bindings.Count);

                    foreach (var kv in _bindings)
                    {
                        res[kv.Parameter] = kv.Value.Evaluate();
                    }

                    _environment = res;
                }

                return _environment;
            }
        }

        /// <summary>
        /// Returns an invocation expression that will evaluate the expression in its bound environment.
        /// </summary>
        /// <returns>Invocation expression to evaluate the bound expression.</returns>
        public InvocationExpression ToInvocation()
        {
            var n = _bindings.Count;

            var parameters = new ParameterExpression[n];
            var values = new Expression[n];

            var i = 0;
            foreach (var kv in _bindings)
            {
                parameters[i] = kv.Parameter;
                values[i] = kv.Value;
                i++;
            }

            var res =
                Expression.Invoke(
                    Expression.Lambda(Expression, parameters),
                    values
                );

            return res;
        }

        /// <summary>
        /// Returns a friendly string representation of the expression and its environment.
        /// </summary>
        /// <returns>Friendly string representation of the expression and its environment.</returns>
        public override string ToString()
        {
            var env = string.Join(", ", _bindings.Select(kv => string.Format(CultureInfo.InvariantCulture, "{0} : {1} = {2}", kv.Parameter.Name, kv.Parameter.Type, kv.Value.ToString())));
            return string.Format(CultureInfo.InvariantCulture, "({0})[|{1}|]", Expression, env);
        }

        /// <summary>
        /// Gets a string representation of the environment-bound expression tree using C# syntax.
        /// The resulting string is not guaranteed to be semantically equivalent and should be used for diagnostic purposes only.
        /// </summary>
        /// <returns>String representation of the environment-bound expression tree using C# syntax.</returns>
        public string ToCSharpString()
        {
            var sb = new StringBuilder();

            foreach (var kv in _bindings)
            {
                sb.AppendFormat(CultureInfo.InvariantCulture, "{0} {1} = {2};\r\n", kv.Parameter.Type.ToCSharpString(useNamespaceQualifiedNames: false, useCSharpTypeAliases: true, disallowCompilerGeneratedTypes: false), kv.Parameter.Name, kv.Value.ToCSharpString());
            }

            sb.AppendFormat(CultureInfo.InvariantCulture, "return {0};\r\n", Expression.ToCSharpString());

            return sb.ToString();
        }
    }
}
