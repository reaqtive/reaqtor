// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace DelegatingBinder
{
    [AttributeUsage(AttributeTargets.Method)]
    internal class ResourceAttribute : Attribute
    {
        public ResourceAttribute(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}
