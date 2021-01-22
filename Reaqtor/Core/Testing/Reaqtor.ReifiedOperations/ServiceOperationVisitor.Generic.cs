// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Validate arguments of public methods. (Omitting null checks for protected methods.)

using System;
using System.Linq.CompilerServices;
using System.Reflection;

namespace Reaqtor.TestingFramework
{
    public abstract class ServiceOperationVisitor<TResult>
    {
        private static readonly MethodInfo s_onNextGeneric = ((MethodInfo)ReflectionHelpers.InfoOf((ServiceOperationVisitor<TResult> v) => v.VisitObserverOnNextCore<object>(null))).GetGenericMethodDefinition();
        private static readonly MethodInfo s_onErrorGeneric = ((MethodInfo)ReflectionHelpers.InfoOf((ServiceOperationVisitor<TResult> v) => v.VisitObserverOnErrorCore<object>(null))).GetGenericMethodDefinition();
        private static readonly MethodInfo s_onCompletedGeneric = ((MethodInfo)ReflectionHelpers.InfoOf((ServiceOperationVisitor<TResult> v) => v.VisitObserverOnCompletedCore<object>(null))).GetGenericMethodDefinition();

        public virtual TResult Visit(ServiceOperation operation)
        {
            if (operation == null)
            {
                throw new ArgumentNullException(nameof(operation));
            }

            return operation.Kind switch
            {
                ServiceOperationKind.CreateObserver or ServiceOperationKind.CreateStream or ServiceOperationKind.CreateSubscription => VisitCreate((CreateServiceOperation)operation),
                ServiceOperationKind.DefineObservable or ServiceOperationKind.DefineObserver or ServiceOperationKind.DefineStreamFactory or ServiceOperationKind.DefineSubscriptionFactory => VisitDefine((DefineServiceOperation)operation),
                ServiceOperationKind.DeleteStream or ServiceOperationKind.DeleteSubscription => VisitDelete((DeleteServiceOperation)operation),
                ServiceOperationKind.DeleteObservableMetadata or ServiceOperationKind.DeleteObserverMetadata or ServiceOperationKind.DeleteStreamFactoryMetadata or ServiceOperationKind.DeleteSubscriptionFactoryMetadata or ServiceOperationKind.DeleteStreamMetadata or ServiceOperationKind.DeleteSubscriptionMetadata or ServiceOperationKind.InsertObservableMetadata or ServiceOperationKind.InsertObserverMetadata or ServiceOperationKind.InsertStreamFactoryMetadata or ServiceOperationKind.InsertSubscriptionFactoryMetadata or ServiceOperationKind.InsertStreamMetadata or ServiceOperationKind.InsertSubscriptionMetadata or ServiceOperationKind.LookupObservableMetadata or ServiceOperationKind.LookupObserverMetadata or ServiceOperationKind.LookupStreamFactoryMetadata or ServiceOperationKind.LookupSubscriptionFactoryMetadata or ServiceOperationKind.LookupStreamMetadata or ServiceOperationKind.LookupSubscriptionMetadata or ServiceOperationKind.MetadataQuery => VisitMetadata((MetadataOperation)operation),
                ServiceOperationKind.ObserverOnNext or ServiceOperationKind.ObserverOnError or ServiceOperationKind.ObserverOnCompleted => VisitObserver((ObserverOperation)operation),
                ServiceOperationKind.UndefineObservable or ServiceOperationKind.UndefineObserver or ServiceOperationKind.UndefineStreamFactory or ServiceOperationKind.UndefineSubscriptionFactory => VisitUndefine((UndefineServiceOperation)operation),
                _ => VisitExtensions(operation),
            };
        }

        protected virtual TResult VisitCreate(CreateServiceOperation operation)
        {
            return operation.Kind switch
            {
                ServiceOperationKind.CreateObserver => VisitCreateObserver((CreateObserver)operation),
                ServiceOperationKind.CreateStream => VisitCreateStream((CreateStream)operation),
                ServiceOperationKind.CreateSubscription => VisitCreateSubscription((CreateSubscription)operation),
                _ => throw new NotImplementedException(),
            };
        }

