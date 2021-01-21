// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Validate arguments of public methods. (Omitting null checks for protected methods.)

using System.Collections.Generic;
using System.Linq.Expressions;

namespace Reaqtor.TestingFramework
{
    public abstract class ServiceOperationEqualityComparer : IEqualityComparer<ServiceOperation>
    {
        public virtual bool Equals(ServiceOperation x, ServiceOperation y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            if (!(x.Kind == y.Kind && object.Equals(x.State, y.State) && x.TargetObjectUri == y.TargetObjectUri))
            {
                return false;
            }

            return x.Kind switch
            {
                ServiceOperationKind.CreateObserver or ServiceOperationKind.CreateStream or ServiceOperationKind.CreateSubscription => EqualsCreate((CreateServiceOperation)x, (CreateServiceOperation)y),
                ServiceOperationKind.DefineObservable or ServiceOperationKind.DefineObserver or ServiceOperationKind.DefineStreamFactory or ServiceOperationKind.DefineSubscriptionFactory => EqualsDefine((DefineServiceOperation)x, (DefineServiceOperation)y),
                ServiceOperationKind.DeleteStream or ServiceOperationKind.DeleteSubscription => EqualsDelete((DeleteServiceOperation)x, (DeleteServiceOperation)y),
                ServiceOperationKind.DeleteObservableMetadata or ServiceOperationKind.DeleteObserverMetadata or ServiceOperationKind.DeleteStreamFactoryMetadata or ServiceOperationKind.DeleteSubscriptionFactoryMetadata or ServiceOperationKind.DeleteStreamMetadata or ServiceOperationKind.DeleteSubscriptionMetadata or ServiceOperationKind.InsertObservableMetadata or ServiceOperationKind.InsertObserverMetadata or ServiceOperationKind.InsertStreamFactoryMetadata or ServiceOperationKind.InsertSubscriptionFactoryMetadata or ServiceOperationKind.InsertStreamMetadata or ServiceOperationKind.InsertSubscriptionMetadata or ServiceOperationKind.LookupObservableMetadata or ServiceOperationKind.LookupObserverMetadata or ServiceOperationKind.LookupStreamFactoryMetadata or ServiceOperationKind.LookupSubscriptionFactoryMetadata or ServiceOperationKind.LookupStreamMetadata or ServiceOperationKind.LookupSubscriptionMetadata or ServiceOperationKind.MetadataQuery => EqualsMetadata((MetadataOperation)x, (MetadataOperation)y),
                ServiceOperationKind.ObserverOnNext or ServiceOperationKind.ObserverOnError or ServiceOperationKind.ObserverOnCompleted => EqualsObserver((ObserverOperation)x, (ObserverOperation)y),
                ServiceOperationKind.UndefineObservable or ServiceOperationKind.UndefineObserver or ServiceOperationKind.UndefineStreamFactory or ServiceOperationKind.UndefineSubscriptionFactory => EqualsUndefine((UndefineServiceOperation)x, (UndefineServiceOperation)y),
                _ => false,
            };
        }

        protected virtual bool EqualsCreate(CreateServiceOperation x, CreateServiceOperation y) => x.Kind switch
        {
            ServiceOperationKind.CreateObserver => EqualsCreateObserver((CreateObserver)x, (CreateObserver)y),
            ServiceOperationKind.CreateStream => EqualsCreateStream((CreateStream)x, (CreateStream)y),
            ServiceOperationKind.CreateSubscription => EqualsCreateSubscription((CreateSubscription)x, (CreateSubscription)y),
            _ => false,
        };

        protected virtual bool EqualsCreateObserver(CreateObserver x, CreateObserver y) => Equals(x.Expression, y.Expression);

        protected virtual bool EqualsCreateStream(CreateStream x, CreateStream y) => Equals(x.Expression, y.Expression);

        protected virtual bool EqualsCreateSubscription(CreateSubscription x, CreateSubscription y) => Equals(x.Expression, y.Expression);

        protected virtual bool EqualsDefine(DefineServiceOperation x, DefineServiceOperation y) => x.Kind switch
        {
            ServiceOperationKind.DefineObserver => EqualsDefineObserver((DefineObserver)x, (DefineObserver)y),
            ServiceOperationKind.DefineObservable => EqualsDefineObservable((DefineObservable)x, (DefineObservable)y),
            ServiceOperationKind.DefineStreamFactory => EqualsDefineStreamFactory((DefineStreamFactory)x, (DefineStreamFactory)y),
            ServiceOperationKind.DefineSubscriptionFactory => EqualsDefineSubscriptionFactory((DefineSubscriptionFactory)x, (DefineSubscriptionFactory)y),
            _ => false,
        };

