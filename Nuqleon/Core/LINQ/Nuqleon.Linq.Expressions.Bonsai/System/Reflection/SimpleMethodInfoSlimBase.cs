// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
// ER - July 2013 - Small tweaks.
//

using System.Collections.ObjectModel;
using System.Diagnostics;

namespace System.Reflection
{
    /// <summary>
    /// Lightweight representation of a simple method.
    /// </summary>
    public abstract class SimpleMethodInfoSlimBase : MethodInfoSlim
    {
        /// <summary>
        /// Creates a new simple method representation object.
        /// </summary>
        /// <param name="declaringType">Type declaring the method.</param>
        /// <param name="name">Name of the method.</param>
        /// <param name="parameterTypes">Type of the method parameters.</param>
        /// <param name="returnType">Return type of the method</param>
        protected SimpleMethodInfoSlimBase(TypeSlim declaringType, string name, ReadOnlyCollection<TypeSlim> parameterTypes, TypeSlim returnType)
            : base(declaringType)
        {
            Debug.Assert(!string.IsNullOrEmpty(name));
            Debug.Assert(parameterTypes != null);

            Name = name;
            ParameterTypes = parameterTypes;
            ReturnType = returnType;
        }

        /// <summary>
        /// Gets the name of the method.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the types of the method parameters.
        /// </summary>
        public override ReadOnlyCollection<TypeSlim> ParameterTypes { get; }

        /// <summary>
        /// Gets the return type of the method.
        /// </summary>
        public override TypeSlim ReturnType { get; }
    }
}
