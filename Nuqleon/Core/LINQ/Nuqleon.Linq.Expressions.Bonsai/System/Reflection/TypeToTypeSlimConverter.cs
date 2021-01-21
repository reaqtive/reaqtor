// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
// ER - July 2013 - Small tweaks.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Omitted null checks similar to expression tree visitors.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Linq.CompilerServices;

namespace System.Reflection
{
    /// <summary>
    /// A type visitor that visits CLR types and returns slim representations of those types.
    /// </summary>
    public class TypeToTypeSlimConverter : TypeVisitor<TypeSlim>
    {
        #region Fields

        private static readonly ReadOnlyCollection<TypeSlim> s_noArgs = EmptyReadOnlyCollection<TypeSlim>.Instance;

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable format // (Formatted as a table.)

        /*
         * <WARNING> - Dear window dressers, please don't reorder these field initializers blindly.
         */

        private static readonly AssemblySlim s_mscorlibSlim = new(typeof(int).Assembly.FullName);
        private static readonly AssemblySlim s_systemSlim   = new(typeof(Uri).Assembly.FullName);

        private static readonly Dictionary<Assembly, AssemblySlim> s_coreAssemblies = new(2)
        {
            { typeof(int).Assembly, s_mscorlibSlim },
            { typeof(Uri).Assembly, s_systemSlim   },
        };

        private static readonly Dictionary<Type, TypeSlim> s_primitiveTypes = new(12)
        {
            { typeof(object),         TypeSlim.Simple(s_mscorlibSlim, "System.Object")         },
            { typeof(int),            TypeSlim.Simple(s_mscorlibSlim, "System.Int32")          },
            { typeof(long),           TypeSlim.Simple(s_mscorlibSlim, "System.Int64")          },
            { typeof(float),          TypeSlim.Simple(s_mscorlibSlim, "System.Single")         },
            { typeof(double),         TypeSlim.Simple(s_mscorlibSlim, "System.Double")         },
            { typeof(bool),           TypeSlim.Simple(s_mscorlibSlim, "System.Boolean")        },
            { typeof(string),         TypeSlim.Simple(s_mscorlibSlim, "System.String")         },
            { typeof(Action),         TypeSlim.Simple(s_mscorlibSlim, "System.Action")         },
            { typeof(TimeSpan),       TypeSlim.Simple(s_mscorlibSlim, "System.TimeSpan")       },
            { typeof(DateTime),       TypeSlim.Simple(s_mscorlibSlim, "System.DateTime")       },
            { typeof(DateTimeOffset), TypeSlim.Simple(s_mscorlibSlim, "System.DateTimeOffset") },
            { typeof(Uri),            TypeSlim.Simple(s_systemSlim,   "System.Uri")            },
        };

        //
        // Not going beyond restricted arities for the ones below; higher arities were relocated
        // at some point from System.Core to mscorlib, so not taking the risk of hardcoding ambiguity.
        //

        private static readonly Dictionary<Type, TypeSlim> s_commonGenericTypes = new(17)
        {
            { typeof(Func<>),         TypeSlim.GenericDefinition(s_mscorlibSlim, "System.Func`1")   },
            { typeof(Func<,>),        TypeSlim.GenericDefinition(s_mscorlibSlim, "System.Func`2")   },
            { typeof(Func<,,>),       TypeSlim.GenericDefinition(s_mscorlibSlim, "System.Func`3")   },
            { typeof(Func<,,,>),      TypeSlim.GenericDefinition(s_mscorlibSlim, "System.Func`4")   },
            { typeof(Func<,,,,>),     TypeSlim.GenericDefinition(s_mscorlibSlim, "System.Func`5")   },

            { typeof(Action<>),       TypeSlim.GenericDefinition(s_mscorlibSlim, "System.Action`1") },
            { typeof(Action<,>),      TypeSlim.GenericDefinition(s_mscorlibSlim, "System.Action`2") },
            { typeof(Action<,,>),     TypeSlim.GenericDefinition(s_mscorlibSlim, "System.Action`3") },
            { typeof(Action<,,,>),    TypeSlim.GenericDefinition(s_mscorlibSlim, "System.Action`4") },

            { typeof(Tuple<>),        TypeSlim.GenericDefinition(s_mscorlibSlim, "System.Tuple`1")  },
            { typeof(Tuple<,>),       TypeSlim.GenericDefinition(s_mscorlibSlim, "System.Tuple`2")  },
            { typeof(Tuple<,,>),      TypeSlim.GenericDefinition(s_mscorlibSlim, "System.Tuple`3")  },
            { typeof(Tuple<,,,>),     TypeSlim.GenericDefinition(s_mscorlibSlim, "System.Tuple`4")  },
            { typeof(Tuple<,,,,>),    TypeSlim.GenericDefinition(s_mscorlibSlim, "System.Tuple`5")  },
            { typeof(Tuple<,,,,,>),   TypeSlim.GenericDefinition(s_mscorlibSlim, "System.Tuple`6")  },
            { typeof(Tuple<,,,,,,>),  TypeSlim.GenericDefinition(s_mscorlibSlim, "System.Tuple`7")  },
            { typeof(Tuple<,,,,,,,>), TypeSlim.GenericDefinition(s_mscorlibSlim, "System.Tuple`8")  },
        };

