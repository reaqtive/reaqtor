// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using System;

using Reaqtive.Storage;

using Reaqtor.QueryEngine;

namespace Tests.ReifiedOperations
{
    internal enum PersistedObjectSpaceOperationKind
    {
        CreateValue,
        GetValue,
        CreateArray,
        GetArray,
        CreateList,
        GetList,
        CreateQueue,
        GetQueue,
        CreateStack,
        GetStack,
        CreateSet,
        GetSet,
        CreateSortedSet,
        GetSortedSet,
        CreateDictionary,
        GetDictionary,
        CreateSortedDictionary,
        GetSortedDictionary,
        Delete,

        /*
        Load,
        Save,
        OnSaved,
        */
    }

    internal abstract class PersistedObjectSpaceOperation : OperationBase, IOperation<IPersistedObjectSpace>
    {
        public abstract PersistedObjectSpaceOperationKind Kind { get; }

        public abstract void Accept(IPersistedObjectSpace value);

        public static CreateValuePersistedObjectSpaceOperation<T> CreateValue<T>(string id) => new(id);

        public static GetValuePersistedObjectSpaceOperation<T> GetValue<T>(string id) => new(id);

        public static CreateArrayPersistedObjectSpaceOperation<T> CreateArray<T>(string id, int length) => new(id, length);

        public static GetArrayPersistedObjectSpaceOperation<T> GetArray<T>(string id) => new(id);

        public static CreateListPersistedObjectSpaceOperation<T> CreateList<T>(string id) => new(id);

        public static GetListPersistedObjectSpaceOperation<T> GetList<T>(string id) => new(id);

        public static CreateQueuePersistedObjectSpaceOperation<T> CreateQueue<T>(string id) => new(id);

        public static GetQueuePersistedObjectSpaceOperation<T> GetQueue<T>(string id) => new(id);

        public static CreateStackPersistedObjectSpaceOperation<T> CreateStack<T>(string id) => new(id);

        public static GetStackPersistedObjectSpaceOperation<T> GetStack<T>(string id) => new(id);

        public static CreateSetPersistedObjectSpaceOperation<T> CreateSet<T>(string id) => new(id);

        public static GetSetPersistedObjectSpaceOperation<T> GetSet<T>(string id) => new(id);

        public static CreateSortedSetPersistedObjectSpaceOperation<T> CreateSortedSet<T>(string id) => new(id);

        public static GetSortedSetPersistedObjectSpaceOperation<T> GetSortedSet<T>(string id) => new(id);

        public static CreateDictionaryPersistedObjectSpaceOperation<TKey, TValue> CreateDictionary<TKey, TValue>(string id) => new(id);

        public static GetDictionaryPersistedObjectSpaceOperation<TKey, TValue> GetDictionary<TKey, TValue>(string id) => new(id);

        public static CreateSortedDictionaryPersistedObjectSpaceOperation<TKey, TValue> CreateSortedDictionary<TKey, TValue>(string id) => new(id);

        public static GetSortedDictionaryPersistedObjectSpaceOperation<TKey, TValue> GetSortedDictionary<TKey, TValue>(string id) => new(id);

        public static DeletePersistedObjectSpaceOperation Delete(string id) => new(id);

        /*
        public static LoadPersistedObjectSpaceOperation Load(IStateReader reader) => new LoadPersistedObjectSpaceOperation(reader);

        public static SavePersistedObjectSpaceOperation Save(IStateWriter writer) => new SavePersistedObjectSpaceOperation(writer);

        public static OnSavedPersistedObjectSpaceOperation OnSaved() => new OnSavedPersistedObjectSpaceOperation();
        */
    }

    internal abstract class EntityPersistedObjectSpaceOperation : PersistedObjectSpaceOperation
    {
        protected EntityPersistedObjectSpaceOperation(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }

    internal sealed class DeletePersistedObjectSpaceOperation : EntityPersistedObjectSpaceOperation
    {
        internal DeletePersistedObjectSpaceOperation(string id)
            : base(id)
        {
        }

        public override PersistedObjectSpaceOperationKind Kind => PersistedObjectSpaceOperationKind.Delete;

