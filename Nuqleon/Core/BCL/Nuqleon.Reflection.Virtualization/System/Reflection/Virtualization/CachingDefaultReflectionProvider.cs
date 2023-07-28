// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2016 - Created this file.
//

using System.Collections.Generic;
using System.Memory;

namespace System.Reflection
{
    // CONSIDER: Would it make sense to have a caching extension method on a given IReflectionProvider?
    //
    //           The interface is massive to implement when having to put "forwarders" in place for all
    //           non-intercepted methods, so it makes sense to implement caching as a derived type for
    //           the default provider. However, it makes it less compositional with custom implementations
    //           of the provider interfaces. Maybe a more general-purpose cache "constructor" would make
    //           sense, i.e. given an interface TInterface and a set of MemberInfo objects for members
    //           whose results to cache, implement a wrapper TInterface that intercepts all the cached
    //           members and forwards all the remaining ones. I.e. something like this:
    //
    //              var memoizedInterfaceType = MemoizedTypeBuilder.Create<TInterface>()
    //                                                             .Memoize((i) => i.Foo())
    //                                                             .Memoize((i, xs) => i.Bar(xs), x => new SpecializedEqualityComparer(xs))
    //                                                             .Build();

    /// <summary>
    /// Reflection provider which caches the results of pure reflection functions.
    /// </summary>
    public class CachingDefaultReflectionProvider : DefaultReflectionProvider, IClearable
    {
        private readonly IMemoizedDelegate<Func<MakeGenericTypeParams, Type>> _makeGenericType;
        private readonly IMemoizedDelegate<Func<MakeGenericMethodParams, MethodInfo>> _makeGenericMethod;

        private readonly IMemoizedDelegate<Func<Assembly, IEnumerable<TypeInfo>>> _assemblyGetDefinedTypes;
        private readonly IMemoizedDelegate<Func<Assembly, Type[]>> _assemblyGetExportedTypes;
        private readonly IMemoizedDelegate<Func<Assembly, bool, Module[]>> _assemblyGetModules;
        private readonly IMemoizedDelegate<Func<Assembly, AssemblyName[]>> _assemblyGetReferencedAssemblies;
        private readonly IMemoizedDelegate<Func<Assembly, Type[]>> _assemblyGetTypes;

        private readonly IMemoizedDelegate<Func<Module, BindingFlags, MethodInfo[]>> _moduleGetMethods;
        private readonly IMemoizedDelegate<Func<Module, BindingFlags, FieldInfo[]>> _moduleGetFields;
        private readonly IMemoizedDelegate<Func<Module, Type[]>> _moduleGetTypes;

        private readonly IMemoizedDelegate<Func<Type, Type[]>> _typeGetGenericArguments;
        private readonly IMemoizedDelegate<Func<Type, BindingFlags, MethodInfo[]>> _typeGetMethods;
        private readonly IMemoizedDelegate<Func<Type, BindingFlags, MemberInfo[]>> _typeGetMembers;
        private readonly IMemoizedDelegate<Func<Type, BindingFlags, FieldInfo[]>> _typeGetFields;
        private readonly IMemoizedDelegate<Func<Type, BindingFlags, PropertyInfo[]>> _typeGetProperties;
        private readonly IMemoizedDelegate<Func<Type, BindingFlags, EventInfo[]>> _typeGetEvents;
        private readonly IMemoizedDelegate<Func<Type, BindingFlags, Type[]>> _typeGetNestedTypes;
        private readonly IMemoizedDelegate<Func<Type, BindingFlags, ConstructorInfo[]>> _typeGetConstructors;
        private readonly IMemoizedDelegate<Func<Type, Type[]>> _typeGetInterfaces;
        private readonly IMemoizedDelegate<Func<Type, MemberInfo[]>> _typeGetDefaultMembers;
        private readonly IMemoizedDelegate<Func<Type, Type[]>> _typeGetGenericParameterConstraints;

        private readonly IMemoizedDelegate<Func<FieldInfo, Type[]>> _fieldGetOptionalCustomModifiers;
        private readonly IMemoizedDelegate<Func<FieldInfo, Type[]>> _fieldGetRequiredCustomModifiers;

        private readonly IMemoizedDelegate<Func<MethodInfo, Type[]>> _methodGetGenericArguments;
        private readonly IMemoizedDelegate<Func<MethodBase, ParameterInfo[]>> _methodGetParameters;

        private readonly IMemoizedDelegate<Func<PropertyInfo, Type[]>> _propertyGetOptionalCustomModifiers;
        private readonly IMemoizedDelegate<Func<PropertyInfo, Type[]>> _propertyGetRequiredCustomModifiers;
        private readonly IMemoizedDelegate<Func<PropertyInfo, ParameterInfo[]>> _propertyGetIndexParameters;
        private readonly IMemoizedDelegate<Func<PropertyInfo, bool, MethodInfo[]>> _propertyGetAccessors;

        private readonly IMemoizedDelegate<Func<EventInfo, bool, MethodInfo[]>> _eventGetOtherMethods;

        private readonly IMemoizedDelegate<Func<ParameterInfo, Type[]>> _parameterGetOptionalCustomModifiers;
        private readonly IMemoizedDelegate<Func<ParameterInfo, Type[]>> _parameterGetRequiredCustomModifiers;

        private readonly IMemoizedDelegate<Func<Assembly, bool, object[]>> _assemblyGetCustomAttributes;
        private readonly IMemoizedDelegate<Func<Assembly, Type, bool, object[]>> _assemblyGetCustomAttributesByType;
        private readonly IMemoizedDelegate<Func<Assembly, IList<CustomAttributeData>>> _assemblyGetCustomAttributesData;
        private readonly IMemoizedDelegate<Func<Assembly, Type, bool, bool>> _assemblyIsDefined;

        private readonly IMemoizedDelegate<Func<Module, bool, object[]>> _moduleGetCustomAttributes;
        private readonly IMemoizedDelegate<Func<Module, Type, bool, object[]>> _moduleGetCustomAttributesByType;
        private readonly IMemoizedDelegate<Func<Module, IList<CustomAttributeData>>> _moduleGetCustomAttributesData;
        private readonly IMemoizedDelegate<Func<Module, Type, bool, bool>> _moduleIsDefined;

        private readonly IMemoizedDelegate<Func<MemberInfo, bool, object[]>> _memberGetCustomAttributes;
        private readonly IMemoizedDelegate<Func<MemberInfo, Type, bool, object[]>> _memberGetCustomAttributesByType;
        private readonly IMemoizedDelegate<Func<MemberInfo, IList<CustomAttributeData>>> _memberGetCustomAttributesData;
        private readonly IMemoizedDelegate<Func<MemberInfo, Type, bool, bool>> _memberIsDefined;

        private readonly IMemoizedDelegate<Func<ParameterInfo, bool, object[]>> _parameterGetCustomAttributes;
        private readonly IMemoizedDelegate<Func<ParameterInfo, Type, bool, object[]>> _parameterGetCustomAttributesByType;
        private readonly IMemoizedDelegate<Func<ParameterInfo, IList<CustomAttributeData>>> _parameterGetCustomAttributesData;
        private readonly IMemoizedDelegate<Func<ParameterInfo, Type, bool, bool>> _parameterIsDefined;

        private readonly IMemoizedDelegate<Func<MethodInfo, ICustomAttributeProvider>> _methodReturnTypeCustomAttributes;

