// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtive
{
    /// <summary>
    /// Exposed version numbers of operator state.
    /// </summary>
    internal static class Versioning
    {
        //
        // PERF: When adding new version numbers for use by Reactive operators,
        //       update the GetVersion logic accordingly in order to return cached
        //       version instances whenever possible.
        //

        /// <summary>
        /// Version 1.0 of operator state.
        /// </summary>
        internal static readonly Version v1 = new(1, 0, 0, 0);

        /// <summary>
        /// Version 2.0 of operator state.
        /// </summary>
        internal static readonly Version v2 = new(2, 0, 0, 0);

        /// <summary>
        /// Version 3.0 of operator state.
        /// </summary>
        internal static readonly Version v3 = new(3, 0, 0, 0);

        /// <summary>
        /// Gets a <see cref="Version"/> instance with the specified version numbers.
        /// </summary>
        /// <param name="major">The major version number.</param>
        /// <param name="minor">The minor version number.</param>
        /// <param name="build">The build version number.</param>
        /// <param name="revision">The revision version number.</param>
        /// <returns>An instance of <see cref="Version"/> with the specified version numbers. The returned instance may be a shared copy.</returns>
        internal static Version GetVersion(int major, int minor, int build, int revision)
        {
            if (minor == 0 && build == 0 && revision == 0)
            {
                switch (major)
                {
                    case 1: return v1;
                    case 2: return v2;
                    case 3: return v3;
                }
            }

            return new Version(major, minor, build, revision);
        }
    }
}
