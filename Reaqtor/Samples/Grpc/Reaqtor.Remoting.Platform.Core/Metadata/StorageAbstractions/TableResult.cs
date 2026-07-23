// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.Metadata;

/// <summary>
/// The result of executing an <see cref="ITableOperation"/> against an <see cref="ITable"/>.
/// </summary>
/// <remarks>
/// Cosmos-free replacement for <c>Microsoft.Azure.Cosmos.Table.TableResult</c> (see plan §2.6), carrying only
/// the members the transport-neutral storage layer sets.
/// </remarks>
public sealed class TableResult
{
    /// <summary>Gets or sets the ETag of the result.</summary>
    public string Etag { get; set; }

    /// <summary>Gets or sets the HTTP status code of the result.</summary>
    public int HttpStatusCode { get; set; }

    /// <summary>Gets or sets the result object, if any.</summary>
    public object Result { get; set; }
}
