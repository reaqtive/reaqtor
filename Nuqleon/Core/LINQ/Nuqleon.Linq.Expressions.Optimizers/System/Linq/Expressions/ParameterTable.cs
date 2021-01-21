// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - February 2017 - Created this file.
//

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Represents a table with <see cref="ParameterInfo"/> objects.
    /// </summary>
    public partial class ParameterTable : IEnumerable<ParameterInfo>
    {
        /// <summary>
        /// Indicates whether the table is read-only.
        /// </summary>
        private bool _readOnly;

        /// <summary>
        /// A set of parameters on non-generic methods.
        /// </summary>
        private readonly HashSet<ParameterInfo> Parameters = new();

        /// <summary>
        /// Marks the current parameter table as read-only, preventing subsequent mutation.
        /// </summary>
        /// <returns>The current parameter table after being marked as read-only.</returns>
        public ParameterTable ToReadOnly()
        {
            _readOnly = true;
            return this;
        }

        /// <summary>
        /// Adds an entry to the parameter table using the <see cref="System.Reflection.ParameterInfo" /> that's assigned
        /// to by the first parameter of the specified lambda <paramref name="expression" />.
        /// </summary>
        /// <param name="expression">The expression to obtain the parameter to add from.</param>
        public void Add(LambdaExpression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (expression.Parameters.Count == 0)
                throw new ArgumentException("The lambda expression should have at least one parameter.", nameof(expression));

            CheckReadOnly();

            var find = new FindParameter(expression.Parameters[0]);
            find.Visit(expression.Body);

            if (find.Parameter == null)
                throw new ArgumentException("The first parameter of the lambda expression should be used in a parameter assignment site within the body of the expression.", nameof(expression));

            Add(find.Parameter);
        }

        /// <summary>
        /// Copies the entries in the specified parameter <paramref name="table"/> to the current table.
        /// </summary>
        /// <param name="table">The parameter table whose entries to copy.</param>
        public void Add(ParameterTable table)
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table));

            CheckReadOnly();

            foreach (var parameter in table.Parameters)
            {
                Parameters.Add(parameter);
            }
        }

        /// <summary>
        /// Adds the specified <paramref name="parameter"/> to the table.
        /// </summary>
        /// <param name="parameter">The parameter to add to the table.</param>
        public void Add(ParameterInfo parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));

            CheckReadOnly();

            if (parameter.Member is MethodBase method)
            {
                if (method.IsGenericMethod)
                    throw new NotSupportedException("Generic methods are not yet supported."); // TODO

                if (method.DeclaringType != null && method.DeclaringType.IsGenericType)
                    throw new NotSupportedException("Methods declared on generic types are not yet supported."); // TODO
            }

            Parameters.Add(parameter);
        }

        /// <summary>
        /// Gets a sequence of parameters in the current parameter table.
        /// </summary>
        /// <returns>A sequence of parameters in the current parameter table.</returns>
        public IEnumerator<ParameterInfo> GetEnumerator() => Parameters.GetEnumerator();

        /// <summary>
        /// Gets a sequence of parameters in the current parameter table.
        /// </summary>
        /// <returns>A sequence of parameters in the current parameter table.</returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Checks if the specified <paramref name="parameter"/> is present in the table.
        /// </summary>
        /// <param name="parameter">The parameter to check.</param>
        /// <returns><c>true</c> if the specified <paramref name="parameter"/> is present in the table; otherwise, <c>false</c>.</returns>
        public bool Contains(ParameterInfo parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));

            return Parameters.Contains(parameter);
        }

        /// <summary>
        /// Checks if the current parameter table is marked as read-only.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the current parameter table is marked as read-only.
        /// </exception>
        private void CheckReadOnly()
        {
            if (_readOnly)
                throw new InvalidOperationException("The table is marked as read-only.");
        }

        private sealed class FindParameter : ExpressionVisitor
        {
            private readonly ParameterExpression _parameter;

            public ParameterInfo Parameter;

            public FindParameter(ParameterExpression parameter) => _parameter = parameter;

            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                Find(node.Arguments, node.Method.GetParameters());

                return base.VisitMethodCall(node);
            }

            protected override Expression VisitNew(NewExpression node)
            {
                if (node.Constructor != null)
                {
                    Find(node.Arguments, node.Constructor.GetParameters());
                }

                return base.VisitNew(node);
            }

            protected override Expression VisitIndex(IndexExpression node)
            {
                if (node.Indexer != null)
                {
                    Find(node.Arguments, node.Indexer.GetIndexParameters());
                }

                return base.VisitIndex(node);
            }

            private void Find(ReadOnlyCollection<Expression> arguments, ParameterInfo[] parameters)
            {
                var i = 0;

                foreach (var arg in arguments)
                {
                    if (arg == _parameter)
                    {
                        Found(parameters, i);
                    }

                    i++;
                }
            }

            private void Found(ParameterInfo[] parameters, int i)
            {
                if (Parameter != null)
                {
                    throw new InvalidOperationException("The parameter has multiple use sites.");
                }

                Parameter = parameters[i];
            }
        }
    }
}