        private readonly IMemoizedDelegate<Func<Module, TypeFilter, object, Type[]>> _moduleFindTypes;
        private readonly IMemoizedDelegate<Func<Type, TypeFilter, object, Type[]>> _typeFindInterfaces;
        private readonly IMemoizedDelegate<Func<Type, MemberTypes, BindingFlags, MemberFilter, object, MemberInfo[]>> _typeFindMembers;

        /// <summary>
        /// Creates a new caching reflection provider using the specified <paramref name="memoizer"/> to create caches.
        /// All reflection methods that are deemed expensive and return objects that can be cached will be subject to caching.
        /// </summary>
        /// <param name="memoizer">The memoizer to use for cache creation.</param>
        public CachingDefaultReflectionProvider(IMemoizer memoizer)
            : this(memoizer, ReflectionCachingOptions.All)
        {
        }

        /// <summary>
        /// Creates a new caching reflection provider using the specified <paramref name="memoizer"/> to create caches and using the specified caching <paramref name="options"/>.
        /// </summary>
        /// <param name="memoizer">The memoizer to use for cache creation.</param>
        /// <param name="options">Options to control which reflection methods get cached.</param>
        public CachingDefaultReflectionProvider(IMemoizer memoizer, ReflectionCachingOptions options)
        {
            if (memoizer == null)
                throw new ArgumentNullException(nameof(memoizer));

            if ((options & ReflectionCachingOptions.CreationGenericObjects) != 0)
            {
                _makeGenericType = memoizer.Memoize<MakeGenericTypeParams, Type>(MakeGenericTypeParams.Invoke);
                _makeGenericMethod = memoizer.Memoize<MakeGenericMethodParams, MethodInfo>(MakeGenericMethodParams.Invoke);
            }

            if ((options & ReflectionCachingOptions.IntrospectionGetMethods) != 0)
            {
                _assemblyGetDefinedTypes = memoizer.Memoize<Assembly, IEnumerable<TypeInfo>>(a => a.DefinedTypes);
                _assemblyGetExportedTypes = memoizer.Memoize<Assembly, Type[]>(a => a.GetExportedTypes());
                _assemblyGetModules = memoizer.Memoize<Assembly, bool, Module[]>((a, r) => a.GetModules(r));
                _assemblyGetReferencedAssemblies = memoizer.Memoize<Assembly, AssemblyName[]>(a => a.GetReferencedAssemblies());
                _assemblyGetTypes = memoizer.Memoize<Assembly, Type[]>(a => a.GetTypes());

                _moduleGetMethods = memoizer.Memoize<Module, BindingFlags, MethodInfo[]>((m, b) => m.GetMethods(b));
                _moduleGetFields = memoizer.Memoize<Module, BindingFlags, FieldInfo[]>((m, b) => m.GetFields(b));
                _moduleGetTypes = memoizer.Memoize<Module, Type[]>(m => m.GetTypes());

                _typeGetGenericArguments = memoizer.Memoize<Type, Type[]>(m => m.GetGenericArguments());
                _typeGetMethods = memoizer.Memoize<Type, BindingFlags, MethodInfo[]>((t, b) => t.GetMethods(b));
                _typeGetMembers = memoizer.Memoize<Type, BindingFlags, MemberInfo[]>((t, b) => t.GetMembers(b));
                _typeGetFields = memoizer.Memoize<Type, BindingFlags, FieldInfo[]>((t, b) => t.GetFields(b));
                _typeGetProperties = memoizer.Memoize<Type, BindingFlags, PropertyInfo[]>((t, b) => t.GetProperties(b));
                _typeGetEvents = memoizer.Memoize<Type, BindingFlags, EventInfo[]>((t, b) => t.GetEvents(b));
                _typeGetNestedTypes = memoizer.Memoize<Type, BindingFlags, Type[]>((t, b) => t.GetNestedTypes(b));
                _typeGetConstructors = memoizer.Memoize<Type, BindingFlags, ConstructorInfo[]>((t, b) => t.GetConstructors(b));
                _typeGetInterfaces = memoizer.Memoize<Type, Type[]>(t => t.GetInterfaces());
                _typeGetDefaultMembers = memoizer.Memoize<Type, MemberInfo[]>(t => t.GetDefaultMembers());
                _typeGetGenericParameterConstraints = memoizer.Memoize<Type, Type[]>(t => t.GetGenericParameterConstraints());

                _fieldGetOptionalCustomModifiers = memoizer.Memoize<FieldInfo, Type[]>(f => f.GetOptionalCustomModifiers());
                _fieldGetRequiredCustomModifiers = memoizer.Memoize<FieldInfo, Type[]>(f => f.GetRequiredCustomModifiers());

                _methodGetGenericArguments = memoizer.Memoize<MethodInfo, Type[]>(m => m.GetGenericArguments());
                _methodGetParameters = memoizer.Memoize<MethodBase, ParameterInfo[]>(m => m.GetParameters());

                _propertyGetIndexParameters = memoizer.Memoize<PropertyInfo, ParameterInfo[]>(p => p.GetIndexParameters());
                _propertyGetOptionalCustomModifiers = memoizer.Memoize<PropertyInfo, Type[]>(p => p.GetOptionalCustomModifiers());
                _propertyGetRequiredCustomModifiers = memoizer.Memoize<PropertyInfo, Type[]>(p => p.GetRequiredCustomModifiers());
                _propertyGetAccessors = memoizer.Memoize<PropertyInfo, bool, MethodInfo[]>((p, n) => p.GetAccessors(n));

                _eventGetOtherMethods = memoizer.Memoize<EventInfo, bool, MethodInfo[]>((e, n) => e.GetOtherMethods(n));

                _parameterGetOptionalCustomModifiers = memoizer.Memoize<ParameterInfo, Type[]>(p => p.GetOptionalCustomModifiers());
                _parameterGetRequiredCustomModifiers = memoizer.Memoize<ParameterInfo, Type[]>(p => p.GetRequiredCustomModifiers());
            }

            if ((options & ReflectionCachingOptions.IntrospectionCustomAttributes) != 0)
            {
                _assemblyGetCustomAttributes = memoizer.Memoize<Assembly, bool, object[]>((a, i) => a.GetCustomAttributes(i));
                _assemblyGetCustomAttributesByType = memoizer.Memoize<Assembly, Type, bool, object[]>((a, t, i) => a.GetCustomAttributes(t, i));
                _assemblyGetCustomAttributesData = memoizer.Memoize<Assembly, IList<CustomAttributeData>>(a => a.GetCustomAttributesData());
                _assemblyIsDefined = memoizer.Memoize<Assembly, Type, bool, bool>((a, t, i) => a.IsDefined(t, i));

                _moduleGetCustomAttributes = memoizer.Memoize<Module, bool, object[]>((m, i) => m.GetCustomAttributes(i));
                _moduleGetCustomAttributesByType = memoizer.Memoize<Module, Type, bool, object[]>((m, t, i) => m.GetCustomAttributes(t, i));
                _moduleGetCustomAttributesData = memoizer.Memoize<Module, IList<CustomAttributeData>>(m => m.GetCustomAttributesData());
                _moduleIsDefined = memoizer.Memoize<Module, Type, bool, bool>((m, t, i) => m.IsDefined(t, i));

                _memberGetCustomAttributes = memoizer.Memoize<MemberInfo, bool, object[]>((m, i) => m.GetCustomAttributes(i));
                _memberGetCustomAttributesByType = memoizer.Memoize<MemberInfo, Type, bool, object[]>((m, t, i) => m.GetCustomAttributes(t, i));
                _memberGetCustomAttributesData = memoizer.Memoize<MemberInfo, IList<CustomAttributeData>>(m => m.GetCustomAttributesData());
                _memberIsDefined = memoizer.Memoize<MemberInfo, Type, bool, bool>((m, t, i) => m.IsDefined(t, i));

                _parameterGetCustomAttributes = memoizer.Memoize<ParameterInfo, bool, object[]>((p, i) => p.GetCustomAttributes(i));
                _parameterGetCustomAttributesByType = memoizer.Memoize<ParameterInfo, Type, bool, object[]>((p, t, i) => p.GetCustomAttributes(t, i));
                _parameterGetCustomAttributesData = memoizer.Memoize<ParameterInfo, IList<CustomAttributeData>>(p => p.GetCustomAttributesData());
                _parameterIsDefined = memoizer.Memoize<ParameterInfo, Type, bool, bool>((p, t, i) => p.IsDefined(t, i));

                _methodReturnTypeCustomAttributes = memoizer.Memoize<MethodInfo, ICustomAttributeProvider>(m => new CachingCustomAttributeProvider(memoizer, m.ReturnTypeCustomAttributes));
            }

            if ((options & ReflectionCachingOptions.IntrospectionFindMethods) != 0)
            {
                _moduleFindTypes = memoizer.Memoize<Module, TypeFilter, object, Type[]>((m, f, o) => m.FindTypes(f, o));
                _typeFindInterfaces = memoizer.Memoize<Type, TypeFilter, object, Type[]>((t, f, o) => t.FindInterfaces(f, o));
                _typeFindMembers = memoizer.Memoize<Type, MemberTypes, BindingFlags, MemberFilter, object, MemberInfo[]>((t, m, b, f, o) => t.FindMembers(m, b, f, o));
            }
        }

