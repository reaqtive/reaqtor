// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - January 2018
//

using System.Collections.Generic;
using System.IO;

namespace Utilities
{
    public sealed class AddOrUpdateStateWriterOperation : StateWriterOperation
    {
        private readonly MemoryStream _data;

        public AddOrUpdateStateWriterOperation(string category, string key, MemoryStream data) : base(category, key)
        {
            _data = data;
        }

        public override StateWriterOperationKind Kind => StateWriterOperationKind.AddOrUpdate;

        public override void Apply(Store store)
        {
            var data = _data.ToArray();

            if (!store.Data.TryGetValue(Category, out var table))
            {
                table = new Dictionary<string, byte[]>();
                store.Data.Add(Category, table);
            }

            if (!table.ContainsKey(Key))
            {
                table.Add(Key, data);
            }
            else
            {
                table[Key] = data;
            }
        }
    }
}
