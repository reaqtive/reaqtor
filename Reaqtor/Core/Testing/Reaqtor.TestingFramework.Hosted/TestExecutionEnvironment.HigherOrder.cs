// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtive;

using Reaqtor.Reactive;

namespace Reaqtor.TestingFramework
{
    public partial class TestExecutionEnvironment
    {
        private HigherOrderExecutionEnvironment _higherOrder;

        private IHigherOrderExecutionEnvironment HigherOrder => _higherOrder ??= new HigherOrderExecutionEnvironment(this, Reactive);

        public ISubscription CreateBridge<T>(ISubscribable<T> subscribable, IObserver<T> observer, IOperatorContext parent) => HigherOrder.CreateBridge<T>(subscribable, observer, parent);

        public ISubscription LoadBridge<T>(IOperatorStateReader reader, IObserver<T> observer, IOperatorContext parent) => HigherOrder.LoadBridge<T>(reader, observer, parent);

        public void SaveBridge(ISubscription subscription, IOperatorStateWriter writer, IOperatorContext parent) => HigherOrder.SaveBridge(subscription, writer, parent);

        public IMultiSubject<T, T> CreateSimpleSubject<T>(Uri uri, IOperatorContext parent) => HigherOrder.CreateSimpleSubject<T>(uri, parent);

        public IMultiSubject<T, T> CreateRefCountSubject<T>(Uri uri, Uri tollbooth, Uri collector, IOperatorContext parent) => HigherOrder.CreateRefCountSubject<T>(uri, tollbooth, collector, parent);

        public void DeleteSubject<T>(Uri uri, IOperatorContext parent) => HigherOrder.DeleteSubject<T>(uri, parent);

        public IGroupedSubscribable<TKey, TElement> Quote<TKey, TElement>(IGroupedMultiSubject<TKey, TElement> subject, Uri uri) => HigherOrder.Quote(subject, uri);

        public ISubscribable<T> Quote<T>(IMultiSubject<T, T> subject, Uri uri) => HigherOrder.Quote(subject, uri);
    }
}
