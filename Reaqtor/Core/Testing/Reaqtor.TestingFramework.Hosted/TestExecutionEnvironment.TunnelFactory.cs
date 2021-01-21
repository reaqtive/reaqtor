// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;

using Reaqtive;

namespace Reaqtor.TestingFramework
{
    public partial class TestExecutionEnvironment
    {
        private sealed class TunnelFactory<I, O, A> : IReactiveQubjectFactory<I, O, A>
        {
            private readonly TestExecutionEnvironment _parent;

            public TunnelFactory(TestExecutionEnvironment parent)
            {
                _parent = parent;
            }

            public IReactiveQubject<I, O> Create(Uri streamUri, A argument, object state)
            {
                if (typeof(I) == typeof(O))
                {
                    var uri = (Tuple<Uri, Uri>)(object)argument;
                    var refCountSubject = (IMultiSubject<bool, bool>)_parent._artifacts[uri.Item1];
                    var refCount = refCountSubject.CreateObserver();
                    var res = new SimpleTunnel<I>(_parent, streamUri, refCount);
                    _parent.AddArtifact(streamUri, res);
                    return (IReactiveQubject<I, O>)res;
                }

                throw new NotImplementedException();
            }

            IReactiveSubject<I, O> IReactiveSubjectFactory<I, O, A>.Create(Uri streamUri, A argument, object state)
            {
                return Create(streamUri, argument, state);
            }

            #region Not implemented

            public IReactiveQueryProvider Provider => throw new NotImplementedException();

            public Expression Expression => throw new NotImplementedException();

            #endregion
        }
    }
}
