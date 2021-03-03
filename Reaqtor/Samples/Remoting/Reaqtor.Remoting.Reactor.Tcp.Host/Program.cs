// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable CA1303 // Do not pass literals as localized parameters. (No localization in sample code.)

namespace Reaqtor.Remoting.Platform.Tcp.Host
{
    public static class Program
    {
        private static readonly TimeSpan OutputDelay = TimeSpan.FromSeconds(1);

        public static void Main()
        {
            var printer = default(Task);
            var settings = new TcpReactivePlatformSettings();

            using (var platform = new TcpMultiRoleReactivePlatform(settings))
            {
                var process = ((IRunnable<Process>)platform.QueryCoordinator.Runnable).Target;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                platform.StartAsync(CancellationToken.None).Wait();

                printer = PrintProcessOutputAsync(process.StandardOutput, CancellationToken.None);

                Console.WriteLine("Query coordinator started at 'tcp://{0}:{1}/{2}'", settings.Host, settings.QueryCoordinatorPort, settings.QueryCoordinatorUri);
                Console.Write("Deploying builtin metadata definitions and domain feeds... ");
                new ReactivePlatformDeployer(platform, new Deployable.CoreDeployable(), new Reactor.Deployable(), new Reactor.DomainFeeds.DomainFeedsDeployable()).Deploy();
                Console.WriteLine("Done.");

                Console.WriteLine("Press any key to shutdown...");
                Console.ReadKey();
            }

            printer.Wait();
        }

        private static async Task PrintProcessOutputAsync(StreamReader reader, CancellationToken token)
        {
            var readLine = reader.ReadLineAsync();
            while (true)
            {
                var finished = await Task.WhenAny(Task.Delay(OutputDelay, token), readLine).ConfigureAwait(false);
                if (finished == readLine)
                {
                    if (readLine.Result == null)
                    {
                        break;
                    }

                    Console.WriteLine(readLine.Result);
                    readLine = reader.ReadLineAsync();
                }
            }
        }
    }
}
