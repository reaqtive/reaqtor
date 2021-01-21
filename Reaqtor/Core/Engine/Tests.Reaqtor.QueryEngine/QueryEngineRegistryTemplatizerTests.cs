// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Reaqtor.QueryEngine;
using Reaqtor.QueryEngine.Mocks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Reaqtor.QueryEngine
{
    [TestClass]
    public class QueryEngineRegistryTemplatizerTests
    {
        private static readonly Expression<Func<int>> SimpleExpression = () => 42;
        private static readonly Expression<Func<int>> SimpleExpression2 = () => 7;
        private static readonly Expression<Func<long>> SimpleExpression3 = () => 29L;
        private static readonly Expression<Func<int, int>> IdentityExpression = x => x;

        [TestMethod]
        public void QueryEngineRegistryTemplatizer_NewExpressionTemplateAdded()
        {
            var registry = CreateRegistry();
            var templatizer = CreateTemplatizer(registry);

            Assert.AreEqual(0, registry.Templates.Count());

            var templatized = (InvocationExpression)templatizer.Templatize(SimpleExpression);
            Assert.AreEqual(SimpleExpression.Type, templatized.Type);

            var template = registry.Templates.First();
            var templatizedBody = templatized.Expression as ParameterExpression;

            Assert.IsNotNull(templatizedBody);
            Assert.AreEqual(template.Key, templatizedBody.Name);
            Assert.AreEqual(1, templatized.Arguments.Count);
            Assert.AreEqual(Tuple.Create(42), templatized.Arguments.First().Evaluate());
        }

        [TestMethod]
        public void QueryEngineRegistryTemplatizer_TemplateReused()
        {
            var registry = CreateRegistry();
            var templatizer = CreateTemplatizer(registry);

            Assert.AreEqual(0, registry.Templates.Count());

            var templatized = (InvocationExpression)templatizer.Templatize(SimpleExpression);
            var template = registry.Templates.First();
            var templatizedBody = templatized.Expression as ParameterExpression;

            Assert.IsNotNull(templatizedBody);
            Assert.AreEqual(template.Key, templatizedBody.Name);
            Assert.AreEqual(1, templatized.Arguments.Count);
            Assert.AreEqual(Tuple.Create(42), templatized.Arguments.First().Evaluate());

            _ = templatizer.Templatize(SimpleExpression2);
            Assert.AreEqual(1, registry.Templates.Count());
        }

        [TestMethod]
        public void QueryEngineRegistryTemplatizer_Detemplatize()
        {
            var registry = CreateRegistry();
            var templatizer = CreateTemplatizer(registry);
            var templatized = templatizer.Templatize(SimpleExpression);
            var detemplatized = templatizer.Detemplatize(templatized);
            AssertEquals(SimpleExpression, detemplatized);
        }

        [TestMethod]
        public void QueryEngineRegistryTemplatizer_NotRetemplatized()
        {
            var registry = CreateRegistry();
            var templatizer = CreateTemplatizer(registry);
            var templatized = templatizer.Templatize(SimpleExpression);
            var retemplatized = templatizer.Templatize(templatized);
            Assert.AreSame(templatized, retemplatized);
        }

        [TestMethod]
        public void QueryEngineRegistryTemplatizer_AddNewTemplateRace()
        {
            var races = 10000;
            for (var i = 0; i < races; ++i)
            {
                var registry = CreateRegistry();
                var templatizer = CreateTemplatizer(registry);

                var r1 = default(Expression);
                var r2 = default(Expression);

                var tasks = new[]
                {
                    Task.Run(() => r1 = templatizer.Templatize(SimpleExpression)),
                    Task.Run(() => r2 = templatizer.Templatize(SimpleExpression)),
                };

                Task.WaitAll(tasks);

                var t1 = ((InvocationExpression)r1).Expression;
                var t2 = ((InvocationExpression)r2).Expression;

                Assert.AreEqual(((ParameterExpression)t1).Name, ((ParameterExpression)t2).Name);
                Assert.AreEqual(1, registry.Templates.Count());
            }
        }

        [TestMethod]
        public void QueryEngineRegistryTemplatizer_NoConstants_Templatized()
        {
            var registry = CreateRegistry();
            var templatizer = CreateTemplatizer(registry);
            var templated = templatizer.Templatize(IdentityExpression);
            Assert.AreNotSame(IdentityExpression, templated);
            var f = templatizer.Detemplatize(templated);
            AssertEquals(IdentityExpression, f);
            Assert.AreEqual(1, registry.Templates.Count());
        }

        [TestMethod]
        public void QueryEngineRegistryTemplatizer_TemplateNotExists_ThrowsInvalidOperation()
        {
            var registry = CreateRegistry();
            var templatizer = CreateTemplatizer(registry);
            var templatized = templatizer.Templatize(SimpleExpression);
            registry.Clear();
            Assert.ThrowsException<InvalidOperationException>(() => templatizer.Detemplatize(templatized));
        }

        [TestMethod]
        public void QueryEngineRegistryTemplatizer_DynamicInvocationFailure()
        {
            var registry = CreateRegistry();
            var templatizer = CreateTemplatizer(registry);
            var templatized = templatizer.Templatize(SimpleExpression);
            var template = ((ParameterExpression)((InvocationExpression)templatized).Expression).Name;

            var newTemplatized = templatizer.Templatize(SimpleExpression3);
            var newTemplate = ((ParameterExpression)((InvocationExpression)newTemplatized).Expression).Name;

            Assert.IsTrue(registry.Templates.TryRemove(template, out var templateDefinition));
            Assert.IsTrue(registry.Templates.TryRemove(newTemplate, out templateDefinition));
            Assert.IsTrue(registry.Templates.TryAdd(template, new OtherDefinitionEntity(new Uri(template), templateDefinition.Expression, null)));

            Assert.ThrowsException<ArgumentException>(() => templatizer.Detemplatize(templatized));
        }

        [TestMethod]
        public void QueryEngineRegistryTemplatizer_NotTemplatized()
        {
            var registry = CreateRegistry();
            var templatizer = CreateTemplatizer(registry);
            _ = templatizer.Detemplatize(SimpleExpression);
            Assert.AreSame(SimpleExpression, SimpleExpression);
        }

        private static void AssertEquals(Expression x, Expression y)
        {
            Assert.IsTrue(new ExpressionEqualityComparer().Equals(x, y));
        }

        private static QueryEngineRegistry CreateRegistry()
        {
            return new QueryEngineRegistry(new MockQueryEngineRegistry());
        }

        private static QueryEngineRegistryTemplatizer CreateTemplatizer(QueryEngineRegistry registry)
        {
            return new QueryEngineRegistryTemplatizer(registry);
        }

        private sealed class TestVisitor : ExpressionVisitor
        {
            protected override Expression VisitBinary(BinaryExpression node)
            {
                if (node.NodeType == ExpressionType.Add)
                {
                    return Expression.Subtract(node.Left, node.Right);
                }

                return base.VisitBinary(node);
            }

            protected override Expression VisitConstant(ConstantExpression node)
            {
                if ((node.Value is int value) && value == 42)
                {
                    return Expression.Constant(43);
                }

                return base.VisitConstant(node);
            }
        }
    }
}
