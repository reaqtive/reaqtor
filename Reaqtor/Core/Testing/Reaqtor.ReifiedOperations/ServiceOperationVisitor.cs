// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Validate arguments of public methods. (Omitting null checks for protected methods.)

using System;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

namespace Reaqtor.TestingFramework
{
    public class ServiceOperationVisitor
    {
        private static readonly MethodInfo s_onNextGeneric = ((MethodInfo)ReflectionHelpers.InfoOf((ServiceOperationVisitor v) => v.VisitObserverOnNextCore<object>(null))).GetGenericMethodDefinition();
        private static readonly MethodInfo s_onErrorGeneric = ((MethodInfo)ReflectionHelpers.InfoOf((ServiceOperationVisitor v) => v.VisitObserverOnErrorCore<object>(null))).GetGenericMethodDefinition();
        private static readonly MethodInfo s_onCompletedGeneric = ((MethodInfo)ReflectionHelpers.InfoOf((ServiceOperationVisitor v) => v.VisitObserverOnCompletedCore<object>(null))).GetGenericMethodDefinition();

        public virtual ServiceOperation Visit(ServiceOperation operation)
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

        protected virtual ServiceOperation VisitCreate(CreateServiceOperation operation)
        {
            return operation.Kind switch
            {
                ServiceOperationKind.CreateObserver => VisitCreateObserver((CreateObserver)operation),
                ServiceOperationKind.CreateStream => VisitCreateStream((CreateStream)operation),
                ServiceOperationKind.CreateSubscription => VisitCreateSubscription((CreateSubscription)operation),
                _ => throw new NotImplementedException(),
            };
        }

        protected virtual ServiceOperation VisitCreateObserver(CreateObserver operation)
        {
            var uri = VisitUri(operation.TargetObjectUri);
            if (uri != operation.TargetObjectUri)
            {
                return new CreateObserver(uri);
            }

            return operation;
        }

        protected virtual ServiceOperation VisitCreateStream(CreateStream operation)
        {
            var uri = VisitUri(operation.TargetObjectUri);
            var expression = VisitExpression(operation.Expression);
            var state = VisitState(operation.State);
            if (uri != operation.TargetObjectUri || expression != operation.Expression || state != operation.State)
            {
                return new CreateStream(uri, expression, state);
            }

            return operation;
        }

        protected virtual ServiceOperation VisitCreateSubscription(CreateSubscription operation)
        {
            var uri = VisitUri(operation.TargetObjectUri);
            var expression = VisitExpression(operation.Expression);
            var state = VisitState(operation.State);
            if (uri != operation.TargetObjectUri || expression != operation.Expression || state != operation.State)
            {
                return new CreateSubscription(uri, expression, state);
            }

            return operation;
        }

        protected virtual ServiceOperation VisitDefine(DefineServiceOperation operation)
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

        protected virtual ServiceOperation VisitDefineObservable(DefineObservable operation)
        {
            var uri = VisitUri(operation.TargetObjectUri);
            var expression = VisitExpression(operation.Expression);
            var state = VisitState(operation.State);
            if (uri != operation.TargetObjectUri || expression != operation.Expression || state != operation.State)
            {
                return new DefineObservable(uri, expression, state);
            }

            return operation;
        }

        protected virtual ServiceOperation VisitDefineObserver(DefineObserver operation)
        {
            var uri = VisitUri(operation.TargetObjectUri);
            var expression = VisitExpression(operation.Expression);
            var state = VisitState(operation.State);
            if (uri != operation.TargetObjectUri || expression != operation.Expression || state != operation.State)
            {
                return new DefineObserver(uri, expression, state);
            }

            return operation;
        }

        protected virtual ServiceOperation VisitDefineStreamFactory(DefineStreamFactory operation)
        {
            var uri = VisitUri(operation.TargetObjectUri);
            var expression = VisitExpression(operation.Expression);
            var state = VisitState(operation.State);

            if (uri != operation.TargetObjectUri || expression != operation.Expression || state != operation.State)
            {
                return new DefineStreamFactory(uri, expression, state);
            }

            return operation;
        }

        protected virtual ServiceOperation VisitDefineSubscriptionFactory(DefineSubscriptionFactory operation)
        {
            var uri = VisitUri(operation.TargetObjectUri);
            var expression = VisitExpression(operation.Expression);
            var state = VisitState(operation.State);

            if (uri != operation.TargetObjectUri || expression != operation.Expression || state != operation.State)
            {
                return new DefineSubscriptionFactory(uri, expression, state);
            }

            return operation;
        }

