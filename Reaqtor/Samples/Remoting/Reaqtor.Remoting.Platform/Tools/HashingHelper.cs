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
            Contract.RequiresNotNullOrEmpty(data);

            using SHA1 sha = new SHA1CryptoServiceProvider();

            byte[] hashbyte = sha.ComputeHash(Encoding.UTF8.GetBytes(data));

            string hash = BitConverter.ToString(hashbyte).Replace("-", string.Empty);

            return hash.ToLowerInvariant();
        }
    }
}
