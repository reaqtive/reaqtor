// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - June 2014 - Created this file.
//

using System;
using System.Reflection;

namespace Nuqleon.DataModel.CompilerServices.Bonsai
{
    internal sealed class TypeCarryingTypeToTypeSlimConverter : TypeToTypeSlimConverter
    {
        public override TypeSlim Visit(Type type)
        {
            var result = base.Visit(type);
            result.CarryType(type);
            return result;
        }
    }
}