        /// <summary>
        /// Makes a generic type with the specified generic type definition and type arguments.
        /// </summary>
        /// <param name="genericTypeDefinition">The generic type definition.</param>
        /// <param name="typeArguments">The type arguments.</param>
        /// <returns>A generic type with the specified generic type definition and type arguments.</returns>
        public override Type MakeGenericType(Type genericTypeDefinition, params Type[] typeArguments) => _makeGenericMethod != null ? _makeGenericType.Delegate(new MakeGenericTypeParams(genericTypeDefinition, typeArguments)) : base.MakeGenericType(genericTypeDefinition, typeArguments);

        /// <summary>
        /// Makes a generic method with the specified generic method definition and type arguments.
        /// </summary>
        /// <param name="genericMethodDefinition">The generic method definition.</param>
        /// <param name="typeArguments">The type arguments.</param>
        /// <returns>A generic method with the specified generic method definition and type arguments.</returns>
        public override MethodInfo MakeGenericMethod(MethodInfo genericMethodDefinition, params Type[] typeArguments) => _makeGenericMethod != null ? _makeGenericMethod.Delegate(new MakeGenericMethodParams(genericMethodDefinition, typeArguments)) : base.MakeGenericMethod(genericMethodDefinition, typeArguments);

        /// <summary>
        /// Gets the types defined in the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly to search in.</param>
        /// <returns>An array of types defined in the specified <paramref name="assembly"/>.</returns>
        public override IEnumerable<TypeInfo> GetDefinedTypes(Assembly assembly) => _assemblyGetDefinedTypes != null ? _assemblyGetDefinedTypes.Delegate(assembly) : base.GetDefinedTypes(assembly);

        /// <summary>
        /// Gets the public types defined in the specified <paramref name="assembly"/> that are visible outside the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly to get exported types from.</param>
        /// <returns>An array that represents the types defined in this assembly that are visible outside the assembly.</returns>
        public override IReadOnlyList<Type> GetExportedTypes(Assembly assembly) => _assemblyGetExportedTypes != null ? _assemblyGetExportedTypes.Delegate(assembly) : base.GetExportedTypes(assembly);

        /// <summary>
        /// Gets all the modules that are part of the specified <paramref name="assembly"/>, specifying whether to include resource modules.
        /// </summary>
        /// <param name="assembly">The assembly to get the modules for.</param>
        /// <param name="getResourceModules">true to include resource modules; otherwise, false.</param>
        /// <returns>An array of modules.</returns>
        public override IReadOnlyList<Module> GetModules(Assembly assembly, bool getResourceModules) => _assemblyGetModules != null ? _assemblyGetModules.Delegate(assembly, getResourceModules) : base.GetModules(assembly, getResourceModules);

        /// <summary>
        /// Gets the <see cref="AssemblyName" /> objects for all the assemblies referenced by the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly to get the referenced assemblies for.</param>
        /// <returns>An array that contains the fully parsed display names of all the assemblies referenced by the specified <paramref name="assembly"/>.</returns>
        public override IReadOnlyList<AssemblyName> GetReferencedAssemblies(Assembly assembly) => _assemblyGetReferencedAssemblies != null ? _assemblyGetReferencedAssemblies.Delegate(assembly) : base.GetReferencedAssemblies(assembly);

        /// <summary>
        /// Gets the types defined in the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly to retrieve the types from.</param>
        /// <returns>An array that contains all the types that are defined in this assembly.</returns>
        public override IReadOnlyList<Type> GetTypes(Assembly assembly) => _assemblyGetTypes != null ? _assemblyGetTypes.Delegate(assembly) : base.GetTypes(assembly);

        /// <summary>
        /// Returns the global methods defined on the module that match the specified binding flags.
        /// </summary>
        /// <param name="module">The module to search in.</param>
        /// <param name="bindingFlags">A bitwise combination of <see cref="BindingFlags" /> values that limit the search.</param>
        /// <returns>An array of type <see cref="MethodInfo" /> representing the global methods defined on the module that match the specified binding flags; if no global methods match the binding flags, an empty array is returned.</returns>
        public override IReadOnlyList<MethodInfo> GetMethods(Module module, BindingFlags bindingFlags) => _moduleGetMethods != null ? _moduleGetMethods.Delegate(module, bindingFlags) : base.GetMethods(module, bindingFlags);

        /// <summary>
        /// Returns the global fields defined on the module that match the specified binding flags.
        /// </summary>
        /// <param name="module">The module to search in.</param>
        /// <param name="bindingFlags">A bitwise combination of <see cref="BindingFlags" /> values that limit the search.</param>
        /// <returns>An array of type <see cref="FieldInfo" /> representing the global fields defined on the module that match the specified binding flags; if no global fields match the binding flags, an empty array is returned.</returns>
        public override IReadOnlyList<FieldInfo> GetFields(Module module, BindingFlags bindingFlags) => _moduleGetFields != null ? _moduleGetFields.Delegate(module, bindingFlags) : base.GetFields(module, bindingFlags);

        /// <summary>
        /// Returns all the types defined within the specified <paramref name="module"/>.
        /// </summary>
        /// <param name="module">The module to search in.</param>
        /// <returns>An array of type <see cref="Type"/> containing types defined within the module that is reflected by this instance.</returns>
        public override IReadOnlyList<Type> GetTypes(Module module) => _moduleGetTypes != null ? _moduleGetTypes.Delegate(module) : base.GetTypes(module);

