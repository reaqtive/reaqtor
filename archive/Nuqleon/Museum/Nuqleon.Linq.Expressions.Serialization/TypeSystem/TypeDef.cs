// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2012
//

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.CompilerServices;
using System.Reflection;
using System.Reflection.Emit;

namespace Nuqleon.Linq.Expressions.Serialization.TypeSystem
{
    using Json = Json.Expressions;

    /// <summary>
    /// Abstract base class for type definitions.
    /// </summary>
    internal abstract class TypeDef
    {
        #region Constants

        /// <summary>
        /// Discriminator for simple types.
        /// </summary>
        internal const string SIMPLE = "Type";

        /// <summary>
        /// Discriminator for array types.
        /// </summary>
        internal const string ARRAYELEMENT = "ElementType";

        /// <summary>
        /// Discriminator for generic types.
        /// </summary>
        internal const string GENERICDEFINITION = "GenericType";

        /// <summary>
        /// Discriminator for structural types.
        /// </summary>
        internal const string STRUCTURALMEMBERS = "Members";

        /// <summary>
        /// Discriminator for closure types.
        /// </summary>
        internal const string CLOSUREFIELDS = "Fields";

        /// <summary>
        /// Discriminator for record types.
        /// </summary>
        internal const string RECORDENTRIES = "Entries";

        /// <summary>
        /// Member name for array with keys in an anonymous type.
        /// </summary>
        internal const string KEYS = "Keys";

        #endregion

        #region Fields

        /// <summary>
        /// Cached final CLR type.
        /// </summary>
        private Type _type;

        /// <summary>
        /// Cached compile time CLR type. May contain TypeBuilder references.
        /// </summary>
        private Type _compiled;

        #endregion

        #region Methods

        #region Conversions to CLR types and JSON

        /// <summary>
        /// Compiles the type definition into a CLR type.
        /// </summary>
        /// <param name="compiler">Compiler instance used to reconstruct unknown types at runtime.</param>
        /// <param name="typeResolutionService">Type resolution service. Can be null.</param>
        /// <returns>CLR type representing the type in the type definition; may contain TypeBuilder references.</returns>
        public Type Compile(RuntimeCompiler compiler, ITypeResolutionService typeResolutionService)
        {
            //
            // Even though types that get compiled will protect against double compilation (e.g.
            // structural types), we can avoid significant reflection cost for all other types by
            // doing some caching here.
            //
            if (_compiled == null)
            {
                _compiled = CompileCore(compiler, typeResolutionService);
            }

            return _compiled;
        }

        /// <summary>
        /// Compiles the type definition into a CLR type.
        /// </summary>
        /// <param name="compiler">Compiler instance used to reconstruct unknown types at runtime.</param>
        /// <param name="typeResolutionService">Type resolution service. Can be null.</param>
        /// <returns>CLR type representing the type in the type definition; may contain TypeBuilder references.</returns>
        protected abstract Type CompileCore(RuntimeCompiler compiler, ITypeResolutionService typeResolutionService);

        /// <summary>
        /// Obtains the CLR type for the type definition instance.
        /// </summary>
        /// <returns>CLR type representing the type in the type definition; ready for usage and instantiation.</returns>
        public Type ToType()
        {
            if (_compiled == null)
                throw new InvalidOperationException("Type should be compiled before it can be used.");

            //
            // This is where the reflection cost savings are.
            //
            if (_type == null)
            {
                _type = ToTypeCore();
            }

            return _type;
        }

        /// <summary>
        /// Obtains the CLR type for the type definition instance.
        /// </summary>
        /// <returns>CLR type representing the type in the type definition; ready for usage and instantiation.</returns>
        protected abstract Type ToTypeCore();

        /// <summary>
        /// Gets a JSON representation for the type definition instance.
        /// </summary>
        /// <returns>JSON representation of the type definition.</returns>
        public abstract Json.Expression ToJson();

        #endregion

        #region Parse-based factory

