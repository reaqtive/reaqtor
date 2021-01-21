// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2014 - Created this file.
//

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace System.Reflection
{
    internal sealed class TypeSlimCSharpPrinter : TypeSlimVisitor<string, string, string, string, string, string, string>
    {
        private static readonly TypeSlim s_nullable = typeof(Nullable<>).ToTypeSlim();

        private const string RECURSION_CANARY = "__CANARY__";
        private readonly Dictionary<StructuralTypeSlim, string> _visited;
        private int _revisited;

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable format // (Formatted as a table.)

        private static readonly Dictionary<TypeSlim, string> s_typeNames = new()
        {
            { typeof(void   ).ToTypeSlim(), "void"    },
            { typeof(object ).ToTypeSlim(), "object"  },
            { typeof(byte   ).ToTypeSlim(), "byte"    },
            { typeof(sbyte  ).ToTypeSlim(), "sbyte"   },
            { typeof(short  ).ToTypeSlim(), "short"   },
            { typeof(ushort ).ToTypeSlim(), "ushort"  },
            { typeof(int    ).ToTypeSlim(), "int"     },
            { typeof(uint   ).ToTypeSlim(), "uint"    },
            { typeof(long   ).ToTypeSlim(), "long"    },
            { typeof(ulong  ).ToTypeSlim(), "ulong"   },
            { typeof(float  ).ToTypeSlim(), "float"   },
            { typeof(double ).ToTypeSlim(), "double"  },
            { typeof(decimal).ToTypeSlim(), "decimal" },
            { typeof(bool   ).ToTypeSlim(), "bool"    },
            { typeof(char   ).ToTypeSlim(), "char"    },
            { typeof(string ).ToTypeSlim(), "string"  },
        };

#pragma warning restore format
#pragma warning restore IDE0079

        public TypeSlimCSharpPrinter()
        {
            _revisited = 0;
            _visited = new Dictionary<StructuralTypeSlim, string>();
        }

        protected override string VisitSimple(SimpleTypeSlim type)
        {

            if (!s_typeNames.TryGetValue(type, out string res))
            {
                res = type.Name;
            }

            return res;
        }

        protected override string MakeArrayType(ArrayTypeSlim type, string elementType, int? rank)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}[{1}]", elementType, new string(',', (rank ?? 1) - 1));
        }

        protected override string MakeGenericDefinition(GenericDefinitionTypeSlim type)
        {
            GetGenericTypeNameAndArity(type, out string name, out int arity);

            if (arity > 0)
            {
                return name + "<" + new string(',', arity - 1) + ">";
            }
            else
            {
                return name + "<?>";
            }
        }

        protected override string VisitGeneric(GenericTypeSlim type)
        {
            if (type.GenericTypeDefinition == s_nullable)
            {
                return Visit(type.GetGenericArgument(0)) + "?";
            }

            var defName = GetGenericTypeNameWithoutArity(type.GenericTypeDefinition);

            return MakeGeneric(type, defName, VisitGenericTypeArguments(type));
        }

        protected override string MakeGeneric(GenericTypeSlim type, string typeDefinition, ReadOnlyCollection<string> arguments)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}<{1}>", typeDefinition, string.Join(", ", arguments));
        }

        protected override string VisitGenericParameter(GenericParameterTypeSlim type)
        {
            return type.Name;
        }

        protected override string VisitStructural(StructuralTypeSlim type)
        {
            if (_visited.TryGetValue(type, out string res))
            {
                if (res == RECURSION_CANARY)
                {
                    _revisited++;
                    res = string.Format(CultureInfo.InvariantCulture, "T{0}", _revisited);
                    _visited[type] = res;
                }
                return res;
            }

            _visited[type] = RECURSION_CANARY;

            return base.VisitStructural(type);
        }

        protected override string MakeStructuralType(StructuralTypeSlim type, IEnumerable<KeyValuePair<PropertyInfoSlim, string>> propertyTypes, IEnumerable<KeyValuePair<PropertyInfoSlim, ReadOnlyCollection<string>>> propertyIndexParameters)
        {
            var properties = propertyTypes.Join(propertyIndexParameters, p => p.Key, p => p.Key, (kv1, kv2) => MakeStructuralProperty(kv1.Key, kv1.Value, kv2.Value));
            var propertiesStr = string.Join(" ", properties);
            var alias = _visited[type] != RECURSION_CANARY ? string.Format(CultureInfo.InvariantCulture, "class {0} ", _visited[type]) : "";
            return string.Format(CultureInfo.InvariantCulture, "{1}{{ {0} }}", propertiesStr, alias);

            static string MakeStructuralProperty(PropertyInfoSlim property, string type, ReadOnlyCollection<string> propertyIndexParameters)
            {
                return propertyIndexParameters.Count > 0
                    ? string.Format(CultureInfo.InvariantCulture, "{0} {1}[{2}];", type, property.Name, string.Join(", ", propertyIndexParameters))
                    : string.Format(CultureInfo.InvariantCulture, "{0} {1};", type, property.Name);
            }
        }

        private static void GetGenericTypeNameAndArity(GenericDefinitionTypeSlim type, out string name, out int arity)
        {
            name = type.Name;
            arity = 0;

            var i = type.Name.LastIndexOf('`');
            if (i >= 0)
            {
                name = type.Name.Substring(0, i);
            }

            if (i + 1 < type.Name.Length)
            {
#if NET5_0 || NETSTANDARD2_1
                arity = int.Parse(type.Name[(i + 1)..], CultureInfo.InvariantCulture);
#else
                arity = int.Parse(type.Name.Substring(i + 1), CultureInfo.InvariantCulture);
#endif
            }
        }

        private static string GetGenericTypeNameWithoutArity(GenericDefinitionTypeSlim type)
        {
            var name = type.Name;

            var i = name.LastIndexOf('`');
            if (i >= 0)
            {
                name = name.Substring(0, i);
            }

            return name;
        }
    }
}
