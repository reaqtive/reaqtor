// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtive;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Subject that allows for distributed reference counting. Used by operators like GroupBy and Window.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <remarks>
    /// For more information, see Reaqtor\Nuqleon\Reactive\HighOrderOutputStatefulOperator.cs.
    /// </remarks>
    internal sealed class RefCountSubject<T> : InnerSubject<T>
    {
        private readonly Uri _refCountStreamUri;
        private readonly Uri _collectorStreamUri;

        private IOperatorContext _context;

        private IObserver<bool> _refCount;
        private IObserver<Uri> _collector;

        public RefCountSubject(Uri refCountUri, Uri collectorUri)
        {
            _refCountStreamUri = refCountUri;
            _collectorStreamUri = collectorUri;
        }

        public override void SetContext(IOperatorContext context)
        {
            base.SetContext(context);

            _context = context;
        }

        public override void Start()
        {
            base.Start();

            var refCountStream = _context.ExecutionEnvironment.GetSubject<bool, bool>(_refCountStreamUri);
            _refCount = refCountStream.CreateObserver();

            var collectorStream = _context.ExecutionEnvironment.GetSubject<Uri, Uri>(_collectorStreamUri);
            _collector = collectorStream.CreateObserver();
        }

        protected override void OnSubscriptionCreated()
        {
            base.OnSubscriptionCreated();

            // No subscriptions can come in after recovery due to sealing. They
            // can come in during recovery, but the parent already keeps this
            // number, so don't reannounce the AddRef operations.
            if (!Recovered)
            {
                _refCount.OnNext(true);
            }
        }

        protected override void OnSubscriptionDisposed()
        {
            base.OnSubscriptionDisposed();

            _refCount.OnNext(false);
        }

        protected override void OnDeleted()
        {
            base.OnDeleted();

            _collector.OnNext(_context.InstanceId);
        }
    }
}
