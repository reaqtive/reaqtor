// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.Samples.Models
{
    using Nuqleon.DataModel;

    /// <summary>
    /// List of possible outcomes of game.
    /// There is no enum having value 1. That has been left out for
    /// backward compatibility with our old backend
    /// </summary>
    public enum GameOutcome
    {
        /// <summary>
        /// The game outcome isnt known, which will happen for pre-game scneario
        /// </summary>
        [Mapping("reactor://platform.bing.com/sport/gameoutcome/unknown")]
        Unknown = 0,

        /// <summary>
        /// The game was won.
        /// </summary>
        [Mapping("reactor://platform.bing.com/sport/gameoutcome/win")]
        Win = 2,

        /// <summary>
        /// The game was lost.
        /// </summary>
        [Mapping("reactor://platform.bing.com/sport/gameoutcome/loss")]
        Loss = 3,

        /// <summary>
        /// The game tied.
        /// </summary>
        [Mapping("reactor://platform.bing.com/sport/gameoutcome/tie")]
        Tie = 4,

        /// <summary>
        /// One of the team/player withdrawn
        /// </summary>
        [Mapping("reactor://platform.bing.com/sport/gameoutcome/withdraw")]
        Withdraw = 5
    }
}
