// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;

namespace Reaqtor.Hosting.Shared.Tools
{
    /// <summary>
    /// Contains helper methods regarding the Reactive entity type enumeration.
    /// </summary>
    public static class ReactiveEntityTypeExtensions
    {
#pragma warning disable format // Formatted as tables.

        private static readonly Type ObserverType             = typeof(IAsyncReactiveQbserver<>);
        private static readonly Type ObservableType           = typeof(IAsyncReactiveQbservable<>);
        private static readonly Type SubscriptionType         = typeof(IAsyncReactiveQubscription);
        private static readonly Type SubjectType1             = typeof(IAsyncReactiveQubject<>);
        private static readonly Type SubjectType2             = typeof(IAsyncReactiveQubject<,>);
        private static readonly Type SubjectFactoryType       = typeof(IAsyncReactiveQubjectFactory<,>);
        private static readonly Type SubscriptionFactoryType1 = typeof(IAsyncReactiveQubscriptionFactory);
        private static readonly Type SubscriptionFactoryType2 = typeof(IAsyncReactiveQubscriptionFactory<>);
        private static readonly Type FuncType                 = typeof(Func<,>);

        private static readonly TypeSlim SlimObserverType             = ObserverType.ToTypeSlim();
        private static readonly TypeSlim SlimObservableType           = ObservableType.ToTypeSlim();
        private static readonly TypeSlim SlimSubscriptionType         = SubscriptionType.ToTypeSlim();
        private static readonly TypeSlim SlimSubjectType1             = SubjectType1.ToTypeSlim();
        private static readonly TypeSlim SlimSubjectType2             = SubjectType2.ToTypeSlim();
        private static readonly TypeSlim SlimSubjectFactoryType       = SubjectFactoryType.ToTypeSlim();
        private static readonly TypeSlim SlimSubscriptionFactoryType1 = SubscriptionFactoryType1.ToTypeSlim();
        private static readonly TypeSlim SlimSubscriptionFactoryType2 = SubscriptionFactoryType2.ToTypeSlim();
        private static readonly TypeSlim SlimFuncType                 = FuncType.ToTypeSlim();

#pragma warning restore format

        /// <summary>
        /// Gets the reactive type from a type name.
        /// </summary>
        /// <param name="typeSlim">The type.</param>
        /// <returns>The reactive entity type.</returns>
        public static ReactiveEntityType FromTypeSlim(TypeSlim typeSlim)
        {
            if (typeSlim == null)
                throw new ArgumentNullException(nameof(typeSlim));

            if (typeSlim is GenericTypeSlim genericTypeSlim)
            {
                var definition = genericTypeSlim.GenericTypeDefinition;

                if (SlimObservableType.Equals(definition))
                {
                    return ReactiveEntityType.Observable;
                }
                else if (SlimObserverType.Equals(definition))
                {
                    return ReactiveEntityType.Observer;
                }
                else if (SlimSubjectFactoryType.Equals(definition))
                {
                    return ReactiveEntityType.StreamFactory;
                }
                else if (SlimSubjectType1.Equals(definition) || SlimSubjectType2.Equals(definition))
                {
                    return ReactiveEntityType.Stream;
                }
                else if (SlimSubscriptionFactoryType2.Equals(definition))
                {
                    return ReactiveEntityType.SubscriptionFactory;
                }
                else if (SlimFuncType.Equals(definition))
                {
                    var argCount = genericTypeSlim.GenericArgumentCount;
                    var lastArg = genericTypeSlim.GetGenericArgument(argCount - 1);

                    var parameterizedType = FromTypeSlim(lastArg);
                    if (parameterizedType != ReactiveEntityType.None)
                    {
                        return ReactiveEntityType.Func | parameterizedType;
                    }
                }
            }
            else if (SlimSubscriptionType.Equals(typeSlim))
            {
                return ReactiveEntityType.Subscription;
            }
            else if (SlimSubscriptionFactoryType1.Equals(typeSlim))
            {
                return ReactiveEntityType.SubscriptionFactory;
            }

            return ReactiveEntityType.None;
        }

        /// <summary>
        /// Gets the reactive type from a type name.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The reactive entity type.</returns>
        public static ReactiveEntityType FromType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (type.IsGenericType)
            {
                var definition = type.GetGenericTypeDefinition();

                if (ObservableType.Equals(definition))
                {
                    return ReactiveEntityType.Observable;
                }
                else if (ObserverType.Equals(definition))
                {
                    return ReactiveEntityType.Observer;
                }
                else if (SubjectFactoryType.Equals(definition))
                {
                    return ReactiveEntityType.StreamFactory;
                }
                else if (SubjectType1.Equals(definition) || SubjectType2.Equals(definition))
                {
                    return ReactiveEntityType.Stream;
                }
                else if (SubscriptionFactoryType2.Equals(definition))
                {
                    return ReactiveEntityType.SubscriptionFactory;
                }
                else if (FuncType.Equals(definition) && !type.IsGenericTypeDefinition)
                {
                    var args = type.GenericTypeArguments;

#if NET6_0 || NETSTANDARD2_1
                    var lastArg = args[^1];
#else
                    var lastArg = args[args.Length - 1];
#endif

                    var parameterizedType = FromType(lastArg);
                    if (parameterizedType != ReactiveEntityType.None)
                    {
                        return ReactiveEntityType.Func | parameterizedType;
                    }
                }
            }
            else if (SubscriptionType.Equals(type))
            {
                return ReactiveEntityType.Subscription;
            }
            else if (SubscriptionFactoryType1.Equals(type))
            {
                return ReactiveEntityType.SubscriptionFactory;
            }

            return ReactiveEntityType.None;
        }
    }
}
