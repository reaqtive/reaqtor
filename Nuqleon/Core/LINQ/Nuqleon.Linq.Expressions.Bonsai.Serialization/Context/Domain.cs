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
using System.Linq.CompilerServices;
using System.Reflection;
using Json = Nuqleon.Json.Expressions;

namespace System.Linq.Expressions.Bonsai.Serialization
{
    internal abstract class Domain
    {
        protected static readonly TypeSlim[] s_noTypesArray = Array.Empty<TypeSlim>();
        protected static readonly ReadOnlyCollection<TypeSlim> s_noTypesCollection = EmptyReadOnlyCollection<TypeSlim>.Instance;

        protected abstract Version Version { get; }

        public bool IsV08 => Version == null || Version.Major == 0 && Version.Minor == 8;

        public bool SupportsVersion(Version version)
        {
            var thisVersion = Version ?? BonsaiVersion.V08;
            return Normalize(thisVersion) >= Normalize(version);
        }

        private static Version Normalize(Version version)
        {
            var major = Normalize(version.Major);
            var minor = Normalize(version.Minor);
            var revision = Normalize(version.Revision);
            var build = Normalize(version.Build);

            if (version.Major != major || version.Minor != minor || version.Revision != revision || version.Build != build)
            {
                return new Version(major, minor, revision, build);
            }

            return version;
        }

        private static int Normalize(int i) => i == -1 ? 0 : i;
    }

    internal sealed class SerializationDomain : Domain
    {
        private readonly Dictionary<TypeSlim, TypeRef> _types;
        private readonly Dictionary<AssemblySlim, int> _assemblies;
        private readonly Dictionary<MemberInfoSlim, int> _members;
        private readonly List<TypeDef> _typeDefs;
        private readonly List<MemberDef> _memberDefs;
        private readonly List<AssemblySlim> _assemblyDefs;

        public SerializationDomain(Version version)
        {
            _types = new Dictionary<TypeSlim, TypeRef>();
            _assemblies = new Dictionary<AssemblySlim, int>();
            _members = new Dictionary<MemberInfoSlim, int>();

            _typeDefs = new List<TypeDef>();
            _assemblyDefs = new List<AssemblySlim>();
            _memberDefs = new List<MemberDef>();

            Version = version;
        }

        protected override Version Version { get; }

        public TypeRef AddType(TypeSlim type) => AddType(type, s_noTypesCollection);

        public TypeRef AddType(TypeSlim type, ReadOnlyCollection<TypeSlim> genericArguments)
        {
            if (type.Kind == TypeSlimKind.GenericParameter)
            {
                return AddTypeGenericParameter((GenericParameterTypeSlim)type, genericArguments);
            }

            if (!_types.TryGetValue(type, out TypeRef res))
            {
                res = type.Kind switch
                {
                    TypeSlimKind.Simple => AddTypeSimple((SimpleTypeSlim)type),
                    TypeSlimKind.Array => AddTypeArray((ArrayTypeSlim)type, genericArguments),
                    TypeSlimKind.GenericDefinition => AddTypeGenericDefinition((GenericDefinitionTypeSlim)type),
                    TypeSlimKind.Generic => AddTypeGeneric((GenericTypeSlim)type, genericArguments),
                    TypeSlimKind.Structural => AddTypeStructural((StructuralTypeSlim)type, genericArguments),
                    _ => throw new NotImplementedException(),
                };
                _types[type] = res;
            }
            return res;
        }

        private TypeRef[] AddTypes(ReadOnlyCollection<TypeSlim> types) => AddTypes(types, s_noTypesCollection);

        private TypeRef[] AddTypes(ReadOnlyCollection<TypeSlim> types, ReadOnlyCollection<TypeSlim> genericArguments)
        {
            var count = types.Count;
            var typeRefs = new TypeRef[count];

            for (var i = 0; i < count; i++)
            {
                typeRefs[i] = AddType(types[i], genericArguments);
            }

            return typeRefs;
        }

        private TypeRef AddTypeArray(ArrayTypeSlim type, ReadOnlyCollection<TypeSlim> genericArguments)
        {
            var rank = type.Rank;
            var elementType = AddType(type.ElementType, genericArguments);

            var index = _typeDefs.Count;
            _typeDefs.Add(new ArrayTypeDef(elementType, rank));
            return new SimpleTypeRef(index);
        }