        protected override string DebugViewCore => FormattableString.Invariant($"Delete({Id})");

        public override void Accept(IPersistedObjectSpace space) => space.Delete(Id);
    }

    internal abstract class ResultPersistedObjectSpaceOperation<TResult, TReified> : EntityPersistedObjectSpaceOperation, IReifiedResultOperation<IPersistedObjectSpace, TResult, TReified>
    {
        internal ResultPersistedObjectSpaceOperation(string id)
            : base(id)
        {
        }

        public abstract TReified Reified { get; }

        public sealed override void Accept(IPersistedObjectSpace space) => _ = GetResult(space);

        public abstract TResult GetResult(IPersistedObjectSpace space);
    }

    internal abstract class ValuePersistedObjectSpaceOperation<T> : ResultPersistedObjectSpaceOperation<IPersistedValue<T>, IPersistedValueOperationFactory<T>>
    {
        internal ValuePersistedObjectSpaceOperation(string id)
            : base(id)
        {
        }

        protected override string DebugViewCore => FormattableString.Invariant($"{Kind}<{typeof(T).Name}>({Id})");

        public override IPersistedValueOperationFactory<T> Reified => PersistedValueOperation.WithType<T>();
    }

    internal sealed class GetValuePersistedObjectSpaceOperation<T> : ValuePersistedObjectSpaceOperation<T>
    {
        internal GetValuePersistedObjectSpaceOperation(string id)
            : base(id)
        {
        }

        public override PersistedObjectSpaceOperationKind Kind => PersistedObjectSpaceOperationKind.GetValue;

        public override IPersistedValue<T> GetResult(IPersistedObjectSpace space) => space.GetValue<T>(Id);
    }

    internal sealed class CreateValuePersistedObjectSpaceOperation<T> : ValuePersistedObjectSpaceOperation<T>
    {
        internal CreateValuePersistedObjectSpaceOperation(string id)
            : base(id)
        {
        }

        public override PersistedObjectSpaceOperationKind Kind => PersistedObjectSpaceOperationKind.CreateValue;

        public override IPersistedValue<T> GetResult(IPersistedObjectSpace space) => space.CreateValue<T>(Id);
    }

    internal abstract class ArrayPersistedObjectSpaceOperation<T> : ResultPersistedObjectSpaceOperation<IPersistedArray<T>, IPersistedArrayOperationFactory<T>>
    {
        internal ArrayPersistedObjectSpaceOperation(string id)
            : base(id)
        {
        }

        protected override string DebugViewCore => FormattableString.Invariant($"{Kind}<{typeof(T).Name}>({Id})");

        public override IPersistedArrayOperationFactory<T> Reified => PersistedArrayOperation.WithType<T>();
    }

    internal sealed class GetArrayPersistedObjectSpaceOperation<T> : ArrayPersistedObjectSpaceOperation<T>
    {
        internal GetArrayPersistedObjectSpaceOperation(string id)
            : base(id)
        {
        }

        public override PersistedObjectSpaceOperationKind Kind => PersistedObjectSpaceOperationKind.GetArray;

        public override IPersistedArray<T> GetResult(IPersistedObjectSpace space) => space.GetArray<T>(Id);
    }

    internal sealed class CreateArrayPersistedObjectSpaceOperation<T> : ArrayPersistedObjectSpaceOperation<T>
    {
        internal CreateArrayPersistedObjectSpaceOperation(string id, int length)
            : base(id)
        {
            Length = length;
        }

        public override PersistedObjectSpaceOperationKind Kind => PersistedObjectSpaceOperationKind.CreateArray;

        public int Length { get; }

        public override IPersistedArray<T> GetResult(IPersistedObjectSpace space) => space.CreateArray<T>(Id, Length);
    }

    internal abstract class ListPersistedObjectSpaceOperation<T> : ResultPersistedObjectSpaceOperation<IPersistedList<T>, IPersistedListOperationFactory<T>>
    {
        internal ListPersistedObjectSpaceOperation(string id)
            : base(id)
        {
        }

        protected override string DebugViewCore => FormattableString.Invariant($"{Kind}<{typeof(T).Name}>({Id})");

