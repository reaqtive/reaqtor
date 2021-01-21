// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Central place to keep well-known constants that are used by the engine.
    /// </summary>
    internal static class QueryEngineConstants
    {
        public const string SubscribeUri = Constants.SubscribeUri;
        public const string ReliableSubscribeUri = "rx://reliableobservable/subscribe";

        public const string InnerSubjectUri = "rx://subject/inner";
        public const string InnerSubjectRefCountUri = "rx://subject/inner/refCount";
        public const string InputUri = "rx://subject/input";
        public const string OutputUri = "rx://subject/output";
        public const string BridgeUri = "rx://subject/bridge";

        //
        // NB: These are historical remnants prior to using rx://builtin/id. They're kept to deal with old state.
        //     Don't remove these; state recovery can (and will) fail. They're harmless and innocent.
        //

        public const string ObservableId = "rx:/observable/id";
        public const string QbservableId = "rx:/qbservable/id";
        public const string SubscribableId = "rx:/subscribable/id";

        public const string ObserverId = "rx:/observer/id";

        public const string ParentUri = "__ParentUri";
    }
}
