// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this code in the Remoting sample's client library.
// HvR - July 2026 - Factored out of the archived Remoting sample (see #158).
//

using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

using Nuqleon.DataModel;

namespace Reaqtor;

/// <summary>
/// Expression tree rewriting services for reactive service clients that check expressions against
/// an allow list of members prior to submission to a service, evaluating rejected subexpressions
/// locally and inlining their results as constants.
/// </summary>
public class CheckedReactiveExpressionServices : ReactiveExpressionServices
{
    /// <summary>
    /// The member allow list.
    /// </summary>
    /// <remarks><para>
    /// Especially uses of “Now” are tricky, because it’s going to use the
    /// server’s clock, reveal its offset from UTC, and whatnot. Rx as we
    /// have it today is making the assumption that absolute time is
    /// observed as the machine sees it, which includes taking DST into
    /// account, by observing system clock change events from the system and
    /// monitoring for clock drift. We never have situations where a system
    /// executes a query on behalf of a remote user who may expect his or
    /// her time zone to be taken into account.
    /// </para><para>
    /// All of DateTimeOffset and DateTime (except for Now and UtcNow) are on
    /// the allow list. This enables queries which perform DateTimeOffset
    /// arithmetic. The reason for excluding [Utc]Now is twofold:
    ///     Ties to the system clock of the query evaluator, as discussed above.
    ///     Not pure functions, so defeating all sorts of optimizations
    /// (e.g. we can’t hoist the sub-expression out of a SelectMany
    /// collection selector, because timing would change).
    /// </para><para>
    /// All of TimeSpan is included as well (no exclusions needed).
    /// </para><para>
    /// Note that DateTime and DateTimeOffset are valid data model types, so <see cref="IsAllowedMember"/>
    /// blesses all of their members before this table is consulted; the [Utc]Now exclusions expressed here
    /// only take effect if that blessing is narrowed. This matches the behavior of the archived Remoting
    /// client library this code was factored out of.
    /// </para></remarks>
    private static readonly IList<MemberInfo> MemberAllowList = new Func<IList<MemberInfo>>(
        () =>
        {
            var members = from type in new[] { typeof(DateTime), typeof(DateTimeOffset) }
                          from member in type.GetMembers()
                          select member;

            members = members.Except(
            [
                ReflectionHelpers.InfoOf(() => DateTime.Now),
                ReflectionHelpers.InfoOf(() => DateTime.UtcNow),
                ReflectionHelpers.InfoOf(() => DateTimeOffset.Now),
                ReflectionHelpers.InfoOf(() => DateTimeOffset.UtcNow),
            ]);

            return [.. members];
        })();

    /// <summary>
    /// Allow list scanner to prevent remote execution of client-side only constructs.
    /// </summary>
    private readonly AllowListScanner _allowListScanner;

    /// <summary>
    /// Creates a new checked expression service provider instance.
    /// </summary>
    /// <param name="reactiveClientInterfaceType">Interface type of the IReactiveClient variant used to obtain reactive resources.</param>
    public CheckedReactiveExpressionServices(Type reactiveClientInterfaceType)
        : base(reactiveClientInterfaceType)
    {
        _allowListScanner = CreateAllowListScanner();
    }

    /// <summary>
    /// Normalizes the specified expression prior to submission to the service.
    /// </summary>
    /// <param name="expression">The expression to normalize.</param>
    /// <returns>The normalized expression.</returns>
    public override Expression Normalize(Expression expression)
    {
        // Runs a standard set of rewrites, causing callbacks to the Funcletize method below.
        var normalized = base.Normalize(expression);

        // Replaces compiler-generated anonymous types, e.g. for transparent identifiers, by tuples which don't need reconstruction in the service.
        var tupletized = AnonymousTypeTupletizer.Tupletize(normalized, Expression.Constant(null), excludeVisibleTypes: true);

        return tupletized;
    }

