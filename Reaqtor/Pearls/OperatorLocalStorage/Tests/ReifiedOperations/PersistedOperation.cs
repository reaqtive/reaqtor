// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using Reaqtive.Storage;

namespace Tests.ReifiedOperations
{
    internal enum PersistedOperationKind
    {
        GetId,
    }

    internal interface IPersistedOperationFactory<TPersisted> : IPersistedOperationFactory where TPersisted : IPersisted
    {
        GetIdPersistedOperation<TPersisted> GetId();

        ThisResultOperation<TPersisted> This();
    }

    internal interface IPersistedOperationFactory : IOperationFactory
    {
        GetIdPersistedOperation<TPersisted> GetId<TPersisted>() where TPersisted : IPersisted;
    }

    internal abstract class PersistedOperation : OperationBase
    {
        public abstract PersistedOperationKind Kind { get; }

        public static GetIdPersistedOperation<TPersisted> GetId<TPersisted>() where TPersisted : IPersisted => new();

        private sealed class PersistedOperationFactory : IPersistedOperationFactory
        {
            public GetIdPersistedOperation<TPersisted> GetId<TPersisted>() where TPersisted : IPersisted => GetId<TPersisted>();

            public ThisResultOperation<TValue> This<TValue>() => Operation.This<TValue>();
        }
    }

    internal abstract class ResultPersistedOperation<TPersisted, TResult> : PersistedOperation, IResultOperation<TPersisted, TResult>
        where TPersisted : IPersisted
    {
        public void Accept(TPersisted value) => _ = GetResult(value);

        public abstract TResult GetResult(TPersisted obj);
    }

    internal sealed class GetIdPersistedOperation<TPersisted> : ResultPersistedOperation<TPersisted, string>
        where TPersisted : IPersisted
    {
        internal GetIdPersistedOperation() { }

        public override PersistedOperationKind Kind => PersistedOperationKind.GetId;

        protected override string DebugViewCore => "GetId";

        public override string GetResult(TPersisted obj) => obj.Id;
    }
}