        private TypeRef AddTypeGeneric(GenericTypeSlim type, ReadOnlyCollection<TypeSlim> genericArguments)
        {
            var genericTypeDefinition = AddType(type.GenericTypeDefinition, genericArguments);

            var genericTypeArgumentCount = type.GenericArgumentCount;
            var genericTypeArguments = new TypeRef[genericTypeArgumentCount];

            for (var i = 0; i < genericTypeArgumentCount; i++)
            {
                var argType = type.GetGenericArgument(i);
                var typeRef = AddType(argType, genericArguments);
                genericTypeArguments[i] = typeRef;
            }

            var index = _typeDefs.Count;
            _typeDefs.Add(new GenericTypeDef(genericTypeDefinition, genericTypeArguments));
            return new SimpleTypeRef(index);
        }

        private TypeRef AddTypeGenericDefinition(GenericDefinitionTypeSlim type)
        {
            if (!_assemblies.TryGetValue(type.Assembly, out int asmIndex))
            {
                asmIndex = _assemblies.Count;
                _assemblies[type.Assembly] = asmIndex;
                _assemblyDefs.Add(type.Assembly);
            }

            var index = _typeDefs.Count;
            _typeDefs.Add(new SimpleTypeDef(type.Name, asmIndex));
            return new SimpleTypeRef(index);
        }

        private static TypeRef AddTypeGenericParameter(GenericParameterTypeSlim type, ReadOnlyCollection<TypeSlim> genericArguments)
        {
            var index = genericArguments.IndexOf(type);
            if (index < 0)
                throw new InvalidOperationException("Can't find generic argument.");

            return new SimpleTypeRef(-1 - index);
        }

        private TypeRef AddTypeSimple(SimpleTypeSlim type)
        {
            if (!_assemblies.TryGetValue(type.Assembly, out int asmIndex))
            {
                asmIndex = _assemblies.Count;
                _assemblies[type.Assembly] = asmIndex;
                _assemblyDefs.Add(type.Assembly);
            }

            var index = _typeDefs.Count;
            _typeDefs.Add(new SimpleTypeDef(type.Name, asmIndex));
            return new SimpleTypeRef(index);
        }

        private TypeRef AddTypeStructural(StructuralTypeSlim type, ReadOnlyCollection<TypeSlim> genericArguments)
        {
            var typeRef = new StructuralTypeRef();
            _types[type] = typeRef;
            var typeDef = type.StructuralKind switch
            {
                StructuralTypeSlimKind.Anonymous => AddTypeAnonymousStructural(type, genericArguments),
                StructuralTypeSlimKind.Record => AddTypeRecordStructural(type, genericArguments),
                _ => throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Structural type of kind '{0}' not supported.", type.StructuralKind)),
            };
            var index = _typeDefs.Count;
            _typeDefs.Add(typeDef);
            typeRef.SetIndex(index);
            return typeRef;
        }

        private TypeDef AddTypeAnonymousStructural(StructuralTypeSlim type, ReadOnlyCollection<TypeSlim> genericArguments)
        {
            var properties = type.Properties;
            var count = properties.Count;

            var members = new AnonymousStructuralTypeMember[count];
            for (var i = 0; i < count; i++)
            {
                var property = properties[i];
                var member = new AnonymousStructuralTypeMember { Name = property.Name, Type = AddType(property.PropertyType, genericArguments), IsKey = !property.CanWrite };
                members[i] = member;
            }

            return new AnonymousStructuralTypeDef(members);
        }

        private TypeDef AddTypeRecordStructural(StructuralTypeSlim type, ReadOnlyCollection<TypeSlim> genericArguments)
        {
            var properties = type.Properties;
            var count = properties.Count;

            var members = new RecordStructuralTypeMember[count];
            for (var i = 0; i < count; i++)
            {
                var property = properties[i];
                var member = new RecordStructuralTypeMember { Name = property.Name, Type = AddType(property.PropertyType, genericArguments) };
                members[i] = member;
            }

            return new RecordStructuralTypeDef(type.HasValueEqualitySemantics, members);
        }

        public Json.Expression AddMember(MemberInfoSlim member)
        {
            if (member == null)
                throw new ArgumentNullException(nameof(member));

            if (!_members.TryGetValue(member, out int index))
            {
                var typeRef = AddType(member.DeclaringType);
                var memberDef = member.MemberType switch
                {
                    MemberTypes.Method => AddMethod(typeRef, (MethodInfoSlim)member),
                    MemberTypes.Constructor => AddConstructor(typeRef, (ConstructorInfoSlim)member),
                    MemberTypes.Field => new FieldDef(typeRef, (FieldInfoSlim)member),
                    MemberTypes.Property => new PropertyDef(typeRef, (PropertyInfoSlim)member),
                    _ => throw new NotSupportedException("Invalid member type."),
                };
                index = _members.Count;
                _memberDefs.Add(memberDef);
                _members.Add(member, index);
            }

            return index.ToJsonNumber();
        }

