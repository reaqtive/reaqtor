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
using System.ComponentModel.Design;
using System.Globalization;
using System.Linq;
using System.Linq.CompilerServices;

namespace Nuqleon.Linq.Expressions.Serialization.TypeSystem
{
    using Json = Nuqleon.Json.Expressions;

    /// <summary>
    /// Defines a closed space for a set of related types, containing their type definitions and cross-type references.
    /// </summary>
    internal class TypeSpace
    {
        #region Fields

        /// <summary>
        /// Mapping of types onto type references in the type space.
        /// </summary>
        private readonly Dictionary<Type, TypeRef> _typeRefs;

        /// <summary>
        /// Mapping of type references (indexed by ordinal) onto type definitions.
        /// </summary>
        private readonly Dictionary<TypeRef, TypeDef> _typeDefs;

        /// <summary>
        /// Runtime compiler to reconstruct structural types during deserialization.
        /// </summary>
        private readonly RuntimeCompiler _compiler;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new type space with no initial type definitions.
        /// </summary>
        public TypeSpace()
        {
            _typeRefs = new Dictionary<Type, TypeRef>();
            _typeDefs = new Dictionary<TypeRef, TypeDef>();
            _compiler = new RuntimeCompiler();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Registers a type in the type space, returning a type reference. The same type will be mapped onto the same reference.
        /// </summary>
        /// <param name="type">Type to register in the type space.</param>
        /// <returns>Reference to the registered type. If the given type already exists in the type space, an existing reference will be returned.</returns>
        public TypeRef Register(Type type)
        {
            if (!_typeRefs.TryGetValue(type, out TypeRef res))
            {
                res = new TypeRef(_typeRefs.Count);
                _typeRefs[type] = res;

                var def = GetTypeDef(type);
                res.Definition = def;
                _typeDefs[res] = def;
            }

            return res;
        }

        /// <summary>
        /// Finds the CLR type for the given type reference.
        /// </summary>
        /// <param name="typeRef">Type reference to find the CLR type for.</param>
        /// <returns>CLR type corresponding to the given type reference.</returns>
        public Type Lookup(TypeRef typeRef)
        {
            return _typeDefs[typeRef].ToType();
        }

        /// <summary>
        /// Gets a JSON representation for the type space instance.
        /// </summary>
        /// <returns>JSON representation of the type space.</returns>
        public Json.Expression ToJson()
        {
            return Json.Expression.Array(_typeRefs.Values.OrderBy(r => r.Ordinal).Select(r => r.Definition.ToJson()).ToArray());
        }

        /// <summary>
        /// Gets a type space object from a JSON representation.
        /// </summary>
        /// <param name="expression">JSON representation of the type space.</param>
        /// <param name="typeResolutionService">Type resolution service. Can be null.</param>
        /// <returns>Type space for the given JSON representation.</returns>
        public static TypeSpace FromJson(Json.Expression expression, ITypeResolutionService typeResolutionService)
        {
            //
            // Referencing mscorlib for default lookups. See SimpleTypeDef.FromJson for the
            // special treatment of this assembly in order to shorten names in serialization
            // payload.
            //
            typeResolutionService?.ReferenceAssembly(typeof(int).Assembly.GetName() /* mscorlib */);

            //
            // Inverse of ToJson.
            //
            var defs = ((Json.ArrayExpression)expression).Elements;

            //
            // First, get enough type reference objects to support the full range of ordinals.
            //
            var typeRefs = Enumerable.Range(0, defs.Count).Select(i => new TypeRef(i)).ToArray();

            //
            // Using a findRef delegate, we can now import the type definitions. Notice this
            // assumes that type references are in a fixed zero-based range.
            //
            var findRef = new Func<Json.Expression, TypeRef>(r => typeRefs[TypeRef.FromJson(r).Ordinal]);
            var typeDefs = defs.Select(def => TypeDef.FromJson(def, findRef, typeResolutionService)).ToArray();

            //
            // Next, we associate all of the type references with their corresponding definition,
            // again based on ordinal number indexes.
            //
            var j = 0;
            foreach (var typeDef in typeDefs)
            {
                var typeRef = typeRefs[j++];
                typeRef.Definition = typeDef;
            }

            //
            // The first phase of defining the type space consists of compilation of the types,
            // resulting in recreation of structural types. Entries are added to the type def
            // table to allow for Lookup calls during deserialization of the expression tree.
            //
            var res = new TypeSpace();
            var structuralTypes = new List<StructuralTypeDef>();

            foreach (var typeRef in typeRefs)
            {
                var def = typeRef.Definition;
                res._typeDefs[typeRef] = def;
                def.Compile(res._compiler, typeResolutionService);

                //
                // Structural types need a separate compilation stage in order to "freeze" the
                // reconstructed types. See comment below for more information.
                //
                if (def is StructuralTypeDef std)
                {
                    structuralTypes.Add(std);
                }
            }

            //
            // Second stage of compilation, triggering the CreateType on TypeBuilder to cause
            // final construction of the types.
            //
            foreach (var structuralType in structuralTypes)
            {
                structuralType.ToType(); // side-effect described above
            }

            //
            // The following piece of code can be re-enabled for debugging purposes in order to
            // see a Type-based index for type references. This is only needed for serialization,
            // so we eliminate this step for deserialization.
            //
            /*
            foreach (var typeRef in typeRefs)
            {
                var type = typeRef.Definition.ToType();
                res._typeRefs[type] = typeRef;
            }
             */

            return res;
        }

        /// <summary>
        /// Gets a type definition for the given CLR type in the current type space. All referenced types will be registered as well.
        /// </summary>
        /// <param name="type">CLR type to get a type definition for in the current type space.</param>
        /// <returns>Type definition in the current type space.</returns>
        private TypeDef GetTypeDef(Type type)
        {
            if (AnonymousTypeDef.TryFromType(type, Register, out StructuralTypeDef res) || ClosureTypeDef.TryFromType(type, Register, out res) || RecordTypeDef.TryFromType(type, Register, out res))
            {
                return res;
            }
            else if (type.IsGenericType && !type.IsGenericTypeDefinition)
            {
                var genericDefinitionRef = Register(type.GetGenericTypeDefinition());
                var genericArgumentsRefs = from arg in type.GetGenericArguments()
                                           select Register(arg);

                return new GenericTypeDef(genericDefinitionRef, /* eager */ genericArgumentsRefs);
            }
            else if (type.IsArray)
            {
                var elementTypeRef = Register(type.GetElementType());
                return new ArrayTypeDef(elementTypeRef, type.GetArrayRank());
            }
            else
            {
                if (type.IsPointer || type.IsByRef || type.IsGenericParameter)
                {
                    throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Unsupported type: '{0}'. The type should not be classified as a pointer (*), a reference (&), or a generic parameter.", type.FullName));
                }

                return new SimpleTypeDef(type);
            }
        }

        #endregion
    }
}
