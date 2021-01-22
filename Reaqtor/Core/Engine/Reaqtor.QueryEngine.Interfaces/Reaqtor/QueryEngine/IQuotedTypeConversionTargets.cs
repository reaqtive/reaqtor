// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Interface to specify mappings between quotation types and target types.
    /// </summary>
    /// <remarks>
    /// This interface was introduced to decouple query operator libraries from the core engine implementation,
    /// and to ensure compatibility with former versions of Nuqleon. In particular, the query engine does not
    /// have a static dependency on Reaqtor.Linq, which defines the Subscribable type. Query expressions
    /// can refer to ReactiveQbservable operators (or alternative libraries), which need to get mapped onto their
    /// concrete implementation, e.g. the Subscribable operators. This mapping can be defined here.
    /// </remarks>
    public interface IQuotedTypeConversionTargets
    {
        /// <summary>
        /// Gets a map from quoted types to unquoted types.
        /// </summary>
        IReadOnlyDictionary<Type, Type> TypeMap { get; }
    }
}
