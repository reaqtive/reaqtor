// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - June 2013 - Created this file.
// ER - July 2013 - Small tweaks.
//

using System.Reflection;
using Json = Nuqleon.Json.Expressions;

namespace System.Linq.Expressions.Bonsai.Serialization
{
    internal abstract class StructuralTypeDef : TypeDef
    {
        protected readonly StructuralTypeMember[] _members;
        protected StructuralTypeSlim _type;

        public StructuralTypeDef(params StructuralTypeMember[] members)
        {
            _members = members;
        }

        public abstract StructuralTypeSlimKind Kind { get; }
    }

    internal sealed class AnonymousStructuralTypeDef : StructuralTypeDef
    {
        public AnonymousStructuralTypeDef(params AnonymousStructuralTypeMember[] members)
            : base(members)
        {
        }

        public override StructuralTypeSlimKind Kind => StructuralTypeSlimKind.Anonymous;

        public override TypeSlim ToType(DeserializationDomain domain, params TypeSlim[] genericArguments)
        {
            if (_type == null)
            {
                var refType = StructuralTypeSlimReference.Create(hasValueEqualitySemantics: true, Kind, _members.Length);
                _type = refType;
                foreach (var member in _members)
                {
                    var newPropertyType = domain.GetType(member.Type).ToType(domain, genericArguments);
                    var newProperty = _type.GetProperty(member.Name, newPropertyType, s_emptyTypeList, member.CanWrite);
                    refType.AddProperty(newProperty);
                }
                refType.Freeze();
            }

            return _type;
        }

        public static StructuralTypeDef FromJson(Json.ArrayExpression type)
        {
            var n = type.ElementCount - 1;

            var stms = new AnonymousStructuralTypeMember[n];

            for (var i = 0; i < n; i++)
            {
                var element = type.GetElement(i + 1);
                var member = AnonymousStructuralTypeMember.FromJson(element);
                stms[i] = member;
            }

            return new AnonymousStructuralTypeDef(stms);
        }

        public override Json.Expression ToJson(SerializationDomain domain)
        {
            var n = _members.Length;

            var args = new Json.Expression[n + 1];

            args[0] = Discriminators.Type.AnonymousDiscriminator;

            for (var i = 0; i < n; i++)
            {
                args[i + 1] = _members[i].ToJson(domain);
            }

            return Json.Expression.Array(
                args
            );
        }
    }

    internal sealed class RecordStructuralTypeDef : StructuralTypeDef
    {
        private readonly bool _hasValueEqualitySemantics;

        public RecordStructuralTypeDef(bool hasValueEqualitySemantics, params RecordStructuralTypeMember[] members)
            : base(members)
        {
            _hasValueEqualitySemantics = hasValueEqualitySemantics;
        }

        public override StructuralTypeSlimKind Kind => StructuralTypeSlimKind.Record;

        public static RecordStructuralTypeDef FromJson(DeserializationDomain domain, Json.ArrayExpression type)
        {
            if (domain.IsV08)
                throw new NotSupportedException("Record types are only supported in Bonsai v0.9 or later.");

            var count = type.ElementCount;

            if (count is not 2 and not 3)
                throw new BonsaiParseException("Expected 2 or 3 JSON array elements for a structural type definition.", type);

            if (type.GetElement(1) is not Json.ArrayExpression membersJson)
                throw new BonsaiParseException("Expected a JSON array in 'node[1]' for the structural type members.", type);

            var membersCount = membersJson.ElementCount;

            var members = new RecordStructuralTypeMember[membersCount];

            for (var i = 0; i < membersCount; i++)
            {
                var memberJson = membersJson.GetElement(i);
                var member = RecordStructuralTypeMember.FromJson(memberJson);
                members[i] = member;
            }

            var hasValueEqualitySemantics = true;
            if (count == 3)
            {
                var hasValueEqualitySemanticsExpr = type.GetElement(2);

                if (hasValueEqualitySemanticsExpr.NodeType != Json.ExpressionType.Boolean)
                    throw new BonsaiParseException("Expected a JSON Boolean in 'node[2]' for the value equality semantics flag.", type);

                hasValueEqualitySemantics = (bool)((Json.ConstantExpression)hasValueEqualitySemanticsExpr).Value;
            }

            return new RecordStructuralTypeDef(hasValueEqualitySemantics, members);
        }

