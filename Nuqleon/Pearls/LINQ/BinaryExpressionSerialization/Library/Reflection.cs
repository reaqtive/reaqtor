// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2014 - Created this file.
//

using System.Collections.Generic;
using System.Reflection;

#if USE_SLIM
using System.Collections.ObjectModel;
#endif

namespace System.Linq.Expressions.Bonsai.Serialization.Binary
{
#if USE_SLIM
    using MemberInfo = System.Reflection.MemberInfoSlim;
    using MethodInfo = System.Reflection.MethodInfoSlim;
    using Type = System.Reflection.TypeSlim;
#endif

    internal static class Types
    {
#if USE_SLIM
        public static readonly Type Void = typeof(void).ToTypeSlim();
        public static readonly Type Boolean = typeof(Boolean).ToTypeSlim();
        public static readonly Type Byte = typeof(Byte).ToTypeSlim();
        public static readonly Type SByte = typeof(SByte).ToTypeSlim();
        public static readonly Type Int16 = typeof(Int16).ToTypeSlim();
        public static readonly Type UInt16 = typeof(UInt16).ToTypeSlim();
        public static readonly Type Int32 = typeof(Int32).ToTypeSlim();
        public static readonly Type UInt32 = typeof(UInt32).ToTypeSlim();
        public static readonly Type Int64 = typeof(Int64).ToTypeSlim();
        public static readonly Type UInt64 = typeof(UInt64).ToTypeSlim();
        public static readonly Type Single = typeof(Single).ToTypeSlim();
        public static readonly Type Double = typeof(Double).ToTypeSlim();
        public static readonly Type Decimal = typeof(Decimal).ToTypeSlim();
        public static readonly Type Char = typeof(Char).ToTypeSlim();
        public static readonly Type String = typeof(String).ToTypeSlim();
        public static readonly Type Object = typeof(Object).ToTypeSlim();
        public static readonly Type DateTime = typeof(DateTime).ToTypeSlim();
        public static readonly Type DateTimeOffset = typeof(DateTimeOffset).ToTypeSlim();
        public static readonly Type TimeSpan = typeof(TimeSpan).ToTypeSlim();
        public static readonly Type Guid = typeof(Guid).ToTypeSlim();
        public static readonly Type Uri = typeof(Uri).ToTypeSlim();
        public static readonly Type NullableOfT = typeof(Nullable<>).ToTypeSlim();

        public static readonly HashSet<Type> GenericDelegateTypes = new()
        {
            typeof(Action).ToTypeSlim(),
            typeof(Action<>).ToTypeSlim(),
            typeof(Action<,>).ToTypeSlim(),
            typeof(Action<,,>).ToTypeSlim(),
            typeof(Action<,,,>).ToTypeSlim(),
            typeof(Action<,,,,>).ToTypeSlim(),
            typeof(Action<,,,,,>).ToTypeSlim(),
            typeof(Action<,,,,,,>).ToTypeSlim(),
            typeof(Action<,,,,,,,>).ToTypeSlim(),
            typeof(Action<,,,,,,,,>).ToTypeSlim(),
            typeof(Action<,,,,,,,,,>).ToTypeSlim(),
            typeof(Action<,,,,,,,,,,>).ToTypeSlim(),
            typeof(Action<,,,,,,,,,,,>).ToTypeSlim(),
            typeof(Action<,,,,,,,,,,,,>).ToTypeSlim(),
            typeof(Action<,,,,,,,,,,,,,>).ToTypeSlim(),
            typeof(Action<,,,,,,,,,,,,,,>).ToTypeSlim(),
            typeof(Action<,,,,,,,,,,,,,,,>).ToTypeSlim(),

            typeof(Func<>).ToTypeSlim(),
            typeof(Func<,>).ToTypeSlim(),
            typeof(Func<,,>).ToTypeSlim(),
            typeof(Func<,,,>).ToTypeSlim(),
            typeof(Func<,,,,>).ToTypeSlim(),
            typeof(Func<,,,,,>).ToTypeSlim(),
            typeof(Func<,,,,,,>).ToTypeSlim(),
            typeof(Func<,,,,,,,>).ToTypeSlim(),
            typeof(Func<,,,,,,,,>).ToTypeSlim(),
            typeof(Func<,,,,,,,,,>).ToTypeSlim(),
            typeof(Func<,,,,,,,,,,>).ToTypeSlim(),
            typeof(Func<,,,,,,,,,,,>).ToTypeSlim(),
            typeof(Func<,,,,,,,,,,,,>).ToTypeSlim(),
            typeof(Func<,,,,,,,,,,,,,>).ToTypeSlim(),
            typeof(Func<,,,,,,,,,,,,,,>).ToTypeSlim(),
            typeof(Func<,,,,,,,,,,,,,,,>).ToTypeSlim(),
            typeof(Func<,,,,,,,,,,,,,,,,>).ToTypeSlim(),
        };
#else
        public static readonly Type Void = typeof(void);
        public static readonly Type Boolean = typeof(Boolean);
        public static readonly Type Byte = typeof(Byte);
        public static readonly Type SByte = typeof(SByte);
        public static readonly Type Int16 = typeof(Int16);
        public static readonly Type UInt16 = typeof(UInt16);
        public static readonly Type Int32 = typeof(Int32);
        public static readonly Type UInt32 = typeof(UInt32);
        public static readonly Type Int64 = typeof(Int64);
        public static readonly Type UInt64 = typeof(UInt64);
        public static readonly Type Single = typeof(Single);
        public static readonly Type Double = typeof(Double);
        public static readonly Type Decimal = typeof(Decimal);
        public static readonly Type Char = typeof(Char);
        public static readonly Type String = typeof(String);
        public static readonly Type Object = typeof(Object);
        public static readonly Type DateTime = typeof(DateTime);
        public static readonly Type DateTimeOffset = typeof(DateTimeOffset);
        public static readonly Type TimeSpan = typeof(TimeSpan);
        public static readonly Type Guid = typeof(Guid);
        public static readonly Type Uri = typeof(Uri);
        public static readonly Type NullableOfT = typeof(Nullable<>);
#endif
    }

