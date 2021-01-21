// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - February 2018
//

using System.Linq.Expressions;
using System.Reflection;

using Reaqtor;

namespace Playground
{
    internal static partial class EngineIntegrationTests
    {
        /// <summary>
        /// Expression visitor to convert invocations of `rx://builtin/id` to expressions of type <see cref="ExpressionType.Convert"/>.
        /// </summary>
        private sealed class CastVisitor : ExpressionVisitor
        {
            public static T Apply<T>(T expression) where T : Expression => new CastVisitor().VisitAndConvert(expression, nameof(Apply));

            //
            // NB: This utility implements a very narrow case needed for integration tests. It's by no means meant to be reusable as-is.
            //

            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                if (node.Object == null)
                {
                    var kra = node.Method.GetCustomAttribute<KnownResourceAttribute>();

                    if (kra != null && kra.Uri == "rx://builtin/id")
                    {
                        var arg = Visit(node.Arguments[0]);
                        return Expression.Convert(arg, node.Type);
                    }
                }

                return base.VisitMethodCall(node);
            }
        }
    }
}
