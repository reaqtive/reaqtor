// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2011 - Created this file.
//

using System.Globalization;

namespace System.Linq.CompilerServices
{
    internal class Match(int state)
    {
        public int State { get; } = state;

        public override string ToString() => State.ToString(CultureInfo.InvariantCulture) + " (0$)";

        public sealed class Wildcard(int state) : Match(state)
        {
            public override string ToString() => State.ToString(CultureInfo.InvariantCulture) + "*";
        }

        public sealed class Final(int state, int cost) : Match(state)
        {
            public int Cost { get; } = cost;

            public override string ToString() => State.ToString(CultureInfo.InvariantCulture) + "! (" + Cost.ToString(CultureInfo.InvariantCulture) + "$)";
        }
    }
}