        /// <summary>
        /// Gets a type definition object from a JSON representation.
        /// </summary>
        /// <param name="json">JSON representation of the type definition.</param>
        /// <param name="findRef">Lookup function invoked for type references that occur in the type definition.</param>
        /// <param name="typeResolutionService">Type resolution service. Can be null.</param>
        /// <returns>Type definition for the given JSON representation.</returns>
        public static TypeDef FromJson(Json.Expression json, Func<Json.Expression, TypeRef> findRef, ITypeResolutionService typeResolutionService)
        {
            //
            // NOTE: Keep this in sync with type definition ToJson overrides.
            //
            if (json is Json.ObjectExpression obj)
            {
                var members = obj.Members;
                if (members.ContainsKey(SIMPLE))
                {
                    return SimpleTypeDef.FromJson(obj, typeResolutionService);
                }
                else if (members.ContainsKey(ARRAYELEMENT))
                {
                    return ArrayTypeDef.FromJson(obj, findRef);
                }
                else if (members.ContainsKey(GENERICDEFINITION))
                {
                    return GenericTypeDef.FromJson(obj, findRef);
                }
                else if (members.ContainsKey(STRUCTURALMEMBERS)
                      || members.ContainsKey(CLOSUREFIELDS)
                      || members.ContainsKey(RECORDENTRIES))
                {
                    return StructuralTypeDef.FromJson(obj, findRef);
                }
            }

            throw new InvalidOperationException("Unknown type map entry.");
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// Simple type definition representing a CLR type with no cross-type references (including open generic types).
    /// </summary>
    internal sealed class SimpleTypeDef : TypeDef
    {
        #region Constants

        /// <summary>
        /// Property name for assembly defining the type.
        /// </summary>
        private const string ASSEMBLY = "Assembly";

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new simple type definition for the given CLR type.
        /// </summary>
        /// <param name="type">CLR type to represent in the simple type definition object.</param>
        public SimpleTypeDef(Type type)
        {
            Type = type;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the CLR type represented by the simple type definition instance.
        /// </summary>
        public Type Type { get; }

        #endregion

        #region Methods

        #region Conversions to CLR types and JSON

        /// <summary>
        /// Compiles the type definition into a CLR type. Returns the type wrapped in the simple type definition.
        /// </summary>
        /// <param name="compiler">Compiler instance used to reconstruct unknown types at runtime. Not used for simple type definitions.</param>
        /// <param name="typeResolutionService">Type resolution service. Can be null.</param>
        /// <returns>CLR type representing the type in the type definition.</returns>
        protected override Type CompileCore(RuntimeCompiler compiler, ITypeResolutionService typeResolutionService)
        {
            return Type;
        }

        /// <summary>
        /// Obtains the CLR type for the type definition instance. Returns the type wrapped in the simple type definition.
        /// </summary>
        /// <returns>CLR type representing the type in the type definition.</returns>
        protected override Type ToTypeCore()
        {
            return Type;
        }

        /// <summary>
        /// Gets a JSON representation for the type definition instance.
        /// </summary>
        /// <returns>JSON representation of the type definition.</returns>
        public override Json.Expression ToJson()
        {
            Debug.Assert(typeof(int).Assembly.FullName.StartsWith("mscorlib,", StringComparison.Ordinal));

            //
            // We'll make mscorlib into a well-known assembly, so we don't have to transport the assembly name.
            // Notice this assumes the same version of the framework is used on both sides. Resolution logic for
            // roll-forward is required to resolve type names across framework versions (similar to the CLR's).
            //
            if (Type.Assembly == typeof(int).Assembly)
            {
                return Json.Expression.Object(
                    new Dictionary<string, Json.Expression>
                    {
                        { SIMPLE, Json.Expression.String(Type.FullName) }
                    }
                );
            }
            else
            {
                return Json.Expression.Object(
                    new Dictionary<string, Json.Expression>
                    {
                        { SIMPLE, Json.Expression.String(Type.FullName) },
                        { ASSEMBLY, Json.Expression.String(Type.Assembly.ToString()) }
                    }
                );
            }
        }

        #endregion

        #region Parse-based factory

        /// <summary>
        /// Gets a type definition object from a JSON representation.
        /// </summary>
        /// <param name="json">JSON representation of the type definition.</param>
        /// <param name="typeResolutionService">Type resolution service. Can be null.</param>
        /// <returns>Type definition for the given JSON representation.</returns>
        public static SimpleTypeDef FromJson(Json.ObjectExpression json, ITypeResolutionService typeResolutionService)
        {
            var members = json.Members;

            //
            // This corresponds to the Type.FullName value (see ToJson).
            //
            var fullName = (string)((Json.ConstantExpression)members[SIMPLE]).Value;

            //
            // Try to obtain the Type.Assembly display name (see ToJson) which may not be set if the type was defined in mscorlib.
            // See TypeSpace.FromJson for the addition of mscorlib to the type resolution service.
            //
            var typeName =
                members.TryGetValue(ASSEMBLY, out Json.Expression ret)
                ? fullName + ", " + ResolveAssemblyNameWithRedirect((string)((Json.ConstantExpression)ret).Value, typeResolutionService)
                : fullName;

            //
            // Attempt to load the type by its name. If the load fails, an alternative resolution is attempted.
            //
            var type =
                typeResolutionService?.GetType(typeName)
                ?? Type.GetType(typeName)
                ?? TryResolveSlow(typeName);

            if (type == null)
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Could not load type '{0}'.", typeName));

            return new SimpleTypeDef(type);
        }

        /// <summary>
        /// Resolution for assembly names, honoring an optional type resolution service, and taking versioning of common BCL assemblies into account.
        /// </summary>
        /// <param name="assemblyName">Assembly name from the serialization payload.</param>
        /// <param name="typeResolutionService">Type resolution service. Can be null.</param>
        /// <returns>Assembly name against the current .NET runtime and BCL version.</returns>
        private static string ResolveAssemblyNameWithRedirect(string assemblyName, ITypeResolutionService typeResolutionService)
        {
            //
            // This method only covers the two most commonly used BCL assemblies. Notice mscorlib is taken
            // care of automatically thanks to the omission of its name in simple type definition JSON
            // representations. If the user needs proper assembly load redirection, the Type.GetType call
            // can be intercepted using AppDomain events, or an ITypeResolutionService can be provided.
            //
            if (typeResolutionService != null)
            {
                var name = new AssemblyName(assemblyName);

                var asm = typeResolutionService.GetAssembly(name);
                if (asm != null)
                {
                    return asm.ToString();
                }
            }

            if (assemblyName.StartsWith("System.Core,", StringComparison.Ordinal))
            {
                var asm = typeof(Enumerable).Assembly;
                Debug.Assert(asm.FullName.StartsWith("System.Core,", StringComparison.Ordinal));
                return asm.ToString();
            }

            if (assemblyName.StartsWith("System,", StringComparison.Ordinal))
            {
                var asm = typeof(Uri).Assembly;
                Debug.Assert(asm.FullName.StartsWith("System,", StringComparison.Ordinal));
                return asm.ToString();
            }

            return assemblyName;
        }

        /// <summary>
        /// Dictionary of known types, indexed by their full name.
        /// </summary>
        private static Dictionary<string, Type> s_knownTypes;

        /// <summary>
        /// Attempts to resolve a well-known type by name.
        /// </summary>
        /// <param name="typeName">Name of the type to lookup.</param>
        /// <returns>The requested CLR type if the type is known; otherwise, null.</returns>
        private static Type TryResolveSlow(string typeName)
        {
            //
            // This is a list of types that are commonly used and have been relocated to different
            // assemblies during the evolution of the BCL. Notice this is only used as a fallback
            // resolution strategy that's triggered in case of deserialization of a type table on
            // another runtime version. If proper full-blown type resolution interception is needed,
            // one can hook up AppDomain events.
            //
            s_knownTypes ??= new List<Type>
                {
                    typeof(Func<>),
                    typeof(Func<,>),
                    typeof(Func<,,>),
                    typeof(Func<,,,>),
                    typeof(Action<>),
                    typeof(Action<,>),
                    typeof(Action<,,>),
                    typeof(Action<,,,>),
                }.ToDictionary(t => t.FullName, t => t);

            if (s_knownTypes.TryGetValue(typeName, out Type res))
                return res;

            return null;
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// Type definition for arrays.
    /// </summary>
    internal sealed class ArrayTypeDef : TypeDef
    {
        #region Constants

        /// <summary>
        /// Property name for the array rank.
        /// </summary>
        private const string RANK = "Rank";

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new array type definition for a multi-dimensional array with the given element type.
        /// </summary>
        /// <param name="elementType">Type reference to the array's element type.</param>
        /// <param name="rank">Rank of the array. A value of 1 is used to denote a vector.</param>
        /// <remarks>If the rank is set to 1, a vector array type is created. Multi-dimensional arrays of rank 1 are not supported.</remarks>
        public ArrayTypeDef(TypeRef elementType, int rank = 1)
        {
            ElementType = elementType;
            Rank = rank;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type reference to the array's element type.
        /// </summary>
        public TypeRef ElementType { get; }

        /// <summary>
        /// Gets the rank of the array. A value of 1 is used to denote a vector.
        /// </summary>
        public int Rank { get; }

        #endregion

        #region Methods

        #region Conversions to CLR types and JSON

        /// <summary>
        /// Compiles the type definition into a CLR type.
        /// </summary>
        /// <param name="compiler">Compiler instance used to reconstruct unknown types at runtime.</param>
        /// <param name="typeResolutionService">Type resolution service. Can be null.</param>
        /// <returns>CLR type representing the type in the type definition; may contain TypeBuilder references.</returns>
        protected override Type CompileCore(RuntimeCompiler compiler, ITypeResolutionService typeResolutionService)
        {
            return CreateType(def => def.Compile(compiler, typeResolutionService));
        }

        /// <summary>
        /// Obtains the CLR type for the type definition instance.
        /// </summary>
        /// <returns>CLR type representing the type in the type definition; ready for usage and instantiation.</returns>
        protected override Type ToTypeCore()
        {
            return CreateType(def => def.ToType());
        }

        /// <summary>
        /// Creates an array type.
        /// </summary>
        /// <param name="getType">Function to get a Type from a TypeDef.</param>
        /// <returns>Array type representing the type in the type definition.</returns>
        private Type CreateType(Func<TypeDef, Type> getType)
        {
            var elementType = getType(ElementType.Definition);

            if (Rank != 1)
            {
                return elementType.MakeArrayType(Rank);
            }
            else
            {
                //
                // Notice this distinction is key, because of the CLR's distinction between multi-dimensional array (which can
                // have one dimension, e.g. System.Int32[*]) and vectors (which always have one dimension, e.g. System.Int32[])
                // so we can't use the MakeArrayType(int) overload with a rank value of 1.
                //
                return elementType.MakeArrayType();
            }
        }

        /// <summary>
        /// Gets a JSON representation for the type definition instance.
        /// </summary>
        /// <returns>JSON representation of the type definition.</returns>
        public override Json.Expression ToJson()
        {
            var res = new Dictionary<string, Json.Expression>
            {
                { ARRAYELEMENT, ElementType.ToJson() }
            };

            //
            // A non-trivial rank is recorded. Notice support for multi-dimensional arrays of rank 1 (not available from C#)
            // could be added later by always recording the rank, unless a one-dimensional vector type is represented.
            //
            if (Rank != 1)
            {
                res[RANK] = Json.Expression.Number(Rank.ToString(CultureInfo.InvariantCulture));
            }

            return Json.Expression.Object(res);
        }

        #endregion

        #region Parse-based factory

        /// <summary>
        /// Gets a type definition object from a JSON representation.
        /// </summary>
        /// <param name="json">JSON representation of the type definition.</param>
        /// <param name="findRef">Lookup function invoked for type references that occur in the type definition.</param>
        /// <returns>Type definition for the given JSON representation.</returns>
        public static ArrayTypeDef FromJson(Json.ObjectExpression json, Func<Json.Expression, TypeRef> findRef)
        {
            var members = json.Members;

            var elementType = findRef(members[ARRAYELEMENT]);

            if (members.TryGetValue(RANK, out Json.Expression ret))
            {
                var rank = int.Parse((string)((Json.ConstantExpression)ret).Value, CultureInfo.InvariantCulture);
                return new ArrayTypeDef(elementType, rank);
            }
            else
            {
                return new ArrayTypeDef(elementType, 1);
            }
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// Type definition to closed generic types.
    /// </summary>
    internal sealed class GenericTypeDef : TypeDef
    {
        #region Constants

        /// <summary>
        /// Property name for the generic arguments.
        /// </summary>
        private const string ARGUMENTS = "Arguments";

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new generic type definition.
        /// </summary>
        /// <param name="definition">Type reference to the open generic type definition.</param>
        /// <param name="arguments">Type references to the generic type arguments.</param>
        public GenericTypeDef(TypeRef definition, IEnumerable<TypeRef> arguments)
        {
            Definition = definition;
            Arguments = arguments.ToList().AsReadOnly();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type reference to the open generic type definition.
        /// </summary>
        public TypeRef Definition { get; }

        /// <summary>
        /// Gets the type references to the generic type arguments.
        /// </summary>
        public ReadOnlyCollection<TypeRef> Arguments { get; }

        #endregion

        #region Methods

        #region Conversions to CLR types and JSON

        /// <summary>
        /// Compiles the type definition into a CLR type.
        /// </summary>
        /// <param name="compiler">Compiler instance used to reconstruct unknown types at runtime.</param>
        /// <param name="typeResolutionService">Type resolution service. Can be null.</param>
        /// <returns>CLR type representing the type in the type definition; may contain TypeBuilder references.</returns>
        protected override Type CompileCore(RuntimeCompiler compiler, ITypeResolutionService typeResolutionService)
        {
            return CreateType(def => def.Compile(compiler, typeResolutionService));
        }

        /// <summary>
        /// Obtains the CLR type for the type definition instance.
        /// </summary>
        /// <returns>CLR type representing the type in the type definition; ready for usage and instantiation.</returns>
        protected override Type ToTypeCore()
        {
            return CreateType(def => def.ToType());
        }

        /// <summary>
        /// Creates a closed generic type.
        /// </summary>
        /// <param name="getType">Function to get a Type from a TypeDef.</param>
        /// <returns>Closed generic type representing the type in the type definition.</returns>
        private Type CreateType(Func<TypeDef, Type> getType)
        {
            var def = getType(Definition.Definition);

            var args = (from arg in Arguments
                        select getType(arg.Definition))
                       .ToArray();

            return def.MakeGenericType(args);
        }

        /// <summary>
        /// Gets a JSON representation for the type definition instance.
        /// </summary>
        /// <returns>JSON representation of the type definition.</returns>
        public override Json.Expression ToJson()
        {
            return Json.Expression.Object(
                new Dictionary<string, Json.Expression>
                {
                    { GENERICDEFINITION, Definition.ToJson() },
                    { ARGUMENTS, Json.Expression.Array(/* eager */ from arg in Arguments select arg.ToJson()) }
                }
            );
        }

        #endregion

        #region Parse-based factory

        /// <summary>
        /// Gets a type definition object from a JSON representation.
        /// </summary>
        /// <param name="json">JSON representation of the type definition.</param>
        /// <param name="findRef">Lookup function invoked for type references that occur in the type definition.</param>
        /// <returns>Type definition for the given JSON representation.</returns>
        public static GenericTypeDef FromJson(Json.ObjectExpression json, Func<Json.Expression, TypeRef> findRef)
        {
            var members = json.Members;

            var def = findRef(members[GENERICDEFINITION]);
            var typeArgs = from arg in ((Json.ArrayExpression)members[ARGUMENTS]).Elements
                           select findRef(arg);

            return new GenericTypeDef(def, /* eager */ typeArgs);
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// Definition of a structural type member.
    /// </summary>
    internal class MemberDefinition
    {
        #region Constants

        /// <summary>
        /// Property name for member names.
        /// </summary>
        private const string MEMBERNAME = "Name";

        /// <summary>
        /// Property name for member type references.
        /// </summary>
        private const string MEMBERTYPE = "Type";

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new member definition with the specified name and type.
        /// </summary>
        /// <param name="name">Name of the member.</param>
        /// <param name="type">Type reference to the type of the member.</param>
        public MemberDefinition(string name, TypeRef type)
        {
            Name = name;
            Type = type;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the member.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the type reference to the type of the member.
        /// </summary>
        public TypeRef Type { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the JSON representation of the member definition.
        /// </summary>
        /// <returns>JSON representation of the member definition.</returns>
        public Json.Expression ToJson()
        {
            return Json.Expression.Object(new Dictionary<string, Json.Expression>
            {
                { MEMBERNAME, Json.Expression.String(Name) },
                { MEMBERTYPE, Type.ToJson() },
            });
        }

        /// <summary>
        /// Obtains a member definition object from a JSON representation.
        /// </summary>
        /// <param name="json">JSON representation of the member definition.</param>
        /// <param name="findRef">Lookup function invoked for type references that occur in the member definition.</param>
        /// <returns>Member definition for the given JSON representation.</returns>
        public static MemberDefinition FromJson(Json.Expression json, Func<Json.Expression, TypeRef> findRef)
        {
            var member = (Json.ObjectExpression)json;

            var name = (string)((Json.ConstantExpression)member.Members[MEMBERNAME]).Value;
            var type = findRef(member.Members[MEMBERTYPE]);

            return new MemberDefinition(name, type);
        }

        #endregion
    }

    /// <summary>
    /// Abstract base class for type definitions of structural types, e.g. anonymous types or closures.
    /// </summary>
    internal abstract class StructuralTypeDef : TypeDef
    {
        #region Constants

        /// <summary>
        /// Property name for the original CLR type name (optional).
        /// </summary>
        private const string ORIGINAL = "OriginalName";

        #endregion

        #region Fields

        /// <summary>
        /// Type builder instance used to recreate the structural type definition.
        /// This instance is also used to resolve recursive anonymous type definitions. See ToType, CreateType, and Compile.
        /// </summary>
        private TypeBuilder _typeBuilder;

        /// <summary>
        /// Cached instance of the CLR type corresponding to the structural type definition.
        /// </summary>
        private Type _type;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new structural type with the given members and the specified original CLR type name.
        /// </summary>
        /// <param name="members">Member declarations of the structural type.</param>
        /// <param name="originalName">Original name of the CLR type.</param>
        protected StructuralTypeDef(IEnumerable<MemberDefinition> members, string originalName)
        {
            Members = members.ToList().AsReadOnly();
            OriginalName = originalName;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the member declarations of the structural type.
        /// </summary>
        public ReadOnlyCollection<MemberDefinition> Members { get; }

        /// <summary>
        /// Gets the original name of the CLR type.
        /// </summary>
        public string OriginalName { get; }

        /// <summary>
        /// Gets the discriminator used as a property name in the JSON representation.
        /// </summary>
        protected abstract string Discriminator
        {
            get;
        }

        #endregion

        #region Methods

        #region ToType conversion with helper methods

        /// <summary>
        /// Compiles the type definition into a CLR type.
        /// </summary>
        /// <param name="compiler">Compiler instance used to reconstruct unknown types at runtime.</param>
        /// <param name="typeResolutionService">Type resolution service. Can be null.</param>
        /// <returns>CLR type representing the type in the type definition; may contain TypeBuilder references.</returns>
        protected override Type CompileCore(RuntimeCompiler compiler, ITypeResolutionService typeResolutionService)
        {
            //
            // Attempt to load the type by name and a structural match. To disable this, the OriginalName
            // can be omitted during construction.
            //
            if (OriginalName != null)
            {
                var candidate = typeResolutionService?.GetType(OriginalName) ?? Type.GetType(OriginalName);

                if (candidate != null && IsMatch(candidate))
                {
                    _type = candidate;
                    return _type;
                }
            }

            //
            // This is tricky. To support recursive types, we should make sure the same type builder object
            // gets reused across recursive Compile calls. The CreateType method assigned to _typeBuilder
            // before returning.
            //
            if (_typeBuilder == null)
            {
                CreateType(compiler, typeResolutionService, out _typeBuilder);
            }

            return _typeBuilder;
        }

        /// <summary>
        /// Obtains the CLR type for the type definition instance.
        /// </summary>
        /// <returns>CLR type representing the type in the type definition; ready for usage and instantiation.</returns>
        protected override Type ToTypeCore()
        {
            //
            // If an exact match was found during compilation, return that one.
            //
            if (_type != null)
            {
                return _type;
            }

            return _typeBuilder.CreateType();
        }

        /// <summary>
        /// Creates a structural type using the specified runtime compiler.
        /// </summary>
        /// <param name="compiler">Runtime compiler to use for the creation of the structural type.</param>
        /// <param name="typeResolutionService">Type resolution service. Can be null.</param>
        /// <param name="builder">Type builder with the prepared definition of the structural type. A subsequent call to Compile will perform the final compilation.</param>
        /// <remarks>The builder output parameter should be assigned to before triggering recursion into ToType calls.</remarks>
        protected abstract void CreateType(RuntimeCompiler compiler, ITypeResolutionService typeResolutionService, out TypeBuilder builder);

        /// <summary>
        /// Checks whether the specified type structurally matches the structural type definition.
        /// This includes an exact match of the members and their types, and possibly the accessibility of members.
        /// </summary>
        /// <param name="type">Type to check for a match with the current instance's structure.</param>
        /// <returns>true if the type matches with the current instance's structure; otherwise, false.</returns>
        protected virtual bool IsMatch(Type type)
        {
            return false;
        }

        #endregion

        #region ToJson conversion

        /// <summary>
        /// Gets a JSON representation for the type definition instance.
        /// </summary>
        /// <returns>JSON representation of the type definition.</returns>
        public override Json.Expression ToJson()
        {
            var res = new Dictionary<string, Json.Expression>
            {
                { Discriminator, Json.Expression.Array(/* eager */ Members.Select(member => member.ToJson())) }
            };

            if (!string.IsNullOrEmpty(OriginalName))
            {
                res[ORIGINAL] = Json.Expression.String(OriginalName);
            }

            return Json.Expression.Object(res);
        }

        #endregion

        #region Parse-based factory

        /// <summary>
        /// Gets a type definition object from a JSON representation.
        /// </summary>
        /// <param name="json">JSON representation of the type definition.</param>
        /// <param name="findRef">Lookup function invoked for type references that occur in the type definition.</param>
        /// <returns>Type definition for the given JSON representation.</returns>
        public static StructuralTypeDef FromJson(Json.ObjectExpression json, Func<Json.Expression, TypeRef> findRef)
        {
            var members = json.Members;

            //
            // Factory pattern for subtypes, using an instance creation delegate set based on the detected
            // discriminator property in the JSON object.
            //
            var createInstance = default(Func<IEnumerable<MemberDefinition>, string, StructuralTypeDef>);

            if (members.TryGetValue(STRUCTURALMEMBERS, out Json.Expression ret))
            {
                var keys = default(string[]);

                if (members.TryGetValue(KEYS, out Json.Expression keysValue))
                {
                    keys = ((Json.ArrayExpression)keysValue).Elements.Select(e => (string)((Json.ConstantExpression)e).Value).ToArray();
                }

                createInstance = (fields, originalType) => new AnonymousTypeDef(fields, originalType, keys);
            }
            else if (members.TryGetValue(CLOSUREFIELDS, out ret))
            {
                createInstance = (fields, originalType) => new ClosureTypeDef(fields, originalType);
            }
            else if (members.TryGetValue(RECORDENTRIES, out ret))
            {
                createInstance = (fields, originalType) => new RecordTypeDef(fields);
            }
            else
            {
                throw new InvalidOperationException("Unknown structural type definition.");
            }

            //
            // Getting the member definitions, lazily. Eager expansion follows further on.
            //
            var properties = from member in ((Json.ArrayExpression)ret).Elements
                             select MemberDefinition.FromJson(member, findRef);

            //
            // Instance creation with optional original type name.
            //
            if (members.TryGetValue(ORIGINAL, out Json.Expression original))
            {
                var originalType = (string)((Json.ConstantExpression)original).Value;
                return createInstance(/* eager */ properties, originalType);
            }
            else
            {
                return createInstance(/* eager */ properties, null);
            }
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// Type definition for anonymous types.
    /// </summary>
    internal sealed class AnonymousTypeDef : StructuralTypeDef
    {
        #region Fields

        /// <summary>
        /// Members acting as keys, i.e. participating in equality.
        /// </summary>
        private readonly string[] _keys;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new anonymous type with the given members and the specified original CLR type name.
        /// </summary>
        /// <param name="members">Member declarations of the structural type.</param>
        /// <param name="originalName">Original name of the CLR type.</param>
        /// <param name="keys">Members acting as keys, i.e. participating in equality.</param>
        public AnonymousTypeDef(IEnumerable<MemberDefinition> members, string originalName, string[] keys)
            : base(members, originalName)
        {
            _keys = keys;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the discriminator used as a property name in the JSON representation.
        /// </summary>
        protected override string Discriminator => STRUCTURALMEMBERS;

        #endregion

        #region Methods

        /// <summary>
        /// Creates an anonymous type using the specified runtime compiler.
        /// </summary>
        /// <param name="compiler">Runtime compiler to use for the creation of the anonymous type.</param>
        /// <param name="typeResolutionService">Type resolution service. Can be null.</param>
        /// <param name="builder">Type builder with the prepared definition of the anonymous type. A subsequent call to Compile will perform the final compilation.</param>
        protected override void CreateType(RuntimeCompiler compiler, ITypeResolutionService typeResolutionService, out TypeBuilder builder)
        {
            builder = compiler.GetNewAnonymousTypeBuilder();
            compiler.DefineAnonymousType(builder, Members.Select(m => new KeyValuePair<string, Type>(m.Name, m.Type.Definition.Compile(compiler, typeResolutionService))).ToList(), _keys);
        }

        /// <summary>
        /// Checks whether the specified type structurally matches the anonymous type definition.
        /// This includes an exact match of the members and their types, in the right declaration order.
        /// </summary>
        /// <param name="type">Type to check for a match with the current instance's structure.</param>
        /// <returns>true if the type matches with the current instance's structure; otherwise, false.</returns>
        protected override bool IsMatch(Type type)
        {
            var ctor = GetAnonymousTypeConstructor(type);
            if (ctor != null)
            {
                //
                // We can't use this instance's Members with ToType conversions yet, because
                // dependencies may not have been compiled yet. We'll resort to a name-based
                // match for the time being. Together with an exact full type name match, it
                // should suffice to detect compatible types. We may want to add a flag to
                // the serializer to disable the OriginalName matching behavior.
                //
                var ctorParamNames = ctor.GetParameters().Select(p => p.Name);
                var ourMemberNames = Members.Select(m => m.Name);

                return ctorParamNames.SequenceEqual(ourMemberNames);
            }

            return false;
        }

        /// <summary>
        /// Gets an anonymous type definition for the given type.
        /// </summary>
        /// <param name="type">Type to get an anonymous type definition for.</param>
        /// <param name="register">Recursive registration function, used to register types of properties on the anonymous type.</param>
        /// <param name="result">Anonymous type definition for the given type.</param>
        /// <returns>true if the specified type is a recognized anonymous type; otherwise, false.</returns>
        public static bool TryFromType(Type type, Func<Type, TypeRef> register, out StructuralTypeDef result)
        {
            var ctor = GetAnonymousTypeConstructor(type);
            if (ctor != null)
            {
                var parameters = ctor.GetParameters();
                var properties = type.GetProperties();

                var props = properties.OrderBy(p => p.Name).Select(p => (p.Name, Type: p.PropertyType));
                var paras = parameters.OrderBy(p => p.Name).Select(p => (p.Name, Type: p.ParameterType));

                //
                // Same number of parameters and properties, with the same names and types. Notice the
                // equality comparison of anonymous types is leveraged here.
                //
                if (Enumerable.SequenceEqual(props, paras))
                {
                    var members = from p in parameters
                                  select new MemberDefinition(p.Name, register(p.ParameterType));

                    var keys = properties.Where(p => !p.CanWrite).Select(p => p.Name).ToArray();
                    if (keys.Length == properties.Length)
                    {
                        //
                        // Default to C# behavior where everything is a key.
                        //
                        keys = null;
                    }

                    result = new AnonymousTypeDef(/* eager */ members, type.AssemblyQualifiedName, keys);
                    return true;
                }
            }

            result = null;
            return false;
        }

        /// <summary>
        /// Checks whether a type follows a known anonymous type definition pattern and the type's single
        /// constructor in case a pattern is recognized.
        /// </summary>
        /// <param name="type">Type to check against anonymous type definition patterns.</param>
        /// <returns>Single constructor of the type if it's an anonymous type; otherwise, null.</returns>
        private static ConstructorInfo GetAnonymousTypeConstructor(Type type)
        {
            if (type.IsAnonymousType())
            {
                var ctors = type.GetConstructors();
                if (ctors.Length == 1)
                {
                    return ctors[0];
                }
            }

            return null;
        }

        /// <summary>
        /// Gets a JSON representation for the type definition instance.
        /// </summary>
        /// <returns>JSON representation of the type definition.</returns>
        public override Json.Expression ToJson()
        {
            var res = (Json.ObjectExpression)base.ToJson();

            if (_keys != null)
            {
                var oldMembers = res.Members;
                var newMembers = oldMembers.ToDictionary(kv => kv.Key, kv => kv.Value);
                newMembers[KEYS] = Json.Expression.Array(_keys.Select(key => Json.Expression.String(key)));
                res = Json.Expression.Object(newMembers);
            }

            return res;
        }

        #endregion
    }

    /// <summary>
    /// Type definition for closure types.
    /// </summary>
    internal sealed class ClosureTypeDef : StructuralTypeDef
    {
        #region Constructors

        /// <summary>
        /// Creates a new closure type with the given members and the specified original CLR type name.
        /// </summary>
        /// <param name="members">Member declarations of the structural type.</param>
        /// <param name="originalName">Original name of the CLR type.</param>
        public ClosureTypeDef(IEnumerable<MemberDefinition> members, string originalName)
            : base(members, originalName)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the discriminator used as a property name in the JSON representation.
        /// </summary>
        protected override string Discriminator => CLOSUREFIELDS;

        #endregion

        #region Methods

        /// <summary>
        /// Creates a closure type using the specified runtime compiler.
        /// </summary>
        /// <param name="compiler">Runtime compiler to use for the creation of the closure type.</param>
        /// <param name="typeResolutionService">Type resolution service. Can be null.</param>
        /// <param name="builder">Type builder with the prepared definition of the anonymous type. A subsequent call to Compile will perform the final compilation.</param>
        protected override void CreateType(RuntimeCompiler compiler, ITypeResolutionService typeResolutionService, out TypeBuilder builder)
        {
            builder = compiler.GetNewClosureTypeBuilder();
            compiler.DefineClosureType(builder, Members.Select(m => new KeyValuePair<string, Type>(m.Name, m.Type.Definition.Compile(compiler, typeResolutionService))).ToList());
        }

        /// <summary>
        /// Checks whether the specified type structurally matches the closure type definition.
        /// This includes an exact match of the fields and their types.
        /// </summary>
        /// <param name="type">Type to check for a match with the current instance's structure.</param>
        /// <returns>true if the type matches with the current instance's structure; otherwise, false.</returns>
        protected override bool IsMatch(Type type)
        {
            //
            // We can't use this instance's Members with ToType conversions yet, because
            // dependencies may not have been compiled yet. We'll resort to a name-based
            // match for the time being. Together with an exact full type name match, it
            // should suffice to detect compatible types. We may want to add a flag to
            // the serializer to disable the OriginalName matching behavior.
            //
            var typeFieldNames = type.GetFields().Select(f => f.Name);
            var ourMemberNames = Members.Select(m => m.Name);

            return typeFieldNames.SequenceEqual(ourMemberNames);
        }

        /// <summary>
        /// Gets a closure type definition for the given type.
        /// </summary>
        /// <param name="type">Type to get a closure type definition for.</param>
        /// <param name="register">Recursive registration function, used to register types of fields on the closure type.</param>
        /// <param name="result">Closure type definition for the given type.</param>
        /// <returns>true if the specified type is a recognized closure type; otherwise, false.</returns>
        public static bool TryFromType(Type type, Func<Type, TypeRef> register, out StructuralTypeDef result)
        {
            if (type.IsClosureClass())
            {
                var fields = type.GetFields();

                var members = fields.Select(f => new MemberDefinition(f.Name, register(f.FieldType)));

                result = new ClosureTypeDef(/* eager */ members, type.AssemblyQualifiedName);
                return true;
            }

            result = null;
            return false;
        }

        #endregion
    }

    /// <summary>
    /// Type definition for record types.
    /// </summary>
    internal sealed class RecordTypeDef : StructuralTypeDef
    {
        #region Constructors

        /// <summary>
        /// Creates a new record type with the given members and the specified original CLR type name.
        /// </summary>
        /// <param name="members">Member declarations of the structural type.</param>
        public RecordTypeDef(IEnumerable<MemberDefinition> members)
            : base(members, originalName: null)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the discriminator used as a property name in the JSON representation.
        /// </summary>
        protected override string Discriminator => RECORDENTRIES;

        #endregion

        #region Methods

        /// <summary>
        /// Creates a record type using the specified runtime compiler.
        /// </summary>
        /// <param name="compiler">Runtime compiler to use for the creation of the record type.</param>
        /// <param name="typeResolutionService">Type resolution service. Can be null.</param>
        /// <param name="builder">Type builder with the prepared definition of the anonymous type. A subsequent call to Compile will perform the final compilation.</param>
        protected override void CreateType(RuntimeCompiler compiler, ITypeResolutionService typeResolutionService, out TypeBuilder builder)
        {
            builder = compiler.GetNewRecordTypeBuilder();
            compiler.DefineRecordType(builder, Members.Select(m => new KeyValuePair<string, Type>(m.Name, m.Type.Definition.Compile(compiler, typeResolutionService))).ToList(), valueEquality: true);
        }

        /// <summary>
        /// Gets a closure type definition for the given type.
        /// </summary>
        /// <param name="type">Type to get a closure type definition for.</param>
        /// <param name="register">Recursive registration function, used to register types of fields on the closure type.</param>
        /// <param name="result">Closure type definition for the given type.</param>
        /// <returns>true if the specified type is a recognized closure type; otherwise, false.</returns>
        public static bool TryFromType(Type type, Func<Type, TypeRef> register, out StructuralTypeDef result)
        {
            if (type.IsRecordType())
            {
                var properties = type.GetProperties();

                var members = properties.Select(f => new MemberDefinition(f.Name, register(f.PropertyType)));

                result = new RecordTypeDef(/* eager */ members);
                return true;
            }

            result = null;
            return false;
        }

        #endregion
    }
}
