// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.Samples.Models
{
    using System;

    using Nuqleon.DataModel;

    /// <summary>
    /// Content that is used to trigger an alert about sports team based game
    /// </summary>
    public class TeamBasedGameAlert
    {
        /// <summary>
        /// League for which the game is being played
        /// </summary>
        [Mapping("reactor://platform.bing.com/sport/league")]
        public League League { get; set; }

        /// <summary>
        /// Identifier of team that is hosting the game
        /// </summary>
        [Mapping("reactor://platform.bing.com/sport/hometeamid")]
        public string HomeTeamId { get; set; }

        /// <summary>
        /// Alias used to represent home team
        /// </summary>
        [Mapping("reactor://platform.bing.com/sport/hometeamalias")]
        public string HomeTeamAlias { get; set; }

        /// <summary>
        /// Short Name of the team that is hosting the game
        /// </summary>
        [Mapping("reactor://platform.bing.com/sport/hometeamshortname")]
        public string HomeTeamShortName { get; set; }

        /// <summary>
        /// Name of the team that is hosting the game
        /// </summary>
        [Mapping("reactor://platform.bing.com/sport/hometeamname")]
        public string HomeTeamName { get; set; }

        /// <summary>
        /// Score of the team that is hosting the game
        /// </summary>
        [Mapping("reactor://platform.bing.com/sport/hometeamscore")]
        public string HomeTeamScore { get; set; }

        /// <summary>
        /// Outcome of the game for hometeam
        /// </summary>
        [Mapping("reactor://platform.bing.com/sport/hometeamoutcome")]
        public GameOutcome HomeTeamOutcome { get; set; }

        /// <summary>
        /// Identifier of team that is visiting
        /// </summary>
        [Mapping("reactor://platform.bing.com/sport/visitingteamid")]
        public string VisitingTeamId { get; set; }

        /// <summary>
        /// Alias used to represent visiting team
        /// </summary>
        [Mapping("reactor://platform.bing.com/sport/visitingteamalias")]
        public string VisitingTeamAlias { get; set; }

        /// <summary>
        /// Short Name of the team that is visiting
        /// </summary>
        [Mapping("reactor://platform.bing.com/sport/visitingteamshortname")]
        public string VisitingTeamShortName { get; set; }

        /// <summary>
        /// Name of the team that is visiting
        /// </summary>
        [Mapping("reactor://platform.bing.com/sport/visitingteamname")]
        public string VisitingTeamName { get; set; }

        /// <summary>
        /// Score of the team that is visiting
        /// </summary>
        [Mapping("reactor://platform.bing.com/sport/visitingteamscore")]
        public string VisitingTeamScore { get; set; }

        /// <summary>
        /// Outcome of the game for hometeam
        /// </summary>
        [Mapping("reactor://platform.bing.com/sport/visitingteamoutcome")]
        public GameOutcome VisitingTeamOutcome { get; set; }

        /// <summary>
        /// State of game when this alert is triggered
        /// </summary>
        [Mapping("reactor://platform.bing.com/sport/gamestate")]
        public GameState GameState { get; set; }

        /// <summary>
        /// Time when the game is going to start
        /// </summary>
        [Mapping("reactor://platform.bing.com/sport/gamestarttime")]
        public DateTime GameStartTime { get; set; }
    }
}
