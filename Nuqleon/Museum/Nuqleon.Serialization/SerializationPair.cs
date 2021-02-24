// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2012
//

using System;

namespace Nuqleon.Serialization
{
    /// <summary>
    /// Pair of serialization and deserialization functions.
    /// </summary>
    /// <typeparam name="TInput">Type of the input to the serializer.</typeparam>
    /// <typeparam name="TOutput">Type of the output of the serializer.</typeparam>
    /// <typeparam name="TContext">Type of the context object that's threaded through serialization and deserialization functions.</typeparam>
    /// <typeparam name="T">Type of the input to the serializer. Needs to derive from TInput.</typeparam>
    internal sealed class SerializationPair<TInput, TOutput, TContext, T>
        where T : TInput
    {
        /// <summary>
        /// Creates a serialization/deserialization function pair.
        /// </summary>
        /// <param name="serialize">Serialization function.</param>
        /// <param name="deserialize">Deserialization function.</param>
        public SerializationPair(Func<T, Func<TInput, TContext, TOutput>, TContext, TOutput> serialize, Func<TOutput, Func<TOutput, TContext, TInput>, TContext, T> deserialize)
        {
            Serialize = serialize;
            Deserialize = deserialize;
        }

        /// <summary>
        /// Gets the serialization function.
        /// </summary>
        public Func<T, Func<TInput, TContext, TOutput>, TContext, TOutput> Serialize { get; }

        /// <summary>
        /// Gets the deserialization function.
        /// </summary>
        public Func<TOutput, Func<TOutput, TContext, TInput>, TContext, T> Deserialize { get; }
    }
}
