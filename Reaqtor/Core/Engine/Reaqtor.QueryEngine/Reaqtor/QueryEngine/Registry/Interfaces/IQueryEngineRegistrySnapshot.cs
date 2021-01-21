// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Interface for snapshots on a query engine registry, used during checkpointing.
    /// </summary>
    /// <remarks>
    /// Snapshots keep track of entities as well as pending deletions. After a succesful checkpoint has taken place,
    /// the snapshot that was taken at the start of the checkpoint is used to apply the changes to the in-memory
    /// registry representation (e.g. deleting entries that were removed, and whose deletion is now reliably
    /// persisted to the underlying checkpoint store).
    /// </remarks>
    internal interface IQueryEngineRegistrySnapshot
    {
        IReadOnlyDictionary<string, SubscriptionEntity> Subscriptions { get; }
        IReadOnlyDictionary<string, SubjectEntity> Subjects { get; }
        IReadOnlyDictionary<string, ReliableSubscriptionEntity> ReliableSubscriptions { get; }
        IReadOnlyDictionary<string, ObservableDefinitionEntity> Observables { get; }
        IReadOnlyDictionary<string, ObserverDefinitionEntity> Observers { get; }
        IReadOnlyDictionary<string, StreamFactoryDefinitionEntity> SubjectFactories { get; }
        IReadOnlyDictionary<string, SubscriptionFactoryDefinitionEntity> SubscriptionFactories { get; }
        IReadOnlyDictionary<string, DefinitionEntity> Other { get; }
        IReadOnlyDictionary<string, DefinitionEntity> Templates { get; }

        IEnumerable<ReactiveEntity> Entities { get; }

        IReadOnlyList<string> RemovedSubscriptions { get; }
        IReadOnlyList<string> RemovedSubjects { get; }
        IReadOnlyList<string> RemovedReliableSubscriptions { get; }
        IReadOnlyList<string> RemovedObservables { get; }
        IReadOnlyList<string> RemovedObservers { get; }
        IReadOnlyList<string> RemovedSubjectFactories { get; }
        IReadOnlyList<string> RemovedSubscriptionFactories { get; }
        IReadOnlyList<string> RemovedOther { get; }
        IReadOnlyList<string> RemovedTemplates { get; }
    }
}