    internal abstract class Epyt
    {
        private Type _type;

        public Type ToType(params Epyt[] genericArguments)
        {
            if (_type != null)
            {
                return _type;
            }

            var res = ToTypeCore(genericArguments);

            if (genericArguments.Length == 0)
            {
                _type = res;
            }

            return res;
        }

        protected abstract Type ToTypeCore(Epyt[] genericArguments);
    }

    internal sealed class SimpleEpyt : Epyt
    {
        private readonly Type _type;

        public SimpleEpyt(Type type)
        {
            _type = type;
        }

        protected override Type ToTypeCore(Epyt[] genericArguments)
        {
            return _type;
        }
    }

#if USE_SLIM
    internal sealed class GenericDefinitionEpyt : Epyt
    {
        private readonly Type _type;

        public GenericDefinitionEpyt(Type type)
        {
            _type = type;
        }

        protected override Type ToTypeCore(Epyt[] genericArguments)
        {
            return _type;
        }
    }
#endif

    internal sealed class VectorEpyt : Epyt
    {
        private readonly Epyt _elementType;

        public VectorEpyt(Epyt elementType)
        {
            _elementType = elementType;
        }

        protected override Type ToTypeCore(Epyt[] genericArguments)
        {
            return _elementType.ToType(genericArguments).MakeArrayType();
        }
    }

#if !USE_SLIM
    internal sealed class ByRefEpyt : Epyt
    {
        private readonly Epyt _elementType;

        public ByRefEpyt(Epyt elementType)
        {
            _elementType = elementType;
        }

        protected override Type ToTypeCore(Epyt[] genericArguments)
        {
            return _elementType.ToType(genericArguments).MakeByRefType();
        }
    }
#endif

    internal sealed class ArrayEpyt : Epyt
    {
        private readonly Epyt _elementType;
        private readonly int _rank;

        public ArrayEpyt(Epyt elementType, int rank)
        {
            _elementType = elementType;
            _rank = rank;
        }

        protected override Type ToTypeCore(Epyt[] genericArguments)
        {
            return _elementType.ToType(genericArguments).MakeArrayType(_rank);
        }
    }

    internal sealed class GenericEpyt : Epyt
    {
        private readonly Epyt _genericDefinition;
        private readonly Epyt[] _arguments;

        public GenericEpyt(Epyt genericDefinition, Epyt[] arguments)
        {
            _genericDefinition = genericDefinition;
            _arguments = arguments;
        }

        protected override Type ToTypeCore(Epyt[] genericArguments)
        {
            var def = _genericDefinition.ToType(genericArguments);

            var n = _arguments.Length;

            var args = new Type[n];

            for (var i = 0; i < n; i++)
            {
                args[i] = _arguments[i].ToType(genericArguments);
            }

            return def.MakeGenericType(args);
        }
    }

    internal sealed class GenericParameterEpyt : Epyt
    {
        private readonly int _index;

        public GenericParameterEpyt(int index)
        {
            _index = index;
        }

        protected override Type ToTypeCore(Epyt[] genericArguments)
        {
            return genericArguments[_index].ToType(EmptyArray<Epyt>.Instance);
        }
    }

#if USE_SLIM
    internal sealed class UnboundGenericParameterEpyt : Epyt
    {
        private readonly Type _type;

        public UnboundGenericParameterEpyt(Type type)
        {
            _type = type;
        }

