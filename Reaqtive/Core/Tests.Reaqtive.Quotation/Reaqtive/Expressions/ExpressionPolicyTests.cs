// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Memory;
using System.Reflection;

using Reaqtive.Expressions;

namespace Test.Reaqtive.Expressions;

[TestClass]
public class ExpressionPolicyTests
{
    [TestMethod]
    public void ExpressionPolicy_Default_Behavior()
    {
        var expr1 = Expression.Lambda(Expression.Constant(42, typeof(int)));
        var expr2 = Expression.Lambda(Expression.Constant(42, typeof(int)));
        Assert.AreNotSame(expr1, expr2);

        var del1 = expr1.Compile(DefaultExpressionPolicy.Instance.DelegateCache);
        var del2 = expr1.Compile(DefaultExpressionPolicy.Instance.DelegateCache);
        Assert.AreNotSame(del1, del2);

        using (var ref1 = DefaultExpressionPolicy.Instance.InMemoryCache.Create(expr1))
        using (var ref2 = DefaultExpressionPolicy.Instance.InMemoryCache.Create(expr2))
        {
            Assert.AreNotSame(ref1.Value, ref2.Value);
        }

        Assert.AreEqual(0, DefaultExpressionPolicy.Instance.DelegateCache.Count);
        DefaultExpressionPolicy.Instance.DelegateCache.Clear();
        Assert.AreEqual(0, DefaultExpressionPolicy.Instance.DelegateCache.Count);
    }

    [TestMethod]
    public void ExpressionPolicy_Extensions_ArgumentChecks()
    {
        var ex = Assert.ThrowsExactly<ArgumentNullException>(() => ExpressionEvaluationPolicyExtensions.Evaluate<int>(null, Expression.Default(typeof(int))));
        Assert.AreEqual("policy", ex.ParamName);
        var ex2 = Assert.ThrowsExactly<ArgumentNullException>(() => ExpressionEvaluationPolicyExtensions.Evaluate<int>(DefaultExpressionPolicy.Instance, null));
        Assert.AreEqual("expression", ex2.ParamName);
        var ex3 = Assert.ThrowsExactly<ArgumentException>(() => ExpressionEvaluationPolicyExtensions.Evaluate<int>(new TestPolicy(), Expression.Constant(42)));
        Assert.AreEqual("policy", ex3.ParamName);
        var ex4 = Assert.ThrowsExactly<ArgumentException>(() =>
            ExpressionEvaluationPolicyExtensions.Evaluate<int>(
                new TestPolicy()
                {
                    DelegateCache = new SimpleCompiledDelegateCache()
                },
                Expression.Constant(42)
            ));
        Assert.AreEqual("policy", ex4.ParamName);

        var ex5 = Assert.ThrowsExactly<ArgumentNullException>(() => DefaultExpressionPolicy.Instance.DelegateCache.GetOrAdd(null));
        Assert.AreEqual("expression", ex5.ParamName);
    }

    [TestMethod]
    public void ExpressionPolicy_WithConstantHoisting()
    {
        var policy = new TestPolicy
        {
            DelegateCache = new SimpleCompiledDelegateCache(),
            ConstantHoister = new TestHoister()
        };
        Assert.AreEqual(42, policy.Evaluate<int>(Expression.Constant(42)));
    }

    private sealed class TestPolicy : IExpressionPolicy
    {
        public ICompiledDelegateCache DelegateCache
        {
            get;
            set;
        }

        public ICache<Expression> InMemoryCache
        {
            get;
            set;
        }

        public IConstantHoister ConstantHoister
        {
            get;
            set;
        }

        public bool OutlineCompilation
        {
            get;
            set;
        }

        public IReflectionProvider ReflectionProvider
        {
            get;
            set;
        }

        public IExpressionFactory ExpressionFactory
        {
            get;
            set;
        }

        public IMemoizer LiftMemoizer
        {
            get;
            set;
        }

        public IMemoizer ReduceMemoizer
        {
            get;
            set;
        }
    }

    private sealed class TestHoister : IConstantHoister
    {
        public IExpressionWithEnvironment Hoist(Expression expression)
        {
            return new ExpressionWithEnvironment(expression);
        }

        private sealed class ExpressionWithEnvironment : IExpressionWithEnvironment
        {
            public ExpressionWithEnvironment(Expression expression)
            {
                Expression = expression;
            }

            public IReadOnlyList<Binding> Bindings => EmptyReadOnlyCollection<Binding>.Instance;

            public Expression Expression { get; }
        }
    }
}
