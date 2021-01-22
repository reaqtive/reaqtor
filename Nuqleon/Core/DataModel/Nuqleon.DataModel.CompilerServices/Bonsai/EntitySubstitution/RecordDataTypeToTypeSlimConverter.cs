// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD, ER - November 2013 - Created this file.
//

using System.Linq.CompilerServices;
using System.Reflection;

namespace Nuqleon.DataModel.CompilerServices.Bonsai
{
    internal sealed class RecordDataTypeToTypeSlimConverter : DataTypeToTypeSlimConverter
    {
        protected override StructuralTypeSlimReference GetTypeSlimBuilder() => StructuralTypeSlimReference.Create(hasValueEqualitySemantics: true, StructuralTypeSlimKind.Record);

        protected override void AddProperty(StructuralTypeSlimReference builder, PropertyDataSlim property)
        {
            var propertyInfo = builder.GetProperty(property.Name, property.Type, EmptyReadOnlyCollection<TypeSlim>.Instance, canWrite: true);
            builder.AddProperty(propertyInfo);
        }
    }
}
