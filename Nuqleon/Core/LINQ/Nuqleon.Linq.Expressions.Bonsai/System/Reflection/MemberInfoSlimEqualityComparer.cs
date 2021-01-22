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

namespace System.Reflection
{
    /// <summary>
    /// Equality comparer for slim representations of member reflection objects.
    /// </summary>
    public class MemberInfoSlimEqualityComparer : IEqualityComparer<MemberInfoSlim>
    {
        private readonly Func<MemberInfoSlimEqualityComparator> _comparatorFactory;

        /// <summary>
        /// Instantiates the comparer with a type comparer, used to assert equality and get hashcodes
        /// for slim representations of types.
        /// </summary>
        /// <param name="typeComparer">The slim type equality comparer.</param>
        public MemberInfoSlimEqualityComparer(IEqualityComparer<TypeSlim> typeComparer)
        {
            if (typeComparer == null)
                throw new ArgumentNullException(nameof(typeComparer));

            _comparatorFactory = () => new MemberInfoSlimEqualityComparator(typeComparer);
        }

        /// <summary>
        /// Instantiates the comparer with a comparator factory.
        /// </summary>
        /// <param name="comparatorFactory">Generates a comparator to use for equality checks.</param>
        public MemberInfoSlimEqualityComparer(Func<MemberInfoSlimEqualityComparator> comparatorFactory)
        {
            _comparatorFactory = comparatorFactory ?? throw new ArgumentNullException(nameof(comparatorFactory));
        }

        /// <summary>
        /// A default instance of the equality comparer.
        /// </summary>
        public static MemberInfoSlimEqualityComparer Default { get; } = new(TypeSlimEqualityComparer.Default);

        #region Equals

        /// <summary>
        /// Checks if two member slims are equal.
        /// </summary>
        /// <param name="x">The left member slim.</param>
        /// <param name="y">The right member slim.</param>
        /// <returns>true if the given member slims are equal, false otherwise.</returns>
        public bool Equals(MemberInfoSlim x, MemberInfoSlim y) => ReferenceEquals(x, y) || GetComparator().Equals(x, y);

        #endregion

        #region GetHashCode

        /// <summary>
        /// Gets a hash code of a member slim.
        /// </summary>
        /// <param name="obj">The slim member.</param>
        /// <returns>A hash code for the slim member.</returns>
        public int GetHashCode(MemberInfoSlim obj) => GetComparator().GetHashCode(obj);

        #endregion

        internal MemberInfoSlimEqualityComparator GetComparator()
        {
            var comparator = _comparatorFactory();

            if (comparator == null)
            {
                throw new ArgumentException("Factory returned null reference.");
            }

            return comparator;
        }
    }

    /// <summary>
    /// Equality comparer for slim representations of member reflection objects.
    /// </summary>
    public class MemberInfoSlimEqualityComparator : IEqualityComparer<MemberInfoSlim>
    {
        #region Constructors & Fields

        private const uint Prime = 0xa5555529; // See CompilationPass.cpp in C# compiler codebase.

        private IEqualityComparer<TypeSlim> _typeComparer;

        /// <summary>
        /// Instantiates the comparer with a type comparer, used to assert equality and get hashcodes
        /// for slim representations of types.
        /// </summary>
        /// <param name="typeComparer">The slim type equality comparer.</param>
        public MemberInfoSlimEqualityComparator(IEqualityComparer<TypeSlim> typeComparer)
        {
            _typeComparer = typeComparer;
        }

        #endregion

        #region Equals

        /// <summary>
        /// Compares two slim member reflection objects.
        /// </summary>
        /// <param name="x">The left slim member.</param>
        /// <param name="y">The right slim member.</param>
        /// <returns>true if the slim representations are equal, false otherwise.</returns>
        public bool Equals(MemberInfoSlim x, MemberInfoSlim y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            else if (x == null || y == null || x.MemberType != y.MemberType)
            {
                return false;
            }

            if (ReferenceEquals(x, y))
            {
                return true;
            }

            return x.MemberType switch
            {
                MemberTypes.Constructor => EqualsConstructor((ConstructorInfoSlim)x, (ConstructorInfoSlim)y),
                MemberTypes.Custom => EqualsCustom(x, y),
                MemberTypes.Event => EqualsEvent(x, y),
                MemberTypes.Field => EqualsField((FieldInfoSlim)x, (FieldInfoSlim)y),
                MemberTypes.Method => EqualsMethod((MethodInfoSlim)x, (MethodInfoSlim)y),
                MemberTypes.NestedType => EqualsNestedType(x, y),
                MemberTypes.Property => EqualsProperty((PropertyInfoSlim)x, (PropertyInfoSlim)y),
                MemberTypes.TypeInfo => EqualsTypeInfo(x, y),
                _ => EqualsExtension(x, y),
            };
        }

