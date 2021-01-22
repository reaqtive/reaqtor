// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2018 - Created this file.
//

//
// NB: This file contains a late bound accessor for System.Reflection.Emit types which are unavailable on .NET Standard 2.0.
//

#pragma warning disable CS1591 // XML docs missing for late bound accessor mirror image of System.Reflection.Emit functionality.

namespace Nuqleon.Reflection.Emit
{
    public class LocalBuilder
    {
        internal readonly object _local;

        public LocalBuilder(object local) => _local = local;
    }
}
