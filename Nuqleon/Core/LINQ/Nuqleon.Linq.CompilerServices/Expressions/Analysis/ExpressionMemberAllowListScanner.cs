// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
//

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace System.Linq.CompilerServices
{
    /// <summary>
    /// Expression visitor used to check uses of type members against a list of allowed members.
    /// </summary>
    public class ExpressionMemberAllowListScanner : ExpressionMemberAllowListScannerBase
    {
        private readonly Dictionary<Type, Entry> _entries = new();

        /// <summary>
        /// Creates a new allow list scanner for members. To complete instantiation, initialize the DeclaringTypes and Members properties, e.g. by using collection initializers.
        /// </summary>
        public ExpressionMemberAllowListScanner()
        {
            DeclaringTypes = new TypeList(AddDeclaringType);
            Members = new MemberList(this);
        }

        /// <summary>
        /// Gets the declaring types whose members are allowed.
        /// </summary>
        public TypeList DeclaringTypes { get; }

        /// <summary>
        /// Gets the members which are allowed.
        /// </summary>
        public MemberList Members { get; }

        /// <summary>
        /// Checks whether the specified member is supported.
        /// </summary>
        /// <param name="member">Member to check.</param>
        /// <returns>true if the member is supported; otherwise, false.</returns>
        protected override bool Check(MemberInfo member)
        {
            if (member == null)
                throw new ArgumentNullException(nameof(member));

            foreach (var m in GetMemberAndInterfaceImplementations(member))
            {
                var decl = m.DeclaringType;

                if (_entries.TryGetValue(decl, out Entry entry))
                {
                    if (entry.IsInclusive || entry.Check(member))
                    {
                        return true;
                    }
                }

                if (decl.IsGenericType)
                {
                    var genDef = decl.GetGenericTypeDefinition();
                    if (_entries.TryGetValue(genDef, out entry))
                    {
                        if (entry.IsInclusive || entry.Check(member))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private static IEnumerable<MemberInfo> GetMemberAndInterfaceImplementations(MemberInfo member)
        {
            yield return member;

            var memberType = member.MemberType;

            if (memberType == MemberTypes.Method)
            {
                foreach (var method in GetMethodInterfaceImplementations((MethodInfo)member))
                {
                    yield return method;
                }
            }
            else if (memberType == MemberTypes.Property)
            {
                foreach (var property in GetPropertyInterfaceImplementations((PropertyInfo)member))
                {
                    yield return property;
                }
            }
        }

        private static IEnumerable<MethodInfo> GetMethodInterfaceImplementations(MethodInfo method)
        {
            var decl = method.DeclaringType;
            if (!decl.IsInterface)
            {
                foreach (var ifType in decl.GetInterfaces())
                {
                    var ifMap = decl.GetInterfaceMap(ifType);

                    var i = Array.IndexOf(ifMap.TargetMethods, method);
                    if (i >= 0)
                    {
                        yield return ifMap.InterfaceMethods[i];
                    }
                }
            }
        }

        private static IEnumerable<PropertyInfo> GetPropertyInterfaceImplementations(PropertyInfo property)
        {
            var decl = property.DeclaringType;
            if (!decl.IsInterface)
            {
                var methods = property.GetAccessors(nonPublic: true);

                foreach (var ifType in decl.GetInterfaces())
                {
                    var ifMap = decl.GetInterfaceMap(ifType);
                    var ifMtd = default(MethodInfo);

                    foreach (var method in methods)
                    {
                        var i = Array.IndexOf(ifMap.TargetMethods, method);
                        if (i >= 0)
                        {
                            ifMtd = ifMap.InterfaceMethods[i];
                            break;
                        }
                    }

                    if (ifMtd != null)
                    {
                        var ifProps = ifType.GetProperties();
                        var ifProp = ifProps.Single(p => p.GetAccessors().Contains(ifMtd));
                        yield return ifProp;
                    }
                }
            }
        }

        private sealed class Entry
        {
            /*
             * Note: This is tricky stuff. We want to allow e.g. List<int>.ctor() to match List<T>.ctor(), which
             *       can be achieved using a comparison by metadata token. Other means of determining this match
             *       seem nonexistent. Also, entries like Activator.CreateInstance<T> should provide for matches
             *       over the closed forms. However, a specific closed generic like Activator.CreateInstance<int>
             *       should not match any other closed generic method binding. Therefore, we keep a separate list
             *       of closed generic methods to match against.
             */
            private readonly HashSet<MemberInfo> _members = new(new MemberInfoEqualityComparer());
            private readonly HashSet<MethodInfo> _closedGenericMethods = new();

            public bool IsInclusive { get; set; }

            public bool Check(MemberInfo member)
            {
                Debug.Assert(member != null);

                if (member.MemberType == MemberTypes.Method)
                {
                    return Check((MethodInfo)member);
                }
                else
                {
                    return _members.Contains(member);
                }
            }

            private bool Check(MethodInfo method)
            {
                if (method.IsGenericMethod)
                {
                    if (_closedGenericMethods.Contains(method))
                    {
                        return true;
                    }

                    method = method.GetGenericMethodDefinition();
                }

                return _members.Contains(method);
            }

            public void Add(MemberInfo member)
            {
                Debug.Assert(member != null);

                if (member.MemberType == MemberTypes.Method)
                {
                    Add((MethodInfo)member);
                }
                else
                {
                    _members.Add(member);
                }
            }

            private void Add(MethodInfo method)
            {
                if (method.IsGenericMethod && !method.IsGenericMethodDefinition)
                {
                    _closedGenericMethods.Add(method);
                }
                else
                {
                    _members.Add(method);
                }
            }

            private sealed class MemberInfoEqualityComparer : IEqualityComparer<MemberInfo>
            {
                public bool Equals(MemberInfo x, MemberInfo y)
                {
                    return x.MetadataToken == y.MetadataToken && x.Module == y.Module;
                }

                public int GetHashCode(MemberInfo obj)
                {
                    return obj.MetadataToken * 17 + obj.Module.GetHashCode();
                }
            }
        }

        internal void AddDeclaringType(Type type)
        {
            if (_entries.TryGetValue(type, out Entry entry))
            {
                if (!entry.IsInclusive)
                {
                    throw InvalidEntry(type);
                }
            }

            entry = new Entry { IsInclusive = true };
            _entries[type] = entry;
        }

        internal void AddMember(MemberInfo member)
        {
            var type = member.DeclaringType;

            if (_entries.TryGetValue(type, out Entry entry))
            {
                if (entry.IsInclusive)
                {
                    throw InvalidEntry(type);
                }
            }
            else
            {
                entry = new Entry { IsInclusive = false };
                _entries[type] = entry;
            }

            entry.Add(member);
        }

        private static InvalidOperationException InvalidEntry(Type type)
        {
            return new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Type '{0}' already occurs on the allow list. Either specify the entire type as an allow list declaring type, or specify its allowed members.", type));
        }
    }

    /// <summary>
    /// List of declaring types that are allowed.
    /// </summary>
    public class TypeList : IEnumerable<Type>
    {
        private readonly Action<Type> _add;

        internal TypeList(Action<Type> add) => _add = add;

        /// <summary>
        /// Adds the specified type to the list of allowed types, optionally including base types.
        /// </summary>
        /// <param name="type">Type to add to the list of allowed types.</param>
        /// <param name="includeBase">Indicates whether to include base types, i.e. base classes and interfaces.</param>
        public void Add(Type type, bool includeBase)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            AddImpl(type, includeBase);
        }

        /// <summary>
        /// Adds the specified type to the list of allowed types.
        /// </summary>
        /// <param name="type">Type to add to the list of allowed types.</param>
        public void Add(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            AddImpl(type);
        }

        private void AddImpl(Type type, bool includeBase)
        {
            AddImpl(type);

            if (includeBase)
            {
                var t = type.BaseType;
                while (t != null)
                {
                    AddImpl(t);
                    t = t.BaseType;
                }

                foreach (var i in type.GetInterfaces())
                {
                    AddImpl(i);
                }
            }
        }

        internal void AddImpl(Type type)
        {
            if (type.ContainsGenericParameters)
            {
                type = type.GetGenericTypeDefinition();
            }

            _add(type);
        }

        /// <summary>
        /// Not supported. This method is only here to support collection initializers.
        /// </summary>
        /// <returns>Always throws.</returns>
        IEnumerator<Type> IEnumerable<Type>.GetEnumerator() => throw new NotSupportedException();

        /// <summary>
        /// Not supported. This method is only here to support collection initializers.
        /// </summary>
        /// <returns>Always throws.</returns>
        IEnumerator IEnumerable.GetEnumerator() => throw new NotSupportedException();
    }

    /// <summary>
    /// List of members that are allowed.
    /// </summary>
    public class MemberList : IEnumerable<MemberInfo>
    {
        private readonly ExpressionMemberAllowListScanner _parent;

        internal MemberList(ExpressionMemberAllowListScanner parent) => _parent = parent;

        /// <summary>
        /// Adds the specified member to the list of allowed members.
        /// </summary>
        /// <param name="member">Member to add to the list of allowed members.</param>
        public void Add(MemberInfo member)
        {
            if (member == null)
                throw new ArgumentNullException(nameof(member));

            _parent.AddMember(member);
        }

        /// <summary>
        /// Not supported. This method is only here to support collection initializers.
        /// </summary>
        /// <returns>Always throws.</returns>
        IEnumerator<MemberInfo> IEnumerable<MemberInfo>.GetEnumerator() => throw new NotSupportedException();

        /// <summary>
        /// Not supported. This method is only here to support collection initializers.
        /// </summary>
        /// <returns>Always throws.</returns>
        IEnumerator IEnumerable.GetEnumerator() => throw new NotSupportedException();
    }
}