        /// <summary>
        /// Compares two slim constructor reflection objects.
        /// </summary>
        /// <param name="x">The left slim member.</param>
        /// <param name="y">The right slim member.</param>
        /// <returns>true if the slim representations are equal, false otherwise.</returns>
        protected virtual bool EqualsConstructor(ConstructorInfoSlim x, ConstructorInfoSlim y)
        {
            var xCount = x.ParameterTypes.Count;

            if (xCount != y.ParameterTypes.Count || !_typeComparer.Equals(x.DeclaringType, y.DeclaringType))
            {
                return false;
            }

            for (var i = 0; i < xCount; ++i)
            {
                if (!_typeComparer.Equals(x.ParameterTypes[i], y.ParameterTypes[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Compares two slim custom member reflection objects.
        /// </summary>
        /// <param name="x">The left slim member.</param>
        /// <param name="y">The right slim member.</param>
        /// <returns>true if the slim representations are equal, false otherwise.</returns>
        protected virtual bool EqualsCustom(MemberInfoSlim x, MemberInfoSlim y)
        {
            throw new NotImplementedException("This method should be implemented by derived types.");
        }

        /// <summary>
        /// Compares two slim event reflection objects.
        /// </summary>
        /// <param name="x">The left slim member.</param>
        /// <param name="y">The right slim member.</param>
        /// <returns>true if the slim representations are equal, false otherwise.</returns>
        protected virtual bool EqualsEvent(MemberInfoSlim x, MemberInfoSlim y)
        {
            throw new NotSupportedException("Events are not currently supported.");
        }

        /// <summary>
        /// Compares two slim field reflection objects.
        /// </summary>
        /// <param name="x">The left slim member.</param>
        /// <param name="y">The right slim member.</param>
        /// <returns>true if the slim representations are equal, false otherwise.</returns>
        protected virtual bool EqualsField(FieldInfoSlim x, FieldInfoSlim y)
        {
            return x.Name == y.Name && _typeComparer.Equals(x.DeclaringType, y.DeclaringType) && _typeComparer.Equals(x.FieldType, y.FieldType);
        }

        /// <summary>
        /// Compares two slim method reflection objects.
        /// </summary>
        /// <param name="x">The left slim member.</param>
        /// <param name="y">The right slim member.</param>
        /// <returns>true if the slim representations are equal, false otherwise.</returns>
        private bool EqualsMethod(MethodInfoSlim x, MethodInfoSlim y)
        {
            if (x.Kind != y.Kind)
            {
                return false;
            }

            return x.Kind switch
            {
                MethodInfoSlimKind.Simple => EqualsSimpleMethod((SimpleMethodInfoSlim)x, (SimpleMethodInfoSlim)y),
                MethodInfoSlimKind.GenericDefinition => EqualsGenericDefinitionMethod((GenericDefinitionMethodInfoSlim)x, (GenericDefinitionMethodInfoSlim)y),
                MethodInfoSlimKind.Generic => EqualsGenericMethod((GenericMethodInfoSlim)x, (GenericMethodInfoSlim)y),
                _ => EqualsMethodExtension(x, y),
            };
        }

        /// <summary>
        /// Compares two slim base simple method reflection objects.
        /// </summary>
        /// <param name="x">The left slim member.</param>
        /// <param name="y">The right slim member.</param>
        /// <returns>true if the slim representations are equal, false otherwise.</returns>
        protected virtual bool EqualsSimpleMethodBase(SimpleMethodInfoSlimBase x, SimpleMethodInfoSlimBase y)
        {
            if (x.Name != y.Name || x.ParameterTypes.Count != y.ParameterTypes.Count || !_typeComparer.Equals(x.DeclaringType, y.DeclaringType) || !_typeComparer.Equals(x.ReturnType, y.ReturnType))
            {
                return false;
            }

            for (int i = 0, n = x.ParameterTypes.Count; i < n; ++i)
            {
                if (!_typeComparer.Equals(x.ParameterTypes[i], y.ParameterTypes[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Compares two slim simple method reflection objects.
        /// </summary>
        /// <param name="x">The left slim member.</param>
        /// <param name="y">The right slim member.</param>
        /// <returns>true if the slim representations are equal, false otherwise.</returns>
        protected virtual bool EqualsSimpleMethod(SimpleMethodInfoSlim x, SimpleMethodInfoSlim y)
        {
            return EqualsSimpleMethodBase(x, y);
        }

        /// <summary>
        /// Compares two slim generic method definition reflection objects.
        /// </summary>
        /// <param name="x">The left slim member.</param>
        /// <param name="y">The right slim member.</param>
        /// <returns>true if the slim representations are equal, false otherwise.</returns>
        protected virtual bool EqualsGenericDefinitionMethod(GenericDefinitionMethodInfoSlim x, GenericDefinitionMethodInfoSlim y)
        {
            var xParams = x.GenericParameterTypes;
            var yParams = y.GenericParameterTypes;

            if (xParams.Count != yParams.Count)
            {
                return false;
            }

            var original = _typeComparer;
            try
            {
                var n = xParams.Count;

                var genericParameterMap = new Dictionary<GenericParameterTypeSlim, GenericParameterTypeSlim>(n);

                for (var i = 0; i < n; i++)
                {
                    var xpt = (GenericParameterTypeSlim)xParams[i];
                    var ypt = (GenericParameterTypeSlim)yParams[i];
                    genericParameterMap.Add(xpt, ypt);
                }

                _typeComparer = new TypeSlimEqualityComparer(() => new GenericMapTypeSlimEqualityComparatorForEquals(genericParameterMap));

                return EqualsSimpleMethodBase(x, y);
            }
            finally
            {
                _typeComparer = original;
            }
        }

        /// <summary>
        /// Compares two slim generic method reflection objects.
        /// </summary>
        /// <param name="x">The left slim member.</param>
        /// <param name="y">The right slim member.</param>
        /// <returns>true if the slim representations are equal, false otherwise.</returns>
        protected virtual bool EqualsGenericMethod(GenericMethodInfoSlim x, GenericMethodInfoSlim y)
        {
            if (!EqualsMethod(x.GenericMethodDefinition, y.GenericMethodDefinition))
            {
                return false;
            }

            for (int i = 0, n = x.GenericArguments.Count; i < n; ++i)
            {
                if (!_typeComparer.Equals(x.GenericArguments[i], y.GenericArguments[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Compares two slim extension method reflection objects.
        /// </summary>
        /// <param name="x">The left slim member.</param>
        /// <param name="y">The right slim member.</param>
        /// <returns>true if the slim representations are equal, false otherwise.</returns>
        protected virtual bool EqualsMethodExtension(MethodInfoSlim x, MethodInfoSlim y)
        {
            throw new NotImplementedException("This method should be implemented by derived types.");
        }

        /// <summary>
        /// Compares two slim nested type reflection objects.
        /// </summary>
        /// <param name="x">The left slim member.</param>
        /// <param name="y">The right slim member.</param>
        /// <returns>true if the slim representations are equal, false otherwise.</returns>
        protected virtual bool EqualsNestedType(MemberInfoSlim x, MemberInfoSlim y)
        {
            throw new NotSupportedException("Nested types are not currently supported.");
        }

        /// <summary>
        /// Compares two slim property reflection objects.
        /// </summary>
        /// <param name="x">The left slim member.</param>
        /// <param name="y">The right slim member.</param>
        /// <returns>true if the slim representations are equal, false otherwise.</returns>
        protected virtual bool EqualsProperty(PropertyInfoSlim x, PropertyInfoSlim y)
        {
            if (x.Name != y.Name || x.IndexParameterTypes.Count != y.IndexParameterTypes.Count || !_typeComparer.Equals(x.DeclaringType, y.DeclaringType) || !_typeComparer.Equals(x.PropertyType, y.PropertyType))
            {
                return false;
            }

            for (int i = 0, n = x.IndexParameterTypes.Count; i < n; ++i)
            {
                if (!_typeComparer.Equals(x.IndexParameterTypes[i], y.IndexParameterTypes[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Compares two slim type info reflection objects.
        /// </summary>
        /// <param name="x">The left slim member.</param>
        /// <param name="y">The right slim member.</param>
        /// <returns>true if the slim representations are equal, false otherwise.</returns>
        protected virtual bool EqualsTypeInfo(MemberInfoSlim x, MemberInfoSlim y)
        {
            throw new NotSupportedException("Type info is not currently supported.");
        }

        /// <summary>
        /// Compares two slim extension member reflection objects.
        /// </summary>
        /// <param name="x">The left slim member.</param>
        /// <param name="y">The right slim member.</param>
        /// <returns>true if the slim representations are equal, false otherwise.</returns>
        protected virtual bool EqualsExtension(MemberInfoSlim x, MemberInfoSlim y)
        {
            throw new NotImplementedException("This method should be implemented by derived types.");
        }

        #endregion

        #region GetHashCode

        /// <summary>
        /// Gets a hash code of a slim member reflection object.
        /// </summary>
        /// <param name="obj">The slim member.</param>
        /// <returns>The hash code of the slim member.</returns>
        public int GetHashCode(MemberInfoSlim obj)
        {
            if (obj == null)
            {
                return EqualityComparer<MemberInfoSlim>.Default.GetHashCode(obj);
            }

            return obj.MemberType switch
            {
                MemberTypes.Constructor => GetHashCodeConstructor((ConstructorInfoSlim)obj),
                MemberTypes.Custom => GetHashCodeCustom(obj),
                MemberTypes.Event => GetHashCodeEvent(obj),
                MemberTypes.Field => GetHashCodeField((FieldInfoSlim)obj),
                MemberTypes.Method => GetHashCodeMethod((MethodInfoSlim)obj),
                MemberTypes.NestedType => GetHashCodeNestedType(obj),
                MemberTypes.Property => GetHashCodeProperty((PropertyInfoSlim)obj),
                MemberTypes.TypeInfo => GetHashCodeTypeInfo(obj),
                _ => GetHashCodeExtension(obj),
            };
        }

        /// <summary>
        /// Gets a hash code of a slim constructor reflection object.
        /// </summary>
        /// <param name="obj">The slim member.</param>
        /// <returns>The hash code of the slim member.</returns>
        protected virtual int GetHashCodeConstructor(ConstructorInfoSlim obj)
        {
            unchecked
            {
                var hash = _typeComparer.GetHashCode(obj.DeclaringType);

                hash = (int)(hash * Prime) + (int)obj.MemberType;

                for (int i = 0, n = obj.ParameterTypes.Count; i < n; ++i)
                {
                    hash = (int)(hash * Prime) + _typeComparer.GetHashCode(obj.ParameterTypes[i]);
                }

                return hash;
            }
        }

        /// <summary>
        /// Gets a hash code of a slim custom member reflection object.
        /// </summary>
        /// <param name="obj">The slim member.</param>
        /// <returns>The hash code of the slim member.</returns>
        protected virtual int GetHashCodeCustom(MemberInfoSlim obj)
        {
            throw new NotImplementedException("This method should be implemented by derived types.");
        }

        /// <summary>
        /// Gets a hash code of a slim event reflection object.
        /// </summary>
        /// <param name="obj">The slim member.</param>
        /// <returns>The hash code of the slim member.</returns>
        protected virtual int GetHashCodeEvent(MemberInfoSlim obj)
        {
            throw new NotSupportedException("Events are not currently supported.");
        }

        /// <summary>
        /// Gets a hash code of a slim field reflection object.
        /// </summary>
        /// <param name="obj">The slim member.</param>
        /// <returns>The hash code of the slim member.</returns>
        protected virtual int GetHashCodeField(FieldInfoSlim obj)
        {
            unchecked
            {
                var hash = _typeComparer.GetHashCode(obj.DeclaringType);

                hash = (int)(hash * Prime) + (int)obj.MemberType;
                hash = (int)(hash * Prime) +
#if NET5_0
                    obj.Name.GetHashCode(StringComparison.Ordinal)
#else
                    obj.Name.GetHashCode()
#endif
                    ;

                if (obj.FieldType != null)
                {
                    hash = (int)(hash * Prime) + _typeComparer.GetHashCode(obj.FieldType);
                }

                return hash;
            }
        }

        /// <summary>
        /// Gets a hash code of a slim method reflection object.
        /// </summary>
        /// <param name="obj">The slim member.</param>
        /// <returns>The hash code of the slim member.</returns>
        private int GetHashCodeMethod(MethodInfoSlim obj)
        {
            return obj.Kind switch
            {
                MethodInfoSlimKind.Simple => GetHashCodeSimpleMethod((SimpleMethodInfoSlim)obj),
                MethodInfoSlimKind.GenericDefinition => GetHashCodeGenericDefinitionMethod((GenericDefinitionMethodInfoSlim)obj),
                MethodInfoSlimKind.Generic => GetHashCodeGenericMethod((GenericMethodInfoSlim)obj),
                _ => GetHashCodeMethodExtension(obj),
            };
        }

        /// <summary>
        /// Gets a hash code of a slim base simple method reflection object.
        /// </summary>
        /// <param name="obj">The slim member.</param>
        /// <returns>The hash code of the slim member.</returns>
        protected virtual int GetHashCodeSimpleMethodBase(SimpleMethodInfoSlimBase obj)
        {
            unchecked
            {
                var hash = _typeComparer.GetHashCode(obj.DeclaringType);

                hash = (int)(hash * Prime) +
#if NET5_0
                    obj.Name.GetHashCode(StringComparison.Ordinal)
#else
                    obj.Name.GetHashCode()
#endif
                    ;

                if (obj.ReturnType != null)
                {
                    hash = (int)(hash * Prime) + _typeComparer.GetHashCode(obj.ReturnType);
                }

                for (int i = 0, n = obj.ParameterTypes.Count; i < n; ++i)
                {
                    hash = (int)(hash * Prime) + _typeComparer.GetHashCode(obj.ParameterTypes[i]);
                }

                return hash;
            }
        }

        /// <summary>
        /// Gets a hash code of a slim simple method reflection object.
        /// </summary>
        /// <param name="obj">The slim member.</param>
        /// <returns>The hash code of the slim member.</returns>
        protected virtual int GetHashCodeSimpleMethod(SimpleMethodInfoSlim obj)
        {
            return GetHashCodeSimpleMethodBase(obj);
        }

        /// <summary>
        /// Gets a hash code of a slim generic method definition reflection object.
        /// </summary>
        /// <param name="obj">The slim member.</param>
        /// <returns>The hash code of the slim member.</returns>
        protected virtual int GetHashCodeGenericDefinitionMethod(GenericDefinitionMethodInfoSlim obj)
        {
            unchecked
            {
                var original = _typeComparer;
                try
                {
                    var strComp = EqualityComparer<string>.Default;

                    var n = obj.GenericParameterTypes.Count;

                    var genericTypeHashes = new Dictionary<GenericParameterTypeSlim, int>(n);

                    for (var i = 0; i < n; i++)
                    {
                        var t = (GenericParameterTypeSlim)obj.GenericParameterTypes[i];
                        var h = strComp.GetHashCode(t.Name);
                        genericTypeHashes.Add(t, h);
                    }

                    _typeComparer = new TypeSlimEqualityComparer(() => new GenericMapTypeSlimEqualityComparatorForGetHashCode(genericTypeHashes));

                    var hash = GetHashCodeSimpleMethodBase(obj);
                    hash = (int)(hash * Prime) + n;
                    return hash;
                }
                finally
                {
                    _typeComparer = original;
                }
            }
        }

        /// <summary>
        /// Gets a hash code of a slim generic method reflection object.
        /// </summary>
        /// <param name="obj">The slim member.</param>
        /// <returns>The hash code of the slim member.</returns>
        protected virtual int GetHashCodeGenericMethod(GenericMethodInfoSlim obj)
        {
            unchecked
            {
                var hash = GetHashCodeGenericDefinitionMethod(obj.GenericMethodDefinition);

                for (int i = 0, n = obj.GenericArguments.Count; i < n; ++i)
                {
                    hash = (int)(hash * Prime) + _typeComparer.GetHashCode(obj.GenericArguments[i]);
                }

                return hash;
            }
        }

        /// <summary>
        /// Gets a hash code of a slim extension method reflection object.
        /// </summary>
        /// <param name="obj">The slim member.</param>
        /// <returns>The hash code of the slim member.</returns>
        protected virtual int GetHashCodeMethodExtension(MethodInfoSlim obj)
        {
            throw new NotImplementedException("This method should be implemented by derived types.");
        }

        /// <summary>
        /// Gets a hash code of a slim nested type reflection object.
        /// </summary>
        /// <param name="obj">The slim member.</param>
        /// <returns>The hash code of the slim member.</returns>
        protected virtual int GetHashCodeNestedType(MemberInfoSlim obj)
        {
            throw new NotSupportedException("Nested types are not currently supported.");
        }

        /// <summary>
        /// Gets a hash code of a slim property reflection object.
        /// </summary>
        /// <param name="obj">The slim member.</param>
        /// <returns>The hash code of the slim member.</returns>
        protected virtual int GetHashCodeProperty(PropertyInfoSlim obj)
        {
            unchecked
            {
                var hash = _typeComparer.GetHashCode(obj.DeclaringType);

                hash = (int)(hash * Prime) + (int)obj.MemberType;
                hash = (int)(hash * Prime) +
#if NET5_0
                    obj.Name.GetHashCode(StringComparison.Ordinal)
#else
                    obj.Name.GetHashCode()
#endif
                    ;

                if (obj.PropertyType != null)
                {
                    hash = (int)(hash * Prime) + _typeComparer.GetHashCode(obj.PropertyType);
                }

                for (int i = 0, n = obj.IndexParameterTypes.Count; i < n; ++i)
                {
                    hash = (int)(hash * Prime) + _typeComparer.GetHashCode(obj.IndexParameterTypes[i]);
                }

                return hash;
            }
        }

        /// <summary>
        /// Gets a hash code of a slim type info reflection object.
        /// </summary>
        /// <param name="obj">The slim member.</param>
        /// <returns>The hash code of the slim member.</returns>
        protected virtual int GetHashCodeTypeInfo(MemberInfoSlim obj)
        {
            throw new NotSupportedException("Type info are not currently supported.");
        }

        /// <summary>
        /// Gets a hash code of a slim extension member reflection object.
        /// </summary>
        /// <param name="obj">The slim member.</param>
        /// <returns>The hash code of the slim member.</returns>
        protected virtual int GetHashCodeExtension(MemberInfoSlim obj)
        {
            throw new NotImplementedException("This method should be implemented by derived types.");
        }

        #endregion

        #region Generic equality helpers

        private sealed class GenericMapTypeSlimEqualityComparatorForEquals : TypeSlimEqualityComparator
        {
            private readonly Dictionary<GenericParameterTypeSlim, GenericParameterTypeSlim> _genericTypeMap;

            public GenericMapTypeSlimEqualityComparatorForEquals(Dictionary<GenericParameterTypeSlim, GenericParameterTypeSlim> genericTypeMap)
            {
                _genericTypeMap = genericTypeMap;
            }

            protected override bool EqualsGenericParameter(GenericParameterTypeSlim x, GenericParameterTypeSlim y)
            {
                return _genericTypeMap.TryGetValue(x, out GenericParameterTypeSlim z) && z == y;
            }

            //
            // NB: Hash codes for generic parameters are intentionally kept stable by the base class.
            //
        }

        private sealed class GenericMapTypeSlimEqualityComparatorForGetHashCode : TypeSlimEqualityComparator
        {
            private readonly Dictionary<GenericParameterTypeSlim, int> _genericTypeHashes;

            public GenericMapTypeSlimEqualityComparatorForGetHashCode(Dictionary<GenericParameterTypeSlim, int> genericTypeHashes)
            {
                _genericTypeHashes = genericTypeHashes;
            }

            protected override int GetHashCodeGenericParameter(GenericParameterTypeSlim obj)
            {
                if (_genericTypeHashes.TryGetValue(obj, out int res))
                {
                    return res;
                }

                return base.GetHashCodeGenericParameter(obj);
            }
        }

        #endregion
    }
}
