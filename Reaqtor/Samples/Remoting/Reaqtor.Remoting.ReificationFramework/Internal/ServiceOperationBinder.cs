// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Threading;

using Reaqtor.Hosting.Shared.Serialization;
using Reaqtor.Remoting.Protocol;
using Reaqtor.TestingFramework;

namespace Reaqtor.Remoting.ReificationFramework
{
    public class ServiceOperationBinder : ServiceOperationVisitor<Expression<Action<IReactiveServiceConnection>>>
    {
        private readonly CommandTextFactory<Expression> _commandTextFactory = new(new ClientSerializationHelpers());

        protected override Expression<Action<IReactiveServiceConnection>> VisitCreateStream(CreateStream operation)
        {
            Debug.Assert(operation != null);
            var commandText = _commandTextFactory.CreateNewText(new NewCommandData<Expression>(operation.TargetObjectUri, operation.Expression, operation.State));
            return InlineClosures(c => c.CreateCommand(CommandVerb.New, CommandNoun.Stream, commandText).ExecuteAsync(CancellationToken.None).Wait());
        }

        protected override Expression<Action<IReactiveServiceConnection>> VisitCreateSubscription(CreateSubscription operation)
        {
            Debug.Assert(operation != null);
            var commandText = _commandTextFactory.CreateNewText(new NewCommandData<Expression>(operation.TargetObjectUri, operation.Expression, operation.State));
            return InlineClosures(c => c.CreateCommand(CommandVerb.New, CommandNoun.Subscription, commandText).ExecuteAsync(CancellationToken.None).Wait());
        }

        protected override Expression<Action<IReactiveServiceConnection>> VisitDefineObservable(DefineObservable operation)
        {
            Debug.Assert(operation != null);
            var commandText = _commandTextFactory.CreateNewText(new NewCommandData<Expression>(operation.TargetObjectUri, operation.Expression, operation.State));
            return InlineClosures(c => c.CreateCommand(CommandVerb.New, CommandNoun.Observable, commandText).ExecuteAsync(CancellationToken.None).Wait());
        }

        protected override Expression<Action<IReactiveServiceConnection>> VisitDefineObserver(DefineObserver operation)
        {
            Debug.Assert(operation != null);
            var commandText = _commandTextFactory.CreateNewText(new NewCommandData<Expression>(operation.TargetObjectUri, operation.Expression, operation.State));
            return InlineClosures(c => c.CreateCommand(CommandVerb.New, CommandNoun.Observer, commandText).ExecuteAsync(CancellationToken.None).Wait());
        }

        protected override Expression<Action<IReactiveServiceConnection>> VisitDefineStreamFactory(DefineStreamFactory operation)
        {
            Debug.Assert(operation != null);
            var commandText = _commandTextFactory.CreateNewText(new NewCommandData<Expression>(operation.TargetObjectUri, operation.Expression, operation.State));
            return InlineClosures(c => c.CreateCommand(CommandVerb.New, CommandNoun.StreamFactory, commandText).ExecuteAsync(CancellationToken.None).Wait());
        }

        protected override Expression<Action<IReactiveServiceConnection>> VisitDefineSubscriptionFactory(DefineSubscriptionFactory operation)
        {
            Debug.Assert(operation != null);
            var commandText = _commandTextFactory.CreateNewText(new NewCommandData<Expression>(operation.TargetObjectUri, operation.Expression, operation.State));
            return InlineClosures(c => c.CreateCommand(CommandVerb.New, CommandNoun.SubscriptionFactory, commandText).ExecuteAsync(CancellationToken.None).Wait());
        }

        protected override Expression<Action<IReactiveServiceConnection>> VisitDeleteStream(DeleteStream operation)
        {
            Debug.Assert(operation != null);
            var commandText = _commandTextFactory.CreateRemoveText(operation.TargetObjectUri);
            return InlineClosures(connection => connection.CreateCommand(CommandVerb.Remove, CommandNoun.Stream, commandText).ExecuteAsync(CancellationToken.None).Wait());
        }

        protected override Expression<Action<IReactiveServiceConnection>> VisitDeleteSubscription(DeleteSubscription operation)
        {
            Debug.Assert(operation != null);
            var commandText = _commandTextFactory.CreateRemoveText(operation.TargetObjectUri);
            return InlineClosures(connection => connection.CreateCommand(CommandVerb.Remove, CommandNoun.Subscription, commandText).ExecuteAsync(CancellationToken.None).Wait());
        }

