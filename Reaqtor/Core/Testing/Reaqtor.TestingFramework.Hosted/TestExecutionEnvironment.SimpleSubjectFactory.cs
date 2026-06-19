// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;

namespace Reaqtor.TestingFramework
{
    public partial class TestExecutionEnvironment
    {
        private sealed class SimpleSubjectFactory<I, O> : IReactiveQubjectFactory<I, O>
        {
            private readonly TestExecutionEnvironment _parent;

            public SimpleSubjectFactory(TestExecutionEnvironment parent)
            {
                _parent = parent;
            }

            public IReactiveQubject<I, O> Create(Uri streamUri, object state)
            {
                if (typeof(I) == typeof(O))
                {
                    var res = new NotSoSimpleSubject<I>(_parent, streamUri);
                    _parent.AddArtifact(streamUri, res);
                    return (IReactiveQubject<I, O>)res;
                }

                throw new NotImplementedException();
            }

            IReactiveSubject<I, O> IReactiveSubjectFactory<I, O>.Create(Uri streamUri, object state)
            {
                return Create(streamUri, state);
            }

            #region Not implemented

            public IReactiveQueryProvider Provider => throw new NotImplementedException();

            public Expression Expression => throw new NotImplementedException();

            #endregion
        }
    }
}