        protected virtual bool EqualsDefineObserver(DefineObserver x, DefineObserver y) => Equals(x.Expression, y.Expression);

        protected virtual bool EqualsDefineObservable(DefineObservable x, DefineObservable y) => Equals(x.Expression, y.Expression);

        protected virtual bool EqualsDefineStreamFactory(DefineStreamFactory x, DefineStreamFactory y) => Equals(x.Expression, y.Expression);

        protected virtual bool EqualsDefineSubscriptionFactory(DefineSubscriptionFactory x, DefineSubscriptionFactory y) => Equals(x.Expression, y.Expression);

        protected virtual bool EqualsDelete(DeleteServiceOperation x, DeleteServiceOperation y) => x.Kind switch
        {
            ServiceOperationKind.DeleteStream => EqualsDeleteStream((DeleteStream)x, (DeleteStream)y),
            ServiceOperationKind.DeleteSubscription => EqualsDeleteSubscription((DeleteSubscription)x, (DeleteSubscription)y),
            _ => false,
        };

        protected virtual bool EqualsDeleteStream(DeleteStream x, DeleteStream y) => true;

        protected virtual bool EqualsDeleteSubscription(DeleteSubscription x, DeleteSubscription y) => true;

        protected virtual bool EqualsMetadata(MetadataOperation x, MetadataOperation y)
        {
            return x.Kind switch
            {
                ServiceOperationKind.DeleteObservableMetadata or ServiceOperationKind.DeleteObserverMetadata or ServiceOperationKind.DeleteStreamFactoryMetadata or ServiceOperationKind.DeleteSubscriptionFactoryMetadata or ServiceOperationKind.DeleteStreamMetadata or ServiceOperationKind.DeleteSubscriptionMetadata => EqualsDeleteMetadata((DeleteMetadataOperation)x, (DeleteMetadataOperation)y),
                ServiceOperationKind.InsertObservableMetadata or ServiceOperationKind.InsertObserverMetadata or ServiceOperationKind.InsertStreamFactoryMetadata or ServiceOperationKind.InsertSubscriptionFactoryMetadata or ServiceOperationKind.InsertStreamMetadata or ServiceOperationKind.InsertSubscriptionMetadata => EqualsInsertMetadata((InsertMetadataOperation)x, (InsertMetadataOperation)y),
                ServiceOperationKind.LookupObservableMetadata or ServiceOperationKind.LookupObserverMetadata or ServiceOperationKind.LookupStreamFactoryMetadata or ServiceOperationKind.LookupSubscriptionFactoryMetadata or ServiceOperationKind.LookupStreamMetadata or ServiceOperationKind.LookupSubscriptionMetadata => EqualsLookupMetadata((LookupMetadataOperation)x, (LookupMetadataOperation)y),
                ServiceOperationKind.MetadataQuery => EqualsMetadataQuery((MetadataQuery)x, (MetadataQuery)y),
                _ => false,
            };
        }

        protected virtual bool EqualsDeleteMetadata(DeleteMetadataOperation x, DeleteMetadataOperation y) => x.Kind switch
        {
            ServiceOperationKind.DeleteObservableMetadata => EqualsDeleteObservableMetadata((DeleteObservableMetadata)x, (DeleteObservableMetadata)y),
            ServiceOperationKind.DeleteObserverMetadata => EqualsDeleteObserverMetadata((DeleteObserverMetadata)x, (DeleteObserverMetadata)y),
            ServiceOperationKind.DeleteStreamFactoryMetadata => EqualsDeleteStreamFactoryMetadata((DeleteStreamFactoryMetadata)x, (DeleteStreamFactoryMetadata)y),
            ServiceOperationKind.DeleteSubscriptionFactoryMetadata => EqualsDeleteSubscriptionFactoryMetadata((DeleteSubscriptionFactoryMetadata)x, (DeleteSubscriptionFactoryMetadata)y),
            ServiceOperationKind.DeleteStreamMetadata => EqualsDeleteStreamMetadata((DeleteStreamMetadata)x, (DeleteStreamMetadata)y),
            ServiceOperationKind.DeleteSubscriptionMetadata => EqualsDeleteSubscriptionMetadata((DeleteSubscriptionMetadata)x, (DeleteSubscriptionMetadata)y),
            _ => false,
        };

