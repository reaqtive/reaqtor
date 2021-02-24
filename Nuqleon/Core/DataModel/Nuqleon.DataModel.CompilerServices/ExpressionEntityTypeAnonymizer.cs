// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
//

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
    /// Expression tree visitor to substitute occurrences of user-defined entity types for anonymous types.
    /// </summary>
    public sealed class ExpressionEntityTypeAnonymizer : ExpressionEntityTypeSubstitutor
    {
        /// <summary>
        /// Creates a new entity type substitutor using anonymous types.
        /// </summary>
        public ExpressionEntityTypeAnonymizer()
            : base(new AnonymousEntityTypeSubstitutor())
        {
        }

        /// <summary>
        /// Gets a type builder to later use to define a type.
        /// </summary>
        /// <param name="rtc">The runtime compiler to get the type builder from.</param>
        /// <returns>The type builder.</returns>
        protected override TypeBuilder GetTypeBuilder(RuntimeCompiler rtc) => rtc.GetNewAnonymousTypeBuilder();

        /// <summary>
        /// Define a type using a runtime compiler instance, a type builder, and a set of properties.
        /// </summary>
        /// <param name="rtc">The runtime compiler.</param>
        /// <param name="builder">The type builder to use for the type.</param>
        /// <param name="properties">The properties to add to the type.</param>
        protected override void DefineType(RuntimeCompiler rtc, TypeBuilder builder, IEnumerable<KeyValuePair<string, Type>> properties) => rtc.DefineAnonymousType(builder, properties);

        private sealed class AnonymousEntityTypeSubstitutor : EntityTypeSubstitutor
        {
            public AnonymousEntityTypeSubstitutor()
                : base(new Dictionary<Type, Type>())
            {
            }

            protected override Expression CreateNewExpression(Type type, IDictionary<MemberInfo, Expression> memberAssignments)
            {
                var constructor = type.GetConstructors().Single();
                var parameters = constructor.GetParameters();

                var arguments = new Expression[parameters.Length];
                var members = new PropertyInfo[parameters.Length];

                var membersAssignmentsByName = memberAssignments.ToDictionary(m => m.Key.Name, m => m.Value);

                var n = 0;

                for (var i = 0; i < parameters.Length; i++)
                {
                    var newParameter = parameters[i];

                    if (!membersAssignmentsByName.TryGetValue(newParameter.Name, out var newArgument))
                    {
                        newArgument = new UnassignedExpression(newParameter.ParameterType);
                    }
                    else
                    {
                        n++;
                    }

                    arguments[i] = newArgument;
                    members[i] = type.GetProperty(newParameter.Name);
                }

                if (n != memberAssignments.Count)
                {
                    throw new InvalidOperationException("Orphaned parameters found.");
                }

                var result = Expression.New(constructor, arguments, members);
                return result;
            }

            protected override object ConvertConstantStructuralCore(object originalValue, StructuralDataType oldDataType, StructuralDataType newDataType)
            {
                var oldPropertyMap = oldDataType.Properties.ToDictionary(p => p.Name, p => p);

                var newProperties = newDataType.Properties;
                var count = newProperties.Count;
                var args = new object[count];

                for (var i = 0; i < count; i++)
                {
                    var newProperty = newProperties[i];
                    var oldProperty = oldPropertyMap[newProperty.Name];

                    var oldValue = oldProperty.GetValue(originalValue);
                    var newValue = ConvertConstant(oldValue, oldProperty.Type, newProperty.Type);

                    args[i] = newValue;
                }

                var res = newDataType.CreateInstance(args);
                return res;
            }
        }
    }
}
