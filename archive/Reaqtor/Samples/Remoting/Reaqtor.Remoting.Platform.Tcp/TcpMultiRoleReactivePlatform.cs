// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

//
// Revision history:
//
// ER - June 2014 - Created this file.
//

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

using Reaqtor.Remoting.Protocol;

#pragma warning disable CA1303 // Do not pass literals as localized parameters. (No localization in sample code.)

namespace Reaqtor.Remoting.Platform
{
    public sealed class TcpMultiRoleReactivePlatform : ReactivePlatformBase
    {
        private const int RetryCount = 1000;

        private readonly IReactiveQueryCoordinator _queryCoordinator;
        private readonly IReactiveQueryEvaluator _queryEvaluator;

        public TcpMultiRoleReactivePlatform(params ReactiveServiceType[][] groups)
            : this(new TcpReactivePlatformSettings(), Validate(groups))
        {
        }

        public TcpMultiRoleReactivePlatform(ITcpReactivePlatformSettings settings, params ReactiveServiceType[][] groups)
            : this(Validate(groups), GetRunnables(settings, Validate(groups)))
        {
        }

        private TcpMultiRoleReactivePlatform(ReactiveServiceType[][] groups, TcpMultiRoleRunnable[] runnables)
#pragma warning disable CA2000 // Dispose objects before losing scope. (Ownership transfer.)
            : base(GetEnvironment(groups, runnables))
#pragma warning restore CA2000
        {
            _queryCoordinator = GetQueryCoordinator(groups, runnables);
            _queryEvaluator = GetQueryEvaluator(groups, runnables);
        }

        public override IReactiveQueryCoordinator QueryCoordinator => _queryCoordinator;

        public override IEnumerable<IReactiveQueryEvaluator> QueryEvaluators
        {
            get
            {
                yield return _queryEvaluator;
            }
        }

        public override async Task StartAsync(CancellationToken token)
        {
            await Environment.StartAsync(token).ConfigureAwait(false);
            await base.StartAsync(token).ConfigureAwait(false);

            var count = 0;
            while (count < RetryCount)
            {
                try
                {
                    foreach (var qe in QueryEvaluators)
                    {
                        qe.GetInstance<IReactiveConnection>().Ping();
                    }

                    QueryCoordinator.GetInstance<IReactiveConnection>().Ping();
                    break;
                }
#pragma warning disable CA1031 // REWIEW: Do not catch general exception types.
                catch
                {
                    Console.WriteLine("Waiting for services to start (retry attempt {0})...", ++count);
                }
#pragma warning restore CA1031
            }

            if (count == RetryCount)
            {
                throw new InvalidOperationException("Error starting the multi-role platform.");
            }
        }

        private static ReactiveServiceType[][] Validate(params ReactiveServiceType[][] groups)
        {
            groups ??= Array.Empty<ReactiveServiceType[]>();

            var roles = new HashSet<ReactiveServiceType>();
            var missingRoles = new List<ReactiveServiceType>();

            foreach (var group in groups)
            {
                foreach (var role in group)
                {
                    if (!roles.Add(role))
                    {
                        throw new ArgumentException(
                            string.Format(CultureInfo.InvariantCulture, "More than one role for '{0}' specified.", role));
                    }
                }
            }

            if (!roles.Contains(ReactiveServiceType.MessagingService)) missingRoles.Add(ReactiveServiceType.MessagingService);
            if (!roles.Contains(ReactiveServiceType.MetadataService)) missingRoles.Add(ReactiveServiceType.MetadataService);
            if (!roles.Contains(ReactiveServiceType.StateStoreService)) missingRoles.Add(ReactiveServiceType.StateStoreService);
            if (!roles.Contains(ReactiveServiceType.KeyValueStoreService)) missingRoles.Add(ReactiveServiceType.KeyValueStoreService);
            if (!roles.Contains(ReactiveServiceType.QueryEvaluator)) missingRoles.Add(ReactiveServiceType.QueryEvaluator);
            if (!roles.Contains(ReactiveServiceType.QueryCoordinator)) missingRoles.Add(ReactiveServiceType.QueryCoordinator);

            if (missingRoles.Count > 0)
            {
                var newGroups = new ReactiveServiceType[groups.Length + 1][];
                Array.Copy(groups, newGroups, groups.Length);
                newGroups[groups.Length] = missingRoles.ToArray();
                groups = newGroups;
            }

            return groups;
        }

        private static TcpMultiRoleRunnable[] GetRunnables(ITcpReactivePlatformSettings settings, params ReactiveServiceType[][] groups)
        {
            var runnables = new TcpMultiRoleRunnable[groups.Length];

            for (var i = 0; i < groups.Length; ++i)
            {
                runnables[i] = new TcpMultiRoleRunnable(GetArgs(settings, groups[i]));
            }

            return runnables;
        }

