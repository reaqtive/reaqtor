// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - April 2013 - Created this file.
//

using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.CompilerServices;
using System.Linq.CompilerServices.TypeSystem;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System
{
    /// <summary>
    /// Provides a set of extension methods for System.Type.
    /// </summary>
    public static class TypeExtensions
    {
        private static readonly HashSet<Type> s_knownWildcards = new()
        {
            typeof(T),
            typeof(T1),
            typeof(T2),
            typeof(T3),
            typeof(R),
        };

        #region Anonymous and closure types

        /// <summary>
        /// Checks whether the specified type is a compiler-generated closure class.
        /// </summary>
        /// <param name="type">Type to check for closure class implementation.</param>
        /// <returns><c>true</c> if the specified type is a closure class; otherwise, <c>false</c>.</returns>
        public static bool IsClosureClass(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            //
            // Yes, this is ugly. Unfortunately there's no better way to detect closure types. However, in
            // combination with the compiler generated attribute and the naming pattern, this should be safe
            // and rather future proof.
            //
            if (type.Name.StartsWith(Constants.CS_CLOSURE_PREFIX, StringComparison.Ordinal) || type.Name.StartsWith(Constants.VB_CLOSURE_PREFIX, StringComparison.Ordinal))
            {
                return type.IsDefined(typeof(CompilerGeneratedAttribute), inherit: false);
            }

            return false;
        }

        /// <summary>
        /// Checks whether the specified type is a compiler-generated anonymous type.
        /// </summary>
        /// <param name="type">Type to check for anonymous type implementation.</param>
        /// <returns><c>true</c> if the specified type is an anonymous type; otherwise, <c>false</c>.</returns>
        public static bool IsAnonymousType(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            //
            // Yes, this is ugly. Unfortunately there's no better way to detect anonymous types in isolation
            // of an expression tree where member bindings could be spotted. However, in combination with the
            // compiler generated attribute and the naming pattern, this should be safe and rather future
            // proof.
            //
            if (type.Name.StartsWith(Constants.CS_ANONYMOUS_PREFIX, StringComparison.Ordinal) || type.Name.StartsWith(Constants.VB_ANONYMOUS_PREFIX, StringComparison.Ordinal))
            {
                return type.IsDefined(typeof(CompilerGeneratedAttribute), inherit: false);
            }

            return false;
        }

        /// <summary>
        /// Checks whether the specified type is a compiler-generated record type.
        /// </summary>
        /// <param name="type">Type to check for record type implementation.</param>
        /// <returns><c>true</c> if the specified type is a record type; otherwise, <c>false</c>.</returns>
        public static bool IsRecordType(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (type.Name.StartsWith(Constants.RECORD_PREFIX, StringComparison.Ordinal))
            {
                return type.IsDefined(typeof(CompilerGeneratedAttribute), inherit: false);
            }

            return false;
        }

        /// <summary>
        /// Checks whether the specified type is a compiler-generated type.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns><c>true</c> if the specified type is compiler generated; otherwise, <c>false</c>.</returns>
        public static bool IsCompilerGenerated(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return type.IsDefined(typeof(CompilerGeneratedAttribute), inherit: false);
        }

        #endregion

        #region Generic type analysis

        /// <summary>
        /// Gets the closed generic type implemented by the specified type, matching the supplied generic type definition.
        /// </summary>
        /// <param name="type">The type to check for an implementation of the generic type.</param>
        /// <param name="definition">The generic type definition to search for.</param>
        /// <returns>Closed generic type implemented by the given type, matching the given generic type definition.</returns>
        public static Type FindGenericType(this Type type, Type definition)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (definition == null)
                throw new ArgumentNullException(nameof(definition));
            if (!definition.IsGenericTypeDefinition)
                throw new ArgumentException("Expected a generic type definition.", nameof(definition));

            return FindGenericTypeImpl(type, definition);
        }

        /// <summary>
        /// Unifies the specified left-hand side type with the specified right-hand side type, taking reference assignment compatibility in mind.
        /// </summary>
        /// <param name="lhsType">Left-hand side type to unify.</param>
        /// <param name="rhsType">Right-hand type to unify.</param>
        /// <returns>Dictionary containing mappings of wildcards to types to satisfy the assignment.</returns>
        public static IDictionary<Type, Type> UnifyReferenceAssignableFrom(this Type lhsType, Type rhsType)
        {
            IsReferenceAssignableFrom(lhsType, rhsType, out IDictionary<Type, Type> res, throwException: true);
            return res;
        }

        /// <summary>
        /// Checks whether the specified left-hand side type is assignable from the specified right-hand side type, taking wildcard type unification into account.
        /// </summary>
        /// <param name="lhsType">Left-hand side type to check for assignability.</param>
        /// <param name="rhsType">Right-hand type to check for assignability.</param>
        /// <param name="substitutions">Dictionary containing mappings of wildcards to types to satisfy the assignment.</param>
        /// <param name="throwException">Indicates whether to throw an exception upon failing the assignability check.</param>
        /// <returns>true of the right-hand side type is assignable to the left-hand side type; otherwise, false.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the assignment check failed and <paramref name="throwException"/> is set to true.</exception>
        public static bool IsReferenceAssignableFrom(this Type lhsType, Type rhsType, out IDictionary<Type, Type> substitutions, bool throwException = false)
        {
            if (lhsType == null)
                throw new ArgumentNullException(nameof(lhsType));
            if (rhsType == null)
                throw new ArgumentNullException(nameof(rhsType));

            substitutions = new Dictionary<Type, Type>();

            var error = default(string);

            var res = lhsType.IsReferenceAssignableFrom(rhsType, substitutions, ref error);

            if (!res && throwException)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Reference assignability check failed. Cannot assign {0} from {1}. {2}", lhsType, rhsType, error));
            }

            return res;
        }

        /// <summary>
        /// Checks whether the specified left-hand side type is assignable from the specified right-hand side type, taking wildcard type unification into account.
        /// </summary>
        /// <param name="lhsType">Left-hand side type to check for assignability.</param>
        /// <param name="rhsType">Right-hand type to check for assignability.</param>
        /// <param name="substitutions">Dictionary containing mappings of wildcards to types to satisfy the assignment.</param>
        /// <param name="error">Error message set upon failing the assignment check.</param>
        /// <param name="invariant">Indicates whether the recursive assignment compatibility check is dealing with an invariant generic parameter position, requiring an exact type match.</param>
        /// <returns>true of the right-hand side type is assignable to the left-hand side type; otherwise, false.</returns>
        private static bool IsReferenceAssignableFrom(this Type lhsType, Type rhsType, IDictionary<Type, Type> substitutions, ref string error, bool invariant = false)
        {
            //
            // If the two types are equivalent, they are assignable to one another. Bail out quickly.
            //
            if (lhsType.AreEquivalent(rhsType))
            {
                return true;
            }

            //
            // If the LHS type is a wildcard, try to add it to the substitutions.
            //
            if (lhsType.IsWildcard())
            {
                return AddWildcard(lhsType, rhsType, substitutions, ref error);
            }

            //
            // If the RHS type is a wildcard, try to add it to the substitutions.
            //
            if (rhsType.IsWildcard())
            {
                return AddWildcard(rhsType, lhsType, substitutions, ref error);
            }

            //
            // If the LHS and RHS types being checked are in an invariant generic parameter position, we skip the following.
            //
            if (!invariant)
            {
                //
                // Don't check for typical assignment compatibility on value types.
                //
                // NB: We defer the checks for value types until the end, so generic value types can be unified.
                //
                if (!lhsType.IsValueType && !rhsType.IsValueType)
                {
                    //
                    // A more derived class instance can be assigned to a less derived class type.
                    //
                    // NB: Wildcards derive from System.Object but have been checked explicitly above.
                    // NB: Types like T[] are considered a subclass of System.Object[].
                    //
                    if (rhsType.IsSubclassOf(lhsType))
                    {
                        return true;
                    }

                    //
                    // If the assignment target is an interface, the RHS may implement it, so check for that.
                    //
                    // NB: if RHS is an array and LHS is an interface like IList, this will pass.
                    //
                    if (lhsType.IsInterface && rhsType.ImplementInterface(lhsType))
                    {
                        return true;
                    }
                }
            }

            //
            // Check for arrays so we can take covariance into account.
            //
            if (lhsType.IsArray)
            {
                //
                // Nothing can be assigned to an array type, other than arrays themselves.
                //
                if (!rhsType.IsArray)
                {
                    error = string.Format(CultureInfo.InvariantCulture, "{1} is not reference assignable to {0} because it is not an array type.", lhsType, rhsType);
                    return false;
                }

                //
                // Array ranks have to match.
                //
                var lhsRank = lhsType.GetArrayRank();
                var rhsRank = rhsType.GetArrayRank();

                if (lhsRank != rhsRank)
                {
                    error = string.Format(CultureInfo.InvariantCulture, "{1} is not reference assignable to {0} because both types differ in array rank.", lhsType, rhsType);
                    return false;
                }

                //
                // In case the rank is 1, both need to be vectors or both need to be multi-dimensional arrays of rank 1.
                //
                if (lhsRank == 1 && lhsType.IsSzArray() != rhsType.IsSzArray())
                {
                    error = string.Format(CultureInfo.InvariantCulture, "{1} is not reference assignable to {0} because both types have different array kinds (multi-dimensional or vector).", lhsType, rhsType);
                    return false;
                }

                //
                // Arrays are covariant in their element type.
                //
                var lhsElem = lhsType.GetElementType();
                var rhsElem = rhsType.GetElementType();

                return lhsElem.IsReferenceAssignableFrom(rhsElem, substitutions, ref error, invariant);
            }

            //
            // Check for pointer types so we can take covariance into account.
            //
            if (lhsType.IsPointer)
            {
                //
                // Nothing can be assigned to a pointer type, other than pointers themselves.
                //
                if (!rhsType.IsPointer)
                {
                    error = string.Format(CultureInfo.InvariantCulture, "{1} is not reference assignable to {0} because it is not a pointer type.", lhsType, rhsType);
                    return false;
                }

                //
                // Pointers are covariant in their element type.
                //
                var lhsElem = lhsType.GetElementType();
                var rhsElem = rhsType.GetElementType();

                return lhsElem.IsReferenceAssignableFrom(rhsElem, substitutions, ref error, invariant);
            }

            //
            // Check for ref types so we can take covariance into account.
            //
            if (lhsType.IsByRef)
            {
                //
                // Nothing can be assigned to a ref type, other than ref types themselves.
                //
                if (!rhsType.IsByRef)
                {
                    error = string.Format(CultureInfo.InvariantCulture, "{1} is not reference assignable to {0} because it is not a ref type.", lhsType, rhsType);
                    return false;
                }

                //
                // Ref types are covariant in their element type.
                //
                var lhsElem = lhsType.GetElementType();
                var rhsElem = rhsType.GetElementType();

                return lhsElem.IsReferenceAssignableFrom(rhsElem, substitutions, ref error, invariant);
            }

            //
            // Check for generic types to descend into the generic parameters based on variance.
            //
            if (lhsType.IsGenericType)
            {
                //
                // Generic type definitions don't play a role in assignability checks. All types need to be fully closed.
                //
                if (lhsType.IsGenericTypeDefinition || rhsType.IsGenericTypeDefinition)
                {
                    error = string.Format(CultureInfo.InvariantCulture, "{1} is not reference assignable to {0} because any of the types is an open generic type.", lhsType, rhsType);
                    return false;
                }

                var lhsDef = lhsType.GetGenericTypeDefinition();

                //
                // If the LHS and RHS types being checked are in an invariant generic parameter position, the generic types need to be identical.
                // Otherwise, find the implementation of the LHS open generic type on the RHS type.
                //
                if (!invariant)
                {
                    //
                    // NB: This deals with cases like IEnumerable<IGrouping<string, int>> being assigned to IEnumerable<IEnumerable<int>>.
                    //                                            ^^^^^^^^^^^^^^^^^^^^^^                                ^^^^^^^^^^^^^^^^
                    //     In here, we'll find IEnumerable<> on IGrouping<string, int> to find it implements IEnumerable<int>.
                    //
                    var rhsImplType = FindGenericType(rhsType, lhsDef);

                    if (rhsImplType == null)
                    {
                        error = string.Format(CultureInfo.InvariantCulture, "{1} is not reference assignable to {0} because it does not have {2} in its base class hierarchy or interface implementation list.", lhsType, rhsType, lhsDef);
                        return false;
                    }

                    rhsType = rhsImplType;
                }
                else
                {
                    //
                    // NB: This deals with cases like Expression<Func<int, bool>> being assigned to Expression<Func<T, bool>>.
                    //                                           ^^^^^^^^^^^^^^^                               ^^^^^^^^^^^^^
                    //     Because the TDelegate parameter of Expression<TDelegate> is invariant, we need an exact match.
                    //
                    if (!rhsType.IsGenericType)
                    {
                        error = string.Format(CultureInfo.InvariantCulture, "{1} is not reference assignable to {0} because it is not a closed generic type.", lhsType, rhsType);
                        return false;
                    }

                    var rhsDef = rhsType.GetGenericTypeDefinition();

                    if (lhsDef != rhsDef)
                    {
                        error = string.Format(CultureInfo.InvariantCulture, "{1} is not reference assignable to {0} because it does not have the same open generic type definition.", lhsType, rhsType);
                        return false;
                    }
                }

                //
                // Now check all the LHS and RHS generic arguments in a pairwise fashion, taking variance into account when needed.
                //
                var genArgs = lhsDef.GetGenericArguments();

                var lhsArgs = lhsType.GetGenericArguments();
                var rhsArgs = rhsType.GetGenericArguments();

                for (var i = 0; i < genArgs.Length; i++)
                {
                    var genArg = genArgs[i];

                    var lhsArg = lhsArgs[i];
                    var rhsArg = rhsArgs[i];

                    //
                    // If the LHS and RHS types being checked are in an invariant generic parameter position, we skip the variance checks.
                    //
                    if (!invariant)
                    {
                        if (genArg.IsCovariant())
                        {
                            if (!lhsArg.IsReferenceAssignableFrom(rhsArg, substitutions, ref error))
                            {
                                Debug.Assert(error != null, "Recursive call to IsReferenceAssignableFrom should have set error.");
                                return false;
                            }
                        }
                        else if (genArg.IsContravariant())
                        {
                            if (!rhsArg.IsReferenceAssignableFrom(lhsArg, substitutions, ref error))
                            {
                                Debug.Assert(error != null, "Recursive call to IsReferenceAssignableFrom should have set error.");
                                return false;
                            }
                        }
                        else
                        {
                            //
                            // This is where invariant checking starts. Once invariant is set to true, it stays true in the recursion.
                            //
                            // NB: This deals with cases like Expression<Func<int, bool>> being assigned to Expression<Func<T, bool>>.
                            //                                           ^^^^^^^^^^^^^^^                               ^^^^^^^^^^^^^
                            //     Because the TDelegate parameter of Expression<TDelegate> is invariant, we need an exact match.
                            //
                            if (!IsReferenceAssignableFrom(lhsArg, rhsArg, substitutions, ref error, invariant: true))
                            {
                                Debug.Assert(error != null, "Recursive call to IsReferenceAssignableFrom should have set error.");
                                return false;
                            }
                        }
                    }
                    else
                    {
                        //
                        // Continue checking in an invariant manner.
                        //
                        // NB: This deals with cases like Expression<Action<IEnumerable<int>>> being assigned to Expression<Action<IEnumerable<T>>>.
                        //                                                  ^^^^^^^^^^^^^^^^                                       ^^^^^^^^^^^^^^
                        //     Because the TDelegate parameter of Expression<TDelegate> is invariant, we need an exact match all the way down.
                        //
                        if (!IsReferenceAssignableFrom(lhsArg, rhsArg, substitutions, ref error, invariant))
                        {
                            Debug.Assert(error != null, "Recursive call to IsReferenceAssignableFrom should have set error.");
                            return false;
                        }
                    }
                }

                //
                // If we didn't bail out early, the generic type unified correctly.
                //
                return true;
            }

            //
            // If either type is a value type, there's no reference conversion (i.e. no boxing is allowed, and no conversions).
            //
            // NB: Equivalence is already covered above.
            // NB: If the value types are generic, the cases above had a chance to bail out with a positive outcome.
            //
            if (lhsType.IsValueType || rhsType.IsValueType)
            {
                error = string.Format(CultureInfo.InvariantCulture, "{1} is not reference assignable to {0} because value types have no reference conversion.", lhsType, rhsType);
                return false;
            }

            error = string.Format(CultureInfo.InvariantCulture, "{1} is not reference assignable to {0}.", lhsType, rhsType);
            return false;
        }

        /// <summary>
        /// Tries to add a wildcard to the unification list.
        /// </summary>
        /// <param name="wildcard">Wildcard to be assigned.</param>
        /// <param name="type">Type to assign to the wildcard.</param>
        /// <param name="substitutions">Dictionary containing mappings of wildcards to types.</param>
        /// <param name="error">Error message set upon failing the wildcard assignment.</param>
        /// <returns>true if the wildcard assignment succeeded; otherwise, false.</returns>
        private static bool AddWildcard(Type wildcard, Type type, IDictionary<Type, Type> substitutions, ref string error)
        {
            if (substitutions.TryGetValue(wildcard, out Type res))
            {
                if (res != type)
                {
                    error = string.Format(CultureInfo.InvariantCulture, "Unification failed. Cannot unify {0} with type {1}. A prior match unified {0} with {2}.", wildcard, type, res);
                    return false;
                }
            }
            else
            {
                if (type.IsWildcard())
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Unification of wildcards {0} and {1} is not supported.", wildcard, type));
                }

                substitutions.Add(wildcard, type);
            }

            return true;
        }

        /// <summary>
        /// Checks whether the two specified types are equivalent.
        /// </summary>
        /// <param name="type1">First type to check.</param>
        /// <param name="type2">Second type to check.</param>
        /// <returns>true if the specified types are equivalent; otherwise, false.</returns>
        private static bool AreEquivalent(this Type type1, Type type2) => type1 == type2 || type1.IsEquivalentTo(type2);

        /// <summary>
        /// Checks whether the specified generic parameter type is covariant.
        /// </summary>
        /// <param name="type">Type to check for covariance.</param>
        /// <returns>true if the type is covariant; otherwise, false.</returns>
        private static bool IsCovariant(this Type type) => (type.GenericParameterAttributes & GenericParameterAttributes.Covariant) > GenericParameterAttributes.None;

        /// <summary>
        /// Checks whether the specified generic parameter type is contravariant.
        /// </summary>
        /// <param name="type">Type to check for contravariance.</param>
        /// <returns>true if the type is contravariant; otherwise, false.</returns>
        private static bool IsContravariant(this Type type) => (type.GenericParameterAttributes & GenericParameterAttributes.Contravariant) > GenericParameterAttributes.None;

        /// <summary>
        /// Checks whether the specified type is a vector array type.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns>true if the type is a vector array type; otherwise, false.</returns>
        private static bool IsSzArray(this Type type)
        {
            Debug.Assert(type.IsArray); // NB: Don't expose this helper without refining the check.

            return type.GetElementType().MakeArrayType() == type;
        }

        /// <summary>
        /// Checks whether the specified type implements the specified interface type.
        /// </summary>
        /// <param name="type">Type to check for interface type implementation.</param>
        /// <param name="interfaceType">Interface type to search for.</param>
        /// <returns>true if the type implements the interface; otherwise, false.</returns>
        private static bool ImplementInterface(this Type type, Type interfaceType)
        {
            while (type != null)
            {
                var interfaces = type.GetInterfaces();
                if (interfaces != null)
                {
                    for (int i = 0; i < interfaces.Length; i++)
                    {
                        var it = interfaces[i];
                        if (it != null)
                        {
                            if (it == interfaceType || it.ImplementInterface(interfaceType))
                            {
                                return true;
                            }
                        }
                    }
                }

                type = type.BaseType;
            }

            return false;
        }

        private static Type FindGenericTypeImpl(Type type, Type definition)
        {
            while (type != null && type != typeof(object))
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == definition)
                {
                    return type;
                }

                if (definition.IsInterface)
                {
                    var interfaces = type.GetInterfaces();
                    for (int i = 0; i < interfaces.Length; i++)
                    {
                        Type interfaceType = interfaces[i];
                        Type result = FindGenericTypeImpl(interfaceType, definition);
                        if (result != null)
                        {
                            return result;
                        }
                    }
                }

                type = type.BaseType;
            }

            return null;
        }

        #endregion

        #region Pretty printing

        /// <summary>
        /// Returns the C# code string representation of the type, without expanded namespaces and using C# type aliases.
        /// When compiler-generated types are encountered, an exception will be thrown.
        /// </summary>
        /// <param name="type">Type whose name to print as C# code.</param>
        /// <returns>C# code string representation of the type.</returns>
        public static string ToCSharpString(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return new TypePrinter(useNamespaceQualifiedNames: false, useCSharpTypeAliases: true, disallowCompilerGeneratedTypes: true).Visit(type);
        }

        /// <summary>
        /// Returns the C# code string representation of the type, without expanded namespaces and using C# type aliases.
        /// When compiler-generated types are encountered, its underlying name will be used.
        /// </summary>
        /// <param name="type">Type whose name to print as C# code.</param>
        /// <returns>String representation of the type using C# syntax. If compiler-generated type names occur, the output will not be valid C#.</returns>
        public static string ToCSharpStringPretty(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return ToCSharpString(type, useNamespaceQualifiedNames: false, useCSharpTypeAliases: true, disallowCompilerGeneratedTypes: false);
        }

        // CONSIDER: A design with a Flags enum may be better than the list of Boolean parameters that organically grew.

        /// <summary>
        /// Returns the C# code string representation of the type, using the specified configuration settings.
        /// </summary>
        /// <param name="type">Type whose name to print as C# code.</param>
        /// <param name="useNamespaceQualifiedNames">Indicates whether to use type names that include namespace names.</param>
        /// <param name="useCSharpTypeAliases">Indicates whether to use C# names for primitive types.</param>
        /// <param name="disallowCompilerGeneratedTypes">Indicates whether to throw upon encountering a C# compiler generated type name. When allowed, a type name may be returned that isn't valid C# code.</param>
        /// <returns>C# code string representation of the type.</returns>
        public static string ToCSharpString(this Type type, bool useNamespaceQualifiedNames, bool useCSharpTypeAliases, bool disallowCompilerGeneratedTypes)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            // TODO: add overloads with support for specification of imported namespaces

            return new TypePrinter(useNamespaceQualifiedNames, useCSharpTypeAliases, disallowCompilerGeneratedTypes).Visit(type);
        }

        private sealed class TypePrinter : TypeVisitor<string>
        {
#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable format // (Formatted as a table.)
            private static readonly Dictionary<Type, string> s_typeNames = new()
            {
                { typeof(void),    "void"    },
                { typeof(object),  "object"  },
                { typeof(byte),    "byte"    },
                { typeof(sbyte),   "sbyte"   },
                { typeof(short),   "short"   },
                { typeof(ushort),  "ushort"  },
                { typeof(int),     "int"     },
                { typeof(uint),    "uint"    },
                { typeof(long),    "long"    },
                { typeof(ulong),   "ulong"   },
                { typeof(float),   "float"   },
                { typeof(double),  "double"  },
                { typeof(decimal), "decimal" },
                { typeof(bool),    "bool"    },
                { typeof(char),    "char"    },
                { typeof(string),  "string"  },
            };
#pragma warning restore format
#pragma warning restore IDE0079

            private readonly bool _useNamespaceQualifiedNames;
            private readonly bool _useCSharpTypeAliases;
            private readonly bool _disallowCompilerGeneratedTypes;

            public TypePrinter(bool useNamespaceQualifiedNames, bool useCSharpTypeAliases, bool disallowCompilerGeneratedTypes)
            {
                _useNamespaceQualifiedNames = useNamespaceQualifiedNames;
                _useCSharpTypeAliases = useCSharpTypeAliases;
                _disallowCompilerGeneratedTypes = disallowCompilerGeneratedTypes;
            }

            public override string Visit(Type type)
            {
                Debug.Assert(type != null);

                if (_disallowCompilerGeneratedTypes && type.Name.StartsWith("<>", StringComparison.Ordinal))
                {
                    throw new InvalidOperationException("Compiler-generated C# type encountered: " + type.FullName);
                }

                return base.Visit(type);
            }

            protected override string MakeArrayType(Type type, string elementType) => elementType + "[]";

            protected override string MakeArrayType(Type type, string elementType, int rank)
            {
                if (rank == 1)
                    throw new InvalidOperationException("Multi-dimensional arrays of rank 1 are not supported in C#.");

                return elementType + "[" + new string(',', rank - 1) + "]";
            }

            protected override string VisitGenericParameter(Type type)
            {
                Debug.Assert(type != null);

                return type.Name;
            }

            protected override string VisitGenericClosed(Type type)
            {
                Debug.Assert(type != null);

                if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    return Visit(type.GetGenericArguments()[0]) + "?";
                }

                var genericDefinition = GetGenericTypeNameWithoutArity(type.GetGenericTypeDefinition());
                var genericArguments = Visit(type.GetGenericArguments());
                return MakeGenericType(type, genericDefinition, genericArguments);
            }

            protected override string MakeGenericType(Type type, string genericTypeDefinition, params string[] genericArguments) => string.Format(CultureInfo.InvariantCulture, "{0}<{1}>", genericTypeDefinition, string.Join(", ", genericArguments));

            protected override string VisitGenericTypeDefinition(Type type)
            {
                Debug.Assert(type != null);

                var nameWithoutArity = GetGenericTypeNameWithoutArity(type);
                var genericParameters = Visit(type.GetGenericArguments());
                return MakeGenericType(type, nameWithoutArity, genericParameters);
            }

            private string GetGenericTypeNameWithoutArity(Type type)
            {
                Debug.Assert(type.IsGenericTypeDefinition);

                var name = _useNamespaceQualifiedNames ? type.FullName : type.Name;

                var nameWithoutArity = name;

                var backtick = name.LastIndexOf('`');
                if (backtick >= 0)
                {
                    nameWithoutArity = nameWithoutArity.Substring(0, backtick);
                }

                return nameWithoutArity;
            }

            protected override string MakeByRefType(Type type, string elementType) => "ref " + elementType;

            protected override string MakePointerType(Type type, string elementType) => elementType + "*";

            protected override string VisitSimple(Type type)
            {
                Debug.Assert(type != null);

                if (_useCSharpTypeAliases)
                {
                    if (s_typeNames.TryGetValue(type, out string res))
                    {
                        return res;
                    }
                }

                return _useNamespaceQualifiedNames ? type.FullName : type.Name;
            }
        }

        #endregion

        #region Wildcards

        /// <summary>
        /// Unifies two types by matching wildcards to types. Matches are exact; subtyping relationships and assignability properties
        /// are not taken into account. Unification fails if wildcard bindings are conflicting or missing.
        /// </summary>
        /// <param name="type1">First type to unify.</param>
        /// <param name="type2">Second type to unify.</param>
        /// <returns>Map of wildcards to their resolved bound type.</returns>
        /// <remarks>
        /// It is common for one type to contain wildcards while the other doesn't contain any wildcards, though this is not required.
        /// In case both types contain wildcards, identity of wildcard types is used to resolve bindings.
        /// </remarks>
        public static IDictionary<Type, Type> UnifyExact(this Type type1, Type type2)
        {
            if (type1 == null)
                throw new ArgumentNullException(nameof(type1));
            if (type2 == null)
                throw new ArgumentNullException(nameof(type2));

            return UnifyExactImpl(type1, type2, comparer: null);
        }

        /// <summary>
        /// Unifies two types by matching wildcards to types. Matches are exact; subtyping relationships and assignability properties
        /// are not taken into account. Unification fails if wildcard bindings are conflicting or missing.
        /// </summary>
        /// <param name="type1">First type to unify.</param>
        /// <param name="type2">Second type to unify.</param>
        /// <param name="comparer">Type equality comparer for simple and open generic types.</param>
        /// <returns>Map of wildcards to their resolved bound type.</returns>
        /// <remarks>
        /// It is common for one type to contain wildcards while the other doesn't contain any wildcards, though this is not required.
        /// In case both types contain wildcards, identity of wildcard types is used to resolve bindings.
        /// </remarks>
        public static IDictionary<Type, Type> UnifyExact(this Type type1, Type type2, IEqualityComparer<Type> comparer)
        {
            if (type1 == null)
                throw new ArgumentNullException(nameof(type1));
            if (type2 == null)
                throw new ArgumentNullException(nameof(type2));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return UnifyExactImpl(type1, type2, comparer);
        }

        /// <summary>
        /// Unifies two types by matching wildcards to types. Matches are exact; subtyping relationships and assignability properties
        /// are not taken into account. Unification fails if wildcard bindings are conflicting or missing. Only the left type is
        /// allowed to have wildcards. One other limitation is that the wildcard attributes must not be on generic definitions.
        /// </summary>
        /// <param name="left">The left type, may contain wildcards.</param>
        /// <param name="right">The right type, without wildcards.</param>
        /// <returns>Map of wildcards to their resolved bound type.</returns>
        public static IDictionary<Type, Type> UnifyWith(this Type left, Type right)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));
            if (right == null)
                throw new ArgumentNullException(nameof(right));

            return UnifyWithImpl(left, right, comparer: null);
        }

        /// <summary>
        /// Unifies two types by matching wildcards to types. Matches are exact; subtyping relationships and assignability properties
        /// are not taken into account. Unification fails if wildcard bindings are conflicting or missing. Only the left type is
        /// allowed to have wildcards. One other limitation is that the wildcard attributes must not be on generic definitions.
        /// </summary>
        /// <param name="left">The left type, may contain wildcards.</param>
        /// <param name="right">The right type, without wildcards.</param>
        /// <param name="comparer">Type equality comparer for simple and open generic types.</param>
        /// <returns>Map of wildcards to their resolved bound type.</returns>
        public static IDictionary<Type, Type> UnifyWith(this Type left, Type right, IEqualityComparer<Type> comparer)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));
            if (right == null)
                throw new ArgumentNullException(nameof(right));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return UnifyWithImpl(left, right, comparer);
        }

        /// <summary>
        /// Tries to unify two types by matching wildcards to types. Matches are exact; subtyping relationships and assignability properties
        /// are not taken into account. Unification fails if wildcard bindings are conflicting or missing.
        /// </summary>
        /// <param name="type1">First type to unify.</param>
        /// <param name="type2">Second type to unify.</param>
        /// <param name="result">Map of wildcards to their resolved bound type.</param>
        /// <returns><c>true</c> if unification succeeded; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// It is common for one type to contain wildcards while the other doesn't contain any wildcards, though this is not required.
        /// In case both types contain wildcards, identity of wildcard types is used to resolve bindings.
        /// </remarks>
        public static bool TryUnifyExact(this Type type1, Type type2, out IDictionary<Type, Type> result)
        {
            if (type1 == null)
                throw new ArgumentNullException(nameof(type1));
            if (type2 == null)
                throw new ArgumentNullException(nameof(type2));

            var error = TryUnifyExactImpl(type1, type2, comparer: null, out result);
            return error == null;
        }

        /// <summary>
        /// Tries to unify two types by matching wildcards to types. Matches are exact; subtyping relationships and assignability properties
        /// are not taken into account. Unification fails if wildcard bindings are conflicting or missing.
        /// </summary>
        /// <param name="type1">First type to unify.</param>
        /// <param name="type2">Second type to unify.</param>
        /// <param name="comparer">Type equality comparer for simple and open generic types.</param>
        /// <param name="result">Map of wildcards to their resolved bound type.</param>
        /// <returns><c>true</c> if unification succeeded; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// It is common for one type to contain wildcards while the other doesn't contain any wildcards, though this is not required.
        /// In case both types contain wildcards, identity of wildcard types is used to resolve bindings.
        /// </remarks>
        public static bool TryUnifyExact(this Type type1, Type type2, IEqualityComparer<Type> comparer, out IDictionary<Type, Type> result)
        {
            if (type1 == null)
                throw new ArgumentNullException(nameof(type1));
            if (type2 == null)
                throw new ArgumentNullException(nameof(type2));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            var error = TryUnifyExactImpl(type1, type2, comparer, out result);
            return error == null;
        }

        /// <summary>
        /// Tries to unify two types by matching wildcards to types. Matches are exact; subtyping relationships and assignability properties
        /// are not taken into account. Unification fails if wildcard bindings are conflicting or missing. Only the left type is
        /// allowed to have wildcards. One other limitation is that the wildcard attributes must not be on generic definitions.
        /// </summary>
        /// <param name="left">The left type, may contain wildcards.</param>
        /// <param name="right">The right type, without wildcards.</param>
        /// <param name="result">Map of wildcards to their resolved bound type.</param>
        /// <returns><c>true</c> if unification succeeded; otherwise, <c>false</c>.</returns>
        public static bool TryUnifyWith(this Type left, Type right, out IDictionary<Type, Type> result)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));
            if (right == null)
                throw new ArgumentNullException(nameof(right));

            var error = TryUnifyWithImpl(left, right, comparer: null, out result);
            return error == null;
        }

        /// <summary>
        /// Tries to unify two types by matching wildcards to types. Matches are exact; subtyping relationships and assignability properties
        /// are not taken into account. Unification fails if wildcard bindings are conflicting or missing. Only the left type is
        /// allowed to have wildcards. One other limitation is that the wildcard attributes must not be on generic definitions.
        /// </summary>
        /// <param name="left">The left type, may contain wildcards.</param>
        /// <param name="right">The right type, without wildcards.</param>
        /// <param name="comparer">Type equality comparer for simple and open generic types.</param>
        /// <param name="result">Map of wildcards to their resolved bound type.</param>
        /// <returns><c>true</c> if unification succeeded; otherwise, <c>false</c>.</returns>
        public static bool TryUnifyWith(this Type left, Type right, IEqualityComparer<Type> comparer, out IDictionary<Type, Type> result)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));
            if (right == null)
                throw new ArgumentNullException(nameof(right));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            var error = TryUnifyWithImpl(left, right, comparer, out result);
            return error == null;
        }

        private static IDictionary<Type, Type> UnifyExactImpl(Type type1, Type type2, IEqualityComparer<Type> comparer)
        {
            var error = TryUnifyExactImpl(type1, type2, comparer, out IDictionary<Type, Type> result);
            if (error != null)
            {
                throw error;
            }

            return result;
        }

        private static Exception TryUnifyExactImpl(Type type1, Type type2, IEqualityComparer<Type> comparer, out IDictionary<Type, Type> result)
        {
            var unifier = new TypeUnifier(comparer);
            var res = unifier.Equals(type1, type2);

            var error = default(Exception);

            if (!res)
            {
                result = null;
                error = unifier.Error;
            }
            else
            {
                result = unifier.Map;
            }

            if (error == null)
            {
                var unbound = default(List<string>);

                foreach (var kv in unifier.Map)
                {
                    if (kv.Value == null)
                    {
                        if (unbound == null)
                        {
                            unbound = new List<string>();
                        }

                        unbound.Add(kv.Key.Name);
                    }
                }

                if (unbound != null)
                {
                    error = new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Unification failed. Type(s) {0} are unbound.", string.Join(", ", unbound)));
                }
            }

            return error;
        }

        private static IDictionary<Type, Type> UnifyWithImpl(Type type1, Type type2, IEqualityComparer<Type> comparer)
        {
            var error = TryUnifyWithImpl(type1, type2, comparer, out IDictionary<Type, Type> result);
            if (error != null)
            {
                throw error;
            }

            return result;
        }

        private static Exception TryUnifyWithImpl(Type type1, Type type2, IEqualityComparer<Type> comparer, out IDictionary<Type, Type> result)
        {
            var unifier = new LeftTypeUnifier(comparer);
            var res = unifier.Equals(type1, type2);

            var error = default(Exception);

            if (!res)
            {
                result = null;
                error = unifier.Error;
            }
            else
            {
                result = unifier.Map;
            }

            return error;
        }

        private sealed class TypeUnifier : TypeEqualityComparer
        {
            private readonly IEqualityComparer<Type> _comparer;
            private readonly Dictionary<Type, TypeHolder> _map;

            public TypeUnifier(IEqualityComparer<Type> comparer)
            {
                _comparer = comparer;
                _map = new Dictionary<Type, TypeHolder>();
            }

            public Dictionary<Type, Type> Map => _map.ToDictionary(e => e.Key, e => e.Value.Type);

            public Exception Error { get; private set; }

            public override bool Equals(Type x, Type y)
            {
                var xWildcard = x.IsWildcard();
                var yWildcard = y.IsWildcard();

                if (xWildcard && yWildcard)
                {
                    return MergeWildcards(x, y);
                }

                if (xWildcard && !AddUnificationEntry(x, y))
                {
                    return false;
                }

                if (yWildcard && !AddUnificationEntry(y, x))
                {
                    return false;
                }

                if (!xWildcard && !yWildcard)
                {
                    if (x == y)
                    {
                        return true;
                    }

                    if (!base.Equals(x, y))
                    {
                        if (Error == null)
                        {
                            Error = new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Unification failed. Cannot unify {0} with type {1}. The types are not structurally identical.", x, y));
                        }

                        return false;
                    }
                }

                return true;
            }

            protected override bool EqualsGenericTypeDefinition(Type x, Type y) => _comparer != null ? _comparer.Equals(x, y) : base.EqualsGenericTypeDefinition(x, y);

            protected override bool EqualsSimple(Type x, Type y) => _comparer != null ? _comparer.Equals(x, y) : base.EqualsSimple(x, y);

            private bool AddUnificationEntry(Type wildcard, Type type)
            {
                if (_map.TryGetValue(wildcard, out TypeHolder h))
                {
                    if (h.Type == null)
                    {
                        h.Type = type;
                    }
                    else if (h.Type != type)
                    {
                        Error = new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Unification failed. Cannot unify {0} with type {1}. A prior match unified {0} with {2}.", wildcard, type, h.Type));
                        return false;
                    }
                }
                else
                {
                    h = new TypeHolder { Type = type };
                    _map[wildcard] = h;
                }

                h.Wildcards.Add(wildcard);
                return true;
            }

            private bool MergeWildcards(Type x, Type y)
            {
                var bx = _map.TryGetValue(x, out TypeHolder hx);
                var by = _map.TryGetValue(y, out TypeHolder hy);

                if (bx && by)
                {
                    var type = default(Type);

                    if (hx.Type != null && hy.Type != null)
                    {
                        if (hx.Type != hy.Type)
                        {
                            Error = new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Unification failed. Cannot unify {0} and {1}. Prior matches unified {0} with {2} and {1} with {3}.", x, y, hx.Type, hy.Type));
                            return false;
                        }

                        type = hx.Type;
                    }
                    else if (hx.Type != null)
                    {
                        type = hx.Type;
                    }
                    else if (hy.Type != null)
                    {
                        type = hy.Type;
                    }

                    var h = new TypeHolder { Type = type };

                    h.Wildcards.UnionWith(hx.Wildcards);
                    h.Wildcards.UnionWith(hy.Wildcards);

                    foreach (var w in h.Wildcards)
                    {
                        _map[w] = h;
                    }
                }
                else if (bx)
                {
                    _map[y] = hx;
                    hx.Wildcards.Add(y);
                }
                else if (by)
                {
                    _map[x] = hy;
                    hy.Wildcards.Add(x);
                }
                else
                {
                    var h = new TypeHolder
                    {
                        Wildcards = { x, y }
                    };

                    _map[x] = h;
                    _map[y] = h;
                }

                return true;
            }

            private sealed class TypeHolder
            {
                public HashSet<Type> Wildcards { get; } = new HashSet<Type>();
                public Type Type;
            }
        }

        private sealed class LeftTypeUnifier : TypeEqualityComparer
        {
            private readonly IEqualityComparer<Type> _comparer;

            public LeftTypeUnifier(IEqualityComparer<Type> comparer)
            {
                _comparer = comparer;
            }

            public Dictionary<Type, Type> Map { get; } = new Dictionary<Type, Type>();

            public Exception Error { get; private set; }

            public override bool Equals(Type x, Type y)
            {
                if (x == y)
                {
                    return true;
                }

                if (!base.Equals(x, y))
                {
                    if (Error == null)
                    {
                        Error = new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Unification failed. Cannot unify {0} with type {1}. The types are not structurally identical.", x, y));
                    }

                    return false;
                }

                return true;
            }

            protected override bool EqualsGenericTypeDefinition(Type x, Type y)
            {
                if (_comparer != null)
                {
                    return _comparer.Equals(x, y);
                }

                return base.EqualsGenericTypeDefinition(x, y);
            }

            protected override bool EqualsSimple(Type x, Type y)
            {
                if (x.IsWildcard())
                {
                    return AddUnificationEntry(x, y);
                }
                else if (_comparer != null)
                {
                    return _comparer.Equals(x, y);
                }

                return base.EqualsSimple(x, y);
            }

            private bool AddUnificationEntry(Type wildcard, Type type)
            {
                if (Map.TryGetValue(wildcard, out Type m))
                {
                    if (m != type)
                    {
                        Error = new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Unification failed. Cannot unify {0} with type {1}. A prior match unified {0} with {2}.", wildcard, type, m));
                        return false;
                    }
                }
                else
                {
                    Map.Add(wildcard, type);
                }

                return true;
            }
        }

        #endregion

        #region Vector array types

        /// <summary>
        /// Checks whether the specified <paramref name="type"/> is a one-dimensional vector array.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns><c>true</c> if the specified <paramref name="type"/> is a vector; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// The name of this method is consistent with internal naming of one-dimensional vector
        /// array types in the CLR. The CoreCLR implementation is exposing this as a property
        /// named <c>IsSzArray</c>, so we're being consistent with the future here.
        /// </remarks>
        public static bool IsSZArray(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return type.IsArray && type.GetElementType().MakeArrayType() == type;
        }

        #endregion

        #region Internal helpers

        internal static bool IsWildcard(this Type type) =>
               s_knownWildcards.Contains(type)
            || type.IsDefined(typeof(TypeWildcardAttribute), inherit: false);

        internal static bool IsNullableType(this Type type) =>
               type.IsGenericType
            && type.GetGenericTypeDefinition() == typeof(Nullable<>);

        internal static Type GetNonNullableType(this Type type)
        {
            if (type.IsNullableType())
            {
                return type.GetGenericArguments()[0];
            }

            return type;
        }

        internal static bool IsInteger(this Type type)
        {
            type = type.GetNonNullableType();
            if (type.IsEnum)
            {
                return false;
            }

            return Type.GetTypeCode(type)
                is TypeCode.SByte
                or TypeCode.Byte
                or TypeCode.Int16
                or TypeCode.UInt16
                or TypeCode.Int32
                or TypeCode.UInt32
                or TypeCode.Int64
                or TypeCode.UInt64;
        }

        #endregion
    }
}