        protected virtual ServiceOperation VisitDelete(DeleteServiceOperation operation)
        {
            return operation.Kind switch
            {
                ServiceOperationKind.DeleteStream => VisitDeleteStream((DeleteStream)operation),
                ServiceOperationKind.DeleteSubscription => VisitDeleteSubscription((DeleteSubscription)operation),
                _ => throw new NotImplementedException(),
            };
        }

        protected virtual ServiceOperation VisitDeleteStream(DeleteStream operation)
        {
            var uri = VisitUri(operation.TargetObjectUri);
            if (uri != operation.TargetObjectUri)
            {
                return new DeleteStream(uri);
            }

            return operation;
        }

        protected virtual ServiceOperation VisitDeleteSubscription(DeleteSubscription operation)
        {
            var uri = VisitUri(operation.TargetObjectUri);
            if (uri != operation.TargetObjectUri)
            {
                return new DeleteSubscription(uri);
            }

            return operation;
        }

        protected virtual ServiceOperation VisitMetadata(MetadataOperation operation)
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

        protected virtual ServiceOperation VisitDeleteMetadata(DeleteMetadataOperation operation)
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

        protected virtual ServiceOperation VisitDeleteObservableMetadata(DeleteObservableMetadata operation)
        {
            var uri = VisitUri(operation.TargetObjectUri);
            if (uri != operation.TargetObjectUri)
            {
                return new DeleteObservableMetadata(uri);
            }

            return operation;
        }

        protected virtual ServiceOperation VisitDeleteObserverMetadata(DeleteObserverMetadata operation)
        {
            var uri = VisitUri(operation.TargetObjectUri);
            if (uri != operation.TargetObjectUri)
            {
                return new DeleteObserverMetadata(uri);
            }

            return operation;
        }

        protected virtual ServiceOperation VisitDeleteStreamFactoryMetadata(DeleteStreamFactoryMetadata operation)
        {
            var uri = VisitUri(operation.TargetObjectUri);
            if (uri != operation.TargetObjectUri)
            {
                return new DeleteStreamFactoryMetadata(uri);
            }

            return operation;
        }

        protected virtual ServiceOperation VisitDeleteSubscriptionFactoryMetadata(DeleteSubscriptionFactoryMetadata operation)
        {
            var uri = VisitUri(operation.TargetObjectUri);
            if (uri != operation.TargetObjectUri)
            {
                return new DeleteSubscriptionFactoryMetadata(uri);
            }

            return operation;
        }

        protected virtual ServiceOperation VisitDeleteStreamMetadata(DeleteStreamMetadata operation)
        {
            var uri = VisitUri(operation.TargetObjectUri);
            if (uri != operation.TargetObjectUri)
            {
                return new DeleteStreamMetadata(uri);
            }

            return operation;
        }

        protected virtual ServiceOperation VisitDeleteSubscriptionMetadata(DeleteSubscriptionMetadata operation)
        {
            var uri = VisitUri(operation.TargetObjectUri);
            if (uri != operation.TargetObjectUri)
            {
                return new DeleteSubscriptionMetadata(uri);
            }

            return operation;
        }

        protected virtual ServiceOperation VisitInsertMetadata(InsertMetadataOperation operation)
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

        protected virtual ServiceOperation VisitInsertObservableMetadata(InsertObservableMetadata operation)
        {
            var uri = VisitUri(operation.TargetObjectUri);
            var expression = VisitExpression(operation.Expression);
            var state = VisitState(operation.State);
            if (uri != operation.TargetObjectUri || expression != operation.Expression || state != operation.State)
            {
                return new InsertObservableMetadata(uri, expression, state);
            }

            return operation;
        }

        protected virtual ServiceOperation VisitInsertObserverMetadata(InsertObserverMetadata operation)
        {
            var uri = VisitUri(operation.TargetObjectUri);
            var expression = VisitExpression(operation.Expression);
            var state = VisitState(operation.State);
            if (uri != operation.TargetObjectUri || expression != operation.Expression || state != operation.State)
            {
                return new InsertObserverMetadata(uri, expression, state);
            }

            return operation;
        }

        protected virtual ServiceOperation VisitInsertStreamFactoryMetadata(InsertStreamFactoryMetadata operation)
        {
            var uri = VisitUri(operation.TargetObjectUri);
            var expression = VisitExpression(operation.Expression);
            var state = VisitState(operation.State);
            if (uri != operation.TargetObjectUri || expression != operation.Expression || state != operation.State)
            {
                return new InsertStreamFactoryMetadata(uri, expression, state);
            }

            return operation;
        }

