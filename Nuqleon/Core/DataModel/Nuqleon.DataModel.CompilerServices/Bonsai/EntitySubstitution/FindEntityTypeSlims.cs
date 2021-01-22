// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD, ER - November 2013 - Created this file.
//

using Nuqleon.DataModel.TypeSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.CompilerServices.Bonsai;
using System.Reflection;

namespace Nuqleon.DataModel.CompilerServices.Bonsai
{
    #region Aliases

    using Expression = System.Linq.Expressions.ExpressionSlim;

    using Type = System.Reflection.TypeSlim;
    using MethodInfo = System.Reflection.MethodInfoSlim;

    using TypeVisitor = System.Reflection.TypeSlimVisitor;
    using ExpressionVisitorWithReflection = System.Linq.CompilerServices.Bonsai.ExpressionSlimVisitorWithReflection;

    using EntityInfo = Nuqleon.DataModel.CompilerServices.Bonsai.EntityInfoSlim;

    #endregion

    internal static class FindEntityTypeSlims
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

            // CONSIDER: We can move this up to `EntityTypeSubstitutor`.

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
            var entities = new Dictionary<Type, StructuralDataType>(TypeSlimEqualityComparer.Default);
            var enumerations = new Dictionary<Type, PrimitiveDataType>(TypeSlimEqualityComparer.Default);

            var entitiesQueue = new Queue<KeyValuePair<Type, StructuralDataType>>(entityInfo.Entities);
            var enumerationsSet = new HashSet<KeyValuePair<Type, PrimitiveDataType>>(entityInfo.Enumerations);

            var tcv = new TransitiveClosureVisitor(entitiesQueue.Enqueue, enumerationsSet.Add);

            // Starting with the types found at each node in the original expression,
            // add each type to the lookup and recurse into the type, looking for
            // further data model types (enqueueing those found). Repeat until the
            // queue is empty. The recursion step is necessary, particularly for init
            // expressions for lists and members.
            while (entitiesQueue.Count > 0)
            {
                var entry = entitiesQueue.Dequeue();

                if (!entities.ContainsKey(entry.Key))
                {
                    entities.Add(entry.Key, entry.Value);

                    // Side-effect of visit is to enqueue further data model types.
                    tcv.Visit(entry.Value);
                }
            }

