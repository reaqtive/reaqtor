// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Interface for transaction logs for create/delete operations that can be replied.
    /// </summary>
    /// <remarks>
    /// The transaction log is used to perform write-ahead logging of create and delete operations for
    /// reactive entities. After a successful checkpoint, this log is truncated. Upon recovery, the current
    /// transaction log entries are replayed.
    /// </remarks>
    internal interface IReplayTransactionLog
    {
        IDictionary<string, ArtifactOperation> SubjectFactories { get; }

        IDictionary<string, ArtifactOperation> SubscriptionFactories { get; }

        IDictionary<string, ArtifactOperation> Observers { get; }

        IDictionary<string, ArtifactOperation> Observables { get; }

        IDictionary<string, ArtifactOperation> Subjects { get; }

        IDictionary<string, ArtifactOperation> Subscriptions { get; }

        IDictionary<string, List<ArtifactOperation>> InvalidReplaySequence { get; }
    }
}