        protected override Type ToTypeCore(Epyt[] genericArguments)
        {
            return _type;
        }
    }
#endif

    internal abstract class MemberOfni
    {
        private MemberInfo _member;

        public MemberOfni(Epyt declaringType)
        {
            DeclaringType = declaringType;
        }

        public Epyt DeclaringType { get; }

        public MemberInfo ToMember(Epyt[] genericArguments)
        {
            if (_member != null)
            {
                return _member;
            }

            var res = ToMemberCore(genericArguments);

            if (genericArguments.Length == 0)
            {
                _member = res;
            }

            return res;
        }

        protected abstract MemberInfo ToMemberCore(Epyt[] genericArguments);
    }

    internal sealed class PropertyOfni : MemberOfni
    {
        private readonly string _name;
#if USE_SLIM
        private readonly Epyt _propertyType;
#endif

        public PropertyOfni(Epyt declaringType, string name
#if USE_SLIM
            , Epyt propertyType
#endif
            )
            : base(declaringType)
        {
            _name = name;
#if USE_SLIM
            _propertyType = propertyType;
#endif
        }

        protected override MemberInfo ToMemberCore(Epyt[] genericArguments)
        {
            return DeclaringType.ToType(genericArguments).GetProperty(_name
#if USE_SLIM
                , _propertyType.ToType(genericArguments)
                , EmptyReadOnlyCollection<Type>.Instance
                , false // REVIEW
#endif
                );
        }
    }

    internal sealed class FieldOfni : MemberOfni
    {
        private readonly string _name;
#if USE_SLIM
        private readonly Epyt _fieldType;
#endif

        public FieldOfni(Epyt declaringType, string name
#if USE_SLIM
            , Epyt fieldType
#endif
            )
            : base(declaringType)
        {
            _name = name;
#if USE_SLIM
            _fieldType = fieldType;
#endif
        }

        protected override MemberInfo ToMemberCore(Epyt[] genericArguments)
        {
            return DeclaringType.ToType(genericArguments).GetField(_name
#if USE_SLIM
                , _fieldType.ToType(genericArguments)
#endif
            );
        }
    }

    internal sealed class IndexerOfni : MemberOfni
    {
        private readonly string _name;
        private readonly Epyt[] _parameterTypes;
#if USE_SLIM
        private readonly Epyt _propertyType;
#endif

        public IndexerOfni(Epyt declaringType, string name, Epyt[] parameterTypes
#if USE_SLIM
            , Epyt propertyType
#endif
            )
            : base(declaringType)
        {
            _name = name;
            _parameterTypes = parameterTypes;
#if USE_SLIM
            _propertyType = propertyType;
#endif
        }

        protected override MemberInfo ToMemberCore(Epyt[] genericArguments)
        {
            var n = _parameterTypes.Length;

            var parameterTypes = new Type[n];

            for (var i = 0; i < n; i++)
            {
                parameterTypes[i] = _parameterTypes[i].ToType(genericArguments);
            }

            return DeclaringType.ToType(genericArguments).GetProperty(_name,
#if USE_SLIM
                _propertyType.ToType(genericArguments),
                new ReadOnlyCollection<Type>(parameterTypes),
                false // REVIEW
#else
                parameterTypes
#endif
            );
        }
    }

    internal sealed class ConstructorOfni : MemberOfni
    {
        private readonly Epyt[] _parameterTypes;

        public ConstructorOfni(Epyt declaringType, Epyt[] parameterTypes)
            : base(declaringType)
        {
            _parameterTypes = parameterTypes;
        }

        protected override MemberInfo ToMemberCore(Epyt[] genericArguments)
        {
            var n = _parameterTypes.Length;

            var parameterTypes = new Type[n];

            for (var i = 0; i < n; i++)
            {
                parameterTypes[i] = _parameterTypes[i].ToType(genericArguments);
            }

            return DeclaringType.ToType(genericArguments).GetConstructor(
#if USE_SLIM
                new ReadOnlyCollection<Type>(parameterTypes)
#else
                parameterTypes
#endif
            );
        }
    }

    internal sealed class MethodOfni : MemberOfni
    {
        private readonly string _name;
        private readonly Epyt[] _parameterTypes;
        private readonly Epyt _returnType;

        public MethodOfni(Epyt declaringType, string name, Epyt[] parameterTypes, Epyt returnType)
            : base(declaringType)
        {
            _name = name;
            _parameterTypes = parameterTypes;
            _returnType = returnType;
        }

