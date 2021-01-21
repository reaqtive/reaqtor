// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - July 2013 - Created this file.
// BD - June 2014 - Created this file.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Omitted null checks similar to expression tree visitors.

using System.Collections.Generic;
using System.Linq.CompilerServices;
using System.Memory;

namespace System.Reflection
{
    /// <summary>
    /// Base implementation for the type slim comparer.
    /// </summary>
    public class TypeSlimEqualityComparator : IEqualityComparer<TypeSlim>, IClearable
    {
        internal const uint Prime = 0xa5555529; // See CompilationPass.cpp in C# compiler codebase.

        // WARNING: This type implements IClearable. If state is added here, make sure to reset it in the Clear method.

        private readonly Dictionary<StructuralTypeSlim, StructuralTypeSlim> _recursiveEquals;
        private readonly HashSet<StructuralTypeSlim> _visited;

        /// <summary>
        /// Instantiates a type slim comparator.
        /// </summary>
        public TypeSlimEqualityComparator()
        {
            _recursiveEquals = new Dictionary<StructuralTypeSlim, StructuralTypeSlim>(ReferenceEqualityComparer<TypeSlim>.Instance);
            _visited = new HashSet<StructuralTypeSlim>(ReferenceEqualityComparer<TypeSlim>.Instance);
        }

        #region Equals

        /// <summary>
        /// Checks if two type slims are equal.
        /// </summary>
        /// <param name="x">The left type slim.</param>
        /// <param name="y">The right type slim.</param>
        /// <returns>true if the given type slims are equal, false otherwise.</returns>
        public virtual bool Equals(TypeSlim x, TypeSlim y)
        {
            if (x is null && y is null)
            {
                return true;
            }

            if (x is null || y is null)
            {
                return false;
            }

            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x.Kind != y.Kind)
            {
                return false;
            }

            return x.Kind switch
            {
                TypeSlimKind.Simple => EqualsSimple((SimpleTypeSlim)x, (SimpleTypeSlim)y),
                TypeSlimKind.Array => EqualsArray((ArrayTypeSlim)x, (ArrayTypeSlim)y),
                TypeSlimKind.Structural => EqualsStructural((StructuralTypeSlim)x, (StructuralTypeSlim)y),
                TypeSlimKind.GenericDefinition => EqualsGenericDefinition((GenericDefinitionTypeSlim)x, (GenericDefinitionTypeSlim)y),
                TypeSlimKind.Generic => EqualsGeneric((GenericTypeSlim)x, (GenericTypeSlim)y),
                TypeSlimKind.GenericParameter => EqualsGenericParameter((GenericParameterTypeSlim)x, (GenericParameterTypeSlim)y),
                _ => EqualsExtensions(x, y),
            };
        }

        /// <summary>
        /// Checks if two simple type slims are equal.
        /// </summary>
        /// <param name="x">The left type slim.</param>
        /// <param name="y">The right type slim.</param>
        /// <returns>true if the given type slims are equal, false otherwise.</returns>
        protected virtual bool EqualsSimple(SimpleTypeSlim x, SimpleTypeSlim y) => EqualsSimpleBase(x, y);

        /// <summary>
        /// Checks if two base simple type slims are equal.  This is called from the default
        /// equality implementation for both the simple and generic definition type slims.
        /// </summary>
        /// <param name="x">The left type slim.</param>
        /// <param name="y">The right type slim.</param>
        /// <returns>true if the given type slims are equal, false otherwise.</returns>
        protected virtual bool EqualsSimpleBase(SimpleTypeSlimBase x, SimpleTypeSlimBase y) => x.Equals(y);

        /// <summary>
        /// Checks if two array type slims are equal.
        /// </summary>
        /// <param name="x">The left type slim.</param>
        /// <param name="y">The right type slim.</param>
        /// <returns>true if the given type slims are equal, false otherwise.</returns>
        protected virtual bool EqualsArray(ArrayTypeSlim x, ArrayTypeSlim y)
        {
            return Equals(x.ElementType, y.ElementType) && x.Rank == y.Rank;
        }

        private bool EqualsStructural(StructuralTypeSlim x, StructuralTypeSlim y)
        {
            if (x.StructuralKind != y.StructuralKind)
            {
                return false;
            }

            if (_recursiveEquals.TryGetValue(x, out StructuralTypeSlim res) && (object)res == y)
            {
                return true;
            }
            else
            {
                _recursiveEquals.Add(x, y);
            }

            try
            {
                return x.StructuralKind switch
                {
                    StructuralTypeSlimKind.Anonymous => EqualsStructuralAnonymous(x, y),
                    StructuralTypeSlimKind.Record => EqualsStructuralRecord(x, y),
                    _ => EqualsStructuralExtensions(x, y),
                };
            }
            finally
            {
                _recursiveEquals.Remove(x);
            }
        }

