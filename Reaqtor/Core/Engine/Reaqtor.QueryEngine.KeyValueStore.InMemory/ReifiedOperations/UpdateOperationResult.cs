// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Reaqtor.QueryEngine.KeyValueStore.InMemory
{
    public class UpdateOperationResult<TKey, TValue> : OperationResult<TKey, TValue>
    {
        private readonly bool _keyNotFound;

        public UpdateOperationResult(bool keyNotFound) => _keyNotFound = keyNotFound;

        public override Exception Exception => _keyNotFound ? new KeyNotFoundException() : null;

        public override object Result => throw new NotSupportedException();

        public override bool Equals(object obj) => obj is UpdateOperationResult<TKey, TValue> state && _keyNotFound == state._keyNotFound;

        public override int GetHashCode() => _keyNotFound.GetHashCode();
    }
}
