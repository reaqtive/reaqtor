// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

using System.Linq.Expressions;
using System.Reflection;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Provides various utilities for reflection.
    /// </summary>
    public static class ReflectionHelpers
    {
        /// <summary>
        /// Gets the reflection member information from the top-level node in the body of the given lambda expression.
        /// </summary>
        /// <typeparam name="T">Input type of the lambda.</typeparam>
        /// <typeparam name="TResult">Return type of the lambda.</typeparam>
        /// <param name="expression">Lambda expression to extract reflection information from</param>
        /// <returns>Member information of the top-level node in the body of the lambda expression. An exception occurs if this node does not contain member information.</returns>
        /// <example>
        /// To obtain the MethodInfo for the "int Math::Abs(int)" overload, you can write:
        /// <code>(MethodInfo)ReflectionHelpers.InfoOf((int x) => Math.Abs(x))</code>
        /// </example>
        public static MemberInfo InfoOf<T, TResult>(Expression<Func<T, TResult>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return InfoOf(expression.Body);
        }

        /// <summary>
        /// Gets the reflection member information from the top-level node in the body of the given lambda expression.
        /// </summary>
        /// <typeparam name="TResult">Return type of the lambda.</typeparam>
        /// <param name="expression">Lambda expression to extract reflection information from</param>
        /// <returns>Member information of the top-level node in the body of the lambda expression. An exception occurs if this node does not contain member information.</returns>
        /// <example>
        /// To obtain the PropertyInfo of "DateTime DateTime::Now { get; }", you can write:
        /// <code>(PropertyInfo)ReflectionHelpers.InfoOf(() => DateTime.Now)</code>
        /// </example>
        public static MemberInfo InfoOf<TResult>(Expression<Func<TResult>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return InfoOf(expression.Body);
        }

        /// <summary>
        /// Gets the reflection member information from the top-level node in the body of the given lambda expression.
        /// </summary>
        /// <typeparam name="T">Input type of the lambda.</typeparam>
        /// <param name="expression">Lambda expression to extract reflection information from</param>
        /// <returns>Member information of the top-level node in the body of the lambda expression. An exception occurs if this node does not contain member information.</returns>
        /// <example>
        /// To obtain the MethodInfo for the "void Console::WriteLine(string)" overload, you can write:
        /// <code>(MethodInfo)ReflectionHelpers.InfoOf((string s) => Console.WriteLine(s))</code>
        /// </example>
        public static MemberInfo InfoOf<T>(Expression<Action<T>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return InfoOf(expression.Body);
        }

        /// <summary>
        /// Gets the reflection member information from the top-level node in the body of the given lambda expression.
        /// </summary>
        /// <param name="expression">Lambda expression to extract reflection information from</param>
        /// <returns>Member information of the top-level node in the body of the lambda expression. An exception occurs if this node does not contain member information.</returns>
        /// <example>
        /// To obtain the MethodInfo for the "void Console::WriteLine()" overload, you can write:
        /// <code>(MethodInfo)ReflectionHelpers.InfoOf(() => Console.WriteLine())</code>
        /// </example>
        public static MemberInfo InfoOf(Expression<Action> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return InfoOf(expression.Body);
        }

        /// <summary>
        /// Gets the reflection member information from the top-level node in the body of the given lambda expression.
        /// </summary>
        /// <param name="expression">Lambda expression to extract reflection information from</param>
        /// <returns>Member information of the top-level node in the body of the lambda expression. An exception occurs if this node does not contain member information.</returns>
        public static MemberInfo InfoOf(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return expression switch
            {
                MethodCallExpression mce => mce.Method,
                MemberExpression me => me.Member,
                NewExpression ne => ne.Constructor,
                UnaryExpression ue when ue.Method != null => ue.Method,
                BinaryExpression be when be.Method != null => be.Method,
                _ => throw new NotSupportedException("Expression tree type doesn't have an extractable MemberInfo object."),
            };
        }
    }
}
