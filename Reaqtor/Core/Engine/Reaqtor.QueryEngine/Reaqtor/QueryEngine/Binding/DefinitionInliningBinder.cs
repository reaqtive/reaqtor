// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

using Reaqtive;

using Reaqtor.Reliable;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Binder that rewrites an expression by inlining definitions.
    /// </summary>
    internal class DefinitionInliningBinder : QueryEngineBinder
    {
        private readonly Func<string, Expression> _lookupForeignFunction;

        /// <summary>
        /// Creates a binder that inlines definitions which are looked up in the specified <paramref name="registry"/>.
        /// </summary>
        /// <param name="registry">Registry used to look up reactive artifact definitions.</param>
        public DefinitionInliningBinder(IQueryEngineRegistry registry)
            : this(registry, lookupForeignFunction: null)
        {
        }

        /// <summary>
        /// Creates a binder that inlines definitions which are looked up in the specified <paramref name="registry"/>.
        /// Foreign functions (that are no returning reactive types) can be looked up using <paramref name="lookupForeignFunction"/>
        /// </summary>
        /// <param name="registry">Registry used to look up reactive artifact definitions.</param>
        /// <param name="lookupForeignFunction">Function to look up foreign functions. Can return null to indicate no binding target is found..</param>
        public DefinitionInliningBinder(IQueryEngineRegistry registry, Func<string, Expression> lookupForeignFunction)
            : base(registry)
        {
            _lookupForeignFunction = lookupForeignFunction;
        }

        protected override Expression LookupOther(string id, Type type, Type funcType)
        {
            if (!TryGetOtherExpression(id, out Expression result, out bool isDefinition)
                || !isDefinition
                || IsFullyBound(result))
            {
                if (result == null && _lookupForeignFunction != null)
                {
                    return _lookupForeignFunction(id);
                }
                else
                {
                    return null;
                }
            }

            return result;
        }

        protected override Expression LookupSubscribable(string id, Type elementType)
        {
            if (!TryGetObservableExpression(id, out Expression result, out bool isDefinition)
                || !isDefinition
                || IsFullyBound(result))
            {
                return null;
            }

            return ToSubscribable(id, result, elementType);
        }

        protected override Expression LookupObserver(string id, Type elementType)
        {
            if (!TryGetObserverExpression(id, out Expression result, out bool isDefinition)
                || !isDefinition
                || IsFullyBound(result))
            {
                return null;
            }

            return ToObserver(id, result, elementType);
        }

        protected override Expression LookupMultiSubjectFactory(string id, params Type[] subjectTypes)
        {
            if (!TryGetSubjectFactoryExpression(id, out Expression result)
                || IsFullyBound(result))
            {
                return null;
            }

            return result;
        }

        protected override Expression LookupSubscriptionFactory(string id, params Type[] subscriptionTypes)
        {
            if (!TryGetSubscriptionFactoryExpression(id, out Expression result)
                || IsFullyBound(result))
            {
                return null;
            }

            return result;
        }

        protected override Expression LookupReliableObservable(string id, Type elementType)
        {
            if (!TryGetObservableExpression(id, out Expression result, out bool isDefinition)
                || !isDefinition
                || IsFullyBound(result))
            {
                return null;
            }

            return ToReliableObservable(id, result);
        }

        protected override Expression LookupReliableObserver(string id, Type elementType)
        {
            if (!TryGetObserverExpression(id, out Expression result, out bool isDefinition)
                || !isDefinition
                || IsFullyBound(result))
            {
                return null;
            }

            return ToReliableObserver(id, result);
        }

        protected override Expression LookupReliableMultiSubjectFactory(string id, Type inputType, Type outputType)
        {
            if (!TryGetSubjectFactoryExpression(id, out Expression result)
                || IsFullyBound(result))
            {
                return null;
            }

            return result;
        }

        protected override Expression LookupReliableSubscriptionFactory(string id, params Type[] subscriptionTypes)
        {
            if (!TryGetSubscriptionFactoryExpression(id, out Expression result)
                || IsFullyBound(result))
            {
                return null;
            }

            return result;
        }

        protected override Expression LookupReliableMultiSubject(string id, Type inputType, Type outputType)
        {
            throw new NotImplementedException(string.Format(CultureInfo.InvariantCulture, "Binding of parameters with generic type '{0}' is not yet implemented.", typeof(IReliableMultiSubject<,>)));
        }

        protected override Expression LookupSubscription(string id)
        {
            //
            // NB: Support to bind subscriptions will be useful when supporting Rx's disposable algebra,
            //     e.g. to create composite subscriptions and whatnot. There are various things to
            //     consider though. For example, because every subscription has an identifier, it'd be
            //     possible for a user to access a subscription that's "underneath" a composition that
            //     sits on top (i.e. "pull the rug from underneath"). However, there could be other more
            //     useful things, such as operators on ISubscription for automatic disposal (e.g. after
            //     a specified time, expressed via a query using a timer and maybe a Finally operator)
            //     or lease-based mechanisms to auto-expire queries whose lease isn't renewed.
            //

            throw new NotImplementedException(string.Format(CultureInfo.InvariantCulture, "Binding of parameters with type '{0}' is not yet implemented.", typeof(ISubscription)));
        }

        protected override Expression LookupReliableSubscription(string id)
        {
            throw new NotImplementedException(string.Format(CultureInfo.InvariantCulture, "Binding of parameters with type '{0}' is not yet implemented.", typeof(IReliableSubscription)));
        }

        private static bool IsFullyBound(Expression expr) => !FreeVariableScanner.HasFreeVariables(expr);
    }
}