        /// <summary>
        /// Returns an array of <see cref="Type"/> objects that represent the type arguments of a closed generic type or the type parameters of a generic type definition.
        /// </summary>
        /// <param name="type">The type to get the generic arguments for.</param>
        /// <returns>An array of <see cref="Type"/> objects that represent the type arguments of a generic type. Returns an empty array if the specified type is not a generic type.</returns>
        public override IReadOnlyList<Type> GetGenericArguments(Type type) => _typeGetGenericArguments != null ? _typeGetGenericArguments.Delegate(type) : base.GetGenericArguments(type);

        /// <summary>
        /// Searches for members defined for the specified type, using the specified binding constraints.
        /// </summary>
        /// <param name="type">The type to get the members for.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags" /> that specify how the search is conducted.</param>
        /// <returns>An array of <see cref="MemberInfo" /> objects representing all members defined for the specified type that match the specified binding constraints.-or- An empty array of type <see cref="MemberInfo" />, if no members are defined for the specified type, or if none of the defined members match the binding constraints.</returns>
        public override IReadOnlyList<MemberInfo> GetMembers(Type type, BindingFlags bindingAttr) => _typeGetMembers != null ? _typeGetMembers.Delegate(type, bindingAttr) : base.GetMembers(type, bindingAttr);

        /// <summary>
        /// Searches for methods defined for the specified type, using the specified binding constraints.
        /// </summary>
        /// <param name="type">The type to get the methods for.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags" /> that specify how the search is conducted.</param>
        /// <returns>An array of <see cref="MethodInfo" /> objects representing all methods defined for the specified type that match the specified binding constraints.-or- An empty array of type <see cref="MethodInfo" />, if no methods are defined for the specified type, or if none of the defined methods match the binding constraints.</returns>
        public override IReadOnlyList<MethodInfo> GetMethods(Type type, BindingFlags bindingAttr) => _typeGetMethods != null ? _typeGetMethods.Delegate(type, bindingAttr) : base.GetMethods(type, bindingAttr);

        /// <summary>
        /// Searches for fields defined for the specified type, using the specified binding constraints.
        /// </summary>
        /// <param name="type">The type to get the fields for.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags" /> that specify how the search is conducted.</param>
        /// <returns>An array of <see cref="FieldInfo" /> objects representing all fields defined for the specified type that match the specified binding constraints.-or- An empty array of type <see cref="FieldInfo" />, if no fields are defined for the specified type, or if none of the defined fields match the binding constraints.</returns>
        public override IReadOnlyList<FieldInfo> GetFields(Type type, BindingFlags bindingAttr) => _typeGetFields != null ? _typeGetFields.Delegate(type, bindingAttr) : base.GetFields(type, bindingAttr);

        /// <summary>
        /// Searches for properties of for the specified type, using the specified binding constraints.
        /// </summary>
        /// <param name="type">The type to get the properties for.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags" /> that specify how the search is conducted.</param>
        /// <returns>An array of <see cref="PropertyInfo" /> objects representing all properties defined for the specified type that match the specified binding constraints.-or- An empty array of type <see cref="PropertyInfo" />, if no properties are defined for the specified type, or if none of the defined properties match the binding constraints.</returns>
        public override IReadOnlyList<PropertyInfo> GetProperties(Type type, BindingFlags bindingAttr) => _typeGetProperties != null ? _typeGetProperties.Delegate(type, bindingAttr) : base.GetProperties(type, bindingAttr);

        /// <summary>
        /// Searches for events that are declared or inherited by the specified type, using the specified binding constraints.
        /// </summary>
        /// <param name="type">The type to get the events for.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags" /> that specify how the search is conducted.</param>
        /// <returns>An array of <see cref="EventInfo" /> objects representing all events defined for the specified type that match the specified binding constraints.-or- An empty array of type <see cref="EventInfo" />, if no events are defined for the specified type, or if none of the defined events match the binding constraints.</returns>
        public override IReadOnlyList<EventInfo> GetEvents(Type type, BindingFlags bindingAttr) => _typeGetEvents != null ? _typeGetEvents.Delegate(type, bindingAttr) : base.GetEvents(type, bindingAttr);

        /// <summary>
        /// Searches for types nested in the specified type, using the specified binding constraints.
        /// </summary>
        /// <param name="type">The type to get the nested types for.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags" /> that specify how the search is conducted.</param>
        /// <returns>An array of <see cref="Type"/> objects representing all nested types defined for the specified type that match the specified binding constraints.-or- An empty array of type <see cref="Type"/>, if no nested types are defined for the specified type, or if none of the nested types methods match the binding constraints.</returns>
        public override IReadOnlyList<Type> GetNestedTypes(Type type, BindingFlags bindingAttr) => _typeGetNestedTypes != null ? _typeGetNestedTypes.Delegate(type, bindingAttr) : base.GetNestedTypes(type, bindingAttr);

        /// <summary>
        /// Searches for the constructors defined for the specified type, using the specified binding constraints.
        /// </summary>
        /// <param name="type">The type to get the constructors for.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags" /> that specify how the search is conducted.</param>
        /// <returns>An array of <see cref="ConstructorInfo" /> objects representing all constructors defined for the specified type that match the specified binding constraints.-or- An empty array of type <see cref="ConstructorInfo" />, if no constructors are defined for the specified type, or if none of the defined constructors match the binding constraints.</returns>
        public override IReadOnlyList<ConstructorInfo> GetConstructors(Type type, BindingFlags bindingAttr) => _typeGetConstructors != null ? _typeGetConstructors.Delegate(type, bindingAttr) : base.GetConstructors(type, bindingAttr);

        /// <summary>
        /// Gets all the interfaces implemented or inherited by the specified type.
        /// </summary>
        /// <param name="type">The type to get the interfaces for.</param>
        /// <returns>An array of <see cref="Type"/> objects representing all the interfaces implemented or inherited by the specified type.-or- An empty array of type <see cref="Type"/>, if no interfaces are implemented or inherited by the specified type.</returns>
        public override IReadOnlyList<Type> GetInterfaces(Type type) => _typeGetInterfaces != null ? _typeGetInterfaces.Delegate(type) : base.GetInterfaces(type);

        /// <summary>
        /// Searches for the members defined for the specified <paramref name="type"/> whose <see cref="DefaultMemberAttribute" /> is set.
        /// </summary>
        /// <param name="type">The type to get the default members for.</param>
        /// <returns>An array of <see cref="MemberInfo" /> objects representing all default members of the specified <paramref name="type"/>.-or- An empty array of type <see cref="MemberInfo" />, if the <paramref name="type"/> does not have default members.</returns>
        public override IReadOnlyList<MemberInfo> GetDefaultMembers(Type type) => _typeGetDefaultMembers != null ? _typeGetDefaultMembers.Delegate(type) : base.GetDefaultMembers(type);

        /// <summary>
        /// Returns an array of <see cref="Type"/> objects that represent the constraints on the generic type parameter.
        /// </summary>
        /// <param name="type">The generic parameter type to get the constraints for.</param>
        /// <returns>An array of <see cref="Type"/> objects that represent the constraints on the specified generic type parameter.</returns>
        public override IReadOnlyList<Type> GetGenericParameterConstraints(Type type) => _typeGetGenericParameterConstraints != null ? _typeGetGenericParameterConstraints.Delegate(type) : base.GetGenericParameterConstraints(type);

