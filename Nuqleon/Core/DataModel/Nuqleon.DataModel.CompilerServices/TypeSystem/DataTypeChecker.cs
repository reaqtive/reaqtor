// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Linq.CompilerServices;
using System.Memory;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Nuqleon.DataModel.TypeSystem
{
    internal static class DataTypeChecker
    {
        private static readonly ObjectPool<Impl> s_poolNoCycles = new(() => new Impl(allowCycles: false));
        private static readonly ObjectPool<Impl> s_poolAllowCycles = new(() => new Impl(allowCycles: true));

        private static readonly ConditionalWeakTable<Type, ReadOnlyCollection<DataTypeError>> s_cacheNoCycles = new();
        private static readonly ConditionalWeakTable<Type, ReadOnlyCollection<DataTypeError>> s_cacheAllowCycles = new();

        public static bool TryCheck(Type type, bool allowCycles, out ReadOnlyCollection<DataTypeError> errors)
        {
            errors = allowCycles
                ? s_cacheAllowCycles.GetValue(type, t => CheckCore(t, allowCycles: true))
                : s_cacheNoCycles.GetValue(type, t => CheckCore(t, allowCycles: false));

            var result = errors == null;
            errors ??= EmptyReadOnlyCollection<DataTypeError>.Instance;
            return result;
        }

        private static ReadOnlyCollection<DataTypeError> CheckCore(Type type, bool allowCycles)
        {
            using var pimpl = (allowCycles ? s_poolAllowCycles : s_poolNoCycles).New();

            var chk = pimpl.Object;

            var res = chk.Visit(type);
            var errors = chk.Errors;

            return res ? null : errors;
        }

        private sealed class Impl : DataModelTypeVisitorBase<bool>, IClearable
        {
            private readonly bool _allowCycles;
            private readonly Stack<Record> _stack;
            private readonly List<DataTypeError> _errors;

            public Impl(bool allowCycles)
            {
                _allowCycles = allowCycles;
                _stack = new Stack<Record>();
                _errors = new List<DataTypeError>();
            }

            private sealed class Record
            {
                private List<string> _errors;

                public Record(Type type)
                {
                    Type = type;
                }

                public Type Type { get; }

                public List<string> Errors
                {
                    get
                    {
                        _errors ??= new List<string>();

                        return _errors;
                    }
                }

                public bool HasErrors => _errors != null && _errors.Count > 0;
            }

            public ReadOnlyCollection<DataTypeError> Errors =>
                    // NB: Needs to copy because the _errors list is reused in the pool.
                    _errors.Count == 0 ? EmptyReadOnlyCollection<DataTypeError>.Instance : new List<DataTypeError>(_errors).AsReadOnly();

            public override bool Visit(Type type)
            {
                if (type == null)
                {
                    throw new ArgumentNullException(nameof(type));
                }

                foreach (var previousRecord in _stack)
                {
                    if (previousRecord.Type == type)
                    {
                        if (!_allowCycles)
                        {
                            var path = _stack.TakeWhile(r => r.Type != type).Select(t => t.Type.ToCSharpString()).ToList();
                            path.Add(type.ToCSharpString());
                            path.Reverse();
                            path.Add(type.ToCSharpString());

                            var cycle = string.Format(CultureInfo.InvariantCulture, "Cycle detected due to recursive type definition: {0}", string.Join(" -> ", path));

                            _errors.Add(new DataTypeError(type, cycle, _stack.Select(t => t.Type).ToArray()));

                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }

                _stack.Push(new Record(type));

                var res = base.Visit(type);

                var record = _stack.Pop();

                if (!res)
                {
                    if (record.HasErrors)
                    {
                        using var psb = PooledStringBuilder.New();

                        var sb = psb.StringBuilder;

                        foreach (var error in record.Errors)
                        {
                            sb.AppendLine(error);
                        }

                        _errors.Add(new DataTypeError(type, sb.ToString(), _stack.Select(t => t.Type).ToArray()));
                    }
                }

                return res;
            }

            protected override bool MakeVoidType(Type type) => true;

            protected override bool MakePrimitiveType(Type type) => true;

            protected override bool MakeEnumType(Type type) => true;

            protected override bool MakeEntityEnumType(Type type, IEnumerable<DataEnumValue> values) => true;

            protected override bool MakeExpressionType(Type type) => true;

            protected override bool MakeQuotationType(Type type, bool functionType) => functionType;

            protected override bool MakeTupleType(Type type, IEnumerable<bool> components) => components.All(b => b);

            protected override bool MakeDynamicType(Type type)
            {
                _stack.Peek().Errors.Add(string.Format(CultureInfo.InvariantCulture, "Type '{0}' is a dynamic type, which is not supported by the data model (yet).", type.ToCSharpStringPretty()));
                return false;
            }

            protected override bool MakeOpenGenericParameterType(Type type) => true;

            protected override bool DefineAnonymousType(Type type) => true;

            protected override bool MakeAnonymousType(bool type, IEnumerable<DataProperty<bool>> properties) => properties.All(kv => kv.Type);

            protected override bool DefineRecordType(Type type) => true;

            protected override bool MakeRecordType(bool type, IEnumerable<DataProperty<bool>> entries) => entries.All(kv => kv.Type);

            protected override bool DefineEntityType(Type type) => true;

            protected override bool MakeEntityType(bool type, IEnumerable<DataProperty<bool>> properties) => properties.All(kv => kv.Type);

            protected override bool MakeFunctionType(Type type, bool[] parameterTypes, bool returnType) => parameterTypes.All(b => b) && returnType;

            protected override bool MakeArrayType(Type type, bool elementType) => elementType;

            protected override bool FailArrayType(Type type, bool elementType, int rank)
            {
                _stack.Peek().Errors.Add(GetFailArrayTypeError(type));
                return false;
            }

            protected override bool FailByRefType(Type type, bool elementType)
            {
                _stack.Peek().Errors.Add(GetFailByRefTypeError(type));
                return false;
            }

            protected override bool FailEntityType(Type type)
            {
                _stack.Peek().Errors.Insert(0, string.Format(CultureInfo.InvariantCulture, "{0} See further for detailed error messages.", GetFailEntityTypeError(type)));
                return false;
            }

            protected override void FailEntityTypePropertyMissingMapping(MemberInfo property)
            {
                _stack.Peek().Errors.Add(GetFailEntityTypePropertyMissingMappingError(property));
            }

            protected override void FailEntityTypePropertyDuplicateMapping(MemberInfo property, string mappingUri)
            {
                _stack.Peek().Errors.Add(GetFailEntityTypePropertyDuplicateMappingError(property, mappingUri));
            }

            protected override void FailEntityTypePropertyNotReadable(PropertyInfo property)
            {
                _stack.Peek().Errors.Add(GetFailEntityTypePropertyNotReadableError(property));
            }

            protected override void FailEntityTypeConstructorParameterAndPropertyTypeMismatch(ConstructorInfo constructor, ParameterInfo parameter, string mappingUri, MemberInfo property)
            {
                _stack.Peek().Errors.Add(GetFailEntityTypeConstructorParameterAndPropertyTypeMismatchError(constructor, parameter, mappingUri, property));
            }

            protected override void FailEntityTypeConstructorParameterInvalidMapping(ConstructorInfo constructor, ParameterInfo parameter, string mappingUri)
            {
                _stack.Peek().Errors.Add(GetFailEntityTypeConstructorParameterInvalidMappingError(constructor, parameter, mappingUri));
            }

            protected override void FailEntityTypeConstructorParameterMissingMapping(ConstructorInfo constructor, ParameterInfo parameter)
            {
                _stack.Peek().Errors.Add(GetFailEntityTypeConstructorParameterMissingMappingError(constructor, parameter));
            }

            protected override bool FailEntityEnumType(Type type)
            {
                _stack.Peek().Errors.Insert(0, string.Format(CultureInfo.InvariantCulture, "{0} See further for detailed error messages.", GetFailEntityEnumTypeError(type)));
                return false;
            }

            protected override void FailEntityEnumTypeFieldMissingMapping(FieldInfo field)
            {
                _stack.Peek().Errors.Add(GetFailEntityEnumTypeFieldMissingMappingError(field));
            }

            protected override void FailEntityEnumTypeFieldDuplicateMapping(FieldInfo field, string mappingUri)
            {
                _stack.Peek().Errors.Add(GetFailEntityEnumTypeFieldDuplicateMappingError(field, mappingUri));
            }

            protected override bool FailGenericParameter(Type type)
            {
                _stack.Peek().Errors.Add(GetFailGenericParameterError(type));
                return false;
            }

            protected override bool FailGenericType(Type type)
            {
                _stack.Peek().Errors.Add(GetFailGenericTypeError(type));
                return false;
            }

            protected override bool FailGenericTypeDefinition(Type type)
            {
                _stack.Peek().Errors.Add(GetFailGenericTypeDefinitionError(type));
                return false;
            }

            protected override bool FailNullableType(Type type)
            {
                _stack.Peek().Errors.Add(GetFailNullableTypeError(type));
                return false;
            }

            protected override bool FailPointerType(Type type, bool elementType)
            {
                _stack.Peek().Errors.Add(GetFailPointerTypeError(type));
                return false;
            }

            protected override bool FailSimpleType(Type type)
            {
                _stack.Peek().Errors.Add(GetFailSimpleTypeError(type));
                return false;
            }

            public void Clear()
            {
                // CONSIDER: Add the ShouldReturnToPool pattern to keep big instances from polluting the pool.
                // CONSIDER: Generalize the ShouldReturnToPool pattern to be supported in System.Memory natively.

                _stack.Clear();
                _errors.Clear();
            }
        }
    }
}
