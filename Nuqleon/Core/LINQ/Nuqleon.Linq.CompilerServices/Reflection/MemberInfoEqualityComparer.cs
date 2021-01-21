// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - September 2013 - Created this file.
//

#pragma warning disable IDE0079 // Remove unnecessary suppression.
#pragma warning disable CA1062 // Omitted null checks similar to expression tree visitors.

using System.Collections.Generic;
using System.Linq;

namespace System.Reflection
{
    /// <summary>
    /// Equality comparer for CLR member info, based on their underlying Equals method
    /// </summary>
    public class MemberInfoEqualityComparer : IEqualityComparer<MemberInfo>
    {
        /// <summary>
        /// Checks whether two type members are equal.
        /// </summary>
        /// <param name="x">First type member.</param>
        /// <param name="y">Second type member.</param>
        /// <returns>True if the members are equal, false otherwise.</returns>
        public virtual bool Equals(MemberInfo x, MemberInfo y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            var xMemberType = x.MemberType;
            var yMemberType = y.MemberType;

            if (xMemberType != yMemberType)
                return false;

            return xMemberType switch
            {
                MemberTypes.Constructor => EqualsConstructor((ConstructorInfo)x, (ConstructorInfo)y),
                MemberTypes.Custom => EqualsCustom(x, y),
                MemberTypes.Event => EqualsEvent((EventInfo)x, (EventInfo)y),
                MemberTypes.Field => EqualsField((FieldInfo)x, (FieldInfo)y),
                MemberTypes.Method => EqualsMethod((MethodInfo)x, (MethodInfo)y),
                MemberTypes.NestedType => EqualsNestedType((Type)x, (Type)y),
                MemberTypes.Property => EqualsProperty((PropertyInfo)x, (PropertyInfo)y),
                MemberTypes.TypeInfo => EqualsType((TypeInfo)x, (TypeInfo)y),
                _ => EqualsExtension(x, y),
            };
        }

        /// <summary>
        /// Checks whether two type members are equal.
        /// </summary>
        /// <param name="x">First type member.</param>
        /// <param name="y">Second type member.</param>
        /// <returns>True if the members are equal, false otherwise.</returns>
        protected virtual bool EqualsConstructor(ConstructorInfo x, ConstructorInfo y) => EqualityComparer<ConstructorInfo>.Default.Equals(x, y);

        /// <summary>
        /// Checks whether two type members are equal.
        /// </summary>
        /// <param name="x">First type member.</param>
        /// <param name="y">Second type member.</param>
        /// <returns>True if the members are equal, false otherwise.</returns>
        /// 
        protected virtual bool EqualsEvent(EventInfo x, EventInfo y) => EqualityComparer<EventInfo>.Default.Equals(x, y);

        /// <summary>
        /// Checks whether two type members are equal.
        /// </summary>
        /// <param name="x">First type member.</param>
        /// <param name="y">Second type member.</param>
        /// <returns>True if the members are equal, false otherwise.</returns>
        protected virtual bool EqualsField(FieldInfo x, FieldInfo y) => EqualityComparer<FieldInfo>.Default.Equals(x, y);

        /// <summary>
        /// Checks whether two type members are equal.
        /// </summary>
        /// <param name="x">First type member.</param>
        /// <param name="y">Second type member.</param>
        /// <returns>True if the members are equal, false otherwise.</returns>
        protected virtual bool EqualsMethod(MethodInfo x, MethodInfo y) => EqualityComparer<MethodInfo>.Default.Equals(x, y);

        /// <summary>
        /// Checks whether two type members are equal.
        /// </summary>
        /// <param name="x">First type member.</param>
        /// <param name="y">Second type member.</param>
        /// <returns>True if the members are equal, false otherwise.</returns>
        protected virtual bool EqualsProperty(PropertyInfo x, PropertyInfo y) => EqualityComparer<PropertyInfo>.Default.Equals(x, y);

        /// <summary>
        /// Checks whether two type members are equal.
        /// </summary>
        /// <param name="x">First type member.</param>
        /// <param name="y">Second type member.</param>
        /// <returns>True if the members are equal, false otherwise.</returns>
        protected virtual bool EqualsNestedType(Type x, Type y) => EqualityComparer<Type>.Default.Equals(x, y);

        /// <summary>
        /// Checks whether two type members are equal.
        /// </summary>
        /// <param name="x">First type member.</param>
        /// <param name="y">Second type member.</param>
        /// <returns>True if the members are equal, false otherwise.</returns>
        protected virtual bool EqualsType(TypeInfo x, TypeInfo y) => EqualityComparer<TypeInfo>.Default.Equals(x, y);

        /// <summary>
        /// Checks whether two type members are equal.
        /// </summary>
        /// <param name="x">First type member.</param>
        /// <param name="y">Second type member.</param>
        /// <returns>True if the members are equal, false otherwise.</returns>
        protected virtual bool EqualsCustom(MemberInfo x, MemberInfo y) => throw new NotImplementedException("Equality of custom member types to be supplied by derived types.");

