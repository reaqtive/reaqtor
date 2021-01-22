// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
//

namespace Nuqleon.DataModel.TypeSystem
{
    /// <summary>
    /// Represents an enum value, conform to the data model.
    /// </summary>
    public class DataEnumValue
    {
        internal DataEnumValue(string name, object value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Gets the enum value name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the enum underlying value.
        /// </summary>
        public object Value { get; }
    }
}
