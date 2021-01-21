// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER, BD - November 2013 - Created this file.
//

using Nuqleon.DataModel.TypeSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.CompilerServices;
using System.Reflection;

namespace Nuqleon.DataModel.CompilerServices.Bonsai
{
    #region Aliases

    using Expression = System.Linq.Expressions.ExpressionSlim;
    using MemberAssignment = System.Linq.Expressions.MemberAssignmentSlim;

    using Type = TypeSlim;
    using MemberInfo = MemberInfoSlim;

    using ExpressionEntityTypeSubstitutor = ExpressionSlimEntityTypeSubstitutor;
    using EntityTypeSubstitutor = EntityTypeSlimSubstitutor;

#if DEBUG
    using Object = ObjectSlim;
#endif

    #endregion

    /// <summary>
    /// Expression tree visitor to substitute occurrences of user-defined entity types for record types.
    /// </summary>
    public sealed class ExpressionSlimEntityTypeRecordizer : ExpressionEntityTypeSubstitutor
    {
        /// <summary>
        /// Creates a new entity type substitutor using record types.
        /// </summary>
        public ExpressionSlimEntityTypeRecordizer()
            : base(new RecordEntityTypeSlimSubstitutor())
        {
            DataTypeConverter = new RecordDataTypeToTypeSlimConverter();
        }

        /// <summary>
        /// Gets a converter to create a slim type from a data type.
        /// </summary>
        protected override DataTypeVisitor<Type, PropertyDataSlim> DataTypeConverter { get; }

        private sealed class RecordEntityTypeSlimSubstitutor : EntityTypeSubstitutor
        {
            public RecordEntityTypeSlimSubstitutor()
                : base(new Dictionary<Type, Type>(TypeSlimEqualityComparer.Default))
            {
            }

            protected override Expression CreateNewExpression(Type type, IDictionary<MemberInfo, Expression> memberAssignments)
            {
                var constructor = type.GetConstructor(EmptyReadOnlyCollection<Type>.Instance);

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

                for (int i = 0, n = newProperties.Count; i < n; i++)
                {
                    var newProperty = newProperties[i];
                    var oldProperty = oldPropertyMap[newProperty.Name];

                    var oldValue = oldProperty.GetValue(originalValue);
                    var newValue = ConvertConstant(oldValue, oldProperty.Type, newProperty.Type);

                    newProperty.SetValue(res, newValue);
                }

                return res;
            }

#if DEBUG
            protected override void CheckConstantStructuralCore(Object newValue, StructuralDataType oldDataType)
            {
                var oldPropertyMap = oldDataType.Properties.ToDictionary(p => p.Name, p => p);

                var newStructuralType = (StructuralTypeSlim)newValue.TypeSlim;
                var newProperties = newStructuralType.Properties;

                for (int i = 0, n = newProperties.Count; i < n; i++)
                {
                    var newProperty = newProperties[i];
                    var oldProperty = oldPropertyMap[newProperty.Name];

                    var oldValue = oldProperty.GetValue(newValue.Value);
                    var newPropertyValue = Object.Create(oldValue, newProperty.PropertyType, oldProperty.Type.UnderlyingType);
                    CheckConstant(newPropertyValue, oldProperty.Type);
                }
            }
#endif
        }
    }
}
