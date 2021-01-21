// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.Samples.Models
{
    using Nuqleon.DataModel;

    public enum GameState
    {
        /// <summary>
        /// The game has not started
        /// </summary>
        [Mapping("reactor://platform.bing.com/sport/gamestate/pregame")]
        PreGame = 1,

        /// <summary>
        /// Game is currently in progress
        /// </summary>
        [Mapping("reactor://platform.bing.com/sport/gamestate/inprogress")]
        InProgress = 2,

        /// <summary>
        /// Break in game while in progress
        /// This could be a fixed break (innings change) or 
        /// forced break due to timeout or some other natural event
        /// </summary>
        [Mapping("reactor://platform.bing.com/sport/gamestate/inprogressbreak")]
        InProgressBreak = 3,

        /// <summary>
        /// Game is properly concluded with a result
        /// </summary>
        [Mapping("reactor://platform.bing.com/sport/gamestate/final")]
        Final = 4,

        /// <summary>
        /// Game is done without a result
        /// This could be due to abandonment or postponement
        /// </summary>
        [Mapping("reactor://platform.bing.com/sport/gamestate/noresult")]
        NoResult = 5,
    }
}