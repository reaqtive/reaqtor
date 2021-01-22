// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

using System;
using System.Collections.ObjectModel;

namespace Nuqleon.DataModel.TypeSystem
{
    /// <summary>
    /// Represents a structural type, i.e. a type with properties typed with data model types.
    /// </summary>
    public class StructuralDataType : DataType
    {
        internal StructuralDataType(Type type, ReadOnlyCollection<DataProperty> properties, StructuralDataTypeKinds kind)
            : base(type)
        {
            Properties = properties;
            StructuralKind = kind;
        }

        /// <summary>
        /// Gets the kind of the data type.
        /// </summary>
        public override DataTypeKinds Kind => DataTypeKinds.Structural;

        /// <summary>
        /// Gets the properties of the structural type.
        /// </summary>
        public ReadOnlyCollection<DataProperty> Properties { get; }

        /// <summary>
        /// Gets the structural data type kind.
        /// </summary>
        public StructuralDataTypeKinds StructuralKind { get; }

        /// <summary>
        /// Creates a new instance of the structural data type.
        /// </summary>
        /// <param name="arguments">The components of the value, in the order of property declaration.</param>
        /// <returns>Instance of the structural data type.</returns>
        public override object CreateInstance(params object[] arguments)
        {
            if (arguments == null)
                throw new ArgumentNullException(nameof(arguments));

            // CONSIDER: We could use subclasses here.
            switch (StructuralKind)
            {
                case StructuralDataTypeKinds.Record:
                    {
                        if (arguments.Length != 0)
                            throw new InvalidOperationException("Record types need to be initialized through property setters.");

                        return Activator.CreateInstance(UnderlyingType);
                    }
                case StructuralDataTypeKinds.Tuple:
                case StructuralDataTypeKinds.Anonymous:
                    {
                        if (arguments.Length != Properties.Count)
                            throw new InvalidOperationException("Insufficient number of parameters to instantiate the type.");

                        return Activator.CreateInstance(UnderlyingType, arguments);
                    }
                case StructuralDataTypeKinds.Entity:
                    // REVIEW: Can/should we lift this limitation?
                    throw new NotImplementedException("User-supplied entity types cannot be instantiated through this method.");
            }

            throw new NotImplementedException();
        }
    }
}
