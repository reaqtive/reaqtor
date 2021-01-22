// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Nuqleon.DataModel.Serialization.Json
{
    /// <summary>
    /// Contract resolver for IPE data model.
    /// </summary>
    internal sealed class MappingContractResolver : DefaultContractResolver
    {
        /// <summary>
        /// Shared instance of the contract resolver which only considers public members.
        /// </summary>
        /// <remarks>
        /// Orginally, we used an overload to <see cref="DefaultContractResolver"/> that took
        /// a <c>shareCache</c> parameter, which is now obsolete. As a short term solution,
        /// we'll cache a singleton instance over here, but it should really be passed in to
        /// the serializer so we can control the lifetime of the entries it keeps.
        /// </remarks>
        public static readonly MappingContractResolver Instance = new(includePrivate: false);

        /// <summary>
        /// Shared instance of the contract resolver which also considers private members.
        /// </summary>
        public static readonly MappingContractResolver IncludePrivate = new(includePrivate: true);

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingContractResolver"/> class.
        /// </summary>
        /// <param name="includePrivate">If true, private members are included in serialization.</param>
        private MappingContractResolver(bool includePrivate)
        {
            if (includePrivate)
            {
#pragma warning disable 618
                DefaultMembersSearchFlags |= BindingFlags.NonPublic;
#pragma warning restore
            }
        }

        /// <summary>
        /// Creates a <see cref="JsonArrayContract" /> for the given type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        /// A <see cref="JsonArrayContract" /> for the given type.
        /// </returns>
        protected override JsonArrayContract CreateArrayContract(Type objectType)
        {
            Debug.Assert(objectType != null, "Incoming type should not be null.");

            ThrowIfMappingAttributesAreFound(objectType);
            return base.CreateArrayContract(objectType);
        }

        /// <summary>
        /// Creates a <see cref="JsonDictionaryContract" /> for the given type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        /// A <see cref="JsonDictionaryContract" /> for the given type.
        /// </returns>
        protected override JsonDictionaryContract CreateDictionaryContract(Type objectType)
        {
            Debug.Assert(objectType != null, "Incoming type should not be null.");

            ThrowIfMappingAttributesAreFound(objectType);

            // Checking that only one IDictionary interface is implemented.
            var idictionary = (from i in objectType.GetInterfaces()
                               where i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDictionary<,>)
                               select i).ToList();

            if (idictionary.Count != 1)
            {
                throw new NotSupportedException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "Multiple implementations of IDictionary<TKey, TValue> are forbidden. Please, fix type '{0}'.",
                        objectType));
            }

            // Checking that object is Dictionary<string, T>.
            Debug.Assert(idictionary[0].GetGenericArguments().Length == 2, "idictionary should be of type IDictionary<,>.");

            if (idictionary[0].GetGenericArguments()[0] != typeof(string))
            {
                throw new NotSupportedException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "Unsupported type '{0}' is used as a key in type '{1}'. Only strings are currently supported.",
                        idictionary[0],
                        idictionary[0].GetGenericArguments()[0]));
            }

            return base.CreateDictionaryContract(objectType);
        }

        /// <summary>
        /// Creates properties for the given <see cref="JsonContract" />.
        /// </summary>
        /// <param name="objectType">The type to create properties for.</param>
        /// <param name="memberSerialization">The member serialization mode for the type.</param>
        /// <returns>
        /// Properties for the given <see cref="JsonContract" />.
        /// </returns>
        protected override IList<JsonProperty> CreateProperties(Type objectType, MemberSerialization memberSerialization)
        {
            Debug.Assert(objectType != null, "Incoming type should not be null.");

            var allProperties = base.CreateProperties(objectType, memberSerialization);
            Debug.Assert(allProperties != null, "Properties should not be null.");

            // For anonymous types and record types we simply return all properties. We assume names of the properties
            // are equal the mapping attributes.
            if (objectType.IsAnonymousType() || objectType.IsRecordType())
            {
                return allProperties;
            }

            // Checking if inherited from Tuple.
            foreach (var type in objectType.InheritanceChain())
            {
                if (type.IsTuple())
                {
                    ThrowIfMappingAttributesAreFound(objectType);
                    return allProperties;
                }
            }

            var mappingProperties = RetrieveMappingProperties(allProperties);

            // Check if there are duplicates in mapping properties.
            var propertyNames = new HashSet<string>();

            foreach (var property in mappingProperties)
            {
                var name = property.PropertyName;

                if (!propertyNames.Add(name))
                {
                    throw new NotSupportedException(
                        string.Format(
                            CultureInfo.InvariantCulture,
                            "Type '{0}' is not supported due to the duplicate mapping property '{1}'.",
                            objectType,
                            name));
                }
            }

            return mappingProperties;
        }

        /// <summary>
        /// Retrieves the mapping properties from JSON properties changing their names to URIs.
        /// </summary>
        /// <param name="properties">The properties.</param>
        /// <returns>A list of properties with mapping attributes.</returns>
        private List<JsonProperty> RetrieveMappingProperties(IEnumerable<JsonProperty> properties)
        {
            Debug.Assert(properties != null, "Properties should not be null.");

            var res = new List<JsonProperty>();

            foreach (var property in properties)
            {
                var declaringType = property.DeclaringType;
                var name = property.UnderlyingName;

                // TODO: Use GetSerializableMembers in lieu of obsolete DefaultMembersSearchFlags.
#pragma warning disable 0618
                var info = declaringType.GetMember(name, MemberTypes.Field | MemberTypes.Property, DefaultMembersSearchFlags).SingleOrDefault();
#pragma warning restore

                if (info != null)
                {
                    var mapping = info.GetCustomAttribute<MappingAttribute>(inherit: false);
                    if (mapping != null)
                    {
                        property.PropertyName = mapping.Uri;
                        res.Add(property);
                    }
                }
            }

            return res;
        }

        /// <summary>
        /// Throws if mapping attributes are found.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <exception cref="NotSupportedException">Is thrown in case of a mapping attribute has been found.</exception>
        private void ThrowIfMappingAttributesAreFound(Type objectType)
        {
            Debug.Assert(objectType != null, "Object type should not be null.");

            var allProperties = base.CreateProperties(objectType, MemberSerialization.OptOut);
            Debug.Assert(allProperties != null, "Properties should not be null.");

            List<JsonProperty> mappings = RetrieveMappingProperties(allProperties);

            if (mappings.Count != 0)
            {
                throw new NotSupportedException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "Type '{0}' is  not allowed to have mapping properties.",
                        objectType));
            }
        }
    }
}
