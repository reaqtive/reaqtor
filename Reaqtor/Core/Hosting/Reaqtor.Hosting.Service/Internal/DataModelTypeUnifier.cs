// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.Reflection;

using Nuqleon.DataModel;

using Reaqtor.Hosting.Shared.Tools;

namespace Reaqtor.Hosting.Shared.Serialization
{
    /// <summary>
    /// A type unifier for slim expressions that is data model aware.
    /// </summary>
    /// TODO: Remove this class once DataModel CompilerService updates are pushed.
    internal sealed class DataModelTypeUnifier : TypeUnifier
    {
        /// <summary>
        /// Instantiates DataModelTypeUnifier
        /// </summary>
        public DataModelTypeUnifier()
            : base(safe: true)
        {
        }

        /// <summary>
        /// Extends unification for simple types to support unification of integers to enums.
        /// </summary>
        /// <param name="typeRich">The CLR type.</param>
        /// <param name="typeSlim">The slim type.</param>
        /// <returns>true if the unification was successful, false otherwise.</returns>
        protected override bool UnifySimple(Type typeRich, SimpleTypeSlim typeSlim)
        {
            var canUnify = base.UnifySimple(typeRich, typeSlim);

            if (!canUnify && typeRich.IsEnum)
            {
                var isUnderlying = base.UnifySimple(Enum.GetUnderlyingType(typeRich), typeSlim);
                return isUnderlying;
            }

            return canUnify;
        }

        /// <summary>
        /// Unifies generic types with awareness of the conversion from asynchronous to synchronous Reactive entity types.
        /// </summary>
        /// <param name="typeRich">The CLR type.</param>
        /// <param name="typeSlim">The slim type.</param>
        /// <returns>true if the unification was successful, false otherwise.</returns>
        protected override bool UnifyGeneric(Type typeRich, GenericTypeSlim typeSlim)
        {
            var reactiveType = ReactiveEntityTypeExtensions.FromTypeSlim(typeSlim);
            if ((reactiveType & ReactiveEntityType.Func) == ReactiveEntityType.Func)
            {
                reactiveType = ReactiveEntityType.Func;
            }

            return reactiveType switch
            {
                ReactiveEntityType.Func => Unify(typeRich.GetGenericArguments(), typeSlim),

                ReactiveEntityType.Observer or
                ReactiveEntityType.Observable or
                ReactiveEntityType.StreamFactory or
                ReactiveEntityType.SubscriptionFactory or
                ReactiveEntityType.Stream => Unify(GetEntityTypes(typeRich, reactiveType), typeSlim),

                _ => base.UnifyGeneric(typeRich, typeSlim),
            };
        }

        /// <summary>
        /// Resolves properties with mapping attributes.
        /// </summary>
        /// <param name="declaringTypeRich">The type declaring the property.</param>
        /// <param name="propertySlim">The property to resolve.</param>
        /// <returns>The resolved property from the declaring type.</returns>
        protected override PropertyInfo ResolveProperty(Type declaringTypeRich, PropertyInfoSlim propertySlim)
        {
            if (propertySlim.DeclaringType.Kind == TypeSlimKind.Structural)
            {
                var propertyMatch = default(PropertyInfo);

                foreach (var prop in declaringTypeRich.GetProperties())
                {
                    var found = false;

                    var attribute = prop.GetCustomAttribute<MappingAttribute>(inherit: false);
                    if (attribute == null)
                    {
                        found = prop.Name == propertySlim.Name;
                    }
                    else
                    {
                        found = attribute.Uri == propertySlim.Name || prop.Name == propertySlim.Name;
                    }

                    if (found)
                    {
                        if (propertyMatch != null)
                        {
                            throw new InvalidOperationException(
                                string.Format(
                                CultureInfo.InvariantCulture,
                                "More than one property with name or mapping attribute '{0}' was found on type '{1}'.",
                                propertySlim.Name,
                                declaringTypeRich));
                        }

                        propertyMatch = prop;
                    }
                }

                if (propertyMatch == default)
                {
                    throw new InvalidOperationException(
                        string.Format(
                            CultureInfo.InvariantCulture,
                            "No property with name or mapping attribute '{0}' was found on type '{1}'.",
                            propertySlim.Name,
                            declaringTypeRich));
                }

                return propertyMatch;
            }

            return base.ResolveProperty(declaringTypeRich, propertySlim);
        }

        /// <summary>
        /// Gets the generic argument of the reactive type.
        /// </summary>
        /// <param name="typeRich">The closed generic type.</param>
        /// <param name="reactiveType">The reactive type.</param>
        /// <returns>The generic argument.</returns>
        private static Type[] GetEntityTypes(Type typeRich, ReactiveEntityType reactiveType)
        {
            switch (reactiveType)
            {
                case ReactiveEntityType.Observer:
                    var observerType = typeRich.FindGenericType(typeof(IObserver<>));
                    return observerType?.GetGenericArguments() ?? Type.EmptyTypes;
                case ReactiveEntityType.Observable:
                    var observableType = typeRich.FindGenericType(typeof(IReactiveQbservable<>));
                    return observableType?.GetGenericArguments() ?? Type.EmptyTypes;
                case ReactiveEntityType.StreamFactory:
                    var streamFactoryType = typeRich.FindGenericType(typeof(IReactiveQubjectFactory<,>));
                    return streamFactoryType?.GetGenericArguments() ?? Type.EmptyTypes;
                case ReactiveEntityType.Stream:
                    var streamType = typeRich.FindGenericType(typeof(IReactiveQubject<>));
                    streamType ??= typeRich.FindGenericType(typeof(IReactiveQubject<,>));
                    return streamType?.GetGenericArguments() ?? Type.EmptyTypes;
                case ReactiveEntityType.SubscriptionFactory:
                    var subscriptionFactoryType = typeRich.FindGenericType(typeof(IReactiveQubscriptionFactory<>));
                    return subscriptionFactoryType?.GetGenericArguments() ?? Type.EmptyTypes;
                case ReactiveEntityType.Subscription:
                case ReactiveEntityType.Func:
                case ReactiveEntityType.None:
                default:
                    return Type.EmptyTypes;
            }
        }

        /// <summary>
        /// Unifies the specified <paramref name="types"/> with the generic types arguments of <paramref name="genericType"/>.
        /// </summary>
        /// <param name="types">The array of types to unify.</param>
        /// <param name="genericType">The slim generic type whose generic arguments to unify.</param>
        /// <returns><c>true</c> if unification succeeded; otherwise, <c>false</c>.</returns>
        private bool Unify(Type[] types, GenericTypeSlim genericType)
        {
            var n = genericType.GenericArgumentCount;

            if (types.Length != n)
            {
                return false;
            }

            for (var i = 0; i < n; i++)
            {
                var typeRich = types[i];
                var typeSlim = genericType.GetGenericArgument(i);

                if (!Unify(typeRich, typeSlim))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
