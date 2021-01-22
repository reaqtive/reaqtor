// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

namespace Reaqtor.Remoting.Deployable.Streams
{
    public class LambdaKeySelector<T, TResult> : IKeySelector<T, TResult>
    {
        private readonly Expression<Func<T, TResult>> _expression;

        private Func<T, TResult> _delegate;

        public LambdaKeySelector(Expression<Func<T, TResult>> expression) => _expression = expression;

        private Func<T, TResult> Delegate => _delegate ??= _expression.Compile();

        public TResult Invoke(T arg) => Delegate(arg);

        public override int GetHashCode() => LambdaKeySelectorHelpers.Comparer.GetHashCode(_expression);

        public override bool Equals(object obj) => obj is LambdaKeySelector<T, TResult> lks && LambdaKeySelectorHelpers.Comparer.Equals(_expression, lks._expression);
    }

    internal static class LambdaKeySelectorHelpers
    {
        public static readonly ExpressionEqualityComparer Comparer = new();
    }
}