        /*
         * </WARNING>
         */

#pragma warning restore format
#pragma warning restore IDE0079

        private readonly Dictionary<Type, TypeSlim> _typeCache;
        private readonly Dictionary<Assembly, AssemblySlim> _assemblies;
        private readonly Stack<IDictionary<Type, TypeSlim>> _genericParameterContext;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates the type to type slim converter.
        /// </summary>
        public TypeToTypeSlimConverter()
        {
            _typeCache = new Dictionary<Type, TypeSlim>();
            _assemblies = new Dictionary<Assembly, AssemblySlim>();
            _genericParameterContext = new Stack<IDictionary<Type, TypeSlim>>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the cache of previously converted types.
        /// </summary>
        protected IDictionary<Type, TypeSlim> TypeCache => _typeCache;

        #endregion

        #region Methods

        #region Visitor Methods

        /// <summary>
        /// Visits a CLR type and returns a slim representation.
        /// </summary>
        /// <param name="type">Type to visit.</param>
        /// <returns>Slim representation of the CLR type.</returns>
        public override TypeSlim Visit(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (_typeCache.TryGetValue(type, out TypeSlim res))
            {
                return res;
            }

            if (type.IsAnonymousType())
            {
                return VisitAnonymousType(type);
            }
            else if (type.IsRecordType())
            {
                return VisitRecordType(type);
            }
            else
            {
                res = base.Visit(type);

                if (!type.ContainsGenericParameters || type.IsGenericTypeDefinition())
                {
                    _typeCache[type] = res;
                }

                return res;
            }
        }

        /// <summary>
        /// Constructs a single-dimensional vector array type slim from the specified element type.
        /// </summary>
        /// <param name="type">The original type.</param>
        /// <param name="elementType">Element type slim of the array.</param>
        /// <returns>Slim representation of a single-dimensional vector array type with the given element type.</returns>
        protected override TypeSlim MakeArrayType(Type type, TypeSlim elementType)
        {
            return TypeSlim.Array(elementType);
        }

        /// <summary>
        /// Constructs a multi-dimensional array type slim from the specified element type and rank.
        /// </summary>
        /// <param name="type">The original type.</param>
        /// <param name="elementType">Element type slim of the array.</param>
        /// <param name="rank">Rank (the number of dimensions) of the array.</param>
        /// <returns>Slim representation of a multi-dimensional array type with the given element type and rank.</returns>
        protected override TypeSlim MakeArrayType(Type type, TypeSlim elementType, int rank)
        {
            return TypeSlim.Array(elementType, rank);
        }

        /// <summary>
        /// Visits a generic parameter and returns a slim representation.
        /// </summary>
        /// <param name="type">Generic parameter to visit.</param>
        /// <returns>Slim representation of the generic parameter.</returns>
        protected override TypeSlim VisitGenericParameter(Type type)
        {
            foreach (var map in _genericParameterContext)
            {
                if (map.TryGetValue(type, out TypeSlim result))
                {
                    return result;
                }
            }

            throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Could not find a slim generic parameter type for generic parameter '{0}'.", type));
        }

        /// <summary>
        /// Constructs a closed generic type slim from the specified generic type slim definition and type slim arguments.
        /// </summary>
        /// <param name="type">The original type.</param>
        /// <param name="genericTypeDefinition">Slim generic type definition.</param>
        /// <param name="genericArguments">Slim generic type arguments.</param>
        /// <returns>Slim representation of a closed generic type with the given generic type slim definition and type slim arguments.</returns>
        protected override TypeSlim MakeGenericType(Type type, TypeSlim genericTypeDefinition, params TypeSlim[] genericArguments)
        {
            return TypeSlim.Generic((GenericDefinitionTypeSlim)genericTypeDefinition, genericArguments.ToReadOnly());
        }

        /// <summary>
        /// Visits a generic type definition and returns a slim representation.
        /// </summary>
        /// <param name="type">Generic type definition to visit.</param>
        /// <returns>Slim representation of the generic type definition.</returns>
        protected override TypeSlim VisitGenericTypeDefinition(Type type)
        {
            if (s_commonGenericTypes.TryGetValue(type, out TypeSlim res))
            {
                return res;
            }

            var assembly = MakeAssembly(type.Assembly);
            return TypeSlim.GenericDefinition(assembly, type.FullName);
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <param name="type">Irrelevant.</param>
        /// <param name="elementType">Irrelevant.</param>
        /// <exception cref="NotSupportedException">Always thrown.</exception>
        protected override TypeSlim MakeByRefType(Type type, TypeSlim elementType)
        {
            throw new NotSupportedException("By ref types are not supported.");
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <param name="type">Irrelevant.</param>
        /// <param name="elementType">Irrelevant.</param>
        /// <exception cref="NotSupportedException">Always thrown.</exception>
        protected override TypeSlim MakePointerType(Type type, TypeSlim elementType)
        {
            throw new NotSupportedException("Pointer types are not supported.");
        }

        /// <summary>
        /// Visits a type with no internal structure and returns a slim representation.
        /// </summary>
        /// <param name="type">Simple type to visit.</param>
        /// <returns>Slim representation of the simple type.</returns>
        protected override TypeSlim VisitSimple(Type type)
        {
            if (s_primitiveTypes.TryGetValue(type, out TypeSlim res))
            {
                return res;
            }

            var assembly = MakeAssembly(type.Assembly);
            return TypeSlim.Simple(assembly, type.FullName);
        }

        /// <summary>
        /// Visits a record type and returns a structural slim type representation.
        /// </summary>
        /// <param name="type">The record type.</param>
        /// <returns>The structural slim type representation.</returns>
        protected virtual TypeSlim VisitAnonymousType(Type type)
        {
            var ctor = type.GetConstructors().Single();
            var parameters = ctor.GetParameters();

            var slimType = StructuralTypeSlimReference.Create(hasValueEqualitySemantics: true, StructuralTypeSlimKind.Anonymous, parameters.Length);
            _typeCache[type] = slimType;

            foreach (var param in parameters)
            {
                var property = type.GetProperty(param.Name);
                var slimProperty = slimType.GetProperty(property.Name, Visit(property.PropertyType), s_noArgs, property.CanWrite);
                slimType.AddProperty(slimProperty);
            }

            slimType.Freeze();

            return slimType;
        }

        /// <summary>
        /// Visits a record type and returns a structural slim type representation.
        /// </summary>
        /// <param name="type">The record type.</param>
        /// <returns>The structural slim type representation.</returns>
        protected virtual TypeSlim VisitRecordType(Type type)
        {
            var properties = type.GetProperties();

            //
            // TODO: once IStructuralEquatable is implemented, use that to check for value equality semantics
            //
            var hasValueEqualitySemantics = type.GetMethod("GetHashCode", BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance) != null;
            var slimType = StructuralTypeSlimReference.Create(hasValueEqualitySemantics, StructuralTypeSlimKind.Record, properties.Length);
            _typeCache[type] = slimType;

            foreach (var property in properties)
            {
                var slimProperty = slimType.GetProperty(property.Name, Visit(property.PropertyType), s_noArgs, property.CanWrite);
                slimType.AddProperty(slimProperty);
            }

            slimType.Freeze();

            return slimType;
        }

        private AssemblySlim MakeAssembly(Assembly assembly)
        {
            if (!s_coreAssemblies.TryGetValue(assembly, out AssemblySlim res) && !_assemblies.TryGetValue(assembly, out res))
            {
                res = new AssemblySlim(assembly.FullName);
                _assemblies[assembly] = res;
            }

            return res;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Manually maps a CLR type to a slim type.
        /// </summary>
        /// <param name="type">The CLR type.</param>
        /// <param name="typeSlim">The slim type.</param>
        public void MapType(Type type, TypeSlim typeSlim)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (typeSlim == null)
                throw new ArgumentNullException(nameof(typeSlim));

            if (_typeCache.TryGetValue(type, out TypeSlim res))
            {
                if (res != typeSlim)
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Type '{0}' has already been resolved to '{1}'.", type, res));
                }
            }
            else
            {
                _typeCache[type] = typeSlim;
            }
        }

        /// <summary>
        /// Pushes a frame of generic parameters so the same generic parameter
        /// is used throughout the generic method definition types.
        /// </summary>
        /// <param name="genericParameterMap">The mapping from types to generic parameters.</param>
        public void Push(IDictionary<Type, TypeSlim> genericParameterMap)
        {
            _genericParameterContext.Push(genericParameterMap);
        }

        /// <summary>
        /// Pops a frame from the generic parameter context.
        /// </summary>
        public void Pop()
        {
            _genericParameterContext.Pop();
        }

        #endregion

        #endregion
    }
}