        /// <summary>
        /// Returns an array of types representing the optional custom modifiers of the field.
        /// </summary>
        /// <param name="field">The field to get the modifiers for.</param>
        /// <returns>An array of <see cref="Type" /> objects that identify the optional custom modifiers of the specified <paramref name="field" />, such as <see cref="System.Runtime.CompilerServices.IsConst" /> or <see cref="System.Runtime.CompilerServices.IsImplicitlyDereferenced" />.</returns>
        public override IReadOnlyList<Type> GetOptionalCustomModifiers(FieldInfo field) => _fieldGetOptionalCustomModifiers != null ? _fieldGetOptionalCustomModifiers.Delegate(field) : base.GetOptionalCustomModifiers(field);

        /// <summary>
        /// Returns an array of types representing the required custom modifiers of the field.
        /// </summary>
        /// <param name="field">The field to get the modifiers for.</param>
        /// <returns>An array of <see cref="Type" /> objects that identify the required custom modifiers of the specified <paramref name="field" />, such as <see cref="System.Runtime.CompilerServices.IsConst" /> or <see cref="System.Runtime.CompilerServices.IsImplicitlyDereferenced" />.</returns>
        public override IReadOnlyList<Type> GetRequiredCustomModifiers(FieldInfo field) => _fieldGetRequiredCustomModifiers != null ? _fieldGetRequiredCustomModifiers.Delegate(field) : base.GetRequiredCustomModifiers(field);

        /// <summary>
        /// Returns an array of <see cref="Type"/> objects that represent the type arguments of a closed generic method or the type parameters of a generic method definition.
        /// </summary>
        /// <param name="method">The method to get the generic arguments for.</param>
        /// <returns>An array of <see cref="Type"/> objects that represent the type arguments of a generic method. Returns an empty array if the specified method is not a generic method.</returns>
        public override IReadOnlyList<Type> GetGenericArguments(MethodInfo method) => _methodGetGenericArguments != null ? _methodGetGenericArguments.Delegate(method) : base.GetGenericArguments(method);

        /// <summary>
        /// Gets the parameters of the specified method or constructor.
        /// </summary>
        /// <param name="method">The method or constructor to get the parameters for.</param>
        /// <returns>An array of type <see cref="ParameterInfo"/> containing information that matches the signature of the method (or constructor) of the specified <see cref="MethodBase"/> instance.</returns>
        public override IReadOnlyList<ParameterInfo> GetParameters(MethodBase method) => _methodGetParameters != null ? _methodGetParameters.Delegate(method) : base.GetParameters(method);

        /// <summary>
        /// Returns an array of all the index parameters for the property.
        /// </summary>
        /// <param name="property">The property to get the index parameters for.</param>
        /// <returns>An array of type <see cref="ParameterInfo"/> containing the parameters for the indexes. If the property is not indexed, the array has 0 (zero) elements.</returns>
        public override IReadOnlyList<ParameterInfo> GetIndexParameters(PropertyInfo property) => _propertyGetIndexParameters != null ? _propertyGetIndexParameters.Delegate(property) : base.GetIndexParameters(property);

        /// <summary>
        /// Returns an array of types representing the optional custom modifiers of the property.
        /// </summary>
        /// <param name="property">The property to get the modifiers for.</param>
        /// <returns>An array of <see cref="Type" /> objects that identify the optional custom modifiers of the <paramref name="property" />, such as <see cref="System.Runtime.CompilerServices.IsConst" /> or <see cref="System.Runtime.CompilerServices.IsImplicitlyDereferenced" />.</returns>
        public override IReadOnlyList<Type> GetOptionalCustomModifiers(PropertyInfo property) => _propertyGetOptionalCustomModifiers != null ? _propertyGetOptionalCustomModifiers.Delegate(property) : base.GetOptionalCustomModifiers(property);

        /// <summary>
        /// Returns an array of types representing the required custom modifiers of the property.
        /// </summary>
        /// <param name="property">The property to get the modifiers for.</param>
        /// <returns>An array of <see cref="Type" /> objects that identify the required custom modifiers of the <paramref name="property" />, such as <see cref="System.Runtime.CompilerServices.IsConst" /> or <see cref="System.Runtime.CompilerServices.IsImplicitlyDereferenced" />.</returns>
        public override IReadOnlyList<Type> GetRequiredCustomModifiers(PropertyInfo property) => _propertyGetRequiredCustomModifiers != null ? _propertyGetRequiredCustomModifiers.Delegate(property) : base.GetRequiredCustomModifiers(property);

        /// <summary>
        /// Returns an array whose elements reflect the public and, if specified, non-public get, set, and other accessors of the specified property.
        /// </summary>
        /// <param name="property">The property to get the accessors for.</param>
        /// <param name="nonPublic">true if non-public methods can be returned; otherwise, false.</param>
        /// <returns>An array of <see cref="MethodInfo" /> objects whose elements reflect the get, set, and other accessors of the specified property. If <paramref name="nonPublic" /> is true, this array contains public and non-public get, set, and other accessors. If <paramref name="nonPublic" /> is false, this array contains only public get, set, and other accessors. If no accessors with the specified visibility are found, this method returns an array with zero (0) elements.</returns>
        public override IReadOnlyList<MethodInfo> GetAccessors(PropertyInfo property, bool nonPublic) => _propertyGetAccessors != null ? _propertyGetAccessors.Delegate(property, nonPublic) : base.GetAccessors(property, nonPublic);

        /// <summary>
        /// Returns the public methods that have been associated with an event in metadata using the .other directive.
        /// </summary>
        /// <param name="event">The event to get the associated methods for.</param>
        /// <param name="nonPublic">true if non-public methods can be returned; otherwise, false.</param>
        /// <returns>An array of <see cref="MethodInfo" /> objects representing the public methods that have been associated with the event in metadata by using the .other directive. If there are no such public methods, an empty array is returned.</returns>
        public override IReadOnlyList<MethodInfo> GetOtherMethods(EventInfo @event, bool nonPublic) => _eventGetOtherMethods != null ? _eventGetOtherMethods.Delegate(@event, nonPublic) : base.GetOtherMethods(@event, nonPublic);

        /// <summary>
        /// Returns an array of types representing the optional custom modifiers of the parameter.
        /// </summary>
        /// <param name="parameter">The parameter to get the modifiers for.</param>
        /// <returns>An array of <see cref="Type" /> objects that identify the optional custom modifiers of the specified <paramref name="parameter" />, such as <see cref="System.Runtime.CompilerServices.IsConst" /> or <see cref="System.Runtime.CompilerServices.IsImplicitlyDereferenced" />.</returns>
        public override IReadOnlyList<Type> GetOptionalCustomModifiers(ParameterInfo parameter) => _parameterGetOptionalCustomModifiers != null ? _parameterGetOptionalCustomModifiers.Delegate(parameter) : base.GetOptionalCustomModifiers(parameter);