        protected override MemberInfo ToMemberCore(Epyt[] genericArguments)
        {
            var n = _parameterTypes.Length;

            var parameterTypes = new Type[n];

            for (var i = 0; i < n; i++)
            {
                parameterTypes[i] = _parameterTypes[i].ToType(genericArguments);
            }

            var returnType = _returnType.ToType(genericArguments);

            var declaringType = DeclaringType.ToType(genericArguments);

#if USE_SLIM
            return declaringType.GetSimpleMethod(_name, new ReadOnlyCollection<Type>(parameterTypes), returnType);
#else
            var methods = declaringType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            foreach (var method in methods)
            {
                if (method.Name == _name)
                {
                    if (method.ReturnType == returnType)
                    {
                        var pars = method.GetParameters();

                        if (pars.Length == n)
                        {
                            var found = true;

                            for (var i = 0; i < n; i++)
                            {
                                if (pars[i].ParameterType != parameterTypes[i])
                                {
                                    found = false;
                                    break;
                                }
                            }

                            if (found)
                            {
                                return method;
                            }
                        }
                    }
                }
            }

            throw new InvalidOperationException("Method not found.");
#endif
        }
    }

    internal sealed class GenericMethodOfni : MemberOfni
    {
        private readonly MemberOfni _genericMethodDefinition;
        private readonly Epyt[] _parameterTypes;

        public GenericMethodOfni(MemberOfni genericMethodDefinition, Epyt[] parameterTypes)
            : base(genericMethodDefinition.DeclaringType)
        {
            _genericMethodDefinition = genericMethodDefinition;
            _parameterTypes = parameterTypes;
        }

        protected override MemberInfo ToMemberCore(Epyt[] genericArguments)
        {
            var n = _parameterTypes.Length;

            var genDef = (MethodInfo)_genericMethodDefinition.ToMember(_parameterTypes);

            var types = new Type[n];

            for (var i = 0; i < n; i++)
            {
                types[i] = _parameterTypes[i].ToType(genericArguments);
            }

            return genDef.MakeGenericMethod(types);
        }
    }

    internal sealed class GenericMethodDefinitionOfni : MemberOfni
    {
        private readonly string _name;
        private readonly Epyt[] _parameterTypes;
        private readonly Epyt _returnType;

        public GenericMethodDefinitionOfni(Epyt declaringType, string name, int arity, Epyt[] parameterTypes, Epyt returnType)
            : base(declaringType)
        {
            _name = name;
            Arity = arity;
            _parameterTypes = parameterTypes;
            _returnType = returnType;
        }

        public int Arity { get; }

        protected override MemberInfo ToMemberCore(Epyt[] genericArguments)
        {
            var n = _parameterTypes.Length;

#if USE_SLIM
            var declaringType = DeclaringType.ToType(EmptyArray<Epyt>.Instance);

            var genericParameters = new Epyt[Arity];
            var genericParameterTypes = new Type[Arity];

            for (var i = 0; i < Arity; i++)
            {
                var parameter = Type.GenericParameter("T" + i);
                genericParameterTypes[i] = parameter;
                genericParameters[i] = new UnboundGenericParameterEpyt(parameter);
            }

            var parameterTypes = new Type[n];

            for (var i = 0; i < n; i++)
            {
                parameterTypes[i] = _parameterTypes[i].ToType(genericParameters);
            }

            var returnType = _returnType.ToType(genericParameters);

            return declaringType.GetGenericDefinitionMethod(
                _name,
                new ReadOnlyCollection<Type>(genericParameterTypes),
                new ReadOnlyCollection<Type>(parameterTypes),
                returnType
            );
#else
            var declaringType = DeclaringType.ToType(genericArguments); // REVIEW: Does this work for generic methods on generic types?

            var candidates = new List<MethodInfo>();

            foreach (var method in declaringType.GetMethods())
            {
                if (method.IsGenericMethodDefinition && method.Name == _name)
                {
                    var genArgs = method.GetGenericArguments();
                    var parameters = method.GetParameters();

                    if (genArgs.Length == Arity && parameters.Length == n)
                    {
                        candidates.Add(method);
                    }
                }
            }

            foreach (var candidate in candidates)
            {
                var genArgs = candidate.GetGenericArguments();

                var arity = genArgs.Length;

                var genArgsTypes = new Epyt[arity];

                for (var i = 0; i < arity; i++)
                {
                    genArgsTypes[i] = new SimpleEpyt(genArgs[i]);
                }

                var parameters = candidate.GetParameters();

                var match = true;

                for (var i = 0; i < n; i++)
                {
                    var parameterType = _parameterTypes[i].ToType(genArgsTypes);
                    if (parameters[i].ParameterType != parameterType)
                    {
                        match = false;
                        break;
                    }
                }

                if (match)
                {
                    var returnType = _returnType.ToType(genArgsTypes);
                    if (candidate.ReturnType != returnType)
                    {
                        match = false;
                    }
                }

                if (match)
                {
                    return candidate; // TODO: continue to detect ambiguity?
                }
            }

            throw new NotImplementedException();
#endif
        }
    }
}
