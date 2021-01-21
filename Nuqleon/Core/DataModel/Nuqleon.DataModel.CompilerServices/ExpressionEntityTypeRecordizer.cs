// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER, BD - June 2013 - Created this file.
//

#if !NO_REFLECTIONEMIT

using Nuqleon.DataModel.TypeSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

#if NETSTANDARD2_0
using Nuqleon.Reflection.Emit;
#else
using System.Reflection.Emit;
#endif

namespace Nuqleon.DataModel.CompilerServices
{
    /// <summary>
    /// Expression tree visitor to substitute occurrences of user-defined entity types for record types.
    /// </summary>
    public sealed class ExpressionEntityTypeRecordizer : ExpressionEntityTypeSubstitutor
    {
        /// <summary>
        /// Creates a new entity type substitutor using record types.
        /// </summary>
        public ExpressionEntityTypeRecordizer()
            : base(new RecordEntityTypeSubstitutor())
        {
        }

        /// <summary>
        /// Gets a type builder to later use to define a type.
        /// </summary>
        /// <param name="rtc">The runtime compiler to get the type builder from.</param>
        /// <returns>The type builder.</returns>
        protected override TypeBuilder GetTypeBuilder(RuntimeCompiler rtc) => rtc.GetNewRecordTypeBuilder();

        /// <summary>
        /// Define a type using a runtime compiler instance, a type builder, and a set of properties.
        /// </summary>
        /// <param name="rtc">The runtime compiler.</param>
        /// <param name="builder">The type builder to use for the type.</param>
        /// <param name="properties">The properties to add to the type.</param>
        protected override void DefineType(RuntimeCompiler rtc, TypeBuilder builder, IEnumerable<KeyValuePair<string, Type>> properties) => rtc.DefineRecordType(builder, properties, valueEquality: true);

        private sealed class RecordEntityTypeSubstitutor : EntityTypeSubstitutor
        {
            public RecordEntityTypeSubstitutor()
                : base(new Dictionary<Type, Type>())
            {
            }

            protected override Expression CreateNewExpression(Type type, IDictionary<MemberInfo, Expression> memberAssignments)
            {
                var constructor = type.GetConstructor(Type.EmptyTypes);

                var newExpression = Expression.New(constructor, Array.Empty<Expression>(), Array.Empty<MemberInfo>());

                var bindings = new MemberAssignment[memberAssignments.Count];

                var i = 0;
                foreach (var kv in memberAssignments)
                {
                    bindings[i++] = Expression.Bind(kv.Key, kv.Value);
                }

                var memberInit = Expression.MemberInit(newExpression, bindings);

                return memberInit;
            }

            protected override object ConvertConstantStructuralCore(object originalValue, StructuralDataType oldDataType, StructuralDataType newDataType)
            {
                var res = newDataType.CreateInstance();

                ConstantsMap[originalValue] = res; // Allow cycles

                var oldPropertyMap = oldDataType.Properties.ToDictionary(p => p.Name, p => p);

                var newProperties = newDataType.Properties;
                var count = newProperties.Count;

                for (var i = 0; i < count; i++)
                {
                    var newProperty = newProperties[i];
                    var oldProperty = oldPropertyMap[newProperty.Name];

                    var oldValue = oldProperty.GetValue(originalValue);
                    var newValue = ConvertConstant(oldValue, oldProperty.Type, newProperty.Type);

                    newProperty.SetValue(res, newValue);
                }

                return res;
            }
        }
    }
}

#endif
