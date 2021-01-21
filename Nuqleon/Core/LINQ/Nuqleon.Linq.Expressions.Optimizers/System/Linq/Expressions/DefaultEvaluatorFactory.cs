// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using System.Diagnostics;
using System.Reflection;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Factory for member invocation delegates.
    /// </summary>
    public class DefaultEvaluatorFactory : IEvaluatorFactory
    {
        /// <summary>
        /// Singleton instance of an empty array of parameters for reuse during expression tree creation.
        /// </summary>
        private static readonly ParameterExpression[] s_noParams = Array.Empty<ParameterExpression>();

        /// <summary>
        /// Gets an evaluator delegate for the specified <paramref name="member"/>.
        /// </summary>
        /// <param name="member">The member to get an evaluator delegate for.</param>
        /// <returns>An evaluator delegate for the specified <paramref name="member"/>.</returns>
        public virtual Delegate GetEvaluator(MemberInfo member)
        {
            if (member == null)
                throw new ArgumentNullException(nameof(member));

            return member.MemberType switch
            {
                MemberTypes.Constructor => GetEvaluator((ConstructorInfo)member),
                MemberTypes.Field => GetEvaluator((FieldInfo)member),
                MemberTypes.Method => GetEvaluator((MethodInfo)member),
                MemberTypes.Property => GetEvaluator((PropertyInfo)member),
                MemberTypes.NestedType or MemberTypes.TypeInfo => GetEvaluator((Type)member),
                _ => throw new InvalidOperationException($"Unexpected member type '{member.MemberType}'."),
            };
        }

        /// <summary>
        /// Gets an evaluator delegate for the specified <paramref name="method"/>.
        /// </summary>
        /// <param name="method">The method to get an evaluator delegate for.</param>
        /// <returns>An evaluator delegate for the specified <paramref name="method"/>.</returns>
        public virtual Delegate GetEvaluator(MethodInfo method)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));

            ParameterExpression[] pars;
            Expression body;

            var parameters = method.GetParameters();
            if (method.IsStatic)
            {
                var n = parameters.Length;

                pars = new ParameterExpression[n];

                var args = new Expression[n];

                for (var i = 0; i < n; i++)
                {
                    var parameter = parameters[i];

                    var parameterType = parameter.ParameterType;

                    var par = Expression.Parameter(typeof(object), parameter.Name);
                    args[i] = Expression.Convert(par, parameterType);
                    pars[i] = par;
                }

                body = Expression.Call(method, args);
            }
            else
            {
                var n = parameters.Length;

                pars = new ParameterExpression[n + 1];

                var args = new Expression[n];

                var instancePar = Expression.Parameter(typeof(object), "obj");
                var instance = Expression.Convert(instancePar, method.DeclaringType);
                pars[0] = instancePar;

                for (var i = 0; i < n; i++)
                {
                    var parameter = parameters[i];
                    var par = Expression.Parameter(typeof(object), parameter.Name);
                    args[i] = Expression.Convert(par, parameter.ParameterType);
                    pars[i + 1] = par;
                }

                body = Expression.Call(instance, method, args);
            }

            return GetEvaluator(body, pars);
        }

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1716 // Reserved language keyword 'property'.

        /// <summary>
        /// Gets an evaluator delegate for the specified <paramref name="property"/>.
        /// </summary>
        /// <param name="property">The method to get an evaluator delegate for.</param>
        /// <returns>An evaluator delegate for the specified <paramref name="property"/>.</returns>
        public virtual Delegate GetEvaluator(PropertyInfo property)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));

            ParameterExpression[] pars;
            Expression body;

            var getMethod = property.GetGetMethod();
            var indexerParameters = property.GetIndexParameters();
            if (getMethod.IsStatic)
            {
                if (indexerParameters.Length != 0)
                {
                    throw new NotSupportedException("Static indexed properties are not supported.");
                }

                pars = s_noParams;
                body = Expression.Property(expression: null, property);
            }
            else
            {
                var instancePar = Expression.Parameter(property.DeclaringType, "obj");

                var n = indexerParameters.Length;
                if (n == 0)
                {
                    pars = new[] { instancePar };
                    body = Expression.Property(instancePar, property);
                }
                else
                {
                    pars = new ParameterExpression[n + 1];
                    var args = new Expression[n];

                    pars[0] = instancePar;

                    for (var i = 0; i < n; i++)
                    {
                        var indexerParameter = indexerParameters[i];

                        var indexerParameterType = indexerParameter.ParameterType;

                        var par = Expression.Parameter(typeof(object), indexerParameter.Name);
                        args[i] = Expression.Convert(par, indexerParameterType);
                        pars[i + 1] = par;
                    }

                    body = Expression.MakeIndex(instancePar, property, args);
                }
            }

            return GetEvaluator(body, pars);
        }

