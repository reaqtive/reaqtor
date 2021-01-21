// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - June 2014 - Created this file.
//

using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor.Remoting.Platform
{
    internal class TcpMultiRoleRunnable : ProcessRunnable
    {
        private readonly TcpMultiRoleArguments[] _args;
        private object[] _instances;
        private Task<int> _task;

        public TcpMultiRoleRunnable(params TcpMultiRoleArguments[] args)
            : base(Helpers.GetExecutablePath("MultiRoleHost"), Inline(args))
        {
            _args = args;
        }

        public override object Instance => _instances ??= GetInstances(_args);

        public override Task<int> RunAsync(CancellationToken token) => _task ??= base.RunAsync(token);

        private static object[] GetInstances(TcpMultiRoleArguments[] args)
        {
            var instances = new object[args.Length];
            for (var i = 0; i < args.Length; ++i)
            {
                if (args[i].Uri == null || args[i].Type == null)
                {
                    throw new ArgumentException("Arguments should not be null.", nameof(args));
                }

                var endpoint = Helpers.GetTcpEndpoint(Helpers.Constants.Host, args[i].Port, args[i].Uri);
                instances[i] = Activator.GetObject(args[i].Type, endpoint);
            }

            return instances;
        }

        private static string[] Inline(TcpMultiRoleArguments[] args)
        {
            var roleCount = args.Length;
            var inlined = new string[roleCount * 3];
            for (var i = 0; i < roleCount; ++i)
            {
                inlined[3 * i + 0] = args[i].Role.ToString();
                inlined[3 * i + 1] = args[i].Uri;
                inlined[3 * i + 2] = args[i].Port.ToString(CultureInfo.InvariantCulture);
            }

            return inlined;
        }
    }

    internal struct TcpMultiRoleArguments
    {
        public ReactiveServiceType Role;
        public string Uri;
        public int Port;
        public Type Type;
    }
}
