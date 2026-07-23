// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Linq.CompilerServices;
using System.Linq.Expressions;

namespace Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>
/// Provides assertions for expression tree equality, treating global (unbound) parameters as equal
/// when they agree on name and type.
/// </summary>
/// <remarks>
/// This file is linked into test projects that need it via an explicit Compile Include; unlike
/// AssertEx.cs it is not linked globally because it requires a reference to Nuqleon.Linq.CompilerServices.
/// </remarks>
public static class ExpressionAssert
{
    public static void AreEqual(Expression expected, Expression actual)
    {
        Assert.IsTrue(new ExpressionEqualityComparer(() => new GlobalParameterComparator()).Equals(expected, actual));
    }

    public static void AreNotEqual(Expression expected, Expression actual)
    {
        Assert.IsFalse(new ExpressionEqualityComparer(() => new GlobalParameterComparator()).Equals(expected, actual));
    }

    private sealed class GlobalParameterComparator : ExpressionEqualityComparator
    {
        protected override bool EqualsGlobalParameter(ParameterExpression x, ParameterExpression y)
        {
            return x.Name == y.Name && Equals(x.Type, y.Type);
        }
    }
}
