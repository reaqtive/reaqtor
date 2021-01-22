// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.Client
{
    /// <summary>
    /// Constant definitions that must be exactly the same on both the client 
    /// and the service. These are just for the Rx operators.
    /// </summary>
    public static partial class Constants
    {
        public static partial class Observable
        {
            public static class Any
            {
                public const string NoArgument = "rx://operators/any";
                public const string Predicate = "rx://operators/any/predicate";
            }

            public static class All
            {
                public const string Predicate = "rx://operators/all/predicate";
            }

            public static class IsEmpty
            {
                public const string NoArgument = "rx://operators/isEmpty";
            }

            public static class Count
            {
                public const string NoArgument = "rx://operators/count";
                public const string Predicate = "rx://operators/count/predicate";
            }

            public static class LongCount
            {
                public const string NoArgument = "rx://operators/count/long";
                public const string Predicate = "rx://operators/count/long/predicate";
            }

            public static class Aggregate
            {
                public const string Accumulate = "rx://operators/aggregate";
                public const string Seed = "rx://operators/aggregate/seed";
                public const string SeedResult = "rx://operators/aggregate/seed/result";
            }

            public static class ToList
            {
                public const string NoArgument = "rx://operators/toList";
            }

            public static class Scan
            {
                public const string Accumulate = "rx://operators/scan";
                public const string Seed = "rx://operators/scan/seed";
            }

            public static class Return
            {
                public const string Value = "rx://operators/return";
            }

            public static class Throw
            {
                public const string Error = "rx://operators/throw";
            }

            public static class Never
            {
                public const string NoArgument = "rx://operators/never";
            }
        }
    }
}
