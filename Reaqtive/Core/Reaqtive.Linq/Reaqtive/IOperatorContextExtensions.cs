// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;

namespace Reaqtive
{
    /// <summary>
    /// Provides a set of extension methods for IOperatorContext.
    /// </summary>
    internal static class IOperatorContextExtensions
    {
        /// <summary>
        /// Tries to retrieve an Int32 setting from the context. The setting value is checked to be strictly greater than zero. If it's not, or the setting wasn't found, int.MaxValue is used.
        /// </summary>
        /// <param name="context">The operator context to get a setting from.</param>
        /// <param name="key">The key of the setting to retrieve.</param>
        /// <param name="value">Resulting value of the setting. This could be int.MaxValue if the setting was not found or the setting value was less than or equal to zero.</param>
        /// <returns>true if a valid setting was found; otherwise, false.</returns>
        public static bool TryGetInt32CheckGreaterThanZeroOrUseMaxValue(this IOperatorContext context, string key, out int value)
        {
            return TryGetInt32(context, key, x => x > 0, int.MaxValue, out value, "The value must be larger than zero.");
        }

        /// <summary>
        /// Tries to retrieve an Int32 setting from the context.
        /// </summary>
        /// <param name="context">The operator context to get a setting from.</param>
        /// <param name="key">The key of the setting to retrieve.</param>
        /// <param name="check">Function to check the obtained setting value. If this check doesn't pass for the retrieved setting value, the specified <paramref name="defaultValue"/> is used.</param>
        /// <param name="defaultValue">Default setting value to use if the <paramref name="check"/> doesn't pass or no setting value is found.</param>
        /// <param name="value">Resulting value of the setting. This could be the <paramref name="defaultValue"/> if the setting was not found or the <paramref name="check"/> didn't pass.</param>
        /// <param name="message">Message to supply when logging an error for an invalid setting, as determined by <paramref name="check"/>.</param>
        /// <returns>true if a valid setting was found; otherwise, false.</returns>
        public static bool TryGetInt32(this IOperatorContext context, string key, Func<int, bool> check, int defaultValue, out int value, string message = "")
        {
            var success = false;

            if (context.TryGetElement(key, out int res))
            {
                success = check(res);

                if (!success)
                {
                    var trace = context.TraceSource;
                    trace?.Invalid_Setting(key, res.ToString(CultureInfo.InvariantCulture), context.InstanceId, defaultValue.ToString(CultureInfo.InvariantCulture), message);
                }
            }

            value = success ? res : defaultValue;
            return success;
        }
    }
}
