// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - November 2013 - Created this file.
//

using System;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Nuqleon.DataModel.CompilerServices.Bonsai
{
    /// <summary>
    /// A slim representation of a constructor that contains information about
    /// mapping attributes on the constructor parameters.
    /// </summary>
    public sealed class DataModelConstructorInfoSlim : ConstructorInfoSlim
    {
        /// <summary>
        /// Initializes the slim representation of the constructor.
        /// </summary>
        /// <param name="declaringType">The declaring type.</param>
        /// <param name="parameterTypes">The parameter types.</param>
        /// <param name="parameterMappings">The parameter mappings.</param>
        public DataModelConstructorInfoSlim(TypeSlim declaringType, ReadOnlyCollection<TypeSlim> parameterTypes, ReadOnlyCollection<string> parameterMappings)
            : base(declaringType, parameterTypes)
        {
            ParameterMappings = parameterMappings ?? throw new ArgumentNullException(nameof(parameterMappings));
        }

        /// <summary>
        /// Gets the ordered list of parameter mappings used on the constructor.
        /// </summary>
        public ReadOnlyCollection<string> ParameterMappings { get; }
    }
}
