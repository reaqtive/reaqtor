﻿// Licensed to the .NET Foundation under one or more agreements.
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

#pragma warning disable CA5350 // REVIEW: Do Not Use Weak Cryptographic Algorithms.
            using SHA1 sha = new SHA1CryptoServiceProvider();
#pragma warning restore CA5350

            byte[] hashbyte = sha.ComputeHash(Encoding.UTF8.GetBytes(data));

            string hash = BitConverter.ToString(hashbyte).Replace("-", string.Empty);

            return hash.ToUpperInvariant();
        }
    }
}
