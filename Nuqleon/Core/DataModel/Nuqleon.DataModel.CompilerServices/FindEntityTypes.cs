// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.Expressions;
using System.Reflection;

using Nuqleon.DataModel.TypeSystem;

namespace Nuqleon.DataModel.CompilerServices
{
    internal static class FindEntityTypes
    {
        /// <summary>
        /// Applies an expression visitor to find all the entity types within an expression, including the transitive closure of reachable entity types.
        /// </summary>
        /// <param name="expression">The expression to search.</param>
        /// <returns>A set of types found in the expression.</returns>
        public static EntityInfo Apply(Expression expression)
        {
            var impl = new Impl();
            impl.Visit(expression);

            var res = new EntityInfo
            {
                Entities = impl.Entities,
                Enumerations = impl.Enumerations
            };

            ComputeTransitiveClosure(res);

            //
            // CONSIDER: We can move this up to `EntityTypeSubstitutor`.
            //

            res = FilterKnownTypes(res);

            return res;
        }

        /// <summary>
        /// Computes the transitive closure of reachable entity types based on the specified set of initial types.
        /// </summary>
        /// <param name="entityInfo">Discovered entity information.</param>
        /// <returns>Transitive closure of reachable entity types.</returns>
        private static void ComputeTransitiveClosure(EntityInfo entityInfo)
        {
            var entities = new Dictionary<Type, StructuralDataType>();
            var enumerations = new Dictionary<Type, PrimitiveDataType>();

            var entitiesQueue = new Queue<KeyValuePair<Type, StructuralDataType>>(entityInfo.Entities);
            var enumerationsSet = new HashSet<KeyValuePair<Type, PrimitiveDataType>>(entityInfo.Enumerations);

            var tcv = new TransitiveClosureVisitor(entitiesQueue.Enqueue, kv => enumerationsSet.Add(kv));

            while (entitiesQueue.Count > 0)
            {
                var entry = entitiesQueue.Dequeue();

                if (!entities.ContainsKey(entry.Key))
                {
                    entities.Add(entry.Key, entry.Value);
                    tcv.Visit(entry.Value);
                }
            }

            foreach (var enumeration in enumerationsSet)
            {
                var type = enumeration.Key;
                var dataType = enumeration.Value;

                if (dataType.IsNullable)
                {
                    type = Nullable.GetUnderlyingType(type);
                    dataType = new PrimitiveDataType(type, PrimitiveDataTypeKinds.EntityEnum);
                }

                if (!enumerations.ContainsKey(type))
                {
                    enumerations.Add(type, dataType);
                }
            }

            entityInfo.Entities = entities;
            entityInfo.Enumerations = enumerations;
        }