        /// <summary>
        /// Returns an array of types representing the required custom modifiers of the parameter.
        /// </summary>
        /// <param name="parameter">The parameter to get the modifiers for.</param>
        /// <returns>An array of <see cref="Type" /> objects that identify the required custom modifiers of the specified <paramref name="parameter" />, such as <see cref="System.Runtime.CompilerServices.IsConst" /> or <see cref="System.Runtime.CompilerServices.IsImplicitlyDereferenced" />.</returns>
        public override IReadOnlyList<Type> GetRequiredCustomModifiers(ParameterInfo parameter) => _parameterGetRequiredCustomModifiers != null ? _parameterGetRequiredCustomModifiers.Delegate(parameter) : base.GetRequiredCustomModifiers(parameter);

        /// <summary>
        /// Gets all the custom attributes for the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly to get custom attributes for.</param>
        /// <param name="inherit">This argument is ignored for objects of type <see cref="Assembly" />. </param>
        /// <returns>An array that contains the custom attributes for the specified <paramref name="assembly"/>.</returns>
        public override IReadOnlyList<object> GetCustomAttributes(Assembly assembly, bool inherit) => _assemblyGetCustomAttributes != null ? _assemblyGetCustomAttributes.Delegate(assembly, inherit) : base.GetCustomAttributes(assembly, inherit);

        /// <summary>
        /// Gets the custom attributes for the specified <paramref name="assembly"/> as specified by type.
        /// </summary>
        /// <param name="assembly">The assembly to get custom attributes for.</param>
        /// <param name="attributeType">The type for which the custom attributes are to be returned. </param>
        /// <param name="inherit">This argument is ignored for objects of type <see cref="Assembly" />.</param>
        /// <returns>An array that contains the custom attributes for the specified <paramref name="assembly"/> as specified by <paramref name="attributeType" />.</returns>
        public override IReadOnlyList<object> GetCustomAttributes(Assembly assembly, Type attributeType, bool inherit) => _assemblyGetCustomAttributesByType != null ? _assemblyGetCustomAttributesByType.Delegate(assembly, attributeType, inherit) : base.GetCustomAttributes(assembly, attributeType, inherit);

        /// <summary>
        /// Returns information about the attributes that have been applied to the specified <paramref name="assembly"/>, expressed as <see cref="CustomAttributeData" /> objects.
        /// </summary>
        /// <param name="assembly">The assembly to get custom attributes data for.</param>
        /// <returns>A generic list of <see cref="CustomAttributeData" /> objects representing data about the attributes that have been applied to the specified <paramref name="assembly"/>.</returns>
        public override IEnumerable<CustomAttributeData> GetCustomAttributesData(Assembly assembly) => _assemblyGetCustomAttributesData != null ? _assemblyGetCustomAttributesData.Delegate(assembly) : base.GetCustomAttributesData(assembly);

        /// <summary>
        /// Determines whether the custom attribute of the specified <paramref name="attributeType"/> type or its derived types is applied to the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly to check the custom attribute on.</param>
        /// <param name="attributeType">The type of attribute to search for. </param>
        /// <param name="inherit">This argument is ignored for objects of this type.</param>
        /// <returns>true if one or more instances of <paramref name="attributeType" /> or its derived types are applied to the specified <paramref name="assembly"/>; otherwise, false.</returns>
        public override bool IsDefined(Assembly assembly, Type attributeType, bool inherit) => _assemblyIsDefined != null ? _assemblyIsDefined.Delegate(assembly, attributeType, inherit) : base.IsDefined(assembly, attributeType, inherit);

        /// <summary>
        /// Gets all the custom attributes for the specified <paramref name="module"/>.
        /// </summary>
        /// <param name="module">The module to get custom attributes for.</param>
        /// <param name="inherit">This argument is ignored for objects of type <see cref="Module" />. </param>
        /// <returns>An array that contains the custom attributes for the specified <paramref name="module"/>.</returns>
        public override IReadOnlyList<object> GetCustomAttributes(Module module, bool inherit) => _moduleGetCustomAttributes != null ? _moduleGetCustomAttributes.Delegate(module, inherit) : base.GetCustomAttributes(module, inherit);

        /// <summary>
        /// Gets the custom attributes for the specified <paramref name="module"/> as specified by type.
        /// </summary>
        /// <param name="module">The module to get custom attributes for.</param>
        /// <param name="attributeType">The type for which the custom attributes are to be returned. </param>
        /// <param name="inherit">This argument is ignored for objects of type <see cref="Module" />.</param>
        /// <returns>An array that contains the custom attributes for the specified <paramref name="module"/> as specified by <paramref name="attributeType" />.</returns>
        public override IReadOnlyList<object> GetCustomAttributes(Module module, Type attributeType, bool inherit) => _moduleGetCustomAttributesByType != null ? _moduleGetCustomAttributesByType.Delegate(module, attributeType, inherit) : base.GetCustomAttributes(module, attributeType, inherit);

        /// <summary>
        /// Returns information about the attributes that have been applied to the specified <paramref name="module"/>, expressed as <see cref="CustomAttributeData" /> objects.
        /// </summary>
        /// <param name="module">The module to get custom attributes data for.</param>
        /// <returns>A generic list of <see cref="CustomAttributeData" /> objects representing data about the attributes that have been applied to the specified <paramref name="module"/>.</returns>
        public override IEnumerable<CustomAttributeData> GetCustomAttributesData(Module module) => _moduleGetCustomAttributesData != null ? _moduleGetCustomAttributesData.Delegate(module) : base.GetCustomAttributesData(module);

        /// <summary>
        /// Determines whether the custom attribute of the specified <paramref name="attributeType"/> type or its derived types is applied to the specified <paramref name="module"/>.
        /// </summary>
        /// <param name="module">The module to check the custom attribute on.</param>
        /// <param name="attributeType">The type of attribute to search for. </param>
        /// <param name="inherit">This argument is ignored for objects of this type.</param>
        /// <returns>true if one or more instances of <paramref name="attributeType" /> or its derived types are applied to the specified <paramref name="module"/>; otherwise, false.</returns>
        public override bool IsDefined(Module module, Type attributeType, bool inherit) => _moduleIsDefined != null ? _moduleIsDefined.Delegate(module, attributeType, inherit) : base.IsDefined(module, attributeType, inherit);

        /// <summary>
        /// Returns an array of all custom attributes applied to the specified member.
        /// </summary>
        /// <param name="member">The member to get the custom attributes for.</param>
        /// <param name="inherit">true to search the specified member's inheritance chain to find the attributes; otherwise, false. This parameter is ignored for properties and events.</param>
        /// <returns>An array that contains all the custom attributes applied to the specified member, or an array with zero elements if no attributes are defined.</returns>
        public override IReadOnlyList<object> GetCustomAttributes(MemberInfo member, bool inherit) => _memberGetCustomAttributes != null ? _memberGetCustomAttributes.Delegate(member, inherit) : base.GetCustomAttributes(member, inherit);

        /// <summary>
        /// Returns an array of custom attributes applied to the specified member and identified by the specified <paramref name="attributeType"/>.
        /// </summary>
        /// <param name="member">The member to get the custom attributes for.</param>
        /// <param name="attributeType">The type of attribute to search for. Only attributes that are assignable to this type are returned.</param>
        /// <param name="inherit">true to search the specified member's inheritance chain to find the attributes; otherwise, false. This parameter is ignored for properties and events.</param>
        /// <returns>An array of custom attributes applied to the specified member, or an array with zero elements if no attributes assignable to <paramref name="attributeType" /> have been applied.</returns>
        public override IReadOnlyList<object> GetCustomAttributes(MemberInfo member, Type attributeType, bool inherit) => _memberGetCustomAttributesByType != null ? _memberGetCustomAttributesByType.Delegate(member, attributeType, inherit) : base.GetCustomAttributes(member, attributeType, inherit);

