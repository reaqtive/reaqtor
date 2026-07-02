// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// BD - February 2018
//

using System.Collections.Concurrent;
using System.Reactive.Subjects;

namespace Playground
{
    internal static partial class EngineIntegrationTests
    {
        /// <summary>
        /// Stream manager exposed to <see cref="Source{T}"/> and <see cref="Sink{T}"/> through the operator context to create and get subject instances with an extrinsic identifier.
        /// </summary>
        private sealed class StreamManager
        {
            private readonly ConcurrentDictionary<string, object> _subjects = new();

            public ISubject<T> CreateSubject<T>(string id) => (ISubject<T>)_subjects.GetOrAdd(id, _ => new Subject<T>());

            public ISubject<T> GetSubject<T>(string id) => (ISubject<T>)_subjects[id];
        }
    }
}
