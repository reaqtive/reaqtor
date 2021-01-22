// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.QueryEngine
{
    internal class MockReactiveServiceContext : ReactiveServiceContext
    {
        public const string SubscribeUri = Constants.SubscribeUri;
        public const string ReliableSubscribeUri = "rx://reliableobservable/subscribe";

        public const string SubjectUri = "rx://subject/create";
        public const string InputUri = "rx://subject/input";
        public const string OutputUri = "rx://subject/output";

        public const string ObservableId = "rx:/observable/id";
        public const string ObserverId = "rx:/observer/id";
        public const string SubscribableId = "rx:/subscribable/id";

        private readonly ReactiveMetadataBase _metadata;

        public MockReactiveServiceContext(IReactiveEngineProvider provider)
            : base(new ExpressionServices(), provider)
        {
        }

        public MockReactiveServiceContext(IReactiveEngineProvider provider, ReactiveMetadataBase metadata)
            : this(provider)

        {
            _metadata = metadata;
        }

        protected override ReactiveMetadataBase Metadata => _metadata ?? base.Metadata;

        private sealed class ExpressionServices : ReactiveExpressionServices
        {
            public ExpressionServices()
                : base(typeof(IReactiveClient))
            {
            }
        }
    }
}
