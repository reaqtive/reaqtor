// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Security.Cryptography;
using System.Text;

namespace Reaqtor.Remoting
{
    /// <summary>
    /// Helper for hashing algorithms.
    /// </summary>
    public static class HashingHelper
    {
        /// <summary>
        /// Computes the hash value of the data passed.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>The data passed in as a hashed value.</returns>
        public static string ComputeHash(string data)
        {
            if (string.IsNullOrEmpty(data))
                throw new ArgumentNullException(nameof(data));

            //
            // NB: The archived copy used `new SHA1CryptoServiceProvider().ComputeHash(...)`. The derived crypto
            //     type is obsolete on net10.0 (SYSLIB0021) and the instance ComputeHash is flagged by CA1850;
            //     this uses the static SHA1.HashData instead. The algorithm (SHA-1 of the UTF-8 bytes, rendered
            //     as upper-case hex without separators) is preserved.
            //
#pragma warning disable CA5350 // REVIEW: Do Not Use Weak Cryptographic Algorithms.
            byte[] hashbyte = SHA1.HashData(Encoding.UTF8.GetBytes(data));
#pragma warning restore CA5350

            string hash = Convert.ToHexString(hashbyte);

            return hash.ToUpperInvariant();
        }
    }
}
