// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.Remoting.Platform
{
    public class Constants
    {
        /// <summary>
        /// Declarations of string constants uses as keys in the operator context.
        /// </summary>
        public static class ContextKey
        {
            /// <summary>
            /// A key to use for storing the query evaluator name in the operator context.
            /// </summary>
            public const string Name = "QueryEvaluatorName";
        }

        /// <summary>
        /// Resource identifiers.
        /// </summary>
        public static class Identifiers
        {
            /// <summary>
            /// Identifier for custom function to generate language from an entity.
            /// </summary>
            public const string GenerateLanguage = "reactor://platform.bing.com/custom/generateLanguage";

            /// <summary>
            /// Identifier for custom function for checking if a number is prime.
            /// </summary>
            public const string IsPrime = "reactor://platform.bing.com/custom/isPrime";

            /// <summary>
            /// Identifier for custom function to serialize a data model object to JSON.
            /// </summary>
            public const string DataModelJsonSerializeV1 = "reactor://platform.bing.com/datamodel/json/serialize";

            /// <summary>
            /// Identifier for custom function to serialize a data model object to JSON.
            /// </summary>
            public const string DataModelJsonSerializeV2 = "reactor://platform.bing.com/datamodel/json/serialize/fast";

            /// <summary>
            /// Definition of identifiers of well-known observables.
            /// </summary>
            public static class Observable
            {
                /// <summary>
                /// The fire hose observable.
                /// </summary>
                public static class FireHose
                {
                    /// <summary>The resource id as a string.</summary>
                    public const string String = "reactor://platform.bing.com/observable/firehose";

                    /// <summary>The resource id as a URI.</summary>
                    public static readonly Uri Uri = new(String);
                }

                /// <summary>
                /// The garbage collector observable.
                /// </summary>
                public static class GarbageCollector
                {
                    /// <summary>The resource id as a string.</summary>
                    public const string String = "reactor://platform.bing.com/observable/garbageCollector";

                    /// <summary>The resource id as a URI.</summary>
                    public static readonly Uri Uri = new(String);
                }
            }

            /// <summary>
            /// Definition of identifiers of well-known observers.
            /// </summary>
            public static class Observer
            {
                /// <summary>
                /// The console observer.
                /// </summary>
                public static class ConsoleObserver
                {
                    /// <summary>The resource id as a string.</summary>
                    public const string String = "reactor://platform.bing.com/observer/console";

                    /// <summary>The resource id as a URI.</summary>
                    public static readonly Uri Uri = new(String);
                }

                /// <summary>
                /// The console observer.
                /// </summary>
                public static class ConsoleObserverParam
                {
                    /// <summary>The resource id as a string.</summary>
                    public const string String = "reactor://platform.bing.com/observer/console/param";

                    /// <summary>The resource id as a URI.</summary>
                    public static readonly Uri Uri = new(String);
                }

                /// <summary>
                /// The fire hose observer.
                /// </summary>
                public static class FireHose
                {
                    /// <summary>The resource id as a string.</summary>
                    public const string String = "reactor://platform.bing.com/observer/firehose";

                    /// <summary>The resource id as a URI.</summary>
                    public static readonly Uri Uri = new(String);
                }

                /// <summary>The Trace observer.</summary>
                public static class TraceObserver
                {
                    /// <summary>The resource id as a string.</summary>
                    public const string String = "reactor://platform.bing.com/observer/trace";

                    /// <summary>The resource id as a URI.</summary>
                    public static readonly Uri Uri = new(String);
                }

                /// <summary>The Trace observer.</summary>
                public static class TraceObserverParam
                {
                    /// <summary>The resource id as a string.</summary>
                    public const string String = "reactor://platform.bing.com/observer/trace/param";

                    /// <summary>The resource id as a URI.</summary>
                    public static readonly Uri Uri = new(String);
                }

                /// <summary>The Throughput observer.</summary>
                public static class Throughput
                {
                    /// <summary>The resource id as a string.</summary>
                    public const string String = "reactor://platform.bing.com/observer/throughput";

                    /// <summary>The resource id as a URI.</summary>
                    public static readonly Uri Uri = new(String);
                }
            }
        }
    }
}
