// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

using Newtonsoft.Json.Linq;

using Nuqleon.DataModel;

namespace Reaqtor.Remoting.Client
{
    /// <summary>
    /// Expression tree rewriting services used by the client-side library.
    /// </summary>
    public class ExpressionServices : ReactiveExpressionServices
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
        /// We still need to write all the time-based Rx operators in the I
        /// Subscribable space to run inside the QE. As we speak, the scheduler
        /// got checked in, using DateTimeOffset across the board (just like
        /// Rx’s), so we can start on those very soon. But we still need to
        /// think about the user API surface.
        /// </para><para>
        /// For now, you can put all of DateTimeOffset and DateTime (except for
        /// Now and UtcNow) on the allow list. This will enable queries like XYZ’s, which
        /// performs DateTimeOffset arithmetic. You can use a LINQ query to get
        /// all the members of both classes and exclude those properties, before
        /// passing the sequence to the allow list scanner. The reason for
        /// excluding [Utc]Now is twofold:
        ///     Ties to the system clock of the QE, as discussed above.
        ///     Not pure functions, so defeating all sorts of optimizations
        /// (e.g. we can’t hoist the sub-expression out of a SelectMany
        /// collection selector, because timing would change).
        /// </para><para>
        /// You can also add all of TimeSpan (no exclusions needed).
        /// </para></remarks>
        private static readonly IList<MemberInfo> MemberAllowList = new Func<IList<MemberInfo>>(
            () =>
            {
                var members = from type in new[] { typeof(DateTime), typeof(DateTimeOffset) }
                              from member in type.GetMembers()
                              select member;

                members = members.Except(new[]
                {
                    ReflectionHelpers.InfoOf(() => DateTime.Now),
                    ReflectionHelpers.InfoOf(() => DateTime.UtcNow),
                    ReflectionHelpers.InfoOf(() => DateTimeOffset.Now),
                    ReflectionHelpers.InfoOf(() => DateTimeOffset.UtcNow),
                });

                return members.ToArray();
            })();

        /// <summary>
        /// Allow list scanner to prevent remote execution of client-side only constructs.
        /// </summary>
        private readonly AllowListScanner _allowlistScanner;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionServices"/> class.
        /// </summary>
        public ExpressionServices(Type reactiveClientInterfaceType)
            : base(reactiveClientInterfaceType)
        {
            // TODO - Allow list should also be added to the service, to protect the service against malformed expressions.
            _allowlistScanner = GetAllowListScanner();
        }

        /// <summary>
        /// Normalizes the specified expression prior to submission to the service.
        /// </summary>
        /// <param name="expression">The expression to normalize.</param>
        /// <returns>The normalized expression.</returns>
        public override Expression Normalize(Expression expression)
        {
            // TODO: [IRP Core] Ordering of the rewrites (base/here; exposure of virtuals).

            // Runs a standard set of rewrites, causing callbacks to the Funcletize method below.
            var normalized = base.Normalize(expression);

            // Replaces compiler-generated anonymous types, e.g. for transparent identifiers, by tuples which don't need reconstruction in the service.
            var tupletized = AnonymousTypeTupletizer.Tupletize(normalized, Expression.Constant(null), excludeVisibleTypes: true);

            return tupletized;
        }

        /// <summary>
        /// Funcletizes the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>the funcletized expression</returns>
        protected override Expression Funcletize(Expression expression)
        {
            var res = base.Funcletize(expression);
            res = _allowlistScanner.Visit(res);
            return res;
        }

        /// <summary>
        /// Gets an expression representing the invocation of a known resource encountered during expression tree analysis.
        /// </summary>
        /// <param name="knownResourceUri">The logical URI of the known resource being invoked.</param>
        /// <param name="member">The member discovered in the expression tree, representing the known resource. This member is annotated with KnownResourceAttribute.</param>
        /// <param name="instance">Instance on which the known resource is invoked. This can occur for instance methods on reactive interfaces.</param>
        /// <param name="arguments">The arguments passed to the known resource invocation.</param>
        /// <param name="returnType">Return type of the known resource invocation.</param>
        /// <returns>Expression representing the invocation of the specified known resource.</returns>
        protected override Expression GetKnownResourceInvocation(Uri knownResourceUri, MemberInfo member, Expression instance, IEnumerable<Expression> arguments, Type returnType)
        {
            // TODO: [IRP Core] Basic implementation to be provided, using [Mapping] annotations on method parameters to recordize calls. By default, calls are tupletized.
            return base.GetKnownResourceInvocation(knownResourceUri, member, instance, arguments, returnType);
        }

        /// <summary>
        /// Gets the allow list scanner used to detect client-side only constructs that should not be run on the service.
        /// </summary>
        /// <returns>Allow list scanner used to detect client-side only constructs that should not be run on the service.</returns>
        private static AllowListScanner GetAllowListScanner()
        {
            // The following is a declarative table of allowed constructs whose expression tree representation can be
            // sent to the service for remote execution. This also allows to detect user code, such as reads from local
            // fields, which end up in the expression tree representation of a query or a define operation. Use this
            // table with EXTREME caution! In particular, only allowed BCL functionality should be added here. Under no
            // circumstance whatsoever should this table contain reactive interfaces, query operators, and whatnot. If
            // such a need is suspected, something else is wrong higher up the expression rewrite stack. While the use
            // of the allow list scanner will throw exceptions for invalid constructs encountered, the fix is (almost)
            // never to extend the table.
            var allowListScanner = new AllowListScanner()
            {
                // Allowing use of all members on the following types:
                DeclaringTypes =
                {
                    typeof(Math),
                    typeof(TimeSpan),
                    typeof(JObject),
                    typeof(JProperty),
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

                    // Enable new JObject(new JProperty(...), new JProperty(...)).ToString()
                    typeof(object).GetMethod("ToString", Type.EmptyTypes),
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
        /// Implementation of a allow list scanner used to detect client-side only constructs that should not be run on the service, with default policies to deal with unsupported constructs.
        /// </summary>
        private class AllowListScanner : ExpressionMemberAllowListScanner
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="AllowListScanner"/> class.
            /// </summary>
            public AllowListScanner()
            {
            }

            /// <summary>
            /// Checks whether the specified member is allowed by the allow list.
            /// </summary>
            /// <param name="member">Member to check against the allow list.</param>
            /// <returns><c>true</c> if usage of the member is allowed by the allow list; otherwise, <c>false</c>.</returns>
            protected override bool Check(MemberInfo member)
            {
                var d = member.DeclaringType;

                // Annotation used for type that are deployed to the service and are well-known. This should be used sparingly, e.g. for custom feed adapters etc.
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

                // TODO: To be refined using a list of assemblies once all layering and factoring of client-side components is complete.
                if (d.Assembly.FullName.StartsWith("Reaqtive", StringComparison.Ordinal) ||
                    d.Assembly.FullName.StartsWith("Reaqtor", StringComparison.Ordinal))
                {
                    return true;
                }

                // Checks against the declarative allow list (see GetAllowListScanner to extend this list).
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

                // TODO: [IRP Core] Use funclet, but only if we can retain it throughout the expression tree rewrite stages, such that we can
                //                  cache the compiled funclet across different uses (e.g. by creating a parameterized query plan a la SQL).
                return Expression.Constant(value, expression.Type);
            }
        }
    }
}