        protected abstract TResult VisitCreateObserver(CreateObserver operation);

        protected abstract TResult VisitCreateStream(CreateStream operation);

        protected abstract TResult VisitCreateSubscription(CreateSubscription operation);

        protected virtual TResult VisitDefine(DefineServiceOperation operation)
        {
            return operation.Kind switch
            {
                ServiceOperationKind.DefineObservable => VisitDefineObservable((DefineObservable)operation),
                ServiceOperationKind.DefineObserver => VisitDefineObserver((DefineObserver)operation),
                ServiceOperationKind.DefineStreamFactory => VisitDefineStreamFactory((DefineStreamFactory)operation),
                ServiceOperationKind.DefineSubscriptionFactory => VisitDefineSubscriptionFactory((DefineSubscriptionFactory)operation),
                _ => throw new NotImplementedException(),
            };
        }

        protected abstract TResult VisitDefineObservable(DefineObservable operation);

        protected abstract TResult VisitDefineObserver(DefineObserver operation);

        protected abstract TResult VisitDefineStreamFactory(DefineStreamFactory operation);

        protected abstract TResult VisitDefineSubscriptionFactory(DefineSubscriptionFactory operation);

        protected virtual TResult VisitDelete(DeleteServiceOperation operation)
        {
            return operation.Kind switch
            {
                ServiceOperationKind.DeleteStream => VisitDeleteStream((DeleteStream)operation),
                ServiceOperationKind.DeleteSubscription => VisitDeleteSubscription((DeleteSubscription)operation),
                _ => throw new NotImplementedException(),
            };
        }

        protected abstract TResult VisitDeleteStream(DeleteStream operation);

        protected abstract TResult VisitDeleteSubscription(DeleteSubscription operation);

        protected virtual TResult VisitMetadata(MetadataOperation operation)
        {
            return operation.Kind switch
            {
                ServiceOperationKind.DeleteObservableMetadata or ServiceOperationKind.DeleteObserverMetadata or ServiceOperationKind.DeleteStreamFactoryMetadata or ServiceOperationKind.DeleteSubscriptionFactoryMetadata or ServiceOperationKind.DeleteStreamMetadata or ServiceOperationKind.DeleteSubscriptionMetadata => VisitDeleteMetadata((DeleteMetadataOperation)operation),
                ServiceOperationKind.InsertObservableMetadata or ServiceOperationKind.InsertObserverMetadata or ServiceOperationKind.InsertStreamFactoryMetadata or ServiceOperationKind.InsertSubscriptionFactoryMetadata or ServiceOperationKind.InsertStreamMetadata or ServiceOperationKind.InsertSubscriptionMetadata => VisitInsertMetadata((InsertMetadataOperation)operation),
                ServiceOperationKind.LookupObservableMetadata or ServiceOperationKind.LookupObserverMetadata or ServiceOperationKind.LookupStreamFactoryMetadata or ServiceOperationKind.LookupSubscriptionFactoryMetadata or ServiceOperationKind.LookupStreamMetadata or ServiceOperationKind.LookupSubscriptionMetadata => VisitLookupMetadata((LookupMetadataOperation)operation),
                ServiceOperationKind.MetadataQuery => VisitMetadataQuery((MetadataQuery)operation),
                _ => throw new NotImplementedException(),
            };
        }

        protected virtual TResult VisitDeleteMetadata(DeleteMetadataOperation operation)
        {
            return operation.Kind switch
            {
                ServiceOperationKind.DeleteObservableMetadata => VisitDeleteObservableMetadata((DeleteObservableMetadata)operation),
                ServiceOperationKind.DeleteObserverMetadata => VisitDeleteObserverMetadata((DeleteObserverMetadata)operation),
                ServiceOperationKind.DeleteStreamFactoryMetadata => VisitDeleteStreamFactoryMetadata((DeleteStreamFactoryMetadata)operation),
                ServiceOperationKind.DeleteSubscriptionFactoryMetadata => VisitDeleteSubscriptionFactoryMetadata((DeleteSubscriptionFactoryMetadata)operation),
                ServiceOperationKind.DeleteStreamMetadata => VisitDeleteStreamMetadata((DeleteStreamMetadata)operation),
                ServiceOperationKind.DeleteSubscriptionMetadata => VisitDeleteSubscriptionMetadata((DeleteSubscriptionMetadata)operation),
                _ => throw new NotImplementedException(),
            };
        }