        public override Json.Expression ToJson(SerializationDomain domain)
        {
            if (domain.IsV08)
                throw new NotSupportedException("Record types can only be serialized in Bonsai v0.9 or later.");

            var count = _members.Length;

            var members = new Json.Expression[count];
            for (var i = 0; i < count; i++)
            {
                members[i] = _members[i].ToJson(domain);
            }

            if (!_hasValueEqualitySemantics)
            {
                return Json.Expression.Array(
                    Discriminators.Type.RecordDiscriminator,
                    Json.Expression.Array(members),
                    Json.Expression.Boolean(false)
                );
            }
            else
            {
                return Json.Expression.Array(
                    Discriminators.Type.RecordDiscriminator,
                    Json.Expression.Array(members)
                );
            }
        }

        public override TypeSlim ToType(DeserializationDomain domain, params TypeSlim[] genericArguments)
        {
            if (_type == null)
            {
                var refType = StructuralTypeSlimReference.Create(_hasValueEqualitySemantics, Kind, _members.Length);
                _type = refType;
                foreach (var member in _members)
                {
                    var newPropertyType = domain.GetType(member.Type).ToType(domain, genericArguments);
                    var newProperty = _type.GetProperty(member.Name, newPropertyType, s_emptyTypeList, member.CanWrite);
                    refType.AddProperty(newProperty);
                }
                refType.Freeze();
            }

            return _type;
        }
    }

    internal abstract class StructuralTypeMember
    {
        public string Name { get; set; }
        public TypeRef Type { get; set; }

        public abstract bool CanWrite { get; }

        public abstract Json.Expression ToJson(SerializationDomain domain);

        public override string ToString()
        {
            var key = CanWrite ? "Key " : "";
            return key + base.ToString();
        }
    }

    internal sealed class AnonymousStructuralTypeMember : StructuralTypeMember
    {
        public bool IsKey { get; set; }

        public override bool CanWrite => !IsKey;

        public static AnonymousStructuralTypeMember FromJson(Json.Expression expression)
        {
            if (expression is not Json.ArrayExpression array)
                throw new BonsaiParseException("Expected a JSON array for an anonymous structural type member definition.", expression);

            var count = array.ElementCount;
            if (count is not 2 and not 3)
                throw new BonsaiParseException("Expected 2 or 3 JSON array elements for an anonymous structural type member definition.", expression);

            var nameExpr = array.GetElement(0);
            if (nameExpr.NodeType != Json.ExpressionType.String)
                throw new BonsaiParseException("Expected a JSON string in 'node[0]' for the name of an anonymous structural type member definition.", expression);

            var name = (string)((Json.ConstantExpression)nameExpr).Value;
            var type = TypeRef.FromJson(array.GetElement(1));

            var isKey = true;
            if (count == 3)
            {
                var isKeyExpr = array.GetElement(2);

                if (isKeyExpr.NodeType != Json.ExpressionType.Boolean)
                    throw new BonsaiParseException("Expected a JSON Boolean in 'node[2]' for IsKey flag of an anonymous structural type member definition.", expression);

                isKey = (bool)((Json.ConstantExpression)isKeyExpr).Value;
            }

            return new AnonymousStructuralTypeMember { Name = name, Type = type, IsKey = isKey };
        }

        public override Json.Expression ToJson(SerializationDomain domain)
        {
            var type = Type.ToJson();

            if (!IsKey)
            {
                return Json.Expression.Array(
                    Json.Expression.String(Name),
                    type,
                    Json.Expression.Boolean(false)
                );
            }
            else
            {
                return Json.Expression.Array(
                    Json.Expression.String(Name),
                    type
                );
            }
        }
    }

    internal sealed class RecordStructuralTypeMember : StructuralTypeMember
    {
        public override bool CanWrite => true;

        public static RecordStructuralTypeMember FromJson(Json.Expression expression)
        {
            if (expression is not Json.ArrayExpression array)
                throw new BonsaiParseException("Expected a JSON array for a record structural type member definition.", expression);

            if (array.ElementCount != 2)
                throw new BonsaiParseException("Expected 2 JSON array elements for a record structural type member definition.", expression);

            var nameExpr = array.GetElement(0);
            if (nameExpr.NodeType != Json.ExpressionType.String)
                throw new BonsaiParseException("Expected a JSON string in 'node[0]' for the name of a record structural type member definition.", expression);

            var name = (string)((Json.ConstantExpression)nameExpr).Value;
            var type = TypeRef.FromJson(array.GetElement(1));

            return new RecordStructuralTypeMember { Name = name, Type = type };
        }

        public override Json.Expression ToJson(SerializationDomain domain)
        {
            var type = Type.ToJson();

            return Json.Expression.Array(
                Json.Expression.String(Name),
                type
            );
        }
    }
}
