// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - December 2016 - Created this file.
//

using Newtonsoft.Json;

using Nuqleon.Json.Expressions;

namespace Nuqleon.Json.Interop.Newtonsoft
{
    internal struct Token
    {
        public string Property;
        public Expression Expression;
        public JsonToken JsonToken;
    }
}