#pragma warning restore CA1716
#pragma warning restore IDE0079

        /// <summary>
        /// Gets an evaluator delegate for the specified <paramref name="field"/>.
        /// </summary>
        /// <param name="field">The field to get an evaluator delegate for.</param>
        /// <returns>An evaluator delegate for the specified <paramref name="field"/>.</returns>
        public virtual Delegate GetEvaluator(FieldInfo field)
        {
            if (field == null)
                throw new ArgumentNullException(nameof(field));

            ParameterExpression[] pars;
            Expression body;

            if (field.IsStatic)
            {
                pars = s_noParams;
                body = Expression.Field(expression: null, field);
            }
            else
            {
                var instancePar = Expression.Parameter(field.DeclaringType, "obj");

                pars = new[] { instancePar };
                body = Expression.Field(instancePar, field);
            }

            return GetEvaluator(body, pars);
        }

        /// <summary>
        /// Gets an evaluator delegate for the specified <paramref name="constructor"/>.
        /// </summary>
        /// <param name="constructor">The constructor to get an evaluator delegate for.</param>
        /// <returns>An evaluator delegate for the specified <paramref name="constructor"/>.</returns>
        public virtual Delegate GetEvaluator(ConstructorInfo constructor)
        {
            if (constructor == null)
                throw new ArgumentNullException(nameof(constructor));

            var parameters = constructor.GetParameters();

            var n = parameters.Length;

            var pars = new ParameterExpression[n];
            var args = new Expression[n];

            for (var i = 0; i < n; i++)
            {
                var parameter = parameters[i];

                var parameterType = parameter.ParameterType;

                var par = Expression.Parameter(typeof(object), parameter.Name);
                args[i] = Expression.Convert(par, parameterType);
                pars[i] = par;
            }

            var body = Expression.New(constructor, args);

            return GetEvaluator(body, pars);
        }

        /// <summary>
        /// Gets an evaluator delegate to create an instance of the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to get an instantiation evaluator delegate for.</param>
        /// <returns>An evaluator delegate to create an instance of the specified <paramref name="type"/>.</returns>
        public virtual Delegate GetEvaluator(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return GetEvaluator(Expression.New(type));
        }

        /// <summary>
        /// Get an evaluator delegate using the specified <paramref name="body"/> and <paramref name="parameters"/> to compile an evaluation lambda.
        /// </summary>
        /// <param name="body">The body of the evaluation lambda.</param>
        /// <param name="parameters">The parameters of the evaluation lambda.</param>
        /// <returns>An evaluation delegate.</returns>
        public virtual Delegate GetEvaluator(Expression body, params ParameterExpression[] parameters)
        {
            if (body == null)
                throw new ArgumentNullException(nameof(body));

            var evalBody = body;

            if (body.Type != typeof(void))
            {
                evalBody =
                    Utils.ConvertTo(
                        body,
                        typeof(object)
                    );
            }

            var expr =
                Expression.Lambda(
                    evalBody,
                    parameters
                );

            var eval = expr.Compile();

            return eval;
        }

        /// <summary>
        /// Gets an evaluator delegate for the specified <paramref name="expression"/> that's free of parameters.
        /// </summary>
        /// <param name="expression">The expression to get an evaluator delegate for.</param>
        /// <returns>A delegate that returns an <see cref="object"/> with the result of evaluating the expression.</returns>
        public virtual Func<object> GetEvaluator(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return (Func<object>)GetEvaluator(Utils.ConvertTo(expression, typeof(object)), s_noParams);
        }

        /// <summary>
        /// Get an evaluator delegate for the specified unary expression template. The operand of the template expression
        /// will be substituted for the parameter passed in to the evaluator.
        /// </summary>
        /// <param name="node">The unary expression template to build an evaluator for.</param>
        /// <returns>An evaluator delegate to evaluate the specified parameterized unary expression.</returns>
        public virtual Func<object, object> GetEvaluator(UnaryExpression node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            Debug.Assert(node.Method == null);
            Debug.Assert(node.Operand != null);

            var opType = node.Operand.Type;

            var opParam = Expression.Parameter(typeof(object));
            var opValue = Utils.ConvertTo(opParam, opType);

            var unary = node.Update(opValue);
            var body = Utils.ConvertTo(unary, typeof(object));

            return (Func<object, object>)GetEvaluator(body, opParam);
        }

        /// <summary>
        /// Get an evaluator delegate for the specified binary expression template. The operands of the template expression
        /// will be substituted for the parameters passed in to the evaluator.
        /// </summary>
        /// <param name="node">The binary expression template to build an evaluator for.</param>
        /// <returns>An evaluator delegate to evaluate the specified parameterized binary expression.</returns>
        public virtual Func<object, object, object> GetEvaluator(BinaryExpression node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            Debug.Assert(node.Method == null);
            Debug.Assert(node.Conversion == null);
            Debug.Assert(node.Left != null);
            Debug.Assert(node.Right != null);

            var leftType = node.Left.Type;
            var rightType = node.Right.Type;

            var leftParam = Expression.Parameter(typeof(object));
            var leftValue = Utils.ConvertTo(leftParam, leftType);

            var rightParam = Expression.Parameter(typeof(object));
            var rightValue = Utils.ConvertTo(rightParam, rightType);

            var binary = node.Update(leftValue, conversion: null, rightValue);
            var body = Utils.ConvertTo(binary, typeof(object));

            return (Func<object, object, object>)GetEvaluator(body, leftParam, rightParam);
        }
    }
}
