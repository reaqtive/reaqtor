// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - February 2014 - Created this file.
//

using System;
using System.Linq.Expressions;
using System.Reflection;

using Reaqtor.Hosting.Shared.Serialization;

namespace Reaqtor.Remoting.TestingFramework
{
    public static class TestDeployableExtensions
    {
        [KnownResource(Constants.Test.StatefulAugmentationObservable.String)]
        public static IAsyncReactiveQbservable<T> StatefulAugmentation<T>(this IAsyncReactiveQbservable<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Provider.CreateQbservable<T>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression));
        }

        private sealed class RemoteObserver<T> : MarshalByRefObject, IObserver<string>
        {
            private readonly SerializationHelpers serializer = new();

            private readonly IObserver<T> _localObserver;

            public RemoteObserver(IObserver<T> localObserver)
            {
                _localObserver = localObserver;
            }

            public void OnCompleted()
            {
                _localObserver.OnCompleted();
            }

            public void OnError(Exception error)
            {
                _localObserver.OnError(error);
            }

            public void OnNext(string value)
            {
                _localObserver.OnNext(serializer.Deserialize<T>(value));
            }

            public override object InitializeLifetimeService() => null;
        }
    }
}