        protected virtual bool EqualsDeleteObservableMetadata(DeleteObservableMetadata x, DeleteObservableMetadata y) => Equals(x.Expression, y.Expression);

        protected virtual bool EqualsDeleteObserverMetadata(DeleteObserverMetadata x, DeleteObserverMetadata y) => Equals(x.Expression, y.Expression);

        protected virtual bool EqualsDeleteStreamFactoryMetadata(DeleteStreamFactoryMetadata x, DeleteStreamFactoryMetadata y) => Equals(x.Expression, y.Expression);

        protected virtual bool EqualsDeleteSubscriptionFactoryMetadata(DeleteSubscriptionFactoryMetadata x, DeleteSubscriptionFactoryMetadata y) => Equals(x.Expression, y.Expression);

        protected virtual bool EqualsDeleteStreamMetadata(DeleteStreamMetadata x, DeleteStreamMetadata y) => Equals(x.Expression, y.Expression);

        protected virtual bool EqualsDeleteSubscriptionMetadata(DeleteSubscriptionMetadata x, DeleteSubscriptionMetadata y) => Equals(x.Expression, y.Expression);

        protected virtual bool EqualsInsertMetadata(InsertMetadataOperation x, InsertMetadataOperation y) => x.Kind switch
        {
            ServiceOperationKind.InsertObservableMetadata => EqualsInsertObservableMetadata((InsertObservableMetadata)x, (InsertObservableMetadata)y),
            ServiceOperationKind.InsertObserverMetadata => EqualsInsertObserverMetadata((InsertObserverMetadata)x, (InsertObserverMetadata)y),
            ServiceOperationKind.InsertStreamFactoryMetadata => EqualsInsertStreamFactoryMetadata((InsertStreamFactoryMetadata)x, (InsertStreamFactoryMetadata)y),
            ServiceOperationKind.InsertSubscriptionFactoryMetadata => EqualsInsertSubscriptionFactoryMetadata((InsertSubscriptionFactoryMetadata)x, (InsertSubscriptionFactoryMetadata)y),
            ServiceOperationKind.InsertStreamMetadata => EqualsInsertStreamMetadata((InsertStreamMetadata)x, (InsertStreamMetadata)y),
            ServiceOperationKind.InsertSubscriptionMetadata => EqualsInsertSubscriptionMetadata((InsertSubscriptionMetadata)x, (InsertSubscriptionMetadata)y),
            _ => false,
        };

        protected virtual bool EqualsInsertObservableMetadata(InsertObservableMetadata x, InsertObservableMetadata y) => Equals(x.Expression, y.Expression);

        protected virtual bool EqualsInsertObserverMetadata(InsertObserverMetadata x, InsertObserverMetadata y) => Equals(x.Expression, y.Expression);

        protected virtual bool EqualsInsertStreamFactoryMetadata(InsertStreamFactoryMetadata x, InsertStreamFactoryMetadata y) => Equals(x.Expression, y.Expression);

        protected virtual bool EqualsInsertSubscriptionFactoryMetadata(InsertSubscriptionFactoryMetadata x, InsertSubscriptionFactoryMetadata y) => Equals(x.Expression, y.Expression);

        protected virtual bool EqualsInsertStreamMetadata(InsertStreamMetadata x, InsertStreamMetadata y) => Equals(x.Expression, y.Expression);

        protected virtual bool EqualsInsertSubscriptionMetadata(InsertSubscriptionMetadata x, InsertSubscriptionMetadata y) => Equals(x.Expression, y.Expression);

