// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1034 // Do not nest types. (By design for collection of constants.)
#pragma warning disable CA1720 // Identifier 'String' contains a type name.

using System;

namespace Reaqtor.ReificationFramework
{
    /// <summary>
    /// Constants related to reification.
    /// </summary>
    public static class ReificationConstants
    {
        /// <summary>
        /// A wildcard identifier for streams.
        /// </summary>
        public static class Firehose
        {
            /// <summary>The resource id as a string.</summary>
            public const string String = "rx://reification/firehose";

            /// <summary>The resource id as a URI.</summary>
            public static readonly Uri Uri = new(String);
        }

        /// <summary>
        /// A wildcard identifier for parameterized observable sources.
        /// </summary>
        public static class ParameterizedSource
        {
            /// <summary>The resource id as a string.</summary>
            public const string String = "rx://reification/source/param";

            /// <summary>The resource id as a URI.</summary>
            public static readonly Uri Uri = new(String);
        }

        /// <summary>
        /// A wildcard identifier for observer sinks.
        /// </summary>
        public static class Sink
        {
            /// <summary>The resource id as a string.</summary>
            public const string String = "rx://reification/sink";

            /// <summary>The resource id as a URI.</summary>
            public static readonly Uri Uri = new(String);
        }

        /// <summary>
        /// A wildcard identifier for observable sources.
        /// </summary>
        public static class Source
        {
            /// <summary>The resource id as a string.</summary>
            public const string String = "rx://reification/source";

            /// <summary>The resource id as a URI.</summary>
            public static readonly Uri Uri = new(String);
        }

        /// <summary>
        /// A wildcard identifier for extrinsic identities.
        /// </summary>
        public static class Wildcard
        {
            /// <summary>The resource id as a string.</summary>
            public const string String = "rx://reification/wildcard";

            /// <summary>The resource id as a URI.</summary>
            public static readonly Uri Uri = new(String);
        }

        /// <summary>
        /// Get a unique wildcard URI.
        /// </summary>
        /// <returns>A unique wildcard URI.</returns>
        public static Uri GetWildcard()
        {
            return new Uri(Wildcard.String + "/" + Guid.NewGuid());
        }

        internal static bool IsWildcard(this Uri uri)
        {
            return uri.ToCanonicalString().StartsWith(Wildcard.String, StringComparison.Ordinal);
        }
    }
}
