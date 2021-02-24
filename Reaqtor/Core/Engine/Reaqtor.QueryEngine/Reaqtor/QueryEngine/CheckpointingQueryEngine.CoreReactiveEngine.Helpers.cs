// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Reaqtor.QueryEngine
{
    public partial class CheckpointingQueryEngine
    {
        private partial class CoreReactiveEngine
        {
            private static void FailSafe(Action a, List<Exception> errors)
            {
#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1031 // Do not catch general exception types. (Purpose of Safe variant.)

                try
                {
                    a();
                }
                catch (Exception ex)
                {
                    errors.Add(ex);
                }

#pragma warning restore CA1031
#pragma warning restore IDE0079
            }

            private static bool TryParse(string pattern, out Regex regex)
            {
                try
                {
                    regex = new Regex(pattern, RegexOptions.Compiled);
                    return true;
                }
                catch (ArgumentException)
                {
                    regex = null;
                    return false;
                }
            }
        }
    }
}