        public override IPersistedListOperationFactory<T> Reified => PersistedListOperation.WithType<T>();
    }

    internal sealed class GetListPersistedObjectSpaceOperation<T> : ListPersistedObjectSpaceOperation<T>
    {
        internal GetListPersistedObjectSpaceOperation(string id)
            : base(id)
        {
        }

        public override PersistedObjectSpaceOperationKind Kind => PersistedObjectSpaceOperationKind.GetList;

        public override IPersistedList<T> GetResult(IPersistedObjectSpace space) => space.GetList<T>(Id);
    }

    internal sealed class CreateListPersistedObjectSpaceOperation<T> : ListPersistedObjectSpaceOperation<T>
    {
        internal CreateListPersistedObjectSpaceOperation(string id)
            : base(id)
        {
        }

        public override PersistedObjectSpaceOperationKind Kind => PersistedObjectSpaceOperationKind.CreateList;

        public override IPersistedList<T> GetResult(IPersistedObjectSpace space) => space.CreateList<T>(Id);
    }

    internal abstract class QueuePersistedObjectSpaceOperation<T> : ResultPersistedObjectSpaceOperation<IPersistedQueue<T>, IPersistedQueueOperationFactory<T>>
    {
        internal QueuePersistedObjectSpaceOperation(string id)
            : base(id)
        {
        }

        protected override string DebugViewCore => FormattableString.Invariant($"{Kind}<{typeof(T).Name}>({Id})");

        public override IPersistedQueueOperationFactory<T> Reified => PersistedQueueOperation.WithType<T>();
    }

    internal sealed class GetQueuePersistedObjectSpaceOperation<T> : QueuePersistedObjectSpaceOperation<T>
    {
        internal GetQueuePersistedObjectSpaceOperation(string id)
            : base(id)
        {
        }

        public override PersistedObjectSpaceOperationKind Kind => PersistedObjectSpaceOperationKind.GetQueue;

        public override IPersistedQueue<T> GetResult(IPersistedObjectSpace space) => space.GetQueue<T>(Id);
    }

    internal sealed class CreateQueuePersistedObjectSpaceOperation<T> : QueuePersistedObjectSpaceOperation<T>
    {
        internal CreateQueuePersistedObjectSpaceOperation(string id)
            : base(id)
        {
        }

        public override PersistedObjectSpaceOperationKind Kind => PersistedObjectSpaceOperationKind.CreateQueue;

        public override IPersistedQueue<T> GetResult(IPersistedObjectSpace space) => space.CreateQueue<T>(Id);
    }

    internal abstract class StackPersistedObjectSpaceOperation<T> : ResultPersistedObjectSpaceOperation<IPersistedStack<T>, IPersistedStackOperationFactory<T>>
    {
        internal StackPersistedObjectSpaceOperation(string id)
            : base(id)
        {
        }

        protected override string DebugViewCore => FormattableString.Invariant($"{Kind}<{typeof(T).Name}>({Id})");

        public override IPersistedStackOperationFactory<T> Reified => PersistedStackOperation.WithType<T>();
    }

    internal sealed class GetStackPersistedObjectSpaceOperation<T> : StackPersistedObjectSpaceOperation<T>
    {
        internal GetStackPersistedObjectSpaceOperation(string id)
            : base(id)
        {
        }

        public override PersistedObjectSpaceOperationKind Kind => PersistedObjectSpaceOperationKind.GetStack;

        public override IPersistedStack<T> GetResult(IPersistedObjectSpace space) => space.GetStack<T>(Id);
    }

    internal sealed class CreateStackPersistedObjectSpaceOperation<T> : StackPersistedObjectSpaceOperation<T>
    {
        internal CreateStackPersistedObjectSpaceOperation(string id)
            : base(id)
        {
        }

        public override PersistedObjectSpaceOperationKind Kind => PersistedObjectSpaceOperationKind.CreateStack;

        public override IPersistedStack<T> GetResult(IPersistedObjectSpace space) => space.CreateStack<T>(Id);
    }

    internal abstract class SetPersistedObjectSpaceOperation<T> : ResultPersistedObjectSpaceOperation<IPersistedSet<T>, IPersistedSetOperationFactory<T>>
    {
        internal SetPersistedObjectSpaceOperation(string id)
            : base(id)
        {
        }

