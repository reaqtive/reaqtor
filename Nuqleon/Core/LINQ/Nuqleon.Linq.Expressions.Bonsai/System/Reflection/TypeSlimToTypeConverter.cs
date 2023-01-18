// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - July 2013 - Created this file.
//

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Linq.CompilerServices;

namespace System.Reflection
{
    /// <summary>
    /// A type slim visitor that recursively visits slim types and returns the CLR types they represent.
    /// </summary>
    internal class TypeSlimToTypeConverter : TypeSlimVisitor<Type, Type, Type, Type, Type, Type, Type>
    {
        #region Fields

        private readonly IReflectionProvider _provider;
        private readonly Dictionary<TypeSlim, Type> _typeMap;
        private readonly Stack<Dictionary<TypeSlim, Type>> _genericParameterContext;

        // TODO: Implement cache eviction policies
        private static readonly ConcurrentDictionary<SlimName, Type> s_simpleTypeCache = new();

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a type slim to type converter.
        /// </summary>
        /// <param name="provider">The reflection provider to use.</param>
        public TypeSlimToTypeConverter(IReflectionProvider provider)
        {
            _provider = provider;
            _typeMap = new Dictionary<TypeSlim, Type>();
            _genericParameterContext = new Stack<Dictionary<TypeSlim, Type>>();
        }

        #endregion

        #region Methods

        #region Visitor Methods

        /// <summary>
        /// Visits a type slim and returns a CLR type.
        /// </summary>
        /// <param name="type">Type slim to visit.</param>
        /// <returns>CLR type represented by the slim type.</returns>
        public override Type Visit(TypeSlim type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (_typeMap.TryGetValue(type, out Type res))
            {
                return res;
            }

            if (type.Kind == TypeSlimKind.Structural)
            {
                res = VisitStructural((StructuralTypeSlim)type);
            }
            else
            {
                res = base.Visit(type);
            }

            if (res == null)
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Could not get CLR type represented by slim type '{0}'.", type));

            if (!res.ContainsGenericParameters)
            {
                _typeMap[type] = res;
            }

            return res;
        }

        /// <summary>
        /// Visits a simple type slim and returns a CLR type.
        /// </summary>
        /// <param name="type">Original type.</param>
        /// <returns>CLR type represented by the slim type.</returns>
        protected override Type VisitSimple(SimpleTypeSlim type)
        {
            return TypeResolve(type);
        }

        /// <summary>
        /// Constructs an array type with the specified element type and rank.
        /// </summary>
        /// <param name="type">Original type.</param>
        /// <param name="elementType">Element type of the array.</param>
        /// <param name="rank">Rank of the array, i.e. the number of dimensions.</param>
        /// <returns>CLR type represented by the slim type.</returns>
        protected override Type MakeArrayType(ArrayTypeSlim type, Type elementType, int? rank)
        {
            if (rank is null or 1)
            {
                return _provider.MakeArrayType(elementType);
            }
            else
            {
                return _provider.MakeArrayType(elementType, rank.Value);
            }
        }

        // TODO: Implement cache eviction policies
        private static readonly ConcurrentDictionary<StructuralTypeSlim, Type> s_structuralTypeCache = new();

        /// <summary>
        /// Visits a structural type slim and returns a CLR type.
        /// </summary>
        /// <param name="type">Original type.</param>
        /// <returns>CLR type represented by the slim type.</returns>
        protected override Type VisitStructural(StructuralTypeSlim type)
        {
            return type.StructuralKind switch
            {
                StructuralTypeSlimKind.Anonymous => VisitAnonymous(type),
                StructuralTypeSlimKind.Record => VisitRecord(type),
                _ => throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Conversion of structural type slim of kind '{0}' to CLR type not supported.", type.StructuralKind)),
            };
        }

        // TODO: visit types in original order
        private Type VisitAnonymous(StructuralTypeSlim type)
        {
            return s_structuralTypeCache.GetOrAdd(type, VisitAnonymousCore);
        }

        private Type VisitAnonymousCore(StructuralTypeSlim type)
        {
            var rtc = new RuntimeCompiler();
            var typeBuilder = rtc.GetNewAnonymousTypeBuilder();
            _typeMap[type] = typeBuilder;

            var properties = type.Properties;
            var count = properties.Count;

            var newPropertyTypes = new KeyValuePair<string, Type>[count];
            var keys = new List<string>(count);

            for (var i = 0; i < count; i++)
            {
                var property = properties[i];

                newPropertyTypes[i] = new KeyValuePair<string, Type>(property.Name, Visit(property.PropertyType));

                if (!property.CanWrite)
                {
                    keys.Add(property.Name);
                }
            }

            rtc.DefineAnonymousType(
                typeBuilder,
                newPropertyTypes,
                keys.ToArray()
            );

            var newType = typeBuilder.CreateType();
            _typeMap[type] = newType;
            return newType;
        }

        private Type VisitRecord(StructuralTypeSlim type)
        {
            return s_structuralTypeCache.GetOrAdd(type, VisitRecordCore);
        }

        private Type VisitRecordCore(StructuralTypeSlim type)
        {
            var rtc = new RuntimeCompiler();
            var typeBuilder = rtc.GetNewRecordTypeBuilder();
            _typeMap[type] = typeBuilder;

            var properties = type.Properties;
            var count = properties.Count;

            var newPropertyTypes = new KeyValuePair<string, Type>[count];

            for (var i = 0; i < count; i++)
            {
                var property = properties[i];
                newPropertyTypes[i] = new KeyValuePair<string, Type>(property.Name, Visit(property.PropertyType));
            }

            rtc.DefineRecordType(
                typeBuilder,
                newPropertyTypes,
                type.HasValueEqualitySemantics
            );

            var newType = typeBuilder.CreateType();
            _typeMap[type] = newType;
            return newType;
        }

