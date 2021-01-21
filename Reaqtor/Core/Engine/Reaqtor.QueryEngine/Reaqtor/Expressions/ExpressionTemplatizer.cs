// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

namespace Reaqtor.Expressions
{
    /// <summary>
    /// Expression rewriter to extract constants from expressions and return a lambda
    /// template, along with a tuple of all the constants in the original expression.
    /// The invocation of the lambda template with the argument returns an expression
    /// that is semantically equivalent to the original.
    /// </summary>
    internal static class ExpressionTemplatizer
    {
        private static readonly IConstantHoister s_constantHoister = ConstantHoister.Create(useDefaultForNull: false);

        /// <summary>
        /// Templatizes an expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>The expression template.</returns>
        public static ExpressionTemplate Templatize(this Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            ICollection<ParameterExpression> globals = ExpressionHelpers.FindFreeVariables(expression);

            IExpressionWithEnvironment hoisted = s_constantHoister.Hoist(expression);
            IReadOnlyList<System.Linq.CompilerServices.Binding> hoistedBindings = hoisted.Bindings;

            var hoistedBindingsCount = hoistedBindings.Count;

            var n = globals.Count + hoistedBindingsCount;

            var res = new ExpressionTemplate();

            if (n == 0)
            {
                res.Template = Expression.Lambda(expression);
            }
            else
            {
                var parameters = new ParameterExpression[n];
                var arguments = new Expression[n];

                int i = 0;

                while (i < hoistedBindingsCount)
                {
                    var c = hoistedBindings[i];
                    parameters[i] = c.Parameter;
                    arguments[i] = c.Value;

                    i++;
                }

                foreach (var p in globals)
                {
                    parameters[i] = p;
                    arguments[i] = p;

                    i++;
                }

                Debug.Assert(i == n);

                //
                // In case you wonder why we're not building a LambdaExpression from the parameters
                // and the visited body, the reason is quite subtle. For lambda expressions with an
                // arity of 17 and beyond the Expression.Lambda factory method will use lightweight
                // code generation to create a delegate type. The code for this can be found in:
                //
                //  %DDROOT%\sources\ndp\fx\src\Core\Microsoft\Scripting\Compiler\AssemblyGen.cs
                //
                // The dynamic assembly used to host those delegate types is generated with the Run
                // option rather than RunAndCollect. If we end up creating a lambda expression that
                // uses LCG-generated types that are marked as RunAndCollect, an exception occurs:
                //
                //  System.NotSupportedException: A non-collectible assembly may not reference a collectible assembly.
                //    at System.Reflection.Emit.ModuleBuilder.GetTypeRef(RuntimeModule module, String strFullName, RuntimeModule refedModule, String strRefedModuleFileName, Int32 tkResolution)
                //    at System.Reflection.Emit.ModuleBuilder.GetTypeRefNested(Type type, Module refedModule, String strRefedModuleFileName)
                //    at System.Reflection.Emit.ModuleBuilder.GetTypeTokenWorkerNoLock(Type type, Boolean getGenericDefinition)
                //    at System.Reflection.Emit.ModuleBuilder.GetTypeTokenInternal(Type type, Boolean getGenericDefinition)
                //    at System.Reflection.Emit.SignatureHelper.AddOneArgTypeHelperWorker(Type clsArgument, Boolean lastWasGenericInst)
                //    at System.Reflection.Emit.SignatureHelper.AddOneArgTypeHelperWorker(Type clsArgument, Boolean lastWasGenericInst)
                //    at System.Reflection.Emit.SignatureHelper.AddOneArgTypeHelper(Type clsArgument, Type[] requiredCustomModifiers, Type[] optionalCustomModifiers)
                //    at System.Reflection.Emit.SignatureHelper.AddArguments(Type[] arguments, Type[][] requiredCustomModifiers, Type[][] optionalCustomModifiers)
                //    at System.Reflection.Emit.SignatureHelper.GetMethodSigHelper(Module scope, CallingConventions callingConvention, Int32 cGenericParam, Type returnType, Type[] requiredReturnTypeCustomModifiers, Type[] optionalReturnTypeCustomModifiers, Type[] parameterTypes, Type[][] requiredParameterTypeCustomModifiers, Type[][] optionalParameterTypeCustomModifiers)
                //    at System.Reflection.Emit.MethodBuilder.GetMethodSignature()
                //    at System.Reflection.Emit.MethodBuilder.GetTokenNoLock()
                //    at System.Reflection.Emit.MethodBuilder.GetToken()
                //    at System.Reflection.Emit.MethodBuilder.SetImplementationFlags(MethodImplAttributes attributes)
                //    at System.Linq.Expressions.Compiler.DelegateHelpers.MakeNewCustomDelegate(Type[] types)
                //    at System.Linq.Expressions.Compiler.DelegateHelpers.MakeDelegateType(Type[] types)
                //    at System.Linq.Expressions.Expression.Lambda(Expression body, String name, Boolean tailCall, IEnumerable`1 parameters)
                //    at System.Linq.Expressions.Expression.Lambda(Expression body, ParameterExpression[] parameters)
                //
                // To work around this limitation, we sidestep the creation of a lambda altogether
                // and use a specialized overload to Pack that builds a tupletized lambda from the
                // specified body and parameters collection.
                //
                res.Template = ExpressionTupletizer.Pack(hoisted.Expression, parameters);
                res.Argument = ExpressionTupletizer.Pack(arguments);
            }

            return res;
        }
    }

    /// <summary>
    /// An expression template.
    /// </summary>
    internal struct ExpressionTemplate
    {
        /// <summary>
        /// The lambda template.
        /// </summary>
        public LambdaExpression Template;

        /// <summary>
        /// An expression, which, when used to invoke the template, returns a
        /// semantically equivalent expression to the original expression.
        /// </summary>
        public Expression Argument;
    }
}
