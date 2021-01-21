// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;
using System.Threading;
using System.Threading.Tasks;

namespace Reaqtor.Remoting.Platform
{
    public class TcpRemoteServiceHost<T>
        where T : new()
    {
        private static readonly AutoResetEvent s_nullEvent = new(false);

        private readonly Type _remotedType;
        private readonly int _port;
        private readonly string _path;

        public TcpRemoteServiceHost(string[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }
            else if (args.Length != 2)
            {
                throw new ArgumentException("Expected array with two elements, a port number and a path string.", nameof(args));
            }
            else if (!int.TryParse(args[0], out _port))
            {
                throw new ArgumentException("Expected port number in first argument.", nameof(args));
            }

            _remotedType = typeof(T);
            _path = args[1];
        }

        public T Instance => (T)Activator.GetObject(_remotedType, Helpers.GetTcpEndpoint(Helpers.Constants.Host, _port, _path));

        public void Start()
        {
            var set = new AutoResetEvent(false);
            var wait = new AutoResetEvent(false);

            Task.Run(() =>
            {
                Start(set, wait);
            });

            set.WaitOne();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            wait.Set();
        }

        public void Start(EventWaitHandle wait)
        {
            Start(s_nullEvent, wait);
        }

        public void Start(EventWaitHandle set, EventWaitHandle wait)
        {
            Console.Title = _remotedType.Name;
            Console.Write("{0} starting at '{1}'... ", _remotedType.Name, Helpers.GetTcpEndpoint(Helpers.Constants.Host, _port, _path));

            InitializeClient();

            var serverProvider = new BinaryServerFormatterSinkProvider { TypeFilterLevel = TypeFilterLevel.Full };
            var props = new Hashtable
            {
                { "name", Guid.NewGuid().ToString() },
                { "port", _port },
                { "typeFilterLevel", TypeFilterLevel.Full }
            };
            var channel = new TcpChannel(props, null, serverProvider);
            ChannelServices.RegisterChannel(channel, ensureSecurity: false);
            RemotingConfiguration.RegisterWellKnownServiceType(_remotedType, _path, WellKnownObjectMode.Singleton);

            Console.WriteLine("Done.");

            set.Set();
            wait.WaitOne();
        }

        private static void InitializeClient()
        {
            var clientProvider = new BinaryClientFormatterSinkProvider();
            var serverProvider = new BinaryServerFormatterSinkProvider { TypeFilterLevel = TypeFilterLevel.Full };
            var props = new Hashtable
            {
                { "port", 0 },
                { "name", System.Guid.NewGuid().ToString() },
                { "typeFilterLevel", TypeFilterLevel.Full }
            };
            ChannelServices.RegisterChannel(new TcpChannel(props, clientProvider, serverProvider), ensureSecurity: false);
        }
    }
}
