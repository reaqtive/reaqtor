// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

using Reaqtive;

namespace Reaqtor.TestingFramework
{
    public partial class TestExecutionEnvironment : IHigherOrderExecutionEnvironment
    {
        private readonly Dictionary<Uri, object> _artifacts = new();
        private bool _frozen;

        public void Freeze() => _frozen = true;

        public void Defrost() => _frozen = false;

        public int ArtifactCount => _artifacts.Count;

        public Exception BridgeSubscriptionError { get; set; }

        public bool TryGetSubject<TInput, TOutput>(Uri uri, out IMultiSubject<TInput, TOutput> subject)
        {
            var res = _artifacts.TryGetValue(uri, out var artifact);

            subject = (IMultiSubject<TInput, TOutput>)artifact;
            return res;
        }

        public IMultiSubject<TInput, TOutput> GetSubject<TInput, TOutput>(Uri uri)
        {
            var obj = _artifacts[uri];
            if (obj is not IMultiSubject<TInput, TOutput> subject)
            {
                subject = ((IMultiSubject)obj).ToTyped<TInput, TOutput>();
            }
            return subject;
        }

        public ISubscription GetSubscription(Uri uri) => throw new NotImplementedException();

        public void AddArtifact(Uri uri, object artifact)
        {
            if (_frozen)
                throw new InvalidOperationException();

            _artifacts.Add(uri, artifact);
        }

        public void RemoveArtifact(Uri uri)
        {
            if (_frozen)
                return;

            _artifacts.Remove(uri);
        }

        protected object GetArtifact(Uri uri) => _artifacts[uri];
    }
}
