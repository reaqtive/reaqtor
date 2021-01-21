// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtive
{
    /// <summary>
    /// Base class for a subject that supports multiple observers and has a key.
    /// </summary>
    /// <typeparam name="TKey">Type of the key associated with the subject.</typeparam>
    /// <typeparam name="TSource">Type of the elements processed by the subject.</typeparam>
    public abstract class GroupedMultiSubjectBase<TKey, TSource> : MultiSubjectBase<TSource>, IGroupedMultiSubject<TKey, TSource>
    {
        /// <summary>
        /// Gets the grouping key.
        /// </summary>
        public abstract TKey Key
        {
            get;
        }
    }

    // NB: This class is not exposed publicly because it has the sealing construct enabled.
    //     Alternatively, we could rely on a special subject in the reactive environment and move this to the engine.

    internal class GroupedMultiSubject<TKey, TSource> : GroupedMultiSubjectBase<TKey, TSource>, ISealable
    {
        private readonly IMultiSubject<TSource, TSource> _subject;
        private readonly TKey _key;

        public GroupedMultiSubject(IMultiSubject<TSource, TSource> subject, TKey key)
        {
            _subject = subject;
            _key = key;
        }

        public override TKey Key => _key;

        protected override IObserver<TSource> CreateObserverCore()
        {
            return _subject.CreateObserver();
        }

        protected override ISubscription SubscribeCore(IObserver<TSource> observer)
        {
            return _subject.Subscribe(observer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _subject.Dispose();
            }
        }

        public void Seal()
        {
            if (_subject is ISealable seal)
            {
                seal.Seal();
            }
        }
    }
}
