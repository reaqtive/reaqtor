// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
//

namespace Reaqtor.TestingFramework
{
    public enum ServiceOperationKind
    {
        CreateObserver,
        CreateStream,
        CreateSubscription,
        DefineObservable,
        DefineObserver,
        DefineStreamFactory,
        DeleteStream,
        DeleteSubscription,
        DeleteObservableMetadata,
        DeleteObserverMetadata,
        DeleteStreamFactoryMetadata,
        DeleteStreamMetadata,
        DeleteSubscriptionMetadata,
        InsertObservableMetadata,
        InsertObserverMetadata,
        InsertStreamFactoryMetadata,
        InsertStreamMetadata,
        InsertSubscriptionMetadata,
        LookupObservableMetadata,
        LookupObserverMetadata,
        LookupStreamFactoryMetadata,
        LookupStreamMetadata,
        LookupSubscriptionMetadata,
        MetadataQuery,
        ObserverOnNext,
        ObserverOnError,
        ObserverOnCompleted,
        UndefineObservable,
        UndefineObserver,
        UndefineStreamFactory,
        DefineSubscriptionFactory,
        UndefineSubscriptionFactory,
        InsertSubscriptionFactoryMetadata,
        DeleteSubscriptionFactoryMetadata,
        LookupSubscriptionFactoryMetadata,
    }
}
