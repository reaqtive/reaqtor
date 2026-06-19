// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER, BD - July 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Nuqleon.DataModel;

namespace Tests.Nuqleon.DataModel.CompilerServices
{
    internal class RecordTreeComparator : ExpressionComparator
    {
        public RecordTreeComparator(TypeComparator typeComparer)
            : base(typeComparer, ObjectComparator.CreateInstance())
        {
        }

        public override bool Equals(Expression x, Expression y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            if (x.NodeType == ExpressionType.New && y.NodeType == ExpressionType.MemberInit)
            {
                return Equals((NewExpression)x, (MemberInitExpression)y);
            }

            if (x.NodeType == ExpressionType.MemberInit && y.NodeType == ExpressionType.New)
            {
                return Equals((NewExpression)y, (MemberInitExpression)x);
            }

            if (x.NodeType != y.NodeType)
            {
                return false;
            }

            return base.Equals(x, y);
        }

        protected override bool EqualsMemberInit(MemberInitExpression x, MemberInitExpression y)
        {
            if (!x.NewExpression.Type.IsRecordType() && y.NewExpression.Type.IsRecordType())
            {
                var xEntities = GetEntitiesFromExpression(x);
                var yEntities = GetEntitiesFromExpression(y, isRecordType: true);
                return CompareEntitySets(xEntities, yEntities);
            }

            return base.EqualsMemberInit(x, y);
        }

        protected override bool EqualsMemberListBinding(MemberListBinding x, MemberListBinding y)
        {
            if (!x.Member.ReflectedType.IsRecordType() && y.Member.ReflectedType.IsRecordType())
            {
                var e1 = x.Initializers.GetEnumerator();
                var e2 = y.Initializers.GetEnumerator();

                while (e1.MoveNext())
                {
                    //Compares add method and compares all arguments
                    if (!e2.MoveNext() || !(Equals(e1.Current.AddMethod, e2.Current.AddMethod)) || e1.Current.Arguments.Count != e2.Current.Arguments.Count ||
                        !e1.Current.Arguments.Zip(e2.Current.Arguments, (arg1, arg2) => Equals(arg1, arg2)).Aggregate((t1, t2) => t1 && t2))
                        return false;
                }
                if (e2.MoveNext()) return false;

                return Equals(x.Member, y.Member);
            }

            return base.EqualsMemberListBinding(x, y);
        }

        protected override bool EqualsNew(NewExpression x, NewExpression y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return
                Equals(x.Type, y.Type) && // Constructor can be null
                Equals(x.Constructor, y.Constructor) &&
                Equals(x.Arguments, y.Arguments) &&

                // Expression.New(Type) where a default constructor is available emits
                // a NewExpression with a null `Members` property. The  NewExpression
                // also has the `Constructor` property set, so a visitor would expect
                // to re-construct the NewExpression using the
                // Expression.New(ConstructorInfo, ...) factory method. However, when
                // using this method, the `Members` property can never be set to null,
                // the `Members` property is automatically converted to an empty array.
                (Equals(x.Members, y.Members) ||
                x.Members == null && y.Members.Count == 0 ||
                x.Members.Count == 0 && y.Members == null);
        }

        private bool Equals(MemberBinding x, Expression y)
        {
            if (x.BindingType == MemberBindingType.MemberBinding && y.NodeType == ExpressionType.MemberInit)
            {
                return Equals((MemberMemberBinding)x, (MemberInitExpression)y);
            }

            if (x.BindingType == MemberBindingType.ListBinding && y.NodeType == ExpressionType.ListInit)
            {
                return Equals((MemberListBinding)x, (ListInitExpression)y);
            }

            throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Cannot compare member binding of type '{0}' with expression of type '{1}'", x.GetType().FullName, y.GetType().FullName));
        }

        private bool Equals(NewExpression x, MemberInitExpression y)
        {
            var xEntities = GetEntitiesFromExpression(x);
            var yEntities = GetEntitiesFromExpression(y, isRecordType: true);

            return CompareEntitySets(xEntities, yEntities);
        }

        private bool Equals(MemberMemberBinding x, MemberInitExpression y)
        {
            var xEntities = GetEntitiesFromMemberBindings(x.Bindings);
            var yEntities = GetEntitiesFromExpression(y, isRecordType: true);

            return CompareEntitySets(xEntities, yEntities);
        }

        private bool Equals(MemberListBinding x, ListInitExpression y)
        {
            var e1 = x.Initializers.GetEnumerator();
            var e2 = y.Initializers.GetEnumerator();

            while (e1.MoveNext())
            {
                if (!e2.MoveNext() ||
                    !e1.Current.Arguments
                        .Zip(e2.Current.Arguments, (expr1, expr2) => Equals(expr1, expr2))
                        .Aggregate((b1, b2) => b1 && b2))
                {
                    return false;
                }
            }

            return Equals(GetMemberType(x.Member), y.NewExpression.Type);
        }