        /// <summary>
        /// Gets the custom attributes data defined on the specified member.
        /// </summary>
        /// <param name="member">The member to get the custom attributes data for.</param>
        /// <returns>The custom attributes data for the specified member.</returns>
        public override IEnumerable<CustomAttributeData> GetCustomAttributesData(MemberInfo member) => _memberGetCustomAttributesData != null ? _memberGetCustomAttributesData.Delegate(member) : base.GetCustomAttributesData(member);

        /// <summary>
        /// Determines whether the custom attribute of the specified <paramref name="attributeType"/> type or its derived types is applied to the specified <paramref name="member"/>.
        /// </summary>
        /// <param name="member">The member to check the custom attribute on.</param>
        /// <param name="attributeType">The type of attribute to search for. </param>
        /// <param name="inherit">This argument is ignored for objects of this type.</param>
        /// <returns>true if one or more instances of <paramref name="attributeType" /> or its derived types are applied to the specified <paramref name="member"/>; otherwise, false.</returns>
        public override bool IsDefined(MemberInfo member, Type attributeType, bool inherit) => _memberIsDefined != null ? _memberIsDefined.Delegate(member, attributeType, inherit) : base.IsDefined(member, attributeType, inherit);

        /// <summary>
        /// Returns an array of all custom attributes applied to the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter to get the custom attributes for.</param>
        /// <param name="inherit">true to search the specified parameter's inheritance chain to find the attributes; otherwise, false.</param>
        /// <returns>An array that contains all the custom attributes applied to the specified parameter, or an array with zero elements if no attributes are defined.</returns>
        public override IReadOnlyList<object> GetCustomAttributes(ParameterInfo parameter, bool inherit) => _parameterGetCustomAttributes != null ? _parameterGetCustomAttributes.Delegate(parameter, inherit) : base.GetCustomAttributes(parameter, inherit);

        /// <summary>
        /// Returns an array of custom attributes applied to the specified parameter and identified by the specified <paramref name="attributeType"/>.
        /// </summary>
        /// <param name="parameter">The parameter to get the custom attributes for.</param>
        /// <param name="attributeType">The type of attribute to search for. Only attributes that are assignable to this type are returned.</param>
        /// <param name="inherit">true to search the specified parameter's inheritance chain to find the attributes; otherwise, false.</param>
        /// <returns>An array of custom attributes applied to the specified parameter, or an array with zero elements if no attributes assignable to <paramref name="attributeType" /> have been applied.</returns>
        public override IReadOnlyList<object> GetCustomAttributes(ParameterInfo parameter, Type attributeType, bool inherit) => _parameterGetCustomAttributesByType != null ? _parameterGetCustomAttributesByType.Delegate(parameter, attributeType, inherit) : base.GetCustomAttributes(parameter, attributeType, inherit);

        /// <summary>
        /// Gets the custom attributes data defined on the specified parameter.
        /// </summary>
        /// <param name="parameter">The member to get the custom attributes data for.</param>
        /// <returns>The custom attributes data for the specified parameter.</returns>
        public override IEnumerable<CustomAttributeData> GetCustomAttributesData(ParameterInfo parameter) => _parameterGetCustomAttributesData != null ? _parameterGetCustomAttributesData.Delegate(parameter) : base.GetCustomAttributesData(parameter);

        /// <summary>
        /// Determines whether the custom attribute of the specified <paramref name="attributeType"/> type or its derived types is applied to the specified <paramref name="parameter"/>.
        /// </summary>
        /// <param name="parameter">The parameter to check the custom attribute on.</param>
        /// <param name="attributeType">The type of attribute to search for. </param>
        /// <param name="inherit">This argument is ignored for objects of this type.</param>
        /// <returns>true if one or more instances of <paramref name="attributeType" /> or its derived types are applied to the specified <paramref name="parameter"/>; otherwise, false.</returns>
        public override bool IsDefined(ParameterInfo parameter, Type attributeType, bool inherit) => _parameterIsDefined != null ? _parameterIsDefined.Delegate(parameter, attributeType, inherit) : base.IsDefined(parameter, attributeType, inherit);

        /// <summary>
        /// Gets the custom attributes for the return type of the specified <paramref name="method"/>.
        /// </summary>
        /// <param name="method">The method to get the custom attributes for the return type for.</param>
        /// <returns>An <see cref="ICustomAttributeProvider"/> object representing the custom attributes for the return type.</returns>
        public override ICustomAttributeProvider GetReturnTypeCustomAttributes(MethodInfo method) => _methodReturnTypeCustomAttributes != null ? _methodReturnTypeCustomAttributes.Delegate(method) : base.GetReturnTypeCustomAttributes(method);

        /// <summary>
        /// Returns an array of classes accepted by the given filter and filter criteria.
        /// </summary>
        /// <param name="module">The module to find types in.</param>
        /// <param name="filter">The delegate used to filter the classes. </param>
        /// <param name="filterCriteria">An Object used to filter the classes. </param>
        /// <returns>An array of type <see cref="Type"/> containing classes that were accepted by the filter.</returns>
        public override IReadOnlyList<Type> FindTypes(Module module, TypeFilter filter, object filterCriteria) => _moduleFindTypes != null ? _moduleFindTypes.Delegate(module, filter, filterCriteria) : base.FindTypes(module, filter, filterCriteria);

        /// <summary>
        /// Returns an array of <see cref="Type" /> objects representing a filtered list of interfaces implemented or inherited by the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to find interfaces for.</param>
        /// <param name="filter">The delegate that compares the interfaces against <paramref name="filterCriteria" />.</param>
        /// <param name="filterCriteria">The search criteria that determines whether an interface should be included in the returned array.</param>
        /// <returns>An array of <see cref="Type" /> objects representing a filtered list of the interfaces implemented or inherited by the specified <paramref name="type"/>, or an empty array of type <see cref="Type" /> if no interfaces matching the filter are implemented or inherited by the specified <paramref name="type"/>.</returns>
        public override IReadOnlyList<Type> FindInterfaces(Type type, TypeFilter filter, object filterCriteria) => _typeFindInterfaces != null ? _typeFindInterfaces.Delegate(type, filter, filterCriteria) : base.FindInterfaces(type, filter, filterCriteria);

