// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this code in the Remoting sample's client library.
// HvR - July 2026 - Factored out of the archived Remoting sample (see #158).
//

using System.Runtime.CompilerServices;

using Nuqleon.DataModel.TypeSystem;

namespace Reaqtor;

/// <summary>
/// Provides helper methods used to enforce data model constraints on used CLR types.
/// </summary>
internal static class DataModelHelpers
{
    private static readonly ConditionalWeakTable<Type, StrongBox<bool>> s_isDataModelTypeCache = [];

    /// <summary>
    /// Determines whether the specified type is a valid data model type.
    /// </summary>
    /// <param name="type">The type to check for data model type validity.</param>
    /// <returns>
    ///   <c>true</c> if the specified type is a valid data model type; otherwise, <c>false</c>.
    /// </returns>
    internal static bool IsDataModelType(Type type)
    {
        return s_isDataModelTypeCache.GetValue(type, t =>
        {
            return new StrongBox<bool>(DataType.TryCheck(t, out _));
        }).Value;
    }
}