        protected abstract TResult VisitDeleteObservableMetadata(DeleteObservableMetadata operation);

        protected abstract TResult VisitDeleteObserverMetadata(DeleteObserverMetadata operation);

        protected abstract TResult VisitDeleteStreamFactoryMetadata(DeleteStreamFactoryMetadata operation);

        protected abstract TResult VisitDeleteSubscriptionFactoryMetadata(DeleteSubscriptionFactoryMetadata operation);

        protected abstract TResult VisitDeleteStreamMetadata(DeleteStreamMetadata operation);

        protected abstract TResult VisitDeleteSubscriptionMetadata(DeleteSubscriptionMetadata operation);

        protected virtual TResult VisitInsertMetadata(InsertMetadataOperation operation)
        {
            return operation.Kind switch
            {
                ServiceOperationKind.InsertObservableMetadata => VisitInsertObservableMetadata((InsertObservableMetadata)operation),
                ServiceOperationKind.InsertObserverMetadata => VisitInsertObserverMetadata((InsertObserverMetadata)operation),
                ServiceOperationKind.InsertStreamFactoryMetadata => VisitInsertStreamFactoryMetadata((InsertStreamFactoryMetadata)operation),
                ServiceOperationKind.InsertSubscriptionFactoryMetadata => VisitInsertSubscriptionFactoryMetadata((InsertSubscriptionFactoryMetadata)operation),
                ServiceOperationKind.InsertStreamMetadata => VisitInsertStreamMetadata((InsertStreamMetadata)operation),
                ServiceOperationKind.InsertSubscriptionMetadata => VisitInsertSubscriptionMetadata((InsertSubscriptionMetadata)operation),
                _ => throw new NotImplementedException(),
            };
        }

        protected abstract TResult VisitInsertObservableMetadata(InsertObservableMetadata operation);

        protected abstract TResult VisitInsertObserverMetadata(InsertObserverMetadata operation);

        protected abstract TResult VisitInsertStreamFactoryMetadata(InsertStreamFactoryMetadata operation);

        protected abstract TResult VisitInsertSubscriptionFactoryMetadata(InsertSubscriptionFactoryMetadata operation);

        protected abstract TResult VisitInsertStreamMetadata(InsertStreamMetadata operation);

        protected abstract TResult VisitInsertSubscriptionMetadata(InsertSubscriptionMetadata operation);

        protected virtual TResult VisitLookupMetadata(LookupMetadataOperation operation)
        {
            return operation.Kind switch
            {
                ServiceOperationKind.LookupObservableMetadata => VisitLookupObservableMetadata((LookupObservableMetadata)operation),
                ServiceOperationKind.LookupObserverMetadata => VisitLookupObserverMetadata((LookupObserverMetadata)operation),
                ServiceOperationKind.LookupStreamFactoryMetadata => VisitLookupStreamFactoryMetadata((LookupStreamFactoryMetadata)operation),
                ServiceOperationKind.LookupSubscriptionFactoryMetadata => VisitLookupSubscriptionFactoryMetadata((LookupSubscriptionFactoryMetadata)operation),
                ServiceOperationKind.LookupStreamMetadata => VisitLookupStreamMetadata((LookupStreamMetadata)operation),
                ServiceOperationKind.LookupSubscriptionMetadata => VisitLookupSubscriptionMetadata((LookupSubscriptionMetadata)operation),
                _ => throw new NotImplementedException(),
            };
        }

        protected abstract TResult VisitLookupObservableMetadata(LookupObservableMetadata operation);

        protected abstract TResult VisitLookupObserverMetadata(LookupObserverMetadata operation);

