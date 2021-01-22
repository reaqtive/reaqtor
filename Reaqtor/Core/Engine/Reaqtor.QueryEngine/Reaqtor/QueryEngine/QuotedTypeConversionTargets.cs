// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Used to specify mappings between quotation types and target types.
    /// </summary>
    public static class QuotedTypeConversionTargets
    {
        /// <summary>
        /// Gets an empty conversion map.
        /// </summary>
        /// <remarks>
        /// This is the default and is useful when operators are defined using `new` expressions rather than
        /// factory methods on some static class.
        /// </remarks>
        public static IQuotedTypeConversionTargets Empty { get; } = new Impl(new Dictionary<Type, Type>());

        /// <summary>
        /// Creates a conversion map from the specified dictionary.
        /// </summary>
        /// <param name="typeMap">Dictionary mapping quoted types to non-quoted types.</param>
        /// <returns>A new conversion map instance.</returns>
        public static IQuotedTypeConversionTargets From(IReadOnlyDictionary<Type, Type> typeMap)
            => new Impl(typeMap ?? throw new ArgumentNullException(nameof(typeMap)));

        private sealed class Impl : IQuotedTypeConversionTargets
        {
            public Impl(IReadOnlyDictionary<Type, Type> typeMap) => TypeMap = typeMap;

            public IReadOnlyDictionary<Type, Type> TypeMap { get; }
        }
    }
}
