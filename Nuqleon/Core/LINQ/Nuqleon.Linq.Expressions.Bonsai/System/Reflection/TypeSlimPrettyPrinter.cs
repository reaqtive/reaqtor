// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
// ER - July 2013 - Small tweaks.
//

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace System.Reflection
{
    internal class TypeSlimPrettyPrinter : TypeSlimVisitor<string, string, string, string, string, string, string>
    {
        private const string RECURSION_CANARY = "__CANARY__";
        private readonly Dictionary<StructuralTypeSlim, string> _visited;
        private int _revisited;


        public TypeSlimPrettyPrinter()
        {
            _revisited = 0;
            _visited = new Dictionary<StructuralTypeSlim, string>();
        }

        public override string Visit(TypeSlim typeSlim)
        {
            if (typeSlim == null)
            {
                return "null";
            }

            return base.Visit(typeSlim);
        }

        protected override string VisitSimple(SimpleTypeSlim type)
        {
            return type.Name;
        }

        protected override string MakeArrayType(ArrayTypeSlim type, string elementType, int? rank)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}[{1}]", elementType, new string(',', (rank ?? 1) - 1));
        }

        protected override string MakeGenericDefinition(GenericDefinitionTypeSlim type)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}", type.Name);
        }

        protected override string VisitGeneric(GenericTypeSlim type)
        {
            return MakeGeneric(type, type.GenericTypeDefinition.Name, VisitGenericTypeArguments(type));
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
                    res = string.Format(CultureInfo.InvariantCulture, "{{{0}}}", _revisited);
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
            var propertiesStr = string.Join(", ", properties);
            var alias = _visited[type] != RECURSION_CANARY ? string.Format(CultureInfo.InvariantCulture, " as {0}", _visited[type]) : "";
            return string.Format(CultureInfo.InvariantCulture, "{{{0}}}{1}", propertiesStr, alias);
        }

        private static string MakeStructuralProperty(PropertyInfoSlim property, string type, ReadOnlyCollection<string> propertyIndexParameters)
        {
            var keyPrefix = !property.CanWrite ? "Key " : "";
            if (property.IndexParameterTypes.Count > 0)
            {
                var indexParameters = string.Join(", ", propertyIndexParameters);
                return string.Format(CultureInfo.InvariantCulture, "{0}{1}[{2}] : {3}", keyPrefix, property.Name, indexParameters, type);
            }
            else
            {
                return string.Format(CultureInfo.InvariantCulture, "{0}{1} : {2}", keyPrefix, property.Name, type);
            }
        }
    }
}
