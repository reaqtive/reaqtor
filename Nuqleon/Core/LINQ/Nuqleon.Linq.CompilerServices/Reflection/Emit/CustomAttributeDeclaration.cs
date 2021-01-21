// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - July 2013 - Created this file.
//

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Container for property custom attribute information.
    /// </summary>
    public class CustomAttributeDeclaration
    {
        #region Constructors

        //
        // TODO: Add constructors for named properties and fields.
        //

        /// <summary>
        /// Instantiates a container for property custom attribute information.
        /// </summary>
        /// <param name="type">The type of custom attribute.</param>
        /// <param name="arguments">The constructor arguments for the custom attribute type.</param>
        public CustomAttributeDeclaration(Type type, params object[] arguments)
            : this(type, arguments.ToReadOnly())
        {
        }

        /// <summary>
        /// Instantiates a container for property custom attribute information.
        /// </summary>
        /// <param name="type">The type of custom attribute.</param>
        /// <param name="arguments">The constructor arguments for the custom attribute type.</param>
        public CustomAttributeDeclaration(Type type, IEnumerable<object> arguments)
            : this(type, arguments.ToReadOnly())
        {
        }

        /// <summary>
        /// Instantiates a container for property custom attribute information.
        /// </summary>
        /// <param name="type">The type of custom attribute.</param>
        /// <param name="arguments">The constructor arguments for the custom attribute type.</param>
        public CustomAttributeDeclaration(Type type, ReadOnlyCollection<object> arguments)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (!typeof(Attribute).IsAssignableFrom(type))
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Provided type must derive from '{0}'. Provided type '{1}' does not.", typeof(Attribute).FullName, type.FullName));

            CustomAttributeType = type;
            Arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the custom attribute type.
        /// </summary>
        public Type CustomAttributeType { get; }

        /// <summary>
        /// Gets the custom attribute type constructor arguments.
        /// </summary>
        public ReadOnlyCollection<object> Arguments { get; }

        /// <summary>
        /// Gets the custom attribute type constructor.
        /// </summary>
        public ConstructorInfo Constructor
        {
            get
            {
                var ctorSignature = Arguments.Select(o => o.GetType()).ToArray();
                var ctor = CustomAttributeType.GetConstructor(ctorSignature);
                if (ctor == null)
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Custom attribute type '{0}' does not have a constructor with signature '{1}'.", CustomAttributeType, ctorSignature));
                return ctor;
            }
        }

        #endregion
    }
}
