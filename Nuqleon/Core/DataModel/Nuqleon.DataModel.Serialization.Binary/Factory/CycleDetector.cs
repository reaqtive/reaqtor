﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Nuqleon.DataModel.Serialization.Binary
{
    internal delegate void CycleDetector(CycleDetector f, object value, HashSet<object> visited);
}
