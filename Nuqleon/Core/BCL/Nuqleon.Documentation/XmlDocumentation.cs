// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
//   BD - 07/19/2014 - Created this type.
//

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
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
        /// Lazily loaded collection of XDocument objects for assemblies a member was queried for.
        /// Notice the use of a CWT to ensure that the use of collectible assemblies with this
        /// class's functionality doesn't cause them to become non-collectible.
        /// </summary>
        private static readonly ConditionalWeakTable<Assembly, XDocument> s_assemblies = new();

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
                res = doc.Elements("param").FirstOrDefault(m => m.Attribute("name").Value == parameter.Name);
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

            var asm = (member as Type ?? member.DeclaringType).Assembly;

            var xml = GetXmlDocFile(asm);
            if (xml != null)
            {
                var mem = GetXmlDocNameImpl(member);

                var doc = xml.Element("doc");
                if (doc != null)
                {
                    var ms = doc.Element("members");
                    if (ms != null)
                    {
                        res = ms.Elements("member").FirstOrDefault(m => m.Attribute("name").Value == mem);
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

                        res = "M:" + tdn + "." + Escape(m.Name) + a + GetParamsListDocName(m.GetParameters(), targs, margs);

                        if (m.Name is "op_Explicit" or "op_Implicit")
                        {
                            res += "~" + GetTypeDocName(m.ReturnType, targs, margs);
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

        private static string GetParamsListDocName(ParameterInfo[] parameters, Type[] genTypeArgs, Type[] genMtdArgs)
        {
            if (parameters.Length == 0)
            {
                return "";
            }
            else
            {
                return "(" + string.Join(",", parameters.Select(p => GetTypeDocName(p.ParameterType, genTypeArgs, genMtdArgs))) + ")";
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
        /// Tries to locate the XML documentation file for the given assembly. If found,
        /// the document is loaded and returned as an XDocument. If not found, null is
        /// returned.
        /// </summary>
        /// <param name="assembly">Assembly to find the XML documentation file for.</param>
        /// <returns>XDocument containing the XML documentation for the specified assembly, if found; otherwise, null.</returns>
        private static XDocument GetXmlDocFile(Assembly assembly)
        {
            return s_assemblies.GetValue(assembly, asm =>
            {
                var res = default(XDocument);

                var location = assembly.Location;
                var fileName = Path.GetFileNameWithoutExtension(location);
                var xmlDocFileName = fileName + ".xml";

                var candidateDirectories = (IEnumerable<string>)new[]
                {
                    Path.GetDirectoryName(location),
                };

                var programFiles = Environment.GetFolderPath(Environment.Is64BitOperatingSystem ? Environment.SpecialFolder.ProgramFilesX86 : Environment.SpecialFolder.ProgramFiles);
                var referenceAssemblies = Path.Combine(programFiles, @"Reference Assemblies\Microsoft\Framework\.NETFramework");

                if (Directory.Exists(referenceAssemblies))
                {
                    candidateDirectories = candidateDirectories.Concat(
                        from d in Directory.GetDirectories(referenceAssemblies)
                        let f = Path.GetFileName(d)
                        where f.StartsWith("v", StringComparison.Ordinal)
                        orderby f descending
                        select d
                    );
                }

                foreach (var directory in candidateDirectories)
                {
                    var xmlDocFile = Path.Combine(directory, xmlDocFileName);
                    if (File.Exists(xmlDocFile))
                    {
                        res = XDocument.Load(xmlDocFile);
                        break;
                    }
                }

                return res;
            });
        }
    }
}
