// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Interface for query engine registries containing reactive entities.
    /// </summary>
    internal interface IQueryEngineRegistry : IDisposable
    {
        IReactiveEntityCollection<string, ObservableDefinitionEntity> Observables { get; }
        IReactiveEntityCollection<string, ObserverDefinitionEntity> Observers { get; }
        IReactiveEntityCollection<string, StreamFactoryDefinitionEntity> SubjectFactories { get; }
        IReactiveEntityCollection<string, SubscriptionFactoryDefinitionEntity> SubscriptionFactories { get; }

        IReactiveEntityCollection<string, DefinitionEntity> Other { get; }
        IInvertedLookupReactiveEntityCollection<string, DefinitionEntity> Templates { get; }

        IReactiveEntityCollection<string, SubscriptionEntity> Subscriptions { get; }
        IReactiveEntityCollection<string, SubjectEntity> Subjects { get; }

        IReactiveEntityCollection<string, ReliableSubscriptionEntity> ReliableSubscriptions { get; }

        void Clear();
    }
}