        private MemberDef AddMethod(TypeRef declaringTypeRef, MethodInfoSlim method)
        {
            return method.Kind switch
            {
                MethodInfoSlimKind.Simple => AddSimpleMethod(declaringTypeRef, (SimpleMethodInfoSlim)method),
                MethodInfoSlimKind.GenericDefinition => AddGenericDefinitionMethod(declaringTypeRef, (GenericDefinitionMethodInfoSlim)method),
                MethodInfoSlimKind.Generic => AddGenericMethod((GenericMethodInfoSlim)method),
                _ => throw new NotImplementedException(),
            };
        }

        private MemberDef AddGenericDefinitionMethod(TypeRef declaringTypeRef, GenericDefinitionMethodInfoSlim method)
        {
            var genericArgs = method.GenericParameterTypes;
            var parameters = AddTypes(method.ParameterTypes, genericArgs);
            var returnType = AddType(method.ReturnType, genericArgs);
            return new OpenGenericMethodDef(declaringTypeRef, method, returnType, parameters);
        }

        private MemberDef AddGenericMethod(GenericMethodInfoSlim method)
        {
            var genericDefinition = method.GenericMethodDefinition;
            var genericMethodIndex = AddMember(genericDefinition);
            var genericArgs = AddTypes(method.GenericArguments);
            return new ClosedGenericMethodDef(genericMethodIndex, genericArgs);
        }

        private MemberDef AddSimpleMethod(TypeRef declaringTypeRef, SimpleMethodInfoSlim method)
        {
            var parameters = AddTypes(method.ParameterTypes);
            var returnType = AddType(method.ReturnType);
            return new SimpleMethodDef(declaringTypeRef, method, returnType, parameters);
        }

        private MemberDef AddConstructor(TypeRef declaringTypeRef, ConstructorInfoSlim constructor)
        {
            var parameters = AddTypes(constructor.ParameterTypes);
            return new ConstructorDef(declaringTypeRef, constructor, parameters);
        }

        internal Dictionary<string, Json.Expression> GetDomainContext()
        {
            var res = new Dictionary<string, Json.Expression>();

            if (_memberDefs.Count != 0)
            {
                var members = SerializeMembers();
                res.Add("Members", members);
            }

            var types = SerializeTypes();
            var assemblies = SerializeAssemblies();

            res.Add("Types", types);
            res.Add("Assemblies", assemblies);

            if (Version != null)
            {
                // PERF: Consider if it's worth to cache the JSON expression with a version string
                //       given that the range of versions is very narrow.

                res.Add("Version", Json.Expression.String(Version.ToString()));
            }

            return res;
        }

        private Json.Expression SerializeAssemblies()
        {
            var count = _assemblyDefs.Count;
            var assemblyDefs = new Json.Expression[count];
            for (var i = 0; i < count; i++)
            {
                assemblyDefs[i] = Json.Expression.String(_assemblyDefs[i].Name);
            }

            return Json.Expression.Array(assemblyDefs);
        }

        private Json.Expression SerializeTypes()
        {
            var count = _typeDefs.Count;
            var typeDefs = new Json.Expression[count];
            for (var i = 0; i < count; i++)
            {
                typeDefs[i] = _typeDefs[i].ToJson(this);
            }

            return Json.Expression.Array(typeDefs);
        }

        private Json.Expression SerializeMembers()
        {
            var count = _memberDefs.Count;
            var memberDefs = new Json.Expression[count];
            for (var i = 0; i < count; i++)
            {
                memberDefs[i] = _memberDefs[i].ToJson(this);
            }

            return Json.Expression.Array(memberDefs);
        }
    }

    internal sealed class DeserializationDomain : Domain
    {
        private readonly TypeDef[] _typeDefs;
        private readonly MemberDef[] _memberDefs;
        private readonly AssemblySlim[] _assemblyDefs;

        public DeserializationDomain(Json.Expression json)
        {
            if (json is not Json.ObjectExpression obj)
                throw new BonsaiParseException("Expected a JSON object containing the context object used by the Bonsai representation of an expression.", json);

            if (obj.Members.TryGetValue("Version", out Json.Expression version))
            {
                Version = DeserializeVersion(version);
            }

            if (!obj.Members.TryGetValue("Assemblies", out Json.Expression assemblies))
                throw new BonsaiParseException("Expected a JSON object property 'node.Assemblies' containing the assembly table.", json);

            DeserializeAssemblies(assemblies, ref _assemblyDefs);

            if (!obj.Members.TryGetValue("Types", out Json.Expression types))
                throw new BonsaiParseException("Expected a JSON object property 'node.Types' containing the type table.", json);

            DeserializeTypes(types, ref _typeDefs);

            if (obj.Members.TryGetValue("Members", out Json.Expression members))
            {
                DeserializeMembers(members, ref _memberDefs);
            }
            else
            {
                _memberDefs = Array.Empty<MemberDef>();
            }
        }