        protected virtual ServiceOperation VisitInsertSubscriptionFactoryMetadata(InsertSubscriptionFactoryMetadata operation)
        {
            var uri = VisitUri(operation.TargetObjectUri);
            var expression = VisitExpression(operation.Expression);
            var state = VisitState(operation.State);
            if (uri != operation.TargetObjectUri || expression != operation.Expression || state != operation.State)
            {
                return new InsertSubscriptionFactoryMetadata(uri, expression, state);
            }

            return operation;
        }

        protected virtual ServiceOperation VisitInsertStreamMetadata(InsertStreamMetadata operation)
        {
            var uri = VisitUri(operation.TargetObjectUri);
            var expression = VisitExpression(operation.Expression);
            var state = VisitState(operation.State);
            if (uri != operation.TargetObjectUri || expression != operation.Expression || state != operation.State)
            {
                return new InsertStreamMetadata(uri, expression, state);
            }

            return operation;
        }

        protected virtual ServiceOperation VisitInsertSubscriptionMetadata(InsertSubscriptionMetadata operation)
        {
            var uri = VisitUri(operation.TargetObjectUri);
            var expression = VisitExpression(operation.Expression);
            var state = VisitState(operation.State);
            if (uri != operation.TargetObjectUri || expression != operation.Expression || state != operation.State)
            {
                return new InsertSubscriptionMetadata(uri, expression, state);
            }

            return operation;
        }

        protected virtual ServiceOperation VisitLookupMetadata(LookupMetadataOperation operation)
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

        protected virtual ServiceOperation VisitLookupObservableMetadata(LookupObservableMetadata operation)
        {
            var uri = VisitUri(operation.TargetObjectUri);
            if (uri != operation.TargetObjectUri)
            {
                return new LookupObservableMetadata(uri);
            }

            return operation;
        }

        protected virtual ServiceOperation VisitLookupObserverMetadata(LookupObserverMetadata operation)
        {
            var uri = VisitUri(operation.TargetObjectUri);
            if (uri != operation.TargetObjectUri)
            {
                return new LookupObserverMetadata(uri);
            }

            return operation;
        }

        protected virtual ServiceOperation VisitLookupStreamFactoryMetadata(LookupStreamFactoryMetadata operation)
        {
            var uri = VisitUri(operation.TargetObjectUri);
            if (uri != operation.TargetObjectUri)
            {
                return new LookupStreamFactoryMetadata(uri);
            }

            return operation;
        }

        protected virtual ServiceOperation VisitLookupSubscriptionFactoryMetadata(LookupSubscriptionFactoryMetadata operation)
        {
            var uri = VisitUri(operation.TargetObjectUri);
            if (uri != operation.TargetObjectUri)
            {
                return new LookupSubscriptionFactoryMetadata(uri);
            }

            return operation;
        }

        protected virtual ServiceOperation VisitLookupStreamMetadata(LookupStreamMetadata operation)
        {
            var uri = VisitUri(operation.TargetObjectUri);
            if (uri != operation.TargetObjectUri)
            {
                return new LookupStreamMetadata(uri);
            }

            return operation;
        }

        protected virtual ServiceOperation VisitLookupSubscriptionMetadata(LookupSubscriptionMetadata operation)
        {
            var uri = VisitUri(operation.TargetObjectUri);
            if (uri != operation.TargetObjectUri)
            {
                return new LookupSubscriptionMetadata(uri);
            }

            return operation;
        }

        protected virtual ServiceOperation VisitMetadataQuery(MetadataQuery operation)
        {
            var expression = VisitExpression(operation.Expression);
            if (expression != operation.Expression)
            {
                return new MetadataQuery(expression);
            }

            return operation;
        }

        protected virtual ServiceOperation VisitObserver(ObserverOperation operation)
        {
            return operation.Kind switch
            {
                ServiceOperationKind.ObserverOnNext => VisitObserverOnNext((ObserverOnNext)operation),
                ServiceOperationKind.ObserverOnError => VisitObserverOnError((ObserverOnError)operation),
                ServiceOperationKind.ObserverOnCompleted => VisitObserverOnCompleted((ObserverOnCompleted)operation),
                _ => throw new NotImplementedException(),
            };
        }

        private ServiceOperation VisitObserverOnNext(ObserverOnNext operation)
        {
            var genericType = operation.GetType().FindGenericType(typeof(ObserverOnNext<>));
            if (genericType != null)
            {
                var method = s_onNextGeneric.MakeGenericMethod(genericType.GenericTypeArguments[0]);
                return (ServiceOperation)method.Invoke(this, new object[] { operation });
            }

            return VisitObserverOnNextCore(operation);
        }

