// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Reaqtor.QueryEngine.KeyValueStore.InMemory
{
    public class RemoveOperationResult<TKey, TValue> : OperationResult<TKey, TValue>
    {
        private readonly bool _keyDoesNotExist;

        public RemoveOperationResult(bool keyDoesNotExist) => _keyDoesNotExist = keyDoesNotExist;

        public override Exception Exception => _keyDoesNotExist ? new KeyNotFoundException() : null;

        public override object Result => throw new NotSupportedException();

        public override bool Equals(object obj) => obj is RemoveOperationResult<TKey, TValue> state && _keyDoesNotExist == state._keyDoesNotExist;

        public override int GetHashCode() => _keyDoesNotExist.GetHashCode();
    }
}
