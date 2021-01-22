// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2013 - Created this file.
//

using System;

namespace Reaqtor.Remoting
{
    internal static class Json
    {
        /// <summary>
        /// Serializes an object to JSON.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        /// <param name="value">The object to serialize.</param>
        /// <returns>The JSON serialized representation of the object.</returns>
        [KnownResource(Platform.Constants.Identifiers.DataModelJsonSerializeV1)]
        public static string Serialize<TObject>(TObject value)
        {
            throw new NotImplementedException("This operator should only be used in expressions.");
        }

        /// <summary>
        /// Serializes an object to JSON.
        /// </summary>
        /// <typeparam name="TObject">The type of object.</typeparam>
        /// <param name="value">The object to serialize.</param>
        /// <returns>The JSON serialized representation of the object.</returns>
        [KnownResource(Platform.Constants.Identifiers.DataModelJsonSerializeV2)]
        public static string SerializeFast<TObject>(TObject value)
        {
            throw new NotImplementedException("This operator should only be used in expressions.");
        }
    }
}
