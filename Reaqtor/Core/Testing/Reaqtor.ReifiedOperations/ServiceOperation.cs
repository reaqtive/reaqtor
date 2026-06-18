// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - July 2013 - Created this file.
//

using System;
using System.Globalization;

namespace Reaqtor.TestingFramework
{
    [Serializable]
    public class ServiceOperation(ServiceOperationKind kind, Uri targetObjectUri, object state)
    {
        public ServiceOperationKind Kind { get; } = kind;
        public Uri TargetObjectUri { get; } = targetObjectUri;
        public object State { get; } = state;

        public override string ToString() => string.Format(CultureInfo.InvariantCulture, "{0}({1}, {2})", Kind, TargetObjectUri, State);
    }
}
