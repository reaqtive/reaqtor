// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.QueryEngine.KeyValueStore.InMemory
{
    public class ContainsOperationResult<TKey, TValue> : OperationResult<TKey, TValue>
    {
        private readonly bool _result;

        public ContainsOperationResult(bool result) => _result = result;

        public override Exception Exception => null;

        public override object Result => _result;

        public override bool Equals(object obj) => obj is ContainsOperationResult<TKey, TValue> state && _result == state._result;

        public override int GetHashCode() => _result.GetHashCode();
    }
}