        protected virtual ServiceOperation VisitObserverOnNextCore(ObserverOnNext operation)
        {
            var uri = VisitUri(operation.TargetObjectUri);
            if (uri != operation.TargetObjectUri)
            {
                return new ObserverOnNext(uri, operation.Value);
            }

            return operation;
        }

        protected virtual ServiceOperation VisitObserverOnNextCore<T>(ObserverOnNext<T> operation)
        {
            var uri = VisitUri(operation.TargetObjectUri);
            if (uri != operation.TargetObjectUri)
            {
                return new ObserverOnNext<T>(uri, operation.Value);
            }

            return operation;
        }

        private ServiceOperation VisitObserverOnError(ObserverOnError operation)
        {
            var genericType = operation.GetType().FindGenericType(typeof(ObserverOnError<>));
            if (genericType != null)
            {
                var method = s_onErrorGeneric.MakeGenericMethod(genericType.GenericTypeArguments[0]);
                return (ServiceOperation)method.Invoke(this, new object[] { operation });
            }

            return VisitObserverOnErrorCore(operation);
        }

        protected virtual ServiceOperation VisitObserverOnErrorCore(ObserverOnError operation)
        {
            var uri = VisitUri(operation.TargetObjectUri);
            if (uri != operation.TargetObjectUri)
            {
                return new ObserverOnError(uri, operation.Error);
            }

            return operation;
        }

        protected virtual ServiceOperation VisitObserverOnErrorCore<T>(ObserverOnError<T> operation)
        {
            var uri = VisitUri(operation.TargetObjectUri);
            if (uri != operation.TargetObjectUri)
            {
                return new ObserverOnError<T>(uri, operation.Error);
            }

            return operation;
        }

        private ServiceOperation VisitObserverOnCompleted(ObserverOnCompleted operation)
        {
            var genericType = operation.GetType().FindGenericType(typeof(ObserverOnCompleted<>));
            if (genericType != null)
            {
                var method = s_onCompletedGeneric.MakeGenericMethod(genericType.GenericTypeArguments[0]);
                return (ServiceOperation)method.Invoke(this, new object[] { operation });
            }

            return VisitObserverOnCompletedCore(operation);
        }

        protected virtual ServiceOperation VisitObserverOnCompletedCore(ObserverOnCompleted operation)
        {
            var uri = VisitUri(operation.TargetObjectUri);
            if (uri != operation.TargetObjectUri)
            {
                return new ObserverOnCompleted(uri);
            }

            return operation;
        }

        protected virtual ServiceOperation VisitObserverOnCompletedCore<T>(ObserverOnCompleted<T> operation)
        {
            var uri = VisitUri(operation.TargetObjectUri);
            if (uri != operation.TargetObjectUri)
            {
                return new ObserverOnCompleted<T>(uri);
            }

            return operation;
        }

        protected virtual ServiceOperation VisitUndefine(UndefineServiceOperation operation)
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

        protected virtual ServiceOperation VisitUndefineObservable(UndefineObservable operation)
        {
            var uri = VisitUri(operation.TargetObjectUri);
            if (uri != operation.TargetObjectUri)
            {
                return new UndefineObservable(uri);
            }

            return operation;
        }

        protected virtual ServiceOperation VisitUndefineObserver(UndefineObserver operation)
        {
            var uri = VisitUri(operation.TargetObjectUri);
            if (uri != operation.TargetObjectUri)
            {
                return new UndefineObserver(uri);
            }

            return operation;
        }

        protected virtual ServiceOperation VisitUndefineStreamFactory(UndefineStreamFactory operation)
        {
            var uri = VisitUri(operation.TargetObjectUri);
            if (uri != operation.TargetObjectUri)
            {
                return new UndefineStreamFactory(uri);
            }

            return operation;
        }

        protected virtual ServiceOperation VisitUndefineSubscriptionFactory(UndefineSubscriptionFactory operation)
        {
            var uri = VisitUri(operation.TargetObjectUri);
            if (uri != operation.TargetObjectUri)
            {
                return new UndefineSubscriptionFactory(uri);
            }

            return operation;
        }

        protected virtual ServiceOperation VisitExtensions(ServiceOperation operation) => throw new NotImplementedException();

        protected virtual Uri VisitUri(Uri uri) => uri;

        protected virtual Expression VisitExpression(Expression expression) => expression;

        protected virtual object VisitState(object state) => state;
    }
}
