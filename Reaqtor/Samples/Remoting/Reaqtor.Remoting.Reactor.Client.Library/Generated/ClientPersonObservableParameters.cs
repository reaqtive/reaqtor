// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Nuqleon.DataModel;

namespace Reaqtor.Remoting.Reactor.Client
{
    public class ClientPersonObservableParameters
    {
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        [Mapping("reactor://platform.bing.com/observables/sample/person/params/firstname")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        [Mapping("reactor://platform.bing.com/observables/sample/person/params/lastname")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the age.
        /// </summary>
        [Mapping("reactor://platform.bing.com/observables/sample/person/params/age")]
        public int Age { get; set; }

        /// <summary>
        /// Gets or sets the occupation.
        /// </summary>
        [Mapping("reactor://platform.bing.com/observables/sample/person/params/occupation")]
        public ClientOccupation Occupation { get; set; }
    }
}
