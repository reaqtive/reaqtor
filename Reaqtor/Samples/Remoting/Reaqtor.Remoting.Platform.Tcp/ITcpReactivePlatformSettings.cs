// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - November 2013 - Created this file.
//

namespace Reaqtor.Remoting.Platform
{
#pragma warning disable CA1056 // URI-like properties should not be strings. (Legacy approach; can be changed in the future.)
    public interface ITcpReactivePlatformSettings
    {
        string Host { get; }
        int QueryCoordinatorPort { get; }
        string QueryCoordinatorUri { get; }
        int MetadataPort { get; }
        string MetadataUri { get; }
        int QueryEvaluatorPort { get; }
        string QueryEvaluatorUri { get; }
        int MessagingPort { get; }
        string MessagingUri { get; }
        int StateStorePort { get; }
        string StateStoreUri { get; }
        int KeyValueStorePort { get; }
        string KeyValueStoreUri { get; }
        int PlaybackStorePort { get; }
        string PlaybackStoreUri { get; }
        string GetExecutablePath(string component);
    }
#pragma warning restore CA1056
}
