// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using System.Globalization;
using System.IO;

namespace Reaqtor.Remoting.Platform
{
    public static class Helpers
    {
        public static string GetExecutablePath(string component)
        {
            var executablePath = "Reaqtor.Remoting." + component + ".exe";
            if (!File.Exists(executablePath))
            {
#if DEBUG
                executablePath = string.Format(CultureInfo.InvariantCulture, @"..\..\..\Reaqtor.Remoting.{0}\bin\Debug\Reaqtor.Remoting.{0}.exe", component);
#else
                executablePath = string.Format(CultureInfo.InvariantCulture, @"..\..\..\Reaqtor.Remoting.{0}\bin\Release\Reaqtor.Remoting.{0}.exe", component);
#endif
                if (!File.Exists(executablePath))
                {
                    throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, "Could not find executable at '{0}'.", executablePath));
                }
            }

            return executablePath;
        }

        public static string GetTcpEndpoint(string host, int port, string path)
        {
            return string.Format(CultureInfo.InvariantCulture, "tcp://{0}:{1}/{2}", host, port, path);
        }

        public static class Constants
        {
            public const string Host = "127.0.0.1";
            public const int QueryCoordinatorPort = 8080;
            public const string QueryCoordinatorUri = "QueryCoordinator";
            public const int MetadataPort = 8081;
            public const string MetadataUri = "Metadata";
            public const int QueryEvaluatorPort = 8082;
            public const string QueryEvaluatorUri = "QueryEvaluator";
            public const int MessagingPort = 8083;
            public const string MessagingUri = "Messaging";
            public const int StateStorePort = 8084;
            public const string StateStoreUri = "StateStore";
            public const int PlaybackStorePort = 8085;
            public const string PlaybackStoreUri = "PlaybackStore";
            public const int KeyValueStorePort = 8086;
            public const string KeyValueStoreUri = "KeyValueStore";
        }
    }
}
