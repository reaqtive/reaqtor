// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.Samples.Models
{
    using Nuqleon.DataModel;

    public enum League
    {
        /// <summary>
        /// National Football league - Football
        /// </summary>
        [Mapping("reactor://platform.bing.com/sport/league/nfl")]
        NFL = 1,

        /// <summary>
        /// English Premier League - Soccer
        /// </summary>
        [Mapping("reactor://platform.bing.com/sport/league/epl")]
        EPL = 2,

        /// <summary>
        /// National Hockey League - Ice Hockey
        /// </summary>
        [Mapping("reactor://platform.bing.com/sport/league/nhl")]
        NHL = 3,

        /// <summary>
        /// National Basketball Association - Basketball
        /// </summary>
        [Mapping("reactor://platform.bing.com/sport/league/nba")]
        NBA = 4,

        /// <summary>
        /// Major League Baseball - Baseball
        /// </summary>
        [Mapping("reactor://platform.bing.com/sport/league/mlb")]
        MLB = 5,

        /// <summary>
        /// UEFA Champions League - Soccer
        /// </summary>
        [Mapping("reactor://platform.bing.com/sport/league/soccer_chlg")]
        Soccer_chlg = 6,

        /// <summary>
        /// UEFA Europa League - Soccer
        /// </summary>
        [Mapping("reactor://platform.bing.com/sport/league/soccer_uefa_europa")]
        Soccer_uefa_europa = 7
    }
}