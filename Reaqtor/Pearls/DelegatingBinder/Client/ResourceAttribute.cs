// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace DelegatingBinder
{
    [AttributeUsage(AttributeTargets.Method)]
    internal sealed class ResourceAttribute(string id) : Attribute
    {
        public string Id { get; } = id;
    }
}
