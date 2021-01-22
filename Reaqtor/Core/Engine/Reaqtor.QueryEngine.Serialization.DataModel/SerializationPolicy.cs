// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.IO;

using Reaqtive.Expressions;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Serialization policy, providing a central location to obtain <see cref="ISerializer"/> instances, for example
    /// when serializing state during a checkpoint, or when deserializing state from a checkpoint where the persisted
    /// state contains a name and version for the serializer that was used to write the state.
    /// </summary>
    public sealed class SerializationPolicy : ISerializationPolicy
    {
        private readonly ConcurrentDictionary<Version, ISerializer> _serializers = new();
        private readonly IExpressionPolicy _expressionPolicy;

        /// <summary>
        /// Creates a serialization policy instance with a specified policy on how to handle expression trees, and a
        /// default version of the serializer to use.
        /// </summary>
        /// <param name="expressionPolicy">The policy for serializing expressions.</param>
        /// <param name="defaultVersion">The default version used for new serialization operations.</param>
        public SerializationPolicy(IExpressionPolicy expressionPolicy, Version defaultVersion)
        {
            _expressionPolicy = expressionPolicy;
            DefaultVersion = defaultVersion;
        }

        internal Version DefaultVersion { get; }

        //
        // NB: This version oscillates in different source code branches of the product, based on roll out of various
        //     serializers (e.g. encrypting and checksummed state persistence for compliant environments). We could
        //     consider making this a piece of configuration and pluggable by dynamic discovery of a "primary"
        //     serializer assembly installed alongside the engine. However, for most hosted environments, a single
        //     hardcoded serializer suffices (note that old serializers are kept in place as well to support
        //     deserialization of old state).
        //

        /// <summary>
        /// Default version of the serialization policy.
        /// </summary>
        public static ISerializationPolicy Default { get; } = new SerializationPolicy(DefaultExpressionPolicy.Instance, SerializerVersioning.v1);

        /// <summary>
        /// Gets a serializer with the specified <paramref name="name"/> and <paramref name="version"/>. This is used
        /// when recovering from a checkpoint, where serializer names and versions are stored in order to locate the
        /// right (de)serializer to use for reading the state.
        /// </summary>
        /// <param name="name">The name of the serializer.</param>
        /// <param name="version">The version of the serializer.</param>
        /// <returns>The serializer instance.</returns>
        public ISerializer GetSerializer(string name, Version version)
        {
            if (name != "JSON")
                throw new InvalidDataException(string.Format(CultureInfo.InvariantCulture, "Checkpoint state serializer '{0}' with version {1} is not supported.", name, version));

            // NB: Removed support for other formats (proprietary and others having big dependency graphs) in OSS release.

            return _serializers.GetOrAdd(version, v => new Serializer(_expressionPolicy, v));
        }

        /// <summary>
        /// Gets the default serializer. Useful for serialization of new state using the "latest known good" serializer.
        /// </summary>
        /// <returns>The serializer instance.</returns>
        public ISerializer GetSerializer()
        {
            return _serializers.GetOrAdd(DefaultVersion, v => new Serializer(_expressionPolicy, v));
        }

        //
        // CONSIDER: Port the pearls functionality for stream wrappers and support defining a chain of transformers rather
        //           than just serializers. E.g.
        //
        //             T -(serialize)-> byte[] -(compress)-> byte[] -(encrypt)-> byte[] -(checksum)-> byte[] -(base64)-> string
        //
        //           where all transformers are represented as triplets of (name, version, params). For example, encryption
        //           may refer to a key to use (if there's no global key). As such, a transformer chain could simple be persisted
        //           as an array of data model entities that have the triplet as properties (assuming params is a string, which
        //           the instantiated component can further deserialize).
        //
        //           (Note that in the pearls, we've gone one step further and made this "chain" really into a LambdaExpression
        //           obtained through function composition, and we simply serialize the whole expression tree as a one-trick
        //           pony. While architecturally satisfying, it's quite verbose to keep on a per-entity state; in fact, the tree
        //           itself is often bigger than the state the transformation is applied to. A compact represetnation of the
        //           flat list of triplets, possibly using binary serialization, is more economic.)
        //
        //           Right now, we apply these extra transforms globally and outside the engine, in specialized implementations
        //           of IStateReader and IStateWriter. However, table names should be treated as opaque by such readers/writers,
        //           so it isn't the right place if transforms differ for various tables (or even individual entries within).
        //
        //           By moving this to the engine (and bringing a "policy engine" along with it, which can apply a global policy,
        //           a per-collection policy, or even infer a policy from entity state - e.g. PII info, stored secrets, etc.)
        //           we can have tighter control over these things.
        //
    }
}