    /// <summary>
    /// Funcletizes the specified expression.
    /// </summary>
    /// <param name="expression">The expression to funcletize.</param>
    /// <returns>The funcletized expression.</returns>
    protected override Expression Funcletize(Expression expression)
    {
        var res = base.Funcletize(expression);
        res = _allowListScanner.Visit(res);
        return res;
    }

    /// <summary>
    /// Checks whether the specified member is allowed to occur in expressions submitted to the service,
    /// prior to consulting the declarative allow list table. Derived classes can override this method to
    /// bless additional members, e.g. types provided by a serialization library available on the service.
    /// </summary>
    /// <param name="member">Member to check against the allow list.</param>
    /// <returns><c>true</c> if usage of the member is allowed; otherwise, <c>false</c>.</returns>
    /// <remarks>
    /// The default implementation allows members declared on types annotated with
    /// <see cref="KnownTypeAttribute"/>, members declared on valid data model types, members annotated
    /// with <see cref="KnownResourceAttribute"/>, and members declared in Reaqtive or Reaqtor assemblies.
    /// The assembly name based check is to be refined using a curated list of assemblies once all layering
    /// and factoring of client-side components is complete.
    /// </remarks>
    protected virtual bool IsAllowedMember(MemberInfo member)
    {
        ArgumentNullException.ThrowIfNull(member);

        var d = member.DeclaringType;

        // Annotation used for types that are deployed to the service and are well-known. This should be used sparingly, e.g. for custom feed adapters etc.
        if (d.IsDefined(typeof(KnownTypeAttribute)))
        {
            return true;
        }

        // Main check to allow any of the members on any of the data model types with no further questions asked. This includes things like int, string, DateTime, etc.
        if (DataModelHelpers.IsDataModelType(d))
        {
            return true;
        }

        // Members annotated with the KnownResourceAttribute will get rewritten to invocation expressions and are allowed. It is assumed the service will know how to bind those.
        if (member.IsDefined(typeof(KnownResourceAttribute)))
        {
            return true;
        }

        if (d.Assembly.FullName.StartsWith("Reaqtive", StringComparison.Ordinal) ||
            d.Assembly.FullName.StartsWith("Reaqtor", StringComparison.Ordinal))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Gets the allow list scanner used to detect client-side only constructs that should not be run on the service.
    /// </summary>
    /// <returns>Allow list scanner used to detect client-side only constructs that should not be run on the service.</returns>
    private AllowListScanner CreateAllowListScanner()
    {
        // The following is a declarative table of allowed constructs whose expression tree representation can be
        // sent to the service for remote execution. This also allows to detect user code, such as reads from local
        // fields, which end up in the expression tree representation of a query or a define operation. Use this
        // table with EXTREME caution! In particular, only allowed BCL functionality should be added here. Under no
        // circumstance whatsoever should this table contain reactive interfaces, query operators, and whatnot. If
        // such a need is suspected, something else is wrong higher up the expression rewrite stack. While the use
        // of the allow list scanner will throw exceptions for invalid constructs encountered, the fix is (almost)
        // never to extend the table.
        var allowListScanner = new AllowListScanner(this)
        {
            // Allowing use of all members on the following types:
            DeclaringTypes =
            {
                typeof(Math),
                typeof(TimeSpan),
                typeof(System.Text.Json.Nodes.JsonNode),
                typeof(System.Text.Json.Nodes.JsonObject),
                typeof(System.Text.Json.Nodes.JsonArray),
                typeof(System.Text.Json.Nodes.JsonValue),
                typeof(Enumerable),
                typeof(Queryable),
                typeof(KeyValuePair<,>),
                typeof(Tuple<>),
                typeof(Tuple<,>),
                typeof(Tuple<,,>),
                typeof(Tuple<,,,>),
                typeof(Tuple<,,,,>),
                typeof(Tuple<,,,,,>),
                typeof(Tuple<,,,,,,>),
                typeof(Tuple<,,,,,,,>),
            },

            // Allowing use of the following members:
            Members =
            {
                ReflectionHelpers.InfoOf(() => Environment.MachineName), // used for diagnostics

                // Enable e.g. new JsonObject { ... }.ToString()
                typeof(object).GetMethod(nameof(ToString), Type.EmptyTypes),
            },
        };

        // Allowing use of the following specific members:
        foreach (var memberInfo in MemberAllowList)
        {
            allowListScanner.Members.Add(memberInfo);
        }

        return allowListScanner;
    }

    /// <summary>
    /// Implementation of an allow list scanner used to detect client-side only constructs that should not be run on the service, with default policies to deal with unsupported constructs.
    /// </summary>
    private sealed class AllowListScanner : ExpressionMemberAllowListScanner
    {
        /// <summary>
        /// The expression services whose <see cref="IsAllowedMember"/> policy is consulted during scanning.
        /// </summary>
        private readonly CheckedReactiveExpressionServices _parent;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllowListScanner"/> class.
        /// </summary>
        /// <param name="parent">The expression services whose <see cref="IsAllowedMember"/> policy is consulted during scanning.</param>
        public AllowListScanner(CheckedReactiveExpressionServices parent) => _parent = parent;

        /// <summary>
        /// Checks whether the specified member is allowed by the allow list.
        /// </summary>
        /// <param name="member">Member to check against the allow list.</param>
        /// <returns><c>true</c> if usage of the member is allowed by the allow list; otherwise, <c>false</c>.</returns>
        protected override bool Check(MemberInfo member)
        {
            if (_parent.IsAllowedMember(member))
            {
                return true;
            }

            // Checks against the declarative allow list (see CreateAllowListScanner to extend this list).
            return base.Check(member);
        }

        /// <summary>
        /// Resolves an element initializer expression which was rejected by the allow list scanner.
        /// </summary>
        /// <param name="initializer">The initializer expression to resolve.</param>
        /// <param name="visit">Function to visit the element initializer.</param>
        /// <returns>Original element initializer (see remarks).</returns>
        /// <remarks>We allow element initializers with no questions asked, under the assumption that the containing ListInitExpression or ListMemberBinding node was allowed based on its collection type.</remarks>
        protected override ElementInit ResolveElementInit(ElementInit initializer, Func<ElementInit, ElementInit> visit)
        {
            return initializer;
        }

        /// <summary>
        /// Resolves a member binding expression which was rejected by the allow list scanner.
        /// </summary>
        /// <typeparam name="T">The concrete subtype of the binding expression.</typeparam>
        /// <param name="binding">The binding expression to resolve.</param>
        /// <param name="visit">Function to visit the member binding.</param>
        /// <returns>Original member binding (see remarks).</returns>
        /// <remarks>We allow member bindings with no questions asked, under the assumption that the containing MemberInitExpression or MemberBinding node was allowed based on its constructor's declaring type.</remarks>
        protected override MemberBinding ResolveMemberBinding<T>(T binding, Func<T, MemberBinding> visit)
        {
            return binding;
        }

        /// <summary>
        /// Resolves an expression which was rejected by the allow list scanner.
        /// </summary>
        /// <typeparam name="T">The concrete subtype of the expression.</typeparam>
        /// <param name="expression">The expression to resolve.</param>
        /// <param name="member">The member used by the expression, which caused it to be rejected.</param>
        /// <param name="visit">Function to visit the expression.</param>
        /// <returns>Expression holding the result of local evaluation of the expression.</returns>
        protected override Expression ResolveExpression<T>(T expression, MemberInfo member, Func<T, Expression> visit)
        {
            // Note that this method will throw if the expression contains unbound parameters that are only available on the service. This is
            // by design and almost never indicates that something is wrong with the allow list scanner. Instead, another rewrite higher up the
            // stack is likely going haywire.
            var value = expression.Evaluate();

            return Expression.Constant(value, expression.Type);
        }
    }
}
