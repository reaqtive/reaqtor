// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
// ER - July 2013 - Small tweaks.
//

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq.CompilerServices;
using System.Linq.CompilerServices.Bonsai;

namespace System.Reflection
{
    // PERF: Consider specializations with low argument counts.

    /// <summary>
    /// Lightweight representation of a closed generic method.
    /// </summary>
    public sealed class GenericMethodInfoSlim : MethodInfoSlim
    {
        /// <summary>
        /// Lazily computed value for the <see cref="ReturnType"/> property.
        /// See <see cref="EnsureProperties"/> for the code initializing this field.
        /// </summary>
        private TypeSlim _returnType;

        /// <summary>
        /// Lazily computed value for the <see cref="ParameterTypes"/> property.
        /// See <see cref="EnsureProperties"/> for the code initializing this field.
        /// </summary>
        private ReadOnlyCollection<TypeSlim> _parameterTypes;

        /// <summary>
        /// Creates a new closed generic method representation object.
        /// </summary>
        /// <param name="declaringType">The declaring type of the generic method.</param>
        /// <param name="methodDefinition">Generic method definition.</param>
        /// <param name="arguments">Generic type arguments.</param>
        internal GenericMethodInfoSlim(TypeSlim declaringType, GenericDefinitionMethodInfoSlim methodDefinition, ReadOnlyCollection<TypeSlim> arguments)
            : base(declaringType)
        {
            Debug.Assert(methodDefinition != null);
            Debug.Assert(arguments != null);

            GenericMethodDefinition = methodDefinition;
            GenericArguments = arguments;
        }

        /// <summary>
        /// Gets the kind of the method.
        /// </summary>
        public sealed override MethodInfoSlimKind Kind => MethodInfoSlimKind.Generic;

        /// <summary>
        /// Checks if the method is generic.
        /// </summary>
        public sealed override bool IsGenericMethod => true;

        /// <summary>
        /// Gets the generic method definition.
        /// </summary>
        public GenericDefinitionMethodInfoSlim GenericMethodDefinition { get; }

        /// <summary>
        /// Gets the generic type arguments.
        /// </summary>
        public ReadOnlyCollection<TypeSlim> GenericArguments { get; }

        /// <summary>
        /// Gets the return type of the method.
        /// </summary>
        public sealed override TypeSlim ReturnType
        {
            get
            {
                EnsureProperties();

                return _returnType;
            }
        }

        /// <summary>
        /// Gets the parameter types of the method.
        /// </summary>
        public sealed override ReadOnlyCollection<TypeSlim> ParameterTypes
        {
            get
            {
                EnsureProperties();

                return _parameterTypes;
            }
        }

        /// <summary>
        /// Ensures that the values of properties <see cref="ReturnType"/> and <see cref="ParameterTypes"/> are
        /// computed and available in fields.
        /// </summary>
        private void EnsureProperties()
        {
            // REVIEW: Consider if we should compute ReturnType and ParameterTypes at the same time when these
            //         properties are triggering evaluation, or do it separately.

            if (_returnType == null || _parameterTypes == null)
            {
                var typeSubstitutor = GetSubstitutor();

                if (_returnType == null)
                {
                    _returnType = GetReturnType(typeSubstitutor);
                }

                _parameterTypes ??= GetParameterTypes(typeSubstitutor);
            }
        }

        /// <summary>
        /// Gets a type substitutor to replace generic parameters by generic arguments for use when computing
        /// the <see cref="ReturnType"/> and <see cref="ParameterTypes"/> property values.
        /// </summary>
        /// <returns>A new type substitutor instance that can be used to bind generic parameters.</returns>
        private TypeSlimSubstitutor GetSubstitutor()
        {
            var genericArgumentsCount = GenericArguments.Count;
            var definedGenericParameterTypes = GenericMethodDefinition.GenericParameterTypes;

            var genericParametersMap = new Dictionary<TypeSlim, TypeSlim>(genericArgumentsCount);
            for (var i = 0; i < genericArgumentsCount; ++i)
            {
                genericParametersMap[definedGenericParameterTypes[i]] = GenericArguments[i];
            }

            return new TypeSlimSubstitutor(genericParametersMap); // PERF: This causes equality comparator allocations; consider passing a pooled comparer.
        }

        /// <summary>
        /// Computes the value for <see cref="ReturnType"/> with bound generic parameters.
        /// </summary>
        /// <param name="substitutor">The type substitutor to use for generic parameter binding.</param>
        /// <returns>The bound type to expose in <see cref="ReturnType"/>.</returns>
        private TypeSlim GetReturnType(TypeSlimSubstitutor substitutor)
        {
            return substitutor.Visit(GenericMethodDefinition.ReturnType);
        }

        /// <summary>
        /// Computes the value for <see cref="ParameterTypes"/> with bound generic parameters.
        /// </summary>
        /// <param name="substitutor">The type substitutor to use for generic parameter binding.</param>
        /// <returns>The bound types to expose in <see cref="ParameterTypes"/>.</returns>
        private ReadOnlyCollection<TypeSlim> GetParameterTypes(TypeSlimSubstitutor substitutor)
        {
            var parameterTypesCount = GenericMethodDefinition.ParameterTypes.Count;

            var parameterTypes = new TypeSlim[parameterTypesCount];
            for (var i = 0; i < parameterTypesCount; ++i)
            {
                parameterTypes[i] = substitutor.Visit(GenericMethodDefinition.ParameterTypes[i]);
            }

            return new TrueReadOnlyCollection<TypeSlim>(/* transfer ownership */ parameterTypes);
        }
    }
}
