// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - May 2013 - Created this file.
//

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Attribute to annotate constructors, methods, properties, and fields with a visitor type that will be used to process
    /// expression trees referring to those reflection members.
    /// </summary>
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class VisitorAttribute : Attribute
    {
        /// <summary>
        /// Creates a new visitor attribute referring to the specified visitor type.
        /// </summary>
        /// <param name="visitorType">Type of the visitor that will process expression nodes that use the reflection member the attribute is applied to.</param>
        public VisitorAttribute(Type visitorType)
        {
            VisitorType = visitorType;
        }

        /// <summary>
        /// Gets the type of the visitor that will process expression nodes that use the reflection member the attribute is applied to.
        /// </summary>
        public Type VisitorType { get; }
    }
}