        protected virtual bool EqualsLookupMetadata(LookupMetadataOperation x, LookupMetadataOperation y) => x.Kind switch
        {
            ServiceOperationKind.LookupObservableMetadata => EqualsLookupObservableMetadata((LookupObservableMetadata)x, (LookupObservableMetadata)y),
            ServiceOperationKind.LookupObserverMetadata => EqualsLookupObserverMetadata((LookupObserverMetadata)x, (LookupObserverMetadata)y),
            ServiceOperationKind.LookupStreamFactoryMetadata => EqualsLookupStreamFactoryMetadata((LookupStreamFactoryMetadata)x, (LookupStreamFactoryMetadata)y),
            ServiceOperationKind.LookupSubscriptionFactoryMetadata => EqualsLookupSubscriptionFactoryMetadata((LookupSubscriptionFactoryMetadata)x, (LookupSubscriptionFactoryMetadata)y),
            ServiceOperationKind.LookupStreamMetadata => EqualsLookupStreamMetadata((LookupStreamMetadata)x, (LookupStreamMetadata)y),
            ServiceOperationKind.LookupSubscriptionMetadata => EqualsLookupSubscriptionMetadata((LookupSubscriptionMetadata)x, (LookupSubscriptionMetadata)y),
            _ => false,
        };

        protected virtual bool EqualsLookupObservableMetadata(LookupObservableMetadata x, LookupObservableMetadata y) => Equals(x.Expression, y.Expression);

        protected virtual bool EqualsLookupObserverMetadata(LookupObserverMetadata x, LookupObserverMetadata y) => Equals(x.Expression, y.Expression);

        protected virtual bool EqualsLookupStreamFactoryMetadata(LookupStreamFactoryMetadata x, LookupStreamFactoryMetadata y) => Equals(x.Expression, y.Expression);

        protected virtual bool EqualsLookupSubscriptionFactoryMetadata(LookupSubscriptionFactoryMetadata x, LookupSubscriptionFactoryMetadata y) => Equals(x.Expression, y.Expression);

        protected virtual bool EqualsLookupStreamMetadata(LookupStreamMetadata x, LookupStreamMetadata y) => Equals(x.Expression, y.Expression);

        protected virtual bool EqualsLookupSubscriptionMetadata(LookupSubscriptionMetadata x, LookupSubscriptionMetadata y) => Equals(x.Expression, y.Expression);

        protected virtual bool EqualsMetadataQuery(MetadataQuery x, MetadataQuery y) => Equals(x.Expression, y.Expression);

        protected virtual bool EqualsObserver(ObserverOperation x, ObserverOperation y) => x.Kind switch
        {
            ServiceOperationKind.ObserverOnNext => EqualsOnNext((ObserverOnNext)x, (ObserverOnNext)y),
            ServiceOperationKind.ObserverOnError => EqualsOnError((ObserverOnError)x, (ObserverOnError)y),
            ServiceOperationKind.ObserverOnCompleted => EqualsOnCompleted((ObserverOnCompleted)x, (ObserverOnCompleted)y),
            _ => false,
        };

        protected virtual bool EqualsOnNext(ObserverOnNext x, ObserverOnNext y) => object.Equals(x.Value, y.Value);

        protected virtual bool EqualsOnError(ObserverOnError x, ObserverOnError y) => object.Equals(x.Error, y.Error);

        protected virtual bool EqualsOnCompleted(ObserverOnCompleted x, ObserverOnCompleted y) => true;

        protected virtual bool EqualsUndefine(UndefineServiceOperation x, UndefineServiceOperation y) => x.Kind switch
        {
            ServiceOperationKind.UndefineObserver => EqualsUndefineObserver((UndefineObserver)x, (UndefineObserver)y),
            ServiceOperationKind.UndefineObservable => EqualsUndefineObservable((UndefineObservable)x, (UndefineObservable)y),
            ServiceOperationKind.UndefineStreamFactory => EqualsUndefineStreamFactory((UndefineStreamFactory)x, (UndefineStreamFactory)y),
            ServiceOperationKind.UndefineSubscriptionFactory => EqualsUndefineSubscriptionFactory((UndefineSubscriptionFactory)x, (UndefineSubscriptionFactory)y),
            _ => false,
        };

        protected virtual bool EqualsUndefineObserver(UndefineObserver x, UndefineObserver y) => true;

        protected virtual bool EqualsUndefineObservable(UndefineObservable x, UndefineObservable y) => true;

        protected virtual bool EqualsUndefineStreamFactory(UndefineStreamFactory x, UndefineStreamFactory y) => true;

        protected virtual bool EqualsUndefineSubscriptionFactory(UndefineSubscriptionFactory x, UndefineSubscriptionFactory y) => true;

        protected abstract bool Equals(Expression x, Expression y);

        public int GetHashCode(ServiceOperation obj) => 0;
    }
}
