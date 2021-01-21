// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
//

using System.Reflection;

namespace Nuqleon.DataModel.TypeSystem
{
    /// <summary>
    /// Represents a property in a type, conform to the data model.
    /// </summary>
    /// <typeparam name="TType">Type representing a data model type.</typeparam>
    public class DataProperty<TType>
    {
        internal DataProperty(MemberInfo property, string name, TType type)
        {
            Property = property;
            Name = name;
            Type = type;
        }

        /// <summary>
        /// Gets the underlying CLR property or field.
        /// </summary>
        public MemberInfo Property { get; }

        /// <summary>
        /// Gets the property name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the property type.
        /// </summary>
        public TType Type { get; }
    }

    /// <summary>
    /// Represents a property in a type, conform to the data model.
    /// </summary>
    public class DataProperty : DataProperty<DataType>
    {
        internal DataProperty(MemberInfo property, string name, DataType type)
            : base(property, name, type)
        {
        }

        /// <summary>
        /// Gets the value of the property.
        /// </summary>
        /// <param name="target">Object to get the property value from.</param>
        /// <returns>Value of the property.</returns>
        public object GetValue(object target)
        {
            if (Property is PropertyInfo property)
            {
                return property.GetValue(target);
            }
            else
            {
                return ((FieldInfo)Property).GetValue(target);
            }
        }

        /// <summary>
        /// Sets the value of the property.
        /// </summary>
        /// <param name="target">Object to set the property value on.</param>
        /// <param name="value">Property value to set.</param>
        public void SetValue(object target, object value)
        {
            if (Property is PropertyInfo property)
            {
                property.SetValue(target, value);
            }
            else
            {
                ((FieldInfo)Property).SetValue(target, value);
            }
        }
    }
}
