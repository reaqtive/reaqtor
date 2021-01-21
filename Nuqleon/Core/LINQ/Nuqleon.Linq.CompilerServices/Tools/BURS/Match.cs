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
    internal class Match
    {
        public Match(int state) => State = state;

        public int State { get; }

        public override string ToString() => State.ToString(CultureInfo.InvariantCulture) + " (0$)";

        public sealed class Wildcard : Match
        {
            public Wildcard(int state)
                : base(state)
            {
            }

            public override string ToString() => State.ToString(CultureInfo.InvariantCulture) + "*";
        }

        public sealed class Final : Match
        {
            public Final(int state, int cost)
                : base(state)
            {
                Cost = cost;
            }

            public int Cost { get; }

            public override string ToString() => State.ToString(CultureInfo.InvariantCulture) + "! (" + Cost.ToString(CultureInfo.InvariantCulture) + "$)";
        }
    }
}
