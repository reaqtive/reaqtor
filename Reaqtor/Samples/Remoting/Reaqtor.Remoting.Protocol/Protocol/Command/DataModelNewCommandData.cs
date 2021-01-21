// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Nuqleon.DataModel;
using System;

namespace Reaqtor.Remoting.Protocol
{
    internal sealed class DataModelNewCommandData<TExpression>
    {
        [Mapping("rx://artifact/uri")]
        public Uri Uri { get; set; }

        [Mapping("rx://artifact/expression")]
        public TExpression Expression { get; set; }

        [Mapping("rx://artifact/state")]
        public object State { get; set; }
    }
}