        protected override string DebugViewCore => FormattableString.Invariant($"{Kind}<{typeof(T).Name}>({Id})");

        public override IPersistedSetOperationFactory<T> Reified => PersistedSetOperation.WithType<T>();
    }

    internal sealed class GetSetPersistedObjectSpaceOperation<T> : SetPersistedObjectSpaceOperation<T>
    {
        internal GetSetPersistedObjectSpaceOperation(string id)
            : base(id)
        {
        }

        public override PersistedObjectSpaceOperationKind Kind => PersistedObjectSpaceOperationKind.GetSet;

        public override IPersistedSet<T> GetResult(IPersistedObjectSpace space) => space.GetSet<T>(Id);
    }

    internal sealed class CreateSetPersistedObjectSpaceOperation<T> : SetPersistedObjectSpaceOperation<T>
    {
        internal CreateSetPersistedObjectSpaceOperation(string id)
            : base(id)
        {
        }

        public override PersistedObjectSpaceOperationKind Kind => PersistedObjectSpaceOperationKind.CreateSet;

        public override IPersistedSet<T> GetResult(IPersistedObjectSpace space) => space.CreateSet<T>(Id);
    }

    internal abstract class SortedSetPersistedObjectSpaceOperation<T> : ResultPersistedObjectSpaceOperation<IPersistedSortedSet<T>, IPersistedSortedSetOperationFactory<T>>
    {
        internal SortedSetPersistedObjectSpaceOperation(string id)
            : base(id)
        {
        }

        protected override string DebugViewCore => FormattableString.Invariant($"{Kind}<{typeof(T).Name}>({Id})");

        public override IPersistedSortedSetOperationFactory<T> Reified => PersistedSortedSetOperation.WithType<T>();
    }

    internal sealed class GetSortedSetPersistedObjectSpaceOperation<T> : SortedSetPersistedObjectSpaceOperation<T>
    {
        internal GetSortedSetPersistedObjectSpaceOperation(string id)
            : base(id)
        {
        }

        public override PersistedObjectSpaceOperationKind Kind => PersistedObjectSpaceOperationKind.GetSortedSet;

        public override IPersistedSortedSet<T> GetResult(IPersistedObjectSpace space) => space.GetSortedSet<T>(Id);
    }

    internal sealed class CreateSortedSetPersistedObjectSpaceOperation<T> : SortedSetPersistedObjectSpaceOperation<T>
    {
        internal CreateSortedSetPersistedObjectSpaceOperation(string id)
            : base(id)
        {
        }

        public override PersistedObjectSpaceOperationKind Kind => PersistedObjectSpaceOperationKind.CreateSortedSet;

        public override IPersistedSortedSet<T> GetResult(IPersistedObjectSpace space) => space.CreateSortedSet<T>(Id);
    }

    internal abstract class DictionaryPersistedObjectSpaceOperation<TKey, TValue> : ResultPersistedObjectSpaceOperation<IPersistedDictionary<TKey, TValue>, IPersistedDictionaryOperationFactory<TKey, TValue>>
    {
        internal DictionaryPersistedObjectSpaceOperation(string id)
            : base(id)
        {
        }

        protected override string DebugViewCore => FormattableString.Invariant($"{Kind}<{typeof(TKey).Name}, {typeof(TValue).Name}>({Id})");

        public override IPersistedDictionaryOperationFactory<TKey, TValue> Reified => PersistedDictionaryOperation.WithType<TKey, TValue>();
    }

    internal sealed class GetDictionaryPersistedObjectSpaceOperation<TKey, TValue> : DictionaryPersistedObjectSpaceOperation<TKey, TValue>
    {
        internal GetDictionaryPersistedObjectSpaceOperation(string id)
            : base(id)
        {
        }

        public override PersistedObjectSpaceOperationKind Kind => PersistedObjectSpaceOperationKind.GetDictionary;

        public override IPersistedDictionary<TKey, TValue> GetResult(IPersistedObjectSpace space) => space.GetDictionary<TKey, TValue>(Id);
    }

