// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - November 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.CompilerServices;
using System.Linq.CompilerServices.TypeSystem;
using System.Linq.Expressions;

namespace Reaqtor.Remoting.QueryEvaluator
{
    public class MockQueryEvaluatorUriToReactiveProxyBinder : UriToReactiveProxyBinder
    {
        private static readonly IDictionary<string, LambdaExpression> externalFunctions = new Dictionary<string, LambdaExpression>
        {
            { Platform.Constants.Identifiers.GenerateLanguage, (Expression<Func<Tuple<T, string>, string>>)(t => Generate<T>(t.Item1, t.Item2)) },
            { Platform.Constants.Identifiers.IsPrime, (Expression<Func<Tuple<int>, bool>>)(t => IsPrime(t.Item1)) }
        };

        protected override Expression LookupOther(string id, Type type, Type funcType)
        {
            if (!externalFunctions.TryGetValue(id, out var expr))
            {
                return base.LookupOther(id, type, funcType);
            }

            var typeMap = funcType.UnifyExact(expr.Type);
            var substitutor = new TypeSubstitutionExpressionVisitor(typeMap);
            var unifiedExpr = substitutor.Visit(expr);

            return unifiedExpr;
        }

        private static string Generate<T>(T value, string lang)
        {
            return string.Format(CultureInfo.InvariantCulture, "Generating output for '{0}' in language '{1}'.", value, lang);
        }

        private static bool IsPrime(int i)
        {
            if (i <= 1)
                return false;

            var max = (int)Math.Sqrt(i);
            for (var j = 2; j <= max; ++j)
                if (i % j == 0)
                    return false;

            return true;
        }
    }
}
