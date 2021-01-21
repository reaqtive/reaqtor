// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD, ER - October 2013 - Created this file.
//

using Nuqleon.DataModel.TypeSystem;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Nuqleon.DataModel.CompilerServices.Bonsai
{
    #region Aliases

    using EntityTypeSubstitutor = EntityTypeSlimSubstitutor;
    using FindEntityTypes = FindEntityTypeSlims;
    using Type = TypeSlim;
    using Expression = ExpressionSlim;
    using EntityInfo = EntityInfoSlim;

    #endregion

    /// <summary>
    /// Expression tree visitor to substitute user-defined mapped types with an anonymous or record type.
    /// </summary>
    public abstract class ExpressionSlimEntityTypeSubstitutor
    {
        private readonly EntityTypeSubstitutor _substitutor;

        /// <summary>
        /// Creates the visitor that applies the type substitutor provided to replace entity types.
        /// </summary>
        /// <param name="substitutor">The type substitutor to use.</param>
        protected ExpressionSlimEntityTypeSubstitutor(EntityTypeSubstitutor substitutor)
        {
            _substitutor = substitutor ?? throw new ArgumentNullException(nameof(substitutor));
            _substitutor.Parent = this;
        }

        #region Methods

        /// <summary>
        /// Transforms an expression by replacing entity types using the type substitutor provided in the constructor.
        /// </summary>
        /// <param name="expression">The expression to transform.</param>
        /// <returns>An expression with entity types replaced.</returns>
        public virtual Expression Apply(Expression expression)
        {
            if (expression == null)
            {
                return null;
            }

            var entityInfo = FindEntityTypes.Apply(expression);

            CompileEntityTypes(entityInfo);

            var result = _substitutor.Apply(expression);
            return result;
        }

        /// <summary>
        /// Gets a converter to create a slim type from a data type.
        /// </summary>
        protected abstract DataTypeVisitor<Type, PropertyDataSlim> DataTypeConverter { get; }

        private void CompileEntityTypes(EntityInfo entityInfo)
        {
            var entities = entityInfo.Entities;
            var enumerations = entityInfo.Enumerations;

            var typeMap = _substitutor.TypeMap;

            var typesToCompile = new Dictionary<Type, StructuralDataType>();

            foreach (var entity in entities)
            {
                var type = entity.Key;
                var dataType = entity.Value;

                if (!typeMap.ContainsKey(type))
                {
                    var newType = DataTypeConverter.Visit(dataType);
                    typeMap[type] = newType;
                    typesToCompile.Add(type, dataType);
                }
            }

            foreach (var enumeration in enumerations)
            {
                var type = enumeration.Key;
                var dataType = enumeration.Value;

                var underlyingType = Helpers.GetUnderlyingEnumType(dataType.UnderlyingType);

#if ENUM_AS_STRING
                //
                // TODO: Flexibility to map to string. This can get tricky with non-nullable uses of
                //       the enum during type substitution and when dealing with [Flags].
                //
                typeMap[type] = typeof(string);
#else
                var typeSlim = underlyingType.ToTypeSlim();
                typeSlim.CarryType(underlyingType);
                typeMap[type] = typeSlim;
#endif
            }

            foreach (var type in typesToCompile.Keys)
            {
                var builder = (StructuralTypeSlimReference)typeMap[type];
                builder.Freeze();

                typeMap[type] = builder;
            }
        }

        #endregion
    }
}
