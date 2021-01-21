// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD, ER - July 2013 - Created this file.
//

using System;
using System.Collections.ObjectModel;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

namespace Tests.System.Linq.Expressions.Bonsai
{
    public class TestBase
    {
        protected static readonly ReadOnlyCollection<TypeSlim> Empty = EmptyReadOnlyCollection<TypeSlim>.Instance;

        protected static SimpleTypeSlim SlimType => TypeSlimExtensions.IntegerType;

#pragma warning disable IDE0060 // Remove unused parameter
#pragma warning disable CA1822 // Mark static
        protected class Foo
        {
            public int baz;

            public Foo(int i, int j) { baz = 0; }

            public int this[int i, int j] => i * j;

            public int Bar { get; set; }

            public int Qux1(int i, int j) { return 0; }

            public T Qux2<T>(T i) { return i; }

            public Bar Element { get; set; }
        }

        protected class Qux
        {
            public int baz;

            public Qux(int i, int j) { baz = 0; }

            public int this[int i, int j] => i * j;

            public int Bar { get; set; }

            public int Qux1(int i, int j) { return 0; }

            public T Qux2<T>(T i) { return i; }

            public Baz Element { get; set; }
        }
#pragma warning restore CA1822

        protected class Bar
        {
        }

        protected class Baz
        {
        }
#pragma warning restore IDE0060 // Remove unused parameter

        protected static Expression ExpressionRoundtrip(Expression e)
        {
            var exprToSlim = new ExpressionToExpressionSlimConverter();
            var slimToExpr = new ExpressionSlimToExpressionConverter();
            return slimToExpr.Visit(exprToSlim.Visit(e));
        }

        protected static Type TypeRoundtrip(Type t)
        {
            var ts = new TypeSpace();
            var its = new InvertedTypeSpace();
            return TypeRoundtrip(t, ts, its);
        }

        protected static Type TypeRoundtrip(Type t, TypeSpace ts, InvertedTypeSpace its)
        {
            return its.ConvertType(ts.ConvertType(t));
        }

        protected static MemberInfo MemberInfoRoundtrip(MemberInfo m)
        {
            var ts = new TypeSpace();
            var its = new InvertedTypeSpace();
            return MemberInfoRoundtrip(m, ts, its);
        }

        protected static MemberInfo MemberInfoRoundtrip(MemberInfo m, TypeSpace ts, InvertedTypeSpace its)
        {
            return its.GetMember(ts.GetMember(m));
        }
    }
}
