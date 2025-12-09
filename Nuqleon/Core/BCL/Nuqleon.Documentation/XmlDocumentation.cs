// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/19/2014 - Created this type.
//   IG - 2025/12    - Modified to work with .NET SDK reference assemblies.
//

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace System.Documentation
{
    /// <summary>
    /// Provides a set of extension methods to gather XML documentation for types and their members
    /// using their reflection object representations.
    /// </summary>
    public static class XmlDocumentation
    {
        /// <summary>
        /// Lazily loaded map from type names to reference assembly locations.
        /// </summary>
        /// <remarks>
        /// The T4 templates that use this class typically pass reflection objects based on runtime
        /// types (and not from reflection-only assemblies). The runtime class libraries often use
        /// type forwarders to enable them to put the runtime implementations of types in different
        /// assemblies than the reference assemblies where they were originally defined, which
        /// means that the <c>Assembly</c> objects reported by reflection won't necessarily
        /// correspond to the reference assembly that defines the type. That's a problem because
        /// the XML documentation files are associated with reference assemblies, not runtime ones.
        /// So we need to be able to locate the reference assembly for any given type. And the only
        /// way we can do that is to scan all of the reference assemblies. To avoid doing that for
        /// every type lookup, we build a map from types to assemblies the first time we need it.
        /// </remarks>
        private static WeakReference<Dictionary<string, string>> s_typeNameToRefAssemblyLocation;

        /// <summary>
        /// Lazily loaded collection of XDocument objects for reference assemblies.
        /// </summary>
        private static WeakReference<Dictionary<string, XDocument>> s_assemblies;

        /// <summary>
        /// Gets the XML documentation for the specified parameter.
        /// </summary>
        /// <param name="parameter">Parameter to get XML documentation for.</param>
        /// <returns>The XML documentation for the specified parameter, if found; otherwise, null.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="parameter"/> is null.</exception>
        public static XElement GetXmlDoc(this ParameterInfo parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));

            var res = default(XElement);

            var member = parameter.Member;

            var doc = GetXmlDoc(member);
            if (doc != null)
            {
                res = doc.Elements(XNames.Param).FirstOrDefault(m => m.Attribute(XNames.Name).Value == parameter.Name);
            }

            return res;
        }

        /// <summary>
        /// Gets the XML documentation for the specified member.
        /// </summary>
        /// <param name="member">Member to get XML documentation for.</param>
        /// <returns>The XML documentation for the specified member, if found; otherwise, null.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="member"/> is null.</exception>
        public static XElement GetXmlDoc(this MemberInfo member)
        {
            if (member == null)
                throw new ArgumentNullException(nameof(member));

            var res = default(XElement);

            var type = member as Type ?? member.DeclaringType;

            var xml = GetXmlDocFile(type);
            if (xml != null)
            {
                var mem = GetXmlDocNameImpl(member);

                var doc = xml.Element(XNames.Doc);
                if (doc != null)
                {
                    var ms = doc.Element(XNames.Members);
                    if (ms != null)
                    {
                        res = ms.Elements(XNames.Member).FirstOrDefault(m => m.Attribute(XNames.Name).Value == mem);
                    }
                }
            }

            return res;
        }

        /// <summary>
        /// Gets the name used to refer to the specified member in XML documentation files.
        /// See http://msdn.microsoft.com/en-us/library/fsbx0t7x.aspx for the specification of this format.
        /// </summary>
        /// <param name="member">Member to get the name for.</param>
        /// <returns>Name of the member for use in XML documentation files.</returns>
        public static string GetXmlDocName(this MemberInfo member)
        {
            if (member == null)
                throw new ArgumentNullException(nameof(member));

            if (member is Type t)
            {
                if (t.IsGenericTypeDefinition)
                {
                    return "T:" + GetTypeDocName(t);
                }
                else
                {
                    // Just to avoid the unexpected; normally, closed generics etc. cannot occur with a T: in front of them.
                    var targs = t.IsGenericType ? t.GetGenericArguments() : Type.EmptyTypes;
                    return "T:" + GetTypeDocName(t, targs, Type.EmptyTypes);
                }
            }

            return GetXmlDocNameImpl(member);
        }

        private static string GetXmlDocNameImpl(MemberInfo member)
        {
            var res = "";

            var typ = member.DeclaringType;

            var tdn = "";
            var targs = Type.EmptyTypes;
            if (typ != null)
            {
                tdn = GetTypeDocName(typ);
                targs = typ.IsGenericType ? typ.GetGenericArguments() : Type.EmptyTypes;
            }

            switch (member.MemberType)
            {
                case MemberTypes.Constructor:
                    res = "M:" + tdn + ".#ctor" + GetParamsListDocName(((ConstructorInfo)member).GetParameters(), targs, Type.EmptyTypes);
                    break;
                case MemberTypes.Field:
                    res = "F:" + tdn + "." + Escape(((FieldInfo)member).Name);
                    break;
                case MemberTypes.Event:
                    res = "E:" + tdn + "." + Escape(((EventInfo)member).Name);
                    break;
                case MemberTypes.Method:
                    {
                        var m = (MethodInfo)member;
                        var a = "";
                        var margs = Type.EmptyTypes;
                        if (m.IsGenericMethod)
                        {
                            margs = m.GetGenericArguments();
                            a = "``" + margs.Length;
                        }

                        var isConversionOperator = m.Name is "op_Explicit" or "op_Implicit";
                        res = "M:" + tdn + "." + Escape(m.Name) + a + GetParamsListDocName(m.GetParameters(), targs, margs, isConversionOperator);

                        if (isConversionOperator)
                        {
                            var returnTypeText = m.ReturnType.IsGenericParameter
                                ? m.ReturnType.Name
                                : GetTypeDocName(m.ReturnType, targs, margs);
                            res += "~" + returnTypeText;
                        }
                    }
                    break;
                case MemberTypes.Property:
                    {
                        var p = (PropertyInfo)member;
                        res = "P:" + tdn + "." + Escape(p.Name) + GetParamsListDocName(p.GetIndexParameters(), targs, Type.EmptyTypes);
                    }
                    break;
                case MemberTypes.TypeInfo:
                case MemberTypes.NestedType:
                    res = "T:" + GetTypeDocName((Type)member);
                    break;
                case MemberTypes.Custom:
                case MemberTypes.All:
                default:
                    break;
            }

            return res;
        }

        private static string Escape(string memberName)
        {
            return memberName.Replace(".", "#").Replace("<", "{").Replace(">", "}");
        }

        private static string GetParamsListDocName(ParameterInfo[] parameters, Type[] genTypeArgs, Type[] genMtdArgs, bool isConversionOperator = false)
        {
            if (parameters.Length == 0)
            {
                return "";
            }
            else
            {
                return "(" + string.Join(",", parameters.Select(p =>
                {
                    if (isConversionOperator)
                    {
                        // The implicit and explicit conversion operators use the type parameter name
                        // instead of the positional form (`0, `1 etc) in the parameter list.
                        if (p.ParameterType.IsArray)
                        {
                            var elementType = p.ParameterType.GetElementType();
                            if (elementType.IsGenericParameter)
                            {
                                return elementType.Name + "[]";
                            }
                        }

                        if (p.ParameterType.IsGenericParameter)
                        {
                            return p.ParameterType.Name;
                        }
                    }
                    return GetTypeDocName(p.ParameterType, genTypeArgs, genMtdArgs);
                })) + ")";
            }
        }

        private static string GetTypeDocName(Type type)
        {
            string res;

            if (type.IsGenericType)
            {
                var ns = type.Namespace;
                res = ns + (!string.IsNullOrEmpty(ns) ? "." : "") + type.Name;
            }
            else
            {
                res = type.FullName;
            }

            return res.Replace("&", "@").Replace("+", ".");
        }

        private static string GetTypeDocName(Type type, Type[] genTypeArgs, Type[] genMtdArgs)
        {
            var res = "";

            if (type.IsGenericType)
            {
                var d = type.GetGenericTypeDefinition();
                var n = d.FullName;
                n = n.Substring(0, n.LastIndexOf('`'));

                res = n + "{" + string.Join(",", type.GetGenericArguments().Select(t => GetTypeDocName(t, genTypeArgs, genMtdArgs))) + "}";
            }
            else if (type.IsArray)
            {
                var e = GetTypeDocName(type.GetElementType(), genTypeArgs, genMtdArgs);
                var r = type.GetArrayRank();
                var rank = "";
                if (r > 1)
                {
                    rank = string.Join(",", Enumerable.Repeat("0:", r));
                }
                res = e + "[" + rank + "]";
            }
            else if (type.IsByRef)
            {
                var e = GetTypeDocName(type.GetElementType(), genTypeArgs, genMtdArgs);
                res = e + "&";
            }
            else if (type.IsGenericParameter)
            {
                var i = Array.IndexOf(genTypeArgs, type);
                if (i >= 0)
                {
                    res = "`" + i;
                }
                else
                {
                    i = Array.IndexOf(genMtdArgs, type);
                    if (i >= 0)
                        res = "``" + i;
                }
            }
            else
            {
                res = type.FullName;
            }

            return res.Replace("&", "@").Replace("+", ".");
        }

        /// <summary>
        /// Tries to locate the XML documentation file for the reference assembly that defines
        /// the given type. If found, the document is loaded and returned as an XDocument. If not
        /// found, null is returned.
        /// </summary>
        /// <param name="type">
        /// Type for which to find the containing reference assembly's XML documentation file.
        /// </param>
        /// <returns>
        /// XDocument containing the XML documentation for the reference assembly that defines the
        /// specified type, if found; otherwise, null.
        /// </returns>
        private static XDocument GetXmlDocFile(Type type)
        {
            static string GetTypeKey(Type t) => t.Namespace + "." + t.Name; // FullName is null for generic types

            if (s_typeNameToRefAssemblyLocation is null ||
                !s_typeNameToRefAssemblyLocation.TryGetTarget(out var typesToRefAssemblyLocations))
            {
                var referenceAssembliesFolder = Nuqleon.Documentation.RuntimeInfo.ReferenceAssembliesFolder;
                var referenceAssemblies = Directory.Exists(referenceAssembliesFolder)
                    ? Directory.GetFiles(referenceAssembliesFolder, "*.dll", SearchOption.AllDirectories)
                    : Array.Empty<string>();
                var resolver = new PathAssemblyResolver(referenceAssemblies);
                using var mlc = new MetadataLoadContext(resolver);
                typesToRefAssemblyLocations = new Dictionary<string, string>();
                foreach (var dll in referenceAssemblies)
                {
                    var asm = mlc.LoadFromAssemblyPath(dll);
                    foreach (var t in asm.GetTypes())
                    {
                        typesToRefAssemblyLocations.TryAdd(GetTypeKey(t), asm.Location);
                    }
                }

                if (s_typeNameToRefAssemblyLocation is null)
                {
                    s_typeNameToRefAssemblyLocation = new(typesToRefAssemblyLocations);
                }
                else
                {
                    s_typeNameToRefAssemblyLocation.SetTarget(typesToRefAssemblyLocations);
                }
            }

            if (!typesToRefAssemblyLocations.TryGetValue(GetTypeKey(type), out var assemblyLocation))
            {
                return null;
            }

            s_assemblies ??= new(new Dictionary<string, XDocument>());

            if (!s_assemblies.TryGetTarget(out var assemblyLocationToXmlDoc))
            {
                assemblyLocationToXmlDoc = new();
                s_assemblies.SetTarget(assemblyLocationToXmlDoc);
            }

            if (!assemblyLocationToXmlDoc.TryGetValue(assemblyLocation, out XDocument res))
            {
                var fileName = Path.GetFileNameWithoutExtension(assemblyLocation);
                var xmlDocFileName = fileName + ".xml";

                var referenceAssemblies = Nuqleon.Documentation.RuntimeInfo.ReferenceAssembliesFolder;

                if (Directory.Exists(referenceAssemblies))
                {
                    var xmlDocFile = Path.Combine(referenceAssemblies, xmlDocFileName);
                    if (File.Exists(xmlDocFile))
                    {
                        res = XDocument.Load(xmlDocFile);
                        assemblyLocationToXmlDoc.Add(assemblyLocation, res);
                    }
                }
            }

            return res;
        }

        // Profiling shows that a surprising amount of time is spent in the implicit conversion
        // from string to XName if we just pass string literals directly to XDocument APIs,
        // so we cache the XName instances we need here.
        private static class XNames
        {
            public static readonly XName Doc = XName.Get("doc");
            public static readonly XName Member = XName.Get("member");
            public static readonly XName Members = XName.Get("members");
            public static readonly XName Name = XName.Get("name");
            public static readonly XName Param = XName.Get("param");
        }
    }
}