        private static TcpMultiRoleArguments[] GetArgs(ITcpReactivePlatformSettings settings, params ReactiveServiceType[] group)
        {
            var args = new TcpMultiRoleArguments[group.Length];

            for (var i = 0; i < group.Length; ++i)
            {
                args[i] = group[i] switch
                {
                    ReactiveServiceType.MetadataService => new TcpMultiRoleArguments
                    {
                        Role = ReactiveServiceType.MetadataService,
                        Uri = settings.MetadataUri,
                        Port = settings.MetadataPort,
                        Type = typeof(IReactiveStorageConnection),
                    },
                    ReactiveServiceType.QueryCoordinator => new TcpMultiRoleArguments
                    {
                        Role = ReactiveServiceType.QueryCoordinator,
                        Uri = settings.QueryCoordinatorUri,
                        Port = settings.QueryCoordinatorPort,
                        Type = typeof(IRemotingReactiveServiceConnection),
                    },
                    ReactiveServiceType.QueryEvaluator => new TcpMultiRoleArguments
                    {
                        Role = ReactiveServiceType.QueryEvaluator,
                        Uri = settings.QueryEvaluatorUri,
                        Port = settings.QueryEvaluatorPort,
                        Type = typeof(IReactiveQueryEvaluatorConnection),
                    },
                    ReactiveServiceType.MessagingService => new TcpMultiRoleArguments
                    {
                        Role = ReactiveServiceType.MessagingService,
                        Uri = settings.MessagingUri,
                        Port = settings.MessagingPort,
                        Type = typeof(IReactiveMessagingConnection),
                    },
                    ReactiveServiceType.StateStoreService => new TcpMultiRoleArguments
                    {
                        Role = ReactiveServiceType.StateStoreService,
                        Uri = settings.StateStoreUri,
                        Port = settings.StateStorePort,
                        Type = typeof(IReactiveStateStoreConnection),
                    },
                    ReactiveServiceType.KeyValueStoreService => new TcpMultiRoleArguments
                    {
                        Role = ReactiveServiceType.KeyValueStoreService,
                        Uri = settings.KeyValueStoreUri,
                        Port = settings.KeyValueStorePort,
                        Type = typeof(ITransactionalKeyValueStoreConnection),
                    },
                    _ => throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Service type '{0}' not supported in the multi-role platform.", group[i])),
                };
            }

            return args;
        }

        private static IReactiveEnvironment GetEnvironment(ReactiveServiceType[][] groups, TcpMultiRoleRunnable[] runnables)
        {
            var metadata = default(IReactiveMetadataService);
            var messaging = default(IReactiveMessagingService);
            var stateStore = default(IReactiveStateStoreService);
            var keyValueStore = default(IKeyValueStoreService);

            for (var i = 0; i < groups.Length; ++i)
            {
                for (var j = 0; j < groups[i].Length; ++j)
                {
#pragma warning disable CA2000 // Dispose objects before losing scope. (Ownership transfer.)
                    switch (groups[i][j])
                    {
                        case ReactiveServiceType.MetadataService:
                            metadata = new TcpMetadataService(new TcpMultiRoleInstance(((object[])runnables[i].Instance)[j], runnables[i]));
                            break;
                        case ReactiveServiceType.MessagingService:
                            messaging = new TcpMessagingService(new TcpMultiRoleInstance(((object[])runnables[i].Instance)[j], runnables[i]));
                            break;
                        case ReactiveServiceType.StateStoreService:
                            stateStore = new TcpStateStoreService(new TcpMultiRoleInstance(((object[])runnables[i].Instance)[j], runnables[i]));
                            break;
                        case ReactiveServiceType.KeyValueStoreService:
                            keyValueStore = new TcpKeyValueStoreService(new TcpMultiRoleInstance(((object[])runnables[i].Instance)[j], runnables[i]));
                            break;
                        case ReactiveServiceType.DeployerService:
                        case ReactiveServiceType.QueryCoordinator:
                        case ReactiveServiceType.QueryEvaluator:
                        default:
                            break;
                    }
#pragma warning restore CA2000
                }
            }

            return new ReactiveEnvironment(metadata, messaging, stateStore, keyValueStore);
        }

        private IReactiveQueryCoordinator GetQueryCoordinator(ReactiveServiceType[][] groups, TcpMultiRoleRunnable[] runnables)
        {
            for (var i = 0; i < groups.Length; ++i)
            {
                for (var j = 0; j < groups[i].Length; ++j)
                {
#pragma warning disable CA2000 // Dispose objects before losing scope. (Ownership transfer.)
                    switch (groups[i][j])
                    {
                        case ReactiveServiceType.QueryCoordinator:
                            return new TcpQueryCoordinator(this, new TcpMultiRoleInstance(((object[])runnables[i].Instance)[j], runnables[i]));
                        default:
                            break;
                    }
#pragma warning restore CA2000
                }
            }

            throw new InvalidOperationException("Could not find the query coordinator role.");
        }

        private IReactiveQueryEvaluator GetQueryEvaluator(ReactiveServiceType[][] groups, TcpMultiRoleRunnable[] runnables)
        {
            for (var i = 0; i < groups.Length; ++i)
            {
                for (var j = 0; j < groups[i].Length; ++j)
                {
#pragma warning disable CA2000 // Dispose objects before losing scope. (Ownership transfer.)
                    switch (groups[i][j])
                    {
                        case ReactiveServiceType.QueryEvaluator:
                            return new TcpQueryEvaluator(this, new TcpMultiRoleInstance(((object[])runnables[i].Instance)[j], runnables[i]));
                        default:
                            break;
                    }
#pragma warning restore CA2000
                }
            }

            throw new InvalidOperationException("Could not find the query evaluator role.");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Environment.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
