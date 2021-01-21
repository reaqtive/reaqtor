// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2017 - Created this file.
//

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.CompilerServices;
using System.Linq.CompilerServices.TypeSystem;
using System.Reflection;

namespace System.Linq.Expressions
{
    /// <summary>
    /// Represents a table with <see cref="MemberInfo"/> objects.
    /// </summary>
    public partial class MemberTable : IEnumerable<MemberInfo>
    {
        /// <summary>
        /// Indicates whether the table is read-only.
        /// </summary>
        private bool _readOnly;

        /// <summary>
        /// A set of non-generic members which are considered pure.
        /// </summary>
        private readonly HashSet<MemberInfo> Members = new();

        /// <summary>
        /// A set of open generic methods. Any of their closed instantiations is considered to be present in the table.
        /// </summary>
        private readonly HashSet<MethodInfo> GenericMethods = new();

        /// <summary>
        /// Dictionary mapping of open generic types onto members on these types.
        /// </summary>
        private readonly Dictionary<Type, HashSet<MemberInfo>> MembersOnGenericTypes = new();

        // CONSIDER: Add an overload that enables visiting the expression to gather all reflection members.

        /// <summary>
        /// Marks the current member table as read-only, preventing subsequent mutation.
        /// </summary>
        /// <returns>The current member table after being marked as read-only.</returns>
        public MemberTable ToReadOnly()
        {
            _readOnly = true;
            return this;
        }

        /// <summary>
        /// Adds an entry to the member table using the <see cref="System.Reflection.MemberInfo" /> object occurring in the body of the specified <paramref name="expression" />.
        /// </summary>
        /// <param name="expression">The expression to obtain the member to add from.</param>
        public void Add(LambdaExpression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            CheckReadOnly();

            var body = expression.Body;

            switch (body)
            {
                case MethodCallExpression call:
                    Add(call.Method);
                    break;
                case MemberExpression member:
                    Add(member.Member);
                    break;
                case NewExpression @new:
                    Add(@new.Constructor);
                    break;
                case BinaryExpression binary:
                    Add(binary.Method);
                    break;
                case UnaryExpression unary:
                    Add(unary.Method);
                    break;
                case IndexExpression index:
                    Add(index.Indexer);
                    break;
            }
        }

