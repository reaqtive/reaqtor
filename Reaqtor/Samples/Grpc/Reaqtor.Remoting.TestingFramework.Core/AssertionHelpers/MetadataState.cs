// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - November 2013 - Created this file.
//

using System;
using System.Linq.Expressions;

namespace Reaqtor.Remoting.TestingFramework
{
    public class MetadataState
    {
        public MetadataEntityType Type { get; set; }
        public Uri Uri { get; set; }
        public Expression Expression { get; set; }
    }
}
