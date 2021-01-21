// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - October 2014 - Created this file.
//

using System;
using System.Linq.Expressions;
using System.Threading;

using Reaqtor.Remoting.Platform;
using Reaqtor.Remoting.Protocol;
using Reaqtor.Hosting.Shared.Serialization;

namespace Reaqtor.Remoting
{
    /// <summary>
    /// Helper class for marshalling remote subscriptions to clients.
    /// </summary>
    public static class AsObservableExtensions
    {
        /// <summary>
        /// Creates a local observable that, when subscribed to, sets up a
        /// remote subscription that pushes values back to the client through
        /// the messaging role.
        /// </summary>
        /// <typeparam name="T">The message type.</typeparam>
        /// <param name="source">The remote observable.</param>
        /// <param name="platform">The platform containing the messaging connection.</param>
        /// <returns>The local observable.</returns>
        public static IObservable<T> AsObservable<T>(this IAsyncReactiveQbservable<T> source, IReactivePlatform platform)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (platform == null)
            {
                throw new ArgumentNullException(nameof(platform));
            }

            return new Impl<T>(source, platform);
        }

        private sealed class Impl<T> : IObservable<T>
        {
            private readonly IAsyncReactiveQbservable<T> _source;
            private readonly IReactivePlatform _platform;

            public Impl(IAsyncReactiveQbservable<T> source, IReactivePlatform platform)
            {
                _source = source;
                _platform = platform;
            }

            public IDisposable Subscribe(IObserver<T> observer) => new _(this, observer);

            private sealed class _ : IDisposable
            {
                private Action _onDispose;
                private IDisposable _disposable;

                public _(Impl<T> parent, IObserver<T> observer)
                {
                    var uri = new Uri("rx://asObservable/" + Guid.NewGuid());
                    _disposable = CreateLocalSubscription(parent._platform, uri, observer);
                    _onDispose = CreateRemoteSubscription(parent._source, uri);
                }

                public void Dispose()
                {
                    var onDispose = Interlocked.Exchange(ref _onDispose, null);
                    if (onDispose != null)
                    {
                        onDispose();
                        var disposable = Interlocked.Exchange(ref _disposable, null);
                        disposable.Dispose();
                    }
                }

                private static IDisposable CreateLocalSubscription(IReactivePlatform platform, Uri uri, IObserver<T> observer)
                {
                    var messagingConnection = platform.Environment.MessagingService.GetInstance<IReactiveMessagingConnection>();
                    var messageRouter = new MessageRouter(messagingConnection);
                    return messageRouter.Subscribe(uri, new Deserializer(observer));
                }

                private static Action CreateRemoteSubscription(IAsyncReactiveQbservable<T> source, Uri uri)
                {
                    var observer = source.Provider.CreateQbserver<Uri, T>(
                        Expression.Parameter(typeof(Uri)).Let(u =>
                            Expression.Lambda<Func<Uri, IAsyncReactiveQbserver<T>>>(
                                Expression.Invoke(
                                    Expression.Parameter(
                                        typeof(Func<Uri, IAsyncReactiveQbserver<T>>),
                                        Platform.Constants.Identifiers.Observer.FireHose.String
                                    ),
                                    u
                                ),
                                u
                            )
                        )
                    );

                    var sub = source.SubscribeAsync(observer(uri), uri, null, CancellationToken.None).Result;
                    return () => sub.DisposeAsync(CancellationToken.None).Wait();
                }

                private sealed class Deserializer : MarshalByRefObject, IObserver<byte[]>
                {
                    private readonly IObserver<T> _observer;
                    private readonly SerializationHelpers _serializer;

                    public Deserializer(IObserver<T> observer)
                    {
                        _observer = observer;
                        _serializer = new SerializationHelpers();
                    }

                    public void OnCompleted() => _observer.OnCompleted();

                    public void OnError(Exception error) => _observer.OnError(error);

                    public void OnNext(byte[] value) => _observer.OnNext(_serializer.Deserialize<T>(value));

                    public override object InitializeLifetimeService() => null;
                }
            }
        }
    }
}
