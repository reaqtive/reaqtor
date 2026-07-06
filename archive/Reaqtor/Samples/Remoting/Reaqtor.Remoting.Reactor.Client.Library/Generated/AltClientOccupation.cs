// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Nuqleon.DataModel;

namespace Reaqtor.Remoting.Reactor.Client
{
    /// <summary>
    /// Enumeration of occupations.
    /// </summary>
    public enum AltClientOccupation
    {
        [Mapping("reactor://platform.bing.com/observables/sample/person/occupation/unemployed")]
        Unemployed = 0,

        [Mapping("reactor://platform.bing.com/observables/sample/person/occupation/ceo")]
        ChiefExecutiveOfficer = 1,

        [Mapping("reactor://platform.bing.com/observables/sample/person/occupation/other")]
        Other = 2
    }
}