        /// <summary>
        /// Returns a filtered array of <see cref="MemberInfo" /> objects of the specified member type.
        /// </summary>
        /// <param name="type">The type to find members in.</param>
        /// <param name="memberType">An object that indicates the type of member to search for.</param>
        /// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags" /> that specify how the search is conducted.-or- Zero, to return null.</param>
        /// <param name="filter">The delegate that does the comparisons, returning true if the member currently being inspected matches the <paramref name="filterCriteria" /> and false otherwise. You can use the FilterAttribute, FilterName, and FilterNameIgnoreCase delegates supplied by this class. The first uses the fields of FieldAttributes, MethodAttributes, and MethodImplAttributes as search criteria, and the other two delegates use String objects as the search criteria.</param>
        /// <param name="filterCriteria">The search criteria that determines whether a member is returned in the array of MemberInfo objects.The fields of FieldAttributes, MethodAttributes, and MethodImplAttributes can be used in conjunction with the FilterAttribute delegate supplied by this class.</param>
        /// <returns>A filtered array of <see cref="MemberInfo" /> objects of the specified member type.-or- An empty array of type <see cref="MemberInfo" />, if the specified <paramref name="type"/> does not have members of type <paramref name="memberType" /> that match the filter criteria.</returns>
        public override IReadOnlyList<MemberInfo> FindMembers(Type type, MemberTypes memberType, BindingFlags bindingAttr, MemberFilter filter, object filterCriteria) => _typeFindMembers != null ? _typeFindMembers.Delegate(type, memberType, bindingAttr, filter, filterCriteria) : base.FindMembers(type, memberType, bindingAttr, filter, filterCriteria);

        /// <summary>
        /// Clears all the caches.
        /// </summary>
        public void Clear()
        {
            _makeGenericType?.Cache.Clear();
            _makeGenericMethod?.Cache.Clear();

            _moduleGetMethods?.Cache.Clear();
            _moduleGetFields?.Cache.Clear();
            _moduleGetTypes?.Cache.Clear();

            _assemblyGetDefinedTypes?.Cache.Clear();
            _assemblyGetExportedTypes?.Cache.Clear();
            _assemblyGetModules?.Cache.Clear();
            _assemblyGetReferencedAssemblies?.Cache.Clear();
            _assemblyGetTypes?.Cache.Clear();

            _typeGetGenericArguments?.Cache.Clear();
            _typeGetMethods?.Cache.Clear();
            _typeGetMembers?.Cache.Clear();
            _typeGetFields?.Cache.Clear();
            _typeGetProperties?.Cache.Clear();
            _typeGetEvents?.Cache.Clear();
            _typeGetNestedTypes?.Cache.Clear();
            _typeGetConstructors?.Cache.Clear();
            _typeGetInterfaces?.Cache.Clear();
            _typeGetDefaultMembers?.Cache.Clear();
            _typeGetGenericParameterConstraints?.Cache.Clear();

            _fieldGetOptionalCustomModifiers?.Cache.Clear();
            _fieldGetRequiredCustomModifiers?.Cache.Clear();

            _methodGetGenericArguments?.Cache.Clear();
            _methodGetParameters?.Cache.Clear();

            _propertyGetIndexParameters?.Cache.Clear();
            _propertyGetOptionalCustomModifiers?.Cache.Clear();
            _propertyGetRequiredCustomModifiers?.Cache.Clear();
            _propertyGetAccessors?.Cache.Clear();

            _eventGetOtherMethods?.Cache.Clear();

            _parameterGetOptionalCustomModifiers?.Cache.Clear();
            _parameterGetRequiredCustomModifiers?.Cache.Clear();

            _assemblyGetCustomAttributes?.Cache.Clear();
            _assemblyGetCustomAttributesByType?.Cache.Clear();
            _assemblyGetCustomAttributesData?.Cache.Clear();
            _assemblyIsDefined?.Cache.Clear();

            _moduleGetCustomAttributes?.Cache.Clear();
            _moduleGetCustomAttributesByType?.Cache.Clear();
            _moduleGetCustomAttributesData?.Cache.Clear();
            _moduleIsDefined?.Cache.Clear();

            _memberGetCustomAttributes?.Cache.Clear();
            _memberGetCustomAttributesByType?.Cache.Clear();
            _memberGetCustomAttributesData?.Cache.Clear();
            _memberIsDefined?.Cache.Clear();

            _parameterGetCustomAttributes?.Cache.Clear();
            _parameterGetCustomAttributesByType?.Cache.Clear();
            _parameterGetCustomAttributesData?.Cache.Clear();
            _parameterIsDefined?.Cache.Clear();

            _methodReturnTypeCustomAttributes?.Cache.Clear();

            _moduleFindTypes?.Cache.Clear();
            _typeFindInterfaces?.Cache.Clear();
            _typeFindMembers?.Cache.Clear();
        }

        private readonly struct MakeGenericTypeParams : IEquatable<MakeGenericTypeParams>
        {
            public MakeGenericTypeParams(Type definition, Type[] arguments)
            {
                Definition = definition;
                Arguments = arguments;
            }

            public static Type Invoke(MakeGenericTypeParams p) => p.Definition.MakeGenericType(p.Arguments);

            public Type Definition { get; }
            public Type[] Arguments { get; }

            public bool Equals(MakeGenericTypeParams other) => Definition == other.Definition && Utils.SequenceEqual(Arguments, other.Arguments);

            public override bool Equals(object obj) => obj is MakeGenericTypeParams @params && Equals(@params);

            public override int GetHashCode() => Utils.GetHashCode(Definition, Arguments);
        }

        private readonly struct MakeGenericMethodParams : IEquatable<MakeGenericMethodParams>
        {
            public MakeGenericMethodParams(MethodInfo definition, Type[] arguments)
            {
                Definition = definition;
                Arguments = arguments;
            }

            public static MethodInfo Invoke(MakeGenericMethodParams p) => p.Definition.MakeGenericMethod(p.Arguments);

            public MethodInfo Definition { get; }
            public Type[] Arguments { get; }

            public bool Equals(MakeGenericMethodParams other) => Definition == other.Definition && Utils.SequenceEqual(Arguments, other.Arguments);

            public override bool Equals(object obj) => obj is MakeGenericMethodParams @params && Equals(@params);

            public override int GetHashCode() => Utils.GetHashCode(Definition, Arguments);
        }

        private sealed class CachingCustomAttributeProvider : ICustomAttributeProvider, IClearable
        {
            private readonly ICustomAttributeProvider _provider;
            private readonly IMemoizedDelegate<Func<ICustomAttributeProvider, bool, object[]>> _getCustomAttributes;
            private readonly IMemoizedDelegate<Func<ICustomAttributeProvider, Type, bool, object[]>> _getCustomAttributesByType;
            private readonly IMemoizedDelegate<Func<ICustomAttributeProvider, Type, bool, bool>> _isDefined;

            public CachingCustomAttributeProvider(IMemoizer memoizer, ICustomAttributeProvider provider)
            {
                _provider = provider;
                _getCustomAttributes = memoizer.Memoize<ICustomAttributeProvider, bool, object[]>((c, i) => c.GetCustomAttributes(i));
                _getCustomAttributesByType = memoizer.Memoize<ICustomAttributeProvider, Type, bool, object[]>((c, t, i) => c.GetCustomAttributes(t, i));
                _isDefined = memoizer.Memoize<ICustomAttributeProvider, Type, bool, bool>((c, t, i) => c.IsDefined(t, i));
            }

            public void Clear()
            {
                _getCustomAttributes.Cache.Clear();
                _getCustomAttributesByType.Cache.Clear();
                _isDefined.Cache.Clear();
            }

            public object[] GetCustomAttributes(bool inherit) => _getCustomAttributes.Delegate(_provider, inherit);
            public object[] GetCustomAttributes(Type attributeType, bool inherit) => _getCustomAttributesByType.Delegate(_provider, attributeType, inherit);
            public bool IsDefined(Type attributeType, bool inherit) => _isDefined.Delegate(_provider, attributeType, inherit);
        }
    }
}