        /// <summary>
        /// Checks whether two type members are equal.
        /// </summary>
        /// <param name="x">First type member.</param>
        /// <param name="y">Second type member.</param>
        /// <returns>True if the members are equal, false otherwise.</returns>
        protected virtual bool EqualsExtension(MemberInfo x, MemberInfo y) => throw new NotSupportedException("Member type is not supported for this comparer.");

        /// <summary>
        /// Gets a hash code for the given type member.
        /// </summary>
        /// <param name="obj">Member to compute a hash code for.</param>
        /// <returns>Hash code for the given member.</returns>
        public virtual int GetHashCode(MemberInfo obj)
        {
            if (obj == null)
            {
                return EqualityComparer<MemberInfo>.Default.GetHashCode(obj);
            }

            return obj.MemberType switch
            {
                MemberTypes.Constructor => GetHashCodeConstructor((ConstructorInfo)obj),
                MemberTypes.Custom => GetHashCodeCustom(obj),
                MemberTypes.Event => GetHashCodeEvent((EventInfo)obj),
                MemberTypes.Field => GetHashCodeField((FieldInfo)obj),
                MemberTypes.Method => GetHashCodeMethod((MethodInfo)obj),
                MemberTypes.NestedType => GetHashCodeNestedType((Type)obj),
                MemberTypes.Property => GetHashCodeProperty((PropertyInfo)obj),
                MemberTypes.TypeInfo => GetHashCodeType((TypeInfo)obj),
                _ => GetHashCodeExtension(obj),
            };
        }

        /// <summary>
        /// Gets a hash code for the given type member.
        /// </summary>
        /// <param name="obj">Member to compute a hash code for.</param>
        /// <returns>Hash code for the given member.</returns>
        protected virtual int GetHashCodeConstructor(ConstructorInfo obj) => EqualityComparer<ConstructorInfo>.Default.GetHashCode(obj);

        /// <summary>
        /// Gets a hash code for the given type member.
        /// </summary>
        /// <param name="obj">Member to compute a hash code for.</param>
        /// <returns>Hash code for the given member.</returns>
        protected virtual int GetHashCodeEvent(EventInfo obj) => EqualityComparer<EventInfo>.Default.GetHashCode(obj);

        /// <summary>
        /// Gets a hash code for the given type member.
        /// </summary>
        /// <param name="obj">Member to compute a hash code for.</param>
        /// <returns>Hash code for the given member.</returns>
        protected virtual int GetHashCodeField(FieldInfo obj) => EqualityComparer<FieldInfo>.Default.GetHashCode(obj);

        /// <summary>
        /// Gets a hash code for the given type member.
        /// </summary>
        /// <param name="obj">Member to compute a hash code for.</param>
        /// <returns>Hash code for the given member.</returns>
        protected virtual int GetHashCodeMethod(MethodInfo obj) => EqualityComparer<MethodInfo>.Default.GetHashCode(obj);

        /// <summary>
        /// Gets a hash code for the given type member.
        /// </summary>
        /// <param name="obj">Member to compute a hash code for.</param>
        /// <returns>Hash code for the given member.</returns>
        protected virtual int GetHashCodeProperty(PropertyInfo obj) => EqualityComparer<PropertyInfo>.Default.GetHashCode(obj);

        /// <summary>
        /// Gets a hash code for the given type member.
        /// </summary>
        /// <param name="obj">Member to compute a hash code for.</param>
        /// <returns>Hash code for the given member.</returns>
        protected virtual int GetHashCodeNestedType(Type obj) => EqualityComparer<Type>.Default.GetHashCode(obj);

        /// <summary>
        /// Gets a hash code for the given type member.
        /// </summary>
        /// <param name="obj">Member to compute a hash code for.</param>
        /// <returns>Hash code for the given member.</returns>
        protected virtual int GetHashCodeType(TypeInfo obj) => EqualityComparer<TypeInfo>.Default.GetHashCode(obj);

        /// <summary>
        /// Gets a hash code for the given type member.
        /// </summary>
        /// <param name="obj">Member to compute a hash code for.</param>
        /// <returns>Hash code for the given member.</returns>
        protected virtual int GetHashCodeCustom(MemberInfo obj) => throw new NotImplementedException("Hash code of custom member types to be supplied by derived types.");

        /// <summary>
        /// Gets a hash code for the given type member.
        /// </summary>
        /// <param name="obj">Member to compute a hash code for.</param>
        /// <returns>Hash code for the given member.</returns>
        protected virtual int GetHashCodeExtension(MemberInfo obj) => throw new NotSupportedException("Member type is not supported for this comparer.");

