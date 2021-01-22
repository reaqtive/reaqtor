// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.QueryEngine
{
    //
    // NB: This interface is a bit of a historical misnomer; it's not really a log, but offers a means to enter a transactional scope
    //     in which to perform mutations to tables (where implementations internally use a log of operations applied). These operations
    //     are then kept in tables as key/value pairs, where the value denotes the operation applied to the entity with the specified
    //     key (e.g. a create or a delete).
    //

    /// <summary>
    /// Interface for a transaction log, providing transactional access to key/value tables for the various reactive artifacts.
    /// </summary>
    internal interface ITransactionLog
    {
        /// <summary>
        /// Gets the table for artifact operations applied to subject factories.
        /// </summary>
        IKeyValueTable<string, ArtifactOperation> SubjectFactories { get; }

        /// <summary>
        /// Gets the table for artifact operations applied to observers.
        /// </summary>
        IKeyValueTable<string, ArtifactOperation> Observers { get; }

        /// <summary>
        /// Gets the table for artifact operations applied to observables.
        /// </summary>
        IKeyValueTable<string, ArtifactOperation> Observables { get; }

        /// <summary>
        /// Gets the table for artifact operations applied to subjects.
        /// </summary>
        IKeyValueTable<string, ArtifactOperation> Subjects { get; }

        /// <summary>
        /// Gets the table for artifact operations applied to subscriptions.
        /// </summary>
        IKeyValueTable<string, ArtifactOperation> Subscriptions { get; }

        /// <summary>
        /// Gets the table for artifact operations applied to subscription factories.
        /// </summary>
        IKeyValueTable<string, ArtifactOperation> SubscriptionFactories { get; }

        /// <summary>
        /// Enters a transaction scope.
        /// </summary>
        /// <param name="transaction">The transaction object to apply edits on (in the form of table, key, value triplets).</param>
        /// <returns>Handle to the transaction scope; enables abandoning changes by clearing the transaction.</returns>
        IScopedTransactionLog Scope(IKeyValueStoreTransaction transaction);
    }
}
