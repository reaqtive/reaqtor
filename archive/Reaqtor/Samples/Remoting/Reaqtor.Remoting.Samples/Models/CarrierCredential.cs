// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

namespace Reaqtor.Remoting.Samples.Models
{
    using Nuqleon.DataModel;

    public class CarrierCredential
    {
        [Mapping("bing://reactiveprocessingentity/real/carriercredential/accesskey")]
        public string AccessKey { get; set; }

        [Mapping("bing://reactiveprocessingentity/real/carriercredential/meternumber")]
        public string MeterNumber { get; set; }

        [Mapping("bing://reactiveprocessingentity/real/carriercredential/accountnumber")]
        public string AccountNumber { get; set; }

        [Mapping("bing://reactiveprocessingentity/real/carriercredential/password")]
        public string Password { get; set; }
    }
}
