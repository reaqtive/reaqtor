// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.Samples.Models
{
    using Nuqleon.DataModel;

    /// <summary>
    /// Data definition for the parameters to the msnjv alert parameterized Observable.
    /// </summary>
    public class MsnjvAlertParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MsnjvAlertParameters"/> class.
        /// </summary>
        public MsnjvAlertParameters()
        {
        }

        /// <summary>
        /// Gets or sets the domain for the msnjv alert
        /// </summary>
        [Mapping("bing://msnjvalert/domain")]
        public string Domain { get; set; }

        /// <summary>
        /// Gets or sets the scenario for the msnjv alert
        /// </summary>
        [Mapping("bing://msnjvalert/scenario")]
        public string Scenario { get; set; }

        /// <summary>
        /// Gets or sets the trigger query for the msnjv alert
        /// </summary>
        [Mapping("bing://msnjvalert/triggerquery")]
        public string TriggerQuery { get; set; }

        /// <summary>
        /// Gets or sets the answer key for the msnjv alert
        /// </summary>
        [Mapping("bing://msnjvalert/answerkey")]
        public string AnswerKey { get; set; }

        /// <summary>
        /// Gets or sets the market that the user is in. Mostly it is Zh-CN
        /// </summary>
        [Mapping("bing://msnjvalert/market")]
        public string Market { get; set; }

        /// <summary>
        /// Gets or sets the additional parameters for the msnjv alert
        /// </summary>
        [Mapping("bing://msnjvalert/additionalparams")]
        public string AdditionalParams { get; set; }

        /// <summary>
        /// Gets or sets the additional details for the msnjv alert
        /// </summary>
        [Mapping("bing://msnjvalert/additionaldetails")]
        public string AdditionalDetails { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format(
                "Domain: {0}, Scenario: {1}, TriggerQuery: {2}, AnswerKey: {3}, Market: {4}, AdditionalParams: {5}, AdditionalDetails: {6}",
                Domain,
                Scenario,
                TriggerQuery,
                AnswerKey,
                Market,
                AdditionalParams,
                AdditionalDetails);
        }
    }
}
