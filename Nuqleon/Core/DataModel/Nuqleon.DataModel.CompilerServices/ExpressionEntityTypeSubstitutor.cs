// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD, ER - June 2013 - Created this file.
//

using Nuqleon.DataModel.TypeSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;

#if NETSTANDARD2_0
using Nuqleon.Reflection.Emit;
#else
using System.Reflection.Emit;
#endif

namespace Nuqleon.DataModel.CompilerServices
{
    /// <summary>
    /// Expression tree visitor to substitute user-defined mapped types with an anonymous or record type.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Substitutor", Justification = "Identifier adequately reflects the intent of the class.")]
    public abstract partial class ExpressionEntityTypeSubstitutor
    {
        #region Fields

        private readonly EntityTypeSubstitutor _substitutor;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates the visitor that applies the type substitutor provided to replace entity types.
        /// </summary>
        /// <param name="substitutor">The type substitutor to use.</param>
        protected ExpressionEntityTypeSubstitutor(EntityTypeSubstitutor substitutor)
        {
            _substitutor = substitutor ?? throw new ArgumentNullException(nameof(substitutor));
            _substitutor.Parent = this;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Transforms an expression by replacing entity types using the type substitutor provided in the constructor.
        /// </summary>
        /// <param name="expression">The expression to transform.</param>
        /// <returns>An expression with entity types replaced.</returns>
        public virtual Expression Apply(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var entityInfo = FindEntityTypes.Apply(expression);

            CompileEntityTypes(entityInfo);

            var result = _substitutor.Apply(expression);
            return result;
        }

        #endregion
    }

#if !NO_REFLECTIONEMIT
    public partial class ExpressionEntityTypeSubstitutor
    {
        #region Methods

        /// <summary>
        /// Gets a type builder to later use to define a type.
        /// </summary>
        /// <param name="rtc">The runtime compiler to get the type builder from.</param>
        /// <returns>The type builder.</returns>
        protected abstract TypeBuilder GetTypeBuilder(RuntimeCompiler rtc);

        /// <summary>
        /// Define a type using a runtime compiler instance, a type builder, and a set of properties.
        /// </summary>
        /// <param name="rtc">The runtime compiler.</param>
        /// <param name="builder">The type builder to use for the type.</param>
        /// <param name="properties">The properties to add to the type.</param>
        protected abstract void DefineType(RuntimeCompiler rtc, TypeBuilder builder, IEnumerable<KeyValuePair<string, Type>> properties);

        private void CompileEntityTypes(EntityInfo entityInfo)
        {
            var entities = entityInfo.Entities;
            var enumerations = entityInfo.Enumerations;

            var typeMap = _substitutor.TypeMap;

            var rtc = new RuntimeCompiler();

            var typesToCompile = new Dictionary<Type, StructuralDataType>();

            foreach (var entity in entities)
            {
                var type = entity.Key;
                var dataType = entity.Value;

                if (!typeMap.ContainsKey(type))
                {
                    var newType = GetTypeBuilder(rtc);
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
                typeMap[type] = underlyingType;
#endif
            }

            var subst = new EnumAwareTypeSubstitutor(typeMap);

            foreach (var typeToCompile in typesToCompile)
            {
                var type = typeToCompile.Key;
                var dataType = typeToCompile.Value;

                var builder = (TypeBuilder)typeMap[type];

                var properties = dataType.Properties.Select(p => new KeyValuePair<string, Type>(p.Name, subst.Rewrite(p.Type.UnderlyingType)));

                DefineType(rtc, builder, properties);

                var newType = builder.CreateType();
                typeMap[type] = newType;
            }
        }

        #endregion
    }
#else
    public partial class ExpressionEntityTypeSubstitutor
    {
        private void CompileEntityTypes(EntityInfo entityInfo)
        {
            if (entityInfo.Entities.Count > 0)
            {
                throw new NotSupportedException("Entity types are not yet supported in this build flavor.");
            }
        }
    }
#endif
}
