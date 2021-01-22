// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Reaqtor.Remoting.Reactor
{
    internal static class HttpVerbExtensions
    {
        /// <summary>
        /// A dictionary mapping http verbs to strings
        /// </summary>
        private static readonly Lazy<IDictionary<HttpVerb, string>> HttpVerbToString = new(() =>
        {
            var dictionary = EnumDictionary.Create<HttpVerb, string>();

            foreach (HttpVerb httpVerb in Enum.GetValues(typeof(HttpVerb)))
            {
                // This will throw on duplicates by design
                dictionary.Add(httpVerb, httpVerb.ToString().ToUpper());
            }

            dictionary.Remove(HttpVerb.Unknown);

            return dictionary;
        });

        public static string AsString(this HttpVerb httpVerb)
        {
            return HttpVerbToString.Value[httpVerb];
        }

        public static bool AsString(this HttpVerb httpVerb, out string result)
        {
            return HttpVerbToString.Value.TryGetValue(httpVerb, out result);
        }
    }
}
