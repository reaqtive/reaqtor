// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Reaqtor.QueryEngine.KeyValueStore.InMemory
{
    public class GetOperationResult<TKey, TValue> : OperationResult<TKey, TValue>
    {
        private readonly Sequenced<TValue> _result;
        private readonly bool _keyNotFound;

        public GetOperationResult(Sequenced<TValue> result, bool keyNotFound)
        {
            _result = result;
            _keyNotFound = keyNotFound;
        }

        public override Exception Exception => _keyNotFound ? new KeyNotFoundException() : null;

        public override object Result => _result.Object;

        public override bool Equals(object obj) => obj is GetOperationResult<TKey, TValue> state && _keyNotFound == state._keyNotFound && _result.SequenceId == state._result.SequenceId;

        public override int GetHashCode() => _result.SequenceId.GetHashCode() ^ _keyNotFound.GetHashCode();
    }
}
