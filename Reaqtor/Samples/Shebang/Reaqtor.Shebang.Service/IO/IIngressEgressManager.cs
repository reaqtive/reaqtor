﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Reaqtive;

using Reaqtor.Reliable;

namespace Reaqtor.Shebang.Service
{
    public interface IIngressEgressManager
    {
        IReliableObserver<T> GetObserver<T>(string name);

        IReliableObservable<T> GetObservable<T>(string name);
    }
}