        /// <summary>
        /// Copies the entries in the specified member <paramref name="table"/> to the current table.
        /// </summary>
        /// <param name="table">The member table whose entries to copy.</param>
        public void Add(MemberTable table)
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table));

            CheckReadOnly();

            foreach (var member in table.Members)
            {
                Members.Add(member);
            }

            foreach (var method in table.GenericMethods)
            {
                GenericMethods.Add(method);
            }

            foreach (var member in table.MembersOnGenericTypes)
            {
                MembersOnGenericTypes.Add(member.Key, new HashSet<MemberInfo>(member.Value));
            }
        }

        /// <summary>
        /// Adds the specified <paramref name="member"/> to the table.
        /// </summary>
        /// <param name="member">The member to add to the table.</param>
        public void Add(MemberInfo member)
        {
            if (member == null)
                throw new ArgumentNullException(nameof(member));

            CheckReadOnly();

            switch (member.MemberType)
            {
                case MemberTypes.Constructor:
                    Add((ConstructorInfo)member);
                    break;
                case MemberTypes.Field:
                    Add((FieldInfo)member);
                    break;
                case MemberTypes.Method:
                    Add((MethodInfo)member);
                    break;
                case MemberTypes.Property:
                    Add((PropertyInfo)member);
                    break;
                default:
                    throw new ArgumentException($"A member of type '{member.MemberType}' is not supported.", nameof(member));
            }
        }

        /// <summary>
        /// Adds the specified <paramref name="method"/> to the table.
        /// </summary>
        /// <param name="method">The method to add to the table.</param>
        public void Add(MethodInfo method)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));

            CheckReadOnly();

            if (IsOpenGenericMethod(method))
            {
                AddGenericMethod(method);
                return;
            }

            if (IsOpenGenericType(method.DeclaringType))
            {
                AddGeneric(method);
                return;
            }

            CheckTypes(method);

            AddCore(method);
        }

        /// <summary>
        /// Adds the specified <paramref name="property"/> to the table.
        /// </summary>
        /// <param name="property">The property to add to the table.</param>
        public void Add(PropertyInfo property)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));

            CheckReadOnly();

            var getMethod = property.GetGetMethod();
            if (getMethod == null)
                throw new ArgumentException("Property does not have a publicly accessible getter.", nameof(property));

            CheckTypes(getMethod);

            Add(getMethod);

            if (IsOpenGenericType(property.DeclaringType))
            {
                AddGeneric(property);
                return;
            }

            AddCore(property);
        }

        /// <summary>
        /// Adds the specified <paramref name="field"/> to the table.
        /// </summary>
        /// <param name="field">The field to add to the table.</param>
        public void Add(FieldInfo field)
        {
            if (field == null)
                throw new ArgumentNullException(nameof(field));

            CheckReadOnly();

            if (IsOpenGenericType(field.DeclaringType))
            {
                AddGeneric(field);
                return;
            }

            AddCore(field);
        }

        /// <summary>
        /// Adds the specified <paramref name="constructor"/> to the table.
        /// </summary>
        /// <param name="constructor">The constructor to add to the table.</param>
        public void Add(ConstructorInfo constructor)
        {
            if (constructor == null)
                throw new ArgumentNullException(nameof(constructor));

            CheckReadOnly();

            if (constructor.IsStatic)
                throw new ArgumentException("Static constructors are not supported.", nameof(constructor));

            if (IsOpenGenericType(constructor.DeclaringType))
            {
                AddGeneric(constructor);
                return;
            }

            CheckTypes(constructor);

            AddCore(constructor);
        }

        /// <summary>
        /// Adds an entry for the specified <paramref name="member"/> to the table.
        /// </summary>
        /// <param name="member">The member to add an entry for.</param>
        private void AddCore(MemberInfo member)
        {
            Members.Add(member);
        }

        /// <summary>
        /// Adds an entry for the specified <paramref name="member"/> declared on an open generic type to the table.
        /// </summary>
        /// <param name="member">The member declared on an open generic type to add.</param>
        private void AddGeneric(MemberInfo member)
        {
            var definingMember = GetGenericDefinition(member);
            var definingType = definingMember.DeclaringType;

            if (!MembersOnGenericTypes.TryGetValue(definingType, out var members))
            {
                members = new HashSet<MemberInfo>();
                MembersOnGenericTypes.Add(definingType, members);
            }

            if (!members.Contains(definingMember))
            {
                members.Add(definingMember);
            }
        }

        /// <summary>
        /// Adds an entry for the specified open generic <paramref name="method"/> to the table.
        /// </summary>
        /// <param name="method">The open generic method to add.</param>
        private void AddGenericMethod(MethodInfo method)
        {
            var definition = method;

            if (!method.IsGenericMethodDefinition)
            {
                Debug.Assert(AreAllWildcards(method.GetGenericArguments()));

                definition = method.GetGenericMethodDefinition();
            }

            GenericMethods.Add(definition);
        }

        /// <summary>
        /// Gets a sequence of members in the current member table.
        /// </summary>
        /// <returns>A sequence of members in the current member table.</returns>
        public IEnumerator<MemberInfo> GetEnumerator()
        {
            foreach (var key in Members)
            {
                yield return key;
            }

            foreach (var method in GenericMethods)
            {
                yield return method;
            }

            foreach (var type in MembersOnGenericTypes)
            {
                foreach (var member in type.Value)
                {
                    yield return member;
                }
            }
        }

        /// <summary>
        /// Gets a sequence of members in the current member table.
        /// </summary>
        /// <returns>A sequence of members in the current member table.</returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Checks if the specified <paramref name="member"/> is present in the table.
        /// </summary>
        /// <param name="member">The member to check.</param>
        /// <returns><c>true</c> if the specified <paramref name="member"/> is present in the table; otherwise, <c>false</c>.</returns>
        public bool Contains(MemberInfo member)
        {
            if (member == null)
                throw new ArgumentNullException(nameof(member));

            if (Members.Contains(member))
            {
                return true;
            }

            if (member.MemberType == MemberTypes.Method)
            {
                var method = (MethodInfo)member;
                if (method.IsGenericMethod)
                {
                    Debug.Assert(!method.IsGenericMethodDefinition);

                    var def = method.GetGenericMethodDefinition();
                    if (GenericMethods.Contains(def))
                    {
                        return true;
                    }
                }
            }

            var declaringType = member.DeclaringType;

            if (declaringType.IsGenericType)
            {
                var declaringTypeDef = declaringType.GetGenericTypeDefinition();

                if (MembersOnGenericTypes.TryGetValue(declaringTypeDef, out var members))
                {
                    var memberDef = GetGenericDefinition(member);

                    if (members.Contains(memberDef))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if the current member table is marked as read-only.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the current member table is marked as read-only.
        /// </exception>
        private void CheckReadOnly()
        {
            if (_readOnly)
                throw new InvalidOperationException("The table is marked as read-only.");
        }

        /// <summary>
        /// Checks if the types of the parameters of the specified <paramref name="method"/> can be used in an expression tree.
        /// </summary>
        /// <param name="method">The method whose parameter types to check.</param>
        private static void CheckTypes(MethodBase method)
        {
            foreach (var parameter in method.GetParameters())
            {
                CheckType(parameter.ParameterType);
            }
        }

        /// <summary>
        /// Checks if the specified <paramref name="type"/> can be used in an expression tree.
        /// </summary>
        /// <param name="type">The type to check.</param>
        private static void CheckType(Type type)
        {
            if (type.IsByRef || type.IsPointer)
                throw new NotSupportedException($"The specified type '{type}' is not supported.");
        }

        /// <summary>
        /// Checks if the specified <paramref name="type"/> is an open generic type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the specified <paramref name="type"/> is an open generic type; otherwise, false.</returns>
        private static bool IsOpenGenericType(Type type)
        {
            if (type != null)
            {
                if (type.IsGenericTypeDefinition)
                {
                    return true;
                }

                if (type.IsGenericType && AreAllWildcards(type.GetGenericArguments()))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if the specified <paramref name="method"/> is an open generic method.
        /// </summary>
        /// <param name="method">The method to check.</param>
        /// <returns>true if the specified <paramref name="method"/> is an open generic method; otherwise, false.</returns>
        private static bool IsOpenGenericMethod(MethodInfo method)
        {
            if (method != null)
            {
                if (method.IsGenericMethodDefinition)
                {
                    return true;
                }

                if (method.IsGenericMethod && AreAllWildcards(method.GetGenericArguments()))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if all the specified <paramref name="types"/> are either wildcards or none of them are wildcards.
        /// </summary>
        /// <param name="types">The types to check.</param>
        /// <returns>true if all the specified <paramref name="types"/> are wildcards; otherwise, false.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the specified <paramref name="types"/> are not all of the same kind (i.e. wildcard or not).
        /// </exception>
        private static bool AreAllWildcards(Type[] types)
        {
            // NB: We don't do any advanced matching or unification, e.g. A<T, T> meaning that A<int, int> unifies
            //     but A<int, bool> doesn't. All wildcards are treated equal when specifying pure members. We could
            //     refine this in the future if we find cases where an open generic type or method only exhibits
            //     purity for some constrained binding of its generic type parameters. This could also include
            //     support for partially bound generics, e.g. A<T, int>.

            var allWildcards = default(bool?);

            foreach (var type in types)
            {
                var isWildcard = type.IsDefined(typeof(TypeWildcardAttribute), inherit: false);

                if (allWildcards == null)
                {
                    allWildcards = isWildcard;
                }
                else
                {
                    if (isWildcard != allWildcards.Value)
                    {
                        throw new InvalidOperationException("Either all or none of the generic parameters should be wildcards.");
                    }
                }
            }

            return allWildcards ?? false;
        }

        /// <summary>
        /// Gets the <see cref="MemberInfo"/> representing the member corresponding to the specified <paramref name="member"/>
        /// on the open generic declaring type.
        /// </summary>
        /// <param name="member">The member to get the corresponding member for on the open generic declaring type.</param>
        /// <returns>The <see cref="MemberInfo"/> representing definition of the specified <paramref name="member"/> on the open generic declaring type.</returns>
        private static MemberInfo GetGenericDefinition(MemberInfo member)
        {
            var definingMember = member;
            var definingType = member.DeclaringType;

            if (!definingType.IsGenericTypeDefinition)
            {
                var declaringTypeArgs = definingType.GetGenericArguments();

                definingType = definingType.GetGenericTypeDefinition();

                var definingTypePars = definingType.GetGenericArguments();

                Debug.Assert(declaringTypeArgs.Length == definingTypePars.Length);

                var n = declaringTypeArgs.Length;
                var map = new Dictionary<Type, Type>(n);

                for (var i = 0; i < n; i++)
                {
                    map.Add(definingTypePars[i], declaringTypeArgs[i]);
                }

                var subst = new TypeSubstitutor(map);

                var flags = BindingFlags.Public | (IsStatic(member) ? BindingFlags.Static : BindingFlags.Instance);

                var candidates = definingType.FindMembers(member.MemberType, flags, (candidate, state) =>
                {
                    var t = (Tuple<MemberInfo, TypeSubstitutor>)state;
                    return IsMatch(candidate, t.Item1, t.Item2);
                }, Tuple.Create(member, subst));

                if (candidates.Length != 1)
                {
                    throw new InvalidOperationException($"Could not find the definition of '{member}' on '{definingType}'.");
                }

                definingMember = candidates[0];
            }

            return definingMember;
        }

        /// <summary>
        /// Checks if the specified <paramref name="candidate"/> and <paramref name="original"/> members are equal
        /// when applying the type substitutions specified in <paramref name="subst"/>.
        /// </summary>
        /// <param name="candidate">The candidate member to check for equality.</param>
        /// <param name="original">The original member to check for equality.</param>
        /// <param name="subst">Type substitutor mapping candidate types onto original types (e.g. for generic parameter type bindings).</param>
        /// <returns>true if the specified <paramref name="candidate"/> and <paramref name="original"/> members are equal; otherwise, false.</returns>
        private static bool IsMatch(MemberInfo candidate, MemberInfo original, TypeSubstitutor subst)
        {
            return candidate.MemberType switch
            {
                MemberTypes.Method => IsMatch((MethodInfo)candidate, (MethodInfo)original, subst),
                MemberTypes.Constructor => IsMatch((ConstructorInfo)candidate, (ConstructorInfo)original, subst),
                MemberTypes.Field => IsMatch((FieldInfo)candidate, (FieldInfo)original, subst),
                MemberTypes.Property => IsMatch((PropertyInfo)candidate, (PropertyInfo)original, subst),
                _ => false,
            };
        }

        /// <summary>
        /// Checks if the specified <paramref name="candidate"/> and <paramref name="original"/> methods are equal
        /// when applying the type substitutions specified in <paramref name="subst"/>.
        /// </summary>
        /// <param name="candidate">The candidate method to check for equality.</param>
        /// <param name="original">The original method to check for equality.</param>
        /// <param name="subst">Type substitutor mapping candidate types onto original types (e.g. for generic parameter type bindings).</param>
        /// <returns>true if the specified <paramref name="candidate"/> and <paramref name="original"/> methods are equal; otherwise, false.</returns>
        private static bool IsMatch(MethodInfo candidate, MethodInfo original, TypeSubstitutor subst)
        {
            if (candidate.IsGenericMethod)
                throw new NotImplementedException(); // TODO: open generic methods on open generic types

            if (candidate.Name != original.Name)
                return false;

            if (!IsMatch(candidate.ReturnType, original.ReturnType, subst))
                return false;

            var candidateParameters = candidate.GetParameters();
            var originalParameters = original.GetParameters();

            if (!IsMatch(candidateParameters, originalParameters, subst))
                return false;

            return true;
        }

        /// <summary>
        /// Checks if the specified <paramref name="candidate"/> and <paramref name="original"/> constructors are equal
        /// when applying the type substitutions specified in <paramref name="subst"/>.
        /// </summary>
        /// <param name="candidate">The candidate constructor to check for equality.</param>
        /// <param name="original">The original constructor to check for equality.</param>
        /// <param name="subst">Type substitutor mapping candidate types onto original types (e.g. for generic parameter type bindings).</param>
        /// <returns>true if the specified <paramref name="candidate"/> and <paramref name="original"/> constructors are equal; otherwise, false.</returns>
        private static bool IsMatch(ConstructorInfo candidate, ConstructorInfo original, TypeSubstitutor subst)
        {
            var candidateParameters = candidate.GetParameters();
            var originalParameters = original.GetParameters();

            if (!IsMatch(candidateParameters, originalParameters, subst))
                return false;

            return true;
        }

        /// <summary>
        /// Checks if the specified <paramref name="candidate"/> and <paramref name="original"/> fields are equal
        /// when applying the type substitutions specified in <paramref name="subst"/>.
        /// </summary>
        /// <param name="candidate">The candidate field to check for equality.</param>
        /// <param name="original">The original field to check for equality.</param>
        /// <param name="subst">Type substitutor mapping candidate types onto original types (e.g. for generic parameter type bindings).</param>
        /// <returns>true if the specified <paramref name="candidate"/> and <paramref name="original"/> fields are equal; otherwise, false.</returns>
        private static bool IsMatch(FieldInfo candidate, FieldInfo original, TypeSubstitutor subst)
        {
            if (candidate.Name != original.Name)
                return false;

            if (!IsMatch(candidate.FieldType, original.FieldType, subst))
                return false;

            return true;
        }

        /// <summary>
        /// Checks if the specified <paramref name="candidate"/> and <paramref name="original"/> properties are equal
        /// when applying the type substitutions specified in <paramref name="subst"/>.
        /// </summary>
        /// <param name="candidate">The candidate property to check for equality.</param>
        /// <param name="original">The original property to check for equality.</param>
        /// <param name="subst">Type substitutor mapping candidate types onto original types (e.g. for generic parameter type bindings).</param>
        /// <returns>true if the specified <paramref name="candidate"/> and <paramref name="original"/> properties are equal; otherwise, false.</returns>
        private static bool IsMatch(PropertyInfo candidate, PropertyInfo original, TypeSubstitutor subst)
        {
            if (candidate.Name != original.Name)
                return false;

            if (!IsMatch(candidate.PropertyType, original.PropertyType, subst))
                return false;

            var candidateParameters = candidate.GetIndexParameters();
            var originalParameters = original.GetIndexParameters();

            if (!IsMatch(candidateParameters, originalParameters, subst))
                return false;

            return true;
        }

        /// <summary>
        /// Checks if the specified <paramref name="candidateParameters"/> and <paramref name="originalParameters"/> parameters are equal
        /// when applying the type substitutions specified in <paramref name="subst"/>.
        /// </summary>
        /// <param name="candidateParameters">The candidate parameters to check for equality.</param>
        /// <param name="originalParameters">The original parameters to check for equality.</param>
        /// <param name="subst">Type substitutor mapping candidate types onto original types (e.g. for generic parameter type bindings).</param>
        /// <returns>true if the specified <paramref name="candidateParameters"/> and <paramref name="originalParameters"/> parameters are equal; otherwise, false.</returns>
        private static bool IsMatch(ParameterInfo[] candidateParameters, ParameterInfo[] originalParameters, TypeSubstitutor subst)
        {
            if (candidateParameters.Length != originalParameters.Length)
                return false;

            for (var i = 0; i < candidateParameters.Length; i++)
            {
                if (!IsMatch(candidateParameters[i].ParameterType, originalParameters[i].ParameterType, subst))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks if the specified <paramref name="candidate"/> and <paramref name="original"/> types are equal
        /// when applying the type substitutions specified in <paramref name="subst"/>.
        /// </summary>
        /// <param name="candidate">The candidate type to check for equality.</param>
        /// <param name="original">The original type to check for equality.</param>
        /// <param name="subst">Type substitutor mapping candidate types onto original types (e.g. for generic parameter type bindings).</param>
        /// <returns>true if the specified <paramref name="candidate"/> and <paramref name="original"/> types are equal; otherwise, false.</returns>
        private static bool IsMatch(Type candidate, Type original, TypeSubstitutor subst)
        {
            return candidate == original || subst.Visit(candidate) == original;
        }

        /// <summary>
        /// Checks if the specified <paramref name="member"/> is static.
        /// </summary>
        /// <param name="member">The member to check.</param>
        /// <returns>true if the specified <paramref name="member"/> is static; otherwise, false.</returns>
        private static bool IsStatic(MemberInfo member)
        {
            return member.MemberType switch
            {
                MemberTypes.Method or MemberTypes.Constructor => ((MethodBase)member).IsStatic,
                MemberTypes.Field => ((FieldInfo)member).IsStatic,
                MemberTypes.Property => ((PropertyInfo)member).GetGetMethod().IsStatic,
                _ => false,
            };
        }
    }
}
