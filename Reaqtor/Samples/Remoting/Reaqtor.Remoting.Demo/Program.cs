// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using System.Threading.Tasks;

using Reaqtor.Remoting.Platform;

namespace Reaqtor.Remoting.Demo
{
    public static class Program
    {
        public static void Main()
        {
            Run(async context =>
            {
                var streamUri = new Uri("test://stream/1");
                var subscriptionUri = new Uri("test://subscription/1");
                var streamFactory = context.GetStreamFactory<int, int>(Platform.Constants.Identifiers.Observable.FireHose.Uri);
                var stream = await streamFactory.CreateAsync(streamUri, null, CancellationToken.None);

                var observer = context.GetObserver<string, int>(Platform.Constants.Identifiers.Observer.ConsoleObserverParam.Uri);
                var subscription = await stream.Do(observer("Do")).Where(x => x % 2 == 0).Take(2).SubscribeAsync(observer("Final"), subscriptionUri, null, CancellationToken.None);

                for (var i = 1; i <= 4; ++i)
                {
                    await stream.OnNextAsync(i, CancellationToken.None);
                }

                await subscription.DisposeAsync(CancellationToken.None);

                Console.WriteLine("Press any key to quit...");
                Console.ReadKey();
            });
        }

        private static void Run(Func<ReactiveClientContext, Task> action)
        {
            var platform = default(IReactivePlatform);

            try
            {
                platform = new TcpReactivePlatform();

                try
                {
                    var clientProvider = new System.Runtime.Remoting.Channels.BinaryClientFormatterSinkProvider();
                    var serverProvider = new System.Runtime.Remoting.Channels.BinaryServerFormatterSinkProvider { TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full };
                    var props = new System.Collections.Hashtable
                    {
                        { "port", 0 },
                        { "name", System.Guid.NewGuid().ToString() },
                        { "typeFilterLevel", System.Runtime.Serialization.Formatters.TypeFilterLevel.Full }
                    };
                    System.Runtime.Remoting.Channels.ChannelServices.RegisterChannel(new System.Runtime.Remoting.Channels.Tcp.TcpChannel(props, clientProvider, serverProvider), ensureSecurity: false);

                    platform.StartAsync(CancellationToken.None).Wait();
                    Console.WriteLine("Running demo with the TCP platform.");
                }
                catch
                {
                    try { platform.StopAsync(CancellationToken.None).Wait(); }
                    catch { }

                    platform = new InMemoryReactivePlatform();
                    platform.StartAsync(CancellationToken.None).Wait();
                    Console.WriteLine("Running demo with the in-memory platform.");
                }

                new ReactivePlatformDeployer(platform, new Deployable.Deployable()).Deploy();

                action(platform.CreateClient().Context).Wait();
            }
            finally
            {
                platform?.Dispose();
            }
        }
    }
}