        protected override Expression<Action<IReactiveServiceConnection>> VisitUndefineObservable(UndefineObservable operation)
        {
            Debug.Assert(operation != null);
            var commandText = _commandTextFactory.CreateRemoveText(operation.TargetObjectUri);
            return InlineClosures(connection => connection.CreateCommand(CommandVerb.Remove, CommandNoun.Observable, commandText).ExecuteAsync(CancellationToken.None).Wait());
        }

        protected override Expression<Action<IReactiveServiceConnection>> VisitUndefineObserver(UndefineObserver operation)
        {
            Debug.Assert(operation != null);
            var commandText = _commandTextFactory.CreateRemoveText(operation.TargetObjectUri);
            return InlineClosures(connection => connection.CreateCommand(CommandVerb.Remove, CommandNoun.Observer, commandText).ExecuteAsync(CancellationToken.None).Wait());
        }

        protected override Expression<Action<IReactiveServiceConnection>> VisitUndefineStreamFactory(UndefineStreamFactory operation)
        {
            Debug.Assert(operation != null);
            var commandText = _commandTextFactory.CreateRemoveText(operation.TargetObjectUri);
            return InlineClosures(connection => connection.CreateCommand(CommandVerb.Remove, CommandNoun.StreamFactory, commandText).ExecuteAsync(CancellationToken.None).Wait());
        }

        protected override Expression<Action<IReactiveServiceConnection>> VisitUndefineSubscriptionFactory(UndefineSubscriptionFactory operation)
        {
            Debug.Assert(operation != null);
            var commandText = _commandTextFactory.CreateRemoveText(operation.TargetObjectUri);
            return InlineClosures(connection => connection.CreateCommand(CommandVerb.Remove, CommandNoun.SubscriptionFactory, commandText).ExecuteAsync(CancellationToken.None).Wait());
        }

        #region Not Implemented

        protected override Expression<Action<IReactiveServiceConnection>> VisitCreateObserver(CreateObserver operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactiveServiceConnection>> VisitDeleteObservableMetadata(DeleteObservableMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactiveServiceConnection>> VisitDeleteObserverMetadata(DeleteObserverMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactiveServiceConnection>> VisitDeleteStreamFactoryMetadata(DeleteStreamFactoryMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactiveServiceConnection>> VisitDeleteSubscriptionFactoryMetadata(DeleteSubscriptionFactoryMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactiveServiceConnection>> VisitDeleteStreamMetadata(DeleteStreamMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactiveServiceConnection>> VisitDeleteSubscriptionMetadata(DeleteSubscriptionMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactiveServiceConnection>> VisitInsertObservableMetadata(InsertObservableMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactiveServiceConnection>> VisitInsertObserverMetadata(InsertObserverMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactiveServiceConnection>> VisitInsertStreamFactoryMetadata(InsertStreamFactoryMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactiveServiceConnection>> VisitInsertSubscriptionFactoryMetadata(InsertSubscriptionFactoryMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactiveServiceConnection>> VisitInsertStreamMetadata(InsertStreamMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactiveServiceConnection>> VisitInsertSubscriptionMetadata(InsertSubscriptionMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactiveServiceConnection>> VisitLookupObservableMetadata(LookupObservableMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactiveServiceConnection>> VisitLookupObserverMetadata(LookupObserverMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactiveServiceConnection>> VisitLookupStreamFactoryMetadata(LookupStreamFactoryMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactiveServiceConnection>> VisitLookupSubscriptionFactoryMetadata(LookupSubscriptionFactoryMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactiveServiceConnection>> VisitLookupStreamMetadata(LookupStreamMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactiveServiceConnection>> VisitLookupSubscriptionMetadata(LookupSubscriptionMetadata operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactiveServiceConnection>> VisitMetadataQuery(MetadataQuery operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactiveServiceConnection>> VisitObserverOnNextCore(ObserverOnNext operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactiveServiceConnection>> VisitObserverOnNextCore<T>(ObserverOnNext<T> operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactiveServiceConnection>> VisitObserverOnErrorCore(ObserverOnError operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactiveServiceConnection>> VisitObserverOnErrorCore<T>(ObserverOnError<T> operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactiveServiceConnection>> VisitObserverOnCompletedCore(ObserverOnCompleted operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactiveServiceConnection>> VisitObserverOnCompletedCore<T>(ObserverOnCompleted<T> operation)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Action<IReactiveServiceConnection>> VisitExtensions(ServiceOperation operation)
        {
            throw new NotImplementedException();
        }

        #endregion

        private static Expression<Action<IReactiveServiceConnection>> InlineClosures(Expression<Action<IReactiveServiceConnection>> lambda)
        {
            return lambda.InlineClosures();
        }
    }
}
