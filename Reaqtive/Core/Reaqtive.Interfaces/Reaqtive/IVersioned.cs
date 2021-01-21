// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtive
{
    /// <summary>
    /// Represents artifacts that support versioning.
    /// </summary>
    public interface IVersioned
    {
        /// <summary>
        /// Gets the name tag of the artifact, used to persist state headers.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the version of the artifact.
        /// </summary>
        Version Version { get; }
    }
}