        /// <summary>
        /// Checks if two anonymous structural type slims are equal.
        /// </summary>
        /// <param name="x">The left type slim.</param>
        /// <param name="y">The right type slim.</param>
        /// <returns>true if the given type slims are equal, false otherwise.</returns>
        protected virtual bool EqualsStructuralAnonymous(StructuralTypeSlim x, StructuralTypeSlim y)
        {
            var xCount = x.Properties.Count;

            if (xCount != y.Properties.Count)
            {
                return false;
            }

            for (int i = 0, n = xCount; i < n; ++i)
            {
                if (!EqualsProperty(x.Properties[i], y.Properties[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks if two record structural type slims are equal.
        /// </summary>
        /// <param name="x">The left type slim.</param>
        /// <param name="y">The right type slim.</param>
        /// <returns>true if the given type slims are equal, false otherwise.</returns>
        protected virtual bool EqualsStructuralRecord(StructuralTypeSlim x, StructuralTypeSlim y)
        {
            var xCount = x.Properties.Count;
            var yCount = y.Properties.Count;

            if (xCount != yCount || x.HasValueEqualitySemantics != y.HasValueEqualitySemantics)
            {
                return false;
            }

            var xProperties = new Dictionary<string, PropertyInfoSlim>(xCount);
            for (var i = 0; i < xCount; ++i)
            {
                var property = x.Properties[i];
                xProperties.Add(property.Name, property);
            }

            for (var i = 0; i < yCount; ++i)
            {
                var yProperty = y.Properties[i];
                if (!xProperties.TryGetValue(yProperty.Name, out PropertyInfoSlim xProperty) || !EqualsProperty(xProperty, yProperty))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks if two structural type slims with custom kinds are equal.  Must be implemented by derived classes using custom structural types.
        /// </summary>
        /// <param name="x">The left type slim.</param>
        /// <param name="y">The right type slim.</param>
        /// <returns>true if the given type slims are equal, false otherwise.</returns>
        protected virtual bool EqualsStructuralExtensions(StructuralTypeSlim x, StructuralTypeSlim y)
        {
            throw new NotSupportedException("To be implemented by subclasses of the comparer using custom structural type slims.");
        }

        /// <summary>
        /// Checks if two properties of structural types are equivalent.
        /// </summary>
        /// <param name="x">The left property info slim.</param>
        /// <param name="y">The right property info slim.</param>
        /// <returns>true if the given property info slims are equal, false otherwise</returns>
        protected virtual bool EqualsProperty(PropertyInfoSlim x, PropertyInfoSlim y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            if (x.Name != y.Name || x.CanWrite != y.CanWrite || !Equals(x.PropertyType, y.PropertyType) || x.IndexParameterTypes.Count != y.IndexParameterTypes.Count)
            {
                return false;
            }

            for (int i = 0, n = x.IndexParameterTypes.Count; i < n; ++i)
            {
                if (!Equals(x.IndexParameterTypes[i], y.IndexParameterTypes[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks if two generic definition type slims are equal.
        /// </summary>
        /// <param name="x">The left type slim.</param>
        /// <param name="y">The right type slim.</param>
        /// <returns>true if the given type slims are equal, false otherwise.</returns>
        protected virtual bool EqualsGenericDefinition(GenericDefinitionTypeSlim x, GenericDefinitionTypeSlim y) => EqualsSimpleBase(x, y);

        /// <summary>
        /// Checks if two generic type slims are equal.
        /// </summary>
        /// <param name="x">The left type slim.</param>
        /// <param name="y">The right type slim.</param>
        /// <returns>true if the given type slims are equal, false otherwise.</returns>
        protected virtual bool EqualsGeneric(GenericTypeSlim x, GenericTypeSlim y)
        {
            if (!Equals(x.GenericTypeDefinition, y.GenericTypeDefinition) || x.GenericArgumentCount != y.GenericArgumentCount)
            {
                return false;
            }

            for (int i = 0, n = x.GenericArgumentCount; i < n; ++i)
            {
                if (!Equals(x.GetGenericArgument(i), y.GetGenericArgument(i)))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks if two generic parameter type slims are equal.
        /// </summary>
        /// <param name="x">The left type slim.</param>
        /// <param name="y">The right type slim.</param>
        /// <returns>true if the given type slims are equal, false otherwise.</returns>
        protected virtual bool EqualsGenericParameter(GenericParameterTypeSlim x, GenericParameterTypeSlim y) => ReferenceEquals(x, y);

        /// <summary>
        /// An extension to equals for new type slim kinds
        /// </summary>
        /// <param name="x">The left type slim.</param>
        /// <param name="y">The right type slim.</param>
        /// <exception cref="NotImplementedException">Always thrown.</exception>
        protected virtual bool EqualsExtensions(TypeSlim x, TypeSlim y)
        {
            throw new NotImplementedException("To be implemented by subclasses of the comparer using custom type slims.");
        }

        #endregion

        #region GetHashCode

        /// <summary>
        /// Gets a hash code of a type slim.
        /// </summary>
        /// <param name="obj">The slim type.</param>
        /// <returns>A hash code for the slim type.</returns>
        public int GetHashCode(TypeSlim obj)
        {
            if (obj == null)
            {
                return 0;
            }

            var res = obj._hashCode;

            if (res != 0)
            {
                return res;
            }

            res = obj.Kind switch
            {
                TypeSlimKind.Simple => GetHashCodeSimple((SimpleTypeSlim)obj),
                TypeSlimKind.Array => GetHashCodeArray((ArrayTypeSlim)obj),
                TypeSlimKind.Structural => GetHashCodeStructural((StructuralTypeSlim)obj),
                TypeSlimKind.GenericDefinition => GetHashCodeGenericDefinition((GenericDefinitionTypeSlim)obj),
                TypeSlimKind.Generic => GetHashCodeGeneric((GenericTypeSlim)obj),
                TypeSlimKind.GenericParameter => GetHashCodeGenericParameter((GenericParameterTypeSlim)obj),
                _ => GetHashCodeExtensions(obj),
            };
            obj._hashCode = res;

            return res;
        }

        /// <summary>
        /// Gets a hash code of a simple type slim.
        /// </summary>
        /// <param name="obj">The slim type.</param>
        /// <returns>A hash code for the slim type.</returns>
        protected virtual int GetHashCodeSimple(SimpleTypeSlim obj) => GetHashCodeSimpleBase(obj);

        /// <summary>
        /// Gets a hash code of a base simple type slim.  This is called from the default
        /// hash code implementation for both the simple and generic definition type slims.
        /// </summary>
        /// <param name="obj">The slim type.</param>
        /// <returns>A hash code for the slim type.</returns>
        protected virtual int GetHashCodeSimpleBase(SimpleTypeSlimBase obj) => obj.GetHashCode();

        /// <summary>
        /// Gets a hash code of an array type slim.
        /// </summary>
        /// <param name="obj">The slim type.</param>
        /// <returns>A hash code for the slim type.</returns>
        protected virtual int GetHashCodeArray(ArrayTypeSlim obj)
        {
            unchecked
            {
                var hash = GetHashCode(obj.ElementType);
                return (int)(hash * Prime) + obj.Rank.GetHashCode();
            }
        }

        private int GetHashCodeStructural(StructuralTypeSlim obj)
        {
            return obj.StructuralKind switch
            {
                StructuralTypeSlimKind.Anonymous => GetHashCodeStructuralAnonymous(obj),
                StructuralTypeSlimKind.Record => GetHashCodeStructuralRecord(obj),
                _ => GetHashCodeStructuralExtensions(obj),
            };
        }

        /// <summary>
        /// Gets a hash code of an anonymous structural type slim.
        /// </summary>
        /// <param name="obj">The slim type.</param>
        /// <returns>A hash code for the slim type.</returns>
        protected virtual int GetHashCodeStructuralAnonymous(StructuralTypeSlim obj)
        {
            unchecked
            {
                if (_visited.Contains(obj))
                {
                    return (int)Prime;
                }
                else
                {
                    try
                    {
                        _visited.Add(obj);

                        var hash = 0;

                        for (int i = 0, n = obj.Properties.Count; i < n; ++i)
                        {
                            // Rather than sort the properties before computing a hash code, addition
                            // is used to ensure that property order does not affect the final result.
                            hash += GetHashCodeProperty(obj.Properties[i]);
                        }

                        hash = (int)(hash * Prime) + obj.HasValueEqualitySemantics.GetHashCode();
                        hash = (int)(hash * Prime) + (int)obj.StructuralKind;

                        return hash;
                    }
                    finally
                    {
                        _visited.Remove(obj);
                    }
                }
            }
        }

        /// <summary>
        /// Gets a hash code of a record structural type slim.
        /// </summary>
        /// <param name="obj">The slim type.</param>
        /// <returns>A hash code for the slim type.</returns>
        protected virtual int GetHashCodeStructuralRecord(StructuralTypeSlim obj)
        {
            unchecked
            {
                if (_visited.Contains(obj))
                {
                    return (int)Prime;
                }
                else
                {
                    try
                    {
                        _visited.Add(obj);

                        var hash = 0;

                        for (int i = 0, n = obj.Properties.Count; i < n; ++i)
                        {
                            // Rather than sort the properties before computing a hash code, addition
                            // is used to ensure that property order does not affect the final result.
                            hash += GetHashCodeProperty(obj.Properties[i]);
                        }

                        hash = (int)(hash * Prime) + obj.HasValueEqualitySemantics.GetHashCode();
                        hash = (int)(hash * Prime) + (int)obj.StructuralKind;

                        return hash;
                    }
                    finally
                    {
                        _visited.Remove(obj);
                    }
                }
            }
        }

        /// <summary>
        /// Gets a hash code of a custom structural type slim.
        /// </summary>
        /// <param name="obj">The slim type.</param>
        /// <returns>A hash code for the slim type.</returns>
        protected virtual int GetHashCodeStructuralExtensions(StructuralTypeSlim obj)
        {
            throw new NotSupportedException("To be implemented by subclasses of the comparer using custom structural type slims.");
        }

        /// <summary>
        /// Gets a hash code of a structural type slim property.
        /// </summary>
        /// <param name="obj">The structural type slim property.</param>
        /// <returns>A hash code for the structural type slim property.</returns>
        protected virtual int GetHashCodeProperty(PropertyInfoSlim obj)
        {
            unchecked
            {
                var hash = 0;

                for (int i = 0, n = obj.IndexParameterTypes.Count; i < n; ++i)
                {
                    hash = (int)(hash * Prime) + GetHashCode(obj.IndexParameterTypes[i]);
                }

                if (obj.PropertyType != null)
                {
                    hash = (int)(hash * Prime) + GetHashCode(obj.PropertyType);
                }

                hash = (int)(hash * Prime) +
#if NET5_0
                    obj.Name.GetHashCode(StringComparison.Ordinal)
#else
                    obj.Name.GetHashCode()
#endif
                    ;
                return (int)(hash * Prime) + obj.CanWrite.GetHashCode();
            }
        }

        /// <summary>
        /// Gets a hash code of a generic definition type slim.
        /// </summary>
        /// <param name="obj">The slim type.</param>
        /// <returns>A hash code for the slim type.</returns>
        protected virtual int GetHashCodeGenericDefinition(GenericDefinitionTypeSlim obj) => GetHashCodeSimpleBase(obj);

        /// <summary>
        /// Gets a hash code of a generic type slim.
        /// </summary>
        /// <param name="obj">The slim type.</param>
        /// <returns>A hash code for the slim type.</returns>
        protected virtual int GetHashCodeGeneric(GenericTypeSlim obj)
        {
            unchecked
            {
                var hash = 0;

                for (int i = 0, n = obj.GenericArgumentCount; i < n; ++i)
                {
                    hash = (int)(hash * Prime) + GetHashCode(obj.GetGenericArgument(i));
                }

                return (int)(hash * Prime) + GetHashCode(obj.GenericTypeDefinition);
            }
        }

        /// <summary>
        /// Gets a hash code of a generic parameter type slim.
        /// </summary>
        /// <param name="obj">The slim type.</param>
        /// <returns>A hash code for the slim type.</returns>
        protected virtual int GetHashCodeGenericParameter(GenericParameterTypeSlim obj)
        {
            // NB: We should consider unbound generic as *potentially* parameters equal and
            //     thus return the same hash code for every one of them. While this results
            //     in collisions, it's not the worst thing, because it has two beneficial
            //     properties:
            //
            //       1. It prevents two different instances of an unbound generic parameter
            //          from influencing equality decisions which are context-sensitive, e.g.
            //          Func<T, int> and Func<T, int> where the two instances of T are
            //          reference-non-equal. If a type contains open generic parameters,
            //          equality decisions should be left to a place with awareness of the
            //          context where bindings can take place (e.g. generic methods).
            //
            //       2. Types that only differ in unbound parameters "unify" for free and
            //          can thus be de-duplicated. E.g. Func<T, int> and Func<T, int> where
            //          the two instances of T are reference-non-equal can be interchanged
            //          at will.

            return GenericParameterTypeSlim.UnboundHashCode;
        }

        /// <summary>
        /// An extension to GetHashCode for new type slim kinds.
        /// </summary>
        /// <param name="obj">The slim type.</param>
        /// <exception cref="NotImplementedException">Always thrown.</exception>
        protected virtual int GetHashCodeExtensions(TypeSlim obj)
        {
            throw new NotImplementedException("To be implemented by subclasses of the comparer using custom type slims.");
        }

        #endregion

        #region Clear

        /// <summary>
        /// Clears the state of the comparator.
        /// </summary>
        public virtual void Clear()
        {
            _recursiveEquals.Clear();
            _visited.Clear();
        }

        #endregion
    }
}
