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

    [TestMethod]
    public void CheckedReactiveExpressionServices_DateTimeNowAccessorForm_ExcludedFromAllowListTable()
    {
        // Uses the accessor MethodInfo call form (get_Now) rather than the property access form, and a
        // subclass that stops blessing DateTime wholesale so the declarative table's exclusions engage.
        var getNow = typeof(DateTime).GetProperty(nameof(DateTime.Now)).GetMethod;
        var expr = Expression.Lambda<Func<DateTime>>(Expression.Call(getNow));

        var services = new NoDateTimeBlessingExpressionServices(typeof(IReactiveClientProxy));
        var normalized = services.Normalize(expr);

        var lambda = (LambdaExpression)normalized;
        Assert.AreEqual(ExpressionType.Constant, lambda.Body.NodeType);
    }

    [TestMethod]
    public void CheckedReactiveExpressionServices_DateTimeArithmetic_AllowedByTableWhenBlessingNarrowed()
    {
        // With DateTime no longer blessed wholesale, non-excluded members still pass via the declarative table.
        Expression<Func<DateTime, DateTime>> expr = d => d.AddDays(1);

        var services = new NoDateTimeBlessingExpressionServices(typeof(IReactiveClientProxy));
        var normalized = services.Normalize(expr);

        var lambda = (LambdaExpression)normalized;
        var call = (MethodCallExpression)lambda.Body;
        Assert.AreEqual(nameof(DateTime.AddDays), call.Method.Name);
    }

    [TestMethod]
    public void CheckedReactiveExpressionServices_MemberWithoutDeclaringType_RejectedNotCrashing()
    {
        var dynamicMethod = new System.Reflection.Emit.DynamicMethod("Answer", typeof(int), Type.EmptyTypes);
        var il = dynamicMethod.GetILGenerator();
        il.Emit(System.Reflection.Emit.OpCodes.Ldc_I4, 42);
        il.Emit(System.Reflection.Emit.OpCodes.Ret);

        Assert.IsNull(dynamicMethod.DeclaringType);
        Assert.IsFalse(new ExposedExpressionServices(typeof(IReactiveClientProxy)).IsAllowed(dynamicMethod));
    }

    [TestMethod]
    public void CheckedReactiveExpressionServices_AssemblyPrefixLookalike_NotBlessed()
    {
        var assemblyBuilder = System.Reflection.Emit.AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("ReaqtorContrib"), System.Reflection.Emit.AssemblyBuilderAccess.Run);
        var moduleBuilder = assemblyBuilder.DefineDynamicModule("ReaqtorContrib");
        var typeBuilder = moduleBuilder.DefineType("ReaqtorContrib.Lookalike", TypeAttributes.Public | TypeAttributes.Abstract | TypeAttributes.Sealed);
        var methodBuilder = typeBuilder.DefineMethod("Evil", MethodAttributes.Public | MethodAttributes.Static, typeof(int), Type.EmptyTypes);
        var il = methodBuilder.GetILGenerator();
        il.Emit(System.Reflection.Emit.OpCodes.Ldc_I4, 42);
        il.Emit(System.Reflection.Emit.OpCodes.Ret);
        var lookalike = typeBuilder.CreateType().GetMethod("Evil");

        var services = new ExposedExpressionServices(typeof(IReactiveClientProxy));
        Assert.IsFalse(services.IsAllowed(lookalike));

        // Sanity check: genuine Reaqtor assemblies remain blessed.
        Assert.IsTrue(services.IsAllowed(typeof(ReactiveExpressionServices).GetMethod(nameof(ReactiveExpressionServices.Normalize))));
    }

    [TestMethod]
    public void CheckedReactiveExpressionServices_RejectedElementInit_ArgumentsStillScanned()
    {
        Expression<Func<TestBag>> expr = () => new TestBag { Path.Combine("a", "b") };

        var services = new CtorOnlyBlessingExpressionServices(typeof(IReactiveClientProxy));
        var normalized = services.Normalize(expr);

        // The rejected Add method is forgiven, but the initializer's arguments are still scanned,
        // so the disallowed closed subexpression gets folded to a constant.
        var lambda = (LambdaExpression)normalized;
        var listInit = (ListInitExpression)lambda.Body;
        var argument = (ConstantExpression)listInit.Initializers[0].Arguments[0];
        Assert.AreEqual(Path.Combine("a", "b"), argument.Value);
    }

    [TestMethod]
    public void CheckedReactiveExpressionServices_RejectedMemberBinding_ChildrenStillScanned()
    {
        Expression<Func<TestBag>> expr = () => new TestBag { Name = Path.Combine("a", "b") };

        var services = new CtorOnlyBlessingExpressionServices(typeof(IReactiveClientProxy));
        var normalized = services.Normalize(expr);

        // The rejected bound member is forgiven, but the binding's child expression is still scanned,
        // so the disallowed closed subexpression gets folded to a constant.
        var lambda = (LambdaExpression)normalized;
        var memberInit = (MemberInitExpression)lambda.Body;
        var assignment = (MemberAssignment)memberInit.Bindings[0];
        var value = (ConstantExpression)assignment.Expression;
        Assert.AreEqual(Path.Combine("a", "b"), value.Value);
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

    private sealed class CtorOnlyBlessingExpressionServices : CheckedReactiveExpressionServices
    {
        public CtorOnlyBlessingExpressionServices(Type reactiveClientInterfaceType)
            : base(reactiveClientInterfaceType)
        {
        }

        protected override bool IsAllowedMember(MemberInfo member)
        {
            if (member.DeclaringType == typeof(TestBag))
            {
                return member is ConstructorInfo;
            }

            return base.IsAllowedMember(member);
        }
    }

    private sealed class TestBag : System.Collections.IEnumerable
    {
        public string Name { get; set; }

        public void Add(string item) => Name = item;

        public System.Collections.IEnumerator GetEnumerator() => throw new NotImplementedException();
    }

    private sealed class NoDateTimeBlessingExpressionServices : CheckedReactiveExpressionServices
    {
        public NoDateTimeBlessingExpressionServices(Type reactiveClientInterfaceType)
            : base(reactiveClientInterfaceType)
        {
        }

        protected override bool IsAllowedMember(MemberInfo member)
        {
            return member.DeclaringType != typeof(DateTime) && base.IsAllowedMember(member);
        }
    }

    private sealed class ExposedExpressionServices : CheckedReactiveExpressionServices
    {
        public ExposedExpressionServices(Type reactiveClientInterfaceType)
            : base(reactiveClientInterfaceType)
        {
        }

        public bool IsAllowed(MemberInfo member) => IsAllowedMember(member);
    }
}
