﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Subjects;

namespace DelegatingBinder
{
    internal sealed class SubjectFactory<T> : ISubjectFactory<T>
    {
        public ISubject<T> Create()
        {
            return new Subject<T>();
        }
    }
}
