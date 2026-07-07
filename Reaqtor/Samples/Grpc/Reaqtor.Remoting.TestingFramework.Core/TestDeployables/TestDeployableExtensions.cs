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
            ArgumentNullException.ThrowIfNull(source);

            return source.Provider.CreateQbservable<T>(
                Expression.Call(
                    ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(T)),
                    source.Expression));
        }

        // NB: de-MBR'd. The archived RemoteObserver derived from MarshalByRefObject (and overrode
        //     InitializeLifetimeService to disable lease expiry) so a client-side observer could be handed across
        //     the .NET Remoting boundary. Observers now run in-process, so the MarshalByRefObject base and the
        //     lease override are removed (keeping only IObserver<string>).
        private sealed class RemoteObserver<T> : IObserver<string>
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
        }
    }
}