        /// <summary>
        /// Resolves the member to compare against on the target type.
        /// </summary>
        /// <param name="targetType">The type to find a comparable member on.</param>
        /// <param name="member">The member to lookup on the target type.</param>
        /// <returns>The comparable member on the target type.</returns>
        public MemberInfo ResolveMember(Type targetType, MemberInfo member)
        {
            if (targetType == null)
            {
                throw new ArgumentNullException(nameof(targetType));
            }

            if (member == null)
            {
                throw new ArgumentNullException(nameof(member));
            }

            return member.MemberType switch
            {
                MemberTypes.Constructor => ResolveConstructor(targetType, (ConstructorInfo)member),
                MemberTypes.Custom => ResolveCustom(targetType, member),
                MemberTypes.Event => ResolveEvent(targetType, (EventInfo)member),
                MemberTypes.Field => ResolveField(targetType, (FieldInfo)member),
                MemberTypes.Method => ResolveMethod(targetType, (MethodInfo)member),
                MemberTypes.NestedType => ResolveNestedType(targetType, (Type)member),
                MemberTypes.Property => ResolveProperty(targetType, (PropertyInfo)member),
                MemberTypes.TypeInfo => ResolveTypeInfo(targetType, (TypeInfo)member),
                _ => ResolveExtension(targetType, member),
            };
        }

        /// <summary>
        /// Attempts to resolve a constructor on the target type.
        /// </summary>
        /// <param name="targetType">The target type.</param>
        /// <param name="constructor">The constructor.</param>
        /// <returns>The resolved constructor if found, null otherwise.</returns>
        protected virtual ConstructorInfo ResolveConstructor(Type targetType, ConstructorInfo constructor) => targetType.GetConstructors().SingleOrDefault(c => EqualsConstructor(constructor, c));

        /// <summary>
        /// Attempts to resolve a custom member on the target type.
        /// </summary>
        /// <param name="targetType">The target type.</param>
        /// <param name="member">The member.</param>
        /// <returns>The resolved member if found, null otherwise.</returns>
        protected virtual MemberInfo ResolveCustom(Type targetType, MemberInfo member) => throw new NotImplementedException("Resolution of custom members should be handled by derived classes.");

        /// <summary>
        /// Attempts to resolve an event on the target type.
        /// </summary>
        /// <param name="targetType">The target type.</param>
        /// <param name="eventInfo">The event.</param>
        /// <returns>The resolved event if found, null otherwise.</returns>
        protected virtual EventInfo ResolveEvent(Type targetType, EventInfo eventInfo) => targetType.GetEvents().SingleOrDefault(e => EqualsEvent(eventInfo, e));

        /// <summary>
        /// Attempts to resolve a field on the target type.
        /// </summary>
        /// <param name="targetType">The target type.</param>
        /// <param name="field">The field.</param>
        /// <returns>The resolved field if found, null otherwise.</returns>
        protected virtual FieldInfo ResolveField(Type targetType, FieldInfo field) => targetType.GetFields().SingleOrDefault(f => EqualsField(field, f));

        /// <summary>
        /// Attempts to resolve a method on the target type.
        /// </summary>
        /// <param name="targetType">The target type.</param>
        /// <param name="method">The method.</param>
        /// <returns>The resolved method if found, null otherwise.</returns>
        protected virtual MethodInfo ResolveMethod(Type targetType, MethodInfo method) => targetType.GetMethods().SingleOrDefault(m => EqualsMethod(method, m));

        /// <summary>
        /// Attempts to resolve a nested type on the target type.
        /// </summary>
        /// <param name="targetType">The target type.</param>
        /// <param name="nestedType">The nested type.</param>
        /// <returns>The resolved nested type if found, null otherwise.</returns>
        protected virtual Type ResolveNestedType(Type targetType, Type nestedType) => targetType.GetNestedTypes().SingleOrDefault(t => EqualsNestedType(nestedType, t));

        /// <summary>
        /// Attempts to resolve a property on the target type.
        /// </summary>
        /// <param name="targetType">The target type.</param>
        /// <param name="propertyInfo">The property.</param>
        /// <returns>The resolved property if found, null otherwise.</returns>
        protected virtual PropertyInfo ResolveProperty(Type targetType, PropertyInfo propertyInfo) => targetType.GetProperties().SingleOrDefault(p => EqualsProperty(propertyInfo, p));

        /// <summary>
        /// Attempts to resolve type info on the target type.
        /// </summary>
        /// <param name="targetType">The target type.</param>
        /// <param name="typeInfo">The type info.</param>
        /// <returns>The resolved type info if found, null otherwise.</returns>
        protected virtual TypeInfo ResolveTypeInfo(Type targetType, TypeInfo typeInfo)
        {
            var targetTypeInfo = targetType.GetTypeInfo();
            return EqualsType(typeInfo, targetTypeInfo) ? targetTypeInfo : null;
        }

        /// <summary>
        /// Attempts to resolve an extension member on the target type.
        /// </summary>
        /// <param name="targetType">The target type.</param>
        /// <param name="member">The member.</param>
        /// <returns>The resolved extension member if found, null otherwise.</returns>
        protected virtual MemberInfo ResolveExtension(Type targetType, MemberInfo member) => throw new NotSupportedException("Resolution of extension members should be handled by derived classes.");
    }
}
