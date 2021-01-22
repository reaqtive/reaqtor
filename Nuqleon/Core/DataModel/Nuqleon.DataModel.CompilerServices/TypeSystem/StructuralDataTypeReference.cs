// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
//

using System;
using System.Collections.Generic;

namespace Nuqleon.DataModel.TypeSystem
{
    internal sealed class StructuralDataTypeReference : StructuralDataType
    {
        public StructuralDataTypeReference(Type type, StructuralDataTypeKinds kind)
            : this(type, new List<DataProperty>(), kind)
        {
        }

        /// <summary>
        /// Constructor used to establish a mutable structural data type. Note
        /// the use of `AsReadOnly()` in the call to the base constructor. This
        /// usage relies on the fact that the underlying collection of the
        /// ReadOnlyCollection that is generated will still be the original
        /// list. Using that fact, we can expose the ReadOnlyCollection in the
        /// base class, while exposing the mutable list in this builder class.
        /// </summary>
        /// <param name="type">The original type.</param>
        /// <param name="properties">The mutable property collection.</param>
        /// <param name="kind">The structural data type kind.</param>
        private StructuralDataTypeReference(Type type, List<DataProperty> properties, StructuralDataTypeKinds kind)
            : base(type, properties.AsReadOnly(), kind)
        {
            Properties = properties;
        }

        public new List<DataProperty> Properties { get; private set; }

        public void Freeze() => Properties = null;
    }
}
