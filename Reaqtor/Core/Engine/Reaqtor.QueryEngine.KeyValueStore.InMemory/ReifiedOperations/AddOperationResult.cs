// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.QueryEngine.KeyValueStore.InMemory
{
    public class AddOperationResult<TKey, TValue> : OperationResult<TKey, TValue>
    {
        private readonly bool _keyAlreadyExists;

        public AddOperationResult(bool keyAlreadyExists) => _keyAlreadyExists = keyAlreadyExists;

        public override Exception Exception => _keyAlreadyExists ? new ArgumentException("An item with the same key has already been added.") : null;

        public override object Result => throw new NotSupportedException();

        public override bool Equals(object obj) => obj is AddOperationResult<TKey, TValue> state && _keyAlreadyExists == state._keyAlreadyExists;

        public override int GetHashCode() => _keyAlreadyExists.GetHashCode();
    }
}