    internal sealed class CreateDictionaryPersistedObjectSpaceOperation<TKey, TValue> : DictionaryPersistedObjectSpaceOperation<TKey, TValue>
    {
        internal CreateDictionaryPersistedObjectSpaceOperation(string id)
            : base(id)
        {
        }

        public override PersistedObjectSpaceOperationKind Kind => PersistedObjectSpaceOperationKind.CreateDictionary;

        public override IPersistedDictionary<TKey, TValue> GetResult(IPersistedObjectSpace space) => space.CreateDictionary<TKey, TValue>(Id);
    }

    internal abstract class SortedDictionaryPersistedObjectSpaceOperation<TKey, TValue> : ResultPersistedObjectSpaceOperation<IPersistedSortedDictionary<TKey, TValue>, IPersistedSortedDictionaryOperationFactory<TKey, TValue>>
    {
        internal SortedDictionaryPersistedObjectSpaceOperation(string id)
            : base(id)
        {
        }

        protected override string DebugViewCore => FormattableString.Invariant($"{Kind}<{typeof(TKey).Name}, {typeof(TValue).Name}>({Id})");

        public override IPersistedSortedDictionaryOperationFactory<TKey, TValue> Reified => PersistedSortedDictionaryOperation.WithType<TKey, TValue>();
    }

    internal sealed class GetSortedDictionaryPersistedObjectSpaceOperation<TKey, TValue> : SortedDictionaryPersistedObjectSpaceOperation<TKey, TValue>
    {
        internal GetSortedDictionaryPersistedObjectSpaceOperation(string id)
            : base(id)
        {
        }

        public override PersistedObjectSpaceOperationKind Kind => PersistedObjectSpaceOperationKind.GetSortedDictionary;

        public override IPersistedSortedDictionary<TKey, TValue> GetResult(IPersistedObjectSpace space) => space.GetSortedDictionary<TKey, TValue>(Id);
    }

    internal sealed class CreateSortedDictionaryPersistedObjectSpaceOperation<TKey, TValue> : SortedDictionaryPersistedObjectSpaceOperation<TKey, TValue>
    {
        internal CreateSortedDictionaryPersistedObjectSpaceOperation(string id)
            : base(id)
        {
        }

        public override PersistedObjectSpaceOperationKind Kind => PersistedObjectSpaceOperationKind.CreateSortedDictionary;

        public override IPersistedSortedDictionary<TKey, TValue> GetResult(IPersistedObjectSpace space) => space.CreateSortedDictionary<TKey, TValue>(Id);
    }

    /*
    internal sealed class LoadPersistedObjectSpaceOperation : PersistedObjectSpaceOperation
    {
        internal LoadPersistedObjectSpaceOperation(IStateReader reader)
        {
            Reader = reader;
        }

        public override PersistedObjectSpaceOperationKind Kind => PersistedObjectSpaceOperationKind.Load;

        public IStateReader Reader { get; }

        protected override string DebugViewCore => FormattableString.Invariant($"Load({Reader})");

        public override void Accept(IPersistedObjectSpace space) => space.Load(Reader);
    }

    internal sealed class SavePersistedObjectSpaceOperation : PersistedObjectSpaceOperation
    {
        internal SavePersistedObjectSpaceOperation(IStateWriter writer)
        {
            Writer = writer;
        }

        public override PersistedObjectSpaceOperationKind Kind => PersistedObjectSpaceOperationKind.Save;

        public IStateWriter Writer { get; }

        protected override string DebugViewCore => FormattableString.Invariant($"Save({Writer})");

        public override void Accept(IPersistedObjectSpace space) => space.Save(Writer);
    }

    internal sealed class OnSavedPersistedObjectSpaceOperation : PersistedObjectSpaceOperation
    {
        internal OnSavedPersistedObjectSpaceOperation() { }

        public override PersistedObjectSpaceOperationKind Kind => PersistedObjectSpaceOperationKind.OnSaved;

        protected override string DebugViewCore => FormattableString.Invariant($"OnSaved()");

        public override void Accept(IPersistedObjectSpace space) => space.OnSaved();
    }
    */
}
