// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - October 2013 - Created this file.
//

using System;

namespace Reaqtor
{
    /// <summary>
    /// Provides a set of convenience helpers to deal with URIs.
    /// </summary>
    public static class UriHelper
    {
        /// <summary>
        /// Returns the canonical string representation of the specified URI.
        /// </summary>
        /// <param name="uri">URI to get a canonical string representation for.</param>
        /// <returns>Canonical string representation of the given URI.</returns>
        public static string ToCanonicalString(this Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            return uri.AbsoluteUri;
        }
    }
}
