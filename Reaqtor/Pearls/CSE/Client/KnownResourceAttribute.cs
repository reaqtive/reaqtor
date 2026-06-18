// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
// Ad-hoc clone of KnownResourceAttribute from IRP.
//
// Can be replaced by using the existing library.
//
// BD - September 2014
//

using System;

namespace Pearls.Reaqtor.CSE
{
    /// <summary>
    /// Attribute to annotate known resources.
    /// </summary>
    /// <remarks>
    /// Creates a new known resource attribute using the specified resource identifier.
    /// </remarks>
    /// <param name="id">Identifier of the known resource.</param>
    [AttributeUsage(AttributeTargets.Method)]
    internal sealed class KnownResourceAttribute(string id) : Attribute
    {

        /// <summary>
        /// Gets the identifier of the known resource.
        /// </summary>
        public string Id { get; } = id;
    }
}
