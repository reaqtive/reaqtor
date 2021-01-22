// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;

namespace Reaqtor.TestingFramework
{
    public partial class TestExecutionEnvironment
    {
        private sealed class BridgeFactory<I, O, A> : IReactiveQubjectFactory<I, O, A>
        {
            private readonly TestExecutionEnvironment _parent;

            public BridgeFactory(TestExecutionEnvironment parent)
            {
                _parent = parent;
            }

            public IReactiveQubject<I, O> Create(Uri streamUri, A argument, object state)
            {
                if (typeof(I) == typeof(O) && typeof(A) == typeof(Expression))
                {
                    var res = new SimpleBridge<I>(_parent, streamUri, (Expression)(object)argument);
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
