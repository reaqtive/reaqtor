// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;

using Reaqtor.Shebang.Linq;
using Reaqtor.Shebang.Service;

namespace Reaqtor.Shebang.App
{
    public static class Program
    {
        public static async Task Main()
        {
            var store = new InMemoryKeyValueStore();

            {
                Console.Write("Create new engine... ");

                var qe = await QueryEngineFactory.CreateNewAsync(store);

                Console.WriteLine("Done.");

                var ctx = qe.GetClient();

                Console.Write("Create timer subscription... ");

                await ctx.Timer(TimeSpan.FromSeconds(1)).Select(t => t.ToString()).SubscribeAsync(ctx.ConsoleOut, new Uri("nucleus://shebang/subscriptions/tick"), state: null);

                Console.WriteLine("Done.");

                Console.Write("Checkpoint engine... ");

                await qe.CheckpointAsync();

                Console.WriteLine("Done.");

                await Task.Delay(5000);

                Console.Write("Unload engine... ");

                await qe.UnloadAsync();

                Console.WriteLine("Done.");
            }

            {
                Console.Write("Recover engine... ");

                var qe = await QueryEngineFactory.RecoverAsync(store);

                Console.WriteLine("Done.");

                var ctx = qe.GetClient();

                await Task.Delay(5000);

                Console.Write("Delete subscription... ");

                await ctx.GetSubscription(new Uri("nucleus://shebang/subscriptions/tick")).DisposeAsync();

                Console.WriteLine("Done.");

                await Task.Delay(5000);

                Console.Write("Checkpoint engine... ");

                await qe.CheckpointAsync();

                Console.WriteLine("Done.");

                Console.Write("Unload engine... ");

                await qe.UnloadAsync();

                Console.WriteLine("Done.");
            }
        }
    }
}