        protected abstract TResult VisitLookupStreamFactoryMetadata(LookupStreamFactoryMetadata operation);

        protected abstract TResult VisitLookupSubscriptionFactoryMetadata(LookupSubscriptionFactoryMetadata operation);

        protected abstract TResult VisitLookupStreamMetadata(LookupStreamMetadata operation);

        protected abstract TResult VisitLookupSubscriptionMetadata(LookupSubscriptionMetadata operation);

        protected abstract TResult VisitMetadataQuery(MetadataQuery operation);

        protected virtual TResult VisitObserver(ObserverOperation operation)
        {
            return operation.Kind switch
            {
                ServiceOperationKind.ObserverOnNext => VisitObserverOnNext((ObserverOnNext)operation),
                ServiceOperationKind.ObserverOnError => VisitObserverOnError((ObserverOnError)operation),
                ServiceOperationKind.ObserverOnCompleted => VisitObserverOnCompleted((ObserverOnCompleted)operation),
                _ => throw new NotImplementedException(),
            };
        }

        private TResult VisitObserverOnNext(ObserverOnNext operation)
        {
            var genericType = operation.GetType().FindGenericType(typeof(ObserverOnNext<>));
            if (genericType != null)
            {
                var method = s_onNextGeneric.MakeGenericMethod(genericType.GenericTypeArguments[0]);
                return (TResult)method.Invoke(this, new object[] { operation });
            }

            return VisitObserverOnNextCore(operation);
        }

        protected abstract TResult VisitObserverOnNextCore(ObserverOnNext operation);

        protected abstract TResult VisitObserverOnNextCore<T>(ObserverOnNext<T> operation);

        private TResult VisitObserverOnError(ObserverOnError operation)
        {
            var genericType = operation.GetType().FindGenericType(typeof(ObserverOnError<>));
            if (genericType != null)
            {
                var method = s_onErrorGeneric.MakeGenericMethod(genericType.GenericTypeArguments[0]);
                return (TResult)method.Invoke(this, new object[] { operation });
            }

            return VisitObserverOnErrorCore(operation);
        }

        protected abstract TResult VisitObserverOnErrorCore(ObserverOnError operation);

        protected abstract TResult VisitObserverOnErrorCore<T>(ObserverOnError<T> operation);

        private TResult VisitObserverOnCompleted(ObserverOnCompleted operation)
        {
            var genericType = operation.GetType().FindGenericType(typeof(ObserverOnCompleted<>));
            if (genericType != null)
            {
                var method = s_onCompletedGeneric.MakeGenericMethod(genericType.GenericTypeArguments[0]);
                return (TResult)method.Invoke(this, new object[] { operation });
            }

            return VisitObserverOnCompletedCore(operation);
        }

        protected abstract TResult VisitObserverOnCompletedCore(ObserverOnCompleted operation);

        protected abstract TResult VisitObserverOnCompletedCore<T>(ObserverOnCompleted<T> operation);

        protected virtual TResult VisitUndefine(UndefineServiceOperation operation)
        {
            return operation.Kind switch
            {
                ServiceOperationKind.UndefineObservable => VisitUndefineObservable((UndefineObservable)operation),
                ServiceOperationKind.UndefineObserver => VisitUndefineObserver((UndefineObserver)operation),
                ServiceOperationKind.UndefineStreamFactory => VisitUndefineStreamFactory((UndefineStreamFactory)operation),
                ServiceOperationKind.UndefineSubscriptionFactory => VisitUndefineSubscriptionFactory((UndefineSubscriptionFactory)operation),
                _ => throw new NotImplementedException(),
            };
        }

        protected abstract TResult VisitUndefineObservable(UndefineObservable operation);

        protected abstract TResult VisitUndefineObserver(UndefineObserver operation);

        protected abstract TResult VisitUndefineStreamFactory(UndefineStreamFactory operation);

        protected abstract TResult VisitUndefineSubscriptionFactory(UndefineSubscriptionFactory operation);

        protected abstract TResult VisitExtensions(ServiceOperation operation);
    }
}
