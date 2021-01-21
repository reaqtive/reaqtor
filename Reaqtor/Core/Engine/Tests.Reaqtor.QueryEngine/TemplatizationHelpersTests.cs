// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtive;

using Reaqtor.Expressions;

namespace Tests.Reaqtor.QueryEngine
{
    [TestClass]
    public class TemplatizationHelpersTests
    {
        [TestMethod]
        public void TemplatizationHelpers_IsTemplatized()
        {
            var tparam = GetParameter(typeof(Tuple<int>), typeof(ISubscription));
            var targ = GetArg(42);
            var templatized = Expression.Invoke(tparam, targ);

            Assert.IsTrue(templatized.IsTemplatized());
            Assert.IsTrue(templatized.IsTemplatized(out var outParam, out var outArg));
            Assert.AreSame(tparam, outParam);
            Assert.AreSame(targ, outArg);
        }

        [TestMethod]
        public void TemplatizationHelpers_IsNotTemplatized()
        {
            var nonTemplatized1 = Expression.Invoke(GetParameter(typeof(Tuple<int>), typeof(ISubscription), invalid: true), GetArg(1));
            var nonTemplatized2 = Expression.Default(typeof(int));
            Expression<Func<Tuple<int>, ISubscription>> nonTemplatizedBody3 = t => default;
            var nonTemplatized3 = Expression.Invoke(nonTemplatizedBody3, GetArg(42));
            var nonTemplatized4 = Expression.Invoke(Expression.Parameter(typeof(Func<Tuple<int>, ISubscription>)), GetArg(42));
            var nonTemplatized5 = Expression.Invoke(Expression.Parameter(typeof(Func<int, int, ISubscription>), TemplatizationHelpers.TemplateBase + "2"), Expression.Constant(42), Expression.Constant(43));

            Assert.IsFalse(nonTemplatized1.IsTemplatized());
            Assert.IsFalse(nonTemplatized2.IsTemplatized());
            Assert.IsFalse(nonTemplatized3.IsTemplatized());
            Assert.IsFalse(nonTemplatized4.IsTemplatized());
            Assert.IsFalse(nonTemplatized5.IsTemplatized());
            Assert.IsFalse(((Expression)null).IsTemplatized());
        }

        [TestMethod]
        public void TemplatizationHelpers_TemplatizeAndIdentify_ArgumentChecks()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => ((Expression)null).TemplatizeAndIdentify(), ex => Assert.AreEqual("expression", ex.ParamName));
        }

        [TestMethod]
        public void TemplatizationHelpers_TemplatizeAndIdentify()
        {
            Expression<Func<string>> withConstants = () => 42.ToString();
            var appliedTemplate = withConstants.TemplatizeAndIdentify();
            Assert.AreNotSame(withConstants, appliedTemplate.Expression);
            Assert.AreEqual(typeof(Func<Tuple<int>, Func<string>>), appliedTemplate.Template.Type);
            Assert.IsTrue(appliedTemplate.TemplateId.StartsWith(TemplatizationHelpers.TemplateBase));
        }

        [TestMethod]
        public void TemplatizationHelpers_TemplatizeAndIdentify_Unchanged()
        {
            var tparam = GetParameter(typeof(Tuple<int>), typeof(ISubscription));
            var targ = GetArg(42);
            var templatized = Expression.Invoke(tparam, targ);
            Assert.AreSame(templatized, templatized.TemplatizeAndIdentify().Expression);
        }

        [TestMethod]
        public void TemplatizationHelpers_SubstituteTemplateId_ArgumentChecks()
        {
            AssertEx.ThrowsException<ArgumentNullException>(() => ((Expression)null).SubstituteTemplateId(replacement: null), ex => Assert.AreEqual("replacement", ex.ParamName));
        }

        [TestMethod]
        public void TemplatizationHelpers_SubstituteTemplateId_ThrowsInvalidOperation()
        {
            Assert.ThrowsException<InvalidOperationException>(() => Expression.New(typeof(int)).SubstituteTemplateId("foo"));
        }

        [TestMethod]
        public void TemplatizationHelpers_SubstituteTemplateId()
        {
            var tparam = GetParameter(typeof(Tuple<int>), typeof(ISubscription));
            var targ = GetArg(42);
            var templatized = Expression.Invoke(tparam, targ);

            var replacement = TemplatizationHelpers.TemplateBase + Guid.NewGuid();
            var replaced = templatized.SubstituteTemplateId(replacement);

            Assert.IsTrue(replaced.IsTemplatized(out var outParam, out var outArg));
            Assert.AreSame(replacement, outParam.Name);
            Assert.AreSame(targ, outArg);
        }

        private static ParameterExpression GetParameter(Type arg, Type res, bool invalid = false)
        {
            var id = invalid ? Guid.NewGuid().ToString() : TemplatizationHelpers.TemplateBase + Guid.NewGuid();
            return Expression.Parameter(typeof(Func<,>).MakeGenericType(arg, res), id);
        }

        private static Expression GetArg<T>(T arg)
        {
            Expression<Func<Tuple<T>>> factory = () => new Tuple<T>(arg);
            return factory.Body;
        }
    }
}
