// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtive;

namespace Reaqtor.Reliable
{
    public interface IReliableSubscription : ISubscription
    {
        Uri ResubscribeUri { get; }

        // Sequence IDs are inclusive and start from 0. 
        // Start replays from the given sequence ID (included).
        void Start(long sequenceId);

        // Sequence IDs are inclusive and start from 0.
        // Idempotent - allowing for double ACK of the same sequence ID.
        void AcknowledgeRange(long sequenceId);
    }
}
