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

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1008 // Add None value to enum.
#pragma warning disable CS1591 // XML docs missing for late bound accessor mirror image of System.Reflection.Emit functionality.

namespace Nuqleon.Reflection.Emit
{
    public enum AssemblyBuilderAccess
    {
        Run = 1,
        Save = 2,
        RunAndSave = 3,
        ReflectionOnly = 6,
        RunAndCollect = 9
    }
}