            foreach (var enumeration in enumerationsSet)
            {
                var type = enumeration.Key;
                var dataType = enumeration.Value;

                if (dataType.IsNullable && type.TryGetCarriedType(out var carriedType))
                {
                    var nonNullable = Nullable.GetUnderlyingType(carriedType);
                    type = ((GenericTypeSlim)type).GetGenericArgument(0);
                    dataType = new PrimitiveDataType(nonNullable, PrimitiveDataTypeKinds.EntityEnum);
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
            var entities = new Dictionary<Type, StructuralDataType>();
            var enumerations = new Dictionary<Type, PrimitiveDataType>();

            foreach (var kv in entityInfo.Entities)
            {
                if (!kv.Key.TryGetCarriedType(out var carriedType) || !carriedType.IsDefined(typeof(KnownTypeAttribute), inherit: false))
                {
                    entities.Add(kv.Key, kv.Value);
                }
            }

            foreach (var kv in entityInfo.Enumerations)
            {
                if (!kv.Key.TryGetCarriedType(out var carriedType) || !carriedType.IsDefined(typeof(KnownTypeAttribute), inherit: false))
                {
                    enumerations.Add(kv.Key, kv.Value);
                }
            }

            return new EntityInfo { Entities = entities, Enumerations = enumerations };
        }

        // PERF: Instances of this type could be pooled but careful treatment is needed given that the Entities and Enumerations are extracted as results
        //       and get copied later when computing the transitive closure.

        private sealed class Impl : ExpressionVisitorWithReflection
        {
            private readonly FindEntityDataTypes _findEntityDataTypes;
            private readonly TypeSlimDerivationVisitor _derivationVisitor;

            public Impl()
            {
                Entities = new Dictionary<Type, StructuralDataType>();
                Enumerations = new Dictionary<Type, PrimitiveDataType>();
                _findEntityDataTypes = new FindEntityDataTypes(this);
                _derivationVisitor = new TypeSlimDerivationVisitor();
            }

            public Dictionary<Type, StructuralDataType> Entities { get; }

            public Dictionary<Type, PrimitiveDataType> Enumerations { get; }

            public override Expression Visit(Expression node)
            {
                if (node != null)
                {
                    var type = _derivationVisitor.Visit(node);

                    if (type != null)
                    {
                        _findEntityDataTypes.Visit(type);
                    }
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
                        foreach (var arg in ((GenericMethodInfoSlim)method).GenericArguments)
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

                    if (type.TryGetCarriedType(out var carriedType))
                    {
                        if (!_parent.Entities.ContainsKey(type))
                        {
                            if (DataTypeHelpers.IsStructuralEntityDataTypeCached(carriedType))
                            {
                                var structuralType = (StructuralDataType)DataTypeHelpers.FromTypeCached(carriedType, allowCycles: true);
                                _parent.Entities[type] = structuralType;
                            }
                            else
                            {
                                //
                                // Non-generic record types or anonymous types (created by RuntimeCompiler) may contain entities,
                                // so we have to grab those as well.
                                //

                                if (!carriedType.IsGenericType && DataTypeHelpers.TryFromTypeCached(carriedType, allowCycles: true, out var result) && result.Kind == DataTypeKinds.Structural)
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
                            if (DataTypeHelpers.IsEntityEnumDataTypeCached(carriedType))
                            {
                                var primitiveType = (PrimitiveDataType)DataTypeHelpers.FromTypeCached(carriedType, allowCycles: true);
                                Debug.Assert(primitiveType.PrimitiveKind == PrimitiveDataTypeKinds.EntityEnum);
                                _parent.Enumerations[type] = primitiveType;
                            }
                        }
                    }

                    return base.Visit(type);
                }
            }
        }

        private sealed class TransitiveClosureVisitor : DataTypeVisitor
        {
            private readonly Action<KeyValuePair<Type, StructuralDataType>> _enqueue;
            private readonly Func<KeyValuePair<Type, PrimitiveDataType>, bool> _addEnumeration;
            private readonly HashSet<Type> _processed;

            public TransitiveClosureVisitor(Action<KeyValuePair<Type, StructuralDataType>> enqueue, Func<KeyValuePair<Type, PrimitiveDataType>, bool> addEnumeration)
            {
                _enqueue = enqueue;
                _addEnumeration = addEnumeration;
                _processed = new HashSet<Type>(TypeSlimEqualityComparer.Default);
            }

            protected override DataType VisitStructural(StructuralDataType type)
            {
                var typeSlim = type.UnderlyingType.ToTypeSlim();
                typeSlim.CarryType(type.UnderlyingType);
                if (_processed.Add(typeSlim))
                {
                    if (type.StructuralKind == StructuralDataTypeKinds.Entity)
                    {
                        _enqueue(new KeyValuePair<Type, StructuralDataType>(typeSlim, type));
                    }

                    return base.VisitStructural(type);
                }

                return type;
            }

            protected override DataType VisitPrimitive(PrimitiveDataType type)
            {
                if (type.PrimitiveKind == PrimitiveDataTypeKinds.EntityEnum)
                {
                    var typeSlim = type.UnderlyingType.ToTypeSlim();
                    typeSlim.CarryType(type.UnderlyingType);
                    if (_processed.Add(typeSlim))
                    {
                        _addEnumeration(new KeyValuePair<Type, PrimitiveDataType>(typeSlim, type));
                    }
                }

                return base.VisitPrimitive(type);
            }
        }
    }

    internal sealed class EntityInfoSlim
    {
        public Dictionary<Type, StructuralDataType> Entities { get; set; }
        public Dictionary<Type, PrimitiveDataType> Enumerations { get; set; }
    }
}
