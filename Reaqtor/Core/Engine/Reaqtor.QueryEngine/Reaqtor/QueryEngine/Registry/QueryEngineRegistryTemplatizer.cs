// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

using Reaqtor.Expressions;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Templatizes expressions using a query engine registry as the backing store.
    /// </summary>
    internal class QueryEngineRegistryTemplatizer
    {
        private readonly QueryEngineRegistry _registry;

        /// <summary>
        /// Instantiate the templatization helper.
        /// </summary>
        /// <param name="registry">The registry containing templates.</param>
        public QueryEngineRegistryTemplatizer(QueryEngineRegistry registry)
        {
            _registry = registry;
        }

        /// <summary>
        /// Templatize an expression, using an existing template in the
        /// query engine registry if possible. Otherwise, if no template
        /// exists yet, add a new entry to the registry as a side-effect.
        /// </summary>
        /// <param name="expression">The original expression.</param>
        /// <returns>The templatized expression.</returns>
        /// <remarks>
        /// If the expression does not contain any constants, the original
        /// expression will be returned unchanged.
        /// </remarks>
        public virtual Expression Templatize(Expression expression)
        {
            var appliedTemplate = expression.TemplatizeAndIdentify();
            var templatized = appliedTemplate.Expression;

            // If the expression has been templatized, it will differ from the original
            if (expression != templatized)
            {
#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA2000 // Dispose objects before losing scope. (Entity will be owned by the registry after operation completes; otherwise, its Dispose is unnecessary.)

                var templateEntity = new OtherDefinitionEntity(new Uri(appliedTemplate.TemplateId), appliedTemplate.Template, state: null);

                // Check if the template already exists using the inverted entity collection
                var updateTemplate = true;
                if (!_registry.Templates.TryGetKey(templateEntity, out string templateId))
                {
                    // Try to add the template, note there is a race here.
                    templateId = templateEntity.Uri.ToCanonicalString();
                    updateTemplate = false;
                    if (!_registry.Templates.TryAdd(templateId, templateEntity))
                    {
                        // Lost the race, grab the template that was just added.
                        updateTemplate = true;
                        var found = _registry.Templates.TryGetKey(templateEntity, out templateId);
                        Debug.Assert(found);
                    }
                }

#pragma warning restore CA2000
#pragma warning restore IDE0079

                if (updateTemplate)
                {
                    templatized = templatized.SubstituteTemplateId(templateId);
                }
            }

            return templatized;
        }

        /// <summary>
        /// Detemplatize an expression.
        /// </summary>
        /// <param name="expression">
        /// The expression to detemplatize.
        /// </param>
        /// <returns>
        /// The detemplatized expression, or the unmodified expression if
        /// detemplatization is not needed.
        /// </returns>
        public virtual Expression Detemplatize(Expression expression)
        {
            if (expression.IsTemplatized(out ParameterExpression parameter, out Expression argument))
            {
                // Get the template lambda
                var templateExpression = LookupTemplateLambda(parameter);

                if (argument != null)
                {
                    // Unpack the template
                    UnpackTemplateLambda(templateExpression, out Expression unpackedBody, out IEnumerable<ParameterExpression> unpackedParameters);

                    // Unpack the argument expression
                    var unpackedArguments = UnpackTemplateArguments(argument);

                    // Invoke the template with the unpacked arguments
                    return Bind(unpackedBody, unpackedParameters, unpackedArguments);
                }
                else
                {
                    return templateExpression.Body;
                }
            }

            return expression;
        }

        /// <summary>
        /// Unpacks the template lambda.
        /// </summary>
        /// <param name="lambda">The template lambda.</param>
        /// <param name="body">The unpacked template lambda body.</param>
        /// <param name="parameters">The unpacked template lambda parameters.</param>
        protected virtual void UnpackTemplateLambda(LambdaExpression lambda, out Expression body, out IEnumerable<ParameterExpression> parameters)
        {
            ExpressionTupletizer.Unpack(lambda, out body, out parameters);
        }

        /// <summary>
        /// Unpacks the template arguments.
        /// </summary>
        /// <param name="argument">The packed template argument.</param>
        /// <returns>The unpacked arguments.</returns>
        protected virtual IEnumerable<Expression> UnpackTemplateArguments(Expression argument)
        {
            return ExpressionTupletizer.Unpack(argument);
        }

        private LambdaExpression LookupTemplateLambda(ParameterExpression parameter)
        {
            // Get the template expression
            if (!_registry.Templates.TryGetValue(parameter.Name, out DefinitionEntity templateEntity))
            {
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "Could not find expression template for identifier '{0}'.",
                        parameter.Name));
            }

            return (LambdaExpression)templateEntity.Expression;
        }

        private static Expression Bind(Expression body, IEnumerable<ParameterExpression> parameters, IEnumerable<Expression> arguments)
        {
            var paramsArray = parameters.ToArray();
            var argsArray = arguments.ToArray();

            if (paramsArray.Length != argsArray.Length)
            {
                throw new InvalidOperationException("Too many arguments for template lambda.");
            }

            var bindings = new Dictionary<ParameterExpression, Expression>(paramsArray.Length);
            for (var i = 0; i < paramsArray.Length; ++i)
            {
                bindings.Add(paramsArray[i], argsArray[i]);
            }

            var binder = new Binder(bindings);
            return binder.Visit(body);
        }

        private sealed class Binder : ScopedExpressionVisitor<ParameterExpression>
        {
            private readonly Dictionary<ParameterExpression, Expression> _bindings;

            public Binder(Dictionary<ParameterExpression, Expression> bindings)
            {
                _bindings = bindings;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (!TryLookup(node, out _) && _bindings.TryGetValue(node, out Expression binding))
                {
                    return binding;
                }

                return base.VisitParameter(node);
            }

            protected override ParameterExpression GetState(ParameterExpression parameter) => parameter;
        }
    }
}