        private bool CompareEntitySets(Dictionary<string, EntityInfo> xEntities, Dictionary<string, EntityInfo> yEntities)
        {
            if (xEntities.Keys.Count != yEntities.Keys.Count)
            {
                return false;
            }

            foreach (var kvp in xEntities)
            {
                if (!yEntities.ContainsKey(kvp.Key))
                {
                    return false;
                }

                var xEntity = kvp.Value;
                var yEntity = yEntities[kvp.Key];

                if (xEntity.HasExpression && yEntity.HasExpression &&
                    (!Equals(xEntity.Expression, yEntity.Expression) || !Equals(xEntity.Type, yEntity.Type)))
                {
                    return false;
                }

                else if (!xEntity.HasExpression && yEntity.HasExpression &&
                    (!Equals(xEntity.Binding, yEntity.Expression) || !Equals(xEntity.Type, yEntity.Type)))
                {
                    return false;
                }

                else if (!xEntity.HasExpression && !yEntity.HasExpression &&
                    (!Equals(xEntity.Binding, yEntity.Binding) || !Equals(xEntity.Type, yEntity.Type)))
                {
                    return false;
                }
            }

            return true;
        }

        private static Dictionary<string, EntityInfo> GetEntitiesFromExpression(MemberInitExpression expression, bool isRecordType = false)
        {
            var entities = new Dictionary<string, EntityInfo>();

            var constructorEntities = GetEntitiesFromExpression(expression.NewExpression);
            var bindingEntities = GetEntitiesFromMemberBindings(expression.Bindings, isRecordType);

            foreach (var kvp in constructorEntities)
            {
                entities.Add(kvp.Key, kvp.Value);
            }

            foreach (var kvp in bindingEntities)
            {
                entities.Add(kvp.Key, kvp.Value);
            }

            return entities;
        }

        private static Dictionary<string, EntityInfo> GetEntitiesFromExpression(NewExpression expression)
        {
            var entities = new Dictionary<string, EntityInfo>();
            var ctorParams = expression.Constructor.GetParameters();

            for (var i = 0; i < ctorParams.Length; ++i)
            {
                var uri = ctorParams[i].GetCustomAttribute<MappingAttribute>(inherit: false).Uri;
                entities.Add(uri, new EntityInfo { Expression = expression.Arguments[i], Type = ctorParams[i].ParameterType });
            }

            return entities;
        }

        private static Dictionary<string, EntityInfo> GetEntitiesFromMemberBindings(IEnumerable<MemberBinding> bindings, bool isRecordType = false)
        {
            if (isRecordType)
            {
                return GetEntitiesFromRecordTypeMemberBindings(bindings);
            }

            var entities = new Dictionary<string, EntityInfo>();
            foreach (var binding in bindings)
            {
                var mapping = binding.Member.GetCustomAttribute<MappingAttribute>(inherit: false);
                if (mapping != null)
                {
                    entities.Add(mapping.Uri, GetEntityFromMemberBinding(binding));
                }
            }
            return entities;
        }

        private static Dictionary<string, EntityInfo> GetEntitiesFromRecordTypeMemberBindings(IEnumerable<MemberBinding> bindings)
        {
            var entities = new Dictionary<string, EntityInfo>();

            foreach (var binding in bindings)
            {
                entities.Add(binding.Member.Name, GetEntityFromRecordTypeBinding(binding));
            }

            return entities;
        }

        private static EntityInfo GetEntityFromMemberBinding(MemberBinding binding)
        {
            switch (binding.BindingType)
            {
                case MemberBindingType.Assignment:
                    var assignment = (binding as MemberAssignment);
                    return new EntityInfo { Type = GetMemberType(assignment.Member), Expression = assignment.Expression };
                case MemberBindingType.ListBinding:
                    return new EntityInfo { Type = GetMemberType(binding.Member), Binding = (binding as MemberListBinding), HasExpression = false };
                case MemberBindingType.MemberBinding:
                    return new EntityInfo { Type = GetMemberType(binding.Member), Binding = (binding as MemberMemberBinding), HasExpression = false };
                default:
                    throw new NotImplementedException();
            }
        }

        private static EntityInfo GetEntityFromRecordTypeBinding(MemberBinding binding)
        {
            switch (binding.BindingType)
            {
                case MemberBindingType.Assignment:
                    var assignment = (binding as MemberAssignment);
                    return new EntityInfo { Expression = assignment.Expression, Type = GetMemberType(assignment.Member) };
                case MemberBindingType.ListBinding:
                    return new EntityInfo { Type = GetMemberType(binding.Member), Binding = (binding as MemberListBinding), HasExpression = false };
                case MemberBindingType.MemberBinding:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }

        private static Type GetMemberType(MemberInfo member)
        {
            if (member is PropertyInfo property)
            {
                return property.PropertyType;
            }
            else
            {
                var field = (FieldInfo)member;
                return field.FieldType;
            }
        }

        private class EntityInfo
        {
            public EntityInfo() { HasExpression = true; }
            public bool HasExpression { get; set; }
            public Expression Expression { get; set; }
            public MemberBinding Binding { get; set; }
            public Type Type { get; set; }
        }
    }
}
