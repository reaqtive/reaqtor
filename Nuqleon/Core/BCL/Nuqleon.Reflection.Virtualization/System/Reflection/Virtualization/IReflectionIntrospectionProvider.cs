// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2016 - Created this file.
//

namespace System.Reflection
{
    /// <summary>
    /// Interface representing a reflection provider used to introspect reflection objects.
    /// </summary>
    public interface IReflectionIntrospectionProvider : IAssemblyIntrospectionProvider, IModuleIntrospectionProvider, ITypeIntrospectionProvider, IMemberInfoIntrospectionProvider, IFieldInfoIntrospectionProvider, IMethodInfoIntrospectionProvider, IMethodBaseIntrospectionProvider, IPropertyInfoIntrospectionProvider, IEventInfoIntrospectionProvider, IParameterInfoIntrospectionProvider
    {
    }
}
