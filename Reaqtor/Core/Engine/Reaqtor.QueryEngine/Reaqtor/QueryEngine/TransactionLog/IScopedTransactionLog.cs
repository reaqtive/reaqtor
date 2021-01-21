// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.QueryEngine
{
    /// <summary>
    /// Interface to denote a scope in a transaction log (returned when a scope is entered).
    /// </summary>
    internal interface IScopedTransactionLog
    {
        void Clear();
    }
}