        protected override Version Version { get; }

        private static Version DeserializeVersion(Json.Expression version)
        {
            if (version.NodeType != Json.ExpressionType.String)
                throw new BonsaiParseException("Expected a JSON string containing the Bonsai version.", version);

            return new Version((string)((Json.ConstantExpression)version).Value);
        }

        private static void DeserializeAssemblies(Json.Expression assemblies, ref AssemblySlim[] assemblyDefs)
        {
            //
            // TODO: confirm this behavior, technically "optional" in Bonsai spec, so value could be null.
            //
            if (assemblies is not Json.ArrayExpression assemblyTable)
                throw new BonsaiParseException("Expected a JSON array containing the assembly table.", assemblies);

            var n = assemblyTable.ElementCount;
            assemblyDefs = new AssemblySlim[n];

            for (var i = 0; i < n; i++)
            {
                var assembly = assemblyTable.GetElement(i);

                if (assembly.NodeType != Json.ExpressionType.String)
                    throw new BonsaiParseException("Expected a JSON string containing the an assembly name.", assembly);

                var asm = new AssemblySlim((string)((Json.ConstantExpression)assembly).Value);
                assemblyDefs[i] = asm;
            }
        }

        private void DeserializeTypes(Json.Expression types, ref TypeDef[] typeDefs)
        {
            //
            // TODO: confirm this behavior, technically "optional" in Bonsai spec, so value could be null.
            //
            if (types is not Json.ArrayExpression typeTable)
                throw new BonsaiParseException("Expected a JSON array containing the type table.", types);

            var n = typeTable.ElementCount;
            typeDefs = new TypeDef[n];

            for (var i = 0; i < n; i++)
            {
                var type = typeTable.GetElement(i);

                var typeDef = TypeDef.FromJson(this, type);
                typeDefs[i] = typeDef;
            }
        }

        private void DeserializeMembers(Json.Expression members, ref MemberDef[] memberDefs)
        {
            //
            // TODO: confirm this behavior, technically "optional" in Bonsai spec, so value could be null.
            //
            if (members is not Json.ArrayExpression memberTable)
                throw new BonsaiParseException("Expected a JSON array containing the members table.", members);

            var n = memberTable.ElementCount;
            memberDefs = new MemberDef[n];

            for (var i = 0; i < n; i++)
            {
                var member = memberTable.GetElement(i);

                var memberDef = MemberDef.FromJson(this, member);
                memberDefs[i] = memberDef;
            }
        }

        public TypeSlim GetType(Json.Expression expression) => GetType(expression, s_noTypesArray);

        public TypeSlim GetType(Json.Expression expression, params TypeSlim[] genericArguments)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (expression.NodeType != Json.ExpressionType.Number)
                throw new BonsaiParseException("Expected a JSON number for the type table index of the type to look up.", expression);

            var index = Helpers.ParseInt32((string)((Json.ConstantExpression)expression).Value);
            if (index < 0)
                return genericArguments[-index - 1];

            if (index >= _typeDefs.Length)
                throw new BonsaiParseException(string.Format(CultureInfo.InvariantCulture, "A type with index {0} was not found in the type table.", index), expression);

            var def = _typeDefs[index];
            return def.ToType(this, genericArguments);
        }

        public MemberInfoSlim GetMember(Json.Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (expression.NodeType != Json.ExpressionType.Number)
                throw new BonsaiParseException("Expected a JSON number for the member table index of the member to look up.", expression);

            var index = Helpers.ParseInt32((string)((Json.ConstantExpression)expression).Value);

            if (index < 0 || index >= _memberDefs.Length)
                throw new BonsaiParseException(string.Format(CultureInfo.InvariantCulture, "A member with index {0} was not found in the member table.", index), expression);

            var def = _memberDefs[index];
            return def.ToMember(this);
        }

        public AssemblySlim GetAssembly(int index) => _assemblyDefs[index];

        public TypeDef GetType(TypeRef typeRef)
        {
            if (typeRef == null)
                throw new ArgumentNullException(nameof(typeRef));

            if (typeRef.Index < 0)
            {
                return new GenericParameterTypeDef(-typeRef.Index - 1);
            }

            return _typeDefs[typeRef.Index];
        }
    }
}
