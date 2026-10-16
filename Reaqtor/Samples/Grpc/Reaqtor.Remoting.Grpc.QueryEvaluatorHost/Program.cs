// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Globalization;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using ProtoBuf.Grpc.Server;

using Reaqtor.Remoting.Grpc.Client;
using Reaqtor.Remoting.Grpc.Contracts;
using Reaqtor.Remoting.Grpc.Server;
using Reaqtor.Remoting.Platform;
using Reaqtor.Remoting.Protocol;

//
// Kestrel/HTTP-2 gRPC host for the query-evaluator role (plan §7). The replacement for the archived console
// TcpRemoteServiceHost: a minimal WebApplication listening h2c (cleartext HTTP/2) on a loopback port. The port is
// passed as args[0] by GrpcProcessRunnable (default 8081). h2c on the *client* side requires the
// SocketsHttpHandler.Http2UnencryptedSupport switch (handled in Reaqtor.Remoting.Grpc.Client); the server simply
// listens with HttpProtocols.Http2.
//
// args[1] (optional, Milestone 5): a standalone Messaging broker host address. When present, the engine's firehose
// legs (both the input FirehoseSubscribable and the output FirehoseObserver, which resolve via MessageRouter.Instance)
// are routed to that separate broker process instead of the in-host broker (§3.6/§4.3).
//
var port = args.Length > 0 && int.TryParse(args[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out var p)
    ? p
    : 8081;

var brokerAddress = args.Length > 1 && !string.IsNullOrWhiteSpace(args[1]) ? args[1] : null;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
    options.ListenLocalhost(port, listen => listen.Protocols = HttpProtocols.Http2));

builder.Services.AddCodeFirstGrpc();
builder.Services.AddSingleton<EngineHost>();
builder.Services.AddSingleton<ReactiveServiceControlAdapter>();
builder.Services.AddSingleton<ReactiveServiceConnectionAdapter>();
builder.Services.AddSingleton<QueryEvaluatorControlAdapter>();
// The in-QE-host broker connection backs the messaging adapter (Milestone 5 may instead point clients at a
// standalone broker host; the engine's own firehose routing is redirected below when args[1] is supplied).
builder.Services.AddSingleton<IReactiveMessagingConnection>(sp => sp.GetRequiredService<EngineHost>().MessagingConnection);
builder.Services.AddSingleton<MessagingGrpcAdapter>();
builder.Services.AddSingleton<StateStoreGrpcAdapter>();
builder.Services.AddSingleton<KeyValueStoreGrpcAdapter>();
builder.Services.AddSingleton<StorageGrpcAdapter>();

var app = builder.Build();

// Start the in-host engine (InMemoryReactivePlatform + CoreDeployable) before serving, so that Ping-readiness
// implies the engine is up and the command channel is live (plan §9 step 1).
app.Services.GetRequiredService<EngineHost>().EnsureStarted();

// Milestone 5: redirect the engine's firehose legs to the standalone broker host. EngineHost.EnsureStarted points
// MessageRouter.Instance at the in-host broker; override it here so both legs (input subscribe + output publish)
// flow to the separate broker process. Firehoses are instantiated lazily at subscribe time (after this point), so
// the override takes effect before any query runs.
if (brokerAddress != null)
{
#pragma warning disable CA2000 // Disposed on graceful shutdown via ApplicationStopping (below); the analyzer can't see the lifetime-callback path. On a hard kill the process simply exits.
    var brokerClient = new GrpcMessagingConnection(brokerAddress);
#pragma warning restore CA2000
    MessageRouter.Initialize(brokerClient);
    // Release the broker client on graceful shutdown (it otherwise lives for the host's lifetime).
    app.Lifetime.ApplicationStopping.Register(brokerClient.Dispose);
}

app.MapGrpcService<ReactiveServiceControlAdapter>();
app.MapGrpcService<ReactiveServiceConnectionAdapter>();
app.MapGrpcService<QueryEvaluatorControlAdapter>();
app.MapGrpcService<MessagingGrpcAdapter>();
app.MapGrpcService<StateStoreGrpcAdapter>();
app.MapGrpcService<KeyValueStoreGrpcAdapter>();
app.MapGrpcService<StorageGrpcAdapter>();

app.Run();