        /// <summary>
        /// Filter the set of entity types, excluding known types.
        /// </summary>
        /// <param name="entityInfo">The set of entity types to filter.</param>
        /// <returns>The filtered set of entity types.</returns>
        private static EntityInfo FilterKnownTypes(EntityInfo entityInfo)
        {
            var result = new EntityInfo
            {
                Entities = entityInfo.Entities.Where(kvp => !kvp.Key.IsDefined(typeof(KnownTypeAttribute), inherit: false)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
                Enumerations = entityInfo.Enumerations.Where(kvp => !kvp.Key.IsDefined(typeof(KnownTypeAttribute), inherit: false)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
            };

            return result;
        }

        private sealed class Impl : ExpressionVisitorWithReflection
        {
            private readonly FindEntityDataTypes _findEntityDataTypes;

            public Impl()
            {
                Entities = new Dictionary<Type, StructuralDataType>();
                Enumerations = new Dictionary<Type, PrimitiveDataType>();
                _findEntityDataTypes = new FindEntityDataTypes(this);
            }

            public Dictionary<Type, StructuralDataType> Entities { get; }

            public Dictionary<Type, PrimitiveDataType> Enumerations { get; }

            public override Expression Visit(Expression node)
            {
                if (node != null)
                {
                    var type = node.Type;
                    _findEntityDataTypes.Visit(type);
                }

                return base.Visit(node);
            }

            protected override Type VisitType(Type type)
            {
                if (type != null)
                {
                    _findEntityDataTypes.Visit(type);
                }

                return base.VisitType(type);
            }

            protected override MethodInfo VisitMethod(MethodInfo method)
            {
                if (method != null)
                {
                    if (method.IsGenericMethod)
                    {
                        foreach (var arg in method.GetGenericArguments())
                        {
                            _findEntityDataTypes.Visit(arg);
                        }
                    }
                }

                return base.VisitMethod(method);
            }

            private sealed class FindEntityDataTypes : TypeVisitor
            {
                private readonly Impl _parent;

                public FindEntityDataTypes(Impl parent) => _parent = parent;

                public override Type Visit(Type type)
                {
                    if (!_parent.Entities.ContainsKey(type))
                    {
                        if (DataType.IsStructuralEntityDataType(type))
                        {
                            var structuralType = (StructuralDataType)DataType.FromType(type, allowCycles: true /* cycles should be rejected higher up; this utility can deal with them */);
                            _parent.Entities[type] = structuralType;
                        }
                        else
                        {
                            //
                            // Non-generic record types or anonymous types (created by RuntimeCompiler) may contain entities,
                            // so we have to grab those as well.
                            //

                            if (!type.IsGenericType && DataType.TryFromType(type, out var result) && result.Kind == DataTypeKinds.Structural)
                            {
                                var structuralType = (StructuralDataType)result;

                                var runtimeCompilerTypes = StructuralDataTypeKinds.Anonymous | StructuralDataTypeKinds.Record;

                                if ((runtimeCompilerTypes & structuralType.StructuralKind) != 0)
                                {
                                    _parent.Entities[type] = structuralType;
                                }
                            }
                        }
                    }

                    if (!_parent.Enumerations.ContainsKey(type))
                    {
                        if (DataType.IsEntityEnumDataType(type))
                        {
                            var primitiveType = (PrimitiveDataType)DataType.FromType(type, allowCycles: true /* doesn't matter */);
                            Debug.Assert(primitiveType.PrimitiveKind == PrimitiveDataTypeKinds.EntityEnum);
                            _parent.Enumerations[type] = primitiveType;
                        }
                    }

                    return base.Visit(type);
                }
            }
        }

        private sealed class TransitiveClosureVisitor : DataTypeVisitor
        {
            private readonly Action<KeyValuePair<Type, StructuralDataType>> _enqueue;
            private readonly Action<KeyValuePair<Type, PrimitiveDataType>> _addEnumeration;
            private readonly HashSet<Type> _processed;

            public TransitiveClosureVisitor(Action<KeyValuePair<Type, StructuralDataType>> enqueue, Action<KeyValuePair<Type, PrimitiveDataType>> addEnumeration)
            {
                _enqueue = enqueue;
                _addEnumeration = addEnumeration;
                _processed = new HashSet<Type>();
            }

            protected override DataType VisitStructural(StructuralDataType type)
            {
                if (_processed.Add(type.UnderlyingType))
                {
                    if (type.StructuralKind == StructuralDataTypeKinds.Entity)
                    {
                        _enqueue(new KeyValuePair<Type, StructuralDataType>(type.UnderlyingType, type));
                    }

                    return base.VisitStructural(type);
                }

                return type;
            }

            protected override DataType VisitPrimitive(PrimitiveDataType type)
            {
                if (type.PrimitiveKind == PrimitiveDataTypeKinds.EntityEnum)
                {
                    var enumType = type.UnderlyingType;

                    if (_processed.Add(enumType))
                    {
                        _addEnumeration(new KeyValuePair<Type, PrimitiveDataType>(enumType, type));
                    }
                }

                return base.VisitPrimitive(type);
            }
        }
    }

    internal sealed class EntityInfo
    {
        public IDictionary<Type, StructuralDataType> Entities { get; set; }
        public IDictionary<Type, PrimitiveDataType> Enumerations { get; set; }
    }
}