        /// <summary>
        /// Method not used for this derived class.
        /// </summary>
        /// <param name="type">Irrelevant.</param>
        /// <param name="propertyTypes">Irrelevant.</param>
        /// <param name="propertyIndexParameters">Irrelevant.</param>
        /// <exception cref="InvalidOperationException">Always thrown by this derived class.</exception>
        protected override Type MakeStructuralType(StructuralTypeSlim type, IEnumerable<KeyValuePair<PropertyInfoSlim, Type>> propertyTypes, IEnumerable<KeyValuePair<PropertyInfoSlim, ReadOnlyCollection<Type>>> propertyIndexParameters)
        {
            throw new InvalidOperationException("This derived class should not be using this method.");
        }

        /// <summary>
        /// Constructs an open generic type definition with the specified parameter types.
        /// </summary>
        /// <param name="type">Original type.</param>
        /// <returns>CLR type represented by the slim type.</returns>
        protected override Type MakeGenericDefinition(GenericDefinitionTypeSlim type)
        {
            return TypeResolve(type);
        }

        /// <summary>
        /// Constructs a closed generic type with the specified type definition and type arguments.
        /// </summary>
        /// <param name="type">Original type.</param>
        /// <param name="typeDefinition">Generic type definition.</param>
        /// <param name="arguments">Generic type arguments.</param>
        /// <returns>CLR type represented by the type slim.</returns>
        protected override Type MakeGeneric(GenericTypeSlim type, Type typeDefinition, ReadOnlyCollection<Type> arguments)
        {
            return _provider.MakeGenericType(typeDefinition, arguments.ToArray());
        }

        /// <summary>
        /// Visits a generic parameter type slim and returns a CLR type.
        /// </summary>
        /// <param name="type">Type slim to visit.</param>
        /// <returns>CLR type represented by the slim type.</returns>
        protected override Type VisitGenericParameter(GenericParameterTypeSlim type)
        {
            foreach (var context in _genericParameterContext)
            {
                if (context.TryGetValue(type, out Type res))
                {
                    return res;
                }
            }

            throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Did not find a CLR type corresponding to the generic parameter type '{0}'.", type.Name));
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Loads the appropriate CLR type from the TypeSlim
        /// </summary>
        /// <param name="type">The slim type.</param>
        /// <returns>The CLR type represented by the slim type.</returns>
        protected virtual Type TypeResolve(SimpleTypeSlimBase type)
        {
            var name = new SlimName(type.Assembly.Name, type.Name);
            return s_simpleTypeCache.GetOrAdd(name, GetType);
        }

        private Type GetType(SlimName name)
        {
            var assemblyQualifiedTypeName = name.ToString();
            return _provider.GetType(assemblyQualifiedTypeName, throwOnError: true);
        }

        /// <summary>
        /// Manually maps slim types to CLR types.
        /// </summary>
        /// <param name="typeSlim">The slim type.</param>
        /// <param name="type">The CLR type.</param>
        public void MapType(TypeSlim typeSlim, Type type)
        {
            if (typeSlim == null)
                throw new ArgumentNullException(nameof(typeSlim));
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (_typeMap.TryGetValue(typeSlim, out Type res))
            {
                if (res != type)
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Cannot resolve type '{0}' to '{1}'. The type has already been resolved to '{2}'.", typeSlim, type, res));
            }
            else
            {
                _typeMap[typeSlim] = type;
            }
        }

        /// <summary>
        /// Pushes a generic parameter mapping onto the visitor environment.
        /// </summary>
        /// <param name="context">A mapping from generic parameter type slims to CLR generic parameter types.</param>
        public void Push(Dictionary<TypeSlim, Type> context)
        {
            _genericParameterContext.Push(context);
        }

        /// <summary>
        /// Pops a generic parameter mapping from the visitor environment.
        /// </summary>
        public void Pop()
        {
            _genericParameterContext.Pop();
        }

        #endregion

        #endregion

        #region Types

        /// <summary>
        /// Slim representation of an assembly name and type name pair.
        /// </summary>
        private readonly struct SlimName : IEquatable<SlimName>
        {
            public SlimName(string assemblyName, string typeName)
            {
                AssemblyName = assemblyName;
                TypeName = typeName;
            }

            public string AssemblyName { get; }
            public string TypeName { get; }

            public override bool Equals(object obj) => obj is SlimName name && Equals(name);

            public bool Equals(SlimName name)
            {
                // NB: For compat, we're using OrdinalIgnoreCase.

                var comp = StringComparer.OrdinalIgnoreCase;

                return comp.Equals(name.AssemblyName, AssemblyName) && comp.Equals(name.TypeName, TypeName);
            }

            public override int GetHashCode()
            {
                // NB: For compat, we're using OrdinalIgnoreCase.

                var comp = StringComparer.OrdinalIgnoreCase;

                var h1 = comp.GetHashCode(AssemblyName);
                var h2 = comp.GetHashCode(TypeName);

                uint rol5 = ((uint)h1 << 5) | ((uint)h1 >> 27);
                return ((int)rol5 + h1) ^ h2;
            }

            public override string ToString() => TypeName + ", " + AssemblyName;
        }

        #endregion
    }
}
