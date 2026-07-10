// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// HvR - July 2026 - Created this file as part of factoring expression services out of the archived Remoting sample (see #158).
//

using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json.Nodes;

using Reaqtor;

namespace Tests.Reaqtor;

[TestClass]
public class CheckedReactiveExpressionServicesTests
{
    [TestMethod]
    public void CheckedReactiveExpressionServices_DataModelMember_Preserved()
    {
        Expression<Func<string, int>> expr = s => s.Length;

        var normalized = Normalize(expr);

        var lambda = (LambdaExpression)normalized;
        var member = (MemberExpression)lambda.Body;
        Assert.AreEqual(typeof(string), member.Member.DeclaringType);
    }

    [TestMethod]
    public void CheckedReactiveExpressionServices_DateTimeOffsetArithmetic_Preserved()
    {
        Expression<Func<DateTimeOffset, DateTimeOffset>> expr = d => d + TimeSpan.FromSeconds(1);

        var normalized = Normalize(expr);

        var lambda = (LambdaExpression)normalized;
        Assert.AreEqual(ExpressionType.Add, lambda.Body.NodeType);
    }

    [TestMethod]
    public void CheckedReactiveExpressionServices_JsonNodeIndexer_Preserved()
    {
        Expression<Func<JsonNode, JsonNode>> expr = n => n["x"];

        var normalized = Normalize(expr);

        var lambda = (LambdaExpression)normalized;
        var call = (MethodCallExpression)lambda.Body;
        Assert.AreEqual(typeof(JsonNode), call.Method.DeclaringType);
    }

    [TestMethod]
    public void CheckedReactiveExpressionServices_JsonArrayIndexer_Preserved()
    {
        Expression<Func<JsonArray, JsonNode>> expr = a => a[0];

        var normalized = Normalize(expr);

        var lambda = (LambdaExpression)normalized;
        Assert.IsInstanceOfType<MethodCallExpression>(lambda.Body);
    }

    [TestMethod]
    public void CheckedReactiveExpressionServices_JsonValueCreate_Preserved()
    {
        Expression<Func<int, JsonNode>> expr = x => JsonValue.Create(x, null);

        var normalized = Normalize(expr);

        var lambda = (LambdaExpression)normalized;
        var call = (MethodCallExpression)lambda.Body;
        Assert.AreEqual(typeof(JsonValue), call.Method.DeclaringType);
    }

    [TestMethod]
    public void CheckedReactiveExpressionServices_DisallowedClosedSubexpression_FoldedToConstant()
    {
        Expression<Func<string>> expr = () => Path.Combine("a", "b");

        var normalized = Normalize(expr);

        var lambda = (LambdaExpression)normalized;
        var constant = (ConstantExpression)lambda.Body;
        Assert.AreEqual(Path.Combine("a", "b"), constant.Value);
    }

    [TestMethod]
    public void CheckedReactiveExpressionServices_DateTimeNow_PreservedByDataModelBlessing()
    {
        Expression<Func<DateTime>> expr = () => DateTime.Now;

        var normalized = Normalize(expr);

        // DateTime is a valid data model type, so IsAllowedMember blesses all of its members before
        // the declarative table (which deliberately omits Now and UtcNow) is ever consulted. This
        // documents the behavior inherited from the archived Remoting client library.
        var lambda = (LambdaExpression)normalized;
        Assert.IsInstanceOfType<MemberExpression>(lambda.Body);
    }

    [TestMethod]
    public void CheckedReactiveExpressionServices_DisallowedMemberOverBoundParameter_Throws()
    {
        Expression<Func<string, string>> expr = s => Path.Combine(s, "b");

        Assert.ThrowsExactly<System.Linq.CompilerServices.UnboundParameterException>(() => _ = Normalize(expr));
    }

    [TestMethod]
    public void CheckedReactiveExpressionServices_IsAllowedMemberHook_BlessesAdditionalMembers()
    {
        Expression<Func<string, string>> expr = s => Path.Combine(s, "b");

        var services = new PathBlessingExpressionServices(typeof(IReactiveClientProxy));
        var normalized = services.Normalize(expr);

        var lambda = (LambdaExpression)normalized;
        var call = (MethodCallExpression)lambda.Body;
        Assert.AreEqual(typeof(Path), call.Method.DeclaringType);
    }

    private static Expression Normalize(Expression expression)
    {
        var services = new CheckedReactiveExpressionServices(typeof(IReactiveClientProxy));
        return services.Normalize(expression);
    }

    private sealed class PathBlessingExpressionServices : CheckedReactiveExpressionServices
    {
        public PathBlessingExpressionServices(Type reactiveClientInterfaceType)
            : base(reactiveClientInterfaceType)
        {
        }

        protected override bool IsAllowedMember(MemberInfo member)
        {
            return member.DeclaringType == typeof(Path) || base.IsAllowedMember(member);
        }
    }
}
