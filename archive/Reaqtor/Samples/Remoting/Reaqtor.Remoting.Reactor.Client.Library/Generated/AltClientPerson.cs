// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Nuqleon.DataModel;

namespace Reaqtor.Remoting.Reactor.Client
{
    public class AltClientPerson
    {
        /// <summary>
        /// Gets or sets the given name.
        /// </summary>
        [Mapping("reactor://platform.bing.com/observables/sample/person/firstname")]
        public string GivenName { get; set; }

        /// <summary>
        /// Gets or sets the family name.
        /// </summary>
        [Mapping("reactor://platform.bing.com/observables/sample/person/lastname")]
        public string FamilyName { get; set; }

        /// <summary>
        /// Ges or sets the years since birth.
        /// </summary>
        [Mapping("reactor://platform.bing.com/observables/sample/person/age")]
        public int YearsSinceBirth { get; set; }

        /// <summary>
        /// Gets or sets the occupation of this person.
        /// </summary>
        [Mapping("reactor://platform.bing.com/observables/sample/person/occupation")]
        public AltClientOccupation Occupation { get; set; }
    }
}